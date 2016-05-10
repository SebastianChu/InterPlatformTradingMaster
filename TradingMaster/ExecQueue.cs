using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using TradingMaster.JYData;
using System.Threading.Tasks;

namespace TradingMaster
{
    public class ExecQueue
    {
        /// <summary>
        /// 行情由一个线程负责转发
        /// </summary>
        private Task _CmdTask;
        private Task _OrdTask;
        //private Thread CmdThread;
        //private Thread OrdThread;
        private SynQueue<RequestContent> _CmdQueue;
        private SynQueue<RequestContent> _OrdQueue;
        public static DateTime ReqTime;
        private object _LockObject;
        //private bool ExecFlag = true;
        //private Dictionary<CodeKey, ReqCodeValue> reqCodeInfoDict;

        public ExecQueue(CancellationTokenSource cts)
        {
            _CmdQueue = new SynQueue<RequestContent>();
            _OrdQueue = new SynQueue<RequestContent>();
            _LockObject = new object();            
            //reqCodeInfoDict = new Dictionary<CodeKey, ReqCodeValue>();

            _CmdTask = Task.Factory.StartNew(() => CommandThreadProc(cts.Token),cts.Token);
            //CmdThread = new Task(CommandThreadProc);
            //CmdThread.IsBackground = true;
            //CmdThread.Start();            

            _OrdTask = Task.Factory.StartNew(() => OrderThreadProc(cts.Token), cts.Token);
            //OrdThread = new Thread(OrderThreadProc);
            //OrdThread.IsBackground = true;
            //OrdThread.Start();
        }

        public int QryCount()
        {
            return _CmdQueue.Count();
        }

        public int OrdCount()
        {
            return _OrdQueue.Count();
        }

        private void CommandThreadProc(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)//(ExecFlag)
            {
                try
                {
                    RequestContent cmd = _CmdQueue.Dequeue();
                    Util.Log("TradeApi CommandThreadProc: " + cmd.FunctionName + " dequeues.");
                    MethodInfo serverMethod = CtpDataServer.GetUserInstance().GetType().GetMethod(cmd.FunctionName);
                    serverMethod.Invoke(CtpDataServer.GetUserInstance(), cmd.Params.ToArray());
                    ReqTime = DateTime.Now;
                    Sleeping(1024);
                    // Windows 8，Windows 7 和 Windows Vista 的时钟频率，这是DateTime.Ticks，
                    // Environment.TickCount 以及 WinAPI GetSystemTime 的标准频率，但 Task.Delay，
                    // Thread.Sleep 或 WinAPI Sleep 的标准频率通常是 15.4 ～ 15.6 毫秒，有时也会超出这个范围
                    // 如果 OS 能够提供保证小于等于而不是大于等于所需延迟时间的API，就可以同时保证效率和精确度了
                    // 其实将 clockRate 设成 15 毫秒就非常精确了，并不需要一定设成 15.6001674109375
                }
                catch (Exception ex)
                {
                    Util.Log("exception: " + ex.Message);
                    Util.Log("exception: " + ex.StackTrace);
                    ct.ThrowIfCancellationRequested();
                }
            }
            if (_CmdTask.IsCompleted)
            {
                _CmdTask.Dispose();
            }
            else
            {
                Util.Log("CmdTask Status: " + _CmdTask.Status);
            }
        }

