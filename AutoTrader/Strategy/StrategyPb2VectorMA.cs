using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader
{
    public class StrategyPb2VectorMA : StrategyVectorMA
    {
        public StrategyPb2VectorMA(string instantInstrumentID, TradeDataServer tradeDataServer) : base(instantInstrumentID, tradeDataServer)
        {
            TimeWindow = int.Parse(Util.StrategyParameter[instantInstrumentID]["Pb2Strategy"]["TimeWindow"]);
            TargetPosCount = int.Parse(Util.StrategyParameter[instantInstrumentID]["Pb2Strategy"]["TargetPosCount"]);
            MaxDuration = int.Parse(Util.StrategyParameter[instantInstrumentID]["Pb2Strategy"]["MaxDuration"]);
            JumpLevel = int.Parse(Util.StrategyParameter[instantInstrumentID]["Pb2Strategy"]["JumpLevel"]);

            _LongCredit = int.Parse(Util.StrategyParameter[instantInstrumentID]["Pb2Strategy"]["LongCredit"]);
            _ShortCredit = int.Parse(Util.StrategyParameter[instantInstrumentID]["Pb2Strategy"]["ShortCredit"]);
            _MiniGap = double.Parse(Util.StrategyParameter[instantInstrumentID]["Pb2Strategy"]["MiniGap"]);
        }

        private double MaxLost = 0.0;
        private double _InitPosLost = 0.0;
        private double _InitLongLost = 0.0;
        private double _InitShortLost = 0.0;
        private double _OppoBidPrice, _OppoAskPrice = 0.0;
        private int _LongCredit;
        private int _ShortCredit;
        private double _MiniGap;
        private double _MiniSignalOpen, _MiniSignalClose, _MiniSignalCutLost;

        //用于计算一个向上wave中的最高价和向下wave中的最低价
        private double _MaxPrice = 0;
        private double _MinPrice = double.MaxValue;

        //单次浮动盈亏信息
        private double _LongLost = 0;
        private double _ShortLost = 0;

        //实现非对称开平仓
        private int _StateOpen = 0;
        private int _StateClose = 0;

        //记录高低点
        private double _HighPrice = double.MaxValue;
        private double _LowPrice = 0;

        //记录下当前趋势特征
        private int _TrendFlag = 0;

        private void ExecutePb2Strategy()
        {
            if (MaxLost.IsEqualTo(0.0))
            {
                MaxLost = TargetPosCount * PriceTick * double.Parse(Util.StrategyParameter[InstrumentId]["Pb2Strategy"]["MaxLost"]);
            }
            if (_MiniSignalOpen.IsEqualTo(0.0))
            {
                _MiniSignalOpen = PriceTick * double.Parse(Util.StrategyParameter[InstrumentId]["Pb2Strategy"]["MiniSignalOpen"]);
            }
            if (_MiniSignalClose.IsEqualTo(0.0))
            {
                _MiniSignalClose = PriceTick * double.Parse(Util.StrategyParameter[InstrumentId]["Pb2Strategy"]["MiniSignalClose"]);
            }
            if (_MiniSignalCutLost.IsEqualTo(0.0))
            {
                _MiniSignalCutLost = PriceTick * double.Parse(Util.StrategyParameter[InstrumentId]["Pb2Strategy"]["MiniSignalCutLost"]);
            }
            int longCount = TradeDataServer.PositionFields.ContainsKey(LongPosKey) ? TradeDataServer.PositionFields[LongPosKey].Position : 0;
            int shortCount = TradeDataServer.PositionFields.ContainsKey(ShortPosKey) ? TradeDataServer.PositionFields[ShortPosKey].Position : 0;
            int netPos = longCount - shortCount;
            _OppoAskPrice = Math.Min(PresentAskPrice + PriceTick * JumpLevel, UpLimitPrice);
            _OppoBidPrice = Math.Max(PresentBidPrice - PriceTick * JumpLevel, DownLimitPrice);
            if (Util.IsInReLoadingStatus(InstrumentId))
            {
                int posKey = netPos > 0 ? LongPosKey : ShortPosKey;
                if (TradeDataServer.PositionFields.ContainsKey(posKey))
                {
                    double posCost = TradeDataServer.PositionFields[posKey].PositionCost / (VolumeMultiple * Math.Abs(netPos));
                    //Constant.WriteInfo(string.Format("Position {0} , volume {1} , cost {2}", InstrumentId, netPos, posCost));
                    _InitPosLost = netPos * (LastPrice - posCost);
                    if (posKey == LongPosKey)
                    {
                        _InitLongLost = longCount * (LastPrice - posCost);
                    }
                    else if (posKey == ShortPosKey)
                    {
                        _InitShortLost = shortCount * (posCost - LastPrice);
                    }
                }
            }
            if (netPos > 0)
            {
                PositonLost += netPos * (LastPrice - LastOldPrice);
            }
            else
            {
                PositonLost = 0;
            }
            //多空头持仓的浮动盈亏
            if (longCount > 0)
            {
                _LongLost += longCount * (LastPrice - LastOldPrice);
            }
            else
            {
                _LongLost = 0;
            }
            if (shortCount > 0)
            {
                _ShortLost -= shortCount * (LastPrice - LastOldPrice);
            }
            else
            {
                _ShortLost = 0;
            }
            if (!_InitPosLost.IsEqualTo(0.0) && !Util.IsInReLoadingStatus(InstrumentId))
            {
                PositonLost = _InitPosLost;
                _InitPosLost = 0;
            }
            if (!_InitLongLost.IsEqualTo(0.0) && !Util.IsInReLoadingStatus(InstrumentId))
            {
                _LongLost = _InitLongLost;
                _InitLongLost = 0;
            }
            if (!_InitShortLost.IsEqualTo(0.0) && !Util.IsInReLoadingStatus(InstrumentId))
            {
                _ShortLost = _InitShortLost;
                _InitShortLost = 0;
            }
            double signal = AveragePriceByVolume - AveragePriceByDuration;
            if (!Util.IsInReLoadingStatus(InstrumentId))
            {
#if DEBUG
                Util.WriteInfo(string.Format("Pb2: InstrumentId {0}, Signal {1}, PriceMean {2}, HighPrice {3}, LowPrice {4}, MaxPrice {5}, MinPrice {6}, PositonLost {7}, longCount {8}, shortCount {9}, LongLost {10}, ShortLost {11}, StateOpen {12}, StateClose {13}, TrendFlag {14}, AveragePriceByVolume {15}, AveragePriceByDuration {16}",
                    InstrumentId, signal, PriceMean, _HighPrice, _LowPrice, _MaxPrice, _MinPrice, PositonLost, longCount, shortCount, _LongLost, _ShortLost, _StateOpen, _StateClose, _TrendFlag, AveragePriceByVolume, AveragePriceByDuration));
#endif
            }

            if (TradingTimeManager.IsInEndingTime(InstrumentId, ExchangeId, EndingSeconds)) // close all，添加保护代码
            {
                //强行平仓
                if (longCount > 0 && !Util.IsSentSellOrder[InstrumentId] && Util.IsFilledSellOrder[InstrumentId])
                {
                    int hands = Math.Abs(longCount);
                    int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Sell, _OppoBidPrice, hands, true);// Only Close
                    if (st == 0)
                    {
                        TradingWatch.Stop();
                        string lastOrderRef = OrderManager.CombineOrderRef;
                        if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                        {
                            Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                        }
                        if (!Util.IsInReLoadingStatus(InstrumentId))
                        {
                            Util.WriteInfo(string.Format("Submit Pb2 Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, EnumThostOffsetFlagType.Close, hands, PresentBidPrice));
                        }
                    }
                }
                if (shortCount > 0 && !Util.IsSentBuyOrder[InstrumentId] && Util.IsFilledBuyOrder[InstrumentId])
                {
                    int hands = Math.Abs(shortCount);
                    int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Buy, _OppoAskPrice, hands, true);// Only Close
                    if (st == 0)
                    {
                        TradingWatch.Stop();
                        string lastOrderRef = OrderManager.CombineOrderRef;
                        if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                        {
                            Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                        }
                        if (!Util.IsInReLoadingStatus(InstrumentId))
                        {
                            Util.WriteInfo(string.Format("Submit Pb2 Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, EnumThostOffsetFlagType.Close, hands, PresentAskPrice));
                        }
                    }
                }
            }
            else if (DurationLst.Count >= TimeWindow)
            {
                if (TradingTimeManager.IsInEndingTime(InstrumentId, ExchangeId, PendingEndSeconds)) // close all，添加保护代码
                {
                    _LongCredit = 0;
                    _ShortCredit = 0;
                }

                //强制止损
                if (longCount > 0 && (_LongLost.IsLessThan(-MaxLost) || LastPrice.IsLessThanOrEqualTo(DownWarningPrice) || LastPrice.IsGreaterThanOrEqualTo(TradeDataServer.DepthMarketDataFields[InstrumentId].UpperLimitPrice)
                    && !Util.IsSentSellOrder[InstrumentId] && Util.IsFilledSellOrder[InstrumentId]))
                {//平多
                    int hands = longCount;
                    int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Sell, _OppoBidPrice, hands, true);// Only Close
                    if (st == 0)
                    {
                        TradingWatch.Stop();
                        string lastOrderRef = OrderManager.CombineOrderRef;
                        if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                        {
                            Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                        }
                        if (!Util.IsInReLoadingStatus(InstrumentId))
                        {
                            Util.WriteInfo(string.Format("Submit Pb2 Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, EnumThostOffsetFlagType.Close, hands, PresentBidPrice));
                        }
                        PositonLost = _LongLost = 0;
                    }
                }
                if (shortCount > 0 && (_ShortLost.IsLessThan(-MaxLost) || LastPrice.IsGreaterThanOrEqualTo(UpWarningPrice) || LastPrice.IsLessThanOrEqualTo(TradeDataServer.DepthMarketDataFields[InstrumentId].LowerLimitPrice))
                    && !Util.IsSentBuyOrder[InstrumentId] && Util.IsFilledBuyOrder[InstrumentId])
                {//平空
                    int hands = shortCount;
                    int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Buy, _OppoAskPrice, hands, true);// Only Close
                    if (st == 0)
                    {
                        TradingWatch.Stop();
                        string lastOrderRef = OrderManager.CombineOrderRef;
                        if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                        {
                            Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                        }
                        if (!Util.IsInReLoadingStatus(InstrumentId))
                        {
                            Util.WriteInfo(string.Format("Submit Pb2 Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, EnumThostOffsetFlagType.Close, hands, PresentAskPrice));
                        }
                        PositonLost = _ShortLost = 0;
                    }
                }

                //高点被突破
                if (PriceMean.IsGreaterThan(_HighPrice + _MiniGap * PriceTick))//jiaGeZhongShuList[i]
                {
                    //原来的高点不再认为是高点
                    _HighPrice = double.MaxValue;
                    // 减少空头额度，因为顶部判断出错了
                    _ShortCredit -= 1;
                    //趋势偏多头
                    _TrendFlag = 1;
                    Util.WriteInfo(string.Format("{0}: Current Pb2 ShortCredit: {1}", InstrumentId, _ShortCredit));
                }
                //低点被突破
                if (PriceMean.IsLessThan(_LowPrice - _MiniGap * PriceTick))
                {
                    //原来低点不认为还是低点
                    _LowPrice = 0;
                    //减少多头额度，因为底部判断出错了
                    _LongCredit -= 1;
                    // 趋势偏空头
                    _TrendFlag = -1;
                    Util.WriteInfo(string.Format("{0}: Current Pb2 LongCredit: {1}", InstrumentId, _LongCredit));
                }

                //多头趋势下，又出现了多头信号，而持仓是空头
                if (shortCount > 0 && _TrendFlag == 1 && signal.IsGreaterThan(_MiniSignalCutLost))
                {
                    //平空止损
                    int hands = shortCount;
                    int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Buy, _OppoAskPrice, hands, true);// Only Close
                    if (st == 0)
                    {
                        TradingWatch.Stop();
                        string lastOrderRef = OrderManager.CombineOrderRef;
                        if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                        {
                            Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                        }
                        if (!Util.IsInReLoadingStatus(InstrumentId))
                        {
                            Util.WriteInfo(string.Format("Submit Pb2 Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, EnumThostOffsetFlagType.Close, hands, PresentAskPrice));
                        }
                        //_ShortLost = 0;?
                        PositonLost = _ShortLost = 0;
                    }
                }
                // 空头趋势下又出了空头信号，但持仓是多头
                if (longCount > 0 && _TrendFlag == -1 && signal.IsLessThan(-_MiniSignalCutLost))
                {
                    //平多止损
                    int hands = longCount;
                    int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Sell, _OppoBidPrice, hands, true);// Only Close
                    if (st == 0)
                    {
                        TradingWatch.Stop();
                        string lastOrderRef = OrderManager.CombineOrderRef;
                        if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                        {
                            Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                        }
                        if (!Util.IsInReLoadingStatus(InstrumentId))
                        {
                            Util.WriteInfo(string.Format("Submit Pb2 Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, EnumThostOffsetFlagType.Close, hands, PresentBidPrice));
                        }
                        //_LongLost = 0;?
                        PositonLost = _LongLost = 0;
                    }
                }

                //维护PB策略主信号
                if (_StateOpen == 0)
                {
                    if (signal.IsGreaterThan(_MiniSignalOpen))
                    {
                        _StateOpen = 1;
                        //记录下最高价
                        _MaxPrice = PriceMean; //jiaGeZhongShuList[i];
                    }
                    if (signal.IsLessThan(-_MiniSignalOpen))
                    {
                        _StateOpen = -1;
                        //记录下最低价
                        _MinPrice = PriceMean; //jiaGeZhongShuList[i];
                    }
                }

                if (_StateOpen == 1)
                {
                    //动态记录下最高价
                    if (PriceMean.IsGreaterThan(_MaxPrice))
                    {
                        _MaxPrice = PriceMean;// jiaGeZhongShuList[i];
                    }
                    // 一个wave结束
                    if (signal.IsLessThan(0))
                    {
                        _StateOpen = 0;
                        //最高价作为高点
                        _HighPrice = _MaxPrice;
                        //默认趋势结束
                        _TrendFlag = 0;
                        //如果空头还有额度
                        if (shortCount == 0 && _ShortCredit > 0)
                        {
                            //做一把PB
                            if (LastPrice.IsGreaterThan(DownWarningPrice) && LastPrice.IsLessThan(UpWarningPrice) && !Util.IsSentBuyOrder[InstrumentId] && Util.IsFilledBuyOrder[InstrumentId])
                            {
                                //chiCang = -1;
                                int hands = Math.Abs(-TargetPosCount - netPos);// 目标仓位 -1
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Sell, _OppoBidPrice, hands, false);
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    string lastOrderRef = OrderManager.CombineOrderRef;
                                    if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                                    {
                                        Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                                    }
                                    if (!Util.IsInReLoadingStatus(InstrumentId))
                                    {
                                        Util.WriteInfo(string.Format("Submit Pb2 Order {0} ticks, {1} ns, {2}, {3}, {4}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, hands, PresentBidPrice));
                                    }
                                }
                            }
                        }
                    }
                }
                //反方向类似处理
                if (_StateOpen == -1)
                {
                    if (PriceMean.IsLessThan(_MinPrice))
                    {
                        _MinPrice = PriceMean;// jiaGeZhongShuList[i];
                    }
                    if (signal.IsGreaterThan(0))
                    {
                        _StateOpen = 0;
                        _LowPrice = _MinPrice;
                        _TrendFlag = 0;
                        if (longCount == 0 && _LongCredit > 0)
                        {
                            if (LastPrice.IsGreaterThan(DownWarningPrice) && LastPrice.IsLessThan(UpWarningPrice) && !Util.IsSentBuyOrder[InstrumentId] && Util.IsFilledBuyOrder[InstrumentId])
                            {
                                //duoTouChiCang = 1;
                                int hands = Math.Abs(TargetPosCount - netPos);
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Buy, _OppoAskPrice, hands, false);
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    string lastOrderRef = OrderManager.CombineOrderRef;
                                    if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                                    {
                                        Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                                    }
                                    if (!Util.IsInReLoadingStatus(InstrumentId))
                                    {
                                        Util.WriteInfo(string.Format("Submit Pb2 Order {0} ticks, {1} ns, {2}, {3}, {4}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, hands, PresentAskPrice));
                                    }
                                }
                            }
                        }
                    }
                }

                //非对称开平仓，此为平仓信号，与PB信号逻辑相同，阀值可以不同
                if (_StateClose == 0)
                {
                    if (signal.IsGreaterThan(_MiniSignalClose))
                    {
                        _StateClose = 1;
                    }
                    if (signal.IsLessThan(-_MiniSignalClose))
                    {
                        _StateClose = -1;
                    }
                }

                if (_StateClose == 1)
                {
                    if (signal < 0)
                    {
                        _StateClose = 0;
                        if (longCount > 0)
                        {
                            //duoTouChiCang = 0;
                            int hands = longCount;
                            int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Sell, _OppoBidPrice, hands, true);// Only Close
                            if (st == 0)
                            {
                                TradingWatch.Stop();
                                string lastOrderRef = OrderManager.CombineOrderRef;
                                if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                                {
                                    Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                                }
                                if (!Util.IsInReLoadingStatus(InstrumentId))
                                {
                                    Util.WriteInfo(string.Format("Submit Pb2 Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, EnumThostOffsetFlagType.Close, hands, PresentBidPrice));
                                }
                                //_LongLost = 0;?
                                PositonLost = _LongLost = 0;
                            }
                        }
                    }
                }
                if (_StateClose == -1)
                {
                    if (signal > 0)
                    {
                        _StateClose = 0;
                        if (shortCount > 0)
                        {
                            //kongTouChiCang = 0;
                            int hands = shortCount;
                            int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Buy, _OppoAskPrice, hands, true);// Only Close
                            if (st == 0)
                            {
                                TradingWatch.Stop();
                                string lastOrderRef = OrderManager.CombineOrderRef;
                                if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                                {
                                    Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                                }
                                if (!Util.IsInReLoadingStatus(InstrumentId))
                                {
                                    Util.WriteInfo(string.Format("Submit Pb2 Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, EnumThostOffsetFlagType.Close, hands, PresentAskPrice));
                                }
                                //_ShortLost = 0;?
                                PositonLost = _ShortLost = 0;
                            }
                        }
                    }
                }
            }
            else if (!Util.IsInReLoadingStatus(InstrumentId))
            {
                //Constant.WriteInfo(string.Format("{0} Pb: DurationLst items {1}", InstrumentId, DurationLst.Count));
            }
            LastSignal = signal;
        }

        protected sealed override void ExecuteStrategy()
        {
            ExecutePb2Strategy();
        }

    }
}
