// CtpTrade.cpp : 定义 DLL 应用程序的导出函数。
//

#include "FemasTrader.h"

// 请求编号
int iRequestID = 0;
//连接
TRADEAPI_API void WINAPI Connect(char* FRONT_ADDR)
{
	// 初始化UserApi
	pUserApi = CUstpFtdcTraderApi::CreateFtdcTraderApi();			// 创建UserApi
	CTraderSpi* pUserSpi = new CTraderSpi();
	pUserApi->RegisterSpi((CUstpFtdcTraderSpi*)pUserSpi);			// 注册事件类
	pUserApi->SubscribePublicTopic(USTP_TERT_QUICK/*USTP_TERT_RESTART*/);					// 注册公有流
	pUserApi->SubscribePrivateTopic(USTP_TERT_QUICK/*USTP_TERT_RESTART*/);					// 注册私有流
	pUserApi->RegisterFront(FRONT_ADDR);							// connect
	pUserApi->Init();
	//pUserApi->Join();
}

TRADEAPI_API const char *GetTradingDay()
{
	return pUserApi->GetTradingDay();
}

//断开
TRADEAPI_API void WINAPI DisConnect()
{
	// 释放UserApi
	if (pUserApi)
	{
		pUserApi->RegisterSpi(NULL);
		pUserApi->Release();
		pUserApi = NULL;
	}
	// 释放UserSpi实例
	/*if (m_pUserSpiImpl)
	{
		delete m_pUserSpiImpl;
		m_pUserSpiImpl = NULL;
	}*/
}

//风控前置系统用户登录请求
TRADEAPI_API int WINAPI ReqUserLogin(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcUserIDType USER_ID,TUstpFtdcPasswordType PASSWORD)
{
	CUstpFtdcReqUserLoginField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.UserID, USER_ID);
	strcpy_s(req.Password, PASSWORD);
	strcpy_s(req.UserProductInfo,"IPTM_v1.0");
	return pUserApi->ReqUserLogin(&req, ++iRequestID);
}

//用户退出请求
TRADEAPI_API int WINAPI ReqUserLogout(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcUserIDType INVESTOR_ID)
{
	CUstpFtdcReqUserLogoutField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.UserID, INVESTOR_ID);
	return pUserApi->ReqUserLogout(&req, ++iRequestID);
}

//用户密码修改请求
TRADEAPI_API int WINAPI ReqUserPasswordUpdate(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcUserIDType USER_ID,TUstpFtdcUserIDType OLD_PASSWORD,TUstpFtdcPasswordType NEW_PASSWORD)
{
	CUstpFtdcUserPasswordUpdateField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.UserID, USER_ID);
	strcpy_s(req.OldPassword,OLD_PASSWORD);
	strcpy_s(req.NewPassword,NEW_PASSWORD);
	return pUserApi->ReqUserPasswordUpdate(&req, ++iRequestID);
}

//报单录入请求
TRADEAPI_API int WINAPI ReqOrderInsert(CUstpFtdcInputOrderField *pOrder)
{
	strcpy_s(pOrder->BusinessUnit,"IPTM_v1.0");
	return pUserApi->ReqOrderInsert(pOrder, ++iRequestID);
}

//报单操作请求
TRADEAPI_API int WINAPI ReqOrderAction(CUstpFtdcOrderActionField *pOrder)
{
	return pUserApi->ReqOrderAction(pOrder, ++iRequestID);
}

///报价录入请求
TRADEAPI_API int WINAPI ReqQuoteInsert(CUstpFtdcInputQuoteField *pInputQuote)
{
	return pUserApi->ReqQuoteInsert(pInputQuote, ++iRequestID);
}

///报价操作请求
TRADEAPI_API int WINAPI ReqQuoteAction(CUstpFtdcQuoteActionField *pInputQuoteAction)
{
	return pUserApi->ReqQuoteAction(pInputQuoteAction, ++iRequestID);
}

///客户询价请求
TRADEAPI_API int WINAPI ReqForQuote(CUstpFtdcReqForQuoteField *pReqForQuote)
{
	return pUserApi->ReqForQuote(pReqForQuote, ++iRequestID);
}

///客户申请组合请求
TRADEAPI_API int WINAPI ReqMarginCombAction(CUstpFtdcInputMarginCombActionField *pInputMarginCombAction)
{
	return pUserApi->ReqMarginCombAction(pInputMarginCombAction, ++iRequestID);
}

///报单查询请求
TRADEAPI_API int WINAPI ReqQryOrder(CUstpFtdcQryOrderField *pQryOrder)
{
	return pUserApi->ReqQryOrder(pQryOrder, ++iRequestID);
}

///成交单查询请求
TRADEAPI_API int WINAPI ReqQryTrade(CUstpFtdcQryTradeField *pQryTrade)
{
	return pUserApi->ReqQryTrade(pQryTrade, ++iRequestID);
}

