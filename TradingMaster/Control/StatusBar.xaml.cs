using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TradingMaster.JYData;

namespace TradingMaster.Control
{
    /// <summary>
    /// StatusBar.xaml 的交互逻辑
    /// </summary>
    public partial class StatusBar : UserControl
    {
        private BitmapImage _ImageJY_CONN;
        private BitmapImage _ImageJY_DISCONN;
        private BitmapImage _ImageHQ_CONN;
        private BitmapImage _ImageHQ_DISCONN;

        public Cursor DefaultCursor;

        public StatusBar()
        {
            CommonStaticMemeber.LoadStyleFromUserFile();
            this.DataContext = this;
            InitializeComponent();
            tbStartTime.Text = DateTime.Now.ToString("HH:mm:ss");

            _ImageJY_CONN = new BitmapImage();
            _ImageJY_CONN.BeginInit();
            _ImageJY_CONN.UriSource = new Uri("/TradingMaster;component/image/JY_Conn.png", UriKind.RelativeOrAbsolute);
            _ImageJY_CONN.EndInit();

            _ImageJY_DISCONN = new BitmapImage();
            _ImageJY_DISCONN.BeginInit();
            _ImageJY_DISCONN.UriSource = new Uri("/TradingMaster;component/image/JY_DISCONN.png", UriKind.RelativeOrAbsolute);
            _ImageJY_DISCONN.EndInit();

            _ImageHQ_CONN = new BitmapImage();
            _ImageHQ_CONN.BeginInit();
            _ImageHQ_CONN.UriSource = new Uri("/TradingMaster;component/image/HQ_Conn.png", UriKind.RelativeOrAbsolute);
            _ImageHQ_CONN.EndInit();

            _ImageHQ_DISCONN = new BitmapImage();
            _ImageHQ_DISCONN.BeginInit();
            _ImageHQ_DISCONN.UriSource = new Uri("/TradingMaster;component/image/HQ_DISCONN.png", UriKind.RelativeOrAbsolute);
            _ImageHQ_DISCONN.EndInit();
        }

        public void Init(MainWindow parent)
        {
            this._MainWindow = parent;

            DefaultCursor = this.Cursor;
            tbStartTime.MouseEnter += new MouseEventHandler(tbMouseEnter);
            tbStartTime.MouseLeave += new MouseEventHandler(tbMouseLeave);

            tbMessge.MouseEnter += new MouseEventHandler(tbMouseEnter);
            tbMessge.MouseLeave += new MouseEventHandler(tbMouseLeave);
        }


        void tbMouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = DefaultCursor;
        }

        void tbMouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private MainWindow _MainWindow { get; set; }
        public CommonUtil Common = new CommonUtil();
        private Boolean jyLogOn;
        private Boolean hqLogOn;
        //private bool JYConnection;
        //private bool ZTConnection;

        public void OnGotMoney(JYRealData jyRealData)
        {
            //mainWindow.CapitalDataCollection.Add(jyRealData);
            //money.Content = "可用:¥ " + jyRealData.TodayAvailable.ToString() + " 浮盈:¥ " + jyRealData.Dsfy.ToString() + " 平盈:¥ " + jyRealData.Dspy.ToString() + " 保证金:¥ " + jyRealData.Bond.ToString();
        }

        public void TimeChange(ExchangeTime exchangeTime)
        {
            tbZhengzhouTime.Text = FormTimeString(exchangeTime.CzcsTime, tbZhengzhouTime);
            tbShanghaiTime.Text = FormTimeString(exchangeTime.ShfeTime, tbShanghaiTime);
            tbDalianTime.Text = FormTimeString(exchangeTime.DceTime, tbDalianTime);
            tbZhongjinTime.Text = FormTimeString(exchangeTime.FeexTime, tbZhongjinTime);
            tbEnergyTime.Text = FormTimeString(exchangeTime.IneTime, tbEnergyTime);
        }

