using System;
using System.Configuration;

namespace AutoTrader
{
    public class StrategyPbVectorMA : StrategyVectorMA
    {
        public StrategyPbVectorMA(string instantInstrumentID, TradeDataServer tradeDataServer) : base(instantInstrumentID, tradeDataServer)
        {
            TimeWindow = int.Parse(Util.StrategyParameter[instantInstrumentID]["PbStrategy"]["TimeWindow"]);
            TargetPosCount = int.Parse(Util.StrategyParameter[instantInstrumentID]["PbStrategy"]["TargetPosCount"]);
            MiniWindow = double.Parse(Util.StrategyParameter[instantInstrumentID]["PbStrategy"]["MiniSignal"]);
            MaxDuration = int.Parse(Util.StrategyParameter[instantInstrumentID]["PbStrategy"]["MaxDuration"]);
            JumpLevel = int.Parse(Util.StrategyParameter[instantInstrumentID]["PbStrategy"]["JumpLevel"]);
            SimulateNetPosition = 0;
        }

        private int SimulateNetPosition;
        private double MaxLost = 0.0;
        private double _InitPosLost = 0.0;
        private int _State = 0;
        private double _OppoBidPrice, _OppoAskPrice = 0.0;
        private bool _StartChecked = false;

        private void ExecutePbStrategy()
        {
            if (MaxLost.IsEqualTo(0.0))
            {
                MaxLost = TargetPosCount * PriceTick * double.Parse(Util.StrategyParameter[InstrumentId]["PbStrategy"]["MaxLost"]);
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
            if (!_InitPosLost.IsEqualTo(0.0) && !Util.IsInReLoadingStatus(InstrumentId))
            {
                PositonLost = _InitPosLost;
                _InitPosLost = 0;
            }
            double signal = AveragePriceByVolume - AveragePriceByDuration;
            if (!Util.IsInReLoadingStatus(InstrumentId))
            {
                if (!_StartChecked)
                {
                    _StartChecked = CheckPositionDirection(netPos);
                }
#if DEBUG
                Util.WriteInfo(string.Format("Pb: InstrumentId {0}, Signal {1}, LastSignal {2}, PriceMean {3}, LastPriceMean {4}, SimulateNetPosition {5}, PositonLost {6}, longCount {7}, shortCount {8}, State {9}, AveragePriceByVolume {10}, AveragePriceByDuration {11}",
                    InstrumentId, signal, LastSignal, PriceMean, LastPriceMean, SimulateNetPosition, PositonLost, longCount, shortCount, _State, AveragePriceByVolume, AveragePriceByDuration));
#endif
            }
            if (TradingTimeManager.IsInEndingTime(InstrumentId, ExchangeId, PendingEndSeconds)) // close all，添加保护代码
            {
                if (PositonLost.IsLessThan(-MaxLost) || LastPrice.IsLessThanOrEqualTo(DownWarningPrice) || LastPrice.IsGreaterThanOrEqualTo(TradeDataServer.DepthMarketDataFields[InstrumentId].UpperLimitPrice)
                    || TradingTimeManager.IsInEndingTime(InstrumentId, ExchangeId, EndingSeconds))
                {
                    if (netPos > 0 && !Util.IsSentSellOrder[InstrumentId] && Util.IsFilledSellOrder[InstrumentId])
                    {//平多
                        int hands = Math.Abs(netPos);
                        int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Sell, _OppoBidPrice, hands, true);// Only Close
                        if (st == 0)
                        {
                            TradingWatch.Stop();
                            string lastOrderRef = OrderManager.CombineOrderRef.ToString();
                            if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                            {
                                Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                            }
                            if (!Util.IsInReLoadingStatus(InstrumentId))
                            {
                                //Constant.WriteInfo(string.Format("PositonLost {0}, longCount {1}, shortCount {2}", PositonLost, longCount, shortCount));
                                Util.WriteInfo(string.Format("Submit Pb Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, EnumThostOffsetFlagType.Close, hands, PresentBidPrice));
                            }
                        }
                    }
                }
                if (PositonLost.IsLessThan(-MaxLost) || LastPrice.IsGreaterThanOrEqualTo(UpWarningPrice) || LastPrice.IsLessThanOrEqualTo(TradeDataServer.DepthMarketDataFields[InstrumentId].LowerLimitPrice)
                    || TradingTimeManager.IsInEndingTime(InstrumentId, ExchangeId, EndingSeconds))
                {
                    if (netPos < 0 && !Util.IsSentBuyOrder[InstrumentId] && Util.IsFilledBuyOrder[InstrumentId])
                    {//平空
                        int hands = Math.Abs(netPos);
                        int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Buy, _OppoAskPrice, hands, true);// Only Close
                        if (st == 0)
                        {
                            TradingWatch.Stop();
                            string lastOrderRef = OrderManager.CombineOrderRef.ToString();
                            if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                            {
                                Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                            }
                            if (!Util.IsInReLoadingStatus(InstrumentId))
                            {
                                //Constant.WriteInfo(string.Format("PositonLost {0}, longCount {1}, shortCount {2}", PositonLost, longCount, shortCount));
                                Util.WriteInfo(string.Format("Submit Pb Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, EnumThostOffsetFlagType.Close, hands, PresentAskPrice));
                            }
                        }
                    }
                }
                if (netPos == 0)
                {
                    SimulateNetPosition = 0;
                    //continue
                }
                else
                {
                    if (double.IsNaN(signal))
                    {
                        Util.WriteWarn("Illegal signal value!");
                        return;
                    }
                    if (_State == 0)
                    {
                        if (signal.IsGreaterThan(MiniWindow))
                        {
                            _State = 1;
                        }
                        if (signal.IsLessThan(-MiniWindow))
                        {
                            _State = -1;
                        }
                    }
                    if (_State == 1)
                    {
                        if (signal.IsLessThan(0) && !Util.IsSentSellOrder[InstrumentId] && Util.IsFilledSellOrder[InstrumentId])
                        {
                            if (SimulateNetPosition >= 0)
                            {
                                //平多
                                int hands = Math.Abs(netPos);
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Sell, _OppoBidPrice, hands, true);// Only Close
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    string lastOrderRef = OrderManager.CombineOrderRef.ToString();
                                    if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                                    {
                                        Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                                    }
                                    if (!Util.IsInReLoadingStatus(InstrumentId))
                                    {
                                        Util.WriteInfo(string.Format("Submit Pb Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, EnumThostOffsetFlagType.Close, hands, PresentBidPrice));
                                    }
                                    SimulateNetPosition = 0;
                                    PositonLost = 0;
                                }
                            }
                            _State = 0;
                        }
                    }
                    if (_State == -1)
                    {
                        if (signal.IsGreaterThan(0.0) && !Util.IsSentBuyOrder[InstrumentId] && Util.IsFilledBuyOrder[InstrumentId])
                        {
                            if (SimulateNetPosition <= 0)
                            {
                                //平空
                                int hands = Math.Abs(netPos);
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Buy, _OppoAskPrice, hands, true);// Only Close
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    string lastOrderRef = OrderManager.CombineOrderRef.ToString();
                                    if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                                    {
                                        Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                                    }
                                    if (!Util.IsInReLoadingStatus(InstrumentId))
                                    {
                                        Util.WriteInfo(string.Format("Submit Pb Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, EnumThostOffsetFlagType.Close, hands, PresentAskPrice));
                                    }
                                    SimulateNetPosition = 0;
                                    PositonLost = 0;
                                }
                            }
                            _State = 0;
                        }
                    }
                }
            }
            else
            {
                //如果vectorList还没攒够timeWindow个vector，就不做
                if (DurationLst.Count >= TimeWindow)
                {
                    if (netPos > 0 && !Util.IsSentSellOrder[InstrumentId] && Util.IsFilledSellOrder[InstrumentId])
                    {
                        if (PositonLost.IsLessThan(-MaxLost) || LastPrice.IsLessThanOrEqualTo(DownWarningPrice) || LastPrice.IsGreaterThanOrEqualTo(TradeDataServer.DepthMarketDataFields[InstrumentId].UpperLimitPrice))
                        {
                            //chiCang = 0;//平多
                            int hands = Math.Abs(netPos);
                            int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Sell, _OppoBidPrice, hands, true);// Only Close
                            if (st == 0)
                            {
                                TradingWatch.Stop();
                                string lastOrderRef = OrderManager.CombineOrderRef.ToString();
                                if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                                {
                                    Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                                }
                                if (!Util.IsInReLoadingStatus(InstrumentId))
                                {
                                    Util.WriteInfo(string.Format("Submit Pb Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, EnumThostOffsetFlagType.Close, hands, PresentBidPrice));
                                }
                            }
                        }
                    }
                    if (netPos < 0 && !Util.IsSentBuyOrder[InstrumentId] && Util.IsFilledBuyOrder[InstrumentId])
                    {
                        if (PositonLost.IsLessThan(-MaxLost) || LastPrice.IsGreaterThanOrEqualTo(UpWarningPrice) || LastPrice.IsLessThanOrEqualTo(TradeDataServer.DepthMarketDataFields[InstrumentId].LowerLimitPrice))
                        {
                            //chiCang = 0;//平空
                            int hands = Math.Abs(netPos);
                            int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Buy, _OppoAskPrice, hands, true);// Only Close
                            if (st == 0)
                            {
                                TradingWatch.Stop();
                                string lastOrderRef = OrderManager.CombineOrderRef.ToString();
                                if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                                {
                                    Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                                }
                                if (!Util.IsInReLoadingStatus(InstrumentId))
                                {
                                    Util.WriteInfo(string.Format("Submit Pb Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, EnumThostOffsetFlagType.Close, hands, PresentAskPrice));
                                }
                            }
                        }
                    }
                    if (double.IsNaN(signal))
                    {
                        Util.WriteWarn("Illegal signal value!");
                        return;
                    }
                    if (_State == 0)
                    {
                        if (signal.IsGreaterThan(MiniWindow))
                        {
                            _State = 1;
                        }
                        if (signal.IsLessThan(-MiniWindow))
                        {
                            _State = -1;
                        }
                    }
                    if (_State == 1)
                    {
                        if (signal.IsLessThan(0.0))
                        {
                            if (SimulateNetPosition >= 0 && LastPrice.IsGreaterThan(DownWarningPrice) && LastPrice.IsLessThan(UpWarningPrice) && !Util.IsSentBuyOrder[InstrumentId] && Util.IsFilledBuyOrder[InstrumentId])
                            {
                                //chiCang = -1;
                                int hands = Math.Abs(-TargetPosCount - netPos);// 目标仓位 -1
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Sell, _OppoBidPrice, hands, false);
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    string lastOrderRef = OrderManager.CombineOrderRef.ToString();
                                    if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                                    {
                                        Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                                    }
                                    if (!Util.IsInReLoadingStatus(InstrumentId))
                                    {
                                        Util.WriteInfo(string.Format("Submit Pb Order {0} ticks, {1} ns, {2}, {3}, {4}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, hands, PresentBidPrice));
                                    }
                                    SimulateNetPosition = -TargetPosCount;
                                    PositonLost = 0;
                                }
                            }
                            _State = 0;
                        }
                    }
                    if (_State == -1)
                    {
                        if (signal.IsGreaterThan(0.0))
                        {
                            if (SimulateNetPosition <= 0 && LastPrice.IsGreaterThan(DownWarningPrice) && LastPrice.IsLessThan(UpWarningPrice) && !Util.IsSentBuyOrder[InstrumentId] && Util.IsFilledBuyOrder[InstrumentId])
                            {
                                //chiCang = 1;
                                int hands = Math.Abs(TargetPosCount - netPos);
                                int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Buy, _OppoAskPrice, hands, false);
                                if (st == 0)
                                {
                                    TradingWatch.Stop();
                                    string lastOrderRef = OrderManager.CombineOrderRef.ToString();
                                    if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                                    {
                                        Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                                    }
                                    if (!Util.IsInReLoadingStatus(InstrumentId))
                                    {
                                        Util.WriteInfo(string.Format("Submit Pb Order {0} ticks, {1} ns, {2}, {3}, {4}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, hands, PresentAskPrice));
                                    }
                                    SimulateNetPosition = TargetPosCount;
                                    PositonLost = 0;
                                }
                            }
                            _State = 0;
                        }
                    }
                }
                else if (!Util.IsInReLoadingStatus(InstrumentId))
                {
                    //Constant.WriteInfo(string.Format("{0} Pb: DurationLst items {1}", InstrumentId, DurationLst.Count));
                }
                LastSignal = signal;
            }
        }

        private bool CheckPositionDirection(int netPos)
        {
            if (SimulateNetPosition > 0 && netPos < 0)
            {
                //平空
                int hands = Math.Abs(netPos);
                int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Buy, _OppoAskPrice, hands, true);// Only Close
                if (st == 0)
                {
                    TradingWatch.Stop();
                    string lastOrderRef = OrderManager.CombineOrderRef.ToString();
                    if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                    {
                        Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                    }
                    if (!Util.IsInReLoadingStatus(InstrumentId))
                    {
                        Util.WriteInfo(string.Format("Submit Pb Force Close Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, EnumThostOffsetFlagType.Close, hands, PresentAskPrice));
                    }
                    SimulateNetPosition = 0;
                    PositonLost = 0;
                }
            }
            else if (SimulateNetPosition < 0 && netPos > 0)
            {
                //平多
                int hands = Math.Abs(netPos);
                int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Sell, _OppoBidPrice, hands, true);// Only Close
                if (st == 0)
                {
                    TradingWatch.Stop();
                    string lastOrderRef = OrderManager.CombineOrderRef.ToString();
                    if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                    {
                        Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                    }
                    if (!Util.IsInReLoadingStatus(InstrumentId))
                    {
                        Util.WriteInfo(string.Format("Submit Pb Force Close Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, EnumThostOffsetFlagType.Close, hands, PresentBidPrice));
                    }
                    SimulateNetPosition = 0;
                    PositonLost = 0;
                }
            }
            return true;
        }

        protected sealed override void ExecuteStrategy()
        {
            ExecutePbStrategy();
        }
    }
}
