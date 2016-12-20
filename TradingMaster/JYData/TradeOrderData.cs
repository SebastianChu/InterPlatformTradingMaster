using System.ComponentModel;

namespace TradingMaster
{
    /// <summary>
    /// 快期的委托数据
    /// </summary>
    public class TradeOrderData : INotifyPropertyChanged
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
        /// 报单编号
        /// </summary>
        private string _OrderID;
        /// <summary>
        /// 报单编号
        /// </summary>
        public string OrderID
        {
            get { return _OrderID; }
            set
            {
                _OrderID = value;
                OnPropertyChanged("Orderid");
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

        private string _Code2;
        public string Code2
        {
            get { return _Code2; }
            set { _Code2 = value; OnPropertyChanged("Code2"); }
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
        /// 开平仓状态
        /// </summary>
        private string _OpenClose;
        /// <summary>
        /// 开平仓状态
        /// </summary>
        public string OpenClose
        {
            get { return _OpenClose; }
            set { _OpenClose = value; OnPropertyChanged("OpenClose"); }
        }

        /// <summary>
        /// 挂单状态
        /// </summary>
        private string _OrderStatus;
        /// <summary>
        /// 挂单状态
        /// </summary>
        public string OrderStatus
        {
            get { return _OrderStatus; }
            set { _OrderStatus = value; OnPropertyChanged("OrderStatus"); }
        }

        /// <summary>
        /// 报单价格
        /// </summary>
        private double _CommitPrice;
        /// <summary>
        /// 报单价格
        /// </summary>
        public double CommitPrice
        {
            get { return _CommitPrice; }
            set { _CommitPrice = value; OnPropertyChanged("CommitPrice"); }
        }

        /// <summary>
        /// 报单手数
        /// </summary>
        private int _CommitHandCount;
        /// <summary>
        /// 报单手数
        /// </summary>
        public int CommitHandCount
        {
            get { return _CommitHandCount; }
            set { _CommitHandCount = value; OnPropertyChanged("CommitHandCount"); OnPropertyChanged("UnTradeHandCount"); }
        }

        /// <summary>
        /// 未成交手数
        /// </summary>
        public int UnTradeHandCount
        {
            get { return (CommitHandCount - TradeHandCount > 0 ? CommitHandCount - TradeHandCount : 0); }
        }

        /// <summary>
        /// 成交手数
        /// </summary>
        private int _TradeHandCount;
        /// <summary>
        /// 成交手数
        /// </summary>
        public int TradeHandCount
        {
            get { return _TradeHandCount; }
            set { _TradeHandCount = value; OnPropertyChanged("TradeHandCount"); OnPropertyChanged("UnTradeHandCount"); }
        }

        /// <summary>
        /// 详细的委托状态
        /// </summary>
        private string _FeedBackInfo;
        /// <summary>
        /// 详细的委托状态
        /// </summary>
        public string FeedBackInfo
        {
            get { return _FeedBackInfo; }
            set { _FeedBackInfo = value; OnPropertyChanged("FeedBackInfo"); }
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
        /// 该笔单子的条件单（埋单）的提交时间
        /// </summary>
        private string _ConditionOrderCommitTime;
        /// <summary>
        /// 该笔单子的条件单（埋单）的提交时间
        /// </summary>
        public string ConditionOrderCommitTime
        {
            get { return _ConditionOrderCommitTime; }
            set { _ConditionOrderCommitTime = value; OnPropertyChanged("ConditionOrderCommitTime"); }
        }

        /// <summary>
        /// 最后成交时间
        /// </summary>
        private string _TradeTime;
        /// <summary>
        /// 最后成交时间
        /// </summary>
        public string TradeTime
        {
            get { return _TradeTime; }
            set { _TradeTime = value; OnPropertyChanged("TradeTime"); }
        }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        private string _UpdateTime;
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public string UpdateTime
        {
            get { return _UpdateTime; }
            set { _UpdateTime = value; OnPropertyChanged("UpdateTime"); }
        }

        /// <summary>
        /// 成交均价
        /// </summary>
        private double _AvgPx;
        /// <summary>
        /// 成交均价
        /// </summary>
        public double AvgPx
        {
            get { return _AvgPx; }
            set { _AvgPx = value; OnPropertyChanged("AvgPx"); }
        }

        /// <summary>
        /// 投机、套保
        /// </summary>
        private string _Hedge;
        /// <summary>
        /// 投机、套保
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
        /// 触发方式
        /// </summary>
        private string _TouchMethod;
        /// <summary>
        /// 触发方式
        /// </summary>
        public string TouchMethod
        {
            get { return _TouchMethod; }
            set { _TouchMethod = value; OnPropertyChanged("TouchMethod"); }
        }
        /// <summary>
        /// 触发条件（大于等于，小于等于）
        /// </summary>
        private string _TouchCondition;
        /// <summary>
        /// 触发条件（大于等于，小于等于）
        /// </summary>
        public string TouchCondition
        {
            get { return _TouchCondition; }
            set { _TouchCondition = value; OnPropertyChanged("TouchCondition"); }
        }
        /// <summary>
        /// 触发价格
        /// </summary>
        private string _TouchPrice;
        /// <summary>
        /// 触发价格
        /// </summary>
        public string TouchPrice
        {
            get { return _TouchPrice; }
            set { _TouchPrice = value; OnPropertyChanged("TouchPrice"); }
        }

        /// <summary>
        /// 成交编号
        /// </summary>
        private string _TradeID;
        /// <summary>
        /// 成交编号
        /// </summary>
        public string TradeID
        {
            get { return _TradeID; }
            set { _TradeID = value; OnPropertyChanged("TradeID"); }
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
        /// 报单类型
        /// </summary>
        private string _OrderType;
        /// <summary>
        /// 报单类型
        /// </summary>
        public string OrderType
        {
            get { return _OrderType; }
            set { _OrderType = value; OnPropertyChanged("OrderType"); }
        }

        /// <summary>
        /// 订单类型
        /// </summary>
        private string _EffectionType = "当日有效";
        /// <summary>
        /// 订单类型
        /// </summary>
        public string EffectionType
        {
            get { return _EffectionType; }
            set { _EffectionType = value; OnPropertyChanged("EffectiionType"); }
        }

        /// <summary>
        /// 定单类型
        /// </summary>
        private string _TaoliDan;
        /// <summary>
        /// 定单类型
        /// </summary>
        public string TaoliDan
        {
            get { return _TaoliDan; }
            set { _TaoliDan = value; OnPropertyChanged("TaoliDan"); }
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
        private string _BrokerOrderSeq;
        /// <summary>
        /// 主场单号
        /// </summary>
        public string BrokerOrderSeq
        {
            get { return _BrokerOrderSeq; }
            set
            {
                _BrokerOrderSeq = value;
                OnPropertyChanged("BrokerOrderSeq");
            }
        }

        /// <summary>
        /// 报单引用
        /// </summary>
        private string _OrderRef = "";
        /// <summary>
        /// 报单引用
        /// </summary>
        public string OrderRef
        {
            get { return _OrderRef; }
            set { _OrderRef = value; OnPropertyChanged("OrderRef"); }
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
        /// 相关报单编号
        /// </summary>
        private string _RelativeID;
        /// <summary>
        /// 相关报单编号
        /// </summary>
        public string RelativeID
        {
            get { return _RelativeID; }
            set { _RelativeID = value; OnPropertyChanged("RelativeID"); }
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

        public TradeOrderData Copy()
        {
            TradeOrderData ret = (TradeOrderData)this.MemberwiseClone();
            return ret;
        }

        public static int CompareByTradeTime(TradeOrderData o1, TradeOrderData o2)
        {
            if (o1 == null && o2 == null) return 0;
            if (o1 == null) return -1;
            if (o2 == null) return 1;

            if (o1.TradeTime != null && o2.TradeTime != null)
            {
                if (o1.TradeTime.StartsWith("2") && !o2.TradeTime.StartsWith("2"))
                {
                    return 1;
                }
                else if (!o1.TradeTime.StartsWith("2") && o2.TradeTime.StartsWith("2"))
                {
                    return -1;
                }

                if (o1.TradeTime.CompareTo(o2.TradeTime) < 0) return 1;
                if (o1.TradeTime.CompareTo(o2.TradeTime) == 0)
                {
                    if (o1._TradeID.CompareTo(o2._TradeID) == 1)
                    {
                        return -1;
                    }
                    else if (o1._TradeID.CompareTo(o2._TradeID) == -1)
                    {
                        return 1;
                    }
                    else if (o1._TradeID.CompareTo(o2._TradeID) == 0)
                    {
                        return 0;
                    }
                }
            }
            else
            {
                return -1;
            }
            return -1;
        }

        public static int CompareByCommitTime(TradeOrderData o1, TradeOrderData o2)
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
                if (o1._OrderID.CompareTo(o2._OrderID) == 1)
                {
                    return -1;
                }
                else if (o1._OrderID.CompareTo(o2._OrderID) == -1)
                {
                    return 1;
                }
                else if (o1._OrderID.CompareTo(o2._OrderID) == 0)
                {
                    return 0;
                }
            }
            return -1;
        }

        public static int CompareByOrderID(TradeOrderData o1, TradeOrderData o2)
        {
            if (o1 == null && o2 == null) return 0;
            if (o1 == null) return -1;
            if (o2 == null) return 1;

            if (o1._OrderID.CompareTo(o2._OrderID) == 1)
            {
                return -1;
            }
            else if (o1._OrderID.CompareTo(o2._OrderID) == -1)
            {
                return 1;
            }
            else if (o1._OrderID.CompareTo(o2._OrderID) == 0)
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

        public static int CompareByTradeID(TradeOrderData o1, TradeOrderData o2)
        {
            if (o1 == null && o2 == null) return 0;
            if (o1 == null) return -1;
            if (o2 == null) return 1;

            if (o1._TradeID.CompareTo(o2._TradeID) == 1)
            {
                return -1;
            }
            else if (o1._TradeID.CompareTo(o2._TradeID) == -1)
            {
                return 1;
            }
            else if (o1._TradeID.CompareTo(o2._TradeID) == 0)
            {
                if (o1.TradeTime != null && o2.TradeTime != null)
                {
                    if (o1.TradeTime.StartsWith("2") && !o2.TradeTime.StartsWith("2"))
                    {
                        return 1;
                    }
                    else if (!o1.TradeTime.StartsWith("2") && o2.TradeTime.StartsWith("2"))
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }

                if (o1.TradeTime.CompareTo(o2.TradeTime) < 0) return 1;
                if (o1.TradeTime.CompareTo(o2.TradeTime) == 0)
                {
                    return 0;
                }
            }
            return -1;
        }

        public static int CompareByCode(TradeOrderData o1, TradeOrderData o2)
        {
            if (o1 == null && o2 == null) return 0;
            if (o1 == null) return -1;
            if (o2 == null) return 1;

            if (o1._Code.Length > 6 && o2._Code.Length <= 6)
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
                    if (o1._TradeHandCount.CompareTo(o2._TradeHandCount) > 0) return 1;
                    if (o1._TradeHandCount.CompareTo(o2._TradeHandCount) < 0) return -1;
                    if (o1._TradeHandCount.CompareTo(o2._TradeHandCount) == 0) return 0;
                }
            }
            return 1;
        }
    }
}
