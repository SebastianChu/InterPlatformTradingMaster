using System.Windows;
using System.Windows.Controls;

namespace TradingMaster.Control
{
    /// <summary>
    /// SystemTips.xaml 的交互逻辑
    /// </summary>
    public partial class SystemTips : UserControl
    {
        public SystemTips()
        {
            CommonStaticMemeber.LoadStyleFromUserFile();
            this.DataContext = this;
            InitializeComponent();
            //ItemsSource="{Binding SystemMessageCollection}"
        }

        public void Init(MainWindow parent)
        {
            this._MainWindow = parent;
            this.dgSystemMessage.ItemsSource = _MainWindow.SystemMessageCollection;
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
    }
}
