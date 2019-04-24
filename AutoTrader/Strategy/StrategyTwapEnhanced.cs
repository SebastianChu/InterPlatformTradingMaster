using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Configuration;
using System.Linq;
using System.Collections.Concurrent;
using System.Data;
using System.Data.SqlClient;

namespace AutoTrader
{
    public class StrategyTwapEnhanced : StrategyControl
    {
        public StrategyTwapEnhanced(string instantInstrumentID, TradeDataServer tradeDataServer, AlgorithmicTrader algoTrader) : base(tradeDataServer)
        {
            InstrumentId = instantInstrumentID;
            if (TradeDataServer.InstrumentFields.ContainsKey(InstrumentId))
            {
                ExchangeId = TradeDataServer.InstrumentFields[InstrumentId].ExchangeId;
            }
            else
            {
                Util.WriteError("No illegal exchange id!");
            }
            AlgoTrader = algoTrader;
            Init();
            tradeDataServer.RtnNotifyOrderStatus += NotifyStrategyOrderStatusReceiver;
            tradeDataServer.RtnNotifyFilled += NotifyStrategyFilledReceiver;
        }
        protected string InstrumentId { get; set; }

        protected string ExchangeId { get; set; }
        protected double PriceTick { get; set; }
        protected int VolumeMultiple { get; set; }
        protected bool IsSupportCloseToday;

        protected int PendingEndSeconds;
        protected int EndingSeconds;
        protected int LongPosKey;
        protected int ShortPosKey;

        protected double LastBidPrice;
        protected double LastAskPrice;
        protected int LastRawVolume;
        protected double LastSignal;
        protected double LastPrice;
        protected double LastOldPrice;
        protected double PresentAskPrice;
        protected double PresentBidPrice;
        //protected int PresentRawVolume;

        protected double HandicapRatioThreshold { get; set; }
        protected double LongHandicapRatio { get; set; }
        protected double ShortHandicapRatio { get; set; }

        protected double UpWarningPrice;
        protected double DownWarningPrice;
        protected double UpLimitPrice;
        protected double DownLimitPrice;
        private CThostFtdcDepthMarketDataField LastDepthMarketData;

        protected int MarketOrderCount;
        protected int LimitOrderCount;
        protected int MarkupOrderCount;
        protected double CancelPendingSec;
        private AlgorithmicTrader AlgoTrader;
        protected bool _StaticticsLaunched = false;
        protected List<string> PendingCancelOrderRef;

        protected override void StartDepthMarketDataProcessing(CThostFtdcDepthMarketDataField depthMarketData)
        {
            if (depthMarketData.InstrumentID == InstrumentId)
            {
                DateTime tickTime = DateTime.Parse(depthMarketData.UpdateTime);
                if (depthMarketData.UpdateMillisec > 0)
                {
                    tickTime = DateTime.ParseExact(string.Format("{0}.{1:d3}", depthMarketData.UpdateTime, depthMarketData.UpdateMillisec), "HH:mm:ss.fff", null);
                }
                if (TradingTimeManager.IsInTradingTime(tickTime, ExchangeId, InstrumentId, true))
                {
                    QuoteCalculator(depthMarketData);
                }
                LastDepthMarketData = depthMarketData;
            }
        }

