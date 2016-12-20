using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using TradingMaster.CodeSet;

namespace TradingMaster
{
    public class BackgroundDataServer
    {
        internal HashSet<Contract> _CodesList;
        internal BackgroundReportTableCommObj _CommObj;

        private Mutex _Mutex;
        private object _Locker;
        private Dictionary<string, RealData> _CodeDic;
        private List<Contract> _DelListCode;

        public ObservableCollection<RealData> RealDataList = new ObservableCollection<RealData>();

        public BackgroundDataServer()
        {
            _CodesList = new HashSet<Contract>();
            _DelListCode = new List<Contract>();
            _CommObj = new BackgroundReportTableCommObj(this);
            _CodeDic = new Dictionary<string, RealData>();
            _Mutex = new Mutex();
            _Locker = new object();
        }

        public BackgroundDataServer(Dictionary<string, RealData> backupCodeDic)
        {
            _CodesList = new HashSet<Contract>();
            _DelListCode = new List<Contract>();
            _CommObj = new BackgroundReportTableCommObj(this);
            _CodeDic = backupCodeDic;
            _Mutex = new Mutex();
            _Locker = new object();
        }

        //private QHOtherRealData m_otherRealData;
        //internal ObservableCollection<RealData> m_displayRealDataList;
        public ObservableCollection<Contract> m_codeArray = new ObservableCollection<Contract>();
        public ObservableCollection<Contract> m_oldCodeArray = new ObservableCollection<Contract>();

        public BackgroundDataServer(ObservableCollection<DisplayRealData> realList, Dictionary<string, RealData> backupCodeDic, List<Contract> delCodeList, ObservableCollection<Contract> codeArray, ObservableCollection<Contract> oldCodeArray)
        {
            //m_displayRealDataList = new ObservableCollection<RealData>();
            _CodeDic = new Dictionary<string, RealData>();
            _CodesList = new HashSet<Contract>();
            //m_otherRealData = new QHOtherRealData();
            _DelListCode = new List<Contract>();
            _CommObj = new BackgroundReportTableCommObj(this);
            m_codeArray = new ObservableCollection<Contract>();
            m_oldCodeArray = new ObservableCollection<Contract>();
            //m_displayRealDataList = realList;
            _CodeDic = backupCodeDic;
            m_codeArray = codeArray;
            m_oldCodeArray = oldCodeArray;
            _Mutex = new Mutex();
            _Locker = new object();
        }

        public void AddContract(Contract[] codes)
        {
            if (codes == null) return;
            //var reqList = codes.ToList().Except(_CodesList);
            _CodesList.Clear();
            foreach (Contract c in codes)
            {
                _CodesList.Add(c);
            }
        }

        public void Request()
        {
            _CommObj.RequestAutopush(_CodesList.ToArray(), false);
        }

        public void UnRequest()
        {
            _CommObj.RequestAutopush(_CodesList.ToArray(), true);
        }

        public void GetRealData()
        {
            MainWindow mainWindow = TradeDataClient.GetClientInstance().getMainWindow();//CtpDataServer.GetUserInstance().getMainWindow();
            if (mainWindow != null)
            {
                //TODO
                //mainWindow.RequestSnapShot(codesList.ToList());
            }
        }

        public RealData GetRealDataByCode(string code)
        {
            RealData realData = null;
            foreach (var item in RealDataList)
            {
                if (item.CodeInfo.Code == code)
                {
                    realData = item;
                    break;
                }
            }
            return realData;
        }

