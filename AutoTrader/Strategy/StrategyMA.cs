using System;
using System.Collections.Generic;
using System.Linq;
using CTP;
using WrapperTest;
using System.Diagnostics;
using System.Configuration;

namespace HFTrader
{
    public class StrategyMA : StrategyControl
    {
        protected StrategyMA(string contractCode, TradeDataServer tradeDataServer) : base(tradeDataServer)
        {
            InstrumentId = contractCode;
            _ShortPeriod = tradeDataServer.ShortPeriod;
            _LongPeriod = tradeDataServer.LongPeriod;
            CancelSec = int.Parse(ConfigurationManager.AppSettings["OverTimeSec"]);//30
            IsSupportCloseToday = Utils.IsShfeInstrument(InstrumentId);
        }

        protected override void StartDepthMarketDataProcessing(ThostFtdcDepthMarketDataField depthMarketData)
        {
            if (Constant.IsNearBreakTime(60, depthMarketData.InstrumentID, "SHFE") || !IsFollowRules)
            {
                OperationEndofTradingDay(depthMarketData.InstrumentID);
            }
            else if (Constant.IsInTradingTime(DateTime.Now, "SHFE", InstrumentId))
            {
                QuoteCalculator(depthMarketData.InstrumentID);
            }
        }

        protected string InstrumentId { get; set; }

        private Dictionary<string, Dictionary<int, double>> _MovingAvgPrice = new Dictionary<string, Dictionary<int, double>>();
        private Dictionary<string, Dictionary<int, double>> _MovingAvgBidPrice = new Dictionary<string, Dictionary<int, double>>();
        private Dictionary<string, Dictionary<int, double>> _MovingAvgAskPrice = new Dictionary<string, Dictionary<int, double>>();
        private Dictionary<string, bool> _ChaseDic = new Dictionary<string, bool>();

        private int _ShortPeriod;
        private int _LongPeriod;
        private int _PositionNetLimit = int.Parse(ConfigurationManager.AppSettings["HighExposure"]);
        private int _CancelPriceLmt = int.Parse(ConfigurationManager.AppSettings["CancelTick"]);
        private int _SideLimit = int.Parse(ConfigurationManager.AppSettings["PositionLimit"]);
        //private int _UnitHand = 100;
        private int CancelSec;
        private bool IsSupportCloseToday;