        private void OrderThreadProc(CancellationToken ct)
        {
            List<RequestContent> cancelQuoteOrderLst = new List<RequestContent>();
            List<RequestContent> quoteOrderLst = new List<RequestContent>();
            List<RequestContent> cancelOrderLst = new List<RequestContent>();
            List<RequestContent> orderLst = new List<RequestContent>();
            while (!ct.IsCancellationRequested)//(true)
            {
                try
                {
                    //CtpRequestContent cmd = _OrdQueue.Dequeue();
                    //Util.Log("TradeApi OrderThreadProc: " + cmd.FunctionName + " dequeues.");
                    //MethodInfo serverMethod = CtpDataServer.getServerInstance().GetType().GetMethod(cmd.FunctionName);
                    //serverMethod.Invoke(CtpDataServer.getServerInstance(), cmd.Params.ToArray());

                    List<RequestContent> dataList = _OrdQueue.DequeueAll();
                    foreach (RequestContent data in dataList)
                    {
                        if (data.FunctionName == "CancelQuoteOrder")
                        {
                            cancelQuoteOrderLst.Add(data);
                        }
                        if (data.FunctionName == "NewQuoteOrder")
                        {
                            quoteOrderLst.Add(data);
                        }
                        if (data.FunctionName == "CancelOrder")
                        {
                            cancelOrderLst.Add(data);
                        }
                        if (data.FunctionName == "NewOrderSingle")
                        {
                            orderLst.Add(data);
                        }
                    }

                    dataList.Clear();
                    dataList.AddRange(cancelQuoteOrderLst);
                    dataList.AddRange(quoteOrderLst);
                    dataList.AddRange(cancelOrderLst);
                    dataList.AddRange(orderLst);
                    if (dataList.Count > 0)
                    {
                        Util.Log(String.Format("Order Count: {0}; Cancel Quote Order: {1}, Quote Order: {2}, Cancel Order: {3}, Order: {4}"
                            , dataList.Count, cancelQuoteOrderLst.Count, quoteOrderLst.Count, cancelOrderLst.Count, orderLst.Count));
                    }
                    foreach (RequestContent cmd in dataList)
                    {
                        Util.Log("TradeApi OrderThreadProc: " + cmd.FunctionName + " dequeues.");
                        MethodInfo serverMethod = CtpDataServer.GetUserInstance().GetType().GetMethod(cmd.FunctionName);
                        serverMethod.Invoke(CtpDataServer.GetUserInstance(), cmd.Params.ToArray());
                        //Thread.Sleep(175);
                    }
                    cancelQuoteOrderLst.Clear();
                    quoteOrderLst.Clear();
                    cancelOrderLst.Clear();
                    orderLst.Clear();
                }
                catch (Exception ex)
                {                    
                    Util.Log("exception: " + ex.Message);
                    Util.Log("exception: " + ex.StackTrace);
                    cancelQuoteOrderLst.Clear();
                    quoteOrderLst.Clear();
                    cancelOrderLst.Clear();
                    orderLst.Clear();
                    ct.ThrowIfCancellationRequested();
                }
            }
            if (_OrdTask.IsCompleted)
            {
                _OrdTask.Dispose();
            }
            else
            {
                Util.Log("OrdTask Status: " + _OrdTask.Status);
            }
        }

        public void QryEnqueue(RequestContent cmd)
        {
            if (cmd != null)
            {
                if (cmd.Params.Count == 0 && _CmdQueue.CheckUpdateReqInQueue(cmd))
                {
                    Util.Log("TradeApi ExecQueue: " + cmd.FunctionName + " is blocked when enqueues.");
                }
                else
                {
                    Util.Log("TradeApi ExecQueue: " + cmd.FunctionName + " enqueues.");
                    _CmdQueue.Enqueue(cmd, false);
                }
            }
        }

        public void OrdEnqueue(RequestContent cmd)
        {
            if (cmd != null)
            {
                _OrdQueue.Enqueue(cmd, false);
            }
        }

        public void Sleeping(int interval)
        {
            TimeSpan interSec = DateTime.Now - ReqTime;
            if (interSec.TotalMilliseconds < interval)
            {
                Util.Log("Thread sleeps " + (interval - interSec.Milliseconds).ToString() + "ms for CTP's Limitation.");
                Thread.Sleep(interval - interSec.Milliseconds);
            }
        }

        //public void ClearThread()
        //{
            //if (CmdThread != null && CmdThread.IsAlive)
            //{
            //    ExecFlag = false;
            //    CmdThread.Abort();
            //    Util.Log("CmdThread Abort");
            //    Sleeping(100);
            //}
        //}
    }

    public class RequestContent
    {
        public string FunctionName { get; set; }
        public List<object> Params { get; set; }

        public RequestContent(string functionName, List<object> args)
        {
            this.FunctionName = functionName;
            this.Params = args;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            //if (ReferenceEquals(this, obj))
            //{
            //    return false;
            //}
            if (this.GetType() != obj.GetType())
            {
                return false;
            }
            if (this.GetHashCode() != obj.GetHashCode())
            {
                return false;
            }

            RequestContent ctpReq = obj as RequestContent;
            if (ctpReq == null) return false;
            if (Params == null || FunctionName == null) return false;
            return FunctionName.Equals(ctpReq.FunctionName) && Params.Count == 0;
        }

        public override int GetHashCode()
        {
            return FunctionName.GetHashCode();
        }

        public static bool operator ==(RequestContent leftHandSide, RequestContent rightHandSide)
        {
            if (ReferenceEquals(leftHandSide, null))
            {
                return ReferenceEquals(rightHandSide, null);
            }
            return leftHandSide.Equals(rightHandSide);
        }

        public static bool operator !=(RequestContent leftHandSide, RequestContent rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }
    }
}
