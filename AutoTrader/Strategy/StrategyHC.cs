using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using CTP;
using WrapperTest;
using System.Configuration;

namespace HFTrader
{
    public class StrategyHC : StrategyControl
    {
        public StrategyHC(string contractCode, TradeDataServer tradeDataServer) : base(tradeDataServer)
        {
            InstrumentId = contractCode;
            Init();
        }

        public string InstrumentId { get; set; }

        private Dictionary<string, double> _UpMovement = new Dictionary<string, double>();
        private Dictionary<string, double> _DownMovement = new Dictionary<string, double>();
        private Dictionary<string, int> _JiuQi = new Dictionary<string, int>();
        private Dictionary<string, bool> _ReDu = new Dictionary<string, bool>();

        private int LongPosKey;
        private int ShortPosKey;
        private bool IsSupportCloseToday;

        double LastAskPrice = 0.0;
        double LastBidPrice = 0.0;
        double LastRawAmount = 0.0;
        int LastAskSize = 0;
        int LastBidSize = 0;
        int LastRawVolume = 0;
        int LastJiuQi = 1;
        int TotalHeat = 0;
        int PresentHeat = 0;
        int LowPositionCount;
        int HighPositionCount;
        int ClosePositionCount;
        double CancelLimit;
        int HandCount;
        int flag = 0;
        int CancelSec;

        protected override void StartDepthMarketDataProcessing(ThostFtdcDepthMarketDataField depthMarketData)
        {
            if (depthMarketData.InstrumentID == InstrumentId)
            {
                if (Constant.IsNearBreakTime(60, InstrumentId, "SHFE"))
                {
                    OperationEndofTradingDay();
                }
                else if (Constant.IsInTradingTime(DateTime.Now, "SHFE", InstrumentId))
                {
                    QuoteCalculator();
                }
            }
        }

        private void Init()
        {
            LowPositionCount = int.Parse(ConfigurationManager.AppSettings["LowExposure"]);//10;
            HighPositionCount = int.Parse(ConfigurationManager.AppSettings["HighExposure"]);//20;
            ClosePositionCount = int.Parse(ConfigurationManager.AppSettings["PositionLimit"]);//50;
            CancelLimit = int.Parse(ConfigurationManager.AppSettings["CancelTick"]);//3.0;
            CancelSec = int.Parse(ConfigurationManager.AppSettings["OverTimeSec"]);//30

            ShortPosKey = Utils.GetPositionKey(InstrumentId, EnumPosiDirectionType.Short);
            LongPosKey = Utils.GetPositionKey(InstrumentId, EnumPosiDirectionType.Long);
            IsSupportCloseToday = Utils.IsShfeInstrument(InstrumentId);

            if (!_UpMovement.ContainsKey(InstrumentId))
            {
                _UpMovement.Add(InstrumentId, default(double));
            }

            if (!_DownMovement.ContainsKey(InstrumentId))
            {
                _DownMovement.Add(InstrumentId, default(double));
            }

            if (!_JiuQi.ContainsKey(InstrumentId))
            {
                _JiuQi.Add(InstrumentId, default(int));
            }

            if (!_ReDu.ContainsKey(InstrumentId))
            {
                _ReDu.Add(InstrumentId, default(bool));
            }
        }

