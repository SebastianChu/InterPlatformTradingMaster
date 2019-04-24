using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Configuration;
using System.Threading.Tasks;

namespace AutoTrader
{
    public class StrategyVectorMA : StrategyControl
    {
        protected StrategyVectorMA(string instantInstrumentID, TradeDataServer tradeDataServer) : base(tradeDataServer)
        {
            InstrumentId = instantInstrumentID;
            if (TradeDataServer.InstrumentFields.ContainsKey(InstrumentId))
            {
                ExchangeId = TradeDataServer.InstrumentFields[InstrumentId].ExchangeId;
            }
            else
            {
                Util.WriteError("No illegal exchange id!");
            }
            Init();
            tradeDataServer.DepthMarketDataInit += StartDepthMarketDataInit;
        }

        protected string InstrumentId { get; set; }

        protected string ExchangeId { get; set; }
        protected double PriceTick { get; set; }
        protected int VolumeMultiple { get; set; }
        protected bool IsSupportCloseToday;

        protected int PendingEndSeconds;
        protected int EndingSeconds;
        protected int LongPosKey;
        protected int ShortPosKey;

        protected int TimeWindow;
        protected int StrategyState;
        protected double MiniWindow;
        protected int MaxDuration;

        protected double PriceMean;
        protected double LastPriceMean;
        protected double LastBidPrice;
        protected double LastAskPrice;
        protected int LastRawVolume;
        protected double LastSignal;
        protected double LastPrice;
        protected double LastOldPrice;
        protected double PresentAskPrice;
        protected double PresentBidPrice;
        //protected int PresentRawVolume;

        protected List<int> DurationLst = new List<int>();
        protected List<int> TotalVolumeLst = new List<int>();
        protected List<double> MidPriceLst = new List<double>();

        protected double PriceVolume;
        protected int AllVolume;
        protected double PriceDuration;
        protected double AllDuration;
        protected int TotalVolume;
        protected int Duration;

        protected double AveragePriceByVolume;
        protected double AveragePriceByDuration;

        protected double MiniFactor;
        protected double PositonLost;
        protected int TargetPosCount;
        protected int JumpLevel;
        protected double UpWarningPrice;
        protected double DownWarningPrice;
        protected double UpLimitPrice;
        protected double DownLimitPrice;
        protected int CancelSec;
        private CThostFtdcDepthMarketDataField LastInitDepthMarketData = new CThostFtdcDepthMarketDataField();
        private CThostFtdcDepthMarketDataField LastDepthMarketData;

        protected override void StartDepthMarketDataProcessing(CThostFtdcDepthMarketDataField depthMarketData)
        {
            if (depthMarketData.InstrumentID == InstrumentId)
            {
                DateTime tickTime = DateTime.Parse(depthMarketData.UpdateTime);
                if (depthMarketData.UpdateMillisec > 0)
                {
                    tickTime = DateTime.ParseExact(string.Format("{0}.{1:d3}", depthMarketData.UpdateTime, depthMarketData.UpdateMillisec), "HH:mm:ss.fff", null);
                }
                if (TradingTimeManager.IsInTradingTime(tickTime, ExchangeId, InstrumentId, true))
                {
                    if ((ExchangeId == "DCE" || ExchangeId == "SHFE") && LastInitDepthMarketData.InstrumentID == InstrumentId)// LastDepthMarketData != null)
                    {
                        DateTime lastTickTime = DateTime.ParseExact(string.Format("{0}.{1:d3}", LastDepthMarketData.UpdateTime, LastDepthMarketData.UpdateMillisec), "HH:mm:ss.fff", null);
                        if (TradingTimeManager.IsInTradingTime(lastTickTime, ExchangeId, InstrumentId, true))
                        {
                            TimeSpan tickGap = TradingTimeManager.GetTimeGap(InstrumentId, tickTime, lastTickTime);
                            int makeUpCycle = (int)Math.Round(tickGap.TotalMilliseconds / 500.0, MidpointRounding.AwayFromZero) - 1;
                            for (int i = 0; i < makeUpCycle; ++i)
                            {
                                QuoteProcessor(LastDepthMarketData);
                            }
                        }
                    }
                    QuoteProcessor(depthMarketData);
                }
                //else if () //开盘前
                //{
                //    SetNoTradingValue();
                //}
                //if (Constant.IsInNightBreakTime(InstrumentId, ExchangeId))
                //{
                //    SetNoTradingValue();
                //}
                LastDepthMarketData = depthMarketData;
            }
        }

