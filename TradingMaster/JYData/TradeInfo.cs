using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingMaster.CodeSet;

namespace TradingMaster
{
    /// <summary>
    /// 后台接口类型
    /// </summary>
    public enum BACKENDTYPE
    {
        UNKNOWN = 0,
        CTP = 1,
        HST2 = 2,
        FEMAS = 3,
        XSPEED = 4
    }

    /// <summary>
    /// 买或者卖
    /// </summary>
    public enum SIDETYPE
    {
        BUY = 1,
        SELL = 2
    }

    /// <summary>
    /// 最大的可操作数
    /// </summary>
    public class MaxOperation//TODO: data type
    {
        public Contract CodeInfo;
        public SIDETYPE Side;
        public PosEffect PosEffect;
        public int Count;
    }
}
