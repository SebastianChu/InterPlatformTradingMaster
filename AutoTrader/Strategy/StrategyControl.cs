using System;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AutoTrader
{
    public abstract class StrategyControl
    {
        protected StrategyControl(TradeDataServer tradeDataServer)
        {
            TradeDataServer = tradeDataServer;
            StatusManager = new StatusManager(tradeDataServer);
            RiskManager = tradeDataServer.RiskManager;
            OrderManager = tradeDataServer.OrderManager;
            IsFollowRules = true;
            tradeDataServer.DepthMarketDataProcessing += StartDepthMarketDataProcessing;
            TradingWatch = new Stopwatch();
            MessageTimer.Elapsed += MessageTimer_Elapsed;
            MessageTimer.Start();
        }

        private void MessageTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (TradingTimeManager.IsInTradingTime(DateTime.Now, "SHFE"))
            {
                //foreach (int dirKey in TradeDataServer.DicPendingOrder.Keys)
                //{
                //    Constant.WriteInfo(StatusManager.GetQuantityofPendingOrderMsg(dirKey));
                //}
                foreach (int posKey in TradeDataServer.PositionFields.Keys)
                {
                    Util.WriteInfo(StatusManager.GetQuantityofPositionMsg(posKey));
                }
            }
        }

        public void StopStrategy()
        {
            IsRunning = false;
        }

        internal OrderManager OrderManager { get; set; }

        internal StatusManager StatusManager { get; set; }

        internal RiskManager RiskManager { get; set; }

        protected TradeDataServer TradeDataServer { get; set; }

        protected abstract void StartDepthMarketDataProcessing(CThostFtdcDepthMarketDataField depthMarketData);

        public virtual void SetNoTradingValue() { }

        protected bool IsFollowRules { get; set; }

        protected System.Timers.Timer MessageTimer = new System.Timers.Timer(20000);

        protected Stopwatch TradingWatch;

        protected bool IsRunning { get; set; }
    }
}