        protected void StartDepthMarketDataInit(string instrumentId, List<CThostFtdcDepthMarketDataField> depthMarketDataLst)
        {
            if (instrumentId == InstrumentId)
            {
                foreach (CThostFtdcDepthMarketDataField depthMarketData in depthMarketDataLst)
                {
                    if (LastInitDepthMarketData.InstrumentID == InstrumentId && (LastInitDepthMarketData.Volume > depthMarketData.Volume && LastInitDepthMarketData.Turnover.IsGreaterThan(depthMarketData.Turnover)
                        || LastInitDepthMarketData.UpdateTime == depthMarketData.UpdateTime
                        && LastInitDepthMarketData.Volume == depthMarketData.Volume && LastInitDepthMarketData.Turnover == depthMarketData.Turnover
                        && LastInitDepthMarketData.BidPrice1 == depthMarketData.BidPrice1 && LastInitDepthMarketData.BidVolume1 == depthMarketData.BidVolume1
                        && LastInitDepthMarketData.AskPrice1 == depthMarketData.AskPrice1 && LastInitDepthMarketData.AskVolume1 == depthMarketData.AskVolume1
                        && LastInitDepthMarketData.LastPrice == depthMarketData.LastPrice))
                    {
                        Util.WriteInfo(string.Format("Drop {0} duplicated data: {1}, {2}", InstrumentId, depthMarketData.UpdateTime, depthMarketData.Turnover));
                        continue;
                    }
                    DateTime tickTime = DateTime.Parse(depthMarketData.UpdateTime);
                    if (depthMarketData.UpdateMillisec > 0)
                    {
                        tickTime = DateTime.ParseExact(string.Format("{0}.{1:d3}", depthMarketData.UpdateTime, depthMarketData.UpdateMillisec), "HH:mm:ss.fff", null);
                    }
                    //tickTime = tickTime.AddDays(-1);
                    if (TradingTimeManager.IsInTradingTime(tickTime, ExchangeId, InstrumentId, true))
                    {
                        if ((ExchangeId == "DCE" || ExchangeId == "SHFE") && LastInitDepthMarketData.InstrumentID == InstrumentId )//&& LastDepthMarketData != null)
                        {
                            DateTime lastTickTime = DateTime.Parse(LastDepthMarketData.UpdateTime);
                            if (LastDepthMarketData.UpdateMillisec > 0)
                            {
                                lastTickTime = DateTime.ParseExact(string.Format("{0}.{1:d3}", LastDepthMarketData.UpdateTime, LastDepthMarketData.UpdateMillisec), "HH:mm:ss.fff", null);
                            }
                            //lastTickTime = lastTickTime.AddDays(-1);
                            if (TradingTimeManager.IsInTradingTime(lastTickTime, ExchangeId, InstrumentId, true))
                            {
                                TimeSpan tickGap = TradingTimeManager.GetTimeGap(InstrumentId, tickTime, lastTickTime);
                                int makeUpCycle = (int)Math.Round(tickGap.TotalMilliseconds / 500.0, MidpointRounding.AwayFromZero) - 1;
                                for (int i = 0; i < makeUpCycle; ++i)
                                {
                                    QuoteCalculator(LastDepthMarketData);
                                }
                            }
                        }
                        if (QuoteCalculator(depthMarketData))
                        {
                            ExecuteStrategy();
                            if (PriceMean > 0)
                            {
                                LastPriceMean = PriceMean;
                            }
                            LastOldPrice = LastPrice;
                            LastAskPrice = depthMarketData.AskPrice1;
                            LastBidPrice = depthMarketData.BidPrice1;
                            LastRawVolume = depthMarketData.Volume;
                        }
                    }
                    LastDepthMarketData = depthMarketData;
                }
                LastInitDepthMarketData = depthMarketDataLst.Count > 0 ? depthMarketDataLst[depthMarketDataLst.Count - 1] : new CThostFtdcDepthMarketDataField();
                if (depthMarketDataLst.Count > 0)
                {
                    Util.WriteInfo(string.Format("Initiated {0} data: {1} items", InstrumentId, depthMarketDataLst.Count));
                }
            }
        }

