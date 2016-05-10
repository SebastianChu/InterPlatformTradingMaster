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
    /// AllOrder.xaml 的交互逻辑
    /// </summary>
    public partial class AllOrder : UserControl
    {
        public AllOrder()
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

        //撤单
        private void CancelSelectedOrder(object sender, RoutedEventArgs e)
        {
            CommonUtil.CancelSelectedOrder(dgAllOrder, _MainWindow, true);
        }

        private void CancelAllOrder(object sender, RoutedEventArgs e)
        {
            CommonUtil.CancelAllOrder(_MainWindow.OrderDataCollection, _MainWindow);
        }

        private void dgAllOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridCell dgCell = CommonUtil.GetClickedCell(e);
            if (dgCell != null)
            {
                CommonUtil.CancelOrderByDoubleClick(true, dgAllOrder, _MainWindow);//false
            }
        }

        private void CancelOrderforNewPrice_Click(object sender, RoutedEventArgs e)
        {
           // CommonUtil.CancelOrder_NewOrder(sender, mainWindow.OrderDataCollection);
        }

        private void rbAll_Checked(object sender, RoutedEventArgs e)
        {
            dgAllOrder.ItemsSource = _MainWindow.OrderDataCollection;
            btnCancelAll.IsEnabled = true;
        }

        private void rbPending_Checked(object sender, RoutedEventArgs e)
        {
            dgAllOrder.ItemsSource = _MainWindow.PendingCollection;
            btnCancel.IsEnabled = btnCancelAll.IsEnabled = true;
        }

        private void rbTransaction_Checked(object sender, RoutedEventArgs e)
        {
            //dgAllOrder.ItemsSource = mainWindow.TradeCollection_MX;
            dgAllOrder.ItemsSource = _MainWindow.TradedOrderCollection;
            btnCancel.IsEnabled = btnCancelAll.IsEnabled = false;
        }

        private void rbCanceled_Checked(object sender, RoutedEventArgs e)
        {
            dgAllOrder.ItemsSource = _MainWindow.CancelledOrderData;
            btnCancel.IsEnabled = btnCancelAll.IsEnabled = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //CommonUtil.CancelSelectedOrder(dgAllOrder, _MainWindow, true);
            CommonUtil.CancelOrderByDoubleClick(true, dgAllOrder, _MainWindow);
        }

        private void btnCancelAll_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.CancelAllOrder(_MainWindow.OrderDataCollection, _MainWindow);
        }

        private void btnPreCancel_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.PreCancelSelectedOrder(dgAllOrder, _MainWindow, true);
        }

        private void btnPreCancelAll_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.PreCancelAllOrder(_MainWindow.OrderDataCollection, _MainWindow);
        }

        private void ExportDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.ExportedData_Click(sender, e, true);
        }

        private void miAutoAdjustColumnWidth_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.AutoAdjustColumnWidth_Click(sender, e);
        }

        


        private void dgAllOrder_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ///先判断是否需要显示撤单菜单
            bool hasCancellableOrder = false;

            if (dgAllOrder.SelectedItem != null)
            {
                List<object> selectedOrder = new List<object>();
                foreach (var item in dgAllOrder.SelectedItems)
                {
                    selectedOrder.Add(item);
                }

                foreach (var item in selectedOrder)
                {
                    if (item != null)
                    {
                        //撤单
                        if (CommonUtil.IsCancellable(item))
                        {
                            hasCancellableOrder = true;
                        }
                    }
                }
            }

            if (hasCancellableOrder)
            {
                sptDgMenu.Visibility = miCancel.Visibility = miCancelAll.Visibility = miCancelAndReOrder.Visibility = Visibility.Visible;
            }
            else
            {
                sptDgMenu.Visibility = miCancel.Visibility = miCancelAll.Visibility = miCancelAndReOrder.Visibility = Visibility.Collapsed;
            }
        }

        private void miCancelAndReOrder_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.CancelOrderByDoubleClick(true, dgAllOrder, _MainWindow);
        }

        private void dgAllOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAllOrder.SelectedItem != null)
            {
                List<Q7JYOrderData> newOrderList = new List<Q7JYOrderData>();
                foreach (Q7JYOrderData orderItem in dgAllOrder.SelectedItems)
                {
                    newOrderList.Add(orderItem);
                }

                foreach (Q7JYOrderData orderData in newOrderList)
                {
                    if (CommonUtil.IsCancellable(orderData))
                    {
                        btnCancel.IsEnabled = true;
                        return;
                    }
                }
                btnCancel.IsEnabled = false;
            }
        }
    }
}
