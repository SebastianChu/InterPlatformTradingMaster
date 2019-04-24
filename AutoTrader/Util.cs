using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Configuration;
using log4net;

namespace AutoTrader
{
    public enum EnumProcessingType : byte
    {
        ACCEPTED = 0,
        TRADED = 10,
        CANCELLED = 11
    }

    public static class Util
    {
        public static ILog LogInfo = LogManager.GetLogger("logInfo");
        public static ILog LogError = LogManager.GetLogger("logError");

        public static string ConfigFile = ConfigurationManager.AppSettings["ConfigFile"].Trim();
        public static bool NeedRestartQuote = bool.Parse(ConfigurationManager.AppSettings["NeedRestartQuote"]);
        public static string ConnectionStr = ConfigurationManager.ConnectionStrings["HFTraderConnection"].ConnectionString;

        public static string ProductInfo = "IPTM_v1.0";
        public static string Separator = "";//":";
        public static int RefIndexLength = 6;

        private static ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, string>>> _StrategyParameter = new ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, string>>>();

        public static ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, string>>> StrategyParameter
        {
            get { return _StrategyParameter; }
            set { _StrategyParameter = value; }
        }

        private static ConcurrentDictionary<string, ConcurrentDictionary<string, StrategyControl>> _StrategyMap = new ConcurrentDictionary<string, ConcurrentDictionary<string, StrategyControl>>();
        public static ConcurrentDictionary<string, ConcurrentDictionary<string, StrategyControl>> StrategyMap
        {
            get { return _StrategyMap; }
            set { _StrategyMap = value; }
        }

        private static ConcurrentDictionary<string, List<CThostFtdcDepthMarketDataField>> _InstrumentMarketDataRecord = new ConcurrentDictionary<string, List<CThostFtdcDepthMarketDataField>>();
        public static ConcurrentDictionary<string, List<CThostFtdcDepthMarketDataField>> InstrumentMarketDataRecord
        {
            get { return _InstrumentMarketDataRecord; }
            set { _InstrumentMarketDataRecord = value; }
        }

        private static ConcurrentDictionary<string, List<CThostFtdcDepthMarketDataField>> _MarketDataCache = new ConcurrentDictionary<string, List<CThostFtdcDepthMarketDataField>>();
        public static ConcurrentDictionary<string, List<CThostFtdcDepthMarketDataField>> MarketDataCache
        {
            get { return _MarketDataCache; }
            set { _MarketDataCache = value; }
        }

        private static ConcurrentDictionary<string, bool> _IsSentBuyOrder = new ConcurrentDictionary<string, bool>();
        public static ConcurrentDictionary<string, bool> IsSentBuyOrder
        {
            get { return _IsSentBuyOrder; }
            set { _IsSentBuyOrder = value; }
        }

        private static ConcurrentDictionary<string, bool> _IsSentSellOrder = new ConcurrentDictionary<string, bool>();

        public static ConcurrentDictionary<string, bool> IsSentSellOrder
        {
            get { return _IsSentSellOrder; }
            set { _IsSentSellOrder = value; }
        }

        private static ConcurrentDictionary<string, bool> _IsSentCancelBuy = new ConcurrentDictionary<string, bool>();
        public static ConcurrentDictionary<string, bool> IsSentCancelBuy
        {
            get { return _IsSentCancelBuy; }
            set { _IsSentCancelBuy = value; }
        }

        private static ConcurrentDictionary<string, bool> _IsSentCancelSell = new ConcurrentDictionary<string, bool>();

        public static ConcurrentDictionary<string, bool> IsSentCancelSell
        {
            get { return _IsSentCancelSell; }
            set { _IsSentCancelSell = value; }
        }

        private static ConcurrentDictionary<string, bool> _IsFilledBuyOrder = new ConcurrentDictionary<string, bool>();
        public static ConcurrentDictionary<string, bool> IsFilledBuyOrder
        {
            get { return _IsFilledBuyOrder; }
            set { _IsFilledBuyOrder = value; }
        }

        private static ConcurrentDictionary<string, bool> _IsFilledSellOrder = new ConcurrentDictionary<string, bool>();

        public static ConcurrentDictionary<string, bool> IsFilledSellOrder
        {
            get { return _IsFilledSellOrder; }
            set { _IsFilledSellOrder = value; }
        }
        public static ConcurrentDictionary<string, bool> IsQuoteReloaded = new ConcurrentDictionary<string, bool>();
        public static ConcurrentDictionary<string, bool> IsQuoteReloading = new ConcurrentDictionary<string, bool>();

        private static ConcurrentDictionary<string, TradingReport> _TradingReportDict = new ConcurrentDictionary<string, TradingReport>();

        public static ConcurrentDictionary<string, TradingReport> TradingReportDict
        {
            get { return _TradingReportDict; }
            set { _TradingReportDict = value; }
        }

