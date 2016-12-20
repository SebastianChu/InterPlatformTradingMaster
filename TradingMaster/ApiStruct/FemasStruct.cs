using System.Runtime.InteropServices;

namespace TradingMaster
{
    /// <summary>
    /// 系统用户登录请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcReqUserLoginField
    {
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
		/// 交易用户代码
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
		/// 密码
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 用户端产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string UserProductInfo;
        /// <summary>
        /// 接口端产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string InterfaceProductInfo;
        /// <summary>
        /// 协议信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string ProtocolInfo;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
        /// <summary>
        /// 数据中心代码
        /// </summary>
        public int DataCenterID;
    };

    /// <summary>
    /// 系统用户登录应答
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcRspUserLoginField
    {
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 登录成功时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LoginTime;
        /// <summary>
        /// 用户最大本地报单号地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MaxOrderLocalID;
        /// <summary>
        /// 交易系统名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 61)]
        public string TradingSystemName;
        /// <summary>
        /// 数据中心代码
        /// </summary>
        public int DataCenterID;
        /// <summary>
        /// 会员私有流当前长度
        /// </summary>
        public int PrivateFlowSize;
        /// <summary>
        /// 交易员私有流当前长度
        /// </summary>
        public int UserFlowSize;
    };

    /// <summary>
    /// 用户登出请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcReqUserLogoutField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
    };

    /// <summary>
    /// 用户登出请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcRspUserLogoutField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
    };

    /// <summary>
    /// 强制用户退出
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcForceUserExitField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
    };

    /// <summary>
    /// 用户口令修改
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcUserPasswordUpdateField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 旧密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string OldPassword;
        /// <summary>
        /// 新密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string NewPassword;
    };

    /// <summary>
    /// 输入报单
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcInputOrderField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 系统报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string OrderSysID;
        /// <summary>
        /// 投资者编号
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 用户本地报单号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string UserOrderLocalID;
        /// <summary>
        /// 报单类型
        /// </summary>
        public EnumUstpOrderPriceTypeType OrderPriceType;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumUstpDirectionType Direction;
        /// <summary>
        /// 开平标志
        /// </summary>
        public EnumUstpOffsetFlagType OffsetFlag;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumUstpHedgeFlagType HedgeFlag;
        /// <summary>
        /// 价格
        /// </summary>
        public double LimitPrice;
        /// <summary>
        /// 数量
        /// </summary>
        public int Volume;
        /// <summary>
        /// 有效期类型
        /// </summary>
        public EnumUstpTimeConditionType TimeCondition;
        /// <summary>
        /// GTD日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string GTDDate;
        /// <summary>
        /// 成交量类型
        /// </summary>
        public EnumUstpVolumeConditionType VolumeCondition;
        /// <summary>
        /// 最小成交量
        /// </summary>
        public int MinVolume;
        /// <summary>
        /// 止损价
        /// </summary>
        public double StopPrice;
        /// <summary>
        /// 强平原因
        /// </summary>
        public EnumUstpForceCloseReasonType ForceCloseReason;
        /// <summary>
        /// 自动挂起标志
        /// </summary>
        public int IsAutoSuspend;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 用户自定义域
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
        public string UserCustom;
    };

    /// <summary>
    /// 报单操作
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcOrderActionField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string OrderSysID;
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者编号
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 本次撤单操作的本地编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string UserOrderActionLocalID;
        /// <summary>
        /// 被撤订单的本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string UserOrderLocalID;
        /// <summary>
        /// 报单操作标志
        /// </summary>
        public EnumUstpActionFlagType ActionFlag;
        /// <summary>
        /// 价格
        /// </summary>
        public double LimitPrice;
        /// <summary>
        /// 数量变化
        /// </summary>
        public int VolumeChange;
    };

    /// <summary>
    /// 内存表导出
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcMemDbField
    {
        /// <summary>
        /// 内存表名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 61)]
        public string MemTableName;
    };

    /// <summary>
    /// 响应信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcRspInfoField
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
    };

    /// <summary>
    /// 报单查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcQryOrderField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string OrderSysID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    };

    /// <summary>
    /// 成交查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcQryTradeField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 成交编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TradeID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    };

    /// <summary>
    /// 合约查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcQryInstrumentField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 产品代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ProductID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    };

    /// <summary>
    /// 合约查询应答
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcRspInstrumentField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 品种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ProductID;
        /// <summary>
        /// 品种名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string ProductName;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 合约名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string InstrumentName;
        /// <summary>
        /// 交割年份
        /// </summary>
        public int DeliveryYear;
        /// <summary>
        /// 交割月
        /// </summary>
        public int DeliveryMonth;
        /// <summary>
        /// 限价单最大下单量
        /// </summary>
        public int MaxLimitOrderVolume;
        /// <summary>
        /// 限价单最小下单量
        /// </summary>
        public int MinLimitOrderVolume;
        /// <summary>
        /// 市价单最大下单量
        /// </summary>
        public int MaxMarketOrderVolume;
        /// <summary>
        /// 市价单最小下单量
        /// </summary>
        public int MinMarketOrderVolume;
        /// <summary>
        /// 数量乘数
        /// </summary>
        public int VolumeMultiple;
        /// <summary>
        /// 报价单位
        /// </summary>
        public double PriceTick;
        /// <summary>
        /// 币种
        /// </summary>
        public EnumUstpCurrencyType Currency;
        /// <summary>
        /// 多头限仓
        /// </summary>
        public int LongPosLimit;
        /// <summary>
        /// 空头限仓
        /// </summary>
        public int ShortPosLimit;
        /// <summary>
        /// 跌停板价
        /// </summary>
        public double LowerLimitPrice;
        /// <summary>
        /// 涨停板价
        /// </summary>
        public double UpperLimitPrice;
        /// <summary>
        /// 昨结算
        /// </summary>
        public double PreSettlementPrice;
        /// <summary>
        /// 合约交易状态
        /// </summary>
        public EnumUstpInstrumentStatusType InstrumentStatus;
        /// <summary>
        /// 创建日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string CreateDate;
        /// <summary>
        /// 上市日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string OpenDate;
        /// <summary>
        /// 到期日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExpireDate;
        /// <summary>
        /// 开始交割日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string StartDelivDate;
        /// <summary>
        /// 最后交割日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string EndDelivDate;
        /// <summary>
        /// 挂牌基准价
        /// </summary>
        public double BasisPrice;
        /// <summary>
        /// 当前是否交易
        /// </summary>
        public int IsTrading;
        /// <summary>
        /// 基础商品代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string UnderlyingInstrID;
        /// <summary>
        /// 基础商品乘数
        /// </summary>
        public int UnderlyingMultiple;
        /// <summary>
        /// 持仓类型
        /// </summary>
        public EnumUstpPositionTypeType PositionType;
        /// <summary>
        /// 执行价
        /// </summary>
        public double StrikePrice;
        /// <summary>
        /// 期权类型
        /// </summary>
        public EnumUstpOptionsTypeType OptionsType;
    };

    /// <summary>
    /// 合约状态
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcInstrumentStatusField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 品种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ProductID;
        /// <summary>
        /// 品种名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string ProductName;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 合约名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string InstrumentName;
        /// <summary>
        /// 交割年份
        /// </summary>
        public int DeliveryYear;
        /// <summary>
        /// 交割月
        /// </summary>
        public int DeliveryMonth;
        /// <summary>
        /// 限价单最大下单量
        /// </summary>
        public int MaxLimitOrderVolume;
        /// <summary>
        /// 限价单最小下单量
        /// </summary>
        public int MinLimitOrderVolume;
        /// <summary>
        /// 市价单最大下单量
        /// </summary>
        public int MaxMarketOrderVolume;
        /// <summary>
        /// 市价单最小下单量
        /// </summary>
        public int MinMarketOrderVolume;
        /// <summary>
        /// 数量乘数
        /// </summary>
        public int VolumeMultiple;
        /// <summary>
        /// 报价单位
        /// </summary>
        public double PriceTick;
        /// <summary>
        /// 币种
        /// </summary>
        public EnumUstpCurrencyType Currency;
        /// <summary>
        /// 多头限仓
        /// </summary>
        public int LongPosLimit;
        /// <summary>
        /// 空头限仓
        /// </summary>
        public int ShortPosLimit;
        /// <summary>
        /// 跌停板价
        /// </summary>
        public double LowerLimitPrice;
        /// <summary>
        /// 涨停板价
        /// </summary>
        public double UpperLimitPrice;
        /// <summary>
        /// 昨结算
        /// </summary>
        public double PreSettlementPrice;
        /// <summary>
        /// 合约交易状态
        /// </summary>
        public EnumUstpInstrumentStatusType InstrumentStatus;
        /// <summary>
        /// 创建日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string CreateDate;
        /// <summary>
        /// 上市日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string OpenDate;
        /// <summary>
        /// 到期日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExpireDate;
        /// <summary>
        /// 开始交割日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string StartDelivDate;
        /// <summary>
        /// 最后交割日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string EndDelivDate;
        /// <summary>
        /// 挂牌基准价
        /// </summary>
        public double BasisPrice;
        /// <summary>
        /// 当前是否交易
        /// </summary>
        public int IsTrading;
        /// <summary>
        /// 基础商品代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string UnderlyingInstrID;
        /// <summary>
        /// 基础商品乘数
        /// </summary>
        public int UnderlyingMultiple;
        /// <summary>
        /// 持仓类型
        /// </summary>
        public EnumUstpPositionTypeType PositionType;
        /// <summary>
        /// 执行价
        /// </summary>
        public double StrikePrice;
        /// <summary>
        /// 期权类型
        /// </summary>
        public EnumUstpOptionsTypeType OptionsType;
    };

    /// <summary>
    /// 投资者资金查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcQryInvestorAccountField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
    };

    /// <summary>
    /// 投资者资金应答
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcRspInvestorAccountField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 资金帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 上次结算准备金
        /// </summary>
        public double PreBalance;
        /// <summary>
        /// 入金金额
        /// </summary>
        public double Deposit;
        /// <summary>
        /// 出金金额
        /// </summary>
        public double Withdraw;
        /// <summary>
        /// 冻结的保证金
        /// </summary>
        public double FrozenMargin;
        /// <summary>
        /// 冻结手续费
        /// </summary>
        public double FrozenFee;
        /// <summary>
        /// 冻结权利金
        /// </summary>
        public double FrozenPremium;
        /// <summary>
        /// 手续费
        /// </summary>
        public double Fee;
        /// <summary>
        /// 平仓盈亏
        /// </summary>
        public double CloseProfit;
        /// <summary>
        /// 持仓盈亏
        /// </summary>
        public double PositionProfit;
        /// <summary>
        /// 可用资金
        /// </summary>
        public double Available;
        /// <summary>
        /// 多头冻结的保证金
        /// </summary>
        public double LongFrozenMargin;
        /// <summary>
        /// 空头冻结的保证金
        /// </summary>
        public double ShortFrozenMargin;
        /// <summary>
        /// 多头占用保证金
        /// </summary>
        public double LongMargin;
        /// <summary>
        /// 空头占用保证金
        /// </summary>
        public double ShortMargin;
        /// <summary>
        /// 当日释放保证金
        /// </summary>
        public double ReleaseMargin;
        /// <summary>
        /// 动态权益
        /// </summary>
        public double DynamicRights;
        /// <summary>
        /// 今日出入金
        /// </summary>
        public double TodayInOut;
        /// <summary>
        /// 占用保证金
        /// </summary>
        public double Margin;
        /// <summary>
        /// 期权权利金收支
        /// </summary>
        public double Premium;
        /// <summary>
        /// 风险度
        /// </summary>
        public double Risk;
    };

    /// <summary>
    /// 可用投资者查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcQryUserInvestorField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
    };

    /// <summary>
    /// 可用投资者
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcRspUserInvestorField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
    };

    /// <summary>
    /// 交易编码查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcQryTradingCodeField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
    };

    /// <summary>
    /// 交易编码查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcRspTradingCodeField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string ClientID;
        /// <summary>
        /// 客户代码权限
        /// </summary>
        public EnumUstpTradingRightType ClientRight;
        /// <summary>
        /// 是否活跃
        /// </summary>
        public EnumUstpIsActiveType IsActive;
    };

    /// <summary>
    /// 交易所查询请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcQryExchangeField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
    };

    /// <summary>
    /// 交易所查询应答
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcRspExchangeField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 交易所名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeName;
    };

    /// <summary>
    /// 投资者持仓查询请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcQryInvestorPositionField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    };

    /// <summary>
    /// 投资者持仓查询应答
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcRspInvestorPositionField
    {
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string ClientID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumUstpDirectionType Direction;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumUstpHedgeFlagType HedgeFlag;
        /// <summary>
        /// 占用保证金
        /// </summary>
        public double UsedMargin;
        /// <summary>
        /// 今持仓量
        /// </summary>
        public int Position;
        /// <summary>
        /// 今日持仓成本
        /// </summary>
        public double PositionCost;
        /// <summary>
        /// 昨持仓量
        /// </summary>
        public int YdPosition;
        /// <summary>
        /// 昨日持仓成本
        /// </summary>
        public double YdPositionCost;
        /// <summary>
        /// 冻结的保证金
        /// </summary>
        public double FrozenMargin;
        /// <summary>
        /// 开仓冻结持仓
        /// </summary>
        public int FrozenPosition;
        /// <summary>
        /// 平仓冻结持仓
        /// </summary>
        public int FrozenClosing;
        /// <summary>
        /// 冻结的权利金
        /// </summary>
        public double FrozenPremium;
        /// <summary>
        /// 最后一笔成交编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string LastTradeID;
        /// <summary>
        /// 最后一笔本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string LastOrderLocalID;
        /// <summary>
        /// 币种
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Currency;
    };

    /// <summary>
    /// 合规参数查询请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcQryComplianceParamField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
    };

    /// <summary>
    /// 合规参数查询应答
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcRspComplianceParamField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 客户号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string ClientID;
        /// <summary>
        /// 每日最大报单笔
        /// </summary>
        public int DailyMaxOrder;
        /// <summary>
        /// 每日最大撤单笔
        /// </summary>
        public int DailyMaxOrderAction;
        /// <summary>
        /// 每日最大错单笔
        /// </summary>
        public int DailyMaxErrorOrder;
        /// <summary>
        /// 每日最大报单手
        /// </summary>
        public int DailyMaxOrderVolume;
        /// <summary>
        /// 每日最大撤单手
        /// </summary>
        public int DailyMaxOrderActionVolume;
    };

    /// <summary>
    /// 用户查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcQryUserField
    {
        /// <summary>
        /// 交易用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string StartUserID;
        /// <summary>
        /// 交易用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string EndUserID;
    };

    /// <summary>
    /// 用户
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcUserField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 用户登录密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 是否活跃
        /// </summary>
        public EnumUstpIsActiveType IsActive;
        /// <summary>
        /// 用户名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string UserName;
        /// <summary>
        /// 用户类型
        /// </summary>
        public EnumUstpUserTypeType UserType;
        /// <summary>
        /// 营业部
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Department;
        /// <summary>
        /// 授权功能集
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string GrantFuncSet;
        /// <summary>
        /// 修改用户编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string SetUserID;
        /// <summary>
        /// 操作日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string CommandDate;
        /// <summary>
        /// 操作时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string CommandTime;
    };

    /// <summary>
    /// 投资者手续费率查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcQryInvestorFeeField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    };

    /// <summary>
    /// 投资者手续费率
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcInvestorFeeField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 客户号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string ClientID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 品种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ProductID;
        /// <summary>
        /// 开仓手续费按比例
        /// </summary>
        public double OpenFeeRate;
        /// <summary>
        /// 开仓手续费按手数
        /// </summary>
        public double OpenFeeAmt;
        /// <summary>
        /// 平仓手续费按比例
        /// </summary>
        public double OffsetFeeRate;
        /// <summary>
        /// 平仓手续费按手数
        /// </summary>
        public double OffsetFeeAmt;
        /// <summary>
        /// 平今仓手续费按比例
        /// </summary>
        public double OTFeeRate;
        /// <summary>
        /// 平今仓手续费按手数
        /// </summary>
        public double OTFeeAmt;
    };

    /// <summary>
    /// 投资者保证金率查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcQryInvestorMarginField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    };

    /// <summary>
    /// 投资者保证金率
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcInvestorMarginField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 客户号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string ClientID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 品种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ProductID;
        /// <summary>
        /// 多头占用保证金按比例
        /// </summary>
        public double LongMarginRate;
        /// <summary>
        /// 多头保证金按手数
        /// </summary>
        public double LongMarginAmt;
        /// <summary>
        /// 空头占用保证金按比例
        /// </summary>
        public double ShortMarginRate;
        /// <summary>
        /// 空头保证金按手数
        /// </summary>
        public double ShortMarginAmt;
    };

    /// <summary>
    /// 交叉外汇汇率查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcQryExchangeRateField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 品种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ProductID;
    };

    /// <summary>
    /// 交叉外汇汇率查询应答
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcRspExchangeRateField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 品种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ProductID;
        /// <summary>
        /// 折算汇率
        /// </summary>
        public double ExchangeRate;
    };

    /// <summary>
    /// 成交
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcTradeField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 会员编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 下单席位号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string SeatID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 客户号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string ClientID;
        /// <summary>
        /// 用户编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 成交编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TradeID;
        /// <summary>
        /// 报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string OrderSysID;
        /// <summary>
        /// 本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string UserOrderLocalID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumUstpDirectionType Direction;
        /// <summary>
        /// 开平标志
        /// </summary>
        public EnumUstpOffsetFlagType OffsetFlag;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumUstpHedgeFlagType HedgeFlag;
        /// <summary>
        /// 成交价格
        /// </summary>
        public double TradePrice;
        /// <summary>
        /// 成交数量
        /// </summary>
        public int TradeVolume;
        /// <summary>
        /// 成交时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 清算会员编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClearingPartID;
    };

    /// <summary>
    /// 报单
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcOrderField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 系统报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string OrderSysID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 用户本地报单号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string UserOrderLocalID;
        /// <summary>
        /// 报单类型
        /// </summary>
        public EnumUstpOrderPriceTypeType OrderPriceType;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumUstpDirectionType Direction;
        /// <summary>
        /// 开平标志
        /// </summary>
        public EnumUstpOffsetFlagType OffsetFlag;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumUstpHedgeFlagType HedgeFlag;
        /// <summary>
        /// 价格
        /// </summary>
        public double LimitPrice;
        /// <summary>
        /// 数量
        /// </summary>
        public int Volume;
        /// <summary>
        /// 有效期类型
        /// </summary>
        public EnumUstpTimeConditionType TimeCondition;
        /// <summary>
        /// GTD日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string GTDDate;
        /// <summary>
        /// 成交量类型
        /// </summary>
        public EnumUstpVolumeConditionType VolumeCondition;
        /// <summary>
        /// 最小成交量
        /// </summary>
        public int MinVolume;
        /// <summary>
        /// 止损价
        /// </summary>
        public double StopPrice;
        /// <summary>
        /// 强平原因
        /// </summary>
        public EnumUstpForceCloseReasonType ForceCloseReason;
        /// <summary>
        /// 当前自动挂起标志是否交易
        /// </summary>
        public int IsAutoSuspend;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 用户自定义域
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
        public string UserCustom;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 会员编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string ClientID;
        /// <summary>
        /// 下单席位号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string SeatID;
        /// <summary>
        /// 插入时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertTime;
        /// <summary>
        /// 本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderLocalID;
        /// <summary>
        /// 报单来源
        /// </summary>
        public EnumUstpOrderSourceType OrderSource;
        /// <summary>
        /// 报单状态
        /// </summary>
        public EnumUstpOrderStatusType OrderStatus;
        /// <summary>
        /// 撤销时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string CancelTime;
        /// <summary>
        /// 撤单用户编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string CancelUserID;
        /// <summary>
        /// 今成交数量
        /// </summary>
        public int VolumeTraded;
        /// <summary>
        /// 剩余数量
        /// </summary>
        public int VolumeRemain;
    };

    /// <summary>
    /// 数据流回退
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcFlowMessageCancelField
    {
        /// <summary>
        /// 序列系列号
        /// </summary>
        public int SequenceSeries;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 数据中心代码
        /// </summary>
        public int DataCenterID;
        /// <summary>
        /// 回退起始序列号
        /// </summary>
        public int StartSequenceNo;
        /// <summary>
        /// 回退结束序列号
        /// </summary>
        public int EndSequenceNo;
    };

    /// <summary>
    /// 信息分发
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcDisseminationField
    {
        /// <summary>
        /// 序列系列号
        /// </summary>
        public int SequenceSeries;
        /// <summary>
        /// 序列号
        /// </summary>
        public int SequenceNo;
    };

    /// <summary>
    /// 出入金结果
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcInvestorAccountDepositResField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 资金帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 资金流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string AccountSeqNo;
        /// <summary>
        /// 金额
        /// </summary>
        public double Amount;
        /// <summary>
        /// 出入金方向
        /// </summary>
        public EnumUstpAccountDirectionType AmountDirection;
        /// <summary>
        /// 可用资金
        /// </summary>
        public double Available;
        /// <summary>
        /// 结算准备金
        /// </summary>
        public double Balance;
    };

    /// <summary>
    /// 报价录入
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcInputQuoteField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易系统返回的系统报价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string QuoteSysID;
        /// <summary>
        /// 用户设定的本地报价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string UserQuoteLocalID;
        /// <summary>
        /// 飞马向交易系统报的本地报价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string QuoteLocalID;
        /// <summary>
        /// 买方买入数量
        /// </summary>
        public int BidVolume;
        /// <summary>
        /// 买方开平标志
        /// </summary>
        public EnumUstpOffsetFlagType BidOffsetFlag;
        /// <summary>
        /// 买方投机套保标志
        /// </summary>
        public EnumUstpHedgeFlagType BidHedgeFlag;
        /// <summary>
        /// 买方买入价格
        /// </summary>
        public double BidPrice;
        /// <summary>
        /// 卖方卖出数量
        /// </summary>
        public int AskVolume;
        /// <summary>
        /// 卖方开平标志
        /// </summary>
        public EnumUstpOffsetFlagType AskOffsetFlag;
        /// <summary>
        /// 卖方投机套保标志
        /// </summary>
        public EnumUstpHedgeFlagType AskHedgeFlag;
        /// <summary>
        /// 卖方卖出价格
        /// </summary>
        public double AskPrice;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 用户自定义域
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
        public string UserCustom;
        /// <summary>
        /// 拆分出来的买方用户本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BidUserOrderLocalID;
        /// <summary>
        /// 拆分出来的卖方用户本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string AskUserOrderLocalID;
    };

    /// <summary>
    /// 报价录入
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcRtnQuoteField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易系统返回的系统报价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string QuoteSysID;
        /// <summary>
        /// 用户设定的本地报价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string UserQuoteLocalID;
        /// <summary>
        /// 飞马向交易系统报的本地报价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string QuoteLocalID;
        /// <summary>
        /// 买方买入数量
        /// </summary>
        public int BidVolume;
        /// <summary>
        /// 买方开平标志
        /// </summary>
        public EnumUstpOffsetFlagType BidOffsetFlag;
        /// <summary>
        /// 买方投机套保标志
        /// </summary>
        public EnumUstpHedgeFlagType BidHedgeFlag;
        /// <summary>
        /// 买方买入价格
        /// </summary>
        public double BidPrice;
        /// <summary>
        /// 卖方卖出数量
        /// </summary>
        public int AskVolume;
        /// <summary>
        /// 卖方开平标志
        /// </summary>
        public EnumUstpOffsetFlagType AskOffsetFlag;
        /// <summary>
        /// 卖方投机套保标志
        /// </summary>
        public EnumUstpHedgeFlagType AskHedgeFlag;
        /// <summary>
        /// 卖方卖出价格
        /// </summary>
        public double AskPrice;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 用户自定义域
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
        public string UserCustom;
        /// <summary>
        /// 拆分出来的买方用户本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BidUserOrderLocalID;
        /// <summary>
        /// 拆分出来的卖方用户本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string AskUserOrderLocalID;
        /// <summary>
        /// 买方系统报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BidOrderSysID;
        /// <summary>
        /// 卖方系统报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string AskOrderSysID;
        /// <summary>
        /// 报价单状态
        /// </summary>
        public EnumUstpQuoteStatusType QuoteStatus;
        /// <summary>
        /// 插入时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertTime;
        /// <summary>
        /// 撤销时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string CancelTime;
        /// <summary>
        /// 成交时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
    };

    /// <summary>
    /// 报价操作
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcQuoteActionField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 交易系统返回的系统报价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string QuoteSysID;
        /// <summary>
        /// 用户设定的被撤的本地报价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string UserQuoteLocalID;
        /// <summary>
        /// 用户向飞马报的本地撤消报价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string UserQuoteActionLocalID;
        /// <summary>
        /// 报单操作标志
        /// </summary>
        public EnumUstpActionFlagType ActionFlag;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 用户自定义域
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)]
        public string UserCustom;
    };

    /// <summary>
    /// 询价输入
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcReqForQuoteField
    {
        /// <summary>
        /// 询价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ReqForQuoteID;
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 询价时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ReqForQuoteTime;
    };

    /// <summary>
    /// 行情基础属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcMarketDataBaseField
    {
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算组代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string SettlementGroupID;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 昨结算
        /// </summary>
        public double PreSettlementPrice;
        /// <summary>
        /// 昨收盘
        /// </summary>
        public double PreClosePrice;
        /// <summary>
        /// 昨持仓量
        /// </summary>
        public double PreOpenInterest;
        /// <summary>
        /// 昨虚实度
        /// </summary>
        public double PreDelta;
    };

    /// <summary>
    /// 行情静态属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcMarketDataStaticField
    {
        /// <summary>
        /// 今开盘
        /// </summary>
        public double OpenPrice;
        /// <summary>
        /// 最高价
        /// </summary>
        public double HighestPrice;
        /// <summary>
        /// 最低价
        /// </summary>
        public double LowestPrice;
        /// <summary>
        /// 今收盘
        /// </summary>
        public double ClosePrice;
        /// <summary>
        /// 涨停板价
        /// </summary>
        public double UpperLimitPrice;
        /// <summary>
        /// 跌停板价
        /// </summary>
        public double LowerLimitPrice;
        /// <summary>
        /// 今结算
        /// </summary>
        public double SettlementPrice;
        /// <summary>
        /// 今虚实度
        /// </summary>
        public double CurrDelta;
    };

    /// <summary>
    /// 行情最新成交属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcMarketDataLastMatchField
    {
        /// <summary>
        /// 最新价
        /// </summary>
        public double LastPrice;
        /// <summary>
        /// 数量
        /// </summary>
        public int Volume;
        /// <summary>
        /// 成交金额
        /// </summary>
        public double Turnover;
        /// <summary>
        /// 持仓量
        /// </summary>
        public double OpenInterest;
    };

    /// <summary>
    /// 行情最优价属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcMarketDataBestPriceField
    {
        /// <summary>
        /// 申买价一
        /// </summary>
        public double BidPrice1;
        /// <summary>
        /// 申买量一
        /// </summary>
        public int BidVolume1;
        /// <summary>
        /// 申卖价一
        /// </summary>
        public double AskPrice1;
        /// <summary>
        /// 申卖量一
        /// </summary>
        public int AskVolume1;
    };

    /// <summary>
    /// 行情申买二、三属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcMarketDataBid23Field
    {
        /// <summary>
        /// 申买价二
        /// </summary>
        public double BidPrice2;
        /// <summary>
        /// 申买量二
        /// </summary>
        public int BidVolume2;
        /// <summary>
        /// 申买价三
        /// </summary>
        public double BidPrice3;
        /// <summary>
        /// 申买量三
        /// </summary>
        public int BidVolume3;
    };

    /// <summary>
    /// 行情申卖二、三属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcMarketDataAsk23Field
    {
        /// <summary>
        /// 申卖价二
        /// </summary>
        public double AskPrice2;
        /// <summary>
        /// 申卖量二
        /// </summary>
        public int AskVolume2;
        /// <summary>
        /// 申卖价三
        /// </summary>
        public double AskPrice3;
        /// <summary>
        /// 申卖量三
        /// </summary>
        public int AskVolume3;
    };

    /// <summary>
    /// 行情申买四、五属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcMarketDataBid45Field
    {
        /// <summary>
        /// 申买价四
        /// </summary>
        public double BidPrice4;
        /// <summary>
        /// 申买量四
        /// </summary>
        public int BidVolume4;
        /// <summary>
        /// 申买价五
        /// </summary>
        public double BidPrice5;
        /// <summary>
        /// 申买量五
        /// </summary>
        public int BidVolume5;
    };

    /// <summary>
    /// 行情申卖四、五属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcMarketDataAsk45Field
    {
        /// <summary>
        /// 申卖价四
        /// </summary>
        public double AskPrice4;
        /// <summary>
        /// 申卖量四
        /// </summary>
        public int AskVolume4;
        /// <summary>
        /// 申卖价五
        /// </summary>
        public double AskPrice5;
        /// <summary>
        /// 申卖量五
        /// </summary>
        public int AskVolume5;
    };

    /// <summary>
    /// 行情更新时间属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcMarketDataUpdateTimeField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 最后修改时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string UpdateTime;
        /// <summary>
        /// 最后修改毫秒
        /// </summary>
        public int UpdateMillisec;
    };

    /// <summary>
    /// 深度行情
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcDepthMarketDataField
    {
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算组代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string SettlementGroupID;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 昨结算
        /// </summary>
        public double PreSettlementPrice;
        /// <summary>
        /// 昨收盘
        /// </summary>
        public double PreClosePrice;
        /// <summary>
        /// 昨持仓量
        /// </summary>
        public double PreOpenInterest;
        /// <summary>
        /// 昨虚实度
        /// </summary>
        public double PreDelta;
        /// <summary>
        /// 今开盘
        /// </summary>
        public double OpenPrice;
        /// <summary>
        /// 最高价
        /// </summary>
        public double HighestPrice;
        /// <summary>
        /// 最低价
        /// </summary>
        public double LowestPrice;
        /// <summary>
        /// 今收盘
        /// </summary>
        public double ClosePrice;
        /// <summary>
        /// 涨停板价
        /// </summary>
        public double UpperLimitPrice;
        /// <summary>
        /// 跌停板价
        /// </summary>
        public double LowerLimitPrice;
        /// <summary>
        /// 今结算
        /// </summary>
        public double SettlementPrice;
        /// <summary>
        /// 今虚实度
        /// </summary>
        public double CurrDelta;
        /// <summary>
        /// 最新价
        /// </summary>
        public double LastPrice;
        /// <summary>
        /// 数量
        /// </summary>
        public int Volume;
        /// <summary>
        /// 成交金额
        /// </summary>
        public double Turnover;
        /// <summary>
        /// 持仓量
        /// </summary>
        public double OpenInterest;
        /// <summary>
        /// 申买价一
        /// </summary>
        public double BidPrice1;
        /// <summary>
        /// 申买量一
        /// </summary>
        public int BidVolume1;
        /// <summary>
        /// 申卖价一
        /// </summary>
        public double AskPrice1;
        /// <summary>
        /// 申卖量一
        /// </summary>
        public int AskVolume1;
        /// <summary>
        /// 申买价二
        /// </summary>
        public double BidPrice2;
        /// <summary>
        /// 申买量二
        /// </summary>
        public int BidVolume2;
        /// <summary>
        /// 申买价三
        /// </summary>
        public double BidPrice3;
        /// <summary>
        /// 申买量三
        /// </summary>
        public int BidVolume3;
        /// <summary>
        /// 申卖价二
        /// </summary>
        public double AskPrice2;
        /// <summary>
        /// 申卖量二
        /// </summary>
        public int AskVolume2;
        /// <summary>
        /// 申卖价三
        /// </summary>
        public double AskPrice3;
        /// <summary>
        /// 申卖量三
        /// </summary>
        public int AskVolume3;
        /// <summary>
        /// 申买价四
        /// </summary>
        public double BidPrice4;
        /// <summary>
        /// 申买量四
        /// </summary>
        public int BidVolume4;
        /// <summary>
        /// 申买价五
        /// </summary>
        public double BidPrice5;
        /// <summary>
        /// 申买量五
        /// </summary>
        public int BidVolume5;
        /// <summary>
        /// 申卖价四
        /// </summary>
        public double AskPrice4;
        /// <summary>
        /// 申卖量四
        /// </summary>
        public int AskVolume4;
        /// <summary>
        /// 申卖价五
        /// </summary>
        public double AskPrice5;
        /// <summary>
        /// 申卖量五
        /// </summary>
        public int AskVolume5;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 最后修改时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string UpdateTime;
        /// <summary>
        /// 最后修改毫秒
        /// </summary>
        public int UpdateMillisec;
    };

    /// <summary>
    /// 订阅合约的相关信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcSpecificInstrumentField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    };

    /// <summary>
    /// 申请组合
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcInputMarginCombActionField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 交易用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumUstpHedgeFlagType HedgeFlag;
        /// <summary>
        /// 用户本地编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string UserActionLocalID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string CombInstrumentID;
        /// <summary>
        /// 组合数量
        /// </summary>
        public int CombVolume;
        /// <summary>
        /// 组合动作方向
        /// </summary>
        public EnumUstpCombDirectionType CombDirection;
        /// <summary>
        /// 本地编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ActionLocalID;
    };

    /// <summary>
    /// 交易编码组合持仓查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcQryInvestorCombPositionField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumUstpHedgeFlagType HedgeFlag;
        /// <summary>
        /// 组合合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string CombInstrumentID;
    };

    /// <summary>
    /// 交易编码组合持仓
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcRspInvestorCombPositionField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumUstpHedgeFlagType HedgeFlag;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string ClientID;
        /// <summary>
        /// 组合合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string CombInstrumentID;
        /// <summary>
        /// 组合持仓
        /// </summary>
        public int CombPosition;
        /// <summary>
        /// 冻结组合持仓
        /// </summary>
        public int CombFrozenPosition;
        /// <summary>
        /// 组合一手释放的保证金
        /// </summary>
        public double CombFreeMargin;
    };

    /// <summary>
    /// 组合规则
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcMarginCombinationLegField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 组合合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string CombInstrumentID;
        /// <summary>
        /// 单腿编号
        /// </summary>
        public int LegID;
        /// <summary>
        /// 单腿合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string LegInstrumentID;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumUstpDirectionType Direction;
        /// <summary>
        /// 单腿乘数
        /// </summary>
        public int LegMultiple;
        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority;
    };

    /// <summary>
    /// 交易编码单腿持仓查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcQryInvestorLegPositionField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumUstpHedgeFlagType HedgeFlag;
        /// <summary>
        /// 单腿合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string LegInstrumentID;
    };

    /// <summary>
    /// 交易编码单腿持仓
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CUstpFtdcRspInvestorLegPositionField
    {
        /// <summary>
        /// 经纪公司编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeID;
        /// <summary>
        /// 投资者编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string InvestorID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumUstpHedgeFlagType HedgeFlag;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 19)]
        public string ClientID;
        /// <summary>
        /// 单腿合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 多头持仓
        /// </summary>
        public int LongPosition;
        /// <summary>
        /// 空头持仓
        /// </summary>
        public int ShortPosition;
        /// <summary>
        /// 多头占用保证金
        /// </summary>
        public double LongMargin;
        /// <summary>
        /// 空头占用保证金
        /// </summary>
        public double ShortMargin;
        /// <summary>
        /// 多头冻结持仓
        /// </summary>
        public int LongFrozenPosition;
        /// <summary>
        /// 空头冻结持仓
        /// </summary>
        public int ShortFrozenPosition;
        /// <summary>
        /// 多头冻结保证金
        /// </summary>
        public double LongFrozenMargin;
        /// <summary>
        /// 空头冻结保证金
        /// </summary>
        public double ShortFrozenMargin;
    };


    /// <summary>
    /// TFtdcUstpVolumeConditionType是一个成交量类型类型
    /// </summary>
    public enum EnumUstpVolumeConditionType
    {
        /// <summary>
        /// 任何数量
        /// </summary>
        AV = (byte)'1',

        /// <summary>
        /// 最小数量
        /// </summary>
        MV = (byte)'2',

        /// <summary>
        /// 全部数量
        /// </summary>
        CV = (byte)'3'
    }

    /// <summary>
    /// TFtdcUstpForceCloseReasonType是一个强平原因类型
    /// </summary>
    public enum EnumUstpForceCloseReasonType
    {
        /// <summary>
        /// 非强平
        /// </summary>
        NotForceClose = (byte)'0',

        /// <summary>
        /// 资金不足
        /// </summary>
        LackDeposit = (byte)'1',

        /// <summary>
        /// 客户超仓
        /// </summary>
        ClientOverPositionLimit = (byte)'2',

        /// <summary>
        /// 会员超仓
        /// </summary>
        MemberOverPositionLimit = (byte)'3',

        /// <summary>
        /// 持仓非整数倍
        /// </summary>
        NotMultiple = (byte)'4'

    }

    /// <summary>
    /// TFtdcUstpInstrumentStatusType是一个合约交易状态类型
    /// </summary>
    public enum EnumUstpInstrumentStatusType
    {
        /// <summary>
        /// 开盘前
        /// </summary>
        BeforeTrading = (byte)'0',

        /// <summary>
        /// 非交易
        /// </summary>
        NoTrading = (byte)'1',

        /// <summary>
        /// 连续交易
        /// </summary>
        Continous = (byte)'2',

        /// <summary>
        /// 集合竞价报单
        /// </summary>
        AuctionOrdering = (byte)'3',

        /// <summary>
        /// 集合竞价价格平衡
        /// </summary>
        AuctionBalance = (byte)'4',

        /// <summary>
        /// 集合竞价撮合
        /// </summary>
        AuctionMatch = (byte)'5',

        /// <summary>
        /// 收盘
        /// </summary>
        Closed = (byte)'6'
    }

    /// <summary>
    /// TFtdcUstpOffsetFlagType是一个开平标志类型
    /// </summary>
    public enum EnumUstpOffsetFlagType
    {
        /// <summary>
        /// 开仓
        /// </summary>
        Open = (byte)'0',

        /// <summary>
        /// 平仓
        /// </summary>
        Close = (byte)'1',

        /// <summary>
        /// 强平
        /// </summary>
        ForceClose = (byte)'2',

        /// <summary>
        /// 平今
        /// </summary>
        CloseToday = (byte)'3',

        /// <summary>
        /// 平昨
        /// </summary>
        CloseYesterday = (byte)'4'
    }

    /// <summary>
    /// TFtdcUstpOrderPriceTypeType是一个报单价格条件类型
    /// </summary>
    public enum EnumUstpOrderPriceTypeType
    {
        /// <summary>
        /// 任意价
        /// </summary>
        AnyPrice = (byte)'1',

        /// <summary>
        /// 限价
        /// </summary>
        LimitPrice = (byte)'2',

        /// <summary>
        /// 最优价
        /// </summary>
        BestPrice = (byte)'3',

        /// <summary>
        /// 五档价
        /// </summary>
        FiveLevelPrice = (byte)'4'
    }

    /// <summary>
    /// TFtdcUstpOrderStatusType是一个报单状态类型
    /// </summary>
    public enum EnumUstpOrderStatusType
    {
        /// <summary>
        /// 全部成交
        /// </summary>
        AllTraded = (byte)'0',

        /// <summary>
        /// 部分成交还在队列中
        /// </summary>
        PartTradedQueueing = (byte)'1',

        /// <summary>
        /// 部分成交不在队列中
        /// </summary>
        PartTradedNotQueueing = (byte)'2',

        /// <summary>
        /// 未成交还在队列中
        /// </summary>
        NoTradeQueueing = (byte)'3',

        /// <summary>
        /// 未成交不在队列中
        /// </summary>
        NoTradeNotQueueing = (byte)'4',

        /// <summary>
        /// 撤单
        /// </summary>
        Canceled = (byte)'5',

        /// <summary>
        /// 订单已报入交易所未应答
        /// </summary>
        AcceptedNoReply = (byte)'6'
    }

    /// <summary>
    /// TFtdcUstpUserTypeType是一个用户类型类型
    /// </summary>
    public enum EnumUstpUserTypeType
    {
        /// <summary>
        /// 自然人
        /// </summary>
        Person = (byte)'1',

        /// <summary>
        /// 理财产品
        /// </summary>
        Product = (byte)'2',

        /// <summary>
        /// 期货公司管理员
        /// </summary>
        Manager = (byte)'3',

        /// <summary>
        /// 席位
        /// </summary>
        Seat = (byte)'4'
    }

    /// <summary>
    /// TFtdcUstpTradingRightType是一个交易权限类型
    /// </summary>
    public enum EnumUstpTradingRightType
    {
        /// <summary>
        /// 可以交易
        /// </summary>
        Allow = (byte)'0',

        /// <summary>
        /// 只能平仓
        /// </summary>
        CloseOnly = (byte)'1',

        /// <summary>
        /// 不能交易
        /// </summary>
        Forbidden = (byte)'2'
    }

    /// <summary>
    /// TFtdcUstpTimeConditionType是一个有效期类型类型
    /// </summary>
    public enum EnumUstpTimeConditionType
    {
        /// <summary>
        /// 立即完成，否则撤销
        /// </summary>
        IOC = (byte)'1',

        /// <summary>
        /// 本节有效
        /// </summary>
        GFS = (byte)'2',

        /// <summary>
        /// 当日有效
        /// </summary>
        GFD = (byte)'3',

        /// <summary>
        /// 指定日期前有效
        /// </summary>
        GTD = (byte)'4',

        /// <summary>
        /// 撤销前有效
        /// </summary>
        GTC = (byte)'5',

        /// <summary>
        /// 集合竞价有效
        /// </summary>
        GFA = (byte)'6'
    }

    /// <summary>
    /// TFtdcUstpOrderSourceType是一个报单来源类型
    /// </summary>
    public enum EnumUstpOrderSourceType
    {
        /// <summary>
        /// 来自参与者
        /// </summary>
        Participant = (byte)'0',

        /// <summary>
        /// 来自管理员
        /// </summary>
        Administrator = (byte)'1',

        /// <summary>
        /// 报价单拆分出来的买单或卖单
        /// </summary>
        QuoteSplit = (byte)'2'
    }

    /// <summary>
    /// TFtdcUstpDirectionType是一个买卖方向类型
    /// </summary>
    public enum EnumUstpDirectionType
    {
        /// <summary>
        /// 买
        /// </summary>
        Buy = (byte)'0',

        /// <summary>
        /// 卖
        /// </summary>
        Sell = (byte)'1'
    }

    /// <summary>
    /// TFtdcUstpCurrencyType是一个币种类型
    /// </summary>
    public enum EnumUstpCurrencyType
    {
        /// <summary>
        /// 人民币
        /// </summary>
        RMB = (byte)'1',

        /// <summary>
        /// 美元
        /// </summary>
        UDOLLAR = (byte)'2'
    }

    /// <summary>
    /// TFtdcUstpAccountDirectionType是一个出入金方向类型
    /// </summary>
    public enum EnumUstpAccountDirectionType
    {
        /// <summary>
        /// 入金
        /// </summary>
        In = (byte)'1',

        /// <summary>
        /// 出金
        /// </summary>
        Out = (byte)'2'
    }

    /// <summary>
    /// TFtdcUstpHedgeFlagType是一个投机套保标志类型
    /// </summary>
    public enum EnumUstpHedgeFlagType
    {
        /// <summary>
        /// 投机
        /// </summary>
        Speculation = (byte)'1',

        /// <summary>
        /// 套利
        /// </summary>
        Arbitrage = (byte)'2',

        /// <summary>
        /// 套保
        /// </summary>
        Hedge = (byte)'3',

        /// <summary>
        /// 做市商
        /// </summary>
        MarketMaker = (byte)'4'
    }

    /// <summary>
    /// TFtdcUstpActionFlagType是一个操作标志类型
    /// </summary>
    public enum EnumUstpActionFlagType
    {
        /// <summary>
        /// 删除
        /// </summary>
        Delete = (byte)'0',

        /// <summary>
        /// 挂起
        /// </summary>
        Suspend = (byte)'1',

        /// <summary>
        /// 激活
        /// </summary>
        Active = (byte)'2',

        /// <summary>
        /// 修改
        /// </summary>
        Modify = (byte)'3'
    }

    /// <summary>
    /// TFtdcUstpPositionTypeType是一个持仓类型类型
    /// </summary>
    public enum EnumUstpPositionTypeType
    {
        /// <summary>
        /// 净持仓
        /// </summary>
        Net = (byte)'1',

        /// <summary>
        /// 综合持仓
        /// </summary>
        Gross = (byte)'2'
    }

    /// <summary>
    /// TFtdcUstpOptionsTypeType是一个期权类型类型
    /// </summary>
    public enum EnumUstpOptionsTypeType
    {
        /// <summary>
        /// 非期权
        /// </summary>
        NotOptions = (byte)'0',

        /// <summary>
        /// 看涨
        /// </summary>
        CallOptions = (byte)'1',

        /// <summary>
        /// 看跌
        /// </summary>
        PutOptions = (byte)'2'
    }

    /// <summary>
    /// TFtdcUstpIsActiveType是一个是否活跃类型
    /// </summary>
    public enum EnumUstpIsActiveType
    {
        /// <summary>
        /// 不活跃
        /// </summary>
        NoActive = (byte)'0',

        /// <summary>
        /// 活跃
        /// </summary>
        Active = (byte)'1'
    }

    /// <summary>
    /// TFtdcUstpQuoteStatusType是一个报价单状态类型类型
    /// </summary>
    public enum EnumUstpQuoteStatusType
    {
        /// <summary>
        /// 在飞马中还未进入交易系统
        /// </summary>
        Inited_InFEMAS = (byte)'0',

        /// <summary>
        /// 已经报入交易系统中
        /// </summary>
        Accepted_InTradingSystem = (byte)'1',

        /// <summary>
        /// 已经撤掉单腿
        /// </summary>
        Canceled_SingleLeg = (byte)'2',

        /// <summary>
        /// 已经全部撤掉
        /// </summary>
        Canceled_All = (byte)'3',

        /// <summary>
        /// 已经有单腿成交
        /// </summary>
        Traded_SingleLeg = (byte)'4',

        /// <summary>
        /// 已经全部成交
        /// </summary>
        Traded_All = (byte)'5',

        /// <summary>
        /// 错误的撤消报价请求
        /// </summary>
        Error_QuoteAction = (byte)'6'
    }

    /// <summary>
    /// TFtdcUstpCombDirectionType是一个申请保证金组合指令方向类型
    /// </summary>
    public enum EnumUstpCombDirectionType
    {
        /// <summary>
        /// 申请组合
        /// </summary>
        Combine = (byte)'0',

        /// <summary>
        /// 申请拆分组合
        /// </summary>
        UnCombine = (byte)'1'
    }

}
