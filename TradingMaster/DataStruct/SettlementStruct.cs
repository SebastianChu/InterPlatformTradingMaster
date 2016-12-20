using System.Collections.Generic;

namespace TradingMaster
{
    public class SettlementStruct
    {
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
            set { _BackEnd = value; }
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
            }
        }

        /// <summary>
        /// 经纪公司代码
        /// </summary>
        private string _BrokerID;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        public string BrokerID
        {
            get { return _BrokerID; }
            set
            {
                _BrokerID = value;
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
            }
        }

        /// <summary>
        /// 结算单字节链表
        /// </summary>
        private List<byte> _SettlementInfoCharList;
        /// <summary>
        /// 结算单字节链表
        /// </summary>
        public List<byte> SettlementInfoCharList
        {
            get { return _SettlementInfoCharList; }
            set
            {
                _SettlementInfoCharList = value;
            }
        }

        public SettlementStruct()
        {
            _SettlementInfoCharList = new List<byte>();
        }
    }
}
