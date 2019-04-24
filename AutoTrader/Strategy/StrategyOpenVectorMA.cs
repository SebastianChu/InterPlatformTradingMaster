using System;
using System.Configuration;

namespace AutoTrader
{
    public class StrategyOpenVectorMA : StrategyVectorMA
    {
        public StrategyOpenVectorMA(string instantInstrumentID, TradeDataServer tradeDataServer) : base(instantInstrumentID, tradeDataServer)
        {
            StrategyState = int.Parse(Util.StrategyParameter[instantInstrumentID]["OpenStrategy"]["State"]);
            TimeWindow = int.Parse(Util.StrategyParameter[instantInstrumentID]["OpenStrategy"]["TimeWindow"]);
            TargetPosCount = int.Parse(Util.StrategyParameter[instantInstrumentID]["OpenStrategy"]["TargetPosCount"]);
            MiniFactor = double.Parse(Util.StrategyParameter[instantInstrumentID]["OpenStrategy"]["MiniFactor"]);
            MaxDuration = int.Parse(Util.StrategyParameter[instantInstrumentID]["OpenStrategy"]["MaxDuration"]);
            JumpLevel = int.Parse(Util.StrategyParameter[instantInstrumentID]["OpenStrategy"]["JumpLevel"]);
        }

        private double _OppoBidPrice, _OppoAskPrice = 0.0;

