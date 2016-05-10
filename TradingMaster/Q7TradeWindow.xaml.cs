using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using plc;
using CTPMaster.Control;
using System.ComponentModel;
using System.Threading;
using KiiikCommon;

namespace CTPMaster
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : plc.PluginUC
    {
        internal MainWindow()
        {
            ResourceDictionary o = (ResourceDictionary)System.Windows.Application.LoadComponent(new Uri("/CTPMaster;component/Dictionary1.xaml", UriKind.Relative));
            System.Windows.Application.Current.Resources.MergedDictionaries.Add(o);
            InitializeComponent();            
            InitControls();
            JYDataServer.getServerInstance().setMainWindow(this);
            uscStatusBar.Init(this);
            systemTips = new SystemTips();
            systemTips.Init(this);
            this.WindowTitle = "交易";
            mutex1 = new Mutex();
        }        

        private void InitControls()
        {
            uscNewOrderPanel.Init(this);

            uscAllOrder.Init(this);
            uscTransactionQuery.Init(this);
            uscPositionsInquiry.Init(this);
            uscPositionsInquiry.PositionDataMouseLeftButtonDown += new PositionDataMouseLeftButtonDownDelegate(uscPositionsInquiry_PositionDataMouseLeftButtonDown);
            uscPositionsInquiry.PositionDataMouseDoubleClicked += new PositionDataMouseDoubleClickDelegate(uscPositionsInquiry_PositionDataMouseDoubleClicked);

            capitalQuery.Init(this);
            capitalQuery.btnQuery.Click += new RoutedEventHandler(CapitalDetail_Click);

            uscStatementsInquiry = new StatementsInquiry();

            HQBackgroundRealData = new BackgroundDataServer();
        }

        Window orderPreplayWindow = null;
        Window capitalDetailWindow;
        Window systemMessageWindow = null;
        SystemTips systemTips = null;
        public StatementsInquiry uscStatementsInquiry = null;
        private Mutex mutex1;
        public ChangePass updatePwd = null;

        public BackgroundDataServer HQBackgroundRealData;
        private bool isWindowLoaded = false;
        private bool isNeedShowAlertWndOnStart = false;
        private bool hasShowedRisk = false;

        private ObservableCollection<DisplayRealData> realDataCollection = new ObservableCollection<DisplayRealData>();

        //private ServerData JYServer;
        //private ServerData HQServer;

        public ObservableCollection<DisplayRealData> RealDataCollection
        {
            get { return realDataCollection; }
            set { realDataCollection = value; }
        }

        private JYRealData capitalDataCollection = new JYRealData();

        public JYRealData CapitalDataCollection
        {
            get { return capitalDataCollection; }
            set { capitalDataCollection = value; }
        }

        /// <summary>
        /// 委托单
        /// </summary>
        private ObservableCollection<Q7JYOrderData> orderDataCollection = new ObservableCollection<Q7JYOrderData>();
        /// <summary>
        /// 挂单
        /// </summary>
        private ObservableCollection<Q7JYOrderData> pendingCollection = new ObservableCollection<Q7JYOrderData>();
        /// <summary>
        /// 所有成交，明细
        /// </summary>
        private ObservableCollection<Q7JYOrderData> tradeCollection_MX = new ObservableCollection<Q7JYOrderData>();

        /// <summary>
        /// 所有成交，按合约分组
        /// </summary>
        private ObservableCollection<Q7JYOrderData> tradeCollection_Code = new ObservableCollection<Q7JYOrderData>();

        /// <summary>
        /// 所有成交委托单
        /// </summary>
        private ObservableCollection<Q7JYOrderData> tradedOrderCollection = new ObservableCollection<Q7JYOrderData>();        

        /// <summary>
        /// 持仓明细
        /// </summary>
        public ObservableCollection<Q7PosInfoDetail> positionDetailCollection = new ObservableCollection<Q7PosInfoDetail>();

        /// <summary>
        /// 持仓合计
        /// </summary>
        private ObservableCollection<Q7PosInfoTotal> positionDetailCollection_Total = new ObservableCollection<Q7PosInfoTotal>();

        /// <summary>
        /// 平仓查询
        /// </summary>
        private ObservableCollection<CloseData> closeDataCollection = new ObservableCollection<CloseData>();

        /// <summary>
        /// 所有埋单条件单
        /// </summary>
        private ObservableCollection<Q7JYOrderData> maiConditionOrderData = new ObservableCollection<Q7JYOrderData>();

        /// <summary>
        /// 所有埋单条件单
        /// </summary>
        public ObservableCollection<Q7JYOrderData> MaiConditionOrderData
        {
            get { return maiConditionOrderData; }
            set { maiConditionOrderData = value; }
        }

        /// <summary>
        /// 条件单
        /// </summary>
        private ObservableCollection<Q7JYOrderData> conditionOrderData = new ObservableCollection<Q7JYOrderData>();

        /// <summary>
        /// 条件单
        /// </summary>
        public ObservableCollection<Q7JYOrderData> ConditionOrderData
        {
            get { return conditionOrderData; }
            set { conditionOrderData = value; }
        }

        /// <summary>
        /// 已发送埋单条件单
        /// </summary>
        private ObservableCollection<Q7JYOrderData> maiOrderData = new ObservableCollection<Q7JYOrderData>();

        /// <summary>
        /// 已发送埋单条件单
        /// </summary>
        public ObservableCollection<Q7JYOrderData> MaiOrderData
        {
            get { return maiOrderData; }
            set { maiOrderData = value; }
        }

        /// <summary>
        /// 埋单
        /// </summary>
        private ObservableCollection<Q7JYOrderData> sentOrderData = new ObservableCollection<Q7JYOrderData>();
        /// <summary>
        /// 埋单
        /// </summary>
        public ObservableCollection<Q7JYOrderData> SentOrderData
        {
            get { return sentOrderData; }
            set { sentOrderData = value; }
        }

        /// <summary>
        /// 已撤单
        /// </summary>
        private ObservableCollection<Q7JYOrderData> cancelledOrderData = new ObservableCollection<Q7JYOrderData>();

        /// <summary>
        /// 已撤单
        /// </summary>
        public ObservableCollection<Q7JYOrderData> CancelledOrderData
        {
            get { return cancelledOrderData; }
            set { cancelledOrderData = value; }
        }

        /// <summary>
        /// 已成交单
        /// </summary>
        private ObservableCollection<Q7JYOrderData> transactionOrderData = new ObservableCollection<Q7JYOrderData>();

        /// <summary>
        /// 已成交单
        /// </summary>
        public ObservableCollection<Q7JYOrderData> TransactionOrderData
        {
            get { return transactionOrderData; }
            set { transactionOrderData = value; }
        }

        /// <summary>
        /// 平仓查询
        /// </summary>
        public ObservableCollection<CloseData> CloseDataCollection
        {
            get { return closeDataCollection; }
        }

        /// <summary>
        /// 所有委托单
        /// </summary>
        public ObservableCollection<Q7JYOrderData> OrderDataCollection
        {
            get { return orderDataCollection; }
            set { orderDataCollection = value; }
        }

        /// <summary>
        /// 所有挂单（未成交）
        /// </summary>
        public ObservableCollection<Q7JYOrderData> PendingCollection
        {
            get { return pendingCollection; }
            set { pendingCollection = value; }
        }

        /// <summary>
        /// 所有成交委托单
        /// </summary>
        public ObservableCollection<Q7JYOrderData> TradedOrderCollection
        {
            get { return tradedOrderCollection; }
            set { tradedOrderCollection = value; }
        }

        /// <summary>
        /// 所有成交_合计
        /// </summary>
        public ObservableCollection<Q7JYOrderData> TradeCollection_Code
        {
            get { return tradeCollection_Code; }
            set { tradeCollection_Code = value; }
        }

        /// <summary>
        /// 所有成交_明细
        /// </summary>
        public ObservableCollection<Q7JYOrderData> TradeCollection_MX
        {
            get { return tradeCollection_MX; }
            set { tradeCollection_MX = value; }
        }

        /// <summary>
        /// 持仓明细
        /// </summary>
        public ObservableCollection<Q7PosInfoDetail> PositionDetailCollection
        {
            get { return positionDetailCollection; }
            set { positionDetailCollection = value; }
        }

        /// <summary>
        /// 持仓合计
        /// </summary>
        public ObservableCollection<Q7PosInfoTotal> PositionCollection_Total
        {
            get { return positionDetailCollection_Total; }
            set { positionDetailCollection_Total = value; }
        }

        public HashSet<CodeInfo> codeInfoset = new HashSet<CodeInfo>();
        private System.Windows.Controls.DataGrid realListView;
        public System.Windows.Controls.DataGrid RealListView
        {
            get { return realListView; }
            set { realListView = value; }
        }

        private ObservableCollection<SystemMessage> systemMessageCollection = new ObservableCollection<SystemMessage>();

        public ObservableCollection<SystemMessage> SystemMessageCollection
        {
            get { return systemMessageCollection; }
            set { systemMessageCollection = value; }
        }  

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="messageType">1 commit;2 transact; 3 cancel</param>
        public void HandleOrderInfo(Q7JYOrderData orderInfo)
        {
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    try
                    {
                        if (orderPreplayWindow == null)
                        {
                            OrderReplayMessageBox orderReplayMessageBox = new OrderReplayMessageBox();
                            orderReplayMessageBox.Init(this);
                            orderPreplayWindow = CommonUtil.GetWindow("", orderReplayMessageBox, plc.API.GetMainWindow());
                            orderPreplayWindow.Closing += new CancelEventHandler(orderPreplayWindow_Closing);

                            if (orderReplayMessageBox.AddOrderInfo(orderInfo))
                            {
                                orderReplayMessageBox.Index = 0;
                                orderPreplayWindow.Show();
                            }
                            return;
                        }

                        OrderReplayMessageBox orderReplay = orderPreplayWindow.Content as OrderReplayMessageBox;
                        if (orderReplay.AddOrderInfo(orderInfo))
                        {
                            if (orderReplay.Index < 0)
                            {
                                orderReplay.Index = 0;
                            }
                            orderReplay.chkConfirmSubmit.IsChecked = false;
                            orderReplay.chkConfirmTransact.IsChecked = false;
                            orderReplay.chkConfirmCancel.IsChecked = false;

                            orderPreplayWindow.Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.Log(ex.ToString());
                    }
                });
            }
        }

        void orderPreplayWindow_Closing(object sender, CancelEventArgs e)
        {
            orderPreplayWindow.Hide();
            OrderReplayMessageBox orderReplayMessageBox = orderPreplayWindow.Content as OrderReplayMessageBox;
            orderReplayMessageBox.Clear();
            e.Cancel = true;
        }

        internal void ShowSystemMessge()
        {
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    if (systemMessageWindow == null)
                    {
                        systemMessageWindow =CommonUtil.GetWindow("日志记录", systemTips, plc.API.GetMainWindow());
                        systemMessageWindow.ResizeMode = ResizeMode.CanResize;
                        systemMessageWindow.Closing += new CancelEventHandler(systemMessageWindow_Closing);
                    }
                    systemMessageWindow.Show();
                });
            }
        }

        void systemMessageWindow_Closing(object sender, CancelEventArgs e)
        {
            systemMessageWindow.Hide();
            e.Cancel = true;
        }

        public void OnGotMoney(JYRealData jyRealData)
        {
            //CapitalDataCollection.StaticEquity = jyRealData.StaticEquity;
            CapitalDataCollection.YesterdayEquity = jyRealData.YesterdayEquity;
            CapitalDataCollection.LastCredit = jyRealData.LastCredit;
            CapitalDataCollection.LastMortage = jyRealData.LastMortage;
            CapitalDataCollection.Mortage = jyRealData.Mortage;
            CapitalDataCollection.OutMoney = jyRealData.OutMoney;
            CapitalDataCollection.InMoney = jyRealData.InMoney;
            CapitalDataCollection.StaticEquity = jyRealData.StaticEquity;
            CapitalDataCollection.Dspy = jyRealData.Dspy;
            CapitalDataCollection.Dsfy = jyRealData.Dsfy;
            CapitalDataCollection.Charge = jyRealData.Charge;
            //CapitalDataCollection.DynamicEquity = jyRealData.DynamicEquity;
            CapitalDataCollection.Bond = jyRealData.Bond;
            CapitalDataCollection.FrozenMargin = jyRealData.FrozenMargin;
            CapitalDataCollection.FrozenCommision = jyRealData.FrozenCommision;
            CapitalDataCollection.Credit = jyRealData.Credit;
            CapitalDataCollection.Frozen = jyRealData.Frozen;
            //CapitalDataCollection.CaculatedAvailable = jyRealData.CaculatedAvailable;
            CapitalDataCollection.Fetchable = jyRealData.Fetchable;
            CapitalDataCollection.RiskRatio = jyRealData.RiskRatio;
            
            if (!hasShowedRisk)
            {
                if (jyRealData.RiskRatio > 80)
                {
                    if (isWindowLoaded == false)
                    {
                        isNeedShowAlertWndOnStart = true;
                    }
                    else
                    {
                        ShowAlertWindow();
                    }
                }
            }
            //money.Content = "可用:¥ " + jyRealData.TodayAvailable.ToString() + " 浮盈:¥ " + jyRealData.Dsfy.ToString() + " 平盈:¥ " + jyRealData.Dspy.ToString() + " 保证金:¥ " + jyRealData.Bond.ToString();
        }

        private void ShowAlertWindow()
        {
            if (CapitalDataCollection == null) return;
            JYRealData jyRealData = CapitalDataCollection;
            Login PreLogWindow = JYDataServer.getServerInstance().getLoginControl();
            string userName = PreLogWindow.TbUserName.Text.ToString();
            string message = string.Format("{0}：尊敬的用户{1},您的风险级别为警示，风险度(客户保证金/总权益*100%)为{2}%。",
                DateTime.Now.ToString("HH:mm:ss"), userName, jyRealData.RiskRatio.ToString("0.00"));
            AddSystemMessage(DateTime.Now, message, "信息", "System");
            hasShowedRisk = true;
        }

        void uscPositionsInquiry_PositionDataMouseLeftButtonDown(string buyOrSell, string kp, int num, RealData realData)
        {
            uscNewOrderPanel.SetOrderInfoByPositionData(buyOrSell, kp, num, realData);
        }

        void uscPositionsInquiry_PositionDataMouseDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            //uscNewOrderPanel.AddNewOrder();
        }

        private void CapitalDetail_Click(object sender, RoutedEventArgs e)
        {
            JYDataServer.getServerInstance().AddToQryQueue(new CTPRequestContent("ReqCapital", new List<object>()));
            if (CapitalDataCollection != null )
            {
                if (capitalDetailWindow == null)
                {
                    CapitalDetail capitalQuery = new CapitalDetail();
                    capitalQuery.SetJYRealData(CapitalDataCollection);
                    capitalDetailWindow = CommonUtil.GetWindow("期货资金账户详情", capitalQuery, plc.API.GetMainWindow());
                    capitalDetailWindow.Closing += new System.ComponentModel.CancelEventHandler(capitalDetailWindow_Closing);
                    capitalDetailWindow.Show();
                }
                else
                {
                    CapitalDetail capitalQuery = capitalDetailWindow.Content as CapitalDetail;
                    capitalQuery.SetJYRealData(CapitalDataCollection);
                    capitalDetailWindow.Visibility = Visibility.Visible;
                }
            }
        }

        void capitalDetailWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            capitalDetailWindow.Visibility = Visibility.Collapsed;
            e.Cancel = true;
        }

        public void AddSystemMessage(DateTime feedbackTime, string feedbackMessage, string messageLevel, string messageClass)
        {
            if (this == null)
                return;
            AddSystemMessage(feedbackTime.ToString(SystemMessage.FeedBackTimeFormat), feedbackMessage, messageLevel, messageClass);
        }

        public void AddSystemMessage(DateTime feedbackTime, string feedbackMessage)
        {
            AddSystemMessage(feedbackTime.ToString(SystemMessage.FeedBackTimeFormat), feedbackMessage, "信息", "System");
        }

        private void AddSystemMessage(string feedbackTime, string feedbackMessage, string messageLevel, string messageClass)
        {
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    AddMessageToCollection(feedbackTime, feedbackMessage, messageLevel, messageClass);
                });
            }
        }

        private void AddMessageToCollection(string feedbackTime, string feedbackMessage, string messageLevel, string messageClass)
        {
            mutex1.WaitOne();            
            try
            {
                //Todo: 存疑
                if (SystemMessageCollection.Count > 0
                    && SystemMessageCollection[0].FeedbackMessage.Trim().Equals(feedbackMessage.Trim()) && !SystemMessageCollection[0].MessageClass.Contains("API"))
                {
                    //mutex1.ReleaseMutex();

                    ////防止添加相同的系统提示
                    //return;
                }
                else
                {
                    SystemMessage newMessage = new SystemMessage()
                    {
                        MessageID = SystemMessage.GetNextId(),
                        FeedbackTime = feedbackTime,
                        FeedbackMessage = feedbackMessage,
                        MessageLevel = messageLevel,
                        MessageClass = messageClass
                    };
                    SystemMessageCollection.Add(newMessage);
                    string messageTime = string.Empty;
                    if (feedbackTime.IndexOf(" ") > 0 && feedbackTime.IndexOf(".") > 0)
                    {
                        uscStatusBar.tbStartTime.Text = feedbackTime.Substring(feedbackTime.IndexOf(" ") + 1, feedbackTime.IndexOf(".") - feedbackTime.IndexOf(" ") - 1);
                    }
                    uscStatusBar.tbMessge.Text = feedbackMessage;
                }
            }
            catch (Exception e)
            {
                Util.Log(e.Message);
                Util.Log(e.StackTrace);
            }
            mutex1.ReleaseMutex();
        }

        /// <summary>
        /// 改变交易服务器状态
        /// </summary>
        /// <param name="isConnected"></param>
        public void ChangeJYServerStatus(Boolean isConnected)
        {
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    uscStatusBar.FormString();
                });
            }
        }

        public void UpdateMaxOperation(MaxOperation maxOperation)
        {
            uscNewOrderPanel.UpdateMaxOperation(maxOperation);
        }

        /// <summary>
        /// 接收到行情或者主推数据
        /// </summary>
        public override void OnReceiveSnapShotOrUpdateCallBack(RealData realData)
        {
            HQBackgroundRealData.ChangeType(realData);
        }

        private void Window_loaded(object sender, RoutedEventArgs e)
        {
            //InitControls();
            isWindowLoaded = true;
            if (isNeedShowAlertWndOnStart == true)
            {
                ShowAlertWindow();
            }
        }

        private void ExitTrading()
        {
            OrderDataCollection.Clear();
            PendingCollection.Clear();
            CancelledOrderData.Clear();
            TradedOrderCollection.Clear();
            TradeCollection_MX.Clear();
            PositionDetailCollection.Clear();
            CapitalDataCollection.Clear();
        }

        public override void Closing(object sender, CancelEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                if (MessageBox.Show("您确定要退出交易应用么？", "注意", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    ExitTrading();//Environment.Exit(0);
                    JYDataServer.getServerInstance().AddToQryQueue(new CTPRequestContent("ClientLogOff", new List<object>()));
                    Login.IsTerminated = false;
                    JYDataServer.getServerInstance().getLoginControl().ClearToInit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                ExitTrading();//Environment.Exit(0);
                JYDataServer.getServerInstance().AddToQryQueue(new CTPRequestContent("ClientLogOff", new List<object>()));
            }
        }
    }
}
