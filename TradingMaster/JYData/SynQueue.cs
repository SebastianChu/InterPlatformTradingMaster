using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TradingMaster.JYData
{
    public class SynQueue<T>
    {
        #region "Variables"

        private List<T> _QList;
        private Mutex _SysMutex;
        private AutoResetEvent _Arse;

        public int ToSentCount = 0;
        public int EraseCount = 0;

        #endregion

        public SynQueue()
        {
            _QList = new List<T>();
            //ars = new AutoResetEvent(true);
            _SysMutex = new Mutex();
            _Arse = new AutoResetEvent(false);
        }

        public int Count()
        {
            int count = 0;
            _SysMutex.WaitOne();
            count = _QList.Count; ;
            _SysMutex.ReleaseMutex();
            return count;
        }

        /// <summary>
        /// 入队，如果前面有相同的内容，则不入队
        /// </summary>
        /// <param name="t"></param>
        public void Enqueue(T t, Boolean eraseSameBefore)
        {
            if (t == null) return;
            _SysMutex.WaitOne(Timeout.Infinite, true);
            if (eraseSameBefore)
            {
                try
                {
                    Boolean bFound = false;
                    for (int i = 0; i < _QList.Count; i++)
                    {
                        if (_QList[i] != null)
                        {
                            if (_QList[i].Equals(t))
                            {
                                this.EraseCount += 1;
                                //Util.Log("发现相同的内容:" + t.ToString());
                                //Util.Log("eraseCount=" + eraseCount.ToString());
                                bFound = true;
                            }
                        }
                    }
                    //Util.Log("bFound=" + bFound);
                    if (bFound == false)
                    {
                        _QList.Add(t);
                        _Arse.Set();
                    }
                }
                catch (Exception e)
                {
                    Util.Log(e.Message);
                    Util.Log(e.StackTrace);
                }
            }
            else
            {
                _QList.Add(t);
                _Arse.Set();
            }
            _SysMutex.ReleaseMutex();
        }

        /// <summary>
        /// 出队，如果没内容则阻塞
        /// </summary>
        /// <returns></returns>
        public T Dequeue(int timeout = Timeout.Infinite)
        {
            _SysMutex.WaitOne(Timeout.Infinite, true);
            while (_QList.Count == 0)
            {
                _SysMutex.ReleaseMutex();
                _Arse.WaitOne(Timeout.Infinite, true);
                _SysMutex.WaitOne(Timeout.Infinite, true);
            }
            T ret = _QList.ElementAt(0);
            _QList.RemoveAt(0);
            _SysMutex.ReleaseMutex();
            return ret;
        }

        /// <summary>
        /// 取得队首元素，如果没内容则阻塞
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            _SysMutex.WaitOne(Timeout.Infinite, true);
            while (_QList.Count == 0)
            {
                _SysMutex.ReleaseMutex();
                _Arse.WaitOne(Timeout.Infinite, true);
                _SysMutex.WaitOne(Timeout.Infinite, true);
            }
            T ret = _QList.ElementAt(0);
            _SysMutex.ReleaseMutex();
            return ret;
        }

        /// <summary>
        /// 取得队首元素，如果没内容则阻塞
        /// </summary>
        /// <returns></returns>
        public T Peek(int interval)
        {
            _SysMutex.WaitOne(Timeout.Infinite, true);
            _Arse.WaitOne(interval, true);
            T ret = default(T);
            if (_QList.Count > 0)
            {
                ret = _QList.ElementAt(0);
            }
            _SysMutex.ReleaseMutex();
            return ret;
        }

        /// <summary>
        /// 全部出队，如果没有则阻塞
        /// </summary>
        /// <returns></returns>
        public List<T> DequeueAll(int timeout = Timeout.Infinite)
        {
            //Util.Log("DequeueAll()  mutex.WaitOne(Timeout.Infinite, true)");
            _SysMutex.WaitOne(Timeout.Infinite, true);
            List<T> ret = new List<T>();
            while (_QList.Count == 0)
            {
                _SysMutex.ReleaseMutex();
                _Arse.WaitOne(timeout, true);
                if (_QList.Count == 0 && timeout != Timeout.Infinite)
                {
                    //若为超时
                    return ret;
                }
                _SysMutex.WaitOne(Timeout.Infinite, true);
            }
            ret.AddRange(_QList);
            _QList.Clear();
            _SysMutex.ReleaseMutex();
            return ret;
        }

        /// <summary>
        /// 取得所有元素，如果没内容则阻塞
        /// </summary>
        /// <returns></returns>
        public List<T> PeekAll()
        {
            _SysMutex.WaitOne(Timeout.Infinite, true);
            while (_QList.Count == 0)
            {
                _SysMutex.ReleaseMutex();
                _Arse.WaitOne(Timeout.Infinite, true);
                _SysMutex.WaitOne(Timeout.Infinite, true);
            }

            List<T> ret = new List<T>();
            ret.AddRange(_QList);
            _SysMutex.ReleaseMutex();
            return ret;
        }

        /// <summary>
        /// 删除元素t
        /// </summary>
        /// <param name="t"></param>
        public void RemoveItem(T t)
        {
            _SysMutex.WaitOne(Timeout.Infinite, true);
            _QList.Remove(t);
            _SysMutex.ReleaseMutex();
        }

        /// <summary>
        /// 检查最新入队成员
        /// </summary>
        /// <param name="t"></param>
        public bool CheckLastReqInQueue(RequestContent t)
        {
            if (t == null) return false;
            _SysMutex.WaitOne(Timeout.Infinite, true);
            bool chkFlag = false;
            try
            {
                if (_QList.Count > 0 && _QList[_QList.Count - 1] != null && _QList[_QList.Count - 1] is RequestContent)
                {
                    if (t.Equals(_QList[_QList.Count - 1] as RequestContent))
                    {
                        chkFlag =  true;
                    }
                    else
                    {
                        chkFlag = false;
                    }
                }
            }
            catch (Exception e)
            {
                Util.Log(e.Message);
                Util.Log(e.StackTrace);
            }
            _SysMutex.ReleaseMutex();
            return chkFlag;
        }

        /// <summary>
        /// 检查入队成员是否有后续更新
        /// </summary>
        /// <param name="t"></param>
        public bool CheckUpdateReqInQueue(RequestContent t)
        {
            if (t == null) return false;
            _SysMutex.WaitOne(Timeout.Infinite, true);
            bool chkFlag = false;
            try
            {
                for (int i = 0; i < _QList.Count; i++)
                {
                    if (_QList[i] != null && _QList[i] is RequestContent)
                    {
                        if (t.Equals(_QList[i] as RequestContent))
                        {
                            chkFlag = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Util.Log(e.Message);
                Util.Log(e.StackTrace);
            }
            _SysMutex.ReleaseMutex();
            return chkFlag;
        }
    }
}