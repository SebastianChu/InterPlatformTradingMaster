using System.ComponentModel;

namespace TradingMaster
{
    /// <summary>
    /// 资金信息
    /// </summary>
    public class CapitalInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
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
        /// 资金账号
        /// </summary>
        private string _CapitalID;
        /// <summary>
        /// 资金账号
        /// </summary>
        public string CapitalID
        {
            get { return _CapitalID; }
            set { _CapitalID = value; OnPropertyChanged("CapitalID"); }
        }


        /// <summary>
        /// 昨可用
        /// </summary>
        private double _YesterdayAvailabe;
        /// <summary>
        /// 昨可用
        /// </summary>
        public double YesterdayAvailabe
        {
            get { return _YesterdayAvailabe; }
            set { _YesterdayAvailabe = value; OnPropertyChanged("YesterdayAvailabe"); }
        }




        /// <summary>
        /// 上次结算准备金
        /// </summary>
        private double _YesterdayEquity;
        /// <summary>
        /// 上次结算准备金
        /// </summary>
        public double YesterdayEquity
        {
            get { return _YesterdayEquity; }
            set { _YesterdayEquity = value; OnPropertyChanged("YesterdayEquity"); }
        }

        /// <summary>
        /// 上日结算准备金
        /// </summary>
        private double _YesterdayBalance;
        /// <summary>
        /// 上日结算准备金
        /// </summary>
        public double YesterdayBalance
        {
            get { return _YesterdayBalance; }
            set { _YesterdayBalance = value; OnPropertyChanged("YesterdayBalance"); }
        }


        /// <summary>
        /// 入金
        /// </summary>
        private double _InMoney;
        /// <summary>
        /// 入金
        /// </summary>
        public double InMoney
        {
            get { return _InMoney; }
            set { _InMoney = value; OnPropertyChanged("InMoney"); }
        }

        /// <summary>
        /// 出金
        /// </summary>
        private double _OutMoney;
        /// <summary>
        /// 出金
        /// </summary>
        public double OutMoney
        {
            get { return _OutMoney; }
            set { _OutMoney = value; OnPropertyChanged("OutMoney"); }
        }

        /// <summary>
        /// 信用额度
        /// </summary>
        private double _Credit;
        /// <summary>
        /// 信用额度
        /// </summary>
        public double Credit
        {
            get { return _Credit; }
            set { _Credit = value; OnPropertyChanged("Credit"); }
        }

        /// <summary>
        /// 手续费
        /// </summary>
        private double _Charge;
        /// <summary>
        /// 手续费
        /// </summary>
        public double Charge
        {
            get { return _Charge; }
            set { _Charge = value; OnPropertyChanged("Charge"); }
        }

        /// <summary>
        /// 今结存
        /// </summary>
        private double _TodayBalance;
        /// <summary>
        /// 今结存
        /// </summary>
        public double TodayBalance
        {
            get { return _TodayBalance; }
            set { _TodayBalance = value; OnPropertyChanged("TodayBalance"); }
        }

        /// <summary>
        /// 今可用
        /// </summary>
        private double _TodayAvailable;
        /// <summary>
        /// 今可用
        /// </summary>
        public double TodayAvailable
        {
            get { return _TodayAvailable; }
            set { _TodayAvailable = value; OnPropertyChanged("TodayAvailable"); }
        }

        /// <summary>
        /// 今权益
        /// </summary>
        private double _TodayEquity;
        /// <summary>
        /// 今权益
        /// </summary>
        public double TodayEquity
        {
            get { return _TodayEquity; }
            set { _TodayEquity = value; OnPropertyChanged("TodayEquity"); }
        }
        /// <summary>
        /// 冻结资金
        /// </summary>
        private double _Frozen;
        /// <summary>
        /// 冻结资金
        /// </summary>
        public double Frozen
        {
            get { return _Frozen; }
            set { _Frozen = value; OnPropertyChanged("Frozen"); }
        }

        /// <summary>
        /// 冻结权利金
        /// </summary>
        private double _FrozenRoyalty;
        /// <summary>
        /// 冻结权利金
        /// </summary>
        public double FrozenRoyalty
        {
            get { return _FrozenRoyalty; }
            set { _FrozenRoyalty = value; OnPropertyChanged("FrozenRoyalty"); }
        }

        /// <summary>
        /// 冻结保证金
        /// </summary>
        private double _FrozenMargin;
        /// <summary>
        /// 冻结保证金
        /// </summary>
        public double FrozenMargin
        {
            get { return _FrozenMargin; }
            set { _FrozenMargin = value; OnPropertyChanged("FrozenMargin"); }
        }

        /// <summary>
        /// 占用保证金
        /// </summary>
        private double _OccupyMarginAmt;
        /// <summary>
        /// 占用保证金
        /// </summary>
        public double OccupyMarginAmt
        {
            get { return _OccupyMarginAmt; }
            set { _OccupyMarginAmt = value; OnPropertyChanged("OccupyMarginAmt"); }
        }