        private static ConcurrentDictionary<string, ConcurrentDictionary<string, MarkupOrderStatistics>> _MarkUpOrderReportDict = new ConcurrentDictionary<string, ConcurrentDictionary<string, MarkupOrderStatistics>>();
        public static ConcurrentDictionary<string, ConcurrentDictionary<string, MarkupOrderStatistics>> MarkupOrderReportDict
        {
            get { return _MarkUpOrderReportDict; }
            set { _MarkUpOrderReportDict = value; }
        }


        private static ConcurrentDictionary<int, CThostFtdcTradeField> _OpenPositionTrade = new ConcurrentDictionary<int, CThostFtdcTradeField>();

        public static ConcurrentDictionary<int, CThostFtdcTradeField> OpenPositionTrade
        {
            get { return _OpenPositionTrade; }
            set { _OpenPositionTrade = value; }
        }



        public static long NanosecPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;

        public static double MicrosecPerTick = (1000.0 * 1000.0) / Stopwatch.Frequency;

        public const double Tolerance = 0.00001;

        public static bool StrategyNeedLaunch(string code, int validVal)
        {
            if (validVal == 1) //全天有效
            {
                return true;
            }
            else if (validVal == -1 && TradingTimeManager.IsBeforeNightClose(DateTime.Now, code)) //夜盘有效
            {
                return true;
            }
            else if (validVal == 2 && !TradingTimeManager.IsBeforeNightClose(DateTime.Now, code))//日盘有效
            {
                return true;
            }
            return false;
        }

        public static int ExchangeTimeOffset = 0;

        public static int GetSecFromDateTime(DateTime dt)
        {
            try
            {
                return dt.Hour * 3600 + dt.Minute * 60 + dt.Second;
            }
            catch (Exception ex)
            {
            }
            return 0;
        }

        public static void WriteExceptionToLogFile(Exception ex)
        {
            string sExceptionMsg = "";
            if (ex != null)
            {
                if (ex.Message != null)
                    sExceptionMsg += ex.Message;
                if (ex.StackTrace != null)
                    sExceptionMsg += ex.StackTrace;
            }
            string sExceptionSource = "错误: 异常" + sExceptionMsg;
            WriteError(sExceptionSource);
        }

        public static void WriteInfo(string sText, bool isPrintMode = true)
        {
            LogInfo.Info(sText);
            if (isPrintMode)
            {
                Console.WriteLine(sText);
            }
        }

        public static void WriteError(string sText)
        {
            Console.WriteLine(sText);
            LogInfo.Info(sText);
            LogError.Error(sText);

        }

        public static void WriteWarn(string sText)
        {
            WriteError(sText);
        }

        public static void WriteException(Exception ex)
        {
            WriteInfo(ex.Source + ex.Message + ex.StackTrace, true);
        }


        /// <summary>
        /// double相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>

        public static bool IsEqualTo(this double a, double b)
        {
            return (a <= b + Tolerance && a >= b - Tolerance);
        }

        /// <summary>
        /// double大于
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsGreaterThan(this double a, double b)
        {
            return (a - b > Tolerance);
        }

        /// <summary>
        /// double小于
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsLessThan(this double a, double b)
        {
            return (b - a > Tolerance);
        }

        /// <summary>
        /// double小于等于
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsLessThanOrEqualTo(this double a, double b)
        {
            return !(a.IsGreaterThan(b));
        }

        /// <summary>
        /// double大于等于
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool IsGreaterThanOrEqualTo(this double a, double b)
        {
            return !(a.IsLessThan(b));
        }

        //public static int Remove<T>(this EnumerableRowCollection<T> coll, Func<T, bool> condition)
        //{
        //    var itemsToRemove = coll.Where(condition).ToList();
        //    foreach (var itemToRemove in itemsToRemove)
        //    {
        //        coll.Remove(itemToRemove);
        //    }
        //    return itemsToRemove.Count;
        //}

        public static void RemoveElements<T>(this IList<T> source, Func<T, bool> filter, Action<T> method)
        {
            var indexs = (from d in source where filter(d) select source.IndexOf(d)).ToList();
            indexs.Sort();
            for (var i = indexs.Count - 1; i >= 0; i--)
            {
                method?.Invoke(source[indexs[i]]);
                source.RemoveAt(indexs[i]);
            }
        }

        public static int StartId = 1000;
        private static Dictionary<string, int> _instrumentIdToId = new Dictionary<string, int>();
        public static Dictionary<string, int> InstrumentIdToId
        {
            get { return _instrumentIdToId; }
            set { _instrumentIdToId = value; }
        }

