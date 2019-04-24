using System;
using System.Configuration;

namespace AutoTrader
{
    public class StrategyRevertVectorMA : StrategyVectorMA
    {
        public StrategyRevertVectorMA(string instantInstrumentID, TradeDataServer tradeDataServer) : base(instantInstrumentID, tradeDataServer)
        {
            StrategyState = int.Parse(Util.StrategyParameter[instantInstrumentID]["RevertStrategy"]["State"]);
            TimeWindow = int.Parse(Util.StrategyParameter[instantInstrumentID]["RevertStrategy"]["TimeWindow"]);
            TargetPosCount = int.Parse(Util.StrategyParameter[instantInstrumentID]["RevertStrategy"]["TargetPosCount"]);
            MiniFactor = double.Parse(Util.StrategyParameter[instantInstrumentID]["RevertStrategy"]["MiniFactor"]);
            MaxDuration = int.Parse(Util.StrategyParameter[instantInstrumentID]["RevertStrategy"]["MaxDuration"]);
            JumpLevel = int.Parse(Util.StrategyParameter[instantInstrumentID]["RevertStrategy"]["JumpLevel"]);
            SimulateNetPosition = 0;
        }

        private int SimulateNetPosition;
        private double MaxLost = 0.0;
        private double _InitPosLost = 0.0;
        private double _OppoBidPrice, _OppoAskPrice = 0.0;
        private bool _StartChecked = false;

