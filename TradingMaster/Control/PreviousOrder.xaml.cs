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
using TradingMaster.CodeSet;

namespace TradingMaster.Control
{
    /// <summary>
    /// PreviousOrder.xaml 的交互逻辑
    /// </summary>
    public partial class PreviousOrder : UserControl
    {
        public static PreviousOrder PreOrder;

        public PreviousOrder()
        {
            InitializeComponent();
            PreOrder = this;
        }

        public void Init(MainWindow parent)
        {
            CommonStaticMemeber.LoadStyleFromUserFile();
            this.DataContext = this;
            this._MainWindow = parent;
            //DataGridColumnBridgeUtils.LoadColumnsSettingFromFile(dgMaiConditionOrder, MainWindow.SettingDictionaryPath);

            rbAll.IsChecked = true;
        }

        public TradeClientStyleUI CurrentClientStyleUI
        {
            get { return CommonStaticMemeber.CurrentClientStyleUI; }
        }

        private MainWindow _MainWindow { get; set; }

        private void rbAll_Checked(object sender, RoutedEventArgs e)
        {
            dgMaiConditionOrder.ItemsSource = _MainWindow.PreConditionOrderData;
        }

        private void rbMaidan_Checked(object sender, RoutedEventArgs e)
        {
            dgMaiConditionOrder.ItemsSource = _MainWindow.PreOrderData;
        }

        private void rbCondition_Checked(object sender, RoutedEventArgs e)
        {
            dgMaiConditionOrder.ItemsSource = _MainWindow.ConditionOrderData;
        }

        private void rbSent_Checked(object sender, RoutedEventArgs e)
        {
            dgMaiConditionOrder.ItemsSource = _MainWindow.SentOrderData;
        }
        /// <summary>
        /// 自动单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbAuto_Checked(object sender, RoutedEventArgs e)
        {
            //dgMaiConditionOrder.ItemsSource = mainWindow.AutoOrderData;//Todo: 自动单绑定
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            MaiDanHandle(sender, e);
        }

        /// <summary>
        /// 埋单提交（选中）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaiDanHandle(object sender, RoutedEventArgs e)
        {
            List<Q7JYOrderData> selectedOrder = new List<Q7JYOrderData>();
            foreach (var item in dgMaiConditionOrder.SelectedItems)
            {
                selectedOrder.Add((Q7JYOrderData)item);
            }

            foreach (var item in selectedOrder)
            {
                Q7JYOrderData o = (Q7JYOrderData)item;
                if (o != null)
                {
                    //对o撤单
                    if (IsMaiDanHandlable(o))
                    {
                        TradeDataClient.GetClientInstance().RequestOrder(o.InvestorID, BACKENDTYPE.CTP, new RequestContent("NewOrderSingle", new List<object>() 
                            { CodeSetManager.GetContractInfo(o.Code, CodeSetManager.ExNameToCtp(o.Exchange)), SIDETYPE.BUY, PosEffect.Close, 0, 0, 0, o.OrderID }));
                    }
                }
            }
        }

        /// <summary>
        /// 埋单能够提交
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private Boolean IsMaiDanHandlable(Q7JYOrderData o)
        {
            if (o.OrderStatus == "预置单有效")
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 撤单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CommonUtil.CancelSelectedOrder(dgMaiConditionOrder, _MainWindow, true);
                //foreach (var item in dgMaiConditionOrder.SelectedItems)
                //{
                //    if (item != null && CommonUtil.IsCancellable(item))
                //    {
                //        selectedOrder.Add(item);
                //    }
                //}
                //foreach (var item in selectedOrder)
                //{
                //    if (item != null && item is Q7JYOrderData)
                //    {
                //        //撤单
                //        CommonUtil.CancelOrder(item);
                //        //Q7JYOrderData tempCancelItem = item as Q7JYOrderData;
                //        //JYDataServer.getServerInstance().AddToOrdQueue(new CTPRequestContent("DeletePreOrder", new List<object>() { tempCancelItem.OrderID }));
                //        //JYDataServer.getServerInstance().AddToOrdQueue(new CTPRequestContent("PreCancelOrder", new List<object>() { tempCancelItem.Code, tempCancelItem.FrontID, tempCancelItem.SessionID, tempCancelItem.OrderRef, tempCancelItem.Exchange, tempCancelItem.OrderID }));
                //    }
                //}
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
        }

        private void ExportDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.ExportedData_Click(sender, e);
        }

        private void miAutoAdjustColumnWidth_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.AutoAdjustColumnWidth_Click(sender, e);
        }

        private void dgMaiConditionOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataGridCell dgCell = CommonUtil.GetClickedCell(e);
                if (dgCell != null)
                {
                    CommonUtil.CancelSelectedOrder(dgMaiConditionOrder, _MainWindow, true);//TODO
                    //if (dgMaiConditionOrder.SelectedItem != null)
                    //{
                    //    Q7JYOrderData tempCancelItem = dgMaiConditionOrder.SelectedItem as Q7JYOrderData;
                    //    if (tempCancelItem != null && CommonUtil.IsCancellable(tempCancelItem))
                    //    {
                    //        //JYDataServer.getServerInstance().AddToOrdQueue(new CTPRequestContent("DeletePreOrder", new List<object>() { tempCancelItem.OrderID }));
                    //        JYDataServer.getServerInstance().AddToOrdQueue(new CTPRequestContent("PreCancelOrder", new List<object>() { tempCancelItem.Code, tempCancelItem.FrontID, tempCancelItem.SessionID, tempCancelItem.OrderRef, tempCancelItem.Exchange, tempCancelItem.OrderID }));
                    //    }
                    //}
                }
                
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
        }

    }
}