        public static bool IsCloseDirection(EnumThostDirectionType closeDirection, EnumThostDirectionType openDirection, EnumThostOffsetFlagType openClose, EnumThostOffsetFlagType OrgOpenClose)
        {
            return closeDirection != openDirection && openClose != EnumThostOffsetFlagType.Open && OrgOpenClose == EnumThostOffsetFlagType.Open;
        }

        public static bool IsInReLoadingStatus(string instrumentId)
        {
            return IsQuoteReloading.ContainsKey(instrumentId) && IsQuoteReloading[instrumentId] || MarketDataCache.ContainsKey(instrumentId) && Util.MarketDataCache[instrumentId].Count > 0;
        }

        public static string GetInputOrderRefFromRefIndex(string clientTag, int orderRef)
        {
            if (string.IsNullOrEmpty(clientTag))
            {
                return orderRef.ToString();
            }
            //string.Format("D{0}", RefIndexLength);
            if (string.IsNullOrEmpty(Separator))
            {
                return string.Format("{0}{1}", orderRef, clientTag.PadLeft(RefIndexLength, '0'));//clientTag.ToString(formatStr));
            }
            return string.Format("{0}{1}{2}", orderRef, Separator, clientTag);
        }

        public static string GetRefIndexFromOrderRef(string orderRef)
        {
            if (!string.IsNullOrEmpty(Separator) && orderRef.Contains(Separator))
            {
                return orderRef.Substring(orderRef.IndexOf(Separator) + 1);
            }
            else if (orderRef.Length > RefIndexLength)
            {
                return orderRef.Substring(orderRef.Length - RefIndexLength).TrimStart('0');
            }
            return string.Empty;
        }

        public static string GetMaxRefFromOrderRef(string orderRef)
        {
            if (!string.IsNullOrEmpty(Separator) && orderRef.Contains(Separator))
            {
                return orderRef.Substring(0, orderRef.IndexOf(Separator));
            }
            else if (orderRef.Length > RefIndexLength)
            {
                return orderRef.Substring(0, orderRef.Length - RefIndexLength);
            }
            return string.Empty;
        }

        public static bool IsClientTagRef(string orderRef, string productInfo)
        {
            if (productInfo == ProductInfo || string.IsNullOrEmpty(productInfo))
            {
                if (!string.IsNullOrEmpty(Separator) && orderRef.Contains(Separator))
                {
                    return true;
                }
                else if (orderRef.Length > RefIndexLength)
                {
                    return true;
                }
                else
                {
                    WriteInfo(string.Format("No client tag: {0}", orderRef));
                }
            }
            return false;
        }

        public static int GetPositionKey(string instrumentId, EnumThostPosiDirectionType direction)
        {
            if (InstrumentIdToId.ContainsKey(instrumentId))
            {
                return InstrumentIdToId[instrumentId] + (int)direction;
            }

            return -1;
        }

        public static string GetOrderKey(CThostFtdcOrderField pOrder)
        {
            return string.Format("{0}:{1}:{2}", pOrder.BrokerOrderSeq, pOrder.OrderRef, pOrder.ExchangeID);
        }

        public static string GetDirectionKey(string instrumentId, EnumThostDirectionType direction)
        {
            return string.Format("{0}:{1}", instrumentId, direction);
        }
    }
    
    public class TradingTime
    {
        public DateTime DayOpen { get; set; }

        public DateTime DayBreak { get; set; }

        public DateTime DayBreakEnd { get; set; }

        public DateTime NoonBreak { get; set; }

        public DateTime NoonOpen { get; set; }

        public DateTime DayClose { get; set; }

        public bool NightTrading { get; set; }

        public DateTime NightOpen { get; set; }

        public DateTime NightClose { get; set; }
    }


    public class TradingReport
    {
        public string InvestorID { get; set; }
        public string BrokerID { get; set; }
        public string InstrumentID { get; set; }

        public EnumThostDirectionType BuySell { get; set; }

        public EnumThostOffsetFlagType OpenClose { get; set; }

        public double AvgTradedPrice { get; set; }

        public double OppositePrice { get; set; }

        public int CommitVolume { get; set; }

        public int TradedVolume { get; set; }

        public int CancelVolume { get; set; }

        public string UpdateTime { get; set; }

        public double InsertCostTime { get; set; }

        public string OrderRef { get; set; }

        public double Slippage { get; set; }

        public double CloseProfit { get; set; }

        public Stopwatch Watch { get; set; }

    }


    public class MarkupOrderStatistics
    {
        public string InstrumentID { get; set; }
        //public int LimitOrderCount { get; set; }
        //public int MarkupCount { get; set; }
        //public string OrderRef { get; set; }
        public double PriceTick { get; set; }
        public double LastCancelledPrice { get; set; }
        public int CommitVolume { get; set; }
        public int TradedVolume { get; set; }
        public double LastTradedPrice { get; set; }
        public double TradedUnitCost { get; set; }
    }

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
}
