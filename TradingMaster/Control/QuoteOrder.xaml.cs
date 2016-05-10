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

namespace TradingMaster.Control
{
    /// <summary>
    /// QuoteOrder.xaml 的交互逻辑
    /// </summary>
    public partial class QuoteOrder : UserControl
    {
        public QuoteOrder()
        {
            CommonStaticMemeber.LoadStyleFromUserFile();
            this.DataContext = this;
            InitializeComponent();
        }

        public void Init(MainWindow parent)
        {
            this._MainWindow = parent;
            this.rbAll.IsChecked = true;
        }

        public TradeClientStyleUI CurrentClientStyleUI
        {
            get { return CommonStaticMemeber.CurrentClientStyleUI; }
        }

        private MainWindow _MainWindow { get; set; }

        private void btnFresh_Click(object sender, RoutedEventArgs e)
        {
            //CtpDataServer.getServerInstance().AddToTradeDataQryQueue(new RequestContent("QryQuoteOrder", new List<object>() { }));
        }

        private void miAutoAdjustColumnWidth_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.AutoAdjustColumnWidth_Click(sender, e);
        }

        private void ExportDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.ExportedData_Click(sender, e, true);
        }

        private void rbAll_Checked(object sender, RoutedEventArgs e)
        {
            dgQuoteOrder.ItemsSource = _MainWindow.QuoteOrderDataCollection;
            btnCancelAll.IsEnabled = true;
        }

        private void rbPending_Checked(object sender, RoutedEventArgs e)
        {
            dgQuoteOrder.ItemsSource = _MainWindow.QuotePendingOrderDataCollection;
            btnCancel.IsEnabled = btnCancelAll.IsEnabled = true;
        }

        private void rbTransaction_Checked(object sender, RoutedEventArgs e)
        {
            dgQuoteOrder.ItemsSource = _MainWindow.QuoteTradedOrderDataCollection;
            btnCancel.IsEnabled = btnCancelAll.IsEnabled = false;
        }

        private void rbCanceled_Checked(object sender, RoutedEventArgs e)
        {
            dgQuoteOrder.ItemsSource = _MainWindow.QuoteCancelledOrderDataCollection;
            btnCancel.IsEnabled = btnCancelAll.IsEnabled = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.CancelQuoteOrderByDoubleClick(true, dgQuoteOrder, _MainWindow);
        }

        private void btnCancelAll_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.CancelAllQuoteOrder(_MainWindow.QuoteOrderDataCollection, _MainWindow);
        }

        private void dgQuoteOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridCell dgCell = CommonUtil.GetClickedCell(e);
            if (dgCell != null)
            {
                CommonUtil.CancelQuoteOrderByDoubleClick(true, dgQuoteOrder, _MainWindow);//false
            }
        }

        private void dgQuoteOrder_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ///先判断是否需要显示撤单菜单
            //bool hasCancellableOrder = false;

            //if (dgQuoteOrder.SelectedItem != null)
            //{
            //    List<object> selectedOrder = new List<object>();
            //    foreach (var item in dgQuoteOrder.SelectedItems)
            //    {
            //        selectedOrder.Add(item);
            //    }

            //    foreach (var item in selectedOrder)
            //    {
            //        if (item != null)
            //        {
            //            //撤单
            //            if (CommonUtil.IsQuoteCancellable(item))
            //            {
            //                hasCancellableOrder = true;
            //            }
            //        }
            //    }
            //}

            //if (hasCancellableOrder)
            //{
            //    sptDgMenu.Visibility = miCancel.Visibility = miCancelAll.Visibility = miCancelAndReOrder.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    sptDgMenu.Visibility = miCancel.Visibility = miCancelAll.Visibility = miCancelAndReOrder.Visibility = Visibility.Collapsed;
            //}
        }

        private void dgQuoteOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgQuoteOrder.SelectedItem != null)
            {
                List<QuoteOrderData> newOrderList = new List<QuoteOrderData>();
                foreach (QuoteOrderData orderItem in dgQuoteOrder.SelectedItems)
                {
                    newOrderList.Add(orderItem);
                }

                foreach (QuoteOrderData orderData in newOrderList)
                {
                    if (CommonUtil.IsQuoteCancellable(orderData))
                    {
                        btnCancel.IsEnabled = true;
                        return;
                    }
                }
                btnCancel.IsEnabled = false;
            }
        }

        private void CancelSelectedOrder(object sender, RoutedEventArgs e)
        {
            CommonUtil.CancelSelectedQuoteOrder(dgQuoteOrder, _MainWindow, true);
        }

        private void CancelAllOrder(object sender, RoutedEventArgs e)
        {
            CommonUtil.CancelAllQuoteOrder(_MainWindow.QuoteOrderDataCollection, _MainWindow);
        }

    }
}
