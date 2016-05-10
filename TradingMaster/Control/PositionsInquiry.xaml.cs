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
using System.Runtime.InteropServices;
using TradingMaster.CodeSet;

namespace TradingMaster.Control
{
    /// <summary>
    /// PositionsInquiry.xaml 的交互逻辑
    /// </summary>
    public delegate void PositionDataMouseDoubleClickDelegate(object sender, MouseButtonEventArgs e);
    public delegate void PositionDataMouseLeftButtonDownDelegate(string buyOrSell, string kp, int num, RealData realData);

    /// <summary>
    /// PositionsInquiry.xaml 的交互逻辑
    /// </summary>
    public partial class PositionsInquiry : UserControl
    {
        public event PositionDataMouseDoubleClickDelegate PositionDataMouseDoubleClicked;
        public event PositionDataMouseLeftButtonDownDelegate PositionDataMouseLeftButtonDown;
        private System.Timers.Timer ticker = new System.Timers.Timer(5000);

        private DateTime lastCtrlPressedTime;
        public DataGridCell cell;

        [DllImport("user32.dll", EntryPoint = "GetDoubleClickTime")]
        public extern static int GetDoubleClickTime();

        public PositionsInquiry()
        {
            CommonStaticMemeber.LoadStyleFromUserFile();
            this.DataContext = this;
            InitializeComponent();

            //ticker.Elapsed += new System.Timers.ElapsedEventHandler(ticker_Elapsed);
        }

        public TradeClientStyleUI CurrentClientStyleUI
        {
            get { return CommonStaticMemeber.CurrentClientStyleUI; }
        }

        public void Init(MainWindow parent)
        {
            this._MainWindow = parent;
            //DataGridColumnBridgeUtils.LoadColumnsSettingFromFile(this.dgPositionsInq, MainWindow.SettingDictionaryPath);
            //DataGridColumnBridgeUtils.LoadColumnsSettingFromFile(this.dgPositionsInqDetail, MainWindow.SettingDictionaryPath);

            this.dgPositionsInq.ItemsSource = _MainWindow.PositionCollection_Total;
            this.dgPositionsInqDetail.ItemsSource = _MainWindow.PositionDetailCollection;
            rbPositionTotal.Checked += new RoutedEventHandler(rbPositionTotal_Checked);
            rbPositionDetail.Checked += new RoutedEventHandler(rbPositionDetail_Checked);
        }

        private MainWindow _MainWindow { get; set; }

        private bool CancelFreezeOrder(Q7PosInfoTotal dhPosInfoTotal)
        {
            if (dhPosInfoTotal == null)
            {
                if (dgPositionsInq.SelectedItem == null)
                {
                    //一条都未选中，返回
                    return false;
                }
                dhPosInfoTotal = dgPositionsInq.SelectedItem as Q7PosInfoTotal;
            }
            string keyStr = dhPosInfoTotal.InvestorID + dhPosInfoTotal.Code + dhPosInfoTotal.BuySell.Contains("买");
            return TradeDataClient.GetClientInstance().CancelPositionFreezeOrder(keyStr);
        }


        private void PositionsInquiry_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = CommonUtil.GetClickedCell(e);
            if (cell == null)
            {
                return;
            }
            //if (cell.Column.Header.Equals("总持仓"))
            //{
            //    CloseSelectedOrder(false, false);
            //}
            if (cell.Column.Header.Equals("未成平仓") || cell.Column.Header.Equals("可平"))
            {
                //取消未成平仓
                CancelFreezeOrder(null);
            }
            else
            {
                //PositionDataMouseDoubleClicked(sender, e);
                CloseSelectedOrder(false, false);
            }
        }

        /// <summary>
        /// 进入总持
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InnerGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            if (grid == null) return;
            TextBlock tp = grid.ToolTip as TextBlock;

