using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader
{
    public static class HSStruct
    {
        #region 价格模式
        public const string 普通限价盘 = "0";
        public const string 限价FAK_立即成交剩余自动撤销 = "6";
        public const string 限价FOK_即时全额否则取消 = "7";
        #endregion

        #region 交易类别
        public const string 前台未知交易所 = "0";
        public const string 上海 = "1";
        public const string 深圳 = "2";
        public const string 中金所 = "5";
        public const string 郑州交易所 = "a";
        public const string 大连交易所 = "b";
        public const string 上期所 = "c";
        public const string 沪港通 = "G";
        #endregion

        #region 委托方向
        public const string 股票买入 = "1";
        public const string 股票卖出 = "2";
        public const string 开基申购 = "3";
        public const string 开基赎回 = "4";
        public const string 开基认购 = "5";
        public const string 权证行权 = "6";
        public const string 债券买入 = "A";
        public const string 债券卖出 = "B";
        public const string 质押出库 = "(";
        public const string 质押入库 = ")";
        public const string 融资回购 = "C";
        public const string 融券回购 = "D";
        public const string 沪债转股 = "G";
        public const string 深债转股 = "H";
        public const string 转债回售 = "J";
        public const string 网络投票 = "T";


        public const string 开仓 = "[";
        public const string 平仓 = "]";
        public const string 上海申购 = "g";
        public const string 深圳申购 = "h";
        #endregion

        #region 期货委托买卖类别
        public const string 买入 = "1";
        public const string 卖出 = "2";
        public const string 期货开仓 = "1";
        public const string 期货平仓 = "2";
        public const string 期货平今仓 = "4";
        #endregion

        #region 委托属性
        public const string 买卖 = "0";
        public const string 配股 = "1";
        public const string 转托 = "2";
        public const string 申购 = "3";
        public const string 回购 = "4";
        public const string 配售 = "5";
        public const string 指定 = "6";
        public const string 转股 = "7";
        public const string 回售 = "8";
        public const string 股息 = "9";
        public const string 深圳配售确认 = "A";
        public const string 配售放弃 = "B";
        public const string 无冻质押 = "C";
        public const string 冻结质押 = "D";
        public const string 无冻解押 = "E";
        public const string 解冻解押 = "F";
        public const string 投票 = "H";
        public const string 要约收购预受 = "I";
        public const string 预受要约解除 = "J";
        public const string 基金设红 = "K";
        public const string 跨市转托 = "M";
        public const string 权证行权属性 = "P";
        public const string 对手方最优价格 = "Q";
        public const string 最优五档即时成交剩余转限价 = "R";
        public const string 本方最优价格 = "S";
        public const string 即时成交剩余撤销 = "T";
        public const string 最优五档即时成交剩余撤销 = "U";
        public const string 全额成交并撤单 = "V";
        public const string 质押出入库 = "f";
        public const string 限价FOK = "W";
        public const string 市价FOK = "X";
        public const string 市价订单剩余转限价 = "Y";
        public const string 市价订单剩余撤销 = "Z";
        public const string 个股期权标的证券锁定解锁 = "a";

        #endregion

        #region 委托方式
        public const string internet委托 = "7";
        #endregion

        #region 批量标志
        public const string 单笔 = "0";
        public const string 批量 = "1";
        #endregion

        #region 排序方式
        public const string 正常 = "0";
        public const string 倒序 = "1";
        #endregion

        #region 查询方向
        public const string 往后翻_倒序 = "0";
        public const string 往前翻_顺序 = "1";
        #endregion

        #region 查询模式
        public const int 明细 = 0;
        public const int 汇总 = 1;
        #endregion

        #region 币种
        public const string 人民币 = "0";
        public const string 美圆 = "1";
        public const string 港币 = "2";
        #endregion

        #region 确认状态
        public const string 未确认 = "0";
        public const string 已确认 = "1";
        #endregion

        #region 委托状态
        public const string 未报 = "0";
        public const string 待报 = "1";
        public const string 已报 = "2";
        public const string 已报待撤 = "3";
        public const string 部成待撤 = "4";
        public const string 部撤 = "5";
        public const string 已撤 = "6";
        public const string 部成 = "7";
        public const string 已成 = "8";
        public const string 废单 = "9";
        public const string 撤废 = "A";
        public const string 待明确 = "W";
        #endregion
    }
}
