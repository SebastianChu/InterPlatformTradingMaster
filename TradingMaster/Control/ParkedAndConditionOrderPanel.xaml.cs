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
    /// ParkedAndConditionOrderPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ParkedAndConditionOrderPanel : UserControl
    {
        public ParkedAndConditionOrderPanel(string formatString)
        {
            InitializeComponent();

            InitControls();

            rbMaidan.Checked += new RoutedEventHandler(rbMaidan_Checked);
            rbAuto.Checked += new RoutedEventHandler(rbMaidan_Checked);
            rbCondition.Checked += new RoutedEventHandler(rbCondition_Checked);
            dudPrice.FormatString = formatString;
        }

        private void rbMaidan_Checked(object sender, RoutedEventArgs e)
        {
            spCondition.Visibility = Visibility.Collapsed;
        }

        private void rbCondition_Checked(object sender, RoutedEventArgs e)
        {
            spCondition.Visibility = Visibility.Visible;
        }

        private void InitControls()
        {
            List<SimpleComboxItem> lstConditionType = new List<SimpleComboxItem>();
            lstConditionType.Add(new SimpleComboxItem() { ID = 1, Name = "最新价" });
            lstConditionType.Add(new SimpleComboxItem() { ID = 3, Name = "卖一价" });
            lstConditionType.Add(new SimpleComboxItem() { ID = 2, Name = "买一价" });
            cbConditionType.SelectedValuePath = "ID";
            cbConditionType.DisplayMemberPath = "Name";
            cbConditionType.ItemsSource = lstConditionType;
            cbConditionType.SelectedIndex = 0;


            List<SimpleComboxItem> lstPriceType = new List<SimpleComboxItem>();
            lstPriceType.Add(new SimpleComboxItem() { ID = 1, Name = "≥" });
            lstPriceType.Add(new SimpleComboxItem() { ID = 2, Name = "≤" });
            cbPriceType.SelectedValuePath = "ID";
            cbPriceType.DisplayMemberPath = "Name";
            cbPriceType.ItemsSource = lstPriceType;
            cbPriceType.SelectedIndex = 0;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Window window = (this.Parent as Window);
            window.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Window window = (this.Parent as Window);
            window.DialogResult = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>0:正常，1：自动单， 2：埋单</returns>
        public int GetOrderType()
        {
            if (rbAuto.IsChecked == true)
            {
                return 1;
            }
            else if (this.rbMaidan.IsChecked == true)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>触发条件 0表示没有触发条件，1表示大于等于，2表示小于等于</returns>
        public int GetTouchCondition()
        {
            if (rbCondition.IsChecked != true)
            {
                return 0;
            }
            else
            {
                return (int)cbPriceType.SelectedValue;
            }
        }

        public double GetTouchPrice()
        {
            if (rbCondition.IsChecked != true)
            {
                return 0;
            }
            else
            {
                return CommonUtil.GetDoubleValue(dudPrice.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>触发方式 0表示没有触发方式，1表示最新价，2表示买价，3表示卖价</returns>
        public int GetTouchMethod()
        {
            if (rbCondition.IsChecked != true)
            {
                return 0;
            }
            else
            {
                return (int)cbConditionType.SelectedValue;
            }
        }
    }

    public class SimpleComboxItem
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}


