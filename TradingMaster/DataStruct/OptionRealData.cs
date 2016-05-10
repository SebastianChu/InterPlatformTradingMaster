using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using TradingMaster.CodeSet;

namespace TradingMaster
{
    public class OptionRealData : INotifyPropertyChanged
    {
        private string _LogMessage;

        public string LogMessage
        {
            get { return _LogMessage; }
            set
            {
                _LogMessage = value;
                OnPropertyChanged("LogMessage");
            }
        }
        private string _Code;               //期货编码

        public string Code
        {
            get { return _Code; }
            set
            {
                _Code = value;
                OnPropertyChanged("Code");
            }
        }

        private double _ExecutePrice;			//行权价

        public double ExecutePrice
        {
            get { return _ExecutePrice; }
            set
            {
                _ExecutePrice = value;
                OnPropertyChanged("ExecutePrice");
            }
        }

        /// <summary>
        /// 交易所代码
        /// </summary>
        private string _ExchCode;
        public string ExchCode
        {
            get { return _ExchCode; }
            set { _ExchCode = value; }
        }

        private string _Market;               //交易所

        public string Market
        {
            get { return _Market; }
            set
            {
                _Market = value;
                OnPropertyChanged("Market");
            }
        }

        private string _Name;               //期货名称

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }
        private int _UiTime;				//更新时间

        public int UiTime
        {
            get { return _UiTime; }
            set
            {
                _UiTime = value;
                OnPropertyChanged("UiTime");
            }
        }

        private int _Hycs;			//合约乘数

        public int Hycs
        {
            get { return _Hycs; }
            set
            {
                _Hycs = value;
                OnPropertyChanged("Hycs");
                OnPropertyChanged("AvgPrice_C");
                OnPropertyChanged("AvgPrice_P");
            }
        }

        #region Call Option

        private string _Code_c;               //看涨期权编码

        public string Code_C
        {
            get { return _Code_c; }
            set
            {
                _Code_c = value;
                OnPropertyChanged("Code_C");
            }
        }


        private double _Open_c;				//今开盘

        public double Open_C
        {
            get { return _Open_c; }
            set
            {
                _Open_c = value;
                OnPropertyChanged("Open_C");
            }
        }

        private double _IMaxPrice_c;			//今最高价

        public double IMaxPrice_C
        {
            get { return _IMaxPrice_c; }
            set
            {
                _IMaxPrice_c = value;
                OnPropertyChanged("IMaxPrice_C");
            }
        }
        private double _IMinPrice_c;			//今最低价

        public double IMinPrice_C
        {
            get { return _IMinPrice_c; }
            set
            {
                _IMinPrice_c = value;
                OnPropertyChanged("IMinPrice_C");
            }
        }
        private double _IClose_c;				//今收盘

        public double IClose_C
        {
            get { return _IClose_c; }
            set
            {
                _IClose_c = value;
                OnPropertyChanged("IClose_C");
            }
        }
        private double _INewPrice_c;			//最新价

        public double INewPrice_C
        {
            get { return _INewPrice_c; }
            set
            {
                LastINewPrice_C = _INewPrice_c == 0.0 ? value : _INewPrice_c;
                _INewPrice_c = value;
                OnPropertyChanged("INewPrice_C");
                OnPropertyChanged("PriceFluctuation_C");
                OnPropertyChanged("PriceFluctuationFD_C");
            }
        }

        private double _LastiNewPrice_c;			//上一个最新价

        public double LastINewPrice_C
        {
            get { return _LastiNewPrice_c; }
            set
            {
                _LastiNewPrice_c = value;
                OnPropertyChanged("LastINewPrice_C");
            }
        }


        private UInt64 _ChiCangLiang_c;				//持仓量

        public UInt64 ChiCangLiang_C
        {
            get { return _ChiCangLiang_c; }
            set
            {
                _ChiCangLiang_c = value;
                OnPropertyChanged("ChiCangLiang_C");
                OnPropertyChanged("PositionFluctuation_C");
            }
        }
        private UInt64 _I64Sum_c;				//成交金额

        public UInt64 I64Sum_C
        {
            get { return _I64Sum_c; }
            set
            {
                _I64Sum_c = value;
                OnPropertyChanged("I64Sum_C");
                OnPropertyChanged("AvgPrice_C");
            }
        }
        private double _StBuyPrice_c;		//买十价

        public double StBuyPrice_C
        {
            get { return _StBuyPrice_c; }
            set
            {
                LastStBuyPrice_C = _StBuyPrice_c == 0.0? value: _StBuyPrice_c;

                _StBuyPrice_c = value;
                OnPropertyChanged("StBuyPrice_C");
            }
        }

        private double _LaststBuyPrice_c;		//上一个买十价

        public double LastStBuyPrice_C
        {
            get { return _LaststBuyPrice_c; }
            set
            {
                _LaststBuyPrice_c = value;
                OnPropertyChanged("LastStBuyPrice_C");
            }
        }

        private uint _StBuyCount_c;		//买十量

        public uint StBuyCount_C
        {
            get { return _StBuyCount_c; }
            set
            {
                _StBuyCount_c = value;
                OnPropertyChanged("StBuyCount_C");
            }
        }
        private double _StSellPrice_c;		//卖十价

