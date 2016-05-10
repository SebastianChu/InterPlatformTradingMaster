using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingMaster
{
    /// <summary>
    /// 快期界面的持仓合计
    /// </summary>
    public class Q7PosInfoTotal : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

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
        /// 合约
        /// </summary>
        private string _Code;
        /// <summary>
        /// 合约
        /// </summary>
        public string Code
        {
            get { return _Code; }
            set { _Code = value; OnPropertyChanged("Code"); }
        }

        /// <summary>
        /// 合约名
        /// </summary>
        private string _Name;
        /// <summary>
        /// 合约名
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged("Name"); }
        }

        /// <summary>
        /// 合约类型
        /// </summary>
        private string _ProductType;
        /// <summary>
        /// 合约类型
        /// </summary>
        public string ProductType
        {
            get { return _ProductType; }
            set { _ProductType = value; OnPropertyChanged("ProductType"); }
        }

        /// <summary>
        /// 买卖
        /// </summary>
        private string _BuySell;
        /// <summary>
        /// 买卖
        /// </summary>
        public string BuySell
        {
            get { return _BuySell; }
            set { _BuySell = value; OnPropertyChanged("BuySell"); }
        }

        /// <summary>
        /// 总持仓
        /// </summary>
        private int _TotalPosition;
        /// <summary>
        /// 总持仓
        /// </summary>
        public int TotalPosition
        {
            get { return _TotalPosition; }
            set { _TotalPosition = value; OnPropertyChanged("TotalPosition"); }
        }

        /// <summary>
        /// 昨仓
        /// </summary>
        private int _YesterdayPosition;
        /// <summary>
        /// 昨仓
        /// </summary>
        public int YesterdayPosition
        {
            get { return _YesterdayPosition; }
            set { _YesterdayPosition = value; OnPropertyChanged("YesterdayPosition"); }
        }

        /// <summary>
        /// 今仓
        /// </summary>
        private int _TodayPosition;
        /// <summary>
        /// 今仓
        /// </summary>
        public int TodayPosition
        {
            get { return _TodayPosition; }
            set { _TodayPosition = value; OnPropertyChanged("TodayPosition"); }
        }

        /// <summary>
        /// 未成交的平仓数量
        /// </summary>
        private int _FreezeCount;
        /// <summary>
        /// 未成交的平仓数量
        /// </summary>
        public int FreezeCount
        {
            get { return _FreezeCount; }
            set { _FreezeCount = value; OnPropertyChanged("FreezeCount"); }
        }

        /// <summary>
        /// 可平仓数量
        /// </summary>
        private int _CanCloseCount;
        /// <summary>
        /// 可平仓数量
        /// </summary>
        public int CanCloseCount
        {
            get { return _CanCloseCount; }
            set { _CanCloseCount = value; OnPropertyChanged("CanCloseCount"); }
        }

        /// <summary>
        /// 开仓均价
        /// </summary>
        private double _AvgPx;
        /// <summary>
        /// 开仓均价
        /// </summary>
        public double AvgPx
        {
            get { return _AvgPx; }
            set { _AvgPx = value; OnPropertyChanged("AvgPx"); }
        }
        /// <summary>
        /// 持仓均价
        /// </summary>
        private double _AvgPositionPrice;
        /// <summary>
        /// 持仓均价
        /// 昨仓* 做结算 + 今仓 * 今成交均价
        /// </summary>
        public double AvgPositionPrice
        {
            get { return _AvgPositionPrice; }
            set { _AvgPositionPrice = value; OnPropertyChanged("AvgPositionPrice"); }
        }

        /// <summary>
        /// 持仓盈亏
        /// </summary>
        private double _Ccyk;
        /// <summary>
        /// 持仓盈亏（盯市），根据最新价和持仓均价计算
        /// </summary>
        public double Ccyk
        {
            get { return _Ccyk; }
            set { _Ccyk = value; OnPropertyChanged("Ccyk"); }
        }

        /// <summary>
        /// 浮动盈亏
        /// </summary>
        private double _Fdyk;
        /// <summary>
        /// 浮动盈亏，根据开仓价格和最新价计算
        /// </summary>
        public double Fdyk
        {
            get { return _Fdyk; }
            set { _Fdyk = value; OnPropertyChanged("Fdyk"); }
        }

        /// <summary>
        /// 占用保证金 8048
        /// </summary>
        private double _OccupyMarginAmt;
        /// <summary>
        /// 占用保证金 8048
        /// </summary>
        public double OccupyMarginAmt
        {
            get { return _OccupyMarginAmt; }
            set { _OccupyMarginAmt = value; OnPropertyChanged("OccupyMarginAmt"); }
        }

        /// <summary>
        /// 投保
        /// </summary>
        public string _Hedge;
        /// <summary>
        /// 投保
        /// </summary>
        public string Hedge
        {
            get { return _Hedge; }
            set { _Hedge = value; OnPropertyChanged("Hedge"); }
        }

        /// <summary>
        /// 交易所
        /// </summary>
        private string _Exchange;
        /// <summary>
        /// 交易所
        /// </summary>
        public string Exchange
        {
            get { return _Exchange; }
            set { _Exchange = value; OnPropertyChanged("Exchange"); }
        }

        /// <summary>
        /// 昨仓的持仓均价
        /// </summary>
        private double _YesterdayOpenAvgPx;
        /// <summary>
        /// 昨仓的持仓均价
        /// </summary>
        public double YesterdayOpenAvgPx
        {
            get { return _YesterdayOpenAvgPx; }
            set { _YesterdayOpenAvgPx = value; OnPropertyChanged("YesterdayOpenAvgPx"); }
        }


        /// <summary>
        /// 今仓的持仓均价
        /// </summary>
        private double _TodayOpenAvgPx;
        /// <summary>
        /// 今仓的持仓均价
        /// </summary>
        public double TodayOpenAvgPx
        {
            get { return _TodayOpenAvgPx; }
            set { _TodayOpenAvgPx = value; OnPropertyChanged("TodayOpenAvgPx"); }
        }

        private Double _PrevSettleMent;
        /// <summary>
        /// 昨结算
        /// </summary>
        public Double PrevSettleMent
        {
            get
            {
                return _PrevSettleMent;
            }
            set
            {
                _PrevSettleMent = value;
                OnPropertyChanged("PrevSettleMent");
            }
        }

        private double _INewPrice;			//最新价

        public double INewPrice
        {
            get { return _INewPrice; }
            set
            {
                _INewPrice = value;
                OnPropertyChanged("INewPrice");
            }
        }

        private double _OptionMarketCap;
        /// <summary>
        /// 期权市值
        /// </summary>
        public double OptionMarketCap
        {
            get
            {
                return _OptionMarketCap;
            }
            set
            {
                _OptionMarketCap = value;
                OnPropertyChanged("OptionMarketCap");
            }
        }

        private double _Premium;
        /// <summary>
        /// 权利金收付
        /// </summary>
        public double Premium
        {
            get
            {
                return _Premium;// base.AvgPx * base.TradeHandCount;
            }
            set
            {
                _Premium = value;
                OnPropertyChanged("Premium");
            }
        }

        /// <summary>
        /// 期权盈亏
        /// </summary>
        private double _OptionProfit;
        /// <summary>
        /// 期权盈亏
        /// </summary>
        public double OptionProfit
        {
            get
            {
                return _OptionProfit;
            }
            set
            {
                _OptionProfit = value;
                OnPropertyChanged("OptionProfit");
            }
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

        public Q7PosInfoTotal Copy()
        {
            Q7PosInfoTotal ret = (Q7PosInfoTotal)this.MemberwiseClone();
            return ret;
        }

        public static int CompareByCode(Q7PosInfoTotal o1, Q7PosInfoTotal o2)
        {
            if (o1 == null && o2 == null) return 0;
            if (o1 == null) return -1;
            if (o2 == null) return 1;

            if (o1._Code.Length > 6  && o2._Code.Length <= 6) 
            {
                return 1;
            }
            else if (o1._Code.Length <= 6 && o2._Code.Length > 6)
            {
                return -1;
            }
            else
            {
                if (o1._Code.CompareTo(o2._Code) == 1)
                {
                    return 1;
                }
                else if (o1._Code.CompareTo(o2._Code) == -1)
                {
                    return -1;
                }
                else if (o1._Code.CompareTo(o2._Code) == 0)
                {
                    if (o1._TotalPosition.CompareTo(o2._TotalPosition) > 0) return 1;
                    if (o1._TotalPosition.CompareTo(o2._TotalPosition) < 0) return -1;
                    if (o1._TotalPosition.CompareTo(o2._TotalPosition) == 0) return 0;
                }
            }
            return 1;
        }
    }
}
