using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Configuration;
using System.Collections.Concurrent;

namespace AutoTrader
{
    public class OrderManager
    {
        public OrderManager(TradeDataServer tradeDataServer)
        {
            TradeDataServer = tradeDataServer;
            //string tempOrderRef = DateTime.Now.Ticks.ToString();
            _MaxOrderRef = 0;//int.Parse(tempOrderRef.Substring(6, Constant.ReferenceLength));
            MarketToLimit = false;
            MarketToLimit = bool.Parse(ConfigurationManager.AppSettings["MarketToLimit"]);
            //CancelSec = int.Parse(ConfigurationManager.AppSettings["OverTimeSec"]);
            CancelLimit = int.Parse(ConfigurationManager.AppSettings["CancelTick"]);
        }

        public bool MarketToLimit { get; set; }
        //public int CancelSec { get; set; }
        public int CancelLimit { get; set; }

        public TradeDataServer TradeDataServer { get; set; }

        private Stopwatch _TradingWatch = new Stopwatch();

        public string CombineOrderRef;
        /// <summary>
        /// 最大报单引用
        /// </summary>
        private int _MaxOrderRef;

        public void SerMaxOrderRef(int maxOrderRef)
        {
            if (maxOrderRef > _MaxOrderRef)
            {
                _MaxOrderRef = maxOrderRef;
            }
        }

