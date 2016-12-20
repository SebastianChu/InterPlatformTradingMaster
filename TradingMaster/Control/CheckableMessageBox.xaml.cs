using System.Windows;
using System.Windows.Controls;

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
