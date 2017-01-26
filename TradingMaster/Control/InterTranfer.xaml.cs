using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace TradingMaster.Control
{
    public delegate void FuturesCapitalQueryClickDelegate(object sender, RoutedEventArgs e);

    /// <summary>
    /// InterTranfer.xaml 的交互逻辑
    /// </summary>
    public partial class InterTranfer : UserControl
    {
        public InterTranfer()
        {
            InitializeComponent();
        }

        public void Init(MainWindow parent)
        {
            this._MainWindow = parent;
            _UserAccount = CtpDataServer.GetUserInstance().InvestorID;
            tb_User.IsEnabled = true;
            tb_User.Text = _UserAccount;
            tb_User.IsEnabled = false;
        }

        public void InitBinding(List<string> bankList)
        {
            this.cb_Banks.ItemsSource = bankList;
        }

        private Window _TranRecWindow = null;

        private MainWindow _MainWindow { get; set; }
        public event FuturesCapitalQueryClickDelegate FuturesCapitalQueryClick;

        private string _BankID;
        private string _BankBrchID;
        private string _Currency;
        private string _UserAccount;

        private void cb_Banks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _BankID = BankManager.GetBankIdFromName(cb_Banks.SelectedItem as string);
            if (string.IsNullOrEmpty(_BankID))
            {
                //JYDataServer.getServerInstance().AddToQryQueue(new CTPRequestContent("QryTransferSerial", new List<object>() { BankID }));
            }
            else
            {
                Util.Log("Warning! Invalid Bank ID.");
                return;
            }

            _BankBrchID = BankManager.GetBankBranchIdFromName(cb_Banks.SelectedItem as string);
            if (string.IsNullOrEmpty(_BankBrchID))
            {
                Util.Log("Warning! Invalid Bank ID.");
                return;
            }
        }

        private void cb_Currency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(cb_Currency.SelectedValue.ToString().Trim()))
            {
                _Currency = cb_Currency.SelectedValue.ToString();
            }
        }

        private void btnQryBank_Click(object sender, RoutedEventArgs e)
        {
            string capitalPwd = pb_CapPwd.Password;
            string bankPwd = pb_BankPwd.Password;

            if (string.IsNullOrEmpty(_Currency))
            {
                if (cb_Currency.SelectedValue != null && !String.IsNullOrEmpty(cb_Currency.SelectedValue.ToString().Trim()))
                {
                    _Currency = cb_Currency.SelectedValue.ToString();
                }
                else
                {
                    Util.Log("Warning! Invalid Currency Type: " + cb_Currency.SelectedValue);
                }
            }
            TradeDataClient.GetClientInstance().RequestTradeData(_UserAccount, BACKENDTYPE.CTP, new RequestContent("QryBankAccount", new List<object>() { _BankID, _BankBrchID, capitalPwd, bankPwd, _Currency }));
        }

        private void btnQryFut_Click(object sender, RoutedEventArgs e)
        {
            if (FuturesCapitalQueryClick != null)
            {
                FuturesCapitalQueryClick(sender, e);
            }
        }

        private void btnToBank_Click(object sender, RoutedEventArgs e)
        {
            string capitalPwd = pb_CapPwd.Password;
            string bankPwd = pb_BankPwd.Password;
            double tranAmt = 0.0;
            bool isNum = double.TryParse(tb_Amount.Text.Trim(), out tranAmt);

            if (isNum)
            {
                TradeDataClient.GetClientInstance().RequestTradeData(_UserAccount, BACKENDTYPE.CTP, new RequestContent("TransferFromFutureToBankByFuture", new List<object>() { _BankID, _BankBrchID, capitalPwd, bankPwd, tranAmt, _Currency }));
            }
            else
            {
                Util.Log("Warning! Trading Amount is illegal!");
                MessageBox.Show("非法金额输入，请重新输入！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                tb_Amount.Text = "";
            }
        }


        private void btnToFut_Click(object sender, RoutedEventArgs e)
        {
            string capitalPwd = pb_CapPwd.Password;
            string bankPwd = pb_BankPwd.Password;
            double tranAmt = 0.0;
            bool isNum = double.TryParse(tb_Amount.Text.Trim(), out tranAmt);

            if (isNum)
            {
                TradeDataClient.GetClientInstance().RequestTradeData(_UserAccount, BACKENDTYPE.CTP, new RequestContent("TransferFromBankToFutureByFuture", new List<object>() { _BankID, _BankBrchID, capitalPwd, bankPwd, tranAmt, _Currency }));
            }
            else
            {
                Util.Log("Warning! Trading Amount is illegal!");
                MessageBox.Show("非法金额输入，请重新输入！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                tb_Amount.Text = "";
            }
        }

        private void btnQryTran_Click(object sender, RoutedEventArgs e)
        {
            if (_BankID != null && _BankID != "")
            {
                TradeDataClient.GetClientInstance().RequestTradeData(_UserAccount, BACKENDTYPE.CTP, new RequestContent("QryTransferSerial", new List<object>() { _BankID }));
            }

            if (_TranRecWindow == null)
            {
                TransferRecord tranRecControl = new TransferRecord();
                tranRecControl.Init(_MainWindow);
                _TranRecWindow = CommonUtil.GetWindow("银期转账记录", tranRecControl, this._MainWindow);
                _TranRecWindow.Closing += new CancelEventHandler(TranRecWindow_Closing);
                _TranRecWindow.ResizeMode = ResizeMode.NoResize;
                _TranRecWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            _TranRecWindow.Show();
        }

        void TranRecWindow_Closing(object sender, CancelEventArgs e)
        {
            _TranRecWindow.Visibility = Visibility.Collapsed;
            e.Cancel = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Window)
            {
                (this.Parent as Window).Close();
            }
        }

    }
}