///可用投资者账户查询请求
TRADEAPI_API int WINAPI ReqQryUserInvestor(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcUserIDType USER_ID)
{
	CUstpFtdcQryUserInvestorField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID ,BROKER_ID);
	strcpy_s(req.UserID,USER_ID);
	return pUserApi->ReqQryUserInvestor(&req, ++iRequestID);
}

///交易编码查询请求
TRADEAPI_API int WINAPI ReqQryTradingCode(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcInvestorIDType INVESTOR_ID,TUstpFtdcUserIDType USER_ID,TUstpFtdcExchangeIDType	EXCHANGE_ID)
{
	CUstpFtdcQryTradingCodeField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID ,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	if(USER_ID != NULL)
		strcpy_s(req.UserID,USER_ID);
	if(EXCHANGE_ID != NULL)
		strcpy_s(req.ExchangeID,EXCHANGE_ID);
	return pUserApi->ReqQryTradingCode(&req, ++iRequestID);
}

//投资者资金账户查询请求
TRADEAPI_API int WINAPI ReqQryInvestorAccount(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcInvestorIDType INVESTOR_ID,TUstpFtdcUserIDType USER_ID)
{
	CUstpFtdcQryInvestorAccountField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.InvestorID, INVESTOR_ID);
	strcpy_s(req.UserID, USER_ID);
	return pUserApi->ReqQryInvestorAccount(&req, ++iRequestID);
}

///合约查询请求
TRADEAPI_API int WINAPI ReqQryInstrument(TUstpFtdcInstrumentIDType INSTRUMENT_ID)
{
	CUstpFtdcQryInstrumentField req;
	memset(&req, 0, sizeof(req));
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.InstrumentID, INSTRUMENT_ID);
	return pUserApi->ReqQryInstrument(&req, ++iRequestID);
}

///交易所查询请求
TRADEAPI_API int WINAPI ReqQryExchange(TUstpFtdcExchangeIDType EXCHANGE_ID)
{
	CUstpFtdcQryExchangeField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.ExchangeID,EXCHANGE_ID);
	return pUserApi->ReqQryExchange(&req, ++iRequestID);
}

//投资者持仓查询请求
TRADEAPI_API int WINAPI ReqQryInvestorPosition(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcInvestorIDType INVESTOR_ID,TUstpFtdcInstrumentIDType INSTRUMENT_ID)
{
	CUstpFtdcQryInvestorPositionField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.InvestorID, INVESTOR_ID);
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.InstrumentID, INSTRUMENT_ID);
	return pUserApi->ReqQryInvestorPosition(&req, ++iRequestID);
}

///订阅主题请求
TRADEAPI_API int WINAPI ReqSubscribeTopic(CUstpFtdcDisseminationField *pDissemination)
{
	return pUserApi->ReqSubscribeTopic(pDissemination, ++iRequestID);
}

///合规参数查询请求
TRADEAPI_API int WINAPI ReqQryComplianceParam(CUstpFtdcQryComplianceParamField *pQryComplianceParam)
{
	return pUserApi->ReqQryComplianceParam(pQryComplianceParam, ++iRequestID);
}

///主题查询请求
TRADEAPI_API int WINAPI ReqQryTopic(CUstpFtdcDisseminationField *pDissemination)
{
	return pUserApi->ReqQryTopic(pDissemination, ++iRequestID);
}

///投资者手续费率查询请求
TRADEAPI_API int WINAPI ReqQryInvestorFee(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcInvestorIDType INVESTOR_ID,TUstpFtdcUserIDType USER_ID,TUstpFtdcInstrumentIDType INSTRUMENT_ID, TUstpFtdcExchangeIDType EXCHANGE_ID)
{
	CUstpFtdcQryInvestorFeeField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID ,BROKER_ID);
	if(EXCHANGE_ID != NULL)
		strcpy_s(req.ExchangeID,EXCHANGE_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);	
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.InstrumentID,INSTRUMENT_ID);
	strcpy_s(req.UserID, USER_ID);
	return pUserApi->ReqQryInvestorFee(&req, ++iRequestID);
}

///投资者保证金率查询请求
TRADEAPI_API int WINAPI ReqQryInvestorMargin(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcInvestorIDType INVESTOR_ID,TUstpFtdcUserIDType USER_ID,TUstpFtdcInstrumentIDType INSTRUMENT_ID,TUstpFtdcExchangeIDType EXCHANGE_ID)
{
	CUstpFtdcQryInvestorMarginField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID ,BROKER_ID);
	if(EXCHANGE_ID != NULL)
		strcpy_s(req.ExchangeID,EXCHANGE_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.InstrumentID,INSTRUMENT_ID);//*不*能采用null进行所有查询
	strcpy_s(req.UserID, USER_ID);
	return pUserApi->ReqQryInvestorMargin(&req, ++iRequestID);
}

