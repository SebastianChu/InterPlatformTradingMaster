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