        private void Init()
        {
            //MiniWindow = 0.0;
            //LastOldPrice = LastPriceMean = LastAskPrice = LastBidPrice = 0.0;
            //AveragePriceByVolume = AveragePriceByDuration = 0.0;
            //ShortPosKey = Utils.GetPositionKey(InstrumentId, EnumThostPosiDirectionType.Short);
            //LongPosKey = Utils.GetPositionKey(InstrumentId, EnumThostPosiDirectionType.Long);
            //IsSupportCloseToday = Utils.IsShfeInstrument(InstrumentId);
            HandicapRatioThreshold = double.Parse(Util.StrategyParameter[InstrumentId]["TwapAdvanced"]["HandicapRatio"]);
            CancelPendingSec = double.Parse(Util.StrategyParameter[InstrumentId]["TwapAdvanced"]["PendingSeconds"].ToString());
            VolumeMultiple = TradeDataServer.InstrumentFields[InstrumentId].VolumeMultiple;
            PriceTick = TradeDataServer.InstrumentFields[InstrumentId].PriceTick;

            if (!Util.MarkupOrderReportDict.ContainsKey(InstrumentId))
            {
                Util.MarkupOrderReportDict[InstrumentId] = new ConcurrentDictionary<string, MarkupOrderStatistics>();
            }
            PendingCancelOrderRef = new List<string>();
            PendingEndSeconds = int.Parse(ConfigurationManager.AppSettings["PendingEndSeconds"]);
            EndingSeconds = int.Parse(ConfigurationManager.AppSettings["EndingSeconds"]);
        }

        public override void SetNoTradingValue()
        {
            Util.WriteInfo("SetNoTradingValue Works.");
        }

        public void StartWatch()
        {
            TradingWatch = Stopwatch.StartNew();
        }

        private bool QuoteCalculator(CThostFtdcDepthMarketDataField depthMarketDataField)
        {
            if (!TradeDataServer.InstrumentFields.ContainsKey(InstrumentId))
            {
                return false;
            }
            if (UpLimitPrice == DownLimitPrice && UpLimitPrice == 0.0)
            {
                UpLimitPrice = depthMarketDataField.UpperLimitPrice;
                DownLimitPrice = depthMarketDataField.LowerLimitPrice;
            }
            //if (AlgoTrader.InstrumentToPendingOrders.ContainsKey(InstrumentId))
            //{
            //    if (LimitOrderCount == 0)
            //        LimitOrderCount = (from x in AlgoTrader.InstrumentToPendingOrders[InstrumentId].Values where x.LimitPrice != UpLimitPrice && x.LimitPrice != DownLimitPrice select x).Count();
            //    if (MarkupOrderCount == 0)
            //        MarkupOrderCount = (from x in AlgoTrader.InstrumentToPendingOrders[InstrumentId].Values where x.LimitPrice == UpLimitPrice || x.LimitPrice == DownLimitPrice select x).Count();
            //}

            LastPrice = depthMarketDataField.LastPrice;
            PresentAskPrice = depthMarketDataField.AskPrice1;
            PresentBidPrice = depthMarketDataField.BidPrice1;
            int tradedVolume = (depthMarketDataField.Volume - LastRawVolume) / 2;

            if (LastAskPrice.IsEqualTo(LastBidPrice) && LastBidPrice.IsEqualTo(0.0) && LastOldPrice.IsEqualTo(LastBidPrice))
            {
                LastAskPrice = depthMarketDataField.AskPrice1;
                LastBidPrice = depthMarketDataField.BidPrice1;
                LastRawVolume = depthMarketDataField.Volume;
                LastOldPrice = LastPrice;
                return false;
            }

            GetHandicapRatio(InstrumentId, depthMarketDataField.AskPrice1, depthMarketDataField.AskVolume1, depthMarketDataField.BidPrice1, depthMarketDataField.BidVolume1);
            if (TradingTimeManager.IsInEndingTime(InstrumentId, ExchangeId, PendingEndSeconds))
            {
                CancelStrategyInstrumentOrders();
            }
            else
            {
                CancelPendingOrders();
            }
            return true;
        }