        public int SubmitOrder(string instrumentId, EnumThostDirectionType direction, double price, int nVolume, bool isReqClose = false, string clientTag = "", EnumOrderType orderType = EnumOrderType.Limit, bool closeFirst = true, bool closePrevFirst = true, bool isMaxLimit = false, int maxLimit = 0)
        {
            if (Util.IsInReLoadingStatus(instrumentId))
            {
                Util.WriteInfo(string.Format("SubmitOrder fake order: {0}", instrumentId), Program.InPrintMode);
                return 0;
            }
            if (nVolume <= 0)
            {
                Util.WriteError(string.Format("SubmitOrder illegal volume: {0}", nVolume));
                return -10;
            }
            if (isMaxLimit && maxLimit <= 0)
            {
                Util.WriteError(string.Format("SubmitOrder illegal maxLimit: {0}", maxLimit));
                return -10;
            }
            int st = 0;
            bool isShfeRule = TradeDataServer.IsShfeRule(instrumentId);
            bool isCffexRule = TradeDataServer.IsCffexRule(instrumentId);
            if (direction == EnumThostDirectionType.Buy)
            {
                int shortKey = Util.GetPositionKey(instrumentId, EnumThostPosiDirectionType.Short);
                int shortPos = 0;
                int shortTodayPos = 0;
                if (TradeDataServer.PositionFields.ContainsKey(shortKey))
                {
                    PositionField currPos = TradeDataServer.PositionFields[shortKey];
                    shortPos = currPos.Position;
                    shortTodayPos = currPos.TodayPosition;
                }
                if (closeFirst || isReqClose)
                {
                    if (shortPos > 0)
                    {
                        int canCloseVol = Math.Min(shortPos, nVolume);
                        int closeLeftVolume = nVolume;
                        if (isShfeRule)
                        {
                            if (shortPos > shortTodayPos && closePrevFirst)
                            {
                                int prevHold = shortPos - shortTodayPos;
                                int canClosePrevVol = Math.Min(prevHold, canCloseVol);
                                st += OrderInsert(instrumentId, direction, price, canClosePrevVol, EnumThostOffsetFlagType.Close, orderType, clientTag);
                                if (canCloseVol > canClosePrevVol)
                                {
                                    st += OrderInsert(instrumentId, direction, price, canCloseVol - canClosePrevVol, EnumThostOffsetFlagType.CloseToday, orderType, clientTag);
                                }
                            }
                            else //if (!onlyClosePrev)
                            {
                                int canCloseTodayVol = Math.Min(shortTodayPos, canCloseVol);
                                st += OrderInsert(instrumentId, direction, price, canCloseTodayVol, EnumThostOffsetFlagType.CloseToday, orderType, clientTag);
                                if (canCloseVol > canCloseTodayVol)
                                {
                                    st += OrderInsert(instrumentId, direction, price, canCloseVol - canCloseTodayVol, EnumThostOffsetFlagType.Close, orderType, clientTag);
                                }
                            }
                            closeLeftVolume = Math.Max(nVolume - canCloseVol, 0);
                        }
                        else if (isCffexRule)
                        {
                            int prevHold = shortPos - shortTodayPos;
                            if (prevHold > 0 && closePrevFirst)
                            {
                                int canClosePrevVol = Math.Min(prevHold, canCloseVol);
                                st += OrderInsert(instrumentId, direction, price, canClosePrevVol, EnumThostOffsetFlagType.Close, orderType, clientTag);
                                closeLeftVolume = Math.Max(nVolume - canClosePrevVol, 0);
                            }
                            else if (!closePrevFirst)
                            {
                                int canClosePrevVol = Math.Min(prevHold, canCloseVol);
                                st += OrderInsert(instrumentId, direction, price, canClosePrevVol, EnumThostOffsetFlagType.Close, orderType, clientTag);
                                if (canCloseVol > canClosePrevVol)
                                {
                                    st += OrderInsert(instrumentId, direction, price, canCloseVol - canClosePrevVol, EnumThostOffsetFlagType.CloseToday, orderType, clientTag);
                                }
                                closeLeftVolume = Math.Max(nVolume - canCloseVol, 0);
                            }
                        }
                        else
                        {
                            st += OrderInsert(instrumentId, direction, price, canCloseVol, EnumThostOffsetFlagType.Close, orderType, clientTag);
                            closeLeftVolume = Math.Max(nVolume - canCloseVol, 0);
                        }
                        if (closeLeftVolume > 0 && !isReqClose)
                        {
                            int volume = closeLeftVolume;
                            if (isMaxLimit)
                            {
                                volume = Math.Min(maxLimit, closeLeftVolume);
                            }
                            st += OrderInsert(instrumentId, direction, price, volume, EnumThostOffsetFlagType.Open, orderType, clientTag);
                        }
                        Util.WriteInfo(string.Format("Position before order: {0}, Short {1}, TodayShort {2}", instrumentId, shortPos, shortTodayPos), Program.InPrintMode);
                    }
                    else if (!isReqClose)
                    {
                        int volume = nVolume;
                        if (isMaxLimit)
                        {
                            volume = Math.Min(maxLimit, nVolume);
                        }
                        st += OrderInsert(instrumentId, direction, price, volume, EnumThostOffsetFlagType.Open, orderType, clientTag);
                    }
                    else
                    {
                        Util.WriteError(string.Format("Error in submitted order field: isReqClose {0}, clientTag {1}, orderType {2}, closeFirst{3}, closePrevFirst {4}", isReqClose, clientTag, orderType, closeFirst, closePrevFirst));
                        return -20;
                    }
                }
                else
                {
                    EnumThostOffsetFlagType openOrClose = ChooseType(shortPos, shortTodayPos, nVolume, isShfeRule, closePrevFirst);
                    st += OrderInsert(instrumentId, direction, price, nVolume, openOrClose, orderType, clientTag);
                }
            }
            else if (direction == EnumThostDirectionType.Sell)
            {
                int longKey = Util.GetPositionKey(instrumentId, EnumThostPosiDirectionType.Long);
                int longPos = 0;
                int longTodayPos = 0;
                if (TradeDataServer.PositionFields.ContainsKey(longKey))
                {
                    PositionField currPos = TradeDataServer.PositionFields[longKey];
                    longPos = currPos.Position;
                    longTodayPos = currPos.TodayPosition;
                }
                if (closeFirst || isReqClose)
                {
                    if (longPos > 0)
                    {
                        int canCloseVol = Math.Min(longPos, nVolume);
                        int closeLeftVolume = nVolume;
                        if (isShfeRule)
                        {
                            if (longPos > longTodayPos && closePrevFirst)
                            {
                                int prevHold = longPos - longTodayPos;
                                int canClosePrevVol = Math.Min(prevHold, canCloseVol);
                                st += OrderInsert(instrumentId, direction, price, canClosePrevVol, EnumThostOffsetFlagType.Close, orderType, clientTag);
                                if (canCloseVol > canClosePrevVol)
                                {
                                    st += OrderInsert(instrumentId, direction, price, canCloseVol - canClosePrevVol, EnumThostOffsetFlagType.CloseToday, orderType, clientTag);
                                }
                            }
                            else //if (!onlyClosePrev)
                            {
                                int canCloseTodayVol = Math.Min(longTodayPos, canCloseVol);
                                st += OrderInsert(instrumentId, direction, price, canCloseTodayVol, EnumThostOffsetFlagType.CloseToday, orderType, clientTag);
                                if (canCloseVol > canCloseTodayVol)
                                {
                                    st += OrderInsert(instrumentId, direction, price, canCloseVol - canCloseTodayVol, EnumThostOffsetFlagType.Close, orderType, clientTag);
                                }
                            }
                            closeLeftVolume = Math.Max(nVolume - canCloseVol, 0);
                        }
                        else if (isCffexRule)
                        {
                            int prevHold = longPos - longTodayPos;
                            if (prevHold > 0 && closePrevFirst)
                            {
                                int canClosePrevVol = Math.Min(prevHold, canCloseVol);
                                st += OrderInsert(instrumentId, direction, price, canClosePrevVol, EnumThostOffsetFlagType.Close, orderType, clientTag);
                                closeLeftVolume = Math.Max(nVolume - canClosePrevVol, 0);
                            }
                            else if (!closePrevFirst)
                            {
                                int canClosePrevVol = Math.Min(prevHold, canCloseVol);
                                st += OrderInsert(instrumentId, direction, price, canClosePrevVol, EnumThostOffsetFlagType.Close, orderType, clientTag);
                                if (canCloseVol > canClosePrevVol)
                                {
                                    st += OrderInsert(instrumentId, direction, price, canCloseVol - canClosePrevVol, EnumThostOffsetFlagType.CloseToday, orderType, clientTag);
                                }
                                closeLeftVolume = Math.Max(nVolume - canCloseVol, 0);
                            }
                        }
                        else
                        {
                            st += OrderInsert(instrumentId, direction, price, canCloseVol, EnumThostOffsetFlagType.Close, orderType, clientTag);
                            closeLeftVolume = Math.Max(nVolume - canCloseVol, 0);
                        }
                        if (closeLeftVolume > 0 && !isReqClose)
                        {
                            int volume = closeLeftVolume;
                            if (isMaxLimit)
                            {
                                volume = Math.Min(maxLimit, closeLeftVolume);
                            }
                            st += OrderInsert(instrumentId, direction, price, volume, EnumThostOffsetFlagType.Open, orderType, clientTag);//nVolume - longPos
                        }
                        Util.WriteInfo(string.Format("Position before order: {0}, Long {1}, TodayLong {2}", instrumentId, longPos, longTodayPos), Program.InPrintMode);
                    }
                    else if (!isReqClose)
                    {
                        int volume = nVolume;
                        if (isMaxLimit)
                        {
                            volume = Math.Min(maxLimit, nVolume);
                        }
                        st += OrderInsert(instrumentId, direction, price, volume, EnumThostOffsetFlagType.Open, orderType, clientTag);
                    }
                    else
                    {
                        Util.WriteError(string.Format("Error in submitted order field: isReqClose {0}, clientTag {1}, orderType {2}, closeFirst{3}, closePrevFirst {4}", isReqClose, clientTag, orderType, closeFirst, closePrevFirst));
                        return -20;
                    }
                }
                else
                {
                    EnumThostOffsetFlagType openOrClose = ChooseType(longPos, longTodayPos, nVolume, isShfeRule, closePrevFirst);
                    st += OrderInsert(instrumentId, direction, price, nVolume, openOrClose, orderType, clientTag);
                }
            }
            return st;
        }

