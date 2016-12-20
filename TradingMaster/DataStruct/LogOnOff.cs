namespace TradingMaster
{
    public class LogonStruct
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
        /// 前置类型
        /// </summary>
        private string _FrontType;
        /// <summary>
        /// 前置类型
        /// </summary>
        public string FrontType
        {
            get { return _FrontType; }
            set
            {
                _FrontType = value;
            }
        }

        // <summary>
        /// 交易所时间
        /// </summary>
        private ExchangeTime _ExchTime;
        /// <summary>
        /// 交易所时间
        /// </summary>
        public ExchangeTime ExchTime
        {
            get { return _ExchTime; }
            set
            {
                _ExchTime = value;
            }
        }
    }

    public class LogoffStruct
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
        /// 前置类型
        /// </summary>
        private string _FrontType;
        /// <summary>
        /// 前置类型
        /// </summary>
        public string FrontType
        {
            get { return _FrontType; }
            set
            {
                _FrontType = value;
            }
        }

        /// <summary>
        /// 退出信息
        /// </summary>
        private string _OutMsg;
        /// <summary>
        /// 退出信息
        /// </summary>
        public string OutMsg
        {
            get { return _OutMsg; }
            set
            {
                _OutMsg = value;
            }
        }
    }

    public class DisconnectStruct
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
        /// 前置类型
        /// </summary>
        private string _FrontType;
        /// <summary>
        /// 前置类型
        /// </summary>
        public string FrontType
        {
            get { return _FrontType; }
            set
            {
                _FrontType = value;
            }
        }
    }
}
