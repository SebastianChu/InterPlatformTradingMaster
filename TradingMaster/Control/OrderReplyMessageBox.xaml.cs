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
using TradingMaster.Properties;

namespace TradingMaster.Control
{
    /// <summary>
    /// OrderReplayMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class OrderReplyMessageBox : UserControl
    {
        private List<object> _LstOrders = new List<object>();

        public OrderReplyMessageBox()
        {
            InitializeComponent();
            btnOk.Focus();
        }

        public void Init(MainWindow parent)
        {
            this.DataContext = parent;
            this._MainWindow = parent;
        }

        private MainWindow _MainWindow { get; set; }


        public bool AddOrderInfo(Q7JYOrderData orderInfo)
        {
            bool canAdd = false;
            if (CommonUtil.GetOrderStatus(orderInfo.OrderStatus) == OrderStatus.Queued)
            {
                if (Settings.Default.ConfirmForOrderSubmit == true)
                {
                    canAdd = true;
                }
            }
            else if (CommonUtil.GetOrderStatus(orderInfo.OrderStatus) == OrderStatus.Chengjiao)
            {
                if (Settings.Default.ConfirmForOrderTransact == true)
                {
                    canAdd = true;
                }
            }
            else if (CommonUtil.GetOrderStatus(orderInfo.OrderStatus) == OrderStatus.Cancelled)
            {
                if (Settings.Default.ConfirmForOrderCancel == true)
                {
                    canAdd = true;
                }
            }
            else if (CommonUtil.GetOrderStatus(orderInfo.OrderStatus) == OrderStatus.Failed)
            {
                if (Settings.Default.ConfirmForOrderCancel == true)
                {
                    canAdd = true;
                }
            }

            if (canAdd)
            {
                _LstOrders.Add(orderInfo);
                btnNext.Visibility = Visibility.Visible;
                string title = (this.Parent as Window).Title;
                if (title.Length > 4)
                {
                    title = title.Substring(title.Length - 4);
                    title = "TradingMaster：" + title;
                    (this.Parent as Window).Title = string.Format("({0}/{1}) {2}", index + 1, _LstOrders.Count, title);
                }
            }
            AddSystemMessage(orderInfo);
            return canAdd;
        }

        private void AddSystemMessage(Q7JYOrderData orderInfo)
        {
            string title = string.Empty;
            string code = orderInfo.Code;//orderInfo.legs[0] != null ? orderInfo.legs[0].Code ： "";
            string buySell = orderInfo.BuySell;
            string openClose = orderInfo.OpenClose;
            string message = string.Empty;
            if (CommonUtil.GetOrderStatus(orderInfo.OrderStatus) == OrderStatus.Queued)
            {
                message = string.Format("报单号：{0} 下单成功：{1},{2},{3},{4}手，委托价{5}, {6}",
                    orderInfo.OrderID.Trim(), code, buySell, openClose, orderInfo.CommitHandCount, orderInfo.CommitPrice, orderInfo.Hedge);
            }
            else if (CommonUtil.GetOrderStatus(orderInfo.OrderStatus) == OrderStatus.Chengjiao)
            {
                message = string.Format("报单号：{0} 成交通知：{1},{2},{3},{4}手，委托价{5}, {6}",
                    orderInfo.OrderID.Trim(), code, buySell, openClose, orderInfo.CommitHandCount, orderInfo.CommitPrice, orderInfo.Hedge);//成交价
            }
            else if (CommonUtil.GetOrderStatus(orderInfo.OrderStatus) == OrderStatus.Cancelled)
            {
                message = string.Format("撤单通知：{0},{1},{2},{3}手，委托价{4}, {5}",
                    code, buySell, openClose, orderInfo.CommitHandCount, orderInfo.CommitPrice, orderInfo.Hedge);
            }
            else if (CommonUtil.GetOrderStatus(orderInfo.OrderStatus) == OrderStatus.Failed)
            {
                message = string.Format("下单失败：{0},{1},{2},{3}手，委托价{4}, {5}",
                    code, buySell, openClose, orderInfo.CommitHandCount, orderInfo.CommitPrice, orderInfo.Hedge);
            }
            if (!string.IsNullOrEmpty(message))
            {
                _MainWindow.AddSystemMessage(DateTime.Now, message, "信息", "ORDER");
            }
        }

        public bool AddQuoteOrderInfo(QuoteOrderData orderInfo)
        {
            bool canAdd = false;
            if (CommonUtil.GetOrderStatus(orderInfo.QuoteStatus) == OrderStatus.Queued)
            {
                if (Settings.Default.ConfirmForOrderSubmit == true)
                {
                    canAdd = true;
                }
            }
            else if (CommonUtil.GetOrderStatus(orderInfo.QuoteStatus) == OrderStatus.Chengjiao)
            {
                if (Settings.Default.ConfirmForOrderTransact == true)
                {
                    canAdd = true;
                }
            }
            else if (CommonUtil.GetOrderStatus(orderInfo.QuoteStatus) == OrderStatus.Cancelled)
            {
                if (Settings.Default.ConfirmForOrderCancel == true)
                {
                    canAdd = true;
                }
            }
            else if (CommonUtil.GetOrderStatus(orderInfo.QuoteStatus) == OrderStatus.Failed)
            {
                if (Settings.Default.ConfirmForOrderCancel == true)
                {
                    canAdd = true;
                }
            }

            if (canAdd)
            {
                _LstOrders.Add(orderInfo);
                btnNext.Visibility = Visibility.Visible;
                string title = (this.Parent as Window).Title;
                if (title.Length > 4)
                {
                    title = title.Substring(title.Length - 4);
                    title = "TradingMaster：" + title;
                    (this.Parent as Window).Title = string.Format("({0}/{1}) {2}", index + 1, _LstOrders.Count, title);
                }
            }
            AddSystemMessage(orderInfo);
            return canAdd;
        }

        private void AddSystemMessage(QuoteOrderData orderInfo)
        {
            string title = string.Empty;
            string code = orderInfo.Code;//orderInfo.legs[0] != null ? orderInfo.legs[0].Code ： "";
            string bidOpenClose = orderInfo.BidOpenClose;
            string askOpenClose = orderInfo.AskOpenClose;
            string message = string.Empty;
            if (CommonUtil.GetOrderStatus(orderInfo.QuoteStatus) == OrderStatus.Queued)
            {
                message = string.Format("报价号：{0} 下单成功：{1}：买 {2},{3}手，买价{4}；卖 {5},{6}手，卖价{7}；应价编号：{8}",
                   orderInfo.QuoteOrderID.Trim(), code, 
                   bidOpenClose, orderInfo.BidHandCount, orderInfo.BidPrice, askOpenClose, orderInfo.AskHandCount, orderInfo.AskPrice, orderInfo.ForQuoteOrderID);
            }
            else if (CommonUtil.GetOrderStatus(orderInfo.QuoteStatus) == OrderStatus.Chengjiao)
            {
                message = string.Format("报价号：{0} 成交通知：{1}：买 {2},{3}手，买价{4}；卖 {5},{6}手，卖价{7}；应价编号：{8}",
                   orderInfo.QuoteOrderID.Trim(), code,
                   bidOpenClose, orderInfo.BidHandCount, orderInfo.BidPrice, askOpenClose, orderInfo.AskHandCount, orderInfo.AskPrice, orderInfo.ForQuoteOrderID);//成交价
            }
            else if (CommonUtil.GetOrderStatus(orderInfo.QuoteStatus) == OrderStatus.Cancelled)
            {
                message = string.Format("撤报价：{0},{1}：买 {2},{3}手，买价{4}；卖 {5},{6}手，卖价{7}；应价编号：{8}",
                   orderInfo.QuoteOrderID.Trim(), code,
                   bidOpenClose, orderInfo.BidHandCount, orderInfo.BidPrice, askOpenClose, orderInfo.AskHandCount, orderInfo.AskPrice, orderInfo.ForQuoteOrderID);
            }
            else if (CommonUtil.GetOrderStatus(orderInfo.QuoteStatus) == OrderStatus.Failed)
            {
                message = string.Format("报价失败：{0},{1}买 {2},{3}手，买价{4}；卖 {5},{6}手，卖价{7}；应价编号：{8}",
                   orderInfo.QuoteOrderID.Trim(), code,
                   bidOpenClose, orderInfo.BidHandCount, orderInfo.BidPrice, askOpenClose, orderInfo.AskHandCount, orderInfo.AskPrice, orderInfo.ForQuoteOrderID);
            }
            if (!string.IsNullOrEmpty(message))
            {
                _MainWindow.AddSystemMessage(DateTime.Now, message, "信息", "QUOTEORDER");
            }
        }

        public bool AddExecOrderInfo(ExecOrderData orderInfo)
        {
            bool canAdd = false;
            if (CommonUtil.GetOrderStatus(orderInfo.ExecStatus) == OrderStatus.Queued)
            {
                if (Settings.Default.ConfirmForOrderSubmit == true)
                {
                    canAdd = true;
                }
            }
            else if (CommonUtil.GetOrderStatus(orderInfo.ExecStatus) == OrderStatus.Chengjiao)
            {
                if (Settings.Default.ConfirmForOrderTransact == true)
                {
                    canAdd = true;
                }
            }
            else if (CommonUtil.GetOrderStatus(orderInfo.ExecStatus) == OrderStatus.Cancelled)
            {
                if (Settings.Default.ConfirmForOrderCancel == true)
                {
                    canAdd = true;
                }
            }
            else if (CommonUtil.GetOrderStatus(orderInfo.ExecStatus) == OrderStatus.Failed)
            {
                if (Settings.Default.ConfirmForOrderCancel == true)
                {
                    canAdd = true;
                }
            }

            if (canAdd)
            {
                _LstOrders.Add(orderInfo);
                btnNext.Visibility = Visibility.Visible;
                string title = (this.Parent as Window).Title;
                if (title.Length > 4)
                {
                    title = title.Substring(title.Length - 4);
                    title = "TradingMaster：" + title;
                    (this.Parent as Window).Title = string.Format("({0}/{1}) {2}", index + 1, _LstOrders.Count, title);
                }
            }
            AddSystemMessage(orderInfo);
            return canAdd;
        }

        private void AddSystemMessage(ExecOrderData orderInfo)
        {
            string title = string.Empty;
            string code = orderInfo.Code;//orderInfo.legs[0] != null ? orderInfo.legs[0].Code ： "";
            string openClose = orderInfo.OpenClose;
            string message = string.Empty;
            if (CommonUtil.IsExecCancellable(orderInfo))
            {
                message = string.Format("行权号：{0} 申请成功：{1},{2}手",
                   orderInfo.ExecOrderID.Trim(), code, orderInfo.HandCount);
            }
            else if (orderInfo.ExecStatus == "执行成功")
            {
                message = string.Format("行权号：{0} 执行成功：{1},{2}手",
                    orderInfo.ExecOrderID.Trim(), code, orderInfo.HandCount);//成交价
            }
            else if (orderInfo.ExecStatus == "已取消")
            {
                message = string.Format("取消申请：{0},{1}手",
                    code, orderInfo.HandCount);
            }
            else if (orderInfo.ExecStatus == "申请失败" || orderInfo.ExecStatus == "已拒绝")
            {
                message = string.Format("申请失败：{0},{1}手",
                    code, orderInfo.HandCount);
            }
            if (!string.IsNullOrEmpty(message))
            {
                _MainWindow.AddSystemMessage(DateTime.Now, message, "信息", "EXECORDER");
            }
        }

        public void Clear()
        {
            _LstOrders.Clear();
            index = -1;
            btnNext.Visibility = btnPrevious.Visibility = Visibility.Collapsed;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            bool needSaveSetting = false;
            if (chkConfirmCancel.IsChecked == true)
            {
                Settings.Default.ConfirmForOrderCancel = false;
                needSaveSetting = true;
            }
            if (chkConfirmSubmit.IsChecked == true)
            {
                Settings.Default.ConfirmForOrderSubmit = false;
                needSaveSetting = true;
            }
            if (chkConfirmTransact.IsChecked == true)
            {
                Settings.Default.ConfirmForOrderTransact = false;
                needSaveSetting = true;
            }
            try
            {
                if (needSaveSetting)
                {
                    Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {
                Util.Log(ex.ToString());
            }

            (this.Parent as Window).Close();
        }

        private int index = -1;

        public int Index
        {
            get { return index; }
            set
            {
                if (value < 0 || value > _LstOrders.Count - 1)
                {
                    return;
                }

                index = value;
                if (index == 0)
                {
                    btnPrevious.Visibility = Visibility.Collapsed;
                }
                else
                {
                    btnPrevious.Visibility = Visibility.Visible;
                }

                if (index == _LstOrders.Count - 1)
                {
                    btnNext.Visibility = Visibility.Collapsed;
                }
                else
                {
                    btnNext.Visibility = Visibility.Visible;
                }

                ShowOrderInfo();
            }
        }

        private void ShowOrderInfo()
        {
            HideCheckBox();
            string title = string.Empty;
            string message = "";
            if (_LstOrders[index] is Q7JYOrderData)
            {
                Q7JYOrderData orderInfo = _LstOrders[index] as Q7JYOrderData;
                string code = orderInfo.Code;//orderInfo.legs[0] != null ? orderInfo.legs[0].Code ： "";
                string buySell = orderInfo.BuySell;
                string openClose = orderInfo.OpenClose;// JYDataServer.getServerInstance().GetPosEffect(orderInfo.posEffect);

                message = string.Format("报单号 ：{0}", orderInfo.OrderID);
                message = message + string.Format("\n合约：{0}", code);
                message = message + string.Format("\n买卖：{0}", buySell);
                message = message + string.Format("\n开平：{0}", openClose);
                message = message + string.Format("\n委托量：{0}", orderInfo.CommitHandCount);
                message = message + string.Format("\n委托价：{0}", orderInfo.CommitPrice);
                message = message + string.Format("\n备注：{0}", orderInfo.FeedBackInfo);


                if (CommonUtil.GetOrderStatus(orderInfo.OrderStatus) == OrderStatus.Queued)
                {
                    title = "下单成功";
                    chkConfirmSubmit.Visibility = Visibility.Visible;
                }
                else if (CommonUtil.GetOrderStatus(orderInfo.OrderStatus) == OrderStatus.Chengjiao)
                {
                    title = "成交通知";
                    chkConfirmTransact.Visibility = Visibility.Visible;
                }
                else if (CommonUtil.GetOrderStatus(orderInfo.OrderStatus) == OrderStatus.Cancelled)
                {
                    title = "撤单成功";
                    chkConfirmCancel.Visibility = Visibility;
                }
                else if (CommonUtil.GetOrderStatus(orderInfo.OrderStatus) == OrderStatus.Failed)
                {
                    title = "下单失败";
                    chkConfirmCancel.Visibility = Visibility;
                }
            }
            else if (_LstOrders[index] is QuoteOrderData)
            {
                QuoteOrderData orderInfo = _LstOrders[index] as QuoteOrderData;
                string code = orderInfo.Code;//orderInfo.legs[0] != null ? orderInfo.legs[0].Code ： "";
                string bidOpenClose = orderInfo.BidOpenClose;
                string askOpenClose = orderInfo.AskOpenClose;
                // JYDataServer.getServerInstance().GetPosEffect(orderInfo.posEffect);
                
                message = string.Format("报价号 ：{0}", orderInfo.QuoteOrderID);
                message = message + string.Format("\n合约：{0}", code);
                message = message + string.Format("\n买开平：{0}", bidOpenClose);
                message = message + string.Format("\n买量：{0}", orderInfo.BidHandCount);
                message = message + string.Format("\n买价：{0}", orderInfo.BidPrice);
                message = message + string.Format("\n买套保：{0}", orderInfo.BidHedge);
                message = message + string.Format("\n卖开平：{0}", askOpenClose);
                message = message + string.Format("\n卖量：{0}", orderInfo.AskOpenClose);
                message = message + string.Format("\n卖价：{0}", orderInfo.AskPrice);
                message = message + string.Format("\n卖套保：{0}", orderInfo.AskHedge);
                message = message + string.Format("\n备注：{0}", orderInfo.StatusMsg);

                if (CommonUtil.GetOrderStatus(orderInfo.QuoteStatus) == OrderStatus.Queued)
                {
                    title = "报价成功";
                    chkConfirmSubmit.Visibility = Visibility.Visible;
                }
                else if (CommonUtil.GetOrderStatus(orderInfo.QuoteStatus) == OrderStatus.Chengjiao)
                {
                    title = "成交通知";
                    chkConfirmTransact.Visibility = Visibility.Visible;
                }
                else if (CommonUtil.GetOrderStatus(orderInfo.QuoteStatus) == OrderStatus.Cancelled)
                {
                    title = "撤单成功";
                    chkConfirmCancel.Visibility = Visibility;
                }
                else if (CommonUtil.GetOrderStatus(orderInfo.QuoteStatus) == OrderStatus.Failed)
                {
                    title = "报价失败";
                    chkConfirmCancel.Visibility = Visibility;
                }
            }
            else if (_LstOrders[index] is ExecOrderData)
            {
                ExecOrderData orderInfo = _LstOrders[index] as ExecOrderData;
                string code = orderInfo.Code;//orderInfo.legs[0] != null ? orderInfo.legs[0].Code ： "";
                string openClose = orderInfo.OpenClose;// JYDataServer.getServerInstance().GetPosEffect(orderInfo.posEffect);

                message = string.Format("执行号 ：{0}", orderInfo.ExecOrderID);
                message = message + string.Format("\n合约：{0}", code);
                message = message + string.Format("\n开平：{0}", openClose);
                message = message + string.Format("\n委托量：{0}", orderInfo.HandCount);
                message = message + string.Format("\n备注：{0}", orderInfo.StatusMsg);
                
                if (CommonUtil.GetOrderStatus(orderInfo.ExecStatus) == OrderStatus.Queued)
                {
                    title = "指令成功";
                    chkConfirmSubmit.Visibility = Visibility.Visible;
                }
                else if (CommonUtil.GetOrderStatus(orderInfo.ExecStatus) == OrderStatus.Chengjiao)
                {
                    title = "成交通知";
                    chkConfirmTransact.Visibility = Visibility.Visible;
                }
                else if (CommonUtil.GetOrderStatus(orderInfo.ExecStatus) == OrderStatus.Cancelled)
                {
                    title = "撤单成功";
                    chkConfirmCancel.Visibility = Visibility;
                }
                else if (CommonUtil.GetOrderStatus(orderInfo.ExecStatus) == OrderStatus.Failed)
                {
                    title = "指令失败";
                    chkConfirmCancel.Visibility = Visibility;
                }
            }
            else
            {
                title = "提示数据不合法！";
                message = "提示数据不合法！";
            }
            (this.Parent as Window).Title = string.Format("({0}/{1}) {2}", index + 1, _LstOrders.Count, "TradingMaster：" + title);
            tbMessage.Text = message;
        }

        private void HideCheckBox()
        {
            chkConfirmCancel.Visibility = chkConfirmSubmit.Visibility = chkConfirmTransact.Visibility = Visibility.Collapsed;
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            Index = index - 1;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Index = index + 1;
        }

    }
}
