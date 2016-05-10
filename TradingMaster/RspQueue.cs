using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TradingMaster.JYData;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace TradingMaster
{
    public class RspQueue
    {
        private Task _RspTask;
        //private Thread RspThread;
        private SynQueue<object> _ResponseQueue;
        private object _LockObject;
        private BACKENDTYPE _BackEnd;
        //private bool ExecFlag = true;

        public RspQueue(CancellationTokenSource cts, BACKENDTYPE backEnd)
        {
            _BackEnd = BACKENDTYPE.CTP;
            _ResponseQueue = new SynQueue<object>();
            _LockObject = new object();

            _RspTask = Task.Factory.StartNew(() => ResponseThreadProc(cts.Token), cts.Token);
            //RspThread = new Thread(ResponseThreadProc);
            //RspThread.IsBackground = true;
            //RspThread.Start();
        }

        private void ResponseThreadProc(CancellationToken ct)
        {
            List<CThostFtdcOrderField> orderDataList = new List<CThostFtdcOrderField>();
            List<CThostFtdcTradeField> tradeDataList = new List<CThostFtdcTradeField>();
            List<CThostFtdcQuoteField> quoteOrderDataList = new List<CThostFtdcQuoteField>();
            List<CThostFtdcExecOrderField> execOrderDataList = new List<CThostFtdcExecOrderField>();
            List<CThostFtdcDepthMarketDataField> marketDataList = new List<CThostFtdcDepthMarketDataField>();
            while ((!ct.IsCancellationRequested))//(ExecFlag)
            {
                try
                {
                    // Dequeue All
                    //int count = _ResponseQueue.Count;
                    //List<object> dataList = new List<object>();
                    //for (int i = 0; i < count; i++)
                    //{
                    //    object item;
                    //    if (_ResponseQueue.TryDequeue(out item))
                    //    {
                    //        dataList.Add(item);
                    //    }
                    //}
                    List<object> dataList = _ResponseQueue.DequeueAll();
                    foreach (object data in dataList)
                    {
                        if (_BackEnd == BACKENDTYPE.CTP)
                        {
                            if (data is CThostFtdcOrderField)
                            {
                                orderDataList.Add((CThostFtdcOrderField)data);
                            }
                            else if (data is CThostFtdcTradeField)
                            {
                                tradeDataList.Add((CThostFtdcTradeField)data);
                            }
                            else if (data is CThostFtdcQuoteField)
                            {
                                quoteOrderDataList.Add((CThostFtdcQuoteField)data);
                            }
                            else if (data is CThostFtdcExecOrderField)
                            {
                                execOrderDataList.Add((CThostFtdcExecOrderField)data);
                            }
                            else if (data is CThostFtdcDepthMarketDataField)
                            {
                                marketDataList.Add((CThostFtdcDepthMarketDataField)data);
                            }
                            else
                            {
                                Util.Log("RspQueue: Error for CTP Response Data!");
                            }
                        }
                        else if (_BackEnd == BACKENDTYPE.FEMAS)
                        {
                            //if (data is CUstpFtdcOrderField)
                            //{
                            //    orderDataList.Add((CUstpFtdcOrderField)data);
                            //}
                            //else if (data is CThostFtdcTradeField)
                            //{
                            //    tradeDataList.Add((CUstpFtdcTradeField)data);
                            //}
                            //else if (data is CUstpFtdcDepthMarketDataField)
                            //{
                            //    marketDataList.Add((CUstpFtdcDepthMarketDataField)data);
                            //}
                            //else
                            //{
                            //    Util.Log("RspQueue: Error for CTP Response Data!");
                            //}
                        }
                    }

                    if (orderDataList.Count > 0)
                    {
                        Util.Log("Processing Order Data...");
                        CreateFromOrderData(orderDataList);
                        orderDataList.Clear();
                    }
                    if (tradeDataList.Count > 0)
                    {
                        Util.Log("Processing Trade Data...");
                        CreateFromTradeData(tradeDataList);
                        tradeDataList.Clear();
                    }
                    if (quoteOrderDataList.Count > 0)
                    {
                        Util.Log("Processing Quote Order Data...");
                        CreateFromQuoteOrderData(quoteOrderDataList);
                        quoteOrderDataList.Clear();
                    }
                    if (execOrderDataList.Count > 0)
                    {
                        Util.Log("Processing Execution Data...");
                        CreateFromExecOrderData(execOrderDataList);
                        execOrderDataList.Clear();
                    }
                    if (marketDataList.Count > 0)
                    {
                        CreateFromMarketData(marketDataList);
                        marketDataList.Clear();
                    }
                    //if (dataList.Count > 0)
                    //{
                    //    Util.Log("Response Thread has processed all data!");
                    //}
                }
                catch (Exception ex)
                {
                    Util.Log("exception: " + ex.Message);
                    Util.Log("exception: " + ex.StackTrace);
                    orderDataList.Clear();
                    tradeDataList.Clear();
                    quoteOrderDataList.Clear();
                    execOrderDataList.Clear();
                    marketDataList.Clear();
                    ct.ThrowIfCancellationRequested();
                }
            }
        }

        public void Enqueue(object data)
        {
            if (data != null)
            {
                _ResponseQueue.Enqueue(data, false);
            }
        }

        private void CreateFromOrderData(List<CThostFtdcOrderField> orderDataList)
        {
            try
            {
                //foreach (CThostFtdcOrderField pOrder in orderDataList)
                //{
                //    CtpDataServer.getServerInstance().OnSingleRtnOrder(pOrder);
                //}
                //CtpDataServer.getServerInstance().ProcessOrderData();
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        private void CreateFromTradeData(List<CThostFtdcTradeField> tradeDataList)
        {
            try
            {
                //foreach (CThostFtdcTradeField pTrade in tradeDataList)
                //{
                //    CtpDataServer.getServerInstance().OnSingleRtnTrade(pTrade);
                //}
                //CtpDataServer.getServerInstance().ProcessTradeData();
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        private void CreateFromQuoteOrderData(List<CThostFtdcQuoteField> quoteOrderDataList)
        {
            try
            {
                //foreach (CThostFtdcQuoteField pQuote in quoteOrderDataList)
                //{
                //    CtpDataServer.getServerInstance().OnSingleRtnQuote(pQuote);
                //}
                //CtpDataServer.getServerInstance().ProcessQuoteOrderData();
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        private void CreateFromExecOrderData(List<CThostFtdcExecOrderField> execOrderDataList)
        {
            try
            {
                //foreach (CThostFtdcExecOrderField pExecOrder in execOrderDataList)
                //{
                //    CtpDataServer.getServerInstance().OnSingleRtnExecOrder(pExecOrder);
                //}
                //CtpDataServer.getServerInstance().ProcessExecOrderData();
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        private void CreateFromMarketData(List<CThostFtdcDepthMarketDataField> marketDataList)
        {
            try
            {
                foreach (CThostFtdcDepthMarketDataField pDepthMarketData in marketDataList)
                {
                    CtpDataServer.getServerInstance().OnSingleRtnDepthMarketData(pDepthMarketData);
                }
                //CtpDataServer.getServerInstance().ProcessMarketData();
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        //public void ClearThread()
        //{
            //if (RspThread != null && RspThread.IsAlive)
            //{
            //    ExecFlag = false;
            //    RspThread.Abort();
            //    Util.Log("CmdThread Abort");
            //    Thread.Sleep(100);
            //}
        //}
    }
}
