using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace TradingMaster.Control
{
    /// <summary>
    /// OrderAffirmWindow.xaml 的交互逻辑
    /// </summary>
    /// <summary>
    /// OrderAffirmWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OrderAffirmWindow : Window
    {
        public static OrderAffirmWindow G_OrderAffirmWindow = null;
        /// <summary>
        /// 委托确认信息
        /// </summary>
        private ObservableCollection<OrderAffirmItem> _OrderAffirmItemCollection = new ObservableCollection<OrderAffirmItem>();
        /// <summary>
        /// 委托确认信息
        /// </summary>
        public ObservableCollection<OrderAffirmItem> OrderAffirmItemCollection
        {
            get { return _OrderAffirmItemCollection; }
            set { _OrderAffirmItemCollection = value; }
        }


        public bool IsConfirm;


        public OrderAffirmWindow()
        {
            InitializeComponent();
            G_OrderAffirmWindow = this;

            foreach (var item in dataGrid1.Columns)
            {
                item.Width = DataGridLength.Auto;
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            IsConfirm = true;
            this.Hide();
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            IsConfirm = false;
            this.Hide();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsConfirm = false;
            chkConfirm.IsChecked = false;
            button1.Focus();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void ShowModalWindow()
        {
            chkConfirm.IsChecked = false;
            //需要根据列表中内容的个数调整高度
            foreach (var item in OrderAffirmItemCollection)
            {
                if (item.Code.StartsWith("SP") || item.Code.StartsWith("IO"))
                {
                    CodeName.Width = 130;
                }
                else
                {
                    CodeName.Width = 57;
                }
                break;
            }
            if (OrderAffirmItemCollection.Count <= 5)
            {
                dataGrid1.Height = OrderAffirmItemCollection.Count * 20;
                dataGrid1.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            }
            else
            {
                dataGrid1.Height = 100;
                dataGrid1.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            }
            this.button1.Focus();
            this.ShowDialog();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.button1.Focus();
        }
    }


    /// <summary>
    /// 委托确认信息
    /// </summary>
    public class OrderAffirmItem : INotifyPropertyChanged
    {
        /// <summary>
        /// 投资者代码
        /// </summary>
        private string _InvestorID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        public string InvestorID
        {
            get { return _InvestorID; }
            set
            {
                _InvestorID = value;
                OnPropertyChanged("InvestorID");
            }
        }

        /// <summary>
        /// 用户代码
        /// </summary>
        private string _UserID;
        /// <summary>
        /// 用户代码
        /// </summary>
        public string UserID
        {
            get { return _UserID; }
            set
            {
                _UserID = value;
                OnPropertyChanged("UserID");
            }
        }

        /// <summary>
        /// 下单，撤单
        /// </summary>
        private string orderString;
        /// <summary>
        /// 下单撤单
        /// </summary>
        public string OrderString
        {
            get { return orderString; }
            set { orderString = value; OnPropertyChanged("OrderString"); }
        }

        /// <summary>
        /// 开仓，平今，平仓
        /// </summary>
        private string posEffect;
        /// <summary>
        /// 开仓，平今，平仓
        /// </summary>
        public string PosEffect
        {
            get { return posEffect; }
            set { posEffect = value; OnPropertyChanged("PosEffect"); }
        }

        /// <summary>
        /// 买卖
        /// </summary>
        private string buysell;
        /// <summary>
        /// 买卖
        /// </summary>
        public string Buysell
        {
            get { return buysell; }
            set { buysell = value; OnPropertyChanged("Buysell"); }
        }

        /// <summary>
        /// 代码
        /// </summary>
        private string code;
        /// <summary>
        /// 代码
        /// </summary>
        public string Code
        {
            get { return code; }
            set { code = value; OnPropertyChanged("Code"); }
        }

        /// <summary>
        /// 多少手
        /// </summary>
        private int handCount;
        /// <summary>
        /// 手数
        /// </summary>
        public int HandCount
        {
            get { return handCount; }
            set { handCount = value; OnPropertyChanged("HandCount"); }
        }

        /// <summary>
        /// 价格
        /// </summary>
        private double price;
        /// <summary>
        /// 价格
        /// </summary>
        public double Price
        {
            get { return price; }
            set { price = value; OnPropertyChanged("Price"); }
        }

        /// <summary>
        /// 恒为投机
        /// </summary>
        private string hedge;
        /// <summary>
        /// 恒为投机
        /// </summary>
        public string Hedge
        {
            get { return hedge; }
            set { hedge = value; OnPropertyChanged("Hedge"); }
        }

        /// <summary>
        /// 后台接口类型
        /// </summary
        private BACKENDTYPE _BackEnd;
        /// <summary>
        /// 后台接口类型
        /// </summary
        public BACKENDTYPE BackEnd
        {
            get { return _BackEnd; }
            set { _BackEnd = value; OnPropertyChanged("BackEnd"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
