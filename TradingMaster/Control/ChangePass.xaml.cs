using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TradingMaster.JYData;

namespace TradingMaster.Control
{
    /// <summary>
    /// ChangePass.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePass : UserControl
    {
        public ChangePass()
        {
            InitializeComponent();
            tbUserAcct.IsEnabled = true;
            _UserAccount = DataContainer.GetUserInstance().InvestorID;
            tbUserAcct.Text = _UserAccount;
            tbUserAcct.IsEnabled = false;
        }

        private string _UserAccount;

        public void Reset()
        {
            pbOldPass.Password = pbNewPass.Password = pbNewPassConfirm.Password = string.Empty;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            string errMsg = VerifyPassword();
            if (errMsg != null)
            {
                CommonUtil.ShowWarning(errMsg);
                return;
            }

            //修改密码
            List<object> args = new List<object>();
            args.Add(pbNewPass.Password);
            args.Add(pbOldPass.Password);
            TradeDataClient.GetClientInstance().RequestTradeData(_UserAccount, BACKENDTYPE.CTP, new RequestContent("ChangeUserPassword", args));
            //JYDataServer.getServerInstance().ChangeUserPassword(pbNewPass.Password, pbOldPass.Password);            

            //发出指令。
            CloseParentWindow();
        }

        private string VerifyPassword()
        {
            string errMsg = null;
            if (pbNewPass.Password == string.Empty)
            {
                errMsg = "新密码不能为空";
            }
            else if (pbNewPass.Password != pbNewPassConfirm.Password)
            {
                errMsg = "您两次输入的新密码不一致，请重新输入";
            }
            //else if (JYDataServer.getServerInstance().getLoginWindow().PassWord.Password != pbOldPass.Password)
            //{
            //    errMsg = "综合交易平台：原口令不匹配";
            //}
            return errMsg;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseParentWindow();
        }

        private void CloseParentWindow()
        {
            Window parentWindow = CommonUtil.GetParentWindow(this);
            if (parentWindow != null)
            {
                parentWindow.Close();
            }
        }
    }
}
