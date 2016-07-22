using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
//using FixHQDataLib;
//using CodeGen;
//using JYData;
using System.Xml.Serialization;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using TradingMaster.Control;
using TradingMaster.CodeSet;
using System.Windows.Data;
using TradingMaster.JYData;

namespace TradingMaster
{
    public class CommonUtil
    {
        public static readonly string DefaultHQDisplayValue = "-";
        public string Map_oldValue;

        public static readonly string DataGridExportedFileType = ".txt";
        public static readonly string DataGridExportedFileTypeTXT = ".txt";
        public static readonly string DataGridExportedFileTypeCSV = ".csv";
        public static readonly string DataGridExportedFileFilter = "文本文件(*.txt)|*.txt|数据列表文件(*.csv)|*.csv";
        public static readonly string DataGridExportedFileFilterYS = "文本文件(*.txt)|*.txt|Excel文件(*.csv)|*.csv";
        public static readonly string DataGridExportedFileFilterAll = "文本文件(*.txt;*.csv)|*.csv;*.txt";
        public static readonly string DataGridExportedFileFilterTXT = "文本文件(*.txt)|*.txt";
        public static readonly string DataGridExportedFileFilterCSV = "数据列表文件(*.csv)|*.csv";
        //文本文件(*.txt)|*.txt|所有文件(*.*)|*.*
        private static readonly int DataGridExportedBlankPadNumber = 1;

        public static readonly DateTime DefaultEmptyDateTime = new DateTime(1900, 1, 1, 0, 0, 0);
        private static readonly string ConfirmFileFullPath = "\\setting\\curDate.txt";
        private static readonly string ConfirmDateFormat = "yyyyMMdd";
        public static readonly double AutoAdjustColumnWidth = -1;

        public void kp_TextChanged(object sender, TextChangedEventArgs e, Key map_Key, bool allowDelete)
        {
            TextBox tb_kp = sender as TextBox;
            switch (map_Key.ToString())
            {
                case "D1":
                    tb_kp.Text = "开仓";
                    Map_oldValue = tb_kp.Text;
                    tb_kp.Tag = tb_kp.Text;
                    break;
                case "D2":
                    tb_kp.Text = "平仓";
                    Map_oldValue = tb_kp.Text;
                    tb_kp.Tag = tb_kp.Text;
                    break;
                case "D3":
                    tb_kp.Text = "平今";
                    Map_oldValue = tb_kp.Text;
                    tb_kp.Tag = tb_kp.Text;
                    break;
                default:
                    tb_kp.Text = (string)tb_kp.Tag;
                    break;

            }
        }

        public void buyAndSell_TextChanged(object sender, TextChangedEventArgs e, Key map_Key)
        {
            buyAndSell_TextChanged(sender, e, map_Key, false);
        }


        public void buyAndSell_TextChanged(object sender, TextChangedEventArgs e, Key map_Key, bool allowDelete)
        {
            TextBox tb_buyAndSell = sender as TextBox;
            switch (map_Key.ToString())
            {
                case "D1":
                    tb_buyAndSell.Text = "买入";
                    Map_oldValue = tb_buyAndSell.Text;
                    tb_buyAndSell.Tag = tb_buyAndSell.Text;
                    break;

                case "D3":
                    tb_buyAndSell.Text = "卖出";
                    Map_oldValue = tb_buyAndSell.Text;
                    tb_buyAndSell.Tag = tb_buyAndSell.Text;
                    break;
                default:
                    tb_buyAndSell.Text = (string)tb_buyAndSell.Tag;
                    break;
            }
        }

        /// <summary>
        /// 将小键盘上的123转换成普通的123
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public Key ConvertNumPadKey(KeyEventArgs e)
        {
            Key map_Key = e.Key;
            switch (map_Key.ToString())
            {
                case "NumPad1":
                    map_Key = Key.D1;
                    break;
                case "NumPad2":
                    map_Key = Key.D2;
                    break;
                case "NumPad3":
                    map_Key = Key.D3;
                    break;
                case "NumPad4":
                    map_Key = Key.D4;
                    break;
                case "NumPad5":
                    map_Key = Key.D5;
                    break;
                case "NumPad6":
                    map_Key = Key.D6;
                    break;
                case "NumPad7":
                    map_Key = Key.D7;
                    break;
                default:
                    map_Key = e.Key;
                    break;
            }
            return map_Key;
        }

        public void ChangeListViewCellColor(int rowIndex, int cellIndex, ListView lv, Brush colorBrush)
        {
            // rowIndex 和cellIndex 基於 0.
            // 首先应得到ListViewItem,毋庸置疑,所有可视UI元素都继承了UIElement:
            TextBlock tb = new TextBlock();
            UIElement u = lv.ItemContainerGenerator.ContainerFromIndex(rowIndex) as UIElement;
            if (u == null) return;
            // 然后在ListViewItem元素树中搜寻单元格:
            while (u != null)
            {
                if (u is Grid)
                {
                    u = VisualTreeHelper.GetChild(u, 1) as UIElement;
                }
                else
                {
                    u = VisualTreeHelper.GetChild(u, 0) as UIElement;
                }
                if (u is GridViewRowPresenter)
                {
                    u = VisualTreeHelper.GetChild(u, cellIndex) as UIElement;
                    tb = u as TextBlock;
                    tb.Foreground = colorBrush;
                    return;
                }
            }
        }
        /// <summary>
        /// 设置listview单元格背景色
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="cellIndex"></param>
        /// <param name="lv"></param>
        /// <param name="colorBrush"></param>
        public void ChangeListViewCellBackColor(int rowIndex, int cellIndex, ListView lv, Brush colorBrush)
        {
            // rowIndex 和cellIndex 基於 0.
            // 首先应得到ListViewItem,毋庸置疑,所有可视UI元素都继承了UIElement:
            TextBlock tb = new TextBlock();
            UIElement u = lv.ItemContainerGenerator.ContainerFromIndex(rowIndex) as UIElement;
            if (u == null) return;
            // 然后在ListViewItem元素树中搜寻单元格:
            while (u != null)
            {
                if (u is Grid)
                {
                    u = VisualTreeHelper.GetChild(u, 1) as UIElement;
                }
                else
                {
                    u = VisualTreeHelper.GetChild(u, 0) as UIElement;
                }
                if (u is GridViewRowPresenter)
                {
                    u = VisualTreeHelper.GetChild(u, cellIndex) as UIElement;
                    tb = u as TextBlock;
                    tb.Background = colorBrush;
                    return;
                }
            }
        }

        public static string GetSettingPath()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }

        public static void writeFile(string fileName, string fileContent)
        {
            int i = fileName.LastIndexOf('\\');
            string f = fileName.Substring(0, i + 1);
            if (Directory.Exists(f) == false)
            {
                Directory.CreateDirectory(f);
            }
            StreamWriter sw = File.CreateText(fileName);
            sw.WriteLine(fileContent);
            sw.Close();
            sw.Dispose();
        }
        //public BitmapImage getImageSource(Uri uri)
        //{
        //    BitmapImage image = new BitmapImage();
        //    image.BeginInit();
        //    image.UriSource = uri;
        //    image.EndInit();
        //    return image;
        //}


        public static void IPaddress_Updated(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            TextChange[] change = new TextChange[e.Changes.Count];
            e.Changes.CopyTo(change, 0);

            int offset = change[0].Offset;
            if (change[0].AddedLength > 0)
            {
                string str_withdot = null;
                double num = 0;
                if (textbox.Text.Contains("."))
                {
                    str_withdot = textbox.Text.Replace(".", "");
                    if (!(Double.TryParse(str_withdot, out num)) || textbox.Text.Contains(","))
                    {
                        textbox.Text = textbox.Text.Remove(offset, change[0].AddedLength);
                        textbox.Select(offset, 0);
                    }
                }
                else
                    if (!(Double.TryParse(textbox.Text, out num)) || textbox.Text.Contains(","))
                    {
                        textbox.Text = textbox.Text.Remove(offset, change[0].AddedLength);
                        textbox.Select(offset, 0);
                    }
            }
        }

