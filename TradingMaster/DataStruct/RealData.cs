using System;
using TradingMaster.CodeSet;
using System.ComponentModel;

namespace TradingMaster
{
    /// <summary>
    /// 某个时刻的行情
    /// </summary>
    public class RealData : INotifyPropertyChanged
    {
        public RealData()
        {
            CodeInfo = new Contract();
        }

        /// <summary>
        /// 
        /// </summary>
        private Contract _CodeInfo;
        public Contract CodeInfo
        {
            get { return _CodeInfo; }
            set
            {
                _CodeInfo = value;
                OnPropertyChanged("CodeInfo");
            }
        }

        /// <summary>
        /// 成交总手数
        /// </summary>
        public ulong Volumn { get; set; }

        /// <summary>
        /// 当前手数
        /// </summary>
        public uint Hand { get; set; }

        /// <summary>
        /// 最新价
        /// </summary>
        private double _NewPrice;
        public double NewPrice
        {
            get { return _NewPrice; }
            set
            {
                _NewPrice = value;
                OnPropertyChanged("NewPrice");
            }
        }

        /// <summary>
        /// 昨结算
        /// </summary>
        public double PrevSettlementPrice { get; set; }

        /// <summary>
        /// 昨收
        /// </summary>
        public double PrevClose { get; set; }

        /// <summary>
        /// 昨持仓
        /// </summary>
        public ulong PrevPosition { get; set; }

        /// <summary>
        /// 开盘价
        /// </summary>
        public double OpenPrice { get; set; }

        /// <summary>
        /// 最高价
        /// </summary>
        public double MaxPrice { get; set; }

        /// <summary>
        /// 最低价
        /// </summary>
        public double MinPrice { get; set; }

        /// <summary>
        /// 涨停价
        /// </summary>
        public double UpperLimitPrice { get; set; }

        /// <summary>
        /// 跌停价
        /// </summary>
        public double LowerLimitPrice { get; set; }

        ///持仓量
        public ulong Position { get; set; }

        /// <summary>
        /// 收盘价
        /// </summary>
        public double ClosePrice { get; set; }

        /// <summary>
        /// 结算价
        /// </summary>
        public double SettlmentPrice { get; set; }

        /// <summary>
        /// 均价
        /// </summary>
        public double AvgPrice { get; set; }

        /// <summary>
        /// 成交额
        /// </summary>
        public double Sum { get; set; }

        /// <summary>
        /// 这次最新数据的时间
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 买价
        /// </summary>
        private double[] _BidPrice = new double[10];
        public double[] BidPrice 
        {
            get { return _BidPrice; }
            set
            {
                _BidPrice = value;
                OnPropertyChanged("BidPrice");
            }
        }

        /// <summary>
        /// 买手
        /// </summary>
        public uint[] BidHand = new uint[10];
        /// <summary>
        /// 卖价
        /// </summary>
        private double[] _AskPrice = new double[10];
        public double[] AskPrice
        {
            get { return _AskPrice; }
            set
            {
                _AskPrice = value;
                OnPropertyChanged("AskPrice");
            }
        }

        /// <summary>
        /// 卖手
        /// </summary>
        public uint[] AskHand = new uint[10];

        /// <summary>
        /// 基准中间价
        /// </summary>
        //private double _AvgBasePrice;
        /// <summary>
        /// 基准中间价
        /// </summary>
        public double MarketBasePrice
        {
            get
            {
                if (_BidPrice[0] > 0 && _AskPrice[0] > 0 && AvgPrice > 0)
                {
                    if (AvgPrice > 0)
                    {
                        return 0.4 * (_BidPrice[0] + _AskPrice[0]) / 2 + 0.6 * (AvgPrice + _NewPrice) / 2;
                    }
                    else
                    {
                        return 0.4 * (_BidPrice[0] + _AskPrice[0]) / 2 + 0.6 * _NewPrice;
                    }
                }
                else
                {
                    return _NewPrice;
                }
            }
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

        public void CopyProperties(RealData realData)
        {
            this.Position = realData.Position;
            this.BidHand = realData.BidHand;
            this.BidPrice = realData.BidPrice;
            this.AskHand = realData.AskHand;
            this.AskPrice = realData.AskPrice;
            this.MinPrice = realData.MinPrice;
            //this.HYCS = realData.HYCS;
            this.Sum = realData.Sum;
            //this.ICurrentSum = realData.ICurrentSum;
            this.SettlmentPrice = realData.SettlmentPrice;
            this.PrevSettlementPrice = realData.PrevSettlementPrice;
            this.MaxPrice = realData.MaxPrice;
            this.CodeInfo = realData.CodeInfo;
            this.Hand = realData.Hand;
            //this.I64Outside = realData.I64Outside;
            this.ClosePrice = realData.ClosePrice;
            //this.IDealNumber = realData.IDealNumber;
            this.UpperLimitPrice = realData.UpperLimitPrice;
            this.LowerLimitPrice = realData.LowerLimitPrice;
            this.NewPrice = realData.NewPrice;
            //this.IsFirstData = realData.IsFirstData;
            //this.LogMessage = realData.LogMessage;
            //this.Name = realData.Name;
            this.OpenPrice = realData.OpenPrice;
            //this.NTime = realData.NTime;
            this.PrevClose = realData.PrevClose;
            //this.UiTime = realData.UiTime;
            //this.UsUpdateNumber = realData.UsUpdateNumber;
            this.Volumn = realData.Volumn;
        }

        public bool UpdateRealProperties(DisplayRealData displayData)
        {
            try
            {
                this.CodeInfo.Code = displayData.Code;
                this.CodeInfo.Name = displayData.Name;
                this.Position = displayData.ChiCangLiang;
                this.BidHand[0] = displayData.StBuyCount;
                this.BidPrice[0] = displayData.StBuyPrice;
                this.AskHand[0] = displayData.StSellCount;
                this.AskPrice[0] = displayData.StSellPrice;
                this.LowerLimitPrice = displayData.DownStopPrice;
                this.Sum = displayData.I64Sum;

                this.SettlmentPrice = displayData.ISettlementPrice;
                this.PrevSettlementPrice = displayData.PrevSettleMent;
                this.UpperLimitPrice = displayData.UpStopPrice;
                this.Hand = displayData.CurrentHand;

                this.ClosePrice = displayData.IClose;

                this.MaxPrice = displayData.IMaxPrice;
                this.MinPrice = displayData.IMinPrice;
                this.NewPrice = displayData.INewPrice;

                this.OpenPrice = displayData.Open;

                this.PrevClose = displayData.PrevClose;

                this.Volumn = displayData.Volumn;
                this.CodeInfo.ExchCode = CodeSetManager.ExNameToCtp(displayData.Market);
                this.PrevPosition = displayData.PreChicang;
                this.PrevClose = displayData.PrevClose;
                this.UpdateTime = displayData.UpdateTime;
                return true;
            }
            catch (Exception ex)
            {
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error("exception: " + ex.StackTrace);
                return false;
            }
        }
    }
}
