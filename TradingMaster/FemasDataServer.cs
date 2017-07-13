using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Xml;
using TradingMaster.CodeSet;
using TradingMaster.Control;
using TradingMaster.JYData;

namespace TradingMaster
{
    public class FemasDataServer : DataContainer
    {
        private static FemasDataServer _FemasServerInstance = null;
        private FemasTraderApi _FemasTraderApi = null;
        private FemasMdApi _FemasMdApi = null;

        public override Boolean TradeServerLogOn()
        {
            if (_FemasTraderApi != null)
            {
                return IsLoggedOn;
            }
            return false;
        }

        public override Boolean QuoteServerLogOn()
        {
            if (_FemasMdApi != null)
            {
                return IsConnected;
            }
            return false;
        }

        public override string GetCurrentInvestorID()
        {
            if (_FemasTraderApi != null)
            {
                return _FemasTraderApi.InvestorID;
            }
            return "未获取";
        }

        public override string GetCurrentBroker()
        {
            if (_FemasTraderApi != null)
            {
                return _FemasTraderApi.BrokerID;
            }
            return "未获取";
        }

        public override string GetCurrentTradeAddress()
        {
            if (_FemasTraderApi != null)
            {
                return _FemasTraderApi.FrontAddr;
            }
            return "未获取";
        }

        public override string GetCurrentQuoteAddress()
        {
            if (_FemasMdApi != null)
            {
                return _FemasMdApi.FrontAddr;
            }
            return "未获取";
        }

        public override string GetCounter()
        {
            return "FEMAS";
        }

        private FemasDataServer()
        {
            _TradingCts = new CancellationTokenSource();
            if (_TradingCts != null)
            {
                _TradeDataReqQueue = new ExecQueue(_TradingCts);
                //_TradeDataRspQueue = new RspQueue(TradingCts, BACKENDTYPE.Femas);
            }
            _MdCts = new CancellationTokenSource();
            if (_MdCts != null)
            {
                _MarketDataReqQueue = new ExecQueue(_MdCts);
                //_MarketDataRspQueue = new RspQueue(MdCts, BACKENDTYPE.Femas);
            }
            ServerLock = new object();
            _LogWindow = Login.LoginInstace;
            TempOrderFlag = true;
            TempTradeFlag = true;
            TempPosFlag = true;
            TempQuoteInsertFlag = true;
            TempExecFlag = true;
            _BackEnd = BACKENDTYPE.FEMAS;
        }

        public new static FemasDataServer GetUserInstance()
        {
            if (_FemasServerInstance == null)
            {
                _FemasServerInstance = new FemasDataServer();
            }
            _CounterInstance = _FemasServerInstance;
            return _FemasServerInstance;
        }