        private void QuoteCalculator(string code)
        {
            string buyDirectionKey = Utils.GetDirectionKey(code, EnumDirectionType.Buy);
            string sellDirectionKey = Utils.GetDirectionKey(code, EnumDirectionType.Sell);
            if (//!TradeDataServer.MarketDataTrigger.ContainsKey(code) || !TradeDataServer.MarketDataTrigger[code]
                 _ChaseDic.ContainsKey(buyDirectionKey) && !_ChaseDic[buyDirectionKey]
                || _ChaseDic.ContainsKey(sellDirectionKey) && !_ChaseDic[sellDirectionKey])
            {
                return;
            }
            double bidPrice = TradeDataServer.QuoteAdapter.DepthMarketDataFields[code].BidPrice1;
            double askPrice = TradeDataServer.QuoteAdapter.DepthMarketDataFields[code].AskPrice1;

            #region Order

            if (!_MovingAvgPrice.ContainsKey(code))
            {
                _MovingAvgPrice.Add(code, new Dictionary<int, double>());
            }
            if (!_MovingAvgBidPrice.ContainsKey(code))
            {
                _MovingAvgBidPrice.Add(code, new Dictionary<int, double>());
            }
            if (!_MovingAvgAskPrice.ContainsKey(code))
            {
                _MovingAvgAskPrice.Add(code, new Dictionary<int, double>());
            }
            //if (!_CommissionBid.ContainsKey(code))
            //{
            //    _CommissionBid.Add(code, 0.0);
            //}
            //if (!_CommissionAsk.ContainsKey(code))
            //{
            //    _CommissionAsk.Add(code, 0.0);
            //}

            double oldShortPeriod = 0.0, oldLongPeriod = 0.0, oldShortPeriodBid = 0.0, oldLongPeriodBid = 0.0, oldShortPeriodAsk = 0.0, oldLongPeriodAsk = 0.0;
            if (_MovingAvgPrice[code].ContainsKey(_ShortPeriod))
            {
                oldShortPeriod = _MovingAvgPrice[code][_ShortPeriod];
            }
            if (_MovingAvgPrice[code].ContainsKey(_LongPeriod))
            {
                oldLongPeriod = _MovingAvgPrice[code][_LongPeriod];
            }
            if (_MovingAvgBidPrice[code].ContainsKey(_ShortPeriod))
            {
                oldShortPeriodBid = _MovingAvgBidPrice[code][_ShortPeriod];
            }
            if (_MovingAvgBidPrice[code].ContainsKey(_LongPeriod))
            {
                oldLongPeriodBid = _MovingAvgBidPrice[code][_LongPeriod];
            }
            if (_MovingAvgAskPrice[code].ContainsKey(_ShortPeriod))
            {
                oldShortPeriodAsk = _MovingAvgAskPrice[code][_ShortPeriod];
            }
            if (_MovingAvgAskPrice[code].ContainsKey(_LongPeriod))
            {
                oldLongPeriodAsk = _MovingAvgAskPrice[code][_LongPeriod];
            }

            bool isMaQualified = true;
            foreach (int period in TradeDataServer.DepthMarketDataQueue[code].Keys)
            {
                if (period <= TradeDataServer.DepthMarketDataQueue[code][period].Count)
                {
                    List<ThostFtdcDepthMarketDataField> priceLst = TradeDataServer.DepthMarketDataQueue[code][period].ToList();
                    if (priceLst.Count < period)
                    {
                        isMaQualified = false;
                        break;
                    }

                    double basePrice = (from x in priceLst select x.LastPrice).Average();
                    if (_MovingAvgPrice[code].ContainsKey(period))
                    {
                        _MovingAvgPrice[code][period] = basePrice;
                    }
                    else
                    {
                        _MovingAvgPrice[code].Add(period, basePrice);
                    }

                    double bidBasePrice = (from x in priceLst select x.BidPrice1).Average();
                    if (_MovingAvgBidPrice[code].ContainsKey(period))
                    {
                        _MovingAvgBidPrice[code][period] = bidBasePrice;
                    }
                    else
                    {
                        _MovingAvgBidPrice[code].Add(period, bidBasePrice);
                    }

                    double askBasePrice = (from x in priceLst select x.AskPrice1).Average();
                    if (_MovingAvgAskPrice[code].ContainsKey(period))
                    {
                        _MovingAvgAskPrice[code][period] = askBasePrice;
                    }
                    else
                    {
                        _MovingAvgAskPrice[code].Add(period, askBasePrice);
                    }
                }
            }
            //Utils.WriteLine(string.Format("Get Signal {0} ticks, {1} ns", _TradingWatch.ElapsedTicks, _TradingWatch.ElapsedTicks * Constant.NanosecPerTick));

            if (isMaQualified && _MovingAvgPrice[code].ContainsKey(_ShortPeriod) && _MovingAvgPrice[code].ContainsKey(_LongPeriod))
            {
                bool isUpSignal = IsUpSignal(code, oldShortPeriod, oldLongPeriod, oldShortPeriodAsk, oldLongPeriodAsk);
                bool isDownSignal = IsDownSignal(code, oldShortPeriod, oldLongPeriod, oldShortPeriodBid, oldLongPeriodBid);

                #region 上穿
                if (isUpSignal || _ChaseDic.ContainsKey(buyDirectionKey) && _ChaseDic[buyDirectionKey] && !isDownSignal)
                {
                    if (!_ChaseDic.ContainsKey(buyDirectionKey) || !_ChaseDic[buyDirectionKey])
                    {
                        //Utils.WriteLine(string.Format("Get up signal: {0}", code));
                    }
                    //Cancel Short Order
                    var tempKey = Utils.GetPositionKey(code, EnumPosiDirectionType.Short);
                    if (TradeDataServer.DicPendingOrder.ContainsKey(tempKey))
                    {
                        OrderManager.CancelOrderList(TradeDataServer.DicPendingOrder[tempKey].Values.ToList());
                    }

                    var shortPosKey = Utils.GetPositionKey(code, EnumPosiDirectionType.Short);
                    var longPosKey = Utils.GetPositionKey(code, EnumPosiDirectionType.Long);

                    int longCount = 0;
                    if (TradeDataServer.TraderAdapter.PositionFields.ContainsKey(longPosKey))
                    {
                        longCount = TradeDataServer.TraderAdapter.PositionFields[longPosKey].Position;
                    }
                    int shortCount = 0;
                    if (TradeDataServer.TraderAdapter.PositionFields.ContainsKey(shortPosKey))
                    {
                        shortCount = TradeDataServer.TraderAdapter.PositionFields[shortPosKey].Position;
                    }
                    int netPostionCount = longCount - shortCount;

                    if (shortCount > 0)
                    {
                        int handCount = 2 * Math.Abs(netPostionCount);// _UnitHand; //;
                        int closeHand = handCount <= shortCount ? handCount : shortCount;
                        if (!Constant.IsSentBuyOrder[code])
                        {
                            if (closeHand > 0 && shortCount > closeHand && shortCount > _SideLimit)
                            {
                                int st = 0;
                                int shortTodayCount = TradeDataServer.TraderAdapter.PositionFields[shortPosKey].TodayPosition;
                                int shortHistoryCount = TradeDataServer.TraderAdapter.PositionFields[shortPosKey].Position - shortTodayCount;
                                if (IsSupportCloseToday && shortTodayCount > 0)
                                {
                                    if (closeHand > shortHistoryCount)
                                    {
                                        //Buy Close Today
                                        st = OrderManager.SubmitOrder(code, EnumDirectionType.Buy, askPrice, shortHistoryCount, EnumOffsetFlagType.Close);
                                        //Utils.WriteLine(string.Format("SubmitOrder: {0}, {1}, {2}, {3}, {4}", code, EnumDirectionType.Buy, EnumOffsetFlagType.Close, shortHistoryCount.ToString(), askPrice));
                                        if (st == 0)
                                        {
                                        }
                                        closeHand -= shortHistoryCount;

                                        //Buy Close
                                        st = OrderManager.SubmitOrder(code, EnumDirectionType.Buy, askPrice, closeHand, EnumOffsetFlagType.CloseToday);
                                        //Utils.WriteLine(string.Format("SubmitOrder: {0}, {1}, {2}, {3}, {4}", code, EnumDirectionType.Buy, EnumOffsetFlagType.CloseToday, closeHand.ToString(), askPrice));
                                        if (st == 0)
                                        {
                                            TradingWatch.Stop();
                                            Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                        }
                                    }
                                    else
                                    {
                                        //Buy Close Today
                                        st = OrderManager.SubmitOrder(code, EnumDirectionType.Buy, askPrice, closeHand, EnumOffsetFlagType.Close);
                                        //Utils.WriteLine(string.Format("SubmitOrder: {0}, {1}, {2}, {3}, {4}", code, EnumDirectionType.Buy, EnumOffsetFlagType.Close, closeHand.ToString(), askPrice));
                                        if (st == 0)
                                        {
                                            TradingWatch.Stop();
                                            Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                        }
                                    }
                                }
                                else
                                {
                                    //Buy Close
                                    st = OrderManager.SubmitOrder(code, EnumDirectionType.Buy, askPrice, closeHand, EnumOffsetFlagType.Close);
                                    //Utils.WriteLine(string.Format("SubmitOrder: {0}, {1}, {2}, {3}, {4}", code, EnumDirectionType.Buy, EnumOffsetFlagType.Close, closeHand.ToString(), askPrice));
                                    if (st == 0)
                                    {
                                        TradingWatch.Stop();
                                        Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                    }
                                }
                            }
                            //handCount -= shortCount;
                            else if (handCount > 0 && Math.Abs(netPostionCount + handCount) <= _PositionNetLimit)
                            {
                                //Buy Open
                                int st = OrderManager.SubmitOrder(code, EnumDirectionType.Buy, askPrice, handCount, EnumOffsetFlagType.Open);
                                //Utils.WriteLine(string.Format("SubmitOrder: {0}, {1}, {2}, {3}, {4}", code, EnumDirectionType.Buy, EnumOffsetFlagType.Open, handCount.ToString(), askPrice));
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                }
                            }
                        }
                    }
                    else
                    {
                        int handCount = 2 * Math.Abs(netPostionCount);// _UnitHand;// ;
                        var longKey = Utils.GetPositionKey(code, EnumPosiDirectionType.Long);
                        if (TradeDataServer.TraderAdapter.PositionFields.ContainsKey(longKey))
                        {
                            handCount = _PositionNetLimit > Math.Abs(netPostionCount) ? Math.Min(_PositionNetLimit - Math.Abs(netPostionCount), handCount) : 0;
                        }
                        if (handCount > 0 && !Constant.IsSentBuyOrder[code])
                        {
                            //Buy Open
                            int st = OrderManager.SubmitOrder(code, EnumDirectionType.Buy, askPrice, handCount, EnumOffsetFlagType.Open);
                            //Utils.WriteLine(string.Format("SubmitOrder: {0}, {1}, {2}, {3}, {4}", code, EnumDirectionType.Buy, EnumOffsetFlagType.Open, handCount, askPrice));
                            if (st == 0)
                            {
                                TradingWatch.Stop();
                                Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                            }
                        }

                    }
                    SetInstrumentChaseDic(code, EnumDirectionType.Buy, false);
                }
                #endregion

                #region 下穿
                else if (isDownSignal || _ChaseDic.ContainsKey(sellDirectionKey) && _ChaseDic[sellDirectionKey] && !isUpSignal)
                {
                    if (!_ChaseDic.ContainsKey(sellDirectionKey) || !_ChaseDic[sellDirectionKey])
                    {
                        //Utils.WriteLine(string.Format("Get down signal: {0}", code));
                    }
                    //Cancel Long Order
                    var tempKey = Utils.GetPositionKey(code, EnumPosiDirectionType.Long);
                    if (TradeDataServer.DicPendingOrder.ContainsKey(tempKey))
                    {
                        OrderManager.CancelOrderList(TradeDataServer.DicPendingOrder[tempKey].Values.ToList());
                    }

                    var longPosKey = Utils.GetPositionKey(code, EnumPosiDirectionType.Long);
                    var shortPosKey = Utils.GetPositionKey(code, EnumPosiDirectionType.Short);
                    int longCount = 0;
                    if (TradeDataServer.TraderAdapter.PositionFields.ContainsKey(longPosKey))
                    {
                        longCount = TradeDataServer.TraderAdapter.PositionFields[longPosKey].Position;
                    }
                    int shortCount = 0;
                    if (TradeDataServer.TraderAdapter.PositionFields.ContainsKey(shortPosKey))
                    {
                        shortCount = TradeDataServer.TraderAdapter.PositionFields[shortPosKey].Position;
                    }
                    int netPostionCount = longCount - shortCount;

                    if (longCount > 0)
                    {
                        int handCount = 2 * Math.Abs(netPostionCount); //_UnitHand;// 
                        int closeHand = handCount <= longCount ? handCount : longCount;
                        if (!Constant.IsSentSellOrder[code])
                        {
                            if (closeHand > 0 && longCount > closeHand && longCount > _SideLimit)
                            {
                                int st = 0;
                                int longTodayCount = TradeDataServer.TraderAdapter.PositionFields[longPosKey].TodayPosition;
                                int longHistoryCount = TradeDataServer.TraderAdapter.PositionFields[shortPosKey].Position - longTodayCount;
                                if (IsSupportCloseToday && longHistoryCount > 0)
                                {
                                    if (closeHand > longHistoryCount)
                                    {
                                        //Buy Close Today
                                        st = OrderManager.SubmitOrder(code, EnumDirectionType.Sell, bidPrice, longHistoryCount, EnumOffsetFlagType.Close);
                                        //Utils.WriteLine(string.Format("SubmitOrder: {0}, {1}, {2}, {3}, {4}", code, EnumDirectionType.Sell, EnumOffsetFlagType.Close, longHistoryCount.ToString(), bidPrice));
                                        if (st == 0)
                                        {
                                        }
                                        closeHand -= longHistoryCount;

                                        //Buy Close
                                        st = OrderManager.SubmitOrder(code, EnumDirectionType.Sell, bidPrice, closeHand, EnumOffsetFlagType.CloseToday);
                                        //Utils.WriteLine(string.Format("SubmitOrder: {0}, {1}, {2}, {3}, {4}", code, EnumDirectionType.Sell, EnumOffsetFlagType.CloseToday, closeHand.ToString(), bidPrice));
                                        if (st == 0)
                                        {
                                            TradingWatch.Stop();
                                            Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                        }
                                    }
                                    else
                                    {
                                        //Buy Close Today
                                        st = OrderManager.SubmitOrder(code, EnumDirectionType.Sell, bidPrice, closeHand, EnumOffsetFlagType.CloseToday);
                                        //Utils.WriteLine(string.Format("SubmitOrder: {0}, {1}, {2}, {3}, {4}", code, EnumDirectionType.Sell, EnumOffsetFlagType.Close, closeHand.ToString(), bidPrice));
                                        if (st == 0)
                                        {
                                            TradingWatch.Stop();
                                            Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                        }
                                    }
                                }
                                else
                                {
                                    //Sell Close
                                    st = OrderManager.SubmitOrder(code, EnumDirectionType.Sell, bidPrice, closeHand, EnumOffsetFlagType.Close);
                                    //Utils.WriteLine(string.Format("SubmitOrder: {0}, {1}, {2}, {3}, {4}", code, EnumDirectionType.Sell, EnumOffsetFlagType.Close, closeHand.ToString(), bidPrice));
                                    if (st == 0)
                                    {
                                        TradingWatch.Stop();
                                        Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                    }
                                }
                            }
                            //handCount -= longCount;
                            else if (handCount > 0 && Math.Abs(netPostionCount - handCount) <= _PositionNetLimit)//if (handCount > 0)
                            {
                                //Sell Open
                                int st = OrderManager.SubmitOrder(code, EnumDirectionType.Sell, bidPrice, handCount, EnumOffsetFlagType.Open);
                                //Utils.WriteLine(string.Format("SubmitOrder: {0}, {1}, {2}, {3}, {4}", code, EnumDirectionType.Sell, bidPrice, EnumOffsetFlagType.Open, handCount.ToString(), bidPrice));
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                }
                            }
                        }
                    }
                    else
                    {
                        int handCount = 2 * Math.Abs(netPostionCount); //_UnitHand;// 
                        var shortKey = Utils.GetPositionKey(code, EnumPosiDirectionType.Short);
                        if (TradeDataServer.TraderAdapter.PositionFields.ContainsKey(shortKey))
                        {
                            handCount = _PositionNetLimit > Math.Abs(netPostionCount) ? Math.Min(_PositionNetLimit - Math.Abs(netPostionCount), handCount) : 0;
                        }
                        if (handCount > 0 && !Constant.IsSentSellOrder[code])
                        {
                            //Sell Open
                            int st = OrderManager.SubmitOrder(code, EnumDirectionType.Sell, bidPrice, handCount, EnumOffsetFlagType.Open);
                            //Utils.WriteLine(string.Format("SubmitOrder: {0}, {1}, {2}, {3}, {4}", code, EnumDirectionType.Sell, bidPrice, EnumOffsetFlagType.Close, handCount, bidPrice));
                            if (st == 0)
                            {
                                TradingWatch.Stop();
                                Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                            }
                        }
                    }
                    SetInstrumentChaseDic(code, EnumDirectionType.Sell, false);
                }
                #endregion

