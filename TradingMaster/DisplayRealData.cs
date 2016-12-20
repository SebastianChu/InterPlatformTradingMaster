using System;
using System.ComponentModel;
using TradingMaster.CodeSet;

namespace TradingMaster
{
    public class DisplayRealData : INotifyPropertyChanged
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

        private string _Code1;               //期货组合套利编码第一腿

        public string Code1
        {
            get { return _Code1; }
            set
            {
                _Code1 = value;
                OnPropertyChanged("Code1");
            }
        }

        private string _Code2;               //期货组合套利编码第二腿

        public string Code2
        {
            get { return _Code2; }
            set
            {
                _Code2 = value;
                OnPropertyChanged("Code2");
            }
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

        private string _Name;               //名称

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
        private double _Open;				//今开盘

        public double Open
        {
            get { return _Open; }
            set
            {
                _Open = value;
                OnPropertyChanged("Open");
            }
        }
        private double _IMaxPrice;			//今最高价

        public double IMaxPrice
        {
            get { return _IMaxPrice; }
            set
            {
                _IMaxPrice = value;
                OnPropertyChanged("IMaxPrice");
            }
        }
        private double _IMinPrice;			//今最低价

        public double IMinPrice
        {
            get { return _IMinPrice; }
            set
            {
                _IMinPrice = value;
                OnPropertyChanged("IMinPrice");
            }
        }
        private double _IClose;				//今收盘

        public double IClose
        {
            get { return _IClose; }
            set
            {
                _IClose = value;
                OnPropertyChanged("IClose");
            }
        }
        private double _INewPrice;			//最新价

        public double INewPrice
        {
            get { return _INewPrice; }
            set
            {
                LastINewPrice = _INewPrice;
                _INewPrice = value;
                OnPropertyChanged("INewPrice");
                OnPropertyChanged("PriceFluctuation");
                OnPropertyChanged("PriceFluctuationFD");
            }
        }

        private double _LastiNewPrice;			//上一个最新价

        public double LastINewPrice
        {
            get { return _LastiNewPrice; }
            set
            {
                _LastiNewPrice = value;
                OnPropertyChanged("LastINewPrice");
            }
        }

        private UInt64 _ChiCangLiang;				//持仓量

        public UInt64 ChiCangLiang
        {
            get { return _ChiCangLiang; }
            set
            {
                _ChiCangLiang = value;
                OnPropertyChanged("ChiCangLiang");
                OnPropertyChanged("PositionFluctuation");
            }
        }
        private UInt64 _I64Sum;				//成交金额

        public UInt64 I64Sum
        {
            get { return _I64Sum; }
            set
            {
                _I64Sum = value;
                OnPropertyChanged("I64Sum");
                OnPropertyChanged("AvgPrice");
            }
        }
        private double _StBuyPrice;		//买十价

        public double StBuyPrice
        {
            get { return _StBuyPrice; }
            set
            {
                LastStBuyPrice = _StBuyPrice;

                _StBuyPrice = value;
                OnPropertyChanged("StBuyPrice");

            }
        }

        private double _LaststBuyPrice;		//上一个买十价

        public double LastStBuyPrice
        {
            get { return _LaststBuyPrice; }
            set
            {
                _LaststBuyPrice = value;
                OnPropertyChanged("LastStBuyPrice");
            }
        }

        private uint _StBuyCount;		//买十量

        public uint StBuyCount
        {
            get { return _StBuyCount; }
            set
            {
                _StBuyCount = value;
                OnPropertyChanged("StBuyCount");
            }
        }
        private double _StSellPrice;		//卖十价

        public double StSellPrice
        {
            get { return _StSellPrice; }
            set
            {
                LastStSellPrice = _StSellPrice;
                _StSellPrice = value;
                OnPropertyChanged("StSellPrice");
            }
        }

        private double _LaststSellPrice;		//上一个卖十价

        public double LastStSellPrice
        {
            get { return _LaststSellPrice; }
            set
            {
                _LaststSellPrice = value;
                OnPropertyChanged("LastStSellPrice");
            }
        }

        private uint _StSellCount;		//卖十量

        public uint StSellCount
        {
            get { return _StSellCount; }
            set
            {
                _StSellCount = value;
                OnPropertyChanged("StSellCount");
            }
        }
        private UInt64 _Volumn;		//成交量

        public UInt64 Volumn
        {
            get { return _Volumn; }
            set
            {
                _Volumn = value;
                OnPropertyChanged("Volumn");
                OnPropertyChanged("AvgPrice");
            }
        }
        private Int64 _I64Outside;			//外盘

        public Int64 I64Outside
        {
            get { return _I64Outside; }
            set
            {
                _I64Outside = value;
                OnPropertyChanged("I64Outside");
            }
        }
        private UInt32 _CurrentHand;			//现手

        public UInt32 CurrentHand
        {
            get { return _CurrentHand; }
            set
            {
                _CurrentHand = value;
                OnPropertyChanged("CurrentHand");
            }
        }
        private int _ICurrentSum;			//现额

        public int ICurrentSum
        {
            get { return _ICurrentSum; }
            set
            {
                _ICurrentSum = value;
                OnPropertyChanged("ICurrentSum");
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
                OnPropertyChanged("AvgPrice");
            }
        }

        //double _AvgPrice;			//平均价

        public double AvgPrice
        {
            get
            {
                double v = (double)_Hycs * Volumn;
                if (v > 0)
                {
                    double ret = (double)I64Sum / v;
                    if (ret < this._IMinPrice)
                    {
                        ret = ret * (double)_Hycs;
                    }
                    return ret;
                }
                else
                {
                    return 0;
                }
            }
        }
        private int _IDealNumber;			//成交笔数

        public int IDealNumber
        {
            get { return _IDealNumber; }
            set
            {
                _IDealNumber = value;
                OnPropertyChanged("IDealNumber");
            }
        }
        private double _ISettlementPrice;		//现结算价

        public double ISettlementPrice
        {
            get { return _ISettlementPrice; }
            set
            {
                _ISettlementPrice = value;
                OnPropertyChanged("ISettlementPrice");
            }
        }

        /// <summary>
        /// 昨收
        /// </summary>
        private double _PrevClose;
        /// <summary>
        /// 昨收
        /// </summary>
        public double PrevClose
        {
            get
            {
                return _PrevClose;
            }
            set
            {
                _PrevClose = value;
                OnPropertyChanged("PrevClose");
            }
        }

        private short _UsUpdateNumber;		//行情变动总次数

        public short UsUpdateNumber
        {
            get { return _UsUpdateNumber; }
            set
            {
                _UsUpdateNumber = value;
                OnPropertyChanged("UsUpdateNumber");
            }
        }

        private Double _PrevSettlement;
        /// <summary>
        /// 昨结算
        /// </summary>
        public Double PrevSettleMent
        {
            get
            {
                return _PrevSettlement;
            }
            set
            {
                _PrevSettlement = value;
                OnPropertyChanged("PrevSettleMent");
                OnPropertyChanged("PriceFluctuation");
            }
        }

        /// <summary>
        /// 涨跌
        /// </summary>
        public double PriceFluctuation
        {
            get
            {
                if (_INewPrice == 0)
                {
                    return 0;
                }
                else if (_PrevSettlement == 0)
                {
                    return _INewPrice - _PrevClose;
                }
                else
                {
                    return _INewPrice - _PrevSettlement;
                }
            }
        }

        /// <summary>
        /// 涨跌幅度
        /// </summary>
        public Double PriceFluctuationFD
        {
            get
            {
                if (_PrevSettlement == 0)
                {
                    return PriceFluctuation / _PrevClose;
                }
                return PriceFluctuation / _PrevSettlement;
            }
        }

        private Double _UpStopPrice;
        /// <summary>
        /// 涨停价
        /// </summary>
        public Double UpStopPrice
        {
            get
            {
                return _UpStopPrice;
            }
            set
            {
                _UpStopPrice = value;
                OnPropertyChanged("UpStopPrice");
            }
        }

        private Double _DownStopPrice;
        /// <summary>
        /// 涨停价
        /// </summary>
        public Double DownStopPrice
        {
            get
            {
                return _DownStopPrice;
            }
            set
            {
                _DownStopPrice = value;
                OnPropertyChanged("DownStopPrice");
            }
        }
        private short _NTime;				//开盘分钟偏移数，冗余数据

        public short NTime
        {
            get { return _NTime; }
            set
            {
                _NTime = value;
                OnPropertyChanged("UsUpdateNumber");
            }
        }

        /// <summary>
        /// 昨持仓
        /// </summary>
        private UInt64 _I64PreChiCang;
        /// <summary>
        /// 昨持仓
        /// </summary>
        public UInt64 PreChicang
        {
            get
            {
                return _I64PreChiCang;
            }
            set
            {
                _I64PreChiCang = value;
                OnPropertyChanged("PreChicang");
                OnPropertyChanged("PositionFluctuation");
            }
        }

        /// <summary>
        /// 持仓增减
        /// </summary>
        public int PositionFluctuation
        {
            get
            {
                return (int)ChiCangLiang - (int)PreChicang;

            }
        }

        private string _UpdateTime;				//更新时间

        public string UpdateTime
        {
            get { return _UpdateTime; }
            set
            {
                _UpdateTime = value;
                OnPropertyChanged("UpdateTime");
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

        public void UpdateProperties(RealData realData)
        {
            if (realData.CodeInfo.Code == this.Code)
            {
                this.ChiCangLiang = realData.Position;
                this.StBuyCount = realData.BidHand[0];
                this.StBuyPrice = realData.BidPrice[0];
                this.StSellCount = realData.AskHand[0];
                this.StSellPrice = realData.AskPrice[0];
                this.DownStopPrice = realData.LowerLimitPrice;
                this.I64Sum = (UInt64)realData.Sum;
                //this.ICurrentSum = realData.ICurrentSum ;
                this.ISettlementPrice = realData.SettlmentPrice;
                this.PrevSettleMent = realData.PrevSettlementPrice;
                this.UpStopPrice = realData.UpperLimitPrice;
                this.CurrentHand = realData.Hand;
                //this.I64Outside = realData.I64Outside;
                this.IClose = realData.ClosePrice;
                //this.IDealNumber = realData.IDealNumber;
                this.IMaxPrice = realData.MaxPrice;
                this.IMinPrice = realData.MinPrice;
                this.INewPrice = realData.NewPrice;

                this.Open = realData.OpenPrice;
                //this.NTime = realData.NTime;
                this.PrevClose = realData.PrevClose;
                //this.UsUpdateNumber = realData.UsUpdateNumber;
                this.Volumn = realData.Volumn;
                this.Market = CodeSetManager.CtpToExName(realData.CodeInfo.ExchCode);
                this.PreChicang = realData.PrevPosition;
                this.PrevClose = realData.PrevClose;
                this.UpdateTime = realData.UpdateTime;
            }
        }
    }
}