        /// <summary>
        /// 投资者交割保证金
        /// </summary>
        private double _DeliveryMargin;
        /// <summary>
        /// 投资者交割保证金
        /// </summary>
        public double DeliveryMargin
        {
            get { return _DeliveryMargin; }
            set { _DeliveryMargin = value; OnPropertyChanged("DeliveryMargin"); }
        }

        /// <summary>
        /// 质押金额
        /// </summary>
        private double _Mortgage;
        /// <summary>
        /// 质押金额
        /// </summary>
        public double Mortgage
        {
            get { return _Mortgage; }
            set { _Mortgage = value; OnPropertyChanged("Mortgage"); }
        }

        /// <summary>
        /// 前信用额度
        /// </summary>
        private double _LastCredit;
        /// <summary>
        /// 前信用额度
        /// </summary>
        public double LastCredit
        {
            get { return _LastCredit; }
            set { _LastCredit = value; }
        }

        /// <summary>
        /// 前质押金额
        /// </summary>
        private double _LastMortage;
        /// <summary>
        /// 前质押金额
        /// </summary>
        public double LastMortage
        {
            get { return _LastMortage; }
            set { _LastMortage = value; }
        }


        /// <summary>
        /// 保证金
        /// </summary>
        private double _Bond;
        /// <summary>
        /// 保证金
        /// </summary>
        public double Bond
        {
            get { return _Bond; }
            set { _Bond = value; OnPropertyChanged("Bond"); }
        }

        /// <summary>
        /// 逐笔浮盈
        /// </summary>
        private double _Zbfy;
        /// <summary>
        /// 逐笔浮盈
        /// </summary>
        public double Zbfy
        {
            get { return _Zbfy; }
            set { _Zbfy = value; OnPropertyChanged("Zbfy"); }
        }

        /// <summary>
        /// 风险率
        /// </summary>
        private double _RiskRatio;
        /// <summary>
        /// 风险率
        /// </summary>
        public double RiskRatio
        {
            get { return _RiskRatio; }
            set { _RiskRatio = value; OnPropertyChanged("RiskRatio"); }
        }


        /// <summary>
        /// 逐笔平盈
        /// </summary>
        private double _Zbpy;
        /// <summary>
        /// 逐笔平盈
        /// </summary>
        public double Zbpy
        {
            get { return _Zbpy; }
            set { _Zbpy = value; OnPropertyChanged("Zbpy"); }
        }


        /// <summary>
        /// 逐笔总盈亏
        /// </summary>
        private double _Zbzyk;
        /// <summary>
        /// 逐笔总盈亏
        /// </summary>
        public double Zbzyk
        {
            get { return _Zbzyk; }
            set { _Zbzyk = value; OnPropertyChanged("Zbzyk"); }
        }


        /// <summary>
        /// 盈利率
        /// </summary>
        private double _ProfitRatio;
        /// <summary>
        /// 盈利率
        /// </summary>
        public double ProfitRatio
        {
            get { return _ProfitRatio; }
            set { _ProfitRatio = value; OnPropertyChanged("ProfitRatio"); }
        }

        /// <summary>
        /// 盯市浮盈
        /// </summary>
        private double _Dsfy;
        /// <summary>
        /// 盯市浮盈
        /// </summary>
        public double Dsfy
        {
            get { return _Dsfy; }
            set { _Dsfy = value; OnPropertyChanged("Dsfy"); }
        }


        /// <summary>
        /// 盯市平盈
        /// </summary>
        private double _Dspy;
        /// <summary>
        /// 盯市平盈
        /// </summary>
        public double Dspy
        {
            get { return _Dspy; }
            set { _Dspy = value; OnPropertyChanged("Dspy"); }
        }


        /// <summary>
        /// 盯市总盈亏
        /// </summary>
        private double _Dszyk;
        /// <summary>
        /// 盯市总盈亏
        /// </summary>
        public double Dszyk
        {
            get { return _Dszyk; }
            set { _Dszyk = value; OnPropertyChanged("Dszyk"); }
        }


        /// <summary>
        /// 总交易所保证金
        /// </summary>
        private double _TotalExchangeBond;
        /// <summary>
        /// 总交易所保证金
        /// </summary>
        public double TotalExchangeBond
        {
            get { return _TotalExchangeBond; }
            set { _TotalExchangeBond = value; OnPropertyChanged("TotalExchangeBond"); }
        }

        /// <summary>
        /// 冻结手续费
        /// </summary>
        private double _FrozenCommision;
        /// <summary>
        /// 冻结手续费
        /// </summary>
        public double FrozenCommision
        {
            get { return _FrozenCommision; }
            set { _FrozenCommision = value; OnPropertyChanged("FrozenCommision"); }
        }


        /// <summary>
        /// 买保证金
        /// </summary>
        private double _BuyMarginAmt;
        /// <summary>
        /// 买保证金
        /// </summary>
        public double BuyMarginAmt
        {
            get { return _BuyMarginAmt; }
            set { _BuyMarginAmt = value; OnPropertyChanged("BuyMarginAmt"); }
        }


