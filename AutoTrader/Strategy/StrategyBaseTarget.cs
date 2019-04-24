using System;
using System.Collections.Generic;

namespace AutoTrader
{
    public class StrategyBaseTarget : StrategyControl
    {
        public StrategyBaseTarget(string instantInstrumentID, TradeDataServer tradeDataServer, AlgorithmicTrader algoTrader) : base(tradeDataServer)
        {
            if (TradeDataServer.InstrumentFields.ContainsKey(instantInstrumentID.ToUpper()))
            {
                InstrumentId = instantInstrumentID.ToUpper();
                ExchangeId = TradeDataServer.InstrumentFields[InstrumentId].ExchangeId;
            }
            else if (TradeDataServer.InstrumentFields.ContainsKey(instantInstrumentID.ToLower()))
            {
                InstrumentId = instantInstrumentID.ToLower();
                ExchangeId = TradeDataServer.InstrumentFields[InstrumentId].ExchangeId;
            }
            else
            {
                Util.WriteError("No illegal exchange id!");
            }
            AlgoTrader = algoTrader;
            Init();
            tradeDataServer.RtnNotifyOrderStatus += NotifyStrategyOrderStatusReceiver;
            IsRunning = true;
        }


        protected AlgorithmicTrader AlgoTrader;

        protected string StrategyId { get; set; }

        protected string InstrumentId { get; set; }

        protected string ExchangeId { get; set; }
        protected double PriceTick { get; set; }
        protected int VolumeMultiple { get; set; }

        protected int TargetVolume { get; set; }

        protected int MaxVolume { get; set; }

        protected EnumThostDirectionType EntryDirection;

        protected double CancelPendingSec;

        protected CThostFtdcDepthMarketDataField LastDepthMarketData;

        protected List<string> PendingCancelOrderRef = new List<string>();

        private void Init()
        {
            VolumeMultiple = (int)TradeDataServer.InstrumentFields[InstrumentId].VolumeMultiple;
            PriceTick = (double)TradeDataServer.InstrumentFields[InstrumentId].PriceTick;
        }

        public virtual void InitParameters()
        {
            
        }

        protected bool IsInEntryStatus()
        {
            int shortKey = Util.GetPositionKey(InstrumentId, EnumThostPosiDirectionType.Short);
            int shortPos = 0;
            if (TradeDataServer.PositionFields.ContainsKey(shortKey))
            {
                shortPos = TradeDataServer.PositionFields[shortKey].Position;
            }

            int longKey = Util.GetPositionKey(InstrumentId, EnumThostPosiDirectionType.Long);
            int longPos = 0;
            if (TradeDataServer.PositionFields.ContainsKey(longKey))
            {
                longPos = TradeDataServer.PositionFields[longKey].Position;
            }

            if (EntryDirection == EnumThostDirectionType.Buy && longPos > shortPos)
            {
                return true;
            }
            else if (EntryDirection == EnumThostDirectionType.Sell && longPos < shortPos)
            {
                return true;
            }
            return false;
        }

        protected override void StartDepthMarketDataProcessing(CThostFtdcDepthMarketDataField depthMarketData)
        {
            if (IsRunning && depthMarketData.InstrumentID == InstrumentId)
            {
                DateTime tickTime = DateTime.Parse(depthMarketData.UpdateTime);
                if (depthMarketData.UpdateMillisec > 0)
                {
                    tickTime = DateTime.ParseExact(string.Format("{0}.{1:d3}", depthMarketData.UpdateTime, depthMarketData.UpdateMillisec), "HH:mm:ss.fff", null);
                }
                if (TradingTimeManager.IsInTradingTime(tickTime, ExchangeId, InstrumentId, true) && TradingTimeManager.IsInTradingTime(DateTime.Now, ExchangeId, InstrumentId, false))
                {
                    StrategyTrigger(tickTime, depthMarketData);
                }
                LastDepthMarketData = depthMarketData;
            }
        }

        protected virtual void StrategyTrigger(DateTime tickTime, CThostFtdcDepthMarketDataField depthMarketData)
        {
        }

