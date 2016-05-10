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
    /// CheckableMessageBox.xaml 的交互逻辑
    /// </summary>
    /// <summary>
    /// CheckableMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class CheckableMessageBox : UserControl
    {
        public bool ShowAsDialog = true;
        public CheckableMessageBox()
        {
            InitializeComponent();
            btnOk.Focus();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            SetDialogResult(true);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            SetDialogResult(false);
        }

        private void SetDialogResult(bool dialogResult)
        {
            if (ShowAsDialog)
            {
                Window window = (this.Parent as Window);
                window.DialogResult = dialogResult;
            }
            (this.Parent as Window).Close();
        }
    }
}