        public static void Port_Updated(object sender, TextChangedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            TextChange[] change = new TextChange[e.Changes.Count];
            e.Changes.CopyTo(change, 0);

            int offset = change[0].Offset;
            if (change[0].AddedLength > 0)
            {
                double num = 0;
                if (!Double.TryParse(textbox.Text, out num) || textbox.Text.Contains(",") || textbox.Text.Contains("."))
                {
                    textbox.Text = textbox.Text.Remove(offset, change[0].AddedLength);
                    textbox.Select(offset, textbox.Text.Length);
                }
            }
        }


        /// <summary>
        /// 这个方法最开始用于获得订单属性，因为用户操作的数据有JYOrderData和GeneralData两种，所以使用反射设置方法
        /// 放到CommonUtils中供其他类调用
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static object GetOrderProperty(object item, string property)
        {
            //string orderStatusProperty = "OrderStatus";
            Type type = item.GetType();
            if (type.GetProperty(property).GetValue(item, null) != null)
            {
                return type.GetProperty(property).GetValue(item, null);
            }
            return null;
        }

        public static void CancelSelectedOrder(object sender, MainWindow mainWindow, bool needConfirm)
        {
            DataGrid dg = GetDataGridFromMenuItemBySender(sender);

            List<object> selectedOrder = new List<object>();
            try
            {
                foreach (var item in dg.SelectedItems)
                {
                    if (item != null && CommonUtil.IsCancellable(item))
                    {
                        selectedOrder.Add(item);
                    }
                }
                CancelOrderWithConfirm(mainWindow, selectedOrder, false, needConfirm);

            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        public static void CancelSelectedQuoteOrder(object sender, MainWindow mainWindow, bool needConfirm)
        {
            DataGrid dg = GetDataGridFromMenuItemBySender(sender);

            List<object> selectedOrder = new List<object>();
            try
            {
                foreach (var item in dg.SelectedItems)
                {
                    if (item != null && CommonUtil.IsQuoteCancellable(item))
                    {
                        selectedOrder.Add(item);
                    }
                }
                CancelQuoteOrderWithConfirm(mainWindow, selectedOrder, false, needConfirm);

            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        public static void CancelSelectedExecOrder(object sender, MainWindow mainWindow, bool needConfirm)
        {
            DataGrid dg = GetDataGridFromMenuItemBySender(sender);

            List<object> selectedOrder = new List<object>();
            try
            {
                foreach (var item in dg.SelectedItems)
                {
                    if (item != null && CommonUtil.IsExecCancellable(item))
                    {
                        selectedOrder.Add(item);
                    }
                }
                CancelExecOrderWithConfirm(mainWindow, selectedOrder, false, needConfirm);

            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        public static void PreCancelSelectedOrder(object sender, MainWindow mainWindow, bool needConfirm)
        {
            DataGrid dg = GetDataGridFromMenuItemBySender(sender);

            List<object> selectedOrder = new List<object>();
            try
            {
                foreach (var item in dg.SelectedItems)
                {
                    if (item != null && CommonUtil.IsCancellable(item))
                    {
                        selectedOrder.Add(item);
                    }
                }
                PreCancelOrderWithConfirm(mainWindow, selectedOrder, false, needConfirm);

            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        public static DataGrid GetDataGridFromMenuItemBySender(object sender)
        {
            DataGrid dg = null;
            if (sender is DataGrid)
            {
                dg = sender as DataGrid;
                return dg;
            }
            MenuItem mi = sender as MenuItem;
            ContextMenu cm = mi.Parent as ContextMenu;
            Popup popup = cm.Parent as Popup;
            dg = popup.PlacementTarget as DataGrid;
            return dg;
        }

        private static void CancelOrderWithConfirm(MainWindow mainWindow, List<object> selectedOrder, bool cancelAll, bool needConfirm)
        {
            if (selectedOrder.Count > 0)
            {
                bool cancelOrder = false;
                string messageText = string.Empty;
                string canceledOrderIds = string.Empty;
                foreach (var item in selectedOrder)
                {
                    Q7JYOrderData order = item as Q7JYOrderData;
                    if (messageText != string.Empty)
                    {
                        messageText = messageText + "\n";
                    }
                    messageText = messageText + string.Format("撤单{0}: {1} {2}  {3} {4}手 于价格{5}, {6}",
                        order.OrderID, order.OpenClose, order.BuySell,
                        order.Code, order.CommitHandCount, order.CommitPrice.ToString(), order.Hedge);
                    canceledOrderIds = canceledOrderIds + order.OrderID + " ";
                }


                if (needConfirm && TradingMaster.Properties.Settings.Default.ConfirmBeforeCancelOrder == true)
                {
                    CheckableMessageBox messageBox = new CheckableMessageBox();
                    messageBox.tbMessage.Text = messageText;

                    string windowTitle = string.Format("确认撤单:{0}", canceledOrderIds);
                    if (cancelAll)
                    {
                        windowTitle = "确认撤单全部单";
                    }

                    Window confirmWindow = GetWindow(windowTitle, messageBox, mainWindow);

                    if (confirmWindow.ShowDialog() == true)
                    {
                        if (messageBox.chkConfirm.IsChecked == true)
                        {
                            TradingMaster.Properties.Settings.Default.ConfirmBeforeCancelOrder = false;
                            TradingMaster.Properties.Settings.Default.Save();
                        }
                        cancelOrder = true;
                    }
                }
                else
                {
                    cancelOrder = true;
                }

                if (cancelOrder)
                {
                    foreach (var item in selectedOrder)
                    {
                        CancelOrder(item);
                    }
                }
            }
        }

        private static void CancelQuoteOrderWithConfirm(MainWindow mainWindow, List<object> selectedOrder, bool cancelAll, bool needConfirm)
        {
            if (selectedOrder.Count > 0)
            {
                bool cancelOrder = false;
                string messageText = string.Empty;
                string canceledOrderIds = string.Empty;
                foreach (var item in selectedOrder)
                {
                    QuoteOrderData order = item as QuoteOrderData;
                    if (messageText != string.Empty)
                    {
                        messageText = messageText + "\n";
                    }
                    messageText = messageText + string.Format("撤报价{0}: {1}\n 买 {2} {3}手 于价格{4}; 卖 {5} {6}手 于价格{7}",
                            order.QuoteOrderID, order.Code,
                            order.BidOpenClose, order.BidHandCount, order.BidPrice.ToString(), order.AskOpenClose, order.AskHandCount, order.AskPrice.ToString());
                    canceledOrderIds = canceledOrderIds + order.QuoteOrderID + " ";
                }


                if (needConfirm && TradingMaster.Properties.Settings.Default.ConfirmBeforeCancelOrder == true)
                {
                    CheckableMessageBox messageBox = new CheckableMessageBox();
                    messageBox.tbMessage.Text = messageText;

                    string windowTitle = string.Format("确认撤报价:{0}", canceledOrderIds);
                    if (cancelAll)
                    {
                        windowTitle = "确认撤单全部报价";
                    }

                    Window confirmWindow = GetWindow(windowTitle, messageBox, mainWindow);

                    if (confirmWindow.ShowDialog() == true)
                    {
                        if (messageBox.chkConfirm.IsChecked == true)
                        {
                            TradingMaster.Properties.Settings.Default.ConfirmBeforeCancelOrder = false;
                            TradingMaster.Properties.Settings.Default.Save();
                        }
                        cancelOrder = true;
                    }
                }
                else
                {
                    cancelOrder = true;
                }

                if (cancelOrder)
                {
                    foreach (var item in selectedOrder)
                    {
                        CancelOrder(item);
                    }
                }
            }
        }

        private static void CancelExecOrderWithConfirm(MainWindow mainWindow, List<object> selectedOrder, bool cancelAll, bool needConfirm)
        {
            if (selectedOrder.Count > 0)
            {
                bool cancelOrder = false;
                string messageText = string.Empty;
                string canceledOrderIds = string.Empty;
                foreach (var item in selectedOrder)
                {
                    ExecOrderData order = item as ExecOrderData;
                    if (messageText != string.Empty)
                    {
                        messageText = messageText + "\n";
                    }
                    messageText = messageText + string.Format("取消指令{0} {1}：{2} {3} {4}手 , {5}",
                           order.ExecOrderID, order.Code, order.ActionDirection, order.OpenClose, order.HandCount, order.Hedge);
                    canceledOrderIds = canceledOrderIds + order.ExecOrderID + " ";
                }


                if (needConfirm && TradingMaster.Properties.Settings.Default.ConfirmBeforeCancelOrder == true)
                {
                    CheckableMessageBox messageBox = new CheckableMessageBox();
                    messageBox.tbMessage.Text = messageText;

                    string windowTitle = string.Format("确认取消:{0}", canceledOrderIds);
                    if (cancelAll)
                    {
                        windowTitle = "确认取消全部单";
                    }

                    Window confirmWindow = GetWindow(windowTitle, messageBox, mainWindow);

                    if (confirmWindow.ShowDialog() == true)
                    {
                        if (messageBox.chkConfirm.IsChecked == true)
                        {
                            TradingMaster.Properties.Settings.Default.ConfirmBeforeCancelOrder = false;
                            TradingMaster.Properties.Settings.Default.Save();
                        }
                        cancelOrder = true;
                    }
                }
                else
                {
                    cancelOrder = true;
                }

                if (cancelOrder)
                {
                    foreach (var item in selectedOrder)
                    {
                        CancelOrder(item);
                    }
                }
            }
        }

        private static void PreCancelOrderWithConfirm(MainWindow mainWindow, List<object> selectedOrder, bool cancelAll, bool needConfirm)
        {
            if (selectedOrder.Count > 0)
            {
                bool cancelOrder = false;
                string messageText = string.Empty;
                string canceledOrderIds = string.Empty;
                foreach (var item in selectedOrder)
                {
                    Q7JYOrderData order = item as Q7JYOrderData;
                    if (messageText != string.Empty)
                    {
                        messageText = messageText + "\n";
                    }
                    messageText = messageText + string.Format("预埋撤单{0}: {1} {2}  {3} {4}手 于价格{5}, {6}",
                        order.OrderID, order.OpenClose, order.BuySell,
                        order.Code, order.CommitHandCount, order.CommitPrice.ToString(), order.Hedge);
                    canceledOrderIds = canceledOrderIds + order.OrderID + " ";
                }


                if (needConfirm && TradingMaster.Properties.Settings.Default.ConfirmBeforeCancelOrder == true)
                {
                    CheckableMessageBox messageBox = new CheckableMessageBox();
                    messageBox.tbMessage.Text = messageText;

                    string windowTitle = string.Format("确认预埋撤单:{0}", canceledOrderIds);
                    if (cancelAll)
                    {
                        windowTitle = "确认预埋撤全部单";
                    }

                    Window confirmWindow = GetWindow(windowTitle, messageBox, mainWindow);

                    if (confirmWindow.ShowDialog() == true)
                    {
                        if (messageBox.chkConfirm.IsChecked == true)
                        {
                            TradingMaster.Properties.Settings.Default.ConfirmBeforeCancelOrder = false;
                            TradingMaster.Properties.Settings.Default.Save();
                        }
                        cancelOrder = true;
                    }
                }
                else
                {
                    cancelOrder = true;
                }

                if (cancelOrder)
                {
                    foreach (var item in selectedOrder)
                    {
                        CancelPreCancelOrder(item);
                    }
                }
            }
        }

        public static Window GetWindow(string title, object content, Window owner)
        {
            Window window = new Window();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Uri IconImage = new Uri("pack://application:,,,/TradingMaster;component/image/Master.ico", UriKind.RelativeOrAbsolute);
            window.Icon = getImageSource(IconImage);
            window.Content = content;
            window.Title = title;
            window.ResizeMode = ResizeMode.NoResize;
            window.SizeToContent = SizeToContent.WidthAndHeight;
            try
            {
                window.Owner = owner;
            }
            catch (Exception ex)
            {
                //owner还没有显示过时不能设置
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
            window.KeyUp += new KeyEventHandler(window_KeyUp);
            return window;
        }

        static void window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (sender is Window)
                {
                    try
                    {
                        (sender as Window).DialogResult = false;
                    }
                    catch (Exception ex)
                    {
                        Util.Log("exception: " + ex.Message);
                        Util.Log("exception: " + ex.StackTrace);
                    }
                    (sender as Window).Close();
                }
            }
        }

        public static BitmapImage getImageSource(Uri uri)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = uri;
            image.EndInit();
            return image;
        }

        /// <summary>
        /// setNewOrder == true是为撤单改价
        /// </summary>
        /// <param name="setNewOrder"></param>
        public static void CancelOrderByDoubleClick(bool setNewOrder, DataGrid dgOrder, MainWindow mainWindow)
        {
            if (dgOrder.SelectedItem == null)
            {
                return;
            }

            //找出需要撤销的单
            List<Q7JYOrderData> selectedOrder = new List<Q7JYOrderData>();
            foreach (var item in dgOrder.SelectedItems)
            {
                if (CommonUtil.IsCancellable(item))
                {
                    selectedOrder.Add(item as Q7JYOrderData);
                }
            }

            string canceledOrderIds = string.Empty;
            if (selectedOrder.Count > 0)
            {
                //重新下单时下最后一条单
                //string text = new StringBuilder();
                bool cancleOrder = false;
                if (TradingMaster.Properties.Settings.Default.ConfirmBeforeCancelOrder == true)
                {
                    string messageText = string.Empty;

                    foreach (var item in selectedOrder)
                    {
                        if (messageText != string.Empty)
                        {
                            messageText = messageText + "\n";
                        }
                        messageText = messageText + string.Format("撤单{0}: {1} {2}  {3} {4}手 于价格{5}, {6}",
                            item.OrderID, item.OpenClose, item.BuySell,
                            item.Code, item.CommitHandCount, item.CommitPrice.ToString(), item.Hedge);
                        canceledOrderIds = canceledOrderIds + item.OrderID + " ";
                    }

                    CheckableMessageBox messageBox = new CheckableMessageBox();
                    messageBox.tbMessage.Text = messageText;
                    string windowTitle = string.Format("确认撤单：{0}", canceledOrderIds);

                    Window confirmWindow = GetWindow(windowTitle, messageBox, mainWindow);

                    if (confirmWindow.ShowDialog() == true)
                    {
                        if (messageBox.chkConfirm.IsChecked == true)
                        {
                            TradingMaster.Properties.Settings.Default.ConfirmBeforeCancelOrder = false;
                            TradingMaster.Properties.Settings.Default.Save();
                        }
                        cancleOrder = true;

                    }
                }
                else
                {
                    cancleOrder = true;
                }

                if (cancleOrder)
                {
                    //JYDataServer.getServerInstance().CancelOrder(order.OrderID,order.BorkerID, order.Code);
                    if (setNewOrder)
                    {
                        foreach (var item in selectedOrder)
                        {
                            mainWindow.uscNewOrderPanel.SetOrderInfoByExistingOrder(item);
                            break;
                        }
                    }

                    foreach (var item in selectedOrder)
                    {
                        CancelOrder(item);
                    }
                }
            }

        }

        /// <summary>
        /// setNewOrder == true是为撤单改价
        /// </summary>
        /// <param name="setNewOrder"></param>
        public static void CancelQuoteOrderByDoubleClick(bool setNewOrder, DataGrid dgOrder, MainWindow mainWindow)
        {
            if (dgOrder.SelectedItem == null)
            {
                return;
            }

            //找出需要撤销的单
            List<QuoteOrderData> selectedOrder = new List<QuoteOrderData>();
            foreach (var item in dgOrder.SelectedItems)
            {
                if (CommonUtil.IsQuoteCancellable(item))
                {
                    selectedOrder.Add(item as QuoteOrderData);
                }
            }

            string canceledOrderIds = string.Empty;
            if (selectedOrder.Count > 0)
            {
                //重新下单时下最后一条单
                //string text = new StringBuilder();
                bool cancleOrder = false;
                if (TradingMaster.Properties.Settings.Default.ConfirmBeforeCancelOrder == true)
                {
                    string messageText = string.Empty;

                    foreach (var item in selectedOrder)
                    {
                        if (messageText != string.Empty)
                        {
                            messageText = messageText + "\n";
                        }
                        messageText = messageText + string.Format("撤报价{0}: {1}\n 买 {2} {3}手 于价格{4}; 卖 {5} {6}手 于价格{7}",
                            item.QuoteOrderID, item.Code,
                            item.BidOpenClose, item.BidHandCount, item.BidPrice.ToString(), item.AskOpenClose, item.AskHandCount, item.AskPrice.ToString());
                        canceledOrderIds = canceledOrderIds + item.QuoteOrderID + " ";
                    }

                    CheckableMessageBox messageBox = new CheckableMessageBox();
                    messageBox.tbMessage.Text = messageText;
                    string windowTitle = string.Format("确认撤报价：{0}", canceledOrderIds);

                    Window confirmWindow = GetWindow(windowTitle, messageBox, mainWindow);

                    if (confirmWindow.ShowDialog() == true)
                    {
                        if (messageBox.chkConfirm.IsChecked == true)
                        {
                            TradingMaster.Properties.Settings.Default.ConfirmBeforeCancelOrder = false;
                            TradingMaster.Properties.Settings.Default.Save();
                        }
                        cancleOrder = true;

                    }
                }
                else
                {
                    cancleOrder = true;
                }

                if (cancleOrder)
                {
                    //JYDataServer.getServerInstance().CancelOrder(order.OrderID,order.BorkerID, order.Code);
                    //if (setNewOrder)
                    //{
                    //    foreach (var item in selectedOrder)
                    //    {
                    //        mainWindow.uscNewOrderPanel.SetOrderInfoByExistingOrder(item);
                    //        break;
                    //    }
                    //}

                    foreach (var item in selectedOrder)
                    {
                        CancelOrder(item);
                    }
                }
            }

        }

        /// <summary>
        /// setNewOrder == true是为撤单改价
        /// </summary>
        /// <param name="setNewOrder"></param>
        public static void CancelExecOrderByDoubleClick(bool setNewOrder, DataGrid dgOrder, MainWindow mainWindow)
        {
            if (dgOrder.SelectedItem == null)
            {
                return;
            }

            //找出需要撤销的单
            List<ExecOrderData> selectedOrder = new List<ExecOrderData>();
            foreach (var item in dgOrder.SelectedItems)
            {
                if (CommonUtil.IsExecCancellable(item))
                {
                    selectedOrder.Add(item as ExecOrderData);
                }
            }

            string canceledOrderIds = string.Empty;
            if (selectedOrder.Count > 0)
            {
                //重新下单时下最后一条单
                //string text = new StringBuilder();
                bool cancleOrder = false;
                if (TradingMaster.Properties.Settings.Default.ConfirmBeforeCancelOrder == true)
                {
                    string messageText = string.Empty;

                    foreach (var item in selectedOrder)
                    {
                        if (messageText != string.Empty)
                        {
                            messageText = messageText + "\n";
                        }
                        messageText = messageText + string.Format("取消指令{0} {1}：{2} {3} {4}手 , {5}",
                            item.ExecOrderID, item.Code, item.ActionDirection, item.OpenClose, item.HandCount, item.Hedge);
                        canceledOrderIds = canceledOrderIds + item.ExecOrderID + " ";
                    }

                    CheckableMessageBox messageBox = new CheckableMessageBox();
                    messageBox.tbMessage.Text = messageText;
                    string windowTitle = string.Format("确认取消：{0}", canceledOrderIds);

                    Window confirmWindow = GetWindow(windowTitle, messageBox, mainWindow);

                    if (confirmWindow.ShowDialog() == true)
                    {
                        if (messageBox.chkConfirm.IsChecked == true)
                        {
                            TradingMaster.Properties.Settings.Default.ConfirmBeforeCancelOrder = false;
                            TradingMaster.Properties.Settings.Default.Save();
                        }
                        cancleOrder = true;

                    }
                }
                else
                {
                    cancleOrder = true;
                }

                if (cancleOrder)
                {
                    //JYDataServer.getServerInstance().CancelOrder(order.OrderID,order.BorkerID, order.Code);
                    //if (setNewOrder)
                    //{
                    //    foreach (var item in selectedOrder)
                    //    {
                    //        mainWindow.uscNewOrderPanel.SetOrderInfoByExistingOrder(item);
                    //        break;
                    //    }
                    //}

                    foreach (var item in selectedOrder)
                    {
                        CancelOrder(item);
                    }
                }
            }

        }

        public static void CancelAllOrder(object sender, MainWindow mainWindow)
        {
            DataGrid dg = GetDataGridFromMenuItemBySender(sender);

            List<object> lstAllOrder = new List<object>();
            try
            {
                foreach (var item in dg.ItemsSource)
                {
                    if (item != null && CommonUtil.IsCancellable(item))
                    {
                        //撤单
                        lstAllOrder.Add(item);
                    }
                }
                CancelOrderWithConfirm(mainWindow, lstAllOrder, true, true);

            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        public static void CancelAllQuoteOrder(object sender, MainWindow mainWindow)
        {
            DataGrid dg = GetDataGridFromMenuItemBySender(sender);

            List<object> lstAllOrder = new List<object>();
            try
            {
                foreach (var item in dg.ItemsSource)
                {
                    if (item != null && CommonUtil.IsQuoteCancellable(item))
                    {
                        //撤单
                        lstAllOrder.Add(item);
                    }
                }
                CancelQuoteOrderWithConfirm(mainWindow, lstAllOrder, true, true);

            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        public static void CancelAllExecOrder(object sender, MainWindow mainWindow)
        {
            DataGrid dg = GetDataGridFromMenuItemBySender(sender);

            List<object> lstAllOrder = new List<object>();
            try
            {
                foreach (var item in dg.ItemsSource)
                {
                    if (item != null && CommonUtil.IsExecCancellable(item))
                    {
                        //撤单
                        lstAllOrder.Add(item);
                    }
                }
                CancelExecOrderWithConfirm(mainWindow, lstAllOrder, true, true);

            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        public static void PreCancelAllOrder(object sender, MainWindow mainWindow)
        {
            DataGrid dg = GetDataGridFromMenuItemBySender(sender);

            List<object> lstAllOrder = new List<object>();
            try
            {
                foreach (var item in dg.ItemsSource)
                {
                    if (item != null && CommonUtil.IsCancellable(item))
                    {
                        //撤单
                        lstAllOrder.Add(item);
                    }
                }
                PreCancelOrderWithConfirm(mainWindow, lstAllOrder, true, true);
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        public static void CancelAllOrder(System.Collections.IEnumerable cancelledItems, MainWindow mainWindow)
        {

            List<object> lstAllOrder = new List<object>();
            try
            {
                foreach (var item in cancelledItems)
                {
                    if (item != null && CommonUtil.IsCancellable(item))
                    {
                        //撤单
                        lstAllOrder.Add(item);
                    }
                }
                CancelOrderWithConfirm(mainWindow, lstAllOrder, true, true);
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        public static void CancelAllQuoteOrder(System.Collections.IEnumerable cancelledItems, MainWindow mainWindow)
        {

            List<object> lstAllOrder = new List<object>();
            try
            {
                foreach (var item in cancelledItems)
                {
                    if (item != null && CommonUtil.IsQuoteCancellable(item))
                    {
                        //撤单
                        lstAllOrder.Add(item);
                    }
                }
                CancelQuoteOrderWithConfirm(mainWindow, lstAllOrder, true, true);
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        public static void CancelAllExecOrder(System.Collections.IEnumerable cancelledItems, MainWindow mainWindow)
        {

            List<object> lstAllOrder = new List<object>();
            try
            {
                foreach (var item in cancelledItems)
                {
                    if (item != null && CommonUtil.IsExecCancellable(item))
                    {
                        //撤单
                        lstAllOrder.Add(item);
                    }
                }
                CancelExecOrderWithConfirm(mainWindow, lstAllOrder, true, true);
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        public static void PreCancelAllOrder(System.Collections.IEnumerable cancelledItems, MainWindow mainWindow)
        {
            List<object> lstAllOrder = new List<object>();
            try
            {
                foreach (var item in cancelledItems)
                {
                    if (item != null && CommonUtil.IsCancellable(item))
                    {
                        //撤单
                        lstAllOrder.Add(item);
                    }
                }
                PreCancelOrderWithConfirm(mainWindow, lstAllOrder, true, true);

            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        public static void CancelOrder(object order)
        {
            if (order is Q7JYOrderData)
            {
                if (IsCancellable(order))
                {
                    Q7JYOrderData o = (Q7JYOrderData)order;
                    if (o != null)
                    {
                        if (o.OrderID.StartsWith("TJBD"))
                        {
                            TradeDataClient.GetClientInstance().RequestOrder(o.InvestorID, o.BackEnd, new RequestContent("DeletePreOrder", new List<object>() { o.OrderID }));
                        }
                        else
                        {
                            TradeDataClient.GetClientInstance().RequestOrder(o.InvestorID, o.BackEnd, new RequestContent("CancelOrder", new List<object>() { o.Code, o.FrontID, o.SessionID, o.OrderRef, o.Exchange, o.OrderID }));
                        }                        
                    }
                }
            }

            else if (order is QuoteOrderData)
            {
                if (IsQuoteCancellable(order))
                {
                    QuoteOrderData o = (QuoteOrderData)order;
                    if (o != null)
                    {
                        TradeDataClient.GetClientInstance().RequestOrder(o.InvestorID, o.BackEnd, new RequestContent("CancelQuoteOrder", new List<object>() { o.Code, o.FrontID, o.SessionID, o.QuoteRef, o.Exchange, o.QuoteOrderID }));
                    }
                }
            }
            else if (order is ExecOrderData) 
            {
                if (IsExecCancellable(order))
                {
                    ExecOrderData o = (ExecOrderData)order;
                    if (o != null)
                    {
                        TradeDataClient.GetClientInstance().RequestOrder(o.InvestorID, o.BackEnd, new RequestContent("CancelExecOrder", new List<object>() { o.Code, o.FrontID, o.SessionID, o.ExecOrderRef, o.Exchange, o.ExecOrderID }));
                    }
                }
            }
        }

        public static void CancelPreCancelOrder(object order)
        {
            if (IsCancellable(order))
            {
                if (order is Q7JYOrderData)
                {
                    Q7JYOrderData o = (Q7JYOrderData)order;
                    if (o != null)
                    {
                        //if (!o.OrderID.StartsWith("TJBD"))
                        {
                            TradeDataClient.GetClientInstance().RequestOrder(o.InvestorID, o.BackEnd, new RequestContent("PreCancelOrder", new List<object>() { o.Code, o.FrontID, o.SessionID, o.OrderRef, o.Exchange, o.OrderID }));
                        }
                    }
                }
            }
        }

        public static Boolean IsCancellable(object order)
        {
            //return true; // For Test API
            try
            {
                string orderStatus = CommonUtil.GetOrderProperty(order, "OrderStatus").ToString();
                string code = CommonUtil.GetOrderProperty(order, "Code").ToString();
                return code != "" && orderStatus != null && (orderStatus.Contains("正报") || orderStatus.Contains("未成交") || orderStatus.Contains("已报") || orderStatus.Contains("未发送") || orderStatus.Contains("未报") || orderStatus.Contains("自动单") || orderStatus.Contains("条件单") || orderStatus.Contains("埋单") || orderStatus.Contains("未触发") || orderStatus.Contains("部分成交"));
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
            return false;
        }

        public static Boolean IsQuoteCancellable(object order)
        {
            try
            {
                string quoteStatus = CommonUtil.GetOrderProperty(order, "QuoteStatus").ToString();
                string code = CommonUtil.GetOrderProperty(order, "Code").ToString();
                return code != "" && quoteStatus != null && (quoteStatus.Contains("未成交") || quoteStatus.Contains("已报") || quoteStatus.Contains("未发送") || quoteStatus.Contains("未报") || quoteStatus.Contains("部分成交"));
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
            return false;
        }

        public static Boolean IsExecCancellable(object order)
        {
            try
            {
                string execStatus = CommonUtil.GetOrderProperty(order, "ExecStatus").ToString();
                string code = CommonUtil.GetOrderProperty(order, "Code").ToString();
                return code != "" && execStatus != null && (execStatus.Contains("未执行") || execStatus.Contains("正报") || execStatus.Contains("已报") || execStatus.Contains("未发送") || execStatus.Contains("未报"));
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
            return false;
        }

        public static void CancelOrder_NewOrder(object sender, ObservableCollection<Q7JYOrderData> OrderDataCollection)
        {
            //DataGrid dg = GetDataGridFromMenuItemBySender(sender);
            //foreach (var item in OrderDataCollection)
            //{
            //    if (dg.SelectedItem == item)
            //    {
            //        if (item.OrderStatus == "已成" || item.OrderStatus == "已撤单")
            //        {
            //            System.Windows.Forms.MessageBox.Show("委托状态为【" + item.OrderStatus + "】时不可撤单！", "操作提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            //            //break;
            //        }
            //        else
            //        {
            //            CancelOrderforNewPrice Orderwindow = new CancelOrderforNewPrice();
            //            Orderwindow.tbx_Commission.Text = item.OrderID;
            //            Orderwindow.tbx_TxnCode.Text = item.OrderID;
            //            Orderwindow.tbx_Code.Text = item.Code;
            //            Orderwindow.tbx_UpdatedPrice.Text = item.CommitPrice.ToString();
            //            Orderwindow.tbx_OpenClose.Text = item.OpenClose;
            //            Orderwindow.tbx_UpdatedNum.Text = item.CommitHandCount.ToString();
            //            Orderwindow.tbx_Buysell.Text = item.BuySell.TrimStart() + "出";

            //            Orderwindow.ShowDialog();
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 导出表格中的数据到txt文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ExportedData_Click(object sender, RoutedEventArgs e, bool attachDateToFileName)
        {
            ExportedData_Click(sender, e, DataGridExportedFileTypeCSV, DataGridExportedFileFilterAll, attachDateToFileName);
        }

        public static void ExportedData_Click(object sender, RoutedEventArgs e)
        {
            ExportedData_Click(sender, e, DataGridExportedFileTypeCSV, DataGridExportedFileFilterCSV, false);
        }

        public static void ExportedData_Click(object sender, RoutedEventArgs e, string fileType, string fileFilter, bool attachDateToFileName)
        {

            DataGrid dg = GetDataGridFromMenuItemBySender(sender);

            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            //默认的文件名写在菜单的Tag中
            string fileName = (sender as MenuItem).Tag.ToString();
            if (attachDateToFileName)
            {
                fileName = fileName + "_" + DateTime.Now.ToString("yyMMdd");
            }
            sfd.FileName = fileName + fileType;
            sfd.Filter = fileFilter;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                //exportedLines保存将要输出的表格里面的数据
                List<List<string>> exportedLines = new List<List<string>>();
                //lstValue存储表格中一行的数据，所有行存储到exportedLines
                List<string> lstValue = new List<string>();
                //lstMaxLength保存各列最长占用的宽度，用于对其文件
                List<int> lstMaxLength = new List<int>();

                List<DataGridColumn> lstColumnSortByDisplayIndex = new List<DataGridColumn>();

                var sortedColumns = from item in dg.Columns
                                    orderby item.DisplayIndex ascending
                                    select item;

                foreach (var item in sortedColumns)
                {
                    lstColumnSortByDisplayIndex.Add(item);
                }

                //先根据列头初始化lstMaxLength
                for (int i = 0; i < lstColumnSortByDisplayIndex.Count; i++)
                {
                    lstMaxLength.Add(0);
                    lstValue.Add(string.Empty);
                    if (lstColumnSortByDisplayIndex[i].Visibility == Visibility.Visible)
                    {
                        lstValue[i] = lstColumnSortByDisplayIndex[i].Header.ToString();
                        lstMaxLength[i] = Encoding.Default.GetBytes(lstColumnSortByDisplayIndex[i].Header.ToString()).Length;
                    }
                }
                exportedLines.Add(lstValue);

                //将表中的数据放到exportedLines，并更新lstMaxLength
                foreach (var item in dg.Items)
                {
                    lstValue = new List<string>();

                    for (int i = 0; i < lstColumnSortByDisplayIndex.Count; i++)
                    {
                        lstValue.Add(string.Empty);
                        if (lstColumnSortByDisplayIndex[i].Visibility == Visibility.Visible)
                        {
                            object objValue = null;
                            Type type = item.GetType();

                            //列可能是使用Binding，也可能使用MultiBinding；
                            //使用Binding时可能会使用converter，也可能不会。
                            if (lstColumnSortByDisplayIndex[i] is DataGridTextColumn && (lstColumnSortByDisplayIndex[i] as DataGridTextColumn).Binding is Binding)
                            {
                                //<DataGridTextColumn Header="委托价" Width="80" Binding="{Binding ConverterParameter=CommitPrice, Converter={StaticResource cfconverter}}"></DataGridTextColumn>
                                //<DataGridTextColumn Header="报单价格" Width="80" Binding="{Binding CommitPrice, StringFormat=F2}"></DataGridTextColumn>
                                //<DataGridTextColumn Header="涨跌幅 " Width="70" Binding="{Binding Path=PriceFluctuationFD, Converter={StaticResource fconverter}, ConverterParameter=P2}">
                                Binding binding = (lstColumnSortByDisplayIndex[i] as DataGridTextColumn).Binding as Binding;
                                System.Windows.Data.IValueConverter converter = binding.Converter;

                                //反射得到对应的数据值,反射的属性通常存在binding的path中，但使用CustomePriceFormatConverter时，需要存在ConverterParameter中
                                string propertyName = binding.Path.Path;

                                object converterValue = null;
                                if (string.IsNullOrEmpty(propertyName))
                                {
                                    propertyName = binding.ConverterParameter.ToString();
                                }
                                if (type.GetProperty(propertyName) != null
                                    && type.GetProperty(propertyName).GetValue(item, null) != null)
                                {
                                    converterValue = type.GetProperty(propertyName).GetValue(item, null);
                                }

                                if (converter != null)
                                {
                                    objValue = converter.Convert(converterValue, null, binding.ConverterParameter, null);
                                }
                                else
                                {
                                    objValue = converterValue;
                                }

                                string s = binding.StringFormat;
                                if (s != null)
                                {
                                    //有日期格式
                                    objValue = converterValue;
                                    double d = 0;
                                    if (double.TryParse(converterValue.ToString(), out d) == true)
                                    {
                                        objValue = d.ToString(s);
                                    }
                                }
                            }
                            else if (lstColumnSortByDisplayIndex[i] is DataGridTextColumn && (lstColumnSortByDisplayIndex[i] as DataGridTextColumn).Binding is MultiBinding)
                            {
                                //<DataGridTextColumn.Binding>
                                //    <MultiBinding Converter="{StaticResource mcfconverter}">
                                //        <Binding Path="Code"></Binding>
                                //        <Binding Path="INewPrice"></Binding>
                                //    </MultiBinding>
                                //</DataGridTextColumn.Binding>

                                MultiBinding binding = (lstColumnSortByDisplayIndex[i] as DataGridTextColumn).Binding as MultiBinding;
                                IMultiValueConverter converter = binding.Converter;
                                List<object> lstConverterValues = new List<object>();

                                foreach (var childBindingBase in binding.Bindings)
                                {
                                    Binding childBinding = childBindingBase as Binding;
                                    string propertyName = childBinding.Path.Path;
                                    lstConverterValues.Add(type.GetProperty(propertyName).GetValue(item, null));
                                }

                                if (converter != null)
                                {
                                    objValue = converter.Convert(lstConverterValues.ToArray(), null, binding.ConverterParameter, null);
                                }
                            }
                            if (objValue == null)
                            {
                                lstValue[i] = string.Empty;
                            }
                            else
                            {
                                lstValue[i] = objValue.ToString();
                                if (lstValue[i].Contains("\""))
                                {
                                    lstValue[i].Replace("\"", "\"\"");
                                }
                                if (lstValue[i].Contains(","))
                                {
                                    lstValue[i] = "\"" + lstValue[i] + "\"";
                                }
                            }

                            if (Encoding.Default.GetBytes(lstValue[i]).Length > lstMaxLength[i])
                            {
                                lstMaxLength[i] = Encoding.Default.GetBytes(lstValue[i]).Length;
                            }
                        }
                    }
                    exportedLines.Add(lstValue);
                }

                System.IO.StreamWriter fs = null;
                try
                {
                    fileName = sfd.FileName;
                    fs = new System.IO.StreamWriter(fileName, false, Encoding.Default);
                    //输出到文件
                    foreach (var item in exportedLines)
                    {
                        lstValue = item as List<string>;
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < lstColumnSortByDisplayIndex.Count; i++)
                        {
                            if (lstColumnSortByDisplayIndex[i].Visibility == Visibility.Visible)
                            {
                                int textLength = Encoding.Default.GetBytes(lstValue[i]).Length;
                                //每一列之后都要在最后填充满空格
                                sb.Append(lstValue[i]);
                                if (fileName.EndsWith("txt"))
                                {
                                    sb.Append("".PadRight(lstMaxLength[i] + DataGridExportedBlankPadNumber - textLength, ' '));
                                }
                                else if (fileName.EndsWith("csv"))
                                {
                                    sb.Append(',');
                                }
                            }
                        }
                        fs.WriteLine(sb.ToString());
                    }
                }
                catch (Exception ex)
                {
                    CommonUtil.ShowWarning(ex.Message+ "\n");
                    CommonUtil.ShowWarning("导出失败，可能该文件已经被打开，请关闭后重试。");
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                        System.Windows.Forms.MessageBox.Show("数据导出成功！", "系统提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    }
                }
                Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory.ToString();


            }
        }

        public static void ShowWarning(string messageText)
        {
            System.Windows.Forms.MessageBox.Show(messageText, "警告", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
        }

        /// <summary>
        /// 自动调整列宽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void AutoAdjustColumnWidth_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dg = GetDataGridFromMenuItemBySender(sender);
            foreach (var item in dg.Columns)
            {
                item.Width = DataGridLength.Auto;
            }
        }

        public static int GetIntValue(object o)
        {
            if (o == null)
            {
                return 0;
            }
            else
            {
                int result = 0;
                int.TryParse(o.ToString(), out result);
                return result;
            }
        }

        public static double GetDoubleValue(object o)
        {
            if (o == null)
            {
                return 0;
            }
            else
            {
                double result = 0;
                double.TryParse(o.ToString(), out result);
                return result;
            }
        }

        /// <summary>
        /// 写确认单文件
        /// </summary>
        /// <param name="userName"></param>
        public static void WriteBackConfirmInfos(string userName)
        {
            List<ConfirmInfo> confirmInfos = CommonUtil.GetConfirmInfos();
            Boolean bFound = false;
            foreach (ConfirmInfo info in confirmInfos)
            {
                if (info.ConfirmUserName == userName)
                {
                    bFound = true;
                    info.ConfirmDate = DateTime.Now.ToString("yyyyMMdd");
                }
            }
            if (bFound == false)
            {
                confirmInfos.Add(new ConfirmInfo(userName, DateTime.Now.ToString("yyyyMMdd")));
            }

            String appStartupPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string curDateFile = appStartupPath + "\\setting\\curDate.txt";
            FileStream fs = new FileStream(curDateFile, FileMode.Create);

            string content = "";
            foreach (ConfirmInfo info in confirmInfos)
            {
                content += info.ConfirmUserName + "#" + info.ConfirmDate + "\n";
            }

            byte[] bs = ASCIIEncoding.Default.GetBytes(content);
            fs.Write(bs, 0, bs.Length);
            fs.Close();
        }

        /// <summary>
        /// 获取确认单信息
        /// </summary>
        /// <returns></returns>
        public static List<ConfirmInfo> GetConfirmInfos()
        {
            //将当前日期存在setting目录下的curDate.txt中
            String appStartupPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string curDateFile = appStartupPath + ConfirmFileFullPath;

            List<ConfirmInfo> confirmInfos = new List<ConfirmInfo>();

            if (File.Exists(curDateFile) == true)
            {
                FileStream fs = null;
                StreamReader sr = null;
                try
                {
                    fs = new FileStream(curDateFile, FileMode.Open);
                    sr = new StreamReader(fs);
                    string date = sr.ReadLine();
                    while (date != null)
                    {
                        string[] fields = date.Split('#');
                        if (fields.Length == 2)
                        {
                            confirmInfos.Add(new ConfirmInfo(fields[0], fields[1]));
                        }
                        date = sr.ReadLine();
                    }
                }
                catch (Exception e)
                {
                    Util.Log(e.Message);
                }
                finally
                {
                    if (sr != null)
                    {
                        sr.Close();
                    }
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
            return confirmInfos;
        }

        /// <summary>
        /// 保存结算单
        /// </summary>
        /// <param name="content"></param>
        public static void SaveStatementOrder(string content, string date = "")
        {
            string fileName;
            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.Filter = "文本文件(*.txt)|*.txt|Word文件(*.doc)|*.doc|Excel 文件(*.xls)|*.xls|所有文件(*.*)|*.*";
            if (date == "")
            {
                date = DateTime.Now.ToString("yyyyMMdd");
            }
            sfd.FileName = "结算单_" + date;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamWriter fs = null;
                System.IO.FileStream fss = null;
                try
                {
                    fileName = sfd.FileName;
                    fss = new FileStream(fileName, FileMode.Create);
                    fs = new System.IO.StreamWriter(fss, Encoding.Default);
                    //恒生后台返回的结算单用\n换行，保存为文件时格式有错误，需要替换成\r\n，对应缺陷000784
                    if (content.IndexOf("\r\n") < 0)
                    {
                        content = content.Replace("\n", "\r\n");
                    }
                    fs.WriteLine(content);
                }
                catch (Exception ex)
                {
                    CommonUtil.ShowWarning(ex.Message + "\n");
                    CommonUtil.ShowWarning("保存失败，可能该文件已经被打开，请关闭后重试。");
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                    if (fss != null)
                    {
                        fs.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 是否需要确认单
        /// </summary>
        /// <returns></returns>
        public static bool NeedAffirm(string userName)
        {
            bool needAffirm = true;

            List<ConfirmInfo> confirmInfos = GetConfirmInfos();

            foreach (ConfirmInfo info in confirmInfos)
            {
                if (info.ConfirmUserName == userName)
                {
                    if (info.ConfirmDate == DateTime.Now.ToString(ConfirmDateFormat))
                    {
                        needAffirm = false;
                    }
                }
            }
            return needAffirm;
        }

        /// <summary>
        /// 打开保证金监控中心连接
        /// </summary>
        public static void OpenCFMMC()
        {
            try
            {
                System.Diagnostics.Process.Start("https://investorservice.cfmmc.com/");
            }
            catch (Exception ex)
            {
                ShowWarning(ex.Message);
            }
        }

        public static DateTime GetLastWorkDayBeforeToday()
        {
            DateTime lastWorkDay = DateTime.Now.Date;
            lastWorkDay = lastWorkDay.AddDays(-1);
            while (lastWorkDay.DayOfWeek == DayOfWeek.Saturday || lastWorkDay.DayOfWeek == DayOfWeek.Sunday)
            {
                lastWorkDay = lastWorkDay.AddDays(-1);
            }
            return lastWorkDay;
        }

        public static DataGridCell GetClickedCell(MouseButtonEventArgs e)
        {
            try
            {
                DataGridCell cell = null; //Datalist15.SelectedCells[0];

                if (e.OriginalSource is TextBlock)
                {
                    if ((e.OriginalSource as TextBlock).Parent is DataGridCell)
                    {
                        cell = (e.OriginalSource as TextBlock).Parent as DataGridCell;
                    }
                }
                else if (e.OriginalSource is Grid && (e.OriginalSource as Grid).Children[0] is ContentPresenter)
                {
                    cell = (((e.OriginalSource as Grid).Children[0] as ContentPresenter).Content as TextBlock).Parent as DataGridCell;
                }
                else if (e.OriginalSource is Grid && (e.OriginalSource as Grid).Children[0] is System.Windows.Shapes.Rectangle)
                {
                    System.Windows.Shapes.Rectangle rect = (e.OriginalSource as Grid).Children[0] as System.Windows.Shapes.Rectangle;
                    if (rect != null)
                    {
                        if (rect.Parent is Grid)
                        {
                            Grid g = rect.Parent as Grid;
                            if (g != null)
                            {
                                DependencyObject do1 = g.Parent;
                                DependencyObject do2 = g.TemplatedParent;
                                if (do2 is DataGridCell)
                                {
                                    cell = do2 as DataGridCell;
                                }
                            }
                        }
                        //Util.Log("rect.parent=" + rect.Parent.ToString());
                    }
                }
                else if (e.OriginalSource is Border && (e.OriginalSource as Border).Child is ContentPresenter
                    && ((e.OriginalSource as Border).Child as ContentPresenter).Content == null)
                {
                    cell = null;
                }
                else if (e.OriginalSource is Border && (e.OriginalSource as Border).Child is ContentPresenter)
                {
                    if (((e.OriginalSource as Border).Child as ContentPresenter).Content as TextBlock != null)
                    {
                        cell = (((e.OriginalSource as Border).Child as ContentPresenter).Content as TextBlock).Parent as DataGridCell;
                    }
                }
                else if (e.OriginalSource is System.Windows.Shapes.Rectangle)
                {
                    System.Windows.Shapes.Rectangle rect = e.OriginalSource as System.Windows.Shapes.Rectangle;
                    if (rect != null)
                    {
                        if (rect.Parent is Grid)
                        {
                            Grid g = rect.Parent as Grid;
                            if (g != null)
                            {
                                DependencyObject do1 = g.Parent;
                                DependencyObject do2 = g.TemplatedParent;
                                if (do2 is DataGridCell)
                                {
                                    cell = do2 as DataGridCell;
                                }
                            }
                        }
                        //Util.Log("rect.parent=" + rect.Parent.ToString());
                    }

                }
                return cell;
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
                return null;
            }
        }

        public static string PriceFormatString(double fluct)
        {
            string formatString = String.Empty;
            if (fluct < 0.01)
            {
                formatString = "F3";
            }
            else if (fluct < 0.1)
            {
                formatString = "F2";
            }
            else if (fluct < 1)
            {
                formatString = "F1";
            }
            else
            {
                formatString = "F0";
            }
            return formatString;
        }

        public static bool IsValidCode(string strCode)
        {
            List<string> strCodeList = new List<string>();
            foreach(string tempKey in CodeSetManager.ContractMap.Keys)
            {
                strCodeList.Add(CodeSetManager.ContractMap[tempKey].Code);
            }
            return strCodeList.Contains(strCode);
        }

        /// <summary>
        /// 根据最小变动单位返回价格格式
        /// 根据最小变动单位确定小数点位数，目前只有Au的fluct < 0.1；IF的fluct < 1；其他的都大于等于1
        /// 
        /// </summary>
        /// <param name="fluct"></param>
        /// <returns></returns>
        public static string GetPriceFormat(string code)
        {
            double hycs = 0;
            decimal fluct = 0;
            CodeSetManager.GetHycsAndFluct(code, out hycs, out fluct);
            if ((double)fluct < 0.01)
            {
                return "F3";
            }
            else if ((double)fluct < 0.1)
            {
                return "F2";
            }
            else if (fluct < 1)
            {
                return "F1";
            }
            else
            {
                return "F0";
            }
        }

        public static Window GetParentWindow(UIElement uiElement)
        {
            object element = uiElement;
            while (element != null)
            {
                if (element is Window)
                {
                    return element as Window;
                }
                element = CommonUtil.GetOrderProperty(element, "Parent");
            }
            return null;
        }

        public static void SaveObjectToFile(object needSavedObject, string filePath)
        {
            XmlSerializer ser = new XmlSerializer(needSavedObject.GetType());
            TextWriter writer = new StreamWriter(filePath);
            ser.Serialize(writer, needSavedObject);
            writer.Close();
        }

        public static object RecoverObjectFromFile(Type objectTypet, string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    XmlSerializer ser = new XmlSerializer(objectTypet);
                    System.IO.FileStream fs = new System.IO.FileStream(filePath, FileMode.Open);
                    System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(fs);
                    object recoveredObject = ser.Deserialize(reader);
                    reader.Close();
                    fs.Close();

                    return recoveredObject;
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
            return null;
        }

        public static OrderStatus GetOrderStatus(string status)
        {
            OrderStatus orderStatus;
            switch (status)
            {
                case "未知":
                    orderStatus = OrderStatus.Unknown;
                    break;
                case "正报":
                case "已排队":
                //case "已报":                
                    orderStatus = OrderStatus.Submitted;
                    break;
                case "全部成交":
                    orderStatus = OrderStatus.Chengjiao;
                    break;
                case "已报":   
                case "未成交":
                    orderStatus = OrderStatus.Queued;
                    break;
                case "未触发":
                    orderStatus = OrderStatus.NotTouched;
                    break;
                case "已撤单":
                    orderStatus = OrderStatus.Cancelled;
                    break;
                case "部分成交":
                    orderStatus = OrderStatus.PartChengjiao;
                    break;
                case "已拒绝":
                    orderStatus = OrderStatus.Failed;
                    break;
                default:
                    orderStatus = OrderStatus.Unknown;
                    break;

            }
            return orderStatus;
        }

        public static EnumThostOrderPriceTypeType GetPriceType(string type)
        {
            EnumThostOrderPriceTypeType priceType = EnumThostOrderPriceTypeType.LimitPrice;
            if (type == null || type.Trim() == String.Empty)
            {
                return priceType;
            }
            if (type.Contains("最新价+1点"))
                priceType = EnumThostOrderPriceTypeType.LastPricePlusOneTicks;
            else if (type.Contains("最新价+2点"))
                priceType = EnumThostOrderPriceTypeType.LastPricePlusTwoTicks;
            else if (type.Contains("最新价+3点"))
                priceType = EnumThostOrderPriceTypeType.LastPricePlusThreeTicks;
            else if (type.Contains("买一价+1点"))
                priceType = EnumThostOrderPriceTypeType.BidPrice1PlusOneTicks;
            else if (type.Contains("买一价+2点"))
                priceType = EnumThostOrderPriceTypeType.BidPrice1PlusTwoTicks;
            else if (type.Contains("买一价+3点"))
                priceType = EnumThostOrderPriceTypeType.BidPrice1PlusThreeTicks;
            else if (type.Contains("卖一价+1点"))
                priceType = EnumThostOrderPriceTypeType.AskPrice1PlusOneTicks;
            else if (type.Contains("卖一价+2点"))
                priceType = EnumThostOrderPriceTypeType.AskPrice1PlusTwoTicks;
            else if (type.Contains("卖一价+3点"))
                priceType = EnumThostOrderPriceTypeType.AskPrice1PlusThreeTicks;
            return priceType;
        }

        public static EnumOrderType GetOrderType(string type)
        {
            EnumOrderType orderType = EnumOrderType.Limit;
            if (type == null || type.Trim() == String.Empty)
            {
                return orderType;
            }
            if (type.Contains("自动单") || type.Contains("竞价"))
            {
                orderType = EnumOrderType.Opening;
            }
            else if (type.Contains("市价"))
            {
                orderType = EnumOrderType.Market;
            }
            else if (type.Contains("FAK") || type.Contains("IOC"))
            {
                orderType = EnumOrderType.FAK;
            }
            else if (type.Contains("FOK"))
            {
                orderType = EnumOrderType.FOK;
            }
            else if (type.Contains("SAL") || type.Contains("StopLimit"))
            {
                orderType = EnumOrderType.StopLimit;
            }
            else if (type.Contains("MIT"))
            {
                orderType = EnumOrderType.MIT;
            }
            return orderType;
        }

        public static string GetOrderTypeString(EnumOrderType orderType)
        {
            string strType = "限价";
            if (orderType == EnumOrderType.Opening)
            {
                strType = "竞价";
            }
            if (orderType == EnumOrderType.Market)
            {
                strType = "市价";
            }
            if (orderType == EnumOrderType.FAK)
            {
                strType = "FAK";
            }
            if (orderType == EnumOrderType.FOK)
            {
                strType = "FOK";
            }
            if (orderType == EnumOrderType.StopLimit)
            {
                strType = "StopLimit";
            }
            if (orderType == EnumOrderType.MIT)
            {
                strType = "MIT";
            }
            return strType;
        }

        public static EnumHedgeType GetHedgeType(string hedge)
        {
            EnumHedgeType hedgeType = EnumHedgeType.Speculation;
            if (hedge.Contains("套保"))
            {
                hedgeType = EnumHedgeType.Hedge;
            }
            else if (hedge.Contains("套利"))
            {
                hedgeType = EnumHedgeType.Arbitrage;
            }
            return hedgeType;
        }

        public static string GetHedgeString(EnumHedgeType hedgeType)
        {
            string hedgeStr = String.Empty;
            if (hedgeType == EnumHedgeType.Arbitrage)
            {
                hedgeStr = "套利";
            }
            else if (hedgeType == EnumHedgeType.Hedge)
            {
                hedgeStr = "套保";
            }
            else if (hedgeType == EnumHedgeType.Speculation)
            {
                hedgeStr = "投机";
            }
            return hedgeStr;
        }

        public static List<int> BreakLargeOrderHandCount(int handCount)
        {
            List<int> numLst = new List<int>();
            try
            {
                int splitKey = DateTime.Now.Second % 10;
                if (splitKey == 0)
                {
                    splitKey = 10;
                }
                int division = handCount / splitKey + handCount % splitKey;
                int maxHandCount = (int)Math.Ceiling((double)handCount / (double)division);

                int remainingCount = handCount;
                while (remainingCount > 0)
                {
                    Random rd = new Random(remainingCount);
                    int tempHand = rd.Next(Math.Max(1, maxHandCount / 2), maxHandCount);
                    if (remainingCount >= tempHand)
                    {
                        numLst.Add(tempHand);
                        remainingCount -= tempHand;
                    }
                    else
                    {
                        numLst.Add(remainingCount);
                        remainingCount = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return numLst;
        }
    }

    /// <summary>
    /// 结算单是否确认的信息
    /// </summary>
    public class ConfirmInfo
    {
        public string ConfirmUserName;
        public string ConfirmDate;

        public ConfirmInfo(string confirmName, string aConfirmDate)
        {
            ConfirmUserName = confirmName;
            ConfirmDate = aConfirmDate;
        }
    }
    
}