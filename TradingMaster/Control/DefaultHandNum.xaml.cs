using System;
using System.Windows;

namespace TradingMaster.Control
{
    /// <summary>
    /// DefaultHandNum.xaml 的交互逻辑
    /// </summary>
    public partial class DefaultHandNum : Window
    {
        public DefaultHandNum()
        {
            InitializeComponent();
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
        }

        /// <summary>
        /// 设置手数，正数表示手数，负数表示百分比。
        /// </summary>
        /// <param name="handCount1"></param>
        /// <param name="handCount2"></param>
        /// <param name="handCount3"></param>
        public void SetHandCount(int handCount1, int handCount2, int handCount3)
        {
            textBox1.Text = HandCountNumToString(handCount1);
            textBox2.Text = HandCountNumToString(handCount2);
            textBox3.Text = HandCountNumToString(handCount3);
        }

        /// <summary>
        /// 得到手数，正数表示手数，负数表示百分比。
        /// </summary>
        /// <param name="handCount1"></param>
        /// <param name="handCount2"></param>
        /// <param name="handCount3"></param>
        public void GetHandCount(out int handCount1, out int handCount2, out int handCount3)
        {
            handCount1 = HandCountStringToNum(textBox1.Text);
            handCount2 = HandCountStringToNum(textBox2.Text);
            handCount3 = HandCountStringToNum(textBox3.Text);
        }

        public static string HandCountNumToString(int num)
        {
            if (num >= 0) return num.ToString();
            if (num >= -100) return (-num).ToString() + "%";
            return "1";
        }

        public static int HandCountStringToNum(string s)
        {
            if (s == null) return 1;
            s = s.Trim();
            Boolean bNeg = false;
            if (s.EndsWith("%"))
            {
                bNeg = true;
                s = s.Remove(s.IndexOf("%"));
            }

            int ret = 0;
            if (int.TryParse(s, out ret))
            {
                if (bNeg)
                {
                    ret = -ret;
                }
                return ret;
            }
            return 1;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }


    }
}
