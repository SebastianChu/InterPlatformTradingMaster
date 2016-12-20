using System;
using System.ComponentModel;

namespace TradingMaster
{
    public class ContractBank
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        public string BrokerID { get; set; }
        /// <summary>
        /// 银行代码
        /// </summary>
        public string BankID { get; set; }
        /// <summary>
        /// 银行分中心代码
        /// </summary>
        public string BankBrchID { get; set; }
        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }
    }

    public class BankAccountInfo
    {
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        public string BrokerID { get; set; }
        /// <summary>
        /// 银行代码
        /// </summary>
        public string BankID { get; set; }
        /// <summary>
        /// 银行分中心代码
        /// </summary>
        public string BankBrchID { get; set; }
        /// <summary>
        /// 银行帐号
        /// </summary>
        public string BankAccount { get; set; }
        /// <summary>
        /// 期货公司分支机构编码
        /// </summary>
        public string BrokerBranchID { get; set; }
        /// <summary>
        /// 投资者帐号
        /// </summary>
        public string AccountID { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public string IdCardType { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string IdentifiedCardNo { get; set; }
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 币种代码
        /// </summary>
        public string CurrencyID { get; set; }
        /// <summary>
        /// 开销户类别
        /// </summary>
        public string OpenOrDestroy { get; set; }
        /// <summary>
        /// 签约日期
        /// </summary>
        public string RegDate { get; set; }
        /// <summary>
        /// 解约日期
        /// </summary>
        public string OutDate { get; set; }
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID { get; set; }
        /// <summary>
        /// 客户类型
        /// </summary>
        public string CustType { get; set; }
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public string BankAccType { get; set; }
    }

    public class BankAcctDetail
    {
        /// <summary>
        /// 业务功能码
        /// </summary>
        public string TradeCode { get; set; }
        /// <summary>
        /// 银行代码
        /// </summary>
        public string BankID { get; set; }
        /// <summary>
        /// 银行分支机构代码
        /// </summary>
        public string BankBranchID { get; set; }
        /// <summary>
        /// 期商代码
        /// </summary>
        public string BrokerID { get; set; }
        /// <summary>
        /// 期商分支机构代码
        /// </summary>
        public string BrokerBranchID { get; set; }
        /// <summary>
        /// 交易日期
        /// </summary>
        public string TradeDate { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public string TradeTime { get; set; }
        /// <summary>
        /// 银行流水号
        /// </summary>
        public string BankSerial { get; set; }
        /// <summary>
        /// 交易系统日期
        /// </summary>
        public string TradingDay { get; set; }
        /// <summary>
        /// 银期平台消息流水号
        /// </summary>
        public int PlateSerial { get; set; }
        /// <summary>
        /// 会话号
        /// </summary>
        public int SessionID { get; set; }
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        public EnumThostIdCardTypeType IdCardType { get; set; }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string IdentifiedCardNo { get; set; }
        /// <summary>
        /// 客户类型
        /// </summary>
        public EnumThostCustTypeType CustType { get; set; }
        /// <summary>
        /// 银行帐号
        /// </summary>
        public string BankAccount { get; set; }
        /// <summary>
        /// 银行密码
        /// </summary>
        public string BankPassWord { get; set; }
        /// <summary>
        /// 投资者帐号
        /// </summary>
        public string AccountID { get; set; }
        /// <summary>
        /// 期货密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 期货公司流水号
        /// </summary>
        public int FutureSerial { get; set; }
        /// <summary>
        /// 安装编号
        /// </summary>
        public int InstallID { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 验证客户证件号码标志
        /// </summary>
        public EnumThostYesNoIndicatorType VerifyCertNoFlag { get; set; }
        /// <summary>
        /// 币种代码
        /// </summary>
        public string CurrencyID { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Digest { get; set; }
        /// <summary>
        /// 银行帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankAccType { get; set; }
        /// <summary>
        /// 渠道标志
        /// </summary>
        public string DeviceID;
        /// <summary>
        /// 期货单位帐号类型
        /// </summary>
        public EnumThostBankAccTypeType BankSecuAccType { get; set; }
        /// <summary>
        /// 期货公司银行编码
        /// </summary>
        public string BrokerIDByBank { get; set; }
        /// <summary>
        /// 期货单位帐号
        /// </summary>
        public string BankSecuAcc { get; set; }
        /// <summary>
        /// 交易柜员
        /// </summary>
        public string OperNo { get; set; }
        /// <summary>
        /// 请求编号
        /// </summary>
        public int RequestID { get; set; }
        /// <summary>
        /// 交易ID
        /// </summary>
        public int TID { get; set; }
        /// <summary>
        /// 银行可用金额
        /// </summary>
        public double BankUseAmount { get; set; }
        /// <summary>
        /// 银行可取金额
        /// </summary>
        public double BankFetchAmount { get; set; }
    }

    public class TransferSingleRecord : INotifyPropertyChanged
    {
        /// <summary>
        /// 流水号
        /// </summary>
        public int SerialNo
        {
            get { return serialNo; }
            set
            {
                serialNo = value;
                OnPropertyChanged("SerialNo");
            }
        }
        private int serialNo;

        /// <summary>
        /// 银行账号
        /// </summary>
        public string BankAcct
        {
            get { return bankAcct; }
            set
            {
                bankAcct = value;
                OnPropertyChanged("BankAcct");
            }
        }
        private string bankAcct;

        /// <summary>
        /// 交易类型
        /// </summary>
        public string TradeType
        {
            get { return tradeType; }
            set
            {
                tradeType = value;
                OnPropertyChanged("TradeType");
            }
        }
        private string tradeType;

        /// <summary>
        /// 发生金额
        /// </summary>
        public double TradedAmt
        {
            get { return tradedAmt; }
            set
            {
                tradedAmt = value;
                OnPropertyChanged("TradedAmt");
            }
        }
        private double tradedAmt;

        /// <summary>
        /// 交易时间
        /// </summary>
        public string TradingTime
        {
            get { return tradingTime; }
            set
            {
                tradingTime = value;
                OnPropertyChanged("TradingTime");
            }
        }
        private string tradingTime;

        /// <summary>
        /// 信息
        /// </summary>
        public string TradeInfo
        {
            get { return tradeInfo; }
            set
            {
                tradeInfo = value;
                OnPropertyChanged("TradeInfo");
            }
        }
        private string tradeInfo;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        public static int CompareBySerialNo(TransferSingleRecord t1, TransferSingleRecord t2)
        {
            try
            {
                if (t1 == null && t2 == null) return 0;
                if (t1 == null) return -1;
                if (t2 == null) return 1;
                return t1.SerialNo.CompareTo(t2.SerialNo);
            }
            catch (Exception ex)
            {
                Util.Log("exception" + ex.Message);
                Util.Log(ex.StackTrace);
                return 0;
            }
        }
    }
}