        private void ExecuteRevertStrategy()
        {
            if (MaxLost.IsEqualTo(0.0))
            {
                MaxLost = TargetPosCount * PriceTick * double.Parse(Util.StrategyParameter[InstrumentId]["RevertStrategy"]["MaxLost"]);
            }
            if (MiniWindow.IsEqualTo(0.0))
            {
                MiniWindow = MiniFactor * PriceTick;
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
            if (TradingTimeManager.IsInEndingTime(InstrumentId, ExchangeId, EndingSeconds)) // close all，添加保护代码
            {
                //只平不开了
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
                            Util.WriteInfo(string.Format("Submit Revert Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, EnumThostOffsetFlagType.Close, hands, PresentBidPrice));
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
                            Util.WriteInfo(string.Format("Submit Revert Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, EnumThostOffsetFlagType.Close, hands, PresentAskPrice));
                        }
                    }
                }
            }
            else
            {
                double signal = AveragePriceByVolume - AveragePriceByDuration;
                if (double.IsNaN(signal))
                {
                    Util.WriteWarn("Illegal signal value!");
                    return;
                }
                if (!Util.IsInReLoadingStatus(InstrumentId))
                {
                    if (!_StartChecked)
                    {
                        _StartChecked = CheckPositionDirection(netPos);
                    }
#if DEBUG
                    Util.WriteInfo(string.Format("Revert: InstrumentId {0}, Signal {1}, LastSignal {2}, PriceMean {3}, LastPriceMean {4}, SimulateNetPosition {5}, PositonLost {6}, longCount {7}, shortCount {8}, StrategyState {9}, AveragePriceByVolume {10}, AveragePriceByDuration {11}",
                        InstrumentId, signal, LastSignal, PriceMean, LastPriceMean, SimulateNetPosition, PositonLost, longCount, shortCount, StrategyState, AveragePriceByVolume, AveragePriceByDuration));
#endif
                }
                //如果vectorList还没攒够timeWindow个vector，就不做
                if (DurationLst.Count >= TimeWindow)
                {
                    if (StrategyState >= 1)
                    {
                        //改用模拟持仓
                        if (SimulateNetPosition == 0)
                        {
                            //如果我们算的均线价差超过阀值，并且价格中枢至少没有在回撤
                            if (signal.IsGreaterThan(MiniWindow) && PriceMean.IsGreaterThanOrEqualTo(LastPriceMean))
                            {
                                //模拟做多
                                SimulateNetPosition = TargetPosCount;
                            }
                            //相反的情况模拟做空
                            else if (signal.IsLessThan(-MiniWindow) && PriceMean.IsLessThanOrEqualTo(LastPriceMean))
                            {
                                SimulateNetPosition = -TargetPosCount;
                            }
                        }
                        //如果已经有持仓
                        else
                        {
                            // 如果模拟持仓是多头
                            if (SimulateNetPosition == TargetPosCount)
                            {
                                //信号已经掉到负阀值下面了
                                if (signal.IsLessThan(-MiniWindow))
                                {
                                    //状态参量切换，策略2启动
                                    StrategyState -= 1;
                                    SimulateNetPosition = 0;
                                }
                            }
                            // 持仓是负的情况是类似的
                            if (SimulateNetPosition == -TargetPosCount)
                            {
                                if (signal.IsGreaterThan(MiniWindow))
                                {
                                    StrategyState -= 1;
                                    SimulateNetPosition = 0;
                                }
                            }
                        }
                    }
                    else //如果状态量已经切到0，即反转策略已经可以开始启动
                    {
                        //考虑离开收盘前还有600个tick以上的情况
                        if (!TradingTimeManager.IsInEndingTime(InstrumentId, ExchangeId, PendingEndSeconds))
                        {
                            //如果信号大于阀值，行情没有在急涨，信号开始回落
                            if (signal.IsGreaterThan(MiniWindow) && PriceMean.IsLessThanOrEqualTo(LastPriceMean) && signal.IsLessThan(LastSignal))
                            {
                                //如果模拟持仓为0，做空
                                if (SimulateNetPosition >= 0 && LastPrice.IsGreaterThan(DownWarningPrice) && LastPrice.IsLessThan(UpWarningPrice) && !Util.IsSentSellOrder[InstrumentId] && Util.IsFilledSellOrder[InstrumentId])
                                {
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
                                            //Constant.WriteInfo(string.Format("Signal {0}, LastSignal {1}, PriceMean {2}, LastPriceMean {3}, SimulateNetPosition {4}", signal, LastSignal, PriceMean, LastPriceMean, SimulateNetPosition));
                                            Util.WriteInfo(string.Format("Submit Revert Order {0} ticks, {1} ns, {2}, {3}, {4}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, hands, PresentBidPrice));
                                        }
                                        SimulateNetPosition = -TargetPosCount;
                                        PositonLost = 0;
                                    }
                                }
                            }
                            //相反的情况做多
                            else if (signal.IsLessThan(-MiniWindow) && PriceMean.IsGreaterThanOrEqualTo(LastPriceMean) && signal.IsGreaterThan(LastSignal))
                            {
                                if (SimulateNetPosition <= 0 && LastPrice.IsGreaterThan(DownWarningPrice) && LastPrice.IsLessThan(UpWarningPrice) && !Util.IsSentBuyOrder[InstrumentId] && Util.IsFilledBuyOrder[InstrumentId])
                                {
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
                                            //Constant.WriteInfo(string.Format("Signal {0}, LastSignal {1}, PriceMean {2}, LastPriceMean {3}, SimulateNetPosition {4}", signal, LastSignal, PriceMean, LastPriceMean, SimulateNetPosition));
                                            Util.WriteInfo(string.Format("Submit Revert Order {0} ticks, {1} ns, {2}, {3}, {4}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, hands, PresentAskPrice));
                                        }
                                        SimulateNetPosition = TargetPosCount;
                                        PositonLost = 0;
                                    }
                                }
                            }
                            //如果浮动亏损超过了阀值，砍仓！只改变持仓，不改变模拟持仓
                            if (netPos > 0 && !Util.IsSentSellOrder[InstrumentId] && Util.IsFilledSellOrder[InstrumentId])
                            {
                                if (PositonLost.IsLessThan(-MaxLost) || LastPrice.IsLessThanOrEqualTo(DownWarningPrice) || LastPrice.IsGreaterThanOrEqualTo(TradeDataServer.DepthMarketDataFields[InstrumentId].UpperLimitPrice))
                                {
                                    int hands = Math.Abs(netPos);
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
                                            //Constant.WriteInfo(string.Format("PositonLost {0}, longCount {1}, shortCount {2}", PositonLost, longCount, shortCount));
                                            Util.WriteInfo(string.Format("Submit Revert Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, EnumThostOffsetFlagType.Close, hands, PresentBidPrice));
                                        }
                                        PositonLost = 0;
                                    }
                                }
                            }
                            else if (netPos < 0 && !Util.IsSentBuyOrder[InstrumentId] && Util.IsFilledBuyOrder[InstrumentId])
                            {
                                if (PositonLost.IsLessThan(-MaxLost) || LastPrice.IsGreaterThanOrEqualTo(UpWarningPrice) || LastPrice.IsLessThanOrEqualTo(TradeDataServer.DepthMarketDataFields[InstrumentId].LowerLimitPrice))
                                {
                                    int hands = Math.Abs(netPos);
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
                                            //Constant.WriteInfo(string.Format("PositonLost {0}, longCount {1}, shortCount {2}", PositonLost, longCount, shortCount));
                                            Util.WriteInfo(string.Format("Submit Revert Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, EnumThostOffsetFlagType.Close, hands, PresentAskPrice));
                                        }
                                        PositonLost = 0;
                                    }
                                }
                            }
                        }
                        else //考虑最后600个tick的情况
                        {
                            if (SimulateNetPosition == TargetPosCount && !Util.IsSentSellOrder[InstrumentId] && Util.IsFilledSellOrder[InstrumentId])
                            {
                                //如果出现了空头信号，或者离收盘只剩10个tick了
                                if ((signal.IsGreaterThan(MiniWindow) && PriceMean.IsLessThanOrEqualTo(LastPriceMean) && signal.IsLessThan(LastSignal)) || TradingTimeManager.IsInEndingTime(InstrumentId, ExchangeId, EndingSeconds))
                                {
                                    //只平不开了
                                    int hands = Math.Abs(netPos);
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
                                            //Constant.WriteInfo(string.Format("Signal {0}, LastSignal {1}, PriceMean {2}, LastPriceMean {3}, SimulateNetPosition {4}", signal, LastSignal, PriceMean, LastPriceMean, SimulateNetPosition));
                                            Util.WriteInfo(string.Format("Submit Revert Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, EnumThostOffsetFlagType.Close, hands, PresentBidPrice));
                                        }
                                        SimulateNetPosition = 0;
                                    }
                                }
                            }
                            if (SimulateNetPosition == -TargetPosCount && !Util.IsSentBuyOrder[InstrumentId] && Util.IsFilledBuyOrder[InstrumentId])
                            {
                                //如果出现多头信号，或者离收盘只剩10个tick了
                                if ((signal.IsLessThan(-MiniWindow) && PriceMean.IsGreaterThanOrEqualTo(LastPriceMean) && signal.IsGreaterThan(LastSignal)) || TradingTimeManager.IsInEndingTime(InstrumentId, ExchangeId, EndingSeconds))
                                {
                                    //只平不开了
                                    int hands = Math.Abs(netPos);
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
                                            //Constant.WriteInfo(string.Format("Signal {0}, LastSignal {1}, PriceMean {2}, LastPriceMean {3}, SimulateNetPosition {4}", signal, LastSignal, PriceMean, LastPriceMean, SimulateNetPosition));
                                            Util.WriteInfo(string.Format("Submit Revert Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, EnumThostOffsetFlagType.Close, hands, PresentAskPrice));
                                        }
                                        SimulateNetPosition = 0;
                                    }
                                }
                            }
                            // 如果浮动亏损超过了阀值，砍仓！只改变持仓，不改变模拟持仓
                            if (netPos > 0 && !Util.IsSentSellOrder[InstrumentId] && Util.IsFilledSellOrder[InstrumentId])
                            {
                                if (PositonLost.IsLessThan(-MaxLost) || LastPrice.IsLessThanOrEqualTo(DownWarningPrice) || LastPrice.IsGreaterThanOrEqualTo(TradeDataServer.DepthMarketDataFields[InstrumentId].UpperLimitPrice))
                                {
                                    int hands = Math.Abs(netPos);
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
                                            //Constant.WriteInfo(string.Format("PositonLost {0}, longCount {1}, shortCount {2}", PositonLost, longCount, shortCount));
                                            Util.WriteInfo(string.Format("Submit Revert Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, EnumThostOffsetFlagType.Close, hands, PresentBidPrice));
                                        }
                                        PositonLost = 0;
                                    }
                                }
                            }
                            else if (netPos < 0 && !Util.IsSentBuyOrder[InstrumentId] && Util.IsFilledBuyOrder[InstrumentId])
                            {
                                if (PositonLost.IsLessThan(-MaxLost) || LastPrice.IsGreaterThanOrEqualTo(UpWarningPrice) || LastPrice.IsLessThanOrEqualTo(TradeDataServer.DepthMarketDataFields[InstrumentId].LowerLimitPrice))
                                {
                                    int hands = Math.Abs(netPos);
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
                                            //Constant.WriteInfo(string.Format("PositonLost {0}, longCount {1}, shortCount {2}", PositonLost, longCount, shortCount));
                                            Util.WriteInfo(string.Format("Submit Revert Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, EnumThostOffsetFlagType.Close, hands, PresentAskPrice));
                                        }
                                        PositonLost = 0;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (!Util.IsInReLoadingStatus(InstrumentId))
                {
                    //Constant.WriteInfo(string.Format("{0} Revert: DurationLst items {1}", InstrumentId, DurationLst.Count));
                }
                LastSignal = signal;
            }
            if (Util.IsSentBuyOrder[InstrumentId] || !Util.IsFilledBuyOrder[InstrumentId])
            {
                Util.WriteInfo(string.Format("Revert: InstrumentId {0}, IsSentBuyOrder {1}, IsFilledBuyOrder {2}", InstrumentId, Util.IsSentBuyOrder[InstrumentId], Util.IsFilledBuyOrder[InstrumentId]));
            }
            if (Util.IsSentSellOrder[InstrumentId] || !Util.IsFilledSellOrder[InstrumentId])
            {
                Util.WriteInfo(string.Format("Revert: InstrumentId {0}, IsSentSellOrder {1}, IsFilledSellOrder {2}", InstrumentId, Util.IsSentSellOrder[InstrumentId], Util.IsFilledSellOrder[InstrumentId]));
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
                    string lastOrderRef = OrderManager.CombineOrderRef;
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
                    string lastOrderRef = OrderManager.CombineOrderRef;
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
            ExecuteRevertStrategy();
        }
    }
}
