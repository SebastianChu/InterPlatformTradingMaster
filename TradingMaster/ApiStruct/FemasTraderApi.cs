using System;
using System.IO;
using System.Runtime.InteropServices;

namespace TradingMaster
{
    /// <summary>
    /// 行情接口
    /// </summary>
    public class FemasTraderApi
    {
        const string DLLNAME = "dll\\FemasTraderApi.dll";

        /// <summary>
        /// FemasTradeApi.dll/USTPtraderapi.dll 放在主程序的执行文件夹中
        /// </summary>
        /// <param name="_investor">投资者帐号</param>
        /// <param name="_pwd">密码</param>
        /// <param name="_broker">经纪公司代码</param>
        /// <param name="_addr">前置地址</param>
        public FemasTraderApi(string _investor, string _pwd, string _broker
            , string _addr)
        {
            this.FrontAddr = _addr;
            this.BrokerID = _broker;
            this.InvestorID = _investor;
            this.Password = _pwd;
            ClearUserDll();
            LoadDll(DLLNAME);
            string tempOrderRef = DateTime.Now.Ticks.ToString();
            MaxOrderRef = int.Parse(tempOrderRef.Substring(6, 6));
        }

        /// <summary>
        /// 前置地址
        /// </summary>
        public string FrontAddr { get; set; }

        /// <summary>
        /// 经纪公司代码ctp-2030;上期-4030;
        /// </summary>
        public string BrokerID { get; set; }

        /// <summary>
        /// 投资者代码
        /// </summary>
        public string InvestorID { get; set; }

        /// <summary>
        /// 用户代码
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 前置编号
        /// </summary>
        public int FrontID { get; set; }

        /// <summary>
        /// 会话编号
        /// </summary>
        public int SessionID { get; set; }

        /// <summary>
        /// 最大报单引用
        /// </summary>
        public int MaxOrderRef { get; set; }

        private string Password;

        /// <summary>
        /// 原型是 :HMODULE LoadLibrary(LPCTSTR lpFileName);
        /// </summary>
        /// <param name="lpFileName"> DLL 文件名 </param>
        /// <returns> 函数库模块的句柄 </returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        /// <summary>
        /// 原型是 : FARPROC GetProcAddress(HMODULE hModule, LPCWSTR lpProcName);
        /// </summary>
        /// <param name="hModule"> 包含需调用函数的函数库模块的句柄 </param>
        /// <param name="lpProcName"> 调用函数的名称 </param>
        /// <returns> 函数指针 </returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        /// <summary>
        /// 原型是 : BOOL FreeLibrary(HMODULE hModule);
        /// </summary>
        /// <param name="hModule"> 需释放的函数库模块的句柄 </param>
        /// <returns> 是否已释放指定的 Dll </returns>
        [DllImport("kernel32", EntryPoint = "FreeLibrary", SetLastError = true)]
        protected static extern bool FreeLibrary(IntPtr hModule);

        public IntPtr PtrHandle;
        private string _DllFile;
        /// <summary>
        /// Loadlibrary 返回的函数库模块的句柄
        /// </summary>
        private IntPtr _PtrHModule = IntPtr.Zero;

        /// <summary>
        /// GetProcAddress 返回的函数指针
        /// </summary>
        private IntPtr _PtrFarProc = IntPtr.Zero;