        private int OrderInsert(string instrumentId, EnumThostDirectionType direction, double price, int nVolume, EnumThostOffsetFlagType openOrClose, EnumOrderType orderType, string clientTag, bool closeFirst = true)
        {
            if (nVolume <= 0)
            {
                Util.WriteError(string.Format("OrderInsert illeglal volume: {0}", nVolume));
                return -10;
            }
            if (!TradeDataServer.InstrumentFields.ContainsKey(instrumentId))
            {
                return -100;
            }
            double oppositePrice = 0.0;
            int st = 0;
            EnumOrderType originOrderType = orderType;
            if (originOrderType == EnumOrderType.Market && MarketToLimit)
            {
                orderType = EnumOrderType.Limit;
                if (direction == EnumThostDirectionType.Buy)
                {
                    price = TradeDataServer.DepthMarketDataFields[instrumentId].UpperLimitPrice;
                }
                else if (direction == EnumThostDirectionType.Sell)
                {
                    price = TradeDataServer.DepthMarketDataFields[instrumentId].LowerLimitPrice;
                }
            }
            if (direction == EnumThostDirectionType.Buy)
            {
                oppositePrice = TradeDataServer.DepthMarketDataFields[instrumentId].AskPrice1;
            }
            else if (direction == EnumThostDirectionType.Sell)
            {
                oppositePrice = TradeDataServer.DepthMarketDataFields[instrumentId].BidPrice1;
            }
            if (nVolume <= 50)
            {
                if (originOrderType == EnumOrderType.Market && !string.IsNullOrEmpty(clientTag))
                {
                    if (Util.MarkupOrderReportDict.ContainsKey(instrumentId) && Util.MarkupOrderReportDict[instrumentId].ContainsKey(clientTag))
                    {
                        MarkupOrderStatistics mkStatistics = Util.MarkupOrderReportDict[instrumentId][clientTag];
                        mkStatistics.InstrumentID = instrumentId;
                        mkStatistics.CommitVolume += nVolume;
                    }
                }
                Interlocked.Increment(ref _MaxOrderRef);
                CombineOrderRef = Util.GetInputOrderRefFromRefIndex(clientTag, _MaxOrderRef);
                Util.TradingReportDict[CombineOrderRef] = new TradingReport()
                {
                    InstrumentID = instrumentId,
                    BuySell = direction,
                    OpenClose = openOrClose,
                    CommitVolume = nVolume,
                    OppositePrice = oppositePrice,
                    OrderRef = CombineOrderRef,
                    Watch = Stopwatch.StartNew()
                };
                Util.WriteInfo(string.Format("ReqOrderInsert: {0}, direction {1}, price {2}, nVolume {3}, openOrClose {4}, OrderRef {5}, orderType {6}", instrumentId, direction, price, nVolume, openOrClose, CombineOrderRef, orderType));
                st += TradeDataServer._CtpTraderApi.OrderInsert(instrumentId, direction, price, nVolume, openOrClose, CombineOrderRef, orderType);

                if (direction == EnumThostDirectionType.Buy)
                {
                    Util.IsSentBuyOrder[instrumentId] = true;
                    Util.IsFilledBuyOrder[instrumentId] = false;
                }
                else if (direction == EnumThostDirectionType.Sell)
                {
                    Util.IsSentSellOrder[instrumentId] = true;
                    Util.IsFilledSellOrder[instrumentId] = false;
                }
            }
            else
            {
                if (originOrderType == EnumOrderType.Market && !string.IsNullOrEmpty(clientTag))
                {
                    if (Util.MarkupOrderReportDict.ContainsKey(instrumentId) && Util.MarkupOrderReportDict[instrumentId].ContainsKey(clientTag))
                    {
                        MarkupOrderStatistics mkStatistics = Util.MarkupOrderReportDict[instrumentId][clientTag];
                        mkStatistics.InstrumentID = instrumentId;
                        mkStatistics.CommitVolume += nVolume;
                    }
                }
                int division = 10; //nVolume / splitKey + nVolume % splitKey;
                int maxHandCount = nVolume / division;
                int remainingCount = nVolume;
                while (remainingCount > 0)
                {
                    Random rd = new Random(remainingCount);
                    int tempHand = rd.Next(Math.Max(1, maxHandCount >> 1), maxHandCount);
                    if (remainingCount >= tempHand)
                    {
                        Interlocked.Increment(ref _MaxOrderRef);
                        CombineOrderRef = Util.GetInputOrderRefFromRefIndex(clientTag, _MaxOrderRef);
                        Util.TradingReportDict[CombineOrderRef] = new TradingReport()
                        {
                            InstrumentID = instrumentId,
                            BuySell = direction,
                            OpenClose = openOrClose,
                            CommitVolume = nVolume,
                            OppositePrice = oppositePrice,
                            OrderRef = CombineOrderRef,
                            Watch = Stopwatch.StartNew()
                        };
                        st += TradeDataServer._CtpTraderApi.OrderInsert(instrumentId, direction, price, tempHand, openOrClose, CombineOrderRef, orderType);
                        Util.WriteInfo(string.Format("ReqOrderInsert: {0}, direction {1}, price {2}, nVolume {3}, openOrClose {4}, OrderRef {5}, orderType {6}", instrumentId, direction, price, tempHand, openOrClose, CombineOrderRef, orderType));
                        remainingCount -= tempHand;
                    }
                    else
                    {
                        Interlocked.Increment(ref _MaxOrderRef);
                        CombineOrderRef = Util.GetInputOrderRefFromRefIndex(clientTag, _MaxOrderRef);
                        Util.TradingReportDict[CombineOrderRef] = new TradingReport()
                        {
                            InstrumentID = instrumentId,
                            BuySell = direction,
                            OpenClose = openOrClose,
                            CommitVolume = nVolume,
                            OrderRef = CombineOrderRef,
                            Watch = Stopwatch.StartNew()
                        };
                        st += TradeDataServer._CtpTraderApi.OrderInsert(instrumentId, direction, price, remainingCount, openOrClose, CombineOrderRef, orderType);
                        Util.WriteInfo(string.Format("ReqOrderInsert: {0}, direction {1}, price {2}, nVolume {3}, openOrClose {4}, OrderRef {5}, orderType {6}", instrumentId, direction, price, remainingCount, openOrClose, CombineOrderRef, orderType), Program.InPrintMode);
                        remainingCount = 0;
                    }

                    if (direction == EnumThostDirectionType.Buy)
                    {
                        Util.IsSentBuyOrder[instrumentId] = true;
                        Util.IsFilledBuyOrder[instrumentId] = false;
                    }
                    else if (direction == EnumThostDirectionType.Sell)
                    {
                        Util.IsSentSellOrder[instrumentId] = true;
                        Util.IsFilledSellOrder[instrumentId] = false;
                    }
                }
            }
            return 0;
        }