        /// <summary>
        /// 持仓浮动盈亏;	(盯市)
        /// </summary>
        private double _FloatProfit;
        /// <summary>
        /// 持仓浮动盈亏;	(盯市)
        /// </summary>
        public double FloatProfit
        {
            get { return _FloatProfit; }
            set { _FloatProfit = value; OnPropertyChanged("FloatProfit"); }
        }


        /// <summary>
        /// 平仓盈亏;	(盯市)
        /// </summary>
        private double _CloseProfit;
        /// <summary>
        /// 平仓盈亏;	(盯市)
        /// </summary>
        public double CloseProfit
        {
            get { return _CloseProfit; }
            set { _CloseProfit = value; OnPropertyChanged("CloseProfit"); }
        }

        /// <summary>
        /// 可取资金
        /// </summary>
        private double _Fetchable;
        /// <summary>
        /// 可取资金
        /// </summary>
        public double Fetchable
        {
            get { return _Fetchable; }
            set { _Fetchable = value; OnPropertyChanged("Fetchable"); }
        }

        /// <summary>
        /// 货币
        /// </summary>
        private string _Currency;
        /// <summary>
        /// 货币
        /// </summary>
        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; OnPropertyChanged("Currency"); }
        }

        /// <summary>
        /// 维持保证金
        /// </summary>
        private double _MaintainMargin;
        /// <summary>
        /// 维持保证金
        /// </summary>
        public double MaintainMargin
        {
            get { return _MaintainMargin; }
            set { _MaintainMargin = value; OnPropertyChanged("MaintainMargin"); }
        }

        /// <summary>
        /// 权益
        /// </summary>
        private double _OwnAmt;
        /// <summary>
        /// 权益
        /// </summary>
        public double OwnAmt
        {
            get { return _OwnAmt; }
            set { _OwnAmt = value; OnPropertyChanged("OwnAmt"); }
        }

        /// <summary>
        /// 保证金水平
        /// </summary>
        private double _MarginLevel;

        public double MarginLevel
        {
            get { return _MarginLevel; }
            set
            {
                _MarginLevel = value;
                OnPropertyChanged("MarginLevel");
            }
        }

        /// <summary>
        /// 保底资金
        /// </summary>
        private double _Reserve;

        public double Reserve
        {
            get
            {
                return _Reserve;

            }
            set { _Reserve = value; OnPropertyChanged("Reserve"); }
        }

        /// <summary>
        /// 权利金
        /// </summary>
        private double _Royalty;

        public double Royalty
        {
            get
            {
                return _Royalty;

            }
            set { _Royalty = value; OnPropertyChanged("Royalty"); }
        }

        /// <summary>
        /// 期权平仓盈亏
        /// </summary>
        private double _OptionCloseProfit;

        public double OptionCloseProfit
        {
            get
            {
                return _OptionCloseProfit;

            }
            set { _OptionCloseProfit = value; OnPropertyChanged("OptionCloseProfit"); }
        }

        /// <summary>
        /// 期权市值
        /// </summary>
        private double _OptionValue;

        public double OptionValue
        {
            get
            {
                return _OptionValue;

            }
            set { _OptionValue = value; OnPropertyChanged("OptionValue"); }
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

        public CapitalInfo()
        {
            _CapitalID = "";
        }

        public override string ToString()
        {
            string ret = "";
            ret += "资金账号:" + _CapitalID.ToString() + "\n";
            ret += "昨可用:" + _YesterdayAvailabe.ToString() + "\n";
            ret += "昨权益:" + _YesterdayEquity.ToString() + "\n";
            ret += "昨结存:" + _YesterdayBalance.ToString() + "\n";
            ret += "入金:" + _InMoney.ToString() + "\n";
            ret += "出金:" + _OutMoney.ToString() + "\n";
            ret += "手续费:" + _Charge.ToString() + "\n";
            ret += "今结存:" + _TodayBalance.ToString() + "\n";
            ret += "今可用:" + _TodayAvailable.ToString() + "\n";
            ret += "今权益:" + _TodayEquity.ToString() + "\n";
            ret += "冻结资金:" + _Frozen.ToString() + "\n";
            ret += "保证金:" + _Bond.ToString() + "\n";
            ret += "逐笔浮盈:" + _Zbfy.ToString() + "\n";
            ret += "风险率:" + _RiskRatio.ToString() + "\n";
            ret += "逐笔平盈:" + _Zbpy.ToString() + "\n";
            ret += "逐笔总盈亏:" + _Zbzyk.ToString() + "\n";
            ret += "盈利率:" + _ProfitRatio.ToString() + "\n";
            ret += "盯市浮盈:" + _Dsfy.ToString() + "\n";
            ret += "盯市平盈:" + _Dspy.ToString() + "\n";
            ret += "盯市总盈亏:" + _Dszyk.ToString() + "\n";
            ret += "总交易所保证金:" + _TotalExchangeBond.ToString() + "\n";
            ret += "冻结手续费:" + _FrozenCommision.ToString() + "\n";
            ret += "浮动盈亏:" + _FloatProfit.ToString() + "\n";
            ret += "平仓盈亏:" + _CloseProfit.ToString() + "\n";
            ret += "可取资金:" + _Fetchable.ToString() + "\n";
            return ret;
        }
    }
}
