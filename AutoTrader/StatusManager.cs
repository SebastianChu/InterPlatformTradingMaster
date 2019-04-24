using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTrader
{
    public class StatusManager
    {
        public StatusManager(TradeDataServer tradeDataServer)
        {
            TradeDataServer = tradeDataServer;
        }

        public TradeDataServer TradeDataServer { get; set; }

        //public List<CThostFtdcOrderField> GetSubmitOrderListofPendingOrder(string strategyCode, string issueCode, int buysell, int openclose)
        //{
        //    List<CThostFtdcOrderField> lstSubmitOrder = new List<CThostFtdcOrderField>();

        //    if (TradeDataServer.DicPendingOrder.ContainsKey(strategyCode))
        //    {
        //        for (int i = 0; i < TradeDataServer.DicPendingOrder[strategyCode].Count(); i++)
        //        {
        //            if (TradeDataServer.DicPendingOrder[strategyCode][i].InstrumentID == issueCode && TradeDataServer.DicPendingOrder[strategyCode][i].BuySell == buysell && TradeDataServer.DicPendingOrder[strategyCode][i].OpenClose == openclose)
        //            {
        //                CThostFtdcOrderField oneSubmitOrder = new CThostFtdcOrderField();
        //                //oneSubmitOrder.entrustID = TradeDataServer.DicPendingOrder[strategyCode][i].entrustID;
        //                oneSubmitOrder.OrderSysID = TradeDataServer.DicPendingOrder[strategyCode][i].OrderID;
        //                oneSubmitOrder.InstrumentID = TradeDataServer.DicPendingOrder[strategyCode][i].Code;
        //                oneSubmitOrder.BuySell = TradeDataServer.DicPendingOrder[strategyCode][i].BuySell;
        //                oneSubmitOrder.OpenClose = TradeDataServer.DicPendingOrder[strategyCode][i].OpenClose;
        //                oneSubmitOrder.OrderStatus = TradeDataServer.DicPendingOrder[strategyCode][i].OrderStatus;
        //                oneSubmitOrder.CommitPrice = TradeDataServer.DicPendingOrder[strategyCode][i].CommitPrice;
        //                oneSubmitOrder.CommitHandCount = TradeDataServer.DicPendingOrder[strategyCode][i].CommitHandCount;
        //                oneSubmitOrder.TradeHandCount = TradeDataServer.DicPendingOrder[strategyCode][i].TradeHandCount;
        //                oneSubmitOrder.CommitTime = TradeDataServer.DicPendingOrder[strategyCode][i].CommitTime;
        //                oneSubmitOrder.StrategyCode = TradeDataServer.DicPendingOrder[strategyCode][i].StrategyCode;
        //                oneSubmitOrder.Exchange = TradeDataServer.DicPendingOrder[strategyCode][i].Exchange;
        //                oneSubmitOrder.OrderRef = TradeDataServer.DicPendingOrder[strategyCode][i].OrderRef;

        //                lstSubmitOrder.Add(oneSubmitOrder);
        //            }
        //        }
        //    }

        //    return lstSubmitOrder;
        //}

        //public int HasAOperatingOrder(List<CThostFtdcOrderField> lstSubmitOrder) // -1: none; 0: unsend; 1: regular pending order
        //{
        //    if (lstSubmitOrder.Count() == 0) return -1;

        //    for (int i = 0; i < lstSubmitOrder.Count(); i++)
        //    {
        //        if (lstSubmitOrder[i].OrderStatus == (int)OrderStatus.Submitted || lstSubmitOrder[i].OrderStatus == (int)OrderStatus.ConditionSubmmited)
        //            return 0;
        //    }

        //    return 1;
        //}

        public string GetQuantityofPendingOrderMsg(int directionKey)
        {
            string msg = "";
            //string[] dirKeyGrp = directionKey.Split(':');
            int openOrderCount = 0, closeOrderCount = 0;
            if (TradeDataServer.DicPendingOrder.ContainsKey(directionKey))
            {
                foreach (string orderKey in TradeDataServer.DicPendingOrder[directionKey].Keys)
                {
                    CThostFtdcOrderField orderField = TradeDataServer.DicPendingOrder[directionKey][orderKey];
                    if (orderField.CombOffsetFlag_0 == EnumThostOffsetFlagType.Open)
                    {
                        openOrderCount += orderField.VolumeTotal - orderField.VolumeTraded;
                    }
                    else
                    {
                        closeOrderCount += orderField.VolumeTotal - orderField.VolumeTraded;
                    }
                }
                msg = string.Format("EntrustStatus: {0} = Open {1}, Close {2}", directionKey, openOrderCount, closeOrderCount);
            }
            else
            {
                msg = string.Format("EntrustStatus: {0} = Open {1}, Close {2}", directionKey, openOrderCount, closeOrderCount);
            }
            return msg;
        }

        public string GetQuantityofPositionMsg(int posKey) 
        {
            string msg = "";
            //string[] posKeyGrp = posKey.Split(':');
            if (TradeDataServer.PositionFields.ContainsKey(posKey))
            {
                msg = string.Format("PosStatus: {0} = Position {1}, Avail {2}, TodayPosition {3}, AvailToday {4}"
                    , posKey
                    , TradeDataServer.PositionFields[posKey].Position
                    , TradeDataServer.PositionFields[posKey].Avail
                    , TradeDataServer.PositionFields[posKey].TodayPosition//?AvailTodayLong
                    , TradeDataServer.PositionFields[posKey].AvailToday
                    );
            }
            else
            {
                msg = string.Format("PosStatus: {0} = Position {1}, Avail {2}, TodayPosition {3}, AvailToday {4}", posKey, 0, 0, 0, 0);
            }
            return msg;
        }


    }
}
