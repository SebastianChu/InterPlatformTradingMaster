using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader
{
    public class StrategyTargetPrice : StrategyBaseTarget
    {
        public StrategyTargetPrice(string instantInstrumentID, TradeDataServer tradeDataServer, AlgorithmicTrader algoTrader)
            : base(instantInstrumentID, tradeDataServer, algoTrader)
        {
            InitParameters();
            _IsEnteredMarket = IsInEntryStatus();
        }

        public override void InitParameters()
        {
            StrategyId = Util.StrategyParameter[InstrumentId]["TargetPrice"]["StrategyId"];
            EntryDirection = int.Parse(Util.StrategyParameter[InstrumentId]["TargetPrice"]["EntryDirection"]) == 1 ? EnumThostDirectionType.Buy : EnumThostDirectionType.Sell;
            //(EnumThostDirectionType)Enum.Parse(typeof(EnumThostDirectionType), Constant.StrategyParameter[instantInstrumentID]["TargetPrice"]["EntryDirection"].ToString());
            CancelPendingSec = int.Parse(Util.StrategyParameter[InstrumentId]["TargetPrice"]["CancelPendingSeconds"]);
            TargetVolume = int.Parse(Util.StrategyParameter[InstrumentId]["TargetPrice"]["TargetVolume"]);
            MaxVolume = int.Parse(Util.StrategyParameter[InstrumentId]["TargetPrice"]["MaxVolume"]);
            _BuyInPrice = double.Parse(Util.StrategyParameter[InstrumentId]["TargetPrice"]["BuyInPrice"]);
            _BuyOutPrice = double.Parse(Util.StrategyParameter[InstrumentId]["TargetPrice"]["BuyOutPrice"]);
            _SellInPrice = double.Parse(Util.StrategyParameter[InstrumentId]["TargetPrice"]["SellInPrice"]);
            _SellOutPrice = double.Parse(Util.StrategyParameter[InstrumentId]["TargetPrice"]["SellOutPrice"]);
            IsRunning = true;
        }

        private double _BuyInPrice;
        private double _BuyOutPrice;
        private double _SellInPrice;
        private double _SellOutPrice;
        private bool _IsEnteredMarket;

        protected override void StrategyTrigger(DateTime tickTime, CThostFtdcDepthMarketDataField depthMarketData)
        {
            CancelPendingOrders(depthMarketData);
            double askPrice = depthMarketData.AskPrice1 > 0 ? Math.Min(depthMarketData.AskPrice1, depthMarketData.UpperLimitPrice) : depthMarketData.UpperLimitPrice;
            double bidPrice = depthMarketData.BidPrice1 > 0 ? Math.Max(depthMarketData.BidPrice1, depthMarketData.LowerLimitPrice) : depthMarketData.LowerLimitPrice;

            EnumThostDirectionType direction = EntryDirection;
            if (direction == EnumThostDirectionType.Buy && !Util.IsSentBuyOrder[InstrumentId])
            {
                if (!_IsEnteredMarket && askPrice >= _BuyInPrice && askPrice < _BuyOutPrice)
                {
                    int hands = TargetVolume;
                    int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Buy, askPrice, hands, false, StrategyId.ToString(), EnumOrderType.Limit, true, true, true, MaxVolume);
                    if (st == 0)
                    {
                        TradingWatch.Stop();
                        string lastOrderRef = OrderManager.CombineOrderRef;
                        if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                        {
                            Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                        }
                        if (!Util.IsInReLoadingStatus(InstrumentId))
                        {
                            Util.WriteInfo(string.Format("Submit Timing Order {0} ticks, {1} ns, {2}, {3}, {4}"
                                , TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.MicrosecPerTick, EnumThostDirectionType.Buy, hands, askPrice), Program.InPrintMode);
                        }
                        _IsEnteredMarket = true;
                    }
                }
                else if (_IsEnteredMarket && bidPrice >= _BuyOutPrice)
                {
                    int hands = TargetVolume; //??volume
                    int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Sell, bidPrice, hands, false, StrategyId.ToString(), EnumOrderType.Limit, true, true, true, MaxVolume);
                    if (st == 0)
                    {
                        TradingWatch.Stop();
                        string lastOrderRef = OrderManager.CombineOrderRef;
                        if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                        {
                            Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                        }
                        if (!Util.IsInReLoadingStatus(InstrumentId))
                        {
                            Util.WriteInfo(string.Format("Submit Timing Order {0} ticks, {1} ns, {2}, {3}, {4}"
                                , TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.MicrosecPerTick, EnumThostDirectionType.Sell, hands, bidPrice), Program.InPrintMode);
                        }
                        _IsEnteredMarket = false;
                    }
                }
            }

            if (direction == EnumThostDirectionType.Sell && !Util.IsSentSellOrder[InstrumentId])
            {
                if (!_IsEnteredMarket && bidPrice <= _SellInPrice && bidPrice > _SellOutPrice)
                {
                    int hands = TargetVolume;
                    int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Sell, bidPrice, hands, false, StrategyId.ToString(), EnumOrderType.Limit, true, true, true, MaxVolume);
                    if (st == 0)
                    {
                        TradingWatch.Stop();
                        string lastOrderRef = OrderManager.CombineOrderRef;
                        if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                        {
                            Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                        }
                        if (!Util.IsInReLoadingStatus(InstrumentId))
                        {
                            Util.WriteInfo(string.Format("Submit Timing Order {0} ticks, {1} ns, {2}, {3}, {4}"
                                , TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.MicrosecPerTick, EnumThostDirectionType.Sell, hands, bidPrice), Program.InPrintMode);
                        }
                        _IsEnteredMarket = true;
                    }
                }
                else if (_IsEnteredMarket && askPrice <= _SellOutPrice)
                {
                    int hands = TargetVolume; //??volume
                    int st = OrderManager.SubmitOrder(InstrumentId, EnumThostDirectionType.Buy, askPrice, hands, false, StrategyId.ToString(), EnumOrderType.Limit, true, true, true, MaxVolume);
                    if (st == 0)
                    {
                        TradingWatch.Stop();
                        string lastOrderRef = OrderManager.CombineOrderRef;
                        if (Util.TradingReportDict.ContainsKey(lastOrderRef))
                        {
                            Util.TradingReportDict[lastOrderRef].InsertCostTime = TradingWatch.ElapsedTicks * Util.MicrosecPerTick;
                        }
                        if (!Util.IsInReLoadingStatus(InstrumentId))
                        {
                            Util.WriteInfo(string.Format("Submit Timing Order {0} ticks, {1} ns, {2}, {3}, {4}"
                                , TradingWatch.ElapsedTicks, TradingWatch.ElapsedTicks * Util.MicrosecPerTick, EnumThostDirectionType.Buy, hands, askPrice), Program.InPrintMode);
                        }
                        _IsEnteredMarket = false;
                    }
                }

            }
        }
    }
}