        public double StSellPrice_C
        {
            get { return _StSellPrice_c; }
            set
            {
                LastStSellPrice_C = _StSellPrice_c == 0.0 ? value : _StSellPrice_c;
                _StSellPrice_c = value;
                OnPropertyChanged("StSellPrice_C");
            }
        }

        private double _LaststSellPrice_c;		//上一个卖十价

        public double LastStSellPrice_C
        {
            get { return _LaststSellPrice_c; }
            set
            {
                _LaststSellPrice_c = value;
                OnPropertyChanged("LastStSellPrice_C");
            }
        }

        private uint _StSellCount_c;		//卖十量

        public uint StSellCount_C
        {
            get { return _StSellCount_c; }
            set
            {
                _StSellCount_c = value;
                OnPropertyChanged("StSellCount_C");
            }
        }
        private UInt64 _Volumn_c;		//成交量

        public UInt64 Volumn_C
        {
            get { return _Volumn_c; }
            set
            {
                _Volumn_c = value;
                OnPropertyChanged("Volumn_C");
                OnPropertyChanged("AvgPrice_C");
            }
        }
        private Int64 _I64Outside_c;			//外盘

        public Int64 I64Outside_C
        {
            get { return _I64Outside_c; }
            set
            {
                _I64Outside_c = value;
                OnPropertyChanged("I64Outside_C");
            }
        }
        private UInt32 _CurrentHand_c;			//现手

        public UInt32 CurrentHand_C
        {
            get { return _CurrentHand_c; }
            set
            {
                _CurrentHand_c = value;
                OnPropertyChanged("CurrentHand_C");
            }
        }
        private int _ICurrentSu_c;			//现额

        public int ICurrentSu_C
        {
            get { return _ICurrentSu_c; }
            set
            {
                _ICurrentSu_c = value;
                OnPropertyChanged("ICurrentSu_C");
            }
        }

        //double _AvgPrice;			//平均价
        public double AvgPrice_C
        {
            get
            {
                double v = (double)_Hycs * Volumn_C;
                if (v > 0)
                {
                    return (double)I64Sum_C / v;
                }
                else
                {
                    return 0;
                }
            }
        }
        private int _IDealNumber_c;			//成交笔数

        public int IDealNumber_C
        {
            get { return _IDealNumber_c; }
            set
            {
                _IDealNumber_c = value;
                OnPropertyChanged("IDealNumber_C");
            }
        }
        private double _ISettlementPrice_c;		//现结算价

        public double ISettlementPrice_C
        {
            get { return _ISettlementPrice_c; }
            set
            {
                _ISettlementPrice_c = value;
                OnPropertyChanged("ISettlementPrice_C");
            }
        }

        /// <summary>
        /// 昨收
        /// </summary>
        private double _IPrevClose_c;
        /// <summary>
        /// 昨收
        /// </summary>
        public double PrevClose_C
        {
            get
            {
                return _IPrevClose_c;
            }
            set
            {
                _IPrevClose_c = value;
                OnPropertyChanged("PrevClose_C");
            }
        }

        private short _UsUpdateNumber_c;		//行情变动总次数

        public short UsUpdateNumber_C
        {
            get { return _UsUpdateNumber_c; }
            set
            {
                _UsUpdateNumber_c = value;
                OnPropertyChanged("UsUpdateNumber_C");
            }
        }

        private Double _IPrevSettlement_c;
        /// <summary>
        /// 昨结算
        /// </summary>
        public Double PrevSettleMent_C
         {
             get
             {
                 return _IPrevSettlement_c;
             }
             set
             {
                 _IPrevSettlement_c = value;
                 OnPropertyChanged("PrevSettleMent_C");
                 OnPropertyChanged("PriceFluctuation_C");
             }
         }

         /// <summary>
         /// 涨跌
         /// </summary>
         public double PriceFluctuation_C
         {
             get
             {
                 if (_INewPrice_c == 0)
                 {
                     return 0;
                 }
                 else
                 {
                     return _INewPrice_c - _IPrevSettlement_c;
                 }
             }
         }

         /// <summary>
         /// 涨跌幅度
         /// </summary>
         public Double PriceFluctuationFD_C
         {
             get
             {
                 if (_IPrevSettlement_c == 0)
                 {
                     return 0;
                 }
                 return PriceFluctuation_C / _IPrevSettlement_c;
             }
         }
         private Double _UpStopPrice_c;
        /// <summary>
        /// 涨停价
        /// </summary>
         public Double UpStopPrice_C
         {
             get
             {
                 return _UpStopPrice_c;
             }
             set
             {
                 _UpStopPrice_c = value;
                 OnPropertyChanged("UpStopPrice_C");
             }
         }

         private Double _DownStopPrice_c;
         /// <summary>
         /// 跌停价
         /// </summary>
         public Double DownStopPrice_C
         {
             get
             {
                 return _DownStopPrice_c;
             }
             set
             {
                 _DownStopPrice_c = value;
                 OnPropertyChanged("DownStopPrice_C");
             }
         }
        private short _NTime_c;				//开盘分钟偏移数，冗余数据

        public short NTime_C
        {
            get { return _NTime_c; }
            set
            {
                _NTime_c = value;
                OnPropertyChanged("UsUpdateNumber_C");
            }
        }

