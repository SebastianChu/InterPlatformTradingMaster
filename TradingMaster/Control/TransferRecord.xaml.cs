using System.Windows;
using System.Windows.Controls;

namespace TradingMaster.Control
{
    /// <summary>
    /// TransferRecord.xaml 的交互逻辑
    /// </summary>
    public partial class TransferRecord : UserControl
    {
        public TransferRecord()
        {
            InitializeComponent();
        }

        public void Init(MainWindow parent)
        {
            this.DataContext = parent;
            this._MainWindow = parent;
        }

        private MainWindow _MainWindow { get; set; }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Window).Hide();
        }
    }
}