///交易编码组合持仓查询请求
TRADEAPI_API int WINAPI ReqQryInvestorCombPosition(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcInvestorIDType INVESTOR_ID,TUstpFtdcInstrumentIDType INSTRUMENT_ID,TUstpFtdcExchangeIDType EXCHANGE_ID,TUstpFtdcHedgeFlagType HEDGE_FLAG)
{
	CUstpFtdcQryInvestorCombPositionField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	if(EXCHANGE_ID != NULL)
		strcpy_s(req.ExchangeID,EXCHANGE_ID);
	if(HEDGE_FLAG != NULL)
		req.HedgeFlag = HEDGE_FLAG;
	strcpy_s(req.InvestorID,INVESTOR_ID);
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.CombInstrumentID, INSTRUMENT_ID);
	return pUserApi->ReqQryInvestorCombPosition(&req, ++iRequestID);
}

///交易编码单腿持仓查询请求
TRADEAPI_API int WINAPI ReqQryInvestorLegPosition(CUstpFtdcQryInvestorLegPositionField *pQryInvestorLegPosition)
{
	return pUserApi->ReqQryInvestorLegPosition(pQryInvestorLegPosition, ++iRequestID);
}

///请求查询汇率
TRADEAPI_API int WINAPI ReqQryExchangeRate(CUstpFtdcQryExchangeRateField *pQryExchangeRate)
{
	return pUserApi->ReqQryExchangeRate(pQryExchangeRate, ++iRequestID);
}

///==================================== 回调函数 =======================================///
TRADEAPI_API void WINAPI RegOnFrontConnected(DefOnFrontConnected cb)	///当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
{	_OnFrontConnected = cb;	}

TRADEAPI_API void WINAPI RegOnFrontDisconnected(DefOnFrontDisconnected cb)	///当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
{	_OnFrontDisconnected = cb;	}

TRADEAPI_API void WINAPI RegOnHeartBeatWarning(DefOnHeartBeatWarning cb)	///心跳超时警告。当长时间未收到报文时，该方法被调用。
{	_OnHeartBeatWarning = cb;	}

TRADEAPI_API void WINAPI RegPackageStart(DefOnPackageStart cb)	///心跳超时警告。当长时间未收到报文时，该方法被调用。
{	_OnPackageStart = cb;	}

TRADEAPI_API void WINAPI RegPackageEnd(DefOnPackageEnd cb)	///心跳超时警告。当长时间未收到报文时，该方法被调用。
{	_OnPackageEnd = cb;	}

TRADEAPI_API void WINAPI RegRspError(DefOnRspError cb)///错误应答
{	_OnRspError = cb;	}

TRADEAPI_API void WINAPI RegRspUserLogin(DefOnRspUserLogin cb)///风控前置系统用户登录应答
{	_OnRspUserLogin = cb;	}

TRADEAPI_API void WINAPI RegRspUserLogout(DefOnRspUserLogout cb)///用户退出应答
{	_OnRspUserLogout = cb;	}

TRADEAPI_API void WINAPI RegRspUserPasswordUpdate(DefOnRspUserPasswordUpdate cb)///用户密码修改应答
{	_OnRspUserPasswordUpdate = cb;	}

TRADEAPI_API void WINAPI RegRspOrderInsert(DefOnRspOrderInsert cb)///报单录入应答
{	_OnRspOrderInsert = cb;	}

TRADEAPI_API void WINAPI RegRspOrderAction(DefOnRspOrderAction cb)///报单操作应答
{	_OnRspOrderAction = cb;	}

TRADEAPI_API void WINAPI RegRspQuoteInsert(DefOnRspQuoteInsert cb)///报价录入应答
{	_OnRspQuoteInsert = cb;	}

TRADEAPI_API void WINAPI RegRspQuoteAction(DefOnRspQuoteAction cb)///报价操作应答
{	_OnRspQuoteAction = cb;	}

TRADEAPI_API void WINAPI RegRspForQuote(DefOnRspForQuote cb)///询价请求应答
{	_OnRspForQuote = cb;	}

TRADEAPI_API void WINAPI RegRspMarginCombAction(DefOnRspMarginCombAction cb)///客户申请组合应答
{	_OnRspMarginCombAction = cb;	}

TRADEAPI_API void WINAPI RegRtnFlowMessageCancel(DefOnRtnFlowMessageCancel cb)///数据流回退通知
{	_OnRtnFlowMessageCancel = cb;	}

TRADEAPI_API void WINAPI RegRtnTrade(DefOnRtnTrade cb)///成交通知
{	_OnRtnTrade = cb;	}

TRADEAPI_API void WINAPI RegRtnOrder(DefOnRtnOrder cb)///报单通知
{	_OnRtnOrder = cb;	}

