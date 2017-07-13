using System;
using System.IO;
using System.Runtime.InteropServices;

namespace TradingMaster
{
    /// <summary>
	/// 行情接口
	/// </summary>
	public class FemasMdApi
    {
        const string DLLPROFILE = "dll\\FemasMdApi.dll";

        /// <summary>
        /// FemasMdApi.dll/USTPFtdcMdUserApi.dll 放在主程序的执行文件夹中
        /// </summary>
        /// <param name="_investor">投资者帐号</param>
        /// <param name="_pwd">密码</param>
        /// <param name="_broker">经纪公司代码</param>
        /// <param name="_addr">前置地址</param>
        public FemasMdApi(string _investor, string _pwd, string _broker, string _addr)
        {
            this.FrontAddr = _addr;
            this.BrokerID = _broker;
            this.InvestorID = _investor;
            this.Password = _pwd;
            ClearUserDll();
            LoadDll(DLLPROFILE);
        }

        /// <summary>
        /// 前置地址
        /// </summary>
        public string FrontAddr { get; set; }
        /// <summary>
        /// 经纪公司代码
        /// </summary>
        public string BrokerID { get; set; }
        /// <summary>
        /// 投资者代码
        /// </summary>
        public string InvestorID { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
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
                _DllFile = "FemasMdApi_" + this.InvestorID + "_" + DateTime.Now.Ticks + ".dll";
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
                        if (fileItem.StartsWith(strUserDir + "\\FemasMdApi_") && fileItem.EndsWith(".dll") && IsFileAvailable(fileItem))// + this.InvestorID + "_"
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
        /// 订阅合约行情
        /// </summary>
        /// <param name="instruments">合约代码:可填多个,订阅所有填null</param>
        private delegate int subMarketData(string[] instrumentsID, int nCount);
        public int SubMarketData(params string[] instruments)
        {
            return ((subMarketData)Invoke(this.PtrHandle, "SubMarketData", typeof(subMarketData)))(instruments, instruments == null ? 0 : instruments.Length);
        }

        /// <summary>
        /// 退订合约行情
        /// </summary>
        /// <param name="instruments">合约代码:可填多个,退订所有填null</param>
        private delegate int unSubMarketData(string[] instrumentsID, int nCount);
        public int UnSubMarketData(params string[] instruments)
        {
            return ((unSubMarketData)Invoke(this.PtrHandle, "UnSubMarketData", typeof(unSubMarketData)))(instruments, instruments == null ? 0 : instruments.Length);
        }

        /// <summary>
        /// 设置心跳超时时间
        /// </summary>
        private delegate void setHeartbeatTimeout(uint timeout);
        public void SetHeartbeatTimeout(uint timeout)
        {
            ((setHeartbeatTimeout)Invoke(this.PtrHandle, "SetHeartbeatTimeout", typeof(setHeartbeatTimeout)))(timeout);
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

        ///订阅主题请求
        private delegate int reqSubscribeTopic(ref CUstpFtdcDisseminationField pDissemination);
        public int ReqSubscribeTopic(CUstpFtdcDisseminationField pDissemination)
        {
            return ((reqSubscribeTopic)Invoke(this.PtrHandle, "ReqSubscribeTopic", typeof(reqSubscribeTopic)))(ref pDissemination);
        }

        ///主题查询请求
        private delegate int reqQryTopic(ref CUstpFtdcDisseminationField pDissemination);
        public int ReqQryTopic(CUstpFtdcDisseminationField pDissemination)
        {
            return ((reqQryTopic)Invoke(this.PtrHandle, "ReqQryTopic", typeof(reqQryTopic)))(ref pDissemination);
        }

        /// <summary>
        /// 订阅合约的相关信息
		/// </summary>
        private delegate int reqSubMarketData(ref CUstpFtdcSpecificInstrumentField pSpecificInstrument);
        public int ReqSubMarketData(string instrumentID)
        {
            CUstpFtdcSpecificInstrumentField pSpecificInstrument = new CUstpFtdcSpecificInstrumentField();
            pSpecificInstrument.InstrumentID = instrumentID;
            return ((reqSubMarketData)Invoke(this.PtrHandle, "ReqSubMarketData", typeof(reqSubMarketData)))(ref pSpecificInstrument);
        }

        /// <summary>
        /// 退订合约的相关信息
        /// </summary>
        private delegate int reqUnSubMarketData(ref CUstpFtdcSpecificInstrumentField pSpecificInstrument);
        public int ReqUnSubMarketData(string instrumentID)
        {
            CUstpFtdcSpecificInstrumentField pSpecificInstrument = new CUstpFtdcSpecificInstrumentField();
            pSpecificInstrument.InstrumentID = instrumentID;
            return ((reqUnSubMarketData)Invoke(this.PtrHandle, "ReqUnSubMarketData", typeof(reqUnSubMarketData)))(ref pSpecificInstrument);
        }

        //回调函数 ==================================================================================================================

        private delegate void Reg(IntPtr pPtr);

        #region 连接响应
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
        #endregion

        #region 断开应答
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
        #endregion

        #region 心跳超时警告
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
        #endregion

        #region 报文回调开始通知
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
        #endregion

        #region 报文回调结束通知
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
        #endregion

        #region 错误应答
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
        #endregion

        #region 风控前置系统用户登录应答
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
        #endregion

        #region 用户退出应答
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
        #endregion

        #region 订阅主题应答
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
        #endregion

        #region 主题查询应答
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
        #endregion

        #region 深度行情通知
        public delegate void RtnDepthMarketData(ref CUstpFtdcDepthMarketDataField pDepthMarketData);
        private RtnDepthMarketData _OnRtnDepthMarketData;
        /// <summary>
        /// 深度行情通知
        /// </summary>
        public event RtnDepthMarketData OnRtnDepthMarketData
        {
            add
            {
                _OnRtnDepthMarketData += value;
                (Invoke(this.PtrHandle, "RegRtnDepthMarketData", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnDepthMarketData));
            }
            remove
            {
                _OnRtnDepthMarketData -= value;
                (Invoke(this.PtrHandle, "RegRtnDepthMarketData", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRtnDepthMarketData));
            }
        }
        #endregion

        #region 订阅行情应答
        public delegate void RspSubMarketData(ref CUstpFtdcSpecificInstrumentField pSpecificInstrument, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspSubMarketData _OnRspSubMarketData;
        /// <summary>
        /// 订阅行情应答
        /// </summary>
        public event RspSubMarketData OnRspSubMarketData
        {
            add
            {
                _OnRspSubMarketData += value;
                (Invoke(this.PtrHandle, "RegRspSubMarketData", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspSubMarketData));
            }
            remove
            {
                _OnRspSubMarketData -= value;
                (Invoke(this.PtrHandle, "RegRspSubMarketData", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspSubMarketData));
            }
        }
        #endregion

        #region 退订请求应答
        public delegate void RspUnSubMarketData(ref CUstpFtdcSpecificInstrumentField pSpecificInstrument, ref CUstpFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspUnSubMarketData _OnRspUnSubMarketData;
        /// <summary>
        /// 退订请求应答
        /// </summary>
        public event RspUnSubMarketData OnRspUnSubMarketData
        {
            add
            {
                _OnRspUnSubMarketData += value;
                (Invoke(this.PtrHandle, "RegRspUnSubMarketData", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspUnSubMarketData));
            }
            remove
            {
                _OnRspUnSubMarketData -= value;
                (Invoke(this.PtrHandle, "RegRspUnSubMarketData", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspUnSubMarketData));
            }
        }
        #endregion

    }
}
