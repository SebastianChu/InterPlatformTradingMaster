using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingMaster
{
    public class MarginStruct
    {
        /// <summary>
        /// 品种代码
        /// </summary>
        public string Code;
        /// <summary>
        /// 投资者代码
        /// </summary>
        public string InvestorID;
        /// <summary>
        /// 多头保证金率
        /// </summary>
        public double LongMarginRatioByMoney;
        /// <summary>
        /// 多头保证金费
        /// </summary>
        public double LongMarginRatioByVolume;
        /// <summary>
        /// 空头保证金率
        /// </summary>
        public double ShortMarginRatioByMoney;
        /// <summary>
        /// 空头保证金费
        /// </summary>
        public double ShortMarginRatioByVolume;
        /// <summary>
        /// 期权合约保证金不变部分
        /// </summary>
        public double FixedMargin;
        /// <summary>
        /// 期权合约最小保证金
        /// </summary>
        public double MiniMargin;
        /// <summary>
        /// 期权合约权利金
        /// </summary>
        public double Royalty;

        public static bool IsInitiatedValue(string type, MarginStruct mStruct)
        {
            if (type.Contains("Futures") && mStruct.LongMarginRatioByMoney == 0 && mStruct.LongMarginRatioByVolume == 0 && mStruct.ShortMarginRatioByMoney == 0 && mStruct.ShortMarginRatioByVolume == 0
            || (type.Contains("Option")  && mStruct.FixedMargin == 0 && mStruct.MiniMargin == 0 && mStruct.MiniMargin == 0))
            {
                return true;
            }
            return false;
        }
    }
}
