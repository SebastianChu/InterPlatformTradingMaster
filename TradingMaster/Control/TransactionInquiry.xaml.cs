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

namespace TradingMaster.Control
{
    /// <summary>
    /// TransactionInquiry.xaml 的交互逻辑
    /// </summary>
    public partial class TransactionInquiry : UserControl
    {
        public TransactionInquiry()
        {
            CommonStaticMemeber.LoadStyleFromUserFile();
            this.DataContext = this;
            InitializeComponent();
        }

        public void Init(MainWindow parent)
        {
            this._MainWindow = parent;
            TransactionInq.ItemsSource = _MainWindow.TradeCollection_Code;
            TransactionInqDetail.ItemsSource = _MainWindow.TradeCollection_MX;
            //DataGridColumnBridgeUtils.LoadColumnsSettingFromFile(TransactionInq, MainWindow.SettingDictionaryPath);
            //DataGridColumnBridgeUtils.LoadColumnsSettingFromFile(TransactionInqDetail, MainWindow.SettingDictionaryPath);
        }

        public TradeClientStyleUI CurrentClientStyleUI
        {
            get { return CommonStaticMemeber.CurrentClientStyleUI; }
        }

        private MainWindow _MainWindow { get; set; }


        private void ExportDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.ExportedData_Click(sender, e, true);
        }

        private void miAutoAdjustColumnWidth_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.AutoAdjustColumnWidth_Click(sender, e);
        }

        private void rbTotal_Checked(object sender, RoutedEventArgs e)
        {
            TransactionInqDetail.Visibility = Visibility.Collapsed;
            TransactionInq.Visibility = Visibility.Visible;

        }

        private void rbDetail_Checked(object sender, RoutedEventArgs e)
        {
            TransactionInqDetail.Visibility = Visibility.Visible;
            TransactionInq.Visibility = Visibility.Collapsed;
        }
    }
}