        public void InitTradeApi(string account, string password, string brokerId, string ip)
        {
            _FemasTraderApi = ApiTradeConnection(account, password, brokerId, ip);
            Util.Log("TradeApiFemas FemasDataServer: InitTradeApi. Broker = " + _FemasTraderApi.BrokerID + ", IP = " + _FemasTraderApi.FrontAddr);
            _FemasTraderApi.OnFrontConnected += new FemasTraderApi.FrontConnected(FemasTraderApi_OnFrontConnected);
            _FemasTraderApi.OnFrontDisConnected += new FemasTraderApi.FrontDisConnected(FemasTraderApi_OnFrontDisConnected);
            _FemasTraderApi.OnRspUserLogin += new FemasTraderApi.RspUserLogin(FemasTraderApi_OnRspUserLogin);
            _FemasTraderApi.OnRspUserLogout += new FemasTraderApi.RspUserLogout(FemasTraderApi_OnRspUserLogout);
            //_FemasTraderApi.OnRspQryInvestor += new FemasTraderApi.RspQryInvestor(FemasTraderApi_OnRspQryInvestor);
            _FemasTraderApi.OnRspQryTradingCode += new FemasTraderApi.RspQryTradingCode(FemasTraderApi_OnRspQryTradingCode);
            _FemasTraderApi.OnRspError += new FemasTraderApi.RspError(FemasTraderApi_OnRspError);

            _FemasTraderApi.OnRspQryInstrument += new FemasTraderApi.RspQryInstrument(FemasTraderApi_OnRspQryInstrument);
            //_FemasTraderApi.OnRspQryInstrumentMarginRate += new FemasTraderApi.RspQryInstrumentMarginRate(FemasTraderApi_OnRspQryInstrumentMarginRate);
            //_FemasTraderApi.OnRspQryInstrumentCommissionRate += new FemasTraderApi.RspQryInstrumentCommissionRate(FemasTraderApi_OnRspQryInstrumentCommissionRate);
            //_FemasTraderApi.OnRspQryInstrumentOrderCommRate += new FemasTraderApi.RspQryInstrumentOrderCommRate(FemasTraderApi_OnRspQryInstrumentOrderCommRate);
            //_FemasTraderApi.OnRspQryInvestorProductGroupMargin += new FemasTraderApi.RspQryInvestorProductGroupMargin(FemasTraderApi_OnRspQryInvestorProductGroupMargin);

            //_FemasTraderApi.OnRspSettlementInfoConfirm += new FemasTraderApi.RspSettlementInfoConfirm(FemasTraderApi_OnRspSettlementInfoConfirm);
            //_FemasTraderApi.OnRspQrySettlementInfoConfirm += new FemasTraderApi.RspQrySettlementInfoConfirm(FemasTraderApi_OnRspQrySettlementInfoConfirm);
            //_FemasTraderApi.OnRspQrySettlementInfo += new FemasTraderApi.RspQrySettlementInfo(FemasTraderApi_OnRspQrySettlementInfo);
            _FemasTraderApi.OnRspUserPasswordUpdate += new FemasTraderApi.RspUserPasswordUpdate(FemasTraderApi_OnRspUserPasswordUpdate);
            //_FemasTraderApi.OnRspQueryMaxOrderVolume += new FemasTraderApi.RspQueryMaxOrderVolume(FemasTraderApi_OnRspQueryMaxOrderVolume);
            //_FemasTraderApi.OnRtnTradingNotice += new FemasTraderApi.RtnTradingNotice(FemasTraderApi_OnRtnTradingNotice);
            _FemasTraderApi.OnRtnInstrumentStatus += new FemasTraderApi.RtnInstrumentStatus(FemasTraderApi_OnRtnInstrumentStatus);
            //_FemasTraderApi.OnRspQryBrokerTradingParams += new FemasTraderApi.RspQryBrokerTradingParams(FemasTraderApi_OnRspQryBrokerTradingParams);
            //_FemasTraderApi.OnRspQryProductGroup += new FemasTraderApi.RspQryProductGroup(FemasTraderApi_OnRspQryProductGroup);
            //_FemasTraderApi.OnRtnBulletin += new FemasTraderApi.RtnBulletin(FemasTraderApi_OnRtnBulletin);

            _FemasTraderApi.OnRtnOrder += new FemasTraderApi.RtnOrder(FemasTraderApi_OnRtnOrder);
            _FemasTraderApi.OnRspOrderInsert += new FemasTraderApi.RspOrderInsert(FemasTraderApi_OnRspOrderInsert);
            _FemasTraderApi.OnErrRtnOrderInsert += new FemasTraderApi.ErrRtnOrderInsert(FemasTraderApi_OnErrRtnOrderInsert);
            _FemasTraderApi.OnRspOrderAction += new FemasTraderApi.RspOrderAction(FemasTraderApi_OnRspOrderAction);
            _FemasTraderApi.OnErrRtnOrderAction += new FemasTraderApi.ErrRtnOrderAction(FemasTraderApi_OnErrRtnOrderAction);
            _FemasTraderApi.OnRtnTrade += new FemasTraderApi.RtnTrade(FemasTraderApi_OnRtnTrade);
            //_FemasTraderApi.OnRspBatchOrderAction += new FemasTraderApi.RspBatchOrderAction(FemasTraderApi_OnRspBatchOrderAction);
            //_FemasTraderApi.OnErrRtnBatchOrderAction += new FemasTraderApi.ErrRtnBatchOrderAction(FemasTraderApi_OnErrRtnBatchOrderAction);

            _FemasTraderApi.OnRspQryOrder += new FemasTraderApi.RspQryOrder(FemasTraderApi_OnRspQryOrder);
            _FemasTraderApi.OnRspQryTrade += new FemasTraderApi.RspQryTrade(FemasTraderApi_OnRspQryTrade);
            _FemasTraderApi.OnRspQryInvestorPosition += new FemasTraderApi.RspQryInvestorPosition(FemasTraderApi_OnRspQryInvestorPosition);
            //_FemasTraderApi.OnRspQryInvestorPositionDetail += new FemasTraderApi.RspQryInvestorPositionDetail(FemasTraderApi_OnRspQryInvestorPositionDetail);
            //_FemasTraderApi.OnRspQryInvestorPositionCombineDetail += new FemasTraderApi.RspQryInvestorPositionCombineDetail(FemasTraderApi_OnRspQryInvestorPositionCombineDetail);
            //_FemasTraderApi.OnRspQryTradingAccount += new FemasTraderApi.RspQryTradingAccount(FemasTraderApi_OnRspQryTradingAccount);

            ////非银行交互指令
            ////_FemasTraderApi.OnRspQryProductExchRate += new FemasTraderApi.RspQryProductExchRate(FemasTraderApi_OnRspQryProductExchRate);
            //_FemasTraderApi.OnRspQryExchangeRate += new FemasTraderApi.RspQryExchangeRate(FemasTraderApi_OnRspQryExchangeRate);

            ////_FemasTraderApi.OnRspQryContractBank += new FemasTraderApi.RspQryContractBank(FemasTraderApi_OnRspQryContractBank);
            ////_FemasTraderApi.OnRspQryAccountregister += new FemasTraderApi.RspQryAccountregister(FemasTraderApi_OnRspQryAccountregister);
            ////_FemasTraderApi.OnRspTradingAccountPasswordUpdate += new FemasTraderApi.RspTradingAccountPasswordUpdate(FemasTraderApi_OnRspTradingAccountPasswordUpdate);
            ////_FemasTraderApi.OnRspQryTransferSerial += new FemasTraderApi.RspQryTransferSerial(FemasTraderApi_OnRspQryTransferSerial);

            ////银期交互指令
            //_FemasTraderApi.OnRspQueryBankAccountMoneyByFuture += new FemasTraderApi.RspQueryBankAccountMoneyByFuture(FemasTraderApi_OnRspQueryBankAccountMoneyByFuture);
            //_FemasTraderApi.OnRtnQueryBankBalanceByFuture += new FemasTraderApi.RtnQueryBankBalanceByFuture(FemasTraderApi_OnRtnQueryBankBalanceByFuture);
            //_FemasTraderApi.OnErrRtnQueryBankBalanceByFuture += new FemasTraderApi.ErrRtnQueryBankBalanceByFuture(FemasTraderApi_OnErrRtnQueryBankBalanceByFuture);

            //_FemasTraderApi.OnRspFromFutureToBankByFuture += new FemasTraderApi.RspFromFutureToBankByFuture(FemasTraderApi_OnRspFromFutureToBankByFuture);
            //_FemasTraderApi.OnRtnFromFutureToBankByFuture += new FemasTraderApi.RtnFromFutureToBankByFuture(FemasTraderApi_OnRtnFromFutureToBankByFuture);
            //_FemasTraderApi.OnErrRtnFutureToBankByFuture += new FemasTraderApi.ErrRtnFutureToBankByFuture(FemasTraderApi_OnErrRtnFutureToBankByFuture);

            //_FemasTraderApi.OnRspFromBankToFutureByFuture += new FemasTraderApi.RspFromBankToFutureByFuture(FemasTraderApi_OnRspFromBankToFutureByFuture);
            //_FemasTraderApi.OnRtnFromBankToFutureByFuture += new FemasTraderApi.RtnFromBankToFutureByFuture(FemasTraderApi_OnRtnFromBankToFutureByFuture);
            //_FemasTraderApi.OnErrRtnBankToFutureByFuture += new FemasTraderApi.ErrRtnBankToFutureByFuture(FemasTraderApi_OnErrRtnBankToFutureByFuture);

            ////期权交互指令
            //_FemasTraderApi.OnRspQryOptionInstrCommRate += new FemasTraderApi.RspQryOptionInstrCommRate(FemasTraderApi_OnRspQryOptionInstrCommRate);
            //_FemasTraderApi.OnRspQryOptionInstrTradeCost += new FemasTraderApi.RspQryOptionInstrTradeCost(FemasTraderApi_OnRspQryOptionInstrTradeCost);
            //_FemasTraderApi.OnRspQryExecOrder += new FemasTraderApi.RspQryExecOrder(FemasTraderApi_OnRspQryExecOrder);
            //_FemasTraderApi.OnRtnExecOrder += new FemasTraderApi.RtnExecOrder(FemasTraderApi_OnRtnExecOrder);
            //_FemasTraderApi.OnRspExecOrderInsert += new FemasTraderApi.RspExecOrderInsert(FemasTraderApi_OnRspExecOrderInsert);
            //_FemasTraderApi.OnErrRtnExecOrderInsert += new FemasTraderApi.ErrRtnExecOrderInsert(FemasTraderApi_OnErrRtnExecOrderInsert);
            //_FemasTraderApi.OnRspExecOrderAction += new FemasTraderApi.RspExecOrderAction(FemasTraderApi_OnRspExecOrderAction);
            //_FemasTraderApi.OnErrRtnExecOrderAction += new FemasTraderApi.ErrRtnExecOrderAction(FemasTraderApi_OnErrRtnExecOrderAction);

            //做市商交互指令
            //_FemasTraderApi.OnRspQryForQuote += new FemasTraderApi.RspQryForQuote(FemasTraderApi_OnRspQryForQuote);
            //_FemasTraderApi.OnRtnForQuoteRsp += new FemasTraderApi.RtnForQuoteRsp(FemasTraderApi_OnRtnForQuoteRsp);
            //_FemasTraderApi.OnRspForQuoteInsert += new FemasTraderApi.RspForQuoteInsert(FemasTraderApi_OnRspForQuoteInsert);
            //_FemasTraderApi.OnErrRtnForQuoteInsert += new FemasTraderApi.ErrRtnForQuoteInsert(FemasTraderApi_OnErrRtnForQuoteInsert);
            //_FemasTraderApi.OnRspQryQuote += new FemasTraderApi.RspQryQuote(FemasTraderApi_OnRspQryQuote);
            _FemasTraderApi.OnRtnQuote += new FemasTraderApi.RtnQuote(FemasTraderApi_OnRtnQuote);
            _FemasTraderApi.OnRspQuoteInsert += new FemasTraderApi.RspQuoteInsert(FemasTraderApi_OnRspQuoteInsert);
            _FemasTraderApi.OnErrRtnQuoteInsert += new FemasTraderApi.ErrRtnQuoteInsert(FemasTraderApi_OnErrRtnQuoteInsert);
            _FemasTraderApi.OnRspQuoteAction += new FemasTraderApi.RspQuoteAction(FemasTraderApi_OnRspQuoteAction);
            //_FemasTraderApi.OnErrRtnQuoteAction += new FemasTraderApi.ErrRtnQuoteAction(FemasTraderApi_OnErrRtnQuoteAction);
            //_FemasTraderApi.OnRspQryMMInstrumentCommissionRate += new FemasTraderApi.RspQryMMInstrumentCommissionRate(FemasTraderApi_OnRspQryMMInstrumentCommissionRate);
            //_FemasTraderApi.OnRspQryMMOptionInstrCommRate += new FemasTraderApi.RspQryMMOptionInstrCommRate(FemasTraderApi_OnRspQryMMOptionInstrCommRate);

            ////组合保证金指令
            //_FemasTraderApi.OnRtnCombAction += new FemasTraderApi.RtnCombAction(FemasTraderApi_OnRtnCombAction);
            //_FemasTraderApi.OnRspQryCombAction += new FemasTraderApi.RspQryCombAction(FemasTraderApi_OnRspQryCombAction);
            //_FemasTraderApi.OnRspQryCombInstrumentGuard += new FemasTraderApi.RspQryCombInstrumentGuard(FemasTraderApi_OnRspQryCombInstrumentGuard);
            //_FemasTraderApi.OnRspCombActionInsert += new FemasTraderApi.RspCombActionInsert(FemasTraderApi_OnRspCombActionInsert);
            //_FemasTraderApi.OnErrRtnCombActionInsert += new FemasTraderApi.ErrRtnCombActionInsert(FemasTraderApi_OnErrRtnCombActionInsert);

            if (_FemasTraderApi != null)
            {
                _FemasTraderApi.Connect();
            }
            else
            {
                Util.Log("Femas Trader API初始化失败！");
            }
        }

