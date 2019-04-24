using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Concurrent;

namespace AutoTrader
{
    public class TradingTimeManager
    {
        public static DateTime TradingDay;
        public static ConcurrentDictionary<string, TradingTime> InstrumentTradingTimeDic = new ConcurrentDictionary<string, TradingTime>();
        private static DateTime _NightDate;

        private static DateTime _NightInitTime;
        private static DateTime _NightEndTime;
        private static DateTime _DayInitTime;
        private static DateTime _DayEndTime;
        private static DateTime _NextNightInitTime;

        public static void InitTime(string retDateTime)
        {
            TradingDay = DateTime.ParseExact(retDateTime, "yyyyMMdd", new DateTimeFormatInfo());
            _NightDate = TradingDay.AddDays(-1);
            if (_NightDate > DateTime.Now)
            {
                if (DateTime.Now.Hour < 3)
                {
                    _NightDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 1);
                }
                else
                {
                    _NightDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                }
            }
            _NightInitTime = _NightDate.Add(new TimeSpan(20, 00, 0));
            _NightEndTime = (_NightDate.AddDays(1)).Add(new TimeSpan(2, 30, 0));
            _DayInitTime = TradingDay.Add(new TimeSpan(8, 00, 0));
            _DayEndTime = TradingDay.Add(new TimeSpan(15, 15, 0));
            _NextNightInitTime = TradingDay.Add(new TimeSpan(20, 00, 0));
        }

