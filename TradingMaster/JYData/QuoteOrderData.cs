using System.ComponentModel;

namespace TradingMaster
{
    public class QuoteOrderData : INotifyPropertyChanged
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
        /// 报价编号
        /// </summary>
        private string _QuoteOrderID;
        /// <summary>
        /// 报价编号
        /// </summary>
        public string QuoteOrderID
        {
            get { return _QuoteOrderID; }
            set
            {
                _QuoteOrderID = value;
                OnPropertyChanged("QuoteOrderID");
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
        /// 报价引用
        /// </summary>
        private string _QuoteRef = "";
        /// <summary>
        /// 报价引用
        /// </summary>
        public string QuoteRef
        {
            get { return _QuoteRef; }
            set { _QuoteRef = value; OnPropertyChanged("QuoteRef"); }
        }

        /// <summary>
        /// 报价状态
        /// </summary>
        private string _QuoteStatus;
        /// <summary>
        /// 报价状态
        /// </summary>
        public string QuoteStatus
        {
            get { return _QuoteStatus; }
            set { _QuoteStatus = value; OnPropertyChanged("QuoteStatus"); }
        }

        /// <summary>
        /// 买报单编号
        /// </summary>
        private string _BidOrderID;
        /// <summary>
        /// 买报单编号
        /// </summary>
        public string BidOrderID
        {
            get { return _BidOrderID; }
            set
            {
                _BidOrderID = value;
                OnPropertyChanged("BidOrderID");
            }
        }

        /// <summary>
        /// 买价格
        /// </summary>
        private double _BidPrice;
        /// <summary>
        /// 买价格
        /// </summary>
        public double BidPrice
        {
            get { return _BidPrice; }
            set { _BidPrice = value; OnPropertyChanged("BidPrice"); }
        }

        /// <summary>
        /// 买开平状态
        /// </summary>
        private string _BidOpenClose;
        /// <summary>
        /// 买开平状态
        /// </summary>
        public string BidOpenClose
        {
            get { return _BidOpenClose; }
            set { _BidOpenClose = value; OnPropertyChanged("BidOpenClose"); }
        }

        /// <summary>
        /// 买手数
        /// </summary>
        private int _BidHandCount;
        /// <summary>
        /// 买手数
        /// </summary>
        public int BidHandCount
        {
            get { return _BidHandCount; }
            set { _BidHandCount = value; OnPropertyChanged("BidHandCount"); }//OnPropertyChanged("UnTradeHandCount"); }
        }

        /// <summary>
        /// 买报单引用
        /// </summary>
        private string _BidOrderRef = "";
        /// <summary>
        /// 买报单引用
        /// </summary>
        public string BidOrderRef
        {
            get { return _BidOrderRef; }
            set { _BidOrderRef = value; OnPropertyChanged("BidOrderRef"); }
        }

        /// <summary>
        /// 买投机、套保
        /// </summary>
        private string _BidHedge;
        /// <summary>
        /// 买投机、套保
        /// </summary>
        public string BidHedge
        {
            get { return _BidHedge; }
            set { _BidHedge = value; OnPropertyChanged("BidHedge"); }
        }

        /// <summary>
        /// 卖报单编号
        /// </summary>
        private string _AskOrderID;
        /// <summary>
        /// 卖报单编号
        /// </summary>
        public string AskOrderID
        {
            get { return _AskOrderID; }
            set
            {
                _AskOrderID = value;
                OnPropertyChanged("AskOrderID");
            }
        }

        /// <summary>
        /// 卖价格
        /// </summary>
        private double _AskPrice;
        /// <summary>
        /// 卖价格
        /// </summary>
        public double AskPrice
        {
            get { return _AskPrice; }
            set { _AskPrice = value; OnPropertyChanged("AskPrice"); }
        }

        /// <summary>
        /// 卖开平状态
        /// </summary>
        private string _AskOpenClose;
        /// <summary>
        /// 卖开平状态
        /// </summary>
        public string AskOpenClose
        {
            get { return _AskOpenClose; }
            set { _AskOpenClose = value; OnPropertyChanged("AskOpenClose"); }
        }

        /// <summary>
        /// 卖手数
        /// </summary>
        private int _AskHandCount;
        /// <summary>
        /// 卖手数
        /// </summary>
        public int AskHandCount
        {
            get { return _AskHandCount; }
            set { _AskHandCount = value; OnPropertyChanged("AskHandCount"); }//OnPropertyChanged("UnTradeHandCount"); }
        }

        /// <summary>
        /// 卖报单引用
        /// </summary>
        private string _AskOrderRef = "";
        /// <summary>
        /// 卖报单引用
        /// </summary>
        public string AskOrderRef
        {
            get { return _AskOrderRef; }
            set { _AskOrderRef = value; OnPropertyChanged("AskOrderRef"); }
        }

        /// <summary>
        /// 卖投机、套保
        /// </summary>
        private string _AskHedge;
        /// <summary>
        /// 卖投机、套保
        /// </summary>
        public string AskHedge
        {
            get { return _AskHedge; }
            set { _AskHedge = value; OnPropertyChanged("AskHedge"); }
        }

        /// <summary>
        /// 报单时间
        /// </summary>
        private string _CommitTime;
        /// <summary>
        /// 报单时间
        /// </summary>
        public string CommitTime
        {
            get { return _CommitTime; }
            set { _CommitTime = value; OnPropertyChanged("CommitTime"); }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        private string _UpdateTime;
        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateTime
        {
            get { return _UpdateTime; }
            set { _UpdateTime = value; OnPropertyChanged("UpdateTime"); }
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
        /// 应价编号
        /// </summary>
        private string _ForQuoteOrderID;
        /// <summary>
        /// 应价编号
        /// </summary>
        public string ForQuoteOrderID
        {
            get { return _ForQuoteOrderID; }
            set
            {
                _ForQuoteOrderID = value;
                OnPropertyChanged("ForQuoteOrderID");
            }
        }

        /// <summary>
        /// 状态信息
        /// </summary>
        private string _StatusMsg;
        /// <summary>
        /// 状态信息
        /// </summary>
        public string StatusMsg
        {
            get { return _StatusMsg; }
            set { _StatusMsg = value; OnPropertyChanged("StatusMsg"); }
        }

        /// <summary>
        /// 经纪公司编号
        /// </summary>
        private string _BrokerID = "";
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        public string BrokerID
        {
            get { return _BrokerID; }
            set { _BrokerID = value; OnPropertyChanged("BrokerID"); }
        }

        /// <summary>
        /// 主场单号
        /// </summary>
        private string _BrokerQuoteSeq;
        /// <summary>
        /// 主场单号
        /// </summary>
        public string BrokerQuoteSeq
        {
            get { return _BrokerQuoteSeq; }
            set
            {
                _BrokerQuoteSeq = value;
                OnPropertyChanged("BrokerQuoteSeq");
            }
        }

        /// <summary>
        /// 前置编号
        /// </summary>
        private int _FrontID;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID
        {
            get { return _FrontID; }
            set { _FrontID = value; OnPropertyChanged("FrontID"); }
        }

        /// <summary>
        /// 会话编号
        /// </summary>
        private int _SessionID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID
        {
            get { return _SessionID; }
            set { _SessionID = value; OnPropertyChanged("SessionID"); }
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public QuoteOrderData Copy()
        {
            QuoteOrderData ret = (QuoteOrderData)this.MemberwiseClone();
            return ret;
        }

        public static int CompareByCommitTime(QuoteOrderData o1, QuoteOrderData o2)
        {
            if (o1 == null && o2 == null) return 0;
            if (o1 == null) return -1;
            if (o2 == null) return 1;

            if (o1._CommitTime != null && o2._CommitTime != null)
            {
                if (o1._CommitTime.StartsWith("2") && !o2._CommitTime.StartsWith("2"))
                {
                    return 1;
                }
                else if (!o1._CommitTime.StartsWith("2") && o2._CommitTime.StartsWith("2"))
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }

            if (o1._CommitTime.CompareTo(o2._CommitTime) < 0) return 1;
            if (o1._CommitTime.CompareTo(o2._CommitTime) == 0)
            {
                if (o1._QuoteOrderID.CompareTo(o2._QuoteOrderID) == 1)
                {
                    return -1;
                }
                else if (o1._QuoteOrderID.CompareTo(o2._QuoteOrderID) == -1)
                {
                    return 1;
                }
                else if (o1._QuoteOrderID.CompareTo(o2._QuoteOrderID) == 0)
                {
                    return 0;
                }
            }
            return -1;
        }
        public static int CompareByQuoteID(QuoteOrderData o1, QuoteOrderData o2)
        {
            if (o1 == null && o2 == null) return 0;
            if (o1 == null) return -1;
            if (o2 == null) return 1;

            if (o1._QuoteOrderID.CompareTo(o2._QuoteOrderID) == 1)
            {
                return -1;
            }
            else if (o1._QuoteOrderID.CompareTo(o2._QuoteOrderID) == -1)
            {
                return 1;
            }
            else if (o1._QuoteOrderID.CompareTo(o2._QuoteOrderID) == 0)
            {
                if (o1._CommitTime != null && o2._CommitTime != null)
                {
                    if (o1._CommitTime.StartsWith("2") && !o2._CommitTime.StartsWith("2"))
                    {
                        return 1;
                    }
                    else if (!o1._CommitTime.StartsWith("2") && o2._CommitTime.StartsWith("2"))
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }

                if (o1._CommitTime.CompareTo(o2._CommitTime) < 0) return 1;
                if (o1._CommitTime.CompareTo(o2._CommitTime) == 0)
                {
                    return 0;
                }
            }
            return -1;
        }

    }
}
