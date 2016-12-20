using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TradingMaster.CodeSet;
using TradingMaster.JYData;

namespace TradingMaster.Control
{
    /// <summary>
    /// NewOrderPanel.xaml 的交互逻辑
    /// </summary>
    public partial class NewOrderPanel : UserControl
    {
        public static readonly int PriceTypeFixPrice = 0;
        public static readonly int PriceTypeAutoFixPrice = 1;
        public static readonly int PriceTypeAutoPrice = 10;

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong(IntPtr hMenu, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        public static SolidColorBrush UpBrush = new SolidColorBrush((Color)(ColorConverter.ConvertFromString("#FF0000")));
        public static SolidColorBrush DownBrush = new SolidColorBrush((Color)(ColorConverter.ConvertFromString("#008000")));
        public static SolidColorBrush DefaultBrush = new SolidColorBrush((Color)(ColorConverter.ConvertFromString("#000000")));

        public TradeClientStyleUI CurrentClientStyleUI
        {
            get { return CommonStaticMemeber.CurrentClientStyleUI; }
        }

        private Cursor _DefaultCursor;
        private LevelsQuotes _LvQuotes = new LevelsQuotes();
        private int _PriceType;
        /// <summary>
        /// 价格类型：0、手动指定价；1、自动指定价（可自动切换为跟盘价）；10、跟盘价；；
        /// 
        /// 1. 手动点击可进行指定价和跟盘价的切换，此时只会转换成手动指定价；手动指定价一旦选择，只有点击“指定价”才会切换到跟盘价，没有其他办法。
        /// 2. 而通过行情，或者更改下单价格会从跟盘价切换到自动指定价
        /// 3. 通过选择行情数据会从自动指定价切换到跟盘价
        /// 
        /// </summary>
        public int PriceType
        {
            set
            {
                if (value == PriceTypeFixPrice || value == PriceTypeAutoFixPrice)
                {
                    tbPriceType.Text = "指定价";
                    bdPriceType.BorderThickness = new Thickness(1);
                    tbPriceType.Foreground = Brushes.Black;
                }
                else
                {
                    tbPriceType.Text = "跟盘价";
                    bdPriceType.BorderThickness = new Thickness(1, 1, 0, 0);
                    tbPriceType.Foreground = Brushes.Red;

                    //btnXiadan.Focus();

                    foreach (var item in _MainWindow.HQBackgroundRealData.RealDataList)
                    {
                        if (item.CodeInfo.Code == txtCode.Text.Trim())
                        {
                            if (rbBuy.IsChecked == true)
                            {
                                dudPrice.Value = item.AskPrice[0];
                            }
                            else
                            {
                                dudPrice.Value = item.BidPrice[0];
                            }
                            if (dudPrice.Value == 0)
                            {
                                dudPrice.Value = item.PrevSettlementPrice;
                            }

                            break;
                        }
                    }
                }

                _PriceType = value;
            }
            get { return _PriceType; }
        }
        private MainWindow _MainWindow { get; set; }

        /// <summary>
        /// 设置默认手数的对话框
        /// </summary>
        private DefaultHandNum _DefaultHandNumDlg;

        public NewOrderPanel()
        {
            CommonStaticMemeber.LoadStyleFromUserFile();
            this.DataContext = this;
            InitializeComponent();
            InitControls();
            _DefaultCursor = this.Cursor;
            _DefaultHandNumDlg = null;
            tbPriceType.MouseEnter += new MouseEventHandler(tbPriceType_MouseEnter);
            tbPriceType.MouseLeave += new MouseEventHandler(tbPriceType_MouseLeave);
            GetDefaultHandCountFromFile();

            if (TradingMaster.Properties.Settings.Default.SplitLargeOrderHandCount)
            {
                cbxSplit.IsChecked = true;
            }
            else
            {
                cbxSplit.IsChecked = false;
            }
        }

        void tbPriceType_MouseLeave(object sender, MouseEventArgs e)
        {
            if (PriceType == PriceTypeFixPrice || PriceType == PriceTypeAutoFixPrice)
            {
                tbPriceType.Foreground = Brushes.Black;
            }
            else
            {
                tbPriceType.Foreground = Brushes.Red;
            }
        }

        void tbPriceType_MouseEnter(object sender, MouseEventArgs e)
        {
            if (PriceType == PriceTypeFixPrice || PriceType == PriceTypeAutoFixPrice)
            {
                tbPriceType.Foreground = new SolidColorBrush(Color.FromRgb(128, 64, 0));
            }
            else
            {
                tbPriceType.Foreground = Brushes.OrangeRed;
            }
        }

        public void Init(MainWindow parent)
        {
            this._MainWindow = parent;
        }

        private void InitControls()
        {
            _LvQuotes.Init(this._MainWindow);
            _LvQuotes.lblBuyCount1.MouseDoubleClick += new MouseButtonEventHandler(lblBuyCount1_MouseDoubleClick);
            _LvQuotes.lblBuy1.MouseDoubleClick += new MouseButtonEventHandler(lblBuyCount1_MouseDoubleClick);
            _LvQuotes.lblBuyDesc1.MouseDoubleClick += new MouseButtonEventHandler(lblBuyCount1_MouseDoubleClick);

            _LvQuotes.lblSellCount1.MouseDoubleClick += new MouseButtonEventHandler(lblSellCount1_MouseDoubleClick);
            _LvQuotes.lblSell1.MouseDoubleClick += new MouseButtonEventHandler(lblSellCount1_MouseDoubleClick);
            _LvQuotes.lblSellDesc1.MouseDoubleClick += new MouseButtonEventHandler(lblSellCount1_MouseDoubleClick);

            rbBuy.Checked += new RoutedEventHandler(rbBuy_Checked);
            rbSell.Checked += new RoutedEventHandler(rbSell_Checked);
            rbKaicang.Checked += new RoutedEventHandler(rbKaicang_Checked);
            rbPingcang.Checked += new RoutedEventHandler(rbPingcang_Checked);
            rbPingjin.Checked += new RoutedEventHandler(rbPingjin_Checked);
            rbSpeculation.Checked += new RoutedEventHandler(rbSpeculation_Checked);
            rbHedge.Checked += new RoutedEventHandler(rbHedge_Checked);
            rbArbitrage.Checked += new RoutedEventHandler(rbArbitrage_Checked);

            SetPriceTextBlockEvent(tbSellPrice);
            SetPriceTextBlockEvent(tbBuyPrice);
            SetPriceTextBlockEvent(tbMaxPrice);
            SetPriceTextBlockEvent(tbMinPrice);
            tbHandCount.MouseLeftButtonDown += new MouseButtonEventHandler(tbHandCount_MouseLeftButtonDown);
        }

        private void SetPriceTextBlockEvent(TextBlock tb)
        {
            tb.MouseLeftButtonUp += new MouseButtonEventHandler(tbPrice_MouseLeftButtonUp);
            tb.MouseEnter += new MouseEventHandler(tbPrice_MouseEnter);
            tb.MouseLeave += new MouseEventHandler(tbPrice_MouseLeave);
        }

        void tbPrice_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = _DefaultCursor;
        }

        void tbPrice_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        void tbPrice_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double price = CommonUtil.GetDoubleValue((sender as TextBlock).Text);
            dudPrice.Value = price;
        }