        protected void CancelPendingOrders(CThostFtdcDepthMarketDataField depthMarketData)
        {
            if (AlgoTrader.InstrumentToPendingOrders.ContainsKey(InstrumentId))
            {
                foreach (string orderRef in AlgoTrader.InstrumentToPendingOrders[InstrumentId].Keys)
                {
                    CThostFtdcOrderField order = AlgoTrader.InstrumentToPendingOrders[InstrumentId][orderRef];
                    if (order.Direction == EnumThostDirectionType.Buy && Util.IsSentCancelBuy[order.InstrumentID]
                     || order.Direction == EnumThostDirectionType.Sell && Util.IsSentCancelSell[order.InstrumentID])
                        continue;

                    if (OrderManager.IsCancellable(order) &&
                         OrderManager.IsOrderOverTime(order, CancelPendingSec))
                    //|| depthMarketData.BidPrice1.IsGreaterThanOrEqualTo(order.LimitPrice + PriceTick) || depthMarketData.AskPrice1.IsLessThanOrEqualTo(order.LimitPrice - PriceTick)))
                    {
                        Util.WriteInfo(string.Format("InstrumentId {0}: InsertTime {1}, LimitPrice {2}, bidPrice {3}, askPrice {4}"
                            , InstrumentId, order.InsertTime, order.LimitPrice, depthMarketData.BidPrice1, depthMarketData.AskPrice1), Program.InPrintMode);
                        if (Util.IsClientTagRef(order.OrderRef, order.UserProductInfo))
                        {
                            OrderManager.CancelOrder(order);
                            if (order.Direction == EnumThostDirectionType.Buy)
                            {
                                Util.IsSentCancelBuy[order.InstrumentID] = true;
                            }
                            else if (order.Direction == EnumThostDirectionType.Sell)
                            {
                                Util.IsSentCancelSell[order.InstrumentID] = true;
                            }
                            PendingCancelOrderRef.Add(order.OrderRef);
                        }
                    }
                }
            }
        }

        public void NotifyStrategyOrderStatusReceiver(CThostFtdcOrderField pOrder, bool isQry)
        {
            //TriggerMarkupOrder
            if (pOrder.InstrumentID == InstrumentId && pOrder.OrderStatus == EnumThostOrderStatusType.Canceled && !isQry)
            {
                HashSet<string> refRemoval = new HashSet<string>();
                if (PendingCancelOrderRef.Contains(pOrder.OrderRef) && Util.IsClientTagRef(pOrder.OrderRef, pOrder.UserProductInfo))
                {
                    if (pOrder.VolumeTotal > pOrder.VolumeTraded)
                    {
                        ExecuteMarkupOrder(EntryDirection, pOrder.VolumeTotal - pOrder.VolumeTraded, StrategyId);
                        refRemoval.Add(pOrder.OrderRef);
                    }
                    PendingCancelOrderRef.RemoveElements(refRemoval.Contains, null);
                }
            }
        }

        protected bool ExecuteMarkupOrder(EnumThostDirectionType FirstDirection, int volume, string strategyId)
        {
            if (TradingTimeManager.IsInTradingTime(DateTime.Now, ExchangeId, InstrumentId, false))
            {
                if (FirstDirection == EnumThostDirectionType.Buy && !Util.IsSentBuyOrder[InstrumentId])
                {
                    int hands = volume;//
                    int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Buy, 0, hands, false, strategyId, EnumOrderType.Market, true, true, true, MaxVolume);
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
                            Util.WriteInfo(string.Format("Submit Markup Order {0} ticks, {1} ns, {2}, {3}, {4}"
                                , TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.MicrosecPerTick, EnumThostDirectionType.Buy, hands, 0));
                        }
                        return true;
                    }
                }
                else if (FirstDirection == EnumThostDirectionType.Sell && !Util.IsSentSellOrder[InstrumentId])
                {
                    int hands = volume;
                    int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Sell, 0, hands, false, strategyId, EnumOrderType.Market, true, true, true, MaxVolume);
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
                            Util.WriteInfo(string.Format("Submit Markup Order {0} ticks, {1} ns, {2}, {3}, {4}"
                                , TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.MicrosecPerTick, EnumThostDirectionType.Sell, hands, 0));
                        }
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