        private FemasTraderApi ApiTradeConnection(string userName, string passWord, string brokerId, string ip)
        {
            InvestorID = userName;
            return new FemasTraderApi(userName, passWord, brokerId, ip);
        }

        void FemasTraderApi_OnFrontConnected()
        {
            Util.Log("TradeApiFemas FemasDataServer: OnFrontConnected is received.");
            AddToTradeDataQryQueue(new RequestContent("ClientLogin", new List<object>()));
        }

        void FemasTraderApi_OnFrontDisConnected(int reason)
        {
            Util.Log("TradeApiFemas FemasDataServer: OnFrontDisConnected is received.");
            TradeDataClient.GetClientInstance().RtnMessageEnqueue("网络连接断开，请尝试点击界面重连或直接重启");
            if (IsLoggedOn)
            {
                IsLoggedOn = false;
                //MessageBox.Show("网络连接失败，请尝试点击界面重连或直接重启", "信息", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            DisconnectStruct disStruct = GetDisconnectReport("Trader");
            TradeDataClient.GetClientInstance().RtnMessageEnqueue(disStruct);
        }

        void FemasTraderApi_OnRspUserLogin(ref CUstpFtdcRspUserLoginField pRspUserLogin, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer: OnRspUserLogin !bIsLast");
            }
            if (pRspInfo.ErrorID == 0)
            {
                string msg = "TradeApiFemas FemasDataServer: User " + pRspUserLogin.UserID + ", Password " + _LogWindow.pb_passWord.Password.ToString();
                Util.Log(msg);
                IsLoggedOn = true;
                if (pRspUserLogin.TradingDay != null && pRspUserLogin.TradingDay.Trim() != "")
                {
                    Util.Log("TradeApiFemas FemasDataServer: OnRspUserLogin - Current Trading Date: " + pRspUserLogin.TradingDay);
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
                Util.Log("Error! TradeApiFemas FemasDataServer pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                LogoffStruct logoffInfo = GetLogoffReport(pRspUserLogin, pRspInfo, "Trader");
                TradeDataClient.GetClientInstance().RtnQueryEnqueue(logoffInfo);
                //OnLogout(pRspInfo.ErrorMsg);
                //PasswordBroker.Broker(getLoginWindow().pb_passWord.Password.ToString());
            }
        }

        void FemasTraderApi_OnRspUserLogout(ref CUstpFtdcRspUserLogoutField pUserLogout, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer: OnRspUserLogout !bIsLast");
            }
            IsLoggedOn = false;
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer Logout fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
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

        void FemasTraderApi_OnRspQryTradingCode(ref CUstpFtdcRspTradingCodeField pTradingCode, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiFemas OnRspQryTradingCode fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
            }
            else
            {
                Util.Log("====================================================");
                Util.Log("ExchangeID: " + pTradingCode.ExchangeID);
                Util.Log("ClientIDType: " + Enum.GetName(typeof(EnumUstpTradingRightType), pTradingCode.ClientRight));
                Util.Log("ClientID: " + pTradingCode.ClientID);
                Util.Log("IsActive: " + pTradingCode.IsActive);
                if (bIsLast)
                {
                    Util.Log("====================================================");
                }
            }
        }

        void FemasTraderApi_OnRspError(ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //针对用户请求的出错通知
            Util.Log("TradeApiFemas FemasDataServer: OnRspError is received.");
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer: OnRspError !bIsLast");
                return;
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer OnRspError! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg);
            }
        }

        void FemasTraderApi_OnRspQryInstrument(ref CUstpFtdcRspInstrumentField pInstrument, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID == 0)
            {
                Contract tempContract = CodeSetManager.GetContractInfo(pInstrument.InstrumentID, pInstrument.ExchangeID);
                if (tempContract == null)
                {
                    Contract instrumentItem = GetContractInfoFromQuery(pInstrument);
                    CodeSetManager.ContractList.Add(instrumentItem);
                    CodeSetManager.ContractMap.Add(instrumentItem.Code + "_" + instrumentItem.ExchCode, instrumentItem);
                    if (pInstrument.OptionsType == EnumUstpOptionsTypeType.CallOptions || pInstrument.OptionsType == EnumUstpOptionsTypeType.PutOptions) //TODO: ProductType cannot be null 
                    {
                        CodeSetManager.LstOptionCodes.Add(instrumentItem.Code);
                    }
                    GetSpecInfoFromInst(instrumentItem);
                }
            }
            else
            {
                Util.Log("Error! TradeApiFemas FemasDataServer pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
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
                Util.Log("TradeApiFemas FemasDataServer: OnRspQryInstrument is received.");
            }
        }