        void tbHandCount_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int handCount = CommonUtil.GetIntValue((sender as TextBlock).Text);
            iudNum.Value = handCount;
        }

        void lblSellCount1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            rbBuy.IsChecked = true;
            dudPrice.Value = CommonUtil.GetDoubleValue(_LvQuotes.lblBuy1.Content);

            txtCode.Text = _LvQuotes.lblCode.Content.ToString();
            iudNum.Value = 0;

            SetAvailableCapitalAndPosition();
        }

        /// <summary>
        /// 根据分档行情进行卖出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lblBuyCount1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            rbSell.IsChecked = true;
            dudPrice.Value = CommonUtil.GetDoubleValue(_LvQuotes.lblSell1.Content);

            txtCode.Text = _LvQuotes.lblCode.Content.ToString();
            iudNum.Value = 0;

            SetAvailableCapitalAndPosition();
        }

        private void SetAvailableCapitalAndPosition()
        {
            //设置可用金额和买持卖持
            if (_MainWindow.CapitalDataCollection != null)
            {
                int buyCount = 0;
                int sellCount = 0;
                foreach (var item in _MainWindow.PositionCollection_Total)
                {
                    if (item.Code == txtCode.Text)
                    {
                        if (item.BuySell.Contains("买"))
                        {
                            buyCount += CommonUtil.GetIntValue(item.TotalPosition);
                        }
                        else
                        {
                            sellCount += CommonUtil.GetIntValue(item.TotalPosition);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 通过选择行情设置下单数据
        /// </summary>
        /// <param name="buyOrSell"></param>
        /// <param name="realData"></param>
        public void SetOrderInfoByExistingOrder(TradeOrderData orderData)
        {
            if (orderData.BuySell.Contains("买"))
            {
                rbBuy.IsChecked = true;
            }
            else
            {
                rbSell.IsChecked = true;
            }

            if (orderData.OpenClose == "开仓")
            {
                rbKaicang.IsChecked = true;
            }
            else if (orderData.OpenClose == "平今")
            {
                rbPingjin.IsChecked = true;
            }
            else if (orderData.OpenClose == "平仓")
            {
                rbPingcang.IsChecked = true;
            }



            //dudPrice.Value = CommonUtil.GetDoubleValue(realData.INewPrice);
            if (PriceType != PriceTypeFixPrice)
            {
                PriceType = PriceTypeAutoFixPrice;
            }

            txtCode.Text = orderData.Code;
            iudNum.Value = orderData.CommitHandCount;
            dudPrice.Value = orderData.CommitPrice;


            SetAvailableCapitalAndPosition();
        }

        /// <summary>
        /// 通过选择行情设置下单数据
        /// </summary>
        /// <param name="buyOrSell"></param>
        /// <param name="realData"></param>
        public void SetOrderInfoByRealData(string buyOrSell, RealData realData, double fixPrice, double sNewPrice, double sJkpPrice, double sZjsPrice, double sHighPrice, double sLowPrice, double sZspPrice, double sJspPrice, double sJsjPrice, double sDrjjPrice, Boolean isBuySellColumn)
        {
            if (buyOrSell == "买")
            {
                rbBuy.IsChecked = true;
                rbKaicang.IsChecked = true;
            }
            else if (buyOrSell == "卖")
            {
                rbSell.IsChecked = true;
                rbKaicang.IsChecked = true;
            }

            if (rbBuy.IsChecked == true)
            {
                dudPrice.Value = realData.AskPrice[0];//.StSellPrice;
                if (PriceType == PriceTypeAutoFixPrice)
                {
                    PriceType = PriceTypeAutoPrice;
                }
            }
            else
            {
                dudPrice.Value = realData.BidPrice[0];//.StBuyPrice;
                if (PriceType == PriceTypeAutoFixPrice)
                {
                    PriceType = PriceTypeAutoPrice;
                }
            }
            //dudPrice.Value = CommonUtil.GetDoubleValue(realData.INewPrice);


            //点击行情可能会要求使用指定价下单
            if (fixPrice != 0)
            {
                dudPrice.Value = fixPrice;
                if (PriceType != PriceTypeFixPrice)
                {
                    PriceType = PriceTypeAutoFixPrice;
                }
            }
            //点击最新价
            if (sNewPrice != 0)
            {
                dudPrice.Value = sNewPrice;
                if (PriceType != PriceTypeFixPrice)
                {
                    PriceType = PriceTypeAutoFixPrice;
                }
            }
            //点击今开盘
            if (sJkpPrice != 0)
            {
                dudPrice.Value = sJkpPrice;
                if (_PriceType != PriceTypeFixPrice)
                {
                    PriceType = PriceTypeAutoFixPrice;
                }
            }
            //点击昨结算
            if (sZjsPrice != 0)
            {
                dudPrice.Value = sZjsPrice;
                if (_PriceType != PriceTypeFixPrice)
                {
                    PriceType = PriceTypeAutoFixPrice;
                }
            }
            //点击最高价
            if (sHighPrice != 0)
            {
                dudPrice.Value = sHighPrice;
                if (_PriceType != PriceTypeFixPrice)
                {
                    PriceType = PriceTypeAutoFixPrice;
                }
            }
            //点击最低价
            if (sLowPrice != 0)
            {
                dudPrice.Value = sLowPrice;
                if (_PriceType != PriceTypeFixPrice)
                {
                    PriceType = PriceTypeAutoFixPrice;
                }
            }
            //点击昨收盘
            if (sZspPrice != 0)
            {
                dudPrice.Value = sZspPrice;
                if (_PriceType != PriceTypeFixPrice)
                {
                    PriceType = PriceTypeAutoFixPrice;
                }
            }
            //点击今收盘
            if (sJspPrice != 0)
            {
                dudPrice.Value = sJspPrice;
                if (_PriceType != PriceTypeFixPrice)
                {
                    PriceType = PriceTypeAutoFixPrice;
                }
            }
            //点击结算价
            if (sJsjPrice != 0)
            {
                dudPrice.Value = sJsjPrice;
                if (_PriceType != PriceTypeFixPrice)
                {
                    PriceType = PriceTypeAutoFixPrice;
                }
            }
            //点击当日均价
            if (sDrjjPrice != 0)
            {
                dudPrice.Value = sDrjjPrice;
                if (_PriceType != PriceTypeFixPrice)
                {
                    PriceType = PriceTypeAutoFixPrice;
                }
            }
            txtCode.Text = realData.CodeInfo.Code;

            if (isBuySellColumn == false)
            {
                iudNum.Value = 1;
            }
            else if (iudNum.Value == null)
            {
                iudNum.Value = 1;
            }
            else if (iudNum.Value == null || iudNum.Value == 0)
            {
                iudNum.Value = 1;
            }
            SetExtendsInfo(realData);
            SetAvailableCapitalAndPosition();
        }

        /// <summary>
        /// 通过选择多档行情设置下单数据
        /// </summary>
        /// <param name="buyOrSell"></param>
        /// <param name="realData"></param>
        public void SetOrderInfoByLevelRealData(string buyOrSell, RealData realData, double selectedPrice, Boolean isBuySellColumn)
        {
            if (buyOrSell == "买")
            {
                rbBuy.IsChecked = true;
                rbKaicang.IsChecked = true;

                dudPrice.Value = selectedPrice;
                if (PriceType == PriceTypeAutoPrice)
                {
                    PriceType = PriceTypeAutoFixPrice;
                }
            }
            else if (buyOrSell == "卖")
            {
                rbSell.IsChecked = true;
                rbKaicang.IsChecked = true;

                dudPrice.Value = selectedPrice;
                if (PriceType == PriceTypeAutoPrice)
                {
                    PriceType = PriceTypeAutoFixPrice;
                }
            }

            txtCode.Text = realData.CodeInfo.Code;

            if (isBuySellColumn == false)
            {
                iudNum.Value = 1;
            }
            else if (iudNum.Value == null)
            {
                iudNum.Value = 1;
            }
            else if (iudNum.Value == null || iudNum.Value == 0)
            {
                iudNum.Value = 1;
            }
            SetExtendsInfo(realData);
            SetAvailableCapitalAndPosition();
        }


        /// <summary>
        /// 取消条件单和批量单的选择
        /// </summary>
        public void CancelSelectionOfCondtionAndMassOrder()
        {
            //checkBoxCondition.IsChecked = false;
            //checkBoxMass.IsChecked = false;
        }

        /// <summary>
        /// 下单
        /// </summary>
        /// <param name="buyOrSell"></param>
        /// <param name="realData"></param>
        public void AddNewOrder()
        {
            OrderInsert_General(0);
        }


        /// <summary>
        /// 通过选择持仓设置下单数据
        /// </summary>
        /// <param name="buyOrSell"></param>
        /// <param name="kp"></param>
        /// <param name="num"></param>
        /// <param name="realData"></param>
        public void SetOrderInfoByPositionData(string buyOrSell, string kp, int num, RealData realData, string hedge)
        {
            if (buyOrSell == "买")
            {
                rbBuy.IsChecked = true;
                dudPrice.Value = CommonUtil.GetDoubleValue(realData.AskPrice[0]);
            }
            else if (buyOrSell == "卖")
            {
                rbSell.IsChecked = true;
                dudPrice.Value = CommonUtil.GetDoubleValue(realData.BidPrice[0]);
            }
            else
            {
                dudPrice.Value = CommonUtil.GetDoubleValue(realData.NewPrice);
            }

            if (kp == "平仓")
            {
                rbPingcang.IsChecked = true;
            }
            else if (kp == "平今")
            {
                rbPingjin.IsChecked = true;
            }

            //if (txtCode.Text != realData.Code)
            //{
            //    txtMaxPrice.Text = txtMinPrice.Text = string.Empty;
            //}
            if (hedge.Contains("套保"))
            {
                rbHedge.IsChecked = true;
            }
            else if (hedge.Contains("套利"))
            {
                rbArbitrage.IsChecked = true;
            }
            else
            {
                rbSpeculation.IsChecked = true;
            }

            txtCode.Text = realData.CodeInfo.Code;
            needUpdatePriceCode = string.Empty;
            iudNum.Value = num;
            SetHandCount();
            //tbHandCount.Text = num.ToString();
            SetExtendsInfo(realData);
            SetAvailableCapitalAndPosition();
        }

        private void btnMaidan_Click(object sender, RoutedEventArgs e)
        {
            //XiaDan_General(2);
            string verifyMessage = VerifyOrderDataMessage();
            if (verifyMessage == string.Empty)
            {
                string strCode = txtCode.Text;
                Contract orderCodeIndo = CodeSetManager.GetContractInfo(strCode);
                if (orderCodeIndo == null)
                {
                    Util.Log("Invalid Contract code in Previous Order Control: " + strCode);
                    return;
                }
                SIDETYPE isBuy = rbBuy.IsChecked == true ? SIDETYPE.BUY : SIDETYPE.SELL;//tb_buyAndSell.Text;
                PosEffect strKp = PosEffect.Open;
                string kp = "开仓";

                double price = CommonUtil.GetDoubleValue(dudPrice.Value.Value);
                int handCount = CommonUtil.GetIntValue(iudNum.Value.Value);

                if (rbPingcang.IsChecked == true)
                {
                    strKp = PosEffect.Close;
                    kp = "平仓";
                }
                else if (rbPingjin.IsChecked == true)
                {
                    strKp = PosEffect.CloseToday;
                    kp = "平今";
                }

                string buySell = rbBuy.IsChecked == true ? "买" : "卖";

                string resultText = string.Format("按{0}价格, {1} {2} {3} {4}手",
                    price.ToString(), buySell, strCode, kp, handCount);

                ParkedAndConditionOrderPanel PreAndCondition = new ParkedAndConditionOrderPanel(this.dudPrice.FormatString);
                PreAndCondition.txtResult.Text = resultText;
                PreAndCondition.dudPrice.Value = price;
                PreAndCondition.dudPrice.Increment = this.dudPrice.Increment;
                PreAndCondition.dudPrice.FormatString = this.dudPrice.FormatString;
                Window maidanWindow = CommonUtil.GetWindow("设置触发条件", PreAndCondition, this._MainWindow);
                if (maidanWindow.ShowDialog() == true)
                {
                    int isAuto = PreAndCondition.GetOrderType();
                    int touchMethod = PreAndCondition.GetTouchMethod();
                    int touchCondition = PreAndCondition.GetTouchCondition();
                    double touchPrice = PreAndCondition.GetTouchPrice();
                    if (!CodeSetManager.IsCloseTodaySupport(strCode) && strKp == PosEffect.CloseToday)
                    {
                        strKp = PosEffect.Close;
                    }
                    //CtpDataServer.getServerInstance().AddToOrderQueue(new RequestContent("NewPreOrderSingle", new List<object>() { orderCodeIndo.Code, isBuy, strKp, price, handCount, isAuto, "", touchMethod, touchCondition, touchPrice }));
                    TradeDataClient.GetClientInstance().RequestOrder("", BACKENDTYPE.CTP, new RequestContent("NewPreOrderSingle", new List<object>() { orderCodeIndo.Code, isBuy, strKp, price, handCount, isAuto, "", touchMethod, touchCondition, touchPrice }));
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(verifyMessage, "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }

        }

        private void btnXiadan_Click(object sender, RoutedEventArgs e)
        {
            OrderInsert_General(0);
        }

        /// <summary>
        /// 下单，比易胜简单
        /// </summary>
        /// <param name="isAuto"></param>
        private void OrderInsert_General(int isAuto)
        {
            string verifyMessage = VerifyOrderDataMessage();
            if (verifyMessage == string.Empty)
            {
                string strCode = txtCode.Text.Trim();
                Contract orderCodeIndo = CodeSetManager.GetContractInfo(strCode);
                SIDETYPE isBuy = rbBuy.IsChecked == true ? SIDETYPE.BUY : SIDETYPE.SELL;//tb_buyAndSell.Text;
                PosEffect strKp = PosEffect.Open;
                string kp = "开仓";

                //有的时候dudPrice的控件在进行上下改动价格的时候会不按照Increment浮动，会出现很多小数，在使用时需要重新Round一下
                double price = CommonUtil.GetDoubleValue(dudPrice.Value.Value);
                int handCount = CommonUtil.GetIntValue(iudNum.Value.Value);

                if (rbPingcang.IsChecked == true)
                {
                    strKp = PosEffect.Close;
                    kp = "平仓";
                }
                else if (rbPingjin.IsChecked == true)
                {
                    strKp = PosEffect.CloseToday;
                    kp = "平今";
                }

                int touchMethod = 0;
                int touchCondition = 0;
                double touchPrice = 0;
                if (dudTriggerPrice.Value != null)
                {
                    touchPrice = CommonUtil.GetDoubleValue(dudTriggerPrice.Value.Value);
                }
                EnumOrderType orderType = EnumOrderType.Limit;
                //if (chbOrderType.IsChecked == true && cbOrderType.SelectedItem != null)
                if (cbOrderType.SelectedItem != null)
                {
                    orderType = CommonUtil.GetOrderType(cbOrderType.SelectedValue.ToString());
                }

                EnumHedgeType hedge = GetSelectedHedgeType();
                string strHedge = CommonUtil.GetHedgeString(hedge);
                if (TradingMaster.Properties.Settings.Default.ConfirmBeforeSendNewOrder == true)
                {
                    string buySell = rbBuy.IsChecked == true ? "买" : "卖";
                    //string text = new StringBuilder();
                    CheckableMessageBox messageBox = new CheckableMessageBox();
                    messageBox.tbMessage.Text = string.Format("下单：{0} {1}  {2} {3}手 于价格{4} {5}",
                        kp, buySell, strCode, handCount, price.ToString(), strHedge);

                    //if (chbOrderType.IsChecked == true && cbOrderType.SelectedItem != null)
                    if (cbOrderType.SelectedItem != null)
                    {
                        messageBox.tbMessage.Text += " 报单指令：" + cbOrderType.SelectedValue.ToString().Split(':')[1];
                    }

                    if (price == 0 && orderCodeIndo.ProductType != "Combination")
                    {
                        messageBox.tbMessage.Text = messageBox.tbMessage.Text + " (价格为0时以市价单发出)";
                    }

                    string windowTitle = string.Format("确认下单：价格{0},{1} {2}  {3} {4}手 {5}",
                        price.ToString(), kp, buySell, strCode, handCount, strHedge);
                    Window confirmWindow = CommonUtil.GetWindow(windowTitle, messageBox, _MainWindow);

                    if (confirmWindow.ShowDialog() == true)
                    {
                        if (messageBox.chkConfirm.IsChecked == true)
                        {
                            TradingMaster.Properties.Settings.Default.ConfirmBeforeSendNewOrder = false;
                            TradingMaster.Properties.Settings.Default.Save();
                        }
                        if (!CodeSetManager.IsCloseTodaySupport(strCode) && strKp == PosEffect.CloseToday)
                        {
                            strKp = PosEffect.Close;
                        }
                        //单腿市价单和组合0价报单的区别
                        double orderPrice = price;
                        if (price == 0 && orderCodeIndo.ProductType != "Combination")
                        {
                            if (isBuy == SIDETYPE.BUY)
                            {
                                orderPrice = CommonUtil.GetDoubleValue(tbMaxPrice.Text);
                            }
                            else if (isBuy == SIDETYPE.SELL)
                            {
                                orderPrice = CommonUtil.GetDoubleValue(tbMinPrice.Text);
                            }
                        }
                        else
                        {
                            orderPrice = price;
                        }
                        //CtpDataServer.getServerInstance().AddToOrderQueue(new RequestContent("NewOrderSingle", new List<object>() { orderCodeIndo, isBuy, strKp, orderPrice, handCount, isAuto, "", touchMethod, touchCondition, touchPrice, orderType }));
                        TradeDataClient.GetClientInstance().RequestOrder("", BACKENDTYPE.CTP, new RequestContent("NewOrderSingle", new List<object>() { orderCodeIndo, isBuy, strKp, orderPrice, handCount, isAuto, "", touchMethod, touchCondition, touchPrice, orderType, hedge }));
                    }
                }
                else
                {
                    if (!CodeSetManager.IsCloseTodaySupport(strCode) && strKp == PosEffect.CloseToday)
                    {
                        strKp = PosEffect.Close;
                    }
                    if (orderCodeIndo.ProductType != "Combination")
                    {
                        if (price != 0 ||
                            (price == 0 && MessageBox.Show("价格为0时以市价单发出", "确认下单", MessageBoxButton.OKCancel) == MessageBoxResult.OK))
                        {
                            double orderPrice = price;
                            if (price == 0)
                            {
                                if (isBuy == SIDETYPE.BUY)
                                {
                                    orderPrice = CommonUtil.GetDoubleValue(tbMaxPrice.Text);
                                }
                                else if (isBuy == SIDETYPE.SELL)
                                {
                                    orderPrice = CommonUtil.GetDoubleValue(tbMinPrice.Text);
                                }
                            }
                            else
                            {
                                orderPrice = price;
                            }
                            TradeDataClient.GetClientInstance().RequestOrder("", BACKENDTYPE.CTP, new RequestContent("NewOrderSingle", new List<object>() { orderCodeIndo, isBuy, strKp, orderPrice, handCount, isAuto, "", touchMethod, touchCondition, touchPrice, orderType, hedge }));
                        }
                    }
                    else
                    {
                        TradeDataClient.GetClientInstance().RequestOrder("", BACKENDTYPE.CTP, new RequestContent("NewOrderSingle", new List<object>() { orderCodeIndo, isBuy, strKp, price, handCount, isAuto, "", touchMethod, touchCondition, touchPrice, orderType, hedge }));
                    }
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(verifyMessage, "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        private EnumHedgeType GetSelectedHedgeType()
        {
            EnumHedgeType hedge = EnumHedgeType.Speculation;
            if (rbArbitrage.IsChecked == true)
            {
                hedge = EnumHedgeType.Arbitrage;
            }
            else if (rbHedge.IsChecked == true)
            {
                hedge = EnumHedgeType.Hedge;
            }
            return hedge;
        }

        private string VerifyOrderDataMessage()
        {
            string verifyMessage = string.Empty;

            if (txtCode.Text == "")
            {
                verifyMessage = verifyMessage + "合约代码不能为空!\n";
            }
            else if (!CommonUtil.IsValidCode(txtCode.Text))
            {
                verifyMessage = verifyMessage + "合约代码不合法!\n";
            }
            else if (dudPrice.Value == null)
            {
                verifyMessage = verifyMessage + "委托价格不能为空!\n";
            }
            //double o;
            //if (double.TryParse(txtPrice.Text, out o) == false)
            //{
            //    verifyMessage = verifyMessage + "委托价格格式错误!\n";
            //}
            else if (iudNum.Value == null || iudNum.Value.Value == 0)
            {
                verifyMessage = verifyMessage + "委托数量不能为空或为0!\n";
            }
            //else
            //{
            //    int result;
            //    if (int.TryParse(txtNum.Text, out result) == false)
            //    {
            //        verifyMessage = verifyMessage + "委托数量格式错误!\n";
            //    }
            //}
            else if (dudPrice.Value != null)
            {
                double hycs = 0;
                decimal fluct = 0;
                if (true == CodeSetManager.GetHycsAndFluct(txtCode.Text, out hycs, out fluct))
                {
                    double price = CommonUtil.GetDoubleValue(dudPrice.Value.Value); ;
                    //double.TryParse(txtPrice.Text, out price);
                    //CalculatorUpDown
                    if (price != 0)
                    {
                        int price1000 = (int)Math.Round(price * 1000);
                        int fluct1000 = (int)Math.Round(fluct * 1000);
                        if ((double)price1000 / 1000 != price)
                        {
                            verifyMessage = verifyMessage + "委托价格必须为" + fluct + "的倍数\n";
                        }
                        else
                        {
                            if ((price1000) % (fluct1000) != 0)
                            {
                                verifyMessage = verifyMessage + "委托价格必须为" + fluct + "的倍数\n";
                            }
                        }
                    }
                }
            }

            return verifyMessage;
        }

        string needUpdatePriceCode = string.Empty;
        private void txtCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Util.Log("txtCode_TextChanged starts.");
            txtCode.SelectionStart = txtCode.Text.Length;
            int firstNumberIndiex = txtCode.Text.IndexOfAny("0123456789".ToCharArray());
            string SpeciesName = txtCode.Text;
            string SpeciesDate = string.Empty;
            if (firstNumberIndiex >= 0)
            {
                SpeciesName = txtCode.Text.Substring(0, firstNumberIndiex);
                SpeciesDate = txtCode.Text.Substring(firstNumberIndiex, txtCode.Text.Length - firstNumberIndiex);
                //}

                //用户可能不区分大小写输入合约
                string validSpeciesName = CodeSetManager.GetValidSpeciesName(SpeciesName);
                if (validSpeciesName != null && validSpeciesName != SpeciesName)
                {
                    txtCode.Text = validSpeciesName + SpeciesDate;
                    return;
                }
            }
            if (CommonUtil.IsValidCode(txtCode.Text.Trim()))
            {
                Contract newCode = CodeSetManager.GetContractInfo(txtCode.Text.Trim());
                txtCodeName.Text = newCode.Name;
                if (newCode.ProductType == "Combination")
                {
                    dudPrice.Minimum = -dudPrice.Maximum;
                }
                else
                {
                    dudPrice.Minimum = 0;
                }
                iudNum.Value = DefaultCodeHandInstance.GetDefaultCodeHand(txtCode.Text);
                //PriceType不变
                int oldPriceType = PriceType;
                //dudPrice.Value = 0;
                PriceType = oldPriceType;

                SetHandCount();

                _LvQuotes.lblCode.Content = txtCode.Text;

                double hycs = 0;
                decimal fluct = 0;
                CodeSetManager.GetHycsAndFluct(txtCode.Text, out hycs, out fluct);
                dudPrice.Increment = fluct;
                dudPrice.FormatString = CommonUtil.PriceFormatString((double)dudPrice.Increment);//TODO: digit error for decimal?

                //bool hasItemInRealDataCollection = false;
                //foreach (var item in mainWindow.RealDataCollection)
                //{
                //    if (item.Code == txtCode.Text)
                //    {
                //        mainWindow.uscOptionHangqing.SelectDataByCode(txtCode.Text);
                //        hasItemInRealDataCollection = true;
                //        break;
                //    }
                //}
                //if (!hasItemInRealDataCollection)
                //{
                //    mainWindow.uscOptionHangqing.AddExternalHqingData(new DisplayRealData() { Code = txtCode.Text, Name = CodeSet.GetCodeName(txtCode.Text) });
                //    mainWindow.uscOptionHangqing.SelectDataByCode(txtCode.Text);
                //}

                needUpdatePriceCode = txtCode.Text;
                if (_MainWindow.HQBackgroundRealData._CommObj.RequestingCodes.Contains(txtCode.Text))
                {
                    //已经在推送数据，直接请求涨跌停数据和已有的深度行情数据
                    foreach (var item in _MainWindow.HQBackgroundRealData.RealDataList)
                    {
                        if (item.CodeInfo.Code == txtCode.Text)
                        {
                            SetExtendsInfo(item);
                            break;
                        }
                    }
                }
                else
                {
                    _MainWindow.HQBackgroundRealData.AddSingleRealData(txtCode.Text);
                    _MainWindow.HQBackgroundRealData.RemoveUselessRequest();
                }
            }
            //Util.Log("txtCode_TextChanged ends.");            
        }

        public void SetHandCount()
        {
            tbHandCount.Text = "0";
            string strCode = txtCode.Text;
            if (!CommonUtil.IsValidCode(txtCode.Text))
            {
                return;
            }
            Contract orderCodeIndo = CodeSetManager.GetContractInfo(strCode);
            SIDETYPE isBuy = rbBuy.IsChecked == true ? SIDETYPE.BUY : SIDETYPE.SELL;

            double price = 0;
            //先从行情取最新价
            RealData realData = DataContainer.GetRealDataFromContainer(orderCodeIndo);
            if (realData != null)
            {
                price = realData.NewPrice;
            }

            //还没用取到，使用用户输入的价格
            if (price == 0)
            {
                if (dudPrice.Value != null)
                {
                    price = CommonUtil.GetDoubleValue(dudPrice.Value.Value);
                }
            }

            if (rbKaicang.IsChecked == true)
            {
                TradeDataClient.GetClientInstance().RequestTradeData("", BACKENDTYPE.CTP, new RequestContent("RequsetMaxOperation", new List<object>() { orderCodeIndo, isBuy, PosEffect.Open, price, GetSelectedHedgeType(), 0 }));
            }
            else if (rbPingcang.IsChecked == true)
            {
                string buySell = rbBuy.IsChecked == true ? "卖" : "买";
                foreach (var item in _MainWindow.PositionCollection_Total)
                {
                    if (txtCode.Text == item.Code && item.BuySell.Contains(buySell))
                    {
                        if (CodeSetManager.IsCloseTodaySupport(item.Code))
                        {
                            tbHandCount.Text = item.CanCloseCount.ToString(); //(item.YesterdayPosition - item.FreezeCount).ToString();
                        }
                        else
                        {
                            tbHandCount.Text = item.CanCloseCount.ToString(); //(item.TotalPosition - item.FreezeCount).ToString();
                        }

                        int limitCount = 0;
                        if (item.CanCloseCount > orderCodeIndo.MaxLimitOrderVolume)
                        {
                            limitCount = orderCodeIndo.MaxLimitOrderVolume;
                        }
                        else
                        {
                            limitCount = item.CanCloseCount;
                        }
                        if (CodeSetManager.MaxOperationHandCountDict.ContainsKey(orderCodeIndo))
                        {
                            CodeSetManager.MaxOperationHandCountDict[orderCodeIndo] = limitCount;
                        }
                        else
                        {
                            CodeSetManager.MaxOperationHandCountDict.Add(orderCodeIndo, limitCount);
                        }
                    }
                }
            }
            else if (rbPingjin.IsChecked == true)
            {
                string buySell = rbBuy.IsChecked == true ? "卖" : "买";
                foreach (var item in _MainWindow.PositionCollection_Total)
                {
                    if (txtCode.Text == item.Code && item.BuySell.Contains(buySell))
                    {
                        if (CodeSetManager.IsCloseTodaySupport(item.Code))
                        {
                            tbHandCount.Text = item.CanCloseCount.ToString(); //(item.TodayPosition - item.FreezeCount).ToString();
                        }
                        else
                        {
                            tbHandCount.Text = item.CanCloseCount.ToString(); //(item.TotalPosition - item.FreezeCount).ToString();
                        }

                        int limitCount = 0;
                        if (item.CanCloseCount > orderCodeIndo.MaxLimitOrderVolume)
                        {
                            limitCount = orderCodeIndo.MaxLimitOrderVolume;
                        }
                        else
                        {
                            limitCount = item.CanCloseCount;
                        }
                        if (CodeSetManager.MaxOperationHandCountDict.ContainsKey(orderCodeIndo))
                        {
                            CodeSetManager.MaxOperationHandCountDict[orderCodeIndo] = limitCount;
                        }
                        else
                        {
                            CodeSetManager.MaxOperationHandCountDict.Add(orderCodeIndo, limitCount);
                        }
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            iudNum.Value = 0;
        }

        public void UpdateMaxOperation(MaxOperation maxOperation)
        {
            //if (rbKaicang.IsChecked == true && txtCode.Text == maxOperation.codeInfo.Code)
            if (txtCode.Text == maxOperation.CodeInfo.Code && maxOperation.HedgeType == GetSelectedHedgeType())
            {
                tbHandCount.Text = maxOperation.Count.ToString();
            }
        }

        private void rbBuy_Checked(object sender, RoutedEventArgs e)
        {
            SetHandCount();
            UpdateAutoPrice();
        }

        private void rbSell_Checked(object sender, RoutedEventArgs e)
        {
            SetHandCount();
            UpdateAutoPrice();
        }

        private void rbKaicang_Checked(object sender, RoutedEventArgs e)
        {
            SetHandCount();
        }

        private void rbPingjin_Checked(object sender, RoutedEventArgs e)
        {
            SetHandCount();
        }

        private void rbSpeculation_Checked(object sender, RoutedEventArgs e)
        {
            SetHandCount();
        }

        private void rbPingcang_Checked(object sender, RoutedEventArgs e)
        {
            SetHandCount();
        }

        private void rbHedge_Checked(object sender, RoutedEventArgs e)
        {
            SetHandCount();
        }

        private void rbArbitrage_Checked(object sender, RoutedEventArgs e)
        {
            SetHandCount();
        }

        /// <summary>
        /// 设置买1卖1的价格和数量
        /// </summary>
        /// <param name="m_RealData"></param>
        public void SetExtendsInfo(RealData m_RealData)
        {
            if (txtCode.Text == m_RealData.CodeInfo.Code)
            {
                string priceFormat = CommonUtil.GetPriceFormat(m_RealData.CodeInfo.Code);
                tbSellPrice.Text = m_RealData.AskPrice[0].ToString(priceFormat);
                tbBuyPrice.Text = m_RealData.BidPrice[0].ToString(priceFormat);
                tbSellCount.Text = "/ " + m_RealData.AskHand[0].ToString();
                tbBuyCount.Text = "/ " + m_RealData.BidHand[0].ToString();

                tbMaxPrice.Text = m_RealData.UpperLimitPrice.ToString(priceFormat);
                tbMinPrice.Text = m_RealData.LowerLimitPrice.ToString(priceFormat);

                int oldPriceType = PriceType;
                if (PriceType == PriceTypeAutoPrice || needUpdatePriceCode == txtCode.Text)
                {
                    if (rbBuy.IsChecked == true)
                    {
                        dudPrice.Value = m_RealData.AskPrice[0];
                    }
                    else
                    {
                        dudPrice.Value = m_RealData.BidPrice[0];
                    }
                    if (dudPrice.Value == 0)
                    {
                        dudPrice.Value = m_RealData.PrevSettlementPrice;
                    }
                    needUpdatePriceCode = string.Empty;
                }
                PriceType = oldPriceType;
            }
        }

        /// <summary>
        /// 设置涨跌停的价格和数量
        /// </summary>
        /// <param name="m_displayRealData"></param>
        //public void SetMaxMinPrice(string code, double zt, double dt)
        //{
        //    if (mainWindow.uscNewOrderPanel.txtCode.Text == code)
        //    {
        //        string priceFormat = CommonUtil.GetPriceFormat(code);
        //        tbMaxPrice.Text = zt.ToString(priceFormat);
        //        tbMinPrice.Text = dt.ToString(priceFormat);
        //    }
        //}




        private void dudPrice_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (PriceType != PriceTypeFixPrice)
            {
                PriceType = PriceTypeAutoFixPrice;
            }
        }


        private void UpdateAutoPrice()
        {
            if (PriceType == PriceTypeAutoPrice)
            {
                foreach (var item in _MainWindow.HQBackgroundRealData.RealDataList)
                {
                    if (item.CodeInfo.Code == txtCode.Text.Trim())
                    {
                        if (rbBuy.IsChecked == true)
                        {
                            dudPrice.Value = item.AskPrice[0];
                        }
                        else
                        {
                            dudPrice.Value = item.BidPrice[0];
                        }
                        break;
                    }
                }
                PriceType = PriceTypeAutoPrice;
            }
        }

        private void tbPriceType_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (PriceType == PriceTypeFixPrice || PriceType == PriceTypeAutoFixPrice)
            {
                foreach (var item in _MainWindow.HQBackgroundRealData.RealDataList)
                {
                    if (item.CodeInfo.Code == txtCode.Text.Trim())
                    {
                        if (rbBuy.IsChecked == true)
                        {
                            dudPrice.Value = item.AskPrice[0];
                        }
                        else
                        {
                            dudPrice.Value = item.BidPrice[0];
                        }
                        break;
                    }
                }
                PriceType = PriceTypeAutoPrice;
            }
            else
            {
                PriceType = PriceTypeFixPrice;
            }
        }

        private void txtCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter
                || e.Key == Key.Tab
                || e.Key == Key.Down)
            {
                FocusToRbBuySell();
            }
            else if (e.Key == Key.Up)
            {
                btnXiadan.Focus();
            }
            else
            {
                return;
            }

            e.Handled = true;
        }

        private void FocusToRbBuySell()
        {
            if (rbBuy.IsChecked.Value)
            {
                rbBuy.Focus();
            }
            else
            {
                rbSell.Focus();
            }
        }

        private void rbBuy_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownByRbBuySell(sender, e);
        }

        private void rbSell_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownByRbBuySell(sender, e);
        }

        private void KeyDownByRbBuySell(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter
                || e.Key == Key.Tab
                || e.Key == Key.Down)
            {
                FocusToRbKp();
            }
            else if (e.Key == Key.Up)
            {
                txtCode.Focus();
            }
            else if (e.Key == Key.Left
                || e.Key == Key.Right)
            {
                RadioButton rbNext = rbSell;
                if (sender == rbSell)
                {
                    rbNext = rbBuy;
                }
                rbNext.Focus();
                rbNext.IsChecked = true;
            }
            else if (e.Key == Key.NumPad1 || e.Key == Key.D1)
            {
                RadioButton rbNext = rbBuy;
                rbNext.Focus();
                rbNext.IsChecked = true;
            }
            else if (e.Key == Key.NumPad3 || e.Key == Key.D3)
            {
                RadioButton rbNext = rbSell;
                rbNext.Focus();
                rbNext.IsChecked = true;
            }

            e.Handled = true;
        }

        private void FocusToRbKp()
        {
            if (rbKaicang.IsChecked.Value)
            {
                rbKaicang.Focus();
            }
            else if (rbPingcang.IsChecked.Value)
            {
                rbPingcang.Focus();
            }
            else
            {
                rbPingjin.Focus();
            }
        }

        private void rbKaicang_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownByRbKp(sender, e);
        }

        private void KeyDownByRbKp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter
                || e.Key == Key.Tab
                || e.Key == Key.Down)
            {
                iudNum.Focus();
            }
            else if (e.Key == Key.Up)
            {
                FocusToRbBuySell();
            }
            else if (e.Key == Key.Right)
            {
                RadioButton rbNext = rbPingjin;

                if (sender == rbPingjin)
                {
                    rbNext = rbPingcang;
                }
                else if (sender == rbPingcang)
                {
                    rbNext = rbKaicang;
                }
                rbNext.Focus();
                rbNext.IsChecked = true;
            }
            else if (e.Key == Key.Left)
            {
                RadioButton rbNext = rbPingcang;

                if (sender == rbPingcang)
                {
                    rbNext = rbPingjin;
                }
                else if (sender == rbPingjin)
                {
                    rbNext = rbKaicang;
                }
                rbNext.Focus();
                rbNext.IsChecked = true;
            }
            else if (e.Key == Key.D1 || e.Key == Key.NumPad1)
            {
                RadioButton rbNext = rbKaicang;
                rbNext.Focus();
                rbNext.IsChecked = true;
            }
            else if (e.Key == Key.D2 || e.Key == Key.NumPad2)
            {
                RadioButton rbNext = rbPingjin;
                rbNext.Focus();
                rbNext.IsChecked = true;
            }
            else if (e.Key == Key.D3 || e.Key == Key.NumPad3)
            {
                RadioButton rbNext = rbPingcang;
                rbNext.Focus();
                rbNext.IsChecked = true;
            }
            e.Handled = true;
        }

        private void rbPingjin_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownByRbKp(sender, e);
        }

        private void rbPingcang_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDownByRbKp(sender, e);
        }

        private void iudNum_KeyDown(object sender, KeyEventArgs e)
        {
            int iudNumValue = 0;
            if (iudNum.Value != null)
            {
                iudNumValue = CommonUtil.GetIntValue(iudNum.Value.Value);
            }

            if (e.Key == Key.Enter
                || e.Key == Key.Tab
                || e.Key == Key.Down)
            {
                dudPrice.Focus();
            }
            else if (e.Key == Key.Up)
            {
                FocusToRbKp();
            }
            else if (e.Key == Key.Left)
            {
                iudNum.Value = (double)iudNumValue - (double)iudNum.Increment;
            }
            else if (e.Key == Key.Right)
            {
                iudNum.Value = (double)iudNumValue + (double)iudNum.Increment;
            }
            else
            {
                return;
            }

            e.Handled = true;
        }

        private void dudPrice_KeyDown(object sender, KeyEventArgs e)
        {
            double dudPriceValue = 0;
            if (dudPrice.Value != null)
            {
                dudPriceValue = CommonUtil.GetDoubleValue(dudPrice.Value.Value);
            }

            if (e.Key == Key.Enter
                || e.Key == Key.Down
                || e.Key == Key.Tab)
            {
                //btnXiadan.Focus();
                cbOrderType.Focus();
            }
            else if (e.Key == Key.Up)
            {
                iudNum.Focus();
            }
            else if (e.Key == Key.Left)
            {
                dudPrice.Value = dudPriceValue - (double)dudPrice.Increment;
            }
            else if (e.Key == Key.Right)
            {
                dudPrice.Value = dudPriceValue + (double)dudPrice.Increment;
            }
            else
            {
                return;
            }
            e.Handled = true;
        }

        private void dudTriggerPrice_KeyDown(object sender, KeyEventArgs e)
        {
            double dudPriceValue = 0;
            if (dudTriggerPrice.Value != null)
            {
                dudPriceValue = CommonUtil.GetDoubleValue(dudTriggerPrice.Value.Value);
            }

            if (e.Key == Key.Enter
                || e.Key == Key.Down
                || e.Key == Key.Tab)
            {
                btnXiadan.Focus();
            }
            else if (e.Key == Key.Up)
            {
                iudNum.Focus();
            }
            else if (e.Key == Key.Left)
            {
                dudTriggerPrice.Value = dudPriceValue - (double)dudTriggerPrice.Increment;
            }
            else if (e.Key == Key.Right)
            {
                dudTriggerPrice.Value = dudPriceValue + (double)dudTriggerPrice.Increment;
            }
            else
            {
                return;
            }
            e.Handled = true;
        }

        private void btnXiadan_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up
                || e.Key == Key.Left)
            {
                //dudPrice.Focus();
                if (rbSpeculation.IsChecked == true)
                {
                    rbSpeculation.Focus();
                }
                else if (rbHedge.IsChecked == true)
                {
                    rbHedge.Focus();
                }
                else if (rbArbitrage.IsChecked == true)
                {
                    rbArbitrage.Focus();
                }
                e.Handled = true;
                return;
            }
            else if (e.Key == Key.Tab
                || e.Key == Key.Down)
            {
                txtCode.Focus();
                e.Handled = true;
            }

            if (e.Key == Key.Right)
            {
                iudNum.Focus();
            }
        }

        private void txtCode_LostFocus(object sender, RoutedEventArgs e)
        {
            //Util.Log("txtCode_LostFocus starts");
            txtCode.Text = txtCode.Text.Trim();

            double hycs = 0;
            decimal fluct = 0;
            if (true == CodeSetManager.GetHycsAndFluct(txtCode.Text, out hycs, out fluct))
            {

                //if (plc.SubMarket.IsCloseTodaySupport(txtCode.Text))
                //{
                //    //如果是上海的
                //    toggleBtn_PJ.Visibility = Visibility.Visible;
                //}
                //else
                //{
                //    //如果是非上海的
                //    toggleBtn_PJ.Visibility = Visibility.Collapsed;
                //}


                if (0 != ((int)(fluct * 1000)) % 10)
                {
                    //小数点后三位
                    dudPrice.FormatString = "F3";
                }
                else if (0 != ((int)(fluct * 100)) % 10)
                {
                    //小数点后两位
                    dudPrice.FormatString = "F2";
                }
                else if (0 != ((int)(fluct * 10)) % 10)
                {
                    //小数点后一位
                    dudPrice.FormatString = "F1";
                }
                else
                {
                    //无小数点
                    dudPrice.FormatString = "F0";
                }
            }
            //Util.Log("txtCode_LostFocus ends");
        }

        private void btnNotify_Checked(object sender, RoutedEventArgs e)
        {
            TradingMaster.Properties.Settings.Default.ConfirmBeforeSendNewOrder = true;
        }

        private void btnNotify_Unchecked(object sender, RoutedEventArgs e)
        {
            TradingMaster.Properties.Settings.Default.ConfirmBeforeSendNewOrder = false;
        }

        /// <summary>
        /// 设置提示确认按钮的状态,state为1则表示红色的选中，－1则表示蓝色的选中，0表示未选中
        /// </summary>
        /// <param name="state"></param>
        public void SetBtnNotifyState(int state)
        {
            if (state == 1)
            {
                btnNotify.IsChecked = true;
            }
            else if (state == -1)
            {
                btnNotify.IsChecked = true;
            }
            else
            {
                btnNotify.IsChecked = false;
            }
        }

        public void NewOrderFocus()
        {
            rbBuy.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (null == _DefaultHandNumDlg)
            {
                _DefaultHandNumDlg = new DefaultHandNum();
            }

            _DefaultHandNumDlg.SetHandCount(
                DefaultHandNum.HandCountStringToNum(btnHand1.Content.ToString()),
                DefaultHandNum.HandCountStringToNum(btnHand2.Content.ToString()),
                DefaultHandNum.HandCountStringToNum(btnHand3.Content.ToString()));

            try
            {
                _DefaultHandNumDlg.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            int handCount1, handCount2, handCount3;
            _DefaultHandNumDlg.GetHandCount(out handCount1, out handCount2, out handCount3);

            SetDefaultHandCountToButton(handCount1, handCount2, handCount3);
            SaveBackToFile();
        }

        private void btnHand_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int handCount1 = DefaultHandNum.HandCountStringToNum(btn.Content.ToString());
            if (handCount1 >= 0)
            {
                iudNum.Value = handCount1;
            }
            else
            {
                if (tbHandCount.Text != null && tbHandCount.Text != "0")
                {
                    int maxHand = int.Parse(tbHandCount.Text);

                    handCount1 = -handCount1;
                    if (handCount1 > 100)
                    {
                        handCount1 = 0;
                    }
                    iudNum.Value = maxHand * handCount1 / 100;
                }
            }
        }

        private void SetDefaultHandCountToButton(int handCount1, int handCount2, int handCount3)
        {
            btnHand1.Content = DefaultHandNum.HandCountNumToString(handCount1);
            btnHand2.Content = DefaultHandNum.HandCountNumToString(handCount2);
            btnHand3.Content = DefaultHandNum.HandCountNumToString(handCount3);
        }

        void GetDefaultHandCountFromFile()
        {
            string fileName = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)
                + "/setting/DefaultHandCount.txt";
            if (File.Exists(fileName) == false)
            {
                SetDefaultHandCountToButton(1, 5, 10);
            }
            else
            {
                FileStream fs = new FileStream(fileName, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string content = sr.ReadLine();
                sr.Close();
                fs.Close();
                string[] fields = content.Split(';');
                if (fields.Length < 3)
                {
                    SetDefaultHandCountToButton(1, 5, 10);
                    return;
                }

                int handCount1 = 1;
                int handCount2 = 5;
                int handCount3 = 10;
                int.TryParse(fields[0], out handCount1);
                int.TryParse(fields[1], out handCount2);
                int.TryParse(fields[2], out handCount3);

                SetDefaultHandCountToButton(handCount1, handCount2, handCount3);
            }
        }

        private void SaveBackToFile()
        {
            int handCount1 = DefaultHandNum.HandCountStringToNum(btnHand1.Content.ToString());
            int handCount2 = DefaultHandNum.HandCountStringToNum(btnHand2.Content.ToString());
            int handCount3 = DefaultHandNum.HandCountStringToNum(btnHand3.Content.ToString());

            string fileName = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)
               + "/setting/DefaultHandCount.txt";
            FileStream fs = new FileStream(fileName, FileMode.Create);
            string s = handCount1 + ";" + handCount2 + ";" + handCount3;
            byte[] bytes = System.Text.Encoding.Default.GetBytes(s);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        private void cbOrderType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cbOrderType.SelectedIndex == 2)//市价
            if (cbOrderType.SelectedIndex == 1)//市价
            {
                dudPrice.Value = 0;
                dudPrice.IsEnabled = false;
                TriggerRow.Height = new GridLength(0);
            }
            //else if (cbOrderType.SelectedIndex == 5 || cbOrderType.SelectedIndex == 6)//交易所条件单
            else if (cbOrderType.SelectedIndex == 4 || cbOrderType.SelectedIndex == 5)//交易所条件单
            {
                dudPrice.IsEnabled = true;
                if (rbBuy.IsChecked == true)
                {
                    dudPrice.Value = CommonUtil.GetDoubleValue(tbSellPrice.Text);
                }
                else if (rbSell.IsChecked == true)
                {
                    dudPrice.Value = CommonUtil.GetDoubleValue(tbBuyPrice.Text);
                }

                TriggerRow.Height = GridLength.Auto;
                dudTriggerPrice.Value = dudPrice.Value;
            }
            else
            {
                dudPrice.IsEnabled = true;
                if (rbBuy.IsChecked == true)
                {
                    if (tbSellPrice != null)
                    {
                        dudPrice.Value = CommonUtil.GetDoubleValue(tbSellPrice.Text);
                    }
                }
                else if (rbSell.IsChecked == true)
                {
                    if (tbBuyPrice != null)
                    {
                        dudPrice.Value = CommonUtil.GetDoubleValue(tbBuyPrice.Text);
                    }
                }
                TriggerRow.Height = new GridLength(0);
            }
        }

        private void cbxSplit_Checked(object sender, RoutedEventArgs e)
        {
            TradingMaster.Properties.Settings.Default.SplitLargeOrderHandCount = true;
            TradingMaster.Properties.Settings.Default.Save();
            //if (_MainWindow != null && _MainWindow.uscPositionsInquiry != null)
            //{
            //    _MainWindow.uscPositionsInquiry.btnSjfs.IsEnabled = false;
            //}
        }

        private void cbxSplit_Unchecked(object sender, RoutedEventArgs e)
        {
            TradingMaster.Properties.Settings.Default.SplitLargeOrderHandCount = false;
            TradingMaster.Properties.Settings.Default.Save();
            //if (_MainWindow != null && _MainWindow.uscPositionsInquiry != null)
            //{
            //    _MainWindow.uscPositionsInquiry.btnSjfs.IsEnabled = true;
            //}
        }

        private void rbSpeculation_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyDownByRbHedge(sender, e);
        }

        private void rbHedge_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyDownByRbHedge(sender, e);
        }

        private void rbArbitrage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyDownByRbHedge(sender, e);
        }

        private void KeyDownByRbHedge(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter
                || e.Key == Key.Tab
                || e.Key == Key.Down)
            {
                btnXiadan.Focus();
            }
            else if (e.Key == Key.Up)
            {
                cbOrderType.Focus();
            }
            else if (e.Key == Key.Right)
            {
                RadioButton rbNext = rbHedge;

                if (sender == rbHedge)
                {
                    rbNext = rbArbitrage;
                }
                else if (sender == rbArbitrage)
                {
                    rbNext = rbSpeculation;
                }
                rbNext.Focus();
                rbNext.IsChecked = true;
            }
            else if (e.Key == Key.Left)
            {
                RadioButton rbNext = rbHedge;

                if (sender == rbHedge)
                {
                    rbNext = rbSpeculation;
                }
                else if (sender == rbSpeculation)
                {
                    rbNext = rbArbitrage;
                }
                rbNext.Focus();
                rbNext.IsChecked = true;
            }
            e.Handled = true;
        }

        private void cbOrderType_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && cbOrderType.SelectedIndex <= 0 || e.Key == Key.Left)
            {
                dudPrice.Focus();
                e.Handled = true;
            }
            else if (e.Key == Key.Down && cbOrderType.SelectedIndex >= cbOrderType.Items.Count - 1 || e.Key == Key.Right)
            {
                if (rbSpeculation.IsChecked == true)
                {
                    rbSpeculation.Focus();
                }
                else if (rbHedge.IsChecked == true)
                {
                    rbHedge.Focus();
                }
                else if (rbArbitrage.IsChecked == true)
                {
                    rbArbitrage.Focus();
                }
                e.Handled = true;
            }
        }
    }
}
