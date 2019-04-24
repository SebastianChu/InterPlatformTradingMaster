using System.Collections.Generic;
using System.Configuration;

namespace AutoTrader
{
    public class RiskManager
    {
        public RiskManager(TradeDataServer tradeDataServer)
        {
            TradeDataServer = tradeDataServer;
            _DiscountRiskRate = double.Parse(ConfigurationManager.AppSettings["RiskRate"]);
            MaxLimitOrderVolumeDic = new Dictionary<string, int>();
            MaxMarketOrderVolumeDic = new Dictionary<string, int>();

            SelfTradeCountDic = new Dictionary<string, int>();
            _MaxCancelOrderCountDic = new Dictionary<string, int>();
            _LargeCancelOrderCountDic = new Dictionary<string, int>();
            _LargeCancelVolumeDic = new Dictionary<string, bool>();
        }

        public TradeDataServer TradeDataServer { get; set; }

        private double _DiscountRiskRate = 1.0;

        //自成交
        //客户单日某一合约上自成交次数超过5次（含5次）
        private Dictionary<string, int> SelfTradeCountDic;

        //频繁报撤单
        //SHFE:  客户单日在某一合约上的撤单次数超过500次（含500次）
        //CFFEX: 股指期货单个合约每日报撤单行为达到或超过400次的;国债期货，单个合约每日报撤单行为达到或超过500次的
        //CZCE:  客户单日在某一合约撤销定单笔数500笔（含本数）以上；当某个交易日某一合约停板后，客户当日在涨停板价位买单撤量或者在跌停板价位卖单撤量超过100笔的，属于频繁报撤单行为。
        //DCE:   客户单日在某一合约上的撤单次数达到500次（含500次）以上
        private Dictionary<string, int> _MaxCancelOrderCountDic;

        //大额报撤单
        //SHFE:  客户单日在某一合约上的大额撤单次数超过50次（含50次）。单笔撤单的撤单量达到300手以上（含300手），视作大额报撤单
        //CFFEX: 单日在某一合约上的撤单次数超过100次（含），且单笔撤单量达到或者超过交易所规定的限价指令每次最大下单数量的80%的
        //CZCE:  客户单日在某一合约撤单笔数50笔（含本数）以上且每笔撤单量800手（含本数）以上
        //DCE:   单日在某一合约上的撤单次数达到400次（含400次）以上的，且单笔撤单的撤单量超过合约最大下单手数的80%
        private Dictionary<string, int> _LargeCancelOrderCountDic;
        private Dictionary<string, bool> _LargeCancelVolumeDic;

        //合约最大限价下单量
        public Dictionary<string, int> MaxLimitOrderVolumeDic { get; set; }
        //合约最大市价下单量
        public Dictionary<string, int> MaxMarketOrderVolumeDic { get; set; }

        //合约限仓（投机）
        //private Dictionasry<string, int> _MaxPositionLimtDic;

        //CZCE:  利用对倒、对敲等手段交易，造成交割月合约价格异常波动幅度达到该合约涨跌停板幅度40%以上，影响该合约交割结算价

        public bool FollowCancelOrderRule(string instrumentId)
        {
            string exchangeId = "";
            if (!TradeDataServer.InstrumentFields.ContainsKey(instrumentId))
            {
                Util.WriteWarn(string.Format("{0} has not been initialized", instrumentId));
                exchangeId = "SHFE";
            }
            exchangeId = TradeDataServer.InstrumentFields[instrumentId].ExchangeId;
            int maxCancelCount = _MaxCancelOrderCountDic.ContainsKey(instrumentId) ? _MaxCancelOrderCountDic[instrumentId] : 0;
            int maxLargeVolume = _LargeCancelOrderCountDic.ContainsKey(instrumentId) ? _LargeCancelOrderCountDic[instrumentId] : 0;
            if (exchangeId == "SHFE")
            {
                if (maxCancelCount >= 500 * _DiscountRiskRate)
                {
                    return false;
                }
                else if (maxLargeVolume >= 50 * _DiscountRiskRate)
                {
                    return false;
                }
                return true;
            }
            else if (exchangeId == "CFFEX")
            {
                if (instrumentId.StartsWith("I") && maxCancelCount >= 400 * _DiscountRiskRate)
                {
                    return false;
                }
                else if (instrumentId.StartsWith("T") && maxCancelCount >= 500 * _DiscountRiskRate)
                {
                    return false;
                }
                if (maxCancelCount >= 100 * _DiscountRiskRate && _LargeCancelVolumeDic.ContainsKey(instrumentId) && _LargeCancelVolumeDic[instrumentId])
                {
                    return false;
                }
                return true;
            }
            else if (exchangeId == "CZCE")
            {
                if (maxCancelCount >= 500 * _DiscountRiskRate) // ?Todo:
                {
                    return false;
                }
                else if (maxLargeVolume >= 50 * _DiscountRiskRate)
                {
                    return false;
                }
                return true;
            }
            else if (exchangeId == "DCE")
            {
                if (maxCancelCount >= 500 * _DiscountRiskRate)
                {
                    return false;
                }
                if (maxLargeVolume >= 400 * _DiscountRiskRate && _LargeCancelVolumeDic.ContainsKey(instrumentId) && _LargeCancelVolumeDic[instrumentId])
                {
                    return false;
                }
                return true;
            }
            else
            {
                Util.WriteError(string.Format("Illegal exchange id: {0}", exchangeId));
                return false;
            }
        }

        public void SetCancelOrderCount(string code, int cancelCount)
        {
            if (_MaxCancelOrderCountDic.ContainsKey(code))
            {
                _MaxCancelOrderCountDic[code] += cancelCount;
            }
            else
            {
                _MaxCancelOrderCountDic.Add(code, cancelCount);
            }
        }

        public void SetLargeCancelOrder(string code, string exchangeId, int handCount)
        {
            bool needSet = false;
            bool isMaxSet = false;
            if (exchangeId == "SHFE")
            {
                if (handCount >= 300 * _DiscountRiskRate)
                {
                    needSet = true;
                }
            }
            else if (exchangeId == "CFFEX")
            {
                if (handCount >= 0.8 * MaxLimitOrderVolumeDic[code] * _DiscountRiskRate)
                {
                    isMaxSet = true;
                }
            }
            else if (exchangeId == "CZCE")
            {
                if (handCount >= 800 * _DiscountRiskRate)
                {
                    needSet = true;
                }
            }
            else if (exchangeId == "DCE")
            {
                if (handCount >= 0.8 * MaxLimitOrderVolumeDic[code] * _DiscountRiskRate)
                {
                    isMaxSet = true;
                }
            }
            else
            {
                Util.WriteError(string.Format("Illegal exchange id: {0}", exchangeId));
            }
            if (needSet)
            {
                if (_LargeCancelOrderCountDic.ContainsKey(code))
                {
                    _LargeCancelOrderCountDic[code] += 1;
                }
                else
                {
                    _LargeCancelOrderCountDic.Add(code, 1);
                }
            }
            if (isMaxSet)
            {
                if (_LargeCancelVolumeDic.ContainsKey(code))
                {
                    _LargeCancelVolumeDic[code] = true;
                }
                else
                {
                    _LargeCancelVolumeDic.Add(code, true);
                }
            }
        }
    }
}