        void FemasTraderApi_OnRspUserPasswordUpdate(ref CUstpFtdcUserPasswordUpdateField pUserPasswordUpdate, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            Util.Log("TradeApiFemas FemasDataServer: OnRspUserPasswordUpdate is received.");
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer: OnRspUserPasswordUpdate !bIsLast");
                return;
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer OnRspUserPasswordUpdate fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg);
            }
            else
            {
                TradeDataClient.GetClientInstance().RtnMessageEnqueue("账户" + pUserPasswordUpdate.UserID + "修改密码成功！");
                MessageBox.Show("修改密码成功！", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        void FemasTraderApi_OnRtnInstrumentStatus(ref CUstpFtdcInstrumentStatusField pInstrumentStatus)
        {
            //Util.Log(string.Format("TradeApiFemas FemasDataServer: OnRtnInstrumentStatus is received: {0} {1} {2}：{3}, reason: {4}"
            //    , pInstrumentStatus.EnterTime, CodeSetManager.FemasToExName(pInstrumentStatus.ExchangeID.Trim()), pInstrumentStatus.InstrumentID.Trim(), GetExchangeStatus(pInstrumentStatus.InstrumentStatus), pInstrumentStatus.EnterReason));
            //TradeDataClient.GetClientInstance().RtnMessageEnqueue(string.Format("{0} {1}：{2}", CodeSetManager.FemasToExName(pInstrumentStatus.ExchangeID.Trim()), pInstrumentStatus.InstrumentID.Trim(), GetExchangeStatus(pInstrumentStatus.InstrumentStatus)));
        }

        void FemasTraderApi_OnRtnOrder(ref CUstpFtdcOrderField pOrder)
        {
            TradeOrderData jyData = OrderExecutionReport(pOrder);
            Util.Log(string.Format("OnRtnOrder pOrder: Code {0}, BrokerOrderSeq {1}, OrderSysID {2}, OrderRef {3}, Status {4}, Hedge {5}",
                jyData.Code, jyData.BrokerOrderSeq, jyData.OrderID, jyData.OrderRef, jyData.FeedBackInfo, jyData.Hedge));
            if (TempOrderFlag) //接收同步的报单回报
            {
                TempOrderData.Add(jyData);
                Util.Log("TradeApiFemas FemasDataServer: OnRtnOrder is delayed.");
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
                                AddToOrderQueue(new RequestContent("NewOrderSingle", new List<object>() { CodeSetManager.GetContractInfo(item.posInfo.Code, CodeSetManager.ExNameToFemas(jyData.Exchange)), isbuy, openClose, item.Price, item.HandCount, 0, "", 0, 0, 0, item.OrderType, CommonUtil.GetHedgeType(item.posInfo.Hedge) }));
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

                                AddToOrderQueue(new RequestContent("NewOrderSingle", new List<object>() { CodeSetManager.GetContractInfo(item.posInfo.Code, CodeSetManager.ExNameToFemas(jyData.Exchange)), isBuy, PosEffect.Open, item.Price, item.HandCount, 0, "", 0, 0, 0, item.OrderType, CommonUtil.GetHedgeType(item.posInfo.Hedge) }));
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

        void FemasTraderApi_OnRtnTrade(ref CUstpFtdcTradeField pTrade)
        {
            TradeOrderData tradedData = TradeExecutionReport(pTrade);
            Util.Log(string.Format("OnRtnTrade Code {0}, TradeID {1}, OrderSysID {2}, BrokerOrderSeq {3}, Volume {4}, Hedge {5}",
                tradedData.Code, tradedData.TradeID, tradedData.OrderID, tradedData.BrokerOrderSeq, tradedData.TradeHandCount, tradedData.Hedge));
            if (TempTradeFlag)
            {
                TempTradeData.Add(tradedData);
                Util.Log("TradeApiFemas FemasDataServer: OnRtnTrade is delayed.");
                return;
            }
            TradeDataClient.GetClientInstance().RtnTradeEnqueue(tradedData);
            if (!TempPosFlag)
            {
                TradeDataClient.GetClientInstance().RtnPositionEnqueue(tradedData);
            }
            //if (!_TradeProcessingFlag) _TradeProcessingFlag = true;
        }

        void FemasTraderApi_OnErrRtnOrderInsert(ref CUstpFtdcInputOrderField pInputOrder, ref CUstpFtdcRspInfoField pRspInfo)
        {
            //交易所认为报单错误(平仓单)
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer OnErrRtnOrderInsert! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pInputOrder.InstrumentID + " UserOrderLocalID:" + pInputOrder.UserOrderLocalID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pInputOrder.InstrumentID);

                # region Clearing Remaining Re-opening Order

                List<PosInfoOrder> openItemLst = new List<PosInfoOrder>();
                if (Enum.GetName(typeof(EnumUstpOffsetFlagType), pInputOrder.OffsetFlag).Contains("Close"))
                {
                    lock (ResetOrderLocker)
                    {
                        foreach (PosInfoOrder item in ResetOrderList)
                        {
                            //if (item.posInfo.Code == jyData.Code && item.BuySell != jyData.BuySell)
                            if (item.posInfo.Code == pInputOrder.InstrumentID //&& item.HandCount == pInputOrder.VolumeTotalOriginal cannot be used for the existance of Close_Today // Not support ExchangeID content!
                                && ((item.BuySell.Contains("买") && pInputOrder.Direction == EnumUstpDirectionType.Sell) || (item.BuySell.Contains("卖") && pInputOrder.Direction == EnumUstpDirectionType.Buy))
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

        void FemasTraderApi_OnRspOrderInsert(ref CUstpFtdcInputOrderField pInputOrder, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //没有没有通过参数校验，拒绝接受报单指令
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer: OnRspOrderInsert !bIsLast");
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer OnRspOrderInsert! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pInputOrder.InstrumentID + " UserOrderLocalID:" + pInputOrder.UserOrderLocalID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pInputOrder.InstrumentID);

                # region Clearing Remaining Re-opening Order

                List<PosInfoOrder> openItemLst = new List<PosInfoOrder>();
                //foreach (TradeOrderData jyData in _JYOrderData)
                //if (jyData.OpenClose.Contains("平") && pInputOrder.OrderRef == jyData.OrderRef)
                if (Enum.GetName(typeof(EnumUstpOffsetFlagType), pInputOrder.OffsetFlag).Contains("Close"))
                {
                    lock (ResetOrderLocker)
                    {
                        foreach (PosInfoOrder item in ResetOrderList)
                        {
                            //if (item.posInfo.Code == jyData.Code && item.BuySell != jyData.BuySell)
                            if (item.posInfo.Code == pInputOrder.InstrumentID //&& item.HandCount == pInputOrder.VolumeTotalOriginal cannot be used for the existance of Close_Today // Not support ExchangeID con
                                && ((item.BuySell.Contains("买") && pInputOrder.Direction == EnumUstpDirectionType.Sell) || (item.BuySell.Contains("卖") && pInputOrder.Direction == EnumUstpDirectionType.Buy))
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

        void FemasTraderApi_OnErrRtnOrderAction(ref CUstpFtdcOrderActionField pOrderAction, ref CUstpFtdcRspInfoField pRspInfo)
        {
            //撤单报单错误
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer OnErrRtnOrderAction! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", OrderId:" + pOrderAction.OrderSysID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 报单号：" + pOrderAction.OrderSysID);
            }
        }

        void FemasTraderApi_OnRspOrderAction(ref CUstpFtdcOrderActionField pInputOrderAction, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //没有通过参数校验，拒绝接受撤单指令
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer: OnRspOrderAction !bIsLast");
                return;
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer OnRspOrderAction! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", OrderId:" + pInputOrderAction.OrderSysID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 报单号：" + pInputOrderAction.OrderSysID);
            }
        }

        void FemasTraderApi_OnRspQryOrder(ref CUstpFtdcOrderField pOrder, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer OnRspQryOrder! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pOrder.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pOrder.InstrumentID);
            }
            else
            {
                if (bIsLast && QryOrderDataDic.Count == 0 && pOrder.InstrumentID.Trim() == "" && pOrder.BrokerID == "") //排除交易日无委托时Femas推送的无用的初始化信息
                {
                    Util.Log("TradeApiFemas FemasDataServer: OnRspQryOrder(last) is received.");
                    TempOrderFlag = false;
                    return;
                }
                TradeOrderData jyData = OrderExecutionReport(pOrder);
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
                    Util.Log("TradeApiFemas FemasDataServer: OnRspQryOrder(last) is received.");
                    TempOrderFlag = false;
                    if (TempOrderData.Count > 0)
                    {
                        foreach (TradeOrderData tempData in TempOrderData)
                        {
                            if (String.IsNullOrEmpty(tempData.Name) || tempData.Name == "")
                            {
                                Contract tempContract = CodeSetManager.GetContractInfo(tempData.Code, CodeSetManager.ExNameToFemas(tempData.Exchange));
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
                    List<TradeOrderData> qryOrderLst = new List<TradeOrderData>();
                    foreach (string key in QryOrderDataDic.Keys)
                    {
                        qryOrderLst.Add(QryOrderDataDic[key]);
                    }
                    qryOrderLst.Sort(TradeOrderData.CompareByCommitTime);
                    TradeDataClient.GetClientInstance().RtnOrderEnqueue(qryOrderLst);
                    TempOrderData.Clear();
                }
            }
        }

        void FemasTraderApi_OnRspQryTrade(ref CUstpFtdcTradeField pTrade, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer OnRspQryOrder! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pTrade.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pTrade.InstrumentID);
            }
            else
            {
                if (bIsLast && QryTradeDataLst.Count == 0 && pTrade.InstrumentID.Trim() == "" && pTrade.BrokerID == "") //排除交易日无委托时Femas推送的无用的初始化信息
                {
                    Util.Log("TradeApiFemas FemasDataServer: OnRspQryTrade(last) is received.");
                    TempTradeFlag = false;
                    return;
                }
                TradeOrderData jyData = TradeExecutionReport(pTrade);
                QryTradeDataLst.Add(jyData);
                if (bIsLast)
                {
                    Util.Log("TradeApiFemas FemasDataServer: OnRspQryTrade(last) is received.");
                    TempTradeFlag = false;
                    if (TempTradeData.Count > 0)
                    {
                        foreach (TradeOrderData tempData in TempTradeData)
                        {
                            if (String.IsNullOrEmpty(tempData.Name) || tempData.Name == "")
                            {
                                Contract tempContract = CodeSetManager.GetContractInfo(tempData.Code, CodeSetManager.ExNameToFemas(tempData.Exchange));
                                if (tempContract != null)
                                {
                                    tempData.Name = tempContract.Name;
                                }
                            }
                            ProcessNewComeTradeInfo(tempData, false);
                            QryTradeDataLst.Add(tempData);
                        }
                    }
                    QryTradeDataLst.Sort(TradeOrderData.CompareByTradeTime);
                    //ProcessTradedOrderData(QryTradeDataLst);
                    TradeDataClient.GetClientInstance().RtnTradeEnqueue(QryTradeDataLst);
                    TempTradeData.Clear();
                }
            }
        }

        void FemasTraderApi_OnRspQryInvestorPosition(ref CUstpFtdcRspInvestorPositionField pInvestorPosition, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast && QryPosDetailData.Count == 0 && pInvestorPosition.InstrumentID.Trim() == "" && pInvestorPosition.BrokerID == "") //排除交易日无委托时Femas推送的无用的初始化信息
            {
                return;
            }
            PosInfoTotal posDetail = PositionExecutionReport(pInvestorPosition);
            JYPosSumData.Add(posDetail);
            if (bIsLast)
            {
                Util.Log("TradeApiFemas FemasDataServer: OnRspQryInvestorPosition(last) is received.");
                lock (ServerLock)
                {
                    JYPosSumData.Sort(PosInfoTotal.CompareByCode);
                    //ProcessPositions_Total(JYPosSumData);
                }
            }
        }

        void FemasTraderApi_OnRtnQuote(ref CUstpFtdcRtnQuoteField pQuote)
        {
            QuoteOrderData quoteData = QuoteOrderExecutionReport(pQuote);
            if (TempQuoteInsertFlag) //接收同步的报价回报
            {
                TempQuoteOrderData.Add(quoteData);
                Util.Log("TradeApiFemas FemasDataServer: OnRtnQuote is delayed.");
                return;
            }
            TradeDataClient.GetClientInstance().RtnOrderEnqueue(quoteData);
        }

        void FemasTraderApi_OnRspQuoteInsert(ref CUstpFtdcInputQuoteField pInputQuote, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer: OnErrRtnQuoteInsert !bIsLast");
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer OnErrRtnQuoteInsert! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pInputQuote.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pInputQuote.InstrumentID);
            }
        }

        void FemasTraderApi_OnErrRtnQuoteInsert(ref CUstpFtdcInputQuoteField pQuoteInsert, ref CUstpFtdcRspInfoField pRspInfo)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer OnErrRtnQuoteInsert! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pQuoteInsert.InstrumentID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", 合约：" + pQuoteInsert.InstrumentID);
            }
        }

        void FemasTraderApi_OnRspQuoteAction(ref CUstpFtdcQuoteActionField pInputQuoteAction, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer: OnErrRtnQuoteAction !bIsLast");
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! TradeApiFemas FemasDataServer OnErrRtnQuoteAction! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", QuoteSysID:" + pInputQuoteAction.QuoteSysID);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg + ", QuoteSysID：" + pInputQuoteAction.QuoteSysID);
            }
        }


        private Contract GetContractInfoFromQuery(CUstpFtdcRspInstrumentField pInstrument)
        {
            Contract instItem = new Contract();
            try
            {
                instItem.BaseCode = pInstrument.UnderlyingInstrID;
                instItem.Code = pInstrument.InstrumentID;
                instItem.ExchCode = pInstrument.ExchangeID;
                instItem.ExpireDate = pInstrument.ExpireDate;
                instItem.Fluct = pInstrument.PriceTick == 0.0 ? 0.01M : (decimal)pInstrument.PriceTick; //(decimal)pInstrument.PriceTick;//Decimal.Parse(pInstrument.PriceTick.ToString());
                instItem.Hycs = pInstrument.VolumeMultiple;
                //instItem.IsMaxMarginSingleSide = pInstrument.MaxMarginSideAlgorithm == EnumUstpMaxMarginSideAlgorithmType.YES ? true : false;
                instItem.MaxLimitOrderVolume = pInstrument.MaxLimitOrderVolume;
                instItem.MaxMarketOrderVolume = pInstrument.MaxMarketOrderVolume;
                instItem.Name = pInstrument.InstrumentName.Trim();
                instItem.OpenDate = pInstrument.OpenDate;
                instItem.OptionType = GetOptionsType(pInstrument.OptionsType);
                //instItem.ProductType = GetProductClassType(pInstrument.ProductClass);
                instItem.SpeciesCode = pInstrument.ProductID;
                instItem.Strike = pInstrument.StrikePrice;
                //instItem.LongMarginRatio = pInstrument.LongMarginRatio;
                //instItem.ShortMarginRatio = pInstrument.ShortMarginRatio;
                Util.Log(string.Format("Contract item: {0}, Name: {1}, SpeciesCode: {2}, Hycs: {3}, Fluct: {4}, Exchange: {5}, OpenDate:{6}, ExpireDate: {7}, BaseCode: {8}, ProductType: {9}, OptionType: {10}, IsMaxMarginSingleSide: {11}, MaxLimitOrderVolume: {12}, MaxMarketOrderVolume: {13}"
                    , instItem.Code, instItem.Name, instItem.SpeciesCode, instItem.Hycs, instItem.Fluct, instItem.ExchCode, instItem.OpenDate, instItem.ExpireDate, instItem.BaseCode, instItem.ProductType, instItem.OptionType, instItem.IsMaxMarginSingleSide, instItem.MaxLimitOrderVolume, instItem.MaxMarketOrderVolume));
            }
            catch (Exception ex)
            {
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error(ex.StackTrace);
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
                    //Util.Log("TradeApiFemas FemasDataServer: GetSpecInfoFromInst item: " + contract.SpeciesCode);
                    Species newSpec = new Species(contract.SpeciesCode);
                    newSpec.ChineseName = GetSpeciesName(contract.SpeciesCode);
                    newSpec.Codes.Add(contract);
                    newSpec.ProductType = contract.ProductType;
                    newSpec.ExchangeCode = contract.ExchCode;
                    CodeSetManager.SpeciesDict[contract.SpeciesCode] = newSpec;

                    if (!string.IsNullOrEmpty(contract.ProductType) && contract.ProductType.Contains("Option"))
                    {
                        CodeSetManager.OptionSpecList.Add(newSpec);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error(ex.StackTrace);
            }
        }

        private string GetOptionsType(EnumUstpOptionsTypeType productClassType)
        {
            string optionType = "";
            optionType = Enum.GetName(typeof(EnumUstpOptionsTypeType), productClassType);
            return optionType;
        }

        //private string GetProductClassType(EnumUstpProductClassType productClassType)
        //{
        //    string productClass = "";
        //    productClass = Enum.GetName(typeof(EnumUstpProductClassType), productClassType);
        //    return productClass;
        //}

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

            //Util.Log("Warning! TradeApiFemas FemasDataServer: GetSpeciesName cannot find the info for productID: " + productID);
            return "";
        }

        private LogonStruct GetLogonReport(CUstpFtdcRspUserLoginField pRspUserLogin, string frontType)
        {
            LogonStruct logonStruct = new LogonStruct();
            try
            {
                logonStruct.BackEnd = _BackEnd;
                logonStruct.BrokerID = pRspUserLogin.BrokerID;
                logonStruct.UserID = pRspUserLogin.UserID;
                logonStruct.FrontType = frontType;
                //logonStruct.ExchTime = new ExchangeTime(pRspUserLogin.SHFETime, pRspUserLogin.FFEXTime, pRspUserLogin.CZCETime, pRspUserLogin.DCETime, pRspUserLogin.INETime);
            }
            catch (Exception ex)
            {
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error(ex.StackTrace);
            }
            return logonStruct;
        }

        private LogoffStruct GetLogoffReport(CUstpFtdcRspUserLoginField pRspUserLogin, CUstpFtdcRspInfoField pRspInfo, string frontType)
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
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error(ex.StackTrace);
            }
            return logoffStruct;
        }

        private LogoffStruct GetLogoffReport(CUstpFtdcRspUserLogoutField pUserLogout, CUstpFtdcRspInfoField pRspInfo, string frontType)
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
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error(ex.StackTrace);
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
                    disStruct.BrokerID = this._FemasTraderApi.BrokerID;
                }
                else if (frontType == "Md")
                {
                    disStruct.BrokerID = this._FemasMdApi.BrokerID;
                }
                disStruct.UserID = this.InvestorID;
                disStruct.FrontType = frontType;
            }
            catch (Exception ex)
            {
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error(ex.StackTrace);
            }
            return disStruct;
        }

        public TradeOrderData OrderExecutionReport(CUstpFtdcOrderField pOrder)
        {
            TradeOrderData jyData = new TradeOrderData();
            try
            {
                jyData.InvestorID = pOrder.InvestorID;
                jyData.UserID = pOrder.UserID;
                //jyData.FrontID = pOrder.FrontID;
                //jyData.SessionID = pOrder.SessionID;
                jyData.OrderID = pOrder.OrderSysID.Trim();
                jyData.BrokerID = pOrder.BrokerID.Trim();
                jyData.BrokerOrderSeq = pOrder.OrderLocalID.ToString();
                //jyData.AvgPx = pOrder.LimitPrice;
                jyData.BuySell = pOrder.Direction == EnumUstpDirectionType.Buy ? "买" : "卖";
                jyData.Code = pOrder.InstrumentID.Trim();
                //jyData.TaoliDan = "单腿";
                jyData.CommitHandCount = pOrder.Volume;
                jyData.TradeHandCount = pOrder.VolumeTraded;
                jyData.CommitPrice = pOrder.LimitPrice;
                jyData.CommitTime = pOrder.InsertTime;
                //jyData.UpdateTime = pOrder.UpdateTime;// pOrder.ActiveTime == "" ? "00:00:00" : pOrder.ActiveTime;
                jyData.FeedBackInfo = pOrder.UserCustom.Trim();
                jyData.OpenClose = GetOffset(pOrder.OffsetFlag);
                jyData.OrderStatus = GetOrderStatus(pOrder.OrderStatus);
                jyData.TaoliDan = GetOrderPriceType(pOrder.OrderPriceType);
                jyData.Exchange = CodeSetManager.FemasToExName(pOrder.ExchangeID.Trim());
                jyData.OrderRef = pOrder.UserOrderLocalID;
                //jyData.RelativeID = pOrder.RelativeOrderSysID;
                jyData.BackEnd = _BackEnd;
                jyData.Hedge = GetHedgeString(pOrder.HedgeFlag);

                if (pOrder.CancelTime.Length > 0)
                {
                    jyData.UpdateTime = pOrder.CancelTime; //TODO?
                }

                Contract contract = CodeSetManager.GetContractInfo(jyData.Code, CodeSetManager.ExNameToFemas(jyData.Exchange));
                if (contract != null)
                {
                    jyData.Name = contract.Name;
                }

                if (pOrder.LimitPrice == 0 && pOrder.OrderPriceType == EnumUstpOrderPriceTypeType.AnyPrice && pOrder.TimeCondition == EnumUstpTimeConditionType.IOC)
                {
                    jyData.OrderType = "市价";
                }
                else if (pOrder.OrderPriceType == EnumUstpOrderPriceTypeType.LimitPrice && pOrder.TimeCondition == EnumUstpTimeConditionType.IOC && pOrder.VolumeCondition == EnumUstpVolumeConditionType.AV)
                {
                    jyData.OrderType = "FAK/IOC";
                }
                else if (pOrder.OrderPriceType == EnumUstpOrderPriceTypeType.LimitPrice && pOrder.TimeCondition == EnumUstpTimeConditionType.IOC && pOrder.VolumeCondition == EnumUstpVolumeConditionType.CV)
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
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error(ex.StackTrace);
            }
            return jyData;
        }

        private TradeOrderData TradeExecutionReport(CUstpFtdcTradeField pTrade)
        {
            TradeOrderData tradeData = new TradeOrderData();
            try
            {
                tradeData.InvestorID = pTrade.InvestorID;
                tradeData.UserID = pTrade.UserID;
                tradeData.OrderID = pTrade.OrderSysID.Trim();
                tradeData.TradeHandCount = pTrade.TradeVolume;
                tradeData.AvgPx = pTrade.TradePrice;
                tradeData.Exchange = CodeSetManager.FemasToExName(pTrade.ExchangeID.Trim());
                tradeData.TradeTime = pTrade.TradeTime.Trim();
                tradeData.TradeID = pTrade.TradeID.Trim();
                tradeData.Code = pTrade.InstrumentID;
                tradeData.BuySell = pTrade.Direction == EnumUstpDirectionType.Buy ? "买" : "卖";
                tradeData.OpenClose = GetOffset(pTrade.OffsetFlag);
                tradeData.BackEnd = _BackEnd;
                tradeData.Hedge = GetHedgeString(pTrade.HedgeFlag);

                Contract contract = CodeSetManager.GetContractInfo(tradeData.Code, CodeSetManager.ExNameToFemas(tradeData.Exchange));
                if (contract != null)
                {
                    tradeData.Name = contract.Name;
                }
            }
            catch (Exception ex)
            {
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error(ex.StackTrace);
            }
            return tradeData;
        }

        private PosInfoTotal PositionExecutionReport(CUstpFtdcRspInvestorPositionField pInvestorPosition)
        {
            PosInfoTotal posData = new PosInfoTotal();
            try
            {
                posData.InvestorID = pInvestorPosition.InvestorID;
                posData.UserID = posData.UserID;
                posData.Code = pInvestorPosition.InstrumentID.Trim();
                if (pInvestorPosition.Direction == EnumUstpDirectionType.Buy)
                {
                    posData.BuySell = "买";
                }
                else if (pInvestorPosition.Direction == EnumUstpDirectionType.Sell)
                {
                    posData.BuySell = "卖";
                }
                posData.AvgPx = pInvestorPosition.UsedMargin / pInvestorPosition.Position;//OpenCost
                //posData.Ccyk = pInvestorPosition.PositionProfit;
                //posData.Exchange = CodeSetManager.FemasToExName(pInvestorPosition.ExchangeID.Trim());
                posData.Hedge = GetHedgeString(pInvestorPosition.HedgeFlag);
                posData.OccupyMarginAmt = pInvestorPosition.UsedMargin;
                posData.TotalPosition = pInvestorPosition.Position;
                posData.YesterdayPosition = pInvestorPosition.YdPosition;
                posData.TodayPosition = pInvestorPosition.Position > pInvestorPosition.YdPosition ? pInvestorPosition.Position - pInvestorPosition.YdPosition : 0;
                posData.CanCloseCount = pInvestorPosition.Position;//pInvestorPosition.Position - pInvestorPosition.LongFrozen - pInvestorPosition.ShortFrozen
                //    - pInvestorPosition.StrikeFrozen - pInvestorPosition.AbandonFrozen - pInvestorPosition.CombLongFrozen - pInvestorPosition.CombShortFrozen;
                //posData.Fdyk = pInvestorPosition. pInvestorPosition.OpenCost;
                posData.PrevSettleMent = pInvestorPosition.YdPositionCost;
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
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error(ex.StackTrace);
            }
            return posData;
        }

        private QuoteOrderData QuoteOrderExecutionReport(CUstpFtdcRtnQuoteField qOrder)
        {
            QuoteOrderData qData = new QuoteOrderData();
            try
            {
                qData.InvestorID = qOrder.InvestorID;
                qData.UserID = qOrder.UserID;
                qData.Code = qOrder.InstrumentID.Trim();
                //qData.FrontID = qOrder.FrontID;
                //qData.SessionID = qOrder.SessionID;
                qData.QuoteOrderID = qOrder.QuoteSysID.Trim();
                qData.BrokerID = qOrder.BrokerID.Trim();
                qData.BrokerQuoteSeq = qOrder.QuoteLocalID.ToString();
                //qData.ForQuoteOrderID = qOrder.ForQuoteSysID.Trim();
                qData.QuoteRef = qOrder.UserQuoteLocalID;
                qData.QuoteStatus = GetOrderStatus(qOrder.QuoteStatus);
                qData.BackEnd = _BackEnd;

                qData.AskPrice = qOrder.AskPrice;
                qData.AskOpenClose = GetOffset(qOrder.AskOffsetFlag);
                qData.AskHandCount = qOrder.AskVolume;
                qData.AskOrderID = qOrder.BidOrderSysID;
                qData.AskOrderRef = qOrder.AskUserOrderLocalID;

                qData.BidPrice = qOrder.BidPrice;
                qData.BidOpenClose = GetOffset(qOrder.BidOffsetFlag);
                qData.BidHandCount = qOrder.BidVolume;
                qData.BidOrderID = qOrder.BidOrderSysID;
                qData.BidOrderRef = qOrder.BidUserOrderLocalID;

                qData.CommitTime = qOrder.InsertTime;
                qData.UpdateTime = qOrder.InsertTime;
                if (qOrder.CancelTime != "" && qOrder.CancelTime != "00:00:00")
                {
                    qData.UpdateTime = qOrder.CancelTime;
                }
                qData.Exchange = CodeSetManager.CtpToExName(qOrder.ExchangeID.Trim());
                qData.StatusMsg = qOrder.UserCustom.Trim();

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
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error(ex.StackTrace);
            }
            return qData;
        }

        private string GetOffset(EnumUstpOffsetFlagType posOffset)
        {
            string offset = "";
            switch (posOffset)
            {
                case EnumUstpOffsetFlagType.Open:
                    offset = "开仓";
                    break;
                case EnumUstpOffsetFlagType.Close:
                    offset = "平仓";
                    break;
                case EnumUstpOffsetFlagType.CloseToday:
                    offset = "平今";
                    break;
                case EnumUstpOffsetFlagType.CloseYesterday:
                    offset = "平昨";
                    break;
                case EnumUstpOffsetFlagType.ForceClose:
                    offset = "强平";
                    break;
                default:
                    offset = "未知";
                    break;
            }
            return offset;
        }

        private string GetOrderStatus(EnumUstpOrderStatusType status)
        {
            string orderStatus = "";
            switch (status)
            {
                case EnumUstpOrderStatusType.AcceptedNoReply:
                    orderStatus = "正报";
                    break;
                case EnumUstpOrderStatusType.AllTraded:
                    orderStatus = "全部成交";
                    break;
                case EnumUstpOrderStatusType.NoTradeNotQueueing:
                    orderStatus = "已报";
                    break;
                case EnumUstpOrderStatusType.NoTradeQueueing:
                    orderStatus = "未成交";
                    break;
                case EnumUstpOrderStatusType.Canceled:
                    orderStatus = "已撤单";
                    break;
                case EnumUstpOrderStatusType.PartTradedQueueing:
                    orderStatus = "部分成交";
                    break;
                case EnumUstpOrderStatusType.PartTradedNotQueueing:
                    orderStatus = "已撤余单";
                    break;
                default:
                    orderStatus = "未知";
                    break;
            }
            return orderStatus;
        }

        private string GetOrderStatus(EnumUstpQuoteStatusType status)
        {
            string orderStatus = "";
            switch (status)
            {
                case EnumUstpQuoteStatusType.Inited_InFEMAS:
                    orderStatus = "正报";
                    break;
                case EnumUstpQuoteStatusType.Accepted_InTradingSystem:
                    orderStatus = "已报";
                    break;
                case EnumUstpQuoteStatusType.Error_QuoteAction:
                    orderStatus = "已拒绝";
                    break;
                case EnumUstpQuoteStatusType.Canceled_All:
                    orderStatus = "已撤单";
                    break;
                case EnumUstpQuoteStatusType.Canceled_SingleLeg:
                    orderStatus = "单边撤单";
                    break;
                case EnumUstpQuoteStatusType.Traded_All:
                    orderStatus = "全部成交";
                    break;
                case EnumUstpQuoteStatusType.Traded_SingleLeg:
                    orderStatus = "单边成交";
                    break;
                default:
                    orderStatus = "未知";
                    break;
            }
            return orderStatus;
        }

        private string GetOrderPriceType(EnumUstpOrderPriceTypeType orderType)
        {
            string oType = "";
            switch (orderType)
            {
                case EnumUstpOrderPriceTypeType.AnyPrice:
                    oType = "任意价";
                    break;
                case EnumUstpOrderPriceTypeType.BestPrice:
                    oType = "最优价";
                    break;
                case EnumUstpOrderPriceTypeType.FiveLevelPrice:
                    oType = "五档价";
                    break;
                case EnumUstpOrderPriceTypeType.LimitPrice:
                    oType = "限价";
                    break;
                default:
                    oType = "未知";
                    break;
            }
            return oType;
        }

        private string GetHedgeString(EnumUstpHedgeFlagType hedgeFlag)
        {
            if (hedgeFlag != EnumUstpHedgeFlagType.Speculation)
            {
                Util.Log("Warning! HedgeFlag: " + hedgeFlag);
                if (hedgeFlag == EnumUstpHedgeFlagType.Arbitrage)
                {
                    return "套利";
                }
                else if (hedgeFlag == EnumUstpHedgeFlagType.Hedge)
                {
                    return "套保";
                }
                else if (hedgeFlag == EnumUstpHedgeFlagType.MarketMaker)
                {
                    return "做市商";
                }
                return "未知";
            }
            else
            {
                return "投机";
            }
        }

        private string GetExchangeStatus(EnumUstpInstrumentStatusType InstrumentStatus)
        {
            string exStatus = String.Empty;
            switch (InstrumentStatus)
            {
                case EnumUstpInstrumentStatusType.AuctionBalance:
                    exStatus = "集合竞价价格平衡";
                    break;
                case EnumUstpInstrumentStatusType.AuctionMatch:
                    exStatus = "集合竞价撮合";
                    break;
                case EnumUstpInstrumentStatusType.AuctionOrdering:
                    exStatus = "集合竞价报单";
                    break;
                case EnumUstpInstrumentStatusType.BeforeTrading:
                    exStatus = "开盘前";
                    break;
                case EnumUstpInstrumentStatusType.Closed:
                    exStatus = "收盘";
                    break;
                case EnumUstpInstrumentStatusType.Continous:
                    exStatus = "连续交易";
                    break;
                case EnumUstpInstrumentStatusType.NoTrading:
                    exStatus = "非交易";
                    break;
                default:
                    exStatus = "未知";
                    break;
            }
            return exStatus;
        }

        public void InitDataFromAPI()
        {
            if (_FemasTraderApi != null)
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
                //AddToTradeDataQryQueue(new FemasRequestContent("RequestCommissionRate", new List<object>() { null }));
                //AddToTradeDataQryQueue(new FemasRequestContent("RequestMarginRate", new List<object>() { null }));
                //AddToTradeDataQryQueue(new FemasRequestContent("ReqPosition", new List<object>()));
            }
            InitTransferFromAPI();
        }

        public void InitTransferFromAPI()
        {
            if (_FemasTraderApi != null)
            {
                AddToTradeDataQryQueue(new RequestContent("QryInterBanks", new List<object>()));
            }
        }

        public void InitQuoteApi(string account, string password, string brokerId, string ip)
        {
            _FemasMdApi = ApiQuoteConnection(account, password, brokerId, ip);
            Util.Log("MdApiFemas FemasDataServer: ApiQuoteConnection. Broker = " + _FemasMdApi.BrokerID + ", IP = " + _FemasMdApi.FrontAddr);
            _FemasMdApi.OnFrontConnected += new FemasMdApi.FrontConnected(FemasMdApi_OnFrontConnected);
            _FemasMdApi.OnFrontDisConnected += new FemasMdApi.FrontDisConnected(FemasMdApi_OnFrontDisconnected);
            _FemasMdApi.OnHeartBeatWarning += new FemasMdApi.HeartBeatWarning(FemasMdApi_OnHeartBeatWarning);
            _FemasMdApi.OnRspUserLogin += new FemasMdApi.RspUserLogin(FemasMdApi_OnRspUserLogin);
            _FemasMdApi.OnRspUserLogout += new FemasMdApi.RspUserLogout(FemasMdApi_OnRspUserLogout);
            _FemasMdApi.OnRspError += new FemasMdApi.RspError(FemasMdApi_OnRspError);

            _FemasMdApi.OnRspSubMarketData += new FemasMdApi.RspSubMarketData(FemasMdApi_OnRspSubMarketData);
            _FemasMdApi.OnRspUnSubMarketData += new FemasMdApi.RspUnSubMarketData(FemasMdApi_OnRspUnSubMarketData);
            _FemasMdApi.OnRtnDepthMarketData += new FemasMdApi.RtnDepthMarketData(FemasMdApi_OnRtnDepthMarketData);
            //_FemasMdApi.OnRspSubForQuoteRsp += new FemasMdApi.RspSubForQuoteRsp(mdApi_OnRspSubForQuoteRsp);
            //_FemasMdApi.OnRspUnSubForQuoteRsp += new FemasMdApi.RspUnSubForQuoteRsp(mdApi_OnRspUnSubForQuoteRsp);
            //_FemasMdApi.OnRtnForQuoteRsp += new FemasMdApi.RtnForQuoteRsp(mdApi_OnRtnForQuoteRsp);

            if (_FemasMdApi != null)
            {
                _FemasMdApi.Connect();
            }
            else
            {
                Util.Log("Femas Md API的初始化失败！");
            }
        }

        void FemasMdApi_OnFrontDisconnected(int nReason)
        {
            Util.Log("MdApiFemas FemasDataServer: OnDisConnected is received.");
            IsConnected = false;
            DisconnectStruct disStruct = GetDisconnectReport("Md");
            TradeDataClient.GetClientInstance().RtnMessageEnqueue(disStruct);
        }

        void FemasMdApi_OnHeartBeatWarning(int nTimeLapse)
        {
            //throw new NotImplementedException();
        }

        void FemasMdApi_OnRspError(ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! MdApiFemas FemasDataServer: OnRspError !bIsLast");
                return;
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! MdApiFemas FemasDataServer OnRspError! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                TradeDataClient.GetClientInstance().RtnMessageEnqueue(pRspInfo.ErrorMsg);
            }
        }

        private FemasMdApi ApiQuoteConnection(string userName, string passWord, string brokerId, string ip)
        {
            return new FemasMdApi(userName, passWord, brokerId, ip);
        }

        void FemasMdApi_OnFrontConnected()
        {
            Util.Log("MdApiFemas FemasDataServer: OnFrontConnected is received.");
            AddToMarketDataQryQueue(new RequestContent("QuotesLogon", new List<object>()));
        }

        void FemasMdApi_OnRspUserLogin(ref CUstpFtdcRspUserLoginField pRspUserLogin, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! MdApiFemas FemasDataServer: OnRspUserLogin !bIsLast");
            }
            if (pRspInfo.ErrorID == 0)
            {
                Util.Log("MdApiFemas FemasDataServer: Quotes connection is successful.");
                IsConnected = true;
                LogonStruct logonInfo = GetLogonReport(pRspUserLogin, "Md");
                TradeDataClient.GetClientInstance().RtnQueryEnqueue(logonInfo);
            }
            else
            {
                IsConnected = false;
                Util.Log("Error! MdApiFemas FemasDataServer pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
                LogoffStruct logoffInfo = GetLogoffReport(pRspUserLogin, pRspInfo, "Md");
                TradeDataClient.GetClientInstance().RtnQueryEnqueue(logoffInfo);
            }
        }

        void FemasMdApi_OnRspUserLogout(ref CUstpFtdcRspUserLogoutField pUserLogout, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.Log("Error! MdApiFemas FemasDataServer: OnRspUserLogout !bIsLast");
            }
            IsConnected = false;
            if (pRspInfo.ErrorID != 0)
            {
                Util.Log("Error! MdApiFemas FemasDataServer Logout fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
            }
            else
            {
                Util.Log("Logout succeeds，UserID：" + pUserLogout.UserID);
                LogoffStruct logoffInfo = GetLogoffReport(pUserLogout, pRspInfo, "Md");
                TradeDataClient.GetClientInstance().RtnQueryEnqueue(logoffInfo);
                _FemasMdApi.DisConnect();
                _FemasMdApi = null; //TODO: 被disconnect方法阻塞，无法重置对象
            }
        }

        void FemasMdApi_OnRspSubMarketData(ref CUstpFtdcSpecificInstrumentField pSpecificInstrument, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            Util.Log("MdApiFemas FemasDataServer: OnRspSubMarketData is received. Instrument:" + pSpecificInstrument.InstrumentID);
            if (pRspInfo.ErrorID == 0)
            {
                //Util.Log("MdApiFemas FemasDataServer: Quotes subscription succeeds");
            }
            else
            {
                Util.Log("Error! MdApiFemas FemasDataServer pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + " Instrument:" + pSpecificInstrument.InstrumentID);
            }
        }

        void FemasMdApi_OnRspUnSubMarketData(ref CUstpFtdcSpecificInstrumentField pSpecificInstrument, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            Util.Log("MdApiFemas FemasDataServer: OnRspUnSubMarketData is received. Instrument:" + pSpecificInstrument.InstrumentID);
            if (pRspInfo.ErrorID == 0)
            {
                //Util.Log("MdApiFemas FemasDataServer: Quotes unsubscription succeeds");
            }
            else
            {
                Util.Log("Error! MdApiFemas FemasDataServer pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + " Instrument:" + pSpecificInstrument.InstrumentID);
            }
        }

        void FemasMdApi_OnRtnDepthMarketData(ref CUstpFtdcDepthMarketDataField pDepthMarketData)
        {
            RealData FemasRealData = MarketDataReport(pDepthMarketData);
            DataContainer.SetRealDataToContainer(FemasRealData);
            TradeDataClient.GetClientInstance().RtnDepthMarketDataEnqueue(FemasRealData);
        }

        private RealData MarketDataReport(CUstpFtdcDepthMarketDataField pDepthMarketData)
        {
            RealData femasRealData = new RealData();
            try
            {
                femasRealData.BidPrice[0] = pDepthMarketData.BidPrice1 >= Double.MaxValue ? 0.0 : pDepthMarketData.BidPrice1;
                femasRealData.BidPrice[1] = pDepthMarketData.BidPrice2 >= Double.MaxValue ? 0.0 : pDepthMarketData.BidPrice2;
                femasRealData.BidPrice[2] = pDepthMarketData.BidPrice3 >= Double.MaxValue ? 0.0 : pDepthMarketData.BidPrice3;
                femasRealData.BidPrice[3] = pDepthMarketData.BidPrice4 >= Double.MaxValue ? 0.0 : pDepthMarketData.BidPrice4;
                femasRealData.BidPrice[4] = pDepthMarketData.BidPrice5 >= Double.MaxValue ? 0.0 : pDepthMarketData.BidPrice5;
                femasRealData.BidHand[0] = (uint)pDepthMarketData.BidVolume1;
                femasRealData.BidHand[1] = (uint)pDepthMarketData.BidVolume2;
                femasRealData.BidHand[2] = (uint)pDepthMarketData.BidVolume3;
                femasRealData.BidHand[3] = (uint)pDepthMarketData.BidVolume4;
                femasRealData.BidHand[4] = (uint)pDepthMarketData.BidVolume5;
                femasRealData.AskPrice[0] = pDepthMarketData.AskPrice1 >= Double.MaxValue ? 0.0 : pDepthMarketData.AskPrice1;
                femasRealData.AskPrice[1] = pDepthMarketData.AskPrice2 >= Double.MaxValue ? 0.0 : pDepthMarketData.AskPrice2;
                femasRealData.AskPrice[2] = pDepthMarketData.AskPrice3 >= Double.MaxValue ? 0.0 : pDepthMarketData.AskPrice3;
                femasRealData.AskPrice[3] = pDepthMarketData.AskPrice4 >= Double.MaxValue ? 0.0 : pDepthMarketData.AskPrice4;
                femasRealData.AskPrice[4] = pDepthMarketData.AskPrice5 >= Double.MaxValue ? 0.0 : pDepthMarketData.AskPrice5;
                femasRealData.AskHand[0] = (uint)pDepthMarketData.AskVolume1;
                femasRealData.AskHand[1] = (uint)pDepthMarketData.AskVolume2;
                femasRealData.AskHand[2] = (uint)pDepthMarketData.AskVolume3;
                femasRealData.AskHand[3] = (uint)pDepthMarketData.AskVolume4;
                femasRealData.AskHand[4] = (uint)pDepthMarketData.AskVolume5;
                femasRealData.ClosePrice = pDepthMarketData.ClosePrice >= Double.MaxValue ? 0.0 : pDepthMarketData.ClosePrice;
                femasRealData.CodeInfo = CodeSetManager.GetContractInfo(pDepthMarketData.InstrumentID);//Todo: No info in pDepthMarketData.ExchangeID field
                //FemasRealData.hand = pDepthMarketData.;
                femasRealData.MaxPrice = pDepthMarketData.HighestPrice >= Double.MaxValue ? 0.0 : pDepthMarketData.HighestPrice;
                femasRealData.NewPrice = pDepthMarketData.LastPrice >= Double.MaxValue ? 0.0 : pDepthMarketData.LastPrice;
                femasRealData.LowerLimitPrice = pDepthMarketData.LowerLimitPrice >= Double.MaxValue ? 0.0 : pDepthMarketData.LowerLimitPrice;
                femasRealData.MinPrice = pDepthMarketData.LowestPrice >= Double.MaxValue ? 0.0 : pDepthMarketData.LowestPrice;
                femasRealData.OpenPrice = pDepthMarketData.OpenPrice >= Double.MaxValue ? 0.0 : pDepthMarketData.OpenPrice;
                femasRealData.Position = (ulong)pDepthMarketData.OpenInterest;//?
                femasRealData.PrevClose = pDepthMarketData.PreClosePrice >= Double.MaxValue ? 0.0 : pDepthMarketData.PreClosePrice;
                femasRealData.PrevPosition = (ulong)pDepthMarketData.PreOpenInterest;//?
                femasRealData.PrevSettlementPrice = pDepthMarketData.PreSettlementPrice >= Double.MaxValue ? 0.0 : pDepthMarketData.PreSettlementPrice;
                femasRealData.SettlmentPrice = pDepthMarketData.SettlementPrice >= Double.MaxValue ? 0.0 : pDepthMarketData.SettlementPrice;
                femasRealData.Sum = pDepthMarketData.Turnover;
                femasRealData.UpdateTime = string.Format("{0}.{1:D3}", pDepthMarketData.UpdateTime, pDepthMarketData.UpdateMillisec);
                femasRealData.UpperLimitPrice = pDepthMarketData.UpperLimitPrice >= Double.MaxValue ? 0.0 : pDepthMarketData.UpperLimitPrice;
                femasRealData.Volumn = (ulong)pDepthMarketData.Volume;
            }
            catch (Exception ex)
            {
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error("exception: " + ex.StackTrace);
            }
            return femasRealData;
        }
    }
}
