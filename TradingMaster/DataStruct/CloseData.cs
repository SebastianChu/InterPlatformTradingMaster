using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingMaster
{
    /// <summary>
    /// 平仓数据
    /// </summary>
    public class CloseData
    {
        /// <summary>
        /// 平仓编号
        /// </summary>
        private string closeId;
        /// <summary>
        /// 平仓编号
        /// </summary>
        public string CloseId
        {
            get { return closeId; }
            set { closeId = value; }
        }

        /// <summary>
        /// 合约
        /// </summary>
        private string code;
        /// <summary>
        /// 合约
        /// </summary>
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        /// <summary>
        /// 买卖
        /// </summary>
        private string buysell;
        /// <summary>
        /// 买卖
        /// </summary>
        public string BuySell
        {
            get { return buysell; }
            set { buysell = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        private int handCount;
        /// <summary>
        /// 数量
        /// </summary>
        public int HandCount
        {
            get { return handCount; }
            set { handCount = value; }
        }

        /// <summary>
        /// 平仓价
        /// </summary>
        private double closePrice;
        /// <summary>
        /// 平仓价
        /// </summary>
        public double ClosePrice
        {
            get { return closePrice; }
            set { closePrice = value; }
        }

        /// <summary>
        /// 开仓价
        /// </summary>
        private double openPrice;
        /// <summary>
        /// 开仓价
        /// </summary>
        public double OpenPrice
        {
            get { return openPrice; }
            set { openPrice = value; }
        }

        /// <summary>
        /// 投保
        /// </summary>
        private string hedge;
        /// <summary>
        /// 投保
        /// </summary>
        public string Hedge
        {
            get { return hedge; }
            set { hedge = value; }
        }


        /// <summary>
        /// 平仓总盈亏
        /// </summary>
        private double closeTotalYK;
        /// <summary>
        /// 平仓总盈亏
        /// </summary>
        public double CloseTotalYK
        {
            get { return closeTotalYK; }
            set { closeTotalYK = value; }
        }

        /// <summary>
        /// 平仓盯市盈亏
        /// </summary>
        private double clostDSYK;
        /// <summary>
        /// 平仓盯市盈亏
        /// </summary>
        public double ClostDSYK
        {
            get { return clostDSYK; }
            set { clostDSYK = value; }
        }

        /// <summary>
        /// 平仓日期
        /// </summary>
        private string closeDate;
        /// <summary>
        /// 平仓日期
        /// </summary>
        public string CloseDate
        {
            get { return closeDate; }
            set { closeDate = value; }
        }


        public CloseData Copy()
        {
            CloseData ret = (CloseData)this.MemberwiseClone();
            return ret;
        }

    }
}