        /// <summary>
        /// 昨持仓
        /// </summary>
        private UInt64 _I64PreChiCang_c;
        /// <summary>
        /// 昨持仓
        /// </summary>
        public UInt64 PreChicang_C
        {
            get
            {
                return _I64PreChiCang_c;
            }
            set
            {
                _I64PreChiCang_c = value;
                OnPropertyChanged("PreChicang_C");
                OnPropertyChanged("PositionFluctuation_C");
            }
        }

         /// <summary>
         /// 持仓增减
         /// </summary>
        public int PositionFluctuation_C
         {
             get
             {
                 return (int)ChiCangLiang_C - (int)PreChicang_C;
                 
             }
         }

        private string _UpdateTime_c;				//更新时间

        public string UpdateTime_C
        {
            get { return _UpdateTime_c; }
            set
            {
                _UpdateTime_c = value;
                OnPropertyChanged("UpdateTime_C");
            }
        }

        //private double _AvgBasePrice_c;				//平均基准价

        //public double AvgBasePrice_C
        //{
        //    get
        //    {
        //        if (_INewPrice_c > 0)
        //        {
        //            return (_AvgBasePrice_c * 11 + _INewPrice_c) / 12;
        //        }
        //        else
        //        {
        //            return _INewPrice_c;
        //        }
        //    }
        //    set
        //    {
        //        _AvgBasePrice_c = value;
        //        OnPropertyChanged("INewPrice_C");
        //    }
        //}

        private double _Sigma_c;				//隐含波动率

        public double Sigma_C
        {
            get { return _Sigma_c; }
            set
            {
                _Sigma_c = value;
                OnPropertyChanged("Sigma_C");
                OnPropertyChanged("AvgBasePrice_C");
            }
        }

        private double _Delta_c;				//delta

        public double Delta_C
        {
            get { return _Delta_c; }
            set
            {
                _Delta_c = value;
                OnPropertyChanged("Delta_C");
                OnPropertyChanged("AvgBasePrice_C");
                OnPropertyChanged("StBuyPrice_C");
                OnPropertyChanged("StSellPrice_C");
            }
        }

        private double _Gamma_c;				//Gamma

        public double Gamma_C
        {
            get { return _Gamma_c; }
            set
            {
                _Gamma_c = value;
                OnPropertyChanged("Gamma_C");
                OnPropertyChanged("AvgBasePrice_C");
                OnPropertyChanged("StBuyPrice_C");
                OnPropertyChanged("StSellPrice_C");
            }
        }

        private double _Vega_c;				//Vega

        public double Vega_C
        {
            get { return _Vega_c; }
            set
            {
                _Vega_c = value;
                OnPropertyChanged("Vega_C");
                OnPropertyChanged("AvgBasePrice_C");
            }
        }

        private double _Theta_c;				//Theta

        public double Theta_C
        {
            get { return _Theta_c; }
            set
            {
                _Theta_c = value;
                OnPropertyChanged("Theta_C");
                OnPropertyChanged("AvgBasePrice_C");
            }
        }

        private double _Rho_c;				//Rho

        public double Rho_C
        {
            get { return _Rho_c; }
            set
            {
                _Rho_c = value;
                OnPropertyChanged("Rho_C");
                OnPropertyChanged("AvgBasePrice_C");
            }
        }

        #endregion

        #region Put Option

        private string _Code_p;               //看跌期权编码

        public string Code_P
        {
            get { return _Code_p; }
            set
            {
                _Code_p = value;
                OnPropertyChanged("Code_P");
            }
        }

        private double _Open_p;				//今开盘

        public double Open_P
        {
            get { return _Open_p; }
            set
            {
                _Open_p = value;
                OnPropertyChanged("Open_P");
            }
        }

        private double _IMaxPrice_p;			//今最高价

        public double IMaxPrice_P
        {
            get { return _IMaxPrice_p; }
            set
            {
                _IMaxPrice_p = value;
                OnPropertyChanged("IMaxPrice_P");
            }
        }
        private double _IMinPrice_p;			//今最低价

        public double IMinPrice_P
        {
            get { return _IMinPrice_p; }
            set
            {
                _IMinPrice_p = value;
                OnPropertyChanged("IMinPrice_P");
            }
        }
        private double _IClose_p;				//今收盘

        public double IClose_P
        {
            get { return _IClose_p; }
            set
            {
                _IClose_p = value;
                OnPropertyChanged("IClose_P");
            }
        }
        private double _INewPrice_p;			//最新价

        public double INewPrice_P
        {
            get { return _INewPrice_p; }
            set
            {
                LastINewPrice_P = _INewPrice_p == 0.0 ? value : _INewPrice_p;
                _INewPrice_p = value;
                OnPropertyChanged("INewPrice_P");
                OnPropertyChanged("PriceFluctuation_P");
                OnPropertyChanged("PriceFluctuationFD_P");
            }
        }

        private double _LastiNewPrice_p;			//上一个最新价

        public double LastINewPrice_P
        {
            get { return _LastiNewPrice_p; }
            set
            {
                _LastiNewPrice_p = value;
                OnPropertyChanged("LastINewPrice_P");
            }
        }