        public int TestOrderManager(string instrumentId)
        {
            while (!TradeDataServer.DepthMarketDataFields.ContainsKey(instrumentId))
            {
                Util.WriteInfo(string.Format("Waiting for quote key: {0}", instrumentId));
                Thread.Sleep(1000);
            }

            double price = TradeDataServer.DepthMarketDataFields[instrumentId].LowerLimitPrice;
            if (TradeDataServer.InstrumentFields.ContainsKey(instrumentId))
            {
                price = Math.Max(price - TradeDataServer.InstrumentFields[instrumentId].PriceTick, TradeDataServer.InstrumentFields[instrumentId].PriceTick);
            }
            SubmitOrder(instrumentId, EnumThostDirectionType.Buy, price, 1, false);

            price = TradeDataServer.DepthMarketDataFields[instrumentId].UpperLimitPrice;
            if (TradeDataServer.InstrumentFields.ContainsKey(instrumentId))
            {
                price += TradeDataServer.InstrumentFields[instrumentId].PriceTick;
            }
            Interlocked.Increment(ref _MaxOrderRef);
            return TradeDataServer._CtpTraderApi.OrderInsert(instrumentId, EnumThostDirectionType.Sell, price, 1, EnumThostOffsetFlagType.Open, _MaxOrderRef.ToString());
        }

