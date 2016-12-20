using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TradingMaster.Control
{
    /// <summary>
    /// OptionExecQry.xaml 的交互逻辑
    /// </summary>
    public partial class OptionExecutionRecord : UserControl
    {
        public OptionExecutionRecord()
        {
            CommonStaticMemeber.LoadStyleFromUserFile();
            this.DataContext = this;
            InitializeComponent();
        }

        public void Init(MainWindow parent)
        {
            this._MainWindow = parent;
            //this.dgOptionExecInq.ItemsSource = _MainWindow.ExecOrderDataCollection;
            this.rbAll.IsChecked = true;
        }

        public TradeClientStyleUI CurrentClientStyleUI
        {
            get { return CommonStaticMemeber.CurrentClientStyleUI; }
        }

        private MainWindow _MainWindow { get; set; }

        private void dgOptionExecInq_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void dgOptionExecInq_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridCell dgCell = CommonUtil.GetClickedCell(e);
            if (dgCell != null)
            {
                CommonUtil.CancelExecOrderByDoubleClick(true, dgOptionExecInq, _MainWindow);
            }
        }

        private void miAutoAdjustColumnWidth_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.AutoAdjustColumnWidth_Click(sender, e);
        }

        private void ExportDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.ExportedData_Click(sender, e, true);
        }

        private void btnFresh_Click(object sender, RoutedEventArgs e)
        {
            //CtpDataServer.getServerInstance().AddToTradeDataQryQueue(new RequestContent("QryExecOrder", new List<object>()));
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.CancelSelectedExecOrder(dgOptionExecInq, _MainWindow, true);
        }

        private void btnCancelAll_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.CancelAllExecOrder(_MainWindow.ExecOrderDataCollection, _MainWindow);
        }

        private void dgQuoteOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridCell dgCell = CommonUtil.GetClickedCell(e);
            if (dgCell != null)
            {
                CommonUtil.CancelExecOrderByDoubleClick(true, dgOptionExecInq, _MainWindow);//false
            }
        }

        private void CancelSelectedOrder(object sender, RoutedEventArgs e)
        {
            CommonUtil.CancelSelectedExecOrder(dgOptionExecInq, _MainWindow, true);
        }

        private void CancelAllOrder(object sender, RoutedEventArgs e)
        {
            CommonUtil.CancelAllExecOrder(_MainWindow.ExecOrderDataCollection, _MainWindow);
        }

        private void rbAll_Checked(object sender, RoutedEventArgs e)
        {
            dgOptionExecInq.ItemsSource = _MainWindow.ExecOrderDataCollection;
            btnCancel.IsEnabled = btnCancelAll.IsEnabled = true;
        }

        private void rbPending_Checked(object sender, RoutedEventArgs e)
        {
            dgOptionExecInq.ItemsSource = _MainWindow.ExecOrderPendingDataCollection;
            btnCancel.IsEnabled = btnCancelAll.IsEnabled = true;
        }

        private void rbCanceled_Checked(object sender, RoutedEventArgs e)
        {
            dgOptionExecInq.ItemsSource = _MainWindow.ExecOrderCancelledDataCollection;
            btnCancel.IsEnabled = btnCancelAll.IsEnabled = false;
        }
    }
}
