using System;
using System.Collections.Generic;

namespace TradingMaster
{
    /// <summary>
    /// 交易的时间段
    /// </summary>
    public class TradingTime
    {
        public List<TradingPhase> tradingPhaseList;

        public TradingTime()
        {
            tradingPhaseList = new List<TradingPhase>();
        }
    }

    public class TradingPhase
    {
        public Boolean IsPreMarket { get; set; }
        public TimeInfo StartTime { get; set; }
        public TimeInfo EndTime { get; set; }

        /// <summary>
        /// 得到总的分钟数
        /// </summary>
        /// <returns></returns>
        public int GetTotalMinute()
        {
            DateTime dtStart = new DateTime(2000, 1, 1, StartTime.Hour, StartTime.minute, 0);
            DateTime dtEnd = new DateTime(2000, 1, 1, EndTime.Hour, EndTime.minute, 0);
            if (StartTime.TimeType == TIMETYPE.YESTERDAY && EndTime.TimeType == TIMETYPE.TODAY)
            {
                dtEnd = dtEnd.AddDays(1);
            }
            else if (StartTime.TimeType == TIMETYPE.YESTERDAY && EndTime.TimeType == TIMETYPE.TOMORROW)
            {
                dtEnd = dtEnd.AddDays(2);
            }
            else if (StartTime.TimeType == TIMETYPE.TODAY && EndTime.TimeType == TIMETYPE.TOMORROW)
            {
                dtEnd = dtEnd.AddDays(1);
            }
            TimeSpan ts = dtEnd.Subtract(dtStart);
            return (int)ts.TotalMinutes + 1;
        }

        public int GetIndexByMinute(int time)
        {
            DateTime dtStart = new DateTime(2000, 1, 1, StartTime.Hour, StartTime.minute, 0);
            DateTime dtEnd = new DateTime(2000, 1, 1, EndTime.Hour, EndTime.minute, 0);
            if (StartTime.TimeType == TIMETYPE.YESTERDAY && EndTime.TimeType == TIMETYPE.TODAY)
            {
                dtEnd = dtEnd.AddDays(1);
            }
            else if (StartTime.TimeType == TIMETYPE.YESTERDAY && EndTime.TimeType == TIMETYPE.TOMORROW)
            {
                dtEnd = dtEnd.AddDays(2);
            }
            else if (StartTime.TimeType == TIMETYPE.TODAY && EndTime.TimeType == TIMETYPE.TOMORROW)
            {
                dtEnd = dtEnd.AddDays(1);
            }

            DateTime dtNow = new DateTime(2000, 1, 1, time / 100, time % 100, 0);
            if (dtNow >= dtStart && dtNow <= dtEnd)
            {
                return (int)dtNow.Subtract(dtStart).TotalMinutes;
            }
            dtNow = dtNow.AddDays(1);
            if (dtNow >= dtStart && dtNow <= dtEnd)
            {
                return (int)dtNow.Subtract(dtStart).TotalMinutes;
            }
            return -1;
        }

        public override string ToString()
        {
            return "[IsPre:" + IsPreMarket + "][start:" + this.StartTime + "][end:" + this.EndTime + "]";
        }
    }

    public class TimeInfo
    {
        public TimeInfo(string timeOrg)
        {
            if (timeOrg.StartsWith("-"))
            {
                timeType = TIMETYPE.YESTERDAY;
            }
            else if (timeOrg.StartsWith("+"))
            {
                timeType = TIMETYPE.TOMORROW;
            }
            else
            {
                timeType = TIMETYPE.TODAY;
            }
            time = int.Parse(timeOrg);
            if (time < 0)
            {
                time = -time;
            }
        }

        private TIMETYPE timeType;
        public TIMETYPE TimeType
        {
            get { return timeType; }
            set { timeType = value; }
        }

        private int time;
        public int Time
        {
            get { return time; }
            set { time = value; }
        }

        public int Hour
        {
            get
            {
                return time / 100;
            }
        }

        public int minute
        {
            get
            {
                return time % 100;
            }
        }

        public override string ToString()
        {
            return time.ToString() + " " + timeType.ToString();
        }
    }

    public enum TIMETYPE
    {
        TODAY = 0,
        YESTERDAY = 1,
        TOMORROW = 2
    }

    public class DstSettings
    {
        public DstSettings(DateTime startDate, DateTime endDate)
        {
            dstStarting = startDate;
            dstEnding = endDate;
        }

        /// <summary>
        /// 夏令时开始
        /// </summary>
        private DateTime dstStarting;
        public DateTime DstStarting
        {
            get { return dstStarting; }
            set { dstStarting = value; }
        }

        private DateTime dstEnding;
        /// <summary>
        /// 夏令时结束
        /// </summary>
        public DateTime DstEnding
        {
            get { return dstEnding; }
            set { dstEnding = value; }
        }
    }
}