        public void CancelOrderList(List<CThostFtdcOrderField> orderList)
        {
            foreach (CThostFtdcOrderField item in orderList)
            {
                CancelOrder(item);
            }
        }

        public void CancelOrder(CThostFtdcOrderField orderfield)
        {
            TradeDataServer._CtpTraderApi.OrderAction(orderfield.InstrumentID, orderfield.FrontID, orderfield.SessionID, orderfield.OrderRef);
            Util.WriteInfo(string.Format("ReqOrderAction: {0}, FrontID {1}, SessionID {2}, OrderRef {3}", orderfield.InstrumentID, orderfield.FrontID, orderfield.SessionID, orderfield.OrderRef));
        }

        public static bool IsCancellable(CThostFtdcOrderField pOrder)
        {
            return pOrder.OrderStatus != EnumThostOrderStatusType.AllTraded && pOrder.OrderStatus != EnumThostOrderStatusType.Canceled && pOrder.OrderStatus != EnumThostOrderStatusType.Unknown;
        }

        public bool IsOrderOverTime(CThostFtdcOrderField pOrder, double cancelSec)
        {
            DateTime insertTime = DateTime.ParseExact(pOrder.InsertTime, "HH:mm:ss", null);
            TimeSpan breakSpan = TradingTimeManager.GetTimeGap(pOrder.InstrumentID, DateTime.Now, insertTime);
            if (breakSpan.TotalSeconds > cancelSec - Util.ExchangeTimeOffset)
            {
                Util.WriteInfo(string.Format("OrderRef: {0} is overtime: {1} {2} {3} {4} {5}", pOrder.OrderRef, pOrder.InstrumentID, pOrder.Direction, pOrder.CombOffsetFlag_0, pOrder.LimitPrice, pOrder.VolumeTotal), Program.InPrintMode);
                return true;
            }
            return false;
        }