        private void QuoteCalculator()
        {
            TradingWatch = Stopwatch.StartNew();
            int VolumeMultiple = TradeDataServer.TraderAdapter.InstrumentFields[InstrumentId].VolumeMultiple;
            double PriceTick = TradeDataServer.TraderAdapter.InstrumentFields[InstrumentId].PriceTick; ;

            double PresentAskPrice = TradeDataServer.QuoteAdapter.DepthMarketDataFields[InstrumentId].AskPrice1;
            double PresentBidPrice = TradeDataServer.QuoteAdapter.DepthMarketDataFields[InstrumentId].BidPrice1;
            int PresentAskSize = TradeDataServer.QuoteAdapter.DepthMarketDataFields[InstrumentId].AskVolume1;
            int PresentBidSize = TradeDataServer.QuoteAdapter.DepthMarketDataFields[InstrumentId].BidVolume1;
            int PresentRawVolume = TradeDataServer.QuoteAdapter.DepthMarketDataFields[InstrumentId].Volume;
            double PresentRawAmount = TradeDataServer.QuoteAdapter.DepthMarketDataFields[InstrumentId].Turnover;

            int PresentVolume = (PresentRawVolume - LastRawVolume) / 2;
            double PresentAmount = (PresentRawAmount - LastRawAmount) / (2.0 * VolumeMultiple);

            TotalHeat = GetTotalHeat(TotalHeat, PresentVolume);
            PresentHeat = GetPresentHeat(TotalHeat, PresentHeat);

            GetMovement(InstrumentId, LastAskPrice, LastBidPrice, PresentAskPrice, PresentBidPrice, PresentAmount,
                PresentVolume, LastAskSize, LastBidSize, PresentAskSize, PresentBidSize, PriceTick);
            //Utils.WriteLine(string.Format("Get Movement: {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}", code, LastAskPrice, LastBidPrice, PresentAskPrice, PresentBidPrice, PresentAmount,
            //    PresentVolume, LastAskSize, LastBidSize, PresentAskSize, PresentBidSize, PriceTick, _UpMovement[code], _DownMovement[code]));

            GetJiuQi(InstrumentId, LastAskPrice, LastBidPrice, PresentAskPrice, PresentBidPrice, LastJiuQi);

            GetReDu(InstrumentId, PresentHeat);

            int Signal = GetSignal(PresentAskSize, PresentBidSize, _UpMovement[InstrumentId], _DownMovement[InstrumentId], _ReDu[InstrumentId], _JiuQi[InstrumentId], PresentVolume);
            //_TradingWatch.Stop();
            //Utils.WriteLine(string.Format("Get Signal {0} ticks, {1} ns", _TradingWatch.ElapsedTicks, _TradingWatch.ElapsedTicks * Constant.NanosecPerTick));

            if (Signal == 0)
            {
                flag = 0;
            }

            if (Signal == 1 && !Constant.IsSentBuyOrder[InstrumentId])
            {
                //Stopwatch watch = Stopwatch.StartNew();
                if (GetCancelOrderLstShort(InstrumentId, PresentAskPrice))
                {
                    TradingWatch.Stop();
                    Utils.WriteLine(string.Format("Cancel Orders {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                    return;
                }
                int LongCount = 0;
                if (TradeDataServer.TraderAdapter.PositionFields.ContainsKey(LongPosKey))
                {
                    LongCount = TradeDataServer.TraderAdapter.PositionFields[LongPosKey].Position;
                }
                int ShortCount = 0;
                if (TradeDataServer.TraderAdapter.PositionFields.ContainsKey(ShortPosKey))
                {
                    ShortCount = TradeDataServer.TraderAdapter.PositionFields[ShortPosKey].Position;
                }
                int NetPositionCount = LongCount - ShortCount;

                if (IsSupportCloseToday)
                {
                    if (NetPositionCount <= 0)
                    {
                        HandCount = LowPositionCount - NetPositionCount;
                    }
                    else if (NetPositionCount < HighPositionCount)
                    {
                        HandCount = HighPositionCount - NetPositionCount;
                    }
                    else
                    {
                        return;
                    }
                    //watch.Stop();
                    //Utils.WriteLine(string.Format("Processing {0} ticks, {1} ns", watch.ElapsedTicks, watch.ElapsedTicks * Constant.NanosecPerTick));

                    if (LongCount < ClosePositionCount)
                    {
                        //_TradingWatch = Stopwatch.StartNew();
                        int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, PresentAskPrice, HandCount, EnumOffsetFlagType.Open);
                        if (st == 0)
                        {
                            TradingWatch.Stop();
                            Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                            flag = 1;
                        }
                    }
                    else
                    {
                        //_TradingWatch = Stopwatch.StartNew();
                        int ShortHistoryCount = TradeDataServer.TraderAdapter.PositionFields[ShortPosKey].Position - TradeDataServer.TraderAdapter.PositionFields[ShortPosKey].TodayPosition;

                        if (ShortHistoryCount > 0)
                        {
                            if (ShortHistoryCount > HandCount)
                            {
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, PresentAskPrice, HandCount, EnumOffsetFlagType.Close);
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                    flag = 1;
                                }
                            }
                            else
                            {
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, PresentAskPrice, ShortHistoryCount, EnumOffsetFlagType.Close);
                                if (st == 0)
                                {
                                    flag = 1;
                                }

                                st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, PresentAskPrice, HandCount - ShortHistoryCount, EnumOffsetFlagType.CloseToday);
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                    flag = 1;
                                }
                            }
                        }
                        else
                        {
                            int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, PresentAskPrice, HandCount, EnumOffsetFlagType.CloseToday);
                            if (st == 0)
                            {
                                TradingWatch.Stop();
                                Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                flag = 1;
                            }
                        }
                    }
                }
                else
                {
                    if (NetPositionCount <= 0)
                    {
                        HandCount = LowPositionCount - NetPositionCount;
                    }
                    else if (NetPositionCount < HighPositionCount)
                    {
                        HandCount = HighPositionCount - NetPositionCount;
                    }
                    else
                    {
                        return;
                    }

                    if (LongCount < ClosePositionCount)
                    {
                        int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, PresentAskPrice, HandCount, EnumOffsetFlagType.Open);
                        if (st == 0)
                        {
                            TradingWatch.Stop();
                            Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                            flag = 1;
                        }
                    }
                    else
                    {
                        int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, PresentAskPrice, HandCount, EnumOffsetFlagType.Close);
                        if (st == 0)
                        {
                            TradingWatch.Stop();
                            Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                            flag = 1;
                        }
                    }
                }
            }

            if (Signal == -1 && !Constant.IsSentSellOrder[InstrumentId])
            {
                //Stopwatch watch = Stopwatch.StartNew();
                if (GetCancelOrderLstLong(InstrumentId, PresentBidPrice))
                {
                    TradingWatch.Stop();
                    Utils.WriteLine(string.Format("Cancel Orders {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                    return;
                }
                int LongCount = 0;
                if (TradeDataServer.TraderAdapter.PositionFields.ContainsKey(LongPosKey))
                {
                    LongCount = TradeDataServer.TraderAdapter.PositionFields[LongPosKey].Position;
                }
                int ShortCount = 0;
                if (TradeDataServer.TraderAdapter.PositionFields.ContainsKey(ShortPosKey))
                {
                    ShortCount = TradeDataServer.TraderAdapter.PositionFields[ShortPosKey].Position;
                }
                int NetPositionCount = ShortCount - LongCount;

                if (IsSupportCloseToday)
                {
                    if (NetPositionCount <= 0)
                    {
                        HandCount = LowPositionCount - NetPositionCount;
                    }
                    else if (NetPositionCount < HighPositionCount)
                    {
                        HandCount = HighPositionCount - NetPositionCount;
                    }
                    else
                    {
                        return;
                    }
                    //watch.Stop();
                    //Utils.WriteLine(string.Format("Processing {0} ticks, {1} ns", watch.ElapsedTicks, watch.ElapsedTicks * Constant.NanosecPerTick));
                    //_TradingWatch = Stopwatch.StartNew();
                    if (ShortCount < ClosePositionCount)
                    {
                        int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, PresentBidPrice, HandCount, EnumOffsetFlagType.Open);
                        if (st == 0)
                        {
                            TradingWatch.Stop();
                            Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                            flag = -1;
                        }
                    }
                    else
                    {
                        int LongHistoryCount = TradeDataServer.TraderAdapter.PositionFields[LongPosKey].Position - TradeDataServer.TraderAdapter.PositionFields[LongPosKey].TodayPosition;

                        if (LongHistoryCount > 0)
                        {
                            if (LongHistoryCount > HandCount)
                            {
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, PresentBidPrice, HandCount, EnumOffsetFlagType.Close);
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                    flag = -1;
                                }
                            }
                            else
                            {
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, PresentBidPrice, LongHistoryCount, EnumOffsetFlagType.Close);
                                if (st == 0)
                                {
                                    flag = -1;
                                }

                                st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, PresentBidPrice, HandCount - LongHistoryCount, EnumOffsetFlagType.CloseToday);
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                    flag = -1;
                                }
                            }
                        }
                        else
                        {
                            int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, PresentBidPrice, HandCount, EnumOffsetFlagType.CloseToday);
                            if (st == 0)
                            {
                                TradingWatch.Stop();
                                Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                flag = -1;
                            }
                        }
                    }
                }
                else
                {
                    if (NetPositionCount <= 0)
                    {
                        HandCount = LowPositionCount - NetPositionCount;
                    }
                    else if (NetPositionCount < HighPositionCount)
                    {
                        HandCount = HighPositionCount - NetPositionCount;
                    }
                    else
                    {
                        return;
                    }

                    if (ShortCount < ClosePositionCount)
                    {
                        int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, PresentBidPrice, HandCount, EnumOffsetFlagType.Open);
                        if (st == 0)
                        {
                            TradingWatch.Stop();
                            Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                            flag = -1;
                        }
                    }
                    else
                    {
                        int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, PresentBidPrice, HandCount, EnumOffsetFlagType.Close);
                        if (st == 0)
                        {
                            TradingWatch.Stop();
                            Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                            flag = -1;
                        }
                    }
                }
            }

            GetCancelOrderLst(InstrumentId, PresentBidPrice, PresentAskPrice);

            LastAskPrice = PresentAskPrice;
            LastBidPrice = PresentBidPrice;
            LastRawAmount = PresentRawAmount;
            LastAskSize = PresentAskSize;
            LastBidSize = PresentBidSize;
            LastRawVolume = PresentRawVolume;
            LastJiuQi = _JiuQi[InstrumentId];
        }

        private void GetMovement(string code, double LastAskPrice, double LastBidPrice, double PresentAskPrice, double PresentBidPrice, double PresentAmount,
            int PresentVolume, int LastAskSize, int LastBidSize, int PresentAskSize, int PresentBidSize, double PriceTick)
        {
            if ((PresentAskPrice - PresentBidPrice).IsGreaterThan(PriceTick) || LastAskPrice.IsEqualTo(0))
            {
                _UpMovement[code] = 0;
                _DownMovement[code] = 0;
                return;
            }
            else
            {
                if (PresentAskPrice.IsEqualTo(LastAskPrice))
                {
                    if (PresentBidPrice.IsEqualTo(LastBidPrice))
                    {
                        if (PresentVolume == 0)
                        {
                            _UpMovement[code] = LastAskSize - PresentAskSize;
                            _DownMovement[code] = LastBidSize - PresentBidSize;
                            return;
                        }
                        else
                        {
                            double AveragePrice = PresentAmount / PresentVolume;
                            if (AveragePrice.IsGreaterThan(PresentAskPrice + PriceTick))
                            {
                                _UpMovement[code] = Math.Max(PresentVolume, LastAskSize) - PresentAskSize;
                                _DownMovement[code] = LastBidSize - PresentBidSize;
                                return;
                            }
                            else if (AveragePrice.IsGreaterThan(PresentAskPrice) && AveragePrice.IsLessThanOrEqualTo(PresentAskPrice + PriceTick))
                            {
                                _UpMovement[code] = LastAskSize - PresentAskSize + PresentVolume * (AveragePrice - PresentAskPrice) / PriceTick;
                                _DownMovement[code] = LastBidSize - PresentBidSize;
                                return;
                            }
                            else if (AveragePrice.IsLessThan(PresentBidPrice - PriceTick))
                            {
                                _UpMovement[code] = LastAskSize - PresentAskSize;
                                _DownMovement[code] = Math.Max(PresentVolume, LastBidSize) - PresentBidSize;
                                return;
                            }
                            else if (AveragePrice.IsLessThan(PresentBidPrice) && AveragePrice.IsGreaterThanOrEqualTo(PresentBidPrice - PriceTick))
                            {
                                _UpMovement[code] = LastAskSize - PresentAskSize;
                                _DownMovement[code] = LastBidSize - PresentBidSize + PresentVolume * (PresentBidPrice - AveragePrice) / PriceTick;
                                return;
                            }
                            else
                            {
                                _UpMovement[code] = LastAskSize - PresentAskSize;
                                _DownMovement[code] = LastBidSize - PresentBidSize;
                                return;
                            }
                        }
                    }
                    if (PresentBidPrice.IsGreaterThan(LastBidPrice))
                    {
                        if (PresentVolume == 0)
                        {
                            _UpMovement[code] = LastAskSize - PresentAskSize;
                            _DownMovement[code] = -PresentBidSize;
                            return;
                        }
                        else
                        {
                            double AveragePrice = PresentAmount / PresentVolume;
                            if (AveragePrice.IsGreaterThan(PresentAskPrice + PriceTick))
                            {
                                _UpMovement[code] = Math.Max(PresentVolume, LastAskSize) - PresentAskSize;
                                _DownMovement[code] = -PresentBidSize;
                                return;
                            }
                            else if (AveragePrice.IsGreaterThan(PresentAskPrice) && AveragePrice.IsLessThanOrEqualTo(PresentAskPrice + PriceTick))
                            {
                                _UpMovement[code] = LastAskSize - PresentAskSize + PresentVolume * (AveragePrice - PresentAskPrice) / PriceTick;
                                _DownMovement[code] = -PresentBidSize;
                                return;
                            }
                            else if (AveragePrice.IsLessThan(PresentBidPrice - PriceTick))
                            {
                                _UpMovement[code] = LastAskSize - PresentAskSize;
                                _DownMovement[code] = PresentVolume - PresentBidSize;
                                return;
                            }
                            else if (AveragePrice.IsLessThan(PresentBidPrice) && AveragePrice.IsGreaterThanOrEqualTo(PresentBidPrice - PriceTick))
                            {
                                _UpMovement[code] = LastAskSize - PresentAskSize;
                                _DownMovement[code] = -PresentBidSize + PresentVolume * (PresentBidPrice - AveragePrice) / PriceTick;
                                return;
                            }
                            else
                            {
                                _UpMovement[code] = LastAskSize - PresentAskSize;
                                _DownMovement[code] = -PresentBidSize;
                                return;
                            }
                        }
                    }
                }
                else if (LastBidPrice.IsEqualTo(PresentBidPrice) && PresentAskPrice.IsLessThan(LastAskPrice))
                {
                    if (PresentVolume == 0)
                    {
                        _UpMovement[code] = -PresentAskSize;
                        _DownMovement[code] = LastBidSize - PresentBidSize;
                        return;
                    }
                    else
                    {
                        double AveragePrice = PresentAmount / PresentVolume;
                        if (AveragePrice.IsGreaterThan(PresentAskPrice + PriceTick))
                        {
                            _UpMovement[code] = PresentVolume - PresentAskSize;
                            _DownMovement[code] = LastBidSize - PresentBidSize;
                            return;
                        }
                        else if (AveragePrice.IsGreaterThan(PresentAskPrice) && AveragePrice.IsLessThanOrEqualTo(PresentAskPrice + PriceTick))
                        {
                            _UpMovement[code] = -PresentAskSize + PresentVolume * (AveragePrice - PresentAskPrice) / PriceTick;
                            _DownMovement[code] = LastBidSize - PresentBidSize;
                            return;
                        }
                        else if (AveragePrice.IsLessThan(PresentBidPrice - PriceTick))
                        {
                            _UpMovement[code] = -PresentAskSize;
                            _DownMovement[code] = Math.Max(PresentVolume, LastBidSize) - PresentBidSize;
                            return;
                        }
                        else if (AveragePrice.IsLessThan(PresentBidPrice) && AveragePrice.IsGreaterThanOrEqualTo(PresentBidPrice - PriceTick))
                        {
                            _UpMovement[code] = -PresentAskSize;
                            _DownMovement[code] = LastBidSize - PresentBidSize + PresentVolume * (PresentBidPrice - AveragePrice) / PriceTick;
                            return;
                        }
                        else
                        {
                            _UpMovement[code] = -PresentAskSize;
                            _DownMovement[code] = LastBidSize - PresentBidSize;
                            return;
                        }
                    }
                }
                else if (LastAskPrice.IsEqualTo(PresentBidPrice))
                {
                    if (PresentVolume == 0)
                    {
                        _UpMovement[code] = 0;
                        _DownMovement[code] = -LastAskSize - PresentBidSize;
                        return;
                    }
                    else
                    {
                        double AveragePrice = PresentAmount / PresentVolume;
                        if (AveragePrice.IsGreaterThan(PresentAskPrice))
                        {
                            _UpMovement[code] = Math.Max(PresentVolume, LastAskSize) - LastAskSize - PresentAskSize;
                            _DownMovement[code] = -LastAskSize - PresentBidSize;
                            return;
                        }
                        else if (AveragePrice.IsLessThan(PresentBidPrice - PriceTick))
                        {
                            _UpMovement[code] = 0;
                            _DownMovement[code] = PresentVolume - LastAskSize - PresentBidSize;
                            return;
                        }
                        else if (AveragePrice.IsLessThan(PresentBidPrice) && AveragePrice.IsGreaterThanOrEqualTo(PresentBidPrice - PriceTick))
                        {
                            _UpMovement[code] = 0;
                            _DownMovement[code] = PresentVolume * (PresentBidPrice - AveragePrice) / PriceTick - LastAskSize - PresentBidSize;
                            return;
                        }
                        else
                        {
                            _UpMovement[code] = PresentVolume * (AveragePrice - PresentBidPrice) / PriceTick;
                            _DownMovement[code] = -LastAskSize - PresentBidSize;
                            return;
                        }
                    }
                }
                else if (LastAskPrice.IsLessThan(PresentBidPrice))
                {
                    if (PresentVolume == 0)
                    {
                        _UpMovement[code] = 0;
                        _DownMovement[code] = -PresentBidSize;
                        return;
                    }
                    else
                    {
                        double AveragePrice = PresentAmount / PresentVolume;
                        if (AveragePrice.IsGreaterThan(PresentAskPrice))
                        {
                            _UpMovement[code] = PresentVolume - LastAskSize - PresentAskSize;
                            _DownMovement[code] = -PresentBidSize;
                            return;
                        }
                        else if (AveragePrice.IsLessThan(PresentBidPrice - PriceTick))
                        {
                            _UpMovement[code] = 0;
                            _DownMovement[code] = PresentVolume - PresentBidSize;
                            return;
                        }
                        else if (AveragePrice.IsLessThan(PresentBidPrice) && AveragePrice.IsGreaterThanOrEqualTo(PresentBidPrice - PriceTick))
                        {
                            _UpMovement[code] = 0;
                            _DownMovement[code] = PresentVolume * (PresentBidPrice - AveragePrice) / PriceTick - PresentBidSize;
                            return;
                        }
                        else
                        {
                            _UpMovement[code] = PresentVolume * (AveragePrice - PresentBidPrice) / PriceTick;
                            _DownMovement[code] = -PresentBidSize;
                            return;
                        }
                    }
                }
                else if (LastBidPrice.IsEqualTo(PresentAskPrice))
                {
                    if (PresentVolume == 0)
                    {
                        _UpMovement[code] = -PresentAskSize - LastBidSize;
                        _DownMovement[code] = 0;
                        return;
                    }
                    else
                    {
                        double AveragePrice = PresentAmount / PresentVolume;

                        if (AveragePrice.IsLessThan(PresentBidPrice))
                        {
                            _UpMovement[code] = -PresentAskSize - LastBidSize;
                            _DownMovement[code] = -LastBidSize - PresentBidSize + Math.Max(PresentVolume, LastBidSize);
                            return;
                        }
                        else if (AveragePrice.IsGreaterThan(PresentAskPrice + PriceTick))
                        {
                            _UpMovement[code] = -PresentAskSize - LastBidSize + PresentVolume;
                            _DownMovement[code] = 0;
                            return;
                        }
                        else if (AveragePrice.IsLessThanOrEqualTo(PresentAskPrice + PriceTick) && AveragePrice.IsGreaterThan(PresentAskPrice))
                        {
                            _UpMovement[code] = -PresentAskSize - LastBidSize + PresentVolume * (AveragePrice - PresentAskPrice) / PriceTick;
                            _DownMovement[code] = 0;
                            return;
                        }
                        else
                        {
                            _UpMovement[code] = -PresentAskSize - LastBidSize;
                            _DownMovement[code] = PresentVolume * (PresentAskPrice - AveragePrice) / PriceTick;
                            return;
                        }
                    }
                }
                else if (LastBidPrice.IsGreaterThan(PresentAskPrice))
                {
                    if (PresentVolume == 0)
                    {
                        _UpMovement[code] = -PresentAskSize;
                        _DownMovement[code] = 0;
                        return;
                    }
                    else
                    {
                        double AveragePrice = PresentAmount / PresentVolume;

                        if (AveragePrice.IsLessThan(PresentBidPrice))
                        {
                            _UpMovement[code] = -PresentAskSize;
                            _DownMovement[code] = -LastBidSize - PresentBidSize + PresentVolume;
                            return;
                        }
                        else if (AveragePrice.IsGreaterThan(PresentAskPrice + PriceTick))
                        {
                            _UpMovement[code] = -PresentAskSize + PresentVolume;
                            _DownMovement[code] = 0;
                            return;
                        }
                        else if (AveragePrice.IsLessThanOrEqualTo(PresentAskPrice + PriceTick) && AveragePrice.IsGreaterThan(PresentAskPrice))
                        {
                            _UpMovement[code] = -PresentAskSize + PresentVolume * (AveragePrice - PresentAskPrice) / PriceTick;
                            _DownMovement[code] = 0;
                            return;
                        }
                        else
                        {
                            _UpMovement[code] = -PresentAskSize;
                            _DownMovement[code] = PresentVolume * (PresentAskPrice - AveragePrice) / PriceTick;
                            return;
                        }
                    }
                }
            }
        }

        private void GetJiuQi(string code, double LastAskPrice, double LastBidPrice, double PresentAskPrice, double PresentBidPrice, int LastJiuQi)
        {
            if (LastAskPrice.IsEqualTo(PresentAskPrice) && LastBidPrice.IsEqualTo(PresentBidPrice))
            {
                _JiuQi[code] = LastJiuQi + 1;
                return;
            }
            else
            {
                _JiuQi[code] = 1;
                return;
            }
        }

        private int GetTotalHeat(int TotalHeat, int PresentVolume)
        {
            if (PresentVolume > 6)
            {
                return 0;
            }
            else
            {
                if (TotalHeat == 3)
                {
                    return 3;
                }
                else
                {
                    return TotalHeat + 1;
                }
            }
        }

        private int GetPresentHeat(int TotalHeat, int PresentHeat)
        {
            if (TotalHeat == 3)
            {
                return 3;
            }
            else
            {
                if (PresentHeat > 0)
                {
                    return PresentHeat - 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        private void GetReDu(string code, int PresentHeat)
        {
            if (PresentHeat > 0)
            {
                _ReDu[code] = false;
                return;
            }
            else
            {
                _ReDu[code] = true;
                return;
            }
        }

        private int GetSignal(int PresentAskSize, int PresentBidSize, double UpMovement, double DownMovement, bool ReDu, int JiuQi, int PresentVolume)
        {
            double ratio;
            if (ReDu == false && JiuQi >= 8)
            {
                ratio = 6.0;
            }
            else
            {
                ratio = 1.5;
            }
            if (UpMovement - 0.6 * DownMovement > Math.Max(60, ratio * PresentAskSize) && PresentBidSize > 30 && DownMovement < 0.2 * Math.Min(PresentBidSize, UpMovement)
                && UpMovement > 0.2 * PresentVolume)
            {
                return 1;
            }
            else if (DownMovement - 0.6 * UpMovement > Math.Max(60, ratio * PresentBidSize) && PresentAskSize > 30 && UpMovement < 0.2 * Math.Min(DownMovement, PresentAskSize)
                && DownMovement > 0.2 * PresentVolume)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        private bool GetCancelOrderLstLong(string code, double PresentBidPrice)
        {
            bool retCancel = false;
            if (TradeDataServer.DicPendingOrder.ContainsKey(LongPosKey))
            {
                foreach (string orderKey in TradeDataServer.DicPendingOrder[LongPosKey].Keys)
                {
                    ThostFtdcOrderField item = TradeDataServer.DicPendingOrder[LongPosKey][orderKey];
                    if (item.InstrumentID == code)
                    {
                        if (item.LimitPrice.IsEqualTo(PresentBidPrice))
                        {
                            OrderManager.CancelOrder(item);
                            retCancel = true;
                        }
                    }
                }
            }
            return retCancel;
        }

        private bool GetCancelOrderLstShort(string code, double PresentAskPrice)
        {
            bool retCancel = false;
            if (TradeDataServer.DicPendingOrder.ContainsKey(ShortPosKey))
            {
                foreach (ThostFtdcOrderField item in TradeDataServer.DicPendingOrder[ShortPosKey].Values)
                {
                    if (item.InstrumentID == code)
                    {
                        if (item.LimitPrice.IsEqualTo(PresentAskPrice))
                        {
                            OrderManager.CancelOrder(item);
                            retCancel = true;
                        }
                    }
                }
            }
            return retCancel;
        }

        private bool GetCancelOrderLst(string code, double PresentBidPrice, double PresentAskPrice)
        {
            bool retCancel = false;
            if (TradeDataServer.DicPendingOrder.ContainsKey(LongPosKey))
            {
                foreach (ThostFtdcOrderField item in TradeDataServer.DicPendingOrder[LongPosKey].Values)
                {
                    if (item.InstrumentID == code)
                    {
                        if (flag == -1 || (flag == 1 && !PresentAskPrice.Equals(item.LimitPrice)))
                        {
                            OrderManager.CancelOrder(item);
                        }
                        else
                        {
                            if (Constant.IsOrderOverTime(item, CancelSec) && (PresentAskPrice - item.LimitPrice).IsGreaterThan(CancelLimit))
                            {
                                OrderManager.CancelOrder(item);
                            }
                        }
                    }

                }
            }

            if (TradeDataServer.DicPendingOrder.ContainsKey(ShortPosKey))
            {
                foreach (ThostFtdcOrderField item in TradeDataServer.DicPendingOrder[ShortPosKey].Values)
                {
                    if (item.InstrumentID == code)
                    {
                        if (flag == 1 || (flag == -1 && !PresentBidPrice.Equals(item.LimitPrice)))
                        {
                            OrderManager.CancelOrder(item);
                        }
                        else
                        {
                            if (Constant.IsOrderOverTime(item, CancelSec) && (item.LimitPrice - PresentBidPrice).IsGreaterThan(CancelLimit))
                            {
                                OrderManager.CancelOrder(item);
                            }
                        }
                    }

                }
            }

            return retCancel;
        }

        private bool GetTotalLimitOrder(string code)
        {
            bool retCancel = false;
            if (TradeDataServer.DicPendingOrder.ContainsKey(LongPosKey))
            {
                foreach (ThostFtdcOrderField item in TradeDataServer.DicPendingOrder[LongPosKey].Values)
                {
                    if (item.InstrumentID == code)
                    {
                        OrderManager.CancelOrder(item);
                        retCancel = true;
                    }
                }
            }
            if (TradeDataServer.DicPendingOrder.ContainsKey(ShortPosKey))
            {
                foreach (ThostFtdcOrderField item in TradeDataServer.DicPendingOrder[ShortPosKey].Values)
                {
                    if (item.InstrumentID == code)
                    {
                        OrderManager.CancelOrder(item);
                        retCancel = true;
                    }
                }
            }

            return retCancel;
        }

        private void OperationEndofTradingDay()
        {
            TradingWatch = Stopwatch.StartNew();
            int LongCount = 0;
            if (TradeDataServer.TraderAdapter.PositionFields.ContainsKey(LongPosKey))
            {
                LongCount = TradeDataServer.TraderAdapter.PositionFields[LongPosKey].Position;
            }
            int ShortCount = 0;
            if (TradeDataServer.TraderAdapter.PositionFields.ContainsKey(ShortPosKey))
            {
                ShortCount = TradeDataServer.TraderAdapter.PositionFields[ShortPosKey].Position;
            }
            int NetPositionCount = LongCount - ShortCount;

            if (Constant.IsNearBreakTime(10, InstrumentId, "SHFE"))
            {
                if (GetTotalLimitOrder(InstrumentId))
                {
                    TradingWatch.Stop();
                    Utils.WriteLine(string.Format("Cancel Orders {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                    return;
                }
                else
                {
                    if (NetPositionCount == 0)
                    {
                        return;
                    }
                    else if (NetPositionCount > 0)
                    {
                        HandCount = NetPositionCount;

                        if (IsSupportCloseToday)
                        {

                            if (ShortCount < ClosePositionCount)
                            {
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, 0.0, HandCount, EnumOffsetFlagType.Open, EnumOrderType.Market);
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                }
                            }
                            else
                            {
                                int LongHistoryCount = TradeDataServer.TraderAdapter.PositionFields[LongPosKey].Position - TradeDataServer.TraderAdapter.PositionFields[LongPosKey].TodayPosition;

                                if (LongHistoryCount > 0)
                                {
                                    if (LongHistoryCount > HandCount)
                                    {
                                        int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, 0.0, HandCount, EnumOffsetFlagType.Close, EnumOrderType.Market);
                                        if (st == 0)
                                        {
                                            TradingWatch.Stop();
                                            Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                        }
                                    }
                                    else
                                    {
                                        int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, 0.0, LongHistoryCount, EnumOffsetFlagType.Close, EnumOrderType.Market);
                                        if (st == 0)
                                        {
                                        }

                                        st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, 0.0, HandCount - LongHistoryCount, EnumOffsetFlagType.CloseToday, EnumOrderType.Market);
                                        if (st == 0)
                                        {
                                            TradingWatch.Stop();
                                            Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                        }
                                    }
                                }
                                else
                                {
                                    int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, 0.0, HandCount, EnumOffsetFlagType.CloseToday, EnumOrderType.Market);
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
                            if (ShortCount < ClosePositionCount)
                            {
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, 0.0, HandCount, EnumOffsetFlagType.Open, EnumOrderType.Market);
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                }
                            }
                            else
                            {
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, 0.0, HandCount, EnumOffsetFlagType.Close, EnumOrderType.Market);
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
                        HandCount = -NetPositionCount;

                        if (IsSupportCloseToday)
                        {
                            if (LongCount < ClosePositionCount)
                            {
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, 0.0, HandCount, EnumOffsetFlagType.Open, EnumOrderType.Market);
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                }
                            }
                            else
                            {
                                int ShortHistoryCount = TradeDataServer.TraderAdapter.PositionFields[ShortPosKey].Position - TradeDataServer.TraderAdapter.PositionFields[ShortPosKey].TodayPosition;

                                if (ShortHistoryCount > 0)
                                {
                                    if (ShortHistoryCount > HandCount)
                                    {
                                        int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, 0.0, HandCount, EnumOffsetFlagType.Close, EnumOrderType.Market);
                                        if (st == 0)
                                        {
                                            TradingWatch.Stop();
                                            Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                        }
                                    }
                                    else
                                    {
                                        int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, 0.0, ShortHistoryCount, EnumOffsetFlagType.Close, EnumOrderType.Market);
                                        if (st == 0)
                                        {
                                        }

                                        st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, 0.0, HandCount - ShortHistoryCount, EnumOffsetFlagType.CloseToday, EnumOrderType.Market);
                                        if (st == 0)
                                        {
                                            TradingWatch.Stop();
                                            Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                        }
                                    }
                                }
                                else
                                {
                                    int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, 0.0, HandCount, EnumOffsetFlagType.CloseToday, EnumOrderType.Market);
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
                            if (LongCount < ClosePositionCount)
                            {
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, 0.0, HandCount, EnumOffsetFlagType.Open, EnumOrderType.Market);
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                }
                            }
                            else
                            {
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, 0.0, HandCount, EnumOffsetFlagType.Close, EnumOrderType.Market);
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (NetPositionCount == 0)
                {
                    if (GetTotalLimitOrder(InstrumentId))
                    {
                        TradingWatch.Stop();
                        Utils.WriteLine(string.Format("Cancel Orders {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                    }
                    return;
                }

                //_TradingWatch = Stopwatch.StartNew();
                int VolumeMultiple = TradeDataServer.TraderAdapter.InstrumentFields[InstrumentId].VolumeMultiple;
                double PriceTick = TradeDataServer.TraderAdapter.InstrumentFields[InstrumentId].PriceTick; ;

                double PresentAskPrice = TradeDataServer.QuoteAdapter.DepthMarketDataFields[InstrumentId].AskPrice1;
                double PresentBidPrice = TradeDataServer.QuoteAdapter.DepthMarketDataFields[InstrumentId].BidPrice1;
                int PresentAskSize = TradeDataServer.QuoteAdapter.DepthMarketDataFields[InstrumentId].AskVolume1;
                int PresentBidSize = TradeDataServer.QuoteAdapter.DepthMarketDataFields[InstrumentId].BidVolume1;
                int PresentRawVolume = TradeDataServer.QuoteAdapter.DepthMarketDataFields[InstrumentId].Volume;
                double PresentRawAmount = TradeDataServer.QuoteAdapter.DepthMarketDataFields[InstrumentId].Turnover;

                int PresentVolume = (PresentRawVolume - LastRawVolume) / 2;
                double PresentAmount = (PresentRawAmount - LastRawAmount) / (2.0 * VolumeMultiple);

                TotalHeat = GetTotalHeat(TotalHeat, PresentVolume);
                PresentHeat = GetPresentHeat(TotalHeat, PresentHeat);

                GetMovement(InstrumentId, LastAskPrice, LastBidPrice, PresentAskPrice, PresentBidPrice, PresentAmount,
                    PresentVolume, LastAskSize, LastBidSize, PresentAskSize, PresentBidSize, PriceTick);
                //Utils.WriteLine(string.Format("Get Movement: {0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}", code, LastAskPrice, LastBidPrice, PresentAskPrice, PresentBidPrice, PresentAmount,
                //    PresentVolume, LastAskSize, LastBidSize, PresentAskSize, PresentBidSize, PriceTick, _UpMovement[code], _DownMovement[code]));

                GetJiuQi(InstrumentId, LastAskPrice, LastBidPrice, PresentAskPrice, PresentBidPrice, LastJiuQi);

                GetReDu(InstrumentId, PresentHeat);

                int Signal = GetSignal(PresentAskSize, PresentBidSize, _UpMovement[InstrumentId], _DownMovement[InstrumentId], _ReDu[InstrumentId], _JiuQi[InstrumentId], PresentVolume);

                if (Signal == 0)
                {
                    flag = 0;
                }

                if (Signal == 1 && !Constant.IsSentBuyOrder[InstrumentId])
                {
                    if (GetCancelOrderLstShort(InstrumentId, PresentAskPrice))
                    {
                        return;
                    }

                    if (IsSupportCloseToday)
                    {
                        if (NetPositionCount < 0)
                        {
                            HandCount = -NetPositionCount;
                        }
                        else
                        {
                            return;
                        }

                        if (LongCount < ClosePositionCount)
                        {
                            int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, PresentAskPrice, HandCount, EnumOffsetFlagType.Open);
                            if (st == 0)
                            {
                                TradingWatch.Stop();
                                Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                flag = 1;
                            }
                        }
                        else
                        {
                            int ShortHistoryCount = TradeDataServer.TraderAdapter.PositionFields[ShortPosKey].Position - TradeDataServer.TraderAdapter.PositionFields[ShortPosKey].TodayPosition;

                            if (ShortHistoryCount > 0)
                            {
                                if (ShortHistoryCount > HandCount)
                                {
                                    int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, PresentAskPrice, HandCount, EnumOffsetFlagType.Close);
                                    if (st == 0)
                                    {
                                        TradingWatch.Stop();
                                        Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                        flag = 1;
                                    }
                                }
                                else
                                {
                                    int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, PresentAskPrice, ShortHistoryCount, EnumOffsetFlagType.Close);
                                    if (st == 0)
                                    {
                                        flag = 1;
                                    }

                                    st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, PresentAskPrice, HandCount - ShortHistoryCount, EnumOffsetFlagType.CloseToday);
                                    if (st == 0)
                                    {
                                        TradingWatch.Stop();
                                        Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                        flag = 1;
                                    }
                                }
                            }
                            else
                            {
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, PresentAskPrice, HandCount, EnumOffsetFlagType.CloseToday);
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                    flag = 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (NetPositionCount <= 0)
                        {
                            HandCount = -NetPositionCount;
                        }
                        else
                        {
                            return;
                        }

                        if (LongCount < ClosePositionCount)
                        {
                            int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, PresentAskPrice, HandCount, EnumOffsetFlagType.Open);
                            if (st == 0)
                            {
                                TradingWatch.Stop();
                                Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                flag = 1;
                            }
                        }
                        else
                        {
                            int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Buy, PresentAskPrice, HandCount, EnumOffsetFlagType.Close);
                            if (st == 0)
                            {
                                TradingWatch.Stop();
                                Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                flag = 1;
                            }
                        }
                    }
                }

                if (Signal == -1 && !Constant.IsSentSellOrder[InstrumentId])
                {
                    if (GetCancelOrderLstLong(InstrumentId, PresentBidPrice))
                    {
                        return;
                    }

                    if (IsSupportCloseToday)
                    {
                        if (NetPositionCount > 0)
                        {
                            HandCount = NetPositionCount;
                        }
                        else
                        {
                            return;
                        }

                        if (ShortCount < ClosePositionCount)
                        {
                            int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, PresentBidPrice, HandCount, EnumOffsetFlagType.Open);
                            if (st == 0)
                            {
                                flag = -1;
                            }
                        }
                        else
                        {
                            int LongHistoryCount = TradeDataServer.TraderAdapter.PositionFields[LongPosKey].Position - TradeDataServer.TraderAdapter.PositionFields[LongPosKey].TodayPosition;

                            if (LongHistoryCount > 0)
                            {
                                if (LongHistoryCount > HandCount)
                                {
                                    int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, PresentBidPrice, HandCount, EnumOffsetFlagType.Close);
                                    if (st == 0)
                                    {
                                        TradingWatch.Stop();
                                        Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                        flag = -1;
                                    }
                                }
                                else
                                {
                                    int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, PresentBidPrice, LongHistoryCount, EnumOffsetFlagType.Close);
                                    if (st == 0)
                                    {
                                        flag = -1;
                                    }

                                    st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, PresentBidPrice, HandCount - LongHistoryCount, EnumOffsetFlagType.CloseToday);
                                    if (st == 0)
                                    {
                                        TradingWatch.Stop();
                                        Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                        flag = -1;
                                    }
                                }
                            }
                            else
                            {
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, PresentBidPrice, HandCount, EnumOffsetFlagType.CloseToday);
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                    flag = -1;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (NetPositionCount <= 0)
                        {
                            HandCount = LowPositionCount - NetPositionCount;
                        }
                        else if (NetPositionCount < HighPositionCount)
                        {
                            HandCount = HighPositionCount - NetPositionCount;
                        }
                        else
                        {
                            return;
                        }

                        if (ShortCount < ClosePositionCount)
                        {
                            int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, PresentBidPrice, HandCount, EnumOffsetFlagType.Open);
                            if (st == 0)
                            {
                                TradingWatch.Stop();
                                Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                flag = -1;
                            }
                        }
                        else
                        {
                            int st = OrderManager.SubmitOrder(InstrumentId, EnumDirectionType.Sell, PresentBidPrice, HandCount, EnumOffsetFlagType.Close);
                            if (st == 0)
                            {
                                TradingWatch.Stop();
                                Utils.WriteLine(string.Format("Submit Order {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
                                flag = -1;
                            }
                        }
                    }
                }

                GetCancelOrderLst(InstrumentId, PresentBidPrice, PresentAskPrice);

                LastAskPrice = PresentAskPrice;
                LastBidPrice = PresentBidPrice;
                LastRawAmount = PresentRawAmount;
                LastAskSize = PresentAskSize;
                LastBidSize = PresentBidSize;
                LastRawVolume = PresentRawVolume;
                LastJiuQi = _JiuQi[InstrumentId];
            }
        }
    }
}