        private void ExecuteOpenStrategy()
        {
            if (MiniWindow.IsEqualTo(0.0))
            {
                MiniWindow = MiniFactor * PriceTick;
            }
            if (TradingTimeManager.IsInEndingTime(InstrumentId, ExchangeId, PendingEndSeconds))// 300s left: set state
            {
                StrategyState = 0;
            }
            int longCount = TradeDataServer.PositionFields.ContainsKey(LongPosKey) ? TradeDataServer.PositionFields[LongPosKey].Position : 0;
            int shortCount = TradeDataServer.PositionFields.ContainsKey(ShortPosKey) ? TradeDataServer.PositionFields[ShortPosKey].Position : 0;
            int netPos = longCount - shortCount;
            _OppoAskPrice = Math.Min(PresentAskPrice + PriceTick * JumpLevel, UpLimitPrice);
            _OppoBidPrice = Math.Max(PresentBidPrice - PriceTick * JumpLevel, DownLimitPrice);
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
                        string lastOrderRef = OrderManager.CombineOrderRef.ToString();
                        if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                        {
                            Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                        }
                        if (!Util.IsInReLoadingStatus(InstrumentId))
                        {
                            Util.WriteInfo(string.Format("Submit Open Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, EnumThostOffsetFlagType.Close, hands, PresentBidPrice));
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
                        string lastOrderRef = OrderManager.CombineOrderRef.ToString();
                        if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                        {
                            Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                        }
                        if (!Util.IsInReLoadingStatus(InstrumentId))
                        {
                            Util.WriteInfo(string.Format("Submit Open Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, EnumThostOffsetFlagType.Close, hands, PresentAskPrice));
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
#if DEBUG
                    Util.WriteInfo(string.Format("Open: InstrumentId {0}, Signal {1}, LastSignal {2}, PriceMean {3}, LastPriceMean {4}, PositonLost {5}, longCount {6}, shortCount {7}, StrategyState {8}, AveragePriceByVolume {9}, AveragePriceByDuration {10}",
                        InstrumentId, signal, LastSignal, PriceMean, LastPriceMean, PositonLost, longCount, shortCount, StrategyState, AveragePriceByVolume, AveragePriceByDuration));
#endif
                }
                //如果vectorList还没攒够timeWindow个vector，就不做
                if (DurationLst.Count >= TimeWindow) //
                {
                    if (StrategyState >= 1)
                    {
                        if (netPos == 0)
                        {
                            if (signal.IsGreaterThan(MiniWindow) && PriceMean.IsGreaterThanOrEqualTo(LastPriceMean) && LastPrice.IsGreaterThan(DownWarningPrice) && LastPrice.IsLessThan(UpWarningPrice)
                                && !Util.IsSentBuyOrder[InstrumentId] && Util.IsFilledBuyOrder[InstrumentId])
                            {
                                // 开仓做多
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
                                        //Constant.WriteInfo(string.Format("Signal {0}, PriceMean {1}, LastPriceMean {2}", signal, PriceMean, LastPriceMean));
                                        Util.WriteInfo(string.Format("Submit Open Order {0} ticks, {1} ns, {2}, {3}, {4}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, hands, PresentAskPrice));
                                    }
                                }
                            }
                            else if (signal.IsLessThan(-MiniWindow) && PriceMean.IsLessThanOrEqualTo(LastPriceMean) && LastPrice.IsGreaterThan(DownWarningPrice) && LastPrice.IsLessThan(UpWarningPrice)
                                && !Util.IsSentSellOrder[InstrumentId] && Util.IsFilledSellOrder[InstrumentId])
                            {
                                //相反的情况做空
                                int hands = Math.Abs(-TargetPosCount - netPos);
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
                                        //Constant.WriteInfo(string.Format("Signal {0}, PriceMean {1}, LastPriceMean {2}", signal, PriceMean, LastPriceMean));
                                        Util.WriteInfo(string.Format("Submit Open Order {0} ticks, {1} ns, {2}, {3}, {4}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, hands, PresentBidPrice));
                                    }
                                }
                            }
                        }
                        else //如果已经有持仓
                        {
                            if (netPos > 0) //== 1
                            {
                                if ((signal.IsLessThan(-MiniWindow) || LastPrice.IsLessThanOrEqualTo(DownWarningPrice) || LastPrice.IsGreaterThanOrEqualTo(TradeDataServer.DepthMarketDataFields[InstrumentId].UpperLimitPrice))
                                    && !Util.IsSentSellOrder[InstrumentId] && Util.IsFilledSellOrder[InstrumentId])
                                {
                                    //更新状态参量，说明一把动量的结束
                                    StrategyState -= 1;
                                    //砍仓
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
                                            //Constant.WriteInfo(string.Format("Signal {0}, longCount {1}, shortCount {2}, StrategyState {3}", signal, longCount, shortCount, StrategyState));
                                            Util.WriteInfo(string.Format("Submit Open Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Sell, EnumThostOffsetFlagType.Close, hands, PresentBidPrice));
                                        }
                                    }
                                }
                            }
                            else if (netPos < 0) // == -1
                            {
                                if ((signal.IsGreaterThan(MiniWindow) || LastPrice.IsGreaterThanOrEqualTo(UpWarningPrice) || LastPrice.IsLessThanOrEqualTo(TradeDataServer.DepthMarketDataFields[InstrumentId].LowerLimitPrice))
                                    && !Util.IsSentBuyOrder[InstrumentId] && Util.IsFilledBuyOrder[InstrumentId])
                                {
                                    StrategyState -= 1;
                                    //砍仓
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
                                            //Constant.WriteInfo(string.Format("Signal {0}, longCount {1}, shortCount {2}, StrategyState {3}", signal, longCount, shortCount, StrategyState));
                                            Util.WriteInfo(string.Format("Submit Open Order {0} ticks, {1} ns, {2}, {3}, {4}, {5}", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.NanosecPerTick, EnumThostDirectionType.Buy, EnumThostOffsetFlagType.Close, hands, PresentAskPrice));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (!Util.IsInReLoadingStatus(InstrumentId))
                {
                    //Constant.WriteInfo(string.Format("{0} Open: DurationLst items {1}", InstrumentId, DurationLst.Count));
                }
                LastSignal = signal;
            }
            if (Util.IsSentBuyOrder[InstrumentId] || !Util.IsFilledBuyOrder[InstrumentId])
            {
                Util.WriteInfo(string.Format("Open: InstrumentId {0}, IsSentBuyOrder {1}, IsFilledBuyOrder {2}", InstrumentId, Util.IsSentBuyOrder[InstrumentId], Util.IsFilledBuyOrder[InstrumentId]));
            }
            if (Util.IsSentSellOrder[InstrumentId] || !Util.IsFilledSellOrder[InstrumentId])
            {
                Util.WriteInfo(string.Format("Open: InstrumentId {0}, IsSentSellOrder {1}, IsFilledSellOrder {2}", InstrumentId, Util.IsSentSellOrder[InstrumentId], Util.IsFilledSellOrder[InstrumentId]));
            }
        }

        protected sealed override void ExecuteStrategy()
        {
            ExecuteOpenStrategy();
        }
    }
}
