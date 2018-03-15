using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradingMaster.CodeSet;
using TradingMaster.Control;
using TradingMaster.JYData;

namespace TradingMaster
{
    public class TradeDataClient
    {
        const int LOGWINDOWMEDIUNHEIGHT = 505;
        const int LOGWINDOWSMALLHEIGHT = 305;

        private static TradeDataClient _TradeClientInstance = null;
        private Login _LogWindow;
        private MainWindow _MainWindow;
        private StatementOrderAffirm _AffirmWindow;
        private Uri _NormalImage;
        private Uri _ErrorImage;
        private string _TradingDay = String.Empty;

        private Task _ResponseTask;
        //private Task _MarketDataTask;
        private CancellationTokenSource _TraderCts;
        //private CancellationTokenSource _MdCts;
        private SynQueue<object> _MarketDataQueue;
        private SynQueue<object> _OrderResponseQueue;
        private SynQueue<object> _TradeResponseQueue;
        private SynQueue<object> _PositionResponseQueue;
        private SynQueue<object> _MessageResponseQueue;
        private SynQueue<object> _QueryResponseQueue;
        private Guid _Guid;
        private int _TimeOut = 0;
        private bool _IsRestThreadCondition = false;

        /// <summary>
        /// 交易所时间更新的定时器
        /// </summary>
        private Timer _ExchTimer;

        /// <summary>
        /// 交易所时间
        /// </summary>
        private ExchangeTime _ExTime;

        private Dictionary<string, List<TradeOrderData>> _MultiUsersOrderDataDic = new Dictionary<string, List<TradeOrderData>>();
        private Dictionary<string, List<QuoteOrderData>> _MultiUsersQuoteOrderDataDic = new Dictionary<string, List<QuoteOrderData>>();
        private Dictionary<string, List<ExecOrderData>> _MultiUsersExecOrderDataDic = new Dictionary<string, List<ExecOrderData>>();
        private Dictionary<string, List<TradeOrderData>> _MultiUsersTradeDataDic = new Dictionary<string, List<TradeOrderData>>();
        private Dictionary<string, List<PosInfoDetail>> _MultiUsersPosDetailDataDic = new Dictionary<string, List<PosInfoDetail>>();
        private Dictionary<string, List<PosInfoTotal>> _MultiUsersPositionDataDic = new Dictionary<string, List<PosInfoTotal>>();

        /// <summary>
        /// 报单记录更新
        /// </summary>
        private Dictionary<string, TradeOrderData> _OrderDataDic = new Dictionary<string, TradeOrderData>();

        /// <summary>
        /// 报价记录更新
        /// </summary>
        private Dictionary<string, QuoteOrderData> _QuoteOrderDataDic = new Dictionary<string, QuoteOrderData>();

        /// <summary>
        /// 执行记录更新
        /// </summary>
        private Dictionary<string, ExecOrderData> _ExecOrderDataDic = new Dictionary<string, ExecOrderData>();

        /// <summary>
        /// 成交记录更新
        /// </summary>
        private List<TradeOrderData> _TradeDataLst = new List<TradeOrderData>();

        /// <summary>
        /// 持仓相关锁
        /// </summary>
        private static object _ServerLock;
        private List<PosInfoDetail> _PositionDetailDataLst = new List<PosInfoDetail>();
        private List<PosInfoTotal> _PositionSumDataLst = new List<PosInfoTotal>();

        /// <summary>
        /// 某个品种今仓多少手，昨仓多少手的MAP
        /// </summary>
        private Dictionary<string, PosHandCount> _PosHandCountMap = new Dictionary<string, PosHandCount>();

        /// <summary>
        /// 用于记录持仓信息的MAP
        /// </summary>
        public Dictionary<string, PosInfoTotal> PosTotalInfoMap = new Dictionary<string, PosInfoTotal>();

        /// <summary>
        /// 用于记录持仓信息中未成交的平仓单
        /// </summary>
        private Dictionary<string, List<TradeOrderData>> _FreezeOrder = new Dictionary<string, List<TradeOrderData>>();

        /// <summary>
        /// 用于记录行权信息中未执行的平仓单
        /// </summary>
        //private Dictionary<string, List<ExecOrderData>> _FreezeExecOrder = new Dictionary<string, List<ExecOrderData>>();
        private Dictionary<string, int> _FreezeExecOrderHandCount = new Dictionary<string, int>();

        /// <summary>
        /// 合约手续费率
        /// </summary>
        protected Dictionary<string, CommissionStruct> CommissionDict = new Dictionary<string, CommissionStruct>();

        /// <summary>
        /// 合约保证金率
        /// </summary>
        protected Dictionary<string, MarginStruct> MarginDict = new Dictionary<string, MarginStruct>();

        /// <summary>
        /// 已提交报单记录
        /// </summary>
        private List<string> _CommitOrders = new List<string>();
        private List<string> _TransactOrders = new List<string>();
        private List<string> _CancelOrders = new List<string>();

        /// <summary>
        /// 已提交报单记录
        /// </summary>
        private List<TransferSingleRecord> _TransferRecords = new List<TransferSingleRecord>();

        private List<object> _DataServerLst = null;

        private TradeDataClient()
        {
            _DataServerLst = new List<object>();
            _NormalImage = new Uri(@"image\TradeNormal.gif", UriKind.RelativeOrAbsolute);
            _ErrorImage = new Uri(@"image\TradeError.jpg", UriKind.RelativeOrAbsolute);
            _Guid = Guid.NewGuid();
            _MarketDataQueue = new SynQueue<object>();
            //_MdCts = new CancellationTokenSource();
            //_MarketDataTask = Task.Factory.StartNew(() => MarketDataThreadProc(_MdCts.Token), _MdCts.Token);

            _ServerLock = new object();
            _OrderResponseQueue = new SynQueue<object>();
            _TradeResponseQueue = new SynQueue<object>();
            _PositionResponseQueue = new SynQueue<object>();
            _MessageResponseQueue = new SynQueue<object>();
            _QueryResponseQueue = new SynQueue<object>();
            _TraderCts = new CancellationTokenSource();

            _LogWindow = Login.LoginInstace;
            string timeNow = DateTime.Now.ToString("HH:mm:ss");
            if (_ExTime == null)
            {
                _ExTime = new ExchangeTime(timeNow, timeNow, timeNow, timeNow, timeNow);
            }
            _ExchTimer = new Timer(new TimerCallback(TimerCallBack), null, 1000, 1000);
        }

        public static TradeDataClient GetClientInstance()
        {
            if (_TradeClientInstance == null)
            {
                _TradeClientInstance = new TradeDataClient();
            }
            return _TradeClientInstance;
        }

        private void StartClientThread()
        {
            if (!_IsRestThreadCondition && (_ResponseTask == null || _ResponseTask.IsCompleted))
            {
                _ResponseTask = Task.Factory.StartNew(() => ResponseThreadProc(_TraderCts.Token), _TraderCts.Token);
            }
        }

        public void SetMainWindow(MainWindow mainWindow)
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

        private object GetDataServerInstance(string investorID, BACKENDTYPE backEnd)
        {
            foreach (object item in _DataServerLst)
            {
                if (item is CtpDataServer && backEnd == BACKENDTYPE.CTP)
                {
                    CtpDataServer ctpServer = item as CtpDataServer;
                    if (ctpServer.InvestorID == investorID)
                    {
                        return ctpServer;
                    }
                }
                else if (item is FemasDataServer && backEnd == BACKENDTYPE.FEMAS)
                {
                    FemasDataServer femasServer = item as FemasDataServer;
                    if (femasServer.InvestorID == investorID)
                    {
                        return femasServer;
                    }
                }
            }
            return null;
        }

        public void InitiateCounterApi(BACKENDTYPE backEnd, string account, string password, string tradeBrokerId, string tradeIp, string quoteBrokerID, string quoteIp)
        {
            if (backEnd == BACKENDTYPE.CTP)
            {
                CtpDataServer ctpServer = CtpDataServer.GetUserInstance();
                if (ctpServer != null)
                {
                    _DataServerLst.Add(ctpServer);
                    ctpServer.InitTradeApi(account, password, tradeBrokerId, tradeIp);
                    ctpServer.InitQuoteApi(account, password, quoteBrokerID, quoteIp);
                }
                else
                {
                    Util.Log("Error! Illegal Counter: " + backEnd);
                }
            }
            if (backEnd == BACKENDTYPE.FEMAS)
            {
                FemasDataServer femasServer = FemasDataServer.GetUserInstance();
                if (femasServer != null)
                {
                    _DataServerLst.Add(femasServer);
                    femasServer.InitTradeApi(account, password, tradeBrokerId, tradeIp);
                    femasServer.InitQuoteApi(account, password, quoteBrokerID, quoteIp);
                }
                else
                {
                    Util.Log("Error! Illegal Counter: " + backEnd);
                }
            }
        }

        public bool IsTradeServerLogon(BACKENDTYPE backEnd, string account)
        {
            if (backEnd == BACKENDTYPE.CTP)
            {
                CtpDataServer ctpServer = GetDataServerInstance(account, backEnd) as CtpDataServer;
                if (ctpServer != null)
                {
                    return ctpServer.TradeServerLogOn();
                }
                else
                {
                    Util.Log("Error! Illegal Counter: " + backEnd);
                }
            }
            if (backEnd == BACKENDTYPE.FEMAS)
            {
                FemasDataServer femasServer = GetDataServerInstance(account, backEnd) as FemasDataServer;
                if (femasServer != null)
                {
                    return femasServer.TradeServerLogOn();
                }
                else
                {
                    Util.Log("Error! Illegal Counter: " + backEnd);
                }
            }
            return false;
        }

        public void RequestTradeData(string investorID, BACKENDTYPE backEnd, RequestContent cmd)
        {
            object dataServer = null;
            if (investorID.Trim() != "")
            {
                dataServer = GetDataServerInstance(investorID, backEnd);
            }
            else //Todo
            {
                dataServer = CtpDataServer.GetUserInstance();
            }
            if (dataServer is CtpDataServer)
            {
                CtpDataServer ctpServer = dataServer as CtpDataServer;
                ctpServer.AddToTradeDataQryQueue(cmd);
            }
            else if (dataServer is FemasDataServer)
            {

            }
            else
            {
                Util.Log("Illegal back-end type: " + backEnd);
            }
        }

        public void RequestMarketData(string investorID, BACKENDTYPE backEnd, RequestContent cmd)
        {
            object dataServer = null;
            if (investorID.Trim() != "")
            {
                dataServer = GetDataServerInstance(investorID, backEnd);
            }
            else //Todo
            {
                dataServer = CtpDataServer.GetUserInstance();
            }
            if (dataServer is CtpDataServer)
            {
                CtpDataServer ctpServer = dataServer as CtpDataServer;
                ctpServer.AddToMarketDataQryQueue(cmd);
            }
            else if (dataServer is FemasDataServer)
            {

            }
            else
            {
                Util.Log("Illegal back-end type: " + backEnd);
            }
        }

        public void RequestOrder(string investorID, BACKENDTYPE backEnd, RequestContent cmd)
        {
            object dataServer = null;
            if (investorID.Trim() != "")
            {
                dataServer = GetDataServerInstance(investorID, backEnd);
            }
            else
            {
                dataServer = CtpDataServer.GetUserInstance();
            }
            if (dataServer is CtpDataServer)
            {
                CtpDataServer ctpServer = dataServer as CtpDataServer;
                ctpServer.AddToOrderQueue(cmd);
            }
            else if (dataServer is FemasDataServer)
            {

            }
            else
            {
                Util.Log("Illegal back-end type: " + backEnd);
            }
        }

        public void RefreshSystemData()
        {
            ClearServerData();
            CtpDataServer.GetUserInstance().InitDataFromAPI();
        }

        public void RefreshMarketData()
        {
            if (_MainWindow != null)
            {
                _MainWindow.HQBackgroundRealData.RefreshAutoPushMarketData();
                //_MainWindow.HQBackgroundRealData.Request();
                _MainWindow.uscHangqing.HQRealData.RefreshAutoPushMarketData();
                _MainWindow.uscHangqing.GroupHQRealData.RefreshAutoPushMarketData();
                _MainWindow.uscOptionHangqing.OptionQuotesRealData.RefreshAutoPushMarketData();
            }
        }

