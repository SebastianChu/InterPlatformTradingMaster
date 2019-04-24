using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader
{
    public class NewEntrustInfo : InstrumentInfo
    {
        public string EntrustStatus;
        public string EntrustDirection;
        public string EntrustPriceType;
        public double OriginalEntrustPrice;
        public double EntrustPrice;
        public double MultiFactor;
        public double TotalPrice;
        public double EntrustVolume;
        public double TradedVolume;
        public string InvestType;
        public string FuturesDirection;
        public string ErrorMsg;
        public string TradingDay;
        public string CalendarDate;
        public string OrderRef;
        public string ManagerName;
        public string FundName;
        public string Strategy;
        public string Researcher;
        public string BrokerId;
        public string InvestorId;
        public string AssetType;
        public string AssetCode;
        public int ParentRef;
        public string Status;

        public static string GetExchangeInstrumentId(string windCode)
        {
            if (!windCode.Contains('.'))
            {
                Util.WriteError("Illegal wind code: " + windCode);
                return string.Empty;
            }
            string[] windIndex = windCode.Split('.');
            string instruCode = windIndex[0];
            string exchangeId = windIndex[1];
            if (string.IsNullOrEmpty(instruCode) || string.IsNullOrEmpty(exchangeId))
            {
                Util.WriteError("Illegal wind code: " + windCode);
                return string.Empty;
            }
            int numIndex = instruCode.IndexOfAny("0123456789".ToCharArray());
            string spec = instruCode.Substring(0, numIndex);
            string timeStr = instruCode.Substring(numIndex);
            if (exchangeId.Contains("SHF") || exchangeId.Contains("DCE") || exchangeId.Contains("INE"))
            {
                spec = spec.ToLower();
            }
            else if (exchangeId.Contains("CZC") || exchangeId.Contains("FE"))
            {
                spec = spec.ToUpper();
            }
            return spec + timeStr;
        }
    }

    public class InstrumentInfo
    {
        public string MarketType;
        public string InstrumentId;
        public string InstrumentName;
        public string ExchangeInstId;
        public string ExchangeId;
    }

    public class PositionField : InstrumentInfo
    {
        //
        // 摘要:
        //     资金差额
        public double CashIn { get; set; }

        //
        // 摘要:
        //     手续费
        public double Commission { get; set; }
        //
        // 摘要:
        //     平仓盈亏
        public double CloseProfit { get; set; }
        //
        // 摘要:
        //     持仓盈亏
        public double PositionProfit { get; set; }
        //
        // 摘要:
        //     上次结算价
        public double _preSettlementPrice;

        public double PreSettlementPrice { get; set; }
        //
        // 摘要:
        //     本次结算价
        public double SettlementPrice { get; set; }
        //
        // 摘要:
        //     持仓均价
        public double AvgPrice { get; set; }
        //
        // 摘要:
        //     交易日
        public string _tradingDay;

        public string TradingDay { get; set; }
        //
        // 摘要:
        //     结算编号
        public int SettlementId { get; set; }
        //
        // 摘要:
        //     开仓成本
        public double OpenCost { get; set; }
        //
        // 摘要:
        //     交易所保证金
        public double ExchangeMargin { get; set; }
        //
        // 摘要:
        //     组合成交形成的持仓
        public int CombPosition { get; set; }
        //
        // 摘要:
        //     组合多头冻结
        public int CombLongFrozen { get; set; }
        //
        // 摘要:
        //     组合空头冻结
        public int CombShortFrozen { get; set; }
        //
        // 摘要:
        //     逐日盯市平仓盈亏
        public double CloseProfitByDate { get; set; }
        //
        // 摘要:
        //     逐笔对冲平仓盈亏
        public double CloseProfitByTrade { get; set; }
        //
        // 摘要:
        //     今日持仓
        public int TodayPosition { get; set; }
        //
        // 摘要:
        //     保证金率
        public double MarginRateByMoney { get; set; }
        //
        // 摘要:
        //     冻结的手续费
        public double FrozenCommission { get; set; }
        //
        // 摘要:
        //     保证金率(按手数)
        public double MarginRateByVolume { get; set; }
        //
        // 摘要:
        //     冻结的资金
        public double FrozenCash { get; set; }
        //
        // 摘要:
        //     占用的保证金
        private double _useMargin;

        public double UseMargin { get; set; }
        //
        // 摘要:
        //     经纪公司代码
        private string _brokerId;

        public string BrokerId { get; set; }
        //
        // 摘要:
        //     投资者代码
        private string _investorId;

        public string InvestorId { get; set; }
        //
        // 摘要:
        //     持仓多空方向
        public EnumThostPosiDirectionType PosiDirection { get; set; }
        //
        // 摘要:
        //     投机套保标志
        public EnumThostHedgeFlagType HedgeFlag { get; set; }
        //
        // 摘要:
        //     持仓日期
        public EnumThostPositionDateType PositionDate { get; set; }
        //
        // 摘要:
        //     上日持仓
        private int _ydPosition;

        public int YdPosition { get; set; }
        //
        // 摘要:
        //     今日持仓
        private int _position;

        public int Position { get; set; }
        //
        // 摘要:
        //     多头冻结
        private int _longFrozen;

        public int LongFrozen { get; set; }
        //
        // 摘要:
        //     空头冻结
        private int _shortFrozen;

        public int ShortFrozen { get; set; }
        //
        // 摘要:
        //     开仓冻结金额
        private double _longFrozenAmount;

        public double LongFrozenAmount { get; set; }
        //
        // 摘要:
        //     开仓冻结金额
        public double ShortFrozenAmount { get; set; }
        //
        // 摘要:
        //     开仓量
        public int OpenVolume { get; set; }
        //
        // 摘要:
        //     平仓量
        public int CloseVolume { get; set; }
        //
        // 摘要:
        //     开仓金额
        public double OpenAmount { get; set; }
        //
        // 摘要:
        //     平仓金额
        public double CloseAmount { get; set; }
        //
        // 摘要:
        //     持仓成本
        public double PositionCost { get; set; }
        //
        // 摘要:
        //     上次占用的保证金

        public double PreMargin { get; set; }
        //
        // 摘要:
        //     冻结的保证金
        public double FrozenMargin { get; set; }

        /// <summary>
        /// 可平
        /// </summary>
        public int Avail { get; set; }

        /// <summary>
        /// 可平今
        /// </summary>
        public int AvailToday { get; set; }
    }
}