        private void Init()
        {
            VolumeMultiple = TradeDataServer.InstrumentFields[InstrumentId].VolumeMultiple;
            PriceTick = TradeDataServer.InstrumentFields[InstrumentId].PriceTick;
            MiniWindow = 0.0;
            LastOldPrice = LastPriceMean = LastAskPrice = LastBidPrice = 0.0;
            AveragePriceByVolume = AveragePriceByDuration = 0.0;
            ShortPosKey = Util.GetPositionKey(InstrumentId, EnumThostPosiDirectionType.Short);
            LongPosKey = Util.GetPositionKey(InstrumentId, EnumThostPosiDirectionType.Long);
            IsSupportCloseToday = TradeDataServer.IsShfeRule(InstrumentId);
            PendingEndSeconds = int.Parse(ConfigurationManager.AppSettings["PendingEndSeconds"]);
            EndingSeconds = int.Parse(ConfigurationManager.AppSettings["EndingSeconds"]);
            CancelSec = int.Parse(ConfigurationManager.AppSettings["OverTimeSec"]);
        }

        public override void SetNoTradingValue()
        {
            LastOldPrice = LastPriceMean = LastAskPrice = LastBidPrice = 0.0;
            AveragePriceByVolume = AveragePriceByDuration = 0.0;
            LastRawVolume = 0;
            //UpWarningPrice = DownWarningPrice = 0.0;
            DurationLst.Clear();
            TotalVolumeLst.Clear();
            MidPriceLst.Clear();
            PriceVolume = PriceDuration = AllDuration = 0.0;
            TotalVolume = AllVolume = 0;
            Util.WriteInfo("SetNoTradingValue Works.");
        }

        private void QuoteProcessor(CThostFtdcDepthMarketDataField depthMarketDataField)
        {
            TradingWatch = Stopwatch.StartNew();
            QuoteCalculator(depthMarketDataField);
            CheckCancelOrder();
            // Need Override
            ExecuteStrategy();

            if (PriceMean > 0)
            {
                LastPriceMean = PriceMean;
            }
            LastAskPrice = depthMarketDataField.AskPrice1;
            LastBidPrice = depthMarketDataField.BidPrice1;
            LastRawVolume = depthMarketDataField.Volume;
            LastOldPrice = LastPrice;
        }

        private bool QuoteCalculator(CThostFtdcDepthMarketDataField depthMarketDataField)
        {
            if (!TradeDataServer.InstrumentFields.ContainsKey(InstrumentId))
            {
                return false;
            }
            if (UpWarningPrice == DownWarningPrice && UpWarningPrice == 0.0)
            {
                int quoteLimitTick = int.Parse(ConfigurationManager.AppSettings["QuoteLimitTick"]);
                UpLimitPrice = depthMarketDataField.UpperLimitPrice;
                UpWarningPrice = depthMarketDataField.UpperLimitPrice - quoteLimitTick * PriceTick;
                DownLimitPrice = depthMarketDataField.LowerLimitPrice;
                DownWarningPrice = depthMarketDataField.LowerLimitPrice + quoteLimitTick * PriceTick;
                Util.WriteInfo(string.Format("{0}, UpWarningPrice {1}, DownWarningPrice {2}", InstrumentId, UpWarningPrice, DownWarningPrice));
            }

            LastPrice = depthMarketDataField.LastPrice;
            PresentAskPrice = depthMarketDataField.AskPrice1;
            PresentBidPrice = depthMarketDataField.BidPrice1;
            //PresentRawVolume = depthMarketDataField.Volume;
            int tradedVolume = (depthMarketDataField.Volume - LastRawVolume) / 2;

            if (LastAskPrice.IsEqualTo(LastBidPrice) && LastBidPrice.IsEqualTo(0.0) && LastPriceMean.IsEqualTo(0.0) && LastOldPrice.IsEqualTo(LastBidPrice))
            {
                LastAskPrice = depthMarketDataField.AskPrice1;
                LastBidPrice = depthMarketDataField.BidPrice1;
                LastRawVolume = depthMarketDataField.Volume;//PresentRawVolume;
                LastOldPrice = LastPrice;
                LastPriceMean = LastPrice;
                Duration = 1;
                TotalVolume = 1; //TotalVolume = tradedVolume;554
                PriceMean = LastPrice;
                AveragePriceByVolume = PriceMean;
                AveragePriceByDuration = PriceMean;
                return false;
            }

            PriceMean = GetPriceMean(depthMarketDataField.AskPrice1, LastAskPrice, depthMarketDataField.BidPrice1, LastBidPrice, LastPriceMean, LastPrice, PriceTick);
            GetMovingAverage(PriceMean, tradedVolume, TimeWindow);
            return true;
        }

