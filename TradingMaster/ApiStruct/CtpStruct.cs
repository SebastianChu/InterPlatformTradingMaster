using System.Runtime.InteropServices;

namespace TradingMaster
{
    /// <summary>
    /// 信息分发
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcDisseminationField
    {
        /// <summary>
        /// 序列系列号
        /// </summary>
        public short SequenceSeries;
        /// <summary>
        /// 序列号
        /// </summary>
        public int SequenceNo;
    }

    /// <summary>
    /// 用户登录请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcReqUserLoginField
    {
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 用户端产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string UserProductInfo;
        /// <summary>
        /// 接口端产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string InterfaceProductInfo;
        /// <summary>
        /// 协议信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ProtocolInfo;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
        /// <summary>
        /// 动态密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string OneTimePassword;
        /// <summary>
        /// 终端IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ClientIPAddress;
        /// <summary>
        /// 登录备注
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string LoginRemark;
    }

    /// <summary>
    /// 用户登录应答
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcRspUserLoginField
    {
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 登录成功时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LoginTime;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 交易系统名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string SystemName;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 最大报单引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string MaxOrderRef;
        /// <summary>
        /// 上期所时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string SHFETime;
        /// <summary>
        /// 大商所时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string DCETime;
        /// <summary>
        /// 郑商所时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string CZCETime;
        /// <summary>
        /// 中金所时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string FFEXTime;
        /// <summary>
        /// 能源中心时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string INETime;
    }

