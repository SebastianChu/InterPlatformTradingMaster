using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

namespace TradingMaster
{
	/// <summary>
	/// 行情接口
	/// </summary>
	public class CtpMdApi
	{
        const string DLLPROFILE = "dll\\CtpMdApi.dll";

		/// <summary>
		/// MdApi.dll/CTPMdApi.dll/thostmduserapi.dll 放在主程序的执行文件夹中
		/// </summary>
		/// <param name="_investor">投资者帐号:海风351962</param>
		/// <param name="_pwd">密码</param>
		/// <param name="_broker">经纪公司代码:2030-CTP模拟</param>
		/// <param name="_addr">前置地址:默认为CTP模拟</param>
        //public MdApi(string _investor, string _pwd, string _broker = "2030"
        //    , string _addr = "tcp://asp-sim2-md1.financial-trading-platform.com:26213")
        public CtpMdApi(string _investor, string _pwd, string _broker, string _addr)
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
		/// 经纪公司代码ctp-2030;上期-4030;
		/// </summary>
		public string BrokerID { get; set; }
		/// <summary>
		/// 投资者代码 351962-申万
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
            string dir = Environment.CurrentDirectory;// +"\\dll\\Users";
            if (File.Exists(strFile))
            {
                _DllFile = "CtpMdApi_" + this.InvestorID + "_" + DateTime.Now.Ticks + ".dll";
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
                throw (new Exception(String.Format(" 没有找到 :{0}.", _DllFile)));
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
                        if (fileItem.StartsWith(strUserDir + "\\CtpMdApi_") && fileItem.EndsWith(".dll") && IsFileAvailable(fileItem))// + this.InvestorID + "_"
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
		/// 登录
		/// </summary>
        private delegate int reqUserLogin(string pBroker, string pInvestor, string pPwd);
        public int ReqUserLogin()
        {
            return ((reqUserLogin)Invoke(this.PtrHandle, "ReqUserLogin", typeof(reqUserLogin)))(this.BrokerID, this.InvestorID, this.Password);
        }

		/// <summary>
		/// 用户注销
		/// </summary>
        private delegate int reqUserLogout(string BROKER_ID, string INVESTOR_ID);
        public int ReqUserLogout()
        {
            return ((reqUserLogout)Invoke(this.PtrHandle, "ReqUserLogout", typeof(reqUserLogout)))(this.BrokerID, this.InvestorID);
        }

		/// <summary>
		/// 订阅行情
		/// </summary>
		/// <param name="instruments">合约代码:可填多个,订阅所有填null</param>
        private delegate int subMarketData(string[] instrumentsID, int nCount);
        public int SubscribeMarketData(params string[] instruments)
        {
            return ((subMarketData)Invoke(this.PtrHandle, "SubscribeMarketData", typeof(subMarketData)))(instruments, instruments == null ? 0 : instruments.Length);
        }

		/// <summary>
		/// 退订行情
		/// </summary>
		/// <param name="instruments">合约代码:可填多个,退订所有填null</param>
        private delegate int unSubMarketData(string[] instrumentsID, int nCount);
        public int UnSubscribeMarketData(params string[] instruments)
        {
            return ((unSubMarketData)Invoke(this.PtrHandle, "UnSubscribeMarketData", typeof(unSubMarketData)))(instruments, instruments == null ? 0 : instruments.Length);
        }

        /// <summary>
        /// 订阅询价
        /// </summary>
        /// <param name="instruments">合约代码:可填多个,订阅所有填null</param>
        private delegate int subForQuoteRsp(string[] instrumentsID, int nCount);
        public int SubscribeForQuoteRsp(params string[] instruments)
        {
            return ((subForQuoteRsp)Invoke(this.PtrHandle, "SubscribeForQuoteRsp", typeof(subForQuoteRsp)))(instruments, instruments == null ? 0 : instruments.Length);
        }

        /// <summary>
        /// 退订询价
        /// </summary>
        /// <param name="instruments">合约代码:可填多个,退订所有填null</param>
        private delegate int unSubForQuoteRsp(string[] instrumentsID, int nCount);
        public int UnSubscribeForQuoteRsp(params string[] instruments)
        {
            return ((unSubForQuoteRsp)Invoke(this.PtrHandle, "UnSubscribeForQuoteRsp", typeof(unSubForQuoteRsp)))(instruments, instruments == null ? 0 : instruments.Length);
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

        #region 心跳响应
        /// <summary>
        /// 
        /// </summary>
        public delegate void HeartBeatWarning(int nTimeLapse);
        private HeartBeatWarning _OnHeartBeatWarning;
        /// <summary>
        /// 当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
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

		#region 登入请求应答
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
		#endregion

		#region 登出请求应答
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
		#endregion

        #region 错误应答
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
        #endregion

		#region 订阅行情应答
        public delegate void RspSubMarketData(ref CThostFtdcSpecificInstrumentField pSpecificInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
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
        public delegate void RspUnSubMarketData(ref CThostFtdcSpecificInstrumentField pSpecificInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
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

		#region 深度行情通知
        public delegate void RtnDepthMarketData(ref CThostFtdcDepthMarketDataField pDepthMarketData);
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

        #region 订阅询价应答
        public delegate void RspSubForQuoteRsp(ref CThostFtdcSpecificInstrumentField pSpecificInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspSubForQuoteRsp _OnRspSubForQuoteRsp;
        /// <summary>
        /// 订阅询价应答
        /// </summary>
        public event RspSubForQuoteRsp OnRspSubForQuoteRsp
        {
            add
            {
                _OnRspSubForQuoteRsp += value;
                (Invoke(this.PtrHandle, "RegRspSubForQuoteRsp", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspSubForQuoteRsp));
            }
            remove
            {
                _OnRspSubForQuoteRsp -= value;
                (Invoke(this.PtrHandle, "RegRspSubForQuoteRsp", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspSubForQuoteRsp));
            }
        }
        #endregion

        #region 退订询价应答
        public delegate void RspUnSubForQuoteRsp(ref CThostFtdcSpecificInstrumentField pSpecificInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        private RspUnSubForQuoteRsp _OnRspUnSubForQuoteRsp;
        /// <summary>
        /// 退订询价应答
        /// </summary>
        public event RspUnSubForQuoteRsp OnRspUnSubForQuoteRsp
        {
            add
            {
                _OnRspUnSubForQuoteRsp += value;
                (Invoke(this.PtrHandle, "RegRspUnSubForQuoteRsp", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspUnSubForQuoteRsp));
            }
            remove
            {
                _OnRspUnSubForQuoteRsp -= value;
                (Invoke(this.PtrHandle, "RegRspUnSubForQuoteRsp", typeof(Reg)) as Reg)(Marshal.GetFunctionPointerForDelegate(_OnRspUnSubForQuoteRsp));
            }
        }
        #endregion

        #region 询价通知
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
        #endregion

	}
}
