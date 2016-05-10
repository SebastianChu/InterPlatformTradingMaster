using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingMaster
{
    public class CommissionStruct
    {
        /// <summary>
        /// 品种代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 投资者代码
        /// </summary>
        public string InvestorID { get; set; }
        /// <summary>
        /// 开仓手续费率
        /// </summary>
        public double OpenRatioByMoney { get; set; }
        /// <summary>
        /// 开仓手续费
        /// </summary>
        public double OpenRatioByVolume { get; set; }
        /// <summary>
        /// 平仓手续费率
        /// </summary>
        public double CloseRatioByMoney { get; set; }
        /// <summary>
        /// 平仓手续费
        /// </summary>
        public double CloseRatioByVolume { get; set; }
        /// <summary>
        /// 平今手续费率
        /// </summary>
        public double CloseTodayRatioByMoney { get; set; }
        /// <summary>
        /// 平今手续费
        /// </summary>
        public double CloseTodayRatioByVolume { get; set; }
        /// <summary>
        /// 执行手续费率
        /// </summary>
        public double StrikeRatioByMoney { get; set; }
        /// <summary>
        /// 执行手续费
        /// </summary>
        public double StrikeRatioByVolume { get; set; }
    }
}