        public void ChangeType(Dictionary<Contract, RealData> tempDataDict)
        {
            Dictionary<Contract, RealData> futuresDataDict = new Dictionary<Contract, RealData>();
            Dictionary<Contract, RealData> combinationDataDict = new Dictionary<Contract, RealData>();
            foreach (Contract contract in tempDataDict.Keys)
            {
                RealData tempData = tempDataDict[contract];
                if (tempData == null || tempData.CodeInfo == null) return;
                string tempCodeInfoKey = tempData.CodeInfo.Code + "_" + tempData.CodeInfo.ExchCode;
                double newPrice = 0;

                newPrice = Math.Round(tempData.NewPrice, 4);//最新价
                if (_CodeDic.ContainsKey(tempCodeInfoKey))
                {
                    RealData DicData = _CodeDic[tempCodeInfoKey];
                    SetDisplayRealDataValueByRealTimeData(tempData, newPrice, DicData);
                }
                else //if (this._CommObj.RequestingCodes.Contains(tempData.CodeInfo.Code))//只操作正在推送的合约
                {
                    RealData reqRealData = new RealData();
                    SetDisplayRealDataValueByRealTimeData(tempData, newPrice, reqRealData);

                    _CodeDic.Add(tempCodeInfoKey, reqRealData);
                    RealDataList.Add(reqRealData);
                }

                if (tempData.CodeInfo.ProductType == "Futures" || tempData.CodeInfo.ProductType == "Combination" || tempData.CodeInfo.ProductType.Contains("Stock") || tempData.CodeInfo.ProductType.Contains("ETF"))
                {
                    if (futuresDataDict.ContainsKey(contract))
                    {
                        futuresDataDict[contract] = tempData;
                    }
                    else
                    {
                        futuresDataDict.Add(contract, tempData);
                    }
                }

                if (tempData.CodeInfo.ProductType == "Combination")
                {
                    if (combinationDataDict.ContainsKey(contract))
                    {
                        combinationDataDict[contract] = tempData;
                    }
                    else
                    {
                        combinationDataDict.Add(contract, tempData);
                    }
                }

                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        SetNewOrderPanelInfo(tempData);
                        UpdateLevelsQuotes(tempData);
                    });
                }
            }

            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    UpdateFuturesDataCollection(futuresDataDict);
                    UpdateCombinationDataCollection(combinationDataDict);
                    UpdateOptionDataCollection(tempDataDict);
                    SetPositionInfo(tempDataDict);
                });
            }

        }

        public void SavedDataInit(RealData savedData)
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    UpdateOptionDataCollection(savedData);
                });
            }
            //else if (this.commObj.RequestingCodes.Contains(tempData.codeInfo.Code))//只操作正在推送的合约
        }

        private void SetDisplayRealDataValueByRealTimeData(RealData realTimeDataArr, double newPrice, RealData tempData)
        {
            tempData.CodeInfo = realTimeDataArr.CodeInfo;//合约

            //double fluct = 0;
            //CodeSet.GetHycsAndFluct(tempData.Code, out tempData.m_hycs, out fluct);
            //tempData.HYCS = tempData.m_hycs;
            tempData.NewPrice = newPrice;
            tempData.UpperLimitPrice = realTimeDataArr.UpperLimitPrice;
            tempData.LowerLimitPrice = realTimeDataArr.LowerLimitPrice;
            tempData.BidPrice = realTimeDataArr.BidPrice;
            tempData.BidHand = realTimeDataArr.BidHand;//买量
            tempData.AskPrice = realTimeDataArr.AskPrice;
            tempData.AskHand = realTimeDataArr.AskHand;//卖量
            tempData.Position = realTimeDataArr.Position;//持仓量
            tempData.Volumn = realTimeDataArr.Volumn;//成交量
            tempData.Sum = realTimeDataArr.Sum;//成交额
            tempData.Hand = realTimeDataArr.Hand;//现手
            tempData.AvgPrice = realTimeDataArr.AvgPrice;//均价
            tempData.OpenPrice = realTimeDataArr.OpenPrice;//开盘价
            tempData.PrevSettlementPrice = realTimeDataArr.PrevSettlementPrice;//昨结算
            tempData.SettlmentPrice = realTimeDataArr.SettlmentPrice;        //现结算
            tempData.PrevClose = realTimeDataArr.PrevClose;        //昨收
            tempData.PrevPosition = realTimeDataArr.PrevPosition;
            tempData.ClosePrice = realTimeDataArr.ClosePrice;
            //tempData.Market = CodeSet.GetMarketName(tempData.Code);
            tempData.UpdateTime = realTimeDataArr.UpdateTime;
            tempData.MaxPrice = realTimeDataArr.MaxPrice;
            tempData.MinPrice = realTimeDataArr.MinPrice;
            tempData.UpperLimitPrice = realTimeDataArr.UpperLimitPrice;
            tempData.LowerLimitPrice = realTimeDataArr.LowerLimitPrice;
        }

        /// <summary>
        /// 处理分档行情
        /// </summary>
        /// <param name="m_displayRealData"></param>
        private void SetNewOrderPanelInfo(RealData realData)
        {
            MainWindow mainWindow = TradeDataClient.GetClientInstance().getMainWindow();//CtpDataServer.GetUserInstance().getMainWindow();
            if (mainWindow != null && mainWindow.uscNewOrderPanel.txtCode.Text.Trim() == realData.CodeInfo.Code)
            {
                mainWindow.uscNewOrderPanel.SetExtendsInfo(realData);

                //将行情显示到报价表中
                //mainWindow.uscOptionHangqing.AddExternalHqingData(m_displayRealData);

                //if (mainWindow.uscOptionHangqing.fendanghangqing.lblCode.Content.ToString() == m_displayRealData.Code)
                //{
                //    mainWindow.uscOptionHangqing.SetFendanghangqingByDisplayRealData(m_displayRealData);
                //}
            }
        }

        /// <summary>
        /// 处理持仓行情
        /// </summary>
        /// <param name="m_displayRealData"></param>
        public void SetPositionInfo(object realObj)
        {
            if (realObj is RealData)
            {
                RealData realData = realObj as RealData;
                Dictionary<Contract, RealData> realDataDict = new Dictionary<Contract, RealData>();
                realDataDict.Add(realData.CodeInfo, realData);
                UpdateFDYKForPositions(realDataDict);
            }
            else if (realObj is Dictionary<Contract, RealData>)
            {
                UpdateFDYKForPositions(realObj as Dictionary<Contract, RealData>);
            }
        }

        /// <summary>
        /// 单笔盈亏:现价（结算价）-开仓均价
        /// 逐笔浮盈:单笔盈亏*合约乘数*手数
        /// 盯市浮盈（昨仓）:(现价（结算价）-昨结算价)*合约乘数*手数
        /// 盯市浮盈（今仓）:(现价（结算价）-开仓均价)*合约乘数*手数
        /// </summary>
        private void UpdateFDYKForPositions(Dictionary<Contract, RealData> realDataDict)
        {
            _Mutex.WaitOne();
            try
            {
                UpdateFDYK(realDataDict);
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
            _Mutex.ReleaseMutex();
        }

        /// <summary>
        /// 根据实时主推更新资金的盈亏数据和持仓的浮动盈亏数据
        /// </summary>
        /// <param name="commRealTimeDatas"></param>
        private void UpdateFDYK(Dictionary<Contract, RealData> realDataDict)
        {
            MainWindow mainWindow = TradeDataClient.GetClientInstance().getMainWindow();//CtpDataServer.GetUserInstance().getMainWindow();

            foreach (Contract contract in realDataDict.Keys)
            {
                RealData realData = realDataDict[contract];
                double newPrice = realData.NewPrice;   //最新价
                double prevSettlementPrice = realData.PrevSettlementPrice;  //昨结算
                double settlementPrice = realData.SettlmentPrice; //现结算
                double bidPrice = realData.BidPrice[0];
                double askPrice = realData.AskPrice[0];

                if (newPrice == 0 || realData.Volumn == 0)
                {
                    if (prevSettlementPrice > 0)
                    {
                        newPrice = prevSettlementPrice;
                    }
                    else if (newPrice == 0)
                    {
                        newPrice = realData.PrevClose;
                    }
                }
                if (bidPrice == 0)
                {
                    bidPrice = newPrice;
                }
                if (askPrice == 0)
                {
                    askPrice = newPrice;
                }

                double hycs = realData.CodeInfo.Hycs;
                decimal fluct = realData.CodeInfo.Fluct;

                if (mainWindow != null)
                {
                    foreach (PosInfoDetail detail in mainWindow.PositionDetailCollection)
                    {
                        if (detail.Code == contract.Code)
                        {
                            detail.PrevSettleMent = prevSettlementPrice;
                            if (newPrice == 0)
                            {
                                detail.Fdyk = detail.Ccyk = 0;
                            }

                            //更新Detail的数据
                            if (detail.BuySell.Contains("买"))
                            {
                                detail.INewPrice = bidPrice;
                                if (CodeSetManager.GetContractInfo(detail.Code, CodeSetManager.ExNameToCtp(detail.Exchange)).ProductType != null
                                    && CodeSetManager.GetContractInfo(detail.Code, CodeSetManager.ExNameToCtp(detail.Exchange)).ProductType.Contains("Option"))
                                {
                                    detail.OptionMarketCap = bidPrice * detail.TradeHandCount * hycs;
                                    if (detail.PositionType.Contains("今"))
                                    {
                                        detail.Premium = -detail.AvgPx * detail.TradeHandCount * hycs;
                                        //detail.OptionProfit = (newPrice - detail.AvgPx) * detail.TradeHandCount * hycs;
                                    }
                                    else
                                    {
                                        //detail.OptionProfit = (newPrice - detail.PrevSettleMent) * detail.TradeHandCount * hycs;
                                    }
                                }

                                if (detail.PositionType == "今仓")
                                {
                                    detail.Ccyk = (bidPrice - detail.AvgPx) * detail.TradeHandCount * hycs;
                                    detail.Fdyk = detail.Ccyk;
                                }
                                else
                                {
                                    detail.Ccyk = (bidPrice - prevSettlementPrice) * detail.TradeHandCount * hycs;
                                    detail.Fdyk = (bidPrice - detail.AvgPx) * detail.TradeHandCount * hycs;
                                }
                            }
                            else
                            {
                                detail.INewPrice = askPrice;
                                if (CodeSetManager.GetContractInfo(detail.Code, CodeSetManager.ExNameToCtp(detail.Exchange)).ProductType != null
                                    && CodeSetManager.GetContractInfo(detail.Code, CodeSetManager.ExNameToCtp(detail.Exchange)).ProductType.Contains("Option"))
                                {
                                    detail.OptionMarketCap = -askPrice * detail.TradeHandCount * hycs;
                                    if (detail.PositionType.Contains("今"))
                                    {
                                        detail.Premium = detail.AvgPx * detail.TradeHandCount * hycs;
                                        //detail.OptionProfit = -(newPrice - detail.AvgPx) * detail.TradeHandCount * hycs;
                                    }
                                    else
                                    {
                                        //detail.OptionProfit = -(newPrice - detail.PrevSettleMent) * detail.TradeHandCount * hycs;
                                    }
                                }

                                if (detail.PositionType == "今仓")
                                {
                                    detail.Ccyk = (detail.AvgPx - askPrice) * detail.TradeHandCount * hycs;
                                    detail.Fdyk = detail.Ccyk;
                                }
                                else
                                {
                                    detail.Ccyk = (prevSettlementPrice - askPrice) * detail.TradeHandCount * hycs;
                                    detail.Fdyk = (detail.AvgPx - askPrice) * detail.TradeHandCount * hycs;
                                }
                            }
                        }
                    }

                    foreach (PosInfoTotal posTotal in mainWindow.PositionCollection_Total)
                    {
                        if (posTotal.Code == realData.CodeInfo.Code)
                        {
                            double yesterdayDsyk = 0;
                            double todayDsyk = 0;
                            double yesterdayFdyk = 0;
                            double todayFdyk = 0;
                            double todayOpProfit = 0;
                            double yesterdayOpProfit = 0;

                            if (newPrice != 0)
                            {
                                if (CodeSetManager.GetContractInfo(posTotal.Code, CodeSetManager.ExNameToCtp(posTotal.Exchange)).ProductType != null
                                    && CodeSetManager.GetContractInfo(posTotal.Code, CodeSetManager.ExNameToCtp(posTotal.Exchange)).ProductType.Contains("Option"))
                                {
                                    if (posTotal.BuySell.Contains("买"))
                                    {
                                        posTotal.OptionMarketCap = bidPrice * posTotal.TotalPosition * hycs;
                                        posTotal.Premium = -posTotal.TodayOpenAvgPx * posTotal.TodayPosition * hycs; //TODO: TradeHandCount
                                        //yesterdayOpProfit = (newPrice - posTotal.YesterdayOpenAvgPx) * posTotal.YesterdayPosition * hycs;//yesterdayOpProfit
                                        //todayOpProfit = (newPrice - posTotal.TodayOpenAvgPx) * posTotal.TodayPosition * hycs;//todayOpProfit
                                    }
                                    else
                                    {
                                        posTotal.OptionMarketCap = -askPrice * posTotal.TotalPosition * hycs;
                                        posTotal.Premium = posTotal.TodayOpenAvgPx * posTotal.TodayPosition * hycs; //TODO: TradeHandCount
                                        //yesterdayOpProfit = (posTotal.YesterdayOpenAvgPx - newPrice) * posTotal.YesterdayPosition * hycs;//yesterdayOpProfit
                                        //todayOpProfit = (posTotal.TodayOpenAvgPx - newPrice) * posTotal.TodayPosition * hycs;//todayOpProfit
                                    }
                                }

                                //更新Detail的数据
                                if (posTotal.BuySell.Contains("买"))
                                {
                                    yesterdayDsyk = (bidPrice - prevSettlementPrice) * posTotal.YesterdayPosition * hycs;
                                    todayDsyk = (bidPrice - posTotal.TodayOpenAvgPx) * posTotal.TodayPosition * hycs;

                                    yesterdayFdyk = (bidPrice - posTotal.YesterdayOpenAvgPx) * posTotal.YesterdayPosition * hycs;
                                    todayFdyk = todayDsyk;//(newPrice - detail.OpenAvgPx) * detail.TodayOpen * hycs;
                                }
                                else
                                {
                                    if (CodeSetManager.GetContractInfo(posTotal.Code, CodeSetManager.ExNameToCtp(posTotal.Exchange)).ProductType != null
                                        && CodeSetManager.GetContractInfo(posTotal.Code, CodeSetManager.ExNameToCtp(posTotal.Exchange)).ProductType.Contains("Option"))
                                    {
                                        posTotal.OptionMarketCap = -askPrice * posTotal.TotalPosition * hycs;
                                        posTotal.Premium = posTotal.TodayOpenAvgPx * posTotal.TodayPosition * hycs; //TODO: TradeHandCount
                                    }

                                    yesterdayDsyk = (prevSettlementPrice - askPrice) * posTotal.YesterdayPosition * hycs;
                                    todayDsyk = (posTotal.TodayOpenAvgPx - askPrice) * posTotal.TodayPosition * hycs;

                                    yesterdayFdyk = (posTotal.YesterdayOpenAvgPx - askPrice) * posTotal.YesterdayPosition * hycs;
                                    todayFdyk = todayDsyk;//((newPrice - detail.OpenAvgPx) * detail.TodayOpen * hycs);
                                }

                            }

                            posTotal.Ccyk = yesterdayDsyk + todayDsyk;
                            posTotal.Fdyk = yesterdayFdyk + todayFdyk;
                            posTotal.OptionProfit = yesterdayOpProfit + todayOpProfit;

                            posTotal.AvgPositionPrice = (posTotal.TodayPosition * posTotal.TodayOpenAvgPx + posTotal.YesterdayPosition * prevSettlementPrice) / posTotal.TotalPosition;
                        }
                    }

                    //更新资金数据
                    double totalDSFY = 0;
                    double totalFdyk = 0;
                    double totalPremium = 0;
                    double totalOptionCap = 0;
                    double totalOptionProfit = 0;
                    foreach (PosInfoTotal detail in mainWindow.PositionCollection_Total)
                    {
                        if (CodeSetManager.GetContractInfo(detail.Code, CodeSetManager.ExNameToCtp(detail.Exchange)).ProductType == "Futures")
                        {
                            totalDSFY += detail.Ccyk;
                            totalFdyk += detail.Fdyk;
                        }
                        else if (CodeSetManager.GetContractInfo(detail.Code, CodeSetManager.ExNameToCtp(detail.Exchange)).ProductType != null
                            && CodeSetManager.GetContractInfo(detail.Code, CodeSetManager.ExNameToCtp(detail.Exchange)).ProductType.Contains("Option"))
                        {
                            totalOptionProfit += detail.Ccyk;
                        }
                        totalPremium += detail.Premium;
                        totalOptionCap += detail.OptionMarketCap;
                        totalOptionProfit += detail.OptionProfit;
                    }

                    if (mainWindow.CapitalDataCollection != null)
                    {
                        mainWindow.CapitalDataCollection.Dsfy = totalDSFY;
                        mainWindow.CapitalDataCollection.FloatProfit = totalFdyk;
                        //mainWindow.CapitalDataCollection.Premium = totalPremium;
                        mainWindow.CapitalDataCollection.OptionMarketCap = totalOptionCap;
                        mainWindow.CapitalDataCollection.OptionProfit = totalOptionProfit;
                        mainWindow.CapitalDataCollection.AccountCap = totalOptionCap + mainWindow.CapitalDataCollection.DynamicEquity;
                    }
                }
            }
        }

        public void AddSingleRealData(string strCode)
        {
            if (_CommObj.RequestingCodes.Contains(strCode))
            {
                return;
            }
            Contract searchCodeInfo = CodeSetManager.GetContractInfo(strCode);

            this.AddContract(new Contract[] { searchCodeInfo });
            this.GetRealData();
            this.Request();
        }

        public void RemoveUselessRequest()
        {
            //去除不必要的请求
            List<Contract> lstDelCodes = new List<Contract>();
            MainWindow mainWindow = TradeDataClient.GetClientInstance().getMainWindow();//CtpDataServer.GetUserInstance().getMainWindow();
            if (mainWindow != null)
            {
                foreach (var item in _CommObj.RequestingCodes)
                {
                    bool isUsed = false;
                    foreach (var positionItem in mainWindow.PositionDetailCollection)
                    {
                        if (item == positionItem.Code)
                        {
                            isUsed = true;
                            break;
                        }
                    }
                    foreach (var quoteItem in mainWindow.RealDataCollection)
                    {
                        if (item == quoteItem.Code)
                        {
                            isUsed = true;
                            break;
                        }
                    }
                    foreach (var quoteItem in mainWindow.RealDataArbitrageCollection)
                    {
                        if (item == quoteItem.Code)
                        {
                            isUsed = true;
                            break;
                        }
                    }
                    if (item == mainWindow.uscNewOrderPanel.txtCode.Text)
                    {
                        isUsed = true;
                    }

                    string fCode = mainWindow.uscOptionHangqing.titleCode.Content.ToString();
                    if (fCode != null && fCode != String.Empty)
                    {
                        if (fCode == item)
                        {
                            isUsed = true;
                            break;
                        }
                        if (mainWindow.uscOptionHangqing.OptionCodeDict.ContainsKey(fCode))
                        {
                            foreach (string contract in mainWindow.uscOptionHangqing.OptionCodeDict[fCode])
                            {
                                if (contract == item)
                                {
                                    isUsed = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (!isUsed)
                    {
                        Contract cCode = CodeSetManager.GetContractInfo(item);
                        //TODO :临时方案
                        if (cCode == null)
                        {
                            cCode = new Contract(item);
                        }
                        //
                        lstDelCodes.Add(cCode);
                    }
                }
            }

            if (lstDelCodes.Count > 0)
            {
                AddContract(lstDelCodes.ToArray());
                UnRequest();
            }
        }

        /// <summary>
        /// 期货行情
        /// </summary>
        /// <param name="realData"></param>
        private void UpdateFuturesDataCollection(Dictionary<Contract, RealData> realDataDict)
        {
            MainWindow mainWindow = TradeDataClient.GetClientInstance().getMainWindow();//CtpDataServer.GetUserInstance().getMainWindow();
            if (mainWindow != null)
            {
                foreach (var item in mainWindow.RealDataCollection)
                {
                    foreach (Contract contract in realDataDict.Keys)
                    {
                        RealData realData = realDataDict[contract];
                        if (item.Code == contract.Code)
                        {
                            item.UpdateProperties(realData);
                        }
                    }
                }
            }
            else
            {
                Util.Log("Warning!: mainWindow in BackgroundDataServer is NULL! ");
            }
        }

        /// <summary>
        /// 组合行情
        /// </summary>
        /// <param name="realData"></param>
        private void UpdateCombinationDataCollection(Dictionary<Contract, RealData> realDataDict)
        {
            MainWindow mainWindow = TradeDataClient.GetClientInstance().getMainWindow();//CtpDataServer.GetUserInstance().getMainWindow();
            if (mainWindow != null)
            {
                //mainWindow.updateFuturesDataByDisplayRealData(m_realData);
                foreach (var item in mainWindow.RealDataArbitrageCollection)
                {
                    foreach (Contract contract in realDataDict.Keys)
                    {
                        RealData realData = realDataDict[contract];
                        if (item.Code == contract.Code)
                        {
                            item.UpdateProperties(realData);
                        }
                    }
                }
            }
            else
            {
                Util.Log("Warning!: mainWindow in BackgroundDataServer is NULL! ");
            }
        }

        /// <summary>
        /// 期权行情
        /// </summary>
        /// <param name="realDataDict"></param>
        private void UpdateOptionDataCollection(Dictionary<Contract, RealData> realDataDict)
        {
            MainWindow mainWindow = TradeDataClient.GetClientInstance().getMainWindow();//CtpDataServer.GetUserInstance().getMainWindow();
            if (mainWindow != null)
            {
                foreach (var item in mainWindow.OptionRealDataCollection)
                {
                    foreach (Contract contract in realDataDict.Keys)
                    {
                        RealData tempData = realDataDict[contract];
                        mainWindow.updateOptionDataByDisplayRealData(tempData);
                        if (item.Code_C == tempData.CodeInfo.Code || item.Code_P == tempData.CodeInfo.Code)
                        {
                            item.UpdateProperties(tempData);
                            OptionCalculator.Enqueue(item);
                        }
                    }
                }
            }
            else
            {
                Util.Log("Warning!: mainWindow in BackgroundDataServer is NULL! ");
            }
        }

        /// <summary>
        /// 期权行情
        /// </summary>
        /// <param name="m_realData"></param>
        private void UpdateOptionDataCollection(RealData m_realData)
        {
            MainWindow mainWindow = TradeDataClient.GetClientInstance().getMainWindow();//CtpDataServer.GetUserInstance().getMainWindow();
            if (mainWindow != null)
            {
                mainWindow.updateOptionDataByDisplayRealData(m_realData);
                foreach (var item in mainWindow.OptionRealDataCollection)
                {
                    if (item.Code_C == m_realData.CodeInfo.Code || item.Code_P == m_realData.CodeInfo.Code)
                    {
                        //lock (_Locker)
                        //{
                        //    DataContainer.AddRealDataToContainer(m_realData);
                        //}
                        item.UpdateProperties(m_realData);
                        OptionCalculator.Enqueue(item);
                        break;
                    }
                }
            }
            else
            {
                Util.Log("Warning!: mainWindow in BackgroundDataServer is NULL! ");
            }
        }

        public void UpdateRealDataList()
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    for (int i = 0; i < this.RealDataList.Count; i++)
                    {
                        if (!_CommObj.RequestingCodes.Contains(this.RealDataList[i].CodeInfo.Code))
                        {
                            this.RealDataList.RemoveAt(i);
                            i--;
                        }
                    }
                });
            }
        }

        public void UpdateLevelsQuotes(RealData realData)
        {
            MainWindow mainWindow = TradeDataClient.GetClientInstance().getMainWindow();//CtpDataServer.GetUserInstance().getMainWindow();
            if (mainWindow != null)
            {
                if (mainWindow.uscHangqing != null)
                {
                    mainWindow.uscHangqing.LvQuotesPanel.SetLevelsQuotesByRealData(realData);
                }
                if (mainWindow.uscOptionHangqing != null)
                {
                    mainWindow.uscOptionHangqing.LvOptQuotesPanel.SetLevelsQuotesByRealData(realData);
                }
            }
        }
    }

    public class BackgroundReportTableCommObj //: CommObj
    {
        internal BackgroundDataServer _Owner;
        private object _LockerBackground = new object();
        private HashSet<string> _RequestingCodes = new HashSet<string>();

        public HashSet<string> RequestingCodes
        {
            get { return _RequestingCodes; }
            set { _RequestingCodes = value; }
        }

        public BackgroundReportTableCommObj(BackgroundDataServer parent)
        {
            _Owner = parent;
        }

        public void RequestAutopush(Contract[] codeInfoArr, bool isEmpty)
        {
            //GHS add
            List<string> reqList = new List<string>();
            List<string> cancelList = new List<string>();

            if (isEmpty)//不再请求
            {
                foreach (Contract item in codeInfoArr)
                {
                    if (RequestingCodes.Contains(item.Code))
                    {
                        cancelList.Add(item.Code);
                        RequestingCodes.Remove(item.Code);
                    }
                }
            }
            else//请求
            {
                foreach (Contract item in codeInfoArr)
                {
                    if (!RequestingCodes.Contains(item.Code))
                    {
                        reqList.Add(item.Code);
                        RequestingCodes.Add(item.Code);
                    }
                }
            }

            //base.RequestAutopush(codeInfoArr, isEmpty);
            MainWindow mainWindow = TradeDataClient.GetClientInstance().getMainWindow();//CtpDataServer.GetUserInstance().getMainWindow();
            if (mainWindow != null)
            {
                if (reqList.Count > 0)
                {
                    mainWindow.RequestSnapShotPlusUpdate(reqList.ToList());
                }
                if (cancelList.Count > 0)
                {
                    mainWindow.ClearUpdate(cancelList.ToList());
                }
            }
            else
            {
                Util.Log("Warning!: mainWindow in BackgroundDataServer is NULL! ");
            }
        }

    }
}