TRADEAPI_API void WINAPI RegErrRtnOrderInsert(DefOnErrRtnOrderInsert cb)///报单录入错误回报
{	_OnErrRtnOrderInsert = cb;	}

TRADEAPI_API void WINAPI RegErrRtnOrderAction(DefOnErrRtnOrderAction cb)///报单操作错误回报
{	_OnErrRtnOrderAction = cb;	}

TRADEAPI_API void WINAPI RegRtnInstrumentStatus(DefOnRtnInstrumentStatus cb)///合约交易状态通知
{	_OnRtnInstrumentStatus = cb;	}

TRADEAPI_API void WINAPI RegRtnInvestorAccountDeposit(DefOnRtnInvestorAccountDeposit cb)///账户出入金回报
{	_OnRtnInvestorAccountDeposit = cb;	}

TRADEAPI_API void WINAPI RegRtnQuote(DefOnRtnQuote cb)///报价回报
{	_OnRtnQuote = cb;	}

TRADEAPI_API void WINAPI RegErrRtnQuoteInsert(DefOnErrRtnQuoteInsert cb)///报价录入错误回报
{	_OnErrRtnQuoteInsert = cb;	}

TRADEAPI_API void WINAPI RegRtnForQuote(DefOnRtnForQuote cb)///询价回报
{	_OnRtnForQuote = cb;	}

TRADEAPI_API void WINAPI RegRtnMarginCombinationLeg(DefOnRtnMarginCombinationLeg cb)///增加组合规则通知
{	_OnRtnMarginCombinationLeg = cb;	}

TRADEAPI_API void WINAPI RegRtnMarginCombAction(DefOnRtnMarginCombAction cb)///客户申请组合确认
{	_OnRtnMarginCombAction = cb;	}

TRADEAPI_API void WINAPI RegRspQryOrder(DefOnRspQryOrder cb)///报单查询应答
{	_OnRspQryOrder = cb;	}

TRADEAPI_API void WINAPI RegRspQryTrade(DefOnRspQryTrade cb)///成交单查询应答
{	_OnRspQryTrade = cb;	}

TRADEAPI_API void WINAPI RegRspQryUserInvestor(DefOnRspQryUserInvestor cb)///可用投资者账户查询应答
{	_OnRspQryUserInvestor = cb;	}

