using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Threading;

namespace TradingMaster.Control
{
    /// <summary>
    /// StatementsInquiry.xaml 的交互逻辑
    /// </summary>
    public partial class StatementsInquiry : UserControl
    {
        public StatementsInquiry()
        {
            InitializeComponent();
            this.dpStatementOrder.SelectedDate = CommonUtil.GetLastWorkDayBeforeToday();
            if (this.dpStatementOrder.SelectedDate != null)
            {
                _Date = this.dpStatementOrder.SelectedDate.Value.ToString("yyyyMMdd");
            }
        }

        int _QueryTimeOut = 60000;//查询超过60000就认为超时了

        private string _Date = DateTime.Now.ToString("yyyyMMdd");
        public void btnQueryStatementOrder_Click(object sender, RoutedEventArgs e)
        {
            if (dpStatementOrder.SelectedDate != null)
            {
                if (dpStatementOrder.SelectedDate.Value > DateTime.Now)
                {
                    MessageBox.Show("无法查询日期晚于今天的结算单");
                    return;
                }
                string queryDate = dpStatementOrder.SelectedDate.Value.Date.ToString("yyyyMMdd");
                _Date = queryDate;
                txtStatementOrder.Text = "正在查询，请稍等...";
                btnQueryStatementOrder.IsEnabled = btnQueryMonthStatementOrder.IsEnabled = false;

                TradeDataClient.GetClientInstance().RequestTradeData("", BACKENDTYPE.CTP, new RequestContent("RequestSettlementInstructions", new List<object>() { queryDate }));
                //JYDataServer.getServerInstance().RequestSettlementInstructions(queryDate);

                Thread t = new Thread(delegate ()
                {
                    Thread.Sleep(_QueryTimeOut);

                    if (Application.Current != null)
                    {
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            if (btnQueryStatementOrder.IsEnabled == false || btnQueryMonthStatementOrder.IsEnabled == false)
                            {
                                btnQueryStatementOrder.IsEnabled = btnQueryMonthStatementOrder.IsEnabled = true;
                                txtStatementOrder.Text = "系统忙，请稍后...";
                                //Util.Log("查询结算单信息超时" + DateTime.Now.ToLongTimeString());

                                _Date = dpStatementOrder.SelectedDate.Value.ToString("yyyyMMdd");
                            }
                        });
                    }
                });
                t.Start();
            }
        }

        public void btnQueryMonthStatementOrder_Click(object sender, RoutedEventArgs e)
        {
            if (dpStatementOrder.SelectedDate != null)
            {
                if (dpStatementOrder.SelectedDate.Value > DateTime.Now)
                {
                    MessageBox.Show("无法查询日期晚于今天的结算单");
                    return;
                }
                string queryDate = dpStatementOrder.SelectedDate.Value.Date.ToString("yyyyMMdd");
                _Date = queryDate;
                txtStatementOrder.Text = "正在查询，请稍等...";
                btnQueryStatementOrder.IsEnabled = btnQueryMonthStatementOrder.IsEnabled = false;

                TradeDataClient.GetClientInstance().RequestTradeData("", BACKENDTYPE.CTP, new RequestContent("RequestSettlementInstructions", new List<object>() { queryDate.Substring(0, queryDate.Length - 2) }));
                Thread t = new Thread(delegate ()
                {
                    Thread.Sleep(_QueryTimeOut);

                    if (Application.Current != null)
                    {
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            if (btnQueryStatementOrder.IsEnabled == false || btnQueryMonthStatementOrder.IsEnabled == false)
                            {
                                btnQueryStatementOrder.IsEnabled = btnQueryMonthStatementOrder.IsEnabled = true;
                                txtStatementOrder.Text = "系统忙，请稍后...";
                                //Util.Log("查询结算单信息超时" + DateTime.Now.ToLongTimeString());

                                _Date = dpStatementOrder.SelectedDate.Value.ToString("yyyyMMdd");
                            }
                        });
                    }
                });
                t.Start();
            }
        }

        private void btnSaveStatementOrder_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.SaveStatementOrder(txtStatementOrder.Text, _Date);
        }

        private void btnCfmmc_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.OpenCFMMC();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            Run r = new Run(txtStatementOrder.Text);

            Paragraph ph = new Paragraph(r);

            FlowDocument fd = new FlowDocument(ph);

            fd.PagePadding = new Thickness(100);

            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintDocument(((IDocumentPaginatorSource)fd).DocumentPaginator, "结算单");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Window).Close();
        }

    }
}
