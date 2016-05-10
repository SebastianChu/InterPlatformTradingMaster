using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingMaster
{
    /// <summary>
    /// 快期界面的一条持仓记录
    /// </summary>
    public class Q7PosInfoDetail : INotifyPropertyChanged
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
        /// 成交编号
        /// </summary>
        private string _ExecID;
        /// <summary>
        /// 成交编号
        /// </summary>
        public string ExecID
        {
            get { return _ExecID; }
            set
            {
                _ExecID = value;
                OnPropertyChanged("ExecID");
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
        /// 成交量
        /// </summary>
        private int _TradeHandCount;
        /// <summary>
        /// 成交量
        /// </summary>
        public int TradeHandCount
        {
            get { return _TradeHandCount; }
            set { _TradeHandCount = value; OnPropertyChanged("TradeHandCount"); }
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
        /// 持仓类型
        /// </summary>
        public string _PositionType;
        /// <summary>
        /// 持仓类型
        /// </summary>
        public string PositionType
        {
            get { return _PositionType; }
            set { _PositionType = value; OnPropertyChanged("PositionType"); }
        }


        /// <summary>
        /// 组合类型
        /// </summary>
        public string _LegType;
        /// <summary>
        /// 组合类型
        /// </summary>
        public string LegType
        {
            get { return _LegType; }
            set { _LegType = value; OnPropertyChanged("LegType"); }
        }

        /// <summary>
        /// 逐笔浮盈
        /// </summary>
        public string _Zbfy;
        /// <summary>
        /// 逐笔浮盈
        /// </summary>
        public string Zbfy
        {
            get { return _Zbfy; }
            set { _Zbfy = value; OnPropertyChanged("ZhuBiFuYing"); }
        }

        /// <summary>
        /// 盯市浮盈
        /// </summary>
        public string _Dsfy;
        /// <summary>
        /// 盯市浮盈
        /// </summary>
        public string DSfy
        {
            get { return _Dsfy; }
            set { _Dsfy = value; OnPropertyChanged("DingShiFuYing"); }
        }

        /// <summary>
        /// 单笔盈亏
        /// </summary>
        public string _Dbyk;
        /// <summary>
        /// 单笔盈亏
        /// </summary>
        public string Dbyk
        {
            get { return _Dbyk; }
            set { _Dbyk = value; OnPropertyChanged("DanBiYingKui"); }
        }

        /// <summary>
        /// 总交易所保证金 8014
        /// </summary>
        private string _TotalExMarginAmt;
        /// <summary>
        /// 总交易所保证金 8014
        /// </summary>
        public string TotalExMarginAmt
        {
            get { return _TotalExMarginAmt; }
            set { _TotalExMarginAmt = value; OnPropertyChanged("TotalExMarginAmt"); }
        }

        /// <summary>
        /// 占用保证金 8048
        /// </summary>
        private string _OccupyMarginAmt;
        /// <summary>
        /// 占用保证金 8048
        /// </summary>
        public string OccupyMarginAmt
        {
            get { return _OccupyMarginAmt; }
            set
            {
                _OccupyMarginAmt = value;
                OnPropertyChanged("OccupyMarginAmt");
                OnPropertyChanged("OccupyMarginAmtDouble");
            }
        }

        /// <summary>
        /// 持仓浮动盈亏 8085
        /// </summary>
        private string _FloatProfit;
        /// <summary>
        /// 持仓浮动盈亏 8085
        /// </summary>
        public string FloatProfit
        {
            get { return _FloatProfit; }
            set { _FloatProfit = value; OnPropertyChanged("FloatProfit"); }
        }
        /// <summary>
        /// 平仓盈亏 8086
        /// </summary>
        private string _CloseProfit;
        /// <summary>
        /// 平仓盈亏 8086
        /// </summary>
        public string CloseProfit
        {
            get { return _CloseProfit; }
            set { _CloseProfit = value; OnPropertyChanged("CloseProfit"); }
        }

        /// <summary>
        /// 平仓均价 8966
        /// </summary>
        private string _CloseAvgPrice;
        /// <summary>
        /// 平仓均价 8966
        /// </summary>
        public string CloseAvgPrice
        {
            get { return _CloseAvgPrice; }
            set { _CloseAvgPrice = value; OnPropertyChanged("CloseAvgPrice"); }
        }

        /// <summary>
        /// 维持保证金 8967
        /// </summary>
        private string _MaintainMarginAmt;
        /// <summary>
        /// 维持保证金 8967
        /// </summary>
        public string MaintainMarginAmt
        {
            get { return _MaintainMarginAmt; }
            set { _MaintainMarginAmt = value; OnPropertyChanged("MaintainMarginAmt"); }
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

        /// <summary>
        /// 持仓盈亏
        /// </summary>
        private double _Ccyk;
        /// <summary>
        /// 持仓盈亏（盯市）
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
        /// 浮动盈亏
        /// </summary>
        public double Fdyk
        {
            get { return _Fdyk; }
            set { _Fdyk = value; OnPropertyChanged("Fdyk"); }
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
        /// 期权市值
        /// </summary>
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

        /// <summary>
        /// 权利金收付
        /// </summary>
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

        public double OccupyMarginAmtDouble
        {
            get
            {
                double doubleValue = 0.00;
                double.TryParse(OccupyMarginAmt, out doubleValue);
                return doubleValue;
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

        public Q7PosInfoDetail Copy()
        {
            Q7PosInfoDetail ret = (Q7PosInfoDetail)this.MemberwiseClone();
            return ret;
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

        public override string ToString()
        {
            string ret = "";
            ret += "代码:" + Code;
            ret += " " + BuySell.ToString();
            ret += " 开仓均价:" + AvgPx.ToString();
            ret += " 持仓数量:" + TradeHandCount.ToString();
            ret += " 持仓类型:" + PositionType;
            ret += " 成交编号:" + ExecID;
            return ret;
        }

        public static int CompareByExecID(Q7PosInfoDetail o1, Q7PosInfoDetail o2)
        {
            if (o1 == null && o2 == null) return 0;
            if (o1 == null) return -1;
            if (o2 == null) return 1;
            if (o1.ExecID == null || o2.ExecID == null) return 0;
            if (o1.ExecID.CompareTo(o2.ExecID) < 0) return 1;
            if (o1.ExecID.CompareTo(o2.ExecID) == 0)
            {
                if (o1.ExecID.CompareTo(o2.ExecID) == 1)
                {
                    return -1;
                }
                else if (o1.ExecID.CompareTo(o2.ExecID) == -1)
                {
                    return 1;
                }
                else if (o1.ExecID.CompareTo(o2.ExecID) == 0)
                {
                    return 0;
                }
            }
            return -1;
        }
    }
}