TRADEAPI_API void WINAPI RegRspQryTradingCode(DefOnRspQryTradingCode cb)///交易编码查询应答
{	_OnRspQryTradingCode = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorAccount(DefOnRspQryInvestorAccount cb)///投资者资金账户查询应答
{	_OnRspQryInvestorAccount = cb;	}

TRADEAPI_API void WINAPI RegRspQryInstrument(DefOnRspQryInstrument cb)///合约查询应答
{	_OnRspQryInstrument = cb;	}

TRADEAPI_API void WINAPI RegRspQryExchange(DefOnRspQryExchange cb)///交易所查询应答
{	_OnRspQryExchange = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorPosition(DefOnRspQryInvestorPosition cb)///投资者持仓查询应答
{	_OnRspQryInvestorPosition = cb;	}

TRADEAPI_API void WINAPI RegRspSubscribeTopic(DefOnRspSubscribeTopic cb)///订阅主题应答
{	_OnRspSubscribeTopic = cb;	}

TRADEAPI_API void WINAPI RegRspQryComplianceParam(DefOnRspQryComplianceParam cb)///合规参数查询应答
{	_OnRspQryComplianceParam = cb;	}

TRADEAPI_API void WINAPI RegRspQryTopic(DefOnRspQryTopic cb)///投资者持仓查询主题查询应答应答
{	_OnRspQryTopic = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorFee(DefOnRspQryInvestorFee cb)///投资者手续费率查询应答
{	_OnRspQryInvestorFee = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorMargin(DefOnRspQryInvestorMargin cb)///投资者保证金率查询应答
{	_OnRspQryInvestorMargin = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorCombPosition(DefOnRspQryInvestorCombPosition cb)///交易编码组合持仓查询应答
{	_OnRspQryInvestorCombPosition = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorLegPosition(DefOnRspQryInvestorLegPosition cb)///交易编码单腿持仓查询应答
{	_OnRspQryInvestorLegPosition = cb;	}

TRADEAPI_API void WINAPI RegRspQryExchangeRate(DefOnRspQryExchangeRate cb) ///交叉汇率查询应答
{	_OnRspQryExchangeRate = cb;	}


// 请求编号
//extern int iRequestID;

///当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
void CTraderSpi::OnFrontConnected()
{
	if(_OnFrontConnected!=NULL)
	{
		((DefOnFrontConnected)_OnFrontConnected)();
	}
}

///当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
void CTraderSpi::OnFrontDisconnected(int nReason)
{
	if(_OnFrontDisconnected != NULL)
	{
		((DefOnFrontDisconnected)_OnFrontDisconnected)(nReason);
	}
}

///心跳超时警告。当长时间未收到报文时，该方法被调用。
void CTraderSpi::OnHeartBeatWarning(int nTimeLapse)
{
	if(_OnHeartBeatWarning != NULL)
	{
		((DefOnHeartBeatWarning)_OnHeartBeatWarning)(nTimeLapse);
	}
}

///报文回调开始通知。当API收到一个报文后，首先调用本方法，然后是各数据域的回调，最后是报文回调结束通知。
void CTraderSpi::OnPackageStart(int nTopicID, int nSequenceNo)
{}

//报文回调结束通知。当API收到一个报文后，首先调用报文回调开始通知，然后是各数据域的回调，最后调用本方法。
void CTraderSpi::OnPackageEnd(int nTopicID, int nSequenceNo)
{}

///错误应答
void CTraderSpi::OnRspError(CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspError!=NULL)
	{
		if(pRspInfo == NULL)
		{
			CUstpFtdcRspInfoField req;
			memset(&req,0,sizeof(req));
			((DefOnRspError)_OnRspError)(&req, nRequestID, bIsLast);
		}
		else
			((DefOnRspError)_OnRspError)(pRspInfo,nRequestID, bIsLast);
	}
}

///风控前置系统用户登录应答
void CTraderSpi::OnRspUserLogin(CUstpFtdcRspUserLoginField *pRspUserLogin, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspUserLogin!=NULL)
	{
		if(pRspUserLogin == NULL)
		{
			CUstpFtdcRspUserLoginField req;
			memset(&req,0,sizeof(req));
			((DefOnRspUserLogin)_OnRspUserLogin)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspUserLogin)_OnRspUserLogin)(pRspUserLogin,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///用户退出应答
void CTraderSpi::OnRspUserLogout(CUstpFtdcRspUserLogoutField *pUserLogout, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspUserLogout!=NULL)
	{
		if(pUserLogout == NULL)
		{
			CUstpFtdcRspUserLogoutField req;
			memset(&req,0,sizeof(req));
			((DefOnRspUserLogout)_OnRspUserLogout)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspUserLogout)_OnRspUserLogout)(pUserLogout,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///用户密码修改应答
void CTraderSpi::OnRspUserPasswordUpdate(CUstpFtdcUserPasswordUpdateField *pUserPasswordUpdate, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspUserPasswordUpdate!=NULL)
	{
		if(pUserPasswordUpdate == NULL)
		{
			CUstpFtdcUserPasswordUpdateField req;
			memset(&req,0,sizeof(req));
			((DefOnRspUserPasswordUpdate)_OnRspUserPasswordUpdate)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspUserPasswordUpdate)_OnRspUserPasswordUpdate)(pUserPasswordUpdate,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///报单录入应答
void CTraderSpi::OnRspOrderInsert(CUstpFtdcInputOrderField *pInputOrder, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspOrderInsert!=NULL)
	{
		if(pInputOrder == NULL)
		{
			CUstpFtdcInputOrderField req;
			memset(&req,0,sizeof(req));
			((DefOnRspOrderInsert)_OnRspOrderInsert)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspOrderInsert)_OnRspOrderInsert)(pInputOrder,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///报单操作应答
void CTraderSpi::OnRspOrderAction(CUstpFtdcOrderActionField *pInputOrderAction, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspOrderAction!=NULL)
	{
		if(pInputOrderAction == NULL)
		{
			CUstpFtdcOrderActionField req;
			memset(&req,0,sizeof(req));
			((DefOnRspOrderAction)_OnRspOrderAction)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspOrderAction)_OnRspOrderAction)(pInputOrderAction,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///报价录入应答
void CTraderSpi::OnRspQuoteInsert(CUstpFtdcInputQuoteField *pInputQuote, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQuoteInsert!=NULL)
	{
		if(pInputQuote == NULL)
		{
			CUstpFtdcInputQuoteField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQuoteInsert)_OnRspQuoteInsert)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQuoteInsert)_OnRspQuoteInsert)(pInputQuote,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///报价操作应答
void CTraderSpi::OnRspQuoteAction(CUstpFtdcQuoteActionField *pInputQuoteAction, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQuoteAction!=NULL)
	{
		if(pInputQuoteAction == NULL)
		{
			CUstpFtdcQuoteActionField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQuoteAction)_OnRspQuoteAction)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQuoteAction)_OnRspQuoteAction)(pInputQuoteAction,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///询价请求应答
void CTraderSpi::OnRspForQuote(CUstpFtdcReqForQuoteField *pReqForQuote, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspForQuote!=NULL)
	{
		if(pReqForQuote == NULL)
		{
			CUstpFtdcReqForQuoteField req;
			memset(&req,0,sizeof(req));
			((DefOnRspForQuote)_OnRspForQuote)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspForQuote)_OnRspForQuote)(pReqForQuote,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///客户申请组合应答
void CTraderSpi::OnRspMarginCombAction(CUstpFtdcInputMarginCombActionField *pInputMarginCombAction, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspMarginCombAction!=NULL)
	{
		if(pInputMarginCombAction == NULL)
		{
			CUstpFtdcInputMarginCombActionField req;
			memset(&req,0,sizeof(req));
			((DefOnRspMarginCombAction)_OnRspMarginCombAction)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspMarginCombAction)_OnRspMarginCombAction)(pInputMarginCombAction,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///数据流回退通知
void CTraderSpi::OnRtnFlowMessageCancel(CUstpFtdcFlowMessageCancelField *pFlowMessageCancel)
{
	if(_OnRtnFlowMessageCancel!=NULL)
	{
		if(pFlowMessageCancel == NULL)
		{
			CUstpFtdcFlowMessageCancelField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnFlowMessageCancel)_OnRtnFlowMessageCancel)(&req);
		}
		else
			((DefOnRtnFlowMessageCancel)_OnRtnFlowMessageCancel)(pFlowMessageCancel);
	}
}

///成交回报
void CTraderSpi::OnRtnTrade(CUstpFtdcTradeField *pTrade) 
{
	if(_OnRtnTrade!=NULL)
	{
		if(pTrade == NULL)
		{
			CUstpFtdcTradeField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnTrade)_OnRtnTrade)(&req);
		}
		else
			((DefOnRtnTrade)_OnRtnTrade)(pTrade);
	}
}

///报单回报
void CTraderSpi::OnRtnOrder(CUstpFtdcOrderField *pOrder) 
{
	if(_OnRtnOrder!=NULL)
	{
		if(pOrder == NULL)
		{
			CUstpFtdcOrderField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnOrder)_OnRtnOrder)(&req);
		}
		else
			((DefOnRtnOrder)_OnRtnOrder)(pOrder);
	}
}
///报单录入错误回报
void CTraderSpi::OnErrRtnOrderInsert(CUstpFtdcInputOrderField *pInputOrder, CUstpFtdcRspInfoField *pRspInfo) 
{
	if(_OnErrRtnOrderInsert!=NULL)
	{
		if(pInputOrder == NULL)
		{
			CUstpFtdcInputOrderField req;
			memset(&req,0,sizeof(req));
			((DefOnErrRtnOrderInsert)_OnErrRtnOrderInsert)(&req,repareInfo(pRspInfo));
		}
		else
			((DefOnErrRtnOrderInsert)_OnErrRtnOrderInsert)(pInputOrder,repareInfo(pRspInfo));
	}
}

///报单操作错误回报
void CTraderSpi::OnErrRtnOrderAction(CUstpFtdcOrderActionField *pOrderAction, CUstpFtdcRspInfoField *pRspInfo) 
{
	if(_OnErrRtnOrderAction!=NULL)
	{
		if(pOrderAction == NULL)
		{
			CUstpFtdcOrderActionField req;
			memset(&req,0,sizeof(req));
			((DefOnErrRtnOrderAction)_OnErrRtnOrderAction)(&req,repareInfo(pRspInfo));
		}
		else
			((DefOnErrRtnOrderAction)_OnErrRtnOrderAction)(pOrderAction,repareInfo(pRspInfo));
	}
}

///合约交易状态通知
void CTraderSpi::OnRtnInstrumentStatus(CUstpFtdcInstrumentStatusField *pInstrumentStatus) 
{
	if(_OnRtnInstrumentStatus!=NULL)
	{
		if(pInstrumentStatus == NULL)
		{
			CUstpFtdcInstrumentStatusField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnInstrumentStatus)_OnRtnInstrumentStatus)(&req);
		}
		else
			((DefOnRtnInstrumentStatus)_OnRtnInstrumentStatus)(pInstrumentStatus);
	}
}

///账户出入金回报
void CTraderSpi::OnRtnInvestorAccountDeposit(CUstpFtdcInvestorAccountDepositResField *pInvestorAccountDepositRes)
{
	if(_OnRtnInvestorAccountDeposit!=NULL)
	{
		if(pInvestorAccountDepositRes == NULL)
		{
			CUstpFtdcInvestorAccountDepositResField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnInvestorAccountDeposit)_OnRtnInvestorAccountDeposit)(&req);
		}
		else
			((DefOnRtnInvestorAccountDeposit)_OnRtnInvestorAccountDeposit)(pInvestorAccountDepositRes);
	}
}