        public void NotifyStrategyOrderStatusReceiver(CThostFtdcOrderField pOrder, bool isQry)
        {
            //TriggerMarkupOrder
            if (pOrder.InstrumentID == InstrumentId && pOrder.OrderStatus == EnumThostOrderStatusType.Canceled && !isQry)
            {
                HashSet<string> refRemoval = new HashSet<string>();
                if (PendingCancelOrderRef.Contains(pOrder.OrderRef) && Util.IsClientTagRef(pOrder.OrderRef, pOrder.UserProductInfo))
                {
                    string refindex = Util.GetRefIndexFromOrderRef(pOrder.OrderRef);
                    if (AlgoTrader.OrderRefToEntrust.ContainsKey(refindex))
                    {
                        NewEntrustInfo entrustInfo = AlgoTrader.OrderRefToEntrust[refindex];
                        if (entrustInfo.EntrustVolume > entrustInfo.TradedVolume && entrustInfo.EntrustVolume > 0)
                        {
                            ExecuteMarkupOrder(entrustInfo.EntrustDirection, (int)(entrustInfo.EntrustVolume - entrustInfo.TradedVolume), entrustInfo.OrderRef);
                            refRemoval.Add(pOrder.OrderRef);
                        }
                    }
                    else
                    {
                        Util.WriteError(string.Format("Wrong entrustinfo key: {0}", refindex));
                    }
                    PendingCancelOrderRef.RemoveElements(refRemoval.Contains, null);
                }
            }
        }

        private void GetHandicapRatio(string instrument, double askPrice, double askVol, double bidPrice, double bidVol)
        {
            LongHandicapRatio = ShortHandicapRatio = 0.0;
            if ((askPrice - bidPrice).IsGreaterThan(PriceTick))
            {
                LongHandicapRatio = ShortHandicapRatio = 0.0;
            }
            else if ((askPrice - bidPrice).Equals(PriceTick))
            {
                LongHandicapRatio = bidVol / askVol;
                ShortHandicapRatio = askVol / bidVol;
            }
#if DEBUG
            Util.WriteInfo(string.Format("InstrumentId {0}: LongHandicapRatio {1}, ShortHandicapRatio {2}, bidPrice {3}, bidVol {4}, askPrice {5}, askVol {6}"
                , InstrumentId, LongHandicapRatio, ShortHandicapRatio, bidPrice, bidVol, askPrice, askVol), Program.InPrintMode);
#endif
        }