    /// <summary>
    /// 用户登出请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcUserLogoutField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
    }

    /// <summary>
    /// 强制交易员退出
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcForceUserLogoutField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
    }

    ///客户端认证请求
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcReqAuthenticateField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 用户端产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string UserProductInfo;
        /// <summary>
        /// 认证码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string AuthCode;
    };

    ///客户端认证响应
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcRspAuthenticateField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 用户端产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string UserProductInfo;
    };

    ///客户端认证信息
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcAuthenticationInfoField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 用户端产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string UserProductInfo;
        /// <summary>
        /// 认证信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string AuthInfo;
        /// <summary>
        /// 是否为认证结果
        /// </summary>
        public int IsResult;
    };

    /// <summary>
    /// 银期转帐报文头
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTransferHeaderField
    {
        /// <summary>
        /// 版本号，常量，1.0
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string Version;
        /// <summary>
        /// 交易代码，必填
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 交易日期，必填，格式：yyyymmdd
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间，必填，格式：hhmmss
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 发起方流水号，N/A
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeSerial;
        /// <summary>
        /// 期货公司代码，必填
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string FutureID;
        /// <summary>
        /// 银行代码，根据查询银行得到，必填
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分中心代码，根据查询银行得到，必填
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBrchID;
        /// <summary>
        /// 操作员，N/A
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 交易设备类型，N/A
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 记录数，N/A
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string RecordNum;
        /// <summary>
        /// 会话编号，N/A
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 请求编号，N/A
        /// </summary>
        public int RequestID;
    }

    /// <summary>
    /// 银行资金转期货请求，TradeCode=202001
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTransferBankToFutureReqField
    {
        /// <summary>
        /// 期货资金账户
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string FutureAccount;
        /// <summary>
        /// 密码标志
        /// </summary>
        public EnumThostFuturePwdFlagType FuturePwdFlag;
        /// <summary>
        /// 密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string FutureAccPwd;
        /// <summary>
        /// 转账金额
        /// </summary>
        public double TradeAmt;
        /// <summary>
        /// 客户手续费
        /// </summary>
        public double CustFee;
        /// <summary>
        /// 币种：RMB-人民币 USD-美圆 HKD-港元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyCode;
    }

    /// <summary>
    /// 银行资金转期货请求响应
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTransferBankToFutureRspField
    {
        /// <summary>
        /// 响应代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string RetCode;
        /// <summary>
        /// 响应信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string RetInfo;
        /// <summary>
        /// 资金账户
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string FutureAccount;
        /// <summary>
        /// 转帐金额
        /// </summary>
        public double TradeAmt;
        /// <summary>
        /// 应收客户手续费
        /// </summary>
        public double CustFee;
        /// <summary>
        /// 币种
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyCode;
    }

    /// <summary>
    /// 期货资金转银行请求，TradeCode=202002
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTransferFutureToBankReqField
    {
        /// <summary>
        /// 期货资金账户
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string FutureAccount;
        /// <summary>
        /// 密码标志
        /// </summary>
        public EnumThostFuturePwdFlagType FuturePwdFlag;
        /// <summary>
        /// 密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string FutureAccPwd;
        /// <summary>
        /// 转账金额
        /// </summary>
        public double TradeAmt;
        /// <summary>
        /// 客户手续费
        /// </summary>
        public double CustFee;
        /// <summary>
        /// 币种：RMB-人民币 USD-美圆 HKD-港元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyCode;
    }

    /// <summary>
    /// 期货资金转银行请求响应
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTransferFutureToBankRspField
    {
        /// <summary>
        /// 响应代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string RetCode;
        /// <summary>
        /// 响应信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string RetInfo;
        /// <summary>
        /// 资金账户
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string FutureAccount;
        /// <summary>
        /// 转帐金额
        /// </summary>
        public double TradeAmt;
        /// <summary>
        /// 应收客户手续费
        /// </summary>
        public double CustFee;
        /// <summary>
        /// 币种
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyCode;
    }

    /// <summary>
    /// 查询银行资金请求，TradeCode=204002
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTransferQryBankReqField
    {
        /// <summary>
        /// 期货资金账户
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string FutureAccount;
        /// <summary>
        /// 密码标志
        /// </summary>
        public EnumThostFuturePwdFlagType FuturePwdFlag;
        /// <summary>
        /// 密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string FutureAccPwd;
        /// <summary>
        /// 币种：RMB-人民币 USD-美圆 HKD-港元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyCode;
    }

    /// <summary>
    /// 查询银行资金请求响应
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTransferQryBankRspField
    {
        /// <summary>
        /// 响应代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string RetCode;
        /// <summary>
        /// 响应信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string RetInfo;
        /// <summary>
        /// 资金账户
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string FutureAccount;
        /// <summary>
        /// 银行余额
        /// </summary>
        public double TradeAmt;
        /// <summary>
        /// 银行可用余额
        /// </summary>
        public double UseAmt;
        /// <summary>
        /// 银行可取余额
        /// </summary>
        public double FetchAmt;
        /// <summary>
        /// 币种
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyCode;
    }

    /// <summary>
    /// 查询银行交易明细请求，TradeCode=204999
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTransferQryDetailReqField
    {
        /// <summary>
        /// 期货资金账户
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string FutureAccount;
    }

    /// <summary>
    /// 查询银行交易明细请求响应
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTransferQryDetailRspField
    {
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 交易代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 期货流水号
        /// </summary>
        public int FutureSerial;
        /// <summary>
        /// 期货公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string FutureID;
        /// <summary>
        /// 资金帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
        public string FutureAccount;
        /// <summary>
        /// 银行流水号
        /// </summary>
        public int BankSerial;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分中心代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBrchID;
        /// <summary>
        /// 银行账号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string CertCode;
        /// <summary>
        /// 货币代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyCode;
        /// <summary>
        /// 发生金额
        /// </summary>
        public double TxAmount;
        /// <summary>
        /// 有效标志
        /// </summary>
        public EnumThostTransferValidFlagType Flag;
    }

    /// <summary>
    /// 响应信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcRspInfoField
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
    }

    /// <summary>
    /// 交易所
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExchangeField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 交易所名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeName;
        /// <summary>
        /// 交易所属性
        /// </summary>
        public EnumThostExchangePropertyType ExchangeProperty;
    }

    /// <summary>
    /// 产品
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcProductField
    {
        /// <summary>
        /// 产品代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ProductID;
        /// <summary>
        /// 产品名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string ProductName;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 产品类型
        /// </summary>
        public EnumThostProductClassType ProductClass;
        /// <summary>
        /// 合约数量乘数
        /// </summary>
        public int VolumeMultiple;
        /// <summary>
        /// 最小变动价位
        /// </summary>
        public double PriceTick;
        /// <summary>
        /// 市价单最大下单量
        /// </summary>
        public int MaxMarketOrderVolume;
        /// <summary>
        /// 市价单最小下单量
        /// </summary>
        public int MinMarketOrderVolume;
        /// <summary>
        /// 限价单最大下单量
        /// </summary>
        public int MaxLimitOrderVolume;
        /// <summary>
        /// 限价单最小下单量
        /// </summary>
        public int MinLimitOrderVolume;
        /// <summary>
        /// 持仓类型
        /// </summary>
        public EnumThostPositionTypeType PositionType;
        /// <summary>
        /// 持仓日期类型
        /// </summary>
        public EnumThostPositionDateTypeType PositionDateType;
        /// <summary>
        /// 平仓处理类型
        /// </summary>
        public EnumThostCloseDealTypeType CloseDealType;
        ////// <summary>
        /// 交易币种类型
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string TradeCurrencyID;
        /// <summary>
        /// 质押资金可用范围
        /// </summary>
        public EnumThostMortgageFundUseRangeType MortgageFundUseRange;
        /// <summary>
        /// 交易所产品代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeProductID;
        /// <summary>
        /// 合约基础商品乘数
        /// </summary>
        public double UnderlyingMultiple;
    }

    /// <summary>
    /// 合约
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInstrumentField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 合约名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string InstrumentName;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 产品代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ProductID;
        /// <summary>
        /// 产品类型
        /// </summary>
        public EnumThostProductClassType ProductClass;
        /// <summary>
        /// 交割年份
        /// </summary>
        public int DeliveryYear;
        /// <summary>
        /// 交割月
        /// </summary>
        public int DeliveryMonth;
        /// <summary>
        /// 市价单最大下单量
        /// </summary>
        public int MaxMarketOrderVolume;
        /// <summary>
        /// 市价单最小下单量
        /// </summary>
        public int MinMarketOrderVolume;
        /// <summary>
        /// 限价单最大下单量
        /// </summary>
        public int MaxLimitOrderVolume;
        /// <summary>
        /// 限价单最小下单量
        /// </summary>
        public int MinLimitOrderVolume;
        /// <summary>
        /// 合约数量乘数
        /// </summary>
        public int VolumeMultiple;
        /// <summary>
        /// 最小变动价位
        /// </summary>
        public double PriceTick;
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
        /// 结束交割日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string EndDelivDate;
        /// <summary>
        /// 合约生命周期状态
        /// </summary>
        public EnumThostInstLifePhaseType InstLifePhase;
        /// <summary>
        /// 当前是否交易
        /// </summary>
        public int IsTrading;
        /// <summary>
        /// 持仓类型
        /// </summary>
        public EnumThostPositionTypeType PositionType;
        /// <summary>
        /// 持仓日期类型
        /// </summary>
        public EnumThostPositionDateTypeType PositionDateType;
        /// <summary>
        /// 多头保证金率
        /// </summary>
        public double LongMarginRatio;
        /// <summary>
        /// 空头保证金率
        /// </summary>
        public double ShortMarginRatio;
        /// <summary>
        /// 是否使用大额单边保证金算法
        /// </summary>
        public EnumThostMaxMarginSideAlgorithmType MaxMarginSideAlgorithm;
        /// <summary>
        /// 基础商品代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string UnderlyingInstrID;
        /// <summary>
        /// 执行价
        /// </summary>
        public double StrikePrice;
        /// <summary>
        /// 期权类型
        /// </summary>
        public EnumThostOptionsTypeType OptionsType;
        /// <summary>
        /// 合约基础商品乘数
        /// </summary>
        public double UnderlyingMultiple;
        /// <summary>
        /// 组合类型
        /// </summary>
        public EnumThostCombinationTypeType CombinationType;
    }

    /// <summary>
    /// 经纪公司
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcBrokerField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 经纪公司简称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string BrokerAbbr;
        /// <summary>
        /// 经纪公司名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string BrokerName;
        /// <summary>
        /// 是否活跃
        /// </summary>
        public int IsActive;
    }

    /// <summary>
    /// 交易所交易员
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTraderField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 安装数量
        /// </summary>
        public int InstallCount;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
    }

    /// <summary>
    /// 投资者
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInvestorField
    {
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者分组代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorGroupID;
        /// <summary>
        /// 投资者名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string InvestorName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdentifiedCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 是否活跃
        /// </summary>
        public int IsActive;
        /// <summary>
        /// 联系电话
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Telephone;
        /// <summary>
        /// 通讯地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
        public string Address;
        /// <summary>
        /// 开户日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string OpenDate;
        /// <summary>
        /// 手机
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Mobile;
        /// <summary>
        /// 手续费率模板代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string CommModelID;
        /// <summary>
        /// 保证金率模板代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string MarginModelID;
    }

    /// <summary>
    /// 交易编码
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTradingCodeField
    {
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 是否活跃
        /// </summary>
        public int IsActive;
        /// <summary>
        /// 交易编码类型
        /// </summary>
        public EnumThostClientIDTypeType ClientIDType;
    }

    /// <summary>
    /// 会员编码和经纪公司编码对照表
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcPartBrokerField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 是否活跃
        /// </summary>
        public int IsActive;
    }

    /// <summary>
    /// 管理用户
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSuperUserField
    {
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 用户名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string UserName;
        /// <summary>
        /// 密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 是否活跃
        /// </summary>
        public int IsActive;
    }

    /// <summary>
    /// 管理用户功能权限
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSuperUserFunctionField
    {
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 功能代码
        /// </summary>
        public EnumThostFunctionCodeType FunctionCode;
    }

    /// <summary>
    /// 投资者组
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInvestorGroupField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者分组代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorGroupID;
        /// <summary>
        /// 投资者分组名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string InvestorGroupName;
    }

    /// <summary>
    /// 资金账户
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTradingAccountField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 上次质押金额
        /// </summary>
        public double PreMortgage;
        /// <summary>
        /// 上次信用额度
        /// </summary>
        public double PreCredit;
        /// <summary>
        /// 上次存款额
        /// </summary>
        public double PreDeposit;
        /// <summary>
        /// 上次结算准备金
        /// </summary>
        public double PreBalance;
        /// <summary>
        /// 上次占用的保证金
        /// </summary>
        public double PreMargin;
        /// <summary>
        /// 利息基数
        /// </summary>
        public double InterestBase;
        /// <summary>
        /// 利息收入
        /// </summary>
        public double Interest;
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
        /// 冻结的资金
        /// </summary>
        public double FrozenCash;
        /// <summary>
        /// 冻结的手续费
        /// </summary>
        public double FrozenCommission;
        /// <summary>
        /// 当前保证金总额
        /// </summary>
        public double CurrMargin;
        /// <summary>
        /// 资金差额
        /// </summary>
        public double CashIn;
        /// <summary>
        /// 手续费
        /// </summary>
        public double Commission;
        /// <summary>
        /// 平仓盈亏
        /// </summary>
        public double CloseProfit;
        /// <summary>
        /// 持仓盈亏
        /// </summary>
        public double PositionProfit;
        /// <summary>
        /// 期货结算准备金
        /// </summary>
        public double Balance;
        /// <summary>
        /// 可用资金
        /// </summary>
        public double Available;
        /// <summary>
        /// 可取资金
        /// </summary>
        public double WithdrawQuota;
        /// <summary>
        /// 基本准备金
        /// </summary>
        public double Reserve;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 信用额度
        /// </summary>
        public double Credit;
        /// <summary>
        /// 质押金额
        /// </summary>
        public double Mortgage;
        /// <summary>
        /// 交易所保证金
        /// </summary>
        public double ExchangeMargin;
        /// <summary>
        /// 投资者交割保证金
        /// </summary>
        public double DeliveryMargin;
        /// <summary>
        /// 交易所交割保证金
        /// </summary>
        public double ExchangeDeliveryMargin;
        /// <summary>
        /// 保底期货结算准备金
        /// </summary>
        public double ReserveBalance;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 上次货币质入金额
        /// </summary>
        public double PreFundMortgageIn;
        /// <summary>
        /// 上次货币质出金额
        /// </summary>
        public double PreFundMortgageOut;
        /// <summary>
        /// 货币质入金额
        /// </summary>
        public double FundMortgageIn;
        /// <summary>
        /// 货币质出金额
        /// </summary>
        public double FundMortgageOut;
        /// <summary>
        /// 货币质押余额
        /// </summary>
        public double FundMortgageAvailable;
        /// <summary>
        /// 可质押货币金额
        /// </summary>
        public double MortgageableFund;
        /// <summary>
        /// 特殊产品占用保证金
        /// </summary>
        public double SpecProductMargin;
        /// <summary>
        /// 特殊产品冻结保证金
        /// </summary>
        public double SpecProductFrozenMargin;
        /// <summary>
        /// 特殊产品手续费
        /// </summary>
        public double SpecProductCommission;
        /// <summary>
        /// 特殊产品冻结手续费
        /// </summary>
        public double SpecProductFrozenCommission;
        /// <summary>
        /// 特殊产品持仓盈亏
        /// </summary>
        public double SpecProductPositionProfit;
        /// <summary>
        /// 特殊产品平仓盈亏
        /// </summary>
        public double SpecProductCloseProfit;
        /// <summary>
        /// 根据持仓盈亏算法计算的特殊产品持仓盈亏
        /// </summary>
        public double SpecProductPositionProfitByAlg;
        /// <summary>
        /// 特殊产品交易所保证金
        /// </summary>
        public double SpecProductExchangeMargin;
        ///// <summary>
        ///// 期权平仓盈亏
        ///// </summary>
        //public double OptionCloseProfit;
        ///// <summary>
        ///// 期权市值
        ///// </summary>
        //public double OptionValue;
    }

    /// <summary>
    /// 投资者持仓
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInvestorPositionField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 持仓多空方向
        /// </summary>
        public EnumThostPosiDirectionType PosiDirection;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 持仓日期
        /// </summary>
        public EnumThostPositionDateType PositionDate;
        /// <summary>
        /// 上日持仓
        /// </summary>
        public int YdPosition;
        /// <summary>
        /// 今日持仓
        /// </summary>
        public int Position;
        /// <summary>
        /// 多头冻结
        /// </summary>
        public int LongFrozen;
        /// <summary>
        /// 空头冻结
        /// </summary>
        public int ShortFrozen;
        /// <summary>
        /// 开仓冻结金额
        /// </summary>
        public double LongFrozenAmount;
        /// <summary>
        /// 开仓冻结金额
        /// </summary>
        public double ShortFrozenAmount;
        /// <summary>
        /// 开仓量
        /// </summary>
        public int OpenVolume;
        /// <summary>
        /// 平仓量
        /// </summary>
        public int CloseVolume;
        /// <summary>
        /// 开仓金额
        /// </summary>
        public double OpenAmount;
        /// <summary>
        /// 平仓金额
        /// </summary>
        public double CloseAmount;
        /// <summary>
        /// 持仓成本
        /// </summary>
        public double PositionCost;
        /// <summary>
        /// 上次占用的保证金
        /// </summary>
        public double PreMargin;
        /// <summary>
        /// 占用的保证金
        /// </summary>
        public double UseMargin;
        /// <summary>
        /// 冻结的保证金
        /// </summary>
        public double FrozenMargin;
        /// <summary>
        /// 冻结的资金
        /// </summary>
        public double FrozenCash;
        /// <summary>
        /// 冻结的手续费
        /// </summary>
        public double FrozenCommission;
        /// <summary>
        /// 资金差额
        /// </summary>
        public double CashIn;
        /// <summary>
        /// 手续费
        /// </summary>
        public double Commission;
        /// <summary>
        /// 平仓盈亏
        /// </summary>
        public double CloseProfit;
        /// <summary>
        /// 持仓盈亏
        /// </summary>
        public double PositionProfit;
        /// <summary>
        /// 上次结算价
        /// </summary>
        public double PreSettlementPrice;
        /// <summary>
        /// 本次结算价
        /// </summary>
        public double SettlementPrice;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 开仓成本
        /// </summary>
        public double OpenCost;
        /// <summary>
        /// 交易所保证金
        /// </summary>
        public double ExchangeMargin;
        /// <summary>
        /// 组合成交形成的持仓
        /// </summary>
        public int CombPosition;
        /// <summary>
        /// 组合多头冻结
        /// </summary>
        public int CombLongFrozen;
        /// <summary>
        /// 组合空头冻结
        /// </summary>
        public int CombShortFrozen;
        /// <summary>
        /// 逐日盯市平仓盈亏
        /// </summary>
        public double CloseProfitByDate;
        /// <summary>
        /// 逐笔对冲平仓盈亏
        /// </summary>
        public double CloseProfitByTrade;
        /// <summary>
        /// 今日持仓
        /// </summary>
        public int TodayPosition;
        /// <summary>
        /// 保证金率
        /// </summary>
        public double MarginRateByMoney;
        /// <summary>
        /// 保证金率(按手数)
        /// </summary>
        public double MarginRateByVolume;
        /// <summary>
        /// 执行冻结
        /// </summary>
        public int StrikeFrozen;
        /// <summary>
        /// 执行冻结金额
        /// </summary>
        public double StrikeFrozenAmount;
        /// <summary>
        /// 放弃执行冻结
        /// </summary>
        public int AbandonFrozen;
        ///// <summary>
        ///// 期权市值
        ///// </summary>
        //public double OptionValue;
    }

    /// <summary>
    /// 合约保证金率
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInstrumentMarginRateField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
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
        /// 是否相对交易所收取
        /// </summary>
        public EnumThostBoolType IsRelative;
    }

    /// <summary>
    /// 合约手续费率
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInstrumentCommissionRateField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 开仓手续费率
        /// </summary>
        public double OpenRatioByMoney;
        /// <summary>
        /// 开仓手续费
        /// </summary>
        public double OpenRatioByVolume;
        /// <summary>
        /// 平仓手续费率
        /// </summary>
        public double CloseRatioByMoney;
        /// <summary>
        /// 平仓手续费
        /// </summary>
        public double CloseRatioByVolume;
        /// <summary>
        /// 平今手续费率
        /// </summary>
        public double CloseTodayRatioByMoney;
        /// <summary>
        /// 平今手续费
        /// </summary>
        public double CloseTodayRatioByVolume;
    }

    /// <summary>
    /// 深度行情
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcDepthMarketDataField
    {
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 最新价
        /// </summary>
        public double LastPrice;
        /// <summary>
        /// 上次结算价
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
        /// 今收盘
        /// </summary>
        public double ClosePrice;
        /// <summary>
        /// 本次结算价
        /// </summary>
        public double SettlementPrice;
        /// <summary>
        /// 涨停板价
        /// </summary>
        public double UpperLimitPrice;
        /// <summary>
        /// 跌停板价
        /// </summary>
        public double LowerLimitPrice;
        /// <summary>
        /// 昨虚实度
        /// </summary>
        public double PreDelta;
        /// <summary>
        /// 今虚实度
        /// </summary>
        public double CurrDelta;
        /// <summary>
        /// 最后修改时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string UpdateTime;
        /// <summary>
        /// 最后修改毫秒
        /// </summary>
        public int UpdateMillisec;
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
        /// 申卖价二
        /// </summary>
        public double AskPrice2;
        /// <summary>
        /// 申卖量二
        /// </summary>
        public int AskVolume2;
        /// <summary>
        /// 申买价三
        /// </summary>
        public double BidPrice3;
        /// <summary>
        /// 申买量三
        /// </summary>
        public int BidVolume3;
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
        /// 申卖价四
        /// </summary>
        public double AskPrice4;
        /// <summary>
        /// 申卖量四
        /// </summary>
        public int AskVolume4;
        /// <summary>
        /// 申买价五
        /// </summary>
        public double BidPrice5;
        /// <summary>
        /// 申买量五
        /// </summary>
        public int BidVolume5;
        /// <summary>
        /// 申卖价五
        /// </summary>
        public double AskPrice5;
        /// <summary>
        /// 申卖量五
        /// </summary>
        public int AskVolume5;
        /// <summary>
        /// 当日均价
        /// </summary>
        public double AveragePrice;
        /// <summary>
        /// 业务日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionDay;
    }

    /// <summary>
    /// 投资者合约交易权限
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInstrumentTradingRightField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 交易权限
        /// </summary>
        public EnumThostTradingRightType TradingRight;
    }

    /// <summary>
    /// 经纪公司用户
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcBrokerUserField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 用户名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string UserName;
        /// <summary>
        /// 用户类型
        /// </summary>
        public EnumThostUserTypeType UserType;
        /// <summary>
        /// 是否活跃
        /// </summary>
        public int IsActive;
        /// <summary>
        /// 是否使用令牌
        /// </summary>
        public int IsUsingOTP;
    }

    /// <summary>
    /// 经纪公司用户口令
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcBrokerUserPasswordField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
    }

    /// <summary>
    /// 经纪公司用户功能权限
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcBrokerUserFunctionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 经纪公司功能代码
        /// </summary>
        public EnumThostBrokerFunctionCodeType BrokerFunctionCode;
    }

    /// <summary>
    /// 交易所交易员报盘机
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTraderOfferField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderLocalID;
        /// <summary>
        /// 交易所交易员连接状态
        /// </summary>
        public EnumThostTraderConnectStatusType TraderConnectStatus;
        /// <summary>
        /// 发出连接请求的日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ConnectRequestDate;
        /// <summary>
        /// 发出连接请求的时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ConnectRequestTime;
        /// <summary>
        /// 上次报告日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LastReportDate;
        /// <summary>
        /// 上次报告时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LastReportTime;
        /// <summary>
        /// 完成连接日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ConnectDate;
        /// <summary>
        /// 完成连接时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ConnectTime;
        /// <summary>
        /// 启动日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string StartDate;
        /// <summary>
        /// 启动时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string StartTime;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 本席位最大成交编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MaxTradeID;
        /// <summary>
        /// 本席位最大报单备拷
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string MaxOrderMessageReference;
    }

    /// <summary>
    /// 投资者结算结果
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSettlementInfoField
    {
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 序号
        /// </summary>
        public int SequenceNo;
        /// <summary>
        /// 消息正文
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 501)]
        public byte[] Content;
    }

    /// <summary>
    /// 合约保证金率调整
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInstrumentMarginRateAdjustField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
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
        /// 是否相对交易所收取
        /// </summary>
        public int IsRelative;
    }

    /// <summary>
    /// 交易所保证金率
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExchangeMarginRateField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
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
    }

    /// <summary>
    /// 交易所保证金率调整
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExchangeMarginRateAdjustField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 跟随交易所投资者多头保证金率
        /// </summary>
        public double LongMarginRatioByMoney;
        /// <summary>
        /// 跟随交易所投资者多头保证金费
        /// </summary>
        public double LongMarginRatioByVolume;
        /// <summary>
        /// 跟随交易所投资者空头保证金率
        /// </summary>
        public double ShortMarginRatioByMoney;
        /// <summary>
        /// 跟随交易所投资者空头保证金费
        /// </summary>
        public double ShortMarginRatioByVolume;
        /// <summary>
        /// 交易所多头保证金率
        /// </summary>
        public double ExchLongMarginRatioByMoney;
        /// <summary>
        /// 交易所多头保证金费
        /// </summary>
        public double ExchLongMarginRatioByVolume;
        /// <summary>
        /// 交易所空头保证金率
        /// </summary>
        public double ExchShortMarginRatioByMoney;
        /// <summary>
        /// 交易所空头保证金费
        /// </summary>
        public double ExchShortMarginRatioByVolume;
        /// <summary>
        /// 不跟随交易所投资者多头保证金率
        /// </summary>
        public double NoLongMarginRatioByMoney;
        /// <summary>
        /// 不跟随交易所投资者多头保证金费
        /// </summary>
        public double NoLongMarginRatioByVolume;
        /// <summary>
        /// 不跟随交易所投资者空头保证金率
        /// </summary>
        public double NoShortMarginRatioByMoney;
        /// <summary>
        /// 不跟随交易所投资者空头保证金费
        /// </summary>
        public double NoShortMarginRatioByVolume;
    }

    /// <summary>
    /// 汇率
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExchangeRateField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 源币种
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string FromCurrencyID;
        /// <summary>
        /// 源币种单位数量
        /// </summary>
        public double FromCurrencyUnit;
        /// <summary>
        /// 目标币种
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string ToCurrencyID;
        /// <summary>
        /// 汇率
        /// </summary>
        public double ExchangeRate;
    };

    /// <summary>
    /// 结算引用
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSettlementRefField
    {
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
    }

    /// <summary>
    /// 当前时间
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcCurrentTimeField
    {
        /// <summary>
        /// 当前日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string CurrDate;
        /// <summary>
        /// 当前时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string CurrTime;
        /// <summary>
        /// 当前时间（毫秒）
        /// </summary>
        public int CurrMillisec;
        /// <summary>
        /// 业务日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionDay;
    }

    /// <summary>
    /// 通讯阶段
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcCommPhaseField
    {
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 通讯时段编号
        /// </summary>
        public short CommPhaseNo;
        /// <summary>
        /// 系统编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string SystemID;
    }

    /// <summary>
    /// 登录信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcLoginInfoField
    {
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 登录日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LoginDate;
        /// <summary>
        /// 登录时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LoginTime;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// 用户端产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string UserProductInfo;
        /// <summary>
        /// 接口端产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string InterfaceProductInfo;
        /// <summary>
        /// 协议信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ProtocolInfo;
        /// <summary>
        /// 系统名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string SystemName;
        /// <summary>
        /// 密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 最大报单引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string MaxOrderRef;
        /// <summary>
        /// 上期所时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string SHFETime;
        /// <summary>
        /// 大商所时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string DCETime;
        /// <summary>
        /// 郑商所时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string CZCETime;
        /// <summary>
        /// 中金所时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string FFEXTime;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
        /// <summary>
        /// 动态密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string OneTimePassword;
        /// <summary>
        /// 能源中心时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string INETime;
        /// <summary>
        /// 查询时是否需要流控
        /// </summary>
        public EnumThostBoolType IsQryControl;
        /// <summary>
        /// 登录备注
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string LoginRemark;
    }

    /// <summary>
    /// 登录信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcLogoutAllField
    {
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 系统名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string SystemName;
    }

    /// <summary>
    /// 前置状态
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcFrontStatusField
    {
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 上次报告日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LastReportDate;
        /// <summary>
        /// 上次报告时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LastReportTime;
        /// <summary>
        /// 是否活跃
        /// </summary>
        public int IsActive;
    }

    /// <summary>
    /// 用户口令变更
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcUserPasswordUpdateField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 原来的口令
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string OldPassword;
        /// <summary>
        /// 新的口令
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string NewPassword;
    }

    /// <summary>
    /// 输入报单
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInputOrderField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 报单引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderRef;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 报单价格条件
        /// </summary>
        public EnumThostOrderPriceTypeType OrderPriceType;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        public EnumThostOffsetFlagType CombOffsetFlag_0;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        public EnumThostOffsetFlagType CombOffsetFlag_1;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        public EnumThostOffsetFlagType CombOffsetFlag_2;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        public EnumThostOffsetFlagType CombOffsetFlag_3;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        public EnumThostOffsetFlagType CombOffsetFlag_4;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType CombHedgeFlag_0;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType CombHedgeFlag_1;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType CombHedgeFlag_2;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType CombHedgeFlag_3;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType CombHedgeFlag_4;
        /// <summary>
        /// 价格
        /// </summary>
        public double LimitPrice;
        /// <summary>
        /// 数量
        /// </summary>
        public int VolumeTotalOriginal;
        /// <summary>
        /// 有效期类型
        /// </summary>
        public EnumThostTimeConditionType TimeCondition;
        /// <summary>
        /// GTD日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string GTDDate;
        /// <summary>
        /// 成交量类型
        /// </summary>
        public EnumThostVolumeConditionType VolumeCondition;
        /// <summary>
        /// 最小成交量
        /// </summary>
        public int MinVolume;
        /// <summary>
        /// 触发条件
        /// </summary>
        public EnumThostContingentConditionType ContingentCondition;
        /// <summary>
        /// 止损价
        /// </summary>
        public double StopPrice;
        /// <summary>
        /// 强平原因
        /// </summary>
        public EnumThostForceCloseReasonType ForceCloseReason;
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
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 用户强评标志
        /// </summary>
        public int UserForceClose;
        /// <summary>
        /// 互换单标志
        /// </summary>
        public int IsSwapOrder;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// 资金账号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 交易编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    }

    /// <summary>
    /// 报单
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcOrderField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 报单引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderRef;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 报单价格条件
        /// </summary>
        public EnumThostOrderPriceTypeType OrderPriceType;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        public EnumThostOffsetFlagType CombOffsetFlag_0;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        public EnumThostOffsetFlagType CombOffsetFlag_1;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        public EnumThostOffsetFlagType CombOffsetFlag_2;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        public EnumThostOffsetFlagType CombOffsetFlag_3;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        public EnumThostOffsetFlagType CombOffsetFlag_4;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType CombHedgeFlag_0;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType CombHedgeFlag_1;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType CombHedgeFlag_2;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType CombHedgeFlag_3;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType CombHedgeFlag_4;
        /// <summary>
        /// 价格
        /// </summary>
        public double LimitPrice;
        /// <summary>
        /// 数量
        /// </summary>
        public int VolumeTotalOriginal;
        /// <summary>
        /// 有效期类型
        /// </summary>
        public EnumThostTimeConditionType TimeCondition;
        /// <summary>
        /// GTD日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string GTDDate;
        /// <summary>
        /// 成交量类型
        /// </summary>
        public EnumThostVolumeConditionType VolumeCondition;
        /// <summary>
        /// 最小成交量
        /// </summary>
        public int MinVolume;
        /// <summary>
        /// 触发条件
        /// </summary>
        public EnumThostContingentConditionType ContingentCondition;
        /// <summary>
        /// 止损价
        /// </summary>
        public double StopPrice;
        /// <summary>
        /// 强平原因
        /// </summary>
        public EnumThostForceCloseReasonType ForceCloseReason;
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
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderLocalID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 报单提交状态
        /// </summary>
        public EnumThostOrderSubmitStatusType OrderSubmitStatus;
        /// <summary>
        /// 报单提示序号
        /// </summary>
        public int NotifySequence;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string OrderSysID;
        /// <summary>
        /// 报单来源
        /// </summary>
        public EnumThostOrderSourceType OrderSource;
        /// <summary>
        /// 报单状态
        /// </summary>
        public EnumThostOrderStatusType OrderStatus;
        /// <summary>
        /// 报单类型
        /// </summary>
        public EnumThostOrderTypeType OrderType;
        /// <summary>
        /// 今成交数量
        /// </summary>
        public int VolumeTraded;
        /// <summary>
        /// 剩余数量
        /// </summary>
        public int VolumeTotal;
        /// <summary>
        /// 报单日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertDate;
        /// <summary>
        /// 委托时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertTime;
        /// <summary>
        /// 激活时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActiveTime;
        /// <summary>
        /// 挂起时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string SuspendTime;
        /// <summary>
        /// 最后修改时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string UpdateTime;
        /// <summary>
        /// 撤销时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string CancelTime;
        /// <summary>
        /// 最后修改交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string ActiveTraderID;
        /// <summary>
        /// 结算会员编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClearingPartID;
        /// <summary>
        /// 序号
        /// </summary>
        public int SequenceNo;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 用户端产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string UserProductInfo;
        /// <summary>
        /// 状态信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string StatusMsg;
        /// <summary>
        /// 用户强评标志
        /// </summary>
        public int UserForceClose;
        /// <summary>
        /// 操作用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ActiveUserID;
        /// <summary>
        /// 经纪公司报单编号
        /// </summary>
        public int BrokerOrderSeq;
        /// <summary>
        /// 相关报单
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string RelativeOrderSysID;
        /// <summary>
        /// 郑商所成交数量
        /// </summary>
        public int ZCETotalTradedVolume;
        /// <summary>
        /// 互换单标志
        /// </summary>
        public int IsSwapOrder;
        /// <summary>
        /// 营业部编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string BranchID;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// 资金账号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    }

    /// <summary>
    /// 交易所报单
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExchangeOrderField
    {
        /// <summary>
        /// 报单价格条件
        /// </summary>
        public EnumThostOrderPriceTypeType OrderPriceType;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string CombOffsetFlag;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string CombHedgeFlag;
        /// <summary>
        /// 价格
        /// </summary>
        public double LimitPrice;
        /// <summary>
        /// 数量
        /// </summary>
        public int VolumeTotalOriginal;
        /// <summary>
        /// 有效期类型
        /// </summary>
        public EnumThostTimeConditionType TimeCondition;
        /// <summary>
        /// GTD日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string GTDDate;
        /// <summary>
        /// 成交量类型
        /// </summary>
        public EnumThostVolumeConditionType VolumeCondition;
        /// <summary>
        /// 最小成交量
        /// </summary>
        public int MinVolume;
        /// <summary>
        /// 触发条件
        /// </summary>
        public EnumThostContingentConditionType ContingentCondition;
        /// <summary>
        /// 止损价
        /// </summary>
        public double StopPrice;
        /// <summary>
        /// 强平原因
        /// </summary>
        public EnumThostForceCloseReasonType ForceCloseReason;
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
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderLocalID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 报单提交状态
        /// </summary>
        public EnumThostOrderSubmitStatusType OrderSubmitStatus;
        /// <summary>
        /// 报单提示序号
        /// </summary>
        public int NotifySequence;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string OrderSysID;
        /// <summary>
        /// 报单来源
        /// </summary>
        public EnumThostOrderSourceType OrderSource;
        /// <summary>
        /// 报单状态
        /// </summary>
        public EnumThostOrderStatusType OrderStatus;
        /// <summary>
        /// 报单类型
        /// </summary>
        public EnumThostOrderTypeType OrderType;
        /// <summary>
        /// 今成交数量
        /// </summary>
        public int VolumeTraded;
        /// <summary>
        /// 剩余数量
        /// </summary>
        public int VolumeTotal;
        /// <summary>
        /// 报单日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertDate;
        /// <summary>
        /// 委托时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertTime;
        /// <summary>
        /// 激活时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActiveTime;
        /// <summary>
        /// 挂起时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string SuspendTime;
        /// <summary>
        /// 最后修改时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string UpdateTime;
        /// <summary>
        /// 撤销时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string CancelTime;
        /// <summary>
        /// 最后修改交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string ActiveTraderID;
        /// <summary>
        /// 结算会员编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClearingPartID;
        /// <summary>
        /// 序号
        /// </summary>
        public int SequenceNo;
        /// <summary>
        /// 营业部编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string BranchID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    }

    /// <summary>
    /// 交易所报单插入失败
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExchangeOrderInsertErrorField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderLocalID;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
    }

    /// <summary>
    /// 输入报单操作
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInputOrderActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 报单操作引用
        /// </summary>
        public int OrderActionRef;
        /// <summary>
        /// 报单引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderRef;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string OrderSysID;
        /// <summary>
        /// 操作标志
        /// </summary>
        public EnumThostActionFlagType ActionFlag;
        /// <summary>
        /// 价格
        /// </summary>
        public double LimitPrice;
        /// <summary>
        /// 数量变化
        /// </summary>
        public int VolumeChange;
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
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    }

    /// <summary>
    /// 报单操作
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcOrderActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 报单操作引用
        /// </summary>
        public int OrderActionRef;
        /// <summary>
        /// 报单引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderRef;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string OrderSysID;
        /// <summary>
        /// 操作标志
        /// </summary>
        public EnumThostActionFlagType ActionFlag;
        /// <summary>
        /// 价格
        /// </summary>
        public double LimitPrice;
        /// <summary>
        /// 数量变化
        /// </summary>
        public int VolumeChange;
        /// <summary>
        /// 操作日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionDate;
        /// <summary>
        /// 操作时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionTime;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderLocalID;
        /// <summary>
        /// 操作本地编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ActionLocalID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 报单操作状态
        /// </summary>
        public EnumThostOrderActionStatusType OrderActionStatus;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 状态信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string StatusMsg;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 营业部编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string BranchID;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    }

    /// <summary>
    /// 交易所报单操作
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExchangeOrderActionField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string OrderSysID;
        /// <summary>
        /// 操作标志
        /// </summary>
        public EnumThostActionFlagType ActionFlag;
        /// <summary>
        /// 价格
        /// </summary>
        public double LimitPrice;
        /// <summary>
        /// 数量变化
        /// </summary>
        public int VolumeChange;
        /// <summary>
        /// 操作日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionDate;
        /// <summary>
        /// 操作时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionTime;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderLocalID;
        /// <summary>
        /// 操作本地编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ActionLocalID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 报单操作状态
        /// </summary>
        public EnumThostOrderActionStatusType OrderActionStatus;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 营业部编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string BranchID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    }

    /// <summary>
    /// 交易所报单操作失败
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExchangeOrderActionErrorField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string OrderSysID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderLocalID;
        /// <summary>
        /// 操作本地编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ActionLocalID;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
    }

    /// <summary>
    /// 交易所成交
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExchangeTradeField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 成交编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TradeID;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string OrderSysID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 交易角色
        /// </summary>
        public EnumThostTradingRoleType TradingRole;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 开平标志
        /// </summary>
        public EnumThostOffsetFlagType OffsetFlag;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 价格
        /// </summary>
        public double Price;
        /// <summary>
        /// 数量
        /// </summary>
        public int Volume;
        /// <summary>
        /// 成交时期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 成交时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 成交类型
        /// </summary>
        public EnumThostTradeTypeType TradeType;
        /// <summary>
        /// 成交价来源
        /// </summary>
        public EnumThostPriceSourceType PriceSource;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderLocalID;
        /// <summary>
        /// 结算会员编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClearingPartID;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 序号
        /// </summary>
        public int SequenceNo;
        /// <summary>
        /// 成交来源
        /// </summary>
        public EnumThostTradeSourceType TradeSource;
    }

    /// <summary>
    /// 成交
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTradeField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 报单引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderRef;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 成交编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TradeID;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string OrderSysID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 交易角色
        /// </summary>
        public EnumThostTradingRoleType TradingRole;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 开平标志
        /// </summary>
        public EnumThostOffsetFlagType OffsetFlag;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 价格
        /// </summary>
        public double Price;
        /// <summary>
        /// 数量
        /// </summary>
        public int Volume;
        /// <summary>
        /// 成交时期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 成交时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 成交类型
        /// </summary>
        public EnumThostTradeTypeType TradeType;
        /// <summary>
        /// 成交价来源
        /// </summary>
        public EnumThostPriceSourceType PriceSource;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderLocalID;
        /// <summary>
        /// 结算会员编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClearingPartID;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 序号
        /// </summary>
        public int SequenceNo;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 经纪公司报单编号
        /// </summary>
        public int BrokerOrderSeq;
        /// <summary>
        /// 成交来源
        /// </summary>
        public EnumThostTradeSourceType TradeSource;
    }

    /// <summary>
    /// 用户会话
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcUserSessionField
    {
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 登录日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LoginDate;
        /// <summary>
        /// 登录时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LoginTime;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// 用户端产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string UserProductInfo;
        /// <summary>
        /// 接口端产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string InterfaceProductInfo;
        /// <summary>
        /// 协议信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ProtocolInfo;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
        /// <summary>
        /// 登录备注
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string LoginRemark;
    }

    /// <summary>
    /// 查询最大报单数量
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQueryMaxOrderVolumeField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 开平标志
        /// </summary>
        public EnumThostOffsetFlagType OffsetFlag;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 最大允许报单数量
        /// </summary>
        public int MaxVolume;
    }

    /// <summary>
    /// 投资者结算结果确认信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSettlementInfoConfirmField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 确认日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ConfirmDate;
        /// <summary>
        /// 确认时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ConfirmTime;
    }

    /// <summary>
    /// 出入金同步
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSyncDepositField
    {
        /// <summary>
        /// 出入金流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string DepositSeqNo;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 入金金额
        /// </summary>
        public double Deposit;
        /// <summary>
        /// 是否强制进行
        /// </summary>
        public int IsForce;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
    }

    /// <summary>
    /// 货币质押同步
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSyncFundMortgageField
    {
        /// <summary>
        /// 货币质押流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string MortgageSeqNo;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 源币种
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string FromCurrencyID;
        /// <summary>
        /// 质押金额
        /// </summary>
        public double MortgageAmount;
        /// <summary>
        /// 目标币种
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string ToCurrencyID;
    };

    /// <summary>
    /// 经纪公司同步
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcBrokerSyncField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
    }

    /// <summary>
    /// 正在同步中的投资者
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSyncingInvestorField
    {
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者分组代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorGroupID;
        /// <summary>
        /// 投资者名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string InvestorName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdentifiedCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 是否活跃
        /// </summary>
        public int IsActive;
        /// <summary>
        /// 联系电话
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Telephone;
        /// <summary>
        /// 通讯地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
        public string Address;
        /// <summary>
        /// 开户日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string OpenDate;
        /// <summary>
        /// 手机
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Mobile;
        /// <summary>
        /// 手续费率模板代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string CommModelID;
        /// <summary>
        /// 保证金率模板代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string MarginModelID;
    }

    /// <summary>
    /// 正在同步中的交易代码
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSyncingTradingCodeField
    {
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 是否活跃
        /// </summary>
        public int IsActive;
        /// <summary>
        /// 交易编码类型
        /// </summary>
        public EnumThostClientIDTypeType ClientIDType;
    }

    /// <summary>
    /// 正在同步中的投资者分组
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSyncingInvestorGroupField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者分组代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorGroupID;
        /// <summary>
        /// 投资者分组名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string InvestorGroupName;
    }

    /// <summary>
    /// 正在同步中的交易账号
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSyncingTradingAccountField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 上次质押金额
        /// </summary>
        public double PreMortgage;
        /// <summary>
        /// 上次信用额度
        /// </summary>
        public double PreCredit;
        /// <summary>
        /// 上次存款额
        /// </summary>
        public double PreDeposit;
        /// <summary>
        /// 上次结算准备金
        /// </summary>
        public double PreBalance;
        /// <summary>
        /// 上次占用的保证金
        /// </summary>
        public double PreMargin;
        /// <summary>
        /// 利息基数
        /// </summary>
        public double InterestBase;
        /// <summary>
        /// 利息收入
        /// </summary>
        public double Interest;
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
        /// 冻结的资金
        /// </summary>
        public double FrozenCash;
        /// <summary>
        /// 冻结的手续费
        /// </summary>
        public double FrozenCommission;
        /// <summary>
        /// 当前保证金总额
        /// </summary>
        public double CurrMargin;
        /// <summary>
        /// 资金差额
        /// </summary>
        public double CashIn;
        /// <summary>
        /// 手续费
        /// </summary>
        public double Commission;
        /// <summary>
        /// 平仓盈亏
        /// </summary>
        public double CloseProfit;
        /// <summary>
        /// 持仓盈亏
        /// </summary>
        public double PositionProfit;
        /// <summary>
        /// 期货结算准备金
        /// </summary>
        public double Balance;
        /// <summary>
        /// 可用资金
        /// </summary>
        public double Available;
        /// <summary>
        /// 可取资金
        /// </summary>
        public double WithdrawQuota;
        /// <summary>
        /// 基本准备金
        /// </summary>
        public double Reserve;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 信用额度
        /// </summary>
        public double Credit;
        /// <summary>
        /// 质押金额
        /// </summary>
        public double Mortgage;
        /// <summary>
        /// 交易所保证金
        /// </summary>
        public double ExchangeMargin;
        /// <summary>
        /// 投资者交割保证金
        /// </summary>
        public double DeliveryMargin;
        /// <summary>
        /// 交易所交割保证金
        /// </summary>
        public double ExchangeDeliveryMargin;
        /// <summary>
        /// 保底期货结算准备金
        /// </summary>
        public double ReserveBalance;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 上次货币质入金额
        /// </summary>
        public double PreFundMortgageIn;
        /// <summary>
        /// 上次货币质出金额
        /// </summary>
        public double PreFundMortgageOut;
        /// <summary>
        /// 货币质入金额
        /// </summary>
        public double FundMortgageIn;
        /// <summary>
        /// 货币质出金额
        /// </summary>
        public double FundMortgageOut;
        /// <summary>
        /// 货币质押余额
        /// </summary>
        public double FundMortgageAvailable;
        /// <summary>
        /// 可质押货币金额
        /// </summary>
        public double MortgageableFund;
        /// <summary>
        /// 特殊产品占用保证金
        /// </summary>
        public double SpecProductMargin;
        /// <summary>
        /// 特殊产品冻结保证金
        /// </summary>
        public double SpecProductFrozenMargin;
        /// <summary>
        /// 特殊产品手续费
        /// </summary>
        public double SpecProductCommission;
        /// <summary>
        /// 特殊产品冻结手续费
        /// </summary>
        public double SpecProductFrozenCommission;
        /// <summary>
        /// 特殊产品持仓盈亏
        /// </summary>
        public double SpecProductPositionProfit;
        /// <summary>
        /// 特殊产品平仓盈亏
        /// </summary>
        public double SpecProductCloseProfit;
        /// <summary>
        /// 根据持仓盈亏算法计算的特殊产品持仓盈亏
        /// </summary>
        public double SpecProductPositionProfitByAlg;
        /// <summary>
        /// 特殊产品交易所保证金
        /// </summary>
        public double SpecProductExchangeMargin;
        ///// <summary>
        ///// 期权平仓盈亏
        ///// </summary>
        //public double OptionCloseProfit;
        ///// <summary>
        ///// 期权市值
        ///// </summary>
        //public double OptionValue;
    }

    /// <summary>
    /// 正在同步中的投资者持仓
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSyncingInvestorPositionField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 持仓多空方向
        /// </summary>
        public EnumThostPosiDirectionType PosiDirection;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 持仓日期
        /// </summary>
        public EnumThostPositionDateType PositionDate;
        /// <summary>
        /// 上日持仓
        /// </summary>
        public int YdPosition;
        /// <summary>
        /// 今日持仓
        /// </summary>
        public int Position;
        /// <summary>
        /// 多头冻结
        /// </summary>
        public int LongFrozen;
        /// <summary>
        /// 空头冻结
        /// </summary>
        public int ShortFrozen;
        /// <summary>
        /// 开仓冻结金额
        /// </summary>
        public double LongFrozenAmount;
        /// <summary>
        /// 开仓冻结金额
        /// </summary>
        public double ShortFrozenAmount;
        /// <summary>
        /// 开仓量
        /// </summary>
        public int OpenVolume;
        /// <summary>
        /// 平仓量
        /// </summary>
        public int CloseVolume;
        /// <summary>
        /// 开仓金额
        /// </summary>
        public double OpenAmount;
        /// <summary>
        /// 平仓金额
        /// </summary>
        public double CloseAmount;
        /// <summary>
        /// 持仓成本
        /// </summary>
        public double PositionCost;
        /// <summary>
        /// 上次占用的保证金
        /// </summary>
        public double PreMargin;
        /// <summary>
        /// 占用的保证金
        /// </summary>
        public double UseMargin;
        /// <summary>
        /// 冻结的保证金
        /// </summary>
        public double FrozenMargin;
        /// <summary>
        /// 冻结的资金
        /// </summary>
        public double FrozenCash;
        /// <summary>
        /// 冻结的手续费
        /// </summary>
        public double FrozenCommission;
        /// <summary>
        /// 资金差额
        /// </summary>
        public double CashIn;
        /// <summary>
        /// 手续费
        /// </summary>
        public double Commission;
        /// <summary>
        /// 平仓盈亏
        /// </summary>
        public double CloseProfit;
        /// <summary>
        /// 持仓盈亏
        /// </summary>
        public double PositionProfit;
        /// <summary>
        /// 上次结算价
        /// </summary>
        public double PreSettlementPrice;
        /// <summary>
        /// 本次结算价
        /// </summary>
        public double SettlementPrice;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 开仓成本
        /// </summary>
        public double OpenCost;
        /// <summary>
        /// 交易所保证金
        /// </summary>
        public double ExchangeMargin;
        /// <summary>
        /// 组合成交形成的持仓
        /// </summary>
        public int CombPosition;
        /// <summary>
        /// 组合多头冻结
        /// </summary>
        public int CombLongFrozen;
        /// <summary>
        /// 组合空头冻结
        /// </summary>
        public int CombShortFrozen;
        /// <summary>
        /// 逐日盯市平仓盈亏
        /// </summary>
        public double CloseProfitByDate;
        /// <summary>
        /// 逐笔对冲平仓盈亏
        /// </summary>
        public double CloseProfitByTrade;
        /// <summary>
        /// 今日持仓
        /// </summary>
        public int TodayPosition;
        /// <summary>
        /// 保证金率
        /// </summary>
        public double MarginRateByMoney;
        /// <summary>
        /// 保证金率(按手数)
        /// </summary>
        public double MarginRateByVolume;
        /// <summary>
        /// 执行冻结
        /// </summary>
        public int StrikeFrozen;
        /// <summary>
        /// 执行冻结金额
        /// </summary>
        public double StrikeFrozenAmount;
        /// <summary>
        /// 放弃执行冻结
        /// </summary>
        public int AbandonFrozen;
        ///// <summary>
        ///// 期权市值
        ///// </summary>
        //public double OptionValue;
    }

    /// <summary>
    /// 正在同步中的合约保证金率
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSyncingInstrumentMarginRateField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
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
        /// 是否相对交易所收取
        /// </summary>
        public int IsRelative;
    }

    /// <summary>
    /// 正在同步中的合约手续费率
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSyncingInstrumentCommissionRateField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 开仓手续费率
        /// </summary>
        public double OpenRatioByMoney;
        /// <summary>
        /// 开仓手续费
        /// </summary>
        public double OpenRatioByVolume;
        /// <summary>
        /// 平仓手续费率
        /// </summary>
        public double CloseRatioByMoney;
        /// <summary>
        /// 平仓手续费
        /// </summary>
        public double CloseRatioByVolume;
        /// <summary>
        /// 平今手续费率
        /// </summary>
        public double CloseTodayRatioByMoney;
        /// <summary>
        /// 平今手续费
        /// </summary>
        public double CloseTodayRatioByVolume;
    }

    /// <summary>
    /// 正在同步中的合约交易权限
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSyncingInstrumentTradingRightField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 交易权限
        /// </summary>
        public EnumThostTradingRightType TradingRight;
    }

    /// <summary>
    /// 查询报单
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryOrderField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string OrderSysID;
        /// <summary>
        /// 开始时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertTimeStart;
        /// <summary>
        /// 结束时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertTimeEnd;
    }

    /// <summary>
    /// 查询成交
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryTradeField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 成交编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TradeID;
        /// <summary>
        /// 开始时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTimeStart;
        /// <summary>
        /// 结束时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTimeEnd;
    }

    /// <summary>
    /// 查询投资者持仓
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryInvestorPositionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    }

    /// <summary>
    /// 查询资金账户
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryTradingAccountField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
    }

    /// <summary>
    /// 查询投资者
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryInvestorField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
    }

    /// <summary>
    /// 查询交易编码
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryTradingCodeField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 交易编码类型
        /// </summary>
        public EnumThostClientIDTypeType ClientIDType;
    }

    /// <summary>
    /// 查询投资者组
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryInvestorGroupField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
    }

    /// <summary>
    /// 查询合约保证金率
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryInstrumentMarginRateField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
    }

    /// <summary>
    /// 查询手续费率
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryInstrumentCommissionRateField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    }

    /// <summary>
    /// 查询合约交易权限
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryInstrumentTradingRightField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    }

    /// <summary>
    /// 查询经纪公司
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryBrokerField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
    }

    /// <summary>
    /// 查询交易员
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryTraderField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
    }

    /// <summary>
    /// 查询管理用户功能权限
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQrySuperUserFunctionField
    {
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
    }

    /// <summary>
    /// 查询用户会话
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryUserSessionField
    {
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
    }

    /// <summary>
    /// 查询经纪公司会员代码
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryPartBrokerField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
    }

    /// <summary>
    /// 查询前置状态
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryFrontStatusField
    {
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
    }

    /// <summary>
    /// 查询交易所报单
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryExchangeOrderField
    {
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
    }

    /// <summary>
    /// 查询报单操作
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryOrderActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
    }

    /// <summary>
    /// 查询交易所报单操作
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryExchangeOrderActionField
    {
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
    }

    /// <summary>
    /// 查询管理用户
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQrySuperUserField
    {
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
    }

    /// <summary>
    /// 查询交易所
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryExchangeField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
    }

    /// <summary>
    /// 查询产品
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryProductField
    {
        /// <summary>
        /// 产品代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ProductID;
        /// <summary>
        /// 产品类型
        /// </summary>
        public EnumThostProductClassType ProductClass;
    }

    /// <summary>
    /// 查询合约
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryInstrumentField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 产品代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ProductID;
    }

    /// <summary>
    /// 查询行情
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryDepthMarketDataField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    }

    /// <summary>
    /// 查询经纪公司用户
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryBrokerUserField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
    }

    /// <summary>
    /// 查询经纪公司用户权限
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryBrokerUserFunctionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
    }

    /// <summary>
    /// 查询交易员报盘机
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryTraderOfferField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
    }

    /// <summary>
    /// 查询出入金流水
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQrySyncDepositField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 出入金流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string DepositSeqNo;
    }

    /// <summary>
    /// 查询投资者结算结果
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQrySettlementInfoField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
    }

    /// <summary>
    /// 查询交易所保证金率
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryExchangeMarginRateField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
    };

    /// <summary>
    /// 查询交易所调整保证金率
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryExchangeMarginRateAdjustField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
    };

    /// <summary>
    /// 查询汇率
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryExchangeRateField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        ///源币种
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string FromCurrencyID;
        ///目标币种
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string ToCurrencyID;
    };

    /// <summary>
    /// 查询货币质押流水
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQrySyncFundMortgageField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 货币质押流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string MortgageSeqNo;
    };

    /// <summary>
    /// 查询报单
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryHisOrderField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string OrderSysID;
        /// <summary>
        /// 开始时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertTimeStart;
        /// <summary>
        /// 结束时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertTimeEnd;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
    }

    ///当前期权合约最小保证金
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcOptionInstrMiniMarginField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 单位（手）期权合约最小保证金
        /// </summary>
        public double MinMargin;
        /// <summary>
        /// 取值方式
        /// </summary>
        public EnumThostValueMethodType ValueMethod;
        /// <summary>
        /// 是否跟随交易所收取
        /// </summary>
        public int IsRelative;
    };

    ///当前期权合约保证金调整系数
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcOptionInstrMarginAdjustField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 投机空头保证金调整系数
        /// </summary>
        public double SShortMarginRatioByMoney;
        /// <summary>
        /// 投机空头保证金调整系数
        /// </summary>
        public double SShortMarginRatioByVolume;
        /// <summary>
        /// 保值空头保证金调整系数
        /// </summary>
        public double HShortMarginRatioByMoney;
        /// <summary>
        /// 保值空头保证金调整系数
        /// </summary>
        public double HShortMarginRatioByVolume;
        /// <summary>
        /// 套利空头保证金调整系数
        /// </summary>
        public double AShortMarginRatioByMoney;
        /// <summary>
        /// 套利空头保证金调整系数
        /// </summary>
        public double AShortMarginRatioByVolume;
        /// <summary>
        /// 是否跟随交易所收取
        /// </summary>
        public int IsRelative;
        /// <summary>
        /// 做市商空头保证金调整系数
        /// </summary>
        public double MShortMarginRatioByMoney;
        /// <summary>
        /// 做市商空头保证金调整系数
        /// </summary>
        public double MShortMarginRatioByVolume;
    };

    ///当前期权合约手续费的详细内容
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcOptionInstrCommRateField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 开仓手续费率
        /// </summary>
        public double OpenRatioByMoney;
        /// <summary>
        /// 开仓手续费
        /// </summary>
        public double OpenRatioByVolume;
        /// <summary>
        /// 平仓手续费率
        /// </summary>
        public double CloseRatioByMoney;
        /// <summary>
        /// 平仓手续费
        /// </summary>
        public double CloseRatioByVolume;
        /// <summary>
        /// 平今手续费率
        /// </summary>
        public double CloseTodayRatioByMoney;
        /// <summary>
        /// 平今手续费
        /// </summary>
        public double CloseTodayRatioByVolume;
        /// <summary>
        /// 执行手续费率
        /// </summary>
        public double StrikeRatioByMoney;
        /// <summary>
        /// 执行手续费
        /// </summary>
        public double StrikeRatioByVolume;
    };

    ///期权交易成本
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcOptionInstrTradeCostField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
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
        /// <summary>
        /// 交易所期权合约保证金不变部分
        /// </summary>
        public double ExchFixedMargin;
        /// <summary>
        /// 交易所期权合约最小保证金
        /// </summary>
        public double ExchMiniMargin;
    };

    ///期权交易成本查询
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryOptionInstrTradeCostField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 期权合约报价
        /// </summary>
        public double InputPrice;
        /// <summary>
        /// 标的价格,填0则用昨结算价
        /// </summary>
        public double UnderlyingPrice;
    };

    ///期权手续费率查询
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryOptionInstrCommRateField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    };

    ///股指现货指数
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcIndexPriceField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 指数现货收盘价
        /// </summary>
        public double ClosePrice;
    };

    ///输入的执行宣告
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInputExecOrderField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
		/// 执行宣告引用
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ExecOrderRef;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 数量
        /// </summary>
        public int Volume;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 开平标志
        /// </summary>
        public EnumThostOffsetFlagType OffsetFlag;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 执行类型
        /// </summary>
        public EnumThostActionTypeType ActionType;
        /// <summary>
        /// 保留头寸申请的持仓方向
		/// </summary>
		public EnumThostPosiDirectionType PosiDirection;
        /// <summary>
        /// 期权行权后是否保留期货头寸的标记
        /// </summary>
        public EnumThostExecOrderPositionFlagType ReservePositionFlag;
        /// <summary>
        /// 期权行权后生成的头寸是否自动平仓
        /// </summary>
        public EnumThostExecOrderCloseFlagType CloseFlag;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// 资金账号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 交易编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///输入执行宣告操作
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInputExecOrderActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 执行宣告操作引用
        /// </summary>
        public int ExecOrderActionRef;
        /// <summary>
        /// 执行宣告引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ExecOrderRef;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
		/// 交易所代码
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 执行宣告操作编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string ExecOrderSysID;
        /// <summary>
        /// 操作标志
        /// </summary>
        public EnumThostActionFlagType ActionFlag;
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
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///执行宣告
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExecOrderField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 执行宣告引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ExecOrderRef;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 数量
        /// </summary>
        public int Volume;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 开平标志
        /// </summary>
        public EnumThostOffsetFlagType OffsetFlag;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 执行类型
        /// </summary>
        public EnumThostActionTypeType ActionType;
        /// <summary>
        /// 保留头寸申请的持仓方向
        /// </summary>
        public EnumThostPosiDirectionType PosiDirection;
        /// <summary>
        /// 期权行权后是否保留期货头寸的标记
        /// </summary>
        public EnumThostExecOrderPositionFlagType ReservePositionFlag;
        /// <summary>
        /// 期权行权后生成的头寸是否自动平仓
        /// </summary>
        public EnumThostExecOrderCloseFlagType CloseFlag;
        /// <summary>
        /// 本地执行宣告编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ExecOrderLocalID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 执行宣告提交状态
        /// </summary>
        public EnumThostOrderSubmitStatusType OrderSubmitStatus;
        /// <summary>
        /// 报单提示序号
        /// </summary>
        public int NotifySequence;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 执行宣告操作编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string ExecOrderSysID;
        /// <summary>
        /// 报单日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertDate;
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
        /// 执行结果
        /// </summary>
        public EnumThostExecResultType ExecResult;
        /// <summary>
        /// 结算会员编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClearingPartID;
        /// <summary>
        /// 序号
        /// </summary>
        public int SequenceNo;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 用户端产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string UserProductInfo;
        /// <summary>
        /// 状态信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string StatusMsg;
        /// <summary>
        /// 操作用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ActiveUserID;
        /// <summary>
        /// 经纪公司报单编号
        /// </summary>
        public int BrokerExecOrderSeq;
        /// <summary>
        /// 营业部编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string BranchID;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// 资金账号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///执行宣告操作
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExecOrderActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 执行宣告操作引用
        /// </summary>
        public int ExecOrderActionRef;
        /// <summary>
        /// 执行宣告引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ExecOrderRef;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 执行宣告操作编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string ExecOrderSysID;
        /// <summary>
        /// 操作标志
        /// </summary>
        public EnumThostActionFlagType ActionFlag;
        /// <summary>
        /// 操作日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionDate;
        /// <summary>
        /// 操作时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionTime;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 本地执行宣告编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ExecOrderLocalID;
        /// <summary>
        /// 操作本地编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ActionLocalID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
		/// 业务单元
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 报单操作状态
        /// </summary>
        public EnumThostOrderActionStatusType OrderActionStatus;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 执行类型
        /// </summary>
        public EnumThostActionTypeType ActionType;
        /// <summary>
        /// 状态信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string StatusMsg;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 营业部编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string BranchID;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///执行宣告查询
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryExecOrderField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 执行宣告操作编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string ExecOrderSysID;
        /// <summary>
        /// 开始时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertTimeStart;
        /// <summary>
        /// 结束时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertTimeEnd;
    };

    ///交易所执行宣告信息
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExchangeExecOrderField
    {
        /// <summary>
        /// 数量
        /// </summary>
        public int Volume;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 开平标志
        /// </summary>
        public EnumThostOffsetFlagType OffsetFlag;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 执行类型
        /// </summary>
        public EnumThostActionTypeType ActionType;
        /// <summary>
        /// 保留头寸申请的持仓方向
        /// </summary>
        public EnumThostPosiDirectionType PosiDirection;
        /// <summary>
        /// 期权行权后是否保留期货头寸的标记
        /// </summary>
        public EnumThostExecOrderPositionFlagType ReservePositionFlag;
        /// <summary>
        /// 期权行权后生成的头寸是否自动平仓
        /// </summary>
        public EnumThostExecOrderCloseFlagType CloseFlag;
        /// <summary>
        /// 本地执行宣告编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ExecOrderLocalID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 报单提交状态
        /// </summary>
        public EnumThostOrderSubmitStatusType OrderSubmitStatus;
        /// <summary>
        /// 报单提示序号
        /// </summary>
        public int NotifySequence;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 执行宣告操作编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string ExecOrderSysID;
        /// <summary>
        /// 报单日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertDate;
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
        /// 执行结果
        /// </summary>
        public EnumThostExecResultType ExecResult;
        /// <summary>
        /// 结算会员编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClearingPartID;
        /// <summary>
        /// 序号
        /// </summary>
        public int SequenceNo;
        /// <summary>
        /// 营业部编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string BranchID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///交易所执行宣告查询
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryExchangeExecOrderField
    {
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
    };

    ///执行宣告操作查询
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryExecOrderActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
    };

    ///交易所执行宣告操作
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExchangeExecOrderActionField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 执行宣告操作编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string ExecOrderSysID;
        /// <summary>
        /// 操作标志
        /// </summary>
        public EnumThostActionFlagType ActionFlag;
        /// <summary>
        /// 操作日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionDate;
        /// <summary>
        /// 操作时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionTime;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 本地执行宣告编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ExecOrderLocalID;
        /// <summary>
        /// 操作本地编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ActionLocalID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 报单操作状态
        /// </summary>
        public EnumThostOrderActionStatusType OrderActionStatus;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 执行类型
        /// </summary>
        public EnumThostActionTypeType ActionType;
        /// <summary>
        /// 营业部编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string BranchID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///交易所执行宣告操作查询
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryExchangeExecOrderActionField
    {
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
    };

    ///错误执行宣告
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcErrExecOrderField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 执行宣告引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ExecOrderRef;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 数量
        /// </summary>
        public int Volume;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 开平标志
        /// </summary>
        public EnumThostOffsetFlagType OffsetFlag;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 执行类型
        /// </summary>
        public EnumThostActionTypeType ActionType;
        /// <summary>
        /// 保留头寸申请的持仓方向
        /// </summary>
        public EnumThostPosiDirectionType PosiDirection;
        /// <summary>
        /// 期权行权后是否保留期货头寸的标记
        /// </summary>
        public EnumThostExecOrderPositionFlagType ReservePositionFlag;
        /// <summary>
        /// 期权行权后生成的头寸是否自动平仓
        /// </summary>
        public EnumThostExecOrderCloseFlagType CloseFlag;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// 资金账号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 交易编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
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

    ///查询错误执行宣告
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryErrExecOrderField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
    };

    ///错误执行宣告操作
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcErrExecOrderActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 执行宣告操作引用
        /// </summary>
        public int ExecOrderActionRef;
        /// <summary>
        /// 执行宣告引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ExecOrderRef;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 执行宣告操作编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string ExecOrderSysID;
        /// <summary>
        /// 操作标志
        /// </summary>
        public EnumThostActionFlagType ActionFlag;
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
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
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

    ///查询错误执行宣告操作
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryErrExecOrderActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
    };

    ///投资者期权合约交易权限
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcOptionInstrTradingRightField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 交易权限
        /// </summary>
        public EnumThostTradingRightType TradingRight;
    };

    ///查询期权合约交易权限
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryOptionInstrTradingRightField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumThostDirectionType Direction;
    };

    ///输入的询价
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInputForQuoteField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 询价引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ForQuoteRef;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///询价
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcForQuoteField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 询价引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ForQuoteRef;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 本地询价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ForQuoteLocalID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 报单日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertDate;
        /// <summary>
        /// 插入时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertTime;
        /// <summary>
        /// 询价状态
        /// </summary>
        public EnumThostForQuoteStatusType ForQuoteStatus;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 状态信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string StatusMsg;
        /// <summary>
        /// 操作用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ActiveUserID;
        /// <summary>
        /// 经纪公司询价编号
        /// </summary>
        public int BrokerForQutoSeq;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///询价查询
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryForQuoteField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 开始时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertTimeStart;
        /// <summary>
        /// 结束时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertTimeEnd;
    };

    ///交易所询价信息
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExchangeForQuoteField
    {
        /// <summary>
        /// 本地询价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ForQuoteLocalID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 报单日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertDate;
        /// <summary>
        /// 插入时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertTime;
        /// <summary>
        /// 询价状态
        /// </summary>
        public EnumThostForQuoteStatusType ForQuoteStatus;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///交易所询价查询
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryExchangeForQuoteField
    {
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
    };

    ///输入的报价
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInputQuoteField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 报价引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string QuoteRef;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 卖价格
        /// </summary>
        public double AskPrice;
        /// <summary>
        /// 买价格
        /// </summary>
        public double BidPrice;
        /// <summary>
        /// 卖数量
        /// </summary>
        public int AskVolume;
        /// <summary>
        /// 买数量
        /// </summary>
        public int BidVolume;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 卖开平标志
        /// </summary>
        public EnumThostOffsetFlagType AskOffsetFlag;
        /// <summary>
        /// 买开平标志
        /// </summary>
        public EnumThostOffsetFlagType BidOffsetFlag;
        /// <summary>
        /// 卖投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType AskHedgeFlag;
        /// <summary>
        /// 买投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType BidHedgeFlag;
        /// <summary>
        /// 衍生卖报单引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AskOrderRef;
        /// <summary>
        /// 衍生买报单引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BidOrderRef;
        /// <summary>
        /// 应价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string ForQuoteSysID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// 交易编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///输入报价操作
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInputQuoteActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 报价操作引用
        /// </summary>
        public int QuoteActionRef;
        /// <summary>
        /// 报价引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string QuoteRef;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 报价操作编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string QuoteSysID;
        /// <summary>
        /// 操作标志
        /// </summary>
        public EnumThostActionFlagType ActionFlag;
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
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// 交易编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///报价
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQuoteField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 报价引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string QuoteRef;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 卖价格
        /// </summary>
        public double AskPrice;
        /// <summary>
        /// 买价格
        /// </summary>
        public double BidPrice;
        /// <summary>
        /// 卖数量
        /// </summary>
        public int AskVolume;
        /// <summary>
        /// 买数量
        /// </summary>
        public int BidVolume;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 卖开平标志
        /// </summary>
        public EnumThostOffsetFlagType AskOffsetFlag;
        /// <summary>
        /// 买开平标志
        /// </summary>
        public EnumThostOffsetFlagType BidOffsetFlag;
        /// <summary>
        /// 卖投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType AskHedgeFlag;
        /// <summary>
        /// 买投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType BidHedgeFlag;
        /// <summary>
        /// 本地报价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string QuoteLocalID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 报单提示序号
        /// </summary>
        public int NotifySequence;
        /// <summary>
        /// 报单提交状态
        /// </summary>
        public EnumThostOrderSubmitStatusType OrderSubmitStatus;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 报价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string QuoteSysID;
        /// <summary>
        /// 报单日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertDate;
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
        /// 报价状态
        /// </summary>
        public EnumThostOrderStatusType QuoteStatus;
        /// <summary>
        /// 结算会员编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClearingPartID;
        /// <summary>
        /// 序号
        /// </summary>
        public int SequenceNo;
        /// <summary>
        /// 卖方报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string AskOrderSysID;
        /// <summary>
        /// 买方报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BidOrderSysID;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 用户端产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string UserProductInfo;
        /// <summary>
        /// 状态信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string StatusMsg;
        /// <summary>
        /// 操作用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ActiveUserID;
        /// <summary>
        /// 经纪公司报价编号
        /// </summary>
        public int BrokerQuoteSeq;
        /// <summary>
        /// 衍生卖报单引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AskOrderRef;
        /// <summary>
        /// 衍生买报单引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BidOrderRef;
        /// <summary>
        /// 应价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string ForQuoteSysID;
        /// <summary>
        /// 营业部编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string BranchID;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// 资金账号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///报价操作
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQuoteActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 报价操作引用
        /// </summary>
        public int QuoteActionRef;
        /// <summary>
        /// 报价引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string QuoteRef;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 报价操作编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string QuoteSysID;
        /// <summary>
        /// 操作标志
        /// </summary>
        public EnumThostActionFlagType ActionFlag;
        /// <summary>
        /// 操作日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionDate;
        /// <summary>
        /// 操作时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionTime;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 本地报价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string QuoteLocalID;
        /// <summary>
        /// 操作本地编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ActionLocalID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 报单操作状态
        /// </summary>
        public EnumThostOrderActionStatusType OrderActionStatus;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 状态信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string StatusMsg;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 营业部编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string BranchID;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///报价查询
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryQuoteField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 报价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string QuoteSysID;
        /// <summary>
        /// 开始时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertTimeStart;
        /// <summary>
        /// 结束时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertTimeEnd;
    };

    ///交易所报价信息
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExchangeQuoteField
    {
        /// <summary>
        /// 卖价格
        /// </summary>
        public double AskPrice;
        /// <summary>
        /// 买价格
        /// </summary>
        public double BidPrice;
        /// <summary>
        /// 卖数量
        /// </summary>
        public int AskVolume;
        /// <summary>
        /// 买数量
        /// </summary>
        public int BidVolume;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 卖开平标志
        /// </summary>
        public EnumThostOffsetFlagType AskOffsetFlag;
        /// <summary>
        /// 买开平标志
        /// </summary>
        public EnumThostOffsetFlagType BidOffsetFlag;
        /// <summary>
        /// 卖投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType AskHedgeFlag;
        /// <summary>
        /// 买投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType BidHedgeFlag;
        /// <summary>
        /// 本地报价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string QuoteLocalID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 报单提示序号
        /// </summary>
        public int NotifySequence;
        /// <summary>
        /// 报单提交状态
        /// </summary>
        public EnumThostOrderSubmitStatusType OrderSubmitStatus;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 报价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string QuoteSysID;
        /// <summary>
        /// 报单日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertDate;
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
        /// 报价状态
        /// </summary>
        public EnumThostOrderStatusType QuoteStatus;
        /// <summary>
        /// 结算会员编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClearingPartID;
        /// <summary>
        /// 序号
        /// </summary>
        public int SequenceNo;
        /// <summary>
        /// 卖方报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string AskOrderSysID;
        /// <summary>
        /// 买方报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BidOrderSysID;
        /// <summary>
        /// 应价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string ForQuoteSysID;
        /// <summary>
        /// 营业部编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string BranchID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///交易所报价查询
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryExchangeQuoteField
    {
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
    };

    ///报价操作查询
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryQuoteActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
    };

    ///交易所报价操作
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExchangeQuoteActionField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 报价操作编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string QuoteSysID;
        /// <summary>
        /// 操作标志
        /// </summary>
        public EnumThostActionFlagType ActionFlag;
        /// <summary>
        /// 操作日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionDate;
        /// <summary>
        /// 操作时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionTime;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 本地报价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string QuoteLocalID;
        /// <summary>
        /// 操作本地编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ActionLocalID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        //// <summary>
        /// 报单操作状态
        /// </summary>
        public EnumThostOrderActionStatusType OrderActionStatus;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///交易所报价操作查询
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryExchangeQuoteActionField
    {
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
    };

    ///期权合约delta值
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcOptionInstrDeltaField
    {
        /// <summary>
		/// 合约代码
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// Delta值
        /// </summary>
        public double Delta;
    };

    ///发给做市商的询价请求
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcForQuoteRspField
    {
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 询价编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string ForQuoteSysID;
        /// <summary>
        /// 询价时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ForQuoteTime;
        /// <summary>
        /// 业务日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionDay;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
    };

    ///当前期权合约执行偏移值的详细内容
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcStrikeOffsetField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 执行偏移值
        /// </summary>
        public double Offset;
        /// <summary>
        /// 执行偏移类型
        /// </summary>
        public EnumThostFtdcStrikeOffsetType OffsetType;
    };

    ///期权执行偏移值查询
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryStrikeOffsetField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    };

    ///输入批量报单操作
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInputBatchOrderActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 报单操作引用
        /// </summary>
        public int OrderActionRef;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///批量报单操作
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcBatchOrderActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 报单操作引用
        /// </summary>
        public int OrderActionRef;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 操作日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionDate;
        /// <summary>
        /// 操作时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionTime;
        /// <summary>
		/// 交易所交易员代码
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
		/// 安装编号
		/// </summary>
		public int InstallID;
        /// <summary>
        /// 本地申请组合编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ActionLocalID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 报单操作状态
        /// </summary>
        public EnumThostOrderActionStatusType OrderActionStatus;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 状态信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string StatusMsg;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///交易所批量报单操作
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExchangeBatchOrderActionField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 操作日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionDate;
        /// <summary>
        /// 操作时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionTime;
        /// <summary>
		/// 交易所交易员代码
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
		/// 安装编号
		/// </summary>
		public int InstallID;
        /// <summary>
        /// 本地申请组合编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ActionLocalID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 报单操作状态
        /// </summary>
        public EnumThostOrderActionStatusType OrderActionStatus;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    ///查询批量报单操作
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryBatchOrderActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
    };

    /// <summary>
    /// 组合合约安全系数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcCombInstrumentGuardField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 
        /// </summary>
        public double GuarantRatio;
    };

    /// <summary>
    /// 组合合约安全系数查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryCombInstrumentGuardField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    };

    /// <summary>
    /// 输入的申请组合
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInputCombActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 组合引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string CombActionRef;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 数量
        /// </summary>
        public int Volume;
        /// <summary>
        /// 组合指令方向
        /// </summary>
        public EnumThostCombDirectionType CombDirection;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    /// <summary>
    /// 申请组合
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcCombActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 组合引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string CombActionRef;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 数量
        /// </summary>
        public int Volume;
        /// <summary>
        /// 组合指令方向
        /// </summary>
        public EnumThostCombDirectionType CombDirection;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 本地申请组合编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ActionLocalID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
		/// 交易所交易员代码
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
		/// 安装编号
		/// </summary>
		public int InstallID;
        /// <summary>
        /// 组合状态
        /// </summary>
        public EnumThostOrderActionStatusType ActionStatus;
        /// <summary>
        /// 报单提示序号
        /// </summary>
        public int NotifySequence;
        /// <summary>
		/// 交易日
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
		/// 序号
		/// </summary>
		public int SequenceNo;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 用户端产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string UserProductInfo;
        /// <summary>
        /// 状态信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string StatusMsg;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    /// <summary>
    /// 申请组合查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryCombActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
    };

    /// <summary>
    /// 交易所申请组合信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExchangeCombActionField
    {
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 数量
        /// </summary>
        public int Volume;
        /// <summary>
        /// 组合指令方向
        /// </summary>
        public EnumThostCombDirectionType CombDirection;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 本地申请组合编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ActionLocalID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
		/// 交易所交易员代码
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
		/// 安装编号
		/// </summary>
		public int InstallID;
        /// <summary>
        /// 组合状态
        /// </summary>
        public EnumThostOrderActionStatusType ActionStatus;
        /// <summary>
        /// 报单提示序号
        /// </summary>
        public int NotifySequence;
        /// <summary>
		/// 交易日
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 序列号
        /// </summary>
        public int SequenceNo;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    };

    /// <summary>
    /// 交易所申请组合查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryExchangeCombActionField
    {
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
		/// 交易所交易员代码
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
    };

    /// <summary>
    /// 产品报价汇率
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcProductExchRateField
    {
        /// <summary>
        /// 产品代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ProductID;
        /// <summary>
        /// 报价币种类型
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string QuoteCurrencyID;
        /// <summary>
        /// 汇率
        /// </summary>
        public double ExchangeRate;
    };

    /// <summary>
    /// 产品报价汇率查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryProductExchRateField
    {
        /// <summary>
        /// 产品代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ProductID;
    };

    ///查询询价价差参数
    public struct CThostFtdcQryForQuoteParamField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
    };

    ///询价价差参数
    public struct CThostFtdcForQuoteParamField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 最新价
        /// </summary>
        public double LastPrice;
        /// <summary>
        /// 价差
        /// </summary>
        public double PriceInterval;
    };

    ///当前做市商期权合约手续费的详细内容
    public struct CThostFtdcMMOptionInstrCommRateField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 开仓手续费率
        /// </summary>
        public double OpenRatioByMoney;
        /// <summary>
        /// 开仓手续费
        /// </summary>
        public double OpenRatioByVolume;
        /// <summary>
        /// 平仓手续费率
        /// </summary>
        public double CloseRatioByMoney;
        /// <summary>
        /// 平仓手续费
        /// </summary>
        public double CloseRatioByVolume;
        /// <summary>
        /// 平今手续费率
        /// </summary>
        public double CloseTodayRatioByMoney;
        /// <summary>
        /// 平今手续费
        /// </summary>
        public double CloseTodayRatioByVolume;
        /// <summary>
        /// 执行手续费率
        /// </summary>
        public double StrikeRatioByMoney;
        /// <summary>
        /// 执行手续费
        /// </summary>
        public double StrikeRatioByVolume;
    };

    ///做市商期权手续费率查询
    public struct CThostFtdcQryMMOptionInstrCommRateField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    };

    ///做市商合约手续费率
    public struct CThostFtdcMMInstrumentCommissionRateField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 开仓手续费率
        /// </summary>
        public double OpenRatioByMoney;
        /// <summary>
        /// 开仓手续费
        /// </summary>
        public double OpenRatioByVolume;
        /// <summary>
        /// 平仓手续费率
        /// </summary>
        public double CloseRatioByMoney;
        /// <summary>
        /// 平仓手续费
        /// </summary>
        public double CloseRatioByVolume;
        /// <summary>
        /// 平今手续费率
        /// </summary>
        public double CloseTodayRatioByMoney;
        /// <summary>
        /// 平今手续费
        /// </summary>
        public double CloseTodayRatioByVolume;
    };

    ///查询做市商合约手续费率
    public struct CThostFtdcQryMMInstrumentCommissionRateField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    };

    ///当前报单手续费的详细内容
    public struct CThostFtdcInstrumentOrderCommRateField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 报单手续费
        /// </summary>
        public double OrderCommByVolume;
        /// <summary>
        /// 撤单手续费
        /// </summary>
        public double OrderActionCommByVolume;
    };

    ///报单手续费率查询
    public struct CThostFtdcQryInstrumentOrderCommRateField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    };

    /// <summary>
    /// 市场行情
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcMarketDataField
    {
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 最新价
        /// </summary>
        public double LastPrice;
        /// <summary>
        /// 上次结算价
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
        /// 今收盘
        /// </summary>
        public double ClosePrice;
        /// <summary>
        /// 本次结算价
        /// </summary>
        public double SettlementPrice;
        /// <summary>
        /// 涨停板价
        /// </summary>
        public double UpperLimitPrice;
        /// <summary>
        /// 跌停板价
        /// </summary>
        public double LowerLimitPrice;
        /// <summary>
        /// 昨虚实度
        /// </summary>
        public double PreDelta;
        /// <summary>
        /// 今虚实度
        /// </summary>
        public double CurrDelta;
        /// <summary>
        /// 最后修改时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string UpdateTime;
        /// <summary>
        /// 最后修改毫秒
        /// </summary>
        public int UpdateMillisec;
        /// <summary>
        /// 业务日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionDay;
    }

    /// <summary>
    /// 行情基础属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcMarketDataBaseField
    {
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 上次结算价
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
    }

    /// <summary>
    /// 行情静态属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcMarketDataStaticField
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
        /// 本次结算价
        /// </summary>
        public double SettlementPrice;
        /// <summary>
        /// 今虚实度
        /// </summary>
        public double CurrDelta;
    }

    /// <summary>
    /// 行情最新成交属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcMarketDataLastMatchField
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
    }

    /// <summary>
    /// 行情最优价属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcMarketDataBestPriceField
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
    }

    /// <summary>
    /// 行情申买二、三属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcMarketDataBid23Field
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
    }

    /// <summary>
    /// 行情申卖二、三属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcMarketDataAsk23Field
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
    }

    /// <summary>
    /// 行情申买四、五属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcMarketDataBid45Field
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
    }

    /// <summary>
    /// 行情申卖四、五属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcMarketDataAsk45Field
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
    }

    /// <summary>
    /// 行情更新时间属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcMarketDataUpdateTimeField
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
        /// <summary>
        /// 业务日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionDay;
    }

    /// <summary>
    /// 行情交易所代码属性
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcMarketDataExchangeField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
    };

    /// <summary>
    /// 指定的合约
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSpecificInstrumentField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    }

    /// <summary>
    /// 合约状态
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInstrumentStatusField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 结算组代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string SettlementGroupID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 合约交易状态
        /// </summary>
        public EnumThostInstrumentStatusType InstrumentStatus;
        /// <summary>
        /// 交易阶段编号
        /// </summary>
        public int TradingSegmentSN;
        /// <summary>
        /// 进入本状态时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string EnterTime;
        /// <summary>
        /// 进入本状态原因
        /// </summary>
        public EnumThostInstStatusEnterReasonType EnterReason;
    }

    /// <summary>
    /// 查询合约状态
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryInstrumentStatusField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
    }

    /// <summary>
    /// 投资者账户
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInvestorAccountField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
    }

    /// <summary>
    /// 浮动盈亏算法
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcPositionProfitAlgorithmField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 盈亏算法
        /// </summary>
        public EnumThostAlgorithmType Algorithm;
        /// <summary>
        /// 备注
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string Memo;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
    }

    /// <summary>
    /// 会员资金折扣
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcDiscountField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 资金折扣比例
        /// </summary>
        public double Discount;
    }

    /// <summary>
    /// 查询转帐银行
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryTransferBankField
    {
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分中心代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBrchID;
    }

    /// <summary>
    /// 转帐银行
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTransferBankField
    {
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分中心代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBrchID;
        /// <summary>
        /// 银行名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
        public string BankName;
        /// <summary>
        /// 是否活跃
        /// </summary>
        public int IsActive;
    }

    /// <summary>
    /// 查询投资者持仓明细
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryInvestorPositionDetailField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    }

    /// <summary>
    /// 投资者持仓明细
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInvestorPositionDetailField
    {
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 买卖
        /// </summary>
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 开仓日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string OpenDate;
        /// <summary>
        /// 成交编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TradeID;
        /// <summary>
        /// 数量
        /// </summary>
        public int Volume;
        /// <summary>
        /// 开仓价
        /// </summary>
        public double OpenPrice;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 成交类型
        /// </summary>
        public EnumThostTradeTypeType TradeType;
        /// <summary>
        /// 组合合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string CombInstrumentID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 逐日盯市平仓盈亏
        /// </summary>
        public double CloseProfitByDate;
        /// <summary>
        /// 逐笔对冲平仓盈亏
        /// </summary>
        public double CloseProfitByTrade;
        /// <summary>
        /// 逐日盯市持仓盈亏
        /// </summary>
        public double PositionProfitByDate;
        /// <summary>
        /// 逐笔对冲持仓盈亏
        /// </summary>
        public double PositionProfitByTrade;
        /// <summary>
        /// 投资者保证金
        /// </summary>
        public double Margin;
        /// <summary>
        /// 交易所保证金
        /// </summary>
        public double ExchMargin;
        /// <summary>
        /// 保证金率
        /// </summary>
        public double MarginRateByMoney;
        /// <summary>
        /// 保证金率(按手数)
        /// </summary>
        public double MarginRateByVolume;
        /// <summary>
        /// 昨结算价
        /// </summary>
        public double LastSettlementPrice;
        /// <summary>
        /// 结算价
        /// </summary>
        public double SettlementPrice;
        /// <summary>
        /// 平仓量
        /// </summary>
        public int CloseVolume;
        /// <summary>
        /// 平仓金额
        /// </summary>
        public double CloseAmount;
    }

    /// <summary>
    /// 资金账户口令域
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTradingAccountPasswordField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
    }

    /// <summary>
    /// 交易所行情报盘机
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcMDTraderOfferField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderLocalID;
        /// <summary>
        /// 交易所交易员连接状态
        /// </summary>
        public EnumThostTraderConnectStatusType TraderConnectStatus;
        /// <summary>
        /// 发出连接请求的日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ConnectRequestDate;
        /// <summary>
        /// 发出连接请求的时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ConnectRequestTime;
        /// <summary>
        /// 上次报告日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LastReportDate;
        /// <summary>
        /// 上次报告时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string LastReportTime;
        /// <summary>
        /// 完成连接日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ConnectDate;
        /// <summary>
        /// 完成连接时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ConnectTime;
        /// <summary>
        /// 启动日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string StartDate;
        /// <summary>
        /// 启动时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string StartTime;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 本席位最大成交编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MaxTradeID;
        /// <summary>
        /// 本席位最大报单备拷
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string MaxOrderMessageReference;
    }

    /// <summary>
    /// 查询行情报盘机
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryMDTraderOfferField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
    }

    /// <summary>
    /// 查询客户通知
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryNoticeField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
    }

    /// <summary>
    /// 客户通知
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcNoticeField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 消息正文
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 501)]
        public string Content;
        /// <summary>
        /// 经纪公司通知内容序列号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string SequenceLabel;
    }

    /// <summary>
    /// 用户权限
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcUserRightField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 客户权限类型
        /// </summary>
        public EnumThostUserRightTypeType UserRightType;
        /// <summary>
        /// 是否禁止
        /// </summary>
        public int IsForbidden;
    }

    /// <summary>
    /// 查询结算信息确认域
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQrySettlementInfoConfirmField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
    }

    /// <summary>
    /// 装载结算信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcLoadSettlementInfoField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
    }

    /// <summary>
    /// 经纪公司可提资金算法表
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcBrokerWithdrawAlgorithmField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 可提资金算法
        /// </summary>
        public EnumThostAlgorithmType WithdrawAlgorithm;
        /// <summary>
        /// 资金使用率
        /// </summary>
        public double UsingRatio;
        /// <summary>
        /// 可提是否包含平仓盈利
        /// </summary>
        public EnumThostIncludeCloseProfitType IncludeCloseProfit;
        /// <summary>
        /// 本日无仓且无成交客户是否受可提比例限制
        /// </summary>
        public EnumThostAllWithoutTradeType AllWithoutTrade;
        /// <summary>
        /// 可用是否包含平仓盈利
        /// </summary>
        public EnumThostIncludeCloseProfitType AvailIncludeCloseProfit;
        /// <summary>
        /// 是否启用用户事件
        /// </summary>
        public int IsBrokerUserEvent;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 货币质押比率
        /// </summary>
        public double FundMortgageRatio;
        /// <summary>
        /// 权益算法
        /// </summary>
        public EnumThostBalanceAlgorithmType BalanceAlgorithm;
    }

    /// <summary>
    /// 资金账户口令变更域
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTradingAccountPasswordUpdateV1Field
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 原来的口令
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string OldPassword;
        /// <summary>
        /// 新的口令
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string NewPassword;
    }

    /// <summary>
    /// 资金账户口令变更域
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTradingAccountPasswordUpdateField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 原来的口令
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string OldPassword;
        /// <summary>
        /// 新的口令
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string NewPassword;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
    }

    /// <summary>
    /// 查询组合合约分腿
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryCombinationLegField
    {
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
    }

    /// <summary>
    /// 查询组合合约分腿
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQrySyncStatusField
    {
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
    }

    /// <summary>
    /// 组合交易合约的单腿
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcCombinationLegField
    {
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
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 单腿乘数
        /// </summary>
        public int LegMultiple;
        /// <summary>
        /// 派生层数
        /// </summary>
        public int ImplyLevel;
    }

    /// <summary>
    /// 数据同步状态
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSyncStatusField
    {
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 数据同步状态
        /// </summary>
        public EnumThostDataSyncStatusType DataSyncStatus;
    }

    /// <summary>
    /// 查询联系人
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryLinkManField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
    }

    /// <summary>
    /// 联系人
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcLinkManField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 联系人类型
        /// </summary>
        public EnumThostPersonTypeType PersonType;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdentifiedCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string PersonName;
        /// <summary>
        /// 联系电话
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Telephone;
        /// <summary>
        /// 通讯地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
        public string Address;
        /// <summary>
        /// 邮政编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string ZipCode;
        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority;
        /// <summary>
        /// 开户邮政编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string UOAZipCode;
        /// <summary>
        /// 全称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
        public string PersonFullName;
    }

    /// <summary>
    /// 查询经纪公司用户事件
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryBrokerUserEventField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 用户事件类型
        /// </summary>
        public EnumThostUserEventTypeType UserEventType;
    }

    /// <summary>
    /// 查询经纪公司用户事件
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcBrokerUserEventField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 用户事件类型
        /// </summary>
        public EnumThostUserEventTypeType UserEventType;
        /// <summary>
        /// 用户事件序号
        /// </summary>
        public int EventSequenceNo;
        /// <summary>
        /// 事件发生日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string EventDate;
        /// <summary>
        /// 事件发生时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string EventTime;
        /// <summary>
        /// 用户事件信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1025)]
        public string UserEventInfo;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    }

    /// <summary>
    /// 查询签约银行请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryContractBankField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分中心代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBrchID;
    }

    /// <summary>
    /// 查询签约银行响应
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcContractBankField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分中心代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBrchID;
        /// <summary>
        /// 银行名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
        public string BankName;
    }

    /// <summary>
    /// 投资者组合持仓明细
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInvestorPositionCombineDetailField
    {
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 开仓日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string OpenDate;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 组合编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string ComTradeID;
        /// <summary>
        /// 撮合编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TradeID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 买卖
        /// </summary>
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 持仓量
        /// </summary>
        public int TotalAmt;
        /// <summary>
        /// 投资者保证金
        /// </summary>
        public double Margin;
        /// <summary>
        /// 交易所保证金
        /// </summary>
        public double ExchMargin;
        /// <summary>
        /// 保证金率
        /// </summary>
        public double MarginRateByMoney;
        /// <summary>
        /// 保证金率(按手数)
        /// </summary>
        public double MarginRateByVolume;
        /// <summary>
        /// 单腿编号
        /// </summary>
        public int LegID;
        /// <summary>
        /// 单腿乘数
        /// </summary>
        public int LegMultiple;
        /// <summary>
        /// 组合持仓合约编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string CombInstrumentID;
        /// <summary>
        /// 成交组号
        /// </summary>
        public int TradeGroupID;
    }

    /// <summary>
    /// 预埋单
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcParkedOrderField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 报单引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderRef;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 报单价格条件
        /// </summary>
        public EnumThostOrderPriceTypeType OrderPriceType;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        public EnumThostOffsetFlagType CombOffsetFlag_0;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        public EnumThostOffsetFlagType CombOffsetFlag_1;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        public EnumThostOffsetFlagType CombOffsetFlag_2;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        public EnumThostOffsetFlagType CombOffsetFlag_3;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        public EnumThostOffsetFlagType CombOffsetFlag_4;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType CombHedgeFlag_0;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType CombHedgeFlag_1;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType CombHedgeFlag_2;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType CombHedgeFlag_3;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType CombHedgeFlag_4;
        /// <summary>
        /// 价格
        /// </summary>
        public double LimitPrice;
        /// <summary>
        /// 数量
        /// </summary>
        public int VolumeTotalOriginal;
        /// <summary>
        /// 有效期类型
        /// </summary>
        public EnumThostTimeConditionType TimeCondition;
        /// <summary>
        /// GTD日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string GTDDate;
        /// <summary>
        /// 成交量类型
        /// </summary>
        public EnumThostVolumeConditionType VolumeCondition;
        /// <summary>
        /// 最小成交量
        /// </summary>
        public int MinVolume;
        /// <summary>
        /// 触发条件
        /// </summary>
        public EnumThostContingentConditionType ContingentCondition;
        /// <summary>
        /// 止损价
        /// </summary>
        public double StopPrice;
        /// <summary>
        /// 强平原因
        /// </summary>
        public EnumThostForceCloseReasonType ForceCloseReason;
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
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 用户强评标志
        /// </summary>
        public int UserForceClose;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 预埋报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ParkedOrderID;
        /// <summary>
        /// 用户类型
        /// </summary>
        public EnumThostUserTypeType UserType;
        /// <summary>
        /// 预埋单状态
        /// </summary>
        public EnumThostParkedOrderStatusType Status;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
        /// <summary>
        /// 互换单标志
        /// </summary>
        public int IsSwapOrder;
        /// <summary>
        /// 资金账号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 交易编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    }

    /// <summary>
    /// 输入预埋单操作
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcParkedOrderActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 报单操作引用
        /// </summary>
        public int OrderActionRef;
        /// <summary>
        /// 报单引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderRef;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string OrderSysID;
        /// <summary>
        /// 操作标志
        /// </summary>
        public EnumThostActionFlagType ActionFlag;
        /// <summary>
        /// 价格
        /// </summary>
        public double LimitPrice;
        /// <summary>
        /// 数量变化
        /// </summary>
        public int VolumeChange;
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
        /// 预埋撤单单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ParkedOrderActionID;
        /// <summary>
        /// 用户类型
        /// </summary>
        public EnumThostUserTypeType UserType;
        /// <summary>
        /// 预埋撤单状态
        /// </summary>
        public EnumThostParkedOrderStatusType Status;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    }

    /// <summary>
    /// 查询预埋单
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryParkedOrderField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
    }

    /// <summary>
    /// 查询预埋撤单
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryParkedOrderActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
    }

    /// <summary>
    /// 删除预埋单
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcRemoveParkedOrderField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 预埋报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ParkedOrderID;
    }

    /// <summary>
    /// 删除预埋撤单
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcRemoveParkedOrderActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 预埋撤单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ParkedOrderActionID;
    }

    /// <summary>
    /// 经纪公司可提资金算法表
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInvestorWithdrawAlgorithmField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 可提资金比例
        /// </summary>
        public double UsingRatio;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 货币质押比率
        /// </summary>
        public double FundMortgageRatio;
    }

    /// <summary>
    /// 查询组合持仓明细
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryInvestorPositionCombineDetailField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 组合持仓合约编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string CombInstrumentID;
    }

    /// <summary>
    /// 成交均价
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcMarketDataAveragePriceField
    {
        /// <summary>
        /// 当日均价
        /// </summary>
        public double AveragePrice;
    }

    /// <summary>
    /// 校验投资者密码
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcVerifyInvestorPasswordField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
    }

    /// <summary>
    /// 用户IP
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcUserIPField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// IP地址掩码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPMask;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    }

    /// <summary>
    /// 用户事件通知信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTradingNoticeInfoField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 发送时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string SendTime;
        /// <summary>
        /// 消息正文
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 501)]
        public string FieldContent;
        /// <summary>
        /// 序列系列号
        /// </summary>
        public short SequenceSeries;
        /// <summary>
        /// 序列号
        /// </summary>
        public int SequenceNo;
    }

    /// <summary>
    /// 用户事件通知
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTradingNoticeField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者范围
        /// </summary>
        public EnumThostInvestorRangeType InvestorRange;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 序列系列号
        /// </summary>
        public short SequenceSeries;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 发送时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string SendTime;
        /// <summary>
        /// 序列号
        /// </summary>
        public int SequenceNo;
        /// <summary>
        /// 消息正文
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 501)]
        public string FieldContent;
    }

    /// <summary>
    /// 查询交易事件通知
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryTradingNoticeField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
    }

    /// <summary>
    /// 查询错误报单
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryErrOrderField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
    }

    /// <summary>
    /// 错误报单
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcErrOrderField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 报单引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderRef;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 报单价格条件
        /// </summary>
        public EnumThostOrderPriceTypeType OrderPriceType;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string CombOffsetFlag;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string CombHedgeFlag;
        /// <summary>
        /// 价格
        /// </summary>
        public double LimitPrice;
        /// <summary>
        /// 数量
        /// </summary>
        public int VolumeTotalOriginal;
        /// <summary>
        /// 有效期类型
        /// </summary>
        public EnumThostTimeConditionType TimeCondition;
        /// <summary>
        /// GTD日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string GTDDate;
        /// <summary>
        /// 成交量类型
        /// </summary>
        public EnumThostVolumeConditionType VolumeCondition;
        /// <summary>
        /// 最小成交量
        /// </summary>
        public int MinVolume;
        /// <summary>
        /// 触发条件
        /// </summary>
        public EnumThostContingentConditionType ContingentCondition;
        /// <summary>
        /// 止损价
        /// </summary>
        public double StopPrice;
        /// <summary>
        /// 强平原因
        /// </summary>
        public EnumThostForceCloseReasonType ForceCloseReason;
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
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 用户强评标志
        /// </summary>
        public int UserForceClose;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
        /// <summary>
        /// 互换单标志
        /// </summary>
        public int IsSwapOrder;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// 资金账号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 交易编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    }

    /// <summary>
    /// 查询错误报单操作
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcErrorConditionalOrderField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 报单引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderRef;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 报单价格条件
        /// </summary>
        public EnumThostOrderPriceTypeType OrderPriceType;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 组合开平标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string CombOffsetFlag;
        /// <summary>
        /// 组合投机套保标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string CombHedgeFlag;
        /// <summary>
        /// 价格
        /// </summary>
        public double LimitPrice;
        /// <summary>
        /// 数量
        /// </summary>
        public int VolumeTotalOriginal;
        /// <summary>
        /// 有效期类型
        /// </summary>
        public EnumThostTimeConditionType TimeCondition;
        /// <summary>
        /// GTD日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string GTDDate;
        /// <summary>
        /// 成交量类型
        /// </summary>
        public EnumThostVolumeConditionType VolumeCondition;
        /// <summary>
        /// 最小成交量
        /// </summary>
        public int MinVolume;
        /// <summary>
        /// 触发条件
        /// </summary>
        public EnumThostContingentConditionType ContingentCondition;
        /// <summary>
        /// 止损价
        /// </summary>
        public double StopPrice;
        /// <summary>
        /// 强平原因
        /// </summary>
        public EnumThostForceCloseReasonType ForceCloseReason;
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
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderLocalID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 合约在交易所的代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ExchangeInstID;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 报单提交状态
        /// </summary>
        public EnumThostOrderSubmitStatusType OrderSubmitStatus;
        /// <summary>
        /// 报单提示序号
        /// </summary>
        public int NotifySequence;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string OrderSysID;
        /// <summary>
        /// 报单来源
        /// </summary>
        public EnumThostOrderSourceType OrderSource;
        /// <summary>
        /// 报单状态
        /// </summary>
        public EnumThostOrderStatusType OrderStatus;
        /// <summary>
        /// 报单类型
        /// </summary>
        public EnumThostOrderTypeType OrderType;
        /// <summary>
        /// 今成交数量
        /// </summary>
        public int VolumeTraded;
        /// <summary>
        /// 剩余数量
        /// </summary>
        public int VolumeTotal;
        /// <summary>
        /// 报单日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertDate;
        /// <summary>
        /// 委托时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string InsertTime;
        /// <summary>
        /// 激活时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActiveTime;
        /// <summary>
        /// 挂起时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string SuspendTime;
        /// <summary>
        /// 最后修改时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string UpdateTime;
        /// <summary>
        /// 撤销时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string CancelTime;
        /// <summary>
        /// 最后修改交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string ActiveTraderID;
        /// <summary>
        /// 结算会员编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClearingPartID;
        /// <summary>
        /// 序号
        /// </summary>
        public int SequenceNo;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 用户端产品信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string UserProductInfo;
        /// <summary>
        /// 状态信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string StatusMsg;
        /// <summary>
        /// 用户强评标志
        /// </summary>
        public int UserForceClose;
        /// <summary>
        /// 操作用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ActiveUserID;
        /// <summary>
        /// 经纪公司报单编号
        /// </summary>
        public int BrokerOrderSeq;
        /// <summary>
        /// 相关报单
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string RelativeOrderSysID;
        /// <summary>
        /// 郑商所成交数量
        /// </summary>
        public int ZCETotalTradedVolume;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
        /// <summary>
        /// 互换单标志
        /// </summary>
        public int IsSwapOrder;
        /// <summary>
        /// 营业部编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string BranchID;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// 资金账号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
    }

    /// <summary>
    /// 查询错误报单操作
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryErrOrderActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
    }

    /// <summary>
    /// 错误报单操作
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcErrOrderActionField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 报单操作引用
        /// </summary>
        public int OrderActionRef;
        /// <summary>
        /// 报单引用
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderRef;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string OrderSysID;
        /// <summary>
        /// 操作标志
        /// </summary>
        public EnumThostActionFlagType ActionFlag;
        /// <summary>
        /// 价格
        /// </summary>
        public double LimitPrice;
        /// <summary>
        /// 数量变化
        /// </summary>
        public int VolumeChange;
        /// <summary>
        /// 操作日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionDate;
        /// <summary>
        /// 操作时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ActionTime;
        /// <summary>
        /// 交易所交易员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string TraderID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 本地报单编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string OrderLocalID;
        /// <summary>
        /// 操作本地编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string ActionLocalID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 客户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ClientID;
        /// <summary>
        /// 业务单元
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string BusinessUnit;
        /// <summary>
        /// 报单操作状态
        /// </summary>
        public EnumThostOrderActionStatusType OrderActionStatus;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 状态信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string StatusMsg;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 营业部编号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string BranchID;
        /// <summary>
        /// 投资单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string InvestUnitID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
        /// <summary>
        /// Mac地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MacAddress;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
    }

    /// <summary>
    /// 查询交易所状态
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryExchangeSequenceField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
    }

    /// <summary>
    /// 交易所状态
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcExchangeSequenceField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 序号
        /// </summary>
        public int SequenceNo;
        /// <summary>
        /// 合约交易状态
        /// </summary>
        public EnumThostInstrumentStatusType MarketStatus;
    }

    /// <summary>
    /// 根据价格查询最大报单数量
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQueryMaxOrderVolumeWithPriceField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 开平标志
        /// </summary>
        public EnumThostOffsetFlagType OffsetFlag;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 最大允许报单数量
        /// </summary>
        public int MaxVolume;
        /// <summary>
        /// 报单价格
        /// </summary>
        public double Price;
    }

    /// <summary>
    /// 查询经纪公司交易参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryBrokerTradingParamsField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
    }

    /// <summary>
    /// 经纪公司交易参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcBrokerTradingParamsField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 保证金价格类型
        /// </summary>
        public EnumThostMarginPriceTypeType MarginPriceType;
        /// <summary>
        /// 盈亏算法
        /// </summary>
        public EnumThostAlgorithmType Algorithm;
        /// <summary>
        /// 可用是否包含平仓盈利
        /// </summary>
        public EnumThostIncludeCloseProfitType AvailIncludeCloseProfit;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 期权权利金价格类型
        /// </summary>
        public EnumThostOptionRoyaltyPriceTypeType OptionRoyaltyPriceType;
    }

    /// <summary>
    /// 查询经纪公司交易算法
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryBrokerTradingAlgosField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    }

    /// <summary>
    /// 经纪公司交易算法
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcBrokerTradingAlgosField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 持仓处理算法编号
        /// </summary>
        public EnumThostHandlePositionAlgoIDType HandlePositionAlgoID;
        /// <summary>
        /// 寻找保证金率算法编号
        /// </summary>
        public EnumThostFindMarginRateAlgoIDType FindMarginRateAlgoID;
        /// <summary>
        /// 资金处理算法编号
        /// </summary>
        public EnumThostHandleTradingAccountAlgoIDType HandleTradingAccountAlgoID;
    }

    /// <summary>
    /// 查询经纪公司资金
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQueryBrokerDepositField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
    }

    /// <summary>
    /// 经纪公司资金
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcBrokerDepositField
    {
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 会员代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 上次结算准备金
        /// </summary>
        public double PreBalance;
        /// <summary>
        /// 当前保证金总额
        /// </summary>
        public double CurrMargin;
        /// <summary>
        /// 平仓盈亏
        /// </summary>
        public double CloseProfit;
        /// <summary>
        /// 期货结算准备金
        /// </summary>
        public double Balance;
        /// <summary>
        /// 入金金额
        /// </summary>
        public double Deposit;
        /// <summary>
        /// 出金金额
        /// </summary>
        public double Withdraw;
        /// <summary>
        /// 可提资金
        /// </summary>
        public double Available;
        /// <summary>
        /// 基本准备金
        /// </summary>
        public double Reserve;
        /// <summary>
        /// 冻结的保证金
        /// </summary>
        public double FrozenMargin;
    }

    /// <summary>
    /// 查询保证金监管系统经纪公司密钥
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryCFMMCBrokerKeyField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
    }

    /// <summary>
    /// 保证金监管系统经纪公司密钥
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcCFMMCBrokerKeyField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 经纪公司统一编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 密钥生成日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string CreateDate;
        /// <summary>
        /// 密钥生成时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string CreateTime;
        /// <summary>
        /// 密钥编号
        /// </summary>
        public int KeyID;
        /// <summary>
        /// 动态密钥
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string CurrentKey;
        /// <summary>
        /// 动态密钥类型
        /// </summary>
        public EnumThostCFMMCKeyKindType KeyKind;
    }

    /// <summary>
    /// 保证金监管系统经纪公司资金账户密钥
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcCFMMCTradingAccountKeyField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 经纪公司统一编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 密钥编号
        /// </summary>
        public int KeyID;
        /// <summary>
        /// 动态密钥
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string CurrentKey;
    }

    /// <summary>
    /// 请求查询保证金监管系统经纪公司资金账户密钥
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryCFMMCTradingAccountKeyField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
    }

    /// <summary>
    /// 用户动态令牌参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcBrokerUserOTPParamField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 动态令牌提供商
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string OTPVendorsID;
        /// <summary>
        /// 动态令牌序列号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string SerialNumber;
        /// <summary>
        /// 令牌密钥
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string AuthKey;
        /// <summary>
        /// 漂移值
        /// </summary>
        public int LastDrift;
        /// <summary>
        /// 成功值
        /// </summary>
        public int LastSuccess;
        /// <summary>
        /// 动态令牌类型
        /// </summary>
        public EnumThostOTPTypeType OTPType;
    }

    /// <summary>
    /// 手工同步用户动态令牌
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcManualSyncBrokerUserOTPField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 动态令牌类型
        /// </summary>
        public EnumThostOTPTypeType OTPType;
        /// <summary>
        /// 第一个动态密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string FirstOTP;
        /// <summary>
        /// 第二个动态密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string SecondOTP;
    }

    /// <summary>
    /// 投资者手续费率模板
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcCommRateModelField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 手续费率模板代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string CommModelID;
        /// <summary>
        /// 模板名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string CommModelName;
    };

    /// <summary>
    /// 请求查询投资者手续费率模板
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryCommRateModelField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 手续费率模板代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string CommModelID;
    };

    /// <summary>
    /// 投资者保证金率模板
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcMarginModelField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 保证金率模板代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string MarginModelID;
        /// <summary>
        /// 模板名称
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string MarginModelName;
    };

    /// <summary>
    /// 请求查询投资者保证金率模板
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryMarginModelField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 保证金率模板代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string MarginModelID;
    };

    /// <summary>
    /// 仓单折抵信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcEWarrantOffsetField
    {
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
        /// <summary>
        /// 买卖方向
        /// </summary>
        public EnumThostDirectionType Direction;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
        /// <summary>
        /// 数量
        /// </summary>
        public int Volume;
    };

    /// <summary>
    /// 查询仓单折抵信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryEWarrantOffsetField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 合约代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string InstrumentID;
    };

    /// <summary>
    /// 查询投资者品种/跨品种保证金
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryInvestorProductGroupMarginField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 品种/跨品种标示
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ProductGroupID;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
    };

    /// <summary>
    /// 投资者品种/跨品种保证金
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcInvestorProductGroupMarginField
    {
        /// <summary>
        /// 品种/跨品种标示
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ProductGroupID;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 结算编号
        /// </summary>
        public int SettlementID;
        /// <summary>
        /// 冻结的保证金
        /// </summary>
        public double FrozenMargin;
        /// <summary>
        /// 多头冻结的保证金
        /// </summary>
        public double LongFrozenMargin;
        /// <summary>
        /// 空头冻结的保证金
        /// </summary>
        public double ShortFrozenMargin;
        /// <summary>
        /// 占用的保证金
        /// </summary>
        public double UseMargin;
        /// <summary>
        /// 多头保证金
        /// </summary>
        public double LongUseMargin;
        /// <summary>
        /// 空头保证金
        /// </summary>
        public double ShortUseMargin;
        /// <summary>
        /// 交易所保证金
        /// </summary>
        public double ExchMargin;
        /// <summary>
        /// 交易所多头保证金
        /// </summary>
        public double LongExchMargin;
        /// <summary>
        /// 交易所空头保证金
        /// </summary>
        public double ShortExchMargin;
        /// <summary>
        /// 平仓盈亏
        /// </summary>
        public double CloseProfit;
        /// <summary>
        /// 冻结的手续费
        /// </summary>
        public double FrozenCommission;
        /// <summary>
        /// 手续费
        /// </summary>
        public double Commission;
        /// <summary>
        /// 冻结的资金
        /// </summary>
        public double FrozenCash;
        /// <summary>
        /// 资金差额
        /// </summary>
        public double CashIn;
        /// <summary>
        /// 持仓盈亏
        /// </summary>
        public double PositionProfit;
        /// <summary>
        /// 折抵总金额
        /// </summary>
        public double OffsetAmount;
        /// <summary>
        /// 多头折抵总金额
        /// </summary>
        public double LongOffsetAmount;
        /// <summary>
        /// 空头折抵总金额
        /// </summary>
        public double ShortOffsetAmount;
        /// <summary>
        /// 交易所折抵总金额
        /// </summary>
        public double ExchOffsetAmount;
        /// <summary>
        /// 交易所多头折抵总金额
        /// </summary>
        public double LongExchOffsetAmount;
        /// <summary>
        /// 交易所空头折抵总金额
        /// </summary>
        public double ShortExchOffsetAmount;
        /// <summary>
        /// 投机套保标志
        /// </summary>
        public EnumThostHedgeFlagType HedgeFlag;
    };

    /// <summary>
    /// 查询监控中心用户令牌
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQueryCFMMCTradingAccountTokenField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
    };

    /// <summary>
    /// 监控中心用户令牌
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcCFMMCTradingAccountTokenField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 经纪公司统一编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ParticipantID;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 密钥编号
        /// </summary>
        public int KeyID;
        /// <summary>
        /// 动态令牌
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string Token;
    };

    ///查询产品组
    public struct CThostFtdcQryProductGroupField
    {
        /// <summary>
        /// 产品代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ProductID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
    };

    ///投资者品种/跨品种保证金产品组
    public struct CThostFtdcProductGroupField
    {
        /// <summary>
        /// 产品代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ProductID;
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 产品组代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string ProductGroupID;
    };

    ///交易所公告
    public struct CThostFtdcBulletinField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 交易日
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 公告编号
        /// </summary>
        public int BulletinID;
        /// <summary>
        /// 序列号
        /// </summary>
        public int SequenceNo;
        /// <summary>
        /// 公告类型
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string NewsType;
        /// <summary>
        /// 紧急程度
        /// </summary>
        public char NewsUrgency;
        /// <summary>
        /// 发送时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string SendTime;
        /// <summary>
        /// 消息摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string Abstract;
        /// <summary>
        /// 消息来源
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string ComeFrom;
        /// <summary>
        /// 消息正文
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 501)]
        public string Content;
        /// <summary>
        /// WEB地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 201)]
        public string URLLink;
        /// <summary>
        /// 市场代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string MarketID;
    };

    ///查询交易所公告
    public struct CThostFtdcQryBulletinField
    {
        /// <summary>
        /// 交易所代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string ExchangeID;
        /// <summary>
        /// 公告编号
        /// </summary>
        public int BulletinID;
        /// <summary>
        /// 序列号
        /// </summary>
        public int SequenceNo;
        /// <summary>
        /// 公告类型
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string NewsType;
        /// <summary>
        /// 紧急程度
        /// </summary>
        public char NewsUrgency;
    };

    /// <summary>
    /// 转帐开户请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcReqOpenAccountField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 性别
        /// </summary>
        public EnumThostGenderType Gender;
        /// <summary>
        /// 国家代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string CountryCode;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
        public string Address;
        /// <summary>
        /// 邮编
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string ZipCode;
        /// <summary>
        /// 电话号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Telephone;
        /// <summary>
        /// 手机
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MobilePhone;
        /// <summary>
        /// 传真
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Fax;
        /// <summary>
        /// 电子邮件
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string EMail;
        /// <summary>
        /// 资金账户状态
        /// </summary>
        public EnumThostMoneyAccountStatusType MoneyAccountStatus;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 验证客户证件号码标志
        /// </summary>
        public EnumThostYesNoIndicatorType VerifyCertNoFlag;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 汇钞标志
        /// </summary>
        public EnumThostCashExchangeCodeType CashExchangeCode;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货单位帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankSecuAccType;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 期货单位帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankSecuAcc;
        /// <summary>
        /// 银行密码标志
        /// </summary>
        public EnumThostPwdFlagType BankPwdFlag;
        /// <summary>
        /// 期货资金密码核对标志
        /// </summary>
        public EnumThostPwdFlagType SecuPwdFlag;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 长客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string LongCustomerName;
    }

    /// <summary>
    /// 转帐销户请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcReqCancelAccountField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 性别
        /// </summary>
        public EnumThostGenderType Gender;
        /// <summary>
        /// 国家代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string CountryCode;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
        public string Address;
        /// <summary>
        /// 邮编
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string ZipCode;
        /// <summary>
        /// 电话号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Telephone;
        /// <summary>
        /// 手机
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MobilePhone;
        /// <summary>
        /// 传真
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Fax;
        /// <summary>
        /// 电子邮件
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string EMail;
        /// <summary>
        /// 资金账户状态
        /// </summary>
        public EnumThostMoneyAccountStatusType MoneyAccountStatus;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 验证客户证件号码标志
        /// </summary>
        public EnumThostYesNoIndicatorType VerifyCertNoFlag;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 汇钞标志
        /// </summary>
        public EnumThostCashExchangeCodeType CashExchangeCode;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货单位帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankSecuAccType;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 期货单位帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankSecuAcc;
        /// <summary>
        /// 银行密码标志
        /// </summary>
        public EnumThostPwdFlagType BankPwdFlag;
        /// <summary>
        /// 期货资金密码核对标志
        /// </summary>
        public EnumThostPwdFlagType SecuPwdFlag;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 长客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string LongCustomerName;
    }

    /// <summary>
    /// 变更银行账户请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcReqChangeAccountField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 性别
        /// </summary>
        public EnumThostGenderType Gender;
        /// <summary>
        /// 国家代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string CountryCode;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
        public string Address;
        /// <summary>
        /// 邮编
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string ZipCode;
        /// <summary>
        /// 电话号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Telephone;
        /// <summary>
        /// 手机
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MobilePhone;
        /// <summary>
        /// 传真
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Fax;
        /// <summary>
        /// 电子邮件
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string EMail;
        /// <summary>
        /// 资金账户状态
        /// </summary>
        public EnumThostMoneyAccountStatusType MoneyAccountStatus;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 新银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string NewBankAccount;
        /// <summary>
        /// 新银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string NewBankPassWord;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 验证客户证件号码标志
        /// </summary>
        public EnumThostYesNoIndicatorType VerifyCertNoFlag;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 银行密码标志
        /// </summary>
        public EnumThostPwdFlagType BankPwdFlag;
        /// <summary>
        /// 期货资金密码核对标志
        /// </summary>
        public EnumThostPwdFlagType SecuPwdFlag;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 长客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string LongCustomerName;
    }

    /// <summary>
    /// 转账请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcReqTransferField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 期货公司流水号
        /// </summary>
        public int FutureSerial;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 验证客户证件号码标志
        /// </summary>
        public EnumThostYesNoIndicatorType VerifyCertNoFlag;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 转帐金额
        /// </summary>
        public double TradeAmount;
        /// <summary>
        /// 期货可取金额
        /// </summary>
        public double FutureFetchAmount;
        /// <summary>
        /// 费用支付标志
        /// </summary>
        public EnumThostFeePayFlagType FeePayFlag;
        /// <summary>
        /// 应收客户费用
        /// </summary>
        public double CustFee;
        /// <summary>
        /// 应收期货公司费用
        /// </summary>
        public double BrokerFee;
        /// <summary>
        /// 发送方给接收方的消息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string Message;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货单位帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankSecuAccType;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 期货单位帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankSecuAcc;
        /// <summary>
        /// 银行密码标志
        /// </summary>
        public EnumThostPwdFlagType BankPwdFlag;
        /// <summary>
        /// 期货资金密码核对标志
        /// </summary>
        public EnumThostPwdFlagType SecuPwdFlag;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 转账交易状态
        /// </summary>
        public EnumThostTransferStatusType TransferStatus;
        /// <summary>
        /// 长客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string LongCustomerName;
    }

    /// <summary>
    /// 银行发起银行资金转期货响应
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcRspTransferField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 期货公司流水号
        /// </summary>
        public int FutureSerial;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 验证客户证件号码标志
        /// </summary>
        public EnumThostYesNoIndicatorType VerifyCertNoFlag;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 转帐金额
        /// </summary>
        public double TradeAmount;
        /// <summary>
        /// 期货可取金额
        /// </summary>
        public double FutureFetchAmount;
        /// <summary>
        /// 费用支付标志
        /// </summary>
        public EnumThostFeePayFlagType FeePayFlag;
        /// <summary>
        /// 应收客户费用
        /// </summary>
        public double CustFee;
        /// <summary>
        /// 应收期货公司费用
        /// </summary>
        public double BrokerFee;
        /// <summary>
        /// 发送方给接收方的消息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string Message;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货单位帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankSecuAccType;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 期货单位帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankSecuAcc;
        /// <summary>
        /// 银行密码标志
        /// </summary>
        public EnumThostPwdFlagType BankPwdFlag;
        /// <summary>
        /// 期货资金密码核对标志
        /// </summary>
        public EnumThostPwdFlagType SecuPwdFlag;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 转账交易状态
        /// </summary>
        public EnumThostTransferStatusType TransferStatus;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
        /// <summary>
        /// 长客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string LongCustomerName;
    }

    /// <summary>
    /// 冲正请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcReqRepealField
    {
        /// <summary>
        /// 冲正时间间隔
        /// </summary>
        public int RepealTimeInterval;
        /// <summary>
        /// 已经冲正次数
        /// </summary>
        public int RepealedTimes;
        /// <summary>
        /// 银行冲正标志
        /// </summary>
        public EnumThostBankRepealFlagType BankRepealFlag;
        /// <summary>
        /// 期商冲正标志
        /// </summary>
        public EnumThostBrokerRepealFlagType BrokerRepealFlag;
        /// <summary>
        /// 被冲正平台流水号
        /// </summary>
        public int PlateRepealSerial;
        /// <summary>
        /// 被冲正银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankRepealSerial;
        /// <summary>
        /// 被冲正期货流水号
        /// </summary>
        public int FutureRepealSerial;
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 期货公司流水号
        /// </summary>
        public int FutureSerial;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 验证客户证件号码标志
        /// </summary>
        public EnumThostYesNoIndicatorType VerifyCertNoFlag;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 转帐金额
        /// </summary>
        public double TradeAmount;
        /// <summary>
        /// 期货可取金额
        /// </summary>
        public double FutureFetchAmount;
        /// <summary>
        /// 费用支付标志
        /// </summary>
        public EnumThostFeePayFlagType FeePayFlag;
        /// <summary>
        /// 应收客户费用
        /// </summary>
        public double CustFee;
        /// <summary>
        /// 应收期货公司费用
        /// </summary>
        public double BrokerFee;
        /// <summary>
        /// 发送方给接收方的消息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string Message;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货单位帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankSecuAccType;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 期货单位帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankSecuAcc;
        /// <summary>
        /// 银行密码标志
        /// </summary>
        public EnumThostPwdFlagType BankPwdFlag;
        /// <summary>
        /// 期货资金密码核对标志
        /// </summary>
        public EnumThostPwdFlagType SecuPwdFlag;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 转账交易状态
        /// </summary>
        public EnumThostTransferStatusType TransferStatus;
        /// <summary>
        /// 长客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string LongCustomerName;
    }

    /// <summary>
    /// 冲正响应
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcRspRepealField
    {
        /// <summary>
        /// 冲正时间间隔
        /// </summary>
        public int RepealTimeInterval;
        /// <summary>
        /// 已经冲正次数
        /// </summary>
        public int RepealedTimes;
        /// <summary>
        /// 银行冲正标志
        /// </summary>
        public EnumThostBankRepealFlagType BankRepealFlag;
        /// <summary>
        /// 期商冲正标志
        /// </summary>
        public EnumThostBrokerRepealFlagType BrokerRepealFlag;
        /// <summary>
        /// 被冲正平台流水号
        /// </summary>
        public int PlateRepealSerial;
        /// <summary>
        /// 被冲正银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankRepealSerial;
        /// <summary>
        /// 被冲正期货流水号
        /// </summary>
        public int FutureRepealSerial;
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 期货公司流水号
        /// </summary>
        public int FutureSerial;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 验证客户证件号码标志
        /// </summary>
        public EnumThostYesNoIndicatorType VerifyCertNoFlag;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 转帐金额
        /// </summary>
        public double TradeAmount;
        /// <summary>
        /// 期货可取金额
        /// </summary>
        public double FutureFetchAmount;
        /// <summary>
        /// 费用支付标志
        /// </summary>
        public EnumThostFeePayFlagType FeePayFlag;
        /// <summary>
        /// 应收客户费用
        /// </summary>
        public double CustFee;
        /// <summary>
        /// 应收期货公司费用
        /// </summary>
        public double BrokerFee;
        /// <summary>
        /// 发送方给接收方的消息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string Message;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货单位帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankSecuAccType;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 期货单位帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankSecuAcc;
        /// <summary>
        /// 银行密码标志
        /// </summary>
        public EnumThostPwdFlagType BankPwdFlag;
        /// <summary>
        /// 期货资金密码核对标志
        /// </summary>
        public EnumThostPwdFlagType SecuPwdFlag;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 转账交易状态
        /// </summary>
        public EnumThostTransferStatusType TransferStatus;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
        /// <summary>
        /// 长客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string LongCustomerName;
    }

    /// <summary>
    /// 查询账户信息请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcReqQueryAccountField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 期货公司流水号
        /// </summary>
        public int FutureSerial;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 验证客户证件号码标志
        /// </summary>
        public EnumThostYesNoIndicatorType VerifyCertNoFlag;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货单位帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankSecuAccType;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 期货单位帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankSecuAcc;
        /// <summary>
        /// 银行密码标志
        /// </summary>
        public EnumThostPwdFlagType BankPwdFlag;
        /// <summary>
        /// 期货资金密码核对标志
        /// </summary>
        public EnumThostPwdFlagType SecuPwdFlag;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 长客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string LongCustomerName;
    }

    /// <summary>
    /// 查询账户信息响应
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcRspQueryAccountField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 期货公司流水号
        /// </summary>
        public int FutureSerial;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 验证客户证件号码标志
        /// </summary>
        public EnumThostYesNoIndicatorType VerifyCertNoFlag;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货单位帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankSecuAccType;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 期货单位帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankSecuAcc;
        /// <summary>
        /// 银行密码标志
        /// </summary>
        public EnumThostPwdFlagType BankPwdFlag;
        /// <summary>
        /// 期货资金密码核对标志
        /// </summary>
        public EnumThostPwdFlagType SecuPwdFlag;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 银行可用金额
        /// </summary>
        public double BankUseAmount;
        /// <summary>
        /// 银行可取金额
        /// </summary>
        public double BankFetchAmount;
        /// <summary>
        /// 长客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string LongCustomerName;
    }

    /// <summary>
    /// 期商签到签退
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcFutureSignIOField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
    }

    /// <summary>
    /// 期商签到响应
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcRspFutureSignInField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
        /// <summary>
        /// PIN密钥
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string PinKey;
        /// <summary>
        /// MAC密钥
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string MacKey;
    }

    /// <summary>
    /// 期商签退请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcReqFutureSignOutField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
    }

    /// <summary>
    /// 期商签退响应
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcRspFutureSignOutField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
    }

    /// <summary>
    /// 查询指定流水号的交易结果请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcReqQueryTradeResultBySerialField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 流水号
        /// </summary>
        public int Reference;
        /// <summary>
        /// 本流水号发布者的机构类型
        /// </summary>
        public EnumThostInstitutionTypeType RefrenceIssureType;
        /// <summary>
        /// 本流水号发布者机构编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string RefrenceIssure;
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 转帐金额
        /// </summary>
        public double TradeAmount;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 长客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string LongCustomerName;
    }

    /// <summary>
    /// 查询指定流水号的交易结果响应
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcRspQueryTradeResultBySerialField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
        /// <summary>
        /// 流水号
        /// </summary>
        public int Reference;
        /// <summary>
        /// 本流水号发布者的机构类型
        /// </summary>
        public EnumThostInstitutionTypeType RefrenceIssureType;
        /// <summary>
        /// 本流水号发布者机构编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string RefrenceIssure;
        /// <summary>
        /// 原始返回代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string OriginReturnCode;
        /// <summary>
        /// 原始返回码描述
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string OriginDescrInfoForReturnCode;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 转帐金额
        /// </summary>
        public double TradeAmount;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
    }

    /// <summary>
    /// 日终文件就绪请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcReqDayEndFileReadyField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 文件业务功能
        /// </summary>
        public EnumThostFileBusinessCodeType FileBusinessCode;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
    }

    /// <summary>
    /// 返回结果
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcReturnResultField
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string ReturnCode;
        /// <summary>
        /// 返回码描述
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string DescrInfoForReturnCode;
    }

    /// <summary>
    /// 验证期货资金密码
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcVerifyFuturePasswordField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
    }

    /// <summary>
    /// 验证客户信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcVerifyCustInfoField
    {
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 长客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string LongCustomerName;
    }

    /// <summary>
    /// 验证期货资金密码和客户信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcVerifyFuturePasswordAndCustInfoField
    {
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 长客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string LongCustomerName;
    }

    /// <summary>
    /// 验证期货资金密码和客户信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcDepositResultInformField
    {
        /// <summary>
        /// 出入金流水号，该流水号为银期报盘返回的流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string DepositSeqNo;
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 入金金额
        /// </summary>
        public double Deposit;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 返回代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string ReturnCode;
        /// <summary>
        /// 返回码描述
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string DescrInfoForReturnCode;
    }

    /// <summary>
    /// 交易核心向银期报盘发出密钥同步请求
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcReqSyncKeyField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 交易核心给银期报盘的消息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string Message;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
    }

    /// <summary>
    /// 交易核心向银期报盘发出密钥同步响应
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcRspSyncKeyField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 交易核心给银期报盘的消息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string Message;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
    }

    /// <summary>
    /// 查询账户信息通知
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcNotifyQueryAccountField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 期货公司流水号
        /// </summary>
        public int FutureSerial;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 验证客户证件号码标志
        /// </summary>
        public EnumThostYesNoIndicatorType VerifyCertNoFlag;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货单位帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankSecuAccType;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 期货单位帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankSecuAcc;
        /// <summary>
        /// 银行密码标志
        /// </summary>
        public EnumThostPwdFlagType BankPwdFlag;
        /// <summary>
        /// 期货资金密码核对标志
        /// </summary>
        public EnumThostPwdFlagType SecuPwdFlag;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 银行可用金额
        /// </summary>
        public double BankUseAmount;
        /// <summary>
        /// 银行可取金额
        /// </summary>
        public double BankFetchAmount;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
        /// <summary>
        /// 长客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string LongCustomerName;
    }

    /// <summary>
    /// 银期转账交易流水表
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTransferSerialField
    {
        /// <summary>
        /// 平台流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 交易发起方日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 交易代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 期货公司编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 期货公司帐号类型
        /// </summary>
        public EnumThostFutureAccTypeType FutureAccType;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 投资者代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string InvestorID;
        /// <summary>
        /// 期货公司流水号
        /// </summary>
        public int FutureSerial;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 交易金额
        /// </summary>
        public double TradeAmount;
        /// <summary>
        /// 应收客户费用
        /// </summary>
        public double CustFee;
        /// <summary>
        /// 应收期货公司费用
        /// </summary>
        public double BrokerFee;
        /// <summary>
        /// 有效标志
        /// </summary>
        public EnumThostAvailabilityFlagType AvailabilityFlag;
        /// <summary>
        /// 操作员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperatorCode;
        /// <summary>
        /// 新银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankNewAccount;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
    }

    /// <summary>
    /// 请求查询转帐流水
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryTransferSerialField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
    }

    /// <summary>
    /// 期商签到通知
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcNotifyFutureSignInField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
        /// <summary>
        /// PIN密钥
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string PinKey;
        /// <summary>
        /// MAC密钥
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string MacKey;
    }

    /// <summary>
    /// 期商签退通知
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcNotifyFutureSignOutField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
    }

    /// <summary>
    /// 交易核心向银期报盘发出密钥同步处理结果的通知
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcNotifySyncKeyField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 交易核心给银期报盘的消息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 129)]
        public string Message;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
    }

    ///请求查询银期签约关系
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryAccountregisterField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
    };

    /// <summary>
    /// 客户开销户信息表
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcAccountregisterField
    {
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDay;
        /// <summary>
        /// 银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBrchID;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 期货公司编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期货公司分支机构编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string CustomerName;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 开销户类别
        /// </summary>
        public EnumThostOpenOrDestroyType OpenOrDestroy;
        /// <summary>
        /// 签约日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string RegDate;
        /// <summary>
        /// 解约日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string OutDate;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType;
        /// <summary>
        /// 长客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string LongCustomerName;
    }

    /// <summary>
    /// 银期开户信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcOpenAccountField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 性别
        /// </summary>
        public EnumThostGenderType Gender;
        /// <summary>
        /// 国家代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string CountryCode;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
        public string Address;
        /// <summary>
        /// 邮编
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string ZipCode;
        /// <summary>
        /// 电话号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Telephone;
        /// <summary>
        /// 手机
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MobilePhone;
        /// <summary>
        /// 传真
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Fax;
        /// <summary>
        /// 电子邮件
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string EMail;
        /// <summary>
        /// 资金账户状态
        /// </summary>
        public EnumThostMoneyAccountStatusType MoneyAccountStatus;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 验证客户证件号码标志
        /// </summary>
        public EnumThostYesNoIndicatorType VerifyCertNoFlag;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 汇钞标志
        /// </summary>
        public EnumThostCashExchangeCodeType CashExchangeCode;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货单位帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankSecuAccType;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 期货单位帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankSecuAcc;
        /// <summary>
        /// 银行密码标志
        /// </summary>
        public EnumThostPwdFlagType BankPwdFlag;
        /// <summary>
        /// 期货资金密码核对标志
        /// </summary>
        public EnumThostPwdFlagType SecuPwdFlag;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
        /// <summary>
        /// 长客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string LongCustomerName;
    };

    /// <summary>
    /// 银期销户信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcCancelAccountField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 性别
        /// </summary>
        public EnumThostGenderType Gender;
        /// <summary>
        /// 国家代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string CountryCode;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
        public string Address;
        /// <summary>
        /// 邮编
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string ZipCode;
        /// <summary>
        /// 电话号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Telephone;
        /// <summary>
        /// 手机
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MobilePhone;
        /// <summary>
        /// 传真
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Fax;
        /// <summary>
        /// 电子邮件
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string EMail;
        /// <summary>
        /// 资金账户状态
        /// </summary>
        public EnumThostMoneyAccountStatusType MoneyAccountStatus;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 验证客户证件号码标志
        /// </summary>
        public EnumThostYesNoIndicatorType VerifyCertNoFlag;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 汇钞标志
        /// </summary>
        public EnumThostCashExchangeCodeType CashExchangeCode;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType;
        /// <summary>
        /// 渠道标志
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
        public string DeviceID;
        /// <summary>
        /// 期货单位帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankSecuAccType;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 期货单位帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankSecuAcc;
        /// <summary>
        /// 银行密码标志
        /// </summary>
        public EnumThostPwdFlagType BankPwdFlag;
        /// <summary>
        /// 期货资金密码核对标志
        /// </summary>
        public EnumThostPwdFlagType SecuPwdFlag;
        /// <summary>
        /// 交易柜员
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        public string OperNo;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 用户标识
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorID;
        /// <summary>
        /// 错误信息
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
        public string ErrorMsg;
        /// <summary>
        /// 长客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string LongCustomerName;
    };

    /// <summary>
    /// 银期变更银行账号信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcChangeAccountField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 性别
        /// </summary>
        public EnumThostGenderType Gender;
        /// <summary>
        /// 国家代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string CountryCode;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
        public string Address;
        /// <summary>
        /// 邮编
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string ZipCode;
        /// <summary>
        /// 电话号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Telephone;
        /// <summary>
        /// 手机
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MobilePhone;
        /// <summary>
        /// 传真
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Fax;
        /// <summary>
        /// 电子邮件
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string EMail;
        /// <summary>
        /// 资金账户状态
        /// </summary>
        public EnumThostMoneyAccountStatusType MoneyAccountStatus;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 新银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string NewBankAccount;
        /// <summary>
        /// 新银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string NewBankPassWord;
        /// <summary>
		/// 投资者帐号
		/// </summary>
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 验证客户证件号码标志
        /// </summary>
        public EnumThostYesNoIndicatorType VerifyCertNoFlag;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 银行密码标志
        /// </summary>
        public EnumThostPwdFlagType BankPwdFlag;
        /// <summary>
        /// 期货资金密码核对标志
        /// </summary>
        public EnumThostPwdFlagType SecuPwdFlag;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
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
    /// 二级代理操作员银期权限
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcSecAgentACIDMapField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 资金账户
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 币种
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 境外中介机构资金帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string BrokerSecAgentID;
        /// <summary>
        /// 长客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string LongCustomerName;
    };

    /// <summary>
    /// 二级代理操作员银期权限查询
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQrySecAgentACIDMapField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 资金账户
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 币种
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
    };

    /// <summary>
    /// 灾备中心交易权限
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcUserRightsAssignField
    {
        /// <summary>
        /// 应用单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 交易中心代码
        /// </summary>
        public int DRIdentityID;
    };

    /// <summary>
    /// 经济公司是否有在本标示的交易权限
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcBrokerUserRightAssignField
    {
        /// <summary>
        /// 应用单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 交易中心代码
        /// </summary>
        public int DRIdentityID;
        /// <summary>
        /// 能否交易
        /// </summary>
        public int Tradeable;
    };

    /// <summary>
    /// 灾备交易转换报文
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcDRTransferField
    {
        /// <summary>
        /// 原交易中心代码
        /// </summary>
        public int OrigDRIdentityID;
        /// <summary>
        /// 目标交易中心代码
        /// </summary>
        public int DestDRIdentityID;
        /// <summary>
        /// 原应用单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string OrigBrokerID;
        /// <summary>
        /// 目标易用单元代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string DestBrokerID;
    };

    /// <summary>
    /// Fens用户信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcFensUserInfoField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// 登录模式
        /// </summary>
        public EnumThostLoginModeType LoginMode;
    };

    /// <summary>
    /// 当前银期所属交易中心
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcCurrTransferIdentityField
    {
        /// <summary>
        /// 交易中心代码
        /// </summary>
        public int DRIdentityID;
    };

    /// <summary>
    /// 禁止登录用户
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcLoginForbiddenUserField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 用户代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string UserID;
        /// <summary>
        /// IP地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string IPAddress;
    };

    /// <summary>
    /// 查询禁止登录用户
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcQryLoginForbiddenUserField
    {
        /// <summary>
        /// 经纪公司代码
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
    /// UDP组播组信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcMulticastGroupInfoField
    {
        /// <summary>
        /// 组播组
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string GroupIP;
        /// <summary>
        /// 组播组IP端口
        /// </summary>
        public int GroupPort;
        /// <summary>
        /// 源地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string SourceIP;
    };

    /// <summary>
    /// 资金账户基本准备金
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CThostFtdcTradingAccountReserveField
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 基本准备金
        /// </summary>
        public double Reserve;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
    };

    ///银期预约开户确认请求
    public struct CThostFtdcReserveOpenAccountConfirmField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        ///期商代码
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        public int BankSerial;
        /// <summary>
        /// 交易系统日期 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 性别
        /// </summary>
        public EnumThostGenderType Gender;
        /// <summary>
        /// 国家代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string CountryCode;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
        public string Address;
        /// <summary>
        /// 邮编
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string ZipCode;
        /// <summary>
        /// 电话号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Telephone;
        /// <summary>
        /// 手机
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MobilePhone;
        /// <summary>
        /// 传真
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Fax;
        /// <summary>
        /// 电子邮件
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string EMail;
        /// <summary>
        /// 资金账户状态
        /// </summary>
        public EnumThostMoneyAccountStatusType MoneyAccountStatus;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 验证客户证件号码标志
        /// </summary>
        public EnumThostYesNoIndicatorType VerifyCertNoFlag;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 投资者帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 13)]
        public string AccountID;
        /// <summary>
        /// 期货密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Password;
        /// <summary>
        /// 预约开户银行流水号
        /// </summary>
        public int BankReserveOpenSeq;
        /// <summary>
        /// 预约开户日期 
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string BookDate;
        /// <summary>
        /// 预约开户验证密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BookPsw;
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

    ///银期预约开户
    public struct CThostFtdcReserveOpenAccountField
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string TradeCode;
        /// <summary>
        /// 银行代码，根据查询银行得到，必填
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string BankID;
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BankBranchID;
        /// <summary>
        /// 期商代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string BrokerID;
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
        public string BrokerBranchID;
        /// <summary>
        /// 交易日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeDate;
        /// <summary>
        /// 交易时间
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradeTime;
        /// <summary>
        /// 银行流水号
        /// </summary>
        public int BankSerial;
        /// <summary>
        /// 交易系统日期
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
        public string TradingDay;
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial;
        /// <summary>
        /// 最后分片标志
        /// </summary>
        public EnumThostLastFragmentType LastFragment;
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID;
        /// <summary>
        /// 客户姓名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 161)]
        public string CustomerName;
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType;
        /// <summary>
        /// 证件号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string IdentifiedCardNo;
        /// <summary>
        /// 性别
        /// </summary>
        public EnumThostGenderType Gender;
        /// <summary>
        /// 国家代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string CountryCode;
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType;
        /// <summary>
        /// 地址
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 101)]
        public string Address;
        /// <summary>
        /// 邮编
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string ZipCode;
        /// <summary>
        /// 电话号码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Telephone;
        /// <summary>
        /// 手机
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string MobilePhone;
        /// <summary>
        /// 传真
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string Fax;
        /// <summary>
        /// 电子邮件
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string EMail;
        /// <summary>
        /// 资金账户状态
        /// </summary>
        public EnumThostMoneyAccountStatusType MoneyAccountStatus;
        /// <summary>
        /// 银行帐号
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankAccount;
        /// <summary>
        /// 银行密码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string BankPassWord;
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID;
        /// <summary>
        /// 验证客户证件号码标志
        /// </summary>
        public EnumThostYesNoIndicatorType VerifyCertNoFlag;
        /// <summary>
        /// 币种代码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string CurrencyID;
        /// <summary>
        /// 摘要
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string Digest;
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType;
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public string BrokerIDByBank;
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID;
        /// <summary>
        /// 预约开户状态
        /// </summary>
        public EnumThostFtdcReserveOpenAccStasType ReserveOpenAccStas;
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
    /// TFtdcExchangePropertyType是一个交易所属性类型
    /// </summary>
    public enum EnumThostExchangePropertyType : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = (byte)'0',

        /// <summary>
        /// 根据成交生成报单
        /// </summary>
        GenOrderByTrade = (byte)'1'
    }

    /// <summary>
    /// TFtdcIdCardTypeType是一个证件类型类型
    /// </summary>
    public enum EnumThostIdCardTypeType : byte
    {
        /// <summary>
        /// 组织机构代码
        /// </summary>
        EID = (byte)'0',

        /// <summary>
        /// 中国公民身份证
        /// </summary>
        IDCard = (byte)'1',

        /// <summary>
        /// 军官证
        /// </summary>
        OfficerIDCard = (byte)'2',

        /// <summary>
        /// 警官证
        /// </summary>
        PoliceIDCard = (byte)'3',

        /// <summary>
        /// 士兵证
        /// </summary>
        SoldierIDCard = (byte)'4',

        /// <summary>
        /// 户口簿
        /// </summary>
        HouseholdRegister = (byte)'5',

        /// <summary>
        /// 护照
        /// </summary>
        Passport = (byte)'6',

        /// <summary>
        /// 台胞证
        /// </summary>
        TaiwanCompatriotIDCard = (byte)'7',

        /// <summary>
        /// 回乡证
        /// </summary>
        HomeComingCard = (byte)'8',

        /// <summary>
        /// 营业执照号
        /// </summary>
        LicenseNo = (byte)'9',

        /// <summary>
		/// 税务登记号/当地纳税ID
		/// </summary>
		TaxNo = (byte)'A',

        /// <summary>
        /// 港澳居民来往内地通行证
        /// </summary>
        HMMainlandTravelPermit = (byte)'B',

        /// <summary>
		/// 台湾居民来往大陆通行证
		/// </summary>
		TwMainlandTravelPermit = (byte)'C',

        /// <summary>
		/// 驾照
		/// </summary>
		DrivingLicense = (byte)'D',

        /// <summary>
		/// 当地社保
		/// </summary>
		SocialID = (byte)'F',

        /// <summary>
		/// 当地身份证
		/// </summary>
		LocalID = (byte)'G',

        /// <summary>
		/// 商业登记证
		/// </summary>
		BusinessRegistration = (byte)'H',

        /// <summary>
		/// 港澳永久性居民身份证
		/// </summary>
		HKMCIDCard = (byte)'I',

        /// <summary>
        /// 其他证件
        /// </summary>
        OtherCard = (byte)'x'
    }

    /// <summary>
    /// TFtdcInvestorRangeType是一个投资者范围类型
    /// </summary>
    public enum EnumThostInvestorRangeType : byte
    {
        /// <summary>
        /// 所有
        /// </summary>
        All = (byte)'1',

        /// <summary>
        /// 投资者组
        /// </summary>
        Group = (byte)'2',

        /// <summary>
        /// 单一投资者
        /// </summary>
        Single = (byte)'3'
    }

    /// <summary>
    /// TFtdcDepartmentRangeType是一个投资者范围类型
    /// </summary>
    public enum EnumThostDepartmentRangeType : byte
    {
        /// <summary>
        /// 所有
        /// </summary>
        All = (byte)'1',

        /// <summary>
        /// 组织架构
        /// </summary>
        Group = (byte)'2',

        /// <summary>
        /// 单一投资者
        /// </summary>
        Single = (byte)'3'
    }

    /// <summary>
    /// TFtdcDataSyncStatusType是一个数据同步状态类型
    /// </summary>
    public enum EnumThostDataSyncStatusType : byte
    {
        /// <summary>
        /// 未同步
        /// </summary>
        Asynchronous = (byte)'1',

        /// <summary>
        /// 同步中
        /// </summary>
        Synchronizing = (byte)'2',

        /// <summary>
        /// 已同步
        /// </summary>
        Synchronized = (byte)'3'
    }

    /// <summary>
    /// TFtdcBrokerDataSyncStatusType是一个经纪公司数据同步状态类型
    /// </summary>
    public enum EnumThostBrokerDataSyncStatusType : byte
    {
        /// <summary>
        /// 已同步
        /// </summary>
        Synchronized = (byte)'1',

        /// <summary>
        /// 同步中
        /// </summary>
        Synchronizing = (byte)'2'
    }

    /// <summary>
    /// TFtdcExchangeConnectStatusType是一个交易所连接状态类型
    /// </summary>
    public enum EnumThostExchangeConnectStatusType : byte
    {
        /// <summary>
        /// 没有任何连接
        /// </summary>
        NoConnection = (byte)'1',

        /// <summary>
        /// 已经发出合约查询请求
        /// </summary>
        QryInstrumentSent = (byte)'2',

        /// <summary>
        /// 已经获取信息
        /// </summary>
        GotInformation = (byte)'9'
    }

    /// <summary>
    /// TFtdcTraderConnectStatusType是一个交易所交易员连接状态类型
    /// </summary>
    public enum EnumThostTraderConnectStatusType : byte
    {
        /// <summary>
        /// 没有任何连接
        /// </summary>
        NotConnected = (byte)'1',

        /// <summary>
        /// 已经连接
        /// </summary>
        Connected = (byte)'2',

        /// <summary>
        /// 已经发出合约查询请求
        /// </summary>
        QryInstrumentSent = (byte)'3',

        /// <summary>
        /// 订阅私有流
        /// </summary>
        SubPrivateFlow = (byte)'4'
    }

    /// <summary>
    /// TFtdcFunctionCodeType是一个功能代码类型
    /// </summary>
    public enum EnumThostFunctionCodeType : byte
    {
        /// <summary>
        /// 数据异步化
        /// </summary>
        DataAsync = (byte)'1',

        /// <summary>
        /// 强制用户登出
        /// </summary>
        ForceUserLogout = (byte)'2',

        /// <summary>
        /// 变更管理用户口令
        /// </summary>
        UserPasswordUpdate = (byte)'3',

        /// <summary>
        /// 变更经纪公司口令
        /// </summary>
        BrokerPasswordUpdate = (byte)'4',

        /// <summary>
        /// 变更投资者口令
        /// </summary>
        InvestorPasswordUpdate = (byte)'5',

        /// <summary>
        /// 报单插入
        /// </summary>
        OrderInsert = (byte)'6',

        /// <summary>
        /// 报单操作
        /// </summary>
        OrderAction = (byte)'7',

        /// <summary>
        /// 同步系统数据
        /// </summary>
        SyncSystemData = (byte)'8',

        /// <summary>
        /// 同步经纪公司数据
        /// </summary>
        SyncBrokerData = (byte)'9',

        /// <summary>
        /// 批量同步经纪公司数据
        /// </summary>
        BachSyncBrokerData = (byte)'A',

        /// <summary>
        /// 超级查询
        /// </summary>
        SuperQuery = (byte)'B',

        /// <summary>
        /// 报单插入
        /// </summary>
        ParkedOrderInsert = (byte)'C',

        /// <summary>
        /// 报单操作
        /// </summary>
        ParkedOrderAction = (byte)'D',

        /// <summary>
        /// 同步动态令牌
        /// </summary>
        SyncOTP = (byte)'E'
    }

    /// <summary>
    /// TFtdcBrokerFunctionCodeType是一个经纪公司功能代码类型
    /// </summary>
    public enum EnumThostBrokerFunctionCodeType : byte
    {
        /// <summary>
        /// 强制用户登出
        /// </summary>
        ForceUserLogout = (byte)'1',

        /// <summary>
        /// 变更用户口令
        /// </summary>
        UserPasswordUpdate = (byte)'2',

        /// <summary>
        /// 同步经纪公司数据
        /// </summary>
        SyncBrokerData = (byte)'3',

        /// <summary>
        /// 批量同步经纪公司数据
        /// </summary>
        BachSyncBrokerData = (byte)'4',

        /// <summary>
        /// 报单插入
        /// </summary>
        OrderInsert = (byte)'5',

        /// <summary>
        /// 报单操作
        /// </summary>
        OrderAction = (byte)'6',

        /// <summary>
        /// 全部查询
        /// </summary>
        AllQuery = (byte)'7',

        /// <summary>
        /// 系统功能：登入/登出/修改密码等
        /// </summary>
        log = (byte)'a',

        /// <summary>
        /// 基本查询：查询基础数据，如合约，交易所等常量
        /// </summary>
        BaseQry = (byte)'b',

        /// <summary>
        /// 交易查询：如查成交，委托
        /// </summary>
        TradeQry = (byte)'c',

        /// <summary>
        /// 交易功能：报单，撤单
        /// </summary>
        Trade = (byte)'d',

        /// <summary>
        /// 银期转账
        /// </summary>
        Virement = (byte)'e',

        /// <summary>
        /// 风险监控
        /// </summary>
        Risk = (byte)'f',

        /// <summary>
        /// 查询/管理：查询会话，踢人等
        /// </summary>
        Session = (byte)'g',

        /// <summary>
        /// 风控通知控制
        /// </summary>
        RiskNoticeCtl = (byte)'h',

        /// <summary>
        /// 风控通知发送
        /// </summary>
        RiskNotice = (byte)'i',

        /// <summary>
        /// 察看经纪公司资金权限
        /// </summary>
        BrokerDeposit = (byte)'j',

        /// <summary>
        /// 资金查询
        /// </summary>
        QueryFund = (byte)'k',

        /// <summary>
        /// 报单查询
        /// </summary>
        QueryOrder = (byte)'l',

        /// <summary>
        /// 成交查询
        /// </summary>
        QueryTrade = (byte)'m',

        /// <summary>
        /// 持仓查询
        /// </summary>
        QueryPosition = (byte)'n',

        /// <summary>
        /// 行情查询
        /// </summary>
        QueryMarketData = (byte)'o',

        /// <summary>
        /// 用户事件查询
        /// </summary>
        QueryUserEvent = (byte)'p',

        /// <summary>
        /// 风险通知查询
        /// </summary>
        QueryRiskNotify = (byte)'q',

        /// <summary>
        /// 出入金查询
        /// </summary>
        QueryFundChange = (byte)'r',

        /// <summary>
        /// 投资者信息查询
        /// </summary>
        QueryInvestor = (byte)'s',

        /// <summary>
        /// 交易编码查询
        /// </summary>
        QueryTradingCode = (byte)'t',

        /// <summary>
        /// 强平
        /// </summary>
        ForceClose = (byte)'u',

        /// <summary>
        /// 压力测试
        /// </summary>
        PressTest = (byte)'v',

        /// <summary>
        /// 权益反算
        /// </summary>
        RemainCalc = (byte)'w',

        /// <summary>
        /// 净持仓保证金指标
        /// </summary>
        NetPositionInd = (byte)'x',

        /// <summary>
        /// 风险预算
        /// </summary>
        RiskPredict = (byte)'y',

        /// <summary>
        /// 数据导出
        /// </summary>
        DataExport = (byte)'z',

        /// <summary>
        /// 风控指标设置
        /// </summary>
        RiskTargetSetup = (byte)'A',

        /// <summary>
        /// 行情预警
        /// </summary>
        MarketDataWarn = (byte)'B',


        /// <summary>
        /// 业务通知查询
        /// </summary>
        QryBizNotice = (byte)'C',


        /// <summary>
        /// 业务通知模板设置
        /// </summary>
        CfgBizNotice = (byte)'D',

        /// <summary>
        /// 同步动态令牌
        /// </summary>
        SyncOTP = (byte)'E',


        /// <summary>
        /// 发送业务通知
        /// </summary>
        SendBizNotice = (byte)'F',

        /// <summary>
        /// 风险级别标准设置
        /// </summary>
        CfgRiskLevelStd = (byte)'G',

        /// <summary>
        /// 交易终端应急功能
        /// </summary>
        TbCommand = (byte)'H',

        /// <summary>
        /// 删除未知单
        /// </summary>
        DeleteOrder = (byte)'J',

        /// <summary>
        /// 预埋报单插入
        /// </summary>
        ParkedOrderInsert = (byte)'K',

        /// <summary>
        /// 预埋报单操作
        /// </summary>
        ParkedOrderAction = (byte)'L',

        /// <summary>
        /// 资金不够仍允许行权
        /// </summary>
       ExecOrderNoCheck = (byte)'M'


    }

    /// <summary>
    /// TFtdcOrderActionStatusType是一个报单操作状态类型
    /// </summary>
    public enum EnumThostOrderActionStatusType : byte
    {
        /// <summary>
        /// 已经提交
        /// </summary>
        Submitted = (byte)'a',

        /// <summary>
        /// 已经接受
        /// </summary>
        Accepted = (byte)'b',

        /// <summary>
        /// 已经被拒绝
        /// </summary>
        Rejected = (byte)'c'
    }

    /// <summary>
    /// TFtdcOrderStatusType是一个报单状态类型
    /// </summary>
    public enum EnumThostOrderStatusType : byte
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
        /// 未知
        /// </summary>
        Unknown = (byte)'a',

        /// <summary>
        /// 尚未触发
        /// </summary>
        NotTouched = (byte)'b',

        /// <summary>
        /// 已触发
        /// </summary>
        Touched = (byte)'c'
    }

    /// <summary>
    /// TFtdcOrderSubmitStatusType是一个报单提交状态类型
    /// </summary>
    public enum EnumThostOrderSubmitStatusType : byte
    {
        /// <summary>
        /// 已经提交
        /// </summary>
        InsertSubmitted = (byte)'0',

        /// <summary>
        /// 撤单已经提交
        /// </summary>
        CancelSubmitted = (byte)'1',

        /// <summary>
        /// 修改已经提交
        /// </summary>
        ModifySubmitted = (byte)'2',

        /// <summary>
        /// 已经接受
        /// </summary>
        Accepted = (byte)'3',

        /// <summary>
        /// 报单已经被拒绝
        /// </summary>
        InsertRejected = (byte)'4',

        /// <summary>
        /// 撤单已经被拒绝
        /// </summary>
        CancelRejected = (byte)'5',

        /// <summary>
        /// 改单已经被拒绝
        /// </summary>
        ModifyRejected = (byte)'6'
    }

    /// <summary>
    /// TFtdcPositionDateType是一个持仓日期类型
    /// </summary>
    public enum EnumThostPositionDateType : byte
    {
        /// <summary>
        /// 今日持仓
        /// </summary>
        Today = (byte)'1',

        /// <summary>
        /// 历史持仓
        /// </summary>
        History = (byte)'2'
    }

    /// <summary>
    /// TFtdcPositionDateTypeType是一个持仓日期类型类型
    /// </summary>
    public enum EnumThostPositionDateTypeType : byte
    {
        /// <summary>
        /// 使用历史持仓
        /// </summary>
        UseHistory = (byte)'1',

        /// <summary>
        /// 不使用历史持仓
        /// </summary>
        NoUseHistory = (byte)'2'
    }

    /// <summary>
    /// TFtdcTradingRoleType是一个交易角色类型
    /// </summary>
    public enum EnumThostTradingRoleType : byte
    {
        /// <summary>
        /// 代理
        /// </summary>
        Broker = (byte)'1',

        /// <summary>
        /// 自营
        /// </summary>
        Host = (byte)'2',

        /// <summary>
        /// 做市商
        /// </summary>
        Maker = (byte)'3'
    }

    /// <summary>
    /// TFtdcProductClassType是一个产品类型类型
    /// </summary>
    public enum EnumThostProductClassType : byte
    {
        /// <summary>
        /// 期货
        /// </summary>
        Futures = (byte)'1',

        /// <summary>
        /// 期货期权
        /// </summary>
        Options = (byte)'2',

        /// <summary>
        /// 组合
        /// </summary>
        Combination = (byte)'3',

        /// <summary>
        /// 即期
        /// </summary>
        Spot = (byte)'4',

        /// <summary>
        /// 期转现
        /// </summary>
        EFP = (byte)'5',

        /// <summary>
		/// 现货期权
		/// </summary>
        SpotOption = (byte)'6'
    }

    /// <summary>
    /// TFtdcInstLifePhaseType是一个合约生命周期状态类型
    /// </summary>
    public enum EnumThostInstLifePhaseType : byte
    {
        /// <summary>
        /// 未上市
        /// </summary>
        NotStart = (byte)'0',

        /// <summary>
        /// 上市
        /// </summary>
        Started = (byte)'1',

        /// <summary>
        /// 停牌
        /// </summary>
        Pause = (byte)'2',

        /// <summary>
        /// 到期
        /// </summary>
        Expired = (byte)'3'
    }

    /// <summary>
    /// TFtdcDirectionType是一个买卖方向类型
    /// </summary>
    public enum EnumThostDirectionType : byte
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
    /// TFtdcPositionTypeType是一个持仓类型类型
    /// </summary>
    public enum EnumThostPositionTypeType : byte
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
    /// TFtdcPosiDirectionType是一个持仓多空方向类型
    /// </summary>
    public enum EnumThostPosiDirectionType : byte
    {
        /// <summary>
        /// 净
        /// </summary>
        Net = (byte)'1',

        /// <summary>
        /// 多头
        /// </summary>
        Long = (byte)'2',

        /// <summary>
        /// 空头
        /// </summary>
        Short = (byte)'3'
    }

    /// <summary>
    /// TFtdcSysSettlementStatusType是一个系统结算状态类型
    /// </summary>
    public enum EnumThostSysSettlementStatusType : byte
    {
        /// <summary>
        /// 不活跃
        /// </summary>
        NonActive = (byte)'1',

        /// <summary>
        /// 启动
        /// </summary>
        Startup = (byte)'2',

        /// <summary>
        /// 操作
        /// </summary>
        Operating = (byte)'3',

        /// <summary>
        /// 结算
        /// </summary>
        Settlement = (byte)'4',

        /// <summary>
        /// 结算完成
        /// </summary>
        SettlementFinished = (byte)'5'
    }

    /// <summary>
    /// TFtdcRatioAttrType是一个费率属性类型
    /// </summary>
    public enum EnumThostRatioAttrType : byte
    {
        /// <summary>
        /// 交易费率
        /// </summary>
        Trade = (byte)'0',

        /// <summary>
        /// 结算费率
        /// </summary>
        Settlement = (byte)'1'
    }

    /// <summary>
    /// TFtdcHedgeFlagType是一个投机套保标志类型
    /// </summary>
    public enum EnumThostHedgeFlagType : byte
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
        MarketMaker = (byte)'5'
    }

    /// <summary>
    /// TFtdcClientIDTypeType是一个交易编码类型类型
    /// </summary>
    public enum EnumThostClientIDTypeType : byte
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
        MarketMaker = (byte)'5'
    }

    /// <summary>
    /// TFtdcOrderPriceTypeType是一个报单价格条件类型
    /// </summary>
    public enum EnumThostOrderPriceTypeType : byte
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
        /// 最新价
        /// </summary>
        LastPrice = (byte)'4',

        /// <summary>
        /// 最新价浮动上浮1个ticks
        /// </summary>
        LastPricePlusOneTicks = (byte)'5',

        /// <summary>
        /// 最新价浮动上浮2个ticks
        /// </summary>
        LastPricePlusTwoTicks = (byte)'6',

        /// <summary>
        /// 最新价浮动上浮3个ticks
        /// </summary>
        LastPricePlusThreeTicks = (byte)'7',

        /// <summary>
        /// 卖一价
        /// </summary>
        AskPrice1 = (byte)'8',

        /// <summary>
        /// 卖一价浮动上浮1个ticks
        /// </summary>
        AskPrice1PlusOneTicks = (byte)'9',

        /// <summary>
        /// 卖一价浮动上浮2个ticks
        /// </summary>
        AskPrice1PlusTwoTicks = (byte)'A',

        /// <summary>
        /// 卖一价浮动上浮3个ticks
        /// </summary>
        AskPrice1PlusThreeTicks = (byte)'B',

        /// <summary>
        /// 买一价
        /// </summary>
        BidPrice1 = (byte)'C',

        /// <summary>
        /// 买一价浮动上浮1个ticks
        /// </summary>
        BidPrice1PlusOneTicks = (byte)'D',

        /// <summary>
        /// 买一价浮动上浮2个ticks
        /// </summary>
        BidPrice1PlusTwoTicks = (byte)'E',

        /// <summary>
        /// 买一价浮动上浮3个ticks
        /// </summary>
        BidPrice1PlusThreeTicks = (byte)'F'
    }

    /// <summary>
    /// TFtdcOffsetFlagType是一个开平标志类型
    /// </summary>
    public enum EnumThostOffsetFlagType : byte
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
        CloseYesterday = (byte)'4',

        /// <summary>
        /// 强减
        /// </summary>
        ForceOff = (byte)'5',

        /// <summary>
        /// 本地强平
        /// </summary>
        LocalForceClose = (byte)'6'
    }

    /// <summary>
    /// TFtdcForceCloseReasonType是一个强平原因类型
    /// </summary>
    public enum EnumThostForceCloseReasonType : byte
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
        NotMultiple = (byte)'4',

        /// <summary>
        /// 违规
        /// </summary>
        Violation = (byte)'5',

        /// <summary>
        /// 其它
        /// </summary>
        Other = (byte)'6',

        /// <summary>
        /// 自然人临近交割
        /// </summary>
        PersonDeliv = (byte)'7'
    }

    /// <summary>
    /// TFtdcOrderTypeType是一个报单类型类型
    /// </summary>
    public enum EnumThostOrderTypeType : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = (byte)'0',

        /// <summary>
        /// 报价衍生
        /// </summary>
        DeriveFromQuote = (byte)'1',

        /// <summary>
        /// 组合衍生
        /// </summary>
        DeriveFromCombination = (byte)'2',

        /// <summary>
        /// 组合报单
        /// </summary>
        Combination = (byte)'3',

        /// <summary>
        /// 条件单
        /// </summary>
        ConditionalOrder = (byte)'4',

        /// <summary>
        /// 互换单
        /// </summary>
        Swap = (byte)'5'
    }

    /// <summary>
    /// TFtdcTimeConditionType是一个有效期类型类型
    /// </summary>
    public enum EnumThostTimeConditionType : byte
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
    /// TFtdcVolumeConditionType是一个成交量类型类型
    /// </summary>
    public enum EnumThostVolumeConditionType : byte
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
    /// TFtdcContingentConditionType是一个触发条件类型
    /// </summary>
    public enum EnumThostContingentConditionType : byte
    {
        /// <summary>
        /// 立即
        /// </summary>
        Immediately = (byte)'1',

        /// <summary>
        /// 止损
        /// </summary>
        Touch = (byte)'2',

        /// <summary>
        /// 止赢
        /// </summary>
        TouchProfit = (byte)'3',

        /// <summary>
        /// 预埋单
        /// </summary>
        ParkedOrder = (byte)'4',

        /// <summary>
        /// 最新价大于条件价
        /// </summary>
        LastPriceGreaterThanStopPrice = (byte)'5',

        /// <summary>
        /// 最新价大于等于条件价
        /// </summary>
        LastPriceGreaterEqualStopPrice = (byte)'6',

        /// <summary>
        /// 最新价小于条件价
        /// </summary>
        LastPriceLesserThanStopPrice = (byte)'7',

        /// <summary>
        /// 最新价小于等于条件价
        /// </summary>
        LastPriceLesserEqualStopPrice = (byte)'8',

        /// <summary>
        /// 卖一价大于条件价
        /// </summary>
        AskPriceGreaterThanStopPrice = (byte)'9',

        /// <summary>
        /// 卖一价大于等于条件价
        /// </summary>
        AskPriceGreaterEqualStopPrice = (byte)'A',

        /// <summary>
        /// 卖一价小于条件价
        /// </summary>
        AskPriceLesserThanStopPrice = (byte)'B',

        /// <summary>
        /// 卖一价小于等于条件价
        /// </summary>
        AskPriceLesserEqualStopPrice = (byte)'C',

        /// <summary>
        /// 买一价大于条件价
        /// </summary>
        BidPriceGreaterThanStopPrice = (byte)'D',

        /// <summary>
        /// 买一价大于等于条件价
        /// </summary>
        BidPriceGreaterEqualStopPrice = (byte)'E',

        /// <summary>
        /// 买一价小于条件价
        /// </summary>
        BidPriceLesserThanStopPrice = (byte)'F',

        /// <summary>
        /// 买一价小于等于条件价
        /// </summary>
        BidPriceLesserEqualStopPrice = (byte)'H'
    }

    /// <summary>
    /// TFtdcActionFlagType是一个操作标志类型
    /// </summary>
    public enum EnumThostActionFlagType : byte
    {
        /// <summary>
        /// 删除
        /// </summary>
        Delete = (byte)'0',

        /// <summary>
        /// 修改
        /// </summary>
        Modify = (byte)'3'
    }

    /// <summary>
    /// TFtdcTradingRightType是一个交易权限类型
    /// </summary>
    public enum EnumThostTradingRightType : byte
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
    /// TFtdcOrderSourceType是一个报单来源类型
    /// </summary>
    public enum EnumThostOrderSourceType : byte
    {
        /// <summary>
        /// 来自参与者
        /// </summary>
        Participant = (byte)'0',

        /// <summary>
        /// 来自管理员
        /// </summary>
        Administrator = (byte)'1'
    }

    /// <summary>
    /// TFtdcTradeTypeType是一个成交类型类型
    /// </summary>
    public enum EnumThostTradeTypeType : byte
    {
        /// <summary>
        /// 普通成交
        /// </summary>
        Common = (byte)'0',

        /// <summary>
        /// 期权执行
        /// </summary>
        OptionsExecution = (byte)'1',

        /// <summary>
        /// OTC成交
        /// </summary>
        OTC = (byte)'2',

        /// <summary>
        /// 期转现衍生成交
        /// </summary>
        EFPDerived = (byte)'3',

        /// <summary>
        /// 组合衍生成交
        /// </summary>
        CombinationDerived = (byte)'4'
    }

    /// <summary>
    /// TFtdcPriceSourceType是一个成交价来源类型
    /// </summary>
    public enum EnumThostPriceSourceType : byte
    {
        /// <summary>
        /// 前成交价
        /// </summary>
        LastPrice = (byte)'0',

        /// <summary>
        /// 买委托价
        /// </summary>
        Buy = (byte)'1',

        /// <summary>
        /// 卖委托价
        /// </summary>
        Sell = (byte)'2'
    }

    /// <summary>
    /// TFtdcInstrumentStatusType是一个合约交易状态类型
    /// </summary>
    public enum EnumThostInstrumentStatusType : byte
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
    /// TFtdcInstStatusEnterReasonType是一个品种进入交易状态原因类型
    /// </summary>
    public enum EnumThostInstStatusEnterReasonType : byte
    {
        /// <summary>
        /// 自动切换
        /// </summary>
        Automatic = (byte)'1',

        /// <summary>
        /// 手动切换
        /// </summary>
        Manual = (byte)'2',

        /// <summary>
        /// 熔断
        /// </summary>
        Fuse = (byte)'3'
    }

    /// <summary>
    /// TFtdcBatchStatusType是一个处理状态类型
    /// </summary>
    public enum EnumThostBatchStatusType : byte
    {
        /// <summary>
        /// 未上传
        /// </summary>
        NoUpload = (byte)'1',

        /// <summary>
        /// 已上传
        /// </summary>
        Uploaded = (byte)'2',

        /// <summary>
        /// 审核失败
        /// </summary>
        Failed = (byte)'3'
    }

    /// <summary>
    /// TFtdcReturnStyleType是一个按品种返还方式类型
    /// </summary>
    public enum EnumThostReturnStyleType : byte
    {
        /// <summary>
        /// 按所有品种
        /// </summary>
        All = (byte)'1',

        /// <summary>
        /// 按品种
        /// </summary>
        ByProduct = (byte)'2'
    }

    /// <summary>
    /// TFtdcReturnPatternType是一个返还模式类型
    /// </summary>
    public enum EnumThostReturnPatternType : byte
    {
        /// <summary>
        /// 按成交手数
        /// </summary>
        ByVolume = (byte)'1',

        /// <summary>
        /// 按留存手续费
        /// </summary>
        ByFeeOnHand = (byte)'2'
    }

    /// <summary>
    /// TFtdcReturnLevelType是一个返还级别类型
    /// </summary>
    public enum EnumThostReturnLevelType : byte
    {
        /// <summary>
        /// 级别1
        /// </summary>
        Level1 = (byte)'1',

        /// <summary>
        /// 级别2
        /// </summary>
        Level2 = (byte)'2',

        /// <summary>
        /// 级别3
        /// </summary>
        Level3 = (byte)'3',

        /// <summary>
        /// 级别4
        /// </summary>
        Level4 = (byte)'4',

        /// <summary>
        /// 级别5
        /// </summary>
        Level5 = (byte)'5',

        /// <summary>
        /// 级别6
        /// </summary>
        Level6 = (byte)'6',

        /// <summary>
        /// 级别7
        /// </summary>
        Level7 = (byte)'7',

        /// <summary>
        /// 级别8
        /// </summary>
        Level8 = (byte)'8',

        /// <summary>
        /// 级别9
        /// </summary>
        Level9 = (byte)'9'
    }

    /// <summary>
    /// TFtdcReturnStandardType是一个返还标准类型
    /// </summary>
    public enum EnumThostReturnStandardType : byte
    {
        /// <summary>
        /// 分阶段返还
        /// </summary>
        ByPeriod = (byte)'1',

        /// <summary>
        /// 按某一标准
        /// </summary>
        ByStandard = (byte)'2'
    }

    /// <summary>
    /// TFtdcMortgageTypeType是一个质押类型类型
    /// </summary>
    public enum EnumThostMortgageTypeType : byte
    {
        /// <summary>
        /// 质出
        /// </summary>
        Out = (byte)'0',

        /// <summary>
        /// 质入
        /// </summary>
        In = (byte)'1'
    }

    /// <summary>
    /// TFtdcInvestorSettlementParamIDType是一个投资者结算参数代码类型
    /// </summary>
    public enum EnumThostInvestorSettlementParamIDType : byte
    {
        /// <summary>
        /// 基础保证金
        /// </summary>
        BaseMargin = (byte)'1',

        /// <summary>
        /// 最低权益标准
        /// </summary>
        LowestInterest = (byte)'2',

        /// <summary>
        /// 质押比例
        /// </summary>
        MortgageRatio = (byte)'4',

        /// <summary>
        /// 保证金算法
        /// </summary>
        MarginWay = (byte)'5',

        /// <summary>
        /// 结算单(盯市)权益等于结存
        /// </summary>
        BillDeposit = (byte)'9'
    }

    /// <summary>
    /// TFtdcExchangeSettlementParamIDType是一个交易所结算参数代码类型
    /// </summary>
    public enum EnumThostExchangeSettlementParamIDType : byte
    {
        /// <summary>
        /// 质押比例
        /// </summary>
        MortgageRatio = (byte)'1',

        /// <summary>
        /// 分项资金导入项
        /// </summary>
        OtherFundItem = (byte)'2',

        /// <summary>
        /// 分项资金入交易所出入金
        /// </summary>
        OtherFundImport = (byte)'3',

        /// <summary>
        /// 上期所交割手续费收取方式
        /// </summary>
        SHFEDelivFee = (byte)'4',

        /// <summary>
        /// 大商所交割手续费收取方式
        /// </summary>
        DCEDelivFee = (byte)'5',

        /// <summary>
        /// 中金所开户最低可用金额
        /// </summary>
        CFFEXMinPrepa = (byte)'6'
    }

    /// <summary>
    /// TFtdcSystemParamIDType是一个系统参数代码类型
    /// </summary>
    public enum EnumThostSystemParamIDType : byte
    {
        /// <summary>
        /// 投资者代码最小长度
        /// </summary>
        InvestorIDMinLength = (byte)'1',

        /// <summary>
        /// 投资者帐号代码最小长度
        /// </summary>
        AccountIDMinLength = (byte)'2',

        /// <summary>
        /// 投资者开户默认登录权限
        /// </summary>
        UserRightLogon = (byte)'3',

        /// <summary>
        /// 投资者交易结算单成交汇总方式
        /// </summary>
        SettlementBillTrade = (byte)'4',

        /// <summary>
        /// 统一开户更新交易编码方式
        /// </summary>
        TradingCode = (byte)'5',

        /// <summary>
        /// 结算是否判断存在未复核的出入金和分项资金
        /// </summary>
        CheckFund = (byte)'6',

        /// <summary>
        /// 上传的结算文件标识
        /// </summary>
        UploadSettlementFile = (byte)'U',

        /// <summary>
        /// 下载的保证金存管文件
        /// </summary>
        DownloadCSRCFile = (byte)'D',

        /// <summary>
        /// 结算单文件标识
        /// </summary>
        SettlementBillFile = (byte)'S',

        /// <summary>
        /// 证监会文件标识
        /// </summary>
        CSRCOthersFile = (byte)'C',

        /// <summary>
        /// 投资者照片路径
        /// </summary>
        InvestorPhoto = (byte)'P',

        /// <summary>
        /// 上报保证金监控中心数据
        /// </summary>
        CSRCData = (byte)'R',

        /// <summary>
        /// 开户密码录入方式
        /// </summary>
        InvestorPwdModel = (byte)'I'
    }

    /// <summary>
    /// TFtdcTradeParamIDType是一个交易系统参数代码类型
    /// </summary>
    public enum EnumThostTradeParamIDType : byte
    {
        /// <summary>
        /// 系统加密算法
        /// </summary>
        EncryptionStandard = (byte)'E',

        /// <summary>
        /// 系统风险算法
        /// </summary>
        RiskMode = (byte)'R',

        /// <summary>
        /// 系统风险算法是否全局 0-否 1-是
        /// </summary>
        RiskModeGlobal = (byte)'G',

        /// <summary>
        /// 系统风险算法是否全局 0-否 1-是
        /// </summary>
        _modeEncode = (byte)'P',

        /// <summary>
        /// 价格小数位数参数
        /// </summary>
        tickMode = (byte)'T',

        /// <summary>
        /// 用户最大会话数
        /// </summary>
        SingleUserSessionMaxNum = (byte)'S',

        /// <summary>
        /// 最大连续登录失败数
        /// </summary>
        LoginFailMaxNum = (byte)'L',

        /// <summary>
        /// 是否强制认证
        /// </summary>
        IsAuthForce = (byte)'A',

        /// <summary>
        /// 是否冻结证券持仓
        /// </summary>
        IsPosiFreeze = (byte)'F',

        /// <summary>
        /// 是否限仓
        /// </summary>
        IsPosiLimit = (byte)'M',

        /// <summary>
        /// 郑商所询价时间间隔
        /// </summary>
        ForQuoteTimeInterval = (byte)'Q',

        /// <summary>
        /// 是否期货限仓
        /// </summary>
        IsFuturePosiLimit = (byte)'B',

        /// <summary>
        /// 是否期货下单频率限制
        /// </summary>
        IsFutureOrderFreq = (byte)'C',

        /// <summary>
        /// 行权冻结是否计算盈利
        /// </summary>
        IsExecOrderProfit = (byte)'H'
    }

    /// <summary>
    /// TFtdcFileIDType是一个文件标识类型
    /// </summary>
    public enum EnumThostFileIDType : byte
    {
        /// <summary>
        /// 资金数据
        /// </summary>
        SettlementFund = (byte)'F',

        /// <summary>
        /// 成交数据
        /// </summary>
        Trade = (byte)'T',

        /// <summary>
        /// 投资者持仓数据
        /// </summary>
        InvestorPosition = (byte)'P',

        /// <summary>
        /// 投资者分项资金数据
        /// </summary>
        SubEntryFund = (byte)'O',

        /// <summary>
        /// 郑商所组合持仓数据
        /// </summary>
        CZCECombinationPos = (byte)'C',

        /// <summary>
        /// 上报保证金监控中心数据
        /// </summary>
        CSRCData = (byte)'R'
    }

    /// <summary>
    /// TFtdcFileTypeType是一个文件上传类型类型
    /// </summary>
    public enum EnumThostFileTypeType : byte
    {
        /// <summary>
        /// 结算
        /// </summary>
        Settlement = (byte)'0',

        /// <summary>
        /// 核对
        /// </summary>
        Check = (byte)'1'
    }

    /// <summary>
    /// TFtdcFileFormatType是一个文件格式类型
    /// </summary>
    public enum EnumThostFileFormatType : byte
    {
        /// <summary>
        /// 文本文件(.txt)
        /// </summary>
        Txt = (byte)'0',

        /// <summary>
        /// 压缩文件(.zip)
        /// </summary>
        Zip = (byte)'1',

        /// <summary>
        /// DBF文件(.dbf)
        /// </summary>
        DBF = (byte)'2'
    }

    /// <summary>
    /// TFtdcFileUploadStatusType是一个文件状态类型
    /// </summary>
    public enum EnumThostFileUploadStatusType : byte
    {
        /// <summary>
        /// 上传成功
        /// </summary>
        SucceedUpload = (byte)'1',

        /// <summary>
        /// 上传失败
        /// </summary>
        FailedUpload = (byte)'2',

        /// <summary>
        /// 导入成功
        /// </summary>
        SucceedLoad = (byte)'3',

        /// <summary>
        /// 导入部分成功
        /// </summary>
        PartSucceedLoad = (byte)'4',

        /// <summary>
        /// 导入失败
        /// </summary>
        FailedLoad = (byte)'5'
    }

    /// <summary>
    /// TFtdcTransferDirectionType是一个移仓方向类型
    /// </summary>
    public enum EnumThostTransferDirectionType : byte
    {
        /// <summary>
        /// 移出
        /// </summary>
        Out = (byte)'0',

        /// <summary>
        /// 移入
        /// </summary>
        In = (byte)'1'
    }

    /// <summary>
    /// TFtdcBankFlagType是一个银行统一标识类型类型
    /// </summary>
    public enum EnumThostBankFlagType : byte
    {
        /// <summary>
        /// 工商银行
        /// </summary>
        ICBC = (byte)'1',

        /// <summary>
        /// 农业银行
        /// </summary>
        ABC = (byte)'2',

        /// <summary>
        /// 中国银行
        /// </summary>
        BC = (byte)'3',

        /// <summary>
        /// 建设银行
        /// </summary>
        CBC = (byte)'4',

        /// <summary>
        /// 交通银行
        /// </summary>
        BOC = (byte)'5',

        /// <summary>
        /// 其他银行
        /// </summary>
        Other = (byte)'Z'
    }

    /// <summary>
    /// TFtdcSpecialCreateRuleType是一个特殊的创建规则类型
    /// </summary>
    public enum EnumThostSpecialCreateRuleType : byte
    {
        /// <summary>
        /// 没有特殊创建规则
        /// </summary>
        NoSpecialRule = (byte)'0',

        /// <summary>
        /// 不包含春节
        /// </summary>
        NoSpringFestival = (byte)'1'
    }

    /// <summary>
    /// TFtdcBasisPriceTypeType是一个挂牌基准价类型类型
    /// </summary>
    public enum EnumThostBasisPriceTypeType : byte
    {
        /// <summary>
        /// 上一合约结算价
        /// </summary>
        LastSettlement = (byte)'1',

        /// <summary>
        /// 上一合约收盘价
        /// </summary>
        LaseClose = (byte)'2'
    }

    /// <summary>
    /// TFtdcProductLifePhaseType是一个产品生命周期状态类型
    /// </summary>
    public enum EnumThostProductLifePhaseType : byte
    {
        /// <summary>
        /// 活跃
        /// </summary>
        Active = (byte)'1',

        /// <summary>
        /// 不活跃
        /// </summary>
        NonActive = (byte)'2',

        /// <summary>
        /// 注销
        /// </summary>
        Canceled = (byte)'3'
    }

    /// <summary>
    /// TFtdcDeliveryModeType是一个交割方式类型
    /// </summary>
    public enum EnumThostDeliveryModeType : byte
    {
        /// <summary>
        /// 现金交割
        /// </summary>
        CashDeliv = (byte)'1',

        /// <summary>
        /// 实物交割
        /// </summary>
        CommodityDeliv = (byte)'2'
    }

    /// <summary>
    /// TFtdcFundIOTypeType是一个出入金类型类型
    /// </summary>
    public enum EnumThostFundIOTypeType : byte
    {
        /// <summary>
        /// 出入金
        /// </summary>
        FundIO = (byte)'1',

        /// <summary>
        /// 银期转帐
        /// </summary>
        Transfer = (byte)'2'
    }

    /// <summary>
    /// TFtdcFundTypeType是一个资金类型类型
    /// </summary>
    public enum EnumThostFundTypeType : byte
    {
        /// <summary>
        /// 银行存款
        /// </summary>
        Deposite = (byte)'1',

        /// <summary>
        /// 分项资金
        /// </summary>
        ItemFund = (byte)'2',

        /// <summary>
        /// 公司调整
        /// </summary>
        Company = (byte)'3'
    }

    /// <summary>
    /// TFtdcFundDirectionType是一个出入金方向类型
    /// </summary>
    public enum EnumThostFundDirectionType : byte
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
    /// TFtdcFundStatusType是一个资金状态类型
    /// </summary>
    public enum EnumThostFundStatusType : byte
    {
        /// <summary>
        /// 已录入
        /// </summary>
        Record = (byte)'1',

        /// <summary>
        /// 已复核
        /// </summary>
        Check = (byte)'2',

        /// <summary>
        /// 已冲销
        /// </summary>
        Charge = (byte)'3'
    }

    /// <summary>
    /// TFtdcPublishStatusType是一个发布状态类型
    /// </summary>
    public enum EnumThostPublishStatusType : byte
    {
        /// <summary>
        /// 未发布
        /// </summary>
        None = (byte)'1',

        /// <summary>
        /// 正在发布
        /// </summary>
        Publishing = (byte)'2',

        /// <summary>
        /// 已发布
        /// </summary>
        Published = (byte)'3'
    }

    /// <summary>
    /// TFtdcSystemStatusType是一个系统状态类型
    /// </summary>
    public enum EnumThostSystemStatusType : byte
    {
        /// <summary>
        /// 不活跃
        /// </summary>
        NonActive = (byte)'1',

        /// <summary>
        /// 启动
        /// </summary>
        Startup = (byte)'2',

        /// <summary>
        /// 交易开始初始化
        /// </summary>
        Initialize = (byte)'3',

        /// <summary>
        /// 交易完成初始化
        /// </summary>
        Initialized = (byte)'4',

        /// <summary>
        /// 收市开始
        /// </summary>
        Close = (byte)'5',

        /// <summary>
        /// 收市完成
        /// </summary>
        Closed = (byte)'6',

        /// <summary>
        /// 结算
        /// </summary>
        Settlement = (byte)'7'
    }

    /// <summary>
    /// TFtdcSettlementStatusType是一个结算状态类型
    /// </summary>
    public enum EnumThostSettlementStatusType : byte
    {
        /// <summary>
        /// 初始
        /// </summary>
        Initialize = (byte)'0',

        /// <summary>
        /// 结算中
        /// </summary>
        Settlementing = (byte)'1',

        /// <summary>
        /// 已结算
        /// </summary>
        Settlemented = (byte)'2',

        /// <summary>
        /// 结算完成
        /// </summary>
        Finished = (byte)'3'
    }

    /// <summary>
    /// TFtdcInvestorTypeType是一个投资者类型类型
    /// </summary>
    public enum EnumThostInvestorTypeType : byte
    {
        /// <summary>
        /// 自然人
        /// </summary>
        Person = (byte)'0',

        /// <summary>
        /// 法人
        /// </summary>
        Company = (byte)'1',

        /// <summary>
        /// 投资基金
        /// </summary>
        Fund = (byte)'2'
    }

    /// <summary>
    /// TFtdcBrokerTypeType是一个经纪公司类型类型
    /// </summary>
    public enum EnumThostBrokerTypeType : byte
    {
        /// <summary>
        /// 交易会员
        /// </summary>
        Trade = (byte)'0',

        /// <summary>
        /// 交易结算会员
        /// </summary>
        TradeSettle = (byte)'1'
    }

    /// <summary>
    /// TFtdcRiskLevelType是一个风险等级类型
    /// </summary>
    public enum EnumThostRiskLevelType : byte
    {
        /// <summary>
        /// 低风险客户
        /// </summary>
        Low = (byte)'1',

        /// <summary>
        /// 普通客户
        /// </summary>
        Normal = (byte)'2',

        /// <summary>
        /// 关注客户
        /// </summary>
        Focus = (byte)'3',

        /// <summary>
        /// 风险客户
        /// </summary>
        Risk = (byte)'4'
    }

    /// <summary>
    /// TFtdcFeeAcceptStyleType是一个手续费收取方式类型
    /// </summary>
    public enum EnumThostFeeAcceptStyleType : byte
    {
        /// <summary>
        /// 按交易收取
        /// </summary>
        ByTrade = (byte)'1',

        /// <summary>
        /// 按交割收取
        /// </summary>
        ByDeliv = (byte)'2',

        /// <summary>
        /// 不收
        /// </summary>
        None = (byte)'3',

        /// <summary>
        /// 按指定手续费收取
        /// </summary>
        FixFee = (byte)'4'
    }

    /// <summary>
    /// TFtdcPasswordTypeType是一个密码类型类型
    /// </summary>
    public enum EnumThostPasswordTypeType : byte
    {
        /// <summary>
        /// 交易密码
        /// </summary>
        Trade = (byte)'1',

        /// <summary>
        /// 资金密码
        /// </summary>
        Account = (byte)'2'
    }

    /// <summary>
    /// TFtdcAlgorithmType是一个盈亏算法类型
    /// </summary>
    public enum EnumThostAlgorithmType : byte
    {
        /// <summary>
        /// 浮盈浮亏都计算
        /// </summary>
        All = (byte)'1',

        /// <summary>
        /// 浮盈不计，浮亏计
        /// </summary>
        OnlyLost = (byte)'2',

        /// <summary>
        /// 浮盈计，浮亏不计
        /// </summary>
        OnlyGain = (byte)'3',

        /// <summary>
        /// 浮盈浮亏都不计算
        /// </summary>
        None = (byte)'4'
    }

    /// <summary>
    /// TFtdcIncludeCloseProfitType是一个是否包含平仓盈利类型
    /// </summary>
    public enum EnumThostIncludeCloseProfitType : byte
    {
        /// <summary>
        /// 包含平仓盈利
        /// </summary>
        Include = (byte)'0',

        /// <summary>
        /// 不包含平仓盈利
        /// </summary>
        NotInclude = (byte)'2'
    }

    /// <summary>
    /// TFtdcAllWithoutTradeType是一个是否受可提比例限制类型
    /// </summary>
    public enum EnumThostAllWithoutTradeType : byte
    {
        /// <summary>
        /// 不受可提比例限制
        /// </summary>
        Enable = (byte)'0',

        /// <summary>
        /// 受可提比例限制
        /// </summary>
        Disable = (byte)'2'
    }

    /// <summary>
    /// TFtdcFuturePwdFlagType是一个资金密码核对标志类型
    /// </summary>
    public enum EnumThostFuturePwdFlagType : byte
    {
        /// <summary>
        /// 不核对
        /// </summary>
        UnCheck = (byte)'0',

        /// <summary>
        /// 核对
        /// </summary>
        Check = (byte)'1'
    }

    /// <summary>
    /// TFtdcTransferTypeType是一个银期转账类型类型
    /// </summary>
    public enum EnumThostTransferTypeType : byte
    {
        /// <summary>
        /// 银行转期货
        /// </summary>
        BankToFuture = (byte)'0',

        /// <summary>
        /// 期货转银行
        /// </summary>
        FutureToBank = (byte)'1'
    }

    /// <summary>
    /// TFtdcTransferValidFlagType是一个转账有效标志类型
    /// </summary>
    public enum EnumThostTransferValidFlagType : byte
    {
        /// <summary>
        /// 无效或失败
        /// </summary>
        Invalid = (byte)'0',

        /// <summary>
        /// 有效
        /// </summary>
        Valid = (byte)'1',

        /// <summary>
        /// 冲正
        /// </summary>
        Reverse = (byte)'2'
    }

    /// <summary>
    /// TFtdcReasonType是一个事由类型
    /// </summary>
    public enum EnumThostReasonType : byte
    {
        /// <summary>
        /// 错单
        /// </summary>
        CD = (byte)'0',

        /// <summary>
        /// 资金在途
        /// </summary>
        ZT = (byte)'1',

        /// <summary>
        /// 其它
        /// </summary>
        QT = (byte)'2'
    }

    /// <summary>
    /// TFtdcSexType是一个性别类型
    /// </summary>
    public enum EnumThostSexType : byte
    {
        /// <summary>
        /// 未知
        /// </summary>
        None = (byte)'0',

        /// <summary>
        /// 男
        /// </summary>
        Man = (byte)'1',

        /// <summary>
        /// 女
        /// </summary>
        Woman = (byte)'2'
    }

    /// <summary>
    /// TFtdcUserTypeType是一个用户类型类型
    /// </summary>
    public enum EnumThostUserTypeType : byte
    {
        /// <summary>
        /// 投资者
        /// </summary>
        Investor = (byte)'0',

        /// <summary>
        /// 操作员
        /// </summary>
        Operator = (byte)'1',

        /// <summary>
        /// 管理员
        /// </summary>
        SuperUser = (byte)'2'
    }

    /// <summary>
    /// TFtdcRateTypeType是一个费率类型类型
    /// </summary>
    public enum EnumThostRateTypeType : byte
    {
        /// <summary>
        /// 保证金率
        /// </summary>
        MarginRate = (byte)'2',

        /// <summary>
        /// 手续费率
        /// </summary>
        CommRate = (byte)'1',

        /// <summary>
        /// 所有
        /// </summary>
        AllRate = (byte)'0'
    }

    /// <summary>
    /// TFtdcNoteTypeType是一个通知类型类型
    /// </summary>
    public enum EnumThostNoteTypeType : byte
    {
        /// <summary>
        /// 交易结算单
        /// </summary>
        TradeSettleBill = (byte)'1',

        /// <summary>
        /// 交易结算月报
        /// </summary>
        TradeSettleMonth = (byte)'2',

        /// <summary>
        /// 追加保证金通知书
        /// </summary>
        CallMarginNotes = (byte)'3',

        /// <summary>
        /// 强行平仓通知书
        /// </summary>
        ForceCloseNotes = (byte)'4',

        /// <summary>
        /// 成交通知书
        /// </summary>
        TradeNotes = (byte)'5',

        /// <summary>
        /// 交割通知书
        /// </summary>
        DelivNotes = (byte)'6'
    }

    /// <summary>
    /// TFtdcSettlementStyleType是一个结算单方式类型
    /// </summary>
    public enum EnumThostSettlementStyleType : byte
    {
        /// <summary>
        /// 逐日盯市
        /// </summary>
        Day = (byte)'1',

        /// <summary>
        /// 逐笔对冲
        /// </summary>
        Volume = (byte)'2'
    }

    /// <summary>
    /// TFtdcSettlementBillTypeType是一个结算单类型类型
    /// </summary>
    public enum EnumThostSettlementBillTypeType : byte
    {
        /// <summary>
        /// 日报
        /// </summary>
        Day = (byte)'0',

        /// <summary>
        /// 月报
        /// </summary>
        Month = (byte)'1'
    }

    /// <summary>
    /// TFtdcUserRightTypeType是一个客户权限类型类型
    /// </summary>
    public enum EnumThostUserRightTypeType : byte
    {
        /// <summary>
        /// 登录
        /// </summary>
        Logon = (byte)'1',

        /// <summary>
        /// 银期转帐
        /// </summary>
        Transfer = (byte)'2',

        /// <summary>
        /// 邮寄结算单
        /// </summary>
        EMail = (byte)'3',

        /// <summary>
        /// 传真结算单
        /// </summary>
        Fax = (byte)'4',

        /// <summary>
        /// 条件单
        /// </summary>
        ConditionOrder = (byte)'5'
    }

    /// <summary>
    /// TFtdcMarginPriceTypeType是一个保证金价格类型类型
    /// </summary>
    public enum EnumThostMarginPriceTypeType : byte
    {
        /// <summary>
        /// 昨结算价
        /// </summary>
        PreSettlementPrice = (byte)'1',

        /// <summary>
        /// 最新价
        /// </summary>
        SettlementPrice = (byte)'2',

        /// <summary>
        /// 成交均价
        /// </summary>
        AveragePrice = (byte)'3',

        /// <summary>
        /// 开仓价
        /// </summary>
        OpenPrice = (byte)'4'
    }

    /// <summary>
    /// TFtdcBillGenStatusType是一个结算单生成状态类型
    /// </summary>
    public enum EnumThostBillGenStatusType : byte
    {
        /// <summary>
        /// 不生成
        /// </summary>
        None = (byte)'0',

        /// <summary>
        /// 未生成
        /// </summary>
        NoGenerated = (byte)'1',

        /// <summary>
        /// 已生成
        /// </summary>
        Generated = (byte)'2'
    }

    /// <summary>
    /// TFtdcAlgoTypeType是一个算法类型类型
    /// </summary>
    public enum EnumThostAlgoTypeType : byte
    {
        /// <summary>
        /// 持仓处理算法
        /// </summary>
        HandlePositionAlgo = (byte)'1',

        /// <summary>
        /// 寻找保证金率算法
        /// </summary>
        FindMarginRateAlgo = (byte)'2'
    }

    /// <summary>
    /// TFtdcHandlePositionAlgoIDType是一个持仓处理算法编号类型
    /// </summary>
    public enum EnumThostHandlePositionAlgoIDType : byte
    {
        /// <summary>
        /// 基本
        /// </summary>
        Base = (byte)'1',

        /// <summary>
        /// 大连商品交易所
        /// </summary>
        DCE = (byte)'2',

        /// <summary>
        /// 郑州商品交易所
        /// </summary>
        CZCE = (byte)'3'
    }

    /// <summary>
    /// TFtdcFindMarginRateAlgoIDType是一个寻找保证金率算法编号类型
    /// </summary>
    public enum EnumThostFindMarginRateAlgoIDType : byte
    {
        /// <summary>
        /// 基本
        /// </summary>
        Base = (byte)'1',

        /// <summary>
        /// 大连商品交易所
        /// </summary>
        DCE = (byte)'2',

        /// <summary>
        /// 郑州商品交易所
        /// </summary>
        CZCE = (byte)'3'
    }

    /// <summary>
    /// TFtdcHandleTradingAccountAlgoIDType是一个资金处理算法编号类型
    /// </summary>
    public enum EnumThostHandleTradingAccountAlgoIDType : byte
    {
        /// <summary>
        /// 基本
        /// </summary>
        Base = (byte)'1',

        /// <summary>
        /// 大连商品交易所
        /// </summary>
        DCE = (byte)'2',

        /// <summary>
        /// 郑州商品交易所
        /// </summary>
        CZCE = (byte)'3'
    }

    /// <summary>
    /// TFtdcPersonTypeType是一个联系人类型类型
    /// </summary>
    public enum EnumThostPersonTypeType : byte
    {
        /// <summary>
        /// 指定下单人
        /// </summary>
        Order = (byte)'1',

        /// <summary>
        /// 开户授权人
        /// </summary>
        Open = (byte)'2',

        /// <summary>
        /// 资金调拨人
        /// </summary>
        Fund = (byte)'3',

        /// <summary>
        /// 结算单确认人
        /// </summary>
        Settlement = (byte)'4'
    }

    /// <summary>
    /// TFtdcQueryInvestorRangeType是一个查询范围类型
    /// </summary>
    public enum EnumThostQueryInvestorRangeType : byte
    {
        /// <summary>
        /// 所有
        /// </summary>
        All = (byte)'1',

        /// <summary>
        /// 查询分类
        /// </summary>
        Group = (byte)'2',

        /// <summary>
        /// 单一投资者
        /// </summary>
        Single = (byte)'3'
    }

    /// <summary>
    /// TFtdcInvestorRiskStatusType是一个投资者风险状态类型
    /// </summary>
    public enum EnumThostInvestorRiskStatusType : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = (byte)'1',

        /// <summary>
        /// 警告
        /// </summary>
        Warn = (byte)'2',

        /// <summary>
        /// 追保
        /// </summary>
        Call = (byte)'3',

        /// <summary>
        /// 强平
        /// </summary>
        Force = (byte)'4',

        /// <summary>
        /// 异常
        /// </summary>
        Exception = (byte)'5'
    }

    /// <summary>
    /// TFtdcUserEventTypeType是一个用户事件类型类型
    /// </summary>
    public enum EnumThostUserEventTypeType : byte
    {
        /// <summary>
        /// 所有
        /// </summary>
        All = (byte)' ',

        /// <summary>
        /// 登录
        /// </summary>
        Login = (byte)'1',

        /// <summary>
        /// 登出
        /// </summary>
        Logout = (byte)'2',

        /// <summary>
        /// 交易成功
        /// </summary>
        Trading = (byte)'3',

        /// <summary>
        /// 交易失败
        /// </summary>
        TradingError = (byte)'4',

        /// <summary>
        /// 修改密码
        /// </summary>
        UpdatePassword = (byte)'5',

        /// <summary>
        /// 其他
        /// </summary>
        Other = (byte)'9'
    }

    /// <summary>
    /// TFtdcCloseStyleType是一个平仓方式类型
    /// </summary>
    public enum EnumThostCloseStyleType : byte
    {
        /// <summary>
        /// 先开先平
        /// </summary>
        Close = (byte)'0',

        /// <summary>
        /// 先平今再平昨
        /// </summary>
        CloseToday = (byte)'1'
    }

    /// <summary>
    /// TFtdcStatModeType是一个统计方式类型
    /// </summary>
    public enum EnumThostStatModeType : byte
    {
        /// <summary>
        /// ----
        /// </summary>
        Non = (byte)'0',

        /// <summary>
        /// 按合约统计
        /// </summary>
        Instrument = (byte)'1',

        /// <summary>
        /// 按产品统计
        /// </summary>
        Product = (byte)'2',

        /// <summary>
        /// 按投资者统计
        /// </summary>
        Investor = (byte)'3'
    }

    /// <summary>
    /// TFtdcParkedOrderStatusType是一个预埋单状态类型
    /// </summary>
    public enum EnumThostParkedOrderStatusType : byte
    {
        /// <summary>
        /// 未发送
        /// </summary>
        NotSend = (byte)'1',

        /// <summary>
        /// 已发送
        /// </summary>
        Send = (byte)'2',

        /// <summary>
        /// 已删除
        /// </summary>
        Deleted = (byte)'3'
    }

    /// <summary>
    /// TFtdcVirDealStatusType是一个处理状态类型
    /// </summary>
    public enum EnumThostVirDealStatusType : byte
    {
        /// <summary>
        /// 正在处理
        /// </summary>
        Dealing = (byte)'1',

        /// <summary>
        /// 处理成功
        /// </summary>
        DeaclSucceed = (byte)'2'
    }

    /// <summary>
    /// TFtdcOrgSystemIDType是一个原有系统代码类型
    /// </summary>
    public enum EnumThostOrgSystemIDType : byte
    {
        /// <summary>
        /// 综合交易平台
        /// </summary>
        Standard = (byte)'0',

        /// <summary>
        /// 易盛系统
        /// </summary>
        ESunny = (byte)'1',

        /// <summary>
        /// 金仕达V6系统
        /// </summary>
        KingStarV6 = (byte)'2'
    }

    /// <summary>
    /// TFtdcVirTradeStatusType是一个交易状态类型
    /// </summary>
    public enum EnumThostVirTradeStatusType : byte
    {
        /// <summary>
        /// 正常处理中
        /// </summary>
        NaturalDeal = (byte)'0',

        /// <summary>
        /// 成功结束
        /// </summary>
        SucceedEnd = (byte)'1',

        /// <summary>
        /// 失败结束
        /// </summary>
        FailedEND = (byte)'2',

        /// <summary>
        /// 异常中
        /// </summary>
        Exception = (byte)'3',

        /// <summary>
        /// 已人工异常处理
        /// </summary>
        ManualDeal = (byte)'4',

        /// <summary>
        /// 通讯异常 ，请人工处理
        /// </summary>
        MesException = (byte)'5',

        /// <summary>
        /// 系统出错，请人工处理
        /// </summary>
        SysException = (byte)'6'
    }

    /// <summary>
    /// TFtdcVirBankAccTypeType是一个银行帐户类型类型
    /// </summary>
    public enum EnumThostVirBankAccTypeType : byte
    {
        /// <summary>
        /// 存折
        /// </summary>
        BankBook = (byte)'1',

        /// <summary>
        /// 储蓄卡
        /// </summary>
        BankCard = (byte)'2',

        /// <summary>
        /// 信用卡
        /// </summary>
        CreditCard = (byte)'3'
    }

    /// <summary>
    /// TFtdcVirementStatusType是一个银行帐户类型类型
    /// </summary>
    public enum EnumThostVirementStatusType : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        Natural = (byte)'0',

        /// <summary>
        /// 销户
        /// </summary>
        Canceled = (byte)'9'
    }

    /// <summary>
    /// TFtdcVirementAvailAbilityType是一个有效标志类型
    /// </summary>
    public enum EnumThostVirementAvailAbilityType : byte
    {
        /// <summary>
        /// 未确认
        /// </summary>
        NoAvailAbility = (byte)'0',

        /// <summary>
        /// 有效
        /// </summary>
        AvailAbility = (byte)'1',

        /// <summary>
        /// 冲正
        /// </summary>
        Repeal = (byte)'2'
    }

    /// <summary>
    /// TFtdcVirementTradeCodeType是一个交易代码类型
    /// </summary>
    public enum EnumThostVirementTradeCodeType : byte
    {
        /// <summary>
        /// 银行发起银行资金转期货
        /// </summary>
        BankBankToFuture_102001 = (byte)'1',//'102001',

        /// <summary>
        /// 银行发起期货资金转银行
        /// </summary>
        BankFutureToBank_102002 = (byte)'2',//'102002',

        /// <summary>
        /// 期货发起银行资金转期货
        /// </summary>
        FutureBankToFuture_202001 = (byte)'3',//'202001',

        /// <summary>
        /// 期货发起期货资金转银行
        /// </summary>
        FutureFutureToBank_202002 = (byte)'4'//'202002'
    }

    /// <summary>
    /// TFtdcCFMMCKeyKindType是一个动态密钥类别(保证金监管)类型
    /// </summary>
    public enum EnumThostCFMMCKeyKindType : byte
    {
        /// <summary>
        /// 主动请求更新
        /// </summary>
        REQUEST = (byte)'R',

        /// <summary>
        /// CFMMC自动更新
        /// </summary>
        AUTO = (byte)'A',

        /// <summary>
        /// CFMMC手动更新
        /// </summary>
        MANUAL = (byte)'M'
    }

    /// <summary>
    /// TFtdcCertificationTypeType是一个证件类型类型
    /// </summary>
    public enum EnumThostCertificationTypeType : byte
    {
        /// <summary>
        /// 身份证
        /// </summary>
        IDCard = (byte)'0',

        /// <summary>
        /// 护照
        /// </summary>
        Passport = (byte)'1',

        /// <summary>
        /// 军官证
        /// </summary>
        OfficerIDCard = (byte)'2',

        /// <summary>
        /// 士兵证
        /// </summary>
        SoldierIDCard = (byte)'3',

        /// <summary>
        /// 回乡证
        /// </summary>
        HomeComingCard = (byte)'4',

        /// <summary>
        /// 户口簿
        /// </summary>
        HouseholdRegister = (byte)'5',

        /// <summary>
        /// 营业执照号
        /// </summary>
        LicenseNo = (byte)'6',

        /// <summary>
        /// 组织机构代码证
        /// </summary>
        InstitutionCodeCard = (byte)'7',

        /// <summary>
        /// 临时营业执照号
        /// </summary>
        TempLicenseNo = (byte)'8',

        /// <summary>
        /// 民办非企业登记证书
        /// </summary>
        NoEnterpriseLicenseNo = (byte)'9',

        /// <summary>
        /// 其他证件
        /// </summary>
        OtherCard = (byte)'x',

        /// <summary>
        /// 主管部门批文
        /// </summary>
        SuperDepAgree = (byte)'a'
    }

    /// <summary>
    /// TFtdcFileBusinessCodeType是一个文件业务功能类型
    /// </summary>
    public enum EnumThostFileBusinessCodeType : byte
    {
        /// <summary>
        /// 其他
        /// </summary>
        Others = (byte)'0',

        /// <summary>
        /// 转账交易明细对账
        /// </summary>
        TransferDetails = (byte)'1',

        /// <summary>
        /// 客户账户状态对账
        /// </summary>
        CustAccStatus = (byte)'2',

        /// <summary>
        /// 账户类交易明细对账
        /// </summary>
        AccountTradeDetails = (byte)'3',

        /// <summary>
        /// 期货账户信息变更明细对账
        /// </summary>
        FutureAccountChangeInfoDetails = (byte)'4',

        /// <summary>
        /// 客户资金台账余额明细对账
        /// </summary>
        CustMoneyDetail = (byte)'5',

        /// <summary>
        /// 客户销户结息明细对账
        /// </summary>
        CustCancelAccountInfo = (byte)'6',

        /// <summary>
        /// 客户资金余额对账结果
        /// </summary>
        CustMoneyResult = (byte)'7',

        /// <summary>
        /// 其它对账异常结果文件
        /// </summary>
        OthersExceptionResult = (byte)'8',

        /// <summary>
        /// 客户结息净额明细
        /// </summary>
        CustInterestNetMoneyDetails = (byte)'9',

        /// <summary>
        /// 客户资金交收明细
        /// </summary>
        CustMoneySendAndReceiveDetails = (byte)'a',

        /// <summary>
        /// 法人存管银行资金交收汇总
        /// </summary>
        CorporationMoneyTotal = (byte)'b',

        /// <summary>
        /// 主体间资金交收汇总
        /// </summary>
        MainbodyMoneyTotal = (byte)'c',

        /// <summary>
        /// 总分平衡监管数据
        /// </summary>
        MainPartMonitorData = (byte)'d',

        /// <summary>
        /// 存管银行备付金余额
        /// </summary>
        PreparationMoney = (byte)'e',

        /// <summary>
        /// 协办存管银行资金监管数据
        /// </summary>
        BankMoneyMonitorData = (byte)'f'
    }

    /// <summary>
    /// TFtdcCashExchangeCodeType是一个汇钞标志类型
    /// </summary>
    public enum EnumThostCashExchangeCodeType : byte
    {
        /// <summary>
        /// 汇
        /// </summary>
        Exchange = (byte)'1',

        /// <summary>
        /// 钞
        /// </summary>
        Cash = (byte)'2'
    }

    /// <summary>
    /// TFtdcYesNoIndicatorType是一个是或否标识类型
    /// </summary>
    public enum EnumThostYesNoIndicatorType : byte
    {
        /// <summary>
        /// 是
        /// </summary>
        Yes = (byte)'0',

        /// <summary>
        /// 否
        /// </summary>
        No = (byte)'1'
    }

    /// <summary>
    /// TFtdcBanlanceTypeType是一个余额类型类型
    /// </summary>
    public enum EnumThostBanlanceTypeType : byte
    {
        /// <summary>
        /// 当前余额
        /// </summary>
        CurrentMoney = (byte)'0',

        /// <summary>
        /// 可用余额
        /// </summary>
        UsableMoney = (byte)'1',

        /// <summary>
        /// 可取余额
        /// </summary>
        FetchableMoney = (byte)'2',

        /// <summary>
        /// 冻结余额
        /// </summary>
        FreezeMoney = (byte)'3'
    }

    /// <summary>
    /// TFtdcGenderType是一个性别类型
    /// </summary>
    public enum EnumThostGenderType : byte
    {
        /// <summary>
        /// 未知状态
        /// </summary>
        Unknown = (byte)'0',

        /// <summary>
        /// 男
        /// </summary>
        Male = (byte)'1',

        /// <summary>
        /// 女
        /// </summary>
        Female = (byte)'2'
    }

    /// <summary>
    /// TFtdcFeePayFlagType是一个费用支付标志类型
    /// </summary>
    public enum EnumThostFeePayFlagType : byte
    {
        /// <summary>
        /// 由受益方支付费用
        /// </summary>
        BEN = (byte)'0',

        /// <summary>
        /// 由发送方支付费用
        /// </summary>
        OUR = (byte)'1',

        /// <summary>
        /// 由发送方支付发起的费用，受益方支付接受的费用
        /// </summary>
        SHA = (byte)'2'
    }

    /// <summary>
    /// TFtdcPassWordKeyTypeType是一个密钥类型类型
    /// </summary>
    public enum EnumThostPassWordKeyTypeType : byte
    {
        /// <summary>
        /// 交换密钥
        /// </summary>
        ExchangeKey = (byte)'0',

        /// <summary>
        /// 密码密钥
        /// </summary>
        PassWordKey = (byte)'1',

        /// <summary>
        /// MAC密钥
        /// </summary>
        MACKey = (byte)'2',

        /// <summary>
        /// 报文密钥
        /// </summary>
        MessageKey = (byte)'3'
    }

    /// <summary>
    /// TFtdcFBTPassWordTypeType是一个密码类型类型
    /// </summary>
    public enum EnumThostFBTPassWordTypeType : byte
    {
        /// <summary>
        /// 查询
        /// </summary>
        Query = (byte)'0',

        /// <summary>
        /// 取款
        /// </summary>
        Fetch = (byte)'1',

        /// <summary>
        /// 转帐
        /// </summary>
        Transfer = (byte)'2',

        /// <summary>
        /// 交易
        /// </summary>
        Trade = (byte)'3'
    }

    /// <summary>
    /// TFtdcFBTEncryModeType是一个加密方式类型
    /// </summary>
    public enum EnumThostFBTEncryModeType : byte
    {
        /// <summary>
        /// 不加密
        /// </summary>
        NoEncry = (byte)'0',

        /// <summary>
        /// DES
        /// </summary>
        DES = (byte)'1',

        /// <summary>
        /// 3DES
        /// </summary>
        DES3 = (byte)'2'
    }

    /// <summary>
    /// TFtdcBankRepealFlagType是一个银行冲正标志类型
    /// </summary>
    public enum EnumThostBankRepealFlagType : byte
    {
        /// <summary>
        /// 银行无需自动冲正
        /// </summary>
        BankNotNeedRepeal = (byte)'0',

        /// <summary>
        /// 银行待自动冲正
        /// </summary>
        BankWaitingRepeal = (byte)'1',

        /// <summary>
        /// 银行已自动冲正
        /// </summary>
        BankBeenRepealed = (byte)'2'
    }

    /// <summary>
    /// TFtdcBrokerRepealFlagType是一个期商冲正标志类型
    /// </summary>
    public enum EnumThostBrokerRepealFlagType : byte
    {
        /// <summary>
        /// 期商无需自动冲正
        /// </summary>
        BrokerNotNeedRepeal = (byte)'0',

        /// <summary>
        /// 期商待自动冲正
        /// </summary>
        BrokerWaitingRepeal = (byte)'1',

        /// <summary>
        /// 期商已自动冲正
        /// </summary>
        BrokerBeenRepealed = (byte)'2'
    }

    /// <summary>
    /// TFtdcInstitutionTypeType是一个机构类别类型
    /// </summary>
    public enum EnumThostInstitutionTypeType : byte
    {
        /// <summary>
        /// 银行
        /// </summary>
        Bank = (byte)'0',

        /// <summary>
        /// 期商
        /// </summary>
        Future = (byte)'1',

        /// <summary>
        /// 券商
        /// </summary>
        Store = (byte)'2'
    }

    /// <summary>
    /// TFtdcLastFragmentType是一个最后分片标志类型
    /// </summary>
    public enum EnumThostLastFragmentType : byte
    {
        /// <summary>
        /// 是最后分片
        /// </summary>
        Yes = (byte)'0',

        /// <summary>
        /// 不是最后分片
        /// </summary>
        No = (byte)'1'
    }

    /// <summary>
    /// TFtdcBankAccStatusType是一个银行账户状态类型
    /// </summary>
    public enum EnumThostBankAccStatusType : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = (byte)'0',

        /// <summary>
        /// 冻结
        /// </summary>
        Freeze = (byte)'1',

        /// <summary>
        /// 挂失
        /// </summary>
        ReportLoss = (byte)'2'
    }

    /// <summary>
    /// TFtdcMoneyAccountStatusType是一个资金账户状态类型
    /// </summary>
    public enum EnumThostMoneyAccountStatusType : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = (byte)'0',

        /// <summary>
        /// 销户
        /// </summary>
        Cancel = (byte)'1'
    }

    /// <summary>
    /// TFtdcManageStatusType是一个存管状态类型
    /// </summary>
    public enum EnumThostManageStatusType : byte
    {
        /// <summary>
        /// 指定存管
        /// </summary>
        Point = (byte)'0',

        /// <summary>
        /// 预指定
        /// </summary>
        PrePoint = (byte)'1',

        /// <summary>
        /// 撤销指定
        /// </summary>
        CancelPoint = (byte)'2'
    }

    /// <summary>
    /// TFtdcSystemTypeType是一个应用系统类型类型
    /// </summary>
    public enum EnumThostSystemTypeType : byte
    {
        /// <summary>
        /// 银期转帐
        /// </summary>
        FutureBankTransfer = (byte)'0',

        /// <summary>
        /// 银证转帐
        /// </summary>
        StockBankTransfer = (byte)'1',

        /// <summary>
        /// 第三方存管
        /// </summary>
        TheThirdPartStore = (byte)'2'
    }

    /// <summary>
    /// TFtdcTxnEndFlagType是一个银期转帐划转结果标志类型
    /// </summary>
    public enum EnumThostTxnEndFlagType : byte
    {
        /// <summary>
        /// 正常处理中
        /// </summary>
        NormalProcessing = (byte)'0',

        /// <summary>
        /// 成功结束
        /// </summary>
        Success = (byte)'1',

        /// <summary>
        /// 失败结束
        /// </summary>
        Failed = (byte)'2',

        /// <summary>
        /// 异常中
        /// </summary>
        Abnormal = (byte)'3',

        /// <summary>
        /// 已人工异常处理
        /// </summary>
        ManualProcessedForException = (byte)'4',

        /// <summary>
        /// 通讯异常 ，请人工处理
        /// </summary>
        CommuFailedNeedManualProcess = (byte)'5',

        /// <summary>
        /// 系统出错，请人工处理
        /// </summary>
        SysErrorNeedManualProcess = (byte)'6'
    }

    /// <summary>
    /// TFtdcProcessStatusType是一个银期转帐服务处理状态类型
    /// </summary>
    public enum EnumThostProcessStatusType : byte
    {
        /// <summary>
        /// 未处理
        /// </summary>
        NotProcess = (byte)'0',

        /// <summary>
        /// 开始处理
        /// </summary>
        StartProcess = (byte)'1',

        /// <summary>
        /// 处理完成
        /// </summary>
        Finished = (byte)'2'
    }

    /// <summary>
    /// TFtdcCustTypeType是一个客户类型类型
    /// </summary>
    public enum EnumThostCustTypeType : byte
    {
        /// <summary>
        /// 自然人
        /// </summary>
        Person = (byte)'0',

        /// <summary>
        /// 机构户
        /// </summary>
        Institution = (byte)'1'
    }

    /// <summary>
    /// TFtdcFBTTransferDirectionType是一个银期转帐方向类型
    /// </summary>
    public enum EnumThostFBTTransferDirectionType : byte
    {
        /// <summary>
        /// 入金，银行转期货
        /// </summary>
        FromBankToFuture = (byte)'1',

        /// <summary>
        /// 出金，期货转银行
        /// </summary>
        FromFutureToBank = (byte)'2'
    }

    /// <summary>
    /// TFtdcOpenOrDestroyType是一个开销户类别类型
    /// </summary>
    public enum EnumThostOpenOrDestroyType : byte
    {
        /// <summary>
        /// 开户
        /// </summary>
        Open = (byte)'1',

        /// <summary>
        /// 销户
        /// </summary>
        Destroy = (byte)'0'
    }

    /// <summary>
    /// TFtdcAvailabilityFlagType是一个有效标志类型
    /// </summary>
    public enum EnumThostAvailabilityFlagType : byte
    {
        /// <summary>
        /// 未确认
        /// </summary>
        Invalid = (byte)'0',

        /// <summary>
        /// 有效
        /// </summary>
        Valid = (byte)'1',

        /// <summary>
        /// 冲正
        /// </summary>
        Repeal = (byte)'2'
    }

    /// <summary>
    /// TFtdcOrganTypeType是一个机构类型类型
    /// </summary>
    public enum EnumThostOrganTypeType : byte
    {
        /// <summary>
        /// 银行代理
        /// </summary>
        Bank = (byte)'1',

        /// <summary>
        /// 交易前置
        /// </summary>
        Future = (byte)'2',

        /// <summary>
        /// 银期转帐平台管理
        /// </summary>
        PlateForm = (byte)'9'
    }

    /// <summary>
    /// TFtdcOrganLevelType是一个机构级别类型
    /// </summary>
    public enum EnumThostOrganLevelType : byte
    {
        /// <summary>
        /// 银行总行或期商总部
        /// </summary>
        HeadQuarters = (byte)'1',

        /// <summary>
        /// 银行分中心或期货公司营业部
        /// </summary>
        Branch = (byte)'2'
    }

    /// <summary>
    /// TFtdcProtocalIDType是一个协议类型类型
    /// </summary>
    public enum EnumThostProtocalIDType : byte
    {
        /// <summary>
        /// 期商协议
        /// </summary>
        FutureProtocal = (byte)'0',

        /// <summary>
        /// 工行协议
        /// </summary>
        ICBCProtocal = (byte)'1',

        /// <summary>
        /// 农行协议
        /// </summary>
        ABCProtocal = (byte)'2',

        /// <summary>
        /// 中国银行协议
        /// </summary>
        CBCProtocal = (byte)'3',

        /// <summary>
        /// 建行协议
        /// </summary>
        CCBProtocal = (byte)'4',

        /// <summary>
        /// 交行协议
        /// </summary>
        BOCOMProtocal = (byte)'5',

        /// <summary>
        /// 银期转帐平台协议
        /// </summary>
        FBTPlateFormProtocal = (byte)'X'
    }

    /// <summary>
    /// TFtdcConnectModeType是一个套接字连接方式类型
    /// </summary>
    public enum EnumThostConnectModeType : byte
    {
        /// <summary>
        /// 短连接
        /// </summary>
        ShortConnect = (byte)'0',

        /// <summary>
        /// 长连接
        /// </summary>
        LongConnect = (byte)'1'
    }

    /// <summary>
    /// TFtdcSyncModeType是一个套接字通信方式类型
    /// </summary>
    public enum EnumThostSyncModeType : byte
    {
        /// <summary>
        /// 异步
        /// </summary>
        ASync = (byte)'0',

        /// <summary>
        /// 同步
        /// </summary>
        Sync = (byte)'1'
    }

    /// <summary>
    /// TFtdcBankAccTypeType是一个银行帐号类型类型
    /// </summary>
    public enum EnumThostBankAccTypeType : byte
    {
        /// <summary>
        /// 银行存折
        /// </summary>
        BankBook = (byte)'1',

        /// <summary>
        /// 储蓄卡
        /// </summary>
        SavingCard = (byte)'2',

        /// <summary>
        /// 信用卡
        /// </summary>
        CreditCard = (byte)'3'
    }

    /// <summary>
    /// TFtdcFutureAccTypeType是一个期货公司帐号类型类型
    /// </summary>
    public enum EnumThostFutureAccTypeType : byte
    {
        /// <summary>
        /// 银行存折
        /// </summary>
        BankBook = (byte)'1',

        /// <summary>
        /// 储蓄卡
        /// </summary>
        SavingCard = (byte)'2',

        /// <summary>
        /// 信用卡
        /// </summary>
        CreditCard = (byte)'3'
    }

    /// <summary>
    /// TFtdcOrganStatusType是一个接入机构状态类型
    /// </summary>
    public enum EnumThostOrganStatusType : byte
    {
        /// <summary>
        /// 启用
        /// </summary>
        Ready = (byte)'0',

        /// <summary>
        /// 签到
        /// </summary>
        CheckIn = (byte)'1',

        /// <summary>
        /// 签退
        /// </summary>
        CheckOut = (byte)'2',

        /// <summary>
        /// 对帐文件到达
        /// </summary>
        CheckFileArrived = (byte)'3',

        /// <summary>
        /// 对帐
        /// </summary>
        CheckDetail = (byte)'4',

        /// <summary>
        /// 日终清理
        /// </summary>
        DayEndClean = (byte)'5',

        /// <summary>
        /// 注销
        /// </summary>
        Invalid = (byte)'9'
    }

    /// <summary>
    /// TFtdcCCBFeeModeType是一个建行收费模式类型
    /// </summary>
    public enum EnumThostCCBFeeModeType : byte
    {
        /// <summary>
        /// 按金额扣收
        /// </summary>
        ByAmount = (byte)'1',

        /// <summary>
        /// 按月扣收
        /// </summary>
        ByMonth = (byte)'2'
    }

    /// <summary>
    /// TFtdcCommApiTypeType是一个通讯API类型类型
    /// </summary>
    public enum EnumThostCommApiTypeType : byte
    {
        /// <summary>
        /// 客户端
        /// </summary>
        Client = (byte)'1',

        /// <summary>
        /// 服务端
        /// </summary>
        Server = (byte)'2',

        /// <summary>
        /// 交易系统的UserApi
        /// </summary>
        UserApi = (byte)'3'
    }

    /// <summary>
    /// TFtdcLinkStatusType是一个连接状态类型
    /// </summary>
    public enum EnumThostLinkStatusType : byte
    {
        /// <summary>
        /// 已经连接
        /// </summary>
        Connected = (byte)'1',

        /// <summary>
        /// 没有连接
        /// </summary>
        Disconnected = (byte)'2'
    }

    /// <summary>
    /// TFtdcPwdFlagType是一个密码核对标志类型
    /// </summary>
    public enum EnumThostPwdFlagType : byte
    {
        /// <summary>
        /// 不核对
        /// </summary>
        NoCheck = (byte)'0',

        /// <summary>
        /// 明文核对
        /// </summary>
        BlankCheck = (byte)'1',

        /// <summary>
        /// 密文核对
        /// </summary>
        EncryptCheck = (byte)'2'
    }

    /// <summary>
    /// TFtdcSecuAccTypeType是一个期货帐号类型类型
    /// </summary>
    public enum EnumThostSecuAccTypeType : byte
    {
        /// <summary>
        /// 资金帐号
        /// </summary>
        AccountID = (byte)'1',

        /// <summary>
        /// 资金卡号
        /// </summary>
        CardID = (byte)'2',

        /// <summary>
        /// 上海股东帐号
        /// </summary>
        SHStockholderID = (byte)'3',

        /// <summary>
        /// 深圳股东帐号
        /// </summary>
        SZStockholderID = (byte)'4'
    }

    /// <summary>
    /// TFtdcTransferStatusType是一个转账交易状态类型
    /// </summary>
    public enum EnumThostTransferStatusType : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = (byte)'0',

        /// <summary>
        /// 被冲正
        /// </summary>
        Repealed = (byte)'1'
    }

    /// <summary>
    /// TFtdcSponsorTypeType是一个发起方类型
    /// </summary>
    public enum EnumThostSponsorTypeType : byte
    {
        /// <summary>
        /// 期商
        /// </summary>
        Broker = (byte)'0',

        /// <summary>
        /// 银行
        /// </summary>
        Bank = (byte)'1'
    }

    /// <summary>
    /// TFtdcReqRspTypeType是一个请求响应类别类型
    /// </summary>
    public enum EnumThostReqRspTypeType : byte
    {
        /// <summary>
        /// 请求
        /// </summary>
        Request = (byte)'0',

        /// <summary>
        /// 响应
        /// </summary>
        Response = (byte)'1'
    }

    /// <summary>
    /// TFtdcFBTUserEventTypeType是一个银期转帐用户事件类型类型
    /// </summary>
    public enum EnumThostFBTUserEventTypeType : byte
    {
        /// <summary>
        /// 签到
        /// </summary>
        SignIn = (byte)'0',

        /// <summary>
        /// 银行转期货
        /// </summary>
        FromBankToFuture = (byte)'1',

        /// <summary>
        /// 期货转银行
        /// </summary>
        FromFutureToBank = (byte)'2',

        /// <summary>
        /// 开户
        /// </summary>
        OpenAccount = (byte)'3',

        /// <summary>
        /// 销户
        /// </summary>
        CancelAccount = (byte)'4',

        /// <summary>
        /// 变更银行账户
        /// </summary>
        ChangeAccount = (byte)'5',

        /// <summary>
        /// 冲正银行转期货
        /// </summary>
        RepealFromBankToFuture = (byte)'6',

        /// <summary>
        /// 冲正期货转银行
        /// </summary>
        RepealFromFutureToBank = (byte)'7',

        /// <summary>
        /// 查询银行账户
        /// </summary>
        QueryBankAccount = (byte)'8',

        /// <summary>
        /// 查询期货账户
        /// </summary>
        QueryFutureAccount = (byte)'9',

        /// <summary>
        /// 签退
        /// </summary>
        SignOut = (byte)'A',

        /// <summary>
        /// 密钥同步
        /// </summary>
        SyncKey = (byte)'B',

        /// <summary>
        /// 预约开户
        /// </summary>
        ReserveOpenAccount = (byte)'C',
        /// <summary>
        /// 撤销预约开户
        /// </summary>
        CancelReserveOpenAccount = (byte)'D',
        /// <summary>
        /// 预约开户确认
        /// </summary>
        ReserveOpenAccountConfirm = (byte)'E',

        /// <summary>
        /// 其他
        /// </summary>
        Other = (byte)'Z'
    }

    /// <summary>
    /// TFtdcNotifyClassType是一个风险通知类型类型
    /// </summary>
    public enum EnumThostNotifyClassType : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        NOERROR = (byte)'0',

        /// <summary>
        /// 警示
        /// </summary>
        Warn = (byte)'1',

        /// <summary>
        /// 追保
        /// </summary>
        Call = (byte)'2',

        /// <summary>
        /// 强平
        /// </summary>
        Force = (byte)'3',

        /// <summary>
        /// 穿仓
        /// </summary>
        CHUANCANG = (byte)'4',

        /// <summary>
        /// 异常
        /// </summary>
        Exception = (byte)'5'
    }

    /// <summary>
    /// TFtdcForceCloseTypeType是一个强平单类型类型
    /// </summary>
    public enum EnumThostForceCloseTypeType : byte
    {
        /// <summary>
        /// 手工强平
        /// </summary>
        Manual = (byte)'0',

        /// <summary>
        /// 单一投资者辅助强平
        /// </summary>
        Single = (byte)'1',

        /// <summary>
        /// 批量投资者辅助强平
        /// </summary>
        Group = (byte)'2'
    }

    /// <summary>
    /// TFtdcRiskNotifyMethodType是一个风险通知途径类型
    /// </summary>
    public enum EnumThostRiskNotifyMethodType : byte
    {
        /// <summary>
        /// 系统通知
        /// </summary>
        System = (byte)'0',

        /// <summary>
        /// 短信通知
        /// </summary>
        SMS = (byte)'1',

        /// <summary>
        /// 邮件通知
        /// </summary>
        EMail = (byte)'2',

        /// <summary>
        /// 人工通知
        /// </summary>
        Manual = (byte)'3'
    }

    /// <summary>
    /// TFtdcRiskNotifyStatusType是一个风险通知状态类型
    /// </summary>
    public enum EnumThostRiskNotifyStatusType : byte
    {
        /// <summary>
        /// 未生成
        /// </summary>
        NotGen = (byte)'0',

        /// <summary>
        /// 已生成未发送
        /// </summary>
        Generated = (byte)'1',

        /// <summary>
        /// 发送失败
        /// </summary>
        SendError = (byte)'2',

        /// <summary>
        /// 已发送未接收
        /// </summary>
        SendOk = (byte)'3',

        /// <summary>
        /// 已接收未确认
        /// </summary>
        Received = (byte)'4',

        /// <summary>
        /// 已确认
        /// </summary>
        Confirmed = (byte)'5'
    }

    /// <summary>
    /// TFtdcRiskUserEventType是一个风控用户操作事件类型
    /// </summary>
    public enum EnumThostRiskUserEventType : byte
    {
        /// <summary>
        /// 导出数据
        /// </summary>
        ExportData = (byte)'0'
    }

    /// <summary>
    /// TFtdcConditionalOrderSortTypeType是一个条件单索引条件类型
    /// </summary>
    public enum EnumThostConditionalOrderSortTypeType : byte
    {
        /// <summary>
        /// 使用最新价升序
        /// </summary>
        LastPriceAsc = (byte)'0',

        /// <summary>
        /// 使用最新价降序
        /// </summary>
        LastPriceDesc = (byte)'1',

        /// <summary>
        /// 使用卖价升序
        /// </summary>
        AskPriceAsc = (byte)'2',

        /// <summary>
        /// 使用卖价降序
        /// </summary>
        AskPriceDesc = (byte)'3',

        /// <summary>
        /// 使用买价升序
        /// </summary>
        BidPriceAsc = (byte)'4',

        /// <summary>
        /// 使用买价降序
        /// </summary>
        BidPriceDesc = (byte)'5'
    }

    /// <summary>
    /// TFtdcSendTypeType是一个报送状态类型
    /// </summary>
    public enum EnumThostSendTypeType : byte
    {
        /// <summary>
        /// 未发送
        /// </summary>
        NoSend = (byte)'0',

        /// <summary>
        /// 已发送
        /// </summary>
        Sended = (byte)'1',

        /// <summary>
        /// 已生成
        /// </summary>
        Generated = (byte)'2',

        /// <summary>
        /// 报送失败
        /// </summary>
        SendFail = (byte)'3',

        /// <summary>
        /// 接收成功
        /// </summary>
        Success = (byte)'4',

        /// <summary>
        /// 接收失败
        /// </summary>
        Fail = (byte)'5',

        /// <summary>
        /// 取消报送
        /// </summary>
        Cancel = (byte)'6'
    }

    /// <summary>
    /// TFtdcClientIDStatusType是一个交易编码状态类型
    /// </summary>
    public enum EnumThostClientIDStatusType : byte
    {
        /// <summary>
        /// 未申请
        /// </summary>
        NoApply = (byte)'1',

        /// <summary>
        /// 已提交申请
        /// </summary>
        Submited = (byte)'2',

        /// <summary>
        /// 已发送申请
        /// </summary>
        Sended = (byte)'3',

        /// <summary>
        /// 完成
        /// </summary>
        Success = (byte)'4',

        /// <summary>
        /// 拒绝
        /// </summary>
        Refuse = (byte)'5',

        /// <summary>
        /// 已撤销编码
        /// </summary>
        Cancel = (byte)'6'
    }

    /// <summary>
    /// TFtdcQuestionTypeType是一个特有信息类型类型
    /// </summary>
    public enum EnumThostQuestionTypeType : byte
    {
        /// <summary>
        /// 单选
        /// </summary>
        Radio = (byte)'1',

        /// <summary>
        /// 多选
        /// </summary>
        Option = (byte)'2',

        /// <summary>
        /// 填空
        /// </summary>
        Blank = (byte)'3'
    }

    /// <summary>
    /// TFtdcProcessTypeType是一个流程功能类型类型
    /// </summary>
    public enum EnumThostProcessTypeType : byte
    {
        /// <summary>
        /// 申请交易编码
        /// </summary>
        ApplyTradingCode = (byte)'1',

        /// <summary>
        /// 撤销交易编码
        /// </summary>
        CancelTradingCode = (byte)'2',

        /// <summary>
        /// 修改身份信息
        /// </summary>
        ModifyIDCard = (byte)'3',

        /// <summary>
        /// 修改一般信息
        /// </summary>
        ModifyNoIDCard = (byte)'4',

        /// <summary>
        /// 交易所开户报备
        /// </summary>
        ExchOpenBak = (byte)'5',

        /// <summary>
        /// 交易所销户报备
        /// </summary>
        ExchCancelBak = (byte)'6'
    }

    /// <summary>
    /// TFtdcBusinessTypeType是一个业务类型类型
    /// </summary>
    public enum EnumThostBusinessTypeType : byte
    {
        /// <summary>
        /// 请求
        /// </summary>
        Request = (byte)'1',

        /// <summary>
        /// 应答
        /// </summary>
        Response = (byte)'2',

        /// <summary>
        /// 通知
        /// </summary>
        Notice = (byte)'3'
    }

    /// <summary>
    /// TFtdcCfmmcReturnCodeType是一个监控中心返回码类型
    /// </summary>
    public enum EnumThostCfmmcReturnCodeType : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = (byte)'0',

        /// <summary>
        /// 该客户已经有流程在处理中
        /// </summary>
        Working = (byte)'1',

        /// <summary>
        /// 监控中客户资料检查失败
        /// </summary>
        InfoFail = (byte)'2',

        /// <summary>
        /// 监控中实名制检查失败
        /// </summary>
        IDCardFail = (byte)'3',

        /// <summary>
        /// 其他错误
        /// </summary>
        OtherFail = (byte)'4'
    }

    /// <summary>
    /// TFtdcClientTypeType是一个客户类型类型
    /// </summary>
    public enum EnumThostClientTypeType : byte
    {
        /// <summary>
        /// 所有
        /// </summary>
        All = (byte)'0',

        /// <summary>
        /// 个人
        /// </summary>
        Person = (byte)'1',

        /// <summary>
        /// 单位
        /// </summary>
        Company = (byte)'2'
    }

    /// <summary>
    /// TFtdcExchangeIDTypeType是一个交易所编号类型
    /// </summary>
    public enum EnumThostExchangeIDTypeType : byte
    {
        /// <summary>
        /// 上海期货交易所
        /// </summary>
        SHFE = (byte)'S',

        /// <summary>
        /// 郑州商品交易所
        /// </summary>
        CZCE = (byte)'Z',

        /// <summary>
        /// 大连商品交易所
        /// </summary>
        DCE = (byte)'D',

        /// <summary>
        /// 中国金融期货交易所
        /// </summary>
        CFFEX = (byte)'J'
    }

    /// <summary>
    /// TFtdcExClientIDTypeType是一个交易编码类型类型
    /// </summary>
    public enum EnumThostExClientIDTypeType : byte
    {
        /// <summary>
        /// 套保
        /// </summary>
        Hedge = (byte)'1',

        /// <summary>
        /// 套利
        /// </summary>
        Arbitrage = (byte)'2',

        /// <summary>
        /// 投机
        /// </summary>
        Speculation = (byte)'3'
    }

    /// <summary>
    /// TFtdcUpdateFlagType是一个更新状态类型
    /// </summary>
    public enum EnumThostUpdateFlagType : byte
    {
        /// <summary>
        /// 未更新
        /// </summary>
        NoUpdate = (byte)'0',

        /// <summary>
        /// 更新全部信息成功
        /// </summary>
        Success = (byte)'1',

        /// <summary>
        /// 更新全部信息失败
        /// </summary>
        Fail = (byte)'2',

        /// <summary>
        /// 更新交易编码成功
        /// </summary>
        TCSuccess = (byte)'3',

        /// <summary>
        /// 更新交易编码失败
        /// </summary>
        TCFail = (byte)'4',

        /// <summary>
        /// 已丢弃
        /// </summary>
        Cancel = (byte)'5'
    }

    /// <summary>
    /// TFtdcApplyOperateIDType是一个申请动作类型
    /// </summary>
    public enum EnumThostApplyOperateIDType : byte
    {
        /// <summary>
        /// 开户
        /// </summary>
        OpenInvestor = (byte)'1',

        /// <summary>
        /// 修改身份信息
        /// </summary>
        ModifyIDCard = (byte)'2',

        /// <summary>
        /// 修改一般信息
        /// </summary>
        ModifyNoIDCard = (byte)'3',

        /// <summary>
        /// 申请交易编码
        /// </summary>
        ApplyTradingCode = (byte)'4',

        /// <summary>
        /// 撤销交易编码
        /// </summary>
        CancelTradingCode = (byte)'5',

        /// <summary>
        /// 销户
        /// </summary>
        CancelInvestor = (byte)'6'
    }

    /// <summary>
    /// TFtdcApplyStatusIDType是一个申请状态类型
    /// </summary>
    public enum EnumThostApplyStatusIDType : byte
    {
        /// <summary>
        /// 未补全
        /// </summary>
        NoComplete = (byte)'1',

        /// <summary>
        /// 已提交
        /// </summary>
        Submited = (byte)'2',

        /// <summary>
        /// 已审核
        /// </summary>
        Checked = (byte)'3',

        /// <summary>
        /// 已拒绝
        /// </summary>
        Refused = (byte)'4',

        /// <summary>
        /// 已删除
        /// </summary>
        Deleted = (byte)'5'
    }

    /// <summary>
    /// TFtdcSendMethodType是一个发送方式类型
    /// </summary>
    public enum EnumThostSendMethodType : byte
    {
        /// <summary>
        /// 电子发送
        /// </summary>
        ByAPI = (byte)'1',

        /// <summary>
        /// 文件发送
        /// </summary>
        ByFile = (byte)'2'
    }

    /// <summary>
    /// TFtdcEventModeType是一个操作方法类型
    /// </summary>
    public enum EnumThostEventModeType : byte
    {
        /// <summary>
        /// 增加
        /// </summary>
        ADD = (byte)'1',

        /// <summary>
        /// 修改
        /// </summary>
        UPDATE = (byte)'2',

        /// <summary>
        /// 删除
        /// </summary>
        DELETE = (byte)'3',

        /// <summary>
        /// 复核
        /// </summary>
        CHECK = (byte)'4'
    }

    /// <summary>
    /// TFtdcUOAAutoSendType是一个统一开户申请自动发送类型
    /// </summary>
    public enum EnumThostUOAAutoSendType : byte
    {
        /// <summary>
        /// 自动发送并接收
        /// </summary>
        ASR = (byte)'1',

        /// <summary>
        /// 自动发送，不自动接收
        /// </summary>
        ASNR = (byte)'2',

        /// <summary>
        /// 不自动发送，自动接收
        /// </summary>
        NSAR = (byte)'3',

        /// <summary>
        /// 不自动发送，也不自动接收
        /// </summary>
        NSR = (byte)'4'
    }

    /// <summary>
    /// TFtdcFlowIDType是一个流程ID类型
    /// </summary>
    public enum EnumThostFlowIDType : byte
    {
        /// <summary>
        /// 投资者对应投资者组设置
        /// </summary>
        InvestorGroupFlow = (byte)'1'
    }

    /// <summary>
    /// TFtdcCheckLevelType是一个复核级别类型
    /// </summary>
    public enum EnumThostCheckLevelType : byte
    {
        /// <summary>
        /// 零级复核
        /// </summary>
        Zero = (byte)'0',

        /// <summary>
        /// 一级复核
        /// </summary>
        One = (byte)'1',

        /// <summary>
        /// 二级复核
        /// </summary>
        Two = (byte)'2'
    }

    /// <summary>
    /// TFtdcCheckStatusType是一个复核级别类型
    /// </summary>
    public enum EnumThostCheckStatusType : byte
    {
        /// <summary>
        /// 未复核
        /// </summary>
        Init = (byte)'0',

        /// <summary>
        /// 复核中
        /// </summary>
        Checking = (byte)'1',

        /// <summary>
        /// 已复核
        /// </summary>
        Checked = (byte)'2',

        /// <summary>
        /// 拒绝
        /// </summary>
        Refuse = (byte)'3',

        /// <summary>
        /// 作废
        /// </summary>
        Cancel = (byte)'4'
    }

    /// <summary>
    /// TFtdcUsedStatusType是一个生效状态类型
    /// </summary>
    public enum EnumThostUsedStatusType : byte
    {
        /// <summary>
        /// 未生效
        /// </summary>
        Unused = (byte)'0',

        /// <summary>
        /// 已生效
        /// </summary>
        Used = (byte)'1',

        /// <summary>
        /// 生效失败
        /// </summary>
        Fail = (byte)'2'
    }

    /// <summary>
    /// TFtdcBankAcountOriginType是一个账户来源类型
    /// </summary>
    public enum EnumThostBankAcountOriginType : byte
    {
        /// <summary>
        /// 手工录入
        /// </summary>
        ByAccProperty = (byte)'0',

        /// <summary>
        /// 银期转账
        /// </summary>
        ByFBTransfer = (byte)'1'
    }

    /// <summary>
    /// TFtdcMonthBillTradeSumType是一个结算单月报成交汇总方式类型
    /// </summary>
    public enum EnumThostMonthBillTradeSumType : byte
    {
        /// <summary>
        /// 同日同合约
        /// </summary>
        ByInstrument = (byte)'0',

        /// <summary>
        /// 同日同合约同价格
        /// </summary>
        ByDayInsPrc = (byte)'1',

        /// <summary>
        /// 同合约
        /// </summary>
        ByDayIns = (byte)'2'
    }

    /// <summary>
    /// TFtdcFBTTradeCodeEnumType是一个银期交易代码枚举类型
    /// </summary>
    public enum EnumThostFBTTradeCodeEnumType : byte
    {
        /// <summary>
        /// 银行发起银行转期货
        /// </summary>
        BankLaunchBankToBroker_102001 = (byte)'1',//'102001',

        /// <summary>
        /// 期货发起银行转期货
        /// </summary>
        BrokerLaunchBankToBroker_202001 = (byte)'2',//'202001',

        /// <summary>
        /// 银行发起期货转银行
        /// </summary>
        BankLaunchBrokerToBank_102002 = (byte)'3',//'102002',

        /// <summary>
        /// 期货发起期货转银行
        /// </summary>
        BrokerLaunchBrokerToBank_202002 = (byte)'4'//'202002'
    }

    /// <summary>
    /// TFtdcOTPTypeType是一个动态令牌类型类型
    /// </summary>
    public enum EnumThostOTPTypeType : byte
    {
        /// <summary>
        /// 无动态令牌
        /// </summary>
        NONE = (byte)'0',

        /// <summary>
        /// 时间令牌
        /// </summary>
        TOTP = (byte)'1'
    }

    /// <summary>
    /// TFtdcOTPStatusType是一个动态令牌状态类型
    /// </summary>
    public enum EnumThostOTPStatusType : byte
    {
        /// <summary>
        /// 未使用
        /// </summary>
        Unused = (byte)'0',

        /// <summary>
        /// 已使用
        /// </summary>
        Used = (byte)'1',

        /// <summary>
        /// 注销
        /// </summary>
        Disuse = (byte)'2'
    }

    /// <summary>
    /// TFtdcBrokerUserTypeType是一个经济公司用户类型类型
    /// </summary>
    public enum EnumThostBrokerUserTypeType : byte
    {
        /// <summary>
        /// 投资者
        /// </summary>
        Investor = (byte)'1'
    }

    /// <summary>
    /// TFtdcFutureTypeType是一个期货类型类型
    /// </summary>
    public enum EnumThostFutureTypeType : byte
    {
        /// <summary>
        /// 商品期货
        /// </summary>
        Commodity = (byte)'1',

        /// <summary>
        /// 金融期货
        /// </summary>
        Financial = (byte)'2'
    }

    /// <summary>
    /// TFtdcFundEventTypeType是一个资金管理操作类型类型
    /// </summary>
    public enum EnumThostFundEventTypeType : byte
    {
        /// <summary>
        /// 转账限额
        /// </summary>
        Restriction = (byte)'0',

        /// <summary>
        /// 当日转账限额
        /// </summary>
        TodayRestriction = (byte)'1',

        /// <summary>
        /// 期商流水
        /// </summary>
        Transfer = (byte)'2',

        /// <summary>
        /// 资金冻结
        /// </summary>
        Credit = (byte)'3',

        /// <summary>
        /// 投资者可提资金比例
        /// </summary>
        InvestorWithdrawAlm = (byte)'4',

        /// <summary>
        /// 单个银行帐户转账限额
        /// </summary>
        BankRestriction = (byte)'5'
    }

    /// <summary>
    /// TFtdcAccountSourceTypeType是一个资金账户来源类型
    /// </summary>
    public enum EnumThostAccountSourceTypeType : byte
    {
        /// <summary>
        /// 银期同步
        /// </summary>
        FBTransfer = (byte)'0',

        /// <summary>
        /// 手工录入
        /// </summary>
        ManualEntry = (byte)'1'
    }

    /// <summary>
    /// TFtdcCodeSourceTypeType是一个交易编码来源类型
    /// </summary>
    public enum EnumThostCodeSourceTypeType : byte
    {
        /// <summary>
        /// 统一开户
        /// </summary>
        UnifyAccount = (byte)'0',

        /// <summary>
        /// 手工录入
        /// </summary>
        ManualEntry = (byte)'1'
    }

    /// <summary>
    /// TFtdcUserRangeType是一个操作员范围类型
    /// </summary>
    public enum EnumThostUserRangeType : byte
    {
        /// <summary>
        /// 所有
        /// </summary>
        All = (byte)'0',

        /// <summary>
        /// 单一操作员
        /// </summary>
        Single = (byte)'1'
    }

    /// <summary>
    /// TFtdcByGroupType是一个交易统计表按客户统计方式类型
    /// </summary>
    public enum EnumThostByGroupType : byte
    {
        /// <summary>
        /// 按投资者统计
        /// </summary>
        Investor = (byte)'2',

        /// <summary>
        /// 按类统计
        /// </summary>
        Group = (byte)'1'
    }

    /// <summary>
    /// TFtdcTradeSumStatModeType是一个交易统计表按范围统计方式类型
    /// </summary>
    public enum EnumThostTradeSumStatModeType : byte
    {
        /// <summary>
        /// 按合约统计
        /// </summary>
        Instrument = (byte)'1',

        /// <summary>
        /// 按产品统计
        /// </summary>
        Product = (byte)'2',

        /// <summary>
        /// 按交易所统计
        /// </summary>
        Exchange = (byte)'3'
    }

    /// <summary>
    /// 
    /// </summary>
    public enum EnumThostBoolType : int
    {
        /// <summary>
        /// 
        /// </summary>
        No = 0,
        /// <summary>
        /// 
        /// </summary>
        Yes = 1
    }

    /// <summary>
    /// TFtdcCloseDealTypeType是一个平仓处理类型类型
    /// </summary>
    public enum EnumThostCloseDealTypeType : byte
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = (byte)'0',

        /// <summary>
        /// 投机平仓优先
        /// </summary>
        SpecFirst = (byte)'1'
    }

    /// <summary>
    /// TThostFtdcMortgageFundUseRangeType
    /// </summary>
    public enum EnumThostMortgageFundUseRangeType : byte
    {
        /// <summary>
        /// 不能使用
        /// </summary>
        None = (byte)'0',

        /// <summary>
        /// 用于保证金
        /// </summary>
        Margin = (byte)'1',

        /// <summary>
        /// 用于手续费、盈亏、保证金
        /// </summary>
        All = (byte)'2'
    }

    /// <summary>
    /// TFtdcMaxMarginSideAlgorithmType是一个大额单边保证金算法类型
    /// </summary>
    public enum EnumThostMaxMarginSideAlgorithmType : byte
    {
        /// <summary>
        /// 不使用大额单边保证金算法
        /// </summary>
        NO = (byte)'0',

        /// <summary>
        /// 使用大额单边保证金算法
        /// </summary>
        YES = (byte)'1'
    }

    /// <summary>
    /// TFtdcOptionsTypeType是一个期权类型类型
    /// </summary>
    public enum EnumThostOptionsTypeType : byte
    {
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
    /// TFtdcCombinationTypeType是一个组合类型类型
    /// </summary>
    public enum EnumThostCombinationTypeType : byte
    {
        /// <summary>
		/// 期货组合
		/// </summary>
		Future = (byte)'0',

        /// <summary>
        /// 垂直价差BUL
        /// </summary>
        BUL = (byte)'1',

        /// <summary>
        /// 垂直价差BER
        /// </summary>
        BER = (byte)'2',

        /// <summary>
        /// 跨式组合
        /// </summary>
        STD = (byte)'3',

        /// <summary>
        /// 宽跨式组合
        /// </summary>
        STG = (byte)'4',

        /// <summary>
        /// 备兑组合
        /// </summary>
        PRT = (byte)'5'
    }

    /// <summary>
    /// TFtdcTradeSourceType是一个成交来源类型
    /// </summary>
    public enum EnumThostTradeSourceType : byte
    {
        /// <summary>
        /// 来自交易所普通回报
        /// </summary>
        NORMAL = (byte)'0',

        /// <summary>
        /// 来自查询
        /// </summary>
        QUERY = (byte)'1'
    }

    /// <summary>
    /// TFtdcValueMethodType是一个取值方式类型
    /// </summary>
    public enum EnumThostValueMethodType : byte
    {
        /// <summary>
        /// 按绝对值
        /// </summary>
        Absolute = (byte)'0',

        /// <summary>
        /// 按比率
        /// </summary>
        Ratio = (byte)'1'
    }

    /// <summary>
    /// TFtdcExecOrderPositionFlagType是一个期权行权后是否保留期货头寸的标记类型
    /// </summary>
    public enum EnumThostExecOrderPositionFlagType : byte
    {
        /// <summary>
        /// 保留
        /// </summary>
        Reserve = (byte)'0',

        /// <summary>
        /// 不保留
        /// </summary>
        UnReserve = (byte)'1'
    }

    /// <summary>
    /// TFtdcExecOrderCloseFlagType是一个期权行权后生成的头寸是否自动平仓类型
    /// </summary>
    public enum EnumThostExecOrderCloseFlagType : byte
    {
        /// <summary>
        /// 自动平仓
        /// </summary>
        AutoClose = (byte)'0',

        /// <summary>
        /// 免于自动平仓
        /// </summary>
        NotToClose = (byte)'1'
    }

    /// <summary>
	/// TFtdcExecResultType是一个执行结果类型
	/// </summary>
	public enum EnumThostExecResultType : byte
    {
        /// <summary>
		/// 没有执行
		/// </summary>
		NoExec = (byte)'n',

        /// <summary>
		/// 已经取消
		/// </summary>
		Canceled = (byte)'c',

        /// <summary>
        /// 执行成功
        /// </summary>
        OK = (byte)'0',

        /// <summary>
        /// 期权持仓不够
        /// </summary>
        NoPosition = (byte)'1',

        /// <summary>
		/// 资金不够
		/// </summary>
		NoDeposit = (byte)'2',

        /// <summary>
        /// 会员不存在
        /// </summary>
        NoParticipant = (byte)'3',

        /// <summary>
        /// 客户不存在
        /// </summary>
        NoClient = (byte)'4',

        /// <summary>
        /// 合约不存在
        /// </summary>
        NoInstrument = (byte)'6',

        /// <summary>
        /// 没有执行权限
        /// </summary>
        NoRight = (byte)'7',

        /// <summary>
        /// 不合理的数量
        /// </summary>
        InvalidVolume = (byte)'8',

        /// <summary>
		/// 没有足够的历史成交
		/// </summary>
		NoEnoughHistoryTrade = (byte)'9',

        /// <summary>
		/// 未知
		/// </summary>
		Unknown = (byte)'a'
    }

    /// <summary>
    /// TFtdcActionTypeType是一个执行类型类型
    /// </summary>
    public enum EnumThostActionTypeType : byte
    {
        /// <summary>
        /// 执行
        /// </summary>
        Exec = (byte)'1',

        /// <summary>
        /// 放弃
        /// </summary>
        Abandon = (byte)'2'
    }

    /// <summary>
    /// TFtdcForQuoteStatusType是一个询价状态类型
    /// </summary>
    public enum EnumThostForQuoteStatusType : byte
    {
        /// <summary>
        /// 已经提交
        /// </summary>
        Submitted = (byte)'a',

        /// <summary>
        /// 已经接受
        /// </summary>
        Accepted = (byte)'b',

        /// <summary>
        /// 已经被拒绝
        /// </summary>
        Rejected = (byte)'c'
    }

    /// <summary>
    /// TFtdcBalanceAlgorithmType是一个权益算法类型
    /// </summary>
    public enum EnumThostBalanceAlgorithmType : byte
    {
        /// <summary>
        /// 不计算期权市值盈亏
        /// </summary>
        Default = (byte)'1',

        /// <summary>
        /// 计算期权市值亏损
        /// </summary>
        IncludeOptValLost = (byte)'2'
    }

    /// <summary>
    /// TFtdcOptionRoyaltyPriceTypeType是一个期权权利金价格类型类型
    /// </summary>
    public enum EnumThostOptionRoyaltyPriceTypeType : byte
    {
        /// <summary>
        /// 昨结算价
        /// </summary>
        PreSettlementPrice = (byte)'1',

        /// <summary>
        /// 开仓价
        /// </summary>
        OpenPrice = (byte)'4'
    }

    /// <summary>
    /// TFtdcLoginModeType是一个登录模式类型
    /// </summary>
    public enum EnumThostLoginModeType : byte
    {
        /// <summary>
        /// 交易
        /// </summary>
        Trade = (byte)'0',

        /// <summary>
        /// 转账
        /// </summary>
        Transfer = (byte)'1'
    }

    /// <summary>
    /// TFtdcCombDirectionType是一个组合指令方向类型
    /// </summary>
    public enum EnumThostCombDirectionType : byte
    {
        /// <summary>
        /// 申请组合
        /// </summary>
        Comb = (byte)'0',

        /// <summary>
        /// 申请拆分
        /// </summary>
        UnComb = (byte)'1'
    }

    /// <summary>
    /// TFtdcStrikeOffsetTypeType是一个行权偏移类型类型
    /// </summary>
    public enum EnumThostFtdcStrikeOffsetType: byte
    {
        /// <summary>
        /// 实值额
        /// </summary>
        RealValue = (byte)'1',

        /// <summary>
        /// 盈利额
        /// </summary>
        ProfitValue = (byte)'2',

        /// <summary>
        /// 申请组合
        /// </summary>
        RealRatio = (byte)'3',

        /// <summary>
        /// 盈利比例
        /// </summary>
        ProfitRatio = (byte)'4'

    }

    /// <summary>
    /// TFtdcReserveOpenAccStasType是一个预约开户状态类型
    /// </summary>
    public enum EnumThostFtdcReserveOpenAccStasType : byte
    {
        /// <summary>
        /// 等待处理中
        /// </summary>
        Processing = (byte)'0',

        /// <summary>
        /// 已撤销
        /// </summary>
        Cancelled = (byte)'1',

        /// <summary>
        /// 已开户
        /// </summary>
        Opened = (byte)'3',

        /// <summary>
        /// 无效请求
        /// </summary>
        Invalid = (byte)'4'

    }

}