        protected virtual void ExecuteStrategy() { }

        private double GetPriceMean(double presentAsk, double lastAsk, double presentBid, double lastBid, double lastPriceMean, double lastPrice, double priceTick)
        {
            double priceMean = 0;

            if (presentAsk.IsEqualTo(0.0) || presentBid.IsEqualTo(0.0))
                priceMean = lastPrice;

            if ((presentAsk - presentBid).IsGreaterThan(priceTick))
            {
                if (lastPrice.IsGreaterThanOrEqualTo(presentAsk))
                    priceMean = presentAsk - priceTick;
                else if (lastPrice.IsLessThanOrEqualTo(presentBid))
                    priceMean = presentBid + priceTick;
                else
                    priceMean = lastPrice;
            }

            if ((presentAsk - presentBid).IsEqualTo(priceTick))
            {
                if ((presentBid + presentAsk).IsEqualTo(lastAsk + lastBid))
                    priceMean = lastPriceMean;
                else if ((presentBid + presentAsk).IsGreaterThan(lastAsk + lastBid))
                    priceMean = presentBid;
                else
                    priceMean = presentAsk;
            }
            return priceMean;
        }


        private void GetMovingAverage(double priceMean, int tradedVolume, int timeWindow)
        {
            if (priceMean == LastPriceMean && Duration < MaxDuration)
            {
                Duration += 1;
                TotalVolume += tradedVolume;

                AveragePriceByVolume = (LastPriceMean * TotalVolume + PriceVolume) / (TotalVolume + AllVolume);
                AveragePriceByDuration = (LastPriceMean * Duration + PriceDuration) / (Duration + AllDuration);
            }
            else if (LastPriceMean.IsGreaterThan(0.0))
            {
                DurationLst.Add(Duration);
                TotalVolumeLst.Add(TotalVolume);
                MidPriceLst.Add(LastPriceMean);
                // 累加
                PriceVolume += LastPriceMean * TotalVolume;
                AllVolume += TotalVolume;
                PriceDuration += LastPriceMean * Duration;
                AllDuration += Duration;

                if (DurationLst.Count == timeWindow + 1)
                {
                    //remove the first info of the list
                    PriceVolume -= MidPriceLst[0] * TotalVolumeLst[0];
                    PriceDuration -= MidPriceLst[0] * DurationLst[0];
                    AllVolume -= TotalVolumeLst[0];
                    AllDuration -= DurationLst[0];

                    DurationLst.RemoveAt(0);
                    TotalVolumeLst.RemoveAt(0);
                    MidPriceLst.RemoveAt(0);
                }

                //LastPriceMean = priceMean; // ?
                Duration = 1;
                //TotalVolume = tradedVolume; // tradevolume at least 1: max(tradevolume, 1)
                TotalVolume = Math.Max(tradedVolume, 1);

                AveragePriceByVolume = (priceMean * TotalVolume + PriceVolume) / (TotalVolume + AllVolume);
                AveragePriceByDuration = (priceMean * Duration + PriceDuration) / (Duration + AllDuration);
            }
        }

        protected void CheckCancelOrder()//(string instrumentId, double PresentBidPrice, double PresentAskPrice)
        {
            foreach (int key in TradeDataServer.DicPendingOrder.Keys)
            {
                foreach (CThostFtdcOrderField item in TradeDataServer.DicPendingOrder[key].Values)
                {
                    if (item.InstrumentID == InstrumentId)
                    {
                        if (OrderManager.IsOrderOverTime(item, CancelSec) && OrderManager.IsOrderOverTick(item, PresentBidPrice, PresentAskPrice))
                        {
                            OrderManager.CancelOrder(item);
                        }
                    }
                }
            }
        }
    }
}