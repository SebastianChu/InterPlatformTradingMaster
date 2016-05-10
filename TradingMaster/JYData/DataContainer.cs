using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TradingMaster.CodeSet;
using TradingMaster.Control;

namespace TradingMaster.JYData
{
    public class DataContainer
    {
        private static Dictionary<Contract, RealData> RealDataContainer = new Dictionary<Contract, RealData>();

        public static void SetRealDataToContainer(RealData realData)
        {
            Contract codeKey = CodeSetManager.GetContractInfo(realData.CodeInfo.Code, realData.CodeInfo.ExchCode);
            if (codeKey != null)
            {
                if (RealDataContainer.ContainsKey(codeKey))
                {
                    RealDataContainer[codeKey] = realData;
                }
                else
                {
                    RealDataContainer.Add(codeKey, realData);
                }
            }
            else
            {
                Util.Log("Warning!: codeKey is NULL! ");
            }
        }

        public static RealData GetRealDataFromContainer(Contract codeKey)
        {
            if (codeKey != null)
            {
                if (RealDataContainer.ContainsKey(codeKey))
                {
                    return RealDataContainer[codeKey];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                Util.Log("Warning!: codeKey is NULL! ");
                return null;
            }
        }

        /// <summary>
        /// 交易前置连接
        /// </summary>
        protected bool IsLoggedOn = false;
        protected bool IsConnected = false;

        /// <summary>
        /// 已提交报单记录
        /// </summary>
        protected List<string> CommitOrders = new List<string>();
        protected List<string> TransactOrders = new List<string>();
        protected List<string> CancelOrders = new List<string>();

        /// <summary>
        /// 存储/维护容器
        /// </summary>
        protected List<ExecOrderData> QryExecOrderDataLst = new List<ExecOrderData>();
        protected List<QuoteOrderData> QryQuoteOrderDataLst = new List<QuoteOrderData>();
        protected List<Q7JYOrderData> PreOrderData = new List<Q7JYOrderData>();
        protected List<Q7JYOrderData> QryTradeDataLst = new List<Q7JYOrderData>();
        protected List<Q7PosInfoDetail> QryPosDetailData = new List<Q7PosInfoDetail>();
        protected List<Q7PosInfoTotal> JYPosSumData = new List<Q7PosInfoTotal>();

        /// <summary>
        /// 其他查询数据
        /// </summary>
        protected CapitalInfo JYCapitalData = new CapitalInfo();
        protected SettlementStruct SettlementInfo = new SettlementStruct();
        protected List<string> BankCardList = new List<string>();
        protected List<BankAccountInfo> QryBankAcctInfoLst = new List<BankAccountInfo>();
        protected List<TransferSingleRecord> QryTransferRecords = new List<TransferSingleRecord>();

        /// <summary>
        /// 启动初始化时的报单回报标志位和记录
        /// </summary>
        protected bool TempOrderFlag;
        protected List<Q7JYOrderData> TempOrderData = new List<Q7JYOrderData>();

        /// <summary>
        /// 启动初始化时的成交回报标志位和记录
        /// </summary>
        protected bool TempTradeFlag;
        protected List<Q7JYOrderData> TempTradeData = new List<Q7JYOrderData>();

        /// <summary>
        /// 启动初始化时的成交回报的持仓标志位
        /// </summary>
        protected bool TempPosFlag;

        /// <summary>
        /// 启动初始化时的报价回报标志位和记录
        /// </summary>
        protected bool TempQuoteInsertFlag;
        protected List<QuoteOrderData> TempQuoteOrderData = new List<QuoteOrderData>();

        /// <summary>
        /// 启动初始化时的行权申请标志位和记录
        /// </summary>
        protected bool TempExecFlag;
        protected List<ExecOrderData> TempExecData = new List<ExecOrderData>();

        /// <summary>
        /// 报单记录更新
        /// </summary>
        protected Dictionary<string, Q7JYOrderData> QryOrderDataDic = new Dictionary<string, Q7JYOrderData>();

        /// <summary>
        /// 报价记录更新
        /// </summary>
        protected Dictionary<string, QuoteOrderData> QryQuoteOrderDataDic = new Dictionary<string, QuoteOrderData>();

        /// <summary>
        /// 执行记录更新
        /// </summary>
        protected Dictionary<string, ExecOrderData> QryExecOrderDataDic = new Dictionary<string, ExecOrderData>();

        /// <summary>
        /// 成交记录更新
        /// </summary>
        //protected static object TradeRecLocker = new object();

        /// <summary>
        /// 持仓相关锁
        /// </summary>
        protected static object ServerLock;

        /// <summary>
        /// 用于记录行权信息中未执行的平仓单
        /// </summary>
        //private Dictionary<string, List<ExecOrderData>> _FreezeExecOrder = new Dictionary<string, List<ExecOrderData>>();
        protected Dictionary<string, int> FreezeExecOrderHandCount = new Dictionary<string, int>();

        /// <summary>
        /// 预存撤单后新平仓报单
        /// </summary>
        protected List<PosInfoOrder> _OrderAfterCancelList = new List<PosInfoOrder>();
        protected static object _CancelCloseLocker = new object();

        /// <summary>
        /// 预存反手开仓报单
        /// </summary>
        protected List<PosInfoOrder> ResetOrderList = new List<PosInfoOrder>();
        protected Dictionary<PosInfoOrder, int> ReOpenOrderDict = new Dictionary<PosInfoOrder, int>();
        protected static object ResetOrderLocker = new object();
    }

    public class PosHandCount
    {
        /// <summary>
        /// 今仓多少手，买
        /// </summary>
        public int TodayHandCountBuy = 0;
        /// <summary>
        /// 昨仓多少手，买
        /// </summary>
        public int LastDayHandCountBuy = 0;

        /// <summary>
        /// 今仓多少手，卖
        /// </summary>
        public int TodayHandCountSell = 0;
        /// <summary>
        /// 昨仓多少手，卖
        /// </summary>
        public int LastDayHandCountSell = 0;

        /// <summary>
        /// 准备平今仓多少手(多)，冻结住的。
        /// </summary>
        public int TodayBuyFrozen = 0;

        /// <summary>
        /// 准备平今仓多少手(空)，冻结住的。
        /// </summary>
        public int TodaySellFrozen = 0;

        /// <summary>
        /// 准备平昨仓多少手(多)，冻结住的。
        /// </summary>
        public int LastDayBuyFrozen = 0;

        /// <summary>
        /// 准备平昨仓多少手(空)，冻结住的。
        /// </summary>
        public int LastDaySellFrozen = 0;

        public override string ToString()
        {
            String ret = "";
            ret += "昨买:" + LastDayHandCountBuy.ToString() + " 昨卖:" + LastDayHandCountSell.ToString() + " 今买:" + TodayHandCountBuy.ToString() + " 今卖:" + TodayHandCountSell.ToString();
            ret += " 昨买冻:" + LastDayBuyFrozen.ToString() + " 昨卖冻:" + LastDaySellFrozen.ToString() + " 今买冻:" + TodayBuyFrozen.ToString() + " 今卖冻:" + TodaySellFrozen.ToString();
            return ret;
        }
    }
}
