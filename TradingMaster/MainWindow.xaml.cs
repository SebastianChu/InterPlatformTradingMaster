using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TradingMaster.CodeSet;
using TradingMaster.Control;
using TradingMaster.JYData;

namespace TradingMaster
{
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        internal MainWindow()
        {
            ResourceDictionary o = (ResourceDictionary)Application.LoadComponent(new Uri("/TradingMaster;component/Dictionary1.xaml", UriKind.Relative));
            Application.Current.Resources.MergedDictionaries.Add(o);
            InitializeComponent();
            InitControls();
            this.Title += string.Format("  【当前账号：{0}  经纪号：{1}  柜台：{2} 交易服务器：{3}  行情服务器：{4}】",
                DataContainer.GetUserInstance().GetCurrentInvestorID(), DataContainer.GetUserInstance().GetCurrentBroker(), DataContainer.GetUserInstance().GetCounter(),
                DataContainer.GetUserInstance().GetCurrentTradeAddress(), DataContainer.GetUserInstance().GetCurrentQuoteAddress());
            TradeDataClient.GetClientInstance().SetMainWindow(this);
            uscStatusBar.Init(this);
            _SystemTips = new SystemTips();
            _SystemTips.Init(this);
            _MutualExclusion = new Mutex();
            _MutexOption = new Mutex();
            ChangeServerConnectionStatus();
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
            capitalQuery.btnTransfer.Click += new RoutedEventHandler(btnTransfer_Click);

            uscQuoteOrder.Init(this);
            uscOptionExecInquiry.Init(this);
            uscExecOrderPanel.Init(this);
            uscPreviousOrder.Init(this);
            uscPreOrderRecord.Init(this);

            UscStatementsInquiry = new StatementsInquiry();
            UscInterTransfer = new InterTranfer();
            HQBackgroundRealData = new BackgroundDataServer();

            //if (TradingMaster.Properties.Settings.Default.SplitLargeOrderHandCount && uscPositionsInquiry != null)
            //{
            //    uscPositionsInquiry.btnSjfs.IsEnabled = false;
            //}
        }

        public void InitQuotesEvents()
        {
            uscHangqing.RealDataMouseLeftButtonDown += new FuturesQuotes.RealDataMouseLeftButtonDownDelegate(FuturesQuotes_RealDataMouseLeftButtonDown);
            uscHangqing.RealDataMouseDoubleClicked += new FuturesQuotes.RealDataMouseDoubleClickDelegate(FuturesQuotes_RealDataMouseDoubleClicked);
            uscHangqing.LvQuotesPanel.LevelRealDataMouseLeftButtonDown += new LevelsQuotes.LeftRealDataMouseLeftButtonDownDelegate(LvQuotesPanel_LevelRealDataMouseLeftButtonDown);

            uscOptionHangqing.RealDataMouseLeftButtonDown += new OptionQuotes.RealDataMouseLeftButtonDownDelegate(OptionQuotes_RealDataMouseLeftButtonDown);
            uscOptionHangqing.RealDataMouseDoubleClicked += new OptionQuotes.RealDataMouseDoubleClickDelegate(OptionQuotes_RealDataMouseDoubleClicked);
            uscOptionHangqing.LvOptQuotesPanel.LevelRealDataMouseLeftButtonDown += new LevelsQuotes.LeftRealDataMouseLeftButtonDownDelegate(LvOptQuotesPanel_LevelRealDataMouseLeftButtonDown);
        }

        public void ClearEvents()
        {
            uscHangqing.RealDataMouseLeftButtonDown -= FuturesQuotes_RealDataMouseLeftButtonDown;
            uscHangqing.RealDataMouseDoubleClicked -= FuturesQuotes_RealDataMouseDoubleClicked;
            uscHangqing.LvQuotesPanel.LevelRealDataMouseLeftButtonDown -= LvQuotesPanel_LevelRealDataMouseLeftButtonDown;
            uscOptionHangqing.RealDataMouseLeftButtonDown -= OptionQuotes_RealDataMouseLeftButtonDown;
            uscOptionHangqing.RealDataMouseDoubleClicked -= OptionQuotes_RealDataMouseDoubleClicked;
            uscOptionHangqing.LvOptQuotesPanel.LevelRealDataMouseLeftButtonDown -= LvOptQuotesPanel_LevelRealDataMouseLeftButtonDown;
            uscPositionsInquiry.PositionDataMouseLeftButtonDown -= uscPositionsInquiry_PositionDataMouseLeftButtonDown;
            uscPositionsInquiry.PositionDataMouseDoubleClicked -= uscPositionsInquiry_PositionDataMouseDoubleClicked;
        }

        void FuturesQuotes_RealDataMouseLeftButtonDown(string buyOrSell, RealData realData, double sJkpPrice, double sZjsPrice, double fixPrice, double sNewPrice, double sHighPrice, double sLowPrice, double sZspPrice, double sJspPrice, double sJsjPrice, double sDrjjPrice, bool isBuySell)
        {
            uscNewOrderPanel.SetOrderInfoByRealData(buyOrSell, realData, fixPrice, sNewPrice, sJkpPrice, sZjsPrice, sHighPrice, sLowPrice, sZspPrice, sJspPrice, sJsjPrice, sDrjjPrice, isBuySell);
        }

