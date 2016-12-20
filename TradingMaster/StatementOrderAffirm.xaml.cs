using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TradingMaster
{
    /// <summary>
    /// StatementOrderAffirm.xaml 的交互逻辑
    /// </summary>
    public partial class StatementOrderAffirm : Window
    {
        //  MainWindow mainWindow
        //public Search searchwindow = new Search();

        public StatementOrderAffirm()
        {
            InitializeComponent();
        }

        public void AddNewTabItem(string user, string settlementSheet)
        {
            foreach (TabItem item in tbcUserCfm.Items)
            {
                if (item.Header.ToString() == user)
                    return;
            }

            TabItem newTabItem = new TabItem();
            newTabItem.Margin = new Thickness(0);
            newTabItem.Header = user;
            newTabItem.Style = null;// Resources["TabItemTriggerStyle_mul"] as Style;

            TextBox tbUser = new TextBox();
            tbUser.Margin = new Thickness(0);
            tbUser.HorizontalAlignment = HorizontalAlignment.Stretch;
            tbUser.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            tbUser.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            tbUser.FontSize = 14.0;
            tbUser.Text = settlementSheet;
            tbUser.FontFamily = new FontFamily("新宋体");
            tbUser.IsReadOnly = true;//
            tbUser.AcceptsTab = false;
            newTabItem.Content = tbUser;

            tbcUserCfm.Items.Add(newTabItem);
            tbcUserCfm.SelectedIndex = 0;
        }

        private void tb_link_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore.exe", "https://investorservice.cfmmc.com/");
        }

        private void bt_affirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var userItem in (((sender as Button).Parent as Grid).Children[0] as TabControl).Items)
                {
                    if (userItem is TabItem && (userItem as TabItem).IsSelected)
                    {
                        string acct = (userItem as TabItem).Header.ToString();
                        if (acct != null && acct != String.Empty)
                        {
                            TradeDataClient.GetClientInstance().RequestTradeData(acct, BACKENDTYPE.CTP, new RequestContent("RequestSettlementInfoConfirm", new List<object>())); //RequestSettlementInfoConfirm();
                            //TradeDataClient.GetClientInstance().RefreshSystemData(); //InitDataFromAPI();
                            tbcUserCfm.Items.Remove(userItem as TabItem);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception:" + ex.Message);
                Util.Log("exception:" + ex.StackTrace);
            }

            //if (tbcUserCfm.SelectedIndex < tbcUserCfm.Items.Count - 1)
            //{
            //    tbcUserCfm.SelectedIndex++;
            //}

            if (tbcUserCfm.Items.Count == 0)
            {
                this.Hide();
            }
        }

        private void bt_deny_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
            try
            {
                foreach (var userItem in (((sender as Button).Parent as Grid).Children[0] as TabControl).Items)
                {
                    if (userItem is TabItem && (userItem as TabItem).IsSelected)
                    {
                        string acct = (userItem as TabItem).Header.ToString();
                        if (acct != null && acct != String.Empty)
                        {
                            TradeDataClient.GetClientInstance().RequestTradeData(acct, BACKENDTYPE.CTP, new RequestContent("RequestDisConnect", new List<object>())); //RequestDisConnect();
                            tbcUserCfm.Items.Remove(userItem as TabItem);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception:" + ex.Message);
                Util.Log("exception:" + ex.StackTrace);
            }

            //if (tbcUserCfm.SelectedIndex < tbcUserCfm.Items.Count - 1)
            //{
            //    tbcUserCfm.SelectedIndex++;
            //}

            if (tbcUserCfm.Items.Count == 0)
            {
                this.Hide();
            }
        }

        private void bt_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var userItem in (((sender as Button).Parent as Grid).Children[0] as TabControl).Items)
                {
                    if (userItem is TabItem && (userItem as TabItem).IsSelected == true)
                    {
                        string contentText = (userItem as TabItem).Content.ToString();
                        CommonUtil.SaveStatementOrder(contentText);
                        break;
                    }
                }
            }
            //CommonUtil.SaveStatementOrder(tb_content.Text);
            catch (Exception ex)
            {
                Util.Log("exception:" + ex.Message);
                Util.Log("exception:" + ex.StackTrace);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //Environment.Exit(0);

        }

        private void bt_affirmAll_Click(object sender, RoutedEventArgs e)
        {
            //JYDataServer.getServerInstance().AddToQryQueue(new CTPRequestContent("RequestSettlementInfoConfirm", new List<object>()));
            try
            {
                //foreach (UserLogonStruct userInfo in UserSettingsCollection)
                foreach (var userItem in (((sender as Button).Parent as Grid).Children[0] as TabControl).Items)
                {
                    if (userItem is TabItem)
                    {
                        //if (userInfo.LogStatus == "结算确认")
                        string acct = (userItem as TabItem).Header.ToString();
                        if (acct != null && acct != String.Empty)
                        {
                            TradeDataClient.GetClientInstance().RequestTradeData(acct, BACKENDTYPE.CTP, new RequestContent("RequestSettlementInfoConfirm", new List<object>())); //RequestSettlementInfoConfirm();
                        }
                        //TradeDataClient.GetClientInstance().RefreshSystemData(); //InitDataFromAPI();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception:" + ex.Message);
                Util.Log("exception:" + ex.StackTrace);
            }
            tbcUserCfm.Items.Clear();
            this.Hide();
        }

        private void StatementOrderAffirm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CloseStatementAffirmationWindow();
            e.Cancel = true;
        }

        private void bt_quit_Click(object sender, RoutedEventArgs e)
        {
            CloseStatementAffirmationWindow();
        }

        private void CloseStatementAffirmationWindow()
        {
            try
            {
                //foreach (UserLogonStruct userInfo in UserSettingsCollection)
                {
                    //if (userInfo.LogStatus.Contains("结算确认"))
                    {
                        TradeDataClient.GetClientInstance().RequestTradeData("", BACKENDTYPE.CTP, new RequestContent("RequestDisConnect", new List<object>())); //RequestDisConnect();
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception:" + ex.Message);
                Util.Log("exception:" + ex.StackTrace);
            }
            tbcUserCfm.Items.Clear();
            this.Hide();
        }
    }
}
