using System;
using System.IO;
using System.Runtime.InteropServices;

namespace TradingMaster
{
    /// <summary>
    /// 交易接口
    /// </summary>
    public class CtpTraderApi
    {
        const string DLLNAME = "dll\\CtpTraderApi.dll";

        /// <summary>
        /// TradeApi.dll/CTPTradeApi.dll/thosttraderapi.dll 放在主程序的执行文件夹中
        /// </summary>
        /// <param name="_investor">投资者帐号:9010256</param>
        /// <param name="_pwd">密码</param>
        /// <param name="_broker">经纪公司代码:16377  华西期货</param>
        /// <param name="_addr">前置地址:默认为CTP模拟</param>
        public CtpTraderApi(string _investor, string _pwd, string _broker
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
            string dir = Environment.CurrentDirectory;// +"\\dll\\Users";
            if (File.Exists(strFile))
            {
                _DllFile = "CtpTraderApi_" + this.InvestorID + "_" + DateTime.Now.Ticks + ".dll";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                File.Copy(strFile, dir + "\\" + _DllFile);
                this.PtrHandle = LoadLibrary(dir + "\\" + _DllFile);
                //File.Copy(strFile, dir + "\\CtpTradeApi.dll");
                //this.PtrHandle = LoadLibrary(dir + "\\CtpTradeApi.dll");
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
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
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
                string strUserDir = Environment.CurrentDirectory;// +"\\dll\\Users";
                if (Directory.Exists(strUserDir))
                {
                    string[] dllFiles = Directory.GetFiles(strUserDir);
                    foreach (string fileItem in dllFiles)
                    {
                        if (fileItem.StartsWith(strUserDir + "\\CtpTraderApi_") && fileItem.EndsWith(".dll") && IsFileAvailable(fileItem))// + this.InvestorID + "_"
                        {
                            File.Delete(fileItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
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
            //((DefCreateApi)Invoke(this._handle, "CreateApi", typeof(DefCreateApi)))();
            ((reqConnect)Invoke(this.PtrHandle, "Connect", typeof(reqConnect)))(this.FrontAddr);
        }

        /// <summary>
        /// 断开
        /// </summary>
        private delegate int reqDisConnect();
        public void DisConnect()
        {
            ((reqDisConnect)Invoke(this.PtrHandle, "DisConnect", typeof(reqDisConnect)))();
        }

        /// <summary>
        /// 获取交易日
        /// </summary>
        /// <returns></returns>
        public string GetTradingDay()
        { return getTradingDay(); }
        [DllImport(DLLNAME, EntryPoint = "?GetTradingDay@@YAPBDXZ", CallingConvention = CallingConvention.Cdecl)]
        static extern string getTradingDay();

        /// <summary>
        /// 登入请求
        /// </summary>
        private delegate int reqUserLogin(string pBroker, string pInvestor, string pPwd);
        public int ReqUserLogin()
        {
            return ((reqUserLogin)Invoke(this.PtrHandle, "ReqUserLogin", typeof(reqUserLogin)))(this.BrokerID, this.InvestorID, this.Password);
        }

        /// <summary>
        /// 发送登出请求
        /// </summary>
        private delegate int reqUserLogout(string BROKER_ID, string INVESTOR_ID);
        public int ReqUserLogout()
        {
            return ((reqUserLogout)Invoke(this.PtrHandle, "ReqUserLogout", typeof(reqUserLogout)))(this.BrokerID, this.InvestorID);
        }

        /// <summary>
        /// 更新用户口令
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        private delegate int reqUserPasswordUpdate(string BROKER_ID, string USER_ID, string OLD_PASSWORD, string NEW_PASSWORD);
        public int UserPasswordupdate(string userID, string oldPassword, string newPassword)
        {
            return ((reqUserPasswordUpdate)Invoke(this.PtrHandle, "ReqUserPasswordUpdate", typeof(reqUserPasswordUpdate)))(this.BrokerID, userID, oldPassword, newPassword);
        }

        /// <summary>
        /// 资金账户口令更新请求
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        private delegate int reqTradingAccountPasswordUpdate(string BROKER_ID, string ACCOUNT_ID, string OLD_PASSWORD, string NEW_PASSWORD);
        public int TradingAccountPasswordUpdate(string accountID, string oldPassword, string newPassword)
        {
            return ((reqTradingAccountPasswordUpdate)Invoke(this.PtrHandle, "ReqTradingAccountPasswordUpdate", typeof(reqTradingAccountPasswordUpdate)))(this.BrokerID, accountID, oldPassword, newPassword);
        }

        /// <summary>
        /// 下单:录入报单
        /// </summary>
        /// <param name="order">输入的报单</param>
        private delegate int reqOrderInsert(ref CThostFtdcInputOrderField pField);
        public int OrderInsert(CThostFtdcInputOrderField pOrder)
        {
            return ((reqOrderInsert)Invoke(this.PtrHandle, "ReqOrderInsert", typeof(reqOrderInsert)))(ref pOrder);
        }
        /// <summary>
        /// 下单:录入报单
        /// </summary>
        /// <param name="InstrumentID">合约代码</param>
        /// <param name="OffsetFlag">平仓:仅上期所平今时使用CloseToday/其它情况均使用Close</param>
        /// <param name="Direction">买卖</param>
        /// <param name="Price">价格</param>
        /// <param name="Volume">手数</param>
        public int OrderInsert(string InstrumentID, EnumThostDirectionType Direction, EnumThostOffsetFlagType OffsetFlag, double Price, int Volume, double touchPrice, EnumOrderType orderType = EnumOrderType.Limit, EnumThostHedgeFlagType hedge = EnumThostHedgeFlagType.Speculation)
        {
            CThostFtdcInputOrderField pOrder = new CThostFtdcInputOrderField();
            pOrder.BrokerID = this.BrokerID;
            pOrder.BusinessUnit = null;
            pOrder.ContingentCondition = EnumThostContingentConditionType.Immediately;
            pOrder.ForceCloseReason = EnumThostForceCloseReasonType.NotForceClose;
            pOrder.InvestorID = this.InvestorID;
            pOrder.IsAutoSuspend = (int)EnumThostBoolType.No;
            pOrder.MinVolume = 1;
            pOrder.OrderRef = "" + (++this.MaxOrderRef).ToString();
            pOrder.UserForceClose = (int)EnumThostBoolType.No;
            pOrder.UserID = this.InvestorID;

            pOrder.CombHedgeFlag_0 = hedge;
            pOrder.InstrumentID = InstrumentID;
            pOrder.CombOffsetFlag_0 = OffsetFlag;
            pOrder.Direction = Direction;
            pOrder.VolumeTotalOriginal = Volume;

            if (orderType == EnumOrderType.Limit)
            {
                pOrder.LimitPrice = Price;
                pOrder.OrderPriceType = EnumThostOrderPriceTypeType.LimitPrice;
                pOrder.TimeCondition = EnumThostTimeConditionType.GFD;	//当日有效
                pOrder.VolumeCondition = EnumThostVolumeConditionType.AV;
            }
            else if (orderType == EnumOrderType.Opening)
            {
                pOrder.LimitPrice = Price;
                pOrder.OrderPriceType = EnumThostOrderPriceTypeType.LimitPrice;
                pOrder.TimeCondition = EnumThostTimeConditionType.GFA;	//集合竞价有效
                pOrder.VolumeCondition = EnumThostVolumeConditionType.AV;
            }
            else if (orderType == EnumOrderType.Market)
            {
                pOrder.LimitPrice = 0;
                pOrder.OrderPriceType = EnumThostOrderPriceTypeType.AnyPrice;
                pOrder.TimeCondition = EnumThostTimeConditionType.IOC;
                pOrder.VolumeCondition = EnumThostVolumeConditionType.AV;
            }
            else if (orderType == EnumOrderType.FAK)
            {
                pOrder.LimitPrice = Price;
                pOrder.OrderPriceType = EnumThostOrderPriceTypeType.LimitPrice;
                pOrder.TimeCondition = EnumThostTimeConditionType.IOC;
                pOrder.VolumeCondition = EnumThostVolumeConditionType.AV;
            }
            else if (orderType == EnumOrderType.FOK)
            {
                pOrder.LimitPrice = Price;
                pOrder.OrderPriceType = EnumThostOrderPriceTypeType.LimitPrice;
                pOrder.TimeCondition = EnumThostTimeConditionType.IOC;
                pOrder.VolumeCondition = EnumThostVolumeConditionType.CV;
            }
            else if (orderType == EnumOrderType.StopLimit)//ToDo
            {
                pOrder.ContingentCondition = EnumThostContingentConditionType.Immediately;
                pOrder.LimitPrice = Price;
                pOrder.OrderPriceType = EnumThostOrderPriceTypeType.LimitPrice;
                pOrder.StopPrice = touchPrice;
                pOrder.TimeCondition = EnumThostTimeConditionType.GFD;
            }
            else if (orderType == EnumOrderType.MIT)//ToDo
            {
                pOrder.ContingentCondition = EnumThostContingentConditionType.Immediately;
                //pOrder.LimitPrice = Price;
                pOrder.OrderPriceType = EnumThostOrderPriceTypeType.BestPrice;
                pOrder.StopPrice = touchPrice;
                pOrder.TimeCondition = EnumThostTimeConditionType.GFD;
            }
            return ((reqOrderInsert)Invoke(this.PtrHandle, "ReqOrderInsert", typeof(reqOrderInsert)))(ref pOrder);
        }

        /// <summary>
        /// 撤单
        /// </summary>
        /// <param name="InstrumentID"></param>
        /// <param name="FrontID"></param>
        /// <param name="SessionID"></param>
        /// <param name="OrderRef"></param>
        /// <param name="ExchangeID"></param>
        /// <param name="OrderSysID"></param>
        private delegate int reqOrderAction(ref CThostFtdcInputOrderActionField pField);
        public int OrderAction(string InstrumentID, int FrontID = 0, int SessionID = 0, string OrderRef = "0", string ExchangeID = null, string OrderSysID = null)
        {
            CThostFtdcInputOrderActionField pOrderAction = new CThostFtdcInputOrderActionField();
            pOrderAction.ActionFlag = EnumThostActionFlagType.Delete;
            pOrderAction.BrokerID = this.BrokerID;
            pOrderAction.InvestorID = this.InvestorID;
            //tmp.UserID = this.InvestorID;
            pOrderAction.InstrumentID = InstrumentID;
            //tmp.VolumeChange = int.Parse(lvi.SubItems["VolumeTotalOriginal"].Text);
            if (FrontID != 0)
                pOrderAction.FrontID = FrontID;
            if (SessionID != 0)
                pOrderAction.SessionID = SessionID;
            if (OrderRef != "0")
                pOrderAction.OrderRef = OrderRef;
            pOrderAction.ExchangeID = ExchangeID;
            if (OrderSysID != null)
                pOrderAction.OrderSysID = new string('\0', 20 - OrderSysID.Length) + OrderSysID;	//OrderSysID右对齐
            return ((reqOrderAction)Invoke(this.PtrHandle, "ReqOrderAction", typeof(reqOrderAction)))(ref pOrderAction);
        }

        /// <summary>
        /// 查询最大允许报单数量请求
        /// </summary>
        /// <param name="pMaxOrderVolume"></param>
        private delegate int reqQueryMaxOrderVolume(ref CThostFtdcQueryMaxOrderVolumeField pField);
        public int ReqQueryMaxOrderVolume(CThostFtdcQueryMaxOrderVolumeField pMaxOrderVolume)
        {
            return ((reqQueryMaxOrderVolume)Invoke(this.PtrHandle, "ReqQueryMaxOrderVolume", typeof(reqQueryMaxOrderVolume)))(ref pMaxOrderVolume);
        }

        /// <summary>
        /// 请求查询报单:不填-查所有
        /// </summary>
        private delegate int reqQryOrder(ref CThostFtdcQryOrderField pField);
        /// <param name="_exchangeID"></param>
        /// <param name="_timeStart"></param>
        /// <param name="_timeEnd"></param>
        /// <param name="_instrumentID"></param>
        /// <param name="_orderSysID"></param>
        public int QryOrder(string _exchangeID = null, string _timeStart = null, string _timeEnd = null, string _instrumentID = null, string _orderSysID = null)
        {
            CThostFtdcQryOrderField pQryOrder = new CThostFtdcQryOrderField();
            pQryOrder.BrokerID = this.BrokerID;
            pQryOrder.InvestorID = this.InvestorID;
            pQryOrder.ExchangeID = _exchangeID;
            pQryOrder.InsertTimeStart = _timeStart;
            pQryOrder.InsertTimeEnd = _timeEnd;
            pQryOrder.InstrumentID = _instrumentID;
            pQryOrder.OrderSysID = _orderSysID;
            return ((reqQryOrder)Invoke(this.PtrHandle, "ReqQryOrder", typeof(reqQryOrder)))(ref pQryOrder);
        }

        /// <summary>
        ///  请求查询成交:不填-查所有
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
            pQryTrade.InvestorID = this.InvestorID;
            pQryTrade.ExchangeID = _exchangeID;
            pQryTrade.TradeTimeStart = _timeStart == null ? null : _timeStart.Value.ToString("HH:mm:ss");
            pQryTrade.TradeTimeEnd = _timeEnd == null ? null : _timeEnd.Value.ToString("HH:mm:ss");
            pQryTrade.InstrumentID = _instrumentID;
            pQryTrade.TradeID = _tradeID;
            //return reqQryTrade(ref tmp);
            return ((reqQryTrade)Invoke(this.PtrHandle, "ReqQryTrade", typeof(reqQryTrade)))(ref pQryTrade);
        }

        /// <summary>
        /// 查询投资者持仓
        /// </summary>
        /// <param name="instrument">合约代码:不填-查所有</param>
        private delegate int reqQryInvestorPosition(string BROKER_ID, string INVESTOR_ID, string Instrument);
        public int QryInvestorPosition(string instrument = null)
        {
            return ((reqQryInvestorPosition)Invoke(this.PtrHandle, "ReqQryInvestorPosition", typeof(reqQryInvestorPosition)))(this.BrokerID, this.InvestorID, instrument);
        }

        /// <summary>
        /// 查询投资者持仓明细
        /// </summary>
        /// <param name="instrument">合约代码:不填-查所有</param>
        private delegate int reqQryInvestorPositionDetail(string BROKER_ID, string INVESTOR_ID, string Instrument);
        public int QryInvestorPositionDetail(string instrument = null)
        {
            return ((reqQryInvestorPositionDetail)Invoke(this.PtrHandle, "ReqQryInvestorPositionDetail", typeof(reqQryInvestorPositionDetail)))(this.BrokerID, this.InvestorID, instrument);
        }

        /// <summary>
        /// 请求查询投资者组合持仓明细
        /// </summary>
        /// <param name="instrumentID">合约代码:不填-查所有</param>
        private delegate int reqQryInvestorPositionCombineDetail(string BROKER_ID, string INVESTOR_ID, string INSTRUMENT_ID);
        public int QryInvestorPositionCombineDetail(string instrumentID = null)
        {
            return ((reqQryInvestorPositionCombineDetail)Invoke(this.PtrHandle, "ReqQryInvestorPositionCombineDetail", typeof(reqQryInvestorPositionCombineDetail)))(this.BrokerID, this.InvestorID, instrumentID);
        }

        /// <summary>
        /// 查询帐户资金请求
        /// </summary>
        private delegate int reqQryTradingAccount(string BROKER_ID, string INVESTOR_ID);
        public int QryTradingAccount()
        {
            return ((reqQryTradingAccount)Invoke(this.PtrHandle, "ReqQryTradingAccount", typeof(reqQryTradingAccount)))(this.BrokerID, this.InvestorID);
        }

        /// <summary>
        /// 请求查询投资者
        /// </summary>
        private delegate int reqQryInvestor(string BROKER_ID, string INVESTOR_ID);
        public int QryInvestor()
        {
            return ((reqQryInvestor)Invoke(this.PtrHandle, "ReqQryInvestor", typeof(reqQryInvestor)))(this.BrokerID, this.InvestorID);
        }

        /// <summary>
        /// 请求查询交易编码:参数不填-查所有
        /// </summary>
        /// <param name="clientID">客户ID</param>
        /// <param name="exchangeID">交易所ID</param>
        private delegate int reqQryTradingCode(string BROKER_ID, string INVESTOR_ID, string CLIENT_ID, string EXCHANGE_ID);
        public int QryTradingCode(string clientID = null, string exchangeID = null)
        {
            return ((reqQryTradingCode)Invoke(this.PtrHandle, "ReqQryTradingCode", typeof(reqQryTradingCode)))(this.BrokerID, this.InvestorID, clientID, exchangeID);
        }

        /// <summary>
        ///请求查询交易所调整保证金率
        /// </summary>
        private delegate int reqQryExchangeMarginRateAdjust(ref CThostFtdcQryExchangeMarginRateAdjustField pField);
        public int ReqQryExchangeMarginRateAdjust(string instrumentID)
        {
            CThostFtdcQryExchangeMarginRateAdjustField pQryExchangeMarginRateAdjust = new CThostFtdcQryExchangeMarginRateAdjustField();
            pQryExchangeMarginRateAdjust.BrokerID = this.BrokerID;
            pQryExchangeMarginRateAdjust.HedgeFlag = EnumThostHedgeFlagType.Speculation;
            pQryExchangeMarginRateAdjust.InstrumentID = instrumentID;
            return ((reqQryExchangeMarginRateAdjust)Invoke(this.PtrHandle, "ReqQryExchangeMarginRateAdjust", typeof(reqQryExchangeMarginRateAdjust)))(ref pQryExchangeMarginRateAdjust);
        }

        /// <summary>
        /// 请求查询合约保证金率:能为null;每次只能查一个合约
        /// </summary>
        /// <param name="hedgeFlag">投机/套保</param>
        /// <param name="instrumentID">合约代码:不填-查所有</param>
        private delegate int reqQryInstrumentMarginRate(string BROKER_ID, string INVESTOR_ID, string INSTRUMENT_ID, EnumThostHedgeFlagType HEDGE_FLAG);
        public int QryInstrumentMarginRate(string instrumentID, EnumThostHedgeFlagType hedgeFlag = EnumThostHedgeFlagType.Speculation)
        {
            return ((reqQryInstrumentMarginRate)Invoke(this.PtrHandle, "ReqQryInstrumentMarginRate", typeof(reqQryInstrumentMarginRate)))(this.BrokerID, this.InvestorID, instrumentID, hedgeFlag);
        }

        /// <summary>
        /// 请求查询合约手续费率
        /// </summary>
        /// <param name="instrumentID">合约代码</param>
        private delegate int reqQryInstrumentCommissionRate(string BROKER_ID, string INVESTOR_ID, string INSTRUMENT_ID);
        public int QryInstrumentCommissionRate(string instrumentID)
        {
            return ((reqQryInstrumentCommissionRate)Invoke(this.PtrHandle, "ReqQryInstrumentCommissionRate", typeof(reqQryInstrumentCommissionRate)))(this.BrokerID, this.InvestorID, instrumentID);
        }

        /// <summary>
        /// 请求查询汇率
        /// </summary>
        private delegate int reqQryExchangeRate(ref CThostFtdcQryExchangeRateField pField);
        public int ReqQryExchangeRate(string srcCurrency, string destCurrency)
        {
            CThostFtdcQryExchangeRateField pQryExchangeRate = new CThostFtdcQryExchangeRateField();
            pQryExchangeRate.BrokerID = this.BrokerID;
            pQryExchangeRate.FromCurrencyID = srcCurrency;
            pQryExchangeRate.ToCurrencyID = destCurrency;
            return ((reqQryExchangeRate)Invoke(this.PtrHandle, "ReqQryExchangeRate", typeof(reqQryExchangeRate)))(ref pQryExchangeRate);
        }

        /// <summary>
        /// 请求查询产品报价汇率
        /// </summary>
        private delegate int reqQryProductExchRate(ref CThostFtdcQryProductExchRateField pField);
        public int ReqQryProductExchRate(string productID)
        {
            CThostFtdcQryProductExchRateField pQryProductExchRate = new CThostFtdcQryProductExchRateField();
            pQryProductExchRate.ProductID = productID;
            return ((reqQryProductExchRate)Invoke(this.PtrHandle, "ReqQryProductExchRate", typeof(reqQryProductExchRate)))(ref pQryProductExchRate);
        }

        /// <summary>
        /// 请求查询交易所
        /// </summary>
        /// <param name="exchangeID"></param>
        private delegate int reqQryExchange(string EXCHANGE_ID);
        public int QryExchange(string exchangeID)
        {
            return ((reqQryExchange)Invoke(this.PtrHandle, "ReqQryExchange", typeof(reqQryExchange)))(exchangeID);
        }
        /// <summary>
        /// 查询合约
        /// </summary>
        /// <param name="instrument">合约代码:不填-查所有</param>
        private delegate int qryInstrument(string Instrument);
        public int QryInstrument(string instrument = null)
        {
            return ((qryInstrument)Invoke(this.PtrHandle, "ReqQryInstrument", typeof(qryInstrument)))(instrument);
        }

        /// <summary>
        /// 查询行情
        /// </summary>
        /// <param name="instrument">合约代码</param>
        public int QryDepthMarketData(string instrument) { return reqQryDepthMarketData(instrument); }
        [DllImport(DLLNAME, EntryPoint = "ReqQryDepthMarketData", CallingConvention = CallingConvention.Cdecl)]
        static extern int reqQryDepthMarketData(string Instrument);

        /// <summary>
        /// 请求查询投资者结算结果
        /// </summary>
        private delegate int reqQrySettlementInfo(string BROKER_ID, string INVESTOR_ID, string TRADING_DAY);
        public int QrySettlementInfo(string date = null)
        {
            return ((reqQrySettlementInfo)Invoke(this.PtrHandle, "ReqQrySettlementInfo", typeof(reqQrySettlementInfo)))(this.BrokerID, this.InvestorID, date);
        }

        /// <summary>
        /// 确认结算结果
        /// </summary>
        private delegate int reqSettlementInfoConfirm(string BROKER_ID, string INVESTOR_ID);
        public int SettlementInfoConfirm()
        {
            return ((reqSettlementInfoConfirm)Invoke(this.PtrHandle, "ReqSettlementInfoConfirm", typeof(reqSettlementInfoConfirm)))(this.BrokerID, this.InvestorID);
        }

        /// <summary>
        /// 请求查询结算信息确认
        /// </summary>
        private delegate int reqQrySettlementInfoConfirm(string BROKER_ID, string INVESTOR_ID);
        public int QrySettlementInfoConfirm()
        {
            return ((reqQrySettlementInfoConfirm)Invoke(this.PtrHandle, "ReqQrySettlementInfoConfirm", typeof(reqQrySettlementInfoConfirm)))(this.BrokerID, this.InvestorID);
        }

        /// <summary>
        /// 请求查询客户通知
        /// </summary>
        public int QryNotice() { return reqQryNotice(this.BrokerID); }
        [DllImport(DLLNAME, EntryPoint = "?ReqQryNotice@@YAHQAD@Z", CallingConvention = CallingConvention.Cdecl)]
        static extern int reqQryNotice(string BROKERID);

        /// <summary>
        /// 请求查询保证金监管系统经纪公司资金账户密钥
        /// </summary>
        public int QryCFMMCTradingAccountKey()
        { return reqQryCFMMCTradingAccountKey(this.BrokerID, this.InvestorID); }
        [DllImport(DLLNAME, EntryPoint = "?ReqQryCFMMCTradingAccountKey@@YAHQAD0@Z", CallingConvention = CallingConvention.Cdecl)]
        static extern int reqQryCFMMCTradingAccountKey(string BROKER_ID, string INVESTOR_ID);

        /// <summary>
        /// 请求查询交易通知
        /// </summary>
        private delegate int reqQryTradingNotice(string BROKER_ID, string INVESTOR_ID);
        public int QryTradingNotice()
        {
            return ((reqQryTradingNotice)Invoke(this.PtrHandle, "ReqQryTradingNotice", typeof(reqQryTradingNotice)))(this.BrokerID, this.InvestorID);
        }

        /// <summary>
        /// 请求查询经纪公司交易参数
        /// </summary>
        private delegate int reqQryBrokerTradingParams(string broker, string investor);
        public int ReqQryBrokerTradingParams()
        {
            return ((reqQryBrokerTradingParams)Invoke(this.PtrHandle, "ReqQryBrokerTradingParams", typeof(reqQryBrokerTradingParams)))(this.BrokerID, this.InvestorID);
        }

        /// <summary>
        /// 请求查询经纪公司交易算法
        /// </summary>
        /// <param name="exchangeID">交易所代码</param>
        /// <param name="instrumentID">合约代码</param>
        public int QryBrokerTradingAlgos(string exchangeID, string instrumentID)
        { return reqQryBrokerTradingAlgos(this.BrokerID, exchangeID, instrumentID); }
        [DllImport(DLLNAME, EntryPoint = "?ReqQryBrokerTradingAlgos@@YAHQAD00@Z", CallingConvention = CallingConvention.Cdecl)]
        static extern int reqQryBrokerTradingAlgos(string BROKER_ID, string EXCHANGE_ID, string INSTRUMENT_ID);

        /// <summary>
        /// 预埋单录入请求
        /// </summary>
        /// <param name="field"></param>
        private delegate int reqParkedOrderInsert(ref CThostFtdcParkedOrderField pField);
        public int ParkedOrderInsert(CThostFtdcParkedOrderField field)
        {
            return ((reqParkedOrderInsert)Invoke(this.PtrHandle, "ReqParkedOrderInsert", typeof(reqParkedOrderInsert)))(ref field);
        }
        /// <summary>
        /// 预埋单录入请求
        /// </summary>
        /// <param name="InstrumentID"></param>
        /// <param name="OffsetFlag"></param>
        /// <param name="Direction"></param>
        /// <param name="Price"></param>
        /// <param name="Volume"></param>
        public int ParkedOrderInsert(string InstrumentID, EnumThostOffsetFlagType OffsetFlag, EnumThostDirectionType Direction, double Price, int Volume)
        {
            CThostFtdcParkedOrderField tmp = new CThostFtdcParkedOrderField();
            tmp.BrokerID = this.BrokerID;
            tmp.BusinessUnit = null;
            tmp.ContingentCondition = EnumThostContingentConditionType.ParkedOrder;
            tmp.ForceCloseReason = EnumThostForceCloseReasonType.NotForceClose;
            tmp.InvestorID = this.InvestorID;
            tmp.IsAutoSuspend = (int)EnumThostBoolType.No;
            tmp.MinVolume = 1;
            tmp.OrderPriceType = EnumThostOrderPriceTypeType.LimitPrice;
            tmp.OrderRef = "" + (++this.MaxOrderRef).ToString();
            tmp.TimeCondition = EnumThostTimeConditionType.GFD;
            tmp.UserForceClose = (int)EnumThostBoolType.No;
            tmp.UserID = this.InvestorID;
            tmp.VolumeCondition = EnumThostVolumeConditionType.AV;
            tmp.CombHedgeFlag_0 = EnumThostHedgeFlagType.Speculation;

            tmp.InstrumentID = InstrumentID;
            tmp.CombOffsetFlag_0 = OffsetFlag;
            tmp.Direction = Direction;
            tmp.LimitPrice = Price;
            tmp.VolumeTotalOriginal = Volume;
            return ((reqParkedOrderInsert)Invoke(this.PtrHandle, "ReqParkedOrderInsert", typeof(reqParkedOrderInsert)))(ref tmp);
        }

        /// <summary>
        /// 预埋撤单录入请求
        /// </summary>
        /// <param name="field"></param>
        private delegate int reqParkedOrderAction(ref CThostFtdcParkedOrderActionField pField);
        public int ReqParkedOrderAction(CThostFtdcParkedOrderActionField field)
        {
            return ((reqParkedOrderAction)Invoke(this.PtrHandle, "ReqParkedOrderAction", typeof(reqParkedOrderAction)))(ref field);
        }
        public int ReqParkedOrderAction(string InstrumentID, int FrontID, int SessionID, string OrderRef, string ExchangeID = null, string OrderSysID = null)
        {
            CThostFtdcParkedOrderActionField tmp = new CThostFtdcParkedOrderActionField();
            tmp.ActionFlag = EnumThostActionFlagType.Delete;
            tmp.BrokerID = this.BrokerID;
            tmp.InvestorID = this.InvestorID;
            //tmp.UserID = this.InvestorID;
            tmp.InstrumentID = InstrumentID;
            //tmp.VolumeChange = int.Parse(lvi.SubItems["VolumeTotalOriginal"].Text);

            tmp.FrontID = FrontID;
            tmp.SessionID = SessionID;
            tmp.OrderRef = OrderRef;
            tmp.ExchangeID = ExchangeID;
            if (OrderSysID != null)
                tmp.OrderSysID = new string('\0', 21 - OrderSysID.Length) + OrderSysID;	//OrderSysID右对齐
            return ((reqParkedOrderAction)Invoke(this.PtrHandle, "ReqParkedOrderAction", typeof(reqParkedOrderAction)))(ref tmp);
        }

        /// <summary>
        /// 请求删除预埋单
        /// </summary>
        /// <param name="BROKER_ID"></param>
        /// <param name="INVESTOR_ID"></param>
        /// <param name="ParkedOrder_ID"></param>
        private delegate int reqRemoveParkedOrder(string BROKER_ID, string INVESTOR_ID, string ParkedOrder_ID);
        public int ReqRemoveParkedOrder(string ParkedOrder_ID)
        {
            return ((reqRemoveParkedOrder)Invoke(this.PtrHandle, "ReqRemoveParkedOrder", typeof(reqRemoveParkedOrder)))(this.BrokerID, this.InvestorID, ParkedOrder_ID); ;
        }

        /// <summary>
        /// 请求删除预埋撤单
        /// </summary>
        /// <param name="BROKER_ID"></param>
        /// <param name="INVESTOR_ID"></param>
        /// <param name="ParkedOrderAction_ID"></param>
        private delegate int reqRemoveParkedOrderAction(string BROKER_ID, string INVESTOR_ID, string ParkedOrderAction_ID);
        public int ReqRemoveParkedOrderAction(string ParkedOrderAction_ID)
        {
            return ((reqRemoveParkedOrderAction)Invoke(this.PtrHandle, "ReqRemoveParkedOrderAction", typeof(reqRemoveParkedOrderAction)))(this.BrokerID, this.InvestorID, ParkedOrderAction_ID); ;
        }

        /// <summary>
        /// 请求查询银期签约关系
        /// </summary>
        /// <param name="BROKER_ID"></param>
        /// <param name="ACCOUNT_ID"></param>
        private delegate int reqQryAccountregister(string BROKER_ID, string ACCOUNT_ID);
        public int ReqQryAccountregister()
        {
            return ((reqQryAccountregister)Invoke(this.PtrHandle, "ReqQryAccountregister", typeof(reqQryAccountregister)))(this.BrokerID, this.InvestorID);
        }

        /// <summary>
        /// 请求查询转帐银行
        /// </summary>
        /// <param name="Bank_ID"></param>
        /// <param name="BankBrch_ID"></param>
        private delegate int reqQryTransferBank(string Bank_ID, string BankBrch_ID);
        public int ReqQryTransferBank(string Bank_ID, string BankBrch_ID)
        {
            return ((reqQryTransferBank)Invoke(this.PtrHandle, "ReqQryTransferBank", typeof(reqQryTransferBank)))(Bank_ID, BankBrch_ID);
        }

        /// <summary>
        /// 请求查询转帐流水
        /// </summary>
        /// <param name="Broker_ID"></param>
        /// <param name="Account_ID"></param>
        /// <param name="Bank_ID"></param>
        private delegate int reqQryTransferSerial(string Broker_ID, string Account_ID, string Bank_ID);
        public int ReqQryTransferSerial(string Bank_ID)
        {
            return ((reqQryTransferSerial)Invoke(this.PtrHandle, "ReqQryTransferSerial", typeof(reqQryTransferSerial)))(this.BrokerID, this.InvestorID, Bank_ID);
        }

        /// <summary>
        /// 请求查询签约银行
        /// </summary>
        /// <param name="Broker_ID"></param>
        /// <param name="Bank_ID"></param>
        /// <param name="BankBrch_ID"></param>
        private delegate int reqQryContractBank(string Broker_ID, string Bank_ID, string BankBrch_ID);
        public int ReqQryContractBank()
        {
            return ((reqQryContractBank)Invoke(this.PtrHandle, "ReqQryContractBank", typeof(reqQryContractBank)))(this.BrokerID, null, null);
        }

        /// <summary>
        /// 请求查询预埋单
        /// </summary>
        /// <param name="Broker_ID"></param>
        /// <param name="Investor_ID"></param>
        /// <param name="Instrument_ID"></param>
        /// <param name="Exchange_ID"></param>
        private delegate int reqQryParkedOrder(string Broker_ID, string Investor_ID, string Instrument_ID, string Exchange_ID);
        public int ReqQryParkedOrder()
        {
            return ((reqQryParkedOrder)Invoke(this.PtrHandle, "ReqQryParkedOrder", typeof(reqQryParkedOrder)))(this.BrokerID, this.InvestorID, null, null);
        }

        /// <summary>
        /// 请求查询预埋撤单
        /// </summary>
        /// <param name="Broker_ID"></param>
        /// <param name="Investor_ID"></param>
        /// <param name="Instrument_ID"></param>
        /// <param name="Exchange_ID"></param>
        private delegate int reqQryParkedOrderAction(string Broker_ID, string Investor_ID, string Instrument_ID, string Exchange_ID);
        public int ReqQryParkedOrderAction()
        {
            return ((reqQryParkedOrderAction)Invoke(this.PtrHandle, "ReqQryParkedOrderAction", typeof(reqQryParkedOrderAction)))(this.BrokerID, this.InvestorID, null, null);
        }

        /// <summary>
        /// 期货发起银行资金转期货请求
        /// </summary>
        /// <param name="field"></param>
        private delegate int reqFromBankToFutureByFuture(ref CThostFtdcReqTransferField pField);
        public int ReqFromBankToFutureByFuture(CThostFtdcReqTransferField field)
        {
            return ((reqFromBankToFutureByFuture)Invoke(this.PtrHandle, "ReqFromBankToFutureByFuture", typeof(reqFromBankToFutureByFuture)))(ref field);
        }

        /// <summary>
        /// 期货发起期货资金转银行请求
        /// </summary>
        /// <param name="field"></param>
        private delegate int reqFromFutureToBankByFuture(ref CThostFtdcReqTransferField pField);
        public int ReqFromFutureToBankByFuture(CThostFtdcReqTransferField field)
        {
            return ((reqFromFutureToBankByFuture)Invoke(this.PtrHandle, "ReqFromFutureToBankByFuture", typeof(reqFromFutureToBankByFuture)))(ref field);
        }

        /// <summary>
        /// 期货发起查询银行余额请求
        /// </summary>
        /// <param name="field"></param>
        private delegate int reqQueryBankAccountMoneyByFuture(ref CThostFtdcReqQueryAccountField pField);
        public int ReqQueryBankAccountMoneyByFuture(CThostFtdcReqQueryAccountField field)
        {
            return ((reqQueryBankAccountMoneyByFuture)Invoke(this.PtrHandle, "ReqQueryBankAccountMoneyByFuture", typeof(reqQueryBankAccountMoneyByFuture)))(ref field);
        }

        /// <summary>
        /// 请求查询期权合约手续费率
        /// </summary>
        /// <param name="instrumentID">合约代码</param>
        private delegate int reqQryOptionInstrCommRate(string BROKER_ID, string INVESTOR_ID, string INSTRUMENT_ID);
        public int QryOptionInstrCommRate(string instrumentID)
        {
            return ((reqQryOptionInstrCommRate)Invoke(this.PtrHandle, "ReqQryOptionInstrCommRate", typeof(reqQryOptionInstrCommRate)))(this.BrokerID, this.InvestorID, instrumentID);
        }

        /// <summary>
        /// 请求查询期权交易成本
        /// </summary>
        private delegate int reqQryOptionInstrTradeCost(ref CThostFtdcQryOptionInstrTradeCostField pField);
        public int ReqQryOptionInstrTradeCost(string code, double price, double basePrice = 0)
        {
            CThostFtdcQryOptionInstrTradeCostField pQryOptionInstrTradeCost = new CThostFtdcQryOptionInstrTradeCostField();
            pQryOptionInstrTradeCost.BrokerID = this.BrokerID;
            pQryOptionInstrTradeCost.HedgeFlag = EnumThostHedgeFlagType.Speculation;
            pQryOptionInstrTradeCost.InputPrice = price;
            pQryOptionInstrTradeCost.InstrumentID = code;
            pQryOptionInstrTradeCost.InvestorID = this.InvestorID;
            pQryOptionInstrTradeCost.UnderlyingPrice = basePrice;
            return ((reqQryOptionInstrTradeCost)Invoke(this.PtrHandle, "ReqQryOptionInstrTradeCost", typeof(reqQryOptionInstrTradeCost)))(ref pQryOptionInstrTradeCost);
        }

        /// <summary>
        ///请求查询投资者品种/跨品种保证金
        /// </summary>
        private delegate int reqQryInvestorProductGroupMargin(ref CThostFtdcQryInvestorProductGroupMarginField pField);
        public int ReqQryInvestorProductGroupMargin(string instrumentID)
        {
            CThostFtdcQryInvestorProductGroupMarginField pQryInvestorProductGroupMargin = new CThostFtdcQryInvestorProductGroupMarginField();
            pQryInvestorProductGroupMargin.BrokerID = this.BrokerID;
            pQryInvestorProductGroupMargin.HedgeFlag = EnumThostHedgeFlagType.Speculation;
            pQryInvestorProductGroupMargin.InvestorID = this.InvestorID;
            pQryInvestorProductGroupMargin.ProductGroupID = instrumentID;
            return ((reqQryInvestorProductGroupMargin)Invoke(this.PtrHandle, "ReqQryInvestorProductGroupMargin", typeof(reqQryInvestorProductGroupMargin)))(ref pQryInvestorProductGroupMargin);
        }

        /// <summary>
        /// 请求查询询价
        /// </summary>
        private delegate int reqQryForQuote(ref CThostFtdcQryForQuoteField pQryForQuote);
        public int ReqQryForQuote(CThostFtdcQryForQuoteField pQryForQuote)
        {
            return ((reqQryForQuote)Invoke(this.PtrHandle, "ReqQryForQuote", typeof(reqQryForQuote)))(ref pQryForQuote);
        }

        /// <summary>
        /// 询价录入请求
        /// </summary>
        private delegate int reqForQuoteInsert(ref CThostFtdcInputForQuoteField pInputForQuote);
        public int ReqForQuoteInsert(string instrumentID)
        {
            CThostFtdcInputForQuoteField pfield = new CThostFtdcInputForQuoteField();
            pfield.BrokerID = this.BrokerID;
            pfield.InstrumentID = instrumentID;
            pfield.InvestorID = this.InvestorID;
            pfield.ForQuoteRef = (++this.MaxOrderRef).ToString(); //"";
            pfield.UserID = this.InvestorID;
            return ((reqForQuoteInsert)Invoke(this.PtrHandle, "ReqForQuoteInsert", typeof(reqForQuoteInsert)))(ref pfield);
        }

        /// <summary>
        /// 请求查询报价
        /// </summary>
        private delegate int reqQryQuote(ref CThostFtdcQryQuoteField pField);
        public int ReqQryQuote(string exchangeID = null, string timeStart = null, string timeEnd = null, string instrumentID = null, string quoteSysID = null)
        {
            CThostFtdcQryQuoteField pQryQuote = new CThostFtdcQryQuoteField();
            pQryQuote.BrokerID = this.BrokerID;
            pQryQuote.InvestorID = this.InvestorID;
            pQryQuote.ExchangeID = exchangeID;
            pQryQuote.InsertTimeStart = timeStart;
            pQryQuote.InsertTimeEnd = timeEnd;
            pQryQuote.InstrumentID = instrumentID;
            pQryQuote.QuoteSysID = quoteSysID;
            return ((reqQryQuote)Invoke(this.PtrHandle, "ReqQryQuote", typeof(reqQryQuote)))(ref pQryQuote);
        }

        /// <summary>
        /// 报价录入请求
        /// </summary>
        private delegate int reqQuoteInsert(ref CThostFtdcInputQuoteField pField);
        public int ReqQuoteInsert(CThostFtdcInputQuoteField pInputQuote)
        {
            return ((reqQuoteInsert)Invoke(this.PtrHandle, "ReqQuoteInsert", typeof(reqQuoteInsert)))(ref pInputQuote);
        }

        /// <summary>
        /// 报价录入请求:限价单
        /// </summary>
        public int ReqQuoteInsert(string InstrumentID, EnumThostOffsetFlagType BidOffset, double BidPrice, int BidVolume, EnumThostOffsetFlagType AskOffset, double AskPrice, int AskVolume, string ForQuoteID = "")
        {
            CThostFtdcInputQuoteField pInputQuote = new CThostFtdcInputQuoteField();
            pInputQuote.BrokerID = this.BrokerID;
            pInputQuote.BusinessUnit = null;
            pInputQuote.InvestorID = this.InvestorID;
            pInputQuote.InstrumentID = InstrumentID;
            pInputQuote.QuoteRef = "" + (++this.MaxOrderRef).ToString();
            pInputQuote.UserID = this.InvestorID;
            pInputQuote.ForQuoteSysID = ForQuoteID;

            pInputQuote.BidOrderRef = "" + (++this.MaxOrderRef).ToString();
            pInputQuote.BidOffsetFlag = BidOffset;
            pInputQuote.BidPrice = BidPrice;
            pInputQuote.BidVolume = BidVolume;
            pInputQuote.BidHedgeFlag = EnumThostHedgeFlagType.Speculation;

            pInputQuote.AskOrderRef = "" + (++this.MaxOrderRef).ToString();
            pInputQuote.AskOffsetFlag = AskOffset;
            pInputQuote.AskPrice = AskPrice;
            pInputQuote.AskVolume = AskVolume;
            pInputQuote.AskHedgeFlag = EnumThostHedgeFlagType.Speculation;
            return ((reqQuoteInsert)Invoke(this.PtrHandle, "ReqQuoteInsert", typeof(reqQuoteInsert)))(ref pInputQuote);
        }

        /// <summary>
        /// 报价操作请求
        /// </summary>
        private delegate int reqQuoteAction(ref CThostFtdcInputQuoteActionField pField);
        public int ReqQuoteAction(string InstrumentID, int FrontID = 0, int SessionID = 0, string QuoteRef = "0", string ExchangeID = null, string QuoteSysID = null)
        {
            CThostFtdcInputQuoteActionField pInputQuoteAction = new CThostFtdcInputQuoteActionField();
            pInputQuoteAction.ActionFlag = EnumThostActionFlagType.Delete;
            pInputQuoteAction.BrokerID = this.BrokerID;
            pInputQuoteAction.InvestorID = this.InvestorID;
            //tmp.UserID = this.InvestorID;
            pInputQuoteAction.InstrumentID = InstrumentID;
            //tmp.VolumeChange = int.Parse(lvi.SubItems["VolumeTotalOriginal"].Text);
            if (FrontID != 0)
                pInputQuoteAction.FrontID = FrontID;
            if (SessionID != 0)
                pInputQuoteAction.SessionID = SessionID;
            if (QuoteRef != "0")
                pInputQuoteAction.QuoteRef = QuoteRef;
            pInputQuoteAction.ExchangeID = ExchangeID;
            if (QuoteSysID != null)
                pInputQuoteAction.QuoteSysID = new string('\0', 20 - QuoteSysID.Length) + QuoteSysID;	//QuoteSysID右对齐
            return ((reqQuoteAction)Invoke(this.PtrHandle, "ReqQuoteAction", typeof(reqQuoteAction)))(ref pInputQuoteAction);
        }

        /// <summary>
        /// 请求查询执行宣告
        /// </summary>
        private delegate int reqQryExecOrder(ref CThostFtdcQryExecOrderField pField);
        public int ReqQryExecOrder(string exchangeID = null, string timeStart = null, string timeEnd = null, string instrumentID = null, string execSysID = null)
        {
            CThostFtdcQryExecOrderField pQryExecOrder = new CThostFtdcQryExecOrderField();
            pQryExecOrder.BrokerID = this.BrokerID;
            pQryExecOrder.InvestorID = this.InvestorID;
            pQryExecOrder.ExchangeID = exchangeID;
            pQryExecOrder.InsertTimeStart = timeStart;
            pQryExecOrder.InsertTimeEnd = timeEnd;
            pQryExecOrder.InstrumentID = instrumentID;
            pQryExecOrder.ExecOrderSysID = execSysID;
            return ((reqQryExecOrder)Invoke(this.PtrHandle, "ReqQryExecOrder", typeof(reqQryExecOrder)))(ref pQryExecOrder);
        }

        /// <summary>
        /// 执行宣告录入请求
        /// </summary>
        private delegate int reqExecOrderInsert(ref CThostFtdcInputExecOrderField pField);
        public int ReqExecOrderInsert(CThostFtdcInputExecOrderField pInputExecOrder)
        {
            return ((reqExecOrderInsert)Invoke(this.PtrHandle, "ReqExecOrderInsert", typeof(reqExecOrderInsert)))(ref pInputExecOrder);
        }

        public int ReqExecOrderInsert(string instrumentID, int volume, string exchange, bool isExec, EnumThostOffsetFlagType offset)
        {
            CThostFtdcInputExecOrderField pInputExecOrder = new CThostFtdcInputExecOrderField();
            pInputExecOrder.BrokerID = this.BrokerID;
            //pInputExecOrder.BusinessUnit = null;
            pInputExecOrder.InstrumentID = instrumentID;
            pInputExecOrder.InvestorID = this.InvestorID;
            pInputExecOrder.ExecOrderRef = "" + (++this.MaxOrderRef).ToString();
            //pInputExecOrder.UserID = this.InvestorID;
            pInputExecOrder.HedgeFlag = EnumThostHedgeFlagType.Speculation;
            pInputExecOrder.Volume = volume;

            pInputExecOrder.ActionType = isExec ? EnumThostActionTypeType.Exec : EnumThostActionTypeType.Abandon;
            pInputExecOrder.PosiDirection = EnumThostPosiDirectionType.Long;//?
            if (exchange == "CFFEX")
            {
                pInputExecOrder.OffsetFlag = EnumThostOffsetFlagType.Close;
                pInputExecOrder.ReservePositionFlag = EnumThostExecOrderPositionFlagType.UnReserve;
                pInputExecOrder.CloseFlag = EnumThostExecOrderCloseFlagType.AutoClose;
            }
            else if (exchange == "DCE" || exchange == "CZCE")
            {
                pInputExecOrder.OffsetFlag = EnumThostOffsetFlagType.Close;
                pInputExecOrder.ReservePositionFlag = EnumThostExecOrderPositionFlagType.Reserve;
                pInputExecOrder.CloseFlag = EnumThostExecOrderCloseFlagType.NotToClose;
            }
            else if (exchange == "SHFE")
            {
                pInputExecOrder.OffsetFlag = offset;// EnumThostOffsetFlagType.CloseToday;
                pInputExecOrder.ReservePositionFlag = EnumThostExecOrderPositionFlagType.UnReserve;
                pInputExecOrder.CloseFlag = EnumThostExecOrderCloseFlagType.AutoClose;
            }
            return ((reqExecOrderInsert)Invoke(this.PtrHandle, "ReqExecOrderInsert", typeof(reqExecOrderInsert)))(ref pInputExecOrder);
        }

        /// <summary>
        /// 执行宣告操作请求
        /// </summary>
        private delegate int reqExecOrderAction(ref CThostFtdcInputExecOrderActionField pField);
        public int ReqExecOrderAction(string InstrumentID, int FrontID = 0, int SessionID = 0, string ExecRef = "0", string ExchangeID = null, string ExecSysID = null)
        {
            CThostFtdcInputExecOrderActionField pInputExecOrderAction = new CThostFtdcInputExecOrderActionField();
            pInputExecOrderAction.ActionFlag = EnumThostActionFlagType.Delete;
            pInputExecOrderAction.BrokerID = this.BrokerID;
            pInputExecOrderAction.InvestorID = this.InvestorID;
            //tmp.UserID = this.InvestorID;
            pInputExecOrderAction.InstrumentID = InstrumentID;
            //tmp.VolumeChange = int.Parse(lvi.SubItems["VolumeTotalOriginal"].Text);
            if (FrontID != 0)
                pInputExecOrderAction.FrontID = FrontID;
            if (SessionID != 0)
                pInputExecOrderAction.SessionID = SessionID;
            if (ExecRef != "0")
                pInputExecOrderAction.ExecOrderRef = ExecRef;
            pInputExecOrderAction.ExchangeID = ExchangeID;
            if (ExecSysID != null)
                pInputExecOrderAction.ExecOrderSysID = new string('\0', 20 - ExecSysID.Length) + ExecSysID;	//ExecSysID右对齐
            return ((reqExecOrderAction)Invoke(this.PtrHandle, "ReqExecOrderAction", typeof(reqExecOrderAction)))(ref pInputExecOrderAction);
        }

        /// <summary>
        /// 申请组合录入请求
        /// </summary>
        private delegate int reqCombActionInsert(ref CThostFtdcInputCombActionField pField);
        public int ReqCombActionInsert(string instrumentID, EnumThostDirectionType direction, int handCount, EnumThostCombDirectionType combDir)
        {
            CThostFtdcInputCombActionField pInputCombAction = new CThostFtdcInputCombActionField();
            pInputCombAction.BrokerID = this.BrokerID;
            pInputCombAction.CombActionRef = (++this.MaxOrderRef).ToString();
            pInputCombAction.CombDirection = combDir;
            pInputCombAction.Direction = direction;
            pInputCombAction.HedgeFlag = EnumThostHedgeFlagType.Speculation;//
            pInputCombAction.InstrumentID = instrumentID;
            pInputCombAction.InvestorID = this.InvestorID;
            pInputCombAction.UserID = this.InvestorID;
            pInputCombAction.Volume = handCount;
            return ((reqCombActionInsert)Invoke(this.PtrHandle, "ReqCombActionInsert", typeof(reqCombActionInsert)))(ref pInputCombAction);
        }

        /// <summary>
	    ///请求查询组合合约安全系数
        /// </summary>
        private delegate int reqQryCombInstrumentGuard(ref CThostFtdcQryCombInstrumentGuardField pField);
        public int ReqQryCombInstrumentGuard(string instrumentID)
        {
            CThostFtdcQryCombInstrumentGuardField pQryCombInstrumentGuard = new CThostFtdcQryCombInstrumentGuardField();
            pQryCombInstrumentGuard.BrokerID = this.BrokerID;
            pQryCombInstrumentGuard.InstrumentID = instrumentID;
            return ((reqQryCombInstrumentGuard)Invoke(this.PtrHandle, "ReqQryCombInstrumentGuard", typeof(reqQryCombInstrumentGuard)))(ref pQryCombInstrumentGuard);
        }

        /// <summary>
	    ///请求查询申请组合
        /// </summary>
        private delegate int reqQryCombAction(ref CThostFtdcQryCombActionField pField);
        public int ReqQryCombAction(string instrumentID, string exchangeID)
        {
            CThostFtdcQryCombActionField pQryCombAction = new CThostFtdcQryCombActionField();
            pQryCombAction.BrokerID = this.BrokerID;
            pQryCombAction.ExchangeID = exchangeID;
            pQryCombAction.InstrumentID = instrumentID;
            pQryCombAction.InvestorID = this.InvestorID;
            return ((reqQryCombAction)Invoke(this.PtrHandle, "ReqQryCombAction", typeof(reqQryCombAction)))(ref pQryCombAction);
        }

        /// 回调函数 =====================================================================================================================

        private delegate void Reg(IntPtr pPtr);

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

        ///心跳超时警告。当长时间未收到报文时，该方法被调用。
        public delegate void HeartBeatWarning(int pTimeLapes);
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


        public delegate void RspUserLogin(ref CThostFtdcRspUserLoginField pRspUserLogin, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspUserLogin _OnRspUserLogin;
        /// <summary>
        /// 登录请求响应
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

        ///登出请求响应
        public delegate void RspUserLogout(ref CThostFtdcUserLogoutField pUserLogout, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspUserLogout _OnRspUserLogout;
        /// <summary>
        /// 登出请求响应
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

        ///报单操作错误回报
        public delegate void ErrRtnOrderAction(ref CThostFtdcOrderActionField pOrderAction, ref CThostFtdcRspInfoField pRspInfo);
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

        ///报单录入错误回报
        public delegate void ErrRtnOrderInsert(ref CThostFtdcInputOrderField pInputOrder, ref CThostFtdcRspInfoField pRspInfo);
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

        ///错误应答
        public delegate void RspError(ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
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

        ///报单操作请求响应
        public delegate void RspOrderAction(ref CThostFtdcInputOrderActionField pInputOrderAction, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspOrderAction _OnRspOrderAction;
        /// <summary>
        /// 报单操作请求响应
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

        ///报单录入请求响应
        public delegate void RspOrderInsert(ref CThostFtdcInputOrderField pInputOrder, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspOrderInsert _OnRspOrderInsert;
        /// <summary>
        /// 报单录入请求响应
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

        ///预埋撤单录入请求响应
        public delegate void RspParkedOrderAction(ref CThostFtdcParkedOrderActionField pParkedOrderAction, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspParkedOrderAction _OnRspParkedOrderAction;
        /// <summary>
        /// 预埋撤单录入请求响应
        /// </summary>
        public event RspParkedOrderAction OnRspParkedOrderAction
        {
            add
            {
                _OnRspParkedOrderAction += value;
                (Invoke(this.PtrHandle, "RegRspParkedOrderAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspParkedOrderAction));
            }
            remove
            {
                _OnRspParkedOrderAction -= value;
                (Invoke(this.PtrHandle, "RegRspParkedOrderAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspParkedOrderAction));
            }
        }

        ///预埋单录入请求响应
        public delegate void RspParkedOrderInsert(ref CThostFtdcParkedOrderField pParkedOrder, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspParkedOrderInsert _OnRspParkedOrderInsert;
        /// <summary>
        /// 预埋单录入请求响应
        /// </summary>
        public event RspParkedOrderInsert OnRspParkedOrderInsert
        {
            add
            {
                _OnRspParkedOrderInsert += value;
                (Invoke(this.PtrHandle, "RegRspParkedOrderInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspParkedOrderInsert));
            }
            remove
            {
                _OnRspParkedOrderInsert -= value;
                (Invoke(this.PtrHandle, "RegRspParkedOrderInsertZ", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspParkedOrderInsert));
            }
        }

        ///请求查询经纪公司交易算法响应
        [DllImport(DLLNAME, EntryPoint = "?RegRspQryBrokerTradingAlgos@@YGXP6GHPAUCThostFtdcBrokerTradingAlgosField@@PAUCThostFtdcRspInfoField@@H_N@Z@Z", CallingConvention = CallingConvention.StdCall)]
        static extern void regRspQryBrokerTradingAlgos(RspQryBrokerTradingAlgos cb);
        RspQryBrokerTradingAlgos rspQryBrokerTradingAlgos;
        public delegate void RspQryBrokerTradingAlgos(ref CThostFtdcBrokerTradingAlgosField pBrokerTradingAlgos, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        /// <summary>
        /// 请求查询经纪公司交易算法响应
        /// </summary>
        public event RspQryBrokerTradingAlgos OnRspQryBrokerTradingAlgos
        {
            add { rspQryBrokerTradingAlgos += value; regRspQryBrokerTradingAlgos(rspQryBrokerTradingAlgos); }
            remove { rspQryBrokerTradingAlgos -= value; regRspQryBrokerTradingAlgos(rspQryBrokerTradingAlgos); }
        }

        ///请求查询经纪公司交易参数响应
        public delegate void RspQryBrokerTradingParams(ref CThostFtdcBrokerTradingParamsField pBrokerTradingParams, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryBrokerTradingParams _OnRspQryBrokerTradingParams;
        /// <summary>
        /// 请求查询经纪公司交易参数响应
        /// </summary>
        public event RspQryBrokerTradingParams OnRspQryBrokerTradingParams
        {
            add
            {
                _OnRspQryBrokerTradingParams += value;
                (Invoke(this.PtrHandle, "RegRspQryBrokerTradingParams", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryBrokerTradingParams));
            }
            remove
            {
                _OnRspQryBrokerTradingParams -= value;
                (Invoke(this.PtrHandle, "RegRspQryBrokerTradingParams", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryBrokerTradingParams));
            }
        }


        ///查询保证金监管系统经纪公司资金账户密钥响应
        [DllImport(DLLNAME, EntryPoint = "?RegRspQryCFMMCTradingAccountKey@@YGXP6GHPAUCThostFtdcCFMMCTradingAccountKeyField@@PAUCThostFtdcRspInfoField@@H_N@Z@Z", CallingConvention = CallingConvention.StdCall)]
        static extern void regRspQryCFMMCTradingAccountKey(RspQryCFMMCTradingAccountKey cb);
        RspQryCFMMCTradingAccountKey rspQryCFMMCTradingAccountKey;
        public delegate void RspQryCFMMCTradingAccountKey(ref CThostFtdcCFMMCTradingAccountKeyField pCFMMCTradingAccountKey, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        /// <summary>
        /// 查询保证金监管系统经纪公司资金账户密钥响应
        /// </summary>
        public event RspQryCFMMCTradingAccountKey OnRspQryCFMMCTradingAccountKey
        {
            add { rspQryCFMMCTradingAccountKey += value; regRspQryCFMMCTradingAccountKey(rspQryCFMMCTradingAccountKey); }
            remove { rspQryCFMMCTradingAccountKey -= value; regRspQryCFMMCTradingAccountKey(rspQryCFMMCTradingAccountKey); }
        }

        ///请求查询行情响应
        [DllImport(DLLNAME, EntryPoint = "?RegRspQryDepthMarketData@@YGXP6GHPAUCThostFtdcDepthMarketDataField@@PAUCThostFtdcRspInfoField@@H_N@Z@Z", CallingConvention = CallingConvention.StdCall)]
        static extern void regRspQryDepthMarketData(RspQryDepthMarketData cb);
        RspQryDepthMarketData rspQryDepthMarketData;
        public delegate void RspQryDepthMarketData(ref CThostFtdcDepthMarketDataField pDepthMarketData, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        /// <summary>
        /// 请求查询行情响应
        /// </summary>
        public event RspQryDepthMarketData OnRspQryDepthMarketData
        {
            add { rspQryDepthMarketData += value; regRspQryDepthMarketData(rspQryDepthMarketData); }
            remove { rspQryDepthMarketData -= value; regRspQryDepthMarketData(rspQryDepthMarketData); }
        }

        ///请求查询交易所响应
        [DllImport(DLLNAME, EntryPoint = "?RegRspQryExchange@@YGXP6GHPAUCThostFtdcExchangeField@@PAUCThostFtdcRspInfoField@@H_N@Z@Z", CallingConvention = CallingConvention.StdCall)]
        static extern void regRspQryExchange(RspQryExchange cb);
        RspQryExchange rspQryExchange;
        public delegate void RspQryExchange(ref CThostFtdcExchangeField pExchange, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        /// <summary>
        /// 请求查询交易所响应
        /// </summary>
        public event RspQryExchange OnRspQryExchange
        {
            add { rspQryExchange += value; regRspQryExchange(rspQryExchange); }
            remove { rspQryExchange -= value; regRspQryExchange(rspQryExchange); }
        }

        ///请求查询合约响应
        public delegate void RspQryInstrument(ref CThostFtdcInstrumentField pInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryInstrument _OnRspQryInstrument;
        /// <summary>
        /// 请求查询合约响应
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

        ///请求查询合约手续费率响应
        public delegate void RspQryInstrumentCommissionRate(ref CThostFtdcInstrumentCommissionRateField pInstrumentCommissionRate, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryInstrumentCommissionRate _OnRspQryInstrumentCommissionRate;
        /// <summary>
        /// 请求查询合约手续费率响应
        /// </summary>
        public event RspQryInstrumentCommissionRate OnRspQryInstrumentCommissionRate
        {
            add
            {
                _OnRspQryInstrumentCommissionRate += value;
                (Invoke(this.PtrHandle, "RegRspQryInstrumentCommissionRate", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInstrumentCommissionRate));
            }
            remove
            {
                _OnRspQryInstrumentCommissionRate -= value;
                (Invoke(this.PtrHandle, "RegRspQryInstrumentCommissionRate", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInstrumentCommissionRate));
            }
        }

        ///请求查询合约保证金率响应
        public delegate void RspQryInstrumentMarginRate(ref CThostFtdcInstrumentMarginRateField pInstrumentMarginRate, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryInstrumentMarginRate _OnRspQryInstrumentMarginRate;
        /// <summary>
        /// 请求查询合约保证金率响应
        /// </summary>
        public event RspQryInstrumentMarginRate OnRspQryInstrumentMarginRate
        {
            add
            {
                _OnRspQryInstrumentMarginRate += value;
                (Invoke(this.PtrHandle, "RegRspQryInstrumentCommissionRate", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInstrumentMarginRate));
            }
            remove
            {
                _OnRspQryInstrumentMarginRate -= value;
                (Invoke(this.PtrHandle, "RegRspQryInstrumentCommissionRate@Z", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInstrumentMarginRate));
            }
        }

        ///请求查询投资者响应
        public delegate void RspQryInvestor(ref CThostFtdcInvestorField pInvestor, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryInvestor _OnRspQryInvestor;
        /// <summary>
        /// 请求查询投资者响应
        /// </summary>
        public event RspQryInvestor OnRspQryInvestor
        {
            add
            {
                _OnRspQryInvestor += value;
                (Invoke(this.PtrHandle, "RegRspQryInvestor", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestor));
            }
            remove
            {
                _OnRspQryInvestor -= value;
                (Invoke(this.PtrHandle, "RegRspQryInvestor", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestor));
            }
        }

        ///请求查询投资者持仓响应
        public delegate void RspQryInvestorPosition(ref CThostFtdcInvestorPositionField pInvestorPosition, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryInvestorPosition _OnRspQryInvestorPosition;
        /// <summary>
        /// 请求查询投资者持仓响应
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

        ///请求查询投资者组合持仓明细响应
        public delegate void RspQryInvestorPositionCombineDetail(ref CThostFtdcInvestorPositionCombineDetailField pInvestorPositionCombineDetail, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryInvestorPositionCombineDetail _OnRspQryInvestorPositionCombineDetail;
        /// <summary>
        /// 请求查询投资者组合持仓明细响应
        /// </summary>
        public event RspQryInvestorPositionCombineDetail OnRspQryInvestorPositionCombineDetail
        {
            add
            {
                _OnRspQryInvestorPositionCombineDetail += value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorPositionCombineDetail", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorPositionCombineDetail));
            }
            remove
            {
                _OnRspQryInvestorPositionCombineDetail -= value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorPositionCombineDetail", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorPositionCombineDetail));
            }
        }

        ///请求查询投资者持仓明细响应
        public delegate void RspQryInvestorPositionDetail(ref CThostFtdcInvestorPositionDetailField pInvestorPositionDetail, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryInvestorPositionDetail _OnRspQryInvestorPositionDetail;
        /// <summary>
        /// 请求查询投资者持仓明细响应
        /// </summary>
        public event RspQryInvestorPositionDetail OnRspQryInvestorPositionDetail
        {
            add
            {
                _OnRspQryInvestorPositionDetail += value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorPositionDetail", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorPositionDetail));
            }
            remove
            {
                _OnRspQryInvestorPositionDetail -= value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorPositionDetail", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorPositionDetail));
            }
        }

        ///请求查询客户通知响应
        [DllImport(DLLNAME, EntryPoint = "?RegRspQryNotice@@YGXP6GHPAUCThostFtdcNoticeField@@PAUCThostFtdcRspInfoField@@H_N@Z@Z", CallingConvention = CallingConvention.StdCall)]
        static extern void regRspQryNotice(RspQryNotice cb);
        RspQryNotice rspQryNotice;
        public delegate void RspQryNotice(ref CThostFtdcNoticeField pNotice, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        /// <summary>
        /// 请求查询客户通知响应
        /// </summary>
        public event RspQryNotice OnRspQryNotice
        {
            add { rspQryNotice += value; regRspQryNotice(rspQryNotice); }
            remove { rspQryNotice -= value; regRspQryNotice(rspQryNotice); }
        }

        ///请求查询报单响应
        public delegate void RspQryOrder(ref CThostFtdcOrderField pOrder, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryOrder _OnRspQryOrder;
        /// <summary>
        /// 请求查询报单响应
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

        ///请求查询成交响应
        public delegate void RspQryTrade(ref CThostFtdcTradeField pTrade, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryTrade _OnRspQryTrade;
        /// <summary>
        /// 请求查询成交响应
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

        ///请求查询资金账户响应
        public delegate void RspQryTradingAccount(ref CThostFtdcTradingAccountField pTradingAccount, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryTradingAccount _OnRspQryTradingAccount;
        /// <summary>
        /// 请求查询资金账户响应
        /// </summary>
        public event RspQryTradingAccount OnRspQryTradingAccount
        {
            add
            {
                _OnRspQryTradingAccount += value;
                (Invoke(this.PtrHandle, "RegRspQryTradingAccount", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryTradingAccount));
            }
            remove
            {
                _OnRspQryTradingAccount -= value;
                (Invoke(this.PtrHandle, "RegRspQryTradingAccount", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryTradingAccount));
            }
        }

        ///请求查询预埋单响应
        public delegate void RspQryParkedOrder(ref CThostFtdcParkedOrderField pParkedOrder, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryParkedOrder _OnRspQryParkedOrder;
        /// <summary>
        /// 请求查询预埋单响应
        /// </summary>
        public event RspQryParkedOrder OnRspQryParkedOrder
        {
            add
            {
                _OnRspQryParkedOrder += value;
                (Invoke(this.PtrHandle, "RegRspQryParkedOrder", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryParkedOrder));
            }
            remove
            {
                _OnRspQryParkedOrder -= value;
                (Invoke(this.PtrHandle, "RegRspQryParkedOrder", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryParkedOrder));
            }
        }

        ///请求查询预埋撤单响应
        public delegate void RspQryParkedOrderAction(ref CThostFtdcParkedOrderActionField pParkedOrderAction, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryParkedOrderAction _OnRspQryParkedOrderAction;
        /// <summary>
        /// 请求查询预埋撤单响应
        /// </summary>
        public event RspQryParkedOrderAction OnRspQryParkedOrderAction
        {
            add
            {
                _OnRspQryParkedOrderAction += value;
                (Invoke(this.PtrHandle, "RegRspQryParkedOrderAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryParkedOrderAction));
            }
            remove
            {
                _OnRspQryParkedOrderAction -= value;
                (Invoke(this.PtrHandle, "RegRspQryParkedOrderAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryParkedOrderAction));
            }
        }

        ///请求查询投资者结算结果响应
        public delegate void RspQrySettlementInfo(ref CThostFtdcSettlementInfoField pSettlementInfo, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQrySettlementInfo _OnRspQrySettlementInfo;
        /// <summary>
        /// 请求查询投资者结算结果响应
        /// </summary>
        public event RspQrySettlementInfo OnRspQrySettlementInfo
        {
            add
            {
                _OnRspQrySettlementInfo += value;
                (Invoke(this.PtrHandle, "RegRspQrySettlementInfo", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQrySettlementInfo));
            }
            remove
            {
                _OnRspQrySettlementInfo -= value;
                (Invoke(this.PtrHandle, "RegRspQrySettlementInfo", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQrySettlementInfo));
            }
        }


        ///请求查询结算信息确认响应
        public delegate void RspQrySettlementInfoConfirm(ref CThostFtdcSettlementInfoConfirmField pSettlementInfoConfirm, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQrySettlementInfoConfirm _OnRspQrySettlementInfoConfirm;
        /// <summary>
        /// 请求查询结算信息确认响应
        /// </summary>
        public event RspQrySettlementInfoConfirm OnRspQrySettlementInfoConfirm
        {
            add
            {
                _OnRspQrySettlementInfoConfirm += value;
                (Invoke(this.PtrHandle, "RegRspQrySettlementInfoConfirm", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQrySettlementInfoConfirm));
            }
            remove
            {
                _OnRspQrySettlementInfoConfirm -= value;
                (Invoke(this.PtrHandle, "RegRspQrySettlementInfoConfirm", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQrySettlementInfoConfirm));
            }
        }

        ///投资者结算结果确认响应
        public delegate void RspSettlementInfoConfirm(ref CThostFtdcSettlementInfoConfirmField pSettlementInfoConfirm, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspSettlementInfoConfirm _OnRspSettlementInfoConfirm;
        /// <summary>
        /// 投资者结算结果确认响应
        /// </summary>
        public event RspSettlementInfoConfirm OnRspSettlementInfoConfirm
        {
            add
            {
                _OnRspSettlementInfoConfirm += value;
                (Invoke(this.PtrHandle, "RegRspSettlementInfoConfirm", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspSettlementInfoConfirm));
            }
            remove
            {
                _OnRspSettlementInfoConfirm -= value;
                (Invoke(this.PtrHandle, "RegRspSettlementInfoConfirm", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspSettlementInfoConfirm));
            }
        }

        ///请求查询交易编码响应
        public delegate void RspQryTradingCode(ref CThostFtdcTradingCodeField pTradingCode, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryTradingCode _OnRspQryTradingCode;
        /// <summary>
        /// 请求查询交易编码响应
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

        ///请求查询交易通知响应
        public delegate void RspQryTradingNotice(ref CThostFtdcTradingNoticeField pTradingNotice, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryTradingNotice _OnRspQryTradingNotice;
        /// <summary>
        /// 请求查询交易通知响应
        /// </summary>
        public event RspQryTradingNotice OnRspQryTradingNotice
        {
            add
            {
                _OnRspQryTradingNotice += value;
                (Invoke(this.PtrHandle, "RegRspQryTradingNotice", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryTradingNotice));
            }
            remove
            {
                _OnRspQryTradingNotice -= value;
                (Invoke(this.PtrHandle, "RegRspQryTradingNotice", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryTradingNotice));
            }
        }

        ///查询最大报单数量响应
        public delegate void RspQueryMaxOrderVolume(ref CThostFtdcQueryMaxOrderVolumeField pQueryMaxOrderVolume, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQueryMaxOrderVolume _OnRspQueryMaxOrderVolume;
        /// <summary>
        /// 查询最大报单数量响应
        /// </summary>
        public event RspQueryMaxOrderVolume OnRspQueryMaxOrderVolume
        {
            add
            {
                _OnRspQueryMaxOrderVolume += value;
                (Invoke(this.PtrHandle, "RegRspQueryMaxOrderVolume", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQueryMaxOrderVolume));
            }
            remove
            {
                _OnRspQueryMaxOrderVolume -= value;
                (Invoke(this.PtrHandle, "RegRspQueryMaxOrderVolume", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQueryMaxOrderVolume));
            }
        }

        ///删除预埋单响应
        public delegate void RspRemoveParkedOrder(ref CThostFtdcRemoveParkedOrderField pRemoveParkedOrder, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspRemoveParkedOrder _OnRspRemoveParkedOrder;
        /// <summary>
        /// 删除预埋单响应
        /// </summary>
        public event RspRemoveParkedOrder OnRspRemoveParkedOrder
        {
            add
            {
                _OnRspRemoveParkedOrder += value;
                (Invoke(this.PtrHandle, "RegRspRemoveParkedOrder", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspRemoveParkedOrder));
            }
            remove
            {
                _OnRspRemoveParkedOrder -= value;
                (Invoke(this.PtrHandle, "RegRspRemoveParkedOrder", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspRemoveParkedOrder));
            }
        }

        ///删除预埋撤单响应
        public delegate void RspRemoveParkedOrderAction(ref CThostFtdcRemoveParkedOrderActionField pRemoveParkedOrderAction, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspRemoveParkedOrderAction _OnRspRemoveParkedOrderAction;
        /// <summary>
        /// 删除预埋撤单响应
        /// </summary>
        public event RspRemoveParkedOrderAction OnRspRemoveParkedOrderAction
        {
            add
            {
                _OnRspRemoveParkedOrderAction += value;
                (Invoke(this.PtrHandle, "RegRspRemoveParkedOrderAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspRemoveParkedOrderAction));
            }
            remove
            {
                _OnRspRemoveParkedOrderAction -= value;
                (Invoke(this.PtrHandle, "?RegRspRemoveParkedOrderAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspRemoveParkedOrderAction));
            }
        }

        ///资金账户口令更新请求响应
        public delegate void RspTradingAccountPasswordUpdate(ref CThostFtdcTradingAccountPasswordUpdateField pTradingAccountPasswordUpdate, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspTradingAccountPasswordUpdate _OnRspTradingAccountPasswordUpdate;
        /// <summary>
        /// 资金账户口令更新请求响应
        /// </summary>
        public event RspTradingAccountPasswordUpdate OnRspTradingAccountPasswordUpdate
        {
            add
            {
                _OnRspTradingAccountPasswordUpdate += value;
                (Invoke(this.PtrHandle, "RegRspTradingAccountPasswordUpdate", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspTradingAccountPasswordUpdate));
            }
            remove
            {
                _OnRspTradingAccountPasswordUpdate -= value;
                (Invoke(this.PtrHandle, "RegRspTradingAccountPasswordUpdate", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspTradingAccountPasswordUpdate));
            }
        }

        ///用户口令更新请求响应
        public delegate void RspUserPasswordUpdate(ref CThostFtdcUserPasswordUpdateField pUserPasswordUpdate, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspUserPasswordUpdate _OnRspUserPasswordUpdate;
        /// <summary>
        /// 用户口令更新请求响应
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

        ///提示条件单校验错误
        [DllImport(DLLNAME, EntryPoint = "?RegRtnErrorConditionalOrder@@YGXP6GHPAUCThostFtdcErrorConditionalOrderField@@@Z@Z", CallingConvention = CallingConvention.StdCall)]
        static extern void regRtnErrorConditionalOrder(RtnErrorConditionalOrder cb);
        RtnErrorConditionalOrder rtnErrorConditionalOrder;
        public delegate void RtnErrorConditionalOrder(ref CThostFtdcErrorConditionalOrderField pErrorConditionalOrder);
        /// <summary>
        /// 提示条件单校验错误
        /// </summary>
        public event RtnErrorConditionalOrder OnRtnErrorConditionalOrder
        {
            add { rtnErrorConditionalOrder += value; regRtnErrorConditionalOrder(rtnErrorConditionalOrder); }
            remove { rtnErrorConditionalOrder -= value; regRtnErrorConditionalOrder(rtnErrorConditionalOrder); }
        }

        ///合约交易状态通知
        public delegate void RtnInstrumentStatus(ref CThostFtdcInstrumentStatusField pInstrumentStatus);
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

        ///报单通知
        public delegate void RtnOrder(ref CThostFtdcOrderField pOrder);
        private RtnOrder _OnRtnOrder;
        /// <summary>
        /// 报单通知
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
                (Invoke(this.PtrHandle, "?RegRtnOrder", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnOrder));
            }
        }

        ///成交通知
        public delegate void RtnTrade(ref CThostFtdcTradeField pTrade);
        private RtnTrade _OnRtnTrade;
        /// <summary>
        /// 成交通知
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

        ///交易通知
        public delegate void RtnTradingNotice(ref CThostFtdcTradingNoticeInfoField pTradingNoticeInfo);
        private RtnTradingNotice _OnRtnTradingNotice;
        /// <summary>
        /// 交易通知
        /// </summary>
        public event RtnTradingNotice OnRtnTradingNotice
        {
            add
            {
                _OnRtnTradingNotice += value;
                (Invoke(this.PtrHandle, "RegRtnTradingNotice", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnTradingNotice));
            }
            remove
            {
                _OnRtnTradingNotice -= value;
                (Invoke(this.PtrHandle, "RegRtnTradingNotice", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnTradingNotice));
            }
        }

        ///请求查询银期签约关系响应
        public delegate void RspQryAccountregister(ref CThostFtdcAccountregisterField pAccountregister, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryAccountregister _OnRspQryAccountregister;
        /// <summary>
        /// 请求查询银期签约关系响应
        /// </summary>
        public event RspQryAccountregister OnRspQryAccountregister
        {
            add
            {
                _OnRspQryAccountregister += value;
                (Invoke(this.PtrHandle, "RegRspQryAccountregister", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryAccountregister));
            }
            remove
            {
                _OnRspQryAccountregister -= value;
                (Invoke(this.PtrHandle, "RegRspQryAccountregister", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryAccountregister));
            }
        }

        ///请求查询签约银行响应
        public delegate void RspQryContractBank(ref CThostFtdcContractBankField pContractBank, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryContractBank _OnRspQryContractBank;
        /// <summary>
        /// 请求查询签约银行响应
        /// </summary>
        public event RspQryContractBank OnRspQryContractBank
        {
            add
            {
                _OnRspQryContractBank += value;
                (Invoke(this.PtrHandle, "RegRspQryContractBank", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryContractBank));
            }
            remove
            {
                _OnRspQryContractBank -= value;
                (Invoke(this.PtrHandle, "RegRspQryContractBank", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryContractBank));
            }
        }

        ///请求查询转帐银行响应
        public delegate void RspQryTransferBank(ref CThostFtdcTransferBankField pTransferBank, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryTransferBank _OnRspQryTransferBank;
        /// <summary>
        /// 请求查询转帐银行响应
        /// </summary>
        public event RspQryTransferBank OnRspQryTransferBank
        {
            add
            {
                _OnRspQryTransferBank += value;
                (Invoke(this.PtrHandle, "RegRspQryTransferBank", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryTransferBank));
            }
            remove
            {
                _OnRspQryTransferBank -= value;
                (Invoke(this.PtrHandle, "RegRspQryTransferBank", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryTransferBank));
            }
        }

        ///请求查询转帐流水响应
        public delegate void RspQryTransferSerial(ref CThostFtdcTransferSerialField pTransferSerial, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryTransferSerial _OnRspQryTransferSerial;
        /// <summary>
        /// 请求查询转帐流水响应
        /// </summary>
        public event RspQryTransferSerial OnRspQryTransferSerial
        {
            add
            {
                _OnRspQryTransferSerial += value;
                (Invoke(this.PtrHandle, "RegRspQryTransferSerial", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryTransferSerial));
            }
            remove
            {
                _OnRspQryTransferSerial -= value;
                (Invoke(this.PtrHandle, "RegRspQryTransferSerial", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryTransferSerial));
            }
        }

        ///期货发起查询银行余额应答
        public delegate void RspQueryBankAccountMoneyByFuture(ref CThostFtdcReqQueryAccountField pReqQueryAccount, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQueryBankAccountMoneyByFuture _OnRspQueryBankAccountMoneyByFuture;
        /// <summary>
        /// 期货发起查询银行余额应答
        /// </summary>
        public event RspQueryBankAccountMoneyByFuture OnRspQueryBankAccountMoneyByFuture
        {
            add
            {
                _OnRspQueryBankAccountMoneyByFuture += value;
                (Invoke(this.PtrHandle, "RegRspQueryBankAccountMoneyByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQueryBankAccountMoneyByFuture));
            }
            remove
            {
                _OnRspQueryBankAccountMoneyByFuture -= value;
                (Invoke(this.PtrHandle, "RegRspQueryBankAccountMoneyByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQueryBankAccountMoneyByFuture));
            }
        }

        ///期货发起银行资金转期货错误回报
        public delegate void ErrRtnBankToFutureByFuture(ref CThostFtdcReqTransferField pReqTransfer, ref CThostFtdcRspInfoField pRspInfo);
        private ErrRtnBankToFutureByFuture _OnErrRtnBankToFutureByFuture;
        /// <summary>
        /// 期货发起银行资金转期货错误回报
        /// </summary>
        public event ErrRtnBankToFutureByFuture OnErrRtnBankToFutureByFuture
        {
            add
            {
                _OnErrRtnBankToFutureByFuture += value;
                (Invoke(this.PtrHandle, "RegErrRtnBankToFutureByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnBankToFutureByFuture));
            }
            remove
            {
                _OnErrRtnBankToFutureByFuture -= value;
                (Invoke(this.PtrHandle, "RegErrRtnBankToFutureByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnBankToFutureByFuture));
            }
        }

        ///期货发起期货资金转银行错误回报
        public delegate void ErrRtnFutureToBankByFuture(ref CThostFtdcReqTransferField pReqTransfer, ref CThostFtdcRspInfoField pRspInfo);
        private ErrRtnFutureToBankByFuture _OnErrRtnFutureToBankByFuture;
        /// <summary>
        /// 期货发起期货资金转银行错误回报
        /// </summary>
        public event ErrRtnFutureToBankByFuture OnErrRtnFutureToBankByFuture
        {
            add
            {
                _OnErrRtnFutureToBankByFuture += value;
                (Invoke(this.PtrHandle, "RegErrRtnFutureToBankByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnFutureToBankByFuture));
            }
            remove
            {
                _OnErrRtnFutureToBankByFuture -= value;
                (Invoke(this.PtrHandle, "RegErrRtnFutureToBankByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnFutureToBankByFuture));
            }
        }

        ///期货发起查询银行余额错误回报
        public delegate void ErrRtnQueryBankBalanceByFuture(ref CThostFtdcReqQueryAccountField pReqQueryAccount, ref CThostFtdcRspInfoField pRspInfo);
        private ErrRtnQueryBankBalanceByFuture _OnErrRtnQueryBankBalanceByFuture;
        /// <summary>
        /// 期货发起查询银行余额错误回报
        /// </summary>
        public event ErrRtnQueryBankBalanceByFuture OnErrRtnQueryBankBalanceByFuture
        {
            add
            {
                _OnErrRtnQueryBankBalanceByFuture += value;
                (Invoke(this.PtrHandle, "RegErrRtnQueryBankBalanceByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnQueryBankBalanceByFuture));
            }
            remove
            {
                _OnErrRtnQueryBankBalanceByFuture -= value;
                (Invoke(this.PtrHandle, "RegErrRtnQueryBankBalanceByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnQueryBankBalanceByFuture));
            }
        }

        ///系统运行时期货端手工发起冲正银行转期货错误回报
        [DllImport(DLLNAME, EntryPoint = "?RegErrRtnRepealBankToFutureByFutureManual@@YGXP6GHPAUCThostFtdcReqRepealField@@PAUCThostFtdcRspInfoField@@@Z@Z", CallingConvention = CallingConvention.StdCall)]
        static extern void regErrRtnRepealBankToFutureByFutureManual(ErrRtnRepealBankToFutureByFutureManual cb);
        ErrRtnRepealBankToFutureByFutureManual errRtnRepealBankToFutureByFutureManual;
        public delegate void ErrRtnRepealBankToFutureByFutureManual(ref CThostFtdcReqRepealField pReqRepeal, ref CThostFtdcRspInfoField pRspInfo);
        /// <summary>
        /// 系统运行时期货端手工发起冲正银行转期货错误回报
        /// </summary>
        public event ErrRtnRepealBankToFutureByFutureManual OnErrRtnRepealBankToFutureByFutureManual
        {
            add { errRtnRepealBankToFutureByFutureManual += value; regErrRtnRepealBankToFutureByFutureManual(errRtnRepealBankToFutureByFutureManual); }
            remove { errRtnRepealBankToFutureByFutureManual -= value; regErrRtnRepealBankToFutureByFutureManual(errRtnRepealBankToFutureByFutureManual); }
        }

        ///系统运行时期货端手工发起冲正期货转银行错误回报
        [DllImport(DLLNAME, EntryPoint = "?RegErrRtnRepealFutureToBankByFutureManual@@YGXP6GHPAUCThostFtdcReqRepealField@@PAUCThostFtdcRspInfoField@@@Z@Z", CallingConvention = CallingConvention.StdCall)]
        static extern void regErrRtnRepealFutureToBankByFutureManual(ErrRtnRepealFutureToBankByFutureManual cb);
        ErrRtnRepealFutureToBankByFutureManual errRtnRepealFutureToBankByFutureManual;
        public delegate void ErrRtnRepealFutureToBankByFutureManual(ref CThostFtdcReqRepealField pReqRepeal, ref CThostFtdcRspInfoField pRspInfo);
        /// <summary>
        /// 系统运行时期货端手工发起冲正期货转银行错误回报
        /// </summary>
        public event ErrRtnRepealFutureToBankByFutureManual OnErrRtnRepealFutureToBankByFutureManual
        {
            add { errRtnRepealFutureToBankByFutureManual += value; regErrRtnRepealFutureToBankByFutureManual(errRtnRepealFutureToBankByFutureManual); }
            remove { errRtnRepealFutureToBankByFutureManual -= value; regErrRtnRepealFutureToBankByFutureManual(errRtnRepealFutureToBankByFutureManual); }
        }

        ///期货发起银行资金转期货应答
        public delegate void RspFromBankToFutureByFuture(ref CThostFtdcReqTransferField pReqTransfer, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspFromBankToFutureByFuture _OnRspFromBankToFutureByFuture;
        /// <summary>
        /// 期货发起银行资金转期货应答
        /// </summary>
        public event RspFromBankToFutureByFuture OnRspFromBankToFutureByFuture
        {
            add
            {
                _OnRspFromBankToFutureByFuture += value;
                (Invoke(this.PtrHandle, "RegRspFromBankToFutureByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspFromBankToFutureByFuture));
            }
            remove
            {
                _OnRspFromBankToFutureByFuture -= value;
                (Invoke(this.PtrHandle, "?RegRspFromBankToFutureByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspFromBankToFutureByFuture));
            }
        }

        ///期货发起期货资金转银行应答
        public delegate void RspFromFutureToBankByFuture(ref CThostFtdcReqTransferField pReqTransfer, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspFromFutureToBankByFuture _OnRspFromFutureToBankByFuture;
        /// <summary>
        /// 期货发起期货资金转银行应答
        /// </summary>
        public event RspFromFutureToBankByFuture OnRspFromFutureToBankByFuture
        {
            add
            {
                _OnRspFromFutureToBankByFuture += value;
                (Invoke(this.PtrHandle, "RegRspFromFutureToBankByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspFromFutureToBankByFuture));
            }
            remove
            {
                _OnRspFromFutureToBankByFuture -= value;
                (Invoke(this.PtrHandle, "RegRspFromFutureToBankByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspFromFutureToBankByFuture));
            }
        }

        ///银行发起银行资金转期货通知
        public delegate void RtnFromBankToFutureByBank(ref CThostFtdcRspTransferField pRspTransfer);
        private RtnFromBankToFutureByBank _OnRtnFromBankToFutureByBank;
        /// <summary>
        /// 银行发起银行资金转期货通知
        /// </summary>
        public event RtnFromBankToFutureByBank OnRtnFromBankToFutureByBank
        {
            add
            {
                _OnRtnFromBankToFutureByBank += value;
                (Invoke(this.PtrHandle, "RegRtnFromBankToFutureByBank", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnFromBankToFutureByBank));
            }
            remove
            {
                _OnRtnFromBankToFutureByBank -= value;
                (Invoke(this.PtrHandle, "RegRtnFromBankToFutureByBank", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnFromBankToFutureByBank));
            }
        }

        ///期货发起银行资金转期货通知
        public delegate void RtnFromBankToFutureByFuture(ref CThostFtdcRspTransferField pRspTransfer);
        private RtnFromBankToFutureByFuture _OnRtnFromBankToFutureByFuture;
        /// <summary>
        /// 期货发起银行资金转期货通知
        /// </summary>
        public event RtnFromBankToFutureByFuture OnRtnFromBankToFutureByFuture
        {
            add
            {
                _OnRtnFromBankToFutureByFuture += value;
                (Invoke(this.PtrHandle, "RegRtnFromBankToFutureByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnFromBankToFutureByFuture));
            }
            remove
            {
                _OnRtnFromBankToFutureByFuture -= value;
                (Invoke(this.PtrHandle, "RegRtnFromBankToFutureByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnFromBankToFutureByFuture));
            }
        }

        ///银行发起期货资金转银行通知
        public delegate void RtnFromFutureToBankByBank(ref CThostFtdcRspTransferField pRspTransfer);
        private RtnFromFutureToBankByBank _OnRtnFromFutureToBankByBank;
        /// <summary>
        /// 银行发起期货资金转银行通知
        /// </summary>
        public event RtnFromFutureToBankByBank OnRtnFromFutureToBankByBank
        {
            add
            {
                _OnRtnFromFutureToBankByBank += value;
                (Invoke(this.PtrHandle, "RegRtnFromFutureToBankByBank", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnFromFutureToBankByBank));
            }
            remove
            {
                _OnRtnFromFutureToBankByBank -= value;
                (Invoke(this.PtrHandle, "RegRtnFromFutureToBankByBank", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnFromFutureToBankByBank));
            }
        }

        ///期货发起期货资金转银行通知
        public delegate void RtnFromFutureToBankByFuture(ref CThostFtdcRspTransferField pRspTransfer);
        private RtnFromFutureToBankByFuture _OnRtnFromFutureToBankByFuture;
        /// <summary>
        /// 期货发起期货资金转银行通知
        /// </summary>
        public event RtnFromFutureToBankByFuture OnRtnFromFutureToBankByFuture
        {
            add
            {
                _OnRtnFromFutureToBankByFuture += value;
                (Invoke(this.PtrHandle, "RegRtnFromFutureToBankByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnFromFutureToBankByFuture));
            }
            remove
            {
                _OnRtnFromFutureToBankByFuture -= value;
                (Invoke(this.PtrHandle, "RegRtnFromFutureToBankByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnFromFutureToBankByFuture));
            }
        }

        ///期货发起查询银行余额通知
        public delegate void RtnQueryBankBalanceByFuture(ref CThostFtdcNotifyQueryAccountField pNotifyQueryAccount);
        private RtnQueryBankBalanceByFuture _OnRtnQueryBankBalanceByFuture;
        /// <summary>
        /// 期货发起查询银行余额通知
        /// </summary>
        public event RtnQueryBankBalanceByFuture OnRtnQueryBankBalanceByFuture
        {
            add
            {
                _OnRtnQueryBankBalanceByFuture += value;
                (Invoke(this.PtrHandle, "RegRtnQueryBankBalanceByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnQueryBankBalanceByFuture));
            }
            remove
            {
                _OnRtnQueryBankBalanceByFuture -= value;
                (Invoke(this.PtrHandle, "RegRtnQueryBankBalanceByFuture", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnQueryBankBalanceByFuture));
            }
        }

        ///银行发起冲正银行转期货通知
        [DllImport(DLLNAME, EntryPoint = "?RegRtnRepealFromBankToFutureByBank@@YGXP6GHPAUCThostFtdcRspRepealField@@@Z@Z", CallingConvention = CallingConvention.StdCall)]
        static extern void regRtnRepealFromBankToFutureByBank(RtnRepealFromBankToFutureByBank cb);
        RtnRepealFromBankToFutureByBank rtnRepealFromBankToFutureByBank;
        public delegate void RtnRepealFromBankToFutureByBank(ref CThostFtdcRspRepealField pRspRepeal);
        /// <summary>
        /// 银行发起冲正银行转期货通知
        /// </summary>
        public event RtnRepealFromBankToFutureByBank OnRtnRepealFromBankToFutureByBank
        {
            add { rtnRepealFromBankToFutureByBank += value; regRtnRepealFromBankToFutureByBank(rtnRepealFromBankToFutureByBank); }
            remove { rtnRepealFromBankToFutureByBank -= value; regRtnRepealFromBankToFutureByBank(rtnRepealFromBankToFutureByBank); }
        }

        ///期货发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
        [DllImport(DLLNAME, EntryPoint = "?RegRtnRepealFromBankToFutureByFuture@@YGXP6GHPAUCThostFtdcRspRepealField@@@Z@Z", CallingConvention = CallingConvention.StdCall)]
        static extern void regRtnRepealFromBankToFutureByFuture(RtnRepealFromBankToFutureByFuture cb);
        RtnRepealFromBankToFutureByFuture rtnRepealFromBankToFutureByFuture;
        public delegate void RtnRepealFromBankToFutureByFuture(ref CThostFtdcRspRepealField pRspRepeal);
        /// <summary>
        /// 期货发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
        /// </summary>
        public event RtnRepealFromBankToFutureByFuture OnRtnRepealFromBankToFutureByFuture
        {
            add { rtnRepealFromBankToFutureByFuture += value; regRtnRepealFromBankToFutureByFuture(rtnRepealFromBankToFutureByFuture); }
            remove { rtnRepealFromBankToFutureByFuture -= value; regRtnRepealFromBankToFutureByFuture(rtnRepealFromBankToFutureByFuture); }
        }

        ///系统运行时期货端手工发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
        [DllImport(DLLNAME, EntryPoint = "?RegRtnRepealFromBankToFutureByFutureManual@@YGXP6GHPAUCThostFtdcRspRepealField@@@Z@Z", CallingConvention = CallingConvention.StdCall)]
        static extern void regRtnRepealFromBankToFutureByFutureManual(RtnRepealFromBankToFutureByFutureManual cb);
        RtnRepealFromBankToFutureByFutureManual rtnRepealFromBankToFutureByFutureManual;
        public delegate void RtnRepealFromBankToFutureByFutureManual(ref CThostFtdcRspRepealField pRspRepeal);
        /// <summary>
        /// 系统运行时期货端手工发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
        /// </summary>
        public event RtnRepealFromBankToFutureByFutureManual OnRtnRepealFromBankToFutureByFutureManual
        {
            add { rtnRepealFromBankToFutureByFutureManual += value; regRtnRepealFromBankToFutureByFutureManual(rtnRepealFromBankToFutureByFutureManual); }
            remove { rtnRepealFromBankToFutureByFutureManual -= value; regRtnRepealFromBankToFutureByFutureManual(rtnRepealFromBankToFutureByFutureManual); }
        }

        ///银行发起冲正期货转银行通知
        [DllImport(DLLNAME, EntryPoint = "?RegRtnRepealFromFutureToBankByBank@@YGXP6GHPAUCThostFtdcRspRepealField@@@Z@Z", CallingConvention = CallingConvention.StdCall)]
        static extern void regRtnRepealFromFutureToBankByBank(RtnRepealFromFutureToBankByBank cb);
        RtnRepealFromFutureToBankByBank rtnRepealFromFutureToBankByBank;
        public delegate void RtnRepealFromFutureToBankByBank(ref CThostFtdcRspRepealField pRspRepeal);
        /// <summary>
        /// 银行发起冲正期货转银行通知
        /// </summary>
        public event RtnRepealFromFutureToBankByBank OnRtnRepealFromFutureToBankByBank
        {
            add { rtnRepealFromFutureToBankByBank += value; regRtnRepealFromFutureToBankByBank(rtnRepealFromFutureToBankByBank); }
            remove { rtnRepealFromFutureToBankByBank -= value; regRtnRepealFromFutureToBankByBank(rtnRepealFromFutureToBankByBank); }
        }

        ///期货发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
        [DllImport(DLLNAME, EntryPoint = "?RegRtnRepealFromFutureToBankByFuture@@YGXP6GHPAUCThostFtdcRspRepealField@@@Z@Z", CallingConvention = CallingConvention.StdCall)]
        static extern void regRtnRepealFromFutureToBankByFuture(RtnRepealFromFutureToBankByFuture cb);
        RtnRepealFromFutureToBankByFuture rtnRepealFromFutureToBankByFuture;
        public delegate void RtnRepealFromFutureToBankByFuture(ref CThostFtdcRspRepealField pRspRepeal);
        /// <summary>
        /// 期货发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
        /// </summary>
        public event RtnRepealFromFutureToBankByFuture OnRtnRepealFromFutureToBankByFuture
        {
            add { rtnRepealFromFutureToBankByFuture += value; regRtnRepealFromFutureToBankByFuture(rtnRepealFromFutureToBankByFuture); }
            remove { rtnRepealFromFutureToBankByFuture -= value; regRtnRepealFromFutureToBankByFuture(rtnRepealFromFutureToBankByFuture); }
        }

        ///系统运行时期货端手工发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
        [DllImport(DLLNAME, EntryPoint = "?RegRtnRepealFromFutureToBankByFutureManual@@YGXP6GHPAUCThostFtdcRspRepealField@@@Z@Z", CallingConvention = CallingConvention.StdCall)]
        static extern void regRtnRepealFromFutureToBankByFutureManual(RtnRepealFromFutureToBankByFutureManual cb);
        RtnRepealFromFutureToBankByFutureManual rtnRepealFromFutureToBankByFutureManual;
        public delegate void RtnRepealFromFutureToBankByFutureManual(ref CThostFtdcRspRepealField pRspRepeal);
        /// <summary>
        /// 系统运行时期货端手工发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
        /// </summary>
        public event RtnRepealFromFutureToBankByFutureManual OnRtnRepealFromFutureToBankByFutureManual
        {
            add { rtnRepealFromFutureToBankByFutureManual += value; regRtnRepealFromFutureToBankByFutureManual(rtnRepealFromFutureToBankByFutureManual); }
            remove { rtnRepealFromFutureToBankByFutureManual -= value; regRtnRepealFromFutureToBankByFutureManual(rtnRepealFromFutureToBankByFutureManual); }
        }

        ///请求查询期权合约手续费率响应
        public delegate void RspQryOptionInstrCommRate(ref CThostFtdcOptionInstrCommRateField pOptionInstrCommRate, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryOptionInstrCommRate _OnRspQryOptionInstrCommRate;
        /// <summary>
        /// 请求查询期权合约手续费率响应
        /// </summary>
        public event RspQryOptionInstrCommRate OnRspQryOptionInstrCommRate
        {
            add
            {
                _OnRspQryOptionInstrCommRate += value;
                (Invoke(this.PtrHandle, "RegRspQryOptionInstrCommRate", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInstrumentCommissionRate));
            }
            remove
            {
                _OnRspQryOptionInstrCommRate -= value;
                (Invoke(this.PtrHandle, "RegRspQryOptionInstrCommRate", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInstrumentCommissionRate));
            }
        }

        ///请求查询期权交易成本响应
        public delegate void RspQryOptionInstrTradeCost(ref CThostFtdcOptionInstrTradeCostField pOptionInstrTradeCost, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryOptionInstrTradeCost _OnRspQryOptionInstrTradeCost;
        /// <summary>
        /// 请求查询期权交易成本响应
        /// </summary>
        public event RspQryOptionInstrTradeCost OnRspQryOptionInstrTradeCost
        {
            add
            {
                _OnRspQryOptionInstrTradeCost += value;
                (Invoke(this.PtrHandle, "RegRspQryOptionInstrTradeCost", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryOptionInstrTradeCost));
            }
            remove
            {
                _OnRspQryOptionInstrTradeCost -= value;
                (Invoke(this.PtrHandle, "RegRspQryOptionInstrTradeCost", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryOptionInstrTradeCost));
            }
        }

        ///请求查询投资者品种/跨品种保证金响应
        public delegate void RspQryInvestorProductGroupMargin(ref CThostFtdcInvestorProductGroupMarginField pInvestorProductGroupMargin, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryInvestorProductGroupMargin _OnRspQryInvestorProductGroupMargin;
        /// <summary>
        /// 请求查询投资者品种/跨品种保证金响应
        /// </summary>
        public event RspQryInvestorProductGroupMargin OnRspQryInvestorProductGroupMargin
        {
            add
            {
                _OnRspQryInvestorProductGroupMargin += value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorProductGroupMargin", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorProductGroupMargin));
            }
            remove
            {
                _OnRspQryInvestorProductGroupMargin -= value;
                (Invoke(this.PtrHandle, "RegRspQryInvestorProductGroupMargin", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryInvestorProductGroupMargin));
            }
        }

        ///请求查询交易所调整保证金率响应
        public delegate void RspQryExchangeMarginRateAdjust(ref CThostFtdcExchangeMarginRateAdjustField pExchangeMarginRateAdjust, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryExchangeMarginRateAdjust _OnRspQryExchangeMarginRateAdjust;
        /// <summary>
        /// 请求查询交易所调整保证金率响应
        /// </summary>
        public event RspQryExchangeMarginRateAdjust OnRspQryExchangeMarginRateAdjust
        {
            add
            {
                _OnRspQryExchangeMarginRateAdjust += value;
                (Invoke(this.PtrHandle, "RegRspQryExchangeMarginRateAdjust", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryExchangeMarginRateAdjust));
            }
            remove
            {
                _OnRspQryExchangeMarginRateAdjust -= value;
                (Invoke(this.PtrHandle, "RegRspQryExchangeMarginRateAdjust", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryExchangeMarginRateAdjust));
            }
        }

        ///请求查询汇率响应
        public delegate void RspQryExchangeRate(ref CThostFtdcExchangeRateField pExchangeRate, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryExchangeRate _OnRspQryExchangeRate;
        /// <summary>
        /// 请求查询汇率响应
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

        ///请求查询产品报价汇率
        public delegate void RspQryProductExchRate(ref CThostFtdcProductExchRateField pProductExchRate, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryProductExchRate _OnRspQryProductExchRate;
        /// <summary>
        /// 请求查询产品报价汇率
        /// </summary>
        public event RspQryProductExchRate OnRspQryProductExchRate
        {
            add
            {
                _OnRspQryProductExchRate += value;
                (Invoke(this.PtrHandle, "RegRspQryProductExchRate", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryProductExchRate));
            }
            remove
            {
                _OnRspQryProductExchRate -= value;
                (Invoke(this.PtrHandle, "RegRspQryProductExchRate", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryProductExchRate));
            }
        }

        ///请求查询询价响应
        public delegate void RspQryForQuote(ref CThostFtdcForQuoteField pForQuote, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryForQuote _OnRspQryForQuote;
        /// <summary>
        /// 请求查询询价响应
        /// </summary>
        public event RspQryForQuote OnRspQryForQuote
        {
            add
            {
                _OnRspQryForQuote += value;
                (Invoke(this.PtrHandle, "RegRspQryForQuote", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryForQuote));
            }
            remove
            {
                _OnRspQryForQuote -= value;
                (Invoke(this.PtrHandle, "RegRspQryForQuote", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryForQuote));
            }
        }

        ///询价通知
        public delegate void RtnForQuoteRsp(ref CThostFtdcForQuoteRspField pForQuoteRsp);
        private RtnForQuoteRsp _OnRtnForQuoteRsp;
        /// <summary>
        /// 询价通知
        /// </summary>
        public event RtnForQuoteRsp OnRtnForQuoteRsp
        {
            add
            {
                _OnRtnForQuoteRsp += value;
                (Invoke(this.PtrHandle, "RegRtnForQuoteRsp", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnForQuoteRsp));
            }
            remove
            {
                _OnRtnForQuoteRsp -= value;
                (Invoke(this.PtrHandle, "RegRtnForQuoteRsp", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnForQuoteRsp));
            }
        }

        ///询价录入请求响应
        public delegate void RspForQuoteInsert(ref CThostFtdcInputForQuoteField pInputForQuote, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspForQuoteInsert _OnRspForQuoteInsert;
        /// <summary>
        /// 询价录入请求响应
        /// </summary>
        public event RspForQuoteInsert OnRspForQuoteInsert
        {
            add
            {
                _OnRspForQuoteInsert += value;
                (Invoke(this.PtrHandle, "RegRspForQuoteInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspForQuoteInsert));
            }
            remove
            {
                _OnRspForQuoteInsert -= value;
                (Invoke(this.PtrHandle, "RegRspForQuoteInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspForQuoteInsert));
            }
        }

        ///询价录入错误回报
        public delegate void ErrRtnForQuoteInsert(ref CThostFtdcInputForQuoteField pInputForQuote, ref CThostFtdcRspInfoField pRspInfo);
        private ErrRtnForQuoteInsert _OnErrRtnForQuoteInsert;
        /// <summary>
        /// 询价录入错误回报
        /// </summary>
        public event ErrRtnForQuoteInsert OnErrRtnForQuoteInsert
        {
            add
            {
                _OnErrRtnForQuoteInsert += value;
                (Invoke(this.PtrHandle, "RegErrRtnForQuoteInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnForQuoteInsert));
            }
            remove
            {
                _OnErrRtnForQuoteInsert -= value;
                (Invoke(this.PtrHandle, "RegErrRtnForQuoteInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnForQuoteInsert));
            }
        }

        ///请求查询报价响应
        public delegate void RspQryQuote(ref CThostFtdcQuoteField pQuote, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryQuote _OnRspQryQuote;
        /// <summary>
        /// 请求查询报价响应
        /// </summary>
        public event RspQryQuote OnRspQryQuote
        {
            add
            {
                _OnRspQryQuote += value;
                (Invoke(this.PtrHandle, "RegRspQryQuote", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryQuote));
            }
            remove
            {
                _OnRspQryQuote -= value;
                (Invoke(this.PtrHandle, "RegRspQryQuote", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryQuote));
            }
        }

        ///报价通知
        public delegate void RtnQuote(ref CThostFtdcQuoteField pQuote);
        private RtnQuote _OnRtnQuote;
        /// <summary>
        /// 报价通知
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

        ///报价录入请求响应
        public delegate void RspQuoteInsert(ref CThostFtdcInputQuoteField pInputQuote, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQuoteInsert _OnRspQuoteInsert;
        /// <summary>
        /// 报价录入请求响应
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

        ///报价录入错误回报
        public delegate void ErrRtnQuoteInsert(ref CThostFtdcInputQuoteField pQuoteInsert, ref CThostFtdcRspInfoField pRspInfo);
        private ErrRtnQuoteInsert _OnErrRtnQuoteInsert;
        /// <summary>
        /// 报价录入错误回报
        /// </summary>
        public event ErrRtnQuoteInsert OnErrRtnQuoteInsert
        {
            add
            {
                _OnErrRtnQuoteInsert += value;
                (Invoke(this.PtrHandle, "RegErrRtnOrderInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnQuoteInsert));
            }
            remove
            {
                _OnErrRtnQuoteInsert -= value;
                (Invoke(this.PtrHandle, "RegErrRtnOrderInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnQuoteInsert));
            }
        }

        ///报价操作请求响应
        public delegate void RspQuoteAction(ref CThostFtdcInputQuoteActionField pInputQuoteAction, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQuoteAction _OnRspQuoteAction;
        /// <summary>
        /// 报价操作请求响应
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

        ///报价操作错误回报
        public delegate void ErrRtnQuoteAction(ref CThostFtdcQuoteActionField pQuoteAction, ref CThostFtdcRspInfoField pRspInfo);
        private ErrRtnQuoteAction _OnErrRtnQuoteAction;
        /// <summary>
        /// 报价操作错误回报
        /// </summary>
        public event ErrRtnQuoteAction OnErrRtnQuoteAction
        {
            add
            {
                _OnErrRtnQuoteAction += value;
                (Invoke(this.PtrHandle, "RegErrRtnOrderAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnQuoteAction));
            }
            remove
            {
                _OnErrRtnQuoteAction -= value;
                (Invoke(this.PtrHandle, "RegErrRtnOrderAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnQuoteAction));
            }
        }

        ///请求查询执行宣告响应
        public delegate void RspQryExecOrder(ref CThostFtdcExecOrderField pExecOrder, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryExecOrder _OnRspQryExecOrder;
        /// <summary>
        /// 请求查询执行宣告响应
        /// </summary>
        public event RspQryExecOrder OnRspQryExecOrder
        {
            add
            {
                _OnRspQryExecOrder += value;
                (Invoke(this.PtrHandle, "RegRspQryExecOrder", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryExecOrder));
            }
            remove
            {
                _OnRspQryExecOrder -= value;
                (Invoke(this.PtrHandle, "RegRspQryExecOrder", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryExecOrder));
            }
        }

        ///执行宣告通知
        public delegate void RtnExecOrder(ref CThostFtdcExecOrderField pExecOrder);
        private RtnExecOrder _OnRtnExecOrder;
        /// <summary>
        /// 执行宣告通知
        /// </summary>
        public event RtnExecOrder OnRtnExecOrder
        {
            add
            {
                _OnRtnExecOrder += value;
                (Invoke(this.PtrHandle, "RegRtnExecOrder", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnExecOrder));
            }
            remove
            {
                _OnRtnExecOrder -= value;
                (Invoke(this.PtrHandle, "RegRtnExecOrder", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnExecOrder));
            }
        }

        ///执行宣告录入请求响应
        public delegate void RspExecOrderInsert(ref CThostFtdcInputExecOrderField pInputExecOrder, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspExecOrderInsert _OnRspExecOrderInsert;
        /// <summary>
        /// 执行宣告录入请求响应
        /// </summary>
        public event RspExecOrderInsert OnRspExecOrderInsert
        {
            add
            {
                _OnRspExecOrderInsert += value;
                (Invoke(this.PtrHandle, "RegRspExecOrderInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspExecOrderInsert));
            }
            remove
            {
                _OnRspExecOrderInsert -= value;
                (Invoke(this.PtrHandle, "RegRspExecOrderInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspExecOrderInsert));
            }
        }

        ///执行宣告录入错误回报
        public delegate void ErrRtnExecOrderInsert(ref CThostFtdcInputExecOrderField pInputExecOrder, ref CThostFtdcRspInfoField pRspInfo);
        private ErrRtnExecOrderInsert _OnErrRtnExecOrderInsert;
        /// <summary>
        /// 执行宣告录入错误回报
        /// </summary>
        public event ErrRtnExecOrderInsert OnErrRtnExecOrderInsert
        {
            add
            {
                _OnErrRtnExecOrderInsert += value;
                (Invoke(this.PtrHandle, "RegErrRtnExecOrderInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnExecOrderInsert));
            }
            remove
            {
                _OnErrRtnExecOrderInsert -= value;
                (Invoke(this.PtrHandle, "RegErrRtnExecOrderInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnExecOrderInsert));
            }
        }

        ///执行宣告操作请求响应
        public delegate void RspExecOrderAction(ref CThostFtdcInputExecOrderActionField pInputExecOrderAction, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspExecOrderAction _OnRspExecOrderAction;
        /// <summary>
        /// 执行宣告操作请求响应
        /// </summary>
        public event RspExecOrderAction OnRspExecOrderAction
        {
            add
            {
                _OnRspExecOrderAction += value;
                (Invoke(this.PtrHandle, "RegRspExecOrderAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspExecOrderAction));
            }
            remove
            {
                _OnRspExecOrderAction -= value;
                (Invoke(this.PtrHandle, "RegRspExecOrderAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspExecOrderAction));
            }
        }

        ///执行宣告操作错误回报
        public delegate void ErrRtnExecOrderAction(ref CThostFtdcExecOrderActionField pExecOrderAction, ref CThostFtdcRspInfoField pRspInfo);
        private ErrRtnExecOrderAction _OnErrRtnExecOrderAction;
        /// <summary>
        /// 执行宣告操作错误回报
        /// </summary>
        public event ErrRtnExecOrderAction OnErrRtnExecOrderAction
        {
            add
            {
                _OnErrRtnExecOrderAction += value;
                (Invoke(this.PtrHandle, "RegErrRtnExecOrderAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnExecOrderAction));
            }
            remove
            {
                _OnErrRtnExecOrderAction -= value;
                (Invoke(this.PtrHandle, "RegErrRtnExecOrderAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnExecOrderAction));
            }
        }

        ///申请组合录入请求响应
        public delegate void RspCombActionInsert(ref CThostFtdcInputCombActionField pInputCombAction, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspCombActionInsert _OnRspCombActionInsert;
        /// <summary>
        /// 申请组合录入请求响应
        /// </summary>
        public event RspCombActionInsert OnRspCombActionInsert
        {
            add
            {
                _OnRspCombActionInsert += value;
                (Invoke(this.PtrHandle, "RegRspCombActionInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspCombActionInsert));
            }
            remove
            {
                _OnRspCombActionInsert -= value;
                (Invoke(this.PtrHandle, "RegRspCombActionInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspCombActionInsert));
            }
        }

        ///请求查询组合合约安全系数响应
        public delegate void RspQryCombInstrumentGuard(ref CThostFtdcCombInstrumentGuardField pCombInstrumentGuard, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryCombInstrumentGuard _OnRspQryCombInstrumentGuard;
        /// <summary>
        /// 请求查询组合合约安全系数响应
        /// </summary>
        public event RspQryCombInstrumentGuard OnRspQryCombInstrumentGuard
        {
            add
            {
                _OnRspQryCombInstrumentGuard += value;
                (Invoke(this.PtrHandle, "RegRspQryCombInstrumentGuard", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryCombInstrumentGuard));
            }
            remove
            {
                _OnRspQryCombInstrumentGuard -= value;
                (Invoke(this.PtrHandle, "RegRspQryCombInstrumentGuard", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryCombInstrumentGuard));
            }
        }

        ///请求查询申请组合响应
        public delegate void RspQryCombAction(ref CThostFtdcCombActionField pCombAction, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspQryCombAction _OnRspQryCombAction;
        /// <summary>
        /// 请求查询申请组合响应
        /// </summary>
        public event RspQryCombAction OnRspQryCombAction
        {
            add
            {
                _OnRspQryCombAction += value;
                (Invoke(this.PtrHandle, "RegRspQryCombAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryCombAction));
            }
            remove
            {
                _OnRspQryCombAction -= value;
                (Invoke(this.PtrHandle, "RegRspQryCombAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspQryCombAction));
            }
        }

        ///申请组合通知
        public delegate void RtnCombAction(ref CThostFtdcCombActionField pCombAction);
        private RtnCombAction _OnRtnCombAction;
        /// <summary>
        /// 申请组合通知
        /// </summary>
        public event RtnCombAction OnRtnCombAction
        {
            add
            {
                _OnRtnCombAction += value;
                (Invoke(this.PtrHandle, "RegRtnCombAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnCombAction));
            }
            remove
            {
                _OnRtnCombAction -= value;
                (Invoke(this.PtrHandle, "RegRtnCombAction", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnCombAction));
            }
        }

        ///申请组合录入错误回报
        public delegate void ErrRtnCombActionInsert(ref CThostFtdcInputCombActionField pInputCombAction, ref CThostFtdcRspInfoField pRspInfo);
        private ErrRtnCombActionInsert _OnErrRtnCombActionInsert;
        /// <summary>
        /// 申请组合录入错误回报
        /// </summary>
        public event ErrRtnCombActionInsert OnErrRtnCombActionInsert
        {
            add
            {
                _OnErrRtnCombActionInsert += value;
                (Invoke(this.PtrHandle, "RegErrRtnCombActionInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnCombActionInsert));
            }
            remove
            {
                _OnErrRtnCombActionInsert -= value;
                (Invoke(this.PtrHandle, "RegErrRtnCombActionInsert", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnErrRtnCombActionInsert));
            }
        }
    }
}
