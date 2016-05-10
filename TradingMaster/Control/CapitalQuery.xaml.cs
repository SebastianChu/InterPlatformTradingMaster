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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TradingMaster.Control
{
    /// <summary>
    /// CapitalQuery.xaml 的交互逻辑
    /// </summary>
    public partial class CapitalQuery : UserControl
    {
        private Window _JsjWindow = null;
        private Window _UpdWindow = null;
        private MainWindow _MainWindow { get; set; }
        //public JYRealData CapitalData { get; set; }

        public CapitalQuery()
        {
            CommonStaticMemeber.LoadStyleFromUserFile();            
            this.DataContext = this;
            InitializeComponent();
        }

        public void Init(MainWindow parent)
        {
            this._MainWindow = parent;
            grdCapital.DataContext = _MainWindow.CapitalDataCollection;
            //CapitalData = mainWindow.CapitalDataCollection;
        }

        public TradeClientStyleUI CurrentClientStyleUI
        {
            get { return CommonStaticMemeber.CurrentClientStyleUI; }
        }

        private void btnStatement_Click(object sender, RoutedEventArgs e)
        {
            if (_JsjWindow == null)
            {
                _JsjWindow = CommonUtil.GetWindow("历史结算结果查询", _MainWindow.UscStatementsInquiry, _MainWindow);
                _JsjWindow.SizeToContent = SizeToContent.Manual;
                _JsjWindow.ResizeMode = ResizeMode.CanResize;
                _JsjWindow.Height = 500;
                _JsjWindow.Width = 650;
                _JsjWindow.Closing += new CancelEventHandler(jsjWindow_Closing);
            }
            _JsjWindow.Show();
        }

        void jsjWindow_Closing(object sender, CancelEventArgs e)
        {
            _JsjWindow.Hide();
            e.Cancel = true;
        }

        private void btnUpdPwd_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    if (_UpdWindow == null)
                    {
                        _MainWindow.UpdatePwd = new ChangePass();
                        _UpdWindow = CommonUtil.GetWindow("修改交易密码", _MainWindow.UpdatePwd, _MainWindow);
                        _UpdWindow.Closing += new CancelEventHandler(updWindow_Closing);
                    }
                    _UpdWindow.Show();
                });
            }
        }

        void updWindow_Closing(object sender, CancelEventArgs e)
        {
            _UpdWindow.Hide();
            if (_MainWindow.UpdatePwd != null)
            {
                _MainWindow.UpdatePwd.Reset();
            }
            e.Cancel = true;
        }

        private void btnOption_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnFresh_Click(object sender, RoutedEventArgs e)
        {
            //CtpDataServer.getServerInstance().InitDataFromAPI();
            TradeDataClient.GetClientInstance().RefreshSystemData();
        }
    }
}
