using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingMaster
{
    public class JYRealData : INotifyPropertyChanged
    {
        /// <summary>
        /// 资金账号
        /// </summary>
        private string _CapitalID;

        public string CapitalID
        {
            get { return _CapitalID; }
            set
            {
                _CapitalID = value;
                OnPropertyChanged("CapitalID");
            }
        }
        /// <summary>
        /// 昨可用
        /// </summary>
        private double _YesterdayAvailabe;

        public double YesterdayAvailabe
        {
            get { return _YesterdayAvailabe; }
            set { _YesterdayAvailabe = value; OnPropertyChanged("YesterdayAvailabe"); }
        }
        /// <summary>
        /// 昨权益
        /// </summary>
        private double _YesterdayEquity;

        public double YesterdayEquity
        {
            get { return _YesterdayEquity; }
            set
            {
                _YesterdayEquity = value;
                OnPropertyChanged("YesterdayEquity");
                OnPropertyChanged("StaticEquity");
            }
        }
        /// <summary>
        /// 昨结存
        /// </summary>
        private double _YesterdatBalance;

        public double YesterdatBalance
        {
            get { return _YesterdatBalance; }
            set { _YesterdatBalance = value; OnPropertyChanged("YesterdatBalance"); }
        }
        /// <summary>
        /// 入金
        /// </summary>
        private double _InMoney;

        public double InMoney
        {
            get { return _InMoney; }
            set
            {
                _InMoney = value; OnPropertyChanged("InMoney"); OnPropertyChanged("StaticEquity");
            }
        }
        /// <summary>
        /// 出金
        /// </summary>
        private double _OutMoney;

        public double OutMoney
        {
            get { return _OutMoney; }
            set
            {
                _OutMoney = value; OnPropertyChanged("OutMoney"); OnPropertyChanged("StaticEquity");
            }
        }
        /// <summary>
        /// 手续费
        /// </summary>
        private double _Charge;

        public double Charge
        {
            get { return _Charge; }
            set
            {
                _Charge = value; OnPropertyChanged("Charge"); OnPropertyChanged("DynamicEquity");
            }
        }
        /// <summary>
        /// 今结存
        /// </summary>
        private double _TodayBalance;

        public double TodayBalance
        {
            get { return _TodayBalance; }
            set { _TodayBalance = value; OnPropertyChanged("TodayBalance"); }
        }
        /// <summary>
        /// 今可用
        /// </summary>
        private double _TodayAvailable;

        public double TodayAvailable
        {
            get { return _TodayAvailable; }
            set { _TodayAvailable = value; OnPropertyChanged("TodayAvailable"); }
        }
        /// <summary>
        /// 今权益
        /// </summary>
        private double _TodayEquity;

        public double TodayEquity
        {
            get { return _TodayEquity; }
            set { _TodayEquity = value; OnPropertyChanged("TodayEquity"); }
        }
        /// <summary>
        /// 冻结资金
        /// </summary>
        private double _Frozen;

        public double Frozen
        {
            get { return _Frozen; }
            set { _Frozen = value; OnPropertyChanged("Frozen"); }
        }
        /// <summary>
        /// 保证金
        /// </summary>
        private double _Bond;

        public double Bond
        {
            get { return _Bond; }
            set
            {
                _Bond = value; OnPropertyChanged("Bond"); OnPropertyChanged("CaculatedAvailable");
            }
        }


        /// <summary>
        /// 逐笔浮盈
        /// </summary>
        private double _Zbfy;

        public double Zbfy
        {
            get { return _Zbfy; }
            set { _Zbfy = value; OnPropertyChanged("Zbfy"); }
        }
        /// <summary>
        /// 风险率
        /// </summary>
        private double _RiskRatio;

        public double RiskRatio
        {
            get
            {
                double de = DynamicEquity;
                if (de == 0)
                {
                    return 0;
                }
                else
                {
                    return Bond / de * 100;
                }
            }
            set { _RiskRatio = value; OnPropertyChanged("RiskRatio"); OnPropertyChanged("Bond"); OnPropertyChanged("DynamicEquity"); }
        }
        /// <summary>
        /// 逐笔平盈
        /// </summary>
        private double _Zbpy;

        public double Zbpy
        {
            get { return _Zbpy; }
            set { _Zbpy = value; OnPropertyChanged("Zbpy"); }
        }
        /// <summary>
        /// 逐笔总盈亏
        /// </summary>
        private double _Zbzyk;

        public double Zbzyk
        {
            get { return _Zbzyk; }
            set { _Zbzyk = value; OnPropertyChanged("Zbzyk"); }
        }
        /// <summary>
        /// 盈利率
        /// </summary>
        private double _ProfitRatio;

        public double ProfitRatio
        {
            get { return _ProfitRatio; }
            set { _ProfitRatio = value; OnPropertyChanged("ProfitRatio"); }
        }
        /// <summary>
        /// 盯市浮盈
        /// </summary>
        private double _Dsfy;

        public double Dsfy
        {
            get { return _Dsfy; }
            set
            {
                _Dsfy = value;
                OnPropertyChanged("Dsfy");
                OnPropertyChanged("Dszyk");
                OnPropertyChanged("DynamicEquity");
            }
        }
        /// <summary>
        /// 盯市平盈
        /// </summary>
        private double _Dspy;

        public double Dspy
        {
            get { return _Dspy; }
            set
            {
                _Dspy = value;
                OnPropertyChanged("Dspy");
                OnPropertyChanged("Dszyk");
                OnPropertyChanged("DynamicEquity");
            }
        }
        /// <summary>
        /// 盯市总盈亏
        /// </summary>
        //private double dszyk;

        public double Dszyk
        {
            get
            {
                return _Dspy + _Dsfy;
            }
            //set { dszyk = value; OnPropertyChanged("Dszyk"); }
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
        /// 总交易所保证金
        /// </summary>
        private double _TotalExchangeBond;

        public double TotalExchangeBond
        {
            get { return _TotalExchangeBond; }
            set { _TotalExchangeBond = value; }
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
            set
            {
                _FrozenMargin = value; OnPropertyChanged("FrozenMargin"); OnPropertyChanged("CaculatedAvailable");
            }
        }
        /// <summary>
        /// 冻结手续费
        /// </summary>
        private double _FrozenCommision;

        public double FrozenCommision
        {
            get { return _FrozenCommision; }
            set
            {
                _FrozenCommision = value;
                OnPropertyChanged("FrozenCommision");
                OnPropertyChanged("CaculatedAvailable");
            }
        }
        /// <summary>
        /// 买保证金
        /// </summary>
        private double _BuyMarginAmt;

        public double BuyMarginAmt
        {
            get { return _BuyMarginAmt; }
            set { _BuyMarginAmt = value; }
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
            set
            {
                _LastCredit = value;
                OnPropertyChanged("LastCredit");
                OnPropertyChanged("StaticEquity");
            }
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
            set
            {
                _Credit = value; OnPropertyChanged("Credit"); OnPropertyChanged("CaculatedAvailable");
            }
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
            set
            {
                _LastMortage = value;
                OnPropertyChanged("LastMortage");
                OnPropertyChanged("StaticEquity");
            }
        }

        /// <summary>
        /// 质押金额
        /// </summary>
        private double _Mortage;
        /// <summary>
        /// 质押金额
        /// </summary>
        public double Mortage
        {
            get { return _Mortage; }
            set
            {
                _Mortage = value; OnPropertyChanged("Mortage"); OnPropertyChanged("StaticEquity");
            }
        }

        /// 浮动盈亏
        /// </summary>
        private double _FloatProfit;

        public double FloatProfit
        {
            get { return _FloatProfit; }
            set
            {
                _FloatProfit = value;
                OnPropertyChanged("FloatProfit");
            }
        }
        /// <summary>
        /// 平仓盈亏
        /// </summary>
        private double _CloseProfit;

        public double CloseProfit
        {
            get { return _CloseProfit; }
            set
            {
                _CloseProfit = value;
                OnPropertyChanged("CloseProfit");
            }
        }

        private double _OptionProfit;
        /// <summary>
        /// 期权盈亏
        /// </summary>
        public double OptionProfit
        {
            get
            {
                return _OptionProfit;// optionMarketCap + royalty;
            }
            set
            {
                _OptionProfit = value;
                OnPropertyChanged("Royalty");
                OnPropertyChanged("OptionMarketCap");
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
        private double _Royalty;
        /// <summary>
        /// 权利金收付
        /// </summary>
        public double Royalty
        {
            get
            {
                return _Royalty;// base.AvgPx * base.TradeHandCount;
            }
            set
            {
                _Royalty = value;
                OnPropertyChanged("Royalty");
                OnPropertyChanged("DynamicEquity");
            }
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
        /// 可取资金
        /// </summary>
        private double _Fetchable;

        public double Fetchable
        {
            get
            {
                if (_Fetchable > CaculatedAvailable)
                {
                    return CaculatedAvailable;
                }
                else
                {
                    return _Fetchable;
                }
            }
            set { _Fetchable = value; OnPropertyChanged("Fetchable"); }
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
        /// 静态权益
        /// </summary>
        public double StaticEquity
        {
            get
            {
                //return YesterdayEquity - LastCredit - LastMortage + Mortage - OutMoney + InMoney;
                return _StaticEquity;
            }
            set { _StaticEquity = value; OnPropertyChanged("StaticEquity"); }
        }

        private double _StaticEquity;

        //   double staticEquity = jyRealData.YesterdayEquity - jyRealData.LastCredit - jyRealData.LastMortage
        //+ jyRealData.Mortage - jyRealData.OutMoney + jyRealData.InMoney;

        public double DynamicEquity
        {
            get { return StaticEquity + Dspy + Dsfy - Charge + Royalty; }
        }

        //double dynamicEquity = staticEquity + jyRealData.Dspy + jyRealData.Dsfy - jyRealData.Charge;

        /// <summary>
        /// 账户市值
        /// </summary>
        private double _AccountCap;
        /// <summary>
        /// 账户市值
        /// </summary>
        public double AccountCap
        {
            get
            {
                return DynamicEquity + _OptionMarketCap;
            }
            set 
            {
                _AccountCap = value; 
                OnPropertyChanged("AccountCap");
                OnPropertyChanged("DynamicEquity");
                OnPropertyChanged("OptionMarketCap");
            }
        }


        public double CaculatedAvailable
        {
            ///只有当持仓盈利的时候，不能用做可用。即浮盈不能开新仓，对于上海辖区。
            get { return DynamicEquity - Bond - FrozenMargin - FrozenCommision + Credit - (Dsfy > 0 ? Dsfy : 0) - FrozenRoyalty; }
        }
        //double fetchable = dynamicEquity - jyRealData.Bond - jyRealData.FrozenMargin - jyRealData.FrozenCommision + jyRealData.Credit;



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
                if (info == "StaticEquity")
                {
                    OnPropertyChanged("DynamicEquity");
                }
                else if (info == "DynamicEquity")
                {
                    OnPropertyChanged("CaculatedAvailable");
                    OnPropertyChanged("Fetchable");
                    OnPropertyChanged("RiskRatio");
                }
            }
        }

        public void Clear()
        {
            StaticEquity = 0;
            Dspy = 0;
            Dsfy = 0;
            Charge = 0;
            Bond = 0;
            Frozen = 0;
            FrozenMargin = 0;
            FrozenCommision = 0;
            Credit = 0;
            //RiskRatio = 0;
        }
    }
}
