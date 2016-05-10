using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace TradingMaster
{
    public class ExecOrderData : INotifyPropertyChanged
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
        /// 行权编号
        /// </summary>
        private string _ExecOrderID;
        /// <summary>
        /// 行权编号
        /// </summary>
        public string ExecOrderID
        {
            get { return _ExecOrderID; }
            set
            {
                _ExecOrderID = value;
                OnPropertyChanged("ExecOrderID");
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
        /// 行权引用
        /// </summary>
        private string _ExecOrderRef = "";
        /// <summary>
        /// 行权引用
        /// </summary>
        public string ExecOrderRef
        {
            get { return _ExecOrderRef; }
            set { _ExecOrderRef = value; OnPropertyChanged("ExecOrderRef"); }
        }

        /// <summary>
        /// 执行类型
        /// </summary>
        private string _ActionDirection;
        /// <summary>
        /// 行权引用
        /// </summary>
        public string ActionDirection
        {
            get { return _ActionDirection; }
            set { _ActionDirection = value; OnPropertyChanged("ActionDirection"); }
        }

        /// <summary>
        /// 行权状态
        /// </summary>
        private string _ExecStatus;
        /// <summary>
        /// 行权状态
        /// </summary>
        public string ExecStatus
        {
            get { return _ExecStatus; }
            set { _ExecStatus = value; OnPropertyChanged("ExecStatus"); }
        }

        /// <summary>
        /// 行权结果
        /// </summary>
        private string _Result;
        /// <summary>
        /// 行权结果
        /// </summary>
        public string Result
        {
            get { return _Result; }
            set { _Result = value; OnPropertyChanged("Result"); }
        }
        
        /// <summary>
        /// 开平状态
        /// </summary>
        private string _OpenClose;
        /// <summary>
        /// 开平状态
        /// </summary>
        public string OpenClose
        {
            get { return _OpenClose; }
            set { _OpenClose = value; OnPropertyChanged("OpenClose"); }
        }

        /// <summary>
        /// 行权手数
        /// </summary>
        private int _HandCount;
        /// <summary>
        /// 行权手数
        /// </summary>
        public int HandCount
        {
            get { return _HandCount; }
            set { _HandCount = value; OnPropertyChanged("HandCount"); }//OnPropertyChanged("UnTradeHandCount"); }
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
        private string _BrokerExecOrderSeq;
        /// <summary>
        /// 主场单号
        /// </summary>
        public string BrokerExecOrderSeq
        {
            get { return _BrokerExecOrderSeq; }
            set
            {
                _BrokerExecOrderSeq = value;
                OnPropertyChanged("BrokerExecOrderSeq");
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
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        public QuoteOrderData Copy()
        {
            QuoteOrderData ret = (QuoteOrderData)this.MemberwiseClone();
            return ret;
        }

        //public static int CompareByTradeTime(QuoteOrderData o1, QuoteOrderData o2)
        //{
        //    if (o1 == null && o2 == null) return 0;
        //    if (o1 == null) return -1;
        //    if (o2 == null) return 1;

        //    if (o1.TradeTime != null && o2.TradeTime != null)
        //    {
        //        if (o1.TradeTime.StartsWith("2") && !o2.TradeTime.StartsWith("2"))
        //        {
        //            return 1;
        //        }
        //        else if (!o1.TradeTime.StartsWith("2") && o2.TradeTime.StartsWith("2"))
        //        {
        //            return -1;
        //        }
        //    }
        //    else
        //    {
        //        return -1;
        //    }

        //    if (o1.TradeTime.CompareTo(o2.TradeTime) < 0) return 1;
        //    if (o1.TradeTime.CompareTo(o2.TradeTime) == 0)
        //    {
        //        if (o1.orderid.CompareTo(o2.orderid) == 1)
        //        {
        //            return -1;
        //        }
        //        else if (o1.orderid.CompareTo(o2.orderid) == -1)
        //        {
        //            return 1;
        //        }
        //        else if (o1.orderid.CompareTo(o2.orderid) == 0)
        //        {
        //            return 0;
        //        }
        //    }
        //    return -1;
        //}

        public static int CompareByCommitTime(ExecOrderData o1, ExecOrderData o2)
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
                if (o1._ExecOrderID.CompareTo(o2._ExecOrderID) == 1)
                {
                    return -1;
                }
                else if (o1._ExecOrderID.CompareTo(o2._ExecOrderID) == -1)
                {
                    return 1;
                }
                else if (o1._ExecOrderID.CompareTo(o2._ExecOrderID) == 0)
                {
                    return 0;
                }
            }
            return -1;
        }
    }
}
