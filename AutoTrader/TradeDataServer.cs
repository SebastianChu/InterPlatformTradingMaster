using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Xml;

namespace AutoTrader
{
    public class TradeDataServer
    {
        public delegate void DepthMarketDataEvent(CThostFtdcDepthMarketDataField pDepthMarketData);
        public delegate void DepthMarketDataInitEvent(string instrumentId, List<CThostFtdcDepthMarketDataField> pDepthMarketDataLst);
        public delegate void RtnOrderStatusEvent(CThostFtdcOrderField pOrder, bool isQry = false);
        public delegate void RtnFilledEvent(CThostFtdcTradeField pTrade);

        public AutoResetEvent AutoInitEvent = new AutoResetEvent(false);
        public AutoResetEvent AutoCapitalEvent = new AutoResetEvent(false);
        public AutoResetEvent AutoOrderEvent = new AutoResetEvent(false);
        public AutoResetEvent AutoTradeEvent = new AutoResetEvent(false);
        public AutoResetEvent AutoPositionEvent = new AutoResetEvent(false);
        public AutoResetEvent AutoMarginEvent = new AutoResetEvent(false);
        public AutoResetEvent AutoCommissionEvent = new AutoResetEvent(false);

        public TradeDataServer()
        {
            DicPendingOrder = new ConcurrentDictionary<int, ConcurrentDictionary<string, CThostFtdcOrderField>>();
            DepthMarketDataQueue = new ConcurrentDictionary<string, ConcurrentDictionary<int, ConcurrentQueue<CThostFtdcDepthMarketDataField>>>();
            InstrumentCategoryMarginRateDic = new ConcurrentDictionary<string, CThostFtdcInstrumentMarginRateField>();
            InstrumentCategoryCommissionRateDic = new ConcurrentDictionary<string, CThostFtdcInstrumentCommissionRateField>();
            TradingAccountCapitalDic = new ConcurrentDictionary<string, CThostFtdcTradingAccountField>();
            //FixTradedCost = new ConcurrentDictionary<string, double>();
            RiskManager = new RiskManager(this);
            OrderManager = new OrderManager(this);
            RestartObject = new object();
            _TradingWatch = new Stopwatch();
            _AlgoTrader = new AlgorithmicTrader(this);// Thread Launching Strategy
            IsInited = Init();
            if (string.IsNullOrEmpty(Util.ConfigFile))
            {
                _AlgoTrader.Launch();
            }
            else// if (!IsInited)
            {
                ParseStrategyXmlParameter(AppDomain.CurrentDomain.BaseDirectory + Util.ConfigFile);
                LaunchStrategies(Util.StrategyParameter.Keys, false);
            }
        }

        public CtpTraderApi _CtpTraderApi { get; set; }
        public CtpMdApi _CtpMdApi { get; set; }
        //public int ShortPeriod;
        //public int LongPeriod;
        public RiskManager RiskManager { get; set; }
        public OrderManager OrderManager { get; set; }
        public ConcurrentDictionary<int, ConcurrentDictionary<string, CThostFtdcOrderField>> DicPendingOrder { get; set; }
        public ConcurrentDictionary<string, CThostFtdcInstrumentCommissionRateField> InstrumentCategoryCommissionRateDic { get; set; }
        public ConcurrentDictionary<string, CThostFtdcInstrumentMarginRateField> InstrumentCategoryMarginRateDic { get; set; }
        //public ConcurrentDictionary<string, double> FixTradedCost { get; set; }
        public ConcurrentDictionary<string, CThostFtdcTradingAccountField> TradingAccountCapitalDic { get; set; }

        private ConcurrentDictionary<string, Contract> _instrumentFields = new ConcurrentDictionary<string, Contract>();
        public ConcurrentDictionary<string, Contract> InstrumentFields
        {
            get { return _instrumentFields; }
            set { _instrumentFields = value; }
        }

        private ConcurrentDictionary<string, CThostFtdcOrderField> _orderFields = new ConcurrentDictionary<string, CThostFtdcOrderField>();
        public ConcurrentDictionary<string, CThostFtdcOrderField> OrderFields
        {
            get { return _orderFields; }
            set { _orderFields = value; }
        }

        private ConcurrentDictionary<string, CThostFtdcTradeField> _tradedFields = new ConcurrentDictionary<string, CThostFtdcTradeField>();
        public ConcurrentDictionary<string, CThostFtdcTradeField> TradedFields
        {
            get { return _tradedFields; }
            set { _tradedFields = value; }
        }

        private ConcurrentDictionary<int, PositionField> _positionFields = new ConcurrentDictionary<int, PositionField>();
        public ConcurrentDictionary<int, PositionField> PositionFields
        {
            get { return _positionFields; }
            set { _positionFields = value; }
        }

        private ConcurrentDictionary<string, CThostFtdcDepthMarketDataField> _depthMarketDataFields = new ConcurrentDictionary<string, CThostFtdcDepthMarketDataField>();
        public ConcurrentDictionary<string, CThostFtdcDepthMarketDataField> DepthMarketDataFields
        {
            get { return _depthMarketDataFields; }
            set { _depthMarketDataFields = value; }
        }

        public event DepthMarketDataEvent DepthMarketDataProcessing;
        public event DepthMarketDataInitEvent DepthMarketDataInit;
        public event RtnOrderStatusEvent RtnNotifyOrderStatus;
        public event RtnFilledEvent RtnNotifyFilled;
        private Stopwatch _TradingWatch { get; set; }
        private AlgorithmicTrader _AlgoTrader { get; set; }
        private ConcurrentDictionary<string, ConcurrentDictionary<int, ConcurrentQueue<CThostFtdcDepthMarketDataField>>> DepthMarketDataQueue { get; set; }
        private List<string> _LaunchedInstrumentLst = new List<string>();
        private bool IsInited = false;
        private object RestartObject;

        public string InvestorID { get; set; }

        private CtpTraderApi ApiTradeConnection(string userName, string passWord, string brokerId, string ip)
        {
            InvestorID = userName;
            return new CtpTraderApi(userName, passWord, brokerId, ip);
        }

        private bool Init()
        {
            try
            {
                Util.WriteInfo("读取交易所时间对应表");
                _CtpTraderApi = ApiTradeConnection
                (
                    ConfigurationManager.AppSettings["Account"],
                    ConfigurationManager.AppSettings["Password"],
                    ConfigurationManager.AppSettings["BrokerId"],
                    ConfigurationManager.AppSettings["TraderFront"]
                );
                Util.WriteInfo("登录： 账号 = " + _CtpTraderApi.InvestorID + "， Broker = " + _CtpTraderApi.BrokerID + ", IP = " + _CtpTraderApi.FrontAddr);
                if (ConfigurationManager.AppSettings["BrokerId"] == "9999")
                {
                    InitQuoteApi("6001731", "cx170417", "9099", "tcp://180.166.13.41:41213");
                }
                else
                {
                    InitQuoteApi
                        (
                            ConfigurationManager.AppSettings["Account"],
                            ConfigurationManager.AppSettings["Password"],
                            ConfigurationManager.AppSettings["BrokerId"],
                            ConfigurationManager.AppSettings["TraderFront"]
                        );
                }
                InitTradeApi();
                LoginTrader();
                InitDataFromAPI();
            }
            catch (Exception ex)
            {
                Util.WriteExceptionToLogFile(ex);
            }
            return true;
        }

        private void InitTradeApi()
        {
            if (_CtpTraderApi != null)
            {
                _CtpTraderApi.OnFrontConnected += new CtpTraderApi.FrontConnected(CtpTraderApi_OnFrontConnected);
                //_CtpTraderApi.OnFrontDisConnected += new CtpTraderApi.FrontDisConnected(CtpTraderApi_OnFrontDisConnected);
                _CtpTraderApi.OnRspUserLogin += new CtpTraderApi.RspUserLogin(CtpTraderApi_OnRspUserLogin);
                //_CtpTraderApi.OnRspUserLogout += new CtpTraderApi.RspUserLogout(CtpTraderApi_OnRspUserLogout);
                //_CtpTraderApi.OnRspError += new CtpTraderApi.RspError(CtpTraderApi_OnRspError);

                _CtpTraderApi.OnRspQryInstrument += new CtpTraderApi.RspQryInstrument(CtpTraderApi_OnRspQryInstrument);
                _CtpTraderApi.OnRspQryInstrumentMarginRate += new CtpTraderApi.RspQryInstrumentMarginRate(CtpTraderApi_OnRspQryInstrumentMarginRate);
                _CtpTraderApi.OnRspQryInstrumentCommissionRate += new CtpTraderApi.RspQryInstrumentCommissionRate(CtpTraderApi_OnRspQryInstrumentCommissionRate);
                //_CtpTraderApi.OnRspQryInstrumentOrderCommRate += new CtpTraderApi.RspQryInstrumentOrderCommRate(CtpTraderApi_OnRspQryInstrumentOrderCommRate);

                //_CtpTraderApi.OnRspSettlementInfoConfirm += new CtpTraderApi.RspSettlementInfoConfirm(CtpTraderApi_OnRspSettlementInfoConfirm);
                //_CtpTraderApi.OnRspQrySettlementInfoConfirm += new CtpTraderApi.RspQrySettlementInfoConfirm(CtpTraderApi_OnRspQrySettlementInfoConfirm);
                //_CtpTraderApi.OnRspQrySettlementInfo += new CtpTraderApi.RspQrySettlementInfo(CtpTraderApi_OnRspQrySettlementInfo);
                //_CtpTraderApi.OnRspUserPasswordUpdate += new CtpTraderApi.RspUserPasswordUpdate(CtpTraderApi_OnRspUserPasswordUpdate);
                //_CtpTraderApi.OnRspQueryMaxOrderVolume += new CtpTraderApi.RspQueryMaxOrderVolume(CtpTraderApi_OnRspQueryMaxOrderVolume);
                //_CtpTraderApi.OnRtnTradingNotice += new CtpTraderApi.RtnTradingNotice(CtpTraderApi_OnRtnTradingNotice);
                //_CtpTraderApi.OnRtnInstrumentStatus += new CtpTraderApi.RtnInstrumentStatus(CtpTraderApi_OnRtnInstrumentStatus);
                //_CtpTraderApi.OnRtnBulletin += new CtpTraderApi.RtnBulletin(CtpTraderApi_OnRtnBulletin);

                _CtpTraderApi.OnRtnOrder += new CtpTraderApi.RtnOrder(CtpTraderApi_OnRtnOrder);
                _CtpTraderApi.OnRspOrderInsert += new CtpTraderApi.RspOrderInsert(CtpTraderApi_OnRspOrderInsert);
                //_CtpTraderApi.OnErrRtnOrderInsert += new CtpTraderApi.ErrRtnOrderInsert(CtpTraderApi_OnErrRtnOrderInsert);
                _CtpTraderApi.OnRspOrderAction += new CtpTraderApi.RspOrderAction(CtpTraderApi_OnRspOrderAction);
                //_CtpTraderApi.OnErrRtnOrderAction += new CtpTraderApi.ErrRtnOrderAction(CtpTraderApi_OnErrRtnOrderAction);
                _CtpTraderApi.OnRtnTrade += new CtpTraderApi.RtnTrade(CtpTraderApi_OnRtnTrade);

                _CtpTraderApi.OnRspQryOrder += new CtpTraderApi.RspQryOrder(CtpTraderApi_OnRspQryOrder);
                _CtpTraderApi.OnRspQryTrade += new CtpTraderApi.RspQryTrade(CtpTraderApi_OnRspQryTrade);
                _CtpTraderApi.OnRspQryInvestorPosition += new CtpTraderApi.RspQryInvestorPosition(CtpTraderApi_OnRspQryInvestorPosition);
                //_CtpTraderApi.OnRspQryInvestorPositionDetail += new CtpTraderApi.RspQryInvestorPositionDetail(CtpTraderApi_OnRspQryInvestorPositionDetail);
                _CtpTraderApi.OnRspQryTradingAccount += new CtpTraderApi.RspQryTradingAccount(CtpTraderApi_OnRspQryTradingAccount);
            }
        }