        public static void InitTimeMap(string exchangeID, string instrumentId)
        {
            TradingTime tradingTime = new TradingTime();
            tradingTime.NoonBreak = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 11, 30, 1);
            tradingTime.NightTrading = false;
            if (exchangeID == "CFFEX")
            {
                tradingTime.NoonOpen = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 13, 00, 0);
                if (instrumentId.Contains("TF") || instrumentId.Contains("T"))
                {
                    tradingTime.DayOpen = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 9, 30, 0);
                    tradingTime.DayClose = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 15, 0, 0);
                }
                else if (instrumentId.Contains("IC") || instrumentId.Contains("IF") || instrumentId.Contains("IH"))
                {
                    tradingTime.DayOpen = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 9, 15, 0);
                    tradingTime.DayClose = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 15, 0, 0);
                }
            }
            else if (exchangeID == "CZCE")
            {
                tradingTime.DayOpen = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 9, 0, 0);
                tradingTime.DayBreak = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 10, 15, 1);
                tradingTime.DayBreakEnd = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 10, 30, 0);

                tradingTime.NoonOpen = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 13, 30, 0);
                tradingTime.DayClose = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 15, 0, 0);

                if (instrumentId.StartsWith("AP") || instrumentId.StartsWith("CF") || instrumentId.StartsWith("CY") || instrumentId.StartsWith("FG") || instrumentId.StartsWith("MA") || instrumentId.StartsWith("OI")
                    || instrumentId.StartsWith("RM") || instrumentId.StartsWith("SR") || instrumentId.StartsWith("TA") || instrumentId.StartsWith("ZC"))
                {
                    tradingTime.NightTrading = true;
                    tradingTime.NightOpen = new DateTime(_NightDate.Year, _NightDate.Month, _NightDate.Day, 21, 0, 0);
                    tradingTime.NightClose = new DateTime(_NightDate.Year, _NightDate.Month, _NightDate.Day, 23, 30, 0);
                }
                tradingTime.DayOpen = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 9, 0, 1);
            }
            else if (exchangeID == "DCE")
            {
                tradingTime.DayOpen = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 9, 0, 0);
                tradingTime.DayBreak = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 10, 15, 1);
                tradingTime.DayBreakEnd = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 10, 30, 0);

                tradingTime.NoonOpen = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 13, 30, 0);
                tradingTime.DayClose = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 15, 0, 0);

                if (instrumentId.StartsWith("a") || instrumentId.StartsWith("b") || instrumentId.StartsWith("cs") || instrumentId.StartsWith("c") || instrumentId.StartsWith("eg") || instrumentId.StartsWith("i") || instrumentId.StartsWith("jm") || instrumentId.StartsWith("j")
                    || instrumentId.StartsWith("l") || instrumentId.StartsWith("m") || instrumentId.StartsWith("pp") || instrumentId.StartsWith("p") || instrumentId.StartsWith("v") || instrumentId.StartsWith("y"))
                {
                    tradingTime.NightTrading = true;
                    tradingTime.NightOpen = new DateTime(_NightDate.Year, _NightDate.Month, _NightDate.Day, 21, 0, 0);
                    tradingTime.NightClose = new DateTime(_NightDate.Year, _NightDate.Month, _NightDate.Day, 23, 00, 0);
                }
                tradingTime.DayOpen = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 9, 0, 1);
            }
            else
            {
                tradingTime.DayOpen = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 9, 0, 0);
                tradingTime.DayBreak = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 10, 15, 1);
                tradingTime.DayBreakEnd = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 10, 30, 0);

                tradingTime.NoonOpen = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 13, 30, 0);
                tradingTime.DayClose = new DateTime(TradingDay.Year, TradingDay.Month, TradingDay.Day, 15, 0, 0);
                tradingTime.NightOpen = new DateTime(_NightDate.Year, _NightDate.Month, _NightDate.Day, 21, 0, 0);

                if (instrumentId.StartsWith("ag") || instrumentId.StartsWith("au") || instrumentId.StartsWith("sc"))
                {
                    tradingTime.NightTrading = true;
                    DateTime tempDate = _NightDate.AddDays(1);
                    tradingTime.NightClose = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 02, 30, 0);
                }
                else if (instrumentId.StartsWith("al") || instrumentId.StartsWith("cu") || instrumentId.StartsWith("ni") || instrumentId.StartsWith("pb") || instrumentId.StartsWith("sn") || instrumentId.StartsWith("zn"))
                {
                    tradingTime.NightTrading = true;
                    DateTime tempDate = _NightDate.AddDays(1);
                    tradingTime.NightClose = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, 01, 00, 0);
                }
                else if (instrumentId.StartsWith("bu") || instrumentId.StartsWith("fu") || instrumentId.StartsWith("hc") || instrumentId.StartsWith("rb") || instrumentId.StartsWith("ru") || instrumentId.StartsWith("sp"))
                {
                    tradingTime.NightTrading = true;
                    tradingTime.NightClose = new DateTime(_NightDate.Year, _NightDate.Month, _NightDate.Day, 23, 00, 0);
                }
            }
            InstrumentTradingTimeDic[instrumentId] = tradingTime;
        }

        public static bool IsBeforeNightClose(DateTime dateTime, string instrumentId, bool isExchangeTime = false)
        {
            DateTime exchangeNow = dateTime;
            if (!isExchangeTime)
            {
                exchangeNow = dateTime.AddSeconds(Util.ExchangeTimeOffset);
            }
            if (InstrumentTradingTimeDic.ContainsKey(instrumentId))
            {
                TradingTime tradingTime = InstrumentTradingTimeDic[instrumentId];
                if (tradingTime.NightTrading && exchangeNow >= tradingTime.NightOpen.AddHours(-2) && exchangeNow <= tradingTime.NightClose)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsInTradingTime(DateTime dateTime, string exchangeID, string instrumentId = "", bool isExchangeTime = false) // Todo
        {
            DateTime exchangeNow = dateTime;
            if (!isExchangeTime)
            {
                exchangeNow = dateTime.AddSeconds(Util.ExchangeTimeOffset);
            }
            if (InstrumentTradingTimeDic.ContainsKey(instrumentId))
            {
                TradingTime tradingTime = InstrumentTradingTimeDic[instrumentId];
                if (exchangeID == "CFFEX")
                {
                    if (exchangeNow >= tradingTime.DayOpen && exchangeNow <= tradingTime.NoonBreak
                     || exchangeNow >= tradingTime.NoonOpen && exchangeNow <= tradingTime.DayClose)
                        return true;
                }
                if (exchangeNow >= tradingTime.DayOpen && exchangeNow <= tradingTime.DayBreak
                || exchangeNow >= tradingTime.DayBreakEnd && exchangeNow <= tradingTime.NoonBreak
                || exchangeNow >= tradingTime.NoonOpen && exchangeNow <= tradingTime.DayClose
                || tradingTime.NightTrading && exchangeNow >= tradingTime.NightOpen && exchangeNow <= tradingTime.NightClose
                )
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsBeforeOpeningTime(DateTime dateTime, string exchangeID, string instrumentId = "", bool isExchangeTime = false) // Todo
        {
            DateTime exchangeNow = dateTime;
            if (!isExchangeTime)
            {
                exchangeNow = dateTime.AddSeconds(Util.ExchangeTimeOffset);
            }
            if (InstrumentTradingTimeDic.ContainsKey(instrumentId))
            {
                TradingTime tradingTime = InstrumentTradingTimeDic[instrumentId];
                if (tradingTime.NightTrading && exchangeNow > tradingTime.NightClose && exchangeNow < tradingTime.DayOpen
                || tradingTime.NightTrading && exchangeNow > tradingTime.DayClose.AddDays(-1) && exchangeNow < tradingTime.NightOpen
                || exchangeNow < tradingTime.DayOpen
                )
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsNearBreakTime(int sec, string instrumentId, string exchangeID)
        {
            DateTime exchangeNow = DateTime.Now.AddSeconds(Util.ExchangeTimeOffset);
            if (InstrumentTradingTimeDic.ContainsKey(instrumentId))
            {
                TradingTime tradingTime = InstrumentTradingTimeDic[instrumentId];
                if (tradingTime.DayClose > exchangeNow && (tradingTime.DayClose - exchangeNow).TotalSeconds <= sec
                || tradingTime.NightTrading && tradingTime.NightClose != null && tradingTime.NightClose > exchangeNow && (tradingTime.NightClose - exchangeNow).TotalSeconds <= sec
                || tradingTime.DayBreak != null && tradingTime.DayBreak > exchangeNow && (tradingTime.DayBreak - exchangeNow).TotalSeconds <= sec
                || tradingTime.NoonBreak > exchangeNow && (tradingTime.NoonBreak - exchangeNow).TotalSeconds <= sec)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsInOpenTime(int sec, string instrumentId)
        {
            DateTime exchangeNow = DateTime.Now.AddSeconds(Util.ExchangeTimeOffset);
            if (InstrumentTradingTimeDic.ContainsKey(instrumentId))
            {
                TradingTime tradingTime = InstrumentTradingTimeDic[instrumentId];
                if (tradingTime.DayOpen < exchangeNow && (exchangeNow - tradingTime.DayOpen).TotalSeconds <= sec
                || tradingTime.NightTrading && tradingTime.NightOpen < exchangeNow && (exchangeNow - tradingTime.NightOpen).TotalSeconds <= sec
                || tradingTime.DayBreakEnd != null && tradingTime.DayBreakEnd < exchangeNow && (exchangeNow - tradingTime.DayBreakEnd).TotalSeconds <= sec
                || tradingTime.NoonOpen < exchangeNow && (exchangeNow - tradingTime.NoonOpen).TotalSeconds <= sec)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsInEndingTime(string instrumentId, string exchangeID, int sec = 0)
        {
            DateTime exchangeNow = DateTime.Now.AddSeconds(Util.ExchangeTimeOffset);
            if (InstrumentTradingTimeDic.ContainsKey(instrumentId))
            {
                TradingTime tradingTime = InstrumentTradingTimeDic[instrumentId];
                if (tradingTime.NightTrading && exchangeNow < tradingTime.NightClose && (tradingTime.NightClose - exchangeNow).TotalSeconds <= sec
                || exchangeNow < tradingTime.DayClose && (tradingTime.DayClose - exchangeNow).TotalSeconds <= sec)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsAllBreakTime(int sec = 0)
        {
            DateTime exchangeNow = DateTime.Now.AddSeconds(Util.ExchangeTimeOffset);
            DateTime breakStart = TradingDay.Add(new TimeSpan(11, 30, 0));
            DateTime breakEnd = TradingDay.Add(new TimeSpan(13, 00, 0));
            if (exchangeNow >= breakStart.AddSeconds(sec) && exchangeNow <= breakEnd.AddSeconds(-sec))
            {
                return true;
            }
            return false;
        }

        public static bool IsInCycleTime(int sec = 0)
        {
            DateTime exchangeNow = DateTime.Now.AddSeconds(Util.ExchangeTimeOffset);
            if (exchangeNow >= _DayInitTime && exchangeNow <= _DayEndTime.AddSeconds(-sec))
            {
                return true;
            }
            if (exchangeNow >= _NightInitTime && exchangeNow <= _NightEndTime.AddSeconds(-sec))
            {
                return true;
            }
            return false;
        }

        public static bool IsAllEndTime(int sec = 0)
        {
            DateTime exchangeNow = DateTime.Now.AddSeconds(Util.ExchangeTimeOffset);
            if (exchangeNow >= _DayEndTime.AddSeconds(-sec) && exchangeNow < _NextNightInitTime)
            {
                return true;
            }
            else if (exchangeNow >= _NightEndTime.AddSeconds(-sec) && exchangeNow < _DayInitTime)
            {
                return true;
            }
            return false;
        }

        public static bool IsInNightBreakTime(string instrumentId, string exchangeID)
        {
            DateTime exchangeNow = DateTime.Now.AddSeconds(Util.ExchangeTimeOffset);
            if (InstrumentTradingTimeDic.ContainsKey(instrumentId))
            {
                TradingTime tradingTime = InstrumentTradingTimeDic[instrumentId];
                if (tradingTime.NightTrading && exchangeNow >= tradingTime.NightClose && exchangeNow < tradingTime.DayOpen.AddMinutes(-5)
                    || exchangeNow < tradingTime.DayOpen.AddMinutes(-5))
                {
                    return true;
                }
            }
            return false;
        }

        public static TimeSpan GetTimeGap(string instrumentId, DateTime tickTime, DateTime lastTickTime) // Todo
        {
            TimeSpan tickGap = tickTime - lastTickTime;
            DateTime exchangeNow = DateTime.Now.AddSeconds(Util.ExchangeTimeOffset);
            if (InstrumentTradingTimeDic.ContainsKey(instrumentId))
            {
                TradingTime tradingTime = InstrumentTradingTimeDic[instrumentId];
                if (tradingTime.NightTrading && tickTime >= tradingTime.DayOpen && lastTickTime <= tradingTime.NightClose)
                {
                    tickGap -= tradingTime.DayOpen - tradingTime.NightClose;
                }
                if (tickTime >= tradingTime.DayBreakEnd && lastTickTime <= tradingTime.DayBreak)
                {
                    tickGap -= tradingTime.DayBreakEnd - tradingTime.DayBreak;
                }
                if (tickTime >= tradingTime.NoonOpen && lastTickTime <= tradingTime.NoonBreak)
                {
                    tickGap -= tradingTime.NoonOpen - tradingTime.NoonBreak;
                }
            }
            return tickGap;
        }
    }
}
