using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingMaster.CodeSet;

namespace TradingMaster
{
    /// <summary>
    /// 委托状态
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 埋单
        /// </summary>
        MaiDan = 11,
        /// <summary>
        /// 埋单删除
        /// </summary>
        MaiDanDelete = 12,
        /// <summary>
        /// 埋单已经触发
        /// </summary>
        MaiDanTouch = 13,
        /// <summary>
        /// 埋单拒绝
        /// </summary>
        MaiDanReject = 14,

        /// <summary>
        /// 自动单
        /// </summary>
        Auto = 21,
        /// <summary>
        /// 自动单删除
        /// </summary>
        AutoDelete = 22,
        /// <summary>
        /// 自动单已经触发
        /// </summary>
        AutoTouch = 23,
        /// <summary>
        /// 自动单拒绝
        /// </summary>
        AutoReject = 24,

        /// <summary>
        /// 已经提交到服务端
        /// </summary>
        Submitted = 31,
        /// <summary>
        /// 非条件单,未触发
        /// </summary>
        NotTouched = 32,
        /// <summary>
        /// 条件单
        /// </summary>
        Condition = 33,
        /// <summary>
        /// 条件单删除
        /// </summary>
        ConditionDelete = 34,
        /// <summary>
        /// 条件单已经触发
        /// </summary>
        ConditionTouch = 35,
        /// <summary>
        /// 条件单拒绝
        /// </summary>
        ConditionReject = 36,
        /// <summary>
        /// 自动生成的已经发送的条件单
        /// </summary>
        ConditionSubmmited = 37,

        /// <summary>
        /// 已排队
        /// </summary>
        Queued = 91,
        /// <summary>
        /// 部分成交
        /// </summary>
        PartChengjiao = 92,
        /// <summary>
        /// 待撤
        /// </summary>
        PendingCancel = 93,
        /// <summary>
        /// 完全成交
        /// </summary>
        Chengjiao = 94,
        /// <summary>
        /// 已撤单
        /// </summary>
        Cancelled = 95,
        /// <summary>
        /// 指令失败
        /// </summary>
        Failed = 99
    }

    /// <summary>
    /// 开平仓
    /// </summary>
    public enum PosEffect
    {
        Unknown = 0,
        Open = 1,             //开仓
        Close = 2,            //平仓
        CloseToday = 3,      //平今
        CloseYesterday = 4,  //平昨
        ForceClose = 5,       //强平
        ForceOff = 6,         //强减
        LocalForceClose = 7   //本地强平

    }

    /// <summary>
    /// 报单指令类型
    /// </summary>
    public enum EnumOrderType : byte
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 限价
        /// </summary>
        Limit = 1,

        /// <summary>
        /// 市价
        /// </summary>
        Market = 2,

        /// <summary>
        /// 集合竞价
        /// </summary>
        Opening = 3,

        /// <summary>
        /// 立即成交剩余指令自动撤销
        /// </summary>
        FAK = 4,

        /// <summary>
        /// 立即全部成交否则自动撤销
        /// </summary>
        FOK = 5,

        /// <summary>
        /// 限价止损
        /// </summary>
        StopLimit = 6,

        /// <summary>
        /// 触及市价
        /// </summary>
        MIT = 7,

        /// <summary>
        /// 市价转限价
        /// </summary>
        MaketToLimit = 8,

        /// <summary>
        /// 五档市价
        /// </summary>
        FiveLevel = 9,

        /// <summary>
        /// 五档市价转限价
        /// </summary>
        FiveLevelToLimit = 10,

        /// <summary>
        /// 最优价
        /// </summary>
        Best = 11,

        /// <summary>
        /// 最优价转限价
        /// </summary>
        BestToLimit = 12

    }

    /// <summary>
    /// 投机套保套利标志
    /// </summary>
    public enum EnumHedgeType
    {
        Speculation = 1,
        Arbitrage = 2,
        Hedge = 3
    }
}
