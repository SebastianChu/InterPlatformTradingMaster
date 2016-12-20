using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace TradingMaster.Control
{
    /// <summary>
    /// ValueUpDown.xaml 的交互逻辑
    /// </summary>
    /// <summary>
    /// ValueUpDown.xaml 的交互逻辑
    /// </summary>
    public partial class ValueUpDown : UserControl
    {
        //值
        public double? Value
        {
            get
            {
                double num;
                if (double.TryParse(textBox.Text, out num) == false)
                {
                    return null;
                }
                return num;
            }
            set
            {
                if (value != null)
                {
                    if (FormatString != "" && FormatString != null)
                    {
                        textBox.Text = value.Value.ToString(FormatString);
                    }
                    else
                    {
                        textBox.Text = value.Value.ToString("F0");
                    }
                }
            }
        }

        protected decimal _Increment;
        public decimal Increment
        {
            get
            {
                return _Increment;
            }
            set
            {
                _Increment = value;

                if (0 != ((int)(_Increment * 1000)) % 10)
                {
                    //小数点后三位
                    FormatString = "F3";
                }
                else if (0 != ((int)(_Increment * 100)) % 10)
                {
                    //小数点后两位
                    FormatString = "F2";
                }
                else if (0 != ((int)(_Increment * 10)) % 10)
                {
                    //小数点后一位
                    FormatString = "F1";
                }
                else
                {
                    //无小数点
                    FormatString = "F0";
                }
            }
        }

        public double Minimum { get; set; }

        public double Maximum { get; set; }

        protected string formatString;
        public string FormatString
        {
            get
            {
                return formatString;
            }
            set
            {
                formatString = value;
            }
        }

        public string Text
        {
            get
            {
                return textBox.Text;
            }
        }

        /// <summary>
        /// 属性 ColorTag
        /// </summary>
        public static readonly DependencyProperty colorTagProperty = DependencyProperty.Register("ColorTag", typeof(string), typeof(ValueUpDown), new PropertyMetadata(null, new PropertyChangedCallback(ColorTagChange)));
        public String ColorTag
        {
            get
            {
                return (string)GetValue(colorTagProperty);
            }
            set
            {
                SetValue(colorTagProperty, value);
            }
        }
        public static void ColorTagChange(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {

        }


        public ValueUpDown()
        {
            InitializeComponent();
            Increment = 1;
            Minimum = 0;
            Maximum = 100;
        }
        protected void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "-")
            {
                ChangeBackToFix();
            }
            TextChange[] change = new TextChange[e.Changes.Count];
            e.Changes.CopyTo(change, 0);
            int offset = change[0].Offset;
            if (change[0].AddedLength > 0)
            {
                double num = 0;
                if (!Double.TryParse(textBox.Text, out num) || textBox.Text.Contains(',') || (FormatString == "F0" && textBox.Text.Contains('.')))
                {
                    textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
                    textBox.Select(offset, 0);
                }
            }
            Reform(false);
            ChangeBackToFix();
        }

        protected void ChangeBackToFix()
        {
            MainWindow mainWindow = TradeDataClient.GetClientInstance().getMainWindow();
            if (mainWindow != null)
            {
                //if (mainWindow.uscNewOrderPanel.isNormalChange == false && mainWindow.uscNewOrderPanel.followPrice.IsChecked == true)
                //{
                //    //如果是自动
                //    mainWindow.uscNewOrderPanel.followPrice.IsChecked = false;
                //}
            }
        }

        protected void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.A && e.Key <= Key.Z)
            {
                e.Handled = true;
            }
        }

        protected void DoAdd()
        {
            if (Value == null)
            {
                Value = 0;
            }
            UnifyData();
            Value = Value + (double)Increment;
            if (Value > Maximum) Value = Maximum;
            else if (Value < Minimum) Value = Minimum;
        }

        protected void DoMinus()
        {
            if (Value == null)
            {
                Value = 0;
            }
            UnifyData();
            Value = Value - (double)Increment;
            if (Value > Maximum) Value = Maximum;
            else if (Value < Minimum) Value = Minimum;
        }

        /// <summary>
        /// 将不符合最小变动单位的值，统一变为符合最小变动单位的值.
        /// </summary>
        protected void UnifyData()
        {
            if (Value == null) return;

            long price1000 = (long)Math.Round(Value.Value * 1000);
            int fluct1000 = (int)Math.Round(Increment * 1000);
            Boolean bNeedModify = false;
            if ((double)price1000 / 1000 != Value.Value)
            {
                bNeedModify = true;
            }
            else
            {
                if ((price1000) % (fluct1000) != 0)
                {
                    bNeedModify = true;
                }
            }

            if (bNeedModify)
            {
                double v = Value.Value;
                v = (fluct1000 * (price1000 / fluct1000)) / 1000;
                Value = v;
            }
        }

        protected void btnUp_Click(object sender, RoutedEventArgs e)
        {
            DoAdd();
            UpdateOrderPanelPriceFixInfo();
        }

        protected void btnDown_Click(object sender, RoutedEventArgs e)
        {
            DoMinus();
            UpdateOrderPanelPriceFixInfo();
        }

        protected void UpdateOrderPanelPriceFixInfo()
        {
            MainWindow mainWindow = TradeDataClient.GetClientInstance().getMainWindow();
            if (mainWindow != null)
            {
                //if (mainWindow.uscNewOrderPanel.PriceType == NewOrderPanel.PriceTypeAutoPrice)
                //{
                //    如果是自动
                //    mainWindow.uscNewOrderPanel.followPrice.IsChecked = false;
                //}
            }
        }

        protected void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.PageUp)
            {
                DoAdd();
                e.Handled = true;
            }
            else if (e.Key == Key.PageDown)
            {
                DoMinus();
                e.Handled = true;
            }
        }

        protected void textBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                DoAdd();
            }
            else
            {
                DoMinus();
            }
        }

        protected void MySpinControl_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox.Focus();
            textBox.SelectAll();
        }

        public void Reform(Boolean bRefillContent)
        {
            double val = 0;
            if (Double.TryParse(textBox.Text, out val))
            {
                if (val > Maximum)
                {
                    this.Value = Maximum;
                }
                else if (val < Minimum)
                {
                    this.Value = Minimum;
                }
            }

            if (bRefillContent)
            {
                this.Value = val;
            }
        }

        protected void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Reform(true);

        }

        protected void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox.SelectAll();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TagConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return 0;
            if (value.ToString().ToLower() == "red") return 1;
            if (value.ToString().ToLower() == "blue") return -1;
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
