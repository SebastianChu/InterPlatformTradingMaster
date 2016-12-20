using System;
using System.Threading;

namespace TradingMaster
{
    /// <summary>
    /// 交易所时间
    /// </summary>
    public class ExchangeTime
    {
        /// <summary>
        /// 上期所时间 格式为HH*3600+MM*60+SS
        /// </summary>
        private int _ShfeTime;
        /// <summary>
        /// 上期所时间 格式为HH*3600+MM*60+SS
        /// </summary>
        public int ShfeTime
        {
            get
            {
                int ret;
                _ExchTimeMutex.WaitOne();
                ret = _ShfeTime;
                _ExchTimeMutex.ReleaseMutex();
                return ret;
            }
        }

        /// <summary>
        /// 中金所时间 格式为HH*3600+MM*60+SS
        /// </summary>
        private int _FeexTime;
        /// <summary>
        /// 中金所时间 格式为HH*3600+MM*60+SS
        /// </summary>
        public int FeexTime
        {
            get
            {
                int ret;
                _ExchTimeMutex.WaitOne();
                ret = _FeexTime;
                _ExchTimeMutex.ReleaseMutex();
                return ret;
            }
        }


        /// <summary>
        /// 郑商所时间 格式为HH*3600+MM*60+SS
        /// </summary>
        private int _CzcsTime;
        /// <summary>
        /// 郑商所时间 格式为HH*3600+MM*60+SS
        /// </summary>
        public int CzcsTime
        {
            get
            {
                int ret;
                _ExchTimeMutex.WaitOne();
                ret = _CzcsTime;
                _ExchTimeMutex.ReleaseMutex();
                return ret;
            }
        }

        /// <summary>
        /// 大商所时间 格式为HH*3600+MM*60+SS
        /// </summary>
        private int _DceTime;
        /// <summary>
        /// 大商所时间 格式为HH*3600+MM*60+SS
        /// </summary>
        public int DceTime
        {
            get
            {
                int ret;
                _ExchTimeMutex.WaitOne();
                ret = _DceTime;
                _ExchTimeMutex.ReleaseMutex();
                return ret;
            }
        }

        /// <summary>
        /// 能源中心时间 格式为HH*3600+MM*60+SS
        /// </summary>
        private int _IneTime;
        /// <summary>
        /// 能源中心时间 格式为HH*3600+MM*60+SS
        /// </summary>
        public int IneTime
        {
            get
            {
                int ret;
                _ExchTimeMutex.WaitOne();
                ret = _IneTime;
                _ExchTimeMutex.ReleaseMutex();
                return ret;
            }
        }

        /// <summary>
        /// 上期所时间和当前时间的差值
        /// </summary>
        private int _ShfeTimeGap;
        /// <summary>
        /// 中金所时间和当前时间的差值
        /// </summary>
        private int _FeexTimeGap;
        /// <summary>
        /// 郑商所时间和当前时间的差值
        /// </summary>
        private int _CzcsTimeGap;
        /// <summary>
        /// 大商所时间和当前时间的差值
        /// </summary>
        private int _DceTimeGap;
        /// <summary>
        /// 能源中心时间和当前时间的差值
        /// </summary>
        private int _IneTimeGap;

        private Mutex _ExchTimeMutex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shfeTimeStr">上期所时间，若有其他时间遗失，则使用上期所时间</param>
        /// <param name="feexTimeStr">中金所时间</param>
        /// <param name="czcsTimeStr">郑商所时间</param>
        /// <param name="dceTimeStr">大商所时间</param>
        public ExchangeTime(String shfeTimeStr, String feexTimeStr, String czcsTimeStr, String dceTimeStr, String ineTimeStr)
        {
            string sNow = DateTime.Now.ToString("HH:mm:ss");
            //TradeClientApp.Log("当前时间为:" + sNow + " 上期所时间为:" + shfeTimeStr);
            int sssTimeNow = ParseTime(sNow);

            //先判断上期所时间
            _ShfeTime = ParseTime(shfeTimeStr);
            _ShfeTimeGap = _ShfeTime - sssTimeNow;
            //TradeClientApp.Log("时间差:" + shfeTimeGap.ToString());

            //中金所时间
            _FeexTime = ParseTime(feexTimeStr);
            if (_FeexTime == -1)
            {
                _FeexTime = _ShfeTime;
            }
            _FeexTimeGap = _FeexTime - sssTimeNow;

            //郑商所时间
            _CzcsTime = ParseTime(czcsTimeStr);
            if (_CzcsTime == -1)
            {
                _CzcsTime = _ShfeTime;
            }
            _CzcsTimeGap = _CzcsTime - sssTimeNow;

            //大商所时间
            _DceTime = ParseTime(dceTimeStr);
            if (_DceTime == -1)
            {
                _DceTime = _ShfeTime;
            }
            _DceTimeGap = _DceTime - sssTimeNow;

            //能源中心时间
            _IneTime = ParseTime(ineTimeStr);
            if (_IneTime == -1)
            {
                _IneTime = _ShfeTime;
            }
            _IneTimeGap = _IneTime - sssTimeNow;
            _ExchTimeMutex = new Mutex();
        }

        /// <summary>
        /// 解析时间
        /// </summary>
        /// <param name="timeFmtString"></param>
        /// <returns></returns>
        private int ParseTime(String timeFmtString)
        {
            if (timeFmtString == null || timeFmtString == "--:--:--") return -1;
            string[] fs = timeFmtString.Split(':');
            if (fs.Length != 3)
            {
                return GetSystemTime();
            }
            int hour, minute, second;
            if (int.TryParse(fs[0], out hour) == false)
            {
                return GetSystemTime();
            }

            if (int.TryParse(fs[1], out minute) == false)
            {
                return GetSystemTime();
            }

            if (int.TryParse(fs[2], out second) == false)
            {
                return GetSystemTime();
            }
            return hour * 3600 + minute * 60 + second;
        }

        private int GetSystemTime()
        {
            DateTime dtNow = DateTime.Now;
            int hour = dtNow.Hour;
            int minute = dtNow.Minute;
            int second = dtNow.Second;
            return hour * 3600 + minute * 60 + second;
        }

        /// <summary>
        /// 过了一秒
        /// </summary>
        internal void SecondPass()
        {
            _ExchTimeMutex.WaitOne();
            string sNow = DateTime.Now.ToString("HH:mm:ss");
            int sssTimeNow = ParseTime(sNow);
            _ShfeTime = sssTimeNow + _ShfeTimeGap;
            _FeexTime = sssTimeNow + _FeexTimeGap;
            _CzcsTime = sssTimeNow + _CzcsTimeGap;
            _DceTime = sssTimeNow + _DceTimeGap;
            _IneTime = sssTimeNow + _IneTimeGap;
            _ExchTimeMutex.ReleaseMutex();
        }
    }
}