        public bool IsOrderOverTick(CThostFtdcOrderField pOrder, double PresentBidPrice, double PresentAskPrice)
        {
            if (pOrder.Direction == EnumThostDirectionType.Buy && (PresentAskPrice - pOrder.LimitPrice).IsGreaterThan(CancelLimit)
             || pOrder.Direction == EnumThostDirectionType.Sell && (pOrder.LimitPrice - PresentBidPrice).IsGreaterThan(CancelLimit))
            {
                Util.WriteInfo(string.Format("Order id: {0} is overtick: {1} {2} {3} {4} {5}", pOrder.OrderSysID, pOrder.InstrumentID, pOrder.Direction, pOrder.CombOffsetFlag_0, pOrder.LimitPrice, pOrder.VolumeTotal));
                return true;
            }
            return false;
        }

        private EnumThostOffsetFlagType ChooseType(int currHold, int todayHold, int volume, bool isShfeRule, bool isClose = false, bool closePrevFirst = true, bool onlyClosePrev = false)
        {
            int prevHold = currHold - todayHold;

            //if (!closePrevFirst) //CloseToday
            //{
            //    if (!onlyClosePrev)
            //    {
            //        if (CheckHold(currHold, volume))
            //        {
            //            return EnumThostOffsetFlagType.CloseToday;//.close;
            //        }
            //    }

            //    if (CheckHold(prevHold, volume))
            //    {
            //        return EnumThostOffsetFlagType.Close;//.closePrev;
            //    }
            //}
            //else
            //{
            //    if (CheckHold(prevHold, volume))
            //    {
            //        return EnumThostOffsetFlagType.Close;//closePrev;
            //    }

            //    if (!onlyClosePrev)
            //    {
            //        if (CheckHold(currHold, volume))
            //        {
            //            return EnumThostOffsetFlagType.CloseToday;//close; 
            //        }
            //    }
            //}

            if (isShfeRule)
            {
                if (closePrevFirst)
                {
                    if (CheckHold(prevHold, volume))
                    {
                        return EnumThostOffsetFlagType.Close;//closePrev;
                    }

                    if (!onlyClosePrev)
                    {
                        if (CheckHold(todayHold, volume))
                        {
                            return EnumThostOffsetFlagType.CloseToday;//close; 
                        }
                    }
                }
                else //CloseToday
                {
                    if (!onlyClosePrev)
                    {
                        if (CheckHold(todayHold, volume))
                        {
                            return EnumThostOffsetFlagType.CloseToday;//.close;
                        }
                    }

                    if (CheckHold(prevHold, volume))
                    {
                        return EnumThostOffsetFlagType.Close;//.closePrev;
                    }
                }
            }
            else
            {
                if (CheckHold(currHold, volume))
                {
                    return EnumThostOffsetFlagType.Close;
                }
            }
            if (isClose)
            {
                if (isShfeRule)
                {
                    return EnumThostOffsetFlagType.CloseToday;
                }
                return EnumThostOffsetFlagType.Close;
            }
            return EnumThostOffsetFlagType.Open;
        }

        private bool CheckHold(long holdVal, int volume)
        {
            return holdVal >= volume;
        }
    }
}
