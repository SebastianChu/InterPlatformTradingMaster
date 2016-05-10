using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TradingMaster
{
    public class PasswordBroker
    {
        public static void Broker(string pwd)
        {
            if (UInt64.Parse(pwd) < 10000000000)
            {
                string password = (UInt64.Parse(pwd) + 1).ToString();
                Login logWin = TradeDataClient.GetClientInstance().getLoginWindow();
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        //logWin.tb_userName.Text = account;
                        logWin.pb_passWord.Password = password;
                        logWin.tb_authcode.Text = logWin.GetAuthCode();
                        //JYDataServer.getServerInstance().InitAPIforCTP(logWin.tb_userName.Text, password);
                        Thread.Sleep(1000);
                    });
                }
            }
        }
    }
}