        private UInt64 _ChiCangLiang_p;				//持仓量

        public UInt64 ChiCangLiang_P
        {
            get { return _ChiCangLiang_p; }
            set
            {
                _ChiCangLiang_p = value;
                OnPropertyChanged("ChiCangLiang_P");
                OnPropertyChanged("PositionFluctuation_P");
            }
        }
        private UInt64 _I64Sum_p;				//成交金额

        public UInt64 I64Sum_P
        {
            get { return _I64Sum_p; }
            set
            {
                _I64Sum_p = value;
                OnPropertyChanged("I64Sum_P");
                OnPropertyChanged("AvgPrice_P");
            }
        }
        private double _StBuyPrice_p;		//买十价

        public double StBuyPrice_P
        {
            get { return _StBuyPrice_p; }
            set
            {
                LastStBuyPrice_P = _StBuyPrice_p == 0.0 ? value : _StBuyPrice_p;

                _StBuyPrice_p = value;
                OnPropertyChanged("StBuyPrice_P");

            }
        }

        private double _LaststBuyPrice_p;		//上一个买十价

        public double LastStBuyPrice_P
        {
            get { return _LaststBuyPrice_p; }
            set
            {
                _LaststBuyPrice_p = value;
                OnPropertyChanged("LastStBuyPrice_P");
            }
        }

        private uint _StBuyCount_p;		//买十量

        public uint StBuyCount_P
        {
            get { return _StBuyCount_p; }
            set
            {
                _StBuyCount_p = value;
                OnPropertyChanged("StBuyCount_P");
            }
        }
        private double _StSellPrice_p;		//卖十价

        public double StSellPrice_P
        {
            get { return _StSellPrice_p; }
            set
            {
                LastStSellPrice_P = _StSellPrice_p == 0.0 ? value : _StSellPrice_p;
                _StSellPrice_p = value;
                OnPropertyChanged("StSellPrice_P");
            }
        }

        private double _LastStSellPrice_p;		//上一个卖十价

        public double LastStSellPrice_P
        {
            get { return _LastStSellPrice_p; }
            set
            {
                _LastStSellPrice_p = value;
                OnPropertyChanged("LastStSellPrice_P");
            }
        }

        private uint _StSellCount_p;		//卖十量

        public uint StSellCount_P
        {
            get { return _StSellCount_p; }
            set
            {
                _StSellCount_p = value;
                OnPropertyChanged("StSellCount_P");
            }
        }
        private UInt64 _Volumn_p;		//成交量

        public UInt64 Volumn_P
        {
            get { return _Volumn_p; }
            set
            {
                _Volumn_p = value;
                OnPropertyChanged("Volumn_P");
                OnPropertyChanged("AvgPrice_P");
            }
        }
        private Int64 _I64Outside_p;			//外盘

        public Int64 I64Outside_P
        {
            get { return _I64Outside_p; }
            set
            {
                _I64Outside_p = value;
                OnPropertyChanged("I64Outside_P");
            }
        }
        private UInt32 _CurrentHand_p;			//现手

        public UInt32 CurrentHand_P
        {
            get { return _CurrentHand_p; }
            set
            {
                _CurrentHand_p = value;
                OnPropertyChanged("CurrentHand_P");
            }
        }
        private int _ICurrentSu_p;			//现额

        public int ICurrentSu_P
        {
            get { return _ICurrentSu_p; }
            set
            {
                _ICurrentSu_p = value;
                OnPropertyChanged("ICurrentSu_P");
            }
        }




        //double _AvgPrice;			//平均价

        public double AvgPrice_P
        {
            get
            {
                double v = (double)_Hycs * Volumn_P;
                if (v > 0)
                {
                    return (double)I64Sum_P / v;
                }
                else
                {
                    return 0;
                }
            }
        }
        private int _IDealNumber_p;			//成交笔数

        public int IDealNumber_P
        {
            get { return _IDealNumber_p; }
            set
            {
                _IDealNumber_p = value;
                OnPropertyChanged("IDealNumber_P");
            }
        }
        private double _ISettlementPrice_p;		//现结算价

        public double ISettlementPrice_P
        {
            get { return _ISettlementPrice_p; }
            set
            {
                _ISettlementPrice_p = value;
                OnPropertyChanged("ISettlementPrice_P");
            }
        }

        /// <summary>
        /// 昨收
        /// </summary>
        private double _PrevClose_p;
        /// <summary>
        /// 昨收
        /// </summary>
        public double PrevClose_P
        {
            get
            {
                return _PrevClose_p;
            }
            set
            {
                _PrevClose_p = value;
                OnPropertyChanged("PrevClose_P");
            }
        }

        private short _UsUpdateNumber_p;		//行情变动总次数

        public short UsUpdateNumber_P
        {
            get { return _UsUpdateNumber_p; }
            set
            {
                _UsUpdateNumber_p = value;
                OnPropertyChanged("UsUpdateNumber_P");
            }
        }

        private Double _PrevSettlement_p;
        /// <summary>
        /// 昨结算
        /// </summary>
        public Double PrevSettleMent_P
        {
            get
            {
                return _PrevSettlement_p;
            }
            set
            {
                _PrevSettlement_p = value;
                OnPropertyChanged("PrevSettleMent_P");
                OnPropertyChanged("PriceFluctuation_P");
            }
        }