        private void ClearServerData()
        {
            _CancelOrders.Clear();
            _CommitOrders.Clear();
            _ExecOrderDataDic.Clear();
            _MultiUsersExecOrderDataDic.Clear();
            _MultiUsersOrderDataDic.Clear();
            _MultiUsersPosDetailDataDic.Clear();
            _MultiUsersPositionDataDic.Clear();
            _MultiUsersQuoteOrderDataDic.Clear();
            _MultiUsersTradeDataDic.Clear();
            _OrderDataDic.Clear();
            _PositionDetailDataLst.Clear();
            _PositionSumDataLst.Clear();
            _QuoteOrderDataDic.Clear();
            _TradeDataLst.Clear();
            _TransactOrders.Clear();

            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    //_LogWindow.OnLogOut(errorMessage);
                    if (_MainWindow != null)
                    {
                        _MainWindow.UscStatementsInquiry.txtStatementOrder.Text = "";
                    }
                });
            }

        }

        public void RtnDepthMarketDataEnqueue(object data)
        {
            if (data != null)
            {
                _MarketDataQueue.Enqueue(data, false);
                _IsRestThreadCondition = false;
                StartClientThread();
            }
        }

        private void MarketDataThreadProc(CancellationToken ct)
        {
            while ((!ct.IsCancellationRequested))//(ExecFlag)
            {
                try
                {
                    MarketDataProcess();
                }
                catch (Exception ex)
                {
                    Util.Log_Error("exception: " + ex.Message);
                    Util.Log_Error("exception: " + ex.StackTrace);
                    ct.ThrowIfCancellationRequested();
                }
            }
        }

        private void MarketDataProcess()
        {
            List<object> dataList = _MarketDataQueue.DequeueAll(_TimeOut);
            Dictionary<Contract, RealData> realDataDic = new Dictionary<Contract, RealData>();
            foreach (object data in dataList)
            {
                if (data is RealData)
                {
                    RealData tempData = data as RealData;
                    if (realDataDic.ContainsKey(tempData.CodeInfo))
                    {
                        realDataDic[tempData.CodeInfo] = tempData;
                    }
                    else
                    {
                        realDataDic.Add(tempData.CodeInfo, tempData);
                    }

                }
            }
            if (_MainWindow != null)
            {
                _MainWindow.OnReceiveSnapShotOrUpdateCallBack(realDataDic);
            }
        }

        public void RemoveUserRecord(string user)
        {
            RemoveUserOrderRecord(user);
            RemoveUserTradeRecord(user);
            RemoveUserPositionRecord(user);
        }

        private void RemoveUserOrderRecord(string user)
        {
            if (_MultiUsersOrderDataDic.ContainsKey(user))
            {
                foreach (TradeOrderData orderData in _MultiUsersOrderDataDic[user])
                {
                    string tempKey = orderData.BrokerOrderSeq + "_" + orderData.OrderRef + "_" + orderData.Exchange;
                    if (_OrderDataDic.ContainsKey(tempKey))
                    {
                        _OrderDataDic.Remove(tempKey);
                    }
                }
                _MultiUsersOrderDataDic.Remove(user);
                List<TradeOrderData> orderList = new List<TradeOrderData>();
                foreach (string userKey in _MultiUsersOrderDataDic.Keys)
                {
                    orderList.AddRange(_MultiUsersOrderDataDic[userKey]);
                }
                Util.Log("orderList Items: " + orderList.Count);
                ProcessJYOrderData(orderList);
            }
        }

        private void RemoveUserQuoteOrderRecord(string user)
        {
            if (_MultiUsersQuoteOrderDataDic.ContainsKey(user))
            {
                foreach (QuoteOrderData qOrderData in _MultiUsersQuoteOrderDataDic[user])
                {
                    string tempKey = qOrderData.BrokerQuoteSeq + "_" + qOrderData.QuoteRef + "_" + qOrderData.Exchange;
                    if (_QuoteOrderDataDic.ContainsKey(tempKey))
                    {
                        _QuoteOrderDataDic.Remove(tempKey);
                    }
                }
                _MultiUsersQuoteOrderDataDic.Remove(user);
                List<QuoteOrderData> orderList = new List<QuoteOrderData>();
                foreach (string userKey in _MultiUsersQuoteOrderDataDic.Keys)
                {
                    orderList.AddRange(_MultiUsersQuoteOrderDataDic[userKey]);
                }
                ProcessQuoteInsertOrderData(orderList);
            }
        }

        private void RemoveUserExecOrderRecord(string user)
        {
            if (_MultiUsersExecOrderDataDic.ContainsKey(user))
            {
                foreach (ExecOrderData execOrderData in _MultiUsersExecOrderDataDic[user])
                {
                    string tempKey = execOrderData.BrokerExecOrderSeq + "_" + execOrderData.ExecOrderRef + "_" + execOrderData.Exchange;
                    if (_ExecOrderDataDic.ContainsKey(tempKey))
                    {
                        _ExecOrderDataDic.Remove(tempKey);
                    }
                }
                _MultiUsersExecOrderDataDic.Remove(user);
                List<ExecOrderData> orderList = new List<ExecOrderData>();
                foreach (string userKey in _MultiUsersExecOrderDataDic.Keys)
                {
                    orderList.AddRange(_MultiUsersExecOrderDataDic[userKey]);
                }
                ProcessExecInsertOrderData(orderList);
            }
        }

        private void RemoveUserTradeRecord(string user)
        {
            if (_MultiUsersTradeDataDic.ContainsKey(user))
            {
                foreach (TradeOrderData tradeData in _MultiUsersTradeDataDic[user])
                {
                    if (_TradeDataLst.Contains(tradeData))
                    {
                        _TradeDataLst.Remove(tradeData);
                    }
                }
                _MultiUsersTradeDataDic.Remove(user);
                Util.Log("_TradeDataLst Items: " + _TradeDataLst.Count);
                ProcessTradedOrderData(_TradeDataLst);
            }
        }

        private void RemoveUserPositionRecord(string user)
        {
            if (_MultiUsersPosDetailDataDic.Count > 0)
            {
                if (_MultiUsersPosDetailDataDic.ContainsKey(user))
                {
                    foreach (PosInfoDetail posData in _MultiUsersPosDetailDataDic[user])
                    {
                        if (_PositionDetailDataLst.Contains(posData))
                        {
                            _PositionDetailDataLst.Remove(posData);
                        }
                    }
                    _MultiUsersPosDetailDataDic.Remove(user);
                    List<PosInfoDetail> posList = new List<PosInfoDetail>();
                    foreach (string userKey in _MultiUsersPosDetailDataDic.Keys)
                    {
                        posList.AddRange(_MultiUsersPosDetailDataDic[userKey]);
                    }
                    Util.Log("posList Items: " + posList.Count);
                    ProcessPositionDetailData(posList);
                }
            }
            else if (_MultiUsersPositionDataDic.ContainsKey(user))
            {
                foreach (PosInfoTotal posData in _MultiUsersPositionDataDic[user])
                {
                    if (_PositionSumDataLst.Contains(posData))
                    {
                        _PositionSumDataLst.Remove(posData);
                    }
                }
                _MultiUsersPositionDataDic.Remove(user);
                List<PosInfoTotal> positionList = new List<PosInfoTotal>();
                foreach (string userKey in _MultiUsersPositionDataDic.Keys)
                {
                    positionList.AddRange(_MultiUsersPositionDataDic[userKey]);
                }
                ProcessPositions_Total(positionList);
            }
        }

        public void RtnQueryEnqueue(object data)
        {
            if (data != null)
            {
                _QueryResponseQueue.Enqueue(data, false);
                _IsRestThreadCondition = false;
                StartClientThread();
            }
        }

        public void RtnOrderEnqueue(object data)
        {
            if (data != null)
            {
                _OrderResponseQueue.Enqueue(data, false);
                _IsRestThreadCondition = false;
                StartClientThread();
            }
        }

        public void RtnTradeEnqueue(object data)
        {
            if (data != null)
            {
                _TradeResponseQueue.Enqueue(data, false);
                _IsRestThreadCondition = false;
                StartClientThread();
            }
        }

        public void RtnPositionEnqueue(object data)
        {
            if (data != null)
            {
                _PositionResponseQueue.Enqueue(data, false);
                _IsRestThreadCondition = false;
                StartClientThread();
            }
        }

        public void RtnMessageEnqueue(object data)
        {
            if (data != null)
            {
                _MessageResponseQueue.Enqueue(data, false);
                _IsRestThreadCondition = false;
                StartClientThread();
            }
        }

        private void ResponseThreadProc(CancellationToken ct)
        {
            while ((!ct.IsCancellationRequested) && !RestThreadCondition())//(ExecFlag)
            {
                try
                {
                    QueryResponseProcess();
                    OrderResponseProcess();
                    TradeResponseProcess();
                    PositionResponseProcess();
                    MessageProcess();
                    MarketDataProcess();
                }
                catch (Exception ex)
                {
                    Util.Log_Error("exception: " + ex.Message);
                    Util.Log_Error("exception: " + ex.StackTrace);
                    ct.ThrowIfCancellationRequested();
                }
            }
        }

        private bool RestThreadCondition()
        {
            _IsRestThreadCondition = _QueryResponseQueue.Count() <= 0 && _OrderResponseQueue.Count() <= 0 && _TradeResponseQueue.Count() <= 0 && _PositionResponseQueue.Count() <= 0
                && _MessageResponseQueue.Count() <= 0 && _MarketDataQueue.Count() <= 0;
            return _IsRestThreadCondition;
        }

        private void QueryResponseProcess()
        {
            bool transferFlag = false;
            int itemCount = 0;
            List<object> dataList = _QueryResponseQueue.DequeueAll(_TimeOut);
            foreach (object data in dataList)
            {
                if (data is LogonStruct)
                {
                    LogonStruct logonInfo = data as LogonStruct;
                    _ExTime = logonInfo.ExchTime;
                    NotifyTimeChange(_ExTime);
                    if (logonInfo.FrontType == "Trader")
                    {
                        OnLogon(logonInfo.BackEnd);
                    }
                    else if (logonInfo.FrontType == "Md")
                    {
                        OnQuote();
                        RefreshMarketData();
                    }
                }
                if (data is LogoffStruct)
                {
                    LogoffStruct logoffInfo = data as LogoffStruct;
                    if (logoffInfo.FrontType == "Trader")
                    {
                        OnLogout(logoffInfo.OutMsg);
                    }
                    else if (logoffInfo.FrontType == "Md")
                    {
                        OnQuote();
                    }
                }
                if (data is CapitalInfo)
                {
                    CapitalInfo capInfo = data as CapitalInfo;
                    ReceiveCapitalInfo(capInfo);
                }
                if (data is StatementOrderAffirm)
                {
                    _AffirmWindow = data as StatementOrderAffirm;
                }
                if (data is SettlementStruct)
                {
                    SettlementStruct settlementInfo = data as SettlementStruct;
                    List<byte> settlementInfoCharLst = settlementInfo.SettlementInfoCharList;
                    string settlementSheet = String.Empty;
                    byte[] settleBytes = settlementInfoCharLst.ToArray();
                    settlementSheet = Encoding.Default.GetString(settleBytes);
                    OnReceiveSettlementSheet(settlementInfo.InvestorID, settlementSheet);
                }
                if (data is List<BankAccountInfo>)
                {
                    List<BankAccountInfo> bankAcctInfoLst = data as List<BankAccountInfo>;
                    Tuple<List<string>, List<string>> bankAcctInfoTuple = GetBankAcctInfoList(bankAcctInfoLst);
                    ProcessBankAcctInfo(bankAcctInfoTuple);
                }
                if (data is BankAcctDetail)
                {
                    BankAcctDetail bankAcctDetail = data as BankAcctDetail;
                    itemCount++;
                    if (_MainWindow != null)
                    {
                        _MainWindow.OnGotBankAmountDetail(bankAcctDetail);
                    }
                }
                if (data is List<TransferSingleRecord>)
                {
                    transferFlag = true;
                    _TransferRecords.Clear();
                    List<TransferSingleRecord> transferRecordLst = data as List<TransferSingleRecord>;
                    itemCount += transferRecordLst.Count;
                    _TransferRecords.AddRange(transferRecordLst);

                }
                if (data is TransferSingleRecord)
                {
                    transferFlag = true;
                    TransferSingleRecord transferRecord = data as TransferSingleRecord;
                    itemCount++;
                    ProcessNewTransferRecords(transferRecord);
                }
            }
            if (itemCount > 0)
            {
                Util.Log("Dealing with Incoming data: " + itemCount + " items.");
            }
            if (transferFlag)
            {
                ProcessTransferRecords(_TransferRecords);
            }
        }

        private void OrderResponseProcess()
        {
            int itemCount = 0;
            bool orderFlag = false;
            bool execFlag = false;
            bool quoteFlag = false;
            List<object> dataList = _OrderResponseQueue.DequeueAll(_TimeOut);
            foreach (object data in dataList)
            {
                if (data is List<TradeOrderData>)
                {
                    orderFlag = true;
                    List<TradeOrderData> orderLst = data as List<TradeOrderData>;
                    if (orderLst.Count > 0)
                    {
                        RemoveUserOrderRecord(orderLst[0].InvestorID);
                    }
                    itemCount += orderLst.Count;
                    foreach (TradeOrderData jyData in orderLst)
                    {
                        string orderKey = jyData.BrokerOrderSeq + "_" + jyData.OrderRef + "_" + jyData.Exchange;
                        if (_OrderDataDic.ContainsKey(orderKey))
                        {
                            _OrderDataDic[orderKey] = jyData;
                        }
                        else
                        {
                            _OrderDataDic.Add(orderKey, jyData);
                        }
                    }
                }
                if (data is TradeOrderData)
                {
                    orderFlag = true;
                    TradeOrderData jyData = data as TradeOrderData;
                    itemCount++;
                    ProcessNewComeOrderNotice(jyData);
                    string orderKey = jyData.BrokerOrderSeq + "_" + jyData.OrderRef + "_" + jyData.Exchange;
                    if (_OrderDataDic.ContainsKey(orderKey))
                    {
                        if (IsLatestOrderData(_OrderDataDic[orderKey], jyData))
                        {
                            _OrderDataDic[orderKey] = jyData;
                        }
                    }
                    else
                    {
                        _OrderDataDic.Add(orderKey, jyData);
                    }
                }
                if (data is List<ExecOrderData>)
                {
                    execFlag = true;
                    List<ExecOrderData> execLst = data as List<ExecOrderData>;
                    if (execLst.Count > 0)
                    {
                        RemoveUserExecOrderRecord(execLst[0].InvestorID);
                    }
                    itemCount += execLst.Count;
                    foreach (ExecOrderData execData in execLst)
                    {
                        string execKey = execData.BrokerExecOrderSeq + "_" + execData.ExecOrderRef + "_" + execData.Exchange;
                        if (_ExecOrderDataDic.ContainsKey(execKey))
                        {
                            _ExecOrderDataDic[execKey] = execData;
                        }
                        else
                        {
                            _ExecOrderDataDic.Add(execKey, execData);
                        }
                    }
                }
                if (data is ExecOrderData)
                {
                    execFlag = true;
                    ExecOrderData execData = data as ExecOrderData;
                    itemCount++;
                    ProcessNewComeExecOrderNotice(execData);
                    string execKey = execData.BrokerExecOrderSeq + "_" + execData.ExecOrderRef + "_" + execData.Exchange;
                    if (_ExecOrderDataDic.ContainsKey(execKey))
                    {
                        if (IsLatestExecOrderData(_ExecOrderDataDic[execKey], execData))
                        {
                            _ExecOrderDataDic[execKey] = execData;
                        }
                    }
                    else
                    {
                        _ExecOrderDataDic.Add(execKey, execData);
                    }
                    Util.Log(string.Format("OnRtnExecOrder pExecOrder: Code {0}, BrokerExecOrderSeq {1}, ExecOrderSysID {2}, ExecOrderRef {3}, Status {4}",
                        execData.Code, execData.BrokerExecOrderSeq, execData.ExecOrderID, execData.ExecOrderRef, execData.StatusMsg));
                }

                if (data is List<QuoteOrderData>)
                {
                    quoteFlag = true;
                    List<QuoteOrderData> quoteLst = data as List<QuoteOrderData>;
                    if (quoteLst.Count > 0)
                    {
                        RemoveUserQuoteOrderRecord(quoteLst[0].InvestorID);
                    }
                    itemCount += quoteLst.Count;
                    foreach (QuoteOrderData tempData in quoteLst)
                    {
                        string tempKey = tempData.BrokerQuoteSeq + "_" + tempData.QuoteRef + "_" + tempData.Exchange;
                        if (_QuoteOrderDataDic.ContainsKey(tempKey))
                        {
                            _QuoteOrderDataDic[tempKey] = tempData;
                        }
                        else
                        {
                            _QuoteOrderDataDic.Add(tempKey, tempData);
                        }
                    }
                }
                if (data is QuoteOrderData)
                {
                    quoteFlag = true;
                    QuoteOrderData quoteData = data as QuoteOrderData;
                    itemCount++;
                    ProcessNewComeQuoteOrderNotice(quoteData);
                    string quoteKey = quoteData.BrokerQuoteSeq + "_" + quoteData.QuoteRef + "_" + quoteData.Exchange;
                    if (_QuoteOrderDataDic.ContainsKey(quoteKey))
                    {
                        if (IsLatestQuoteOrderData(_QuoteOrderDataDic[quoteKey], quoteData))
                        {
                            _QuoteOrderDataDic[quoteKey] = quoteData;
                        }
                    }
                    else
                    {
                        _QuoteOrderDataDic.Add(quoteKey, quoteData);
                    }
                    Util.Log(string.Format("OnRtnQuote pQuote: Code {0}, BrokerQuoteSeq {1}, QuoteSysID {2}, QuoteRef {3}, Status {4}",
                        quoteData.Code, quoteData.BrokerQuoteSeq, quoteData.QuoteOrderID, quoteData.QuoteRef, quoteData.StatusMsg));
                }
                //else
                //{
                //    Util.Log("Error for Order Response Data!");
                //}
            }
            if (itemCount > 0)
            {
                Util.Log("Dealing with order data: " + itemCount + " items.");
            }
            if (orderFlag)
            {
                ProcessOrderData();
            }
            if (execFlag)
            {
                ProcessExecOrderData();
            }
            if (quoteFlag)
            {
                ProcessQuoteOrderData();
            }
        }

        private void TradeResponseProcess()
        {
            bool tradeFlag = false;
            int itemCount = 0;
            List<object> dataList = _TradeResponseQueue.DequeueAll(_TimeOut);
            foreach (object data in dataList)
            {
                if (data is List<TradeOrderData>)
                {
                    tradeFlag = true;
                    List<TradeOrderData> tradeLst = data as List<TradeOrderData>;
                    if (tradeLst.Count > 0)
                    {
                        RemoveUserTradeRecord(tradeLst[0].InvestorID);
                    }
                    itemCount += tradeLst.Count;
                    foreach (TradeOrderData tradedData in tradeLst)
                    {
                        AddTrededData(tradedData);
                    }
                }
                if (data is TradeOrderData)
                {
                    tradeFlag = true;
                    TradeOrderData tradedData = data as TradeOrderData;
                    itemCount++;
                    AddTrededData(tradedData);
                }
                if (data is CommissionStruct)
                {
                    tradeFlag = true;
                    CommissionStruct tradedCommissionRate = data as CommissionStruct;
                    string tempKey = tradedCommissionRate.Code + "_" + tradedCommissionRate.InvestorID;
                    if (tradedCommissionRate.Code != null & tradedCommissionRate.InvestorID != null)
                    {
                        CommissionDict[tempKey] = tradedCommissionRate;
                    }
                }
                //else
                //{
                //    Util.Log("Error for Trade Response Data!");
                //}
            }
            if (tradeFlag)
            {
                if (itemCount > 0)
                {
                    Util.Log("Dealing with Incoming data: " + itemCount + " items.");
                }
                ProcessTradeData();
            }
        }

        private void PositionResponseProcess()
        {
            int itemCount = 0;
            bool posFlag = false;
            bool posDetailFlag = false;
            List<object> dataList = _PositionResponseQueue.DequeueAll(_TimeOut);
            foreach (object data in dataList)
            {
                if (data is List<PosInfoTotal>)
                {
                    posFlag = true;
                    List<PosInfoTotal> positionLst = data as List<PosInfoTotal>;
                    itemCount += positionLst.Count;
                    if (positionLst.Count > 0)
                    {
                        RemoveUserPositionRecord(positionLst[0].InvestorID);
                    }
                    _PositionSumDataLst.AddRange(positionLst);
                }
                if (data is List<PosInfoDetail>)
                {
                    posDetailFlag = true;
                    List<PosInfoDetail> posDetailLst = data as List<PosInfoDetail>;
                    itemCount += posDetailLst.Count;
                    if (posDetailLst.Count > 0)
                    {
                        RemoveUserPositionRecord(posDetailLst[0].InvestorID);
                    }
                    AddPositionDetailData(posDetailLst);
                }
                if (data is TradeOrderData)
                {
                    posFlag = true;
                    posDetailFlag = true;
                    TradeOrderData tradedData = data as TradeOrderData;
                    itemCount++;
                    ProcessNewComePosInfo(tradedData);
                }
                if (data is MarginStruct)
                {
                    posFlag = true;
                    posDetailFlag = true;
                    MarginStruct posMarginRate = data as MarginStruct;
                    string tempKey = posMarginRate.Code + "_" + posMarginRate.InvestorID;
                    if (posMarginRate.Code != null & posMarginRate.InvestorID != null)
                    {
                        MarginDict[tempKey] = posMarginRate;
                        //ProcessMarginData(tempKey);
                    }
                }
                //else
                //{
                //    Util.Log("Error for Trade Response Data!");
                //}
            }
            if (itemCount > 0)
            {
                Util.Log("Dealing with Incoming data: " + itemCount + " items.");
            }
            if (posDetailFlag && _PositionDetailDataLst.Count > 0)
            {
                ProcessPositionDetailData(_PositionDetailDataLst);
            }
            else if (posFlag && _PositionSumDataLst.Count > 0)
            {
                ProcessPositions_Total(_PositionSumDataLst);
            }
        }

        private void MessageProcess()
        {
            List<object> dataList = _MessageResponseQueue.DequeueAll(_TimeOut);
            foreach (object data in dataList)
            {
                if (data is string)
                {
                    string msg = data.ToString();
                    if (_MainWindow != null)
                    {
                        _MainWindow.AddSystemMessage(DateTime.Now, msg, "信息", "API");
                    }
                }
                if (data is DisconnectStruct)
                {
                    DisconnectStruct disStruct = data as DisconnectStruct;
                    if (disStruct.FrontType == "Trader")
                    {
                        OnDisconnected();
                    }
                    else if (disStruct.FrontType == "Md")
                    {
                        OnQuoteDisconnected();
                    }
                }
            }
        }

        private void TimerCallBack(Object o)
        {
            NotifyTimeChange(_ExTime);
            _ExTime.SecondPass();
        }

        /// <summary>
        /// 通知交易所时间变化
        /// </summary>
        /// <param name="exchangeTime"></param>
        private void NotifyTimeChange(ExchangeTime exchangeTime)
        {
            if (_MainWindow != null && System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    //登录成功后，主窗口连接状态提示信息
                    _MainWindow.uscStatusBar.TimeChange(exchangeTime);
                });
            }
        }

        private void OnLogon(BACKENDTYPE backEnd)
        {
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    _LogWindow.StopWatch();
                    string fileContent = _LogWindow.GetUserNameDatContent();
                    CommonUtil.writeFile(Login.USERNAME_FILEPATH, fileContent);//登录成功后，保存是否记住密码信息
                    _LogWindow.Hide();
                    //登录成功后，主窗口连接状态提示信息
                    if (backEnd != BACKENDTYPE.UNKNOWN && backEnd != BACKENDTYPE.CTP)
                    {
                        GenerateMainWindow();
                    }
                });
            }
        }

        private void OnLogout(string errorMessage)
        {
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    if (!string.IsNullOrWhiteSpace(errorMessage))
                    {
                        _LogWindow.OnLogOut(errorMessage);
                    }
                    if (_MainWindow != null)
                    {
                        _MainWindow.UscStatementsInquiry.txtStatementOrder.Text = "";
                        _MainWindow.SystemMessageCollection.Clear();
                        _MainWindow.CapitalDataCollection.Clear();
                        _MainWindow.ChangeServerConnectionStatus();
                    }
                });
            }
        }

        private void OnQuote()
        {
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    //登录成功后，主窗口连接状态提示信息
                    if (_MainWindow != null)
                    {
                        _MainWindow.ChangeServerConnectionStatus();
                    }
                });
            }
        }

        private void OnDisconnected()
        {
            if (_MainWindow != null && System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    _MainWindow.uscStatusBar.SetJYImageStatus(false);
                    _MainWindow.uscStatusBar.TxnImage.ToolTip = "交易：断开";
                    _MainWindow.ClearEvents();
                });
            }
        }

        private void OnQuoteDisconnected()
        {
            if (_MainWindow != null && System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    _MainWindow.uscStatusBar.SetHQImageStatus(false);
                    _MainWindow.uscStatusBar.HQImage.ToolTip = "行情：断开";
                });
            }
        }

        public void GenerateMainWindow()
        {
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    if (_MainWindow == null)
                    {
                        _MainWindow = new MainWindow();
                    }
                    _MainWindow.ChangeServerConnectionStatus();
                    _MainWindow.Show();

                    _MainWindow.uscHangqing.Init(_MainWindow);
                    _MainWindow.uscOptionHangqing.Init(_MainWindow);
                    _MainWindow.InitQuotesEvents();
                });
            }
            foreach (object serverItem in _DataServerLst)
            {
                if (serverItem is CtpDataServer)
                {
                    (serverItem as CtpDataServer).setMainWindow(_MainWindow);
                }
                else if (serverItem is FemasDataServer)
                {
                    //(serverItem as FemasDataServer).setMainWindow(_MainWindow);
                }
            }
        }

        private bool IsLatestOrderData(TradeOrderData orgOrderData, TradeOrderData laterOrderData)
        {
            if (CommonUtil.IsCancellable(laterOrderData))
            {
                return IsNewStatus(orgOrderData.OrderStatus, laterOrderData.OrderStatus);
            }
            return true;
        }

        private void ProcessNewComeOrderNotice(TradeOrderData order)
        {
            if (order == null)
            {
                return;
            }
            bool isNotice = true;
            string orderId = order.OrderID;
            string orderKey = order.OrderID + "_" + order.OrderRef + "_" + order.Exchange;

            if (_OrderDataDic.ContainsKey(orderKey) && order.OrderStatus == _OrderDataDic[orderKey].OrderStatus && order.OrderStatus == "未成交")// && item.OrderID == order.OrderID && order.OrderID != "")
            {
                isNotice = false;
            }
            if (isNotice)
            {
                if (CommonUtil.GetOrderStatus(order.OrderStatus) == OrderStatus.Queued)
                {
                    if (!_CommitOrders.Contains(orderId))
                    {
                        _CommitOrders.Add(orderId);
                        if (_MainWindow != null)
                        {
                            _MainWindow.HandleOrderInfo(order);
                        }
                    }
                }
                if (CommonUtil.GetOrderStatus(order.OrderStatus) == OrderStatus.Chengjiao)
                {
                    if (!_TransactOrders.Contains(orderId))
                    {
                        //showOrder
                        _TransactOrders.Add(orderId);
                        if (_MainWindow != null)
                        {
                            _MainWindow.HandleOrderInfo(order);
                        }
                    }
                }
                if (CommonUtil.GetOrderStatus(order.OrderStatus) == OrderStatus.Cancelled)
                {
                    if (!_CancelOrders.Contains(orderId) || string.IsNullOrEmpty(orderId))
                    {
                        //showOrder
                        _CancelOrders.Add(orderId);
                        if (_MainWindow != null)
                        {
                            _MainWindow.HandleOrderInfo(order);
                        }
                    }
                }
                if (CommonUtil.GetOrderStatus(order.OrderStatus) == OrderStatus.Failed)
                {
                    if (!_CancelOrders.Contains(orderId) || string.IsNullOrEmpty(orderId))
                    {
                        //showOrder
                        _CancelOrders.Add(orderId);
                        if (_MainWindow != null)
                        {
                            _MainWindow.HandleOrderInfo(order);
                        }
                    }
                }
            }
        }

        private void ProcessOrderData()
        {
            List<TradeOrderData> orderDataLst = new List<TradeOrderData>();
            Dictionary<string, object> userInstanceDict = new Dictionary<string, object>();
            foreach (string orderKey in _OrderDataDic.Keys)
            {
                orderDataLst.Add(_OrderDataDic[orderKey]);

                BACKENDTYPE backEnd = _OrderDataDic[orderKey].BackEnd;
                string tempKey = _OrderDataDic[orderKey].InvestorID + "_" + _OrderDataDic[orderKey].BackEnd.ToString();
                if (!userInstanceDict.ContainsKey(tempKey))
                {
                    userInstanceDict.Add(tempKey, GetDataServerInstance(_OrderDataDic[orderKey].InvestorID, _OrderDataDic[orderKey].BackEnd));
                }

                _MultiUsersOrderDataDic.Clear();
                string userKey = _OrderDataDic[orderKey] == null ? "" : _OrderDataDic[orderKey].InvestorID;
                if (userKey == null)
                {
                    Util.Log("Error for Trade Response Data: InvestorID is null!");
                    continue;
                }
                if (_MultiUsersOrderDataDic.ContainsKey(userKey))
                {
                    _MultiUsersOrderDataDic[userKey].Add(_OrderDataDic[orderKey]);
                }
                else if (userKey.Trim() != "")
                {
                    _MultiUsersOrderDataDic.Add(userKey, new List<TradeOrderData>() { _OrderDataDic[orderKey] });
                }
            }
            orderDataLst.Sort(TradeOrderData.CompareByCommitTime);
            ProcessJYOrderData(orderDataLst);
            foreach (string uKey in userInstanceDict.Keys)
            {
                if (userInstanceDict[uKey] != null)
                {
                    RequestCapitalInfo(userInstanceDict[uKey]);
                }
            }
        }

        /// <summary>
        /// 处理委托查询
        /// </summary>
        /// <param name="jyOrderData"></param>
        private void ProcessJYOrderData(List<TradeOrderData> jyOrderData)
        {
            if (_MainWindow != null && System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    try
                    {
                        ///成交单在ProcessTransactJYOrderData中处理
                        _MainWindow.OrderDataCollection.Clear();
                        _MainWindow.PendingCollection.Clear();
                        _MainWindow.CancelledOrderData.Clear();
                        _MainWindow.TradedOrderCollection.Clear();

                        _FreezeOrder.Clear();
                        foreach (PosInfoTotal posInfoTotal in _MainWindow.PositionCollection_Total)
                        {
                            posInfoTotal.FreezeCount = 0;
                            string key = posInfoTotal.InvestorID + posInfoTotal.Code + posInfoTotal.BuySell.Contains("买").ToString() + posInfoTotal.Hedge;
                            if (!_FreezeOrder.ContainsKey(key))
                            {
                                _FreezeOrder.Add(key, new List<TradeOrderData>());
                            }
                            if (posInfoTotal.BuySell.Contains("买") && posInfoTotal.ProductType.Contains("Option") && _FreezeExecOrderHandCount.ContainsKey(key))
                            {
                                posInfoTotal.CanCloseCount = posInfoTotal.TotalPosition - _FreezeExecOrderHandCount[key];
                            }
                            else
                            {
                                posInfoTotal.CanCloseCount = posInfoTotal.TotalPosition;
                            }
                        }

                        foreach (TradeOrderData orderData in jyOrderData)
                        {
                            _MainWindow.OrderDataCollection.Add(orderData);
                            string posTotalInfo = orderData.InvestorID + orderData.Code + (!orderData.BuySell.Contains("买")).ToString() + orderData.Hedge;
                            if (orderData.OrderStatus.Contains("未成交") || orderData.OrderStatus.Contains("部分成交") || orderData.OrderStatus.Contains("未触发"))
                            {
                                _MainWindow.PendingCollection.Add(orderData);
                                if (orderData.OpenClose.Contains("开") == false)
                                {
                                    //对于平仓
                                    if (PosTotalInfoMap.ContainsKey(posTotalInfo))
                                    {
                                        PosTotalInfoMap[posTotalInfo].FreezeCount += orderData.UnTradeHandCount;
                                        if (orderData.OrderStatus.Contains("未成交") || orderData.OrderStatus.Contains("部分成交"))
                                        {
                                            PosTotalInfoMap[posTotalInfo].CanCloseCount -= orderData.UnTradeHandCount;//?
                                        }
                                    }

                                    if (_FreezeOrder.ContainsKey(posTotalInfo))
                                    {
                                        List<TradeOrderData> orderList = _FreezeOrder[posTotalInfo];
                                        orderList.Add(orderData);
                                    }
                                }
                            }
                            if (orderData.OrderStatus.Contains("已撤单") || orderData.OrderStatus.Contains("已撤余单") || orderData.OrderStatus.Contains("拒绝"))
                            {
                                _MainWindow.CancelledOrderData.Add(orderData);
                            }
                            if (orderData.OrderStatus.Contains("全部成交") || orderData.OrderStatus.Contains("部分成交"))
                            {
                                _MainWindow.TradedOrderCollection.Add(orderData);
                            }
                        }
                        _MainWindow.uscNewOrderPanel.SetHandCount();
                    }
                    catch (Exception ex)
                    {
                        Util.Log_Error("exception: " + ex.Message);
                        Util.Log_Error("exception: " + ex.StackTrace);
                    }
                });
            }
        }

        private void RequestCapitalInfo(object dataServer)
        {
            if (dataServer is CtpDataServer)
            {
                CtpDataServer ctpServer = dataServer as CtpDataServer;
                ctpServer.RequestCapital();
            }
            else if (dataServer is FemasDataServer)
            {

            }
        }

        private bool IsLatestQuoteOrderData(QuoteOrderData orgQuoteOrder, QuoteOrderData laterQuoteOrder)
        {
            if (CommonUtil.IsQuoteCancellable(laterQuoteOrder))
            {
                return IsNewStatus(orgQuoteOrder.QuoteStatus, laterQuoteOrder.QuoteStatus);
            }
            return true;
        }

        private bool IsNewStatus(string orgStatus, string laterStatus)
        {
            return CommonUtil.GetOrderStatus(orgStatus) < CommonUtil.GetOrderStatus(laterStatus);
        }

        private void ProcessNewComeQuoteOrderNotice(QuoteOrderData qOrder)
        {
            if (qOrder == null)
            {
                return;
            }
            bool isNotice = true;
            string quoteID = qOrder.QuoteOrderID;
            string orderKey = qOrder.QuoteOrderID + "_" + qOrder.QuoteRef + "_" + qOrder.Exchange;

            if (_QuoteOrderDataDic.ContainsKey(orderKey) && qOrder.QuoteStatus == _QuoteOrderDataDic[orderKey].QuoteStatus && qOrder.QuoteStatus == "未成交")// && item.OrderID == order.OrderID && order.OrderID != "")
            {
                isNotice = false;
            }
            if (isNotice)
            {
                if (CommonUtil.GetOrderStatus(qOrder.QuoteStatus) == OrderStatus.Queued)
                {
                    if (!_CommitOrders.Contains(quoteID))
                    {
                        _CommitOrders.Add(quoteID);
                        if (_MainWindow != null)
                        {
                            _MainWindow.HandleOrderInfo(qOrder);
                        }
                    }
                }
                if (CommonUtil.GetOrderStatus(qOrder.QuoteStatus) == OrderStatus.Chengjiao)
                {
                    if (!_TransactOrders.Contains(quoteID))
                    {
                        //showOrder
                        _TransactOrders.Add(quoteID);
                        if (_MainWindow != null)
                        {
                            _MainWindow.HandleOrderInfo(qOrder);
                        }
                    }
                }
                if (CommonUtil.GetOrderStatus(qOrder.QuoteStatus) == OrderStatus.Cancelled)
                {
                    if (!_CancelOrders.Contains(quoteID) || string.IsNullOrEmpty(quoteID))
                    {
                        //showOrder
                        _CancelOrders.Add(quoteID);
                        if (_MainWindow != null)
                        {
                            _MainWindow.HandleOrderInfo(qOrder);
                        }
                    }
                }
                if (CommonUtil.GetOrderStatus(qOrder.QuoteStatus) == OrderStatus.Failed)
                {
                    if (!_CancelOrders.Contains(quoteID) || string.IsNullOrEmpty(quoteID))
                    {
                        //showOrder
                        _CancelOrders.Add(quoteID);
                        if (_MainWindow != null)
                        {
                            _MainWindow.HandleOrderInfo(qOrder);
                        }
                    }
                }
            }
        }

        private void ProcessQuoteOrderData()
        {
            List<QuoteOrderData> quoteOrderDataLst = new List<QuoteOrderData>();
            foreach (string orderKey in _QuoteOrderDataDic.Keys)
            {
                quoteOrderDataLst.Add(_QuoteOrderDataDic[orderKey]);

                _MultiUsersQuoteOrderDataDic.Clear();
                string userKey = _QuoteOrderDataDic[orderKey] == null ? "" : _QuoteOrderDataDic[orderKey].InvestorID;
                if (userKey == null)
                {
                    Util.Log("Error for Trade Response Data: InvestorID is null!");
                    continue;
                }
                if (_MultiUsersQuoteOrderDataDic.ContainsKey(userKey))
                {
                    _MultiUsersQuoteOrderDataDic[userKey].Add(_QuoteOrderDataDic[orderKey]);
                }
                else if (userKey.Trim() != "")
                {
                    _MultiUsersQuoteOrderDataDic.Add(userKey, new List<QuoteOrderData>() { _QuoteOrderDataDic[orderKey] });
                }
            }
            quoteOrderDataLst.Sort(QuoteOrderData.CompareByCommitTime);
            ProcessQuoteInsertOrderData(quoteOrderDataLst);
        }

        /// <summary>
        /// 处理报价查询
        /// </summary>
        /// <param name="jyorderData"></param>
        private void ProcessQuoteInsertOrderData(List<QuoteOrderData> qOrderDataLst)
        {
            if (_MainWindow != null && System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    try
                    {
                        _MainWindow.QuoteOrderDataCollection.Clear();
                        _MainWindow.QuotePendingOrderDataCollection.Clear();
                        _MainWindow.QuoteCancelledOrderDataCollection.Clear();
                        _MainWindow.QuoteTradedOrderDataCollection.Clear();

                        foreach (QuoteOrderData qOrderData in qOrderDataLst)
                        {
                            _MainWindow.QuoteOrderDataCollection.Add(qOrderData);
                            if (qOrderData.QuoteStatus.Contains("未成交") || (qOrderData.QuoteStatus.Contains("部分成交")) || (qOrderData.QuoteStatus.Contains("未触发")))
                            {
                                _MainWindow.QuotePendingOrderDataCollection.Add(qOrderData);
                            }
                            if (qOrderData.QuoteStatus.Contains("已撤单") || qOrderData.QuoteStatus.Contains("已撤余单") || qOrderData.QuoteStatus.Contains("拒绝"))
                            {
                                _MainWindow.QuoteCancelledOrderDataCollection.Add(qOrderData);
                            }
                            if (qOrderData.QuoteStatus.Contains("全部成交"))
                            {
                                _MainWindow.QuoteTradedOrderDataCollection.Add(qOrderData);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.Log_Error("exception: " + ex.Message);
                        Util.Log_Error("exception: " + ex.StackTrace);
                    }
                });
            }
        }

        private bool IsLatestExecOrderData(ExecOrderData orgExecOrder, ExecOrderData laterExecOrder)
        {
            if (CommonUtil.IsExecCancellable(laterExecOrder))
            {
                return IsNewStatus(orgExecOrder.ExecStatus, laterExecOrder.ExecStatus);
            }
            return true;
        }

        private void ProcessNewComeExecOrderNotice(ExecOrderData execOrder)
        {
            if (execOrder == null)
            {
                return;
            }
            bool isNotice = true;
            string execID = execOrder.ExecOrderID;
            //string code = order.legs[0] != null ? order.legs[0].Code : "";
            string code = execOrder.Code;
            string orderKey = execOrder.ExecOrderID + "_" + execOrder.ExecOrderRef + "_" + execOrder.Exchange;

            if (_ExecOrderDataDic.ContainsKey(orderKey) && execOrder.ExecStatus == _ExecOrderDataDic[orderKey].ExecStatus && CommonUtil.IsExecCancellable(execOrder))// && item.OrderID == order.OrderID && order.OrderID != "")
            {
                isNotice = false;
            }
            if (isNotice)
            {
                if (CommonUtil.IsExecCancellable(execOrder))
                {
                    if (!_CommitOrders.Contains(execID))
                    {
                        _CommitOrders.Add(execID);
                        if (_MainWindow != null)
                        {
                            _MainWindow.HandleOrderInfo(execOrder);
                        }
                    }
                }
                if (execOrder.ExecStatus == "执行成功")
                {
                    if (!_TransactOrders.Contains(execID))
                    {
                        //showOrder
                        _TransactOrders.Add(execID);
                        if (_MainWindow != null)
                        {
                            _MainWindow.HandleOrderInfo(execOrder);
                        }
                    }
                }
                if (execOrder.ExecStatus == "已取消")
                {
                    if (!_CancelOrders.Contains(execID) || string.IsNullOrEmpty(execID))
                    {
                        //showOrder
                        _CancelOrders.Add(execID);
                        if (_MainWindow != null)
                        {
                            _MainWindow.HandleOrderInfo(execOrder);
                        }
                    }
                }
                if (execOrder.ExecStatus == "申请失败" || execOrder.ExecStatus == "已拒绝")
                {
                    if (!_CancelOrders.Contains(execID) || string.IsNullOrEmpty(execID))
                    {
                        //showOrder
                        _CancelOrders.Add(execID);
                        if (_MainWindow != null)
                        {
                            _MainWindow.HandleOrderInfo(execOrder);
                        }
                    }
                }
            }
        }

        private void ProcessExecOrderData()
        {
            List<ExecOrderData> execOrderDataLst = new List<ExecOrderData>();
            foreach (string orderKey in _ExecOrderDataDic.Keys)
            {
                execOrderDataLst.Add(_ExecOrderDataDic[orderKey]);

                _MultiUsersExecOrderDataDic.Clear();
                string userKey = _ExecOrderDataDic[orderKey] == null ? "" : _ExecOrderDataDic[orderKey].InvestorID;
                if (userKey == null)
                {
                    Util.Log("Error for Trade Response Data: InvestorID is null!");
                    continue;
                }
                if (_MultiUsersOrderDataDic.ContainsKey(userKey))
                {
                    _MultiUsersExecOrderDataDic[userKey].Add(_ExecOrderDataDic[orderKey]);
                }
                else if (userKey.Trim() != "")
                {
                    _MultiUsersExecOrderDataDic.Add(userKey, new List<ExecOrderData>() { _ExecOrderDataDic[orderKey] });
                }
            }
            execOrderDataLst.Sort(ExecOrderData.CompareByCommitTime);
            ProcessExecInsertOrderData(execOrderDataLst);
        }

        /// <summary>
        /// 处理行权查询
        /// </summary>
        /// <param name="jyorderData"></param>
        private void ProcessExecInsertOrderData(List<ExecOrderData> execOrderDataLst)
        {
            if (_MainWindow != null && System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    try
                    {
                        _MainWindow.ExecOrderDataCollection.Clear();
                        _MainWindow.ExecOrderCancelledDataCollection.Clear();
                        _MainWindow.ExecOrderPendingDataCollection.Clear();

                        _FreezeExecOrderHandCount.Clear();
                        foreach (PosInfoTotal posInfoTotal in _MainWindow.PositionCollection_Total)
                        {
                            if (posInfoTotal.BuySell.Contains("买") && posInfoTotal.ProductType.Contains("Option"))
                            {
                                //_FreezeExecOrder.Add(posInfoTotal.Code + true.ToString(), new List<ExecOrderData>());
                                _FreezeExecOrderHandCount.Add(posInfoTotal.InvestorID + posInfoTotal.Code + true.ToString(), 0);
                            }
                        }

                        foreach (ExecOrderData execOrder in execOrderDataLst)
                        {
                            _MainWindow.ExecOrderDataCollection.Add(execOrder);

                            //对于未执行期权
                            string execTotalInfo = execOrder.InvestorID + execOrder.Code + true.ToString();
                            if (execOrder.ExecStatus.Contains("未执行"))
                            {
                                _MainWindow.ExecOrderPendingDataCollection.Add(execOrder);
                                if (_FreezeExecOrderHandCount.ContainsKey(execTotalInfo))
                                {
                                    _FreezeExecOrderHandCount[execTotalInfo] += execOrder.HandCount;
                                }
                            }
                            else if (execOrder.ExecStatus.Contains("已取消") || execOrder.ExecStatus.Contains("申请失败") || execOrder.ExecStatus.Contains("已拒绝"))
                            {
                                _MainWindow.ExecOrderCancelledDataCollection.Add(execOrder);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.Log_Error("exception: " + ex.Message);
                        Util.Log_Error("exception: " + ex.StackTrace);
                    }
                });
            }
        }

        private void AddTrededData(TradeOrderData tradedData)
        {
            _TradeDataLst.Add(tradedData);
            InitCommissionRate(CodeSetManager.GetContractInfo(tradedData.Code, CodeSetManager.ExNameToCtp(tradedData.Exchange)), tradedData.InvestorID, tradedData.BackEnd);
            string userKey = tradedData.InvestorID;
            if (_MultiUsersTradeDataDic.ContainsKey(userKey))
            {
                _MultiUsersTradeDataDic[userKey].Add(tradedData);
            }
            else if (userKey.Trim() != "")
            {
                _MultiUsersTradeDataDic.Add(userKey, new List<TradeOrderData>() { tradedData });
            }
        }

        private void ProcessTradeData()
        {
            _TradeDataLst.Sort(TradeOrderData.CompareByTradeTime);
            ProcessTradedOrderData(_TradeDataLst);
            //lock (ServerLock)
            //{
            //    _PositionDetailDataLst.Sort(PosInfoDetail.CompareByExecID);
            //    ProcessPositions(_PositionDetailDataLst);
            //}
            //_TradeProcessingFlag = false;
        }

        /// <summary>
        /// 处理成交数据
        /// </summary>
        /// <param name="jyOrderData"></param>
        protected void ProcessTradedOrderData(List<TradeOrderData> jyOrderData)
        {
            if (_MainWindow != null && System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    try
                    {
                        _MainWindow.TradeCollection_MX.Clear();
                        foreach (TradeOrderData tradedData in jyOrderData)
                        {
                            if (tradedData.Code != null & tradedData.InvestorID != null)
                            {
                                string tempKey = CodeSetManager.GetContractInfo(tradedData.Code, CodeSetManager.ExNameToCtp(tradedData.Exchange)).SpeciesCode + "_" + tradedData.InvestorID;
                                ProcessCommissionData(tempKey, tradedData);
                            }
                            _MainWindow.TradeCollection_MX.Add(tradedData);
                        }
                        CodeCollection();
                    }
                    catch (Exception ex)
                    {
                        Util.Log_Error("exception: " + ex.Message);
                        Util.Log_Error("exception: " + ex.StackTrace);
                    }
                }
                );
            }
        }

        /// <summary>
        /// 处理成交数据，按合约汇总
        /// </summary>
        private void CodeCollection()
        {
            Dictionary<string, TradeOrderData> dicCode = new Dictionary<string, TradeOrderData>();
            dicCode.Clear();
            List<TradeOrderData> tradeSumLst = new List<TradeOrderData>();
            _MainWindow.TradeCollection_Code.Clear();

            foreach (TradeOrderData MXItem in _MainWindow.TradeCollection_MX)
            {
                string tempkey = MXItem.InvestorID + "_" + MXItem.Code + "_" + MXItem.BuySell + "_" + MXItem.OpenClose + "_" + MXItem.Hedge;
                if (dicCode.ContainsKey(tempkey))
                {
                    TradeOrderData codeItem = dicCode[tempkey];
                    codeItem.AvgPx = (codeItem.AvgPx * codeItem.TradeHandCount + MXItem.AvgPx * MXItem.TradeHandCount) / (MXItem.TradeHandCount + codeItem.TradeHandCount);
                    codeItem.TradeHandCount += MXItem.TradeHandCount;
                    codeItem.Charge += MXItem.Charge;
                }
                else
                {
                    TradeOrderData MXItem_rev = MXItem.Copy();
                    tradeSumLst.Add(MXItem_rev);
                    dicCode.Add(tempkey, MXItem_rev);
                }
            }
            tradeSumLst.Sort(TradeOrderData.CompareByCode);
            foreach (TradeOrderData item in tradeSumLst)
            {
                _MainWindow.TradeCollection_Code.Add(item);
            }
        }

        private void InitCommissionRate(Contract codeInfo, string investorID, BACKENDTYPE backEnd)
        {
            string tempKey = codeInfo.SpeciesCode + "_" + investorID;
            if (tempKey == "") return;
            if (!CommissionDict.ContainsKey(tempKey))
            {
                CommissionDict.Add(codeInfo.SpeciesCode + "_" + investorID, new CommissionStruct());
                object dataServer = GetDataServerInstance(investorID, backEnd);
                if (dataServer is CtpDataServer)
                {
                    CtpDataServer ctpServer = dataServer as CtpDataServer;
                    ctpServer.InitiateCommissionRate(codeInfo); //To do
                }
                else if (dataServer is FemasDataServer)
                { }
            }
        }

        private void AddPositionDetailData(List<PosInfoDetail> posDataLst)
        {
            //_PositionDetailDataLst.AddRange(posDataLst);
            foreach (PosInfoDetail posData in posDataLst)
            {
                if (posData != null)
                {
                    _PositionDetailDataLst.Add(posData);
                }
            }
        }

        private void ProcessNewComePosInfo(TradeOrderData trade)
        {
            //Add for GHS 5.0.2
            if (trade == null)
            {
                return;
            }

            if (trade.BuySell.Contains("买"))
            {
                if (trade.OpenClose.Contains("开"))
                {
                    PosInfoDetail newPosItem = new PosInfoDetail();
                    newPosItem.InvestorID = trade.InvestorID;
                    newPosItem.ExecID = trade.TradeID;
                    newPosItem.Code = trade.Code;
                    newPosItem.BuySell = trade.BuySell;
                    newPosItem.TradeHandCount = trade.TradeHandCount;
                    newPosItem.AvgPx = trade.AvgPx;
                    newPosItem.PositionType = "今仓";
                    newPosItem.Exchange = trade.Exchange;
                    newPosItem.OccupyMarginAmt = "-";
                    newPosItem.BackEnd = trade.BackEnd;
                    newPosItem.Hedge = trade.Hedge;
                    Contract contract = CodeSetManager.GetContractInfo(trade.Code, CodeSetManager.ExNameToCtp(trade.Exchange));
                    if (contract != null)
                    {
                        newPosItem.Name = contract.Name;
                        newPosItem.ProductType = contract.ProductType;
                    }
                    AddPosInfoDetail(newPosItem);
                }
                else if (trade.OpenClose.Contains("平"))
                {
                    RemovePositionDetailFromTrade(trade);
                }
            }
            else if (trade.BuySell.Contains("卖"))
            {
                if (trade.OpenClose.Contains("开"))
                {
                    PosInfoDetail newPosItem = new PosInfoDetail();
                    newPosItem.InvestorID = trade.InvestorID;
                    newPosItem.ExecID = trade.TradeID;
                    newPosItem.Code = trade.Code;
                    newPosItem.BuySell = trade.BuySell;
                    newPosItem.TradeHandCount = trade.TradeHandCount;
                    newPosItem.AvgPx = trade.AvgPx;
                    newPosItem.PositionType = "今仓";
                    newPosItem.Exchange = trade.Exchange;
                    newPosItem.OccupyMarginAmt = "-";
                    newPosItem.BackEnd = trade.BackEnd;
                    newPosItem.Hedge = trade.Hedge;
                    Contract contract = CodeSetManager.GetContractInfo(trade.Code, CodeSetManager.ExNameToCtp(trade.Exchange));
                    if (contract != null)
                    {
                        newPosItem.Name = contract.Name;
                        newPosItem.ProductType = contract.ProductType;
                    }
                    AddPosInfoDetail(newPosItem);
                }
                else if (trade.OpenClose.Contains("平"))
                {
                    RemovePositionDetailFromTrade(trade);
                }
            }
        }

        /// <summary>
        /// 添加一条持仓明细
        /// </summary>
        /// <param name="posInfoDetail"></param>
        private void AddPosInfoDetail(PosInfoDetail posInfoDetail)
        {
            if (posInfoDetail == null) return;
            _PositionDetailDataLst.Add(posInfoDetail);
            PosHandCount posHandCount = null;
            if (_PosHandCountMap.ContainsKey(posInfoDetail.Code))
            {
                //存在该品种了
                posHandCount = _PosHandCountMap[posInfoDetail.Code];

            }
            else
            {
                //不存在该品种'
                posHandCount = new PosHandCount();
                _PosHandCountMap[posInfoDetail.Code] = posHandCount;
            }

            if (posInfoDetail._PositionType == "今仓")
            {
                //今仓
                if (posInfoDetail.BuySell.Contains("买"))
                {
                    posHandCount.TodayHandCountBuy += posInfoDetail.TradeHandCount;
                }
                else
                {
                    posHandCount.TodayHandCountSell += posInfoDetail.TradeHandCount;
                }
            }
            else
            {
                //昨仓
                if (posInfoDetail.BuySell.Contains("买"))
                {
                    posHandCount.LastDayHandCountBuy += posInfoDetail.TradeHandCount;
                }
                else
                {
                    posHandCount.LastDayHandCountSell += posInfoDetail.TradeHandCount;
                }
            }
        }

        /// <summary>
        /// 移除一条持仓明细
        /// </summary>
        /// <param name="tradedInfo"></param>
        private Boolean RemovePositionDetailFromTrade(TradeOrderData tradedInfo)
        {
            if (tradedInfo == null) return false;
            if (!tradedInfo.OpenClose.Contains("平")) return false;
            //买平还是卖平
            Boolean isBuy = tradedInfo.BuySell.Contains("买") ? true : false;
            //平昨还是平今
            Boolean isCloseYesterday = (tradedInfo.OpenClose.Contains("平仓") || tradedInfo.OpenClose.Contains("平昨"));
            //是否是模拟的
            //Boolean isSim = IsSim();
            //是否是上海的
            bool isShanghai = CodeSetManager.IsCloseTodaySupport(tradedInfo.Code);
            bool isCffexRule = CodeSetManager.IsCffexCloseRule(tradedInfo.Code);
            List<PosInfoDetail> posInfoToDelete = new List<PosInfoDetail>();
            int yesterdayPosCount = 0;
            int todayPosCount = 0;
            ///对于持仓明细中所有的记录，选出符合被平仓条件的记录
            //_ServerMutex.WaitOne();
            foreach (PosInfoDetail posInfoDetail in _PositionDetailDataLst)
            {
                if (posInfoDetail == null) continue;
                if (posInfoDetail.Code != tradedInfo.Code) continue;
                if (posInfoDetail.BuySell.Contains("买") && isBuy == true) continue;
                if (posInfoDetail.BuySell.Contains("卖") && isBuy == false) continue;

                if (isShanghai)
                {
                    if (isCloseYesterday && posInfoDetail.PositionType == "昨仓")
                    {
                        posInfoToDelete.Insert(0, posInfoDetail);
                        yesterdayPosCount += 1;
                    }
                    else if (!isCloseYesterday && posInfoDetail.PositionType == "今仓")
                    {
                        posInfoToDelete.Insert(yesterdayPosCount, posInfoDetail);
                    }
                }
                else if (isCffexRule)
                {
                    if (posInfoDetail.PositionType == "今仓")
                    {
                        posInfoToDelete.Insert(0, posInfoDetail);
                        todayPosCount += 1;
                    }
                    else if (posInfoDetail.PositionType == "昨仓")
                    {
                        posInfoToDelete.Insert(todayPosCount, posInfoDetail);
                    }
                }
                else
                {
                    if (posInfoDetail.PositionType == "昨仓")
                    {
                        posInfoToDelete.Insert(0, posInfoDetail);
                        yesterdayPosCount += 1;
                    }
                    else if (posInfoDetail.PositionType == "今仓")
                    {
                        posInfoToDelete.Insert(yesterdayPosCount, posInfoDetail);
                    }
                }
                
            }

            if (posInfoToDelete.Count == 0)
            {
                //如果是平仓，但是没有仓位可以平，说明仓位的成交回报还没有收到。
                //Util.Log("想要平仓，但是其仓位的开仓成交回报还没有收到。平仓成交记录:" + orderInfo);
            }
            int totalHandCount = tradedInfo.TradeHandCount;
            //平仓posInfoToDelete中的内容
            foreach (PosInfoDetail posInfoDetail in posInfoToDelete)
            {
                if (totalHandCount <= 0) break;
                if (posInfoDetail.TradeHandCount > totalHandCount)
                {
                    posInfoDetail.TradeHandCount -= totalHandCount;
                    /////////////////////////////////////更新手数
                    if (_PosHandCountMap.ContainsKey(posInfoDetail.Code))
                    {
                        PosHandCount posHandCount = _PosHandCountMap[posInfoDetail.Code];
                        if (posInfoDetail.PositionType == "昨仓")
                        {
                            if (posInfoDetail.BuySell == "买")
                            {
                                posHandCount.LastDayHandCountBuy -= totalHandCount;
                            }
                            else
                            {
                                posHandCount.LastDayHandCountSell -= totalHandCount;
                            }
                        }
                        else if (posInfoDetail.PositionType == "今仓")
                        {
                            if (posInfoDetail.BuySell == "买")
                            {
                                posHandCount.TodayHandCountBuy -= totalHandCount;
                            }
                            else
                            {
                                posHandCount.TodayHandCountSell -= totalHandCount;
                            }
                        }
                    }
                    ///////////////////////////////////////////////
                    break;
                }
                else
                {
                    /////////////////////////////////////更新手数
                    if (_PosHandCountMap.ContainsKey(posInfoDetail.Code))
                    {
                        PosHandCount posHandCount = _PosHandCountMap[posInfoDetail.Code];
                        if (posInfoDetail.PositionType == "昨仓")
                        {
                            if (posInfoDetail.BuySell == "买")
                            {
                                posHandCount.LastDayHandCountBuy -= posInfoDetail.TradeHandCount;
                            }
                            else
                            {
                                posHandCount.LastDayHandCountSell -= posInfoDetail.TradeHandCount;
                            }
                        }
                        else if (posInfoDetail.PositionType == "今仓")
                        {
                            if (posInfoDetail.BuySell == "买")
                            {
                                posHandCount.TodayHandCountBuy -= posInfoDetail.TradeHandCount;
                            }
                            else
                            {
                                posHandCount.TodayHandCountSell -= posInfoDetail.TradeHandCount;
                            }
                        }
                        if (posHandCount.LastDayHandCountBuy == 0 && posHandCount.LastDayHandCountSell == 0 && posHandCount.TodayHandCountBuy == 0 && posHandCount.TodayHandCountSell == 0)
                        {
                            _PosHandCountMap.Remove(posInfoDetail.Code);
                        }
                    }
                    ///////////////////////////////////////////////
                    totalHandCount -= posInfoDetail.TradeHandCount;
                    _PositionDetailDataLst.Remove(posInfoDetail);
                }
            }
            //_ServerMutex.ReleaseMutex();
            return true;
        }

        private void ProcessPositionDetailData(List<PosInfoDetail> posDetailLst)
        {
            posDetailLst.Sort(PosInfoDetail.CompareByExecID);
            ProcessPositions(posDetailLst);
        }

        /// <summary>
        /// 处理持仓数据，明细
        /// </summary>
        /// <param name="posInfoDetail"></param>
        protected void ProcessPositions(List<PosInfoDetail> posInfoDetail)
        {
            if (_MainWindow != null && System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    try
                    {
                        _MainWindow.PositionDetailCollection.Clear();
                        List<Contract> lstCodeInfo = new List<Contract>();
                        _MultiUsersPosDetailDataDic.Clear();

                        foreach (PosInfoDetail record in posInfoDetail)
                        {
                            if (record.TradeHandCount == 0)
                                continue;

                            if (record != null)
                            {
                                string userKey = record.InvestorID;
                                if (_MultiUsersPosDetailDataDic.ContainsKey(userKey))
                                {
                                    _MultiUsersPosDetailDataDic[userKey].Add(record);
                                }
                                else if (userKey.Trim() != "")
                                {
                                    _MultiUsersPosDetailDataDic.Add(userKey, new List<PosInfoDetail>() { record });
                                }
                            }

                            Contract codeInfo = CodeSetManager.GetContractInfo(record.Code, CodeSetManager.ExNameToCtp(record.Exchange));
                            InitMarginRate(codeInfo, record, record.BackEnd);

                            _MainWindow.PositionDetailCollection.Add(record);
                            if (!lstCodeInfo.Contains(codeInfo))
                            {
                                lstCodeInfo.Add(codeInfo);
                            }
                        }

                        _PositionSumDataLst.Clear();
                        foreach (PosInfoDetail posDetail in posInfoDetail)
                        {
                            if (posDetail.TradeHandCount != 0)
                            {
                                PosInfoTotal posSum = PosExecutionReport(posDetail);
                                _PositionSumDataLst.Add(posSum);
                            }
                        }
                        _PositionSumDataLst.Sort(PosInfoTotal.CompareByCode);
                        ProcessPositions_Total(_PositionSumDataLst);
                        foreach (var item in _MainWindow.PositionCollection_Total)
                        {
                            Contract codeInfo = CodeSetManager.GetContractInfo(item.Code, CodeSetManager.ExNameToCtp(item.Exchange));
                            lstCodeInfo.Add(codeInfo);
                        }
                        //mainWindow.HQBackgroundRealData.UpdateFDYKForPositions();
                        _MainWindow.HQBackgroundRealData.AddContract(lstCodeInfo.ToArray());
                        _MainWindow.HQBackgroundRealData.GetRealData();
                        _MainWindow.HQBackgroundRealData.Request();
                        _MainWindow.HQBackgroundRealData.RemoveUselessRequest();
                    }
                    catch (Exception ex)
                    {
                        Util.Log_Error("exception: " + ex.Message);
                        Util.Log_Error("exception: " + ex.StackTrace);
                    }
                }
                );
            }
        }

        ///<summary>
        ///处理持仓数据，合计
        ///</summary>
        ///<param name="jyorderData"></param>
        private void ProcessPositions_Total(List<PosInfoTotal> posInfoSumsLst)
        {
            if (_MainWindow != null && System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    try
                    {
                        _MainWindow.PositionCollection_Total.Clear();
                        Dictionary<string, PosInfoTotal> posSumDic = new Dictionary<string, PosInfoTotal>();
                        foreach (PosInfoTotal record in posInfoSumsLst)
                        {
                            string tempkey = record.InvestorID + "_" + record.Code + "_" + record.BuySell + "_" + record.Hedge;
                            if (posSumDic.ContainsKey(tempkey))
                            {
                                PosInfoTotal posItem = posSumDic[tempkey];
                                posItem.AvgPx = (posItem.AvgPx * posItem.TotalPosition + record.AvgPx * record.TotalPosition) / (record.TotalPosition + posItem.TotalPosition);
                                posItem.AvgPositionPrice = (posItem.AvgPositionPrice * posItem.TotalPosition + record.AvgPositionPrice * record.TotalPosition) / (record.TotalPosition + posItem.TotalPosition);

                                if (record.TodayPosition + posItem.TodayPosition != 0)
                                {
                                    posItem.TodayOpenAvgPx = (posItem.TodayOpenAvgPx * posItem.TodayPosition + record.TodayOpenAvgPx * record.TodayPosition) / (record.TodayPosition + posItem.TodayPosition);
                                }
                                else
                                {
                                    posItem.TodayOpenAvgPx = 0;
                                }

                                if (record.YesterdayPosition + posItem.YesterdayPosition != 0)
                                {
                                    posItem.YesterdayOpenAvgPx = (posItem.YesterdayOpenAvgPx * posItem.YesterdayPosition + record.YesterdayOpenAvgPx * record.YesterdayPosition) / (record.YesterdayPosition + posItem.YesterdayPosition);
                                }
                                else
                                {
                                    posItem.YesterdayOpenAvgPx = 0;
                                }

                                posItem.TotalPosition += record.TotalPosition;
                                posItem.TodayPosition += record.TodayPosition;
                                posItem.YesterdayPosition += record.YesterdayPosition;
                                posItem.Fdyk += record.Fdyk;
                                posItem.Ccyk += record.Ccyk;
                                posItem.OccupyMarginAmt += record.OccupyMarginAmt;
                                posItem.OptionMarketCap += record.OptionMarketCap;

                                posItem.CanCloseCount = posItem.TotalPosition;
                                string key = posItem.InvestorID + posItem.Code + posItem.BuySell.Contains("买").ToString();
                                if (posItem.BuySell.Contains("买") && posItem.ProductType.Contains("Option") && _FreezeExecOrderHandCount.ContainsKey(key))
                                {
                                    posItem.CanCloseCount -= _FreezeExecOrderHandCount[key];
                                }
                                string posTotalInfo = posItem.InvestorID + record.Code + (record.BuySell.Contains("买")).ToString() + posItem.Hedge;
                                if (PosTotalInfoMap.ContainsKey(posTotalInfo))
                                {
                                    posItem.CanCloseCount -= PosTotalInfoMap[posTotalInfo].FreezeCount;
                                }
                            }
                            else
                            {
                                PosInfoTotal record_rev = record.Copy();
                                if (record_rev.TotalPosition > 0)
                                {
                                    record_rev.CanCloseCount = record_rev.TotalPosition;
                                    _MainWindow.PositionCollection_Total.Add(record_rev);

                                    string key = record_rev.InvestorID + record_rev.Code + record_rev.BuySell.Contains("买").ToString() + record.Hedge;
                                    if (record_rev.BuySell.Contains("买") && record_rev.ProductType.Contains("Option") && _FreezeExecOrderHandCount.ContainsKey(key))
                                    {
                                        record_rev.CanCloseCount -= _FreezeExecOrderHandCount[key];
                                    }

                                    string posTotalInfo = record_rev.InvestorID + record.Code + (record.BuySell.Contains("买")).ToString() + record.Hedge;
                                    if (!PosTotalInfoMap.ContainsKey(posTotalInfo))
                                    {
                                        PosTotalInfoMap[posTotalInfo] = record_rev;
                                    }
                                    else
                                    {
                                        record_rev.FreezeCount = PosTotalInfoMap[posTotalInfo].FreezeCount;
                                        record_rev.CanCloseCount -= PosTotalInfoMap[posTotalInfo].FreezeCount;
                                        PosTotalInfoMap[posTotalInfo] = record_rev;
                                    }
                                    posSumDic.Add(tempkey, record_rev);
                                }
                            }

                            //Contract codeInfo = CodeSetManager.GetContractInfo(record.Code, CodeSetManager.ExNameToCtp(record.Exchange));
                            //RealData realData = DataContainer.GetRealDataFromContainer(codeInfo);
                            //if (realData != null)
                            //{
                            //    _MainWindow.HQBackgroundRealData.SetPositionInfo(realData);
                            //}
                        }

                        _MultiUsersPositionDataDic.Clear();
                        foreach (string tempKey in posSumDic.Keys)
                        {
                            PosInfoTotal posInfoItem = posSumDic[tempKey];
                            string userKey = posInfoItem.InvestorID;
                            if (_MultiUsersPositionDataDic.ContainsKey(userKey))
                            {
                                _MultiUsersPositionDataDic[userKey].Add(posInfoItem);
                            }
                            else if (userKey.Trim() != "")
                            {
                                _MultiUsersPositionDataDic.Add(userKey, new List<PosInfoTotal>() { posInfoItem });
                            }
                        }
                        //mainWindow.uscNewOrderPanel.SetHandCount();
                    }
                    catch (Exception ex)
                    {
                        Util.Log_Error("exception: " + ex.Message);
                        Util.Log_Error("exception: " + ex.StackTrace);
                    }
                }
                );
            }
        }

        private PosInfoTotal PosExecutionReport(PosInfoDetail pPosition)
        {
            PosInfoTotal posData = new PosInfoTotal();
            try
            {
                posData.InvestorID = pPosition.InvestorID;
                posData.UserID = pPosition.UserID;
                posData.Code = pPosition.Code;
                posData.BuySell = pPosition.BuySell;
                posData.TotalPosition = pPosition.TradeHandCount;
                if (pPosition.PositionType.Contains("昨"))
                {
                    posData.YesterdayPosition = pPosition.TradeHandCount;
                    posData.YesterdayOpenAvgPx = pPosition.AvgPx;
                }
                else
                {
                    posData.TodayPosition = pPosition.TradeHandCount;
                    posData.TodayOpenAvgPx = pPosition.AvgPx;
                }
                posData.AvgPx = pPosition.AvgPx;//?
                posData.AvgPositionPrice = pPosition.PositionType.Contains("昨") ? pPosition.PrevSettleMent : pPosition.AvgPx;//?
                if (pPosition.OccupyMarginAmt != null && pPosition.OccupyMarginAmt != String.Empty && pPosition.OccupyMarginAmt != "-")
                {
                    posData.OccupyMarginAmt = Double.Parse(pPosition.OccupyMarginAmt);
                }
                else
                {
                    posData.OccupyMarginAmt = Double.NaN;
                }
                posData.Hedge = pPosition.Hedge;
                posData.Ccyk = pPosition.Ccyk;
                posData.Fdyk = pPosition.Fdyk;
                posData.OptionMarketCap = pPosition.OptionMarketCap;
                posData.Exchange = pPosition.Exchange;
                posData.Name = pPosition.Name;
                posData.ProductType = pPosition.ProductType;
                posData.BackEnd = pPosition.BackEnd;
            }
            catch (Exception ex)
            {
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error(ex.StackTrace);
            }
            return posData;
        }

        private void InitMarginRate(Contract contract, PosInfoDetail record, BACKENDTYPE backEnd)
        {
            string tempKey = contract.Code + "_" + record.InvestorID;
            if (MarginDict.ContainsKey(tempKey))
            {
                if (CheckValidMarginRate(MarginDict[tempKey]))
                {
                    ProcessPositionDetailMarginData(tempKey, contract, record);
                }
            }
            else
            {
                MarginDict.Add(tempKey, new MarginStruct());
                object dataServer = GetDataServerInstance(record.InvestorID, backEnd);
                if (dataServer is CtpDataServer)
                {
                    CtpDataServer ctpServer = dataServer as CtpDataServer;
                    ctpServer.InitiateMarginData(contract, record.InvestorID, record.AvgPx, record.PrevSettleMent); //To do
                }
                else if (dataServer is FemasDataServer)
                {

                }
            }
        }

        private bool CheckValidMarginRate(MarginStruct marginStruct)
        {
            if (marginStruct.LongMarginRatioByMoney == marginStruct.LongMarginRatioByVolume && marginStruct.LongMarginRatioByMoney == 0.0
                && marginStruct.ShortMarginRatioByMoney == marginStruct.ShortMarginRatioByVolume && marginStruct.ShortMarginRatioByMoney == 0.0
                && marginStruct.MiniMargin == marginStruct.Royalty && marginStruct.Royalty == 0.0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 处理手续费数据
        /// </summary>
        /// <param name="jyorderData"></param>
        private void ProcessCommissionData(string commKey, TradeOrderData tradedData)
        {
            Contract contract = CodeSetManager.GetContractInfo(tradedData.Code, CodeSetManager.ExNameToCtp(tradedData.Exchange));
            if (contract == null || commKey == null) return;
            string tempKey = contract.SpeciesCode + "_" + tradedData.InvestorID;
            if (tempKey.StartsWith("_")) return;

            if ((commKey == "" || commKey == tempKey) && CommissionDict.ContainsKey(tempKey))
            {
                if (tradedData.OpenClose.Contains("开"))
                {
                    tradedData.Charge = (tradedData.AvgPx * contract.Hycs * CommissionDict[tempKey].OpenRatioByMoney + CommissionDict[tempKey].OpenRatioByVolume) * tradedData.TradeHandCount;
                }
                else if (tradedData.OpenClose.Contains("平今"))
                {
                    tradedData.Charge = (tradedData.AvgPx * contract.Hycs * CommissionDict[tempKey].CloseTodayRatioByMoney + CommissionDict[tempKey].CloseTodayRatioByVolume) * tradedData.TradeHandCount;
                }
                else//平仓
                {
                    tradedData.Charge = (tradedData.AvgPx * contract.Hycs * CommissionDict[tempKey].CloseRatioByMoney + CommissionDict[tempKey].CloseRatioByVolume) * tradedData.TradeHandCount;
                }
            }
        }

        /// <summary>
        /// 处理持仓明细保证金数据
        /// </summary>
        /// <param name="jyorderData"></param>
        private void ProcessPositionDetailMarginData(string marginKey, Contract contract, PosInfoDetail posDetail)
        {
            if (MarginDict.ContainsKey(marginKey))
            {
                if (contract.ProductType == "Futures")
                {
                    if (posDetail.BuySell.Contains("买"))
                    {
                        if (posDetail.PositionType.Contains("今"))
                        {
                            posDetail.OccupyMarginAmt = ((posDetail.AvgPx * contract.Hycs * MarginDict[marginKey].LongMarginRatioByMoney + MarginDict[marginKey].LongMarginRatioByVolume) * posDetail.TradeHandCount).ToString();
                        }
                        else
                        {
                            posDetail.OccupyMarginAmt = ((posDetail.PrevSettleMent * contract.Hycs * MarginDict[marginKey].LongMarginRatioByMoney + MarginDict[marginKey].LongMarginRatioByVolume) * posDetail.TradeHandCount).ToString();
                        }
                    }
                    else //卖
                    {
                        if (posDetail.PositionType.Contains("今"))
                        {
                            posDetail.OccupyMarginAmt = ((posDetail.AvgPx * contract.Hycs * MarginDict[marginKey].ShortMarginRatioByMoney + MarginDict[marginKey].ShortMarginRatioByVolume) * posDetail.TradeHandCount).ToString();
                        }
                        else
                        {
                            posDetail.OccupyMarginAmt = ((posDetail.PrevSettleMent * contract.Hycs * MarginDict[marginKey].ShortMarginRatioByMoney + MarginDict[marginKey].ShortMarginRatioByVolume) * posDetail.TradeHandCount).ToString();
                        }
                    }
                }
                else if (contract.ProductType.Contains("Option"))
                {
                    if (posDetail.BuySell.Contains("卖") && MarginDict.ContainsKey(marginKey))
                    {
                        posDetail.OccupyMarginAmt = (Math.Max(MarginDict[marginKey].Royalty + MarginDict[marginKey].FixedMargin, MarginDict[marginKey].MiniMargin) * posDetail.TradeHandCount).ToString();
                    }
                    else if (posDetail.BuySell.Contains("买"))
                    {
                        posDetail.OccupyMarginAmt = "0.00";
                    }
                }
            }
        }

        /// <summary>
        /// 处理持仓汇总保证金数据
        /// </summary>
        /// <param name="jyorderData"></param>
        private void ProcessPositionMarginData(string marginKey, Contract contract, PosInfoTotal posTotal)
        {
            if (MarginDict.ContainsKey(marginKey))
            {
                if (contract.ProductType == "Futures")
                {
                    if (posTotal.BuySell.Contains("买"))
                    {
                        posTotal.OccupyMarginAmt = (posTotal.AvgPositionPrice * contract.Hycs * MarginDict[marginKey].LongMarginRatioByMoney + MarginDict[marginKey].LongMarginRatioByVolume) * posTotal.TotalPosition;
                    }
                    else //卖
                    {
                        posTotal.OccupyMarginAmt = (posTotal.AvgPositionPrice * contract.Hycs * MarginDict[marginKey].ShortMarginRatioByMoney + MarginDict[marginKey].ShortMarginRatioByVolume) * posTotal.TotalPosition;
                    }
                }
                else if (contract.ProductType.Contains("Option"))
                {
                    if (posTotal.BuySell.Contains("卖") && MarginDict.ContainsKey(marginKey))
                    {
                        posTotal.OccupyMarginAmt = Math.Max(MarginDict[marginKey].Royalty + MarginDict[marginKey].FixedMargin, MarginDict[marginKey].MiniMargin) * posTotal.TotalPosition;
                    }
                    else if (posTotal.BuySell.Contains("买"))
                    {
                        posTotal.OccupyMarginAmt = 0.00;
                    }
                }
            }
        }

        private void ReceiveCapitalInfo(CapitalInfo capitalInfo)
        {
            if (_MainWindow != null && System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    try
                    {
                        JYRealData jyRealData = new JYRealData();
                        jyRealData.CapitalID = capitalInfo.CapitalID;
                        jyRealData.Charge = Math.Round(capitalInfo.Charge, 2);
                        jyRealData.CloseProfit = Math.Round(capitalInfo.CloseProfit, 2);
                        jyRealData.Bond = Math.Round(capitalInfo.Bond, 2);
                        jyRealData.BuyMarginAmt = Math.Round(capitalInfo.BuyMarginAmt, 2);
                        //jyRealData.Dsfy = Math.Round(capitalInfo.dsfy, 2);
                        jyRealData.Dspy = Math.Round(capitalInfo.CloseProfit, 2);
                        //jyRealData.Dszyk = Math.Round(capitalInfo.dszyk, 2);
                        jyRealData.Fetchable = Math.Round(capitalInfo.Fetchable, 2);
                        jyRealData.StaticEquity = Math.Round(capitalInfo.TodayEquity, 2);

                        //jyRealData.FloatProfit = Math.Round(capitalInfo.floatProfit, 2);
                        jyRealData.Frozen = Math.Round(capitalInfo.Frozen, 2);
                        jyRealData.FrozenCommision = Math.Round(capitalInfo.FrozenCommision, 2);
                        jyRealData.YesterdayAvailabe = Math.Round(capitalInfo.YesterdayAvailabe, 2);
                        jyRealData.YesterdayEquity = Math.Round(capitalInfo.YesterdayEquity, 2);
                        jyRealData.YesterdatBalance = Math.Round(capitalInfo.YesterdayBalance, 2);
                        jyRealData.LastCredit = Math.Round(capitalInfo.LastCredit, 2);
                        jyRealData.Credit = Math.Round(capitalInfo.Credit, 2);
                        jyRealData.LastMortage = Math.Round(capitalInfo.LastMortage, 2);
                        jyRealData.Mortage = Math.Round(capitalInfo.Mortgage, 2);
                        jyRealData.FrozenMargin = Math.Round(capitalInfo.FrozenMargin, 2);

                        jyRealData.InMoney = Math.Round(capitalInfo.InMoney, 2);
                        jyRealData.OutMoney = Math.Round(capitalInfo.OutMoney, 2);
                        jyRealData.Charge = Math.Round(capitalInfo.Charge, 2);
                        jyRealData.TotalExchangeBond = Math.Round(capitalInfo.TotalExchangeBond, 2);
                        jyRealData.TodayBalance = Math.Round(capitalInfo.TodayBalance, 2);
                        jyRealData.TodayAvailable = Math.Round(capitalInfo.TodayAvailable, 2);
                        jyRealData.TodayEquity = Math.Round(capitalInfo.TodayEquity, 2);
                        //jyRealData.Zbfy = Math.Round(capitalInfo.zbfy, 2);
                        jyRealData.Zbpy = Math.Round(capitalInfo.Zbpy, 2);
                        jyRealData.Zbzyk = Math.Round(capitalInfo.Zbzyk, 2);
                        jyRealData.ProfitRatio = Math.Round(capitalInfo.ProfitRatio, 2);
                        jyRealData.TotalExchangeBond = Math.Round(capitalInfo.TotalExchangeBond, 2);
                        jyRealData.Reserve = Math.Round(capitalInfo.Reserve, 2);
                        jyRealData.DeliveryMargin = Math.Round(capitalInfo.DeliveryMargin, 2);
                        jyRealData.Royalty = Math.Round(capitalInfo.Royalty, 2);
                        jyRealData.FrozenRoyalty = Math.Round(capitalInfo.FrozenRoyalty, 2);

                        double totalZBFY = 0;
                        double totalDSFY = 0;
                        double totalFdyk = 0;
                        foreach (PosInfoTotal detail in _MainWindow.PositionCollection_Total)
                        {
                            Contract tempContract = CodeSetManager.GetContractInfo(detail.Code, CodeSetManager.ExNameToCtp(detail.Exchange));
                            if (tempContract != null && !tempContract.ProductType.Contains("Option"))
                            {
                                totalDSFY += detail.Ccyk;
                                totalFdyk += detail.Fdyk;
                            }
                        }

                        jyRealData.Dsfy = totalDSFY;
                        jyRealData.FloatProfit = totalFdyk;
                        jyRealData.Zbfy = totalZBFY;
                        _MainWindow.OnGotMoney(jyRealData);
                    }
                    catch (Exception ex)
                    {
                        Util.Log_Error("exception: " + ex.Message);
                        Util.Log_Error("exception: " + ex.StackTrace);
                    }
                });
            }
        }

        private void OnReceiveSettlementSheet(string investorID, string settlementSheet)
        {
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    try
                    {
                        if (_AffirmWindow != null && _AffirmWindow.Visibility == System.Windows.Visibility.Visible)
                        {
                            //_AffirmWindow.tb_content.Text = settlementSheet;
                            _AffirmWindow.AddNewTabItem(investorID, settlementSheet);
                        }
                        else if (_MainWindow != null && _MainWindow.UscStatementsInquiry != null && _MainWindow.Visibility == System.Windows.Visibility.Visible)
                        {
                            _MainWindow.UscStatementsInquiry.txtStatementOrder.Text = settlementSheet;
                            _MainWindow.UscStatementsInquiry.btnQueryStatementOrder.IsEnabled = _MainWindow.UscStatementsInquiry.btnQueryMonthStatementOrder.IsEnabled = true;
                            Util.Log("收到结算单信息：" + DateTime.Now.ToLongTimeString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.Log_Error("exception: " + ex.Message);
                        Util.Log_Error("exception: " + ex.StackTrace);
                    }
                });
            }
        }

        private Tuple<List<string>, List<string>> GetBankAcctInfoList(List<BankAccountInfo> bankAcctInfoLst)
        {
            Tuple<List<string>, List<string>> bankAcctInfoTuple = Tuple.Create(new List<string>(), new List<string>());
            foreach (BankAccountInfo acctInfo in bankAcctInfoLst)
            {
                string bankName = BankManager.GetBankNameFromBankID(acctInfo.BankID);
                Util.Log("===================================");
                Util.Log("AccountID: " + acctInfo.AccountID);
                Util.Log("BrokerID: " + acctInfo.BrokerID);
                Util.Log("CustType: " + acctInfo.CustType);
                Util.Log("CustomerName: " + acctInfo.CustomerName);
                Util.Log("IdentifiedCardType: " + acctInfo.IdCardType);
                Util.Log("IdentifiedCardNo: " + acctInfo.IdentifiedCardNo);
                Util.Log("BankName: " + bankName);
                Util.Log("BankBranchID: " + acctInfo.BankBrchID);
                Util.Log("BankAccType: " + acctInfo.BankAccType);
                Util.Log("BankAccount: " + acctInfo.BankAccount);
                Util.Log("CurrencyID: " + acctInfo.CurrencyID);
                Util.Log("OpenOrDestroy: " + acctInfo.OpenOrDestroy);
                Util.Log("RegDate: " + acctInfo.RegDate);
                Util.Log("OutDate: " + acctInfo.OutDate);
                string bankAcct = acctInfo.BankAccount.Length > 4 ? acctInfo.BankAccount.Substring(acctInfo.BankAccount.Length - 4, 4) : "";
                string bankCardItem = string.IsNullOrEmpty(bankName.Trim()) ? "" : string.Format("{0}（****{1}）", bankName, bankAcct);
                bankAcctInfoTuple.Item1.Add(bankCardItem); //bankCardList
                if (!string.IsNullOrEmpty(acctInfo.CurrencyID))
                {
                    bankAcctInfoTuple.Item2.Add(acctInfo.CurrencyID);//currencyList
                }
            }
            Util.Log("===================================");
            return bankAcctInfoTuple;
        }

        private void ProcessBankAcctInfo(Tuple<List<string>, List<string>> bankAcctInfoTuple)
        {
            if (_MainWindow != null && System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    if (_MainWindow.UscInterTransfer == null)
                    {
                        _MainWindow.UscInterTransfer = new InterTranfer();
                    }
                    _MainWindow.UscInterTransfer.InitBinding(bankAcctInfoTuple.Item1); //bankCardList
                    _MainWindow.UscInterTransfer.cb_Currency.ItemsSource = bankAcctInfoTuple.Item2; //currencyList
                });
            }
        }

        /// <summary>
        /// 收到新转账流水
        /// </summary
        private void ProcessNewTransferRecords(TransferSingleRecord transferRec)
        {
            _TransferRecords.Add(transferRec);
        }

        /// <summary>
        /// 收到转账流水
        /// </summary
        private void ProcessTransferRecords(List<TransferSingleRecord> transferRecordsList)
        {
            _TransferRecords.Sort(TransferSingleRecord.CompareBySerialNo);
            if (_MainWindow != null && System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    _MainWindow.TransferRecordCollection.Clear();
                    foreach (TransferSingleRecord tRec in transferRecordsList)
                    {
                        _MainWindow.TransferRecordCollection.Add(tRec);
                    }
                });
            }
        }

        /// <summary>
        /// 对未成交的平仓单撤单
        /// </summary>
        /// <param name="orderString"></param>
        public bool CancelPositionFreezeOrder(string orderString)
        {
            if (_FreezeOrder.ContainsKey(orderString))
            {
                List<TradeOrderData> orderList = _FreezeOrder[orderString];
                foreach (TradeOrderData order in orderList)
                {
                    RequestOrder(order.InvestorID, order.BackEnd, new RequestContent("CancelOrder", new List<object>() { order.Code, order.FrontID, order.SessionID, order.OrderRef, order.Exchange, order.OrderID }));
                }
                return true;
            }
            return false;
        }

        public void AddPosOrderList(string investorID, BACKENDTYPE backEnd, PosInfoOrder posOrder)
        {
            object dataServer = GetDataServerInstance(investorID, backEnd);
            if (dataServer is CtpDataServer)
            {
                CtpDataServer ctpServer = dataServer as CtpDataServer;
                ctpServer.AddPosOrderList(posOrder);
            }
            else if (dataServer is FemasDataServer)
            {

            }
            else
            {
                Util.Log("Illegal back-end type: " + backEnd);
            }
        }

        public void AddPosCancelOrderList(string investorID, BACKENDTYPE backEnd, PosInfoOrder posOrder)
        {
            object dataServer = GetDataServerInstance(investorID, backEnd);
            if (dataServer is CtpDataServer)
            {
                CtpDataServer ctpServer = dataServer as CtpDataServer;
                ctpServer.AddPosCancelOrderList(posOrder);
            }
            else if (dataServer is FemasDataServer)
            {

            }
            else
            {
                Util.Log("Illegal back-end type: " + backEnd);
            }
        }

        /// <summary>
        /// 获取未成交的平仓单
        /// </summary>
        /// <param name="orderString"></param>
        public List<TradeOrderData> GetFreezeOrder(string orderString)
        {
            if (_FreezeOrder.ContainsKey(orderString))
            {
                List<TradeOrderData> orderList = _FreezeOrder[orderString];
                if (orderList != null)
                {
                    return orderList;
                }
            }
            return null;
        }

        /// <summary>
        /// 得到某个品种的持仓（多头或者空头），返回参数包括今仓的量和昨仓的量
        /// </summary>
        /// <param name="code"></param>
        /// <param name="isBuy"></param>
        /// <returns></returns>
        public Boolean GetPositionCount(string code, Boolean isBuy, out int todayPos, out int yesterdayPos)
        {
            todayPos = 0;
            yesterdayPos = 0;
            if (code == null || code == "")
            {
                return false;
            }
            if (_PosHandCountMap == null) return false;
            if (_PosHandCountMap.ContainsKey(code))
            {
                PosHandCount posHandCount = _PosHandCountMap[code];
                if (posHandCount == null) return false;
                if (isBuy)
                {
                    //多单
                    todayPos = posHandCount.TodayHandCountBuy;
                    yesterdayPos = posHandCount.LastDayHandCountBuy;
                }
                else
                {
                    //空单
                    todayPos = posHandCount.TodayHandCountSell;
                    yesterdayPos = posHandCount.LastDayHandCountSell;
                }
                return true;
            }
            return false;
        }
    }
}