            DataGridCell dgc = grid.TemplatedParent as DataGridCell;
            if (tp != null && dgc != null)
            {
                Q7PosInfoTotal posInfoTotal = dgc.DataContext as Q7PosInfoTotal;
                //Util.Log("o.tostring()=" + posInfoTotal.ToString());
                tp.Inlines.Clear();

                //if (posInfoTotal.TotalPosition <= posInfoTotal.FreezeCount || posInfoTotal.TodayPosition == 0)
                //{
                //    grid.ToolTip = null;
                //    return;
                //}

                Run run1 = new Run();
                run1.Text = "双击以 ";

                Run run2 = new Run();
                if (posInfoTotal.BuySell.Contains("买"))
                {
                    //卖出平仓
                    run2.Text = "买一价";//卖出平仓" + posInfoTotal.TotalPosition.ToString()+"手";
                }
                else
                {
                    //买入平仓
                    run2.Text = "卖一价";//买入平仓" + posInfoTotal.TotalPosition.ToString() + "手";
                }

                Run run3 = new Run();
                if (posInfoTotal.BuySell.Contains("买"))
                {
                    run3.Text = "卖出";
                    run3.Foreground = NewOrderPanel.DownBrush;
                }
                else
                {
                    run3.Text = "买入";
                    run3.Foreground = NewOrderPanel.UpBrush;
                }

                tp.Inlines.Add(run1);
                tp.Inlines.Add(run2);
                tp.Inlines.Add(run3);

                Run run4 = new Run();
                if (CodeSetManager.IsCloseTodaySupport(posInfoTotal.Code))
                {
                    Boolean hasComma = false;
                    if (posInfoTotal.TodayPosition != 0)
                    {
                        run4.Text = "平今";
                        tp.Inlines.Add(run4);

                        Run run5 = new Run();
                        run5.Text = posInfoTotal.TodayPosition.ToString() + "手";
                        tp.Inlines.Add(run5);
                        hasComma = true;
                    }
                    if (posInfoTotal.YesterdayPosition != 0)
                    {
                        Run run6 = new Run();
                        if (hasComma)
                        {
                            run6.Text = ",平仓";
                        }
                        else
                        {
                            run6.Text = "平仓";
                        }
                        tp.Inlines.Add(run6);

                        Run run7 = new Run();
                        run7.Text = posInfoTotal.YesterdayPosition.ToString() + "手";
                        tp.Inlines.Add(run7);
                    }
                }
                else
                {
                    run4.Text = "平仓";
                    tp.Inlines.Add(run4);

                    Run run5 = new Run();
                    run5.Text = posInfoTotal.TotalPosition.ToString() + "手";
                    tp.Inlines.Add(run5);
                }
            }
        }

        /// <summary>
        /// 进入昨仓
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InnerGrid1_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            if (grid == null) return;
            TextBlock tp = grid.ToolTip as TextBlock;

            DataGridCell dgc = grid.TemplatedParent as DataGridCell;
            if (tp != null && dgc != null)
            {
                Q7PosInfoTotal posInfoTotal = dgc.DataContext as Q7PosInfoTotal;
                //Util.Log("o.tostring()=" + posInfoTotal.ToString());
                //如果没有昨仓，则不显示提示信息
                if (posInfoTotal.YesterdayPosition == 0 )//|| (posInfoTotal.TodayPosition == 0 && posInfoTotal.YesterdayPosition <= posInfoTotal.FreezeCount))
                {
                    grid.ToolTip = null;
                    return;
                }
                else
                {
                    if (grid.ToolTip == null)
                    {
                        grid.ToolTip = new TextBlock();
                    }
                    tp = grid.ToolTip as TextBlock;

                }

                if (tp != null)
                {
                    tp.Inlines.Clear();
                }

                if (posInfoTotal.YesterdayPosition != 0)
                {

                    Run run1 = new Run();
                    run1.Text = "双击以 ";

                    Run run2 = new Run();
                    if (posInfoTotal.BuySell.Contains("买"))
                    {
                        //卖出平仓
                        run2.Text = "买一价";//卖出平仓" + posInfoTotal.TotalPosition.ToString()+"手";
                    }
                    else
                    {
                        //买入平仓
                        run2.Text = "卖一价";//买入平仓" + posInfoTotal.TotalPosition.ToString() + "手";
                    }

                    Run run3 = new Run();
                    if (posInfoTotal.BuySell.Contains("买"))
                    {
                        run3.Text = "卖出";
                        run3.Foreground = NewOrderPanel.DownBrush;
                    }
                    else
                    {
                        run3.Text = "买入";
                        run3.Foreground = NewOrderPanel.UpBrush;
                    }

                    tp.Inlines.Add(run1);
                    tp.Inlines.Add(run2);
                    tp.Inlines.Add(run3);

                    Run run6 = new Run();
                    run6.Text = "平仓";
                    tp.Inlines.Add(run6);

                    Run run7 = new Run();
                    run7.Text = posInfoTotal.YesterdayPosition.ToString() + "手";
                    tp.Inlines.Add(run7);
                }
            }
        }


        /// <summary>
        /// 进入今仓
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InnerGrid2_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            if (grid == null) return;
            TextBlock tp = grid.ToolTip as TextBlock;

            DataGridCell dgc = grid.TemplatedParent as DataGridCell;
            if (tp != null && dgc != null)
            {
                Q7PosInfoTotal posInfoTotal = dgc.DataContext as Q7PosInfoTotal;
                //Util.Log("o.tostring()=" + posInfoTotal.ToString());

                //如果没有仓位，则不显示提示信息
                if (posInfoTotal.TodayPosition == 0)//|| (posInfoTotal.YesterdayPosition == 0 && posInfoTotal.TodayPosition <= posInfoTotal.FreezeCount))
                {
                    grid.ToolTip = null;
                    return;
                }
                else
                {
                    if (grid.ToolTip == null)
                    {
                        grid.ToolTip = new TextBlock();
                    }
                    tp = grid.ToolTip as TextBlock;

                }

                if (tp != null)
                {
                    tp.Inlines.Clear();
                }

                Run run1 = new Run();
                run1.Text = "双击以 ";

                Run run2 = new Run();
                if (posInfoTotal.BuySell.Contains("买"))
                {
                    //卖出平仓
                    run2.Text = "买一价";//卖出平仓" + posInfoTotal.TotalPosition.ToString()+"手";
                }
                else
                {
                    //买入平仓
                    run2.Text = "卖一价";//买入平仓" + posInfoTotal.TotalPosition.ToString() + "手";
                }

                Run run3 = new Run();
                if (posInfoTotal.BuySell.Contains("买"))
                {
                    run3.Text = "卖出";
                    run3.Foreground = NewOrderPanel.DownBrush;
                }
                else
                {
                    run3.Text = "买入";
                    run3.Foreground = NewOrderPanel.UpBrush;
                }

                tp.Inlines.Add(run1);
                tp.Inlines.Add(run2);
                tp.Inlines.Add(run3);

                Run run4 = new Run();
                if (CodeSetManager.IsCloseTodaySupport(posInfoTotal.Code))
                {
                    run4.Text = "平今";
                }
                else
                {
                    run4.Text = "平仓";
                }
                tp.Inlines.Add(run4);
                Run run5 = new Run();
                if (posInfoTotal.YesterdayPosition == 0)
                {
                    run5.Text = posInfoTotal.TodayPosition.ToString() + "手";
                }
                else
                {
                    run5.Text = posInfoTotal.TodayPosition.ToString() + "手";
                }
                tp.Inlines.Add(run5);
            }
        }


        /// <summary>
        /// 进入未成平仓
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InnerGrid3_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            if (grid == null) return;
            TextBlock tp = grid.ToolTip as TextBlock;

            DataGridCell dgc = grid.TemplatedParent as DataGridCell;
            if (tp != null && dgc != null)
            {
                Q7PosInfoTotal posInfoTotal = dgc.DataContext as Q7PosInfoTotal;
                //Util.Log("o.tostring()=" + posInfoTotal.ToString());
                //如果没有未成平仓，则不显示提示信息
                if (posInfoTotal.FreezeCount == 0)
                {
                    grid.ToolTip = null;
                    return;
                }
                else
                {
                    if (grid.ToolTip == null)
                    {
                        grid.ToolTip = new TextBlock();
                    }
                    tp = grid.ToolTip as TextBlock;

                }

                if (tp != null)
                {
                    tp.Inlines.Clear();
                }

                if (posInfoTotal.FreezeCount == 0) return;
                Run run1 = new Run();
                run1.Text = "双击全部撤单";
                run1.Foreground = new SolidColorBrush(Colors.Green);
                tp.Inlines.Add(run1);
            }
        }

        private void ExportDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
           CommonUtil.ExportedData_Click(sender, e, true);
        }

        private void rbPositionTotal_Checked(object sender, RoutedEventArgs e)
        {
            dgPositionsInqDetail.Visibility = Visibility.Collapsed;
            dgPositionsInq.Visibility = Visibility.Visible;

            btnKjpc.Visibility = btnSjfs.Visibility = btnSjpc.Visibility = btnExecOpt.Visibility = Visibility.Visible;
        }

        private void rbPositionDetail_Checked(object sender, RoutedEventArgs e)
        {
            dgPositionsInqDetail.Visibility = Visibility.Visible;
            dgPositionsInq.Visibility = Visibility.Collapsed;

            btnKjpc.Visibility = btnSjfs.Visibility = btnSjpc.Visibility = btnExecOpt.Visibility = Visibility.Collapsed;
        }

        private void miAutoAdjustColumnWidth_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.AutoAdjustColumnWidth_Click(sender, e);
        }

        private void gridPositionsInq_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cell = CommonUtil.GetClickedCell(e);
            if (cell == null)
            {
                return;
            }
            Q7PosInfoTotal record = cell.DataContext as Q7PosInfoTotal;
            GridPositionsTotal(record);

            DataGrid dg = sender as DataGrid;
            if (dg != null)
            {
                ObservableCollection<Q7PosInfoTotal> posInfoTotalList = dg.ItemsSource as ObservableCollection<Q7PosInfoTotal>;
                if (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control)
                {
                    //Util.Log("上一次,按下CTRL键时:" + lastCtrlPressedTime.ToString("hh:mm:ss.fff"));
                    DateTime now = System.DateTime.Now;
                    TimeSpan ts1 = new TimeSpan(lastCtrlPressedTime.Ticks);
                    TimeSpan ts2 = new TimeSpan(now.Ticks);
                    TimeSpan ts = ts1.Subtract(ts2).Duration();
                    lastCtrlPressedTime = now;
                    //Util.Log("这一次,按下CTRL键时:" + lastCtrlPressedTime.ToString("hh:mm:ss.fff"));
                    //Util.Log("差值为:" + (ts.Seconds) + ":" + (ts.Milliseconds) + " 鼠标双击间隔为:" + GetDoubleClickTime());
                    if (ts.TotalMilliseconds < GetDoubleClickTime())
                    {
                        //表示双击
                        dg.SelectedItems.Clear();
                        dg.SelectedItems.Add(record);
                        PositionsInquiry_MouseDoubleClick(sender, e);

                    }
                    else
                    {
                        if (dg.SelectedItems.Contains(record) == false)
                        {
                            //Util.Log("按下CTRL键时，添加一条记录:" + record.Code.ToString());
                            try
                            {
                                dg.SelectedItems.Add(record);
                            }
                            catch (Exception ex)
                            {
                                Util.Log(ex.Message);
                                Util.Log(ex.StackTrace);
                            }
                            //Util.Log("dg.SelectedItems.count=" + dg.SelectedItems.Count.ToString());
                        }
                        else
                        {
                            //Util.Log("按下CTRL键时，移除一条记录:" + record.Code.ToString());
                            dg.SelectedItems.Remove(record);
                        }
                    }
                    e.Handled = true;
                }
                else if (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Shift)
                {
                    ObservableCollection<Q7PosInfoTotal> allPostionTotal = _MainWindow.PositionCollection_Total;
                    if (dg.SelectedItems.Count == 0)
                    {
                        dg.SelectedItems.Add(record);
                        return;
                    }
                    if (dg.SelectedItems.Contains(record))
                    {
                        dg.SelectedItems.Remove(record);
                        return;
                    }
                    int lastIndex = dg.Items.IndexOf(dg.SelectedItems[dg.SelectedItems.Count - 1]);
                    int curIndex = dg.Items.IndexOf(record);
                    if (curIndex < lastIndex)
                    {
                        for (int i = curIndex; i < lastIndex; i++)
                        {
                            dg.SelectedItems.Add(dg.Items[i]);
                        }
                    }
                    else
                    {
                        for (int i = curIndex; i > lastIndex; i--)
                        {
                            dg.SelectedItems.Add(dg.Items[i]);
                        }
                    }
                    e.Handled = true;
                }
                else
                {
                    dg.SelectedItems.Clear();
                    dg.SelectedItems.Add(record);
                }
            }
        }

        private void gridPositionsInqDetail_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cell = CommonUtil.GetClickedCell(e);
            if (cell == null)
            {
                return;
            }
            Q7PosInfoDetail record = cell.DataContext as Q7PosInfoDetail;
            GridPositionsDetail(record);

            DataGrid dg = sender as DataGrid;

            if (dg != null)
            {
                ObservableCollection<Q7PosInfoDetail> posInfoTotalList = dg.ItemsSource as ObservableCollection<Q7PosInfoDetail>;

                //if (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control)
                //{
                //    Util.Log("上一次,按下CTRL键时:" + lastCtrlPressedTime.ToString("hh:mm:ss.fff"));
                //    DateTime now = System.DateTime.Now;
                //    TimeSpan ts1 = new TimeSpan(lastCtrlPressedTime.Ticks);
                //    TimeSpan ts2 = new TimeSpan(now.Ticks);
                //    TimeSpan ts = ts1.Subtract(ts2).Duration();
                //    lastCtrlPressedTime = now;
                //    Util.Log("这一次,按下CTRL键时:" + lastCtrlPressedTime.ToString("hh:mm:ss.fff"));
                //    Util.Log("差值为:" + (ts.Seconds) + ":" + (ts.Milliseconds) + " 鼠标双击间隔为:" + GetDoubleClickTime());
                //    if (ts.TotalMilliseconds < GetDoubleClickTime())
                //    {
                //        //表示双击
                //        dg.SelectedItems.Clear();
                //        dg.SelectedItems.Add(record);
                //        PositionsInquiry_MouseDoubleClick(sender, e);

                //    }
                //    else
                //    {
                //        if (dg.SelectedItems.Contains(record) == false)
                //        {
                //            dg.SelectedItems.Add(record);
                //        }
                //        else
                //        {
                //            dg.SelectedItems.Remove(record);
                //        }
                //    }
                //    e.Handled = true;
                //}
                //else if (System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Shift)
                //{

                //    if (dg.SelectedItems.Count == 0)
                //    {
                //        dg.SelectedItems.Add(record);
                //        return;
                //    }
                //    if (dg.SelectedItems.Contains(record))
                //    {
                //        dg.SelectedItems.Remove(record);
                //        return;
                //    }
                //    int lastIndex = dg.Items.IndexOf(dg.SelectedItems[dg.SelectedItems.Count - 1]);
                //    int curIndex = dg.Items.IndexOf(record);
                //    if (curIndex < lastIndex)
                //    {
                //        for (int i = curIndex; i < lastIndex; i++)
                //        {
                //            dg.SelectedItems.Add(dg.Items[i]);
                //        }
                //    }
                //    else
                //    {
                //        for (int i = curIndex; i > lastIndex; i--)
                //        {
                //            dg.SelectedItems.Add(dg.Items[i]);
                //        }
                //    }
                //    e.Handled = true;
                //}
                //else
                {
                    dg.SelectedItems.Clear();
                    dg.SelectedItems.Add(record);
                }

                //dg.SelectedIndex = posInfoTotalList.IndexOf(record);
            }
        }

        private void gridPositionsInqDetail_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //明细无操作
            //if (PositionDataMouseDoubleClicked != null)
            //{
            //    DataGridCell cell  = GlobalCommonUtils.GetClickedCell(e);
            //    if (cell == null)
            //    {
            //        return;
            //    }
            //    PositionDataMouseDoubleClicked(sender ,e);
            //}
        }

        private void GridPositionsDetail(Q7PosInfoDetail record)
        {
            if (!CommonUtil.IsValidCode(record.Code))
            {
                return;
            }

            string buySell = "买";
            if (record.BuySell.Contains("买"))
            {
                buySell = "卖";
            }

            //找到相应的行情数据
            RealData realData = null;
            foreach (var item in _MainWindow.HQBackgroundRealData.RealDataList)
            {
                if (item.CodeInfo.Code == record.Code)
                {
                    realData = item;
                    break;
                }
            }
            if (realData == null)
            {
                _MainWindow.AddSystemMessage(DateTime.Now, "TradingMaster：无法取得" + record.Code + "的行情数据", "信息", "System");
                return;
            }

            string kp = "平今";
            int num = record.TradeHandCount;
            if (record.PositionType == "昨仓")
            {
                kp = "平仓";
            }

            //mainWindow.uscOptionHangqing.AddExternalHqingData(realData);
            if (PositionDataMouseLeftButtonDown != null)
            {
                PositionDataMouseLeftButtonDown(buySell, kp, num, realData);
            }
            //mainWindow.uscOptionHangqing.SelectDataByCode(realData.Code);
        }

        private void GridPositionsTotal(Q7PosInfoTotal record)
        {
            if (!CommonUtil.IsValidCode(record.Code))
            {
                return;
            }

            string buySell = "买";
            if (record.BuySell.Contains("买"))
            {
                buySell = "卖";
            }

            //找到相应的行情数据
            RealData realData = _MainWindow.HQBackgroundRealData.GetRealDataByCode(record.Code);
            if (realData == null)
            {
                _MainWindow.AddSystemMessage(DateTime.Now, "TradingMaster：无法取得" + record.Code + "的行情数据", "信息", "System");
                return;
            }

            string kp = "";
            int num = 0;
            if (CodeSetManager.IsCloseTodaySupport(record.Code))
            {
                kp = "平今";
                num = record.TodayPosition;
                if (cell.Column == this.colHistoryOpen || (cell.Column != this.colTodayOpen && record.TodayPosition == 0))
                {
                    kp = "平仓";
                    num = record.YesterdayPosition;
                }
            }
            else
            {
                kp = "平仓";
                num = record.TotalPosition;
            }
            

            //mainWindow.uscOptionHangqing.AddExternalHqingData(realData);
            if (PositionDataMouseLeftButtonDown != null)
            {
                PositionDataMouseLeftButtonDown(buySell, kp, num, realData);
            }
            //mainWindow.uscOptionHangqing.SelectDataByCode(realData.Code);
        }

        private void SendCloseOrder(PosInfoOrder posInfoOrder, bool open, bool hasCancelOrder)
        {
            Contract orderCodeIndo = CodeSetManager.GetContractInfo(posInfoOrder.posInfo.Code, CodeSetManager.ExNameToCtp(posInfoOrder.posInfo.Exchange));
            SIDETYPE isBuy = posInfoOrder.posInfo.BuySell.Trim().Contains("买") ? SIDETYPE.SELL : SIDETYPE.BUY;//tb_buyAndSell.Text;
            int touchMethod = 0;
            int touchCondition = 0;
            double touchPrice = 0;
            int isAuto = 0;
            EnumOrderType orderType = posInfoOrder.OrderType;

            ////if (_MainWindow.uscNewOrderPanel.chbOrderType.IsChecked == true && _MainWindow.uscNewOrderPanel.cbOrderType.SelectedItem != null)
            //if (_MainWindow.uscNewOrderPanel.cbOrderType.SelectedItem != null)
            //{
            //    orderType = CommonUtil.GetOrderType(_MainWindow.uscNewOrderPanel.cbOrderType.SelectedValue.ToString());
            //}

            if (hasCancelOrder)
            {
                TradeDataClient.GetClientInstance().AddPosCancelOrderList(posInfoOrder.posInfo.InvestorID, posInfoOrder.posInfo.BackEnd, posInfoOrder);
            }
            else
            {
                TradeDataClient.GetClientInstance().RequestOrder(posInfoOrder.posInfo.InvestorID, posInfoOrder.posInfo.BackEnd, new RequestContent("NewOrderSingle", new List<object>() { orderCodeIndo, isBuy, posInfoOrder.PositionEffect, posInfoOrder.Price, posInfoOrder.HandCount, isAuto, "", touchMethod, touchCondition, touchPrice, orderType }));
            }

            if (open)
            {
                // Optional Logic: Reversely open after waiting successful closing 
                PosInfoOrder posOrder = new PosInfoOrder(posInfoOrder.posInfo, posInfoOrder.posInfo.BuySell.Trim(), posInfoOrder.Price, PosEffect.Open, posInfoOrder.HandCount, orderType);//posInfoOrder.posInfo.TotalPosition
                TradeDataClient.GetClientInstance().AddPosOrderList(posInfoOrder.posInfo.InvestorID, posInfoOrder.posInfo.BackEnd, posOrder);
            }
        }

        //对价平仓
        private void btnKjpc_Click(object sender, RoutedEventArgs e)
        {
            CloseSelectedOrder(false, false);
        }

        //市价平仓
        private void btnSjpc_Click(object sender, RoutedEventArgs e)
        {
            CloseSelectedOrder(false, true);
        }

        //对价反手
        private void btnSjfs_Click(object sender, RoutedEventArgs e)
        {
            CloseSelectedOrder(true, false);
        }

        /// <summary>
        /// 快捷平仓，市价平仓，市价反手，进入该函数的平仓就是全部平掉，包括今仓，昨仓。
        /// </summary>
        /// <param name="open">true表示反手，false则表示平仓</param>
        /// <param name="sSjpc">true表示是市价，false表示非市价</param>
        private void CloseSelectedOrder(bool open, bool sSjpc)
        {
            if (dgPositionsInq.SelectedItem == null)
            {
                return;
            }

            ObservableCollection<Q7PosInfoTotal> posInfoTotalCollction = new ObservableCollection<Q7PosInfoTotal>();
            List<PosInfoOrder> posInfoOrderList = new List<PosInfoOrder>();
            foreach (var item in dgPositionsInq.SelectedItems)
            {
                posInfoTotalCollction.Add(item as Q7PosInfoTotal);
            }

            CheckableMessageBox messageBox = new CheckableMessageBox();
            messageBox.tbMessage.Text = string.Empty;

            foreach (Q7PosInfoTotal posInfo in posInfoTotalCollction)
            {
                //Q7PosInfoTotal posInfo = item as Q7PosInfoTotal;
                RealData realData = _MainWindow.HQBackgroundRealData.GetRealDataByCode(posInfo.Code);
                if (realData == null)
                {
                    _MainWindow.AddSystemMessage(DateTime.Now, "TradingMaster：无法取得" + posInfo.Code + "的行情数据", "信息", "System");
                    return;
                }

                if (posInfo.CanCloseCount == 0 && posInfo.FreezeCount == 0)
                {
                    continue;
                }

                string buySell = posInfo.BuySell.Trim();
                string priceOrientation = buySell.Contains("买") ? "卖" : "买";

                double priceClose = buySell.Contains("买") ? realData.BidPrice[0] : realData.AskPrice[0];

                //if (open)
                //{
                //    priceClose = buySell.Contains("买") ? realData.lowerLimitPrice : realData.upperLimitPrice;
                //}

                EnumOrderType orderType = EnumOrderType.Limit;
                
                if (priceClose == 0)
                {
                    priceClose = realData.NewPrice;
                    if (priceClose == 0)
                    {
                        priceClose = realData.PrevSettlementPrice;
                    }
                }

                if (sSjpc)
                {
                    priceClose = buySell.Contains("买") ? realData.LowerLimitPrice : realData.UpperLimitPrice;
                    //priceClose = 0;
                    //orderType = EnumOrderType.Market;
                }

                //if (_MainWindow.uscNewOrderPanel.chbOrderType.IsChecked == true && _MainWindow.uscNewOrderPanel.cbOrderType.SelectedItem != null && posInfo.Code == _MainWindow.uscNewOrderPanel.txtCode.Text.Trim())
                //if (_MainWindow.uscNewOrderPanel.cbOrderType.SelectedItem != null && posInfo.Code == _MainWindow.uscNewOrderPanel.txtCode.Text.Trim())
                //{
                //    orderType = CommonUtil.GetOrderType(_MainWindow.uscNewOrderPanel.cbOrderType.SelectedValue.ToString());
                //}

                if (TradingMaster.Properties.Settings.Default.ConfirmBeforeSendNewOrder == true)
                {
                    if (CodeSetManager.IsCloseTodaySupport(posInfo.Code))
                    {
                        if (posInfo.FreezeCount > 0)
                        {
                            if (messageBox.tbMessage.Text.Trim().Length > 0)
                            {
                                //同时平今和平昨
                                messageBox.tbMessage.Text = messageBox.tbMessage.Text + "\n";
                            }
                            string keyStr = posInfo.InvestorID + posInfo.Code + posInfo.BuySell.Contains("买");
                            List<Q7JYOrderData> orderLst = TradeDataClient.GetClientInstance().GetFreezeOrder(keyStr);
                            foreach (Q7JYOrderData order in orderLst)
                            {
                                messageBox.tbMessage.Text = messageBox.tbMessage.Text + string.Format("撤单{0}: {1} {2}  {3} {4}手 于价格{5}, {6}",
                                    order.OrderID, order.OpenClose, order.BuySell, order.Code, order.CommitHandCount, order.CommitPrice.ToString(), order.Hedge);
                            }
                        }
                        if (posInfo.TodayPosition > 0)
                        {
                            if (messageBox.tbMessage.Text.Trim().Length > 0)
                            {
                                //同时平今和平昨
                                messageBox.tbMessage.Text = messageBox.tbMessage.Text + "\n";
                            }
                            messageBox.tbMessage.Text = string.Format("下单：平今  {0} {1} {2}手 于价格{3}, {4}",
                                priceOrientation, posInfo.Code, posInfo.TodayPosition, priceClose, CommonUtil.GetOrderTypeString(orderType));

                            PosInfoOrder pOrder = new PosInfoOrder(posInfo, buySell, priceClose, PosEffect.CloseToday, posInfo.TodayPosition, orderType);
                            posInfoOrderList.Add(pOrder);
                        }
                        if (posInfo.YesterdayPosition > 0)
                        {
                            if (messageBox.tbMessage.Text.Trim().Length > 0)
                            {
                                //同时平今和平昨
                                messageBox.tbMessage.Text = messageBox.tbMessage.Text + "\n";
                            }
                            messageBox.tbMessage.Text = messageBox.tbMessage.Text + string.Format("下单：平仓  {0} {1} {2}手 于价格{3}, {4}",
                                priceOrientation, posInfo.Code, posInfo.YesterdayPosition, priceClose, CommonUtil.GetOrderTypeString(orderType));

                            PosInfoOrder pOrder = new PosInfoOrder(posInfo, buySell, priceClose, PosEffect.Close, posInfo.YesterdayPosition, orderType);
                            posInfoOrderList.Add(pOrder);
                        }
                    }
                    else
                    {
                        if (messageBox.tbMessage.Text.Trim().Length > 0)
                        {
                            //同时平今和平昨
                            messageBox.tbMessage.Text = messageBox.tbMessage.Text + "\n";
                        }
                        if (posInfo.FreezeCount > 0)
                        {
                            string keyStr = posInfo.InvestorID + posInfo.Code + posInfo.BuySell.Contains("买");
                            List<Q7JYOrderData> orderLst = TradeDataClient.GetClientInstance().GetFreezeOrder(keyStr);
                            foreach (Q7JYOrderData order in orderLst)
                            {
                                messageBox.tbMessage.Text = messageBox.tbMessage.Text + string.Format("撤单{0}: {1} {2}  {3} {4}手 于价格{5}, {6}\n",
                                    order.OrderID, order.OpenClose, order.BuySell, order.Code, order.CommitHandCount, order.CommitPrice.ToString(), order.Hedge);
                            }
                        }
                        messageBox.tbMessage.Text = messageBox.tbMessage.Text + string.Format("下单：平仓  {0} {1} {2}手 于价格{3}, {4}",
                            priceOrientation, posInfo.Code, posInfo.TotalPosition, priceClose, CommonUtil.GetOrderTypeString(orderType));

                        PosInfoOrder pOrder = new PosInfoOrder(posInfo, buySell, priceClose, PosEffect.Close, posInfo.TotalPosition, orderType);
                        posInfoOrderList.Add(pOrder);
                    }

                    string windowTitle = string.Format("确认下单：价格  {0}一价,自动  {1}  {2}  {3}手",
                        buySell, priceOrientation, posInfo.Code, posInfo.TotalPosition);

                    if (open)
                    {
                        messageBox.tbMessage.Text = messageBox.tbMessage.Text + "\n" + string.Format("下单：开仓  {0} {1} {2}手 于价格{3}, {4}",
                            priceOrientation, posInfo.Code, posInfo.TotalPosition, priceClose, CommonUtil.GetOrderTypeString(orderType));
                        windowTitle = string.Format("确认反手：{0} {1} -> {2}  {3}手",
                            posInfo.Code, buySell, priceOrientation, posInfo.TotalPosition);
                    }
                }
            }

            if (TradingMaster.Properties.Settings.Default.ConfirmBeforeSendNewOrder == true)
            {
                if (messageBox.tbMessage.Text.Trim().Length > 0)
                {
                    Window confirmWindow = CommonUtil.GetWindow("下单确认", messageBox, this._MainWindow);
                    if (confirmWindow.ShowDialog() == true)
                    {
                        if (messageBox.chkConfirm.IsChecked == true)
                        {
                            TradingMaster.Properties.Settings.Default.ConfirmBeforeSendNewOrder = false;
                            TradingMaster.Properties.Settings.Default.Save();
                        }
                        foreach (PosInfoOrder item in posInfoOrderList)
                        {
                            Q7PosInfoTotal posInfo = item.posInfo;
                            //double priceClose = item.sPrice;
                            //bool posOpen = item.posEffect == PosEffect.Open ? true : false;
                            bool hasCancelOrder = false;
                            if (posInfo.FreezeCount > 0)//Todo: Re-cancel order after cancel previous order
                            {
                                hasCancelOrder = CancelFreezeOrder(posInfo);
                                //System.Threading.Thread.Sleep(200);
                            }
                            SendCloseOrder(item, open, hasCancelOrder);
                        }
                    }
                }
            }
            else
            {
                foreach (PosInfoOrder item in posInfoOrderList)
                {
                    Q7PosInfoTotal posInfo = item.posInfo;
                    //double priceClose = item.sPrice;
                    //bool posOpen = item.posEffect == PosEffect.Open ? true : false;
                    bool hasCancelOrder = false;
                    if (posInfo.FreezeCount > 0)//Todo: Re-cancel order after cancel previous order
                    {
                        hasCancelOrder = CancelFreezeOrder(posInfo);
                        //System.Threading.Thread.Sleep(200);
                    }
                    SendCloseOrder(item, open, hasCancelOrder);
                }
            }
        }

        private void dgPositionsInq_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (dgPositionsInq.SelectedItem == null)
            {
                miSjfs.Visibility = Visibility.Collapsed;
            }
            else
            {
                miSjfs.Visibility = Visibility.Visible;
            }
        }

        private void btnFresh_Click(object sender, RoutedEventArgs e)
        {
            ////btnFresh.IsEnabled = false;
            //CtpDataServer.getServerInstance().AddToTradeDataQryQueue(new RequestContent("ReqPositionDetail", new List<object>()));
            //List<string> reqList = new List<string>();
            //foreach (var item in _MainWindow.PositionCollection_Total)
            //{
            //    Contract codeItem = CodeSetManager.GetContractInfo(item.Code, CodeSetManager.ExNameToCtp(item.Exchange));
            //    reqList.Add(codeItem.Code);
            //}
            ////_MainWindow.ClearUpdate(reqList);
            //_MainWindow.RequestSnapShotPlusUpdate(reqList);
            //ticker.Start();
        }

        void ticker_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    //if (!btnFresh.IsEnabled)
                    {
                        //btnFresh.IsEnabled = true;
                    }
                    ticker.Stop();
                });
            }
        }

        private void btnExecOpt_Click(object sender, RoutedEventArgs e)
        {
            if (dgPositionsInq.SelectedItem == null)
            {
                return;
            }

            ObservableCollection<Q7PosInfoTotal> posInfoTotalCollction = new ObservableCollection<Q7PosInfoTotal>();
            List<ExecOrderInfo> execOrderInfoList = new List<ExecOrderInfo>();
            foreach (var item in dgPositionsInq.SelectedItems)
            {
                posInfoTotalCollction.Add(item as Q7PosInfoTotal);
            }

            CheckableMessageBox messageBox = new CheckableMessageBox();
            messageBox.tbMessage.Text = string.Empty;

            foreach (Q7PosInfoTotal record in posInfoTotalCollction)
            {
                if (record.BuySell == "买" && record.ProductType.Contains("Option"))
                {
                    string buySell = record.BuySell.Trim();
                    if (CodeSetManager.IsCloseTodaySupport(record.Code))
                    {
                        if (record.TodayPosition > 0)
                        {
                            if (messageBox.tbMessage.Text.Trim().Length > 0)
                            {
                                //同时平今和平昨
                                messageBox.tbMessage.Text = messageBox.tbMessage.Text + "\n";
                            }
                            messageBox.tbMessage.Text = string.Format("指令：行权 {0} {1}手",
                                record.Code, record.TodayPosition);
                            ExecOrderInfo eOrder = new ExecOrderInfo(record, buySell, PosEffect.CloseToday,record.TodayPosition);
                            execOrderInfoList.Add(eOrder);
                        }
                        if (record.YesterdayPosition > 0)
                        {
                            if (messageBox.tbMessage.Text.Trim().Length > 0)
                            {
                                //同时平今和平昨
                                messageBox.tbMessage.Text = messageBox.tbMessage.Text + "\n";
                            }
                            messageBox.tbMessage.Text = messageBox.tbMessage.Text + string.Format("指令：行权 {0} {1}手",
                                record.Code, record.YesterdayPosition);
                            ExecOrderInfo eOrder = new ExecOrderInfo(record, buySell, PosEffect.CloseYesterday,record.TodayPosition);
                            execOrderInfoList.Add(eOrder);
                        }
                    }
                    else
                    {
                        if (messageBox.tbMessage.Text.Trim().Length > 0)
                        {
                            //同时平今和平昨
                            messageBox.tbMessage.Text = messageBox.tbMessage.Text + "\n";
                        }
                        messageBox.tbMessage.Text = messageBox.tbMessage.Text + string.Format("指令：行权 {0} {1}手",
                            record.Code, record.TotalPosition);
                        ExecOrderInfo eOrder = new ExecOrderInfo(record, buySell, PosEffect.Close, record.TotalPosition);
                        execOrderInfoList.Add(eOrder);
                    }
                }
            }
            if (TradingMaster.Properties.Settings.Default.ConfirmBeforeSendNewOrder == true)
            {
                if (messageBox.tbMessage.Text.Trim().Length > 0)
                {
                    Window confirmWindow = CommonUtil.GetWindow("指令确认", messageBox, this._MainWindow);
                    if (confirmWindow.ShowDialog() == true)
                    {
                        if (messageBox.chkConfirm.IsChecked == true)
                        {
                            TradingMaster.Properties.Settings.Default.ConfirmBeforeSendNewOrder = false;
                            TradingMaster.Properties.Settings.Default.Save();
                        }
                        foreach (ExecOrderInfo item in execOrderInfoList)
                        {
                            SendExecOrder(item);
                        }
                    }
                }
            }
            else
            {
                foreach (ExecOrderInfo item in execOrderInfoList)
                {
                    SendExecOrder(item);
                }
            }
        }

        private void SendExecOrder(ExecOrderInfo execRecord)
        {
            EnumThostOffsetFlagType openClose = EnumThostOffsetFlagType.Close;
            if (execRecord.PositionEffect == PosEffect.CloseToday)
            {
                openClose = EnumThostOffsetFlagType.CloseToday;
            }
            else if (execRecord.PositionEffect == PosEffect.Close)
            {
                openClose = EnumThostOffsetFlagType.Close;
            }
            else if (execRecord.PositionEffect == PosEffect.CloseYesterday)
            {
                openClose = EnumThostOffsetFlagType.CloseYesterday;
            }
            TradeDataClient.GetClientInstance().RequestOrder(execRecord.posInfo.InvestorID, execRecord.posInfo.BackEnd, new RequestContent("NewExecOrder", new List<object>() { execRecord.posInfo.Code, execRecord.HandCount, execRecord.posInfo.Exchange, true, openClose }));
        }

        private void btnCombAct_Click(object sender, RoutedEventArgs e)
        {
            if (dgPositionsInq.SelectedItem == null)
            {
                return;
            }
            ObservableCollection<Q7PosInfoTotal> posInfoTotalCollction = new ObservableCollection<Q7PosInfoTotal>();
            List<ExecOrderInfo> execOrderInfoList = new List<ExecOrderInfo>();
            foreach (var item in dgPositionsInq.SelectedItems)
            {
                posInfoTotalCollction.Add(item as Q7PosInfoTotal);
            }

            foreach (Q7PosInfoTotal record in posInfoTotalCollction)
            {
                //JYDataServer.getServerInstance().AddToOrderQueue(new CtpRequestContent("QryInvestorProductMargin", new List<object>() { record.Code }));
                //JYDataServer.getServerInstance().AddToOrderQueue(new CtpRequestContent("QryProductExchRate", new List<object>() { CodeSetManager.GetContractInfo(record.Code).SpeciesCode }));
                TradeDataClient.GetClientInstance().RequestOrder(record.InvestorID, record.BackEnd, new RequestContent("QryCombinationAction", new List<object>() { record.Code, record.Exchange }));
                SIDETYPE posDir = SIDETYPE.BUY;
                if (record.BuySell.Contains("卖"))
                {
                    posDir = SIDETYPE.SELL;
                }
                //CtpDataServer.getServerInstance().AddToOrderQueue(new RequestContent("NewCombinationAction", new List<object>() { record.Code, posDir, record.TotalPosition, EnumThostCombDirectionType.Comb }));
                TradeDataClient.GetClientInstance().RequestOrder(record.InvestorID, record.BackEnd, new RequestContent("NewCombinationAction", new List<object>() { record.Code, posDir, record.TotalPosition, EnumThostCombDirectionType.Comb }));
            }
        }

        private void gridPositionsInq_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)
            {
                DataGrid dg = sender as DataGrid;
                if (dg.SelectedItem != null)
                {
                    Q7PosInfoTotal record = dg.SelectedItem as Q7PosInfoTotal;
                    GridPositionsTotal(record);
                }
            }
        }

        private void gridPositionsInqDetail_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)
            {
                DataGrid dg = sender as DataGrid;
                if (dg.SelectedItem != null)
                {
                    Q7PosInfoDetail record = dg.SelectedItem as Q7PosInfoDetail;
                    GridPositionsDetail(record);
                }
            }
        }
    }

    /// <summary>
    /// 一条平仓下单的记录
    /// </summary>
    public class PosInfoOrder
    {
        public Q7PosInfoTotal posInfo;      //对应的是平仓合计中的哪条记录

        public string BuySell { get; set; }              //"买"或者"卖"
        public double Price { get; set; }              //价格
        public PosEffect PositionEffect { get; set; }         //开仓，平仓，平今
        public int HandCount { get; set; }             //手数
        public EnumOrderType OrderType { get; set; }

        public PosInfoOrder(Q7PosInfoTotal aPosInfo, string aBuySell, double aPrice, PosEffect aPosEffect, int aHandCount, EnumOrderType aOrderType)
        {
            this.posInfo = aPosInfo;
            this.BuySell = aBuySell;
            this.Price = aPrice;
            this.PositionEffect = aPosEffect;
            this.HandCount = aHandCount;

            this.OrderType = aOrderType;
        }
    }

    /// <summary>
    /// 一条行权的记录
    /// </summary>
    public class ExecOrderInfo
    {
        public Q7PosInfoTotal posInfo;      //对应的是平仓合计中的哪条记录

        public string BuySell { get; set; }             //"买"或者"卖"
        public double Price { get; set; }              //价格
        public PosEffect PositionEffect { get; set; }        //开仓，平仓，平今
        public int HandCount { get; set; }               //手数

        public ExecOrderInfo(Q7PosInfoTotal aPosInfo, string aBuySell, PosEffect aPosEffect, int aHandCount)
        {
            this.posInfo = aPosInfo;
            this.BuySell = aBuySell;
            this.PositionEffect = aPosEffect;
            this.HandCount = aHandCount;
        }
    }
}