        /// <summary>
        /// 涨跌
        /// </summary>
        public double PriceFluctuation_P
        {
            get
            {
                if (_INewPrice_p == 0)
                {
                    return 0;
                }
                else
                {
                    return _INewPrice_p - _PrevSettlement_p;
                }
            }
        }

        /// <summary>
        /// 涨跌幅度
        /// </summary>
        public Double PriceFluctuationFD_P
        {
            get
            {
                if (_PrevSettlement_p == 0)
                {
                    return 0;
                }
                return PriceFluctuation_P / _PrevSettlement_p;
            }
        }
        private Double _UpStopPrice_p;
        /// <summary>
        /// 涨停价
        /// </summary>
        public Double UpStopPrice_P
        {
            get
            {
                return _UpStopPrice_p;
            }
            set
            {
                _UpStopPrice_p = value;
                OnPropertyChanged("UpStopPrice_P");
            }
        }

        private Double _DownStopPrice_p;
        /// <summary>
        /// 跌停价
        /// </summary>
        public Double DownStopPrice_P
        {
            get
            {
                return _DownStopPrice_p;
            }
            set
            {
                _DownStopPrice_p = value;
                OnPropertyChanged("DownStopPrice_P");
            }
        }
        private short _NTime_p;				//开盘分钟偏移数，冗余数据

        public short NTime_P
        {
            get { return _NTime_p; }
            set
            {
                _NTime_p = value;
                OnPropertyChanged("UsUpdateNumber_P");
            }
        }

        /// <summary>
        /// 昨持仓
        /// </summary>
        private UInt64 _I64PreChiCang_p;
        /// <summary>
        /// 昨持仓
        /// </summary>
        public UInt64 PreChicang_P
        {
            get
            {
                return _I64PreChiCang_p;
            }
            set
            {
                _I64PreChiCang_p = value;
                OnPropertyChanged("PreChicang_P");
                OnPropertyChanged("PositionFluctuation_P");
            }
        }

        /// <summary>
        /// 持仓增减
        /// </summary>
        public int PositionFluctuation_P
        {
            get
            {
                return (int)ChiCangLiang_P - (int)PreChicang_P;

            }
        }

        private string updateTime_p;				//更新时间

        public string UpdateTime_P
        {
            get { return updateTime_p; }
            set
            {
                updateTime_p = value;
                OnPropertyChanged("UpdateTime_P");
            }
        }

        //public double _AvgBasePrice_p = 0.0;				//平均基准价

        //public double AvgBasePrice_P				
        //{
        //    get
        //    {
        //        if (_INewPrice_p > 0)
        //        {
        //            return (_AvgBasePrice_p * 11 + _INewPrice_p) / 12;
        //        }
        //        else
        //        {
        //            return _INewPrice_p;
        //        }
        //    }
        //    set 
        //    {
        //        _AvgBasePrice_p = value;
        //        OnPropertyChanged("INewPrice_P");
        //    }
        //}

        private double _Sigma_p;				//隐含波动率

        public double Sigma_P
        {
            get { return _Sigma_p; }
            set
            {
                _Sigma_p = value;
                OnPropertyChanged("Sigma_P");
                OnPropertyChanged("AvgBasePrice_P");
            }
        }

        private double _Delta_p;				//delta

        public double Delta_P
        {
            get { return _Delta_p; }
            set
            {
                _Delta_p = value;
                OnPropertyChanged("Delta_P");
                OnPropertyChanged("AvgBasePrice_P");
                OnPropertyChanged("StBuyPrice_P");
                OnPropertyChanged("StSellPrice_P");
            }
        }

        private double _Gamma_p;				//Gamma

        public double Gamma_P
        {
            get { return _Gamma_p; }
            set
            {
                _Gamma_p = value;
                OnPropertyChanged("Gamma_P");
                OnPropertyChanged("AvgBasePrice_P");
                OnPropertyChanged("StBuyPrice_P");
                OnPropertyChanged("StSellPrice_P");
            }
        }

        private double _Vega_p;				//Vega

        public double Vega_P
        {
            get { return _Vega_p; }
            set
            {
                _Vega_p = value;
                OnPropertyChanged("Vega_P");
                OnPropertyChanged("AvgBasePrice_P");
            }
        }

        private double _Theta_p;				//Theta

        public double Theta_P
        {
            get { return _Theta_p; }
            set
            {
                _Theta_p = value;
                OnPropertyChanged("Theta_P");
                OnPropertyChanged("AvgBasePrice_P");
            }
        }

        private double _Rho_p;				//Rho

