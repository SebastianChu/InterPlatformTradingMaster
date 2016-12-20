using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace TradingMaster
{
    /// <summary>
    /// MultiUserLogin.xaml 的交互逻辑
    /// </summary>
    public partial class MultiUserLogin : Window
    {
        public MultiUserLogin(Login logWindow)
        {
            ResourceDictionary o = (ResourceDictionary)Application.LoadComponent(new Uri("/TradingMaster;component/Dictionary1.xaml", UriKind.Relative));
            Application.Current.Resources.MergedDictionaries.Add(o);
            this.DataContext = this;
            InitializeComponent();
            _LogWindow = logWindow;

            this.dgUserSettings.ItemsSource = UserSettingsCollection;
        }

        private Login _LogWindow { get; set; }

        private ObservableCollection<UserLogonStruct> _UserSettingsCollection = new ObservableCollection<UserLogonStruct>();
        public ObservableCollection<UserLogonStruct> UserSettingsCollection
        {
            get { return _UserSettingsCollection; }
            set { _UserSettingsCollection = value; }
        }

        private void rbSingleUser_Checked(object sender, RoutedEventArgs e)
        {
            this.Hide();
            _LogWindow.rbSingleUser.IsChecked = true;
            _LogWindow.Show();
        }

        private void btnLogonAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLogon_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    /// <summary>
    /// 登录用户信息
    /// </summary>
    public class UserLogonStruct
    {
        public string InvestorID { get; set; }

        public string BrokerID { get; set; }

        public BACKENDTYPE BackEnd { get; set; }

        public string Password { get; set; }

        public string TraderFrontIP { get; set; }

        public string MdFrontIP { get; set; }
    }
}