        void FuturesQuotes_RealDataMouseDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            uscNewOrderPanel.CancelSelectionOfCondtionAndMassOrder();
            uscNewOrderPanel.AddNewOrder();
        }

        void LvQuotesPanel_LevelRealDataMouseLeftButtonDown(string buyOrSell, RealData realData, double selectedPrice, bool isBuySell)
        {
            uscNewOrderPanel.SetOrderInfoByLevelRealData(buyOrSell, realData, selectedPrice, isBuySell);
        }

        void OptionQuotes_RealDataMouseLeftButtonDown(string buyOrSell, RealData realData, double sJkpPrice, double sZjsPrice, double fixPrice, double sNewPrice, double sHighPrice, double sLowPrice, double sZspPrice, double sJspPrice, double sJsjPrice, double sDrjjPrice, bool isBuySell)
        {
            uscNewOrderPanel.SetOrderInfoByRealData(buyOrSell, realData, fixPrice, sNewPrice, sJkpPrice, sZjsPrice, sHighPrice, sLowPrice, sZspPrice, sJspPrice, sJsjPrice, sDrjjPrice, isBuySell);
        }

        void OptionQuotes_RealDataMouseDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = CommonUtil.GetClickedCell(e);
            if (uscOptionHangqing != null)
            {
                //    if (cell.Column == uscOptionHangqing.colContract || cell.Column == uscOptionHangqing.colContractName)
                //    {
                //        //双击合约或者合约名显示分时图
                //        DisplayRealData record = cell.DataContext as DisplayRealData;
                //        string code = record.Code;

                //        HSMarketDataType market = CodeGen.CodeSet.GetMarketByCode(code);
                //        CodeInfo codeInfo = new CodeInfo(market, code);
                //        DocumentContent dc = new DocumentContent();
                //        dc.Title = code;
                //        dc.Closing += new EventHandler<CancelEventArgs>(dc_Closing);
                //        UserControl o = new IntradayCtrl(codeInfo);
                //        dc.Content = o;
                //    }
                //    else
                //    {
                //        //取消条件单和批量下单的选择
                //        //uscNewOrderPanel.CancelSelectionOfCondtionAndMassOrder();
                //        //uscNewOrderPanel.AddNewOrder();

                //        ///需要判断是平仓还是开仓(在量上双击是平仓，否则是开仓)
                if (cell.Column == uscOptionHangqing.colBuyCount_C || cell.Column == uscOptionHangqing.colSellCount_C)
                {
                    //平仓
                    //先查找持仓
                    TradeDataClient server = TradeDataClient.GetClientInstance();
                    if (server != null)
                    {
                        OptionRealData dr = cell.DataContext as OptionRealData;
                        if (dr != null)
                        {
                            string code = dr.Code_C;
                            TradeDataClient jyDataServer = TradeDataClient.GetClientInstance();
                            int todayPosCount;
                            int lastPosCount;
                            if (jyDataServer.GetPositionCount(code, cell.Column == uscOptionHangqing.colBuyCount_C,
                                out todayPosCount, out lastPosCount))
                            {
                                if (todayPosCount != 0 || lastPosCount != 0)
                                {
                                    ClosePosition(code, cell.Column != uscOptionHangqing.colBuyCount_C, todayPosCount, lastPosCount,
                                        cell.Column != uscOptionHangqing.colBuyCount_C ? dr.StSellPrice_C : dr.StBuyPrice_C);
                                }
                            }
                            //Util.Log("双击报价表的卖量或者买量上平仓");
                        }
                    }
                }
                else if (cell.Column == uscOptionHangqing.colBuyCount_P || cell.Column == uscOptionHangqing.colSellCount_P)
                {
                    //平仓
                    //先查找持仓
                    TradeDataClient server = TradeDataClient.GetClientInstance();
                    if (server != null)
                    {
                        OptionRealData dr = cell.DataContext as OptionRealData;
                        if (dr != null)
                        {
                            string code = dr.Code_P;
                            TradeDataClient jyDataServer = TradeDataClient.GetClientInstance();
                            int todayPosCount;
                            int lastPosCount;
                            if (jyDataServer.GetPositionCount(code, cell.Column == uscOptionHangqing.colBuyCount_P,
                                out todayPosCount, out lastPosCount))
                            {
                                if (todayPosCount != 0 || lastPosCount != 0)
                                {
                                    ClosePosition(code, cell.Column != uscOptionHangqing.colBuyCount_P, todayPosCount, lastPosCount,
                                        cell.Column != uscOptionHangqing.colBuyCount_P ? dr.StSellPrice_P : dr.StBuyPrice_P);
                                }
                            }
                            //Util.Log("双击报价表的卖量或者买量上平仓");
                        }
                    }
                }
                else
                {
                    //开仓
                    uscNewOrderPanel.CancelSelectionOfCondtionAndMassOrder();
                    uscNewOrderPanel.AddNewOrder();
                }
                //    }
            }
        }

        void LvOptQuotesPanel_LevelRealDataMouseLeftButtonDown(string buyOrSell, RealData realData, double selectedPrice, bool isBuySell)
        {
            uscNewOrderPanel.SetOrderInfoByLevelRealData(buyOrSell, realData, selectedPrice, isBuySell);
        }

        /// <summary>
        /// 平仓操作，以价格price平仓
        /// </summary>
        /// <param name="code"></param>
        /// <param name="todayPosCount"></param>
        /// <param name="lastPosCount"></param>
        /// <param name="price"></param>
        private void ClosePosition(string code, Boolean isBuy, int todayPosCount, int lastPosCount, double price)
        {
            Boolean bCandXiaDan = true;

            if (OrderAffirmWindow.G_OrderAffirmWindow == null)
            {
                new OrderAffirmWindow();
            }
            OrderAffirmWindow.G_OrderAffirmWindow.OrderAffirmItemCollection.Clear();
            string buySell = isBuy == true ? "买" : "卖";

            OrderAffirmItem ord = null;
            int closeCount = lastPosCount;      //需要平仓的量
            if (CodeSetManager.IsCloseTodaySupport(code) && todayPosCount != 0)
            {
                //如果是上海的品种，则先平今后平仓
                ord = new OrderAffirmItem();
                ord.InvestorID = DataContainer.GetUserInstance().InvestorID;
                ord.Code = code;
                ord.HandCount = todayPosCount;
                ord.Buysell = buySell;
                ord.Hedge = "投机";
                ord.OrderString = "下单:";
                ord.PosEffect = "平今";
                ord.Price = price;
                OrderAffirmWindow.G_OrderAffirmWindow.OrderAffirmItemCollection.Add(ord);
            }
            else
            {
                closeCount += todayPosCount;
            }
            if (closeCount != 0)
            {
                ord = new OrderAffirmItem();
                ord.InvestorID = DataContainer.GetUserInstance().InvestorID;
                ord.Code = code;
                ord.HandCount = closeCount;
                ord.Buysell = buySell;
                ord.Hedge = "投机";
                ord.OrderString = "下单:";
                ord.PosEffect = "平仓";
                ord.Price = price;
                OrderAffirmWindow.G_OrderAffirmWindow.OrderAffirmItemCollection.Add(ord);
            }


            //先判断是否需要下单确认
            if (IsNeedNotify() == true)
            {
                OrderAffirmWindow.G_OrderAffirmWindow.dataGrid1.ItemsSource = OrderAffirmWindow.G_OrderAffirmWindow.OrderAffirmItemCollection;
                OrderAffirmWindow.G_OrderAffirmWindow.ShowModalWindow();
                bCandXiaDan = OrderAffirmWindow.G_OrderAffirmWindow.IsConfirm;
                if (bCandXiaDan == true)
                {
                    if (OrderAffirmWindow.G_OrderAffirmWindow.chkConfirm.IsChecked == true)
                    {
                        TradingMaster.Properties.Settings.Default.ConfirmBeforeSendNewOrder = false;
                        //checkBoxNotify.IsChecked = false;
                        //checkBoxNotify.Content = "取消确认";
                        uscNewOrderPanel.SetBtnNotifyState(0);
                        TradingMaster.Properties.Settings.Default.Save();
                    }
                }
            }
            if (bCandXiaDan)
            {
                //如果可以下单，则下单

                Contract orderCodeInfo = CodeSetManager.GetContractInfo(code);
                PosEffect strKp = PosEffect.Close;
                EnumOrderType orderType = EnumOrderType.Limit;
                //if (uscNewOrderPanel.chbOrderType.IsChecked == true && uscNewOrderPanel.cbOrderType.SelectedItem != null)
                if (uscNewOrderPanel.cbOrderType.SelectedItem != null)
                {
                    orderType = CommonUtil.GetOrderType(uscNewOrderPanel.cbOrderType.SelectedValue.ToString());
                }
                if (OrderAffirmWindow.G_OrderAffirmWindow != null)
                {
                    if (OrderAffirmWindow.G_OrderAffirmWindow.OrderAffirmItemCollection != null)
                    {
                        foreach (OrderAffirmItem ord2 in OrderAffirmWindow.G_OrderAffirmWindow.OrderAffirmItemCollection)
                        {
                            if (ord2.PosEffect == "平仓")
                            {
                                strKp = PosEffect.Close;
                            }
                            else if (ord2.PosEffect == "平今")
                            {
                                strKp = PosEffect.CloseToday;
                            }
                            TradeDataClient.GetClientInstance().RequestOrder(ord2.InvestorID, BACKENDTYPE.CTP, new RequestContent("NewOrderSingle",
                                new List<object>() { orderCodeInfo, isBuy ? SIDETYPE.BUY : SIDETYPE.SELL,
                                strKp, price, ord2.HandCount, 0, "", 0, 0, 0, orderType, CommonUtil.GetHedgeType(ord2.Hedge) }));
                        }
                    }
                }
            }
        }

        public Boolean IsNeedNotify()
        {
            if (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control)
            {
                return false;
            }
            return TradingMaster.Properties.Settings.Default.ConfirmBeforeSendNewOrder;

        }

        public Window BankAmountQrywindow = null;
        public StatementsInquiry UscStatementsInquiry = null;
        public InterTranfer UscInterTransfer = null;
        public ChangePass UpdatePwd = null;
        public BackgroundDataServer HQBackgroundRealData;

        private Window _OrderReplayWindow = null;
        private Window _CapitalDetailWindow;
        private Window _InterTransferWindow;
        private Window _SystemMessageWindow = null;
        private SystemTips _SystemTips = null;
        private Mutex _MutualExclusion;
        private bool _IsWindowLoaded = false;
        private bool _IsNeedShowAlertWndOnStart = false;
        private bool _HasShowedRisk = false;
        private Mutex _MutexOption;

        private ObservableCollection<DisplayRealData> _RealDataCollection = new ObservableCollection<DisplayRealData>();
        public ObservableCollection<DisplayRealData> RealDataCollection
        {
            get { return _RealDataCollection; }
            set { _RealDataCollection = value; }
        }

        private ObservableCollection<DisplayRealData> _RealDataArbitrageCollection = new ObservableCollection<DisplayRealData>();
        public ObservableCollection<DisplayRealData> RealDataArbitrageCollection
        {
            get { return _RealDataArbitrageCollection; }
            set { _RealDataArbitrageCollection = value; }
        }

        private ObservableCollection<OptionRealData> _OptionRealDataCollection = new ObservableCollection<OptionRealData>();
        public ObservableCollection<OptionRealData> OptionRealDataCollection
        {
            get { return _OptionRealDataCollection; }
            set { _OptionRealDataCollection = value; }
        }

        private JYRealData _CapitalDataCollection = new JYRealData();

        public JYRealData CapitalDataCollection
        {
            get { return _CapitalDataCollection; }
            set { _CapitalDataCollection = value; }
        }

        /// <summary>
        /// 委托单
        /// </summary>
        private ObservableCollection<TradeOrderData> _OrderDataCollection = new ObservableCollection<TradeOrderData>();
        /// <summary>
        /// 挂单
        /// </summary>
        private ObservableCollection<TradeOrderData> _PendingCollection = new ObservableCollection<TradeOrderData>();
        /// <summary>
        /// 所有成交，明细
        /// </summary>
        private ObservableCollection<TradeOrderData> _TradeCollection_MX = new ObservableCollection<TradeOrderData>();

        /// <summary>
        /// 所有成交，按合约分组
        /// </summary>
        private ObservableCollection<TradeOrderData> _TradeCollection_Code = new ObservableCollection<TradeOrderData>();

        /// <summary>
        /// 所有成交委托单
        /// </summary>
        private ObservableCollection<TradeOrderData> _TradedOrderCollection = new ObservableCollection<TradeOrderData>();

        /// <summary>
        /// 持仓明细
        /// </summary>
        public ObservableCollection<PosInfoDetail> _PositionDetailCollection = new ObservableCollection<PosInfoDetail>();

        /// <summary>
        /// 持仓合计
        /// </summary>
        private ObservableCollection<PosInfoTotal> _PositionDetailCollection_Total = new ObservableCollection<PosInfoTotal>();

        /// <summary>
        /// 平仓查询
        /// </summary>
        //private ObservableCollection<CloseData> closeDataCollection = new ObservableCollection<CloseData>();

        /// <summary>
        /// 所有报价单
        /// </summary>
        private ObservableCollection<QuoteOrderData> _QuoteOrderDataCollection = new ObservableCollection<QuoteOrderData>();

        /// <summary>
        /// 成交报价单
        /// </summary>
        private ObservableCollection<QuoteOrderData> _QuoteTradedOrderDataCollection = new ObservableCollection<QuoteOrderData>();

        /// <summary>
        /// 挂单报价单
        /// </summary>
        private ObservableCollection<QuoteOrderData> _QuotePendingOrderDataCollection = new ObservableCollection<QuoteOrderData>();

        /// <summary>
        /// 已撤报价单
        /// </summary>
        private ObservableCollection<QuoteOrderData> _QuoteCancelledOrderDataCollection = new ObservableCollection<QuoteOrderData>();

        /// <summary>
        /// 所有行权申请
        /// </summary>
        private ObservableCollection<ExecOrderData> _ExecOrderDataCollection = new ObservableCollection<ExecOrderData>();

        /// <summary>
        /// 已取消行权申请
        /// </summary>
        private ObservableCollection<ExecOrderData> _ExecOrderCancelledDataCollection = new ObservableCollection<ExecOrderData>();

        /// <summary>
        /// 未执行行权申请
        /// </summary>
        private ObservableCollection<ExecOrderData> _ExecOrderPendingDataCollection = new ObservableCollection<ExecOrderData>();

        /// <summary>
        /// 所有埋单条件单
        /// </summary>
        private ObservableCollection<TradeOrderData> _PreConditionOrderData = new ObservableCollection<TradeOrderData>();

        /// <summary>
        /// 所有埋单条件单
        /// </summary>
        public ObservableCollection<TradeOrderData> PreConditionOrderData
        {
            get { return _PreConditionOrderData; }
            set { _PreConditionOrderData = value; }
        }

        /// <summary>
        /// 条件单
        /// </summary>
        private ObservableCollection<TradeOrderData> _ConditionOrderData = new ObservableCollection<TradeOrderData>();

        /// <summary>
        /// 条件单
        /// </summary>
        public ObservableCollection<TradeOrderData> ConditionOrderData
        {
            get { return _ConditionOrderData; }
            set { _ConditionOrderData = value; }
        }

        /// <summary>
        /// 预埋单
        /// </summary>
        private ObservableCollection<TradeOrderData> _PreOrderData = new ObservableCollection<TradeOrderData>();

        /// <summary>
        /// 预埋单
        /// </summary>
        public ObservableCollection<TradeOrderData> PreOrderData
        {
            get { return _PreOrderData; }
            set { _PreOrderData = value; }
        }

        /// <summary>
        /// 已发送埋单条件单
        /// </summary>
        private ObservableCollection<TradeOrderData> _SentOrderData = new ObservableCollection<TradeOrderData>();
        /// <summary>
        /// 已发送埋单条件单
        /// </summary>
        public ObservableCollection<TradeOrderData> SentOrderData
        {
            get { return _SentOrderData; }
            set { _SentOrderData = value; }
        }

        /// <summary>
        /// 已撤单
        /// </summary>
        private ObservableCollection<TradeOrderData> _CancelledOrderData = new ObservableCollection<TradeOrderData>();

        /// <summary>
        /// 已撤单
        /// </summary>
        public ObservableCollection<TradeOrderData> CancelledOrderData
        {
            get { return _CancelledOrderData; }
            set { _CancelledOrderData = value; }
        }

        /// <summary>
        /// 已成交单
        /// </summary>
        private ObservableCollection<TradeOrderData> _TransactionOrderData = new ObservableCollection<TradeOrderData>();

        /// <summary>
        /// 已成交单
        /// </summary>
        public ObservableCollection<TradeOrderData> TransactionOrderData
        {
            get { return _TransactionOrderData; }
            set { _TransactionOrderData = value; }
        }

        /// <summary>
        /// 平仓查询
        /// </summary>
        //public ObservableCollection<CloseData> CloseDataCollection
        //{
        //    get { return closeDataCollection; }
        //}

        /// <summary>
        /// 所有委托单
        /// </summary>
        public ObservableCollection<TradeOrderData> OrderDataCollection
        {
            get { return _OrderDataCollection; }
            set { _OrderDataCollection = value; }
        }

        /// <summary>
        /// 所有挂单（未成交）
        /// </summary>
        public ObservableCollection<TradeOrderData> PendingCollection
        {
            get { return _PendingCollection; }
            set { _PendingCollection = value; }
        }

        /// <summary>
        /// 所有成交委托单
        /// </summary>
        public ObservableCollection<TradeOrderData> TradedOrderCollection
        {
            get { return _TradedOrderCollection; }
            set { _TradedOrderCollection = value; }
        }

        /// <summary>
        /// 所有成交_合计
        /// </summary>
        public ObservableCollection<TradeOrderData> TradeCollection_Code
        {
            get { return _TradeCollection_Code; }
            set { _TradeCollection_Code = value; }
        }

        /// <summary>
        /// 所有成交_明细
        /// </summary>
        public ObservableCollection<TradeOrderData> TradeCollection_MX
        {
            get { return _TradeCollection_MX; }
            set { _TradeCollection_MX = value; }
        }

        /// <summary>
        /// 持仓明细
        /// </summary>
        public ObservableCollection<PosInfoDetail> PositionDetailCollection
        {
            get { return _PositionDetailCollection; }
            set { _PositionDetailCollection = value; }
        }

        /// <summary>
        /// 持仓合计
        /// </summary>
        public ObservableCollection<PosInfoTotal> PositionCollection_Total
        {
            get { return _PositionDetailCollection_Total; }
            set { _PositionDetailCollection_Total = value; }
        }

        //public HashSet<Contract> codeInfoset = new HashSet<Contract>();
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

        private ObservableCollection<TransferSingleRecord> transferRecordCollection = new ObservableCollection<TransferSingleRecord>();

        public ObservableCollection<TransferSingleRecord> TransferRecordCollection
        {
            get { return transferRecordCollection; }
            set { transferRecordCollection = value; }
        }

        /// <summary>
        /// 所有报价单
        /// </summary>
        public ObservableCollection<QuoteOrderData> QuoteOrderDataCollection
        {
            get { return _QuoteOrderDataCollection; }
            set { _QuoteOrderDataCollection = value; }
        }

        /// <summary>
        /// 成交报价单
        /// </summary>
        public ObservableCollection<QuoteOrderData> QuoteTradedOrderDataCollection
        {
            get { return _QuoteTradedOrderDataCollection; }
            set { _QuoteTradedOrderDataCollection = value; }
        }

        /// <summary>
        /// 挂单报价单
        /// </summary>
        public ObservableCollection<QuoteOrderData> QuotePendingOrderDataCollection
        {
            get { return _QuotePendingOrderDataCollection; }
            set { _QuotePendingOrderDataCollection = value; }
        }

        /// <summary>
        /// 已撤报价单
        /// </summary>
        public ObservableCollection<QuoteOrderData> QuoteCancelledOrderDataCollection
        {
            get { return _QuoteCancelledOrderDataCollection; }
            set { _QuoteCancelledOrderDataCollection = value; }
        }

        /// <summary>
        /// 所有行权申请
        /// </summary>
        public ObservableCollection<ExecOrderData> ExecOrderDataCollection
        {
            get { return _ExecOrderDataCollection; }
            set { _ExecOrderDataCollection = value; }
        }

        /// <summary>
        /// 已取消行权申请
        /// </summary>
        public ObservableCollection<ExecOrderData> ExecOrderCancelledDataCollection
        {
            get { return _ExecOrderCancelledDataCollection; }
            set { _ExecOrderCancelledDataCollection = value; }
        }

        /// <summary>
        /// 未执行行权申请
        /// </summary>
        public ObservableCollection<ExecOrderData> ExecOrderPendingDataCollection
        {
            get { return _ExecOrderPendingDataCollection; }
            set { _ExecOrderPendingDataCollection = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderInfo"></param>
        /// <param name="messageType">1 commit;2 transact; 3 cancel</param>
        public void HandleOrderInfo(TradeOrderData orderInfo)
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    try
                    {
                        if (_OrderReplayWindow == null)
                        {
                            OrderReplyMessageBox orderReplayMessageBox = new OrderReplyMessageBox();
                            orderReplayMessageBox.Init(this);
                            _OrderReplayWindow = CommonUtil.GetWindow("", orderReplayMessageBox, this);
                            _OrderReplayWindow.Closing += new CancelEventHandler(orderPreplayWindow_Closing);

                            if (orderReplayMessageBox.AddOrderInfo(orderInfo))
                            {
                                orderReplayMessageBox.Index = 0;
                                _OrderReplayWindow.Show();
                            }
                            return;
                        }

                        OrderReplyMessageBox orderReplay = _OrderReplayWindow.Content as OrderReplyMessageBox;
                        if (orderReplay.AddOrderInfo(orderInfo))
                        {
                            if (orderReplay.Index < 0)
                            {
                                orderReplay.Index = 0;
                            }
                            orderReplay.chkConfirmSubmit.IsChecked = false;
                            orderReplay.chkConfirmTransact.IsChecked = false;
                            orderReplay.chkConfirmCancel.IsChecked = false;

                            _OrderReplayWindow.Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.Log(ex.Message);
                        Util.Log(ex.StackTrace);
                    }
                });
            }
        }

        public void HandleOrderInfo(QuoteOrderData orderInfo)
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    try
                    {
                        if (_OrderReplayWindow == null)
                        {
                            OrderReplyMessageBox orderReplayMessageBox = new OrderReplyMessageBox();
                            orderReplayMessageBox.Init(this);
                            _OrderReplayWindow = CommonUtil.GetWindow("", orderReplayMessageBox, this);
                            _OrderReplayWindow.Closing += new CancelEventHandler(orderPreplayWindow_Closing);

                            if (orderReplayMessageBox.AddQuoteOrderInfo(orderInfo))
                            {
                                orderReplayMessageBox.Index = 0;
                                _OrderReplayWindow.Show();
                            }
                            return;
                        }

                        OrderReplyMessageBox orderReplay = _OrderReplayWindow.Content as OrderReplyMessageBox;
                        if (orderReplay.AddQuoteOrderInfo(orderInfo))
                        {
                            if (orderReplay.Index < 0)
                            {
                                orderReplay.Index = 0;
                            }
                            orderReplay.chkConfirmSubmit.IsChecked = false;
                            orderReplay.chkConfirmTransact.IsChecked = false;
                            orderReplay.chkConfirmCancel.IsChecked = false;

                            _OrderReplayWindow.Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.Log(ex.Message);
                        Util.Log(ex.StackTrace);
                    }
                });
            }
        }

        public void HandleOrderInfo(ExecOrderData orderInfo)
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    try
                    {
                        if (_OrderReplayWindow == null)
                        {
                            OrderReplyMessageBox orderReplayMessageBox = new OrderReplyMessageBox();
                            orderReplayMessageBox.Init(this);
                            _OrderReplayWindow = CommonUtil.GetWindow("", orderReplayMessageBox, this);
                            _OrderReplayWindow.Closing += new CancelEventHandler(orderPreplayWindow_Closing);

                            if (orderReplayMessageBox.AddExecOrderInfo(orderInfo))
                            {
                                orderReplayMessageBox.Index = 0;
                                _OrderReplayWindow.Show();
                            }
                            return;
                        }

                        OrderReplyMessageBox orderReplay = _OrderReplayWindow.Content as OrderReplyMessageBox;
                        if (orderReplay.AddExecOrderInfo(orderInfo))
                        {
                            if (orderReplay.Index < 0)
                            {
                                orderReplay.Index = 0;
                            }
                            orderReplay.chkConfirmSubmit.IsChecked = false;
                            orderReplay.chkConfirmTransact.IsChecked = false;
                            orderReplay.chkConfirmCancel.IsChecked = false;

                            _OrderReplayWindow.Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.Log(ex.Message);
                        Util.Log(ex.StackTrace);
                    }
                });
            }
        }

        void orderPreplayWindow_Closing(object sender, CancelEventArgs e)
        {
            _OrderReplayWindow.Hide();
            OrderReplyMessageBox orderReplayMessageBox = _OrderReplayWindow.Content as OrderReplyMessageBox;
            orderReplayMessageBox.Clear();
            e.Cancel = true;
        }

        internal void ShowSystemMessge()
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    if (_SystemMessageWindow == null)
                    {
                        _SystemMessageWindow = CommonUtil.GetWindow("日志记录", _SystemTips, this);
                        _SystemMessageWindow.ResizeMode = ResizeMode.CanResize;
                        _SystemMessageWindow.Closing += new CancelEventHandler(systemMessageWindow_Closing);
                    }
                    _SystemMessageWindow.Show();
                });
            }
        }

        void systemMessageWindow_Closing(object sender, CancelEventArgs e)
        {
            _SystemMessageWindow.Hide();
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
            CapitalDataCollection.Reserve = jyRealData.Reserve;
            CapitalDataCollection.DeliveryMargin = jyRealData.DeliveryMargin;
            CapitalDataCollection.Royalty = jyRealData.Royalty;
            CapitalDataCollection.FrozenRoyalty = jyRealData.FrozenRoyalty;

            if (!_HasShowedRisk)
            {
                if (jyRealData.RiskRatio > 80)
                {
                    if (_IsWindowLoaded == false)
                    {
                        _IsNeedShowAlertWndOnStart = true;
                    }
                    else
                    {
                        ShowAlertWindow();
                    }
                }
            }
            //money.Content = "可用:¥ " + jyRealData.TodayAvailable.ToString() + " 浮盈:¥ " + jyRealData.Dsfy.ToString() + " 平盈:¥ " + jyRealData.Dspy.ToString() + " 保证金:¥ " + jyRealData.Bond.ToString();

            if (_CapitalDetailWindow != null && _CapitalDetailWindow.Visibility == Visibility.Visible)
            {
                CapitalDetail capitalQuery = _CapitalDetailWindow.Content as CapitalDetail;
                if (capitalQuery != null)
                {
                    capitalQuery.SetJYRealData(CapitalDataCollection);
                }
            }
        }

        public void OnGotBankAmountDetail(BankAcctDetail bankAcctDetail)
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    if (BankAmountQrywindow != null)
                    {
                        BankAmountQry uscAmountQuery = BankAmountQrywindow.Content as BankAmountQry;
                        if (uscAmountQuery != null)
                        {
                            uscAmountQuery.SetBankAmountDetails(bankAcctDetail);
                        }
                        BankAmountQrywindow.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        BankAmountQry uscBankAmountQry = new BankAmountQry(bankAcctDetail);
                        BankAmountQrywindow = CommonUtil.GetWindow("银行资金账户", uscBankAmountQry, this);
                        BankAmountQrywindow.Closing += new System.ComponentModel.CancelEventHandler(bankAmountQrywindow_Closing);
                        BankAmountQrywindow.ResizeMode = ResizeMode.NoResize;
                        BankAmountQrywindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    }
                    BankAmountQrywindow.ShowDialog();
                });
            }
        }

        private void ShowAlertWindow()
        {
            if (CapitalDataCollection == null) return;
            JYRealData jyRealData = CapitalDataCollection;
            Login PreLogWindow = TradeDataClient.GetClientInstance().getLoginWindow();
            string userName = PreLogWindow.TbUserName.Text.ToString();
            string message = string.Format("尊敬的用户{0},您的风险级别为警示，风险度(客户保证金/总权益*100%)为{1}%。",
                userName, jyRealData.RiskRatio.ToString("0.00"));
            AddSystemMessage(DateTime.Now, message, "信息", "System");
            _HasShowedRisk = true;
        }

        void uscPositionsInquiry_PositionDataMouseLeftButtonDown(string buyOrSell, string kp, int num, RealData realData, string hedge)
        {
            uscNewOrderPanel.SetOrderInfoByPositionData(buyOrSell, kp, num, realData, hedge);
            uscExecOrderPanel.SetExecOrderInfoByPositionData(buyOrSell, kp, num, realData);
        }

        void uscPositionsInquiry_PositionDataMouseDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            uscNewOrderPanel.AddNewOrder();
        }

        private void CapitalDetail_Click(object sender, RoutedEventArgs e)
        {
            TradeDataClient.GetClientInstance().RequestTradeData("", BACKENDTYPE.CTP, new RequestContent("ReqCapital", new List<object>()));
            if (CapitalDataCollection != null)
            {
                if (_CapitalDetailWindow == null)
                {
                    CapitalDetail capitalQuery = new CapitalDetail();
                    capitalQuery.SetJYRealData(CapitalDataCollection);
                    _CapitalDetailWindow = CommonUtil.GetWindow("期货资金账户详情", capitalQuery, this);
                    _CapitalDetailWindow.Closing += new System.ComponentModel.CancelEventHandler(capitalDetailWindow_Closing);
                    _CapitalDetailWindow.Show();
                }
                else
                {
                    CapitalDetail capitalQuery = _CapitalDetailWindow.Content as CapitalDetail;
                    capitalQuery.SetJYRealData(CapitalDataCollection);
                    _CapitalDetailWindow.Visibility = Visibility.Visible;
                }
            }
        }


        private void btnTransfer_Click(object sender, RoutedEventArgs e)
        {
            //JYDataServer.getServerInstance().InitTransferFromAPI();
            if (_InterTransferWindow == null)
            {
                if (UscInterTransfer == null)
                {
                    UscInterTransfer = new InterTranfer();
                }
                UscInterTransfer.Init(this);
                UscInterTransfer.FuturesCapitalQueryClick += CapitalDetail_Click;
                _InterTransferWindow = CommonUtil.GetWindow("银期转账", UscInterTransfer, this);
                _InterTransferWindow.Closing += new CancelEventHandler(InterTransferWindow_Closing);
                _InterTransferWindow.ResizeMode = ResizeMode.CanMinimize;
                _InterTransferWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            else
            {
                //CapitalDetail capitalQuery = capitalDetailWindow.Content as CapitalDetail;
                //capitalQuery.SetJYRealData(CapitalDataCollection);
                //capitalDetailWindow.Visibility = Visibility.Visible;
            }
            _InterTransferWindow.Show();
        }

        void capitalDetailWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _CapitalDetailWindow.Visibility = Visibility.Collapsed;
            e.Cancel = true;
        }

        void InterTransferWindow_Closing(object sender, CancelEventArgs e)
        {
            _InterTransferWindow.Visibility = Visibility.Collapsed;
            e.Cancel = true;
        }

        void bankAmountQrywindow_Closing(object sender, CancelEventArgs e)
        {
            BankAmountQrywindow.Visibility = Visibility.Collapsed;
            e.Cancel = true;
        }

        public void AddSystemMessage(DateTime feedbackTime, string feedbackMessage, string messageLevel, string messageClass)
        {
            AddSystemMessage(feedbackTime.ToString(SystemMessage.FeedBackTimeFormat), feedbackMessage, messageLevel, messageClass);
        }

        public void AddSystemMessage(DateTime feedbackTime, string feedbackMessage)
        {
            AddSystemMessage(feedbackTime.ToString(SystemMessage.FeedBackTimeFormat), feedbackMessage, "信息", "System");
        }

        private void AddSystemMessage(string feedbackTime, string feedbackMessage, string messageLevel, string messageClass)
        {
            try
            {
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        AddMessageToCollection(feedbackTime, feedbackMessage, messageLevel, messageClass);
                    });
                }
            }
            catch (Exception ex)
            {
                Util.Log(ex.Message);
                Util.Log(ex.StackTrace);
            }
        }

        private void AddMessageToCollection(string feedbackTime, string feedbackMessage, string messageLevel, string messageClass)
        {
            _MutualExclusion.WaitOne();
            try
            {
                //Todo: 存疑
                if (SystemMessageCollection.Count > 0
                    && SystemMessageCollection[SystemMessageCollection.Count - 1].FeedbackMessage.Trim().Equals(feedbackMessage.Trim())
                    && !(feedbackMessage.Contains("撤单通知") || feedbackMessage.Contains("综合交易平台"))
                   )
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
            catch (Exception ex)
            {
                Util.Log(ex.Message);
                Util.Log(ex.StackTrace);
            }
            _MutualExclusion.ReleaseMutex();
        }

        /// <summary>
        /// 改变交易服务器状态
        /// </summary>
        /// <param name="isConnected"></param>
        public void ChangeServerConnectionStatus()
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
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
        /// 请求行情或者主推数据
        /// </summary>
        public void RequestSnapShotPlusUpdate(List<string> codeList)
        {
            if (codeList.Count > 0)
            {
                List<object> args = new List<object>();
                args.Add(codeList.ToArray());
                TradeDataClient.GetClientInstance().RequestMarketData("", BACKENDTYPE.CTP, new RequestContent("ReqMarketData", args));
                TradeDataClient.GetClientInstance().RequestMarketData("", BACKENDTYPE.CTP, new RequestContent("ReqQuotes", args));
            }
        }

        /// <summary>
        /// 接收到行情或者主推数据
        /// </summary>
        public void OnReceiveSnapShotOrUpdateCallBack(Dictionary<Contract, RealData> realDataDict)
        {
            if (HQBackgroundRealData == null)
            {
                HQBackgroundRealData = new BackgroundDataServer();
            }
            HQBackgroundRealData.ChangeType(realDataDict);

            if (uscHangqing != null)
            {
                if (uscHangqing.HQRealData != null)
                {
                    uscHangqing.HQRealData.ChangeType(realDataDict);
                }

                if (uscHangqing.GroupHQRealData != null)
                {
                    uscHangqing.GroupHQRealData.ChangeType(realDataDict);
                }
            }

            if (uscOptionHangqing != null)
            {
                if (uscOptionHangqing.OptionQuotesRealData != null)
                {
                    uscOptionHangqing.OptionQuotesRealData.ChangeType(realDataDict);
                }
            }
            //分档行情
            //DisplayRealData levelQuote = new DisplayRealData();
            //levelQuote.UpdateProperties(realData);
            //uscHangqing.SetLevelsQuotesByRealData(levelQuote);
        }

        /// <summary>
        /// 取消行情或者主推数据
        /// </summary>
        public void ClearUpdate(List<string> codeList)
        {
            if (codeList.Count > 0)
            {
                List<object> args = new List<object>();
                args.Add(codeList.ToArray());
                TradeDataClient.GetClientInstance().RequestMarketData("", BACKENDTYPE.CTP, new RequestContent("CancelMarketData", args));
            }
        }

        //public void updateFuturesDataByDisplayRealData(RealData realData)
        //{
        //mutexOption.WaitOne();
        //uscHangqing.updateFuturesDataByDisplayRealData(realData);
        //mutexOption.ReleaseMutex();
        //}

        public void updateOptionDataByDisplayRealData(RealData realData)
        {
            _MutexOption.WaitOne();
            uscOptionHangqing.UpdateOptionRelatedDataByRealData(realData);
            //DataContainer.AddRealDataToContainer(realData);
            _MutexOption.ReleaseMutex();
        }

        private void Window_loaded(object sender, RoutedEventArgs e)
        {
            //InitControls();
            _IsWindowLoaded = true;
            if (_IsNeedShowAlertWndOnStart == true)
            {
                ShowAlertWindow();
            }
        }

        public void MainWindow_ChangeUserCodeEvent(ObservableCollection<Contract> newCodes)
        {
            //bool flag = TradingMaster.Properties.Settings.Default.IsPricePanel;
            //if (flag)
            {
                //根据合约代码 创建报价单
                //uscPricePanel.Init(this);
                //uscPricePanel.RealDataMouseLeftButtonDown += new RealDataMouseLeftButtonDownDelegate2(uscHangqing_RealDataMouseLeftButtonDown2);
                //uscPricePanel.RemovePricePanel();
                //foreach (var item in newCodes)
                //{
                //    uscPricePanel.CreatePricePanel(item.StrCode);
                //}
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
            PositionCollection_Total.Clear();
            CapitalDataCollection.Clear();
            SystemMessageCollection.Clear();
            UscStatementsInquiry.txtStatementOrder.Text = "";
            ClearEvents();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                if (MessageBox.Show("您确定要退出交易应用么？", "注意", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    ExitTrading();//Environment.Exit(0);
                    //JYDataServer.getServerInstance().AddToQryQueue(new CTPRequestContent("ClientLogOff", new List<object>()));
                    TradeDataClient.GetClientInstance().RequestTradeData("", BACKENDTYPE.CTP, new RequestContent("RequestTradeDataDisConnect", new List<object>()));
                    TradeDataClient.GetClientInstance().RequestMarketData("", BACKENDTYPE.CTP, new RequestContent("RequestMarketDataDisConnect", new List<object>()));
                    Login.IsTerminated = false;
                    DataContainer.GetUserInstance().getLoginWindow().ClearToInit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                ExitTrading();//Environment.Exit(0);
                //JYDataServer.getServerInstance().AddToQryQueue(new CTPRequestContent("ClientLogOff", new List<object>()));
                TradeDataClient.GetClientInstance().RequestTradeData("", BACKENDTYPE.CTP, new RequestContent("RequestTradeDataDisConnect", new List<object>()));
                TradeDataClient.GetClientInstance().RequestMarketData("", BACKENDTYPE.CTP, new RequestContent("RequestMarketDataDisConnect", new List<object>()));
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