        public bool ExecuteEntrustedOrders(string entrustDirection, int volume, string refIndex)
        {
            if (TradingTimeManager.IsInTradingTime(DateTime.Now, ExchangeId, InstrumentId, false) && UpLimitPrice.IsGreaterThan(0) && DownLimitPrice.IsGreaterThan(0))
            {
                if (entrustDirection == HSStruct.买入 && !Util.IsSentBuyOrder[InstrumentId])
                {
                    int hands = volume;
                    if (PresentBidPrice.IsGreaterThanOrEqualTo(UpLimitPrice))
                    {
                        int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Buy, 0, hands, false, refIndex, EnumOrderType.Market);
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
                                Util.WriteInfo(string.Format("Submit Up Market Order {0} ticks, {1} ns, {2}, {3}, {4}"
                                    , TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.MicrosecPerTick, EnumThostDirectionType.Buy, hands, 0));
                            }
                            return true;
                        }
                    }
                    else
                    {
                        Util.WriteInfo(string.Format("LongHandicapRatio: {0}", LongHandicapRatio));
                        if (LongHandicapRatio.IsLessThan(HandicapRatioThreshold))
                        {
                            double price = PresentAskPrice - PriceTick;
                            int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Buy, price, hands, false, refIndex);
                            if (st == 0)
                            {
                                TradingWatch.Stop();
                                string lastOrderRef = OrderManager.CombineOrderRef;
                                if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                                {
                                    Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                                }
                                ++LimitOrderCount;
                                if (!Util.IsInReLoadingStatus(InstrumentId))
                                {
                                    Util.WriteInfo(string.Format("Submit Pending Order {0} ticks, {1} ns, {2}, {3}, {4}"
                                        , TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.MicrosecPerTick, EnumThostDirectionType.Buy, hands, price));
                                }
                                return true;
                            }
                        }
                        else if (LongHandicapRatio.IsGreaterThanOrEqualTo(HandicapRatioThreshold))
                        {
                            int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Buy, 0, hands, false, refIndex, EnumOrderType.Market);
                            if (st == 0)
                            {
                                TradingWatch.Stop();
                                string lastOrderRef = OrderManager.CombineOrderRef;
                                if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                                {
                                    Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                                }
                                ++MarketOrderCount;
                                if (!Util.IsInReLoadingStatus(InstrumentId))
                                {
                                    Util.WriteInfo(string.Format("Submit Market Order {0} ticks, {1} ns, {2}, {3}, {4}"
                                        , TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.MicrosecPerTick, EnumThostDirectionType.Buy, hands, 0));
                                }
                                return true;
                            }
                        }
                    }
                }
                else if (entrustDirection == HSStruct.卖出 && !Util.IsSentSellOrder[InstrumentId])
                {
                    int hands = volume;
                    if (PresentAskPrice.IsLessThanOrEqualTo(DownLimitPrice))
                    {
                        int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Sell, 0, hands, false, refIndex, EnumOrderType.Market);
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
                                Util.WriteInfo(string.Format("Submit Down Market {0} ticks, {1} ns, {2}, {3}, {4}"
                                    , TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.MicrosecPerTick, EnumThostDirectionType.Sell, hands, 0));
                            }
                            return true;
                        }
                    }
                    else
                    {
                        Util.WriteInfo(string.Format("ShortHandicapRatio: {0}", ShortHandicapRatio));
                        if (ShortHandicapRatio.IsLessThan(HandicapRatioThreshold))
                        {
                            double price = PresentBidPrice + PriceTick;
                            int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Sell, price, hands, false, refIndex);
                            if (st == 0)
                            {
                                TradingWatch.Stop();
                                string lastOrderRef = OrderManager.CombineOrderRef;
                                if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                                {
                                    Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                                }
                                ++LimitOrderCount;
                                if (!Util.IsInReLoadingStatus(InstrumentId))
                                {
                                    Util.WriteInfo(string.Format("Submit Pending Order {0} ticks, {1} ns, {2}, {3}, {4}"
                                        , TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.MicrosecPerTick, EnumThostDirectionType.Sell, hands, price));
                                }
                                return true;
                            }
                        }
                        else if (ShortHandicapRatio.IsGreaterThanOrEqualTo(HandicapRatioThreshold))
                        {
                            int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Sell, 0, hands, false, refIndex, EnumOrderType.Market);
                            if (st == 0)
                            {
                                TradingWatch.Stop();
                                string lastOrderRef = OrderManager.CombineOrderRef;
                                if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                                {
                                    Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                                }
                                ++MarketOrderCount;
                                if (!Util.IsInReLoadingStatus(InstrumentId))
                                {
                                    Util.WriteInfo(string.Format("Submit Up Market Order {0} ticks, {1} ns, {2}, {3}, {4}"
                                        , TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.MicrosecPerTick, EnumThostDirectionType.Sell, hands, 0));
                                }
                                return true;
                            }
                        }
                    }
                }
            }
            else if (TradingTimeManager.IsInCycleTime())
            {
                Util.WriteInfo(string.Format("Current time: {0} for instruemnt: {1}", DateTime.Now.ToShortTimeString(), InstrumentId));
                return true; // Reserver for re-open
            }
            return false;
        }

        private bool ExecuteMarkupOrder(string entrustDirection, int volume, string refIndex)
        {
            if (TradingTimeManager.IsInTradingTime(DateTime.Now, ExchangeId, InstrumentId, false))
            {
                if (entrustDirection == HSStruct.买入 && !Util.IsSentBuyOrder[InstrumentId])
                {
                    int hands = volume;//
                    int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Buy, 0, hands, false, refIndex, EnumOrderType.Market);
                    if (st == 0)
                    {
                        TradingWatch.Stop();
                        string lastOrderRef = OrderManager.CombineOrderRef;
                        if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                        {
                            Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                        }
                        ++MarkupOrderCount;
                        if (Util.MarkupOrderReportDict[InstrumentId].ContainsKey(refIndex))
                        {
                            //Constant.MarkupOrderReportDict[InstrumentId][lastOrderRef].LimitOrderCount = LimitOrderCount;
                            //Constant.MarkupOrderReportDict[InstrumentId][lastOrderRef].MarkupCount = MarkupOrderCount;
                            Util.MarkupOrderReportDict[InstrumentId][refIndex].PriceTick = PriceTick;
                        }
                        if (!Util.IsInReLoadingStatus(InstrumentId))
                        {
                            Util.WriteInfo(string.Format("Submit Markup Order {0} ticks, {1} ns, {2}, {3}, {4}"
                                , TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.MicrosecPerTick, EnumThostDirectionType.Buy, hands, 0));
                        }
                        return true;
                    }

                }
                else if (entrustDirection == HSStruct.卖出 && !Util.IsSentSellOrder[InstrumentId])
                {
                    int hands = volume;
                    int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Sell, 0, hands, false, refIndex, EnumOrderType.Market);
                    if (st == 0)
                    {
                        TradingWatch.Stop();
                        string lastOrderRef = OrderManager.CombineOrderRef;
                        if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                        {
                            Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                        }
                        ++MarkupOrderCount;
                        if (Util.MarkupOrderReportDict[InstrumentId].ContainsKey(refIndex))
                        {
                            //Constant.MarkupOrderReportDict[InstrumentId][lastOrderRef].LimitOrderCount = LimitOrderCount;
                            //Constant.MarkupOrderReportDict[InstrumentId][lastOrderRef].MarkupCount = MarkupOrderCount;
                            Util.MarkupOrderReportDict[InstrumentId][refIndex].PriceTick = PriceTick;
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

        public void CancelPendingOrders()
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
                         (OrderManager.IsOrderOverTime(order, CancelPendingSec) || PresentBidPrice.IsGreaterThanOrEqualTo(order.LimitPrice + PriceTick) || PresentAskPrice.IsLessThanOrEqualTo(order.LimitPrice - PriceTick)))
                    {
                        Util.WriteInfo(string.Format("InstrumentId {0}: InsertTime {1}, LimitPrice {2}, bidPrice {3}, askPrice {4}"
                            , InstrumentId, order.InsertTime, order.LimitPrice, PresentBidPrice, PresentAskPrice), Program.InPrintMode);

                        if (Util.IsClientTagRef(order.OrderRef, order.UserProductInfo))
                        {
                            string refindex = Util.GetRefIndexFromOrderRef(order.OrderRef);
                            if (!Util.MarkupOrderReportDict.ContainsKey(InstrumentId))
                            {
                                Util.MarkupOrderReportDict[InstrumentId] = new ConcurrentDictionary<string, MarkupOrderStatistics>();
                            }
                            if (Util.MarkupOrderReportDict[InstrumentId].ContainsKey(refindex))
                            {
                                MarkupOrderStatistics mkup = Util.MarkupOrderReportDict[InstrumentId][refindex];
                                mkup.InstrumentID = InstrumentId;
                                mkup.LastCancelledPrice = order.LimitPrice;
                            }
                            else
                            {
                                Util.MarkupOrderReportDict[InstrumentId][refindex] = new MarkupOrderStatistics()
                                {
                                    InstrumentID = InstrumentId,
                                    LastCancelledPrice = order.LimitPrice
                                };
                            }
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

        private void CancelStrategyInstrumentOrders()
        {
            if (AlgoTrader.InstrumentToPendingOrders.ContainsKey(InstrumentId))
            {
                foreach (string key in AlgoTrader.InstrumentToPendingOrders[InstrumentId].Keys)
                {
                    CThostFtdcOrderField order = AlgoTrader.InstrumentToPendingOrders[InstrumentId][key];
                    if (OrderManager.IsCancellable(order))
                    {
                        OrderManager.CancelOrder(order);
                    }
                }
            }
        }

        public void NotifyStrategyFilledReceiver(CThostFtdcTradeField pTrade)
        {
            if (Util.IsClientTagRef(pTrade.OrderRef, string.Empty))
            {
                string refIndex = Util.GetRefIndexFromOrderRef(pTrade.OrderRef);
                ProcessMarkupOrdersStatictics(refIndex);
            }
        }

        public void ProcessMarkupOrdersStatictics(string refIndex)
        {
            if (Util.MarkupOrderReportDict.ContainsKey(InstrumentId) && Util.MarkupOrderReportDict[InstrumentId].ContainsKey(refIndex))
            {
                ConcurrentDictionary<string, MarkupOrderStatistics> markupDict = Util.MarkupOrderReportDict[InstrumentId];
                int strategyIntendedVolume = (from x in markupDict.Values select x.TradedVolume).Sum();
                double avgCost = strategyIntendedVolume == 0 ? 0 : (from x in markupDict.Values select x.TradedUnitCost * x.TradedVolume).Sum() / strategyIntendedVolume;
                double gainLoss = (from x in markupDict.Values select LimitOrderCount * x.PriceTick - MarkupOrderCount * (x.PriceTick + avgCost) * x.PriceTick).Sum();
                string deleteCommand = string.Format(@"delete from [tradedb].[dbo].[tb_TwapStatistics] where [LocalUpdateTime] in (select top 1 [LocalUpdateTime] from [tradedb].[dbo].[tb_TwapStatistics] where [TradingDay] = '{0}' and  [InstrumentID] = '{1}' order by [LocalUpdateTime] desc) "
                    , TradingTimeManager.TradingDay.ToString("yyyyMMdd"), InstrumentId);
                string insertCommand = @"Insert into [tradedb].[dbo].[tb_TwapStatistics] ([TradingDay],[InstrumentID],[EntrustSumVolume],[HandicapRatio],[PendingSeconds],[LimitOrderCount],[MarketOrderCount],[MarkupOrderCount],[AvgCost],[GainLoss])
                    values ('{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9})";
                insertCommand = string.Format(insertCommand, TradingTimeManager.TradingDay.ToString("yyyyMMdd"), InstrumentId, strategyIntendedVolume, HandicapRatioThreshold, CancelPendingSec,
                    LimitOrderCount, MarketOrderCount, MarkupOrderCount, avgCost, gainLoss);

                using (SqlConnection sqlconn = new SqlConnection(Util.ConnectionStr))
                {
                    sqlconn.Open();
                    if (sqlconn.State == ConnectionState.Open)
                    {
                        using (SqlTransaction transaction = sqlconn.BeginTransaction())
                        {
                            try
                            {
                                if (_StaticticsLaunched)
                                {
                                    Util.WriteInfo(string.Format("执行更新: {0}", deleteCommand));
                                    var deleteCmd = new SqlCommand
                                    {
                                        Connection = sqlconn,
                                        CommandType = CommandType.Text,
                                        CommandText = deleteCommand,
                                        Transaction = transaction
                                    };
                                    deleteCmd.ExecuteNonQuery();
                                }
                                Util.WriteInfo(string.Format("执行插入: {0}", insertCommand));
                                var insertCmd = new SqlCommand
                                {
                                    Connection = sqlconn,
                                    CommandType = CommandType.Text,
                                    CommandText = insertCommand,
                                    Transaction = transaction
                                };
                                insertCmd.ExecuteNonQuery();
                                transaction.Commit();
                                _StaticticsLaunched = true;
                            }
                            catch (Exception ex)
                            {
                                Util.WriteExceptionToLogFile(ex);
                                transaction.Rollback();
                            }
                        }
                    }
                }
            }
        }
    }
}