        private CtpMdApi ApiQuoteConnection(string userName, string passWord, string brokerId, string ip)
        {
            return new CtpMdApi(userName, passWord, brokerId, ip);
        }


        public void InitQuoteApi(string account, string password, string brokerId, string ip)
        {
            _CtpMdApi = ApiQuoteConnection(account, password, brokerId, ip);
            //Util.WriteInfo("MdApiCTP CtpDataServer: ApiQuoteConnection. Broker = " + _CtpMdApi.BrokerID + ", IP = " + _CtpMdApi.FrontAddr);
            _CtpMdApi.OnFrontConnected += new CtpMdApi.FrontConnected(CtpMdApi_OnFrontConnected);
            _CtpMdApi.OnFrontDisConnected += new CtpMdApi.FrontDisConnected(CtpMdApi_OnFrontDisconnected);
            //_CtpMdApi.OnHeartBeatWarning += new CtpMdApi.HeartBeatWarning(CtpMdApi_OnHeartBeatWarning);
            _CtpMdApi.OnRspUserLogin += new CtpMdApi.RspUserLogin(CtpMdApi_OnRspUserLogin);
            _CtpMdApi.OnRspUserLogout += new CtpMdApi.RspUserLogout(CtpMdApi_OnRspUserLogout);
            //_CtpMdApi.OnRspError += new CtpMdApi.RspError(CtpMdApi_OnRspError);

            _CtpMdApi.OnRspSubMarketData += new CtpMdApi.RspSubMarketData(CtpMdApi_OnRspSubMarketData);
            _CtpMdApi.OnRspUnSubMarketData += new CtpMdApi.RspUnSubMarketData(CtpMdApi_OnRspUnSubMarketData);
            _CtpMdApi.OnRtnDepthMarketData += new CtpMdApi.RtnDepthMarketData(CtpMdApi_OnRtnDepthMarketData);
            //_CtpMdApi.OnRspSubForQuoteRsp += new CtpMdApi.RspSubForQuoteRsp(mdApi_OnRspSubForQuoteRsp);
            //_CtpMdApi.OnRspUnSubForQuoteRsp += new CtpMdApi.RspUnSubForQuoteRsp(mdApi_OnRspUnSubForQuoteRsp);
            //_CtpMdApi.OnRtnForQuoteRsp += new CtpMdApi.RtnForQuoteRsp(mdApi_OnRtnForQuoteRsp);
        }

        void CtpTraderApi_OnFrontConnected()
        {
            Util.WriteInfo("TradeApiCTP CtpDataServer: OnFrontConnected is received.");
            int logFlag = _CtpTraderApi.ReqUserLogin();
        }

        private void CtpTraderApi_OnRspQryTradingAccount(ref CThostFtdcTradingAccountField pTradingAccount, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            //if (pRspInfo != null && pRspInfo.ErrorID == 0 && pTradingAccount != null)
            if (pRspInfo.ErrorID == 0)
            {
                TradingAccountCapitalDic[pTradingAccount.AccountID] = pTradingAccount;
            }
            if (bIsLast)
            {
                //IsQryTradingAccountReady = true;
                AutoCapitalEvent.Set();
            }
        }

        private void CtpTraderApi_OnRspUserLogin(ref CThostFtdcRspUserLoginField pRspUserLogin, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast && pRspInfo.ErrorID == 0)
            {
                DateTime shfeTime = Convert.ToDateTime(pRspUserLogin.SHFETime);
                Util.ExchangeTimeOffset = Util.GetSecFromDateTime(shfeTime) -  Util.GetSecFromDateTime(DateTime.Now);
                Util.WriteInfo(string.Format("交易所时间与本地时间的偏移值为{0}秒", Util.ExchangeTimeOffset), true);
                //IsLoginReady = true;
                AutoInitEvent.Set();
            }
            TradingTimeManager.InitTime(pRspUserLogin.TradingDay);
            OrderManager.SerMaxOrderRef(int.Parse(pRspUserLogin.MaxOrderRef));
            Util.WriteInfo("Trading Day: " + pRspUserLogin.TradingDay);
            if (IsInited && Util.StrategyParameter.Count > 0 && bIsLast && pRspInfo.ErrorID == 0) //断线重连，又登录成功
            {
                Task.Run(() =>
                {
                    CloseLock();
                    InitDataFromAPI();
                });
            }
        }

