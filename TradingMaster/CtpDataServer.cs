using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using TradingMaster.Control;
using System.Threading;
using TradingMaster.JYData;
using TradingMaster.CodeSet;
using System.Xml;
using System.Collections.Concurrent;

namespace TradingMaster
{
    public class CtpDataServer : DataContainer
    {
        const int LOGWINDOWMEDIUNHEIGHT = 505;
        const int LOGWINDOWSMALLHEIGHT = 305;
        const int CTPDATASERVERERROR = -4;

        private static CtpDataServer _CtpServerInstance = null;
        private StatementOrderAffirm _AffirmWindow;
        private Login _LogWindow;
        private MainWindow _MainWindow;
        private string _TradingDay = String.Empty;

        private CtpTraderApi _CtpTraderApi = null;
        private CtpMdApi _CtpMdApi = null;
        private BACKENDTYPE _BackEnd = BACKENDTYPE.CTP;
        
        /// <summary>
        /// 经纪公司设定
        /// </summary>
        private EnumThostMarginPriceTypeType _MarginPriceType;
        private EnumThostAlgorithmType _Algorithm;
        private EnumThostIncludeCloseProfitType _AvailIncludeCloseProfit;
        private EnumThostOptionRoyaltyPriceTypeType _OptionRoyaltyPriceType;

        /// <summary>
        /// 格式为HH*3600+MM*60+SS
        /// </summary>
        //private int _ServerStartTime = 0;

        /// <summary>
        /// 交易查询/执行报单队列
        /// </summary>
        private ExecQueue _TradeDataReqQueue = null;

        /// <summary>
        /// 行情请求队列
        /// </summary>
        private ExecQueue _MarketDataReqQueue = null;

        /// <summary>
        /// 交易成员变量线程取消控制阀
        /// </summary>
        private CancellationTokenSource _TradingCts = null;

        /// <summary>
        /// 行情变量线程取消控制阀
        /// </summary>
        private CancellationTokenSource _MdCts = null;

        public Boolean TradeServerLogOn
        {
            get
            {
                if (_CtpTraderApi != null)
                {
                    return IsLoggedOn;
                }
                return false;
            }
        }

        public Boolean QuoteServerLogOn
        {
            get
            {
                if (_CtpMdApi != null)
                {
                    return IsConnected;
                }
                return false;
            }
        }

        public string GetCurrentInvestorID()
        {
            if (_CtpTraderApi != null)
            {
                return _CtpTraderApi.InvestorID;
            }
            return "未获取";
        }

        public string GetCurrentBroker()
        {
            if (_CtpTraderApi != null)
            {
                return _CtpTraderApi.BrokerID;
            }
            return "未获取";
        }

        public string GetCurrentTradeAddress()
        {
            if (_CtpTraderApi != null)
            {
                return _CtpTraderApi.FrontAddr;
            }
            return "未获取";
        }

        public string GetCurrentQuoteAddress()
        {
            if (_CtpMdApi != null)
            {
                return _CtpMdApi.FrontAddr;
            }
            return "未获取";
        }

        public string InvestorID { get; set; }

        private CtpDataServer()
        {
            _TradingCts = new CancellationTokenSource();
            if (_TradingCts != null)
            {
                _TradeDataReqQueue = new ExecQueue(_TradingCts);
                //_TradeDataRspQueue = new RspQueue(TradingCts, BACKENDTYPE.CTP);
            }
            _MdCts = new CancellationTokenSource();
            if (_MdCts != null)
            {
                _MarketDataReqQueue = new ExecQueue(_MdCts);
                //_MarketDataRspQueue = new RspQueue(MdCts, BACKENDTYPE.CTP);
            }
            ServerLock = new object();
            _LogWindow = Login.LoginInstace;
            TempOrderFlag = true;
            TempTradeFlag = true;
            TempPosFlag = true;
            TempQuoteInsertFlag = true;
            TempExecFlag = true;
        }

        public static CtpDataServer GetUserInstance()
        {
            if (_CtpServerInstance == null)
            {
                _CtpServerInstance = new CtpDataServer();
            }
            return _CtpServerInstance;
        }

        public void WriteBackConfirmInfos()
        {
            if (_CtpTraderApi != null)
            {
                CommonUtil.WriteBackConfirmInfos(this._CtpTraderApi.InvestorID);
            }
            else
            {
                Util.Log("Error! WriteBackConfirmInfos: tradeApi is null!");
            }
        }

        public void AddPosOrderList(PosInfoOrder resetOrder)
        {
            if (resetOrder != null)
            {
                lock (ResetOrderLocker)
                {
                    ResetOrderList.Add(resetOrder);

                    string reOpenKey = resetOrder.posInfo.Code + "_" + resetOrder.posInfo.BuySell;
                    if (ReOpenOrderDict.ContainsKey(resetOrder))
                    {
                        ReOpenOrderDict[resetOrder] = resetOrder.HandCount;
                    }
                    else
                    {
                        ReOpenOrderDict.Add(resetOrder, resetOrder.HandCount);
                    }
                }
            }
        }

        public void AddPosCancelOrderList(PosInfoOrder cancelOrder)
        {
            if (cancelOrder != null)
            {
                lock (_CancelCloseLocker)
                {
                    _OrderAfterCancelList.Add(cancelOrder);
                }
            }
        }

        public void setMainWindow(MainWindow mainWindow)
        {
            this._MainWindow = mainWindow;
        }

        public MainWindow getMainWindow()
        {
            return _MainWindow;
        }

        public void setLogWindow(Login logWindowSrc)
        {
            _LogWindow = logWindowSrc;
        }

        public Login getLoginWindow()
        {
            return _LogWindow;
        }

        public void RequestCapital()
        {
            if (_TradeDataReqQueue == null)
            {
                Util.Log("CtpTraderApi_OnRtnTrade Error: TradeDataReqQueue == null");
                return;
            }
            if (_TradeDataReqQueue.OrdCount() == 0 && !TempOrderFlag)
            {
                JYCapitalData = new CapitalInfo();
                AddToTradeDataQryQueue(new RequestContent("ReqCapital", new List<object>()));
            }
        }

