using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace AutoTrader
{
    public class AlgorithmicTrader
    {
        public ConcurrentDictionary<string, NewEntrustInfo> OrderRefToEntrust = new ConcurrentDictionary<string, NewEntrustInfo>();
        public ConcurrentDictionary<string, Dictionary<string, CThostFtdcOrderField>> InstrumentToPendingOrders = new ConcurrentDictionary<string, Dictionary<string, CThostFtdcOrderField>>();
        private ConcurrentDictionary<string, Dictionary<string, CThostFtdcOrderField>> RefIndexToOrders = new ConcurrentDictionary<string, Dictionary<string, CThostFtdcOrderField>>();
        private List<NewEntrustInfo> NewEntrusts = new List<NewEntrustInfo>();
        private ConcurrentDictionary<string, double> TradingDayToOrderTimeNum = new ConcurrentDictionary<string, double>();
        private ConcurrentDictionary<string, InvestorId_To_FundName> InvestorIdToFundName = new ConcurrentDictionary<string, InvestorId_To_FundName>();
        private HashSet<string> ScrappedEntrustSet = new HashSet<string>();
        private ConcurrentDictionary<string, string> SecuritiesForbiddenMap = new ConcurrentDictionary<string, string>();
        private ConcurrentDictionary<int, double> ParentRefToCommitCount = new ConcurrentDictionary<int, double>();
        private string _OrderBookTableName = "tb_OrderBook";
        private int _LastRefIndex = 0;// int.Parse(ConfigurationManager.AppSettings["LastOrderRef"]);

        double Interval = double.Parse(ConfigurationManager.AppSettings["IntervalSecond"]);
        private TradeDataServer DataServer;
        private Task _AlgoTask;
        private Task _OrderTask;
        private HashSet<string> _InstrumentIds = new HashSet<string>();

        public AlgorithmicTrader(TradeDataServer tradeDataServer)
        {
            DataServer = tradeDataServer;
            tradeDataServer.RtnNotifyOrderStatus += NotifyOrderStatusReceiver;
            tradeDataServer.RtnNotifyFilled += NotifyFilledReceiver;
        }

        public void Launch()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            _AlgoTask = Task.Factory.StartNew(() => AlgoEntrustReceiver(cts.Token), cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            Thread.Sleep((int)Math.Max(3, (1000 * Interval / 2)));
            _OrderTask = Task.Factory.StartNew(() => EntrustedOrderReceiver(cts.Token), cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public void AlgoEntrustReceiver(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                if (TradingTimeManager.IsAllEndTime())
                {
                    break;
                }
                bool isInited = _InstrumentIds.Count > 0;
                List<string> reqLst = new List<string>();
                using (SqlConnection conn = new SqlConnection(Util.ConnectionStr))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter())
                    {
                        DataTable tbTwapSetting = new DataTable();
                        string commandText = string.Format(@"SELECT a.[AssetCode], a.[HandicapRatio], a.[PendingSeconds] FROM [tradedb].[dbo].[tb_TwapSettings] a 
                                                WHERE a.[AssetCode] IN (SELECT b.[AssetCode] FROM [tradedb].[dbo].[tb_OrderBook] b
                                                WHERE b.[TradingDay] = '{0}' AND b.[AssetType] in ('Futures') AND b.[OrderType] = 'TWAP1' and b.[Volume] <> 0)", TradingTimeManager.TradingDay.ToString("yyyyMMdd"));
                        adapter.SelectCommand = new SqlCommand(commandText, conn);
                        adapter.SelectCommand.CommandTimeout = 60;
                        Util.WriteInfo(commandText, Program.InPrintMode);
                        adapter.Fill(tbTwapSetting);
                        foreach (DataRow row in tbTwapSetting.Rows)
                        {
                            string instrumentId = NewEntrustInfo.GetExchangeInstrumentId(Convert.ToString(row["AssetCode"]));
                            string handicapRatio = Convert.ToString(row["HandicapRatio"]);
                            string pendingSeconds = Convert.ToString(row["PendingSeconds"]);
                            // Update parameter continuously
                            Util.StrategyParameter[instrumentId] = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();
                            Util.StrategyParameter[instrumentId]["TwapAdvanced"] = new ConcurrentDictionary<string, string>();
                            Util.StrategyParameter[instrumentId]["TwapAdvanced"]["Value"] = "1"; //全天有效
                            Util.StrategyParameter[instrumentId]["TwapAdvanced"]["HandicapRatio"] = handicapRatio;
                            Util.StrategyParameter[instrumentId]["TwapAdvanced"]["PendingSeconds"] = pendingSeconds;
                            if (!_InstrumentIds.Contains(instrumentId))
                            {
                                reqLst.Add(instrumentId);
                                _InstrumentIds.Add(instrumentId);
                            }
                        }
                    }
                }
                if (reqLst.Count > 0)
                {
                    DataServer.LaunchStrategies(reqLst, isInited);
                }
                Thread.Sleep((int)(1000 * Interval));
            }
        }

        public void EntrustedOrderReceiver(CancellationToken ct)
        {
            using (SqlConnection conn = new SqlConnection(Util.ConnectionStr))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    DataTable listTradingCalendar = new DataTable();
                    string commandText = string.Format("SELECT * FROM [SharedDb].[dbo].[{0}]", "TradingCalendar");
                    adapter.SelectCommand = new SqlCommand(commandText, conn);
                    adapter.SelectCommand.CommandTimeout = 3600;
                    Util.WriteInfo(commandText, Program.InPrintMode);
                    adapter.Fill(listTradingCalendar);
                    foreach (DataRow row in listTradingCalendar.Rows)
                    {
                        var tradingDay = Convert.ToString(row["TRADE_DAYS"]);
                        var dataNumber = Convert.ToDouble(row["DATE_NUMBER"]);

                        if (!TradingDayToOrderTimeNum.ContainsKey(tradingDay))
                        {
                            TradingDayToOrderTimeNum[tradingDay] = dataNumber;
                        }
                    }
                }
            }
            using (SqlConnection conn = new SqlConnection(Util.ConnectionStr))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    DataTable listInvestorId_To_FundName = new DataTable();
                    string commandText = string.Format("SELECT * FROM [tradeDb].[dbo].[{0}]", "InvestorId_To_FundName");
                    adapter.SelectCommand = new SqlCommand(commandText, conn);
                    adapter.SelectCommand.CommandTimeout = 3600;
                    Util.WriteInfo(commandText, Program.InPrintMode);
                    adapter.Fill(listInvestorId_To_FundName);
                    foreach (DataRow ITF in listInvestorId_To_FundName.Rows)
                    {
                        var investorId_To_FundName = new InvestorId_To_FundName
                        {
                            InvestorID = Convert.ToString(ITF["InvestorID"]),
                            FundName = Convert.ToString(ITF["FundName"]),
                            AccountType = Convert.ToString(ITF["AccountType"])
                        };
                        InvestorIdToFundName[investorId_To_FundName.InvestorID] = investorId_To_FundName;
                    }
                }
            }
            bool isFirstLoad = true;
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    if (TradingTimeManager.IsAllEndTime())
                    {
                        //ProcessGlobalMarkupStatistics();
                        break;
                    }
                    //Get Orders
                    if (TradingTimeManager.IsInCycleTime())
                    {
                        if (isFirstLoad)
                        {
                            AddEntrusts(true);
                            isFirstLoad = false;
                        }
                        else
                        {
                            RemoveTradedEntrusts();
                            AddEntrusts();
                        }
                        Util.WriteInfo(string.Format("数据库中有委托数量{0}笔...", NewEntrusts.Count));
                        EntrustPerformer();
                    }
                    if (TradingTimeManager.IsAllBreakTime(300))
                    {
                        Thread.Sleep((int)(20 * 1000 * Interval));
                    }
                    else
                    {
                        Thread.Sleep((int)(1000 * Interval));
                    }
                }
                catch (Exception ex)
                {
                    Util.WriteExceptionToLogFile(ex);
                }
            }
            Util.WriteInfo("OrderTask Loop End");
        }

        private void AddEntrusts(bool isFirstLoad = false)
        {
            int beginRef = _LastRefIndex;
            using (SqlConnection conn = new SqlConnection(Util.ConnectionStr))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    DataTable listOrderBook = new DataTable();
                    string commandText = string.Format(@"SELECT * FROM [tradedb].[dbo].[{0}] WHERE [TradingDay] = '{1}' AND [InvestorID] = '{2}' AND [AssetType] in ('Futures') 
                                        AND [RefIndex] > {3} and [OrderType] = 'DEFAULT1' and [Volume] <> 0 and ([FeedBack] is null or [FeedBack] = 'ACCEPTED') order by [RefIndex],[TradingDay] desc"
                                        , _OrderBookTableName, TradingTimeManager.TradingDay.ToString("yyyyMMdd"), DataServer._CtpTraderApi.InvestorID, _LastRefIndex);
                    if (isFirstLoad)
                    {
                        commandText = string.Format(@"SELECT * FROM [tradedb].[dbo].[{0}] WHERE [TradingDay] = '{1}' AND [InvestorID] = '{2}' AND [AssetType] in ('Futures') 
                                        and [OrderType] = 'DEFAULT1' and Volume <> 0 and ([FeedBack] is null or [FeedBack] = 'ACCEPTED') order by [RefIndex]"
                                        , _OrderBookTableName, TradingTimeManager.TradingDay.ToString("yyyyMMdd"), DataServer._CtpTraderApi.InvestorID, _LastRefIndex);
                    }
                    adapter.SelectCommand = new SqlCommand(commandText, conn);
                    adapter.SelectCommand.CommandTimeout = 3600;
                    Util.WriteInfo(commandText, Program.InPrintMode);
                    adapter.Fill(listOrderBook);
                    foreach (DataRow row in listOrderBook.Rows)
                    {
                        try
                        {
                            int orderRef = Convert.ToInt32(row["RefIndex"]);
                            if (orderRef > _LastRefIndex)
                            {
                                _LastRefIndex = orderRef;
                            }
                            string instrumentIdDotExchangeCode = Convert.ToString(row["AssetCode"]).Trim();
                            string instrumentId = NewEntrustInfo.GetExchangeInstrumentId(instrumentIdDotExchangeCode);// instrumentIdDotExchangeCode.Substring(0, instrumentIdDotExchangeCode.IndexOf('.'));
                            if (!_InstrumentIds.Contains(instrumentId))
                            {
                                Util.WriteError(string.Format("{0} quote hasn't been subscribed!", instrumentId));
                                continue;
                            }
                            string tradingDay = Convert.ToString(row["TradingDay"]).Trim();
                            string calendarDate = Convert.ToString(row["CalendarDate"]).Trim();
                            int volume = Convert.ToInt32(row["Volume"]);
                            double price = Convert.ToDouble(row["OrderPrice"]);
                            string exchangeCode = Convert.ToString(row["ExchangeCode"]).Trim();

                            string managerName = Convert.ToString(row["ManagerName"]).Trim();
                            string fundName = Convert.ToString(row["FundName"]).Trim();
                            string strategy = ((string)row["Strategy"]).Trim().Trim();
                            string researcher = ((string)row["Researcher"]).Trim();
                            string assetType = ((string)row["AssetType"]).Trim();
                            string investorId = ((string)row["InvestorID"]).Trim();
                            string brokerId = null;
                            if (investorId.Length > 4)
                            {
                                brokerId = investorId.Substring(0, 4);
                                investorId = investorId.Substring(4);
                            }
                            int parentRef = row["ParentRef"] == DBNull.Value ? 0 : Convert.ToInt32(row["ParentRef"]);

                            string market = exchangeCode;
                            string feedback = row["Feedback"] != null ? Convert.ToString(row["Feedback"]).Trim().ToUpper() : "";
                            NewEntrustInfo newEntrust = new NewEntrustInfo
                            {
                                TradingDay = tradingDay,
                                CalendarDate = calendarDate,
                                InstrumentId = instrumentId,
                                MarketType = market,
                                ManagerName = managerName,
                                FundName = fundName,
                                Strategy = strategy,
                                Researcher = researcher,
                                AssetType = assetType,
                                AssetCode = instrumentIdDotExchangeCode,
                                ExchangeId = exchangeCode,
                                BrokerId = brokerId,
                                InvestorId = investorId,
                                EntrustDirection = GetEntrustDirection(assetType, volume),
                                EntrustVolume = Math.Abs(volume),
                                OriginalEntrustPrice = price,
                                InvestType = "投机",
                                OrderRef = orderRef.ToString(),
                                ParentRef = parentRef,
                                Status = feedback
                            };

                            if (isFirstLoad && RefIndexToOrders.ContainsKey(newEntrust.OrderRef))
                            {
                                //newEntrust.TradedVolume = (from x in RefIndexToOrders[newEntrust.OrderRef] select x.Value).Sum((orderItem) => orderItem.VolumeTraded);
                                newEntrust.TradedVolume = (from x in RefIndexToOrders[newEntrust.OrderRef] select x.Value.VolumeTraded).Sum();
                            }

                            if (parentRef > 0 && ParentRefToCommitCount.ContainsKey(parentRef) && ParentRefToCommitCount[parentRef] > 0)
                            {
                                double parentLeftVolume = ParentRefToCommitCount[parentRef];
                                newEntrust.EntrustVolume += parentLeftVolume;
                                ParentRefToCommitCount.TryRemove(parentRef, out parentLeftVolume);
                                Util.WriteInfo(string.Format("批次委托修正{0} {1}: {2} 股", instrumentIdDotExchangeCode, parentRef, parentLeftVolume));
                            }
                            if (string.IsNullOrEmpty(newEntrust.EntrustDirection))
                            {
                                Util.WriteError(string.Format("未定义委托引用编号{0}的委托方向{1}...", newEntrust.OrderRef, newEntrust.EntrustDirection));
                            }
                            else if (newEntrust.EntrustVolume > newEntrust.TradedVolume)
                            {
                                OrderRefToEntrust[newEntrust.OrderRef] = newEntrust;
                                NewEntrusts.Add(newEntrust);
                                UpdateFeedBack(newEntrust, EnumProcessingType.ACCEPTED);
                            }
                            else if (newEntrust.Status != EnumProcessingType.TRADED.ToString()
                                && newEntrust.EntrustVolume == newEntrust.TradedVolume && newEntrust.TradedVolume > 0)
                            {
                                UpdateFeedBack(newEntrust, EnumProcessingType.TRADED);
                            }
                        }
                        catch (Exception ex)
                        {
                            Util.WriteExceptionToLogFile(ex);
                        }
                    }
                    //if (beginRef < _LastRefIndex)
                    //{
                    //    UpdateFeedBack(beginRef, _LastRefIndex, EnumProcessingType.ACCEPTED);
                    //}
                }
            }
        }
        private void RemoveTradedEntrusts()
        {
            for (int i = NewEntrusts.Count - 1; i >= 0; --i)
            {
                try
                {
                    //double posAmt = 0.0;
                    //if (T2TraderApi.StocksDict.ContainsKey(NewEntrusts[i].InstrumentId))
                    //{
                    //    posAmt = T2TraderApi.StocksDict[NewEntrusts[i].InstrumentId].可卖股票;
                    //}
                    double remainingVolume = NewEntrusts[i].EntrustVolume > NewEntrusts[i].TradedVolume ? NewEntrusts[i].EntrustVolume - NewEntrusts[i].TradedVolume : 0.0;
                    //bool isDumpPosition = Utils.IsCloseDirection(NewEntrusts[i].EntrustDirection) && posAmt == remainingVolume;
                    //double remainingTotalPrice = remainingVolume * (NewEntrusts[i].AssetType == "Repo" ? 100 : NewEntrusts[i].EntrustPrice);
                    //if (SecuritiesForbiddenMap.ContainsKey(NewEntrusts[i].AssetCode) && SecuritiesForbiddenMap[NewEntrusts[i].AssetCode] == NewEntrusts[i].EntrustDirection)
                    //{
                    //    Constant.WriteInfo(string.Format("自成交限制, 保留委托标的 {0}, RefIndex {1}的信息", NewEntrusts[i].AssetCode, NewEntrusts[i].OrderRef));
                    //}
                    //else 
                    if (NewEntrusts[i].Status == EnumProcessingType.CANCELLED.ToString()
                        || !string.IsNullOrEmpty(NewEntrusts[i].Status) && remainingVolume.IsLessThanOrEqualTo(0.0)
                        //remainingTotalPrice.IsLessThanOrEqualTo(0.0)
                        //|| ScrappedEntrustSet.Contains(NewEntrusts[i].OrderRef) //废单移除
                        //|| Utils.IsOpenDirection(NewEntrusts[i].EntrustDirection) && NewEntrusts[i].MultiFactor > 0 && remainingVolume < NewEntrusts[i].MultiFactor
                        //|| !isDumpPosition && NewEntrusts[i].MultiFactor > 0 && remainingVolume < NewEntrusts[i].MultiFactor
                        //|| NewEntrusts[i].AssetType == "Stock" && !isDumpPosition && Constant.对手盘口量比率 < 1 && Constant.对手盘口量比率 > 0 && (remainingTotalPrice * Constant.预设股票手续费率).IsLessThan(Constant.最小手续费单价)
                        )
                    {
                        if (remainingVolume > 0 && NewEntrusts[i].ParentRef > 0)
                        {
                            ParentRefToCommitCount[NewEntrusts[i].ParentRef] = remainingVolume;
                            Util.WriteInfo(string.Format("批次余股{0} {1}: {2} 股", NewEntrusts[i].AssetCode, NewEntrusts[i].ParentRef, remainingVolume));
                        }
                        if (NewEntrusts[i].TradedVolume > 0 && NewEntrusts[i].Status != EnumProcessingType.CANCELLED.ToString())
                        {
                            UpdateFeedBack(NewEntrusts[i], EnumProcessingType.TRADED);
                        }
                        else
                        {
                            UpdateFeedBack(NewEntrusts[i], EnumProcessingType.CANCELLED);
                        }
                        Util.WriteInfo(string.Format("移除委托标的 {0}, RefIndex {1}的信息", NewEntrusts[i].AssetCode, NewEntrusts[i].OrderRef));
                        NewEntrusts.RemoveAt(i);
                    }
                }
                catch (Exception ex)
                {
                    Util.WriteExceptionToLogFile(ex);
                }
            }
        }

        private void EntrustPerformer()
        {
            foreach (NewEntrustInfo entrust in NewEntrusts)
            {
                if (Util.StrategyMap.ContainsKey(entrust.InstrumentId))
                {
                    foreach (string sKey in Util.StrategyMap[entrust.InstrumentId].Keys)
                    {
                        StrategyControl strategy = Util.StrategyMap[entrust.InstrumentId]["TwapAdvanced"];
                        if (strategy is StrategyTwapEnhanced)
                        {
                            if (entrust.EntrustVolume.IsGreaterThan(entrust.TradedVolume) && entrust.Status == EnumProcessingType.ACCEPTED.ToString())
                            {
                                bool result = (strategy as StrategyTwapEnhanced).ExecuteEntrustedOrders(entrust.EntrustDirection, (int)(entrust.EntrustVolume - entrust.TradedVolume), entrust.OrderRef);
                                if (!result)
                                {
                                    UpdateFeedBack(entrust, EnumProcessingType.CANCELLED);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void RebuildMarkupOrderReport()
        {
            foreach (string instrumentId in InstrumentToPendingOrders.Keys)
            {
                double priceTick = 0;
                if (DataServer.InstrumentFields.ContainsKey(instrumentId))
                {
                    priceTick = DataServer.InstrumentFields[instrumentId].PriceTick;
                }
                if (DataServer.DepthMarketDataFields.ContainsKey(instrumentId))
                {
                    CThostFtdcDepthMarketDataField mdField = DataServer.DepthMarketDataFields[instrumentId];
                    if (!Util.MarkupOrderReportDict.ContainsKey(instrumentId))
                    {
                        Util.MarkupOrderReportDict[instrumentId] = new ConcurrentDictionary<string, MarkupOrderStatistics>();
                    }
                    foreach (string orderRef in InstrumentToPendingOrders[instrumentId].Keys)
                    {
                        CThostFtdcOrderField orderField = InstrumentToPendingOrders[instrumentId][orderRef];
                        if (Util.IsClientTagRef(orderField.OrderRef, orderField.UserProductInfo))
                        {
                            string refIndex = Util.GetRefIndexFromOrderRef(orderField.OrderRef);
                            var refOrderTradedCollection = from tradeField in DataServer.TradedFields.Values where tradeField.OrderRef == orderRef select tradeField;
                            if (orderField.LimitPrice == mdField.UpperLimitPrice && orderField.Direction == EnumThostDirectionType.Buy
                             && orderField.LimitPrice == mdField.LowerLimitPrice && orderField.Direction == EnumThostDirectionType.Sell)
                            {
                                MarkupOrderStatistics markUpField = new MarkupOrderStatistics();
                                markUpField.CommitVolume = orderField.VolumeTotal;
                                //markUpField.CancelledPrice = orderField.LimitPrice;
                                markUpField.InstrumentID = instrumentId;
                                //markUpField.OrderRef = orderField.OrderRef;
                                markUpField.PriceTick = priceTick;
                                markUpField.LastTradedPrice = refOrderTradedCollection.Sum(item => item.Price * item.Volume) / refOrderTradedCollection.Sum(item => item.Volume);
                                markUpField.TradedVolume = orderField.VolumeTraded;
                                markUpField.TradedUnitCost += (Math.Abs(markUpField.LastTradedPrice - markUpField.LastCancelledPrice) - markUpField.PriceTick);
                                Util.MarkupOrderReportDict[instrumentId][refIndex] = markUpField;
                            }
                        }
                    }
                }

            }
        }

        private string GetEntrustDirection(string assetType, int volume)
        {
            switch (assetType)
            {
                case "Bond":
                    return volume > 0 ? HSStruct.债券买入 : HSStruct.债券卖出;
                case "Futures":
                case "Option":
                    return volume > 0 ? HSStruct.买入 : HSStruct.卖出;
                case "Fund":
                case "Stock":
                    return volume > 0 ? HSStruct.股票买入 : HSStruct.股票卖出;
                case "Repo":
                    return volume > 0 ? HSStruct.卖出 : "";
                default:
                    return "";
            }
        }

        private void UpdateFeedBack(int beginRef, int endRef, EnumProcessingType status)
        {
            var updateFormat = @"update [tradedb].[dbo].[tb_OrderBook] set [Feedback] = '{0}' where [RefIndex] > {1} and [RefIndex] <= {2} and [TradingDay] = '{3}' and [Feedback] is null and [OrderType] = 'DEFAULT1'";
            using (SqlConnection sqlconn = new SqlConnection(Util.ConnectionStr))
            {
                sqlconn.Open();
                if (sqlconn.State == ConnectionState.Open)
                {
                    var command = string.Format(updateFormat, status, beginRef, endRef, TradingTimeManager.TradingDay.ToString("yyyyMMdd"));
                    var com = new SqlCommand
                    {
                        Connection = sqlconn,
                        CommandType = CommandType.Text,
                        CommandText = command
                    };
                    Util.WriteInfo(string.Format("执行更新:{0}", command));
                    com.ExecuteNonQuery();
                }
            }
        }

        private void UpdateFeedBack(NewEntrustInfo entrust, EnumProcessingType status)
        {
            string refIndex = entrust.OrderRef;
            var updateFormat = @"update [tradedb].[dbo].[tb_OrderBook] set [Feedback] = '{0}' where [RefIndex] = {1} and [TradingDay] = '{2}' and [OrderType] = 'DEFAULT1'";
            using (SqlConnection sqlconn = new SqlConnection(Util.ConnectionStr))
            {
                sqlconn.Open();
                if (sqlconn.State == ConnectionState.Open)
                {
                    var command = string.Format(updateFormat, status, refIndex, TradingTimeManager.TradingDay.ToString("yyyyMMdd"));
                    var com = new SqlCommand
                    {
                        Connection = sqlconn,
                        CommandType = CommandType.Text,
                        CommandText = command
                    };
                    Util.WriteInfo(string.Format("执行更新:{0}", command));
                    com.ExecuteNonQuery();
                }
            }
            entrust.Status = status.ToString();
        }

        public void NotifyOrderStatusReceiver(CThostFtdcOrderField pOrder, bool isQry)
        {
            if (OrderManager.IsCancellable(pOrder))
            {
                if (!InstrumentToPendingOrders.ContainsKey(pOrder.InstrumentID))
                {
                    InstrumentToPendingOrders[pOrder.InstrumentID] = new Dictionary<string, CThostFtdcOrderField>();
                }
                InstrumentToPendingOrders[pOrder.InstrumentID][pOrder.OrderRef] = pOrder;
            }
            else
            {
                if (InstrumentToPendingOrders.ContainsKey(pOrder.InstrumentID) && InstrumentToPendingOrders[pOrder.InstrumentID].ContainsKey(pOrder.OrderRef))
                {
                    InstrumentToPendingOrders[pOrder.InstrumentID].Remove(pOrder.OrderRef);
                }
            }
            if (Util.IsClientTagRef(pOrder.OrderRef, pOrder.UserProductInfo))
            {
                string refindex = Util.GetRefIndexFromOrderRef(pOrder.OrderRef);//.Substring(0, pOrder.OrderRef.Length - ReferenceLength);
                if (!RefIndexToOrders.ContainsKey(refindex))
                {
                    RefIndexToOrders[refindex] = new Dictionary<string, CThostFtdcOrderField>();
                }
                if (RefIndexToOrders[refindex].ContainsKey(pOrder.OrderRef))
                {
                    RefIndexToOrders[refindex][pOrder.OrderRef] = pOrder;
                }
                else
                {
                    RefIndexToOrders[refindex].Add(pOrder.OrderRef, pOrder);
                }

                //if (isQry)
                //{
                //    int maxRef = int.Parse(Constant.GetMaxRefFromOrderRef(pOrder.OrderRef));
                //    if (!Constant.ClientTagToMaxOrderRefMap.ContainsKey(refindex)
                //      || Constant.ClientTagToMaxOrderRefMap.ContainsKey(refindex) && maxRef > Constant.ClientTagToMaxOrderRefMap[refindex])
                //    {
                //        Constant.ClientTagToMaxOrderRefMap[refindex] = maxRef;
                //    }
                //}
            }
        }

        public void NotifyFilledReceiver(CThostFtdcTradeField pTrade)
        {
            InsertTrade(pTrade);
        }

        private void InsertTrade(CThostFtdcTradeField pTrade)
        {
            try
            {
                var insertFormat = "INSERT INTO [tradedb].[dbo].[TB_Pending]" +
                    " ([DateStr],[OrderTimeNum],[ManagerName],[FundName],[Strategy]," +
                    "[Researcher],[AssetType],[RawCode],[AssetName],[WindCode],[WindName]," +
                    "[ExchangeName],[Price],[Volume],[Amt],[Cost],[MoneyIn],[MultiFactor]," +
                    "[RefIndex],[Comments]) VALUES ('{0}',{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}'," +
                    "'{10}','{11}','{12}',{13},{14},{15},{16},{17},{18},'{19}')";

                using (SqlConnection sqlconn = new SqlConnection(Util.ConnectionStr))
                {
                    sqlconn.Open();
                    if (sqlconn.State == ConnectionState.Open)
                    {
                        string refIndex = Util.GetRefIndexFromOrderRef(pTrade.OrderRef);//T2TraderApi.EntrustNoToStockEntrusts[stockTrade.委托编号].报单引用;
                        if (string.IsNullOrEmpty(refIndex) || !OrderRefToEntrust.ContainsKey(refIndex))
                        {
                            //Constant.WriteError(string.Format("未找到委托编号{0}的数据库报单条目...", pTrade.OrderRef));
                            return;
                        }

                        var entrustInfo = OrderRefToEntrust[refIndex];
                        var volume = pTrade.Volume;
                        if (pTrade.Direction == EnumThostDirectionType.Buy)//(Constant.IsOpenDirection(stockTrade.委托方向))
                        {
                            volume = pTrade.Volume;
                        }
                        else if (pTrade.Direction == EnumThostDirectionType.Sell)//(Constant.IsCloseDirection(stockTrade.委托方向))
                        {
                            volume = -pTrade.Volume;
                        }
                        else
                        {
                            Util.WriteError(string.Format("未定义委托编号{0}的委托方向{1}...", pTrade.OrderRef, pTrade.Direction));
                        }
                        entrustInfo.TradedVolume += Math.Abs(volume);
                        Debug.Assert(entrustInfo.TradedVolume >= 0);

                        //手续费
                        var amount = volume * pTrade.Price;
                        var cost = 0.0;
                        var multiFactor = 1;
                        if (DataServer.InstrumentFields.ContainsKey(pTrade.InstrumentID))
                        {
                            int hycs = DataServer.InstrumentFields[pTrade.InstrumentID].VolumeMultiple;
                            string spec = DataServer.InstrumentFields[pTrade.InstrumentID].ProductID;
                            CThostFtdcInstrumentCommissionRateField commRateField = new CThostFtdcInstrumentCommissionRateField();// = null;
                            if (DataServer.InstrumentCategoryCommissionRateDic.ContainsKey(pTrade.InstrumentID))
                            {
                                commRateField = DataServer.InstrumentCategoryCommissionRateDic[pTrade.InstrumentID];
                            }
                            else if (DataServer.InstrumentCategoryCommissionRateDic.ContainsKey(spec))
                            {
                                commRateField = DataServer.InstrumentCategoryCommissionRateDic[spec];
                            }
                            //if (commRateField != null)
                            {
                                double commissionRate = commRateField.OpenRatioByMoney * pTrade.Price + commRateField.OpenRatioByVolume;
                                if (pTrade.OffsetFlag == EnumThostOffsetFlagType.Open)
                                {
                                    commissionRate = commRateField.OpenRatioByMoney * pTrade.Price + commRateField.OpenRatioByVolume;
                                }
                                else if (pTrade.OffsetFlag == EnumThostOffsetFlagType.CloseToday) //cannot know if closed position is today's position
                                {
                                    commissionRate = commRateField.CloseTodayRatioByMoney * pTrade.Price + commRateField.CloseTodayRatioByVolume;
                                }
                                else if (pTrade.OffsetFlag == EnumThostOffsetFlagType.Close || pTrade.OffsetFlag == EnumThostOffsetFlagType.CloseYesterday)
                                {
                                    commissionRate = commRateField.CloseRatioByMoney * pTrade.Price + commRateField.CloseRatioByVolume;
                                }
                                //FixTradedCost[pTrade.InstrumentID] += commissionRate * pTrade.Volume;
                                cost = commissionRate * hycs * pTrade.Volume;
                            }
                            multiFactor = hycs;
                        }
                        var moneyIn = amount + cost;

                        var command = string.Format(insertFormat, pTrade.TradingDay, TradingDayToOrderTimeNum[pTrade.TradingDay], entrustInfo.ManagerName,
                            entrustInfo.FundName, entrustInfo.Strategy, entrustInfo.Researcher, entrustInfo.AssetType,
                            entrustInfo.InstrumentId, pTrade.TradeID.Trim(), entrustInfo.AssetCode, "", entrustInfo.ExchangeId, pTrade.Price,
                            volume, amount, cost, moneyIn, multiFactor, refIndex, "");//pTrade.TradingDay format??

                        var com = new SqlCommand
                        {
                            Connection = sqlconn,
                            CommandType = CommandType.Text,
                            CommandText = command
                        };

                        Util.WriteInfo(string.Format("执行插入:{0}", command));
                        com.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.WriteExceptionToLogFile(ex);
            }
        }

        //private void ProcessGlobalMarkupStatistics()
        //{
        //    foreach (string instrumentId in Constant.StrategyMap.Keys)
        //    {
        //        if (Constant.StrategyMap[instrumentId].ContainsKey("TwapAdvanced"))
        //        {
        //            StrategyControl strategy = Constant.StrategyMap[instrumentId]["TwapAdvanced"];
        //            if (strategy is StrategyTwapEnhanced)
        //            {
        //                (strategy as StrategyTwapEnhanced).ProcessMarkupOrdersStatictics(Constant.ConnectionStr);
        //            }
        //        }
        //    }
        //}
    }

    public struct InvestorId_To_FundName
    {
        public string InvestorID;
        public string FundName;
        public string AccountType;
    }
}