        private void CtpTraderApi_OnRspQryInstrument(ref CThostFtdcInstrumentField pInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID == 0)
            {
                Contract instrumentItem = GetContractInfoFromQuery(pInstrument);
                InstrumentFields[pInstrument.InstrumentID] = instrumentItem;
                Util.InstrumentIdToId[pInstrument.InstrumentID] = Util.StartId;
                Util.StartId += 1000;
                if (RiskManager.MaxLimitOrderVolumeDic.ContainsKey(pInstrument.InstrumentID))
                {
                    RiskManager.MaxLimitOrderVolumeDic.Add(pInstrument.InstrumentID, pInstrument.MaxLimitOrderVolume);
                    RiskManager.MaxMarketOrderVolumeDic.Add(pInstrument.InstrumentID, pInstrument.MaxMarketOrderVolume);
                }
                else
                {
                    RiskManager.MaxLimitOrderVolumeDic[pInstrument.InstrumentID] = pInstrument.MaxLimitOrderVolume;
                    RiskManager.MaxMarketOrderVolumeDic[pInstrument.InstrumentID] = pInstrument.MaxMarketOrderVolume;
                }
                CThostFtdcInstrumentMarginRateField pInstrumentMarginRate = new CThostFtdcInstrumentMarginRateField();
                pInstrumentMarginRate.LongMarginRatioByMoney = pInstrument.LongMarginRatio;
                pInstrumentMarginRate.ShortMarginRatioByMoney = pInstrument.ShortMarginRatio;
                InstrumentCategoryMarginRateDic[pInstrument.InstrumentID] = pInstrumentMarginRate;
            }
            AutoInitEvent.Set();
        }

        private Contract GetContractInfoFromQuery(CThostFtdcInstrumentField pInstrument)
        {
            Contract instItem = new Contract();
            try
            {
                instItem.BaseCode = pInstrument.UnderlyingInstrID;
                instItem.Code = pInstrument.ExchangeInstID;
                instItem.ExchangeId = pInstrument.ExchangeID;
                instItem.ExpireDate = pInstrument.ExpireDate;
                instItem.PriceTick = pInstrument.PriceTick; //(decimal)pInstrument.PriceTick;//Decimal.Parse(pInstrument.PriceTick.ToString());
                instItem.VolumeMultiple = pInstrument.VolumeMultiple;
                instItem.IsMaxMarginSingleSide = pInstrument.MaxMarginSideAlgorithm == EnumThostMaxMarginSideAlgorithmType.YES ? true : false;
                instItem.MaxLimitOrderVolume = pInstrument.MaxLimitOrderVolume;
                instItem.MaxMarketOrderVolume = pInstrument.MaxMarketOrderVolume;
                instItem.Name = pInstrument.InstrumentName.Trim();
                instItem.OpenDate = pInstrument.OpenDate;
                //instItem.OptionType = GetOptionsType(pInstrument.OptionsType);
                //instItem.ProductType = GetProductClassType(pInstrument.ProductClass);
                instItem.ProductID = pInstrument.ProductID;
                instItem.Strike = pInstrument.StrikePrice;
                instItem.LongMarginRatio = pInstrument.LongMarginRatio;
                instItem.ShortMarginRatio = pInstrument.ShortMarginRatio;
            }
            catch (Exception ex)
            {
                Util.WriteError("exception: " + ex.Message);
                Util.WriteError(ex.StackTrace);
            }
            return instItem;
        }

        private void CtpTraderApi_OnRspQryOrder(ref CThostFtdcOrderField pOrder, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            try
            {
                //if (pOrder != null)
                {
                    string orderKey = Util.GetOrderKey(pOrder);
                    OrderFields[orderKey] = pOrder;
                    EnumThostPosiDirectionType posiDirection = GetOrderPosiDirection(pOrder);
                    var directionKey = Util.GetPositionKey(pOrder.InstrumentID, posiDirection);
                    if (OrderManager.IsCancellable(pOrder))
                    {
                        if (!DicPendingOrder.ContainsKey(directionKey))
                        {
                            DicPendingOrder[directionKey] = new ConcurrentDictionary<string, CThostFtdcOrderField>();
                        }
                        DicPendingOrder[directionKey][orderKey] = pOrder;
                    }
                    if (pOrder.OrderStatus == EnumThostOrderStatusType.Canceled)
                    {
                        RiskManager.SetCancelOrderCount(pOrder.InstrumentID, 1);
                        RiskManager.SetLargeCancelOrder(pOrder.InstrumentID, pOrder.ExchangeID, pOrder.VolumeTotal - pOrder.VolumeTraded);
                    }
                    if (OrderManager != null)
                    {
                        if (string.IsNullOrEmpty(Util.GetMaxRefFromOrderRef(pOrder.OrderRef)))
                        {
                            OrderManager.SerMaxOrderRef(0);
                        }
                        else
                        {
                            OrderManager.SerMaxOrderRef(int.Parse(Util.GetMaxRefFromOrderRef(pOrder.OrderRef)));
                        }
                    }
                    RtnNotifyOrderStatus?.Invoke(pOrder, true);
                }
                if (bIsLast)
                {
                    //IsQryOrderReady = true;
                    AutoOrderEvent.Set();
                }
            }
            catch (Exception ex)
            {
                Util.WriteExceptionToLogFile(ex);
            }
        }

        void CtpTraderApi_OnRspQryTrade(ref CThostFtdcTradeField pTrade, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID != 0)
            {
                Util.WriteWarn("Error! TradeApiCTP CtpDataServer OnRspQryOrder! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + ", Code:" + pTrade.InstrumentID);
            }
            else
            {
                string tradedKey = string.Format("{0}:{1}:{2}", pTrade.BrokerOrderSeq, pTrade.OrderRef, pTrade.ExchangeID);
                TradedFields[tradedKey] = pTrade;
            }
            if (bIsLast)
            {
                //IsQryTradeReady = true;
                AutoTradeEvent.Set();
            }
        }

        private void CtpTraderApi_OnRspQryInvestorPosition(ref CThostFtdcInvestorPositionField pInvestorPosition, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            try
            {
                //if (pInvestorPosition != null)
                {
                    //Utils.WriteLine(PositionInfo(pInvestorPosition), true);
                    if (pInvestorPosition.PosiDirection != EnumThostPosiDirectionType.Net && pInvestorPosition.Position > 0)
                    {
                        var posKey = Util.GetPositionKey(pInvestorPosition.InstrumentID, pInvestorPosition.PosiDirection);
                        if (PositionFields.ContainsKey(posKey))
                        {
                            PositionField field = PositionFields[posKey];
                            CalcAvailablePosition(pInvestorPosition, field);
                            //field.AvgPrice = field.AvgPrice
                            field.ExchangeMargin += pInvestorPosition.ExchangeMargin;
                            field.OpenCost += pInvestorPosition.OpenCost;
                            field.Position += pInvestorPosition.Position;
                            field.PositionCost += pInvestorPosition.PositionCost;
                            field.TodayPosition += pInvestorPosition.TodayPosition;
                            field.UseMargin += pInvestorPosition.UseMargin;
                            field.YdPosition += pInvestorPosition.YdPosition;
                        }
                        else
                        {
                            PositionFields[posKey] = new PositionField
                            {
                                AvgPrice = pInvestorPosition.PreSettlementPrice,
                                BrokerId = pInvestorPosition.BrokerID,
                                ExchangeMargin = pInvestorPosition.ExchangeMargin,
                                HedgeFlag = pInvestorPosition.HedgeFlag,
                                InstrumentId = pInvestorPosition.InstrumentID,
                                InvestorId = pInvestorPosition.InvestorID,
                                OpenCost = pInvestorPosition.OpenCost,
                                PreSettlementPrice = pInvestorPosition.PreSettlementPrice,
                                PosiDirection = pInvestorPosition.PosiDirection,
                                Position = pInvestorPosition.Position,
                                PositionCost = pInvestorPosition.PositionCost,
                                SettlementPrice = pInvestorPosition.SettlementPrice,
                                TodayPosition = pInvestorPosition.TodayPosition,
                                TradingDay = pInvestorPosition.TradingDay,
                                UseMargin = pInvestorPosition.UseMargin,
                                YdPosition = pInvestorPosition.YdPosition
                            };
                            CalcAvailablePosition(pInvestorPosition, PositionFields[posKey]);
                        }
                    }
                }

                if (bIsLast)
                {
                    //IsQryInvestorPositionReady = true;
                    AutoPositionEvent.Set();
                }
            }
            catch (Exception ex)
            {
                Util.WriteException(ex);
            }
        }

        private void CalcAvailablePosition(CThostFtdcInvestorPositionField pInvestorPosition, PositionField field)
        {
            if (pInvestorPosition.PosiDirection == EnumThostPosiDirectionType.Long)
            {
                field.Avail += pInvestorPosition.Position - pInvestorPosition.ShortFrozen;
                if (pInvestorPosition.PositionDate == EnumThostPositionDateType.Today)
                {
                    field.AvailToday += pInvestorPosition.TodayPosition > pInvestorPosition.ShortFrozen ? pInvestorPosition.TodayPosition - pInvestorPosition.ShortFrozen : 0;
                }
                else
                {
                    field.AvailToday += pInvestorPosition.TodayPosition;
                }
            }
            else if (pInvestorPosition.PosiDirection == EnumThostPosiDirectionType.Short)
            {
                field.Avail += pInvestorPosition.Position - pInvestorPosition.LongFrozen;
                if (pInvestorPosition.PositionDate == EnumThostPositionDateType.Today)
                {
                    field.AvailToday += pInvestorPosition.TodayPosition > pInvestorPosition.LongFrozen ? pInvestorPosition.TodayPosition - pInvestorPosition.LongFrozen : 0;
                }
                else
                {
                    field.AvailToday += pInvestorPosition.TodayPosition;
                }
            }
        }

        public void ReqQryAllInstruments()
        {
            Util.WriteInfo("查询所有合约");
            while (_CtpTraderApi.QryInstrument() != 0)
            {
                Thread.Sleep(1024);
            }
        }

        public void ReqSettlementInfoConfirm()
        {
            while (_CtpTraderApi.SettlementInfoConfirm() != 0)
            {
                Thread.Sleep(1024);
            }
        }

        public void ReqQryTradingAccount()
        {
            while (_CtpTraderApi.QryTradingAccount() != 0)
            {
                Thread.Sleep(1024);
            }
        }

        public void ReqQryInvestorPosition()
        {
            while (_CtpTraderApi.QryInvestorPosition() != 0)
            {
                Thread.Sleep(1024);
            }
        }

        public void ReqQryOrder()
        {
            while (_CtpTraderApi.QryOrder() != 0)
            {
                Thread.Sleep(1024);
            }
        }

        public void ReqQryTrade()
        {
            while (_CtpTraderApi.QryTrade() != 0)
            {
                Thread.Sleep(1024);
            }
        }

        private void LoginTrader()
        {
            _CtpTraderApi.Connect();
            Util.WriteInfo("等待交易登录...");
            AutoInitEvent.WaitOne();
            Util.WriteInfo("交易登录完成");

            _CtpMdApi.Connect();
            Util.WriteInfo("等待行情初始化...");
            AutoInitEvent.WaitOne();
            Util.WriteInfo("行情初始化完成");

            ReqQryAllInstruments();
            Util.WriteInfo("等待查询合约...");
            AutoInitEvent.WaitOne();
            Util.WriteInfo("查询合约完成");
        }

        private void InitDataFromAPI()
        {
            ReqSettlementInfoConfirm();
            Util.WriteInfo("确认结算单");

            ReqQryTradingAccount();
            Util.WriteInfo("查询账号资金...");
            AutoCapitalEvent.WaitOne();
            Util.WriteInfo("查询账号资金完成");

            PositionFields.Clear();
            ReqQryInvestorPosition();
            Util.WriteInfo("等待查询投资者持仓...");
            AutoPositionEvent.WaitOne();
            Util.WriteInfo("查询投资者持仓完成");

            Task.Run(() =>
            {
                OrderFields.Clear();
                DicPendingOrder.Clear();
                ReqQryOrder();
                Util.WriteInfo("等待查询委托...");
                AutoOrderEvent.WaitOne();
                Util.WriteInfo("查询委托完成");
            });

            TradedFields.Clear();
            ReqQryTrade();
            Util.WriteInfo("等待查询成交...");
            AutoTradeEvent.WaitOne();
            Util.WriteInfo("查询成交完成");

            Thread.Sleep(3000);
        }

        public void LaunchStrategies(ICollection<string> instrumentsCollection, bool isInit)
        {
            foreach (string instruId in instrumentsCollection)
            {
                //InitMarginCommissionRate(instruId);
                Util.IsSentBuyOrder[instruId] = false;
                Util.IsSentSellOrder[instruId] = false;
                Util.IsFilledBuyOrder[instruId] = true;
                Util.IsFilledSellOrder[instruId] = true;
                Util.IsSentCancelBuy[instruId] = false;
                Util.IsSentCancelSell[instruId] = false;

                if (isInit)
                {
                    if (Util.StrategyMap.ContainsKey(instruId) && Util.StrategyMap[instruId] != null)
                    {
                        lock (RestartObject)
                        {
                            foreach (string strategyName in Util.StrategyMap[instruId].Keys)
                            {
                                Util.StrategyMap[instruId][strategyName].SetNoTradingValue();//Thread safe
                            }
                        }
                    }
                }
                else
                {
                    if (!InstrumentFields.ContainsKey(instruId))
                    {
                        Util.WriteInfo(string.Format("{0}: illegal info", instruId));
                        continue;
                    }
                    TradingTimeManager.InitTimeMap(InstrumentFields[instruId].ExchangeId, instruId);
                    if (!Util.StrategyMap.ContainsKey("code"))
                    {
                        Util.StrategyMap[instruId] = new ConcurrentDictionary<string, StrategyControl>();
                    }
                    if (Util.StrategyParameter[instruId].ContainsKey("OpenStrategy"))
                    {
                        int openValid = int.Parse(Util.StrategyParameter[instruId]["OpenStrategy"]["Value"]);
                        if (Util.StrategyNeedLaunch(instruId, openValid))
                        {
                            StrategyOpenVectorMA strategyOpen = new StrategyOpenVectorMA(instruId, this);
                            Util.StrategyMap[instruId]["OpenStrategy"] = strategyOpen;
                            Util.WriteInfo(string.Format("{0}: {1} 启动.", instruId, "OpenStrategy"));
                            _LaunchedInstrumentLst.Add(instruId);
                        }
                    }
                    if (Util.StrategyParameter[instruId].ContainsKey("RevertStrategy"))
                    {
                        int revertValid = int.Parse(Util.StrategyParameter[instruId]["RevertStrategy"]["Value"]);
                        if (Util.StrategyNeedLaunch(instruId, revertValid))
                        {
                            StrategyRevertVectorMA strategyRevert = new StrategyRevertVectorMA(instruId, this);
                            Util.StrategyMap[instruId]["RevertStrategy"] = strategyRevert;
                            Util.WriteInfo(string.Format("{0}: {1} 启动.", instruId, "RevertStrategy"));
                            _LaunchedInstrumentLst.Add(instruId);
                        }
                    }

                    if (Util.StrategyParameter[instruId].ContainsKey("PbStrategy"))
                    {
                        int revertValid = int.Parse(Util.StrategyParameter[instruId]["PbStrategy"]["Value"]);
                        if (Util.StrategyNeedLaunch(instruId, revertValid))
                        {
                            StrategyPbVectorMA strategyPb = new StrategyPbVectorMA(instruId, this);
                            Util.StrategyMap[instruId]["PbStrategy"] = strategyPb;
                            Util.WriteInfo(string.Format("{0}: {1} 启动.", instruId, "PbStrategy"));
                            _LaunchedInstrumentLst.Add(instruId);
                        }
                    }

                    if (Util.StrategyParameter[instruId].ContainsKey("Pb2Strategy"))
                    {
                        int revertValid = int.Parse(Util.StrategyParameter[instruId]["Pb2Strategy"]["Value"]);
                        if (Util.StrategyNeedLaunch(instruId, revertValid))
                        {
                            StrategyPb2VectorMA strategyPb2 = new StrategyPb2VectorMA(instruId, this);
                            Util.StrategyMap[instruId]["Pb2Strategy"] = strategyPb2;
                            Util.WriteInfo(string.Format("{0}: {1} 启动.", instruId, "Pb2Strategy"));
                            _LaunchedInstrumentLst.Add(instruId);
                        }
                    }
                    if (Util.StrategyParameter[instruId].ContainsKey("TwapAdvanced"))
                    {
                        int revertValid = int.Parse(Util.StrategyParameter[instruId]["TwapAdvanced"]["Value"]);
                        if (Util.StrategyNeedLaunch(instruId, revertValid))
                        {
                            StrategyTwapEnhanced strategyTwapAd = new StrategyTwapEnhanced(instruId, this, _AlgoTrader);
                            Util.StrategyMap[instruId]["TwapAdvanced"] = strategyTwapAd;
                            Util.WriteInfo(string.Format("{0}: {1} 启动.", instruId, "TwapAdvanced"));
                            _LaunchedInstrumentLst.Add(instruId);
                        }
                    }
                    if (Util.StrategyParameter[instruId].ContainsKey("TargetTime"))
                    {
                        StrategyTargetTime strategyTargetTime = new StrategyTargetTime(instruId, this, _AlgoTrader);
                        Util.StrategyMap[instruId]["TargetTime"] = strategyTargetTime;
                        Util.WriteInfo(string.Format("{0}: {1} 启动.", instruId, "TargetTime"));
                        _LaunchedInstrumentLst.Add(instruId);
                    }
                    if (Util.StrategyParameter[instruId].ContainsKey("TargetPrice"))
                    {
                        StrategyTargetPrice strategyTargetPrice = new StrategyTargetPrice(instruId, this, _AlgoTrader);
                        Util.StrategyMap[instruId]["TargetPrice"] = strategyTargetPrice;
                        Util.WriteInfo(string.Format("{0}: {1} 启动.", instruId, "TargetPrice"));
                        _LaunchedInstrumentLst.Add(instruId);
                    }
                }
                Util.IsQuoteReloaded[instruId] = false;
                Util.IsQuoteReloading[instruId] = false;
            }
            InitSubscribedMarketData(_LaunchedInstrumentLst);
            if (!isInit)
            {
                foreach (string code in Util.StrategyMap.Keys)
                {
                    //OrderManager.TestOrderManager(code);
                    break;
                }
            }
        }

        //private StrategyControl GetStrategyFromMap(string instrumentId, string strategyName)
        //{
        //    if (Constant.StrategyMap.ContainsKey(instrumentId) && (Constant.StrategyMap[instrumentId].ContainsKey("TwapAdvanced")))
        //    {
        //        return Constant.StrategyMap[instrumentId]["TwapAdvanced"];
        //    }
        //    return null;
        //}

        public bool IsShfeRule(string instrument)
        {
            if (InstrumentFields.ContainsKey(instrument)
                && InstrumentFields[instrument].ExchangeId == "SHCE")
            {
                return true;
            }
            return false;
        }

        public bool IsCffexRule(string instrument)
        {
            if (InstrumentFields.ContainsKey(instrument)
                && InstrumentFields[instrument].ExchangeId == "CFFEX")
            {
                return true;
            }
            return false;
        }

        public void RenewStrategyStatus()
        {
            // Remove old
            foreach (string instruId in Util.StrategyMap.Keys)
            {
                foreach (string strategyName in Util.StrategyMap[instruId].Keys)
                {
                    // Close Strategy
                    if (!Util.StrategyParameter.ContainsKey(instruId) || !Util.StrategyParameter[instruId].ContainsKey(strategyName))
                    {
                        StrategyControl strategy = Util.StrategyMap[instruId][strategyName];
                        strategy.StopStrategy();
                        Util.WriteInfo(string.Format("{0}: {1} is stopped.", instruId, strategyName), Program.InPrintMode);
                    }
                }
            }

            //Check New
            List<string> newQuoteLst = new List<string>();
            foreach (string instruId in Util.StrategyParameter.Keys)
            {
                if (!InstrumentFields.ContainsKey(instruId))
                {
                    Util.WriteInfo(string.Format("{0}: illegal info", instruId));
                    continue;
                }

                if (Util.StrategyMap.ContainsKey(instruId))
                {
                    if (Util.StrategyParameter[instruId].ContainsKey("TargetTime") && int.Parse(Util.StrategyParameter[instruId]["TargetTime"]["Value"]) == 1)
                    {
                        if (Util.StrategyMap[instruId].ContainsKey("TargetTime"))
                        {
                            StrategyTargetTime strategyTargetTime = Util.StrategyMap[instruId]["TargetTime"] as StrategyTargetTime;
                            strategyTargetTime.InitParameters();
                            Util.WriteInfo(string.Format("{0}: {1} is renewed.", instruId, "TargetTime"));
                        }
                        else
                        {
                            StrategyTargetTime strategyTargetTime = new StrategyTargetTime(instruId, this, _AlgoTrader);
                            Util.StrategyMap[instruId]["TargetTime"] = strategyTargetTime;
                            Util.WriteInfo(string.Format("{0}: {1} 启动.", instruId, "TargetTime"));
                            newQuoteLst.Add(instruId);
                        }
                    }
                    else if (Util.StrategyMap[instruId].ContainsKey("TargetTime")) //stop
                    {
                        StrategyTargetTime strategyTargetTime = Util.StrategyMap[instruId]["TargetTime"] as StrategyTargetTime;
                        strategyTargetTime.StopStrategy();
                        //StrategyControl removeStrategy;
                        //Constant.StrategyMap[instruId].TryRemove("TargetTime", out removeStrategy);
                        Util.WriteInfo(string.Format("{0}: {1} is stopped.", instruId, "TargetTime"));
                    }

                    if (Util.StrategyParameter[instruId].ContainsKey("TargetPrice") && int.Parse(Util.StrategyParameter[instruId]["TargetPrice"]["Value"]) == 1)
                    {
                        if (Util.StrategyMap[instruId].ContainsKey("TargetPrice"))
                        {
                            StrategyTargetPrice strategyTargetPrice = Util.StrategyMap[instruId]["TargetPrice"] as StrategyTargetPrice;
                            strategyTargetPrice.InitParameters();
                            Util.WriteInfo(string.Format("{0}: {1} is renewed.", instruId, "TargetPrice"));
                        }
                        else
                        {
                            StrategyTargetPrice strategyTargetPrice = new StrategyTargetPrice(instruId, this, _AlgoTrader);
                            Util.StrategyMap[instruId]["TargetTime"] = strategyTargetPrice;
                            Util.WriteInfo(string.Format("{0}: {1} 启动.", instruId, "TargetPrice"));
                            newQuoteLst.Add(instruId);
                        }
                    }
                    else if (Util.StrategyMap[instruId].ContainsKey("TargetPrice")) //stop
                    {
                        StrategyTargetPrice strategyTargetPrice = Util.StrategyMap[instruId]["TargetPrice"] as StrategyTargetPrice;
                        strategyTargetPrice.StopStrategy();
                        //StrategyControl removeStrategy;
                        //Constant.StrategyMap[instruId].TryRemove("TargetPrice", out removeStrategy);
                        Util.WriteInfo(string.Format("{0}: {1} is stopped.", instruId, "TargetPrice"));
                    }
                }
                else
                {
                    Util.StrategyMap[instruId] = new ConcurrentDictionary<string, StrategyControl>();
                    TradingTimeManager.InitTimeMap(InstrumentFields[instruId].ExchangeId, instruId);
                    if (Util.StrategyParameter[instruId].ContainsKey("TargetTime") && int.Parse(Util.StrategyParameter[instruId]["TargetTime"]["Value"]) == 1)
                    {
                        StrategyTargetTime strategyTargetTime = new StrategyTargetTime(instruId, this, _AlgoTrader);
                        Util.StrategyMap[instruId]["TargetTime"] = strategyTargetTime;
                        Util.WriteInfo(string.Format("{0}: {1} 启动.", instruId, "TargetTime"));
                        newQuoteLst.Add(instruId);
                    }
                    if (Util.StrategyParameter[instruId].ContainsKey("TargetPrice") && int.Parse(Util.StrategyParameter[instruId]["TargetPrice"]["Value"]) == 1)
                    {
                        StrategyTargetPrice strategyTargetPrice = new StrategyTargetPrice(instruId, this, _AlgoTrader);
                        Util.StrategyMap[instruId]["TargetTime"] = strategyTargetPrice;
                        Util.WriteInfo(string.Format("{0}: {1} 启动.", instruId, "TargetPrice"));
                        newQuoteLst.Add(instruId);
                    }
                }
            }
            if (newQuoteLst.Count > 0)
            {
                _CtpMdApi.SubscribeMarketData(newQuoteLst.ToArray());
            }
        }

        public void ParseStrategyXmlParameter(string path)
        {
            XmlDocument doc = new XmlDocument();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            //xmlFilePath:xml文件路径
            XmlReader reader = XmlReader.Create(path, settings);

            doc.Load(reader);
            XmlNode node = doc.SelectSingleNode("Strategy");
            if (node == null) return;

            Util.StrategyParameter.Clear();
            foreach (XmlNode item in node.ChildNodes)
            {
                if (item.Name == "Code")
                {
                    string code = item.Attributes[0].Value.Trim();
                    string instruKey;
                    if (InstrumentFields.ContainsKey(code.ToUpper()))
                    {
                        instruKey = code.ToUpper();
                    }
                    else if (InstrumentFields.ContainsKey(code.ToLower()))
                    {
                        instruKey = code.ToLower();
                    }
                    else
                    {
                        Util.WriteInfo(string.Format("{0}: illegal info", code));
                        continue;
                    }

                    if (!string.IsNullOrEmpty(instruKey))
                    {
                        Util.StrategyParameter[instruKey] = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();
                        foreach (XmlNode strategyItem in item.ChildNodes)
                        {
                            string valid = strategyItem.Attributes[0].Value;
                            if (!string.IsNullOrEmpty(strategyItem.Name.ToString()) && valid != null && int.Parse(valid) != 0)
                            {
                                Util.StrategyParameter[instruKey][strategyItem.Name] = new ConcurrentDictionary<string, string>();
                                foreach (XmlNode param in strategyItem.ChildNodes)
                                {
                                    string value = param.Attributes[0].Value;
                                    if (param.Name != null && param.Name != "" && value != null)
                                    {
                                        Util.StrategyParameter[instruKey][strategyItem.Name][param.Name] = value;
                                        Util.WriteInfo(string.Format("{0} [{1}][{2}] = {3}", instruKey, strategyItem.Name, param.Name, value));
                                    }
                                }
                                Util.StrategyParameter[instruKey][strategyItem.Name]["Value"] = valid;
                                Util.WriteInfo(string.Format("{0} [{1}][{2}] = {3}\n", instruKey, strategyItem.Name, "Value", valid));
                            }
                            //else if (Constant.StrategyParameter.ContainsKey(code) &&
                            //    Constant.StrategyParameter[code].ContainsKey(strategyItem.Name))
                            //{
                            //    ConcurrentDictionary<string, string> temp;
                            //    Constant.StrategyParameter[code].TryRemove(strategyItem.Name, out temp);
                            //}
                        }
                    }
                }
            }
            reader.Close();
        }

        public void InitSubscribedMarketData(List<string> codeLst)
        {
            if (codeLst.Count > 0)
            {
                _CtpMdApi.SubscribeMarketData(codeLst.ToArray());
            }
            else
            {
                Util.WriteInfo("No instrument set for quote subscription!");
            }
        }

        private void QuoteReload(string instrumentid)
        {
            // Check & load history data
            string connection = ConfigurationManager.ConnectionStrings["TickConnection"].ConnectionString;
            string tableName = string.Format("{0}", instrumentid);
            string quoteCmd = string.Empty;
            if (TradingTimeManager.IsBeforeNightClose(DateTime.Now, instrumentid))
            {
                quoteCmd = @"select [AssetCode],[ExchangeName],[TradingDay],[CalendarDate],[UpdateTime],[LastPrice],
                    [BidPrice1],[BidVolume1],[AskPrice1],[AskVolume1],[BidPrice2],[BidVolume2],[AskPrice2],[AskVolume2],[BidPrice3],[BidVolume3],[AskPrice3],[AskVolume3],
                    [BidPrice4],[BidVolume4],[AskPrice4],[AskVolume4],[BidPrice5],[BidVolume5],[AskPrice5],[AskVolume5],[OpenInterest],[Volume],[Amount],
                    [UpStopPrice],[DownStopPrice],[OpenPrice],[HighestPrice],[LowestPrice],[ClosePrice],[SettlementPrice],
                    [PreClosePrice],[PreSettlementPrice],[PreOpenInterest] from tb_tick_t_{0} where [TradingDay] = '{1}' order by Volume, UpdateTime ";
                quoteCmd = string.Format(quoteCmd, tableName, TradingTimeManager.TradingDay.ToString("yyyy-MM-dd"));
            }
            else
            {
                quoteCmd = @"select [AssetCode],[ExchangeName],[TradingDay],[CalendarDate],[UpdateTime],[LastPrice],
                    [BidPrice1],[BidVolume1],[AskPrice1],[AskVolume1],[BidPrice2],[BidVolume2],[AskPrice2],[AskVolume2],[BidPrice3],[BidVolume3],[AskPrice3],[AskVolume3],
                    [BidPrice4],[BidVolume4],[AskPrice4],[AskVolume4],[BidPrice5],[BidVolume5],[AskPrice5],[AskVolume5],[OpenInterest],[Volume],[Amount],
                    [UpStopPrice],[DownStopPrice],[OpenPrice],[HighestPrice],[LowestPrice],[ClosePrice],[SettlementPrice],
                    [PreClosePrice],[PreSettlementPrice],[PreOpenInterest] from tb_tick_t_{0} where [TradingDay] = '{1}' and UpdateTime >= '{2}' and UpdateTime < '{3}' order by Volume, UpdateTime ";
                quoteCmd = string.Format(quoteCmd, tableName, TradingTimeManager.TradingDay.ToString("yyyy-MM-dd")
                    , TradingTimeManager.InstrumentTradingTimeDic[instrumentid].DayOpen.ToString("HH:mm:ss"), TradingTimeManager.InstrumentTradingTimeDic[instrumentid].DayClose.ToString("HH:mm:ss"));
            }
            using (SqlConnection conn = new SqlConnection(connection))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    DataTable quoteTable = new DataTable();
                    adapter.SelectCommand = new SqlCommand(quoteCmd, conn);
                    adapter.SelectCommand.CommandTimeout = 60;
                    Util.WriteInfo(quoteCmd, Program.InPrintMode);
                    adapter.Fill(quoteTable);
                    Thread.Sleep(1000);
                    foreach (DataRow row in quoteTable.Rows)
                    {
                        CThostFtdcDepthMarketDataField pDepthMarketData = new CThostFtdcDepthMarketDataField();
                        pDepthMarketData.InstrumentID = Convert.ToString(row["AssetCode"]);
                        pDepthMarketData.ExchangeID = Convert.ToString(row["ExchangeName"]);
                        pDepthMarketData.TradingDay = Convert.ToString(row["TradingDay"]);
                        pDepthMarketData.UpdateTime = Convert.ToString(row["UpdateTime"]);
                        pDepthMarketData.LastPrice = Convert.ToDouble(row["LastPrice"]);
                        pDepthMarketData.BidPrice1 = Convert.ToDouble(row["BidPrice1"]);
                        pDepthMarketData.BidPrice2 = Convert.ToDouble(row["BidPrice2"]);
                        pDepthMarketData.BidPrice3 = Convert.ToDouble(row["BidPrice3"]);
                        pDepthMarketData.BidPrice4 = Convert.ToDouble(row["BidPrice4"]);
                        pDepthMarketData.BidPrice5 = Convert.ToDouble(row["BidPrice5"]);
                        pDepthMarketData.BidVolume1 = Convert.ToInt32(row["BidVolume1"]);
                        pDepthMarketData.BidVolume2 = Convert.ToInt32(row["BidVolume2"]);
                        pDepthMarketData.BidVolume3 = Convert.ToInt32(row["BidVolume3"]);
                        pDepthMarketData.BidVolume4 = Convert.ToInt32(row["BidVolume4"]);
                        pDepthMarketData.BidVolume5 = Convert.ToInt32(row["BidVolume5"]);
                        pDepthMarketData.AskPrice1 = Convert.ToDouble(row["AskPrice1"]);
                        pDepthMarketData.AskPrice2 = Convert.ToDouble(row["AskPrice2"]);
                        pDepthMarketData.AskPrice3 = Convert.ToDouble(row["AskPrice3"]);
                        pDepthMarketData.AskPrice4 = Convert.ToDouble(row["AskPrice4"]);
                        pDepthMarketData.AskPrice5 = Convert.ToDouble(row["AskPrice5"]);
                        pDepthMarketData.AskVolume1 = Convert.ToInt32(row["AskVolume1"]);
                        pDepthMarketData.AskVolume2 = Convert.ToInt32(row["AskVolume2"]);
                        pDepthMarketData.AskVolume3 = Convert.ToInt32(row["AskVolume3"]);
                        pDepthMarketData.AskVolume4 = Convert.ToInt32(row["AskVolume4"]);
                        pDepthMarketData.AskVolume5 = Convert.ToInt32(row["AskVolume5"]);
                        pDepthMarketData.OpenInterest = Convert.ToDouble(row["OpenInterest"]);
                        pDepthMarketData.Volume = Convert.ToInt32(row["Volume"]);
                        pDepthMarketData.Turnover = Convert.ToDouble(row["Amount"]);
                        pDepthMarketData.UpperLimitPrice = Convert.ToDouble(row["UpStopPrice"]);
                        pDepthMarketData.LowerLimitPrice = Convert.ToDouble(row["DownStopPrice"]);
                        pDepthMarketData.HighestPrice = Convert.ToDouble(row["HighestPrice"]);
                        pDepthMarketData.LowestPrice = Convert.ToDouble(row["LowestPrice"]);
                        pDepthMarketData.OpenPrice = Convert.ToDouble(row["OpenPrice"]);
                        pDepthMarketData.PreClosePrice = Convert.ToDouble(row["PreClosePrice"]);

                        Util.InstrumentMarketDataRecord[instrumentid].Add(pDepthMarketData);
                    }
                }
            }
        }

        private void InitMarginCommissionRate(string instrumentId)
        {
            //string species = Utils.GetInstrumentCategory(instrumentId);
            ReqQryInstrumentMarginRate(instrumentId);
            Util.WriteInfo("等待查询保证金率...");
            AutoMarginEvent.WaitOne();
            Util.WriteInfo("查询保证金率完成");

            Task.Run(() =>
            {
                ReqQryInstrumentCommissionRate(instrumentId);
                Util.WriteInfo("等待查询手续费率成交...");
                AutoCommissionEvent.WaitOne();
                Util.WriteInfo("查询手续费率完成");
            });
        }

        public void ReqQryInstrumentCommissionRate(string instrumentId)
        {
            while (_CtpTraderApi.QryInstrumentCommissionRate(instrumentId) != 0)
            {
                Thread.Sleep(1024);
            }
        }

        public void ReqQryInstrumentMarginRate(string instrumentId)
        {
            while (_CtpTraderApi.QryInstrumentMarginRate(instrumentId) != 0)
            {
                Thread.Sleep(1024);
            }
        }

        private void CloseLock()
        {
            foreach (string code in Util.StrategyParameter.Keys)
            {
                if (Util.IsSentBuyOrder.ContainsKey(code))
                    Util.IsSentBuyOrder[code] = true;

                if (Util.IsSentSellOrder.ContainsKey(code))
                    Util.IsSentSellOrder[code] = true;

                if (Util.IsFilledBuyOrder.ContainsKey(code))
                    Util.IsFilledBuyOrder[code] = false;

                if (Util.IsFilledSellOrder.ContainsKey(code))
                    Util.IsFilledSellOrder[code] = false;

                if (Util.IsSentCancelBuy.ContainsKey(code))
                    Util.IsSentCancelBuy[code] = true;

                if (Util.IsSentCancelSell.ContainsKey(code))
                    Util.IsSentCancelSell[code] = true;
            }
        }

        private void CtpTraderApi_OnRtnOrder(ref CThostFtdcOrderField pOrder)
        {
            try
            {
                EnumThostPosiDirectionType direction = GetOrderPosiDirection(pOrder);
                var directionKey = Util.GetPositionKey(pOrder.InstrumentID, direction);
                string orderKey = Util.GetOrderKey(pOrder);
                OrderFields[orderKey] = pOrder;
                if (OrderManager.IsCancellable(pOrder))
                {
                    if (!DicPendingOrder.ContainsKey(directionKey))
                    {
                        DicPendingOrder[directionKey] = new ConcurrentDictionary<string, CThostFtdcOrderField>();
                    }
                    DicPendingOrder[directionKey][orderKey] = pOrder;
                }
                else
                {
                    if (DicPendingOrder.ContainsKey(directionKey) && DicPendingOrder[directionKey].ContainsKey(orderKey))
                    {
                        CThostFtdcOrderField tmp;
                        DicPendingOrder[directionKey].TryRemove(orderKey, out tmp);
                    }
                }

                if (OrderManager.IsCancellable(pOrder) && pOrder.VolumeTraded == 0 || pOrder.OrderStatus == EnumThostOrderStatusType.Canceled)//有成交的排除
                {
                    UpdateAvailPosition(pOrder.InvestorID, pOrder.Direction, directionKey);
                }
                if (pOrder.OrderStatus == EnumThostOrderStatusType.Canceled)
                {
                    RiskManager.SetCancelOrderCount(pOrder.InstrumentID, 1);
                    RiskManager.SetLargeCancelOrder(pOrder.InstrumentID, pOrder.ExchangeID, pOrder.VolumeTotal - pOrder.VolumeTraded);
                    //Trigger Strategy
                    //StrategyControl strategy = Constant.StrategyMap[pOrder.InstrumentID]["TwapAdvanced"];
                    //if (strategy is StrategyTwapEnhanced)
                    //{
                    //    (strategy as StrategyTwapEnhanced).TriggerMarkupOrder(pOrder.OrderRef);
                    //}    
                }

                if (pOrder.Direction == EnumThostDirectionType.Buy)
                {
                    Util.IsSentBuyOrder[pOrder.InstrumentID] = false;
                }
                else if (pOrder.Direction == EnumThostDirectionType.Sell)
                {
                    Util.IsSentSellOrder[pOrder.InstrumentID] = false;
                }
                RtnNotifyOrderStatus?.Invoke(pOrder, false);

                if (Util.TradingReportDict.ContainsKey(pOrder.OrderRef))
                {
                    TradingReport report = Util.TradingReportDict[pOrder.OrderRef];
                    report.InvestorID = pOrder.InvestorID;
                    report.BrokerID = pOrder.BrokerID;
                    report.Watch.Stop();
                    report.InsertCostTime += report.Watch.ElapsedTicks * Util.MicrosecPerTick;
                }
                if (pOrder.OrderStatus == EnumThostOrderStatusType.Canceled || (int)pOrder.OrderSubmitStatus >= 52)
                {
                    if (pOrder.Direction == EnumThostDirectionType.Buy)
                    {
                        Util.IsFilledBuyOrder[pOrder.InstrumentID] = true;
                        Util.WriteInfo(string.Format("IsFilledBuyOrder Open: {0}", pOrder.InstrumentID), Program.InPrintMode);

                        Util.IsSentCancelBuy[pOrder.InstrumentID] = false;
                        Util.WriteInfo(string.Format("IsSentCancelBuy Open: {0}", pOrder.InstrumentID), Program.InPrintMode);
                    }
                    else if (pOrder.Direction == EnumThostDirectionType.Sell)
                    {
                        Util.IsFilledSellOrder[pOrder.InstrumentID] = true;
                        Util.WriteInfo(string.Format("IsFilledSellOrder Open: {0}", pOrder.InstrumentID), Program.InPrintMode);

                        Util.IsSentCancelSell[pOrder.InstrumentID] = false;
                        Util.WriteInfo(string.Format("IsSentCancelSell Open: {0}", pOrder.InstrumentID), Program.InPrintMode);
                    }

                    if (Util.TradingReportDict.ContainsKey(pOrder.OrderRef))
                    {
                        TradingReport report = Util.TradingReportDict[pOrder.OrderRef];
                        report.CancelVolume = pOrder.VolumeTotal - pOrder.VolumeTraded;
                        report.UpdateTime = pOrder.UpdateTime;
                        //report.InvestorID = pOrder.InvestorID;
                        //report.BrokerID = pOrder.BrokerID;
                        //report.Watch.Stop();
                        //report.InsertCostTime += report.Watch.ElapsedTicks * Constant.MicrosecPerTick;
                        if (report.CommitVolume == report.TradedVolume + report.CancelVolume)
                        {
                            //InsertReport(report);
                        }
                    }
                }
                var temp =
                        string.Format(
                            "报单回报: [合约]:{0},[买卖]:{1},[交易所]:{2},[开平]:{3},[价格]:{4},[报单状态]:{5},[报单提交状态]:{6},[报单数量]:{7},[成交数量]:{8},[剩余数量]:{9},[报单引用]:{10},[插入时间]:{11},[插入日期]:{12},[系统号]:{13},[前台编号]:{14},[对话编号]:{15},[消息]:{16}",
                            pOrder.InstrumentID, pOrder.Direction, pOrder.ExchangeID, pOrder.CombOffsetFlag_0,
                            pOrder.LimitPrice, pOrder.OrderStatus, pOrder.OrderSubmitStatus, pOrder.VolumeTotalOriginal,
                            pOrder.VolumeTraded, pOrder.VolumeTotal, pOrder.OrderRef, pOrder.InsertTime, pOrder.InsertDate,
                            pOrder.OrderSysID, pOrder.FrontID, pOrder.SessionID, pOrder.StatusMsg);
                Util.WriteInfo(temp, true);
            }
            catch (Exception ex)
            {
                Util.WriteExceptionToLogFile(ex);
            }
        }
        private void CtpTraderApi_OnRtnTrade(ref CThostFtdcTradeField pTrade)
        {
            try
            {
                string tradedKey = string.Format("{0}:{1}:{2}", pTrade.BrokerOrderSeq, pTrade.OrderRef, pTrade.ExchangeID);
                TradedFields[tradedKey] = pTrade;
                ProcessNewComePosInfo(pTrade);

                EnumThostPosiDirectionType direction = GetOrderPosiDirection(pTrade);
                var directionKey = Util.GetPositionKey(pTrade.InstrumentID, direction);
                UpdateAvailPosition(pTrade.InvestorID, pTrade.Direction, directionKey);
                if (pTrade.Direction == EnumThostDirectionType.Buy)
                {
                    Util.IsFilledBuyOrder[pTrade.InstrumentID] = true;
                    Util.WriteInfo(string.Format("IsFilledBuyOrder Open: {0}", pTrade.InstrumentID), Program.InPrintMode);
                }
                else if (pTrade.Direction == EnumThostDirectionType.Sell)
                {
                    Util.IsFilledSellOrder[pTrade.InstrumentID] = true;
                    Util.WriteInfo(string.Format("IsFilledSellOrder Open: {0}", pTrade.InstrumentID), Program.InPrintMode);
                }

                if (Util.IsClientTagRef(pTrade.OrderRef, string.Empty))
                {
                    string refIndex = Util.GetRefIndexFromOrderRef(pTrade.OrderRef);
                    if (Util.MarkupOrderReportDict.ContainsKey(pTrade.InstrumentID)
                        && Util.MarkupOrderReportDict[pTrade.InstrumentID].ContainsKey(refIndex))
                    {
                        MarkupOrderStatistics markUp = Util.MarkupOrderReportDict[pTrade.InstrumentID][refIndex];
                        markUp.TradedVolume += pTrade.Volume;
                        markUp.LastTradedPrice = (pTrade.Price * pTrade.Volume) / (pTrade.Volume); ;
                        markUp.TradedUnitCost += (Math.Abs(markUp.LastTradedPrice - markUp.LastCancelledPrice) - markUp.PriceTick);// * markUp.TradedVolume;                    
                    }
                }
                RtnNotifyFilled?.Invoke(pTrade);

                #region TradingReport
                if (Util.TradingReportDict.ContainsKey(pTrade.OrderRef))
                {
                    TradingReport report = Util.TradingReportDict[pTrade.OrderRef];
                    report.AvgTradedPrice = (report.AvgTradedPrice * report.TradedVolume + pTrade.Price * pTrade.Volume) / (report.TradedVolume + pTrade.Volume);
                    report.TradedVolume += pTrade.Volume;
                    report.UpdateTime = pTrade.TradeTime;
                    //report.InvestorID = pTrade.InvestorID;
                    //report.BrokerID = pTrade.BrokerID;
                    //report.Watch.Stop();
                    //report.InsertCostTime += report.Watch.ElapsedTicks * Constant.MicrosecPerTick;
                    if (report.BuySell == EnumThostDirectionType.Buy)
                    {
                        if (report.OppositePrice == 0.0)
                        {
                            Util.WriteWarn(string.Format("Order opposite ask price missing: {0}", report.OrderRef));
                            if (DepthMarketDataFields.ContainsKey(pTrade.InstrumentID))
                            {
                                report.OppositePrice = DepthMarketDataFields[pTrade.InstrumentID].AskPrice1;
                            }
                        }
                        report.Slippage = report.OppositePrice - pTrade.Price;
                    }
                    else if (report.BuySell == EnumThostDirectionType.Sell)
                    {
                        if (report.OppositePrice == 0.0)
                        {
                            Util.WriteWarn(string.Format("Order opposite bid price missing: {0}", report.OrderRef));
                            if (DepthMarketDataFields.ContainsKey(pTrade.InstrumentID))
                            {
                                report.OppositePrice = DepthMarketDataFields[pTrade.InstrumentID].BidPrice1;
                            }
                        }
                        report.Slippage = pTrade.Price - report.OppositePrice;
                    }

                    if (pTrade.OffsetFlag == EnumThostOffsetFlagType.Open)
                    {
                        if (Util.OpenPositionTrade.ContainsKey(directionKey))
                        {
                            CThostFtdcTradeField trade = Util.OpenPositionTrade[directionKey];
                            trade.Price = (trade.Price * trade.Volume + pTrade.Price * pTrade.Volume) / (trade.Volume + pTrade.Volume);
                            trade.Volume += pTrade.Volume;
                        }
                        else
                        {
                            CThostFtdcTradeField tradeField = new CThostFtdcTradeField();
                            tradeField.InstrumentID = pTrade.InstrumentID;
                            tradeField.Direction = pTrade.Direction;
                            tradeField.Price = pTrade.Price;
                            tradeField.Volume = pTrade.Volume;
                            Util.OpenPositionTrade[directionKey] = tradeField;
                        }
                    }
                    else if (Util.OpenPositionTrade.ContainsKey(directionKey)) // Close direction
                    {
                        if (pTrade.Direction == EnumThostDirectionType.Buy) //find short positions
                        {
                            report.CloseProfit = (Util.OpenPositionTrade[directionKey].Price - pTrade.Price) * pTrade.Volume;
                            int shortKey = Util.GetPositionKey(pTrade.InstrumentID, EnumThostPosiDirectionType.Short);
                            int shortPosition = 0;
                            if (PositionFields.ContainsKey(shortKey))
                            {
                                shortPosition = PositionFields[shortKey].Position;
                            }
                            if (shortPosition == 0)
                            {
                                CThostFtdcTradeField tempTrade;
                                Util.OpenPositionTrade.TryRemove(directionKey, out tempTrade);
                            }
                        }
                        else if (pTrade.Direction == EnumThostDirectionType.Sell) //find long positions
                        {
                            report.CloseProfit = (pTrade.Price - Util.OpenPositionTrade[directionKey].Price) * pTrade.Volume;
                            int longKey = Util.GetPositionKey(pTrade.InstrumentID, EnumThostPosiDirectionType.Long);
                            int longPosition = 0;
                            if (PositionFields.ContainsKey(longKey))
                            {
                                longPosition = PositionFields[longKey].Position;
                            }
                            if (longPosition == 0)
                            {
                                CThostFtdcTradeField tempTrade;
                                Util.OpenPositionTrade.TryRemove(directionKey, out tempTrade);
                            }
                        }
                    }
                    if (report.CommitVolume == report.TradedVolume + report.CancelVolume)
                    {
                        //InsertReport(report);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Util.WriteExceptionToLogFile(ex);
            }
        }

        private void ProcessNewComePosInfo(CThostFtdcTradeField pTrade)
        {
            if (pTrade.Direction == EnumThostDirectionType.Buy)
            {
                if (pTrade.OffsetFlag == EnumThostOffsetFlagType.Open)
                {
                    EnumThostPosiDirectionType direction = EnumThostPosiDirectionType.Long;
                    var posKey = Util.GetPositionKey(pTrade.InstrumentID, direction);

                    if (PositionFields.ContainsKey(posKey))
                    {
                        PositionField posField = PositionFields[posKey];
                        posField.AvgPrice = (posField.AvgPrice * posField.Position + pTrade.Price * pTrade.Volume) / (posField.Position + pTrade.Volume);
                        posField.Position += pTrade.Volume;
                        posField.TodayPosition += pTrade.Volume;
                    }
                    else
                    {
                        PositionFields[posKey] = new PositionField
                        {
                            AvgPrice = pTrade.Price,
                            BrokerId = _CtpTraderApi.BrokerID,
                            HedgeFlag = EnumThostHedgeFlagType.Speculation,
                            InstrumentId = pTrade.InstrumentID,
                            InvestorId = InvestorID,
                            PosiDirection = direction,
                            Position = pTrade.Volume,
                            TodayPosition = pTrade.Volume,
                            TradingDay = pTrade.TradingDay,
                            YdPosition = 0
                            //ExchangeMargin = pInvestorPosition.ExchangeMargin,
                            //OpenCost = pInvestorPosition.OpenCost,
                            //PreSettlementPrice = pInvestorPosition.PreSettlementPrice,
                            //PositionCost = pInvestorPosition.PositionCost,
                            //SettlementPrice = pInvestorPosition.SettlementPrice,
                            //UseMargin = pInvestorPosition.UseMargin,
                        };
                    }
                }
                else //  平
                {
                    EnumThostPosiDirectionType direction = EnumThostPosiDirectionType.Short;
                    if (IsShfeRule(pTrade.InstrumentID)) //上期所合约今仓昨仓分开
                    {
                        if (pTrade.OffsetFlag == EnumThostOffsetFlagType.CloseToday) //平今
                        {
                            ReducePosition(pTrade, direction, EnumThostPositionDateType.Today);
                        }
                        else //平昨
                        {
                            ReducePosition(pTrade, direction, EnumThostPositionDateType.History);
                        }
                    }
                    else if (IsCffexRule(pTrade.InstrumentID))
                    {
                        //中金所优先平今
                        ReducePosition(pTrade, direction, EnumThostPositionDateType.Today);
                    }
                    else
                    {
                        //大连平仓顺序按开仓时间先开先平
                        //郑州平仓顺序细分为：平历史投机持仓，平今投机持仓，平历史套利持仓，平今套利持仓
                        ReducePosition(pTrade, direction, EnumThostPositionDateType.History);
                    }

                }
            }
            else if (pTrade.Direction == EnumThostDirectionType.Sell)
            {
                if (pTrade.OffsetFlag == EnumThostOffsetFlagType.Open)
                {
                    EnumThostPosiDirectionType direction = EnumThostPosiDirectionType.Short;
                    var posKey = Util.GetPositionKey(pTrade.InstrumentID, direction);

                    if (PositionFields.ContainsKey(posKey))
                    {
                        PositionField posField = PositionFields[posKey];
                        posField.AvgPrice = (posField.AvgPrice * posField.Position + pTrade.Price * pTrade.Volume) / (posField.Position + pTrade.Volume);
                        posField.Position += pTrade.Volume;
                        posField.TodayPosition += pTrade.Volume;
                    }
                    else
                    {
                        PositionFields[posKey] = new PositionField
                        {
                            AvgPrice = pTrade.Price,
                            BrokerId = _CtpTraderApi.BrokerID,
                            HedgeFlag = EnumThostHedgeFlagType.Speculation,
                            InstrumentId = pTrade.InstrumentID,
                            InvestorId = _CtpTraderApi.InvestorID,
                            PosiDirection = direction,
                            Position = pTrade.Volume,
                            TodayPosition = pTrade.Volume,
                            TradingDay = pTrade.TradingDay,
                            YdPosition = 0
                            //ExchangeMargin = pInvestorPosition.ExchangeMargin,
                            //OpenCost = pInvestorPosition.OpenCost,
                            //PreSettlementPrice = pInvestorPosition.PreSettlementPrice,
                            //PositionCost = pInvestorPosition.PositionCost,
                            //SettlementPrice = pInvestorPosition.SettlementPrice,
                            //UseMargin = pInvestorPosition.UseMargin,
                        };
                    }
                }
                else
                {
                    EnumThostPosiDirectionType direction = EnumThostPosiDirectionType.Long;
                    if (IsShfeRule(pTrade.InstrumentID)) //上期所合约今仓昨仓分开
                    {
                        if (pTrade.OffsetFlag == EnumThostOffsetFlagType.CloseToday) //平今
                        {
                            ReducePosition(pTrade, direction, EnumThostPositionDateType.Today);
                        }
                        else //平昨
                        {
                            ReducePosition(pTrade, direction, EnumThostPositionDateType.History);
                        }
                    }
                    else if (IsCffexRule(pTrade.InstrumentID))
                    {
                        //中金所优先平今
                        ReducePosition(pTrade, direction, EnumThostPositionDateType.Today);
                    }
                    else
                    {
                        //大连平仓顺序按开仓时间先开先平
                        //郑州平仓顺序细分为：平历史投机持仓，平今投机持仓，平历史套利持仓，平今套利持仓
                        ReducePosition(pTrade, direction, EnumThostPositionDateType.History);
                    }
                }
            }
        }

        private void ReducePosition(CThostFtdcTradeField pTrade, EnumThostPosiDirectionType posDirection,
            EnumThostPositionDateType preferCloseDateType)
        {
            try
            {
                var key = Util.GetPositionKey(pTrade.InstrumentID, posDirection);

                if (PositionFields.ContainsKey(key))
                {
                    var positionToReduce = PositionFields[key];
                    positionToReduce.Position -= pTrade.Volume;

                    if (preferCloseDateType == EnumThostPositionDateType.Today)
                    {
                        if (positionToReduce.TodayPosition >= pTrade.Volume)
                        {
                            positionToReduce.TodayPosition -= pTrade.Volume;
                            //if (posDirection == EnumPosiDirectionType.Long)
                            //{
                            //    positionToReduce.AvailTodayLong -= pTrade.Volume;
                            //    positionToReduce.AvailLong -= pTrade.Volume;
                            //}
                            //else if (posDirection == EnumPosiDirectionType.Short)
                            //{
                            //    positionToReduce.AvailTodayShort -= pTrade.Volume;
                            //    positionToReduce.AvailShort -= pTrade.Volume;
                            //}
                        }
                        else if (IsCffexRule(pTrade.InstrumentID))//Cffex专用
                        {
                            positionToReduce.TodayPosition = 0;
                            positionToReduce.YdPosition -= pTrade.Volume - positionToReduce.TodayPosition;
                        }
                        else
                        {
                            Util.WriteInfo("Illegal close order!");
                        }
                    }
                    else
                    {
                        positionToReduce.YdPosition -= pTrade.Volume;
                    }

                    if (positionToReduce.Position <= 0)
                    {
                        PositionField position;
                        PositionFields.TryRemove(key, out position);
                    }
                }
                else
                {
                    var temp = string.Format("错误:{0}并不含有{1}的{2}仓", pTrade.InstrumentID, posDirection, preferCloseDateType);
                    Util.WriteInfo(temp, true);
                }
            }
            catch (Exception ex)
            {
                Util.WriteException(ex);
            }
        }

        //private void ProcessStrategyMarkupStatistics(string instrumentId)
        //{
        //    StrategyControl strategy = Constant.StrategyMap[instrumentId]["TwapAdvanced"];
        //    if (strategy is StrategyTwapEnhanced)
        //    {
        //        (strategy as StrategyTwapEnhanced).ProcessMarkupOrdersStatictics(Constant.ConnectionStr);
        //    }
        //}

        private EnumThostPosiDirectionType GetOrderPosiDirection(CThostFtdcOrderField pOrder)
        {
            EnumThostPosiDirectionType direction = EnumThostPosiDirectionType.Net;
            if (pOrder.Direction == EnumThostDirectionType.Buy && pOrder.CombOffsetFlag_0 == EnumThostOffsetFlagType.Open
                || pOrder.Direction == EnumThostDirectionType.Sell && pOrder.CombOffsetFlag_0 != EnumThostOffsetFlagType.Open)
            {
                direction = EnumThostPosiDirectionType.Long;
            }
            else if (pOrder.Direction == EnumThostDirectionType.Sell && pOrder.CombOffsetFlag_0 == EnumThostOffsetFlagType.Open
               || pOrder.Direction == EnumThostDirectionType.Buy && pOrder.CombOffsetFlag_0 != EnumThostOffsetFlagType.Open)
            {
                direction = EnumThostPosiDirectionType.Short;
            }
            return direction;
        }

        private EnumThostPosiDirectionType GetOrderPosiDirection(CThostFtdcTradeField pTrade)
        {
            EnumThostPosiDirectionType direction = EnumThostPosiDirectionType.Net;
            if (pTrade.Direction == EnumThostDirectionType.Buy && pTrade.OffsetFlag == EnumThostOffsetFlagType.Open
                || pTrade.Direction == EnumThostDirectionType.Sell && pTrade.OffsetFlag != EnumThostOffsetFlagType.Open)
            {
                direction = EnumThostPosiDirectionType.Long;
            }
            else if (pTrade.Direction == EnumThostDirectionType.Sell && pTrade.OffsetFlag == EnumThostOffsetFlagType.Open
               || pTrade.Direction == EnumThostDirectionType.Buy && pTrade.OffsetFlag != EnumThostOffsetFlagType.Open)
            {
                direction = EnumThostPosiDirectionType.Short;
            }
            return direction;
        }

        private void UpdateAvailPosition(string investorId, EnumThostDirectionType orderDir, int orderDirectionKey)
        {
            //posKey
            if (PositionFields.ContainsKey(orderDirectionKey))
            {
                PositionFields[orderDirectionKey].Avail = PositionFields[orderDirectionKey].Position;
                PositionFields[orderDirectionKey].LongFrozen = PositionFields[orderDirectionKey].ShortFrozen = 0;
                PositionFields[orderDirectionKey].AvailToday = PositionFields[orderDirectionKey].TodayPosition;
                //Constant.WriteInfo(string.Format("{0}, Avail {1}, AvailToday: {2}", posKey, TraderAdapter.PositionFields[posKey].Avail, TraderAdapter.PositionFields[posKey].AvailToday));
            }

            // Processing
            if (DicPendingOrder.ContainsKey(orderDirectionKey))
            {
                foreach (CThostFtdcOrderField orderField in DicPendingOrder[orderDirectionKey].Values)
                {
                    if (PositionFields.ContainsKey(orderDirectionKey))
                    {
                        PositionField posField = PositionFields[orderDirectionKey];
                        posField.InvestorId = investorId;
                        if (posField.PosiDirection == EnumThostPosiDirectionType.Short && posField.Position > 0 && orderField.Direction == EnumThostDirectionType.Buy)
                        {
                            posField.LongFrozen += (orderField.VolumeTotal - orderField.VolumeTraded);
                        }
                        else if (posField.PosiDirection == EnumThostPosiDirectionType.Long && posField.Position > 0 && orderField.Direction == EnumThostDirectionType.Sell)
                        {
                            posField.ShortFrozen += (orderField.VolumeTotal - orderField.VolumeTraded);
                        }
                    }
                }
            }
            if (PositionFields.ContainsKey(orderDirectionKey))
            {
                PositionField posField = PositionFields[orderDirectionKey];
                if (orderDir == EnumThostDirectionType.Buy)
                {
                    posField.Avail = posField.Position - posField.LongFrozen;
                    posField.AvailToday = posField.TodayPosition > posField.LongFrozen ? posField.TodayPosition - posField.LongFrozen : 0;//?
                }
                else //Sell
                {
                    posField.Avail = posField.Position - posField.ShortFrozen;
                    posField.AvailToday = posField.TodayPosition > posField.ShortFrozen ? posField.TodayPosition - posField.ShortFrozen : 0;//?
                }
            }
            //Constant.WriteInfo(string.Format("{0}, Avail {1}, AvailToday: {2}", posKey, TraderAdapter.PositionFields[posKey].Avail, TraderAdapter.PositionFields[posKey].AvailToday));
        }

        private void CtpTraderApi_OnRspOrderInsert(ref CThostFtdcInputOrderField pInputOrder, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            try
            {
                if (pRspInfo.ErrorID != 0)
                {
                    if (pInputOrder.Direction == EnumThostDirectionType.Buy)
                    {
                        Util.IsSentBuyOrder[pInputOrder.InstrumentID] = false;
                        Util.IsFilledBuyOrder[pInputOrder.InstrumentID] = true;
                        Util.WriteInfo(string.Format("IsFilledBuyOrder Open: {0}", pInputOrder.InstrumentID), Program.InPrintMode);
                    }
                    else if (pInputOrder.Direction == EnumThostDirectionType.Sell)
                    {
                        Util.IsSentSellOrder[pInputOrder.InstrumentID] = false;
                        Util.IsFilledSellOrder[pInputOrder.InstrumentID] = true;
                        Util.WriteInfo(string.Format("IsFilledSellOrder Open: {0}", pInputOrder.InstrumentID), Program.InPrintMode);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.WriteExceptionToLogFile(ex);
            }
        }


        private void CtpTraderApi_OnRspOrderAction(ref CThostFtdcInputOrderActionField pInputOrderAction, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            try
            {
                if (pRspInfo.ErrorID != 0)
                {
                    if (OrderFields.ContainsKey(pInputOrderAction.OrderRef))
                    {
                        EnumThostDirectionType sourceDirection = OrderFields[pInputOrderAction.OrderRef].Direction;
                        if (sourceDirection == EnumThostDirectionType.Buy)
                        {
                            Util.IsSentCancelBuy[pInputOrderAction.InstrumentID] = false;
                            Util.WriteInfo(string.Format("IsSentCancelBuy Open: {0}", pInputOrderAction.InstrumentID));
                        }
                        else if (sourceDirection == EnumThostDirectionType.Sell)
                        {
                            Util.IsSentCancelSell[pInputOrderAction.InstrumentID] = false;
                            Util.WriteInfo(string.Format("IsSentCancelSell Open: {0}", pInputOrderAction.InstrumentID));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.WriteExceptionToLogFile(ex);
            }
        }

        private void CtpTraderApi_OnRspQryInstrumentMarginRate(ref CThostFtdcInstrumentMarginRateField pInstrumentMarginRate, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!InstrumentCategoryMarginRateDic.ContainsKey(pInstrumentMarginRate.InstrumentID))//pInstrumentMarginRate != null &&
            {
                InstrumentCategoryMarginRateDic[pInstrumentMarginRate.InstrumentID] = pInstrumentMarginRate;
            }
            AutoMarginEvent.Set();
        }

        private void CtpTraderApi_OnRspQryInstrumentCommissionRate(ref CThostFtdcInstrumentCommissionRateField pInstrumentCommissionRate, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!InstrumentCategoryCommissionRateDic.ContainsKey(pInstrumentCommissionRate.InstrumentID))//pInstrumentCommissionRate != null &&
            {
                InstrumentCategoryCommissionRateDic[pInstrumentCommissionRate.InstrumentID] = pInstrumentCommissionRate;
            }
            AutoCommissionEvent.Set();
        }

        public void ReqMarketData(string[] codes)
        {
            if (_CtpMdApi == null)
            {
                Util.WriteWarn("Error! ReqMarketData: CtpMdApi is null!");
                return;
            }
            _CtpMdApi.SubscribeMarketData(codes);
            Util.WriteInfo("MdApiCTP CtpDataServer: SubMarketData is Executed.");
        }

        public void CancelMarketData(string[] codes)
        {
            if (_CtpMdApi == null)
            {
                Util.WriteWarn("Error! CancelMarketData: CtpMdApi is null!");
                return;
            }

            _CtpMdApi.UnSubscribeMarketData(codes);
            Util.WriteInfo("MdApiCTP CtpDataServer: UnSubMarketData is Executed.");
        }

        private void CtpMdApi_OnFrontConnected()
        {
            Util.WriteInfo("MdApiCTP CtpDataServer: OnFrontConnected is received.");
            int logFlag = _CtpMdApi.ReqUserLogin();
        }

        void CtpMdApi_OnFrontDisconnected(int nReason)
        {
            Util.WriteInfo("MdApiCTP CtpDataServer: OnDisConnected is received.");
        }

        void CtpMdApi_OnRspUserLogin(ref CThostFtdcRspUserLoginField pRspUserLogin, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.WriteInfo("Error! MdApiCTP CtpDataServer: OnRspUserLogin !bIsLast");
            }
            if (bIsLast)
            {
                AutoInitEvent.Set();
            }
        }

        void CtpMdApi_OnRspUserLogout(ref CThostFtdcUserLogoutField pUserLogout, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (!bIsLast)
            {
                Util.WriteInfo("Error! MdApiCTP CtpDataServer: OnRspUserLogout !bIsLast");
            }
            if (pRspInfo.ErrorID != 0)
            {
                Util.WriteInfo("Error! MdApiCTP CtpDataServer Logout fails! pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg);
            }
            else
            {
                Util.WriteInfo("Logout succeeds，UserID：" + pUserLogout.UserID);
                _CtpMdApi.DisConnect();
                _CtpMdApi = null; //TODO: 被disconnect方法阻塞，无法重置对象
            }
        }

        void CtpMdApi_OnRspSubMarketData(ref CThostFtdcSpecificInstrumentField pSpecificInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (pRspInfo.ErrorID == 0)
            {
                Util.WriteInfo("行情请求完成，合约:" + pSpecificInstrument.InstrumentID);
            }
            else
            {
                Util.WriteInfo("Error! MdApiCTP CtpDataServer pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + " Instrument:" + pSpecificInstrument.InstrumentID);
            }
            
        }

        void CtpMdApi_OnRspUnSubMarketData(ref CThostFtdcSpecificInstrumentField pSpecificInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            Util.WriteInfo("MdApiCTP CtpDataServer: OnRspUnSubMarketData is received. Instrument:" + pSpecificInstrument.InstrumentID);
            if (pRspInfo.ErrorID == 0)
            {
                //Util.Log("MdApiCTP CtpDataServer: Quotes unsubscription succeeds");
            }
            else
            {
                Util.WriteInfo("Error! MdApiCTP CtpDataServer pRspInfo: ID:" + pRspInfo.ErrorID + " ErrorMsg:" + pRspInfo.ErrorMsg + " Instrument:" + pSpecificInstrument.InstrumentID);
            }
        }

        private void DepthMarketData(CThostFtdcDepthMarketDataField pDepthMarketData)
        {
            lock (RestartObject)
            {
                if (Util.NeedRestartQuote && !Util.IsQuoteReloaded[pDepthMarketData.InstrumentID])
                {
                    if (InstrumentFields.ContainsKey(pDepthMarketData.InstrumentID))
                    {
                        string exchangeId = InstrumentFields[pDepthMarketData.InstrumentID].ExchangeId;
                        if (!TradingTimeManager.IsBeforeOpeningTime(DateTime.Now, exchangeId, pDepthMarketData.InstrumentID))
                        {
                            Util.IsQuoteReloading[pDepthMarketData.InstrumentID] = true;
                            Task.Run(() =>
                            {
                                Util.WriteInfo("Reload starts");
                                if (!Util.InstrumentMarketDataRecord.ContainsKey(pDepthMarketData.InstrumentID))
                                {
                                    Util.InstrumentMarketDataRecord[pDepthMarketData.InstrumentID] = new List<CThostFtdcDepthMarketDataField>();
                                }
                                QuoteReload(pDepthMarketData.InstrumentID);
                                DepthMarketDataInit?.Invoke(pDepthMarketData.InstrumentID, Util.InstrumentMarketDataRecord[pDepthMarketData.InstrumentID]);
                                Util.IsQuoteReloading[pDepthMarketData.InstrumentID] = false;
                                Util.WriteInfo("Reload ends");
                                //Constant.InstrumentMarketDataRecord[pDepthMarketData.InstrumentID].Clear();
                            });
                            Util.WriteInfo("Reload triggered");
                        }
                    }
                    Util.IsQuoteReloaded[pDepthMarketData.InstrumentID] = true;
                }

                if (Util.IsQuoteReloading.ContainsKey(pDepthMarketData.InstrumentID) && Util.IsQuoteReloading[pDepthMarketData.InstrumentID])
                {
                    if (!Util.MarketDataCache.ContainsKey(pDepthMarketData.InstrumentID))
                    {
                        Util.MarketDataCache[pDepthMarketData.InstrumentID] = new List<CThostFtdcDepthMarketDataField>();
                    }
                    Util.MarketDataCache[pDepthMarketData.InstrumentID].Add(pDepthMarketData);
                }
                else if (Util.MarketDataCache.ContainsKey(pDepthMarketData.InstrumentID))
                {
                    List<CThostFtdcDepthMarketDataField> marketDataLst = new List<CThostFtdcDepthMarketDataField>();
                    DepthMarketDataInit?.Invoke(pDepthMarketData.InstrumentID, Util.MarketDataCache[pDepthMarketData.InstrumentID]);
                    Util.MarketDataCache.TryRemove(pDepthMarketData.InstrumentID, out marketDataLst);
                    DepthMarketDataProcessing?.Invoke(pDepthMarketData);
                }
                else
                {
                    DepthMarketDataProcessing?.Invoke(pDepthMarketData);
                }
            }
        }

        private void CtpMdApi_OnRtnDepthMarketData(ref CThostFtdcDepthMarketDataField pDepthMarketData)
        {
            //if (GCSettings.LatencyMode != GCLatencyMode.NoGCRegion)
            //{
            //    try
            //    {
            //        GC.TryStartNoGCRegion(50 * 1024 * 8);
            //    }
            //    catch (Exception ex)
            //    {
            //        Constant.WriteExceptionToLogFile(ex);
            //    }
            //}
            //TradingWatch = Stopwatch.StartNew();
            CThostFtdcDepthMarketDataField realData = MarketDataReport(pDepthMarketData);
            DepthMarketDataFields[pDepthMarketData.InstrumentID] = realData;
            DepthMarketData(pDepthMarketData);
            //TradingWatch.Stop();
            //Constant.WriteInfo(string.Format("Market Data {0} ticks, {1} ns", TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Constant.NanosecPerTick));
        }

        private void InsertReport(TradingReport report)
        {
            string command = @"Insert into [tradedb].[dbo].[tb_TradingReport] ([TradingDay],[InvestorID],[BrokerID],[InstrumentID],[BuySell],[OpenClose] ,[CommitVolume] ,[TradedVolume],[CancelVolume],[OppositePrice],[AvgTradedPrice],[UpdateTime],[InsertCostTime],[OrderRef],[Slippage],[CloseProfit])
                values ('{0}','{1}','{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},'{11}',{12},'{13}',{14},{15})";
            command = string.Format(command, TradingTimeManager.TradingDay.ToString("yyyyMMdd"), _CtpTraderApi.InvestorID, _CtpTraderApi.BrokerID, report.InstrumentID, report.BuySell, report.OpenClose, report.CommitVolume, report.TradedVolume, report.CancelVolume,
                report.OppositePrice, report.AvgTradedPrice, report.UpdateTime, report.InsertCostTime, report.OrderRef, report.Slippage, report.CloseProfit);
            Util.WriteInfo(string.Format("执行插入: {0}", command));
            using (SqlConnection sqlconn = new SqlConnection(Util.ConnectionStr))
            {
                sqlconn.Open();
                if (sqlconn.State == ConnectionState.Open)
                {
                    var com = new SqlCommand
                    {
                        Connection = sqlconn,
                        CommandType = CommandType.Text,
                        CommandText = command
                    };
                    com.ExecuteNonQuery();
                }
            }
        }

        private CThostFtdcDepthMarketDataField MarketDataReport(CThostFtdcDepthMarketDataField pDepthMarketData)
        {
            CThostFtdcDepthMarketDataField ctpRealData = new CThostFtdcDepthMarketDataField();
            ctpRealData.AveragePrice = pDepthMarketData.AveragePrice;
            ctpRealData.BidPrice1 = pDepthMarketData.BidPrice1 >= double.MaxValue ? 0.0 : pDepthMarketData.BidPrice1;
            ctpRealData.BidPrice2 = pDepthMarketData.BidPrice2 >= double.MaxValue ? 0.0 : pDepthMarketData.BidPrice2;
            ctpRealData.BidPrice3 = pDepthMarketData.BidPrice3 >= double.MaxValue ? 0.0 : pDepthMarketData.BidPrice3;
            ctpRealData.BidPrice4 = pDepthMarketData.BidPrice4 >= double.MaxValue ? 0.0 : pDepthMarketData.BidPrice4;
            ctpRealData.BidPrice5 = pDepthMarketData.BidPrice5 >= double.MaxValue ? 0.0 : pDepthMarketData.BidPrice5;
            ctpRealData.BidVolume1 = pDepthMarketData.BidVolume1;
            ctpRealData.BidVolume2 = pDepthMarketData.BidVolume2;
            ctpRealData.BidVolume3 = pDepthMarketData.BidVolume3;
            ctpRealData.BidVolume4 = pDepthMarketData.BidVolume4;
            ctpRealData.BidVolume5 = pDepthMarketData.BidVolume5;
            ctpRealData.AskPrice1 = pDepthMarketData.AskPrice1 >= double.MaxValue ? 0.0 : pDepthMarketData.AskPrice1;
            ctpRealData.AskPrice2 = pDepthMarketData.AskPrice2 >= double.MaxValue ? 0.0 : pDepthMarketData.AskPrice2;
            ctpRealData.AskPrice3 = pDepthMarketData.AskPrice3 >= double.MaxValue ? 0.0 : pDepthMarketData.AskPrice3;
            ctpRealData.AskPrice4 = pDepthMarketData.AskPrice4 >= double.MaxValue ? 0.0 : pDepthMarketData.AskPrice4;
            ctpRealData.AskPrice5 = pDepthMarketData.AskPrice5 >= double.MaxValue ? 0.0 : pDepthMarketData.AskPrice5;
            ctpRealData.AskVolume1 = pDepthMarketData.AskVolume1;
            ctpRealData.AskVolume2 = pDepthMarketData.AskVolume2;
            ctpRealData.AskVolume3 = pDepthMarketData.AskVolume3;
            ctpRealData.AskVolume4 = pDepthMarketData.AskVolume4;
            ctpRealData.AskVolume5 = pDepthMarketData.AskVolume5;
            ctpRealData.ClosePrice = pDepthMarketData.ClosePrice >= double.MaxValue ? 0.0 : pDepthMarketData.ClosePrice;
            ctpRealData.CurrDelta = pDepthMarketData.CurrDelta >= double.MaxValue ? 0.0 : pDepthMarketData.CurrDelta;
            ctpRealData.ExchangeID = pDepthMarketData.ExchangeID;
            ctpRealData.ExchangeInstID = pDepthMarketData.ExchangeInstID;
            ctpRealData.HighestPrice = pDepthMarketData.HighestPrice >= double.MaxValue ? 0.0 : pDepthMarketData.HighestPrice;
            ctpRealData.InstrumentID = pDepthMarketData.InstrumentID;
            ctpRealData.LastPrice = pDepthMarketData.LastPrice >= double.MaxValue ? 0.0 : pDepthMarketData.LastPrice;
            ctpRealData.LowerLimitPrice = pDepthMarketData.LowerLimitPrice >= double.MaxValue ? 0.0 : pDepthMarketData.LowerLimitPrice;
            ctpRealData.LowestPrice = pDepthMarketData.LowestPrice >= double.MaxValue ? 0.0 : pDepthMarketData.LowestPrice;
            ctpRealData.OpenPrice = pDepthMarketData.OpenPrice >= double.MaxValue ? 0.0 : pDepthMarketData.OpenPrice;
            ctpRealData.OpenInterest = pDepthMarketData.OpenInterest;//?
            ctpRealData.PreClosePrice = pDepthMarketData.PreClosePrice >= double.MaxValue ? 0.0 : pDepthMarketData.PreClosePrice;
            ctpRealData.PreDelta = pDepthMarketData.PreDelta >= double.MaxValue ? 0.0 : pDepthMarketData.PreDelta;
            ctpRealData.PreOpenInterest = pDepthMarketData.PreOpenInterest;//?
            ctpRealData.PreSettlementPrice = pDepthMarketData.PreSettlementPrice >= double.MaxValue ? 0.0 : pDepthMarketData.PreSettlementPrice;
            ctpRealData.SettlementPrice = pDepthMarketData.SettlementPrice >= double.MaxValue ? 0.0 : pDepthMarketData.SettlementPrice;
            ctpRealData.TradingDay = pDepthMarketData.TradingDay;
            ctpRealData.Turnover = pDepthMarketData.Turnover;
            ctpRealData.UpdateTime = pDepthMarketData.UpdateTime;
            ctpRealData.UpdateMillisec = pDepthMarketData.UpdateMillisec;
            ctpRealData.UpperLimitPrice = pDepthMarketData.UpperLimitPrice >= double.MaxValue ? 0.0 : pDepthMarketData.UpperLimitPrice;
            ctpRealData.Volume = pDepthMarketData.Volume;

            return ctpRealData;
        }

    }

}