        /// <summary>
        /// 处理委托查询
        /// </summary>
        /// <param name="pOrderData"></param>
        protected void ProcessParkedOrderData(List<Q7JYOrderData> pOrderData)
        {
            if (_MainWindow != null && System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    try
                    {
                        _MainWindow.PreConditionOrderData.Clear();
                        _MainWindow.ConditionOrderData.Clear();
                        _MainWindow.PreOrderData.Clear();
                        _MainWindow.SentOrderData.Clear();

                        foreach (Q7JYOrderData pOrder in pOrderData)
                        {
                            _MainWindow.PreConditionOrderData.Add(pOrder);

                            if (pOrder.OrderStatus == "已发送")
                            {
                                _MainWindow.SentOrderData.Add(pOrder);
                            }

                            if (pOrder.TouchCondition == "预埋单")
                            {
                                _MainWindow.PreOrderData.Add(pOrder);
                            }
                            else
                            {
                                _MainWindow.ConditionOrderData.Add(pOrder);
                            }
                        }
                        //mainWindow.uscNewOrderPanel.SetHandCount();
                    }
                    catch (Exception ex)
                    {
                        Util.Log("exception: " + ex.Message);
                        Util.Log("exception: " + ex.StackTrace);
                    }
                });
            }
        }

        private string OpositePositionKey(string key)
        {
            try
            {
                string[] temp = key.Split('_');
                string code = temp[0];
                string oppoBuysell = temp[1] == "买" ? "卖" : "买";
                return code + "_" + oppoBuysell;
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
            return null;
        }

        /// <summary>
        /// 收到可处理的最大交易数
        /// </summary>
        /// <param name="maxOperation"></param>
        public void ProcessMaxOperation(MaxOperation maxOperation)
        {
            if (_MainWindow != null && System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    try
                    {
                        _MainWindow.UpdateMaxOperation(maxOperation);
                    }
                    catch (Exception ex)
                    {
                        Util.Log(ex.ToString());
                    }
                }
                );
            }
        }

        public void ProcessNewComeTradeInfo(Q7JYOrderData trade, bool isInited = true)
        {
            //Add for GHS 5.0.2
            if (trade == null)
            {
                return;
            }

            //Synchronizing the trading info when initialized
            List<Q7JYOrderData> removeItems = new List<Q7JYOrderData>();
            if (!isInited)
            {
                foreach (Q7JYOrderData item in QryTradeDataLst)
                {
                    if (trade.TradeID != "" && item.TradeID == trade.TradeID && item.OrderID == trade.OrderID && item.Exchange == trade.Exchange)
                    {
                        removeItems.Add(item);
                    }
                }
                foreach (Q7JYOrderData rItem in removeItems)
                {
                    QryTradeDataLst.Remove(rItem);
                }
            }
        }

        public void OnPasswordChanged(string msg, Boolean isSuccess)
        {
            //base.OnPasswordChanged(msg, isSuccess);
            //getLoginWindow().UpdatePassWord(isSuccess);
            MessageBox.Show(msg);
        }

        public void InitTradeApi(string account, string password, string brokerId, string ip)
        {
            _CtpTraderApi = ApiTradeConnection(account, password, brokerId, ip);
            Util.Log("TradeApiCTP CtpDataServer: InitTradeApi. Broker = " + _CtpTraderApi.BrokerID + ", IP = " + _CtpTraderApi.FrontAddr);
            _CtpTraderApi.OnFrontConnected += new CtpTraderApi.FrontConnected(CtpTraderApi_OnFrontConnected);
            _CtpTraderApi.OnFrontDisConnected += new CtpTraderApi.FrontDisConnected(CtpTraderApi_OnFrontDisConnected);
            _CtpTraderApi.OnRspUserLogin += new CtpTraderApi.RspUserLogin(CtpTraderApi_OnRspUserLogin);
            _CtpTraderApi.OnRspUserLogout += new CtpTraderApi.RspUserLogout(CtpTraderApi_OnRspUserLogout);
            _CtpTraderApi.OnRspQryInvestor += new CtpTraderApi.RspQryInvestor(CtpTraderApi_OnRspQryInvestor);
            _CtpTraderApi.OnRspQryTradingCode += new CtpTraderApi.RspQryTradingCode(CtpTraderApi_OnRspQryTradingCode);
            _CtpTraderApi.OnRspError += new CtpTraderApi.RspError(CtpTraderApi_OnRspError);

            _CtpTraderApi.OnRspQryInstrument += new CtpTraderApi.RspQryInstrument(CtpTraderApi_OnRspQryInstrument);
            _CtpTraderApi.OnRspQryInstrumentMarginRate += new CtpTraderApi.RspQryInstrumentMarginRate(CtpTraderApi_OnRspQryInstrumentMarginRate);
            _CtpTraderApi.OnRspQryInstrumentCommissionRate += new CtpTraderApi.RspQryInstrumentCommissionRate(CtpTraderApi_OnRspQryInstrumentCommissionRate);
            _CtpTraderApi.OnRspQryInvestorProductGroupMargin += new CtpTraderApi.RspQryInvestorProductGroupMargin(CtpTraderApi_OnRspQryInvestorProductGroupMargin);

            _CtpTraderApi.OnRspSettlementInfoConfirm += new CtpTraderApi.RspSettlementInfoConfirm(CtpTraderApi_OnRspSettlementInfoConfirm);
            _CtpTraderApi.OnRspQrySettlementInfoConfirm += new CtpTraderApi.RspQrySettlementInfoConfirm(CtpTraderApi_OnRspQrySettlementInfoConfirm);
            _CtpTraderApi.OnRspQrySettlementInfo += new CtpTraderApi.RspQrySettlementInfo(CtpTraderApi_OnRspQrySettlementInfo);
            _CtpTraderApi.OnRspUserPasswordUpdate += new CtpTraderApi.RspUserPasswordUpdate(CtpTraderApi_OnRspUserPasswordUpdate);
            _CtpTraderApi.OnRspQueryMaxOrderVolume += new CtpTraderApi.RspQueryMaxOrderVolume(CtpTraderApi_OnRspQueryMaxOrderVolume);
            _CtpTraderApi.OnRtnTradingNotice += new CtpTraderApi.RtnTradingNotice(CtpTraderApi_OnRtnTradingNotice);
            _CtpTraderApi.OnRtnInstrumentStatus += new CtpTraderApi.RtnInstrumentStatus(CtpTraderApi_OnRtnInstrumentStatus);
            _CtpTraderApi.OnRspQryBrokerTradingParams += new CtpTraderApi.RspQryBrokerTradingParams(CtpTraderApi_OnRspQryBrokerTradingParams);

            _CtpTraderApi.OnRtnOrder += new CtpTraderApi.RtnOrder(CtpTraderApi_OnRtnOrder);
            _CtpTraderApi.OnRspOrderInsert += new CtpTraderApi.RspOrderInsert(CtpTraderApi_OnRspOrderInsert);
            _CtpTraderApi.OnErrRtnOrderInsert += new CtpTraderApi.ErrRtnOrderInsert(CtpTraderApi_OnErrRtnOrderInsert);
            _CtpTraderApi.OnRspOrderAction += new CtpTraderApi.RspOrderAction(CtpTraderApi_OnRspOrderAction);
            _CtpTraderApi.OnErrRtnOrderAction += new CtpTraderApi.ErrRtnOrderAction(CtpTraderApi_OnErrRtnOrderAction);
            _CtpTraderApi.OnRtnTrade += new CtpTraderApi.RtnTrade(CtpTraderApi_OnRtnTrade);

            _CtpTraderApi.OnRspQryOrder += new CtpTraderApi.RspQryOrder(CtpTraderApi_OnRspQryOrder);
            _CtpTraderApi.OnRspQryTrade += new CtpTraderApi.RspQryTrade(CtpTraderApi_OnRspQryTrade);
            _CtpTraderApi.OnRspQryInvestorPosition += new CtpTraderApi.RspQryInvestorPosition(CtpTraderApi_OnRspQryInvestorPosition);
            _CtpTraderApi.OnRspQryInvestorPositionDetail += new CtpTraderApi.RspQryInvestorPositionDetail(CtpTraderApi_OnRspQryInvestorPositionDetail);
            _CtpTraderApi.OnRspQryInvestorPositionCombineDetail += new CtpTraderApi.RspQryInvestorPositionCombineDetail(CtpTraderApi_OnRspQryInvestorPositionCombineDetail);
            _CtpTraderApi.OnRspQryTradingAccount += new CtpTraderApi.RspQryTradingAccount(CtpTraderApi_OnRspQryTradingAccount);

            //预埋单交互指令
            _CtpTraderApi.OnRspParkedOrderAction += new CtpTraderApi.RspParkedOrderAction(CtpTraderApi_OnRspParkedOrderAction);
            _CtpTraderApi.OnRspParkedOrderInsert += new CtpTraderApi.RspParkedOrderInsert(CtpTraderApi_OnRspParkedOrderInsert);
            _CtpTraderApi.OnRspQryParkedOrder += new CtpTraderApi.RspQryParkedOrder(CtpTraderApi_OnRspQryParkedOrder);
            _CtpTraderApi.OnRspQryParkedOrderAction += new CtpTraderApi.RspQryParkedOrderAction(CtpTraderApi_OnRspQryParkedOrderAction);
            _CtpTraderApi.OnRspRemoveParkedOrder += new CtpTraderApi.RspRemoveParkedOrder(CtpTraderApi_OnRspRemoveParkedOrder);
            _CtpTraderApi.OnRspRemoveParkedOrderAction += new CtpTraderApi.RspRemoveParkedOrderAction(CtpTraderApi_OnRspRemoveParkedOrderAction);

            //非银行交互指令
            _CtpTraderApi.OnRspQryProductExchRate += new CtpTraderApi.RspQryProductExchRate(CtpTraderApi_OnRspQryProductExchRate);
            _CtpTraderApi.OnRspQryExchangeRate += new CtpTraderApi.RspQryExchangeRate(CtpTraderApi_OnRspQryExchangeRate);

            _CtpTraderApi.OnRspQryContractBank += new CtpTraderApi.RspQryContractBank(CtpTraderApi_OnRspQryContractBank);
            _CtpTraderApi.OnRspQryAccountregister += new CtpTraderApi.RspQryAccountregister(CtpTraderApi_OnRspQryAccountregister);
            //tradeApi.OnRspQryTransferBank += new TradeApi.RspQryTransferBank(CtpTraderApi_OnRspQryTransferBank);
            _CtpTraderApi.OnRspTradingAccountPasswordUpdate += new CtpTraderApi.RspTradingAccountPasswordUpdate(CtpTraderApi_OnRspTradingAccountPasswordUpdate);
            _CtpTraderApi.OnRspQryTransferSerial += new CtpTraderApi.RspQryTransferSerial(CtpTraderApi_OnRspQryTransferSerial);

            //银期交互指令
            _CtpTraderApi.OnRspQueryBankAccountMoneyByFuture += new CtpTraderApi.RspQueryBankAccountMoneyByFuture(CtpTraderApi_OnRspQueryBankAccountMoneyByFuture);
            _CtpTraderApi.OnRtnQueryBankBalanceByFuture += new CtpTraderApi.RtnQueryBankBalanceByFuture(CtpTraderApi_OnRtnQueryBankBalanceByFuture);
            _CtpTraderApi.OnErrRtnQueryBankBalanceByFuture += new CtpTraderApi.ErrRtnQueryBankBalanceByFuture(CtpTraderApi_OnErrRtnQueryBankBalanceByFuture);

            _CtpTraderApi.OnRspFromFutureToBankByFuture += new CtpTraderApi.RspFromFutureToBankByFuture(CtpTraderApi_OnRspFromFutureToBankByFuture);
            _CtpTraderApi.OnRtnFromFutureToBankByFuture += new CtpTraderApi.RtnFromFutureToBankByFuture(CtpTraderApi_OnRtnFromFutureToBankByFuture);
            _CtpTraderApi.OnErrRtnFutureToBankByFuture += new CtpTraderApi.ErrRtnFutureToBankByFuture(CtpTraderApi_OnErrRtnFutureToBankByFuture);

            _CtpTraderApi.OnRspFromBankToFutureByFuture += new CtpTraderApi.RspFromBankToFutureByFuture(CtpTraderApi_OnRspFromBankToFutureByFuture);
            _CtpTraderApi.OnRtnFromBankToFutureByFuture += new CtpTraderApi.RtnFromBankToFutureByFuture(CtpTraderApi_OnRtnFromBankToFutureByFuture);
            _CtpTraderApi.OnErrRtnBankToFutureByFuture += new CtpTraderApi.ErrRtnBankToFutureByFuture(CtpTraderApi_OnErrRtnBankToFutureByFuture);

            //期权交互指令
            _CtpTraderApi.OnRspQryOptionInstrCommRate += new CtpTraderApi.RspQryOptionInstrCommRate(CtpTraderApi_OnRspQryOptionInstrCommRate);
            _CtpTraderApi.OnRspQryOptionInstrTradeCost += new CtpTraderApi.RspQryOptionInstrTradeCost(CtpTraderApi_OnRspQryOptionInstrTradeCost);
            _CtpTraderApi.OnRspQryExecOrder += new CtpTraderApi.RspQryExecOrder(CtpTraderApi_OnRspQryExecOrder);
            _CtpTraderApi.OnRtnExecOrder += new CtpTraderApi.RtnExecOrder(CtpTraderApi_OnRtnExecOrder);
            _CtpTraderApi.OnRspExecOrderInsert += new CtpTraderApi.RspExecOrderInsert(CtpTraderApi_OnRspExecOrderInsert);
            _CtpTraderApi.OnErrRtnExecOrderInsert += new CtpTraderApi.ErrRtnExecOrderInsert(CtpTraderApi_OnErrRtnExecOrderInsert);
            _CtpTraderApi.OnRspExecOrderAction += new CtpTraderApi.RspExecOrderAction(CtpTraderApi_OnRspExecOrderAction);
            _CtpTraderApi.OnErrRtnExecOrderAction += new CtpTraderApi.ErrRtnExecOrderAction(CtpTraderApi_OnErrRtnExecOrderAction);

            //做市商交互指令
            _CtpTraderApi.OnRspQryForQuote += new CtpTraderApi.RspQryForQuote(CtpTraderApi_OnRspQryForQuote);
            //_CtpTraderApi.OnRtnForQuoteRsp += new CtpTraderApi.RtnForQuoteRsp(CtpTraderApi_OnRtnForQuoteRsp);
            _CtpTraderApi.OnRspForQuoteInsert += new CtpTraderApi.RspForQuoteInsert(CtpTraderApi_OnRspForQuoteInsert);
            _CtpTraderApi.OnErrRtnForQuoteInsert += new CtpTraderApi.ErrRtnForQuoteInsert(CtpTraderApi_OnErrRtnForQuoteInsert);
            _CtpTraderApi.OnRspQryQuote += new CtpTraderApi.RspQryQuote(CtpTraderApi_OnRspQryQuote);
            _CtpTraderApi.OnRtnQuote += new CtpTraderApi.RtnQuote(CtpTraderApi_OnRtnQuote);
            _CtpTraderApi.OnRspQuoteInsert += new CtpTraderApi.RspQuoteInsert(CtpTraderApi_OnRspQuoteInsert);
            //_CtpTraderApi.OnErrRtnQuoteInsert += new CtpTraderApi.ErrRtnQuoteInsert(CtpTraderApi_OnErrRtnQuoteInsert);
            _CtpTraderApi.OnRspQuoteAction += new CtpTraderApi.RspQuoteAction(CtpTraderApi_OnRspQuoteAction);
            //_CtpTraderApi.OnErrRtnQuoteAction += new CtpTraderApi.ErrRtnQuoteAction(CtpTraderApi_OnErrRtnQuoteAction);

            //组合保证金指令
            _CtpTraderApi.OnRtnCombAction += new CtpTraderApi.RtnCombAction(CtpTraderApi_OnRtnCombAction);
            _CtpTraderApi.OnRspQryCombAction += new CtpTraderApi.RspQryCombAction(CtpTraderApi_OnRspQryCombAction);
            _CtpTraderApi.OnRspQryCombInstrumentGuard += new CtpTraderApi.RspQryCombInstrumentGuard(CtpTraderApi_OnRspQryCombInstrumentGuard);
            _CtpTraderApi.OnRspCombActionInsert += new CtpTraderApi.RspCombActionInsert(CtpTraderApi_OnRspCombActionInsert);
            _CtpTraderApi.OnErrRtnCombActionInsert += new CtpTraderApi.ErrRtnCombActionInsert(CtpTraderApi_OnErrRtnCombActionInsert);

            if (_CtpTraderApi != null)
            {
                _CtpTraderApi.Connect();
            }
            else
            {
                Util.Log("CTP Trader API初始化失败！");
            }
        }

        private CtpTraderApi ApiTradeConnection(string userName, string passWord, string brokerId, string ip)
        {
            InvestorID = userName;
            return new CtpTraderApi(userName, passWord, brokerId, ip);
        }

        void CtpTraderApi_OnFrontConnected()
        {
            Util.Log("TradeApiCTP CtpDataServer: OnFrontConnected is received.");
            AddToTradeDataQryQueue(new RequestContent("ClientLogin", new List<object>()));
        }

        void CtpTraderApi_OnFrontDisConnected(int reason)
        {
            Util.Log("TradeApiCTP CtpDataServer: OnFrontDisConnected is received.");
            TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接断开，请尝试点击界面重连或直接重启");
            if (IsLoggedOn)
            {
                IsLoggedOn = false;
                //MessageBox.Show("网络连接失败，请尝试点击界面重连或直接重启", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            DisconnectStruct disStruct = GetDisconnectReport("Trader");
            TradeDataClient.GetClientInstance().RtnMessageEnqueue(disStruct);
        }

        void CtpTraderApi_OnRspUserLogin(ref CThostFtdcRspUserLoginField pRspUserLogin, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer: OnRspUserLogin !bIsLast");
            }
            if (pRspInfo.ErrorID == 0)
            {
                string msg = "TradeApiCTP CtpDataServer: User " + pRspUserLogin.UserID + ", Password " + _LogWindow.pb_passWord.Password.ToString();
                Util.Log(msg);
                IsLoggedOn = true;
                if (pRspUserLogin.TradingDay != null && pRspUserLogin.TradingDay.Trim() != "")
                {
                    Util.Log("TradeApiCTP CtpDataServer: OnRspUserLogin - Current Trading Date: " + pRspUserLogin.TradingDay);
                    _TradingDay = pRspUserLogin.TradingDay;
                }
                AddToTradeDataQryQueue(new RequestContent("QuerySettlementInfoConfirm", new List<object>()));
                //OnLogon();
                LogonStruct logInfo = GetLogonReport(pRspUserLogin, "Trader");
                TradeDataClient.GetClientInstance().RtnQueryEnqueue(logInfo);
                //this.NotifyTimeChange(ExTime);
            }
            else
            {
                IsLoggedOn = false;
                Util.Log("Error! TradeApiCTP CtpDataServer pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                LogoffStruct logoffInfo = GetLogoffReport(pRspUserLogin, pRspInfo, "Trader");
                TradeDataClient.GetClientInstance().RtnQueryEnqueue(logoffInfo);
                //OnLogout(pRspInfo.ErrorMsg);
                //PasswordBroker.Broker(getLoginWindow().pb_passWord.Password.ToString());
            }
        }

        void CtpTraderApi_OnRspUserLogout(ref CThostFtdcUserLogoutField pUserLogout, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer: OnRspUserLogout !bIsLast");
            }
            IsLoggedOn = false;
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer Logout fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
            }
            else
            {
                Util.Log("Logout succeeds，UserID：" + pUserLogout.UserID);
                LogoffStruct logoffInfo = GetLogoffReport(pUserLogout, pRspInfo, "Trader");
                TradeDataClient.GetClientInstance().RtnQueryEnqueue(logoffInfo);
                //tradeApi.DisConnect();
                //tradeApi = null; //TODO: 被disconnect方法阻塞，无法重置对象
            }
        }

        void CtpTraderApi_OnRspQryInvestor(ref CThostFtdcInvestorField pInvestor, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP OnRspQryInvestor fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
            }
            else
            {
                Util.Log("====================================================");
                Util.Log("InvestorID: " + pInvestor.InvestorID);
                Util.Log("InvestorName: " + pInvestor.InvestorName);
                Util.Log("IdentifiedCardType: " + Enum.GetName(typeof(EnumThostIdCardTypeType), pInvestor.IdentifiedCardType));
                Util.Log("IdentifiedCardNo: " + pInvestor.IdentifiedCardNo);
                Util.Log("OpenDate: " + pInvestor.OpenDate);
                Util.Log("Address: " + pInvestor.Address);
                Util.Log("Telephone: " + pInvestor.Telephone);
                Util.Log("Mobile: " + pInvestor.Mobile);
                Util.Log("IsActive: " + pInvestor.IsActive);
                Util.Log("====================================================");
            }
        }


        void CtpTraderApi_OnRspQryTradingCode(ref CThostFtdcTradingCodeField pTradingCode, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP OnRspQryTradingCode fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
            }
            else
            {
                Util.Log("====================================================");
                Util.Log("ExchangeID: " + pTradingCode.ExchangeID);
                Util.Log("ClientIDType: " + Enum.GetName(typeof(EnumThostClientIDTypeType), pTradingCode.ClientIDType));
                Util.Log("ClientID: " + pTradingCode.ClientID);
                Util.Log("IsActive: " + pTradingCode.IsActive);
                if (bIsLast)
                {
                    Util.Log("====================================================");
                }
            }
        }

        void CtpTraderApi_OnRspQrySettlementInfoConfirm(ref CThostFtdcSettlementInfoConfirmField pSettlementInfoConfirm, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQrySettlementInfoConfirm fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
            }
            if (pSettlementInfoConfirm.ConfirmDate != "" && _TradingDay != String.Empty && int.Parse(pSettlementInfoConfirm.ConfirmDate) == int.Parse(_TradingDay))
            {
                Util.Log("TradeApiCTP CtpDataServer: SettlementInfo has been confirmed.");
                //InitDataFromAPI();
                AddToTradeDataQryQueue(new RequestContent("RequestContractCode", new List<object>()));
            }
            else if (pSettlementInfoConfirm.ConfirmDate != "" && _TradingDay != String.Empty && int.Parse(pSettlementInfoConfirm.ConfirmDate) > int.Parse(_TradingDay))
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQrySettlementInfoConfirm fails! System Time is not correct! Please check it.");
            }
            else
            {
                AddToTradeDataQryQueue(new RequestContent("RequestSettlementInstructions", new List<object>() { "" }));
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        if (_AffirmWindow == null)
                        {
                            _AffirmWindow = new StatementOrderAffirm();
                        }
                        _AffirmWindow.Show();
                    });
                }
                TradeDataClient.GetClientInstance().RtnQueryEnqueue(_AffirmWindow);
            }
        }

        void CtpTraderApi_OnRspQryInstrument(ref CThostFtdcInstrumentField pInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID == 0)
            {
                Contract tempContract = CodeSetManager.GetContractInfo(pInstrument.ExchangeInstID, pInstrument.ExchangeID);
                if (tempContract == null)
                {
                    Contract instrumentItem = GetContractInfoFromQuery(pInstrument);
                    CodeSetManager.ContractList.Add(instrumentItem);
                    CodeSetManager.ContractMap.Add(instrumentItem.Code + "_" + instrumentItem.ExchCode, instrumentItem);
                    if (pInstrument.ProductClass == EnumThostProductClassType.Options || pInstrument.ProductClass == EnumThostProductClassType.SpotOption) //TODO: ProductType cannot be null 
                    {
                        CodeSetManager.LstOptionCodes.Add(instrumentItem.Code);
                    }
                    GetSpecInfoFromInst(instrumentItem);
                }
            }
            else
            {
                Util.Log("Error! TradeApiCTP CtpDataServer pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
            }
            if (bIsLast)
            {
                CodeSetManager.ContractList.Sort(Contract.CompareByCode);
                foreach (string tempKey in CodeSetManager.SpeciesDict.Keys)
                {
                    CodeSetManager.SpeciesDict[tempKey].Codes.Sort(Contract.CompareByCode);
                }
                TradeDataClient.GetClientInstance().GenerateMainWindow();
                InitDataFromAPI();
                Util.Log("TradeApiCTP CtpDataServer: OnRspQryInstrument is received.");
            }
        }

        void CtpTraderApi_OnRspQryInstrumentMarginRate(ref CThostFtdcInstrumentMarginRateField pInstrumentMarginRate, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQryInstrumentMarginRate fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pInstrumentMarginRate.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pInstrumentMarginRate.InstrumentID);
            }
            else
            {
                string tempKey = pInstrumentMarginRate.InstrumentID;
                MarginStruct marginRate = GetMarginRateReport(pInstrumentMarginRate);
                Util.Log(String.Format("Margin Rate {0}: LongMarginRatioByMoney = {1},LongMarginRatioByVolume = {2}, ShortMarginRatioByMoney = {3}, ShortMarginRatioByVolume = {4}"
                    , tempKey, marginRate.LongMarginRatioByMoney, marginRate.LongMarginRatioByVolume, marginRate.ShortMarginRatioByMoney, marginRate.ShortMarginRatioByVolume));
                TradeDataClient.GetClientInstance().RtnPositionEnqueue(marginRate);
            }
        }

        void CtpTraderApi_OnRspQryInstrumentCommissionRate(ref CThostFtdcInstrumentCommissionRateField pInstrumentCommissionRate, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQryInstrumentCommissionRate fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pInstrumentCommissionRate.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pInstrumentCommissionRate.InstrumentID);
            }
            else
            {
                //Util.Log("TradeApiCTP CtpDataServer: OnRspQryInstrumentCommissionRate is received. Code: " + pInstrumentCommissionRate.InstrumentID);
                CommissionStruct commRate = GetCommissionRateReport(pInstrumentCommissionRate);
                Util.Log(String.Format("Commission Rate {0}: CloseRatioByMoney = {1},CloseRatioByVolume = {2}, CloseTodayRatioByMoney= {3}, CloseTodayRatioByVolume= {4}, OpenRatioByMoney = {5},OpenRatioByVolume = {6}"
                    , pInstrumentCommissionRate.InstrumentID, commRate.CloseRatioByMoney, commRate.CloseRatioByVolume, commRate.CloseTodayRatioByMoney, commRate.CloseTodayRatioByVolume
                    , commRate.OpenRatioByMoney, commRate.OpenRatioByVolume));
                TradeDataClient.GetClientInstance().RtnTradeEnqueue(commRate);
            }
        }

        void CtpTraderApi_OnRspQryOptionInstrCommRate(ref CThostFtdcOptionInstrCommRateField pOptionInstrCommRate, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQryInstrumentCommissionRate fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pOptionInstrCommRate.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pOptionInstrCommRate.InstrumentID);
            }
            else
            {
                Util.Log("TradeApiCTP CtpDataServer: OnRspQryInstrumentCommissionRate is received.");
                CommissionStruct commRate = GetOptionCommissionRateReport(pOptionInstrCommRate);
                Util.Log(String.Format("Option Commission Rate {0}: CloseRatioByMoney = {1},CloseRatioByVolume = {2}, CloseTodayRatioByMoney= {3}, CloseTodayRatioByVolume= {4}, OpenRatioByMoney = {5},OpenRatioByVolume = {6}, StrikeRatioByMoney = {7}, StrikeRatioByVolume = {8}"
                    , pOptionInstrCommRate.InstrumentID, commRate.CloseRatioByMoney, commRate.CloseRatioByVolume, commRate.CloseTodayRatioByMoney, commRate.CloseTodayRatioByVolume
                    , commRate.OpenRatioByMoney, commRate.OpenRatioByVolume, commRate.StrikeRatioByMoney, commRate.StrikeRatioByVolume));
                TradeDataClient.GetClientInstance().RtnTradeEnqueue(commRate);
            }
        }

        void CtpTraderApi_OnRspQryInvestorProductGroupMargin(ref CThostFtdcInvestorProductGroupMarginField pInvestorProductGroupMargin, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast)
            {
                Util.Log("TradeApiCTP CtpDataServer: OnRspQryInvestorProductGroupMargin is received. Code: " + pInvestorProductGroupMargin.InvestorID);
            }
        }

        void CtpTraderApi_OnRtnInstrumentStatus(ref CThostFtdcInstrumentStatusField pInstrumentStatus)
        {
            Util.Log("TradeApiCTP CtpDataServer: OnRtnInstrumentStatus is received: " + pInstrumentStatus.EnterTime + " " + CodeSetManager.CtpToExName(pInstrumentStatus.ExchangeID.Trim()) + " " + pInstrumentStatus.ExchangeInstID.Trim() + "：" + GetExchangeStatus(pInstrumentStatus.InstrumentStatus) + ", reason: " + pInstrumentStatus.EnterReason);
            TradeDataClient.GetClientInstance().RtnMessageEnqueue(CodeSetManager.CtpToExName(pInstrumentStatus.ExchangeID.Trim()) + "：" + GetExchangeStatus(pInstrumentStatus.InstrumentStatus));
        }

        void CtpTraderApi_OnRspQryBrokerTradingParams(ref CThostFtdcBrokerTradingParamsField pBrokerTradingParams, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            Util.Log(String.Format("MarginPriceType: {0}, Algorithm: {1}, AvailIncludeCloseProfit: {2}, OptionRoyaltyPriceType: {3}"
                , pBrokerTradingParams.MarginPriceType, pBrokerTradingParams.Algorithm, pBrokerTradingParams.AvailIncludeCloseProfit, pBrokerTradingParams.OptionRoyaltyPriceType));
            if (!bIsLast)
            { }
            else
            {
                _Algorithm = pBrokerTradingParams.Algorithm;
                _AvailIncludeCloseProfit = pBrokerTradingParams.AvailIncludeCloseProfit;
                _MarginPriceType = pBrokerTradingParams.MarginPriceType;
                _OptionRoyaltyPriceType = pBrokerTradingParams.OptionRoyaltyPriceType;
            }
        }

        void CtpTraderApi_OnRspQrySettlementInfo(ref CThostFtdcSettlementInfoField pSettlementInfo, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQrySettlementInfo fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                return;
            }

            if (pSettlementInfo.Content != null)// && pSettlementInfo.Content != String.Empty)
            {
                byte[] settleCharGroup = pSettlementInfo.Content;
                foreach (byte s in settleCharGroup)
                {
                    if (s != '\0')
                    {
                        SettlementInfo.SettlementInfoCharList.Add(s);
                    }
                }
            }
            else
            {
                Util.Log("Error! TradeApiCTP CtpDataServer: OnRspQrySettlementInfo Content is null!");
            }
            if (bIsLast)
            {
                Util.Log("TradeApiCTP CtpDataServer: OnRspQrySettlementInfo(last) is received.");
                SettlementInfo.BackEnd = _BackEnd;
                SettlementInfo.BrokerID = this._CtpTraderApi.BrokerID;
                SettlementInfo.InvestorID = this.InvestorID;
                SettlementInfo.UserID = this.InvestorID;
                TradeDataClient.GetClientInstance().RtnQueryEnqueue(SettlementInfo);
            }
        }

        void CtpTraderApi_OnRspSettlementInfoConfirm(ref CThostFtdcSettlementInfoConfirmField pSettlementInfoConfirm, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer: OnRspSettlementInfoConfirm(!bIsLast");
                return;
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspSettlementInfoConfirm fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg);
            }
            else
            {
                if (pSettlementInfoConfirm.ConfirmDate != "" && int.Parse(pSettlementInfoConfirm.ConfirmDate) == int.Parse(DateTime.Now.ToString("yyyyMMdd")))
                {
                    Util.Log("TradeApiCTP CtpDataServer: OnRspSettlementInfoConfirm - Today's settlement sheet has been confirmed.");
                }
                AddToTradeDataQryQueue(new RequestContent("RequestContractCode", new List<object>()));
            }
        }

        void CtpTraderApi_OnRspUserPasswordUpdate(ref CThostFtdcUserPasswordUpdateField pUserPasswordUpdate, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            Util.Log("TradeApiCTP CtpDataServer: OnRspUserPasswordUpdate is received.");
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer: OnRspUserPasswordUpdate !bIsLast");
                return;
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspUserPasswordUpdate fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg);
            }
            else
            {
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("账户" + pUserPasswordUpdate.UserID + "修改密码成功！");
                MessageBox.Show("修改密码成功！", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        void CtpTraderApi_OnRspQueryMaxOrderVolume(ref CThostFtdcQueryMaxOrderVolumeField pQueryMaxOrderVolume, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            Util.Log("TradeApiCTP CtpDataServer: OnRspQueryMaxOrderVolume is received.");
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer: OnRspQueryMaxOrderVolume !bIsLast");
                return;
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQueryMaxOrderVolume fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
            }
            else
            {
                MaxOperation maxOper = new MaxOperation();
                maxOper.CodeInfo = CodeSetManager.GetContractInfo(pQueryMaxOrderVolume.InstrumentID);
                maxOper.Count = pQueryMaxOrderVolume.MaxVolume;
                maxOper.PosEffect = GetPosEffectType (pQueryMaxOrderVolume.OffsetFlag);
                maxOper.Side = GetSideType(pQueryMaxOrderVolume.Direction);
                maxOper.HedgeType = GetHedgeType(pQueryMaxOrderVolume.HedgeFlag);
                ProcessMaxOperation(maxOper);
                CodeSetManager.MaxOperationHandCountDict[maxOper.CodeInfo] = pQueryMaxOrderVolume.MaxVolume;
            }
        }

        void CtpTraderApi_OnRspError(ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //针对用户请求的出错通知
            Util.Log("TradeApiCTP CtpDataServer: OnRspError is received.");
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer: OnRspError !bIsLast");
                return;
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspError! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg);
            }
        }

        void CtpTraderApi_OnRtnOrder(ref CThostFtdcOrderField pOrder)
        {
            Q7JYOrderData jyData = OrderExecutionReport(pOrder);
            Util.Log(String.Format("OnRtnOrder pOrder: Code {0}, BrokerOrderSeq {1}, OrderSysID {2}, OrderRef {3}, Status {4}, Hedge {5}",
                jyData.Code, jyData.BrokerOrderSeq, jyData.OrderID, jyData.OrderRef, jyData.FeedBackInfo, jyData.Hedge));
            if (TempOrderFlag) //接收同步的报单回报
            {
                TempOrderData.Add(jyData);
                Util.Log("TradeApiCTP CtpDataServer: OnRtnOrder is delayed.");
                return;
            }
            TradeDataClient.GetClientInstance().RtnOrderEnqueue(jyData);

            # region Re-Close Order
            // Declare for close and re-open
            List<PosInfoOrder> closeItemLst = new List<PosInfoOrder>();
            bool isClearOrder = false;
            PosInfoOrder openItem = null;

            // Trigger Re-Closing Order
            if (jyData.OpenClose.Contains("平") && jyData.OrderStatus.Contains("已撤单"))
            {
                lock (_CancelCloseLocker)
                {
                    foreach (PosInfoOrder item in _OrderAfterCancelList)
                    {
                        if (item.posInfo.Code == jyData.Code && item.BuySell != jyData.BuySell
                            && ((item.PositionEffect == PosEffect.Close && !jyData.OpenClose.Contains("平今")) || (item.PositionEffect == PosEffect.CloseToday && jyData.OpenClose.Contains("平今")))
                            )
                        {
                            string orderPosKey = item.posInfo.InvestorID + item.posInfo.Code + item.BuySell.Contains("买").ToString() + item.posInfo.Hedge;
                            if (TradeDataClient.GetClientInstance().PosTotalInfoMap.ContainsKey(orderPosKey))
                            {
                                TradeDataClient.GetClientInstance().PosTotalInfoMap[orderPosKey].FreezeCount -= jyData.UnTradeHandCount;
                            }

                            if (TradeDataClient.GetClientInstance().PosTotalInfoMap.ContainsKey(orderPosKey) && TradeDataClient.GetClientInstance().PosTotalInfoMap[orderPosKey].FreezeCount == 0)
                            {
                                SIDETYPE isbuy = item.BuySell.Contains("买") ? SIDETYPE.SELL : SIDETYPE.BUY;
                                PosEffect openClose = item.PositionEffect;
                                AddToOrderQueue(new RequestContent("NewOrderSingle", new List<object>() { CodeSetManager.GetContractInfo(item.posInfo.Code, CodeSetManager.ExNameToCtp(jyData.Exchange)), isbuy, openClose, item.Price, item.HandCount, 0, "", 0, 0, 0, item.OrderType, CommonUtil.GetHedgeType(item.posInfo.Hedge) }));
                                closeItemLst.Add(item);
                                isClearOrder = true; // Need?
                                break;
                            }
                            else
                            {
                                closeItemLst.Add(item); // Need?
                            }
                        }
                    }
                }
                if (isClearOrder)
                {
                    foreach (PosInfoOrder delItem in closeItemLst)
                    {
                        lock (_CancelCloseLocker)
                        {
                            if (_OrderAfterCancelList.Contains(delItem))
                            {
                                _OrderAfterCancelList.Remove(delItem);
                            }
                        }
                    }
                }
            }
            # endregion

            # region Reverse Open Order
            if (jyData.UnTradeHandCount == 0 && jyData.OpenClose.Contains("平"))
            {
                //string posKey = jyData.InvestorID + "_" + jyData.Code + "_" + (jyData.BuySell.Contains("买") ? "卖" : "买");
                lock (ResetOrderLocker)
                {
                    foreach (PosInfoOrder item in ResetOrderList)
                    {
                        if (ReOpenOrderDict.ContainsKey(item))
                        {
                            ReOpenOrderDict[item] -= jyData.TradeHandCount;
                            if (item.posInfo.Code == jyData.Code && item.BuySell != jyData.BuySell && ReOpenOrderDict[item] == 0)//item.HandCount == jyData.TradeHandCount)
                            {
                                SIDETYPE isBuy = item.BuySell.Contains("买") ? SIDETYPE.SELL : SIDETYPE.BUY;

                                AddToOrderQueue(new RequestContent("NewOrderSingle", new List<object>() { CodeSetManager.GetContractInfo(item.posInfo.Code, CodeSetManager.ExNameToCtp(jyData.Exchange)), isBuy, PosEffect.Open, item.Price, item.HandCount, 0, "", 0, 0, 0, item.OrderType, CommonUtil.GetHedgeType(item.posInfo.Hedge) }));
                                openItem = item;
                                break;
                            }
                        }
                    }
                }
                if (openItem != null && ResetOrderList.Contains(openItem))
                {
                    lock (ResetOrderLocker)
                    {
                        ResetOrderList.Remove(openItem);
                        if (ReOpenOrderDict.ContainsKey(openItem))
                        {
                            ReOpenOrderDict.Remove(openItem);
                        }
                    }
                }
            }
            # endregion
        }

        void CtpTraderApi_OnRtnTrade(ref CThostFtdcTradeField pTrade)
        {
            Q7JYOrderData tradedData = TradeExecutionReport(pTrade);
            Util.Log(String.Format("OnRtnTrade Code {0}, TradeID {1}, OrderSysID {2}, BrokerOrderSeq {3}, Volume {4}, Hedge {5}",
                tradedData.Code, tradedData.TradeID, tradedData.OrderID, tradedData.BrokerOrderSeq, tradedData.TradeHandCount, tradedData.Hedge));
            if (TempTradeFlag)
            {
                TempTradeData.Add(tradedData);
                Util.Log("TradeApiCTP CtpDataServer: OnRtnTrade is delayed.");
                return;
            }
            TradeDataClient.GetClientInstance().RtnTradeEnqueue(tradedData);
            if (!TempPosFlag)
            {
                TradeDataClient.GetClientInstance().RtnPositionEnqueue(tradedData);
            }
            //if (!_TradeProcessingFlag) _TradeProcessingFlag = true;
        }

        void CtpTraderApi_OnErrRtnOrderInsert(ref CThostFtdcInputOrderField pInputOrder, ref CThostFtdcRspInfoField pRspInfo)
        {
            //交易所认为报单错误(平仓单)
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnErrRtnOrderInsert! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pInputOrder.InstrumentID + " OrderRef:" + pInputOrder.OrderRef);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pInputOrder.InstrumentID);

                # region Clearing Remaining Re-opening Order

                List<PosInfoOrder> openItemLst = new List<PosInfoOrder>();
                if (Enum.GetName(typeof(EnumThostOffsetFlagType), pInputOrder.CombOffsetFlag_0).Contains("Close"))
                {
                    lock (ResetOrderLocker)
                    {
                        foreach (PosInfoOrder item in ResetOrderList)
                        {
                            //if (item.posInfo.Code == jyData.Code && item.BuySell != jyData.BuySell)
                            if (item.posInfo.Code == pInputOrder.InstrumentID //&& item.HandCount == pInputOrder.VolumeTotalOriginal cannot be used for the existance of Close_Today // Not support ExchangeID content!
                                && ((item.BuySell.Contains("买") && pInputOrder.Direction == EnumThostDirectionType.Sell) || (item.BuySell.Contains("卖") && pInputOrder.Direction == EnumThostDirectionType.Buy))
                                )
                            {
                                openItemLst.Add(item);
                            }
                        }
                        foreach (PosInfoOrder openItem in openItemLst)
                        {
                            ResetOrderList.Remove(openItem);
                            //string posKey = openItem.posInfo.Code + "_" + openItem.BuySell;
                            if (ReOpenOrderDict.ContainsKey(openItem))
                            {
                                ReOpenOrderDict.Remove(openItem);
                            }
                        }
                    }
                }
                # endregion
            }
        }

        void CtpTraderApi_OnRspOrderInsert(ref CThostFtdcInputOrderField pInputOrder, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //没有没有通过参数校验，拒绝接受报单指令
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer: OnRspOrderInsert !bIsLast");
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspOrderInsert! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pInputOrder.InstrumentID + " OrderRef:" + pInputOrder.OrderRef);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pInputOrder.InstrumentID);

                # region Clearing Remaining Re-opening Order

                List<PosInfoOrder> openItemLst = new List<PosInfoOrder>();
                //foreach (Q7JYOrderData jyData in _JYOrderData)
                //if (jyData.OpenClose.Contains("平") && pInputOrder.OrderRef == jyData.OrderRef)
                if (Enum.GetName(typeof(EnumThostOffsetFlagType), pInputOrder.CombOffsetFlag_0).Contains("Close"))
                {
                    lock (ResetOrderLocker)
                    {
                        foreach (PosInfoOrder item in ResetOrderList)
                        {
                            //if (item.posInfo.Code == jyData.Code && item.BuySell != jyData.BuySell)
                            if (item.posInfo.Code == pInputOrder.InstrumentID //&& item.HandCount == pInputOrder.VolumeTotalOriginal cannot be used for the existance of Close_Today // Not support ExchangeID con
                                && ((item.BuySell.Contains("买") && pInputOrder.Direction == EnumThostDirectionType.Sell) || (item.BuySell.Contains("卖") && pInputOrder.Direction == EnumThostDirectionType.Buy))
                                )
                            {
                                openItemLst.Add(item);
                            }
                        }
                        foreach (PosInfoOrder openItem in openItemLst)
                        {
                            ResetOrderList.Remove(openItem);
                            //string posKey = openItem.posInfo.Code + "_" + openItem.BuySell;
                            if (ReOpenOrderDict.ContainsKey(openItem))
                            {
                                ReOpenOrderDict.Remove(openItem);
                            }
                        }
                    }
                }
                #endregion
            }
        }

        void CtpTraderApi_OnErrRtnOrderAction(ref CThostFtdcOrderActionField pOrderAction, ref CThostFtdcRspInfoField pRspInfo)
        {
            //撤单报单错误
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnErrRtnOrderAction! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pOrderAction.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pOrderAction.InstrumentID);
            }
        }

        void CtpTraderApi_OnRspOrderAction(ref CThostFtdcInputOrderActionField pInputOrderAction, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //没有通过参数校验，拒绝接受撤单指令
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer: OnRspOrderAction !bIsLast");
                return;
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspOrderAction! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pInputOrderAction.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pInputOrderAction.InstrumentID);
            }
        }

        void CtpTraderApi_OnRspQryOrder(ref CThostFtdcOrderField pOrder, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQryOrder! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pOrder.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pOrder.InstrumentID);
            }
            else
            {
                if (bIsLast && QryOrderDataDic.Count == 0 && pOrder.InstrumentID.Trim() == "" && pOrder.BrokerID == "") //排除交易日无委托时CTP推送的无用的初始化信息
                {
                    Util.Log("TradeApiCTP CtpDataServer: OnRspQryOrder(last) is received.");
                    TempOrderFlag = false;
                    return;
                }
                Q7JYOrderData jyData = OrderExecutionReport(pOrder);
                string orderKey = jyData.BrokerOrderSeq + "_" + jyData.OrderRef + "_" + jyData.Exchange;
                if (QryOrderDataDic.ContainsKey(orderKey))
                {
                    QryOrderDataDic[orderKey] = jyData;
                }
                else
                {
                    QryOrderDataDic.Add(orderKey, jyData);
                }
                if (bIsLast)
                {
                    Util.Log("TradeApiCTP CtpDataServer: OnRspQryOrder(last) is received.");
                    TempOrderFlag = false;
                    if (TempOrderData.Count > 0)
                    {
                        foreach (Q7JYOrderData tempData in TempOrderData)
                        {
                            if (String.IsNullOrEmpty(tempData.Name) || tempData.Name == "")
                            {
                                Contract tempContract = CodeSetManager.GetContractInfo(tempData.Code, CodeSetManager.ExNameToCtp(tempData.Exchange));
                                if (tempContract != null)
                                {
                                    tempData.Name = tempContract.Name;
                                }
                            }
                            string tempKey = jyData.BrokerOrderSeq + "_" + jyData.OrderRef + "_" + jyData.Exchange;
                            if (QryOrderDataDic.ContainsKey(tempKey))
                            {
                                QryOrderDataDic[tempKey] = tempData;
                            }
                            else
                            {
                                QryOrderDataDic.Add(tempKey, tempData);
                            }
                            //ProcessNewComeOrderInfo(tempData);
                        }
                    }
                    List<Q7JYOrderData> qryOrderLst = new List<Q7JYOrderData>();
                    foreach (string key in QryOrderDataDic.Keys)
                    {
                        qryOrderLst.Add(QryOrderDataDic[key]);
                    }
                    qryOrderLst.Sort(Q7JYOrderData.CompareByCommitTime);
                    TradeDataClient.GetClientInstance().RtnOrderEnqueue(qryOrderLst);
                    TempOrderData.Clear();
                }
            }
        }

        void CtpTraderApi_OnRspQryTrade(ref CThostFtdcTradeField pTrade, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQryOrder! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pTrade.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pTrade.InstrumentID);
            }
            else
            {
                if (bIsLast && QryTradeDataLst.Count == 0 && pTrade.InstrumentID.Trim() == "" && pTrade.BrokerID == "") //排除交易日无委托时CTP推送的无用的初始化信息
                {
                    Util.Log("TradeApiCTP CtpDataServer: OnRspQryTrade(last) is received.");
                    TempTradeFlag = false;
                    return;
                }
                Q7JYOrderData jyData = TradeExecutionReport(pTrade);
                QryTradeDataLst.Add(jyData);
                if (bIsLast)
                {
                    Util.Log("TradeApiCTP CtpDataServer: OnRspQryTrade(last) is received.");
                    TempTradeFlag = false;
                    if (TempTradeData.Count > 0)
                    {
                        foreach (Q7JYOrderData tempData in TempTradeData)
                        {
                            if (String.IsNullOrEmpty(tempData.Name) || tempData.Name == "")
                            {
                                Contract tempContract = CodeSetManager.GetContractInfo(tempData.Code, CodeSetManager.ExNameToCtp(tempData.Exchange));
                                if (tempContract != null)
                                {
                                    tempData.Name = tempContract.Name;
                                }
                            }
                            ProcessNewComeTradeInfo(tempData, false);
                            QryTradeDataLst.Add(tempData);
                        }
                    }
                    QryTradeDataLst.Sort(Q7JYOrderData.CompareByTradeTime);
                    //ProcessTradedOrderData(QryTradeDataLst);
                    TradeDataClient.GetClientInstance().RtnTradeEnqueue(QryTradeDataLst);
                    TempTradeData.Clear();
                }
            }
        }

        void CtpTraderApi_OnRspQryInvestorPosition(ref CThostFtdcInvestorPositionField pInvestorPosition, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast && QryPosDetailData.Count == 0 && pInvestorPosition.InstrumentID.Trim() == "" && pInvestorPosition.BrokerID == "") //排除交易日无委托时CTP推送的无用的初始化信息
            {
                return;
            }
            Q7PosInfoTotal posDetail = PositionExecutionReport(pInvestorPosition);
            JYPosSumData.Add(posDetail);
            if (bIsLast)
            {
                Util.Log("TradeApiCTP CtpDataServer: OnRspQryInvestorPosition(last) is received.");
                lock (ServerLock)
                {
                    JYPosSumData.Sort(Q7PosInfoTotal.CompareByCode);
                    //ProcessPositions_Total(JYPosSumData);
                }
            }
        }

        void CtpTraderApi_OnRspQryInvestorPositionDetail(ref CThostFtdcInvestorPositionDetailField pInvestorPositionDetail, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast && QryPosDetailData.Count == 0 && pInvestorPositionDetail.InstrumentID.Trim() == "" && pInvestorPositionDetail.BrokerID == "") //排除交易日无委托时CTP推送的无用的初始化信息
            {
                TempPosFlag = false;
                return;
            }
            Q7PosInfoDetail posDetail = PosDetailExecutionReport(pInvestorPositionDetail);
            QryPosDetailData.Add(posDetail);
            if (bIsLast)
            {
                Util.Log("TradeApiCTP CtpDataServer: OnRspQryInvestorPositionDetail(last) is received.");
                lock (ServerLock)
                {
                    QryPosDetailData.Sort(Q7PosInfoDetail.CompareByExecID);
                    TradeDataClient.GetClientInstance().RtnPositionEnqueue(QryPosDetailData);
                    //ProcessPositions(QryPosDetailData);
                }
                TempPosFlag = false;
            }
        }

        void CtpTraderApi_OnRspQryInvestorPositionCombineDetail(ref CThostFtdcInvestorPositionCombineDetailField pInvestorPositionCombineDetail, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast && QryPosDetailData.Count == 0 && pInvestorPositionCombineDetail.InstrumentID.Trim() == "" && pInvestorPositionCombineDetail.BrokerID == "") //排除交易日无委托时CTP推送的无用的初始化信息
            {
                return;
            }
            //Q7PosInfoDetail posDetail = PosDetailExecutionReport(pInvestorPositionDetail);
            //jyPosDetailData.Add(posDetail);
            if (bIsLast)
            {
                Util.Log("TradeApiCTP CtpDataServer: OnRspQryInvestorPositionCombineDetail(last) is received.");
                lock (ServerLock)
                {
                    //jyPosDetailData.Sort(Q7PosInfoDetail.CompareByExecID);
                    //ProcessPositions(jyPosDetailData);
                }
            }
        }

        void CtpTraderApi_OnRspQryTradingAccount(ref CThostFtdcTradingAccountField pTradingAccount, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            Util.Log("TradeApiCTP CtpDataServer: OnRspQryTradingAccount is received.");
            if (!bIsLast)
            {
                Util.Log("OnRspQryTradingAccount !bIsLast");
                return;
            }
            CapitalInfo capDetail = CapitalDetailExecutionReport(pTradingAccount);
            if (bIsLast)
            {
                TradeDataClient.GetClientInstance().RtnQueryEnqueue(capDetail);
            }
        }

        void CtpTraderApi_OnRtnTradingNotice(ref CThostFtdcTradingNoticeInfoField pTradingNoticeInfo)
        {
            Util.Log("TradeApiCTP CtpDataServer: OnRtnTradingNotice is received. " + pTradingNoticeInfo.InvestorID + "：" + pTradingNoticeInfo.FieldContent);
            if (pTradingNoticeInfo.FieldContent.Trim() != "")
            {
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("用户" + pTradingNoticeInfo.InvestorID + "：" + pTradingNoticeInfo.FieldContent);
                string acct = pTradingNoticeInfo.InvestorID;
                string msg = pTradingNoticeInfo.FieldContent;
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show("用户" + acct + "：" + msg, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
        }

        void CtpTraderApi_OnRspParkedOrderAction(ref CThostFtdcParkedOrderActionField pParkedOrderAction, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID == 0)
            {
                //AddToQryQueue(new CTPRequestContent("QryPreOrder", new List<object>()));
                AddToTradeDataQryQueue(new RequestContent("QryPreCancelOrder", new List<object>()));
                Util.Log("TradeApiCTP CtpDataServer: OnRspParkedOrderAction is received.");
            }
            else
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspParkedOrderAction fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("操作失败！" + pRspInfo.ErrorMsg);
            }
        }

        void CtpTraderApi_OnRspParkedOrderInsert(ref CThostFtdcParkedOrderField pParkedOrder, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            Util.Log("TradeApiCTP CtpDataServer: OnRspParkedOrderInsert is received.");
            if (pRspInfo.ErrorID == 0)
            {
                Q7JYOrderData parkedOrder = GetParkedOrderExcutionReport(pParkedOrder);
                PreOrderData.Add(parkedOrder);
                if (bIsLast)
                {
                    Util.Log("TradeApiCTP CtpDataServer: OnRspParkedOrderInsert(last) is received.");
                    lock (ServerLock)
                    {
                        foreach (Q7JYOrderData pOrder in PreOrderData)
                        {
                            //ProcessNewComeParkedOrderInfo(pOrder);
                        }
                        PreOrderData.Sort(Q7JYOrderData.CompareByCommitTime);
                        ProcessParkedOrderData(PreOrderData);
                    }
                }
            }
            else
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspParkedOrderInsert fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("操作失败！" + pRspInfo.ErrorMsg);
            }
        }

        void CtpTraderApi_OnRspQryParkedOrder(ref CThostFtdcParkedOrderField pParkedOrder, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast && pParkedOrder.InstrumentID.Trim() == "" && pParkedOrder.BrokerID == "") //排除交易日无委托时CTP推送的无用的初始化信息
            {
                Util.Log("TradeApiCTP CtpDataServer: OnRspQryParkedOrder is received.");
                TempOrderFlag = false;
                return;
            }
            Q7JYOrderData parkedOrder = GetParkedOrderExcutionReport(pParkedOrder);
            PreOrderData.Add(parkedOrder);
            if (bIsLast)
            {
                Util.Log("TradeApiCTP CtpDataServer: OnRspQryParkedOrder(last) is received.");
                lock (ServerLock)
                {
                    PreOrderData.Sort(Q7JYOrderData.CompareByCommitTime);
                    ProcessParkedOrderData(PreOrderData);
                }
            }
        }

        void CtpTraderApi_OnRspQryParkedOrderAction(ref CThostFtdcParkedOrderActionField pParkedOrderAction, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast && pParkedOrderAction.InstrumentID.Trim() == "" && pParkedOrderAction.BrokerID == "") //排除交易日无委托时CTP推送的无用的初始化信息
            {
                Util.Log("TradeApiCTP CtpDataServer: OnRspQryParkedOrderAction is received.");
                TempOrderFlag = false;
                return;
            }
            Q7JYOrderData parkedOrder = GetParkedOrderActionExcutionReport(pParkedOrderAction);
            PreOrderData.Add(parkedOrder);
            if (bIsLast)
            {
                Util.Log("TradeApiCTP CtpDataServer: OnRspQryParkedOrder(last) is received.");
                lock (ServerLock)
                {
                    PreOrderData.Sort(Q7JYOrderData.CompareByCommitTime);
                    ProcessParkedOrderData(PreOrderData);
                }
            }
        }

        void CtpTraderApi_OnRspRemoveParkedOrder(ref CThostFtdcRemoveParkedOrderField pRemoveParkedOrder, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID == 0)
            {
                AddToTradeDataQryQueue(new RequestContent("QryPreOrder", new List<object>()));
                Util.Log("TradeApiCTP CtpDataServer: OnRspRemoveParkedOrder(last) is received.");
            }
            else
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspRemoveParkedOrder fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("操作失败！" + pRspInfo.ErrorMsg);
            }
            //foreach (Q7JYOrderData removePreOrder in preOrderData)
            //{
            //    if (removePreOrder.OrderID == pRemoveParkedOrder.ParkedOrderID)
            //    {
            //        removePreOrder.OrderStatus = "已撤单";
            //    }
            //}
            //if (bIsLast)
            //{
            //    Util.Log("TradeApiCTP CtpDataServer: OnRspRemoveParkedOrder(last) is received.");
            //    preOrderData.Sort(Q7JYOrderData.CompareByCommitTime);
            //    ProcessParkedOrderData(preOrderData);
            //}
        }

        void CtpTraderApi_OnRspRemoveParkedOrderAction(ref CThostFtdcRemoveParkedOrderActionField pRemoveParkedOrderAction, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID == 0)
            {
                Util.Log("TradeApiCTP CtpDataServer: OnRspRemoveParkedOrderAction(last) is received.");
            }
            else
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspRemoveParkedOrderAction fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("操作失败！" + pRspInfo.ErrorMsg);
            }
            Util.Log("TradeApiCTP CtpDataServer: OnRspRemoveParkedOrderAction is received.");
        }

        void CtpTraderApi_OnRspQryProductExchRate(ref CThostFtdcProductExchRateField pProductExchRate, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
        }

        void CtpTraderApi_OnRspQryExchangeRate(ref CThostFtdcExchangeRateField pExchangeRate, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
        }

        void CtpTraderApi_OnRspQryContractBank(ref CThostFtdcContractBankField pContractBank, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQryContractBank fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg);
            }
            else
            {
                if (BankManager.GetBankIdFromName(pContractBank.BankName) == null)
                {
                    ContractBank bankItem = ContractBanksReport(pContractBank);
                    BankManager.ContractBanks.Add(bankItem);
                }
            }
            //查询银行结束后查询用户银行账户
            if (bIsLast)
            {
                AddToTradeDataQryQueue(new RequestContent("QryAccountRegister", new List<object>()));
            }
        }

        void CtpTraderApi_OnRspQryAccountregister(ref CThostFtdcAccountregisterField pAccountregister, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQryAccountregister fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg);
            }
            else
            {
                BankAccountInfo acctInfo = GetBankAccountInfoReport(pAccountregister);
                QryBankAcctInfoLst.Add(acctInfo);
                if (bIsLast)
                {
                    Util.Log("TradeApiCTP CtpDataServer: OnRspQryAccountregister, " + QryBankAcctInfoLst.Count + " items are received.");
                    TradeDataClient.GetClientInstance().RtnQueryEnqueue(QryBankAcctInfoLst);
                }
            }
        }

        void CtpTraderApi_OnRspTradingAccountPasswordUpdate(ref CThostFtdcTradingAccountPasswordUpdateField pTradingAccountPasswordUpdate, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            Util.Log("TradeApiCTP CtpDataServer: OnRspTradingAccountPasswordUpdate is received.");
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer: OnRspTradingAccountPasswordUpdate !bIsLast");
                return;
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspTradingAccountPasswordUpdate fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg);
            }
            else
            {
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("账户" + pTradingAccountPasswordUpdate.AccountID + "修改密码成功！");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show("修改密码成功！", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                    });
                }
            }
        }

        void CtpTraderApi_OnRspQryTransferSerial(ref CThostFtdcTransferSerialField pTransferSerial, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQryTransferSerial fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg);
            }
            else
            {
                Util.Log("TradeApiCTP CtpDataServer: OnRspQryTransferSerial is received.");
                if (pTransferSerial.FutureSerial > 0)
                {
                    TransferSingleRecord tranRec = TransferSingleReport(pTransferSerial);
                    QryTransferRecords.Add(tranRec);
                }
            }
            if (bIsLast)
            {
                TradeDataClient.GetClientInstance().RtnQueryEnqueue(QryTransferRecords);
            }
        }

        void CtpTraderApi_OnRspQueryBankAccountMoneyByFuture(ref CThostFtdcReqQueryAccountField pReqQueryAccount, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            Util.Log("TradeApiCTP CtpDataServer: OnRspQueryBankAccountMoneyByFuture is received.");
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQueryBankAccountMoneyByFuture fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                string msg = pRspInfo.ErrorMsg;
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(msg);
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show(msg, "银期操作失败", MessageBoxButton.OK, MessageBoxImage.Warning); ;
                    });
                }
            }
            else
            {
                Util.Log("TradeApiCTP CtpDataServer OnRspQueryBankAccountMoneyByFuture Bank Account:" + pReqQueryAccount.BankAccount + " BankSerial:" + pReqQueryAccount.BankSerial);
            }
        }

        void CtpTraderApi_OnRtnQueryBankBalanceByFuture(ref CThostFtdcNotifyQueryAccountField pNotifyQueryAccount)
        {
            if (pNotifyQueryAccount.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRtnQueryBankBalanceByFuture fails! pRspInfo: ID:" + pNotifyQueryAccount.ErrorID + " ErrorMsg:" + pNotifyQueryAccount.ErrorMsg);
                string msg = pNotifyQueryAccount.ErrorMsg;
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(msg);
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show(msg, "银期操作失败", MessageBoxButton.OK, MessageBoxImage.Warning); ;
                    });
                }

            }
            else
            {
                Util.Log("TradeApiCTP CtpDataServer: OnRtnQueryBankBalanceByFuture is received.");
                BankAcctDetail bankAcctDetail = AcctDetailReport(pNotifyQueryAccount);
                TradeDataClient.GetClientInstance().RtnQueryEnqueue(bankAcctDetail);
            }
        }

        void CtpTraderApi_OnErrRtnQueryBankBalanceByFuture(ref CThostFtdcReqQueryAccountField pReqQueryAccount, ref CThostFtdcRspInfoField pRspInfo)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQueryBankAccountMoneyByFuture fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                string msg = pRspInfo.ErrorMsg;
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(msg);
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show(msg, "银期操作失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
        }

        void CtpTraderApi_OnRspFromFutureToBankByFuture(ref CThostFtdcReqTransferField pReqTransfer, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspFromFutureToBankByFuture fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                //if (pReqTransfer.FutureSerial > 0)
                //{
                //    TransferSingleRecord transferRec = NewIncomingTransferReport(pReqTransfer, pRspInfo);
                //    ProcessNewTransferRecords(transferRec);
                //}

                string msg = pRspInfo.ErrorMsg;
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(msg);
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show(msg, "银期操作失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else
            {
                Util.Log("TradeApiCTP CtpDataServer: OnRspFromFutureToBankByFuture is received.");
            }
        }

        void CtpTraderApi_OnRtnFromFutureToBankByFuture(ref CThostFtdcRspTransferField pRspTransfer)
        {
            if (pRspTransfer.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRtnQueryBankBalanceByFuture fails! pRspInfo: ID:" + pRspTransfer.ErrorID + " ErrorMsg:" + pRspTransfer.ErrorMsg);
                string msg = pRspTransfer.ErrorMsg;
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspTransfer.ErrorMsg);
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show(msg, "银期操作失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else
            {
                Util.Log("TradeApiCTP CtpDataServer: OnRtnFromFutureToBankByFuture is received.");
                TransferSingleRecord transferRec = NewIncomingTransferReport(pRspTransfer);
                TradeDataClient.GetClientInstance().RtnQueryEnqueue(transferRec);
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show("转账成功", "转账成功", MessageBoxButton.OK, MessageBoxImage.Information);
                    });
                }
            }
        }

        void CtpTraderApi_OnErrRtnFutureToBankByFuture(ref CThostFtdcReqTransferField pReqTransfer, ref CThostFtdcRspInfoField pRspInfo)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnErrRtnFutureToBankByFuture fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                string msg = pRspInfo.ErrorMsg;
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg);
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show(msg, "转账失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
        }

        void CtpTraderApi_OnRspFromBankToFutureByFuture(ref CThostFtdcReqTransferField pReqTransfer, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspFromBankToFutureByFuture fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                //if (pReqTransfer.FutureSerial > 0)
                //{
                //    TransferSingleRecord transferRec = NewIncomingTransferReport(pReqTransfer, pRspInfo);
                //    ProcessNewTransferRecords(transferRec);
                //}

                string msg = pRspInfo.ErrorMsg;
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg);
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show(msg, "银期操作失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else
            {
                Util.Log("TradeApiCTP CtpDataServer: OnRspFromBankToFutureByFuture is received.");
            }
        }

        void CtpTraderApi_OnRtnFromBankToFutureByFuture(ref CThostFtdcRspTransferField pRspTransfer)
        {
            if (pRspTransfer.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRtnFromBankToFutureByFuture fails! pRspInfo: ID:" + pRspTransfer.ErrorID + " ErrorMsg:" + pRspTransfer.ErrorMsg);
                string msg = pRspTransfer.ErrorMsg;
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspTransfer.ErrorMsg);
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show(msg, "银期操作失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else
            {
                Util.Log("TradeApiCTP CtpDataServer: OnRtnFromBankToFutureByFuture is received.");
                TransferSingleRecord transferRec = NewIncomingTransferReport(pRspTransfer);
                TradeDataClient.GetClientInstance().RtnQueryEnqueue(transferRec);
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show("转账成功", "转账成功", MessageBoxButton.OK, MessageBoxImage.Information);
                    });
                }
            }
        }

        void CtpTraderApi_OnErrRtnBankToFutureByFuture(ref CThostFtdcReqTransferField pReqTransfer, ref CThostFtdcRspInfoField pRspInfo)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnErrRtnBankToFutureByFuture fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                string msg = pRspInfo.ErrorMsg;
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg);
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show(msg, "转账失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
        }

        void CtpTraderApi_OnRspQryOptionInstrTradeCost(ref CThostFtdcOptionInstrTradeCostField pOptionInstrTradeCost, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQryOptionInstrTradeCost! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pOptionInstrTradeCost.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pOptionInstrTradeCost.InstrumentID);
            }
            else
            {
                MarginStruct optMarginRate = GetOptionMarginRateReport(pOptionInstrTradeCost);
                Util.Log(String.Format("Option Margin {0}: FixedMargin = {1}, MiniMargin = {2}, Royalty = {3}",
                    pOptionInstrTradeCost.InstrumentID, optMarginRate.FixedMargin, optMarginRate.MiniMargin, optMarginRate.Royalty));
                TradeDataClient.GetClientInstance().RtnPositionEnqueue(optMarginRate);
            }
        }

        void CtpTraderApi_OnRspQryExecOrder(ref CThostFtdcExecOrderField pExecOrder, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQryExecOrder! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pExecOrder.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pExecOrder.InstrumentID);
            }
            else
            {
                if (bIsLast && QryExecOrderDataLst.Count == 0 && pExecOrder.InstrumentID.Trim() == "" && pExecOrder.BrokerID == "") //排除交易日无委托时CTP推送的无用的初始化信息
                {
                    Util.Log("TradeApiCTP CtpDataServer: OnRspQryExecOrder(last) is received.");
                    TempExecFlag = false;
                    return;
                }
                ExecOrderData execData = OptionExecutionReport(pExecOrder);
                string execKey = execData.BrokerExecOrderSeq + "_" + execData.ExecOrderRef + "_" + execData.Exchange;
                if (QryExecOrderDataDic.ContainsKey(execKey))
                {
                    QryExecOrderDataDic[execKey] = execData;
                }
                else
                {
                    QryExecOrderDataDic.Add(execKey, execData);
                }
                QryExecOrderDataLst.Add(execData);
                if (bIsLast)
                {
                    Util.Log("TradeApiCTP CtpDataServer: OnRspQryExecOrder(last) is received.");
                    TempExecFlag = false;
                    if (TempExecData.Count > 0)
                    {
                        foreach (ExecOrderData tempData in TempExecData)
                        {
                            if (String.IsNullOrEmpty(tempData.Name) || tempData.Name == "")
                            {
                                Contract tempContract = CodeSetManager.GetContractInfo(tempData.Code, CodeSetManager.ExNameToCtp(tempData.Exchange));
                                if (tempContract != null)
                                {
                                    tempData.Name = tempContract.Name;
                                }
                            }
                            string tempKey = tempData.BrokerExecOrderSeq + "_" + tempData.ExecOrderRef + "_" + tempData.Exchange;
                            if (QryExecOrderDataDic.ContainsKey(tempKey))
                            {
                                QryExecOrderDataDic[tempKey] = tempData;
                            }
                            else
                            {
                                QryExecOrderDataDic.Add(tempKey, tempData);
                            }
                        }
                    }
                    QryExecOrderDataLst.Clear();
                    foreach (string key in QryExecOrderDataDic.Keys)
                    {
                        QryExecOrderDataLst.Add(QryExecOrderDataDic[key]);
                    }
                    QryExecOrderDataLst.Sort(ExecOrderData.CompareByCommitTime);
                    //ProcessExecInsertOrderData(QryExecOrderDataLst);
                    TradeDataClient.GetClientInstance().RtnOrderEnqueue(QryExecOrderDataLst);
                    TempExecData.Clear();
                }
            }
        }

        void CtpTraderApi_OnRtnExecOrder(ref CThostFtdcExecOrderField pExecOrder)
        {
            ExecOrderData execData = OptionExecutionReport(pExecOrder);
            if (TempExecFlag) //接收同步的执行回报
            {
                TempExecData.Add(execData);
                Util.Log("TradeApiCTP CtpDataServer: OnRtnExecOrder is delayed.");
                return;
            }
            TradeDataClient.GetClientInstance().RtnOrderEnqueue(execData);
        }

        void CtpTraderApi_OnRspExecOrderInsert(ref CThostFtdcInputExecOrderField pInputExecOrder, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer: OnRspExecOrderInsert !bIsLast");
                return;
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspExecOrderInsert! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pInputExecOrder.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pInputExecOrder.InstrumentID);
            }
        }

        void CtpTraderApi_OnErrRtnExecOrderInsert(ref CThostFtdcInputExecOrderField pInputExecOrder, ref CThostFtdcRspInfoField pRspInfo)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnErrRtnExecOrderInsert! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pInputExecOrder.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pInputExecOrder.InstrumentID);
            }
        }

        void CtpTraderApi_OnRspExecOrderAction(ref CThostFtdcInputExecOrderActionField pInputExecOrderAction, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer: OnRspExecOrderAction !bIsLast");
                return;
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspExecOrderAction! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pInputExecOrderAction.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pInputExecOrderAction.InstrumentID);
            }
        }

        void CtpTraderApi_OnErrRtnExecOrderAction(ref CThostFtdcExecOrderActionField pExecOrderAction, ref CThostFtdcRspInfoField pRspInfo)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnErrRtnExecOrderAction! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pExecOrderAction.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pExecOrderAction.InstrumentID);
            }
        }

        void CtpTraderApi_OnRspQryForQuote(ref CThostFtdcForQuoteField pForQuote, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            Util.Log("TradeApiCTP CtpDataServer: OnRspQryForQuote is received.");
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQryForQuote! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pForQuote.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pForQuote.InstrumentID);
            }
        }

        void CtpTraderApi_OnRtnForQuoteRsp(ref CThostFtdcForQuoteRspField pForQuoteRsp)
        {
            Util.Log(String.Format("TradeApiCTP CtpDataServer: OnRtnForQuoteRsp is received. InstrumentID = {0}, ForQuoteSysID = {1}, ForQuoteTime = {2} {3}", pForQuoteRsp.InstrumentID, pForQuoteRsp.ForQuoteSysID, pForQuoteRsp.TradingDay, pForQuoteRsp.ForQuoteTime));
        }

        void CtpTraderApi_OnRspForQuoteInsert(ref CThostFtdcInputForQuoteField pInputForQuote, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer: OnRspForQuoteInsert !bIsLast");
                return;
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspForQuoteInsert! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pInputForQuote.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pInputForQuote.InstrumentID);
            }
        }

        void CtpTraderApi_OnErrRtnForQuoteInsert(ref CThostFtdcInputForQuoteField pInputForQuote, ref CThostFtdcRspInfoField pRspInfo)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnErrRtnForQuoteInsert! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pInputForQuote.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pInputForQuote.InstrumentID);
            }
        }

        void CtpTraderApi_OnRspQryQuote(ref CThostFtdcQuoteField pQuote, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQryQuote! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pQuote.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pQuote.InstrumentID);
            }
            else
            {
                if (bIsLast && QryQuoteOrderDataLst.Count == 0 && pQuote.InstrumentID.Trim() == "" && pQuote.BrokerID == "") //排除交易日无委托时CTP推送的无用的初始化信息
                {
                    Util.Log("TradeApiCTP CtpDataServer: OnRspQryQuote(last) is received.");
                    TempQuoteInsertFlag = false;
                    return;
                }
                QuoteOrderData qData = QuoteOrderExecutionReport(pQuote);
                string quoteKey = qData.BrokerQuoteSeq + "_" + qData.QuoteRef + "_" + qData.Exchange;
                if (QryQuoteOrderDataDic.ContainsKey(quoteKey))
                {
                    QryQuoteOrderDataDic[quoteKey] = qData;
                }
                else
                {
                    QryQuoteOrderDataDic.Add(quoteKey, qData);
                }
                QryQuoteOrderDataLst.Add(qData);
                if (bIsLast)
                {
                    Util.Log("TradeApiCTP CtpDataServer: OnRspQryQuote(last) is received.");
                    TempQuoteInsertFlag = false;
                    if (TempQuoteOrderData.Count > 0)
                    {
                        foreach (QuoteOrderData tempData in TempQuoteOrderData)
                        {
                            if (String.IsNullOrEmpty(tempData.Name) || tempData.Name == "")
                            {
                                Contract tempContract = CodeSetManager.GetContractInfo(tempData.Code, CodeSetManager.ExNameToCtp(tempData.Exchange));
                                if (tempContract != null)
                                {
                                    tempData.Name = tempContract.Name;
                                }
                            }
                            string tempKey = tempData.BrokerQuoteSeq + "_" + tempData.QuoteRef + "_" + tempData.Exchange;
                            if (QryQuoteOrderDataDic.ContainsKey(tempKey))
                            {
                                QryQuoteOrderDataDic[tempKey] = tempData;
                            }
                            else
                            {
                                QryQuoteOrderDataDic.Add(tempKey, tempData);
                            }
                            //ProcessNewComeQuoteOrderInfo(tempData);
                            //_QuoteOrderDataLst.Add(tempData);
                        }
                    }
                    QryQuoteOrderDataLst.Clear();
                    foreach (string key in QryQuoteOrderDataDic.Keys)
                    {
                        QryQuoteOrderDataLst.Add(QryQuoteOrderDataDic[key]);
                    }
                    QryQuoteOrderDataLst.Sort(QuoteOrderData.CompareByCommitTime);
                    //ProcessQuoteInsertOrderData(_QryQuoteOrderDataLst);
                    TradeDataClient.GetClientInstance().RtnOrderEnqueue(QryQuoteOrderDataLst);
                    TempQuoteOrderData.Clear();
                }
            }
        }

        void CtpTraderApi_OnRtnQuote(ref CThostFtdcQuoteField pQuote)
        {
            QuoteOrderData quoteData = QuoteOrderExecutionReport(pQuote);
            if (TempQuoteInsertFlag) //接收同步的报价回报
            {
                TempQuoteOrderData.Add(quoteData);
                Util.Log("TradeApiCTP CtpDataServer: OnRtnQuote is delayed.");
                return;
            }
            TradeDataClient.GetClientInstance().RtnOrderEnqueue(quoteData);
        }

        void CtpTraderApi_OnRspQuoteInsert(ref CThostFtdcInputQuoteField pInputQuote, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer: OnErrRtnQuoteInsert !bIsLast");
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnErrRtnQuoteInsert! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pInputQuote.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pInputQuote.InstrumentID);
            }
        }

        void CtpTraderApi_OnErrRtnQuoteInsert(ref CThostFtdcInputQuoteField pQuoteInsert, ref CThostFtdcRspInfoField pRspInfo)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnErrRtnQuoteInsert! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pQuoteInsert.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pQuoteInsert.InstrumentID);
            }
        }

        void CtpTraderApi_OnRspQuoteAction(ref CThostFtdcInputQuoteActionField pInputQuoteAction, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer: OnErrRtnQuoteAction !bIsLast");
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnErrRtnQuoteAction! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pInputQuoteAction.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pInputQuoteAction.InstrumentID);
            }
        }

        void CtpTraderApi_OnErrRtnQuoteAction(ref CThostFtdcQuoteActionField pQuoteAction, ref CThostFtdcRspInfoField pRspInfo)
        {
            //撤单报价错误
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnErrRtnQuoteAction! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pQuoteAction.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pQuoteAction.InstrumentID);
            }
        }

        void CtpTraderApi_OnRtnCombAction(ref CThostFtdcCombActionField pCombAction)
        {
            //throw new NotImplementedException();
        }

        void CtpTraderApi_OnRspQryCombAction(ref CThostFtdcCombActionField pCombAction, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQryCombAction! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pCombAction.InstrumentID);
            }
            else
            {

            }
        }

        void CtpTraderApi_OnRspQryCombInstrumentGuard(ref CThostFtdcCombInstrumentGuardField pCombInstrumentGuard, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspQryCombInstrumentGuard! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pCombInstrumentGuard.InstrumentID);
            }
            else
            {
            }
        }

        void CtpTraderApi_OnRspCombActionInsert(ref CThostFtdcInputCombActionField pInputCombAction, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer: OnRspCombActionInsert !bIsLast");
                return;
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnRspCombActionInsert! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pInputCombAction.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pInputCombAction.InstrumentID);
            }
        }

        void CtpTraderApi_OnErrRtnCombActionInsert(ref CThostFtdcInputCombActionField pInputCombAction, ref CThostFtdcRspInfoField pRspInfo)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiCTP CtpDataServer OnErrRtnCombActionInsert! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pInputCombAction.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pInputCombAction.InstrumentID);
            }
        }

        public void RequestTradeDataDisConnect()
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Warning! RequestTradeDataDisConnect: tradeApi is null!");
                return;
            }
            ExecQueue.ReqTime = DateTime.Now;
            IsLoggedOn = false;
            Util.Log("TradeApiCTP CtpDataServer: RequestTradeDataDisConnect: Clearing UI Information...");
            DisconnectStruct disStruct = GetDisconnectReport("Trader");
            TradeDataClient.GetClientInstance().RtnMessageEnqueue(disStruct);
            
            //_CtpServerInstance = null;
            Util.Log("TradeApiCTP CtpDataServer: RequestTradeDataDisConnect: DisConnect starts...");
            try
            {
                _TradingCts.Cancel();
                //reqQueue.ClearThread();
                //DataQueue.ClearThread();
                _CtpTraderApi.DisConnect();
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
            //Thread t = new Thread(delegate()
            //{
            //    try
            //    {
            //        if (reqQueue != null)
            //        {
            //            reqQueue.ClearThread();
            //        }
            //        if (DataQueue != null)
            //        {
            //            DataQueue.ClearThread();
            //        }
            //        tradeApi.DisConnect();
            //        Util.Log("TradeApiCTP CtpDataServer: DisConnect is Executed.");
            //    }
            //    catch (Exception ex)
            //    {
            //        Util.Log("exception: " + ex.Message);
            //        Util.Log("exception: " + ex.StackTrace);
            //    }
            //});
            //t.Start();
        }

        public int RequestContractCode()
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! RequestContractCode: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int contractQryFlag = _CtpTraderApi.QryInstrument();
            Util.Log("TradeApiCTP CtpDataServer: QryInstrument is Executed.");
            if (contractQryFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. QryInstrument() = " + contractQryFlag);
            }
            else if (contractQryFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. QryInstrument() = " + contractQryFlag);
                AddToTradeDataQryQueue(new RequestContent("RequestContractCode", new List<object>()));
            }
            else if (contractQryFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. QryInstrument() = " + contractQryFlag);
                AddToTradeDataQryQueue(new RequestContent("RequestContractCode", new List<object>()));
            }
            return contractQryFlag;
        }

        public int RequestMarginRate(string contract)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! RequestMarginRate: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int marginFlag = _CtpTraderApi.QryInstrumentMarginRate(contract);
            Util.Log("TradeApiCTP CtpDataServer: QryInstrumentMarginRate is Executed. Code: " + contract);
            if (marginFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. QryInstrumentMarginRate() = " + marginFlag);
            }
            else if (marginFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. QryInstrumentMarginRate() = " + marginFlag);
                AddToTradeDataQryQueue(new RequestContent("RequestMarginRate", new List<object>() { contract }));
            }
            else if (marginFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. QryInstrumentMarginRate() = " + marginFlag);
                AddToTradeDataQryQueue(new RequestContent("RequestMarginRate", new List<object>() { contract }));
            }
            return marginFlag;
        }

        public int RequestCommissionRate(string contract)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! RequestCommissionRate: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int commissionFlag = _CtpTraderApi.QryInstrumentCommissionRate(contract);
            Util.Log("TradeApiCTP CtpDataServer: QryInstrumentCommissionRate is Executed.");
            if (commissionFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. QryInstrumentCommissionRate() = " + commissionFlag);
            }
            else if (commissionFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. QryInstrumentCommissionRate() = " + commissionFlag);
                AddToTradeDataQryQueue(new RequestContent("RequestCommissionRate", new List<object>() { contract }));
            }
            else if (commissionFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. QryInstrumentCommissionRate() = " + commissionFlag);
                AddToTradeDataQryQueue(new RequestContent("RequestCommissionRate", new List<object>() { contract }));
            }
            return commissionFlag;
        }

        public int RequestOptionsCommissionRate(string contract)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! RequestOptionsCommissionRate: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int commOptionFlag = _CtpTraderApi.QryOptionInstrCommRate(contract);
            Util.Log("TradeApiCTP CtpDataServer: QryOptionInstrCommRate is Executed.");
            if (commOptionFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. QryOptionInstrCommRate() = " + commOptionFlag);
            }
            else if (commOptionFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. QryOptionInstrCommRate() = " + commOptionFlag);
                AddToTradeDataQryQueue(new RequestContent("RequestOptionsCommissionRate", new List<object>() { contract }));
            }
            else if (commOptionFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. QryOptionInstrCommRate() = " + commOptionFlag);
                AddToTradeDataQryQueue(new RequestContent("RequestOptionsCommissionRate", new List<object>() { contract }));
            }
            return commOptionFlag;
        }

        public int RequsetMaxOperation(Contract codeInfo, SIDETYPE sideType, PosEffect posEffect, double price, EnumHedgeType hedge, int positionHand)
        {
            Util.Log("请求某个品种可操作的最大手数:" + codeInfo.Code + " " + sideType.ToString() + " " + posEffect.ToString());
            if (codeInfo.Code == "")
            {
                return CTPDATASERVERERROR;
            }

            if (_CtpTraderApi == null)
            {
                Util.Log("Error! RequsetMaxOperation: tradeApi is null!");
                return CTPDATASERVERERROR;
            }

            if (positionHand != 0)
            {
                if (!CodeSetManager.MaxOperationHandCountDict.ContainsKey(codeInfo))
                {
                    CodeSetManager.MaxOperationHandCountDict.Add(codeInfo, positionHand);
                }
            }

            CThostFtdcQueryMaxOrderVolumeField req = new CThostFtdcQueryMaxOrderVolumeField();
            req.BrokerID = _CtpTraderApi.BrokerID;
            req.Direction = GetCtpSideType(sideType);
            req.HedgeFlag = GetCtpHedgeFlag(hedge);
            req.InstrumentID = codeInfo.Code;
            req.InvestorID = _CtpTraderApi.InvestorID;
            req.OffsetFlag = GetCtpPosEffectType(posEffect);

            if (_CtpTraderApi == null)
            {
                Util.Log("Error! RequsetMaxOperation: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int maxQryFlag = _CtpTraderApi.ReqQueryMaxOrderVolume(req);
            Util.Log(String.Format("TradeApiCTP CtpDataServer: QueryMaxOrderVolume is Executed: {0} {1} {2} {3}", codeInfo.Code, sideType.ToString(), posEffect.ToString(), GetHedgeString(req.HedgeFlag)));
            if (maxQryFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQueryMaxOrderVolume() = " + maxQryFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (maxQryFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQueryMaxOrderVolume() = " + maxQryFlag);
                AddToTradeDataQryQueue(new RequestContent("RequsetMaxOperation", new List<object>() { codeInfo, sideType, posEffect, price, hedge, positionHand }));
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
            }
            else if (maxQryFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQueryMaxOrderVolume() = " + maxQryFlag);
                AddToTradeDataQryQueue(new RequestContent("RequsetMaxOperation", new List<object>() { codeInfo, sideType, posEffect, price, hedge, positionHand }));
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
            }
            return maxQryFlag;
        }

        /// <summary>
        /// 请求结算单
        /// </summary>
        /// <param name="date">哪一天的结算单</param>
        /// <returns></returns>
        public int RequestSettlementInstructions(string queryDate)
        {
            SettlementInfo.SettlementInfoCharList.Clear();
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! RequestSettlementInstructions: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int settleQryFlag = _CtpTraderApi.QrySettlementInfo(queryDate);
            Util.Log("TradeApiCTP CtpDataServer: QrySettlementInfo is Executed.");
            if (settleQryFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. QrySettlementInfo() = " + settleQryFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (settleQryFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. QrySettlementInfo() = " + settleQryFlag);
                AddToTradeDataQryQueue(new RequestContent("RequestSettlementInstructions", new List<object>() { queryDate }));
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
            }
            else if (settleQryFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. QrySettlementInfo() = " + settleQryFlag);
                AddToTradeDataQryQueue(new RequestContent("RequestSettlementInstructions", new List<object>() { queryDate }));
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
            }
            return settleQryFlag;
        }

        /// <summary>
        /// 请求结算单确认信息
        /// </summary>
        /// <param name="date">哪一天的结算单</param>
        /// <returns></returns>
        public int QuerySettlementInfoConfirm()
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! RequestSettlementInfoConfirm: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int settleQryFlag = _CtpTraderApi.QrySettlementInfoConfirm();
            Util.Log("TradeApiCTP CtpDataServer: QrySettlementInfoConfirm is Executed.");
            if (settleQryFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. QrySettlementInfoConfirm() = " + settleQryFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (settleQryFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. QrySettlementInfoConfirm() = " + settleQryFlag);
                AddToTradeDataQryQueue(new RequestContent("QuerySettlementInfoConfirm", new List<object>()));
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
            }
            else if (settleQryFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. QrySettlementInfoConfirm() = " + settleQryFlag);
                AddToTradeDataQryQueue(new RequestContent("QuerySettlementInfoConfirm", new List<object>()));
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
            }
            return settleQryFlag;
        }

        /// <summary>
        /// 确认结算单
        /// </summary>
        /// <returns></returns>
        public int RequestSettlementInfoConfirm()
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! RequestSettlementInfoConfirm: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int settleCfmFlag = _CtpTraderApi.SettlementInfoConfirm();
            Util.Log("TradeApiCTP CtpDataServer: SettlementInfoConfirm is Executed.");
            if (settleCfmFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. SettlementInfoConfirm() = " + settleCfmFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (settleCfmFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. SettlementInfoConfirm() = " + settleCfmFlag);
                AddToTradeDataQryQueue(new RequestContent("RequestSettlementInfoConfirm", new List<object>()));
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
            }
            else if (settleCfmFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. SettlementInfoConfirm() = " + settleCfmFlag);
                AddToTradeDataQryQueue(new RequestContent("RequestSettlementInfoConfirm", new List<object>()));
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
            }
            return settleCfmFlag;
        }

        /// <summary>
        /// 修改资金密码
        /// </summary>
        /// <param name="newPassword"></param>
        /// <param name="oldPassword"></param>
        /// <returns></returns>
        public int ChangeTradingPassword(string newPassword, string oldPassword)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! ChangeTradingPassword: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int pwdFlag = _CtpTraderApi.TradingAccountPasswordUpdate(_CtpTraderApi.InvestorID, oldPassword, newPassword);
            Util.Log("TradeApiCTP CtpDataServer: TradingAccountPasswordUpdate is Executed.");
            if (pwdFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. TradingAccountPasswordUpdate() = " + pwdFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (pwdFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. TradingAccountPasswordUpdate() = " + pwdFlag);
                AddToTradeDataQryQueue(new RequestContent("ChangeTradingPassword", new List<object>() { newPassword, oldPassword }));
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
            }
            else if (pwdFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. TradingAccountPasswordUpdate() = " + pwdFlag);
                AddToTradeDataQryQueue(new RequestContent("ChangeTradingPassword", new List<object>() { newPassword, oldPassword }));
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
            }
            return pwdFlag;
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="newPassword"></param>
        /// <param name="oldPassword"></param>
        /// <returns></returns>
        public int ChangeUserPassword(string newPassword, string oldPassword)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! ChangeUserPassword: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int passFlag = _CtpTraderApi.UserPasswordupdate(_CtpTraderApi.InvestorID, oldPassword, newPassword);
            Util.Log("TradeApiCTP CtpDataServer: UserPasswordupdate is Executed.");
            if (passFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. UserPasswordupdate() = " + passFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (passFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. UserPasswordupdate() = " + passFlag);
                AddToTradeDataQryQueue(new RequestContent("ChangeUserPassword", new List<object>() { newPassword, oldPassword }));
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
            }
            else if (passFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. UserPasswordupdate() = " + passFlag);
                AddToTradeDataQryQueue(new RequestContent("ChangeUserPassword", new List<object>() { newPassword, oldPassword }));
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
            }
            return passFlag;
        }

        /// <summary>
        /// 查询经纪公司交易参数
        /// </summary>
        /// <param name="newPassword"></param>
        /// <param name="oldPassword"></param>
        /// <returns></returns>
        public int QryBrokerTradingParams()
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! QryBrokerTradingParams: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int passFlag = _CtpTraderApi.ReqQryBrokerTradingParams();
            Util.Log("TradeApiCTP CtpDataServer: ReqQryBrokerTradingParams is Executed.");
            if (passFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQryBrokerTradingParams() = " + passFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (passFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQryBrokerTradingParams() = " + passFlag);
                AddToTradeDataQryQueue(new RequestContent("QryBrokerTradingParams", new List<object>()));
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
            }
            else if (passFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQryBrokerTradingParams() = " + passFlag);
                AddToTradeDataQryQueue(new RequestContent("QryBrokerTradingParams", new List<object>()));
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
            }
            return passFlag;
        }

        public void InitDataFromAPI()
        {
            if (_CtpTraderApi != null)
            {
                AddToTradeDataQryQueue(new RequestContent("QryUserInfo", new List<object>()));
                AddToTradeDataQryQueue(new RequestContent("QryUserTradingCode", new List<object>()));
                AddToTradeDataQryQueue(new RequestContent("ReqCapital", new List<object>()));
                AddToTradeDataQryQueue(new RequestContent("ReqTrade", new List<object>()));
                AddToTradeDataQryQueue(new RequestContent("QryBrokerTradingParams", new List<object>()));
                AddToTradeDataQryQueue(new RequestContent("ReqPositionDetail", new List<object>()));
                AddToTradeDataQryQueue(new RequestContent("ReqPositionCombineDetail", new List<object>()));
                AddToTradeDataQryQueue(new RequestContent("ReqOrder", new List<object>()));
                AddToTradeDataQryQueue(new RequestContent("QryExecOrder", new List<object>()));
                AddToTradeDataQryQueue(new RequestContent("QryQuoteOrder", new List<object>()));
                AddToTradeDataQryQueue(new RequestContent("QryPreOrder", new List<object>()));
                AddToTradeDataQryQueue(new RequestContent("QryPreCancelOrder", new List<object>()));
                //AddToTradeDataQryQueue(new CtpRequestContent("RequestCommissionRate", new List<object>() { null }));
                //AddToTradeDataQryQueue(new CtpRequestContent("RequestMarginRate", new List<object>() { null }));
                //AddToTradeDataQryQueue(new CtpRequestContent("ReqPosition", new List<object>()));
            }
            InitTransferFromAPI();
        }

        public void InitTransferFromAPI()
        {
            if (_CtpTraderApi != null)
            {
                AddToTradeDataQryQueue(new RequestContent("QryInterBanks", new List<object>()));
            }
        }

        public void AddToTradeDataRspQueue(object data)
        {
            //if (_TradeDataRspQueue == null)
            //{
            //    Util.Log("Warning! _TradeDataRspQueue hasn't been initialized!");
            //    return;
            //}
            //_TradeDataRspQueue.Enqueue(data);
        }

        public void AddToTradeDataQryQueue(RequestContent cmd)
        {
            if (_TradeDataReqQueue == null)
            {
                Util.Log("Warning! _TradeDataReqQueue hasn't been initialized!");
                return;
            }
            _TradeDataReqQueue.QryEnqueue(cmd);
        }

        public void AddToOrderQueue(RequestContent cmd)
        {
            if (_TradeDataReqQueue == null)
            {
                Util.Log("Warning! _TradeDataReqQueue hasn't been initialized!");
                return;
            }
            _TradeDataReqQueue.OrdEnqueue(cmd);
        }

        public void AddToMarketDataQryQueue(RequestContent cmd)
        {
            if (_MarketDataReqQueue == null)
            {
                Util.Log("Warning! reqQueue hasn't been initialized!");
                return;
            }
            _MarketDataReqQueue.QryEnqueue(cmd);
        }

        //public void AddToMarketDataRspQueue(object data)
        //{
        //    if (_MarketDataRspQueue == null)
        //    {
        //        Util.Log("Warning! DataQueue hasn't been initialized!");
        //        return;
        //    }
        //    _MarketDataRspQueue.Enqueue(data);
        //}

        public int ReqOrder()
        {
            //_JYOrderData.Clear();
            QryOrderDataDic.Clear();
            TempOrderFlag = true;
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! ReqOrder: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int orderFlag = _CtpTraderApi.QryOrder();
            Util.Log("TradeApiCTP CtpDataServer: QryOrder is Executed.");
            if (orderFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. QryOrder() = " + orderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                System.Windows.MessageBox.Show("网络连接失败，如发现数据有误，请尝试重启控件", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (orderFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. QryOrder() = " + orderFlag);
                AddToTradeDataQryQueue(new RequestContent("ReqOrder", new List<object>()));//Todo:it will be calculated in client 
            }
            else if (orderFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. QryOrder() = " + orderFlag);
                AddToTradeDataQryQueue(new RequestContent("ReqOrder", new List<object>()));//Todo:it will be calculated in client 
            }
            return orderFlag;
        }

        public int ReqTrade()
        {
            QryTradeDataLst.Clear();
            TempTradeFlag = true;
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! reqTrade: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int tradeFlag = _CtpTraderApi.QryTrade();
            Util.Log("TradeApiCTP CtpDataServer: QryTrade is Executed.");
            if (tradeFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. QryTrade() = " + tradeFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                System.Windows.MessageBox.Show("网络连接失败，如发现数据有误，请尝试重启控件", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (tradeFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. QryTrade() = " + tradeFlag);
                AddToTradeDataQryQueue(new RequestContent("ReqTrade", new List<object>()));//Todo:it will be calculated in client 
            }
            else if (tradeFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. QryTrade() = " + tradeFlag);
                AddToTradeDataQryQueue(new RequestContent("ReqTrade", new List<object>()));//Todo:it will be calculated in client 
            }
            return tradeFlag;
        }

        public int ReqPosition()
        {
            QryPosDetailData.Clear();
            JYPosSumData.Clear();
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! reqPosition: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int posFlag = _CtpTraderApi.QryInvestorPosition();
            Util.Log("TradeApiCTP CtpDataServer: QryInvestorPosition is Executed.");
            if (posFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. QryInvestorPosition() = " + posFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                System.Windows.MessageBox.Show("网络连接失败，如发现数据有误，请尝试重启控件", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (posFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. QryInvestorPosition() = " + posFlag);
                AddToTradeDataQryQueue(new RequestContent("ReqPosition", new List<object>()));//Todo:it will be calculated in client 
            }
            else if (posFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. QryInvestorPosition() = " + posFlag);
                AddToTradeDataQryQueue(new RequestContent("ReqPosition", new List<object>()));//Todo:it will be calculated in client 
            }
            return posFlag;
        }

        public int ReqPositionDetail()
        {
            QryPosDetailData.Clear();
            JYPosSumData.Clear();
            TempPosFlag = true;
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! reqPosition: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int posDetailFlag = _CtpTraderApi.QryInvestorPositionDetail();
            Util.Log("TradeApiCTP CtpDataServer: QryInvestorPositionDetail is Executed.");
            if (posDetailFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. QryInvestorPositionDetail() = " + posDetailFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                System.Windows.MessageBox.Show("网络连接失败，如发现数据有误，请尝试重启控件", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (posDetailFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. QryInvestorPositionDetail() = " + posDetailFlag);
                AddToTradeDataQryQueue(new RequestContent("ReqPositionDetail", new List<object>()));//Todo:it will be calculated in client 
            }
            else if (posDetailFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. QryInvestorPositionDetail() = " + posDetailFlag);
                AddToTradeDataQryQueue(new RequestContent("ReqPositionDetail", new List<object>()));//Todo:it will be calculated in client 
            }
            return posDetailFlag;
        }

        public int ReqPositionCombineDetail()
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! ReqPositionCombineDetail: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int posComDetailFlag = _CtpTraderApi.QryInvestorPositionCombineDetail();
            Util.Log("TradeApiCTP CtpDataServer: QryInvestorPositionCombineDetail is Executed.");
            if (posComDetailFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. QryInvestorPositionCombineDetail() = " + posComDetailFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                System.Windows.MessageBox.Show("网络连接失败，如发现数据有误，请尝试重启控件", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (posComDetailFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. QryInvestorPositionCombineDetail() = " + posComDetailFlag);
                AddToTradeDataQryQueue(new RequestContent("ReqPositionCombineDetail", new List<object>()));//Todo:it will be calculated in client 
            }
            else if (posComDetailFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. QryInvestorPositionCombineDetail() = " + posComDetailFlag);
                AddToTradeDataQryQueue(new RequestContent("ReqPositionCombineDetail", new List<object>()));//Todo:it will be calculated in client 
            }
            return posComDetailFlag;
        }

        public int QryUserInfo()
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! QryUserInfo: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int userFlag = _CtpTraderApi.QryInvestor();
            Util.Log("TradeApiCTP CtpDataServer: QryInvestor is Executed.");
            if (userFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. QryInvestor() = " + userFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                System.Windows.MessageBox.Show("网络连接失败，如发现数据有误，请尝试重启控件", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (userFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. QryInvestor() = " + userFlag);
                AddToTradeDataQryQueue(new RequestContent("QryUserInfo", new List<object>()));
            }
            else if (userFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. QryInvestor() = " + userFlag);
                AddToTradeDataQryQueue(new RequestContent("QryUserInfo", new List<object>()));
            }
            return userFlag;
        }

        public int QryUserTradingCode()
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! QryUserTradingCode: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int userTdCodeFlag = _CtpTraderApi.QryTradingCode();
            Util.Log("TradeApiCTP CtpDataServer: QryTradingCode is Executed.");
            if (userTdCodeFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. QryTradingCode() = " + userTdCodeFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                System.Windows.MessageBox.Show("网络连接失败，如发现数据有误，请尝试重启控件", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (userTdCodeFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. QryTradingCode() = " + userTdCodeFlag);
                AddToTradeDataQryQueue(new RequestContent("QryUserTradingCode", new List<object>()));
            }
            else if (userTdCodeFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. QryTradingCode() = " + userTdCodeFlag);
                AddToTradeDataQryQueue(new RequestContent("QryUserTradingCode", new List<object>()));
            }
            return userTdCodeFlag;
        }

        public int ReqCapital()
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! ReqCapital: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int capitalFlag = _CtpTraderApi.QryTradingAccount();
            Util.Log("TradeApiCTP CtpDataServer: QryTradingAccount is Executed.");
            if (capitalFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqCapital() = " + capitalFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                System.Windows.MessageBox.Show("网络连接失败，如发现数据有误，请尝试重启控件", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (capitalFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqCapital() = " + capitalFlag);
                AddToTradeDataQryQueue(new RequestContent("ReqCapital", new List<object>()));//Todo:it will be calculated in client 
            }
            else if (capitalFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqCapital() = " + capitalFlag);
                AddToTradeDataQryQueue(new RequestContent("ReqCapital", new List<object>()));//Todo:it will be calculated in client 
            }
            return capitalFlag;
        }

        public int ClientLogin()
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! ClientLogin: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int logFlag = _CtpTraderApi.ReqUserLogin();
            Util.Log("TradeApiCTP CtpDataServer: UserLogin is Executed.");
            if (logFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqUserLogin() = " + logFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (logFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqUserLogin() = " + logFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
            }
            else if (logFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqUserLogin() = " + logFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
            }
            return logFlag;
        }

        public int ClientLogOff()
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! ClientLogOff: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int logOffFlag = _CtpTraderApi.ReqUserLogout();
            Util.Log("TradeApiCTP CtpDataServer: UserLogout is Executed.");
            if (logOffFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. UserLogout() = " + logOffFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (logOffFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. UserLogout() = " + logOffFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
            }
            else if (logOffFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. UserLogout() = " + logOffFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
            }
            return logOffFlag;
        }

        public int NewOrderSingle(Contract codeInfo, SIDETYPE side, PosEffect posEffect,
            double price, int handCount, int isAuto, string orderId, int touchMethod = 0, int touchCondition = 0, double touchPrice = 0, EnumOrderType orderType = EnumOrderType.Limit, EnumHedgeType hedge = EnumHedgeType.Speculation)
        {
            //clientOrderId = 0;
            string ret = "";
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! NewOrderSingle: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            int orderFlag = 0;
            EnumThostDirectionType sideType = GetCtpSideType(side);
            EnumThostOffsetFlagType offsetType = GetCtpPosEffectType(posEffect);
            EnumThostHedgeFlagType hedgeFlag = GetCtpHedgeFlag(hedge);
            if (TradingMaster.Properties.Settings.Default.SplitLargeOrderHandCount && handCount > 1)
            {
                Util.Log("下单: code=" + codeInfo.Code + " side=" + sideType.ToString() + " posEffect=" + posEffect.ToString() + " price=" + price.ToString() + " handCount=" + handCount.ToString() + " isAuto=" + isAuto.ToString() + " orderId=" + orderId + " touchMethod=" + touchMethod.ToString() + " touchCondition=" + touchCondition.ToString() + " touchPrice=" + touchPrice.ToString());
                List<int> handLst = CommonUtil.BreakLargeOrderHandCount(handCount);
                foreach (int i in handLst)
                {
                    ExecQueue.ReqTime = DateTime.Now;
                    orderFlag = _CtpTraderApi.OrderInsert(codeInfo.Code, sideType, offsetType, price, i, touchPrice, orderType, hedgeFlag);
                    Util.Log("批量拆单: code=" + codeInfo.Code + " side=" + sideType.ToString() + " posEffect=" + posEffect.ToString() + " price=" + price.ToString() + " handCount=" + i.ToString() + " isAuto=" + isAuto.ToString() + " orderId=" + orderId + " touchMethod=" + touchMethod.ToString() + " touchCondition=" + touchCondition.ToString() + " touchPrice=" + touchPrice.ToString());
                    ret = codeInfo.Code + (sideType == EnumThostDirectionType.Buy ? "买" : "卖") + (offsetType == EnumThostOffsetFlagType.Open ? "开" : "平") + handCount.ToString() + "手";
                    if (orderFlag != 0)
                    {
                        break;
                    }
                }
            }
            else
            {
                ExecQueue.ReqTime = DateTime.Now;
                if (orderType == EnumOrderType.Market)
                {
                    while (codeInfo.MaxMarketOrderVolume > 0 && codeInfo.MaxMarketOrderVolume < handCount)
                    {
                        orderFlag = _CtpTraderApi.OrderInsert(codeInfo.Code, sideType, offsetType, price, codeInfo.MaxMarketOrderVolume, touchPrice, orderType, hedgeFlag);
                        Util.Log("下单: code=" + codeInfo.Code + " side=" + sideType.ToString() + " posEffect=" + posEffect.ToString() + " price=" + price.ToString() + " handCount=" + codeInfo.MaxMarketOrderVolume.ToString() + " isAuto=" + isAuto.ToString() + " orderId=" + orderId + " touchMethod=" + touchMethod.ToString() + " touchCondition=" + touchCondition.ToString() + " touchPrice=" + touchPrice.ToString());
                        handCount -= codeInfo.MaxMarketOrderVolume;
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    while (codeInfo.MaxLimitOrderVolume > 0 && codeInfo.MaxLimitOrderVolume < handCount)
                    {
                        orderFlag = _CtpTraderApi.OrderInsert(codeInfo.Code, sideType, offsetType, price, codeInfo.MaxLimitOrderVolume, touchPrice, orderType, hedgeFlag);
                        Util.Log("下单: code=" + codeInfo.Code + " side=" + sideType.ToString() + " posEffect=" + posEffect.ToString() + " price=" + price.ToString() + " handCount=" + codeInfo.MaxLimitOrderVolume.ToString() + " isAuto=" + isAuto.ToString() + " orderId=" + orderId + " touchMethod=" + touchMethod.ToString() + " touchCondition=" + touchCondition.ToString() + " touchPrice=" + touchPrice.ToString());
                        handCount -= codeInfo.MaxLimitOrderVolume;
                        Thread.Sleep(1000);
                    }
                }
                orderFlag = _CtpTraderApi.OrderInsert(codeInfo.Code, sideType, offsetType, price, handCount, touchPrice, orderType, hedgeFlag);
                Util.Log("下单: code=" + codeInfo.Code + " side=" + sideType.ToString() + " posEffect=" + posEffect.ToString() + " price=" + price.ToString() + " handCount=" + handCount.ToString() + " isAuto=" + isAuto.ToString() + " orderId=" + orderId + " touchMethod=" + touchMethod.ToString() + " touchCondition=" + touchCondition.ToString() + " touchPrice=" + touchPrice.ToString());
                ret = codeInfo.Code + (sideType == EnumThostDirectionType.Buy ? "买" : "卖") + (offsetType == EnumThostOffsetFlagType.Open ? "开" : "平") + handCount.ToString() + "手";
            }
            if (orderFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. OrderInsert() = " + orderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("网络连接失败，如发现数据有误，请尝试重新报单", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (orderFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. OrderInsert() = " + orderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("未处理请求超过许可数，如发现数据有误，请尝试重新报单", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (orderFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. OrderInsert() = " + orderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("每秒发送请求数超过许可数，如发现数据有误，请尝试重新报单", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            return orderFlag;
        }

        public int CancelOrder(string code, int frontID, int sessioID, string orderRef, string exchange, string orderId)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! CancelOrder: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int cancelFlag = _CtpTraderApi.OrderAction(code, frontID, sessioID, orderRef, CodeSetManager.ExNameToCtp(exchange), orderId.Trim());
            Util.Log("TradeApiCTP CtpDataServer: OrderAction is Executed.");
            if (cancelFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. OrderAction() = " + cancelFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("网络连接失败，如发现数据有误，请尝试重新撤单", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (cancelFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. OrderAction() = " + cancelFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("未处理请求超过许可数，如发现数据有误，请尝试重新撤单", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (cancelFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. OrderAction() = " + cancelFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("每秒发送请求数超过许可数，如发现数据有误，请尝试重新撤单", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            return cancelFlag;
        }

        public int NewPreOrderSingle(string code, SIDETYPE side, PosEffect posEffect, double price, int handCount, int isAuto, string orderId, int touchMethod, int touchCondition, double touchPrice)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! NewPreOrderSingle: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int pOrdInsertFlag = 0;

            EnumThostDirectionType sideType = GetCtpSideType(side);
            EnumThostOffsetFlagType offsetType = GetCtpPosEffectType(posEffect);

            CThostFtdcParkedOrderField tempField = new CThostFtdcParkedOrderField();
            tempField.BrokerID = _CtpTraderApi.BrokerID;
            tempField.BusinessUnit = null;
            tempField.ForceCloseReason = EnumThostForceCloseReasonType.NotForceClose;
            tempField.InvestorID = _CtpTraderApi.InvestorID;
            tempField.IsAutoSuspend = (int)EnumThostBoolType.No;
            tempField.MinVolume = 1;
            tempField.OrderPriceType = EnumThostOrderPriceTypeType.LimitPrice;
            tempField.OrderRef = (++_CtpTraderApi.MaxOrderRef).ToString();
            tempField.TimeCondition = EnumThostTimeConditionType.GFD;
            tempField.UserForceClose = (int)EnumThostBoolType.No;
            tempField.UserID = _CtpTraderApi.InvestorID;
            tempField.VolumeCondition = EnumThostVolumeConditionType.AV;
            tempField.CombHedgeFlag_0 = EnumThostHedgeFlagType.Speculation;

            tempField.InstrumentID = code;
            tempField.CombOffsetFlag_0 = offsetType;
            tempField.Direction = sideType;
            tempField.LimitPrice = price;
            tempField.VolumeTotalOriginal = handCount;

            if (isAuto == 2)//埋单
            {
                tempField.ContingentCondition = EnumThostContingentConditionType.ParkedOrder;
            }
            else if (isAuto == 1)//自动单
            {

            }
            else//其他（条件单）
            {
                tempField.StopPrice = touchPrice;
                tempField.ContingentCondition = EnumThostContingentConditionType.Immediately;
                if (touchMethod == 1)//1表示最新价
                {
                    if (touchCondition == 1)//1表示大于等于
                    {
                        tempField.ContingentCondition = EnumThostContingentConditionType.LastPriceGreaterEqualStopPrice;
                    }
                    else if (touchCondition == 2)//2表示小于等于
                    {
                        tempField.ContingentCondition = EnumThostContingentConditionType.LastPriceLesserEqualStopPrice;
                    }
                    else if (touchCondition == 3)//3表示大于
                    {
                        tempField.ContingentCondition = EnumThostContingentConditionType.LastPriceGreaterThanStopPrice;
                    }
                    else if (touchCondition == 4)//4表示小于
                    {
                        tempField.ContingentCondition = EnumThostContingentConditionType.LastPriceLesserThanStopPrice;
                    }
                }
                else if (touchMethod == 2)//2表示买价
                {
                    if (touchCondition == 1)//1表示大于等于
                    {
                        tempField.ContingentCondition = EnumThostContingentConditionType.BidPriceGreaterEqualStopPrice;
                    }
                    else if (touchCondition == 2)//2表示小于等于
                    {
                        tempField.ContingentCondition = EnumThostContingentConditionType.BidPriceLesserEqualStopPrice;
                    }
                    else if (touchCondition == 3)//3表示大于
                    {
                        tempField.ContingentCondition = EnumThostContingentConditionType.BidPriceGreaterThanStopPrice;
                    }
                    else if (touchCondition == 4)//4表示小于
                    {
                        tempField.ContingentCondition = EnumThostContingentConditionType.BidPriceLesserThanStopPrice;
                    }
                }
                else if (touchMethod == 3)//3表示卖价
                {
                    if (touchCondition == 1)//1表示大于等于
                    {
                        tempField.ContingentCondition = EnumThostContingentConditionType.AskPriceGreaterEqualStopPrice;
                    }
                    else if (touchCondition == 2)//2表示小于等于
                    {
                        tempField.ContingentCondition = EnumThostContingentConditionType.AskPriceLesserEqualStopPrice;
                    }
                    else if (touchCondition == 3)//3表示大于
                    {
                        tempField.ContingentCondition = EnumThostContingentConditionType.AskPriceGreaterThanStopPrice;
                    }
                    else if (touchCondition == 4)//4表示小于
                    {
                        tempField.ContingentCondition = EnumThostContingentConditionType.AskPriceLesserThanStopPrice;
                    }
                }
            }
            pOrdInsertFlag = _CtpTraderApi.ParkedOrderInsert(tempField);

            Util.Log("TradeApiCTP CtpDataServer: ParkedOrderInsert is Executed.");
            if (pOrdInsertFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ParkedOrderInsert() = " + pOrdInsertFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("网络连接失败，如发现数据有误，请尝试重新埋单", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (pOrdInsertFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ParkedOrderInsert() = " + pOrdInsertFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("未处理请求超过许可数，如发现数据有误，请尝试重新埋单", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
                CtpDataServer.GetUserInstance().AddToOrderQueue(new RequestContent("NewPreOrderSingle", new List<object>() { code, sideType, posEffect, price, handCount, isAuto, orderId, touchMethod, touchCondition, touchPrice }));
            }
            else if (pOrdInsertFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ParkedOrderInsert() = " + pOrdInsertFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("每秒发送请求数超过许可数，如发现数据有误，请尝试重新埋单", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
                CtpDataServer.GetUserInstance().AddToOrderQueue(new RequestContent("NewPreOrderSingle", new List<object>() { code, sideType, posEffect, price, handCount, isAuto, orderId, touchMethod, touchCondition, touchPrice }));
            }
            return pOrdInsertFlag;
        }

        public int PreCancelOrder(string code, int frontID, int sessionID, string orderRef, string exchange, string orderId)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! PreCancelOrder: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int pCancelFlag = _CtpTraderApi.ReqParkedOrderAction(code, frontID, sessionID, orderRef, CodeSetManager.ExNameToCtp(exchange), orderId.Trim());
            Util.Log("TradeApiCTP CtpDataServer: ReqParkedOrderAction is Executed.");
            if (pCancelFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqParkedOrderAction() = " + pCancelFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("网络连接失败，如发现数据有误，请尝试重新埋撤单", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (pCancelFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqParkedOrderAction() = " + pCancelFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("未处理请求超过许可数，如发现数据有误，请尝试重新埋撤单", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (pCancelFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqParkedOrderAction() = " + pCancelFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("每秒发送请求数超过许可数，如发现数据有误，请尝试重新埋撤单", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            return pCancelFlag;
        }

        public int DeletePreOrder(string parkedOrderId)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! DeletePreOrder: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int deletePreFlag = _CtpTraderApi.ReqRemoveParkedOrder(parkedOrderId.Trim());
            Util.Log("TradeApiCTP CtpDataServer: ReqRemoveParkedOrder is Executed.");
            if (deletePreFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqRemoveParkedOrder() = " + deletePreFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("网络连接失败，如发现数据有误，请尝试重新删除", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (deletePreFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqRemoveParkedOrder() = " + deletePreFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("未处理请求超过许可数，如发现数据有误，请尝试重新删除", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (deletePreFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqRemoveParkedOrder() = " + deletePreFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("每秒发送请求数超过许可数，如发现数据有误，请尝试重新删除", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            return deletePreFlag;
        }

        public int DeletePreCancelOrder(string parkerOrderActionId)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! DeletePreCancelOrder: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int deletePreFlag = _CtpTraderApi.ReqRemoveParkedOrderAction(parkerOrderActionId.Trim());
            Util.Log("TradeApiCTP CtpDataServer: ReqRemoveParkedOrderAction is Executed.");
            if (deletePreFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqRemoveParkedOrderAction() = " + deletePreFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("网络连接失败，如发现数据有误，请尝试重新删除", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (deletePreFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqRemoveParkedOrderAction() = " + deletePreFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("未处理请求超过许可数，如发现数据有误，请尝试重新删除", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (deletePreFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqRemoveParkedOrderAction() = " + deletePreFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("每秒发送请求数超过许可数，如发现数据有误，请尝试重新删除", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            return deletePreFlag;
        }

        public int QryPreOrder()
        {
            PreOrderData.Clear();
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! QryPreOrder: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int qryPreOrderFlag = _CtpTraderApi.ReqQryParkedOrder();
            Util.Log("TradeApiCTP CtpDataServer: ReqQryParkedOrder is Executed.");
            if (qryPreOrderFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQryParkedOrder() = " + qryPreOrderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (qryPreOrderFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQryParkedOrder() = " + qryPreOrderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
                AddToTradeDataQryQueue(new RequestContent("QryPreOrder", new List<object>()));
            }
            else if (qryPreOrderFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQryParkedOrder() = " + qryPreOrderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
                AddToTradeDataQryQueue(new RequestContent("QryPreOrder", new List<object>()));
            }
            return qryPreOrderFlag;
        }

        public int QryPreCancelOrder()
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! QryPreCancelOrder: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int qryPreOrderFlag = _CtpTraderApi.ReqQryParkedOrderAction();
            Util.Log("TradeApiCTP CtpDataServer: ReqQryParkedOrderAction is Executed.");
            if (qryPreOrderFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQryParkedOrderAction() = " + qryPreOrderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (qryPreOrderFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQryParkedOrderAction() = " + qryPreOrderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
            }
            else if (qryPreOrderFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQryParkedOrderAction() = " + qryPreOrderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
            }
            return qryPreOrderFlag;
        }

        public int QryInterBanks()
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! QryInterBanks: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int BankFlag = _CtpTraderApi.ReqQryContractBank();
            Util.Log("TradeApiCTP CtpDataServer: ReqQryContractBank is Executed.");
            if (BankFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQryContractBank() = " + BankFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show("网络连接失败，如发现数据有误，请尝试重启控件", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (BankFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQryContractBank() = " + BankFlag);
                AddToTradeDataQryQueue(new RequestContent("QryInterBanks", new List<object>()));
            }
            else if (BankFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQryContractBank() = " + BankFlag);
                AddToTradeDataQryQueue(new RequestContent("QryInterBanks", new List<object>()));
            }
            return BankFlag;
        }

        public int QryExchangeRate(string srcCurr, string destCurr)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! QryExchangeRate: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int exchRateFlag = _CtpTraderApi.ReqQryExchangeRate(srcCurr, destCurr);
            Util.Log("TradeApiCTP CtpDataServer: ReqQryExchangeRate is Executed.");
            if (exchRateFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQryExchangeRate() = " + exchRateFlag);
            }
            else if (exchRateFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQryExchangeRate() = " + exchRateFlag);
                AddToTradeDataQryQueue(new RequestContent("ReqQryExchangeRate", new List<object>() { srcCurr, destCurr }));
            }
            else if (exchRateFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQryExchangeRate() = " + exchRateFlag);
                AddToTradeDataQryQueue(new RequestContent("ReqQryExchangeRate", new List<object>() { srcCurr, destCurr }));
            }
            return exchRateFlag;
        }

        public int QryProductExchRate(string productID)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! QryExchangeRate: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int prodExchRateFlag = _CtpTraderApi.ReqQryProductExchRate(productID);
            Util.Log("TradeApiCTP CtpDataServer: ReqQryProductExchRate is Executed.");
            if (prodExchRateFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQryProductExchRate() = " + prodExchRateFlag);
            }
            else if (prodExchRateFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQryProductExchRate() = " + prodExchRateFlag);
                AddToTradeDataQryQueue(new RequestContent("ReqQryProductExchRate", new List<object>() { productID }));
            }
            else if (prodExchRateFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQryProductExchRate() = " + prodExchRateFlag);
                AddToTradeDataQryQueue(new RequestContent("ReqQryProductExchRate", new List<object>() { productID }));
            }
            return prodExchRateFlag;
        }

        public int QryAccountRegister()
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! QryAccountRegistered: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            QryBankAcctInfoLst.Clear();
            int registerFlag = _CtpTraderApi.ReqQryAccountregister();
            Util.Log("TradeApiCTP CtpDataServer: ReqQryAccountregister is Executed.");
            if (registerFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQryAccountregister() = " + registerFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show("网络连接失败，如发现数据有误，请尝试重启控件", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (registerFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQryAccountregister() = " + registerFlag);
                AddToTradeDataQryQueue(new RequestContent("QryAccountRegister", new List<object>()));
            }
            else if (registerFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQryAccountregister() = " + registerFlag);
                AddToTradeDataQryQueue(new RequestContent("QryAccountRegister", new List<object>()));
            }
            return registerFlag;
        }

        public int QryTransferSerial(string bankID)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! QryTransferSerial: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            QryTransferRecords.Clear();
            int serialFlag = _CtpTraderApi.ReqQryTransferSerial(bankID);
            Util.Log("TradeApiCTP CtpDataServer: ReqQryTransferSerial is Executed.");
            if (serialFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQryTransferSerial() = " + serialFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show("网络连接失败，如发现数据有误，请尝试重启控件", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (serialFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQryTransferSerial() = " + serialFlag);
                AddToTradeDataQryQueue(new RequestContent("QryTransferSerial", new List<object>() { bankID }));
            }
            else if (serialFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQryTransferSerial() = " + serialFlag);
                AddToTradeDataQryQueue(new RequestContent("QryTransferSerial", new List<object>() { bankID }));
            }
            return serialFlag;
        }

        public int QryBankAccount(string bankID, string bankBranchID, string capitalPwd, string bankPwd, string currency)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! QryBankAccount: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;

            CThostFtdcReqQueryAccountField reqQryAcct = new CThostFtdcReqQueryAccountField();
            reqQryAcct.TradeCode = "204002";
            reqQryAcct.BrokerID = _CtpTraderApi.BrokerID;//TODO
            reqQryAcct.BankID = bankID;// 银行代码;
            reqQryAcct.BankBranchID = bankBranchID;// 银行分支代码;
            //reqQryAcct.RequestID = 请求编号;
            reqQryAcct.SecuPwdFlag = EnumThostPwdFlagType.BlankCheck; // 明文核对
            reqQryAcct.BankPwdFlag = EnumThostPwdFlagType.NoCheck; // 不核对
            reqQryAcct.VerifyCertNoFlag = EnumThostYesNoIndicatorType.No;
            reqQryAcct.AccountID = _CtpTraderApi.InvestorID;//capAcctID;// 资金账号;
            reqQryAcct.Password = capitalPwd;//资金密码;
            reqQryAcct.CurrencyID = currency;
            reqQryAcct.BankPassWord = bankPwd;//银行密码;

            int qryBankAcctFlag = _CtpTraderApi.ReqQueryBankAccountMoneyByFuture(reqQryAcct);
            Util.Log("TradeApiCTP CtpDataServer: ReqQueryBankAccountMoneyByFuture is Executed.");
            if (qryBankAcctFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQueryBankAccountMoneyByFuture() = " + qryBankAcctFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show("网络连接失败，如发现数据有误，请尝试重启控件", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (qryBankAcctFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQueryBankAccountMoneyByFuture() = " + qryBankAcctFlag);
                AddToTradeDataQryQueue(new RequestContent("QryBankAccount", new List<object>() { bankID, bankBranchID, capitalPwd, bankPwd }));
            }
            else if (qryBankAcctFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQueryBankAccountMoneyByFuture() = " + qryBankAcctFlag);
                AddToTradeDataQryQueue(new RequestContent("QryBankAccount", new List<object>() { bankID, bankBranchID, capitalPwd, bankPwd }));
            }
            return qryBankAcctFlag;
        }

        public int TransferFromFutureToBankByFuture(string bankID, string bankBranchID, string capitalPwd, string bankPwd, double tranAmt, string currency)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! TransferFromFutureToBankByFuture: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;

            CThostFtdcReqTransferField tranToBank = new CThostFtdcReqTransferField();
            tranToBank.TradeCode = "202002";
            tranToBank.BrokerID = _CtpTraderApi.BrokerID;
            tranToBank.BankID = bankID;// 银行代码;
            tranToBank.BankBranchID = bankBranchID;// 银行分支代码;
            //tranToBank.RequestID = 请求编号;
            tranToBank.SecuPwdFlag = EnumThostPwdFlagType.BlankCheck; // 明文核对
            tranToBank.BankPwdFlag = EnumThostPwdFlagType.NoCheck; // 不核对
            tranToBank.VerifyCertNoFlag = EnumThostYesNoIndicatorType.No;
            tranToBank.AccountID = _CtpTraderApi.InvestorID; //资金账号;
            tranToBank.Password = capitalPwd;//资金密码;
            tranToBank.CurrencyID = currency;
            tranToBank.BankPassWord = bankPwd;// 银行密码;
            tranToBank.TradeAmount = tranAmt;

            int transToBankFlag = _CtpTraderApi.ReqFromFutureToBankByFuture(tranToBank);
            Util.Log("TradeApiCTP CtpDataServer: ReqFromFutureToBankByFuture is Executed.");
            if (transToBankFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqFromFutureToBankByFuture() = " + transToBankFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show("网络连接失败，如发现数据有误，请尝试重启控件", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (transToBankFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqFromFutureToBankByFuture() = " + transToBankFlag);
                AddToTradeDataQryQueue(new RequestContent("TransferFromFutureToBankByFuture", new List<object>() { bankID, bankBranchID, capitalPwd, bankPwd }));
            }
            else if (transToBankFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqFromFutureToBankByFuture() = " + transToBankFlag);
                AddToTradeDataQryQueue(new RequestContent("TransferFromFutureToBankByFuture", new List<object>() { bankID, bankBranchID, capitalPwd, bankPwd }));
            }
            return transToBankFlag;
        }

        public int TransferFromBankToFutureByFuture(string bankID, string bankBranchID, string capitalPwd, string bankPwd, double tranAmt, string currency)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! TransferFromBankToFutureByFuture: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;

            CThostFtdcReqTransferField tranToFut = new CThostFtdcReqTransferField();
            tranToFut.TradeCode = "202001";
            tranToFut.BrokerID = _CtpTraderApi.BrokerID;
            tranToFut.BankID = bankID;// 银行代码;
            tranToFut.BankBranchID = bankBranchID;// 银行分支代码;
            //tranToFut.RequestID = 请求编号;
            tranToFut.SecuPwdFlag = EnumThostPwdFlagType.BlankCheck; // 明文核对
            tranToFut.BankPwdFlag = EnumThostPwdFlagType.NoCheck; // 不核对
            tranToFut.VerifyCertNoFlag = EnumThostYesNoIndicatorType.No;
            tranToFut.AccountID = _CtpTraderApi.InvestorID; //资金账号;
            tranToFut.Password = capitalPwd;//资金密码;
            tranToFut.CurrencyID = currency;
            tranToFut.BankPassWord = bankPwd;// 银行密码;
            tranToFut.TradeAmount = tranAmt;//转账金额

            int transToFutFlag = _CtpTraderApi.ReqFromBankToFutureByFuture(tranToFut);
            Util.Log("TradeApiCTP CtpDataServer: ReqFromBankToFutureByFuture is Executed.");
            if (transToFutFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqFromBankToFutureByFuture() = " + transToFutFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        MessageBox.Show("网络连接失败，如发现数据有误，请尝试重启控件", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (transToFutFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqFromBankToFutureByFuture() = " + transToFutFlag);
                AddToTradeDataQryQueue(new RequestContent("TransferFromBankToFutureByFuture", new List<object>() { bankID, bankBranchID, capitalPwd, bankPwd, tranAmt }));//Todo:it will be calculated in client 
            }
            else if (transToFutFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqFromBankToFutureByFuture() = " + transToFutFlag);
                AddToTradeDataQryQueue(new RequestContent("TransferFromBankToFutureByFuture", new List<object>() { bankID, bankBranchID, capitalPwd, bankPwd, tranAmt }));//Todo:it will be calculated in client 
            }
            return transToFutFlag;
        }

        ///请求查询期权交易成本
        public int QryOptionTradeCost(string code, double price, double basePrice = 0)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! QryOptionContractTradeCost: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int qryOptTradeCostFlag = _CtpTraderApi.ReqQryOptionInstrTradeCost(code, price, basePrice);
            Util.Log("TradeApiCTP CtpDataServer: ReqQryOptionInstrTradeCost is Executed.");
            if (qryOptTradeCostFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQryOptionInstrTradeCost() = " + qryOptTradeCostFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (qryOptTradeCostFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQryOptionInstrTradeCost() = " + qryOptTradeCostFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
                AddToTradeDataQryQueue(new RequestContent("QryOptionTradeCost", new List<object>()));
            }
            else if (qryOptTradeCostFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQryOptionInstrTradeCost() = " + qryOptTradeCostFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
                AddToTradeDataQryQueue(new RequestContent("QryOptionTradeCost", new List<object>()));
            }
            return qryOptTradeCostFlag;
        }

        /// 询价请求
        public int NewQryQuote(string code)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! NewQryQuote: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int quoteFlag = _CtpTraderApi.ReqForQuoteInsert(code);
            Util.Log("TradeApiCTP CtpDataServer: ReqForQuoteInsert is Executed.");
            if (quoteFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqForQuoteInsert() = " + quoteFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (quoteFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqForQuoteInsert() = " + quoteFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
            }
            else if (quoteFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqForQuoteInsert() = " + quoteFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
            }
            return quoteFlag;
        }

        ///请求查询投资者品种/跨品种保证金
        public int QryInvestorProductMargin(string code)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! QryInvestorProductMargin: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int qryProdMarginFlag = _CtpTraderApi.ReqQryInvestorProductGroupMargin(code);
            Util.Log("TradeApiCTP CtpDataServer: ReqQryInvestorProductGroupMargin is Executed.");
            if (qryProdMarginFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQryInvestorProductGroupMargin() = " + qryProdMarginFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (qryProdMarginFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQryInvestorProductGroupMargin() = " + qryProdMarginFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
                AddToTradeDataQryQueue(new RequestContent("QryInvestorProductMargin", new List<object>() { code }));
            }
            else if (qryProdMarginFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQryInvestorProductGroupMargin() = " + qryProdMarginFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
                AddToTradeDataQryQueue(new RequestContent("QryInvestorProductMargin", new List<object>() { code }));
            }
            return qryProdMarginFlag;
        }

        /// 请求查询询价
        public int QryForQuote()
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! QryForQuote: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            CThostFtdcQryForQuoteField qryForQuote = new CThostFtdcQryForQuoteField();

            ExecQueue.ReqTime = DateTime.Now;
            int optQuoteFlag = _CtpTraderApi.ReqQryForQuote(qryForQuote);
            Util.Log("TradeApiCTP CtpDataServer: ReqQryForQuote is Executed.");
            if (optQuoteFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQryForQuote() = " + optQuoteFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (optQuoteFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQryForQuote() = " + optQuoteFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
                AddToMarketDataQryQueue(new RequestContent("QryForQuote", new List<object>()));
            }
            else if (optQuoteFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQryForQuote() = " + optQuoteFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
                AddToMarketDataQryQueue(new RequestContent("QryForQuote", new List<object>()));
            }
            return optQuoteFlag;
        }

        /// 报价录入请求
        public int NewQuoteOrder(string code, EnumThostOffsetFlagType bidOffset, double bidPrice, int bidVolume, EnumThostOffsetFlagType askOffset, double askPrice, int askVolume, string forQuoteID = "")
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! NewQuoteOrder: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int orderFlag = _CtpTraderApi.ReqQuoteInsert(code, bidOffset, bidPrice, bidVolume, askOffset, askPrice, askVolume, forQuoteID);
            Util.Log("TradeApiCTP CtpDataServer: ReqQuoteInsert is Executed.");
            if (orderFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQuoteInsert() = " + orderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                System.Windows.MessageBox.Show("网络连接失败，如发现数据有误，请尝试重启控件", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (orderFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQuoteInsert() = " + orderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("未处理请求超过许可数，如发现数据有误，请尝试重新报单", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            else if (orderFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQuoteInsert() = " + orderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.MessageBox.Show("每秒发送请求数超过许可数，如发现数据有误，请尝试重新报单", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    });
                }
            }
            return orderFlag;
        }

        /// 请求查询报价
        public int QryQuoteOrder()
        {
            //_QuoteOrderDataLst.Clear();
            QryQuoteOrderDataDic.Clear();
            TempQuoteInsertFlag = true;
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! QryQuoteOrder: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int orderFlag = _CtpTraderApi.ReqQryQuote();
            Util.Log("TradeApiCTP CtpDataServer: QryQuoteOrder is Executed.");
            if (orderFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQryQuote() = " + orderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
                System.Windows.MessageBox.Show("网络连接失败，如发现数据有误，请尝试重启控件", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (orderFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQryQuote() = " + orderFlag);
                AddToTradeDataQryQueue(new RequestContent("QryQuoteOrder", new List<object>()));
            }
            else if (orderFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQryQuote() = " + orderFlag);
                AddToTradeDataQryQueue(new RequestContent("QryQuoteOrder", new List<object>()));
            }
            return orderFlag;
        }

        /// 报价操作请求
        public int CancelQuoteOrder(string code, int frontID, int sessioID, string quoteRef, string exchange, string quoteOrderID)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! CancelQuoteOrder: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int qryQuoteActionFlag = _CtpTraderApi.ReqQuoteAction(code, frontID, sessioID, quoteRef, CodeSetManager.ExNameToCtp(exchange), quoteOrderID.Trim());
            Util.Log("TradeApiCTP CtpDataServer: ReqQuoteAction is Executed.");
            if (qryQuoteActionFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQuoteAction() = " + qryQuoteActionFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (qryQuoteActionFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQuoteAction() = " + qryQuoteActionFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
            }
            else if (qryQuoteActionFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQuoteAction() = " + qryQuoteActionFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
            }
            return qryQuoteActionFlag;
        }

        /// 请求查询执行宣告
        public int QryExecOrder()
        {
            //_ExecOrderDataLst.Clear();
            QryExecOrderDataDic.Clear();
            TempExecFlag = true;
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! QryExecOrder: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int qryExecOrderFlag = _CtpTraderApi.ReqQryExecOrder();
            Util.Log("TradeApiCTP CtpDataServer: ReqQryExecOrder is Executed.");
            if (qryExecOrderFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQryExecOrder() = " + qryExecOrderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (qryExecOrderFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQryExecOrder() = " + qryExecOrderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
                AddToMarketDataQryQueue(new RequestContent("QryExecOrder", new List<object>()));
            }
            else if (qryExecOrderFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQryExecOrder() = " + qryExecOrderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
                AddToMarketDataQryQueue(new RequestContent("QryExecOrder", new List<object>()));
            }
            return qryExecOrderFlag;
        }

        /// 执行宣告录入请求
        public int NewExecOrder(string code, int volume, string exchange, bool isExecuted, EnumThostOffsetFlagType offsetFlag = EnumThostOffsetFlagType.CloseToday)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! NewRequestExecOrder: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int execOrderFlag = _CtpTraderApi.ReqExecOrderInsert(code, volume, CodeSetManager.ExNameToCtp(exchange), isExecuted, offsetFlag);
            Util.Log("TradeApiCTP CtpDataServer: ReqExecOrderInsert is Executed.");
            if (execOrderFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqExecOrderInsert() = " + execOrderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (execOrderFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqExecOrderInsert() = " + execOrderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
            }
            else if (execOrderFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqExecOrderInsert() = " + execOrderFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
            }
            return execOrderFlag;
        }

        /// 执行宣告操作请求
        public int CancelExecOrder(string code, int frontID, int sessioID, string execRef, string exchange, string execOrderID)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! CancelExecOrder: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int execOrderActionFlag = _CtpTraderApi.ReqExecOrderAction(code, frontID, sessioID, execRef, CodeSetManager.ExNameToCtp(exchange), execOrderID);
            Util.Log("TradeApiCTP CtpDataServer: ReqExecOrderAction is Executed.");
            if (execOrderActionFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqExecOrderAction() = " + execOrderActionFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (execOrderActionFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqExecOrderAction() = " + execOrderActionFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
            }
            else if (execOrderActionFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqExecOrderAction() = " + execOrderActionFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
            }
            return execOrderActionFlag;
        }

        /// 请求查询申请组合
        public int QryCombinationAction(string code, string exchange)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! QryCombinationAction: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int qryCombActionFlag = _CtpTraderApi.ReqQryCombAction(code, CodeSetManager.ExNameToCtp(exchange));
            Util.Log("TradeApiCTP CtpDataServer: ReqQryCombAction is Executed.");
            if (qryCombActionFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqQryCombAction() = " + qryCombActionFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (qryCombActionFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqQryCombAction() = " + qryCombActionFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
            }
            else if (qryCombActionFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqQryCombAction() = " + qryCombActionFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
            }
            return qryCombActionFlag;
        }

        /// 申请组合录入请求
        public int NewCombinationAction(string code, EnumThostDirectionType direction, int handCount, EnumThostCombDirectionType combDir)
        {
            if (_CtpTraderApi == null)
            {
                Util.Log("Error! NewCombinationAction: tradeApi is null!");
                return CTPDATASERVERERROR;
            }
            ExecQueue.ReqTime = DateTime.Now;
            int reqCombActionFlag = _CtpTraderApi.ReqCombActionInsert(code, direction, handCount, combDir);
            Util.Log("TradeApiCTP CtpDataServer: ReqQryCombAction is Executed.");
            if (reqCombActionFlag == -1)
            {
                Util.Log("Warning! Network Connection Error. ReqCombActionInsert() = " + reqCombActionFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接失败");
            }
            else if (reqCombActionFlag == -2)
            {
                Util.Log("Warning! The number of undealt requests has overlimit the permitted number. ReqCombActionInsert() = " + reqCombActionFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("未处理请求超过许可数");
            }
            else if (reqCombActionFlag == -3)
            {
                Util.Log("Warning! The number of the requests sent per second has overlimit the permitted number. ReqCombActionInsert() = " + reqCombActionFlag);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("每秒发送请求数超过许可数");
            }
            return reqCombActionFlag;
        }

        private Contract GetContractInfoFromQuery(CThostFtdcInstrumentField pInstrument)
        {
            Contract instItem = new Contract();
            try
            {
                instItem.BaseCode = pInstrument.UnderlyingInstrID;
                instItem.Code = pInstrument.ExchangeInstID;
                instItem.ExchCode = pInstrument.ExchangeID;
                instItem.ExpireDate = pInstrument.ExpireDate;
                instItem.Fluct = pInstrument.PriceTick == 0.0 ? 0.01M : (decimal)pInstrument.PriceTick; //(decimal)pInstrument.PriceTick;//Decimal.Parse(pInstrument.PriceTick.ToString());
                instItem.Hycs = pInstrument.VolumeMultiple;
                instItem.IsMaxMarginSingleSide = pInstrument.MaxMarginSideAlgorithm == EnumThostMaxMarginSideAlgorithmType.YES ? true : false;
                instItem.MaxLimitOrderVolume = pInstrument.MaxLimitOrderVolume;
                instItem.MaxMarketOrderVolume = pInstrument.MaxMarketOrderVolume;
                instItem.Name = pInstrument.InstrumentName.Trim();
                instItem.OpenDate = pInstrument.OpenDate;
                instItem.OptionType = GetOptionsType(pInstrument.OptionsType);
                instItem.ProductType = GetProductClassType(pInstrument.ProductClass);
                instItem.SpeciesCode = pInstrument.ProductID;
                instItem.Strike = pInstrument.StrikePrice;
                instItem.LongMarginRatio = pInstrument.LongMarginRatio;
                instItem.ShortMarginRatio = pInstrument.ShortMarginRatio;
                Util.Log(String.Format("Contract item: {0}, Name: {1}, SpeciesCode: {2}, Hycs: {3}, Fluct: {4}, Exchange: {5}, OpenDate:{6}, ExpireDate: {7}, BaseCode: {8}, ProductType: {9}, OptionType: {10}, IsMaxMarginSingleSide: {11}, MaxLimitOrderVolume: {12}, MaxMarketOrderVolume: {13}"
                    , instItem.Code, instItem.Name, instItem.SpeciesCode, instItem.Hycs, instItem.Fluct, instItem.ExchCode, instItem.OpenDate, instItem.ExpireDate, instItem.BaseCode, instItem.ProductType, instItem.OptionType, instItem.IsMaxMarginSingleSide, instItem.MaxLimitOrderVolume, instItem.MaxMarketOrderVolume));
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return instItem;
        }

        private void GetSpecInfoFromInst(Contract contract)
        {
            try
            {
                if (CodeSetManager.SpeciesDict.ContainsKey(contract.SpeciesCode))
                {
                    CodeSetManager.SpeciesDict[contract.SpeciesCode].Codes.Add(contract);
                }
                else
                {
                    //Util.Log("TradeApiCTP CtpDataServer: GetSpecInfoFromInst item: " + contract.SpeciesCode);
                    Species newSpec = new Species(contract.SpeciesCode);
                    newSpec.ChineseName = GetSpeciesName(contract.SpeciesCode);
                    newSpec.Codes.Add(contract);
                    newSpec.ProductType = contract.ProductType;
                    newSpec.ExchangeCode = contract.ExchCode;
                    CodeSetManager.SpeciesDict[contract.SpeciesCode] = newSpec;

                    if (contract.ProductType.Contains("Option"))
                    {
                        CodeSetManager.OptionSpecList.Add(newSpec);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
        }

        private LogonStruct GetLogonReport(CThostFtdcRspUserLoginField pRspUserLogin, string frontType)
        {
            LogonStruct logonStruct = new LogonStruct();
            try
            {
                logonStruct.BackEnd = _BackEnd;
                logonStruct.BrokerID = pRspUserLogin.BrokerID;
                logonStruct.UserID = pRspUserLogin.UserID;
                logonStruct.FrontType = frontType;
                logonStruct.ExchTime = new ExchangeTime(pRspUserLogin.SHFETime, pRspUserLogin.FFEXTime, pRspUserLogin.CZCETime, pRspUserLogin.DCETime, pRspUserLogin.INETime);
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return logonStruct;
        }

        private LogoffStruct GetLogoffReport(CThostFtdcRspUserLoginField pRspUserLogin, CThostFtdcRspInfoField pRspInfo, string frontType)
        {
            LogoffStruct logoffStruct = new LogoffStruct();
            try
            {
                logoffStruct.BackEnd = _BackEnd;
                logoffStruct.BrokerID = pRspUserLogin.BrokerID;
                logoffStruct.UserID = pRspUserLogin.UserID;
                logoffStruct.FrontType = frontType;
                logoffStruct.OutMsg = pRspInfo.ErrorMsg.Trim();
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return logoffStruct;
        }

        private LogoffStruct GetLogoffReport(CThostFtdcUserLogoutField pUserLogout, CThostFtdcRspInfoField pRspInfo, string frontType)
        {
            LogoffStruct logoffStruct = new LogoffStruct();
            try
            {
                logoffStruct.BackEnd = _BackEnd;
                logoffStruct.BrokerID = pUserLogout.BrokerID;
                logoffStruct.UserID = pUserLogout.UserID;
                logoffStruct.FrontType = frontType;
                logoffStruct.OutMsg = pRspInfo.ErrorMsg.Trim();
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return logoffStruct;
        }

        private DisconnectStruct GetDisconnectReport(string frontType)
        {
            DisconnectStruct disStruct = new DisconnectStruct();
            try
            {
                disStruct.BackEnd = _BackEnd;
                if (frontType == "Trader")
                {
                    disStruct.BrokerID = this._CtpTraderApi.BrokerID;
                }
                else if (frontType == "Md")
                {
                    disStruct.BrokerID = this._CtpMdApi.BrokerID;
                }
                disStruct.UserID = this.InvestorID;
                disStruct.FrontType = frontType;
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return disStruct;
        }

        private EnumThostDirectionType GetCtpSideType(SIDETYPE side)
        {
            EnumThostDirectionType directionType = EnumThostDirectionType.Buy;
            if (side == SIDETYPE.BUY)
            {
                directionType = EnumThostDirectionType.Buy;
            }
            else if (side == SIDETYPE.SELL)
            {
                directionType = EnumThostDirectionType.Sell;
            }
            else
            {
                Util.Log("SIDETYPE = " + side);
            }
            return directionType;
        }

        private EnumThostOffsetFlagType GetCtpPosEffectType(PosEffect posEffect)
        {
            EnumThostOffsetFlagType offsetType = EnumThostOffsetFlagType.Open;
            if (posEffect == PosEffect.CloseToday)
            {
                offsetType = EnumThostOffsetFlagType.CloseToday;
            }
            else if (posEffect == PosEffect.Close)
            {
                offsetType = EnumThostOffsetFlagType.Close;
            }
            else if (posEffect == PosEffect.CloseYesterday)
            {
                offsetType = EnumThostOffsetFlagType.CloseYesterday;
            }
            else if (posEffect == PosEffect.ForceClose)
            {
                offsetType = EnumThostOffsetFlagType.ForceClose;
            }
            else if (posEffect == PosEffect.ForceOff)
            {
                offsetType = EnumThostOffsetFlagType.ForceOff;
            }
            else if (posEffect == PosEffect.LocalForceClose)
            {
                offsetType = EnumThostOffsetFlagType.LocalForceClose;
            }
            else if (posEffect != PosEffect.Open)
            {
                Util.Log("PosEffect = " + posEffect.ToString());
            }
            return offsetType;
        }

        private SIDETYPE GetSideType(EnumThostDirectionType directionType)
        {
            SIDETYPE side = SIDETYPE.BUY;
            if (directionType == EnumThostDirectionType.Buy)
            {
                side = SIDETYPE.BUY;
            }
            else if (directionType == EnumThostDirectionType.Sell)
            {
                side = SIDETYPE.SELL;
            }
            else
            {
                Util.Log("EnumThostDirectionType = " + directionType);
            }
            return side;
        }

        private string GetHedgeString(EnumThostHedgeFlagType hedgeFlag)
        {
            if (hedgeFlag != EnumThostHedgeFlagType.Speculation)
            {
                Util.Log("Warning! HedgeFlag: " + hedgeFlag);
                if (hedgeFlag == EnumThostHedgeFlagType.Arbitrage)
                {
                    return "套利";
                }
                else if (hedgeFlag == EnumThostHedgeFlagType.Hedge)
                {
                    return "套保";
                }
                return "未知";
            }
            else
            {
                return "投机";
            }
        }

        private EnumThostHedgeFlagType GetCtpHedgeFlag(EnumHedgeType hedge)
        {
            EnumThostHedgeFlagType hedgeFlag = EnumThostHedgeFlagType.Speculation;
            if (hedge == EnumHedgeType.Arbitrage)
            {
                hedgeFlag = EnumThostHedgeFlagType.Arbitrage;
            }
            else if (hedge == EnumHedgeType.Hedge)
            {
                hedgeFlag = EnumThostHedgeFlagType.Hedge;
            }
            return hedgeFlag;
        }

        private EnumHedgeType GetHedgeType(EnumThostHedgeFlagType hedgeFlag)
        {
            EnumHedgeType hedge = EnumHedgeType.Speculation;
            if (hedgeFlag == EnumThostHedgeFlagType.Arbitrage)
            {
                hedge = EnumHedgeType.Arbitrage;
            }
            else if (hedgeFlag == EnumThostHedgeFlagType.Hedge)
            {
                hedge = EnumHedgeType.Hedge;
            }
            return hedge;
        }

        private PosEffect GetPosEffectType(EnumThostOffsetFlagType offsetType)
        {
            PosEffect posEffect = PosEffect.Unknown;
            if (offsetType == EnumThostOffsetFlagType.Open)
            {
                posEffect = PosEffect.Open;
            }
            else if (offsetType == EnumThostOffsetFlagType.CloseToday)
            {
                posEffect = PosEffect.CloseToday;
            }
            else if (offsetType == EnumThostOffsetFlagType.Close)
            {
                posEffect = PosEffect.Close;
            }
            else if (offsetType == EnumThostOffsetFlagType.CloseYesterday)
            {
                posEffect = PosEffect.CloseYesterday;
            }
            else if (offsetType == EnumThostOffsetFlagType.ForceClose)
            {
                posEffect = PosEffect.ForceClose;
            }
            else if (offsetType == EnumThostOffsetFlagType.ForceOff)
            {
                posEffect = PosEffect.ForceOff;
            }
            else if (offsetType == EnumThostOffsetFlagType.LocalForceClose)
            {
                posEffect = PosEffect.LocalForceClose;
            }
            else
            {
                Util.Log("EnumThostOffsetFlagType = " + offsetType.ToString());
            }
            return posEffect;
        }

        private MarginStruct GetMarginRateReport(CThostFtdcInstrumentMarginRateField pInstrumentMarginRate)
        {
            MarginStruct marginRate = new MarginStruct();
            try
            {
                marginRate.Code = pInstrumentMarginRate.InstrumentID;
                marginRate.InvestorID = pInstrumentMarginRate.InvestorID;
                marginRate.LongMarginRatioByMoney = pInstrumentMarginRate.LongMarginRatioByMoney;
                marginRate.LongMarginRatioByVolume = pInstrumentMarginRate.LongMarginRatioByVolume;
                marginRate.ShortMarginRatioByMoney = pInstrumentMarginRate.ShortMarginRatioByMoney;
                marginRate.ShortMarginRatioByVolume = pInstrumentMarginRate.ShortMarginRatioByVolume;
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return marginRate;
        }

        private MarginStruct GetOptionMarginRateReport(CThostFtdcOptionInstrTradeCostField pOptionInstrTradeCost)
        {
            MarginStruct optMarginRate = new MarginStruct();
            try
            {
                optMarginRate.Code = pOptionInstrTradeCost.InstrumentID;
                optMarginRate.InvestorID = pOptionInstrTradeCost.InvestorID;
                optMarginRate.FixedMargin = pOptionInstrTradeCost.FixedMargin;
                optMarginRate.MiniMargin = pOptionInstrTradeCost.MiniMargin;
                optMarginRate.Royalty = pOptionInstrTradeCost.Royalty;
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return optMarginRate;
        }

        private CommissionStruct GetCommissionRateReport(CThostFtdcInstrumentCommissionRateField pInstrumentCommissionRate)
        {
            CommissionStruct commRate = new CommissionStruct();
            try
            {
                commRate.Code = pInstrumentCommissionRate.InstrumentID;
                commRate.InvestorID = this._CtpTraderApi.InvestorID; // InvestorID is wrong in response
                commRate.CloseRatioByMoney = pInstrumentCommissionRate.CloseRatioByMoney;
                commRate.CloseRatioByVolume = pInstrumentCommissionRate.CloseRatioByVolume;
                commRate.CloseTodayRatioByMoney = pInstrumentCommissionRate.CloseTodayRatioByMoney;
                commRate.CloseTodayRatioByVolume = pInstrumentCommissionRate.CloseTodayRatioByVolume;
                commRate.OpenRatioByMoney = pInstrumentCommissionRate.OpenRatioByMoney;
                commRate.OpenRatioByVolume = pInstrumentCommissionRate.OpenRatioByVolume;
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return commRate;
        }

        private CommissionStruct GetOptionCommissionRateReport(CThostFtdcOptionInstrCommRateField pOptionInstrCommRate)
        {
            CommissionStruct commRate = new CommissionStruct();
            try
            {
                commRate.Code = pOptionInstrCommRate.InstrumentID;
                commRate.InvestorID = pOptionInstrCommRate.InvestorID;
                commRate.CloseRatioByMoney = pOptionInstrCommRate.CloseRatioByMoney;
                commRate.CloseRatioByVolume = pOptionInstrCommRate.CloseRatioByVolume;
                commRate.CloseTodayRatioByMoney = pOptionInstrCommRate.CloseTodayRatioByMoney;
                commRate.CloseTodayRatioByVolume = pOptionInstrCommRate.CloseTodayRatioByVolume;
                commRate.OpenRatioByMoney = pOptionInstrCommRate.OpenRatioByMoney;
                commRate.OpenRatioByVolume = pOptionInstrCommRate.OpenRatioByVolume;
                commRate.StrikeRatioByMoney = pOptionInstrCommRate.StrikeRatioByMoney;
                commRate.StrikeRatioByVolume = pOptionInstrCommRate.StrikeRatioByVolume;
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return commRate;
        }

        public Q7JYOrderData OrderExecutionReport(CThostFtdcOrderField pOrder)
        {
            Q7JYOrderData jyData = new Q7JYOrderData();
            try
            {
                jyData.InvestorID = pOrder.InvestorID;
                jyData.UserID = pOrder.UserID;
                jyData.FrontID = pOrder.FrontID;
                jyData.SessionID = pOrder.SessionID;
                jyData.OrderID = pOrder.OrderSysID.Trim();
                jyData.BrokerID = pOrder.BrokerID.Trim();
                jyData.BrokerOrderSeq = pOrder.BrokerOrderSeq.ToString();
                //jyData.AvgPx = pOrder.LimitPrice;
                jyData.BuySell = pOrder.Direction == EnumThostDirectionType.Buy ? "买" : "卖";
                jyData.Code = pOrder.InstrumentID.Trim();
                //jyData.TaoliDan = "单腿";
                jyData.CommitHandCount = pOrder.VolumeTotalOriginal;
                jyData.TradeHandCount = pOrder.VolumeTraded;
                jyData.CommitPrice = pOrder.LimitPrice;
                jyData.CommitTime = pOrder.InsertTime;
                jyData.UpdateTime = pOrder.UpdateTime;// pOrder.ActiveTime == "" ? "00:00:00" : pOrder.ActiveTime;
                jyData.FeedBackInfo = pOrder.StatusMsg.Trim();
                jyData.OpenClose = GetOffset(pOrder.CombOffsetFlag_0);
                jyData.OrderStatus = GetOrderStatus(pOrder.OrderStatus, pOrder.OrderSubmitStatus, pOrder.StatusMsg);
                jyData.TaoliDan = GetOrderType(pOrder.OrderType);
                jyData.Exchange = CodeSetManager.CtpToExName(pOrder.ExchangeID.Trim());
                jyData.OrderRef = pOrder.OrderRef;
                jyData.RelativeID = pOrder.RelativeOrderSysID;
                jyData.BackEnd = _BackEnd;
                jyData.Hedge = GetHedgeString(pOrder.CombHedgeFlag_0);

                if (pOrder.CancelTime.Length > 0)
                {
                    jyData.UpdateTime = pOrder.CancelTime; //TODO?
                }

                Contract contract = CodeSetManager.GetContractInfo(jyData.Code, CodeSetManager.ExNameToCtp(jyData.Exchange));
                if (contract != null)
                {
                    jyData.Name = contract.Name;
                }

                if (pOrder.LimitPrice == 0 && pOrder.OrderPriceType == EnumThostOrderPriceTypeType.AnyPrice && pOrder.TimeCondition == EnumThostTimeConditionType.IOC)
                {
                    jyData.OrderType = "市价";
                }
                else if (pOrder.OrderPriceType == EnumThostOrderPriceTypeType.LimitPrice && pOrder.TimeCondition == EnumThostTimeConditionType.IOC && pOrder.VolumeCondition == EnumThostVolumeConditionType.AV)
                {
                    jyData.OrderType = "FAK/IOC";
                }
                else if (pOrder.OrderPriceType == EnumThostOrderPriceTypeType.LimitPrice && pOrder.TimeCondition == EnumThostTimeConditionType.IOC && pOrder.VolumeCondition == EnumThostVolumeConditionType.CV)
                {
                    jyData.OrderType = "FOK";
                }
                else
                {
                    jyData.OrderType = "限价";
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return jyData;
        }

        private Q7JYOrderData TradeExecutionReport(CThostFtdcTradeField pTrade)
        {
            Q7JYOrderData tradeData = new Q7JYOrderData();
            try
            {
                tradeData.InvestorID = pTrade.InvestorID;
                tradeData.UserID = pTrade.UserID;
                tradeData.OrderID = pTrade.OrderSysID.Trim();
                tradeData.TradeHandCount = pTrade.Volume;
                tradeData.AvgPx = pTrade.Price;
                tradeData.Exchange = CodeSetManager.CtpToExName(pTrade.ExchangeID.Trim());
                tradeData.TradeTime = pTrade.TradeTime.Trim();
                tradeData.TradeID = pTrade.TradeID.Trim();
                tradeData.Code = pTrade.InstrumentID;
                tradeData.BuySell = pTrade.Direction == EnumThostDirectionType.Buy ? "买" : "卖";
                tradeData.OpenClose = GetOffset(pTrade.OffsetFlag);
                tradeData.BackEnd = _BackEnd;
                tradeData.Hedge = GetHedgeString(pTrade.HedgeFlag);

                Contract contract = CodeSetManager.GetContractInfo(tradeData.Code, CodeSetManager.ExNameToCtp(tradeData.Exchange));
                if (contract != null)
                {
                    tradeData.Name = contract.Name;
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return tradeData;
        }

        public void InitiateCommissionRate(Contract cInfo)
        {
            if (cInfo.ProductType == "Futures")
            {
                AddToTradeDataQryQueue(new RequestContent("RequestCommissionRate", new List<object>() { cInfo.Code }));
            }
            else if (cInfo.ProductType.Contains("Option"))//Todo: options
            {
                AddToTradeDataQryQueue(new RequestContent("RequestOptionsCommissionRate", new List<object>() { cInfo.Code }));
            }
        }

        private Q7PosInfoTotal PositionExecutionReport(CThostFtdcInvestorPositionField pInvestorPosition)
        {
            Q7PosInfoTotal posData = new Q7PosInfoTotal();
            try
            {
                posData.InvestorID = pInvestorPosition.InvestorID;
                posData.UserID = posData.UserID;
                posData.Code = pInvestorPosition.InstrumentID.Trim();
                if (pInvestorPosition.PosiDirection == EnumThostPosiDirectionType.Long)
                {
                    posData.BuySell = "买";
                }
                else if (pInvestorPosition.PosiDirection == EnumThostPosiDirectionType.Short)
                {
                    posData.BuySell = "卖";
                }
                posData.AvgPx = pInvestorPosition.UseMargin / pInvestorPosition.Position;//OpenCost
                posData.Ccyk = pInvestorPosition.PositionProfit;
                //posData.Exchange = CodeSetManager.CtpToExName(pInvestorPosition.ExchangeID.Trim());
                posData.Hedge = GetHedgeString(pInvestorPosition.HedgeFlag);
                posData.OccupyMarginAmt = pInvestorPosition.UseMargin;
                posData.TodayPosition = pInvestorPosition.TodayPosition;
                posData.TotalPosition = pInvestorPosition.Position;
                posData.YesterdayPosition = pInvestorPosition.YdPosition;
                posData.CanCloseCount = pInvestorPosition.Position - pInvestorPosition.LongFrozen - pInvestorPosition.ShortFrozen
                    - pInvestorPosition.StrikeFrozen - pInvestorPosition.AbandonFrozen - pInvestorPosition.CombLongFrozen - pInvestorPosition.CombShortFrozen;
                //posData.Fdyk = pInvestorPosition. pInvestorPosition.OpenCost;
                posData.PrevSettleMent = pInvestorPosition.PreSettlementPrice;
                posData.BackEnd = _BackEnd;
                Contract contract = CodeSetManager.GetContractInfo(posData.Code);
                if (contract != null)
                {
                    posData.Name = contract.Name;
                    posData.ProductType = contract.ProductType;
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return posData;
        }

        private Q7PosInfoDetail PosDetailExecutionReport(CThostFtdcInvestorPositionDetailField pPosDetail)
        {
            //Util.Log("pPosDetail ExecID:" + pPosDetail.TradeID);
            Q7PosInfoDetail posData = new Q7PosInfoDetail();
            try
            {
                posData.InvestorID = pPosDetail.InvestorID;
                posData.UserID = pPosDetail.InvestorID; //?
                posData.ExecID = pPosDetail.TradeID.Trim();
                posData.Code = pPosDetail.InstrumentID.Trim();
                posData.BuySell = pPosDetail.Direction == EnumThostDirectionType.Buy ? "买" : "卖"; ;
                posData.TradeHandCount = pPosDetail.Volume;
                posData.AvgPx = pPosDetail.OpenPrice;
                posData.OccupyMarginAmt = pPosDetail.Margin.ToString();
                posData.Hedge = GetHedgeString(pPosDetail.HedgeFlag);
                posData.PositionType = GetPosType(pPosDetail.OpenDate);
                posData.Ccyk = pPosDetail.PositionProfitByDate;
                posData.Fdyk = pPosDetail.PositionProfitByTrade;
                posData.Exchange = CodeSetManager.CtpToExName(pPosDetail.ExchangeID.Trim());
                posData.PrevSettleMent = pPosDetail.LastSettlementPrice;
                posData.BackEnd = _BackEnd;

                Contract contract = CodeSetManager.GetContractInfo(posData.Code, CodeSetManager.ExNameToCtp(posData.Exchange));
                if (contract != null)
                {
                    posData.Name = contract.Name;
                    posData.ProductType = contract.ProductType;
                    //InitMarginRate(contract, posData.BuySell, posData.AvgPx, posData.PrevSettleMent);
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return posData;
        }

        public void InitiateMarginData(Contract codeInfo, string investorID, double avgPx = 0, double prevSettlement = 0)
        {
            if (codeInfo.ProductType == "Futures")
            {
                AddToTradeDataQryQueue(new RequestContent("RequestMarginRate", new List<object>() { codeInfo.Code }));
            }
            else if (codeInfo.ProductType.Contains("Option"))
            {
                if (_OptionRoyaltyPriceType == EnumThostOptionRoyaltyPriceTypeType.OpenPrice)//(Q7Record.PositionType.Contains("今"))
                {
                    AddToTradeDataQryQueue(new RequestContent("QryOptionTradeCost", new List<object>() { codeInfo.Code, avgPx, 0 }));
                }
                else if (_OptionRoyaltyPriceType == EnumThostOptionRoyaltyPriceTypeType.PreSettlementPrice)//(Q7Record.PositionType.Contains("昨"))
                {
                    AddToTradeDataQryQueue(new RequestContent("QryOptionTradeCost", new List<object>() { codeInfo.Code, prevSettlement, 0 }));
                }
            }
        }

        private CapitalInfo CapitalDetailExecutionReport(CThostFtdcTradingAccountField pTradingAccount)
        {
            CapitalInfo fundData = new CapitalInfo();
            try
            {
                fundData.InvestorID = fundData.UserID = pTradingAccount.AccountID; //?
                fundData.Bond = pTradingAccount.CurrMargin;
                fundData.CapitalID = pTradingAccount.AccountID;
                fundData.Charge = pTradingAccount.Commission;
                fundData.CloseProfit = pTradingAccount.CloseProfit;
                fundData.Credit = pTradingAccount.Credit;
                fundData.InMoney = pTradingAccount.Deposit;
                fundData.DeliveryMargin = pTradingAccount.DeliveryMargin;
                fundData.Fetchable = pTradingAccount.WithdrawQuota;
                fundData.Frozen = pTradingAccount.FrozenCommission + pTradingAccount.FrozenMargin;
                fundData.FrozenCommision = pTradingAccount.FrozenCommission;
                fundData.FrozenMargin = pTradingAccount.FrozenMargin;
                fundData.FrozenRoyalty = pTradingAccount.FrozenCash;
                fundData.LastCredit = pTradingAccount.PreCredit;
                fundData.LastMortage = pTradingAccount.PreMortgage;
                fundData.Mortgage = pTradingAccount.Mortgage;
                fundData.OutMoney = pTradingAccount.Withdraw;
                fundData.OccupyMarginAmt = pTradingAccount.CurrMargin;
                fundData.Royalty = pTradingAccount.CashIn;//Todo
                //fundData.OptionCloseProfit = pTradingAccount.OptionCloseProfit <= 0.00001 ? 0.0 : pTradingAccount.OptionCloseProfit;//Todo
                //fundData.OptionValue = pTradingAccount.OptionValue >= Int64.MaxValue ? 0.0 : pTradingAccount.OptionValue;
                fundData.TodayAvailable = pTradingAccount.Available;
                fundData.Reserve = pTradingAccount.Reserve;
                fundData.TodayEquity = pTradingAccount.PreBalance - pTradingAccount.PreCredit - pTradingAccount.PreMortgage + pTradingAccount.Mortgage - pTradingAccount.Withdraw + pTradingAccount.Deposit;
                fundData.TotalExchangeBond = pTradingAccount.ExchangeMargin;
                fundData.YesterdayEquity = pTradingAccount.PreBalance;
                //pTradingAccount.MortgageableFund;
                //pTradingAccount.CashIn;
                fundData.BackEnd = _BackEnd;
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return fundData;
        }

        private Q7JYOrderData GetParkedOrderExcutionReport(CThostFtdcParkedOrderField pParkedOrder)
        {
            Q7JYOrderData pOrder = new Q7JYOrderData();
            try
            {
                pOrder.InvestorID = pParkedOrder.InvestorID;
                pOrder.UserID = pParkedOrder.UserID;
                pOrder.BuySell = pParkedOrder.Direction == EnumThostDirectionType.Buy ? "买" : "卖";
                pOrder.Code = pParkedOrder.InstrumentID.Trim();
                pOrder.CommitHandCount = pParkedOrder.VolumeTotalOriginal;
                pOrder.CommitPrice = pParkedOrder.LimitPrice;
                pOrder.ConditionOrderCommitTime = DateTime.Now.ToString("HH:mm:ss");//Todo; Client System Time Now
                pOrder.Exchange = CodeSetManager.CtpToExName(pParkedOrder.ExchangeID.Trim());
                pOrder.FeedBackInfo = pParkedOrder.ErrorMsg;
                pOrder.OpenClose = GetOffset(pParkedOrder.CombOffsetFlag_0);
                pOrder.OrderID = pParkedOrder.ParkedOrderID.Trim();
                pOrder.OrderStatus = GetParkedStatus(pParkedOrder.Status);

                if (pParkedOrder.ContingentCondition == EnumThostContingentConditionType.ParkedOrder)
                {
                    pOrder.TouchMethod = "预埋单";
                }
                else if (pParkedOrder.ContingentCondition == EnumThostContingentConditionType.Immediately)
                {
                    pOrder.TouchMethod = "立即";
                }
                else
                {
                    pOrder.TouchMethod = "条件单";
                }
                pOrder.TouchCondition = GetTouchCondition(pParkedOrder.ContingentCondition);
                pOrder.TouchPrice = pParkedOrder.StopPrice.ToString();
                Contract contract = CodeSetManager.GetContractInfo(pOrder.Code, CodeSetManager.ExNameToCtp(pOrder.Exchange));
                if (contract != null)
                {
                    pOrder.Name = contract.Name;
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return pOrder;
        }

        private Q7JYOrderData GetParkedOrderActionExcutionReport(CThostFtdcParkedOrderActionField pParkedOrderAction)
        {
            //Todo
            Q7JYOrderData pOrder = new Q7JYOrderData();
            try
            {
                pOrder.InvestorID = pParkedOrderAction.InvestorID;
                pOrder.UserID = pParkedOrderAction.UserID;
                //pOrder.BuySell = pParkedOrderAction.Direction == EnumThostDirectionType.Buy ? "买" : "卖";
                pOrder.Code = pParkedOrderAction.InstrumentID.Trim();
                pOrder.CommitHandCount = pParkedOrderAction.VolumeChange;
                pOrder.CommitPrice = pParkedOrderAction.LimitPrice;
                pOrder.ConditionOrderCommitTime = DateTime.Now.ToString("HH:mm:ss");//Todo; Client System Time Now
                pOrder.Exchange = CodeSetManager.CtpToExName(pParkedOrderAction.ExchangeID.Trim());
                pOrder.FeedBackInfo = pParkedOrderAction.ErrorMsg;
                //pOrder.OpenClose = GetOffset(pParkedOrderAction.CombOffsetFlag_0);
                pOrder.OrderID = pParkedOrderAction.ParkedOrderActionID.Trim();
                pOrder.OrderStatus = GetParkedStatus(pParkedOrderAction.Status);
                pOrder.BackEnd = _BackEnd;
                //if (pParkedOrderAction.ContingentCondition == EnumThostContingentConditionType.ParkedOrder)
                //{
                //    pOrder.TouchMethod = "预埋单";
                //}
                //else if (pParkedOrderAction.ContingentCondition == EnumThostContingentConditionType.Immediately)
                //{
                //    pOrder.TouchMethod = "立即";
                //}
                //else
                //{
                //    pOrder.TouchMethod = "条件单";
                //}
                //pOrder.TouchCondition = GetTouchCondition(pParkedOrderAction.ContingentCondition);
                //pOrder.TouchPrice = pParkedOrderAction.StopPrice.ToString();

                Contract contract = CodeSetManager.GetContractInfo(pOrder.Code, CodeSetManager.ExNameToCtp(pOrder.Exchange));
                if (contract != null)
                {
                    pOrder.Name = contract.Name;
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return pOrder;
        }

        private string GetExchangeStatus(EnumThostInstrumentStatusType InstrumentStatus)
        {
            string exStatus = String.Empty;
            switch (InstrumentStatus)
            {
                case EnumThostInstrumentStatusType.AuctionBalance:
                    exStatus = "集合竞价价格平衡";
                    break;
                case EnumThostInstrumentStatusType.AuctionMatch:
                    exStatus = "集合竞价撮合";
                    break;
                case EnumThostInstrumentStatusType.AuctionOrdering:
                    exStatus = "集合竞价报单";
                    break;
                case EnumThostInstrumentStatusType.BeforeTrading:
                    exStatus = "开盘前";
                    break;
                case EnumThostInstrumentStatusType.Closed:
                    exStatus = "收盘";
                    break;
                case EnumThostInstrumentStatusType.Continous:
                    exStatus = "连续交易";
                    break;
                case EnumThostInstrumentStatusType.NoTrading:
                    exStatus = "非交易";
                    break;
                default:
                    exStatus = "未知";
                    break;
            }
            return exStatus;
        }

        private string GetOptionsType(EnumThostOptionsTypeType productClassType)
        {
            string optionType = "";
            optionType = Enum.GetName(typeof(EnumThostOptionsTypeType), productClassType);
            return optionType;
        }

        private string GetProductClassType(EnumThostProductClassType productClassType)
        {
            string productClass = "";
            productClass = Enum.GetName(typeof(EnumThostProductClassType), productClassType);
            //switch (productClassType)
            //{
            //    case EnumThostProductClassType.Combination:
            //        productClass = "Combination";
            //        break;
            //    case EnumThostProductClassType.EFP:
            //        productClass = "EFP";
            //        break;
            //    case EnumThostProductClassType.Futures:
            //        productClass = "Futures";
            //        break;
            //    case EnumThostProductClassType.Options:
            //        productClass = "Options";
            //        break;
            //    case EnumThostProductClassType.Spot:
            //        productClass = "Spot";
            //        break;
            //    default:
            //        productClass = "Unknown";
            //        break;
            //}
            return productClass;
        }

        private string GetOrderStatus(EnumThostOrderStatusType status, EnumThostOrderSubmitStatusType submitStatus, string detailInfo)
        {
            string orderStatus = "";
            switch (status)
            {
                case EnumThostOrderStatusType.Unknown:
                    if (submitStatus == EnumThostOrderSubmitStatusType.InsertSubmitted)
                    {
                        orderStatus = "正报";//OrdStatus_PENDING_NEW
                    }
                    else if (submitStatus == EnumThostOrderSubmitStatusType.Accepted)
                    {
                        orderStatus = "已报";//"条件单已完成";//OrdStatus_DONE;
                    }
                    else
                    {
                        orderStatus = "未知";
                    }
                    break;
                case EnumThostOrderStatusType.AllTraded:
                    orderStatus = "全部成交";
                    break;
                case EnumThostOrderStatusType.NoTradeNotQueueing:
                    if (submitStatus == EnumThostOrderSubmitStatusType.InsertSubmitted)
                    {
                        orderStatus = "正报";//OrdStatus_PENDING_NEW
                    }
                    else if (submitStatus == EnumThostOrderSubmitStatusType.Accepted)
                    {
                        orderStatus = "已报";//"条件单已完成";//OrdStatus_DONE;
                    }
                    else
                    {
                        orderStatus = "未报";//OrdStatus_STOPPED;//已排队
                    }
                    break;
                case EnumThostOrderStatusType.NoTradeQueueing:
                    orderStatus = "未成交";
                    break;
                case EnumThostOrderStatusType.Touched:
                    if (submitStatus == EnumThostOrderSubmitStatusType.InsertSubmitted)
                    {
                        orderStatus = "已触发";
                    }
                    //else if (submitStatus == EnumThostOrderSubmitStatusType.Accepted)
                    //{
                    //    orderStatus += "触发已报";
                    //}
                    //else
                    //{
                    //    orderStatus += "触发未报";
                    //}
                    break;
                case EnumThostOrderStatusType.NotTouched:
                    orderStatus = "未触发";
                    break;
                case EnumThostOrderStatusType.Canceled:
                    if (submitStatus == EnumThostOrderSubmitStatusType.InsertRejected)
                    {
                        orderStatus = "已拒绝";
                    }
                    else if (submitStatus == EnumThostOrderSubmitStatusType.Accepted)
                    {
                        orderStatus = "已撤单";
                    }
                    else
                    {
                        orderStatus = "已撤单";
                    }
                    break;
                case EnumThostOrderStatusType.PartTradedQueueing:
                case EnumThostOrderStatusType.PartTradedNotQueueing:
                    {
                        orderStatus = "部分成交";
                        if (detailInfo.Contains("已撤单报单已提交") || detailInfo.Contains("撤单反馈"))
                        {
                            orderStatus = "已撤余单";
                        }
                        break;
                    }
                //case OrderStatus.PendingCancel:
                //    orderStatus = "待撤";
                //    break;
                //case OrderStatus.Failed:
                //    orderStatus = "废单";
                //    break;
                default:
                    orderStatus = "未知";
                    break;
            }
            return orderStatus;
        }

        private string GetOffset(EnumThostOffsetFlagType posOffset)
        {
            string offset = "";
            switch (posOffset)
            {
                case EnumThostOffsetFlagType.Open:
                    offset = "开仓";
                    break;
                case EnumThostOffsetFlagType.Close:
                    offset = "平仓";
                    break;
                case EnumThostOffsetFlagType.CloseToday:
                    offset = "平今";
                    break;
                case EnumThostOffsetFlagType.CloseYesterday:
                    offset = "平昨";
                    break;
                case EnumThostOffsetFlagType.ForceClose:
                    offset = "强平";
                    break;
                case EnumThostOffsetFlagType.ForceOff:
                    offset = "强减";
                    break;
                case EnumThostOffsetFlagType.LocalForceClose:
                    offset = "本地强平";
                    break;
                default:
                    offset = "未知";
                    break;
            }
            return offset;
        }

        private string GetOrderType(EnumThostOrderTypeType orderType)
        {
            string oType = "";
            switch (orderType)
            {
                case EnumThostOrderTypeType.Combination:
                    oType = "组合报单";
                    break;
                case EnumThostOrderTypeType.ConditionalOrder:
                    oType = "条件单";
                    break;
                case EnumThostOrderTypeType.DeriveFromCombination:
                    oType = "组合衍生";
                    break;
                case EnumThostOrderTypeType.DeriveFromQuote:
                    oType = "组合报单";
                    break;
                case 0: //TODO
                case EnumThostOrderTypeType.Normal:
                    oType = "正常";
                    break;
                case EnumThostOrderTypeType.Swap:
                    oType = "互换单";
                    break;
                default:
                    oType = "未知";
                    break;
            }
            return oType;
        }

        private string GetOrderPriceType(EnumThostOrderPriceTypeType type)
        {
            string orderType = "";
            switch (type)
            {
                case EnumThostOrderPriceTypeType.AnyPrice:
                    orderType = "任意价";
                    break;
                case EnumThostOrderPriceTypeType.LimitPrice:
                    orderType = "限价";
                    break;
                case EnumThostOrderPriceTypeType.BestPrice:
                    orderType = "最优价";
                    break;
                case EnumThostOrderPriceTypeType.LastPrice:
                    orderType = "最新价";
                    break;
                case EnumThostOrderPriceTypeType.LastPricePlusOneTicks:
                    orderType = "最新价浮动上浮1个ticks";
                    break;
                case EnumThostOrderPriceTypeType.LastPricePlusTwoTicks:
                    orderType = "最新价浮动上浮2个ticks";
                    break;
                case EnumThostOrderPriceTypeType.LastPricePlusThreeTicks:
                    orderType = "最新价浮动上浮3个ticks";
                    break;
                case EnumThostOrderPriceTypeType.AskPrice1:
                    orderType = "卖一价";
                    break;
                case EnumThostOrderPriceTypeType.AskPrice1PlusOneTicks:
                    orderType = "卖一价浮动上浮1个ticks";
                    break;
                case EnumThostOrderPriceTypeType.AskPrice1PlusTwoTicks:
                    orderType = "卖一价浮动上浮2个ticks";
                    break;
                case EnumThostOrderPriceTypeType.AskPrice1PlusThreeTicks:
                    orderType = "卖一价浮动上浮3个ticks";
                    break;
                case EnumThostOrderPriceTypeType.BidPrice1:
                    orderType = "买一价";
                    break;
                case EnumThostOrderPriceTypeType.BidPrice1PlusOneTicks:
                    orderType = "买一价浮动上浮1个ticks";
                    break;
                case EnumThostOrderPriceTypeType.BidPrice1PlusTwoTicks:
                    orderType = "买一价浮动上浮1个ticks";
                    break;
                case EnumThostOrderPriceTypeType.BidPrice1PlusThreeTicks:
                    orderType = "买一价浮动上浮1个ticks";
                    break;
                default:
                    orderType = "未知";
                    break;

            }
            return orderType;
        }

        private string GetPosType(string openDate)
        {
            string posType = "";
            if (_TradingDay == String.Empty)
            {
                Util.Log("Warning! TradeApiCTP CtpDataServer: GetPosType cannot identify the Trading Date: " + _TradingDay);
                return posType;
            }
            if (String.IsNullOrEmpty(openDate))
            {
                Util.Log("Warning! TradeApiCTP CtpDataServer: Invalid openDate!");
                return posType;
            }
            try
            {
                if (int.Parse(openDate) < int.Parse(_TradingDay))
                {
                    posType = "昨仓";
                }
                else
                {
                    posType = "今仓";
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return posType;
        }

        private string GetSpeciesName(string productID)
        {
            string optionSettingsFile = System.AppDomain.CurrentDomain.BaseDirectory + "/setting/Products.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(optionSettingsFile);
            XmlNodeList nodeLst = doc.SelectNodes("markets/market/product");
            foreach (XmlNode node in nodeLst)
            {
                if (node.Attributes["code"].Value.ToString() == productID)
                {
                    if (node.Attributes["chinesename"] != null)
                    {
                        return node.Attributes["chinesename"].Value.ToString();
                    }
                    else
                    {
                        return productID;
                    }
                }
            }

            //Util.Log("Warning! TradeApiCTP CtpDataServer: GetSpeciesName cannot find the info for productID: " + productID);
            return "";
        }

        private string GetParkedStatus(EnumThostParkedOrderStatusType status)
        {
            string parkedStatus = "";
            //parkedStatus = Enum.GetName(typeof(EnumThostParkedOrderStatusType), status);
            switch (status)
            {
                case EnumThostParkedOrderStatusType.NotSend:
                    parkedStatus = "未发送";
                    break;
                case EnumThostParkedOrderStatusType.Send:
                    parkedStatus = "已发送";
                    break;
                case EnumThostParkedOrderStatusType.Deleted:
                    parkedStatus = "已删除";
                    break;
                default:
                    parkedStatus = "未知";
                    break;
            }
            return parkedStatus;
        }

        private string GetTouchCondition(EnumThostContingentConditionType contingentCondition)
        {
            string touchCondition = "";
            //touchCondition = Enum.GetName(typeof(EnumThostContingentConditionType), contingentCondition);
            switch (contingentCondition)
            {
                case EnumThostContingentConditionType.AskPriceGreaterEqualStopPrice:
                    touchCondition = "卖一价>=条件价";
                    break;
                case EnumThostContingentConditionType.AskPriceGreaterThanStopPrice:
                    touchCondition = "卖一价>条件价";
                    break;
                case EnumThostContingentConditionType.AskPriceLesserEqualStopPrice:
                    touchCondition = "卖一价<=条件价";
                    break;
                case EnumThostContingentConditionType.AskPriceLesserThanStopPrice:
                    touchCondition = "卖一价<条件价";
                    break;
                case EnumThostContingentConditionType.BidPriceGreaterEqualStopPrice:
                    touchCondition = "买一价>=条件价";
                    break;
                case EnumThostContingentConditionType.BidPriceGreaterThanStopPrice:
                    touchCondition = "买一价>条件价";
                    break;
                case EnumThostContingentConditionType.BidPriceLesserEqualStopPrice:
                    touchCondition = "买一价<=条件价";
                    break;
                case EnumThostContingentConditionType.BidPriceLesserThanStopPrice:
                    touchCondition = "买一价<条件价";
                    break;
                case EnumThostContingentConditionType.Immediately:
                    touchCondition = "立即";
                    break;
                case EnumThostContingentConditionType.LastPriceGreaterEqualStopPrice:
                    touchCondition = "最新价>=条件价";
                    break;
                case EnumThostContingentConditionType.LastPriceGreaterThanStopPrice:
                    touchCondition = "最新价>条件价";
                    break;
                case EnumThostContingentConditionType.LastPriceLesserEqualStopPrice:
                    touchCondition = "最新价<=条件价";
                    break;
                case EnumThostContingentConditionType.LastPriceLesserThanStopPrice:
                    touchCondition = "最新价<条件价";
                    break;
                case EnumThostContingentConditionType.ParkedOrder:
                    touchCondition = "预埋单";
                    break;
                case EnumThostContingentConditionType.Touch:
                    touchCondition = "止损";
                    break;
                case EnumThostContingentConditionType.TouchProfit:
                    touchCondition = "止赢";
                    break;
                default:
                    touchCondition = "未知";
                    break;
            }
            return touchCondition;
        }

        private string GetExecOrderStatus(EnumThostExecResultType execResult, EnumThostOrderSubmitStatusType submitStatus, string statusMsg)
        {
            string execStatus = "";
            switch (execResult)
            {
                case EnumThostExecResultType.Unknown:
                    if (submitStatus == EnumThostOrderSubmitStatusType.InsertSubmitted)
                    {
                        execStatus = "正报";
                    }
                    else if (submitStatus == EnumThostOrderSubmitStatusType.Accepted)
                    {
                        execStatus = "已报";
                    }
                    else
                    {
                        execStatus = "未知";
                    }
                    break;
                case EnumThostExecResultType.Canceled:
                    if (submitStatus == EnumThostOrderSubmitStatusType.InsertRejected)
                    {
                        execStatus = "已拒绝";
                    }
                    else if (submitStatus == EnumThostOrderSubmitStatusType.Accepted)
                    {
                        execStatus = "已取消";
                    }
                    else
                    {
                        execStatus = "已取消";
                    }
                    break;
                case EnumThostExecResultType.NoExec:
                    execStatus = "未执行";
                    break;
                case EnumThostExecResultType.OK:
                    execStatus = "执行成功";
                    break;
                //Todo:
                case EnumThostExecResultType.InvalidVolume:
                case EnumThostExecResultType.NoClient:
                case EnumThostExecResultType.NoDeposit:
                case EnumThostExecResultType.NoEnoughHistoryTrade:
                case EnumThostExecResultType.NoInstrument:
                case EnumThostExecResultType.NoParticipant:
                case EnumThostExecResultType.NoPosition:
                case EnumThostExecResultType.NoRight:
                    if (submitStatus == EnumThostOrderSubmitStatusType.Accepted)
                    {
                        execStatus = "申请失败";
                    }
                    else
                    {
                        execStatus = "已拒绝";
                    }
                    break;
                default:
                    execStatus = "未知";
                    break;
            }
            return execStatus;
        }

        private string GetExecResult(EnumThostExecResultType execStatus)
        {
            string result = "";
            result = Enum.GetName(typeof(EnumThostExecResultType), execStatus);
            return result;
        }

        private string GetExecActionType(EnumThostActionTypeType execType)
        {
            string type = "";
            //type = Enum.GetName(typeof(EnumThostActionTypeType), execType);
            if (execType == EnumThostActionTypeType.Exec)
            {
                type = "执行";
            }
            else if (execType == EnumThostActionTypeType.Abandon)
            {
                type = "放弃";
            }
            return type;
        }

        private ContractBank ContractBanksReport(CThostFtdcContractBankField pContractBank)
        {
            ContractBank bank = new ContractBank();
            try
            {
                bank.BankBrchID = pContractBank.BankBrchID;
                bank.BankID = pContractBank.BankID;
                bank.BankName = pContractBank.BankName;
                bank.BrokerID = pContractBank.BrokerID;
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return bank;
        }

        private BankAccountInfo GetBankAccountInfoReport(CThostFtdcAccountregisterField pAccountregister)
        {
            BankAccountInfo bankAcct = new BankAccountInfo();
            try
            {
                bankAcct.AccountID = pAccountregister.AccountID;
                bankAcct.BankAccount = pAccountregister.BankAccount;
                bankAcct.BankAccType = Enum.GetName(typeof(EnumThostBankAccTypeType), pAccountregister.BankAccType);
                bankAcct.BankBrchID = pAccountregister.BankBrchID;
                bankAcct.BankID = pAccountregister.BankID;
                bankAcct.BrokerBranchID = pAccountregister.BrokerBranchID;
                bankAcct.BrokerID = pAccountregister.BrokerID;
                bankAcct.CurrencyID = pAccountregister.CurrencyID;
                bankAcct.CustomerName = pAccountregister.CustomerName;
                bankAcct.CustType = Enum.GetName(typeof(EnumThostCustTypeType), pAccountregister.CustType);
                bankAcct.IdCardType = Enum.GetName(typeof(EnumThostIdCardTypeType), pAccountregister.IdCardType);
                bankAcct.IdentifiedCardNo = pAccountregister.IdentifiedCardNo;
                bankAcct.OpenOrDestroy = Enum.GetName(typeof(EnumThostOpenOrDestroyType), pAccountregister.OpenOrDestroy);
                bankAcct.OutDate = pAccountregister.OutDate;
                bankAcct.RegDate = pAccountregister.RegDate;
                bankAcct.TID = pAccountregister.TID;
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return bankAcct;
        }

        private BankAcctDetail AcctDetailReport(CThostFtdcNotifyQueryAccountField pNotifyQueryAccount)
        {
            BankAcctDetail acctDetail = new BankAcctDetail();
            try
            {
                acctDetail.AccountID = pNotifyQueryAccount.AccountID;
                acctDetail.BankAccount = pNotifyQueryAccount.BankAccount;
                acctDetail.BankAccType = pNotifyQueryAccount.BankAccType;
                acctDetail.BankPassWord = pNotifyQueryAccount.BankPassWord;
                acctDetail.BankSecuAccType = pNotifyQueryAccount.BankSecuAccType;
                acctDetail.BankSecuAcc = pNotifyQueryAccount.BankSecuAcc;
                acctDetail.BankSerial = pNotifyQueryAccount.BankSerial;
                acctDetail.BankFetchAmount = pNotifyQueryAccount.BankFetchAmount;
                acctDetail.BankUseAmount = pNotifyQueryAccount.BankUseAmount;
                acctDetail.BrokerIDByBank = pNotifyQueryAccount.BrokerBranchID;
                acctDetail.CurrencyID = pNotifyQueryAccount.CurrencyID;
                acctDetail.FutureSerial = pNotifyQueryAccount.FutureSerial;
                acctDetail.IdCardType = pNotifyQueryAccount.IdCardType;
                acctDetail.IdentifiedCardNo = pNotifyQueryAccount.IdentifiedCardNo;
                acctDetail.Password = pNotifyQueryAccount.Password;
                acctDetail.PlateSerial = pNotifyQueryAccount.PlateSerial;
                Util.Log(String.Format("User: AccountID {0}, Password {1}, IdCardType {2}, IdentifiedCardNo {3};", acctDetail.AccountID, acctDetail.Password, Enum.GetName(typeof(EnumThostIdCardTypeType), acctDetail.IdCardType), acctDetail.IdentifiedCardNo));
                Util.Log(String.Format("Bank: BankAccount {0}, BankAccType {1}, BankPassWord {2}, BankSecuAccType {3}, BankSecuAcc {4}, BankFetchAmount {5}, BankUseAmount {6}, BrokerIDByBank {7}, CurrencyID {8}."
                    , acctDetail.AccountID, Enum.GetName(typeof(EnumThostBankAccTypeType), acctDetail.BankAccType), acctDetail.BankPassWord, Enum.GetName(typeof(EnumThostBankAccTypeType), acctDetail.BankSecuAccType), acctDetail.BankSecuAcc,
                    acctDetail.BankFetchAmount, acctDetail.BankUseAmount, acctDetail.BrokerIDByBank, acctDetail.CurrencyID));
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return acctDetail;
        }

        private TransferSingleRecord TransferSingleReport(CThostFtdcTransferSerialField pTransferSerial)
        {
            TransferSingleRecord tranRec = new TransferSingleRecord();
            try
            {
                tranRec.BankAcct = pTransferSerial.BankAccount;
                tranRec.SerialNo = pTransferSerial.FutureSerial;
                tranRec.TradedAmt = pTransferSerial.TradeAmount;
                tranRec.TradeInfo = pTransferSerial.ErrorMsg;
                tranRec.TradeType = GetTradeInfoFromTradeCode(pTransferSerial.TradeCode);
                tranRec.TradingTime = pTransferSerial.TradeDate + " " + pTransferSerial.TradeTime;
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return tranRec;
        }

        private TransferSingleRecord NewIncomingTransferReport(CThostFtdcReqTransferField pRspTransfer, CThostFtdcRspInfoField pRspInfo)
        {
            TransferSingleRecord transferRec = new TransferSingleRecord();
            try
            {
                transferRec.BankAcct = pRspTransfer.BankAccount;
                transferRec.SerialNo = pRspTransfer.FutureSerial;
                transferRec.TradedAmt = pRspTransfer.TradeAmount;
                transferRec.TradeInfo = pRspInfo.ErrorMsg;
                transferRec.TradeType = GetTradeInfoFromTradeCode(pRspTransfer.TradeCode);
                transferRec.TradingTime = pRspTransfer.TradingDay + " " + pRspTransfer.TradeTime;
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return transferRec;
        }

        private TransferSingleRecord NewIncomingTransferReport(CThostFtdcRspTransferField pRspTransfer)
        {
            TransferSingleRecord transferRec = new TransferSingleRecord();
            try
            {
                transferRec.BankAcct = pRspTransfer.BankAccount;
                transferRec.SerialNo = pRspTransfer.FutureSerial;
                transferRec.TradedAmt = pRspTransfer.TradeAmount;
                transferRec.TradeInfo = pRspTransfer.ErrorMsg;
                transferRec.TradeType = GetTradeInfoFromTradeCode(pRspTransfer.TradeCode);
                transferRec.TradingTime = pRspTransfer.TradingDay + " " + pRspTransfer.TradeTime;
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return transferRec;
        }

        private QuoteOrderData QuoteOrderExecutionReport(CThostFtdcQuoteField qOrder)
        {
            QuoteOrderData qData = new QuoteOrderData();
            try
            {
                qData.InvestorID = qOrder.InvestorID;
                qData.UserID = qOrder.UserID;
                qData.Code = qOrder.InstrumentID.Trim();
                qData.FrontID = qOrder.FrontID;
                qData.SessionID = qOrder.SessionID;
                qData.QuoteOrderID = qOrder.QuoteSysID.Trim();
                qData.BrokerID = qOrder.BrokerID.Trim();
                qData.BrokerQuoteSeq = qOrder.BrokerQuoteSeq.ToString();
                qData.ForQuoteOrderID = qOrder.ForQuoteSysID.Trim();
                qData.QuoteRef = qOrder.QuoteRef;
                qData.QuoteStatus = GetOrderStatus(qOrder.QuoteStatus, qOrder.OrderSubmitStatus, qOrder.StatusMsg);
                qData.BackEnd = _BackEnd;

                qData.AskPrice = qOrder.AskPrice;
                qData.AskOpenClose = GetOffset(qOrder.AskOffsetFlag);
                qData.AskHandCount = qOrder.AskVolume;
                qData.AskOrderID = qOrder.BidOrderSysID;
                qData.AskOrderRef = qOrder.AskOrderRef;

                qData.BidPrice = qOrder.BidPrice;
                qData.BidOpenClose = GetOffset(qOrder.BidOffsetFlag);
                qData.BidHandCount = qOrder.BidVolume;
                qData.BidOrderID = qOrder.BidOrderSysID;
                qData.BidOrderRef = qOrder.BidOrderRef;

                qData.CommitTime = qOrder.InsertTime;
                qData.UpdateTime = qOrder.InsertTime;
                if (qOrder.CancelTime != "" && qOrder.CancelTime != "00:00:00")
                {
                    qData.UpdateTime = qOrder.CancelTime;
                }
                qData.Exchange = CodeSetManager.CtpToExName(qOrder.ExchangeID.Trim());
                qData.StatusMsg = qOrder.StatusMsg.Trim();

                //if (qOrder.CancelTime.Length > 0)
                //{
                //    qData.TradeTime = qOrder.CancelTime; //TODO?
                //}
                qData.BidHedge = GetHedgeString(qOrder.BidHedgeFlag);
                qData.AskHedge = GetHedgeString(qOrder.AskHedgeFlag);

                Contract contract = CodeSetManager.GetContractInfo(qData.Code, CodeSetManager.ExNameToCtp(qData.Exchange));
                if (contract != null)
                {
                    qData.Name = contract.Name;
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return qData;
        }

        private ExecOrderData OptionExecutionReport(CThostFtdcExecOrderField pExecOrder)
        {
            ExecOrderData execData = new ExecOrderData();
            try
            {
                execData.InvestorID = pExecOrder.InvestorID;
                execData.UserID = pExecOrder.UserID;
                execData.Code = pExecOrder.InstrumentID.Trim();
                execData.FrontID = pExecOrder.FrontID;
                execData.SessionID = pExecOrder.SessionID;
                execData.ExecOrderID = pExecOrder.ExecOrderSysID.Trim();
                execData.BrokerID = pExecOrder.BrokerID.Trim();
                execData.BrokerExecOrderSeq = pExecOrder.BrokerExecOrderSeq.ToString();
                execData.ExecOrderRef = pExecOrder.ExecOrderRef;
                execData.ExecStatus = GetExecOrderStatus(pExecOrder.ExecResult, pExecOrder.OrderSubmitStatus, pExecOrder.StatusMsg);
                execData.Result = GetExecResult(pExecOrder.ExecResult);

                execData.ActionDirection = GetExecActionType(pExecOrder.ActionType);
                execData.OpenClose = GetOffset(pExecOrder.OffsetFlag);
                execData.HandCount = pExecOrder.Volume;
                execData.ExecOrderRef = pExecOrder.ExecOrderRef;

                execData.CommitTime = pExecOrder.InsertTime;
                execData.UpdateTime = pExecOrder.InsertTime;
                if (pExecOrder.CancelTime != "" && pExecOrder.CancelTime != "00:00:00")
                {
                    execData.UpdateTime = pExecOrder.CancelTime;
                }
                execData.Exchange = CodeSetManager.CtpToExName(pExecOrder.ExchangeID.Trim());
                execData.StatusMsg = pExecOrder.StatusMsg.Trim();
                execData.Hedge = GetHedgeString(pExecOrder.HedgeFlag);
                execData.BackEnd = _BackEnd;
                Contract contract = CodeSetManager.GetContractInfo(execData.Code, CodeSetManager.ExNameToCtp(execData.Exchange));
                if (contract != null)
                {
                    execData.Name = contract.Name;
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return execData;
        }

        private string GetTradeInfoFromTradeCode(string tradeCode)
        {
            string tradeType = "";
            switch (tradeCode)
            {
                case ("101001"):
                    tradeType = "银行发起转帐开户";
                    break;
                case ("101002"):
                    tradeType = "银行发起签约销户";
                    break;
                case ("101003"):
                    tradeType = "银行发起帐号变更";
                    break;
                case ("102001"):
                    tradeType = "银行发起资金转期货";
                    break;
                case ("102002"):
                    tradeType = "银行发起期货资金转银行";
                    break;
                case ("103001"):
                    tradeType = "银行发起冲正转期货";
                    break;
                case ("103002"):
                    tradeType = "银行发起冲正期货转银行";
                    break;
                case ("104001"):
                    tradeType = "银行发起查询资金帐户余额";
                    break;
                case ("104003"):
                    tradeType = "银行发起查询账户对应关系";
                    break;
                case ("104005"):
                    tradeType = "完成银行向期货公司发起验证投资账号密码交易，并可查询户信息";
                    break;
                case ("104006"):
                    tradeType = "银行发起查询期货公司系统状态";
                    break;
                case ("105099"):
                    tradeType = "银行下传对帐明细文件";
                    break;
                case ("202001"):
                    tradeType = "期货发起银行资金转期货";
                    break;
                case ("202002"):
                    tradeType = "期货发起资金转银行";
                    break;
                case ("203001"):
                    tradeType = "期货发起冲正银行转期货";
                    break;
                case ("203002"):
                    tradeType = "期货发起冲正转银行";
                    break;
                case ("204002"):
                    tradeType = "期货发起查询银行余额";
                    break;
                case ("204004"):
                    tradeType = "期货发起个人客户查询银直通车开情况";
                    break;
                case ("204005"):
                    tradeType = "期货端发起查询转帐明细";
                    break;
                case ("204006"):
                    tradeType = "期货公司发起查询银行系统状态";
                    break;
                case ("204999"):
                    tradeType = "期货端发起查询客户平台当日流水";
                    break;
                case ("206001"):
                    tradeType = "期货发起银行资金转（入）通知";
                    break;
                case ("206002"):
                    tradeType = "期货发起资金转银行（出）通知";
                    break;
                case ("901001"):
                    tradeType = "个人开通银期直车";
                    break;
                case ("901002"):
                    tradeType = "个人解除银期直通车";
                    break;
                case ("905001"):
                    tradeType = "平台发起期商签到";
                    break;
                case ("905002"):
                    tradeType = "平台发起期商签退";
                    break;
                case ("905003"):
                    tradeType = "期货发起同步密钥";
                    break;
                default:
                    tradeType = "未知类型";
                    break;
            }
            return tradeType;
        }

        public void QuotesLogon()
        {
            if (_CtpMdApi == null)
            {
                Util.Log("Error! QuotesLogon: CtpMdApi is null!");
                return;
            }
            ExecQueue.ReqTime = DateTime.Now;
            _CtpMdApi.ReqUserLogin();
            Util.Log("MdApiCTP CtpDataServer: UserLogin is Executed.");
        }

        public void ReqMarketData(string[] codes)
        {
            if (_CtpMdApi == null)
            {
                Util.Log("Error! ReqMarketData: CtpMdApi is null!");
                return;
            }
            ExecQueue.ReqTime = DateTime.Now;
            _CtpMdApi.SubscribeMarketData(codes);
            Util.Log("MdApiCTP CtpDataServer: SubMarketData is Executed.");
        }

        public void CancelMarketData(string[] codes)
        {
            if (_CtpMdApi == null)
            {
                Util.Log("Error! CancelMarketData: CtpMdApi is null!");
                return;
            }
            ExecQueue.ReqTime = DateTime.Now;
            _CtpMdApi.UnSubscribeMarketData(codes);
            Util.Log("MdApiCTP CtpDataServer: UnSubMarketData is Executed.");
        }

        public void ReqQuotes(string[] codes)
        {
            if (_CtpMdApi == null)
            {
                Util.Log("Error! ReqQuotes: CtpMdApi is null!");
                return;
            }
            ExecQueue.ReqTime = DateTime.Now;
            _CtpMdApi.SubscribeForQuoteRsp(codes);
            Util.Log("MdApiCTP CtpDataServer: SubForQuoteRsp is Executed.");
        }

        public void CancelQuotes(string[] codes)
        {
            if (_CtpMdApi == null)
            {
                Util.Log("Error! CancelQuotes: CtpMdApi is null!");
                return;
            }
            ExecQueue.ReqTime = DateTime.Now;
            _CtpMdApi.UnSubscribeForQuoteRsp(codes);
            Util.Log("MdApiCTP CtpDataServer: UnSubForQuoteRsp is Executed.");
        }

        public void RequestMarketDataDisConnect()
        {
            if (_CtpMdApi == null)
            {
                Util.Log("Warning! RequestMarketDataDisConnect: CtpMdApi is null!");
                return;
            }
            ExecQueue.ReqTime = DateTime.Now;
            IsConnected = false;
            DisconnectStruct disStruct = GetDisconnectReport("Md");
            TradeDataClient.GetClientInstance().RtnMessageEnqueue(disStruct);

            Util.Log("TradeApiCTP CtpDataServer: RequestMarketDataDisConnect: DisConnect starts...");
            try
            {
                _MdCts.Cancel();
                _CtpMdApi.DisConnect();
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        public void InitQuoteApi(string account, string password, string brokerId, string ip)
        {
            _CtpMdApi = ApiQuoteConnection(account, password, brokerId, ip);
            Util.Log("MdApiCTP CtpDataServer: ApiQuoteConnection. Broker = " + _CtpMdApi.BrokerID + ", IP = " + _CtpMdApi.FrontAddr);
            _CtpMdApi.OnFrontConnected += new CtpMdApi.FrontConnected(CtpMdApi_OnFrontConnected);
            _CtpMdApi.OnFrontDisConnected += new CtpMdApi.FrontDisConnected(CtpMdApi_OnFrontDisconnected);
            _CtpMdApi.OnHeartBeatWarning += new CtpMdApi.HeartBeatWarning(CtpMdApi_OnHeartBeatWarning);
            _CtpMdApi.OnRspUserLogin += new CtpMdApi.RspUserLogin(CtpMdApi_OnRspUserLogin);
            _CtpMdApi.OnRspUserLogout += new CtpMdApi.RspUserLogout(CtpMdApi_OnRspUserLogout);
            _CtpMdApi.OnRspError += new CtpMdApi.RspError(CtpMdApi_OnRspError);

            _CtpMdApi.OnRspSubMarketData += new CtpMdApi.RspSubMarketData(CtpMdApi_OnRspSubMarketData);
            _CtpMdApi.OnRspUnSubMarketData += new CtpMdApi.RspUnSubMarketData(CtpMdApi_OnRspUnSubMarketData);
            _CtpMdApi.OnRtnDepthMarketData += new CtpMdApi.RtnDepthMarketData(CtpMdApi_OnRtnDepthMarketData);
            _CtpMdApi.OnRspSubForQuoteRsp += new CtpMdApi.RspSubForQuoteRsp(mdApi_OnRspSubForQuoteRsp);
            _CtpMdApi.OnRspUnSubForQuoteRsp += new CtpMdApi.RspUnSubForQuoteRsp(mdApi_OnRspUnSubForQuoteRsp);
            _CtpMdApi.OnRtnForQuoteRsp += new CtpMdApi.RtnForQuoteRsp(mdApi_OnRtnForQuoteRsp);

            if (_CtpMdApi != null)
            {
                _CtpMdApi.Connect();
            }
            else
            {
                Util.Log("CTP Md API的初始化失败！");
            }
        }

        void CtpMdApi_OnFrontDisconnected(int nReason)
        {
            Util.Log("MdApiCTP CtpDataServer: OnDisConnected is received.");
            IsConnected = false;
            DisconnectStruct disStruct = GetDisconnectReport("Md");
            TradeDataClient.GetClientInstance().RtnMessageEnqueue(disStruct);
        }

        void CtpMdApi_OnHeartBeatWarning(int nTimeLapse)
        {
            //throw new NotImplementedException();
        }

        void CtpMdApi_OnRspError(ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! MdApiCTP CtpDataServer: OnRspError !bIsLast");
                return;
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! MdApiCTP CtpDataServer OnRspError! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg);
            }
        }

        private CtpMdApi ApiQuoteConnection(string userName, string passWord, string brokerId, string ip)
        {
            return new CtpMdApi(userName, passWord, brokerId, ip);
        }

        void CtpMdApi_OnFrontConnected()
        {
            Util.Log("MdApiCTP CtpDataServer: OnFrontConnected is received.");
            AddToMarketDataQryQueue(new RequestContent("QuotesLogon", new List<object>()));
        }

        void CtpMdApi_OnRspUserLogin(ref CThostFtdcRspUserLoginField pRspUserLogin, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! MdApiCTP CtpDataServer: OnRspUserLogin !bIsLast");
            }
            if (pRspInfo.ErrorID == 0)
            {
                Util.Log("MdApiCTP CtpDataServer: Quotes connection is successful.");
                IsConnected = true;
                LogonStruct logonInfo = GetLogonReport(pRspUserLogin, "Md");
                TradeDataClient.GetClientInstance().RtnQueryEnqueue(logonInfo);
            }
            else
            {
                IsConnected = false;
                Util.Log("Error! MdApiCTP CtpDataServer pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                LogoffStruct logoffInfo = GetLogoffReport(pRspUserLogin, pRspInfo, "Md");
                TradeDataClient.GetClientInstance().RtnQueryEnqueue(logoffInfo);
            }
        }

        void CtpMdApi_OnRspUserLogout(ref CThostFtdcUserLogoutField pUserLogout, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! MdApiCTP CtpDataServer: OnRspUserLogout !bIsLast");
            }
            IsConnected = false;
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! MdApiCTP CtpDataServer Logout fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
            }
            else
            {
                Util.Log("Logout succeeds，UserID：" + pUserLogout.UserID);
                LogoffStruct logoffInfo = GetLogoffReport(pUserLogout, pRspInfo, "Md");
                TradeDataClient.GetClientInstance().RtnQueryEnqueue(logoffInfo);
                _CtpMdApi.DisConnect();
                _CtpMdApi = null; //TODO: 被disconnect方法阻塞，无法重置对象
            }
        }

        void CtpMdApi_OnRspSubMarketData(ref CThostFtdcSpecificInstrumentField pSpecificInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            Util.Log("MdApiCTP CtpDataServer: OnRspSubMarketData is received. Instrument:" + pSpecificInstrument.InstrumentID);
            if (pRspInfo.ErrorID == 0)
            {
                //Util.Log("MdApiCTP CtpDataServer: Quotes subscription succeeds");
            }
            else
            {
                Util.Log("Error! MdApiCTP CtpDataServer pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + " Instrument:" + pSpecificInstrument.InstrumentID);
            }
        }

        void CtpMdApi_OnRspUnSubMarketData(ref CThostFtdcSpecificInstrumentField pSpecificInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            Util.Log("MdApiCTP CtpDataServer: OnRspUnSubMarketData is received. Instrument:" + pSpecificInstrument.InstrumentID);
            if (pRspInfo.ErrorID == 0)
            {
                //Util.Log("MdApiCTP CtpDataServer: Quotes unsubscription succeeds");
            }
            else
            {
                Util.Log("Error! MdApiCTP CtpDataServer pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + " Instrument:" + pSpecificInstrument.InstrumentID);
            }
        }

        void CtpMdApi_OnRtnDepthMarketData(ref CThostFtdcDepthMarketDataField pDepthMarketData)
        {
            RealData ctpRealData = MarketDataReport(pDepthMarketData);
            DataContainer.SetRealDataToContainer(ctpRealData);
            TradeDataClient.GetClientInstance().RtnDepthMarketDataEnqueue(ctpRealData);
        }

        void mdApi_OnRspSubForQuoteRsp(ref CThostFtdcSpecificInstrumentField pSpecificInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            Util.Log("MdApiCTP CtpDataServer: OnRspSubForQuoteRsp is received. Instrument:" + pSpecificInstrument.InstrumentID);
            if (pRspInfo.ErrorID == 0)
            {
                //Util.Log("MdApiCTP CtpDataServer: Quotes subscription succeeds");
            }
            else
            {
                Util.Log("Error! MdApiCTP CtpDataServer pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + " Instrument:" + pSpecificInstrument.InstrumentID);
            }
        }

        void mdApi_OnRspUnSubForQuoteRsp(ref CThostFtdcSpecificInstrumentField pSpecificInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            Util.Log("MdApiCTP CtpDataServer: OnRspUnSubForQuoteRsp is received. Instrument:" + pSpecificInstrument.InstrumentID);
            if (pRspInfo.ErrorID == 0)
            {
                //Util.Log("MdApiCTP CtpDataServer: Quotes subscription succeeds");
            }
            else
            {
                Util.Log("Error! MdApiCTP CtpDataServer pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + " Instrument:" + pSpecificInstrument.InstrumentID);
            }
        }

        void mdApi_OnRtnForQuoteRsp(ref CThostFtdcForQuoteRspField pForQuoteRsp)
        {
            Util.Log(String.Format("TradeApiCTP CtpDataServer: OnRtnForQuoteRsp is received. InstrumentID = {0}, ForQuoteSysID = {1}, ForQuoteTime = {2} {3}", pForQuoteRsp.InstrumentID, pForQuoteRsp.ForQuoteSysID, pForQuoteRsp.TradingDay, pForQuoteRsp.ForQuoteTime));
        }

        private RealData MarketDataReport(CThostFtdcDepthMarketDataField pDepthMarketData)
        {
            RealData ctpRealData = new RealData();
            try
            {
                ctpRealData.BidPrice[0] = pDepthMarketData.BidPrice1 >= Double.MaxValue ? 0.0 : pDepthMarketData.BidPrice1;
                ctpRealData.BidPrice[1] = pDepthMarketData.BidPrice2 >= Double.MaxValue ? 0.0 : pDepthMarketData.BidPrice2;
                ctpRealData.BidPrice[2] = pDepthMarketData.BidPrice3 >= Double.MaxValue ? 0.0 : pDepthMarketData.BidPrice3;
                ctpRealData.BidPrice[3] = pDepthMarketData.BidPrice4 >= Double.MaxValue ? 0.0 : pDepthMarketData.BidPrice4;
                ctpRealData.BidPrice[4] = pDepthMarketData.BidPrice5 >= Double.MaxValue ? 0.0 : pDepthMarketData.BidPrice5;
                ctpRealData.BidHand[0] = (uint)pDepthMarketData.BidVolume1;
                ctpRealData.BidHand[1] = (uint)pDepthMarketData.BidVolume2;
                ctpRealData.BidHand[2] = (uint)pDepthMarketData.BidVolume3;
                ctpRealData.BidHand[3] = (uint)pDepthMarketData.BidVolume4;
                ctpRealData.BidHand[4] = (uint)pDepthMarketData.BidVolume5;
                ctpRealData.AskPrice[0] = pDepthMarketData.AskPrice1 >= Double.MaxValue ? 0.0 : pDepthMarketData.AskPrice1;
                ctpRealData.AskPrice[1] = pDepthMarketData.AskPrice2 >= Double.MaxValue ? 0.0 : pDepthMarketData.AskPrice2;
                ctpRealData.AskPrice[2] = pDepthMarketData.AskPrice3 >= Double.MaxValue ? 0.0 : pDepthMarketData.AskPrice3;
                ctpRealData.AskPrice[3] = pDepthMarketData.AskPrice4 >= Double.MaxValue ? 0.0 : pDepthMarketData.AskPrice4;
                ctpRealData.AskPrice[4] = pDepthMarketData.AskPrice5 >= Double.MaxValue ? 0.0 : pDepthMarketData.AskPrice5;
                ctpRealData.AskHand[0] = (uint)pDepthMarketData.AskVolume1;
                ctpRealData.AskHand[1] = (uint)pDepthMarketData.AskVolume2;
                ctpRealData.AskHand[2] = (uint)pDepthMarketData.AskVolume3;
                ctpRealData.AskHand[3] = (uint)pDepthMarketData.AskVolume4;
                ctpRealData.AskHand[4] = (uint)pDepthMarketData.AskVolume5;
                ctpRealData.ClosePrice = pDepthMarketData.ClosePrice >= Double.MaxValue ? 0.0 : pDepthMarketData.ClosePrice;
                ctpRealData.CodeInfo = CodeSetManager.GetContractInfo(pDepthMarketData.InstrumentID);//Todo: No info in pDepthMarketData.ExchangeID field
                //ctpRealData.hand = pDepthMarketData.;
                ctpRealData.MaxPrice = pDepthMarketData.HighestPrice >= Double.MaxValue ? 0.0 : pDepthMarketData.HighestPrice;
                ctpRealData.NewPrice = pDepthMarketData.LastPrice >= Double.MaxValue ? 0.0 : pDepthMarketData.LastPrice;
                ctpRealData.LowerLimitPrice = pDepthMarketData.LowerLimitPrice >= Double.MaxValue ? 0.0 : pDepthMarketData.LowerLimitPrice;
                ctpRealData.MinPrice = pDepthMarketData.LowestPrice >= Double.MaxValue ? 0.0 : pDepthMarketData.LowestPrice;
                ctpRealData.OpenPrice = pDepthMarketData.OpenPrice >= Double.MaxValue ? 0.0 : pDepthMarketData.OpenPrice;
                ctpRealData.Position = (ulong)pDepthMarketData.OpenInterest;//?
                ctpRealData.PrevClose = pDepthMarketData.PreClosePrice >= Double.MaxValue ? 0.0 : pDepthMarketData.PreClosePrice;
                ctpRealData.PrevPosition = (ulong)pDepthMarketData.PreOpenInterest;//?
                ctpRealData.PrevSettlementPrice = pDepthMarketData.PreSettlementPrice >= Double.MaxValue ? 0.0 : pDepthMarketData.PreSettlementPrice;
                ctpRealData.SettlmentPrice = pDepthMarketData.SettlementPrice >= Double.MaxValue ? 0.0 : pDepthMarketData.SettlementPrice;
                ctpRealData.Sum = pDepthMarketData.Turnover;
                ctpRealData.UpdateTime = pDepthMarketData.UpdateTime;
                ctpRealData.UpperLimitPrice = pDepthMarketData.UpperLimitPrice >= Double.MaxValue ? 0.0 : pDepthMarketData.UpperLimitPrice;
                ctpRealData.Volumn = (ulong)pDepthMarketData.Volume;
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
            return ctpRealData;
        }
    }
}