///报价回报
void CTraderSpi::OnRtnQuote(CUstpFtdcRtnQuoteField *pRtnQuote)
{
	if(_OnRtnQuote!=NULL)
	{
		if(pRtnQuote == NULL)
		{
			CUstpFtdcRtnQuoteField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnQuote)_OnRtnQuote)(&req);
		}
		else
			((DefOnRtnQuote)_OnRtnQuote)(pRtnQuote);
	}
}

///报价录入错误回报
void CTraderSpi::OnErrRtnQuoteInsert(CUstpFtdcInputQuoteField *pInputQuote, CUstpFtdcRspInfoField *pRspInfo)
{
	if(_OnErrRtnQuoteInsert!=NULL)
	{
		if(pInputQuote == NULL)
		{
			CUstpFtdcInputQuoteField req;
			memset(&req,0,sizeof(req));
			((DefOnErrRtnQuoteInsert)_OnErrRtnQuoteInsert)(&req,repareInfo(pRspInfo));
		}
		else
			((DefOnErrRtnQuoteInsert)_OnErrRtnQuoteInsert)(pInputQuote,repareInfo(pRspInfo));
	}
}

///询价回报
void CTraderSpi::OnRtnForQuote(CUstpFtdcReqForQuoteField *pReqForQuote)
{
	if(_OnRtnQuote!=NULL)
	{
		if(pReqForQuote == NULL)
		{
			CUstpFtdcReqForQuoteField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnForQuote)_OnRtnQuote)(&req);
		}
		else
			((DefOnRtnForQuote)_OnRtnQuote)(pReqForQuote);
	}
}