        public double Rho_P
        {
            get { return _Rho_p; }
            set
            {
                _Rho_p = value;
                OnPropertyChanged("Rho_P");
                OnPropertyChanged("AvgBasePrice_P");
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        public void CopyProperties(OptionRealData realData)
        {
            this.ChiCangLiang_C = realData.ChiCangLiang_C;
            this.StBuyCount_C = realData.StBuyCount_C;
            this.StBuyPrice_C = realData.StBuyPrice_C;
            this.StSellCount_C = realData.StSellCount_C;
            this.StSellPrice_C = realData.StSellPrice_C;
            this.DownStopPrice_C = realData.DownStopPrice_C;
            this.Hycs = realData.Hycs;
            this.I64Sum_C = realData.I64Sum_C;
            this.ICurrentSu_C = realData.ICurrentSu_C;
            this.ISettlementPrice_C = realData.ISettlementPrice_C;
            this.PrevSettleMent_C = realData.PrevSettleMent_C;
            this.UpStopPrice_C = realData.UpStopPrice_C;
            this.Code = realData.Code;
            this.CurrentHand_C = realData.CurrentHand_C;
            this.I64Outside_C = realData.I64Outside_C;
            this.IClose_C = realData.IClose_C;
            this.IDealNumber_C = realData.IDealNumber_C;
            this.IMaxPrice_C = realData.IMaxPrice_C;
            this.IMinPrice_C = realData.IMinPrice_C;
            this.INewPrice_C = realData.INewPrice_C;
            this.LastINewPrice_C = realData.LastINewPrice_C+r.Next(-2,1);
                //this.IsFirstData = realData.IsFirstData;
            this.LogMessage = realData.LogMessage;
            this.Name = realData.Name;
            this.Open_C = realData.Open_C;
            this.NTime_C = realData.NTime_C;
            this.PrevClose_C = realData.PrevClose_C;
            this.UiTime = realData.UiTime;
            this.UsUpdateNumber_C = realData.UsUpdateNumber_C;
            this.Volumn_C = realData.Volumn_C;


            this.ChiCangLiang_P = realData.ChiCangLiang_P;
            this.StBuyCount_P = realData.StBuyCount_P;
            this.StBuyPrice_P = realData.StBuyPrice_P;
            this.StSellCount_P = realData.StSellCount_P;
            this.StSellPrice_P = realData.StSellPrice_P;
            this.DownStopPrice_P = realData.DownStopPrice_P;
            this.Hycs = realData.Hycs;
            this.I64Sum_P = realData.I64Sum_P;
            this.ICurrentSu_P = realData.ICurrentSu_P;
            this.ISettlementPrice_P = realData.ISettlementPrice_P;
            this.PrevSettleMent_P = realData.PrevSettleMent_P;
            this.UpStopPrice_P = realData.UpStopPrice_P;
            this.Code = realData.Code;
            this.CurrentHand_P = realData.CurrentHand_P;
            this.I64Outside_P = realData.I64Outside_P;
            this.IClose_P = realData.IClose_P;
            this.IDealNumber_P = realData.IDealNumber_P;
            this.IMaxPrice_P = realData.IMaxPrice_P;
            this.IMinPrice_P = realData.IMinPrice_P;
            this.INewPrice_P = realData.INewPrice_P;
            this.LastINewPrice_P = realData.LastINewPrice_P + r.Next(-2, 1);
            //this.IsFirstData = realData.IsFirstData;
            this.LogMessage = realData.LogMessage;
            this.Name = realData.Name;
            this.Open_P = realData.Open_P;
            this.NTime_P = realData.NTime_P;
            this.PrevClose_P = realData.PrevClose_P;
            this.UiTime = realData.UiTime;
            this.UsUpdateNumber_P = realData.UsUpdateNumber_P;
            this.Volumn_P = realData.Volumn_P;
        }

        Random r = new Random(100);

        public void InitProperties(DisplayRealData realData, int flipPrice)
        {
            this.ExecutePrice = realData.PrevSettleMent + flipPrice * 5;
            this.Code = realData.Code;

            this.Code_C = Code.Replace("IF", "IO") + "-C-" + ExecutePrice;
            this.Code_P = Code.Replace("IF", "IO") + "-P-" + ExecutePrice;
            this.Hycs = realData.Hycs;
            this.Code = realData.Code;
            this.UiTime = realData.UiTime;
            this.LogMessage = realData.LogMessage;
            this.Name = realData.Name;

            double basePrice = realData.INewPrice - this.ExecutePrice;

            this.ChiCangLiang_C = realData.ChiCangLiang + (ulong)r.Next(100) *100;
            this.StBuyCount_C = realData.StBuyCount + (uint)r.Next(10);
            this.StBuyPrice_C = realData.StBuyPrice - this.ExecutePrice + r.NextDouble() * 10;
            this.StSellCount_C = realData.StSellCount + (uint)r.Next(10);
            this.StSellPrice_C = realData.StSellPrice - this.ExecutePrice + r.NextDouble() * 10;
            this.DownStopPrice_C = (basePrice + 10) * 1.1;
            this.I64Sum_C = realData.I64Sum + (ulong)r.Next(100);
            this.ICurrentSu_C = realData.ICurrentSum + r.Next(100);
            this.ISettlementPrice_C = basePrice + r.NextDouble() * 10;
            this.PrevSettleMent_C = basePrice + r.NextDouble() * 10;
            this.UpStopPrice_C = (basePrice - 10) * 0.9;
            this.CurrentHand_C = realData.CurrentHand + (uint)r.Next(100);
            this.I64Outside_C = realData.I64Outside + r.Next(100);
            this.IClose_C = basePrice + r.Next(100);
            this.IDealNumber_C = realData.IDealNumber + r.Next(100);
            this.IMaxPrice_C = realData.IMaxPrice - realData.PrevSettleMent;
            this.IMinPrice_C = realData.IMinPrice - realData.PrevSettleMent;
            this.INewPrice_C = basePrice + r.Next(100);
            this.LastINewPrice_C = this.INewPrice_C + r.Next(-2, 1);

            this.Open_C = realData.Open - realData.PrevSettleMent + r.Next(100);
            this.NTime_C = realData.NTime ;
            this.PrevClose_C = realData.PrevClose - realData.PrevSettleMent + r.Next(100);
            this.UsUpdateNumber_C = realData.UsUpdateNumber;
            this.Volumn_C = realData.Volumn + (ulong)r.Next(100);


            this.ChiCangLiang_P = realData.ChiCangLiang + (ulong)r.Next(100) * 100;
            this.StBuyCount_P = realData.StBuyCount + (uint)r.Next(10);
            this.StBuyPrice_P = realData.StBuyPrice - this.ExecutePrice + r.NextDouble() * 10;
            this.StSellCount_P = realData.StSellCount + (uint)r.Next(10);
            this.StSellPrice_P = realData.StSellPrice - this.ExecutePrice + r.NextDouble() * 10;
            this.DownStopPrice_P = (basePrice + 10) * 1.1;
            this.I64Sum_P = realData.I64Sum + (ulong)r.Next(100);
            this.ICurrentSu_P = realData.ICurrentSum + r.Next(100);
            this.ISettlementPrice_P = basePrice + r.NextDouble() * 10;
            this.PrevSettleMent_P = basePrice + r.NextDouble() * 10;
            this.UpStopPrice_P = (basePrice - 10) * 0.9;
            this.CurrentHand_P = realData.CurrentHand + (uint)r.Next(100);
            this.I64Outside_P = realData.I64Outside + r.Next(100);
            this.IClose_P = basePrice + r.Next(100);
            this.IDealNumber_P = realData.IDealNumber + r.Next(100);
            this.IMaxPrice_P = realData.IMaxPrice - realData.PrevSettleMent;
            this.IMinPrice_P = realData.IMinPrice - realData.PrevSettleMent;
            this.INewPrice_P = basePrice + r.Next(100);
            this.LastINewPrice_P = this.INewPrice_P + r.Next(-2, 1);
            this.Open_P = realData.Open - realData.PrevSettleMent + r.Next(100);
            this.NTime_P = realData.NTime;
            this.PrevClose_P = realData.PrevClose - realData.PrevSettleMent + r.Next(100);
            this.UsUpdateNumber_P = realData.UsUpdateNumber;
            this.Volumn_P = realData.Volumn + (ulong)r.Next(100);
        }

        public void UpdateProperties(RealData realData)
        {
            this.ExchCode = realData.CodeInfo.ExchCode;
            if (realData.CodeInfo.Code == this.Code_C)
            {
                this.ChiCangLiang_C = realData.Position;
                this.StBuyCount_C = realData.BidHand[0];
                this.StBuyPrice_C = realData.BidPrice[0];
                this.StSellCount_C = realData.AskHand[0];
                this.StSellPrice_C = realData.AskPrice[0];
                this.DownStopPrice_C = realData.LowerLimitPrice;
                this.I64Sum_C = (UInt64)realData.Sum ;
                //this.ICurrentSu_C = realData.ICurrentSum ;
                this.ISettlementPrice_C = realData.SettlmentPrice;
                this.PrevSettleMent_C = realData.PrevSettlementPrice;
                this.UpStopPrice_C = realData.UpperLimitPrice;
                this.CurrentHand_C = realData.Hand;
                //this.I64Outside_C = realData.I64Outside;
                this.IClose_C = realData.ClosePrice;
                //this.IDealNumber_C = realData.IDealNumber;
                this.IMaxPrice_C = realData.MaxPrice;
                this.IMinPrice_C = realData.MinPrice;
                this.INewPrice_C = realData.NewPrice;

                this.Open_C = realData.OpenPrice;
                //this.NTime_C = realData.NTime;
                this.PrevClose_C = realData.PrevClose;
                //this.UsUpdateNumber_C = realData.UsUpdateNumber;
                this.Volumn_C = realData.Volumn;
            }
            else if (realData.CodeInfo.Code == this.Code_P)
            {
                this.ChiCangLiang_P = realData.Position;
                this.StBuyCount_P = realData.BidHand[0];
                this.StBuyPrice_P = realData.BidPrice[0];
                this.StSellCount_P = realData.AskHand[0];
                this.StSellPrice_P = realData.AskPrice[0];
                this.DownStopPrice_P = realData.LowerLimitPrice;
                this.I64Sum_P = (UInt64)realData.Sum;
                //this.ICurrentSu_P = realData.ICurrentSum;
                this.ISettlementPrice_P = realData.SettlmentPrice;
                this.PrevSettleMent_P = realData.PrevSettlementPrice;
                this.UpStopPrice_P = realData.UpperLimitPrice;
                this.CurrentHand_P = realData.Hand;
                //this.I64Outside_P = realData.I64Outside;
                this.IClose_P = realData.ClosePrice;
                //this.IDealNumber_P = realData.IDealNumber;
                this.IMaxPrice_P = realData.MaxPrice;
                this.IMinPrice_P = realData.MinPrice;
                this.INewPrice_P = realData.NewPrice;
                //this.LastINewPrice_P = this.INewPrice_P + r.Next(-2, 1);

                this.Open_P = realData.OpenPrice;
                //this.NTime_P = realData.NTime;
                this.PrevClose_P = realData.PrevClose;
                //this.UsUpdateNumber_P = realData.UsUpdateNumber;
                this.Volumn_P = realData.Volumn;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string ret = "";
            if (_Code != null)
            {
                ret += "Code=" + Code.ToString()+" ";
            }

            if (_Code_c != null)
            {
                ret += "Code1=" + Code_C.ToString() + " ";
            }

            if (_Code_p != null)
            {
                ret += "Code2=" + Code_P.ToString() + " ";
            }

            return ret;
        }

        public RealData GetOptRealData_C()
        {
            RealData realData = new RealData();
            realData.CodeInfo.Code = this.Code_C;
            realData.CodeInfo.Name = this.Name;
            realData.Position = this.ChiCangLiang_C;
            realData.BidHand[0] = this.StBuyCount_C;
            realData.BidPrice[0] = this.StBuyPrice_C;
            realData.AskHand[0] = this.StSellCount_C;
            realData.AskPrice[0] = this.StSellPrice_C;
            realData.LowerLimitPrice = this.DownStopPrice_C;
            //realData.HYCS = this.HYCS;
            realData.Sum = this.I64Sum_C;
            //realData.ICurrentSum = this.ICurrentSu_C;
            realData.SettlmentPrice = this.ISettlementPrice_C;
            realData.PrevSettlementPrice = this.PrevSettleMent_C;
            realData.UpperLimitPrice = this.UpStopPrice_C;
            realData.CodeInfo.Code = this.Code_C;//??
            realData.Hand = this.CurrentHand_C;
            //realData.I64Outside = this.I64Outside_C;
            realData.ClosePrice = this.IClose_C;
            //realData.IDealNumber = this.IDealNumber_C;
            realData.MaxPrice = this.IMaxPrice_C;
            realData.MinPrice = this.IMinPrice_C;
            realData.NewPrice = this.INewPrice_C;
            //Todo
            //realData.LastINewPrice = this.LastINewPrice_C + r.Next(-2, 1);
            //realData.IsFirstData = this.IsFirstData;
            //realData.LogMessage = this.LogMessage;
            //realData.Name = this.Name;
            realData.OpenPrice = this.Open_C;
            //realData.NTime = this.NTime_C;
            realData.PrevClose = this.PrevClose_C;
            //realData.UiTime = this.UiTime;
            //realData.UsUpdateNumber = this.UsUpdateNumber_C;
            realData.Volumn = this.Volumn_C;
            realData.CodeInfo.ExchCode = CodeSetManager.ExNameToCtp(this.Market);
            realData.PrevPosition = this.PreChicang_C;
            realData.PrevClose = this.PrevClose_C;
            realData.UpdateTime = this.UpdateTime_C;
            return realData;
        }

        public RealData GetOptRealData_P()
        {
            RealData realData = new RealData();
            realData.CodeInfo.Code = this.Code_P;
            realData.CodeInfo.Name = this.Name;
            realData.Position = this.ChiCangLiang_P;
            realData.BidHand[0] = this.StBuyCount_P;
            realData.BidPrice[0] = this.StBuyPrice_P;
            realData.AskHand[0] = this.StSellCount_P;
            realData.AskPrice[0] = this.StSellPrice_P;
            realData.LowerLimitPrice = this.DownStopPrice_P;
            //realData.HYCS = this.HYCS;
            realData.Sum = this.I64Sum_P;
            //realData.ICurrentSum = this.ICurrentSu_P;
            realData.SettlmentPrice = this.ISettlementPrice_P;
            realData.PrevSettlementPrice = this.PrevSettleMent_P;
            realData.UpperLimitPrice = this.UpStopPrice_P;
            realData.CodeInfo.Code = this.Code_P;
            realData.Hand = this.CurrentHand_P;
            //realData.I64Outside = this.I64Outside_P;
            realData.ClosePrice = this.IClose_P;
            //realData.IDealNumber = this.IDealNumber_P;
            realData.MaxPrice = this.IMaxPrice_P;
            realData.MinPrice = this.IMinPrice_P;
            realData.NewPrice = this.INewPrice_P;
            //realData.LastINewPrice = this.LastINewPrice_P + r.Next(-2, 1);
            //realData.LogMessage = this.LogMessage;
            //realData.Name = this.Name;
            realData.OpenPrice = this.Open_P;
            //realData.NTime = this.NTime_P;
            realData.PrevClose = this.PrevClose_P;
            //realData.UiTime = this.UiTime;
            //realData.UsUpdateNumber = this.UsUpdateNumber_P;
            realData.Volumn = this.Volumn_P;
            realData.CodeInfo.ExchCode = CodeSetManager.ExNameToCtp(this.Market);
            realData.PrevPosition = this.PreChicang_P;
            realData.PrevClose = this.PrevClose_P;
            realData.UpdateTime = this.UpdateTime_P;
            return realData;
        }
    }
}
