using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TradingMaster.CodeSet;

namespace TradingMaster.Control
{
    /// <summary>
    /// LevelsQuotes.xaml 的交互逻辑
    /// </summary>
    public partial class LevelsQuotes : UserControl
    {
        public LevelsQuotes()
        {
            CommonStaticMemeber.LoadStyleFromUserFile();
            InitializeComponent();
        }

        public TradeClientStyleUI CurrentClientStyleUI
        {
            get { return CommonStaticMemeber.CurrentClientStyleUI; }
        }

        public delegate void LeftRealDataMouseLeftButtonDownDelegate(string buyOrSell, RealData realData, double selectedPrice, bool isBuySell);
        public event LeftRealDataMouseLeftButtonDownDelegate LevelRealDataMouseLeftButtonDown;

        private MainWindow _MainWindow { get; set; }

        public void Init(MainWindow parent)
        {
            this.DataContext = parent;
            this._MainWindow = parent;
        }

        public void SetLevelsQuotesByRealData(RealData lvRealData)
        {
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    try
                    {
                        Contract realContract = lvRealData.CodeInfo;
                        if (lvRealData != null && lvRealData.CodeInfo != null
                            //&& mainWindow.uscHangqing.LvQuotesPanel.Visibility == Visibility.Visible 
                            && this.lblCode.Content.ToString() == realContract.Code)
                        {
                            double priceFluctuation = GetCommissionRatio(lvRealData);//GetPriceFluctuation(lvRealData);
                            if (priceFluctuation > 0.0)
                            {
                                this.lblPriceFluctuation.Foreground = this.CurrentClientStyleUI.UpColorBrush;
                                this.lblPriceFluctuation.Content = "+" + priceFluctuation.ToString("0.00%");
                                //this.lblPriceFluctuation.Content = "+" + priceFluctuation.ToString(CommonUtil.GetPriceFormat(realContract.Code));
                            }
                            else if (priceFluctuation < 0.0)
                            {
                                this.lblPriceFluctuation.Foreground = this.CurrentClientStyleUI.DownColorBrush;
                                this.lblPriceFluctuation.Content = priceFluctuation.ToString("0.00%");
                                //this.lblPriceFluctuation.Content = priceFluctuation.ToString(CommonUtil.GetPriceFormat(realContract.Code));
                            }
                            else
                            {
                                this.lblPriceFluctuation.Foreground = this.CurrentClientStyleUI.HQContentForegroundBrush;
                                this.lblPriceFluctuation.Content = priceFluctuation.ToString("0.00%");
                                //this.lblPriceFluctuation.Content = priceFluctuation.ToString(CommonUtil.GetPriceFormat(realContract.Code));
                            }

                            this.lblBuy1.Content = lvRealData.BidPrice[0] == 0.0 ? "-" : lvRealData.BidPrice[0].ToString(CommonUtil.GetPriceFormat(realContract.Code));
                            this.lblBuy2.Content = lvRealData.BidPrice[1] == 0.0 ? "-" : lvRealData.BidPrice[1].ToString(CommonUtil.GetPriceFormat(realContract.Code));
                            this.lblBuy3.Content = lvRealData.BidPrice[2] == 0.0 ? "-" : lvRealData.BidPrice[2].ToString(CommonUtil.GetPriceFormat(realContract.Code));
                            this.lblBuy4.Content = lvRealData.BidPrice[3] == 0.0 ? "-" : lvRealData.BidPrice[3].ToString(CommonUtil.GetPriceFormat(realContract.Code));
                            this.lblBuy5.Content = lvRealData.BidPrice[4] == 0.0 ? "-" : lvRealData.BidPrice[4].ToString(CommonUtil.GetPriceFormat(realContract.Code));
                            this.lblBuyCount1.Content = lvRealData.BidPrice[0] == 0.0 ? "-" : lvRealData.BidHand[0].ToString();
                            this.lblBuyCount2.Content = lvRealData.BidPrice[1] == 0.0 ? "-" : lvRealData.BidHand[1].ToString();
                            this.lblBuyCount3.Content = lvRealData.BidPrice[2] == 0.0 ? "-" : lvRealData.BidHand[2].ToString();
                            this.lblBuyCount4.Content = lvRealData.BidPrice[3] == 0.0 ? "-" : lvRealData.BidHand[3].ToString();
                            this.lblBuyCount5.Content = lvRealData.BidPrice[4] == 0.0 ? "-" : lvRealData.BidHand[4].ToString();
                            this.lblSell1.Content = lvRealData.AskPrice[0] == 0.0 ? "-" : lvRealData.AskPrice[0].ToString(CommonUtil.GetPriceFormat(realContract.Code));
                            this.lblSell2.Content = lvRealData.AskPrice[1] == 0.0 ? "-" : lvRealData.AskPrice[1].ToString(CommonUtil.GetPriceFormat(realContract.Code));
                            this.lblSell3.Content = lvRealData.AskPrice[2] == 0.0 ? "-" : lvRealData.AskPrice[2].ToString(CommonUtil.GetPriceFormat(realContract.Code));
                            this.lblSell4.Content = lvRealData.AskPrice[3] == 0.0 ? "-" : lvRealData.AskPrice[3].ToString(CommonUtil.GetPriceFormat(realContract.Code));
                            this.lblSell5.Content = lvRealData.AskPrice[4] == 0.0 ? "-" : lvRealData.AskPrice[4].ToString(CommonUtil.GetPriceFormat(realContract.Code));
                            this.lblSellCount1.Content = lvRealData.AskPrice[0] == 0.0 ? "-" : lvRealData.AskHand[0].ToString();
                            this.lblSellCount2.Content = lvRealData.AskPrice[1] == 0.0 ? "-" : lvRealData.AskHand[1].ToString();
                            this.lblSellCount3.Content = lvRealData.AskPrice[2] == 0.0 ? "-" : lvRealData.AskHand[2].ToString();
                            this.lblSellCount4.Content = lvRealData.AskPrice[3] == 0.0 ? "-" : lvRealData.AskHand[3].ToString();
                            this.lblSellCount5.Content = lvRealData.AskPrice[4] == 0.0 ? "-" : lvRealData.AskHand[4].ToString();

                            this.lblBuyCount1.Background = new SolidColorBrush(Color.FromArgb(255, 255, 221, 221));
                            this.lblBuyCount2.Background = new SolidColorBrush(Color.FromArgb(255, 255, 221, 221));
                            this.lblBuyCount3.Background = new SolidColorBrush(Color.FromArgb(255, 255, 221, 221));
                            this.lblBuyCount4.Background = new SolidColorBrush(Color.FromArgb(255, 255, 221, 221));
                            this.lblBuyCount5.Background = new SolidColorBrush(Color.FromArgb(255, 255, 221, 221));
                            this.lblSellCount1.Background = new SolidColorBrush(Color.FromArgb(255, 221, 255, 221));
                            this.lblSellCount2.Background = new SolidColorBrush(Color.FromArgb(255, 221, 255, 221));
                            this.lblSellCount3.Background = new SolidColorBrush(Color.FromArgb(255, 221, 255, 221));
                            this.lblSellCount4.Background = new SolidColorBrush(Color.FromArgb(255, 221, 255, 221));
                            this.lblSellCount5.Background = new SolidColorBrush(Color.FromArgb(255, 221, 255, 221));
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Util.Log_Error("exception: " + ex.Message);
                        Util.Log_Error("exception: " + ex.StackTrace);
                    }
                });
            }
        }

        private double GetPriceFluctuation(RealData realData)
        {
            if (realData.NewPrice == 0.0)
            {
                return 0;
            }
            else if (realData.PrevSettlementPrice == 0)
            {
                return realData.NewPrice - realData.PrevClose;
            }
            else
            {
                return realData.NewPrice - realData.PrevSettlementPrice;
            }
        }

        private double GetCommissionRatio(RealData realData)
        {
            int askCommission = 0;
            int bidCommission = 0;
            for (int i = 0; i < realData.AskHand.Length; i++)
            {
                askCommission += (int)realData.AskHand[i];
            }
            for (int i = 0; i < realData.BidHand.Length; i++)
            {
                bidCommission += (int)realData.BidHand[i];
            }
            if (askCommission + bidCommission == 0) return 0;
            else if (askCommission > 0 && bidCommission > 0)
            {
                return (double)(bidCommission - askCommission) / (bidCommission + askCommission);
            }
            return 0.0;
        }

        private void LevelBuy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Label uscLabel = sender as Label;
                if (uscLabel != null)
                {
                    string priceName = uscLabel.Name;
                    if (uscLabel.Name.Contains("Count"))
                    {
                        priceName = uscLabel.Name.Replace("Count", "");
                    }
                    if (uscLabel.Name.Contains("Desc"))
                    {
                        priceName = uscLabel.Name.Replace("Desc", "");
                    }
                    Label priceLbl = this.FindName(priceName) as Label;
                    if (priceLbl != null && !priceLbl.Content.ToString().Contains("-"))
                    {
                        double selectedPrice = Double.Parse(priceLbl.Content.ToString());
                        LevelsQuotesClick(selectedPrice, "买");
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error("exception: " + ex.StackTrace);
            }
        }

        private void LevelSell_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Label uscLabel = sender as Label;
                if (uscLabel != null)
                {
                    string priceName = uscLabel.Name;
                    if (uscLabel.Name.Contains("Count"))
                    {
                        priceName = uscLabel.Name.Replace("Count", "");
                    }
                    if (uscLabel.Name.Contains("Desc"))
                    {
                        priceName = uscLabel.Name.Replace("Desc", "");
                    }
                    Label priceLbl = this.FindName(priceName) as Label;
                    if (priceLbl != null && !priceLbl.Content.ToString().Contains("-"))
                    {
                        double selectedPrice = Double.Parse(priceLbl.Content.ToString());
                        LevelsQuotesClick(selectedPrice, "卖");
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error("exception: " + ex.StackTrace);
            }
        }

        private void LevelsQuotesClick(double price, string buySell)
        {
            if (_MainWindow.dcFutHangqing.IsSelected)
            {
                foreach (var item in _MainWindow.RealDataCollection)
                {
                    if (item.Code == this.lblCode.Content.ToString())
                    {
                        if (LevelRealDataMouseLeftButtonDown != null)
                        {
                            RealData realRecord = new RealData();
                            if (realRecord.UpdateRealProperties(item as DisplayRealData))
                            {
                                LevelRealDataMouseLeftButtonDown(buySell, realRecord, price, false);
                                break;
                            }
                        }
                    }
                }
            }
            else if (_MainWindow.dcOptHangqing.IsSelected)
            {
                foreach (OptionRealData item in _MainWindow.OptionRealDataCollection)
                {
                    if (item.Code_C == this.lblCode.Content.ToString())
                    {
                        if (LevelRealDataMouseLeftButtonDown != null)
                        {
                            RealData realRecord = item.GetOptRealData_C();
                            if (realRecord != null)
                            {
                                LevelRealDataMouseLeftButtonDown(buySell, realRecord, price, false);
                                break;
                            }
                        }
                    }
                    else if (item.Code_P == this.lblCode.Content.ToString())
                    {
                        if (LevelRealDataMouseLeftButtonDown != null)
                        {
                            RealData realRecord = item.GetOptRealData_P();
                            if (realRecord != null)
                            {
                                LevelRealDataMouseLeftButtonDown(buySell, realRecord, price, false);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