///增加组合规则通知
void CTraderSpi::OnRtnMarginCombinationLeg(CUstpFtdcMarginCombinationLegField *pMarginCombinationLeg)
{
	if(_OnRtnMarginCombinationLeg!=NULL)
	{
		if(pMarginCombinationLeg == NULL)
		{
			CUstpFtdcMarginCombinationLegField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnMarginCombinationLeg)_OnRtnMarginCombinationLeg)(&req);
		}
		else
			((DefOnRtnMarginCombinationLeg)_OnRtnMarginCombinationLeg)(pMarginCombinationLeg);
	}
}

///客户申请组合确认
void CTraderSpi::OnRtnMarginCombAction(CUstpFtdcInputMarginCombActionField *pInputMarginCombAction)
{
	if(_OnRtnMarginCombAction!=NULL)
	{
		if(pInputMarginCombAction == NULL)
		{
			CUstpFtdcInputMarginCombActionField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnMarginCombAction)_OnRtnMarginCombAction)(&req);
		}
		else
			((DefOnRtnMarginCombAction)_OnRtnMarginCombAction)(pInputMarginCombAction);
	}
}

///报单查询应答
void CTraderSpi::OnRspQryOrder(CUstpFtdcOrderField *pOrder, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryOrder!=NULL)
	{
		if(pOrder == NULL)
		{
			CUstpFtdcOrderField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryOrder)_OnRspQryOrder)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryOrder)_OnRspQryOrder)(pOrder,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///成交单查询应答
void CTraderSpi::OnRspQryTrade(CUstpFtdcTradeField *pTrade, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryTrade!=NULL)
	{
		if(pTrade == NULL)
		{
			CUstpFtdcTradeField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryTrade)_OnRspQryTrade)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryTrade)_OnRspQryTrade)(pTrade,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}


///可用投资者账户查询应答
void CTraderSpi::OnRspQryUserInvestor(CUstpFtdcRspUserInvestorField *pRspUserInvestor, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryUserInvestor!=NULL)
	{
		if(pRspUserInvestor == NULL)
		{
			CUstpFtdcRspUserInvestorField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryUserInvestor)_OnRspQryUserInvestor)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryUserInvestor)_OnRspQryUserInvestor)(pRspUserInvestor,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///交易编码查询应答
void CTraderSpi::OnRspQryTradingCode(CUstpFtdcRspTradingCodeField *pRspTradingCode, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryTradingCode!=NULL)
	{
		if(pRspTradingCode == NULL)
		{
			CUstpFtdcRspTradingCodeField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryTradingCode)_OnRspQryTradingCode)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryTradingCode)_OnRspQryTradingCode)(pRspTradingCode,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///投资者资金账户查询应答
void CTraderSpi::OnRspQryInvestorAccount(CUstpFtdcRspInvestorAccountField *pRspInvestorAccount, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryInvestorAccount!=NULL)
	{
		if(pRspInvestorAccount == NULL)
		{
			CUstpFtdcRspInvestorAccountField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryInvestorAccount)_OnRspQryInvestorAccount)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryInvestorAccount)_OnRspQryInvestorAccount)(pRspInvestorAccount,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///合约查询应答
void CTraderSpi::OnRspQryInstrument(CUstpFtdcRspInstrumentField *pInstrument, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryInstrument!=NULL)
	{
		if(pInstrument == NULL)
		{
			CUstpFtdcRspInstrumentField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryInstrument)_OnRspQryInstrument)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryInstrument)_OnRspQryInstrument)(pInstrument,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///交易所查询应答
void CTraderSpi::OnRspQryExchange(CUstpFtdcRspExchangeField *pRspExchange, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)  
{
	if(_OnRspQryExchange!=NULL)
	{
		if(pRspExchange == NULL)
		{
			CUstpFtdcRspExchangeField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryExchange)_OnRspQryExchange)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryExchange)_OnRspQryExchange)(pRspExchange,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}