                if (TradingWatch.IsRunning)
                {
                    TradingWatch.Stop();
                }
            }
            #endregion

            #region Cancel Orders

            GetCancelOrderLst(code, bidPrice, askPrice);

            #endregion

            IsFollowRules = RiskManager.FollowCancelOrderRule(code);
        }

        private bool IsUpSignal(string code, double oldShortPeriod, double oldLongPeriod, double oldShortPeriodAsk, double oldLongPeriodAsk)
        {
            if (_MovingAvgPrice[code][_ShortPeriod] > _MovingAvgPrice[code][_LongPeriod] && oldShortPeriod < oldLongPeriod && oldShortPeriod > 0)
            {
                if (_MovingAvgAskPrice[code][_ShortPeriod] > _MovingAvgAskPrice[code][_LongPeriod] && oldShortPeriodAsk <= oldLongPeriodAsk && oldShortPeriodAsk > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        private bool IsDownSignal(string code, double oldShortPeriod, double oldLongPeriod, double oldShortPeriodBid, double oldLongPeriodBid)
        {
            if (_MovingAvgPrice[code][_ShortPeriod] < _MovingAvgPrice[code][_LongPeriod] && oldShortPeriod > oldLongPeriod && oldLongPeriod > 0)
            {
                if (_MovingAvgBidPrice[code][_ShortPeriod] < _MovingAvgBidPrice[code][_LongPeriod] && oldShortPeriodBid >= oldLongPeriodBid && oldLongPeriodBid > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        private bool GetCancelOrderLst(string code, double bidPrice, double askPrice)
        {
            bool retCancel = false;
            var tempKey = Utils.GetPositionKey(code, EnumPosiDirectionType.Long);
            if (TradeDataServer.DicPendingOrder.ContainsKey(tempKey))
            {
                foreach (ThostFtdcOrderField item in TradeDataServer.DicPendingOrder[tempKey].Values)
                {
                    if (item.InstrumentID == code)
                    {
                        if (item.Direction == EnumDirectionType.Buy && Math.Abs(bidPrice - item.LimitPrice) > _CancelPriceLmt
                                || item.Direction == EnumDirectionType.Sell && Math.Abs(askPrice - item.LimitPrice) > _CancelPriceLmt
                                || Constant.IsOrderOverTime(item, CancelSec))
                        {
                            OrderManager.CancelOrder(item);
                            retCancel = true;
                        }
                    }
                }
            }

            tempKey = Utils.GetPositionKey(code, EnumPosiDirectionType.Short);
            if (TradeDataServer.DicPendingOrder.ContainsKey(tempKey))
            {
                foreach (ThostFtdcOrderField item in TradeDataServer.DicPendingOrder[tempKey].Values)
                {
                    if (item.InstrumentID == code)
                    {
                        if (item.Direction == EnumDirectionType.Buy && Math.Abs(bidPrice - item.LimitPrice) > _CancelPriceLmt
                                || item.Direction == EnumDirectionType.Sell && Math.Abs(askPrice - item.LimitPrice) > _CancelPriceLmt
                                || Constant.IsOrderOverTime(item, CancelSec))
                        {
                            OrderManager.CancelOrder(item);
                            retCancel = true;
                        }
                    }
                }
            }
            return retCancel;
        }

        private bool GetCancelOrderDirectionLst(string code, EnumPosiDirectionType posiDirection, double bidPrice, double askPrice, double cancelLmt)
        {
            bool retCancel = false;
            var tempKey = Utils.GetPositionKey(code, posiDirection);
            if (TradeDataServer.DicPendingOrder.ContainsKey(tempKey))
            {
                foreach (ThostFtdcOrderField item in TradeDataServer.DicPendingOrder[tempKey].Values)
                {
                    if (item.InstrumentID == code
                        && (item.Direction == EnumDirectionType.Buy && Math.Abs(askPrice - item.LimitPrice) > cancelLmt
                         || item.Direction == EnumDirectionType.Sell && Math.Abs(bidPrice - item.LimitPrice) > cancelLmt))
                    {
                        OrderManager.CancelOrder(item);
                        retCancel = true;
                    }
                }
            }
            return retCancel;
        }

        private void SetInstrumentChaseDic(string instrumentID, EnumDirectionType direction, bool value)
        {
            string directionKey = Utils.GetDirectionKey(instrumentID, direction);
            if (_ChaseDic.ContainsKey(directionKey))
            {
                _ChaseDic[directionKey] = value;
            }
            else
            {
                _ChaseDic.Add(directionKey, value);
            }
            if (value)
            {
                //Utils.WriteLine(string.Format("Set {0} {1}", directionKey, value));
            }
        }

        private void OperationEndofTradingDay(string code)
        {
            var longPosKey = Utils.GetPositionKey(code, EnumPosiDirectionType.Long);
            var shortPosKey = Utils.GetPositionKey(code, EnumPosiDirectionType.Short);
            int longCount = 0;
            if (TradeDataServer.TraderAdapter.PositionFields.ContainsKey(longPosKey))
            {
                longCount = TradeDataServer.TraderAdapter.PositionFields[longPosKey].Position;
            }
            int shortCount = 0;
            if (TradeDataServer.TraderAdapter.PositionFields.ContainsKey(shortPosKey))
            {
                shortCount = TradeDataServer.TraderAdapter.PositionFields[shortPosKey].Position;
            }
            int netPostionCount = longCount - shortCount;

            //Close Net Position
        }
    }
}
