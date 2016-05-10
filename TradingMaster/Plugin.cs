using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using CTPMaster.Control;
using System.ComponentModel;

namespace CTPMaster
{
    /// <summary>
    /// 报价表的插件
    /// </summary>
    public class Plugin : BasePlugin
    {
        protected MainWindow singleInstance;

        protected Login tradeLogon = null;
        public Window tradeLogonWindow = null;

        public Plugin()
        {
            singleInstance = null;
        }

        public override PluginUC GetPluginUserControl()
        {
            //TODO: Whether to show the trade window when closing the connecting logon window.
            //if (Login.IsTerminated)
            //{
            //    Login.IsTerminated = false;
            //    return null;
            //}
            //else
            if (tradeLogon == null)
            {
                tradeLogon = new Login();
            }
            tradeLogon.ClearToInit();
            if (!JYDataServer.getServerInstance().JyServerLogOn)
            {
                JYDataServer.getServerInstance().Plugin = this;
                if (tradeLogonWindow == null)
                {
                    tradeLogonWindow = CommonUtil.GetWindow(tradeLogon.TiTle, tradeLogon, plc.API.GetMainWindow());
                    tradeLogonWindow.Closing += new System.ComponentModel.CancelEventHandler(tradeLogonWindow_Closing);
                }
                tradeLogonWindow.Show();
                return null;
            }
            else
            {
                if (singleInstance == null)
                {
                    singleInstance = new MainWindow();
                }
                return singleInstance;
            }
        }

        private void tradeLogonWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            (sender as Window).Hide();
        }

        /// <summary>
        /// 通知所有的插件，某个事件
        /// </summary>
        /// <param name="typeVal">类型代码</param>
        /// <param name="paramArray">参数</param>
        public override void Notify(int typeVal, object[] paramArray)
        {
            try
            {
                if (typeVal == 1)
                {
                    if (Application.Current != null)
                    {
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            if (singleInstance != null)
                            {
                                //singleInstance.SwitchToSubMarket(singleInstance.defaultButton.Tag as SubMarket);
                            }
                        });
                    }
                }
                else
                {
                    Util.Log("User logged off. Quote requesst is cancelled.");
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception:" + ex.Message);
                Util.Log(ex.StackTrace);
            }
        }

    }

}