///请求查询投资者持仓响应
void CTraderSpi::OnRspQryInvestorPosition(CUstpFtdcRspInvestorPositionField *pInvestorPosition, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryInvestorPosition!=NULL)
	{
		if(pInvestorPosition == NULL)
		{
			CUstpFtdcRspInvestorPositionField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryInvestorPosition)_OnRspQryInvestorPosition)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryInvestorPosition)_OnRspQryInvestorPosition)(pInvestorPosition,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///订阅主题应答
void CTraderSpi::OnRspSubscribeTopic(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspSubscribeTopic!=NULL)
	{
		if(pDissemination == NULL)
		{
			CUstpFtdcDisseminationField req;
			memset(&req,0,sizeof(req));
			((DefOnRspSubscribeTopic)_OnRspSubscribeTopic)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspSubscribeTopic)_OnRspSubscribeTopic)(pDissemination,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///合规参数查询应答
void CTraderSpi::OnRspQryComplianceParam(CUstpFtdcRspComplianceParamField *pRspComplianceParam, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryComplianceParam!=NULL)
	{
		if(pRspComplianceParam == NULL)
		{
			CUstpFtdcRspComplianceParamField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryComplianceParam)_OnRspQryComplianceParam)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryComplianceParam)_OnRspQryComplianceParam)(pRspComplianceParam,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///主题查询应答
void CTraderSpi::OnRspQryTopic(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryTopic!=NULL)
	{
		if(pDissemination == NULL)
		{
			CUstpFtdcDisseminationField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryTopic)_OnRspQryTopic)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryTopic)_OnRspQryTopic)(pDissemination,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///投资者手续费率查询应答
void CTraderSpi::OnRspQryInvestorFee(CUstpFtdcInvestorFeeField *pInvestorFee, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryInvestorFee!=NULL)
	{
		if(pInvestorFee == NULL)
		{
			CUstpFtdcInvestorFeeField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryInvestorFee)_OnRspQryInvestorFee)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryInvestorFee)_OnRspQryInvestorFee)(pInvestorFee,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///投资者保证金率查询应答
void CTraderSpi::OnRspQryInvestorMargin(CUstpFtdcInvestorMarginField *pInvestorMargin, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryInvestorMargin!=NULL)
	{
		if(pInvestorMargin == NULL)
		{
			CUstpFtdcInvestorMarginField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryInvestorMargin)_OnRspQryInvestorMargin)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryInvestorMargin)_OnRspQryInvestorMargin)(pInvestorMargin,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///交易编码组合持仓查询应答
void CTraderSpi::OnRspQryInvestorCombPosition(CUstpFtdcRspInvestorCombPositionField *pRspInvestorCombPosition, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryInvestorCombPosition!=NULL)
	{
		if(pRspInvestorCombPosition == NULL)
		{
			CUstpFtdcRspInvestorCombPositionField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryInvestorCombPosition)_OnRspQryInvestorCombPosition)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryInvestorCombPosition)_OnRspQryInvestorCombPosition)(pRspInvestorCombPosition,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///交易编码单腿持仓查询应答
void CTraderSpi::OnRspQryInvestorLegPosition(CUstpFtdcRspInvestorLegPositionField *pRspInvestorLegPosition, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryInvestorLegPosition!=NULL)
	{
		if(pRspInvestorLegPosition == NULL)
		{
			CUstpFtdcRspInvestorLegPositionField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryInvestorLegPosition)_OnRspQryInvestorLegPosition)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryInvestorLegPosition)_OnRspQryInvestorLegPosition)(pRspInvestorLegPosition,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///交叉汇率查询应答
void CTraderSpi::OnRspQryExchangeRate(CUstpFtdcRspExchangeRateField *pRspExchangeRate, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryExchangeRate!=NULL)
	{
		if(pRspExchangeRate == NULL)
		{
			CUstpFtdcRspExchangeRateField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryExchangeRate)_OnRspQryExchangeRate)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryExchangeRate)_OnRspQryExchangeRate)(pRspExchangeRate,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

//针对收到空反馈的处理
CUstpFtdcRspInfoField rif;
CUstpFtdcRspInfoField* CTraderSpi::repareInfo(CUstpFtdcRspInfoField *pRspInfo)
{
	if(pRspInfo==NULL)
	{
		memset(&rif,0,sizeof(rif));
		return &rif;
	}
	else
		return pRspInfo;
}