        /// <summary>
        /// 装载 Dll
        /// </summary>
        /// <param name="lpFileName">DLL 文件名 </param>
        public void LoadDll(string strFile)
        {
            string dir = Environment.CurrentDirectory;
            if (File.Exists(strFile))
            {
                _DllFile = "FemasTraderApi_" + this.InvestorID + "_" + DateTime.Now.Ticks + ".dll";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                File.Copy(strFile, dir + "\\" + _DllFile);
                this.PtrHandle = LoadLibrary(dir + "\\" + _DllFile);
            }
            if (this.PtrHandle == IntPtr.Zero)
            {
                throw (new Exception(string.Format(" 没有找到 :{0}.", _DllFile)));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pHModule"></param>
        /// <param name="lpProcName"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static Delegate Invoke(IntPtr pHModule, string lpProcName, Type t)
        {
            // 若函数库模块的句柄为空，则抛出异常 
            if (pHModule == IntPtr.Zero)
            {
                throw (new Exception(" 函数库模块的句柄为空 , 请确保已进行 LoadDll 操作 !"));
            }
            // 取得函数指针 
            IntPtr farProc = GetProcAddress(pHModule, lpProcName);
            // 若函数指针，则抛出异常 
            if (farProc == IntPtr.Zero)
            {
                throw (new Exception(" 没有找到 :" + lpProcName + " 这个函数的入口点 "));
            }
            return Marshal.GetDelegateForFunctionPointer(farProc, t);
        }

        /// <summary>
        /// 卸载 Dll
        /// </summary>
        public void UnLoadDll()
        {
            try
            {
                FreeLibrary(_PtrHModule);
                _PtrHModule = IntPtr.Zero;
                _PtrFarProc = IntPtr.Zero;
            }
            catch (System.Exception ex)
            {
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error("exception: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// 清除目录下的冗余用户的Dll
        /// </summary>
        /// <summary>
        /// 清除目录下的冗余用户的Dll
        /// </summary>
        public void ClearUserDll()
        {
            try
            {
                string strUserDir = Environment.CurrentDirectory;
                if (Directory.Exists(strUserDir))
                {
                    string[] dllFiles = Directory.GetFiles(strUserDir);
                    foreach (string fileItem in dllFiles)
                    {
                        if (fileItem.StartsWith(strUserDir + "\\FemasTraderApi_") && fileItem.EndsWith(".dll") && IsFileAvailable(fileItem))// + this.InvestorID + "_"
                        {
                            File.Delete(fileItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error("exception: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// 判断文件是否被占用
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        private bool IsFileAvailable(string fileDir)
        {
            if (!File.Exists(fileDir))
            {
                return false;
            }
            IntPtr vHandle = _lopen(fileDir, OF_READWRITE | OF_SHARE_DENY_NONE);
            if (vHandle == HFILE_ERROR)
            {
                return false;
            }
            CloseHandle(vHandle);
            return true;
        }

        /// <summary>
        /// 登录
        /// </summary>
        private delegate void reqConnect(string pFront);
        public void Connect()
        {
            ((reqConnect)Invoke(this.PtrHandle, "Connect", typeof(reqConnect)))(this.FrontAddr);
        }

        /// <summary>
        /// 获取当前交易日:只有登录成功后,才能得到正确的交易日
        /// </summary>
        /// <returns></returns>
        private delegate string getTradingDay();
        public string GetTradingDay()
        {
            return ((getTradingDay)Invoke(this.PtrHandle, "GetTradingDay", typeof(getTradingDay)))();
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        private delegate void reqDisConnect();
        public void DisConnect()
        {
            ((reqDisConnect)Invoke(this.PtrHandle, "DisConnect", typeof(reqDisConnect)))();
        }

        /// <summary>
        /// 风控前置系统用户登录请求
        /// </summary>
        private delegate int reqUserLogin(string pBroker, string pInvestor, string pPwd);
        public int ReqUserLogin()
        {
            return ((reqUserLogin)Invoke(this.PtrHandle, "ReqUserLogin", typeof(reqUserLogin)))(this.BrokerID, this.InvestorID, this.Password);
        }

        /// <summary>
        /// 用户退出请求
        /// </summary>
        private delegate int reqUserLogout(string BROKER_ID, string INVESTOR_ID);
        public int ReqUserLogout()
        {
            return ((reqUserLogout)Invoke(this.PtrHandle, "ReqUserLogout", typeof(reqUserLogout)))(this.BrokerID, this.InvestorID);
        }

        /// <summary>
        /// 用户密码修改请求
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        private delegate int reqUserPasswordUpdate(string BROKER_ID, string USER_ID, string OLD_PASSWORD, string NEW_PASSWORD);
        public int UserPasswordUpdate(string userID, string oldPassword, string newPassword)
        {
            return ((reqUserPasswordUpdate)Invoke(this.PtrHandle, "ReqUserPasswordUpdate", typeof(reqUserPasswordUpdate)))(this.BrokerID, userID, oldPassword, newPassword);
        }

        /// <summary>
        /// 报单录入请求
        /// </summary>
        /// <param name="order">输入的报单</param>
        private delegate int reqOrderInsert(ref CUstpFtdcInputOrderField pField);
        public int OrderInsert(CUstpFtdcInputOrderField pOrder)
        {
            return ((reqOrderInsert)Invoke(this.PtrHandle, "ReqOrderInsert", typeof(reqOrderInsert)))(ref pOrder);
        }
        /// <summary>
        /// 报单录入请求
        /// </summary>
        /// <param name="InstrumentID">合约代码</param>
        /// <param name="OffsetFlag">平仓:仅上期所平今时使用CloseToday/其它情况均使用Close</param>
        /// <param name="Direction">买卖</param>
        /// <param name="Price">价格</param>
        /// <param name="Volume">手数</param>
        public int OrderInsert(string InstrumentID, EnumUstpOffsetFlagType OffsetFlag, EnumUstpDirectionType Direction, double Price, int Volume, double touchPrice, EnumOrderType orderType = EnumOrderType.Limit)
        {
            CUstpFtdcInputOrderField pOrder = new CUstpFtdcInputOrderField();
            pOrder.BrokerID = this.BrokerID;
            pOrder.BusinessUnit = null;
            //pOrder.ContingentCondition = EnumUstpContingentConditionType.Immediately;
            pOrder.Direction = Direction;
            pOrder.ForceCloseReason = EnumUstpForceCloseReasonType.NotForceClose;
            pOrder.HedgeFlag = EnumUstpHedgeFlagType.Speculation;
            pOrder.InvestorID = this.InvestorID;
            pOrder.InstrumentID = InstrumentID;
            pOrder.IsAutoSuspend = (int)EnumThostBoolType.No;
            pOrder.MinVolume = 1;
            pOrder.OffsetFlag = OffsetFlag;
            pOrder.UserOrderLocalID = "" + (++this.MaxOrderRef).ToString(); //orderref??
            //pOrder.UserForceClose = (int)EnumThostBoolType.No;
            pOrder.UserID = this.UserID;
            pOrder.Volume = Volume;

            if (orderType == EnumOrderType.Limit)
            {
                pOrder.LimitPrice = Price;
                pOrder.OrderPriceType = EnumUstpOrderPriceTypeType.LimitPrice;
                pOrder.TimeCondition = EnumUstpTimeConditionType.GFD;	//当日有效
                pOrder.VolumeCondition = EnumUstpVolumeConditionType.AV;
            }
            else if (orderType == EnumOrderType.FOK)
            {
                pOrder.LimitPrice = Price;
                pOrder.OrderPriceType = EnumUstpOrderPriceTypeType.LimitPrice;
                pOrder.TimeCondition = EnumUstpTimeConditionType.IOC;
                pOrder.VolumeCondition = EnumUstpVolumeConditionType.CV;
            }
            else if (orderType == EnumOrderType.FAK)
            {
                pOrder.LimitPrice = Price;
                pOrder.OrderPriceType = EnumUstpOrderPriceTypeType.LimitPrice;
                pOrder.TimeCondition = EnumUstpTimeConditionType.IOC;
                pOrder.VolumeCondition = EnumUstpVolumeConditionType.AV;
            }
            else if (orderType == EnumOrderType.Market)
            {
                pOrder.LimitPrice = 0;
                pOrder.OrderPriceType = EnumUstpOrderPriceTypeType.AnyPrice;
                pOrder.TimeCondition = EnumUstpTimeConditionType.IOC;
                pOrder.VolumeCondition = EnumUstpVolumeConditionType.AV;
            }
            else if (orderType == EnumOrderType.MaketToLimit)
            {
                pOrder.LimitPrice = 0;
                pOrder.OrderPriceType = EnumUstpOrderPriceTypeType.AnyPrice;
                pOrder.TimeCondition = EnumUstpTimeConditionType.GFD;
                pOrder.VolumeCondition = EnumUstpVolumeConditionType.AV;
            }
            else if (orderType == EnumOrderType.FiveLevel)
            {
                pOrder.LimitPrice = 0;
                pOrder.OrderPriceType = EnumUstpOrderPriceTypeType.FiveLevelPrice;
                pOrder.TimeCondition = EnumUstpTimeConditionType.IOC;
                pOrder.VolumeCondition = EnumUstpVolumeConditionType.AV;
            }
            else if (orderType == EnumOrderType.FiveLevelToLimit)
            {
                pOrder.LimitPrice = 0;
                pOrder.OrderPriceType = EnumUstpOrderPriceTypeType.FiveLevelPrice;
                pOrder.TimeCondition = EnumUstpTimeConditionType.GFD;
                pOrder.VolumeCondition = EnumUstpVolumeConditionType.AV;
            }
            else if (orderType == EnumOrderType.Best)
            {
                pOrder.LimitPrice = 0;
                pOrder.OrderPriceType = EnumUstpOrderPriceTypeType.BestPrice;
                pOrder.TimeCondition = EnumUstpTimeConditionType.IOC;
                pOrder.VolumeCondition = EnumUstpVolumeConditionType.AV;
            }
            else if (orderType == EnumOrderType.BestToLimit)
            {
                pOrder.LimitPrice = 0;
                pOrder.OrderPriceType = EnumUstpOrderPriceTypeType.BestPrice;
                pOrder.TimeCondition = EnumUstpTimeConditionType.GFD;
                pOrder.VolumeCondition = EnumUstpVolumeConditionType.AV;
            }
            return ((reqOrderInsert)Invoke(this.PtrHandle, "ReqOrderInsert", typeof(reqOrderInsert)))(ref pOrder);
        }


        /// <summary>
        /// 报单操作请求
        /// </summary>
        /// <param name="InstrumentID"></param>
        /// <param name="FrontID"></param>
        /// <param name="SessionID"></param>
        /// <param name="OrderRef"></param>
        /// <param name="ExchangeID"></param>
        /// <param name="OrderSysID"></param>
        private delegate int reqOrderAction(ref CUstpFtdcOrderActionField pField);
        public int OrderAction(string InstrumentID, int FrontID = 0, int SessionID = 0, string OrderRef = "0", string ExchangeID = null, string OrderSysID = null)
        {
            CUstpFtdcOrderActionField pOrderAction = new CUstpFtdcOrderActionField();
            pOrderAction.ActionFlag = EnumUstpActionFlagType.Delete;
            pOrderAction.BrokerID = this.BrokerID;
            pOrderAction.ExchangeID = ExchangeID;
            pOrderAction.InvestorID = this.InvestorID;
            if (OrderSysID != null)
            {
                pOrderAction.OrderSysID = new string('\0', 20 - OrderSysID.Length) + OrderSysID;	//OrderSysID右对齐
            }
            pOrderAction.UserID = this.UserID;
            pOrderAction.UserOrderActionLocalID = "" + (++this.MaxOrderRef).ToString();
            if (OrderRef != "0")
            {
                pOrderAction.UserOrderLocalID = OrderRef;
            }
            return ((reqOrderAction)Invoke(this.PtrHandle, "ReqOrderAction", typeof(reqOrderAction)))(ref pOrderAction);
        }

        /// <summary>
        /// 报价录入请求
        /// </summary>
        private delegate int reqQuoteInsert(ref CUstpFtdcInputQuoteField pField);
        public int ReqQuoteInsert(CUstpFtdcInputQuoteField pInputQuote)
        {
            return ((reqQuoteInsert)Invoke(this.PtrHandle, "ReqQuoteInsert", typeof(reqQuoteInsert)))(ref pInputQuote);
        }

        /// <summary>
        /// 报价录入请求:限价单
        /// </summary>
        public int ReqQuoteInsert(string InstrumentID, EnumUstpOffsetFlagType BidOffset, double BidPrice, int BidVolume, EnumUstpOffsetFlagType AskOffset, double AskPrice, int AskVolume, string ForQuoteID = "")
        {
            CUstpFtdcInputQuoteField pInputQuote = new CUstpFtdcInputQuoteField();
            pInputQuote.BrokerID = this.BrokerID;
            pInputQuote.BusinessUnit = null;
            //pInputQuote.ForQuoteSysID = ForQuoteID;
            pInputQuote.InvestorID = this.InvestorID;
            pInputQuote.InstrumentID = InstrumentID;
            pInputQuote.UserID = this.InvestorID;
            pInputQuote.UserQuoteLocalID = "" + (++this.MaxOrderRef).ToString();

            pInputQuote.BidUserOrderLocalID = "" + (++this.MaxOrderRef).ToString();
            pInputQuote.BidOffsetFlag = BidOffset;
            pInputQuote.BidPrice = BidPrice;
            pInputQuote.BidVolume = BidVolume;
            pInputQuote.BidHedgeFlag = EnumUstpHedgeFlagType.Speculation;

            pInputQuote.AskUserOrderLocalID = "" + (++this.MaxOrderRef).ToString();
            pInputQuote.AskOffsetFlag = AskOffset;
            pInputQuote.AskPrice = AskPrice;
            pInputQuote.AskVolume = AskVolume;
            pInputQuote.AskHedgeFlag = EnumUstpHedgeFlagType.Speculation;
            return ((reqQuoteInsert)Invoke(this.PtrHandle, "ReqQuoteInsert", typeof(reqQuoteInsert)))(ref pInputQuote);
        }

        /// <summary>
        /// 报价操作请求
        /// </summary>
        private delegate int reqQuoteAction(ref CUstpFtdcQuoteActionField pField);
        public int ReqQuoteAction(string InstrumentID, int FrontID = 0, int SessionID = 0, string QuoteRef = "0", string ExchangeID = null, string QuoteSysID = null)
        {
            CUstpFtdcQuoteActionField pInputQuoteAction = new CUstpFtdcQuoteActionField();
            pInputQuoteAction.ActionFlag = EnumUstpActionFlagType.Delete;
            pInputQuoteAction.BrokerID = this.BrokerID;
            pInputQuoteAction.ExchangeID = ExchangeID;
            pInputQuoteAction.InvestorID = this.InvestorID;
            if (QuoteSysID != null)
            {
                pInputQuoteAction.QuoteSysID = new string('\0', 20 - QuoteSysID.Length) + QuoteSysID;	//QuoteSysID右对齐
            }
            pInputQuoteAction.UserID = this.UserID;
            pInputQuoteAction.UserQuoteActionLocalID = "" + (++this.MaxOrderRef).ToString();
            if (QuoteRef != "0")
            {
                pInputQuoteAction.UserQuoteLocalID = QuoteRef;
            }
            return ((reqQuoteAction)Invoke(this.PtrHandle, "ReqQuoteAction", typeof(reqQuoteAction)))(ref pInputQuoteAction);
        }

        /// <summary>
        /// 客户询价请求
        /// </summary>
        private delegate int reqForQuote(ref CUstpFtdcReqForQuoteField pField);
        public int ReqForQuote(string instrumentID, string exchangeID, string tradingday, string quoteTime)
        {
            CUstpFtdcReqForQuoteField pInputForQuote = new CUstpFtdcReqForQuoteField();
            pInputForQuote.BrokerID = this.BrokerID;
            pInputForQuote.ExchangeID = exchangeID;
            pInputForQuote.InstrumentID = instrumentID;
            pInputForQuote.InvestorID = this.InvestorID;
            pInputForQuote.ReqForQuoteID = "" + (++this.MaxOrderRef).ToString(); ;
            pInputForQuote.ReqForQuoteTime = quoteTime;
            pInputForQuote.TradingDay = tradingday;
            pInputForQuote.UserID = this.InvestorID;
            return ((reqForQuote)Invoke(this.PtrHandle, "ReqForQuote", typeof(reqForQuote)))(ref pInputForQuote);
        }

        /// <summary>
        /// 客户申请组合请求
        /// </summary>
        private delegate int reqCombActionInsert(ref CUstpFtdcInputMarginCombActionField pField);
        public int ReqCombActionInsert(string instrumentID, string exchangeID, EnumUstpDirectionType direction, int handCount, EnumUstpCombDirectionType combDir)
        {
            CUstpFtdcInputMarginCombActionField pInputMarginCombAction = new CUstpFtdcInputMarginCombActionField();
            pInputMarginCombAction.BrokerID = this.BrokerID;
            pInputMarginCombAction.CombDirection = combDir;
            pInputMarginCombAction.CombInstrumentID = instrumentID;
            pInputMarginCombAction.CombVolume = handCount;
            pInputMarginCombAction.ExchangeID = exchangeID;
            pInputMarginCombAction.HedgeFlag = EnumUstpHedgeFlagType.Speculation;//
            pInputMarginCombAction.InvestorID = this.InvestorID;
            pInputMarginCombAction.UserActionLocalID = (++this.MaxOrderRef).ToString();
            pInputMarginCombAction.UserID = this.InvestorID;
            return ((reqCombActionInsert)Invoke(this.PtrHandle, "ReqCombActionInsert", typeof(reqCombActionInsert)))(ref pInputMarginCombAction);
        }

        /// <summary>
        /// 报单查询请求:不填-查所有
        /// </summary>
        private delegate int reqQryOrder(ref CUstpFtdcQryOrderField pField);
        /// <param name="_exchangeID"></param>
        /// <param name="_timeStart"></param>
        /// <param name="_timeEnd"></param>
        /// <param name="_instrumentID"></param>
        /// <param name="_orderSysID"></param>
        public int QryOrder(string _exchangeID = null, string _timeStart = null, string _timeEnd = null, string _instrumentID = null, string _orderSysID = null)
        {
            CUstpFtdcQryOrderField pQryOrder = new CUstpFtdcQryOrderField();
            pQryOrder.BrokerID = this.BrokerID;
            pQryOrder.ExchangeID = _exchangeID;
            pQryOrder.InstrumentID = _instrumentID;
            pQryOrder.InvestorID = this.InvestorID;
            pQryOrder.UserID = this.UserID;
            pQryOrder.OrderSysID = _orderSysID;
            return ((reqQryOrder)Invoke(this.PtrHandle, "ReqQryOrder", typeof(reqQryOrder)))(ref pQryOrder);
        }

        /// <summary>
        ///  成交单查询请求:不填-查所有
        /// </summary>
        private delegate int reqQryTrade(ref CThostFtdcQryTradeField pField);
        /// <param name="_exchangeID"></param>
        /// <param name="_timeStart"></param>
        /// <param name="_timeEnd"></param>
        /// <param name="_instrumentID"></param>
        /// <param name="_tradeID"></param>
        public int QryTrade(DateTime? _timeStart = null, DateTime? _timeEnd = null, string _instrumentID = null, string _exchangeID = null, string _tradeID = null)
        {
            CThostFtdcQryTradeField pQryTrade = new CThostFtdcQryTradeField();
            pQryTrade.BrokerID = this.BrokerID;
            pQryTrade.ExchangeID = _exchangeID;
            pQryTrade.InstrumentID = _instrumentID;
            pQryTrade.InvestorID = this.InvestorID;
            pQryTrade.TradeID = _tradeID;
            pQryTrade.TradeTimeStart = _timeStart == null ? null : _timeStart.Value.ToString("HH:mm:ss");
            pQryTrade.TradeTimeEnd = _timeEnd == null ? null : _timeEnd.Value.ToString("HH:mm:ss");
            return ((reqQryTrade)Invoke(this.PtrHandle, "ReqQryTrade", typeof(reqQryTrade)))(ref pQryTrade);
        }

        /// <summary>
        /// 可用投资者账户查询请求
        /// </summary>
        private delegate int reqQryUserInvestor(string BROKER_ID, string INVESTOR_ID);
        public int QryUserInvestor()
        {
            return ((reqQryUserInvestor)Invoke(this.PtrHandle, "ReqQryUserInvestor", typeof(reqQryUserInvestor)))(this.BrokerID, this.InvestorID);
        }

        /// <summary>
        /// 交易编码查询请求:参数不填-查所有
        /// </summary>
        private delegate int reqQryTradingCode(string BROKER_ID, string INVESTOR_ID, string CLIENT_ID, string EXCHANGE_ID);
        public int QryTradingCode(string clientID = null, string exchangeID = null)
        {
            return ((reqQryTradingCode)Invoke(this.PtrHandle, "ReqQryTradingCode", typeof(reqQryTradingCode)))(this.BrokerID, this.InvestorID, clientID, exchangeID);
        }

        /// <summary>
        /// 投资者资金账户查询请求
        /// </summary>
        private delegate int reqQryTradingAccount(string BROKER_ID, string INVESTOR_ID);
        public int QryTradingAccount()
        {
            return ((reqQryTradingAccount)Invoke(this.PtrHandle, "ReqQryInvestorAccount", typeof(reqQryTradingAccount)))(this.BrokerID, this.InvestorID);
        }

        /// <summary>
        /// 合约查询请求
        /// </summary>
        /// <param name="instrument">合约代码:不填-查所有</param>
        private delegate int qryInstrument(string Instrument);
        public int QryInstrument(string instrument = null)
        {
            return ((qryInstrument)Invoke(this.PtrHandle, "ReqQryInstrument", typeof(qryInstrument)))(instrument);
        }

        /// <summary>
        /// 交易所查询请求
        /// </summary>
        /// <param name="exchangeID"></param>
        private delegate int reqQryExchange(string EXCHANGE_ID);
        public int QryExchange(string exchangeID)
        {
            return ((reqQryExchange)Invoke(this.PtrHandle, "ReqQryExchange", typeof(reqQryExchange)))(exchangeID);
        }

        /// <summary>
        /// 投资者资金账户查询请求
        /// </summary>
        /// <param name="instrument">合约代码:不填-查所有</param>
        private delegate int reqQryInvestorPosition(string BROKER_ID, string INVESTOR_ID, string Instrument);
        public int QryInvestorPosition(string instrument = null)
        {
            return ((reqQryInvestorPosition)Invoke(this.PtrHandle, "ReqQryInvestorPosition", typeof(reqQryInvestorPosition)))(this.BrokerID, this.InvestorID, instrument);
        }

        /// <summary>
        /// 订阅主题请求
        /// </summary>
        private delegate int reqSubscribeTopic(ref CUstpFtdcDisseminationField pDissemination);
        public int ReqSubscribeTopic(CUstpFtdcDisseminationField pDissemination)
        {
            return ((reqSubscribeTopic)Invoke(this.PtrHandle, "ReqSubscribeTopic", typeof(reqSubscribeTopic)))(ref pDissemination);
        }

        /// <summary>
        /// 合规参数查询请求
        /// </summary>
        private delegate int reqQryComplianceParam(ref CUstpFtdcQryComplianceParamField pField);
        public int ReqSubscribeTopic(CUstpFtdcQryComplianceParamField pQryComplianceParam)
        {
            return ((reqQryComplianceParam)Invoke(this.PtrHandle, "ReqQryComplianceParam", typeof(reqQryComplianceParam)))(ref pQryComplianceParam);
        }

        /// <summary>
        /// 主题查询请求
        /// </summary>
        private delegate int reqQryTopic(ref CUstpFtdcDisseminationField pDissemination);
        public int ReqQryTopic(CUstpFtdcDisseminationField pDissemination)
        {
            return ((reqQryTopic)Invoke(this.PtrHandle, "ReqQryTopic", typeof(reqQryTopic)))(ref pDissemination);
        }

        /// <summary>
        /// 投资者手续费率查询请求
        /// </summary>
        private delegate int reqQryInvestorFee(ref CUstpFtdcQryInvestorFeeField pQryInvestorFee);
        public int QryInvestorFee(CUstpFtdcQryInvestorFeeField pQryInvestorFee)
        {
            return ((reqQryInvestorFee)Invoke(this.PtrHandle, "ReqQryInvestorFee", typeof(reqQryInvestorFee)))(ref pQryInvestorFee);
        }

        /// <summary>
	    /// 投资者保证金率查询请求
        /// </summary>
        private delegate int reqQryInvestorMargin(ref CUstpFtdcQryInvestorMarginField pQryInvestorMargin);
        public int ReqQryInvestorMargin(CUstpFtdcQryInvestorMarginField pQryInvestorMargin)
        {
            return ((reqQryInvestorMargin)Invoke(this.PtrHandle, "ReqQryInvestorMargin", typeof(reqQryInvestorMargin)))(ref pQryInvestorMargin);
        }

        /// <summary>
	    /// 交易编码组合持仓查询请求
        /// </summary>
        private delegate int reqQryInvestorCombPosition(ref CUstpFtdcQryInvestorCombPositionField pQryInvestorCombPosition);
        public int QryInvestorCombPosition(CUstpFtdcQryInvestorCombPositionField pQryInvestorCombPosition)
        {
            return ((reqQryInvestorCombPosition)Invoke(this.PtrHandle, "ReqQryInvestorCombPosition", typeof(reqQryInvestorCombPosition)))(ref pQryInvestorCombPosition);
        }

        /// <summary>
	    /// 交易编码单腿持仓查询请求
        /// </summary>
        private delegate int reqQryInvestorLegPosition(ref CUstpFtdcQryInvestorLegPositionField pQryInvestorLegPosition);
        public int QryInvestorLegPosition(CUstpFtdcQryInvestorLegPositionField pQryInvestorLegPosition)
        {
            return ((reqQryInvestorLegPosition)Invoke(this.PtrHandle, "ReqQryInvestorLegPosition", typeof(reqQryInvestorLegPosition)))(ref pQryInvestorLegPosition);
        }

        /// <summary>
        /// 请求查询汇率
        /// </summary>
        private delegate int reqQryExchangeRate(ref CUstpFtdcQryExchangeRateField pField);
        public int QryExchangeRate(string productID)
        {
            CUstpFtdcQryExchangeRateField pQryProductExchRate = new CUstpFtdcQryExchangeRateField();
            pQryProductExchRate.ProductID = productID;
            return ((reqQryExchangeRate)Invoke(this.PtrHandle, "ReqQryExchangeRate", typeof(reqQryExchangeRate)))(ref pQryProductExchRate);
        }

        //回调函数 ==================================================================================================================

        private delegate void Reg(IntPtr pPtr);

        /// 连接响应
        /// <summary>
        /// 
        /// </summary>
        public delegate void FrontConnected();
        private FrontConnected _OnFrontConnected;
        /// <summary>
        /// 当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
        /// </summary>
        public event FrontConnected OnFrontConnected
        {
            add
            {
                _OnFrontConnected += value;
                (Invoke(this.PtrHandle, "RegOnFrontConnected", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnFrontConnected));
            }
            remove
            {
                _OnFrontConnected -= value;
                (Invoke(this.PtrHandle, "RegOnFrontConnected", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnFrontConnected));
            }
        }

        /// 断开应答
        /// <summary>
        /// 
        /// </summary>
        public delegate void FrontDisConnected(int reason);
        private FrontDisConnected _OnFrontDisConnected;
        /// <summary>
        /// 当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
        /// </summary>
        public event FrontDisConnected OnFrontDisConnected
        {
            add
            {
                _OnFrontDisConnected += value;
                (Invoke(this.PtrHandle, "RegOnFrontDisconnected", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnFrontDisConnected));
            }
            remove
            {
                _OnFrontDisConnected -= value;
                (Invoke(this.PtrHandle, "RegOnFrontDisconnected", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnFrontDisConnected));
            }
        }

        /// 心跳超时警告
        /// <summary>
        /// 
        /// </summary>
        public delegate void HeartBeatWarning(int nTimeLapse);
        private HeartBeatWarning _OnHeartBeatWarning;
        /// <summary>
        /// 心跳超时警告。当长时间未收到报文时，该方法被调用。
        /// </summary>
        public event HeartBeatWarning OnHeartBeatWarning
        {
            add
            {
                _OnHeartBeatWarning += value;
                (Invoke(this.PtrHandle, "RegOnHeartBeatWarning", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnHeartBeatWarning));
            }
            remove
            {
                _OnHeartBeatWarning -= value;
                (Invoke(this.PtrHandle, "RegOnHeartBeatWarning", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnHeartBeatWarning));
            }
        }

        /// 报文回调开始通知
        public delegate void PackageStart(int nTopicID, int nSequenceNo);
        private PackageStart _OnPackageStart;
        /// <summary>
        /// 报文回调开始通知
        /// </summary>
        public event PackageStart OnPackageStart
        {
            add
            {
                _OnPackageStart += value;
                (Invoke(this.PtrHandle, "RegPackageStart", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnPackageStart));
            }
            remove
            {
                _OnPackageStart -= value;
                (Invoke(this.PtrHandle, "RegPackageStart", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnPackageStart));
            }
        }

        /// 报文回调结束通知
        public delegate void PackageEnd(int nTopicID, int nSequenceNo);
        private PackageEnd _OnPackageEnd;
        /// <summary>
        /// 报文回调结束通知
        /// </summary>
        public event PackageEnd OnPackageEnd
        {
            add
            {
                _OnPackageEnd += value;
                (Invoke(this.PtrHandle, "RegPackageEnd", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnPackageEnd));
            }
            remove
            {
                _OnPackageEnd -= value;
                (Invoke(this.PtrHandle, "RegPackageEnd", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnPackageEnd));
            }
        }

        /// 错误应答
        public delegate void RspError(ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspError _OnRspError;
        /// <summary>
        /// 错误应答
        /// </summary>
        public event RspError OnRspError
        {
            add
            {
                _OnRspError += value;
                (Invoke(this.PtrHandle, "RegRspError", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspError));
            }
            remove
            {
                _OnRspError -= value;
                (Invoke(this.PtrHandle, "RegRspError", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspError));
            }
        }

        /// 风控前置系统用户登录应答
        public delegate void RspUserLogin(ref CUstpFtdcRspUserLoginField pRspUserLogin, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspUserLogin _OnRspUserLogin;
        /// <summary>
        /// 风控前置系统用户登录应答
        /// </summary>
        public event RspUserLogin OnRspUserLogin
        {
            add
            {
                _OnRspUserLogin += value;
                (Invoke(this.PtrHandle, "RegRspUserLogin", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspUserLogin));
            }
            remove
            {
                _OnRspUserLogin -= value;
                (Invoke(this.PtrHandle, "RegRspUserLogin", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspUserLogin));
            }
        }

        /// 用户退出应答
        public delegate void RspUserLogout(ref CUstpFtdcRspUserLogoutField pRspUserLogout, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspUserLogout _OnRspUserLogout;
        /// <summary>
        /// 用户退出应答
        /// </summary>
        public event RspUserLogout OnRspUserLogout
        {
            add
            {
                _OnRspUserLogout += value;
                (Invoke(this.PtrHandle, "RegRspUserLogout", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspUserLogout));
            }
            remove
            {
                _OnRspUserLogout -= value;
                (Invoke(this.PtrHandle, "RegRspUserLogout", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspUserLogout));
            }
        }

        /// 用户密码修改应答
        public delegate void RspUserPasswordUpdate(ref CUstpFtdcUserPasswordUpdateField pUserPasswordUpdate, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspUserPasswordUpdate _OnRspUserPasswordUpdate;
        /// <summary>
        /// 用户密码修改应答
        /// </summary>
        public event RspUserPasswordUpdate OnRspUserPasswordUpdate
        {
            add
            {
                _OnRspUserPasswordUpdate += value;
                (Invoke(this.PtrHandle, "RegRspUserPasswordUpdate", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspUserPasswordUpdate));
            }
            remove
            {
                _OnRspUserPasswordUpdate -= value;
                (Invoke(this.PtrHandle, "RegRspUserPasswordUpdate", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspUserPasswordUpdate));
            }
        }

        /// 报单录入应答
        public delegate void RspOrderInsert(ref CUstpFtdcInputOrderField pInputOrder, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspOrderInsert _OnRspOrderInsert;
        /// <summary>
        /// 报单录入应答
        /// </summary>
        public event RspOrderInsert OnRspOrderInsert
        {
            add
            {
                _OnRspOrderInsert += value;
                (Invoke(this.PtrHandle, "RegRspOrderInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspOrderInsert));
            }
            remove
            {
                _OnRspOrderInsert -= value;
                (Invoke(this.PtrHandle, "RegRspOrderInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspOrderInsert));
            }
        }

        /// 报单操作应答
        public delegate void RspOrderAction(ref CUstpFtdcOrderActionField pOrderAction, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspOrderAction _OnRspOrderAction;
        /// <summary>
        /// 报单操作应答
        /// </summary>
        public event RspOrderAction OnRspOrderAction
        {
            add
            {
                _OnRspOrderAction += value;
                (Invoke(this.PtrHandle, "RegRspOrderAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspOrderAction));
            }
            remove
            {
                _OnRspOrderAction -= value;
                (Invoke(this.PtrHandle, "RegRspOrderAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspOrderAction));
            }
        }

        /// 报价录入应答
        public delegate void RspQuoteInsert(ref CUstpFtdcInputQuoteField pInputQuote, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQuoteInsert _OnRspQuoteInsert;
        /// <summary>
        /// 报价录入应答
        /// </summary>
        public event RspQuoteInsert OnRspQuoteInsert
        {
            add
            {
                _OnRspQuoteInsert += value;
                (Invoke(this.PtrHandle, "RegRspQuoteInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQuoteInsert));
            }
            remove
            {
                _OnRspQuoteInsert -= value;
                (Invoke(this.PtrHandle, "RegRspQuoteInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQuoteInsert));
            }
        }

        /// 报价操作应答
        public delegate void RspQuoteAction(ref CUstpFtdcQuoteActionField pQuoteAction, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQuoteAction _OnRspQuoteAction;
        /// <summary>
        /// 报价操作应答
        /// </summary>
        public event RspQuoteAction OnRspQuoteAction
        {
            add
            {
                _OnRspQuoteAction += value;
                (Invoke(this.PtrHandle, "RegRspQuoteAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQuoteAction));
            }
            remove
            {
                _OnRspQuoteAction -= value;
                (Invoke(this.PtrHandle, "RegRspQuoteAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQuoteAction));
            }
        }

        /// 询价请求应答
        public delegate void RspForQuote(ref CUstpFtdcReqForQuoteField pReqForQuote, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspForQuote _OnRspForQuote;
        /// <summary>
        /// 询价请求应答
        /// </summary>
        public event RspForQuote OnRspForQuote
        {
            add
            {
                _OnRspForQuote += value;
                (Invoke(this.PtrHandle, "RegRspForQuote", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspForQuote));
            }
            remove
            {
                _OnRspForQuote -= value;
                (Invoke(this.PtrHandle, "RegRspForQuote", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspForQuote));
            }
        }

        /// 客户申请组合应答
        public delegate void RspMarginCombAction(ref CUstpFtdcInputMarginCombActionField pInputMarginCombAction, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspMarginCombAction _OnRspMarginCombAction;
        /// <summary>
        /// 客户申请组合应答
        /// </summary>
        public event RspMarginCombAction OnRspMarginCombAction
        {
            add
            {
                _OnRspMarginCombAction += value;
                (Invoke(this.PtrHandle, "RegRspMarginCombAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspMarginCombAction));
            }
            remove
            {
                _OnRspMarginCombAction -= value;
                (Invoke(this.PtrHandle, "RegRspMarginCombAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspMarginCombAction));
            }
        }

        /// 数据流回退通知
        public delegate void RtnFlowMessageCancel(ref CUstpFtdcFlowMessageCancelField pFlowMessageCancel);
        private RtnFlowMessageCancel _OnRtnFlowMessageCancel;
        /// <summary>
        /// 数据流回退通知
        /// </summary>
        public event RtnFlowMessageCancel OnRtnFlowMessageCancel
        {
            add
            {
                _OnRtnFlowMessageCancel += value;
                (Invoke(this.PtrHandle, "RegRtnFlowMessageCancel", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnFlowMessageCancel));
            }
            remove
            {
                _OnRtnFlowMessageCancel -= value;
                (Invoke(this.PtrHandle, "RegRtnFlowMessageCancel", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnFlowMessageCancel));
            }
        }

        /// 成交回报
        public delegate void RtnTrade(ref CUstpFtdcTradeField pTrade);
        private RtnTrade _OnRtnTrade;
        /// <summary>
        /// 成交回报
        /// </summary>
        public event RtnTrade OnRtnTrade
        {
            add
            {
                _OnRtnTrade += value;
                (Invoke(this.PtrHandle, "RegRtnTrade", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnTrade));
            }
            remove
            {
                _OnRtnTrade -= value;
                (Invoke(this.PtrHandle, "RegRtnTrade", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnTrade));
            }
        }

        /// 报单回报
        public delegate void RtnOrder(ref CUstpFtdcOrderField pOrder);
        private RtnOrder _OnRtnOrder;
        /// <summary>
        /// 报单回报
        /// </summary>
        public event RtnOrder OnRtnOrder
        {
            add
            {
                _OnRtnOrder += value;
                (Invoke(this.PtrHandle, "RegRtnOrder", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnOrder));
            }
            remove
            {
                _OnRtnOrder -= value;
                (Invoke(this.PtrHandle, "RegRtnOrder", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnOrder));
            }
        }

        /// 报单录入错误回报
        public delegate void ErrRtnOrderInsert(ref CUstpFtdcInputOrderField pInputOrder, ref CUstpFtdcRspInfoField pRspInfo);
        private ErrRtnOrderInsert _OnErrRtnOrderInsert;
        /// <summary>
        /// 报单录入错误回报
        /// </summary>
        public event ErrRtnOrderInsert OnErrRtnOrderInsert
        {
            add
            {
                _OnErrRtnOrderInsert += value;
                (Invoke(this.PtrHandle, "RegErrRtnOrderInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnOrderInsert));
            }
            remove
            {
                _OnErrRtnOrderInsert -= value;
                (Invoke(this.PtrHandle, "RegErrRtnOrderInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnOrderInsert));
            }
        }

        /// 报单操作错误回报
        public delegate void ErrRtnOrderAction(ref CUstpFtdcOrderActionField pOrderAction, ref CUstpFtdcRspInfoField pRspInfo);
        private ErrRtnOrderAction _OnErrRtnOrderAction;
        /// <summary>
        /// 报单操作错误回报
        /// </summary>
        public event ErrRtnOrderAction OnErrRtnOrderAction
        {
            add
            {
                _OnErrRtnOrderAction += value;
                (Invoke(this.PtrHandle, "RegErrRtnOrderAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnOrderAction));
            }
            remove
            {
                _OnErrRtnOrderAction -= value;
                (Invoke(this.PtrHandle, "RegErrRtnOrderAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnOrderAction));
            }
        }

        /// 合约交易状态通知
        public delegate void RtnInstrumentStatus(ref CUstpFtdcInstrumentStatusField pInstrumentStatus);
        private RtnInstrumentStatus _OnRtnInstrumentStatus;
        /// <summary>
        /// 合约交易状态通知
        /// </summary>
        public event RtnInstrumentStatus OnRtnInstrumentStatus
        {
            add
            {
                _OnRtnInstrumentStatus += value;
                (Invoke(this.PtrHandle, "RegRtnInstrumentStatus", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnInstrumentStatus));
            }
            remove
            {
                _OnRtnInstrumentStatus -= value;
                (Invoke(this.PtrHandle, "RegRtnInstrumentStatus", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnInstrumentStatus));
            }
        }

        /// 账户出入金回报
        public delegate void RtnInvestorAccountDeposit(ref CUstpFtdcInvestorAccountDepositResField pInvestorAccountDepositRes);
        private RtnInvestorAccountDeposit _OnRtnInvestorAccountDeposit;
        /// <summary>
        /// 账户出入金回报
        /// </summary>
        public event RtnInvestorAccountDeposit OnRtnInvestorAccountDeposit
        {
            add
            {
                _OnRtnInvestorAccountDeposit += value;
                (Invoke(this.PtrHandle, "RegRtnInvestorAccountDeposit", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnInvestorAccountDeposit));
            }
            remove
            {
                _OnRtnInvestorAccountDeposit -= value;
                (Invoke(this.PtrHandle, "RegRtnInvestorAccountDeposit", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnInvestorAccountDeposit));
            }
        }

        /// 报价回报
        public delegate void RtnQuote(ref CUstpFtdcRtnQuoteField pRtnQuote);
        private RtnQuote _OnRtnQuote;
        /// <summary>
        /// 报价回报
        /// </summary>
        public event RtnQuote OnRtnQuote
        {
            add
            {
                _OnRtnQuote += value;
                (Invoke(this.PtrHandle, "RegRtnQuote", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnQuote));
            }
            remove
            {
                _OnRtnQuote -= value;
                (Invoke(this.PtrHandle, "RegRtnQuote", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnQuote));
            }
        }

        /// 报价录入错误回报
        public delegate void ErrRtnQuoteInsert(ref CUstpFtdcInputQuoteField pInputQuote, ref CUstpFtdcRspInfoField pRspInfo);
        private ErrRtnQuoteInsert _OnErrRtnQuoteInsert;
        /// <summary>
        /// 报价录入错误回报
        /// </summary>
        public event ErrRtnQuoteInsert OnErrRtnQuoteInsert
        {
            add
            {
                _OnErrRtnQuoteInsert += value;
                (Invoke(this.PtrHandle, "RegErrRtnQuoteInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnQuoteInsert));
            }
            remove
            {
                _OnErrRtnQuoteInsert -= value;
                (Invoke(this.PtrHandle, "RegErrRtnQuoteInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnQuoteInsert));
            }
        }

        /// 询价回报
        public delegate void RtnForQuote(ref CUstpFtdcReqForQuoteField pReqForQuote);
        private RtnForQuote _OnRtnForQuote;
        /// <summary>
        /// 询价回报
        /// </summary>
        public event RtnForQuote OnRtnForQuote
        {
            add
            {
                _OnRtnForQuote += value;
                (Invoke(this.PtrHandle, "RegRtnForQuote", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnForQuote));
            }
            remove
            {
                _OnRtnForQuote -= value;
                (Invoke(this.PtrHandle, "RegRtnForQuote", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnForQuote));
            }
        }

        /// 增加组合规则通知
        public delegate void RtnMarginCombinationLeg(ref CUstpFtdcMarginCombinationLegField pMarginCombinationLeg);
        private RtnMarginCombinationLeg _OnRtnMarginCombinationLeg;
        /// <summary>
        /// 增加组合规则通知
        /// </summary>
        public event RtnMarginCombinationLeg OnRtnMarginCombinationLeg
        {
            add
            {
                _OnRtnMarginCombinationLeg += value;
                (Invoke(this.PtrHandle, "RegRtnMarginCombinationLeg", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnMarginCombinationLeg));
            }
            remove
            {
                _OnRtnMarginCombinationLeg -= value;
                (Invoke(this.PtrHandle, "RegRtnMarginCombinationLeg", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnMarginCombinationLeg));
            }
        }

        /// 客户申请组合确认
        public delegate void RtnMarginCombAction(ref CUstpFtdcInputMarginCombActionField pInputMarginCombAction);
        private RtnMarginCombAction _OnRtnMarginCombAction;
        /// <summary>
        /// 客户申请组合确认
        /// </summary>
        public event RtnMarginCombAction OnRtnMarginCombAction
        {
            add
            {
                _OnRtnMarginCombAction += value;
                (Invoke(this.PtrHandle, "RegRtnMarginCombAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnMarginCombAction));
            }
            remove
            {
                _OnRtnMarginCombAction -= value;
                (Invoke(this.PtrHandle, "RegRtnMarginCombAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnMarginCombAction));
            }
        }

        /// 报单查询应答
        public delegate void RspQryOrder(ref CUstpFtdcOrderField pOrder, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryOrder _OnRspQryOrder;
        /// <summary>
        /// 报单查询应答
        /// </summary>
        public event RspQryOrder OnRspQryOrder
        {
            add
            {
                _OnRspQryOrder += value;
                (Invoke(this.PtrHandle, "RegRspQryOrder", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryOrder));
            }
            remove
            {
                _OnRspQryOrder -= value;
                (Invoke(this.PtrHandle, "RegRspQryOrder", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryOrder));
            }
        }

        /// 成交单查询应答
        public delegate void RspQryTrade(ref CUstpFtdcTradeField pTrade, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryTrade _OnRspQryTrade;
        /// <summary>
        /// 成交单查询应答
        /// </summary>
        public event RspQryTrade OnRspQryTrade
        {
            add
            {
                _OnRspQryTrade += value;
                (Invoke(this.PtrHandle, "RegRspQryTrade", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryTrade));
            }
            remove
            {
                _OnRspQryTrade -= value;
                (Invoke(this.PtrHandle, "RegRspQryTrade", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryTrade));
            }
        }

        /// 可用投资者账户查询应答
        public delegate void RspQryUserInvestor(ref CUstpFtdcRspUserInvestorField pRspUserInvestor, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryUserInvestor _OnRspQryUserInvestor;
        /// <summary>
        /// 可用投资者账户查询应答
        /// </summary>
        public event RspQryUserInvestor OnRspQryUserInvestor
        {
            add
            {
                _OnRspQryUserInvestor += value;
                (Invoke(this.PtrHandle, "RegRspQryUserInvestor", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryUserInvestor));
            }
            remove
            {
                _OnRspQryUserInvestor -= value;
                (Invoke(this.PtrHandle, "RegRspQryUserInvestor", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryUserInvestor));
            }
        }

        /// 交易编码查询应答
        public delegate void RspQryTradingCode(ref CUstpFtdcRspTradingCodeField pRspTradingCode, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryTradingCode _OnRspQryTradingCode;
        /// <summary>
        /// 交易编码查询应答
        /// </summary>
        public event RspQryTradingCode OnRspQryTradingCode
        {
            add
            {
                _OnRspQryTradingCode += value;
                (Invoke(this.PtrHandle, "RegRspQryTradingCode", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryTradingCode));
            }
            remove
            {
                _OnRspQryTradingCode -= value;
                (Invoke(this.PtrHandle, "RegRspQryTradingCode", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryTradingCode));
            }
        }

        /// 投资者资金账户查询应答
        public delegate void RspQryInvestorAccount(ref CUstpFtdcRspInvestorAccountField pRspInvestorAccount, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryInvestorAccount _OnRspQryInvestorAccount;
        /// <summary>
        /// 投资者资金账户查询应答
        /// </summary>
        public event RspQryInvestorAccount OnRspQryInvestorAccount
        {
            add
            {
                _OnRspQryInvestorAccount += value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorAccount", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorAccount));
            }
            remove
            {
                _OnRspQryInvestorAccount -= value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorAccount", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorAccount));
            }
        }

        /// 合约查询应答
        public delegate void RspQryInstrument(ref CUstpFtdcRspInstrumentField pRspInstrument, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryInstrument _OnRspQryInstrument;
        /// <summary>
        /// 合约查询应答
        /// </summary>
        public event RspQryInstrument OnRspQryInstrument
        {
            add
            {
                _OnRspQryInstrument += value;
                (Invoke(this.PtrHandle, "RegRspQryInstrument", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInstrument));
            }
            remove
            {
                _OnRspQryInstrument -= value;
                (Invoke(this.PtrHandle, "RegRspQryInstrument", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInstrument));
            }
        }

        /// 交易所查询应答
        public delegate void RspQryExchange(ref CUstpFtdcRspExchangeField pRspExchange, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryExchange _OnRspQryExchange;
        /// <summary>
        /// 交易所查询应答
        /// </summary>
        public event RspQryExchange OnRspQryExchange
        {
            add
            {
                _OnRspQryExchange += value;
                (Invoke(this.PtrHandle, "RegRspQryExchange", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryExchange));
            }
            remove
            {
                _OnRspQryExchange -= value;
                (Invoke(this.PtrHandle, "RegRspQryExchange", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryExchange));
            }
        }

        /// 投资者持仓查询应答
        public delegate void RspQryInvestorPosition(ref CUstpFtdcRspInvestorPositionField pRspInvestorPosition, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryInvestorPosition _OnRspQryInvestorPosition;
        /// <summary>
        /// 投资者持仓查询应答
        /// </summary>
        public event RspQryInvestorPosition OnRspQryInvestorPosition
        {
            add
            {
                _OnRspQryInvestorPosition += value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorPosition", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorPosition));
            }
            remove
            {
                _OnRspQryInvestorPosition -= value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorPosition", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorPosition));
            }
        }

        /// 订阅主题应答
        public delegate void RspSubscribeTopic(ref CUstpFtdcDisseminationField pDissemination, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspSubscribeTopic _OnRspSubscribeTopic;
        /// <summary>
        /// 订阅主题应答
        /// </summary>
        public event RspSubscribeTopic OnRspSubscribeTopic
        {
            add
            {
                _OnRspSubscribeTopic += value;
                (Invoke(this.PtrHandle, "RegRspSubscribeTopic", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspSubscribeTopic));
            }
            remove
            {
                _OnRspSubscribeTopic -= value;
                (Invoke(this.PtrHandle, "RegRspSubscribeTopic", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspSubscribeTopic));
            }
        }

        /// 合规参数查询应答
        public delegate void RspQryComplianceParam(ref CUstpFtdcRspComplianceParamField pRspComplianceParam, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryComplianceParam _OnRspQryComplianceParam;
        /// <summary>
        /// 合规参数查询应答
        /// </summary>
        public event RspQryComplianceParam OnRspQryComplianceParam
        {
            add
            {
                _OnRspQryComplianceParam += value;
                (Invoke(this.PtrHandle, "RegRspQryComplianceParam", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryComplianceParam));
            }
            remove
            {
                _OnRspQryComplianceParam -= value;
                (Invoke(this.PtrHandle, "RegRspQryComplianceParam", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryComplianceParam));
            }
        }

        /// 主题查询应答
        public delegate void RspQryTopic(ref CUstpFtdcDisseminationField pDissemination, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryTopic _OnRspQryTopic;
        /// <summary>
        /// 主题查询应答
        /// </summary>
        public event RspQryTopic OnRspQryTopic
        {
            add
            {
                _OnRspQryTopic += value;
                (Invoke(this.PtrHandle, "RegRspQryTopic", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryTopic));
            }
            remove
            {
                _OnRspQryTopic -= value;
                (Invoke(this.PtrHandle, "RegRspQryTopic", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryTopic));
            }
        }

        /// 投资者手续费率查询应答
        public delegate void RspQryInvestorFee(ref CUstpFtdcInvestorFeeField pInvestorFee, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryInvestorFee _OnRspQryInvestorFee;
        /// <summary>
        /// 投资者手续费率查询应答
        /// </summary>
        public event RspQryInvestorFee OnRspQryInvestorFee
        {
            add
            {
                _OnRspQryInvestorFee += value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorFee", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorFee));
            }
            remove
            {
                _OnRspQryInvestorFee -= value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorFee", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorFee));
            }
        }

        /// 投资者保证金率查询应答
        public delegate void RspQryInvestorMargin(ref CUstpFtdcInvestorMarginField pInvestorMargin, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryInvestorMargin _OnRspQryInvestorMargin;
        /// <summary>
        /// 投资者保证金率查询应答
        /// </summary>
        public event RspQryInvestorMargin OnRspQryInvestorMargin
        {
            add
            {
                _OnRspQryInvestorMargin += value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorMargin", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorMargin));
            }
            remove
            {
                _OnRspQryInvestorMargin -= value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorMargin", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorMargin));
            }
        }

        /// 交易编码组合持仓查询应答
        public delegate void RspQryInvestorCombPosition(ref CUstpFtdcRspInvestorCombPositionField pRspInvestorCombPosition, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryInvestorCombPosition _OnRspQryInvestorCombPosition;
        /// <summary>
        /// 交易编码组合持仓查询应答
        /// </summary>
        public event RspQryInvestorCombPosition OnRspQryInvestorCombPosition
        {
            add
            {
                _OnRspQryInvestorCombPosition += value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorCombPosition", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorCombPosition));
            }
            remove
            {
                _OnRspQryInvestorCombPosition -= value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorCombPosition", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorCombPosition));
            }
        }

        /// 交易编码单腿持仓查询应答
        public delegate void RspQryInvestorLegPosition(ref CUstpFtdcRspInvestorLegPositionField pRspInvestorLegPosition, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryInvestorLegPosition _OnRspQryInvestorLegPosition;
        /// <summary>
        /// 交易编码单腿持仓查询应答
        /// </summary>
        public event RspQryInvestorLegPosition OnRspQryInvestorLegPosition
        {
            add
            {
                _OnRspQryInvestorLegPosition += value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorLegPosition", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorLegPosition));
            }
            remove
            {
                _OnRspQryInvestorLegPosition -= value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorLegPosition", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorLegPosition));
            }
        }

        /// 交叉汇率查询应答
        public delegate void RspQryExchangeRate(ref CUstpFtdcRspExchangeRateField pRspExchangeRate, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryExchangeRate _OnRspQryExchangeRate;
        /// <summary>
        /// 交叉汇率查询应答
        /// </summary>
        public event RspQryExchangeRate OnRspQryExchangeRate
        {
            add
            {
                _OnRspQryExchangeRate += value;
                (Invoke(this.PtrHandle, "RegRspQryExchangeRate", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryExchangeRate));
            }
            remove
            {
                _OnRspQryExchangeRate -= value;
                (Invoke(this.PtrHandle, "RegRspQryExchangeRate", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryExchangeRate));
            }
        }

    }
}