        private string FormTimeString(int time, TextBlock tb)
        {
            int hour = time / 3600;
            int minute = (time % 3600) / 60;
            int second = time % 60;
            string ret = hour.ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00");

            string toolTip = tb.ToolTip.ToString();

            if (tb == tbZhongjinTime)
            {
                //中金所
                //if (time >= 9 * 3600 + 25 * 60 && time < 9 * 3600 + 29 * 60)    //9:25-9:29
                if (time >= 9 * 3600 + 10 * 60 && time < 9 * 3600 + 14 * 60)  //9:10-9:14
                {
                    //9:10-9:14
                    tb.Tag = "Trade";
                    tb.ToolTip = toolTip.Substring(0, toolTip.IndexOf("：") + 1) + "集合竞价报单";
                }
                //else if (time >= 9 * 3600 + 29 * 60 && time < 9 * 3600 + 30 * 60)   //9:29-9:30
                else if (time >= 9 * 3600 + 14 * 60 && time < 9 * 3600 + 15 * 60)   //9:14-9:15
                {
                    //9:14-9:15
                    tb.Tag = "NonTrade";
                    tb.ToolTip = toolTip.Substring(0, toolTip.IndexOf("：") + 1) + "集合竞价撮合";
                }
                //else if ((time >= 9 * 3600 + 30 * 60 && time < 11 * 3600 + 30 * 60) || (time >= 13 * 3600 && time < 15 * 3600))
                else if ((time >= 9 * 3600 + 15 * 60 && time < 11 * 3600 + 30 * 60) || (time >= 13 * 3600 && time < 15 * 3600 + 15 * 60))
                {
                    //9:15-11:30, 13:00-15:15
                    tb.Tag = "Trade";
                    tb.ToolTip = toolTip.Substring(0, toolTip.IndexOf("：") + 1) + "连续交易";
                }
                //else if (time >= 15 * 3600)
                else if (time >= 15 * 3600 + 15 * 60)
                {
                    tb.Tag = "NonTrade";
                    tb.ToolTip = toolTip.Substring(0, toolTip.IndexOf("：") + 1) + "收盘";
                }
                else
                {
                    tb.Tag = "NonTrade";
                    tb.ToolTip = toolTip.Substring(0, toolTip.IndexOf("：") + 1) + "开盘前";
                }
            }
            else if (tb == tbShanghaiTime || tb == tbEnergyTime)//TODO: Temp solution for night trading
            {
                if (time >= 20 * 3600 + 55 * 60 && time < 20 * 3600 + 59 * 60 || time >= 8 * 3600 + 55 * 60 && time < 8 * 3600 + 59 * 60)
                {
                    //20:55-20:59, 8:55-8:59
                    tb.Tag = "Trade";
                    tb.ToolTip = toolTip.Substring(0, toolTip.IndexOf("：") + 1) + "集合竞价报单";
                }
                else if (time >= 20 * 3600 + 59 * 60 && time < 21 * 3600 || time >= 8 * 3600 + 59 * 60 && time < 9 * 3600)
                {
                    //20:59-21:00, 8:59-9:00
                    tb.Tag = "NonTrade";
                    tb.ToolTip = toolTip.Substring(0, toolTip.IndexOf("：") + 1) + "集合竞价撮合";
                }
                else if ((time >= 21 * 3600 || time <= 2 * 3600 + 30 * 60) || (time >= 9 * 3600 && time < 10 * 3600 + 15 * 60) || (time >= 10 * 3600 + 30 * 60 && time < 11 * 3600 + 30 * 60) || (time >= 13 * 3600 + 30 * 60 && time < 15 * 3600))
                {
                    //21:00-2:30, 9:00-10:15, 10:30-11:30, 13:30-15:00
                    tb.Tag = "Trade";
                    tb.ToolTip = toolTip.Substring(0, toolTip.IndexOf("：") + 1) + "连续交易";
                }
                else if (time >= 15 * 3600)
                {
                    tb.Tag = "NonTrade";
                    tb.ToolTip = toolTip.Substring(0, toolTip.IndexOf("：") + 1) + "收盘";
                }
                else
                {
                    tb.Tag = "NonTrade";
                    tb.ToolTip = toolTip.Substring(0, toolTip.IndexOf("：") + 1) + "开盘前";
                }
            }
            else if (tb == tbDalianTime || tb == tbZhengzhouTime)//TODO: Temp solution for night trading
            {
                if (time >= 20 * 3600 + 55 * 60 && time < 20 * 3600 + 59 * 60 || time >= 8 * 3600 + 55 * 60 && time < 8 * 3600 + 59 * 60)
                {
                    //20:55-20:59, 8:55-8:59
                    tb.Tag = "Trade";
                    tb.ToolTip = toolTip.Substring(0, toolTip.IndexOf("：") + 1) + "集合竞价报单";
                }
                else if (time >= 20 * 3600 + 59 * 60 && time < 21 * 3600 || time >= 8 * 3600 + 59 * 60 && time < 9 * 3600)
                {
                    //20:59-21:00, 8:59-9:00
                    tb.Tag = "NonTrade";
                    tb.ToolTip = toolTip.Substring(0, toolTip.IndexOf("：") + 1) + "集合竞价撮合";
                }
                else if ((time >= 21 * 3600 && time <= 23 * 3600 + 30 * 60) || (time >= 9 * 3600 && time < 10 * 3600 + 15 * 60) || (time >= 10 * 3600 + 30 * 60 && time < 11 * 3600 + 30 * 60) || (time >= 13 * 3600 + 30 * 60 && time < 15 * 3600))
                {
                    //21:00-23:30, 9:00-10:15, 10:30-11:30, 13:30-15:00
                    tb.Tag = "Trade";
                    tb.ToolTip = toolTip.Substring(0, toolTip.IndexOf("：") + 1) + "连续交易";
                }
                else if (time >= 15 * 3600)
                {
                    tb.Tag = "NonTrade";
                    tb.ToolTip = toolTip.Substring(0, toolTip.IndexOf("：") + 1) + "收盘";
                }
                else
                {
                    tb.Tag = "NonTrade";
                    tb.ToolTip = toolTip.Substring(0, toolTip.IndexOf("：") + 1) + "开盘前";
                }
            }
            else
            {
                if ((time >= 9 * 3600 && time < 10 * 3600 + 15 * 60) || (time >= 10 * 3600 + 30 * 60 && time < 11 * 3600 + 30 * 60) || (time >= 13 * 3600 + 30 * 60 && time < 15 * 3600))
                {
                    tb.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 255));
                    tb.ToolTip = toolTip.Substring(0, toolTip.IndexOf("：") + 1) + "连续交易";
                }
                else
                {
                    tb.Foreground = Brushes.Red;
                    tb.ToolTip = toolTip.Substring(0, toolTip.IndexOf("：") + 1) + "未知";
                    //mainWindow.uscPositionsInquiry.btnFresh.IsEnabled = false;
                }
            }

            return ret;
        }


        public void FormString()
        {
            try
            {
                jyLogOn = DataContainer.GetUserInstance().TradeServerLogOn();
                hqLogOn = DataContainer.GetUserInstance().QuoteServerLogOn();

                Run run1 = new Run(jyLogOn ? "交易主机正常" : "交易主机断开");
                Run run2 = new Run(hqLogOn ? "行情主机正常" : "行情主机断开");

                SetPath();

                _MainWindow.AddSystemMessage(DateTime.Now, run1.Text + "," + run2.Text);
            }
            catch (Exception ex)
            {
                Util.Log(ex.ToString());
            }
        }

        /// <summary>
        /// 设置行情和交易服务器标志填充色
        /// </summary>
        private void SetPath()
        {
            if (jyLogOn)
            {
                SetJYImageStatus(true);
                //TxnImage.Source = imageJY_CONN;
                //TxnImage.ToolTip = "交易：在线";
                //this.TxnImage.Source = Common.getImageSource(normalImage);
            }
            else
            {
                SetJYImageStatus(false);
                //TxnImage.Source = imageJY_DISCONN;
                //TxnImage.ToolTip = "交易：断开";
                //this.TxnImage.Source = Common.getImageSource(errorImage);
            }

            if (hqLogOn)
            {
                SetHQImageStatus(true);
                //HQImage.Source = imageHQ_CONN;
                //HQImage.ToolTip = "行情：在线";
                //this.ZhutuiImage.Source = Common.getImageSource(normalImage);
            }
            else
            {
                SetHQImageStatus(false);
                //HQImage.Source = imageHQ_DISCONN;
                //HQImage.ToolTip = "行情：断开";
                //this.ZhutuiImage.Source = Common.getImageSource(errorImage); ;
            }
        }

        public void SetJYImageStatus(Boolean isLogOn)
        {
            if (isLogOn)
            {
                TxnImage.Source = _ImageJY_CONN;
                TxnImage.ToolTip = "交易：在线";
                //KiiikCommon.GlobalCommonUtils.Log("设置交易 在线 状态");
            }
            else
            {
                TxnImage.Source = _ImageJY_DISCONN;
                TxnImage.ToolTip = "交易：断开";
                //KiiikCommon.GlobalCommonUtils.Log("设置交易 断开 状态");
            }
        }

        public void SetHQImageStatus(Boolean isLogOn)
        {
            if (isLogOn)
            {
                HQImage.Source = _ImageHQ_CONN;
                HQImage.ToolTip = "行情：在线";
                //KiiikCommon.GlobalCommonUtils.Log("设置行情 在线 状态");
            }
            else
            {
                HQImage.Source = _ImageHQ_DISCONN;
                HQImage.ToolTip = "行情：断开";
                //KiiikCommon.GlobalCommonUtils.Log("设置行情 断开 状态");
            }
        }

        private void txtStartTime_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowSystemMessge();
        }

        private void ShowSystemMessge()
        {
            _MainWindow.ShowSystemMessge();
        }

        private void tbMessge_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowSystemMessge();
        }
    }
}
