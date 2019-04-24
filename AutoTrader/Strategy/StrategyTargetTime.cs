using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader
{
    public class StrategyTargetTime : StrategyBaseTarget
    {
        public StrategyTargetTime(string instantInstrumentID, TradeDataServer tradeDataServer, AlgorithmicTrader algoTrader)
            : base(instantInstrumentID, tradeDataServer, algoTrader)
        {
            InitParameters();
            IsEnteredMarket = IsInEntryStatus();
            IsDepartureTriggered = IsDepartureStatus();
        }

        protected DateTime OpenTime;
        protected DateTime CloseTime;

        private bool IsEnteredMarket = false;
        private bool IsDepartureTriggered = false;

        private int ShortPosition;
        private int LongPosition;

        public override void InitParameters()
        {
            StrategyId = Util.StrategyParameter[InstrumentId]["TargetTime"]["StrategyId"];
            EntryDirection = int.Parse(Util.StrategyParameter[InstrumentId]["TargetTime"]["EntryDirection"]) == 1 ? EnumThostDirectionType.Buy : EnumThostDirectionType.Sell;
            CancelPendingSec = int.Parse(Util.StrategyParameter[InstrumentId]["TargetTime"]["CancelPendingSeconds"]);
            OpenTime = DateTime.ParseExact(Util.StrategyParameter[InstrumentId]["TargetTime"]["OpenTime"].ToString(), "HH:mm:ss", null);
            CloseTime = DateTime.ParseExact(Util.StrategyParameter[InstrumentId]["TargetTime"]["CloseTime"].ToString(), "HH:mm:ss", null);
            TargetVolume = int.Parse(Util.StrategyParameter[InstrumentId]["TargetTime"]["TargetVolume"]);
            MaxVolume = int.Parse(Util.StrategyParameter[InstrumentId]["TargetTime"]["MaxVolume"]);
            IsRunning = true;
        }

        protected bool IsDepartureStatus()
        {
            int shortKey = Util.GetPositionKey(InstrumentId, EnumThostPosiDirectionType.Short);
            ShortPosition = 0;
            if (TradeDataServer.PositionFields.ContainsKey(shortKey))
            {
                ShortPosition = TradeDataServer.PositionFields[shortKey].Position;
            }

            int longKey = Util.GetPositionKey(InstrumentId, EnumThostPosiDirectionType.Long);
            LongPosition = 0;
            if (TradeDataServer.PositionFields.ContainsKey(longKey))
            {
                LongPosition = TradeDataServer.PositionFields[longKey].Position;
            }
            return LongPosition == ShortPosition;
        }

        protected override void StrategyTrigger(DateTime tickTime, CThostFtdcDepthMarketDataField depthMarketData)
        {
            CancelPendingOrders(depthMarketData);
            if (!IsEnteredMarket && tickTime >= OpenTime && tickTime < CloseTime)
            {
                EntryStep(depthMarketData);
                IsEnteredMarket = true;
                IsDepartureTriggered = false;
            }
            else if (IsEnteredMarket && !IsDepartureTriggered && tickTime >= CloseTime)
            {
                DepartureStep(depthMarketData);
                IsDepartureTriggered = true;
            }
        }


        private void EntryStep(CThostFtdcDepthMarketDataField depthMarketData)
        {
            EnumThostDirectionType direction = EntryDirection;
            if (direction == EnumThostDirectionType.Buy && !Util.IsSentBuyOrder[InstrumentId]
                || direction == EnumThostDirectionType.Sell && !Util.IsSentSellOrder[InstrumentId])
            {
                double price = depthMarketData.AskPrice1 > 0 ? Math.Min(depthMarketData.AskPrice1, depthMarketData.UpperLimitPrice) : depthMarketData.UpperLimitPrice;
                //double price = depthMarketData.BidPrice1 > 0 ? Math.Min(depthMarketData.BidPrice1, depthMarketData.UpperLimitPrice) : depthMarketData.UpperLimitPrice;
                int hands = TargetVolume;

                int netPosition = LongPosition - ShortPosition;

                int st = OrderManager.SubmitOrder(InstrumentId, direction, price, hands, false, StrategyId.ToString(), EnumOrderType.Limit, true, true, true, MaxVolume);
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
                        Util.WriteInfo(string.Format("Submit Timing Order {0} ticks, {1} ns, {2}, {3}, {4}"
                            , TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.MicrosecPerTick, EnumThostDirectionType.Buy, hands, price), Program.InPrintMode);
                    }
                }
            }
        }

        private void DepartureStep(CThostFtdcDepthMarketDataField depthMarketData)
        {
            EnumThostDirectionType closeDirection = EntryDirection == EnumThostDirectionType.Buy ? EnumThostDirectionType.Sell : EnumThostDirectionType.Buy;
            if (closeDirection == EnumThostDirectionType.Buy && !Util.IsSentBuyOrder[InstrumentId]
               || closeDirection == EnumThostDirectionType.Sell && !Util.IsSentSellOrder[InstrumentId])
            {
                double price = depthMarketData.BidPrice1 > 0 ? Math.Max(depthMarketData.BidPrice1, depthMarketData.LowerLimitPrice) : depthMarketData.LowerLimitPrice;
                //double price = depthMarketData.AskPrice1 > 0 ? Math.Max(depthMarketData.AskPrice1, depthMarketData.LowerLimitPrice) : depthMarketData.LowerLimitPrice;
                int hands = TargetVolume;//??volume
                int st = OrderManager.SubmitOrder(InstrumentId, closeDirection, price, hands, false, StrategyId.ToString(), EnumOrderType.Limit, true, true, true, MaxVolume);
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
                        Util.WriteInfo(string.Format("Submit Timing Order {0} ticks, {1} ns, {2}, {3}, {4}"
                            , TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.MicrosecPerTick, EnumThostDirectionType.Buy, hands, price), Program.InPrintMode);
                    }
                }
            }
        }
    }
}
