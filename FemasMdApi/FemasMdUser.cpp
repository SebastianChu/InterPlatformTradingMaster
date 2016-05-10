// MdApi.cpp : 定义 DLL 应用程序的导出函数。
//
#include "stdafx.h"
#include "FemasMdUser.h"
#include <iostream>
//#include <vector>		//动态数组,支持赋值
using namespace std;

// 请求编号
int iRequestID = 0;

//连接
MDAPI_API void WINAPI Connect(char* FRONT_ADDR)
{
	// 初始化UserApi
	pUserApi = CUstpFtdcMduserApi::CreateFtdcMduserApi();			// 创建UserApi
	CUstpFtdcMduserSpi* pUserSpi = new CMdSpi();
	pUserApi->RegisterSpi(pUserSpi);						// 注册事件类
	pUserApi->SubscribeMarketDataTopic(100, USTP_TERT_QUICK);
	pUserApi->SubscribeMarketDataTopic(301, USTP_TERT_QUICK);
	pUserApi->RegisterFront(FRONT_ADDR);					// connect
	pUserApi->Init();
	//pUserApi->Join();
}

///获取当前交易日:只有登录成功后,才能得到正确的交易日
MDAPI_API const char *GetTradingDay()
{
	return pUserApi->GetTradingDay();
}

MDAPI_API void WINAPI DisConnect()
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

//订阅行情
MDAPI_API int WINAPI SubMarketData(char* instrumentsID[],int nCount)
{
	return pUserApi->SubMarketData(instrumentsID,nCount);
}
///退订行情
MDAPI_API int WINAPI UnSubMarketData(char *ppInstrumentID[], int nCount)
{
	return pUserApi->UnSubMarketData(ppInstrumentID, nCount);
}

///设置心跳超时时间。
///@param timeout 心跳超时时间(秒)  
MDAPI_API void WINAPI SetHeartbeatTimeout(unsigned int timeout)
{
	return pUserApi->SetHeartbeatTimeout(timeout);
}

//风控前置系统用户登录请求
MDAPI_API int WINAPI ReqUserLogin(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcInvestorIDType INVESTOR_ID,TUstpFtdcPasswordType PASSWORD)
{	
	CUstpFtdcReqUserLoginField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.UserID, INVESTOR_ID);
	strcpy_s(req.Password, PASSWORD);
	return pUserApi->ReqUserLogin(&req, ++iRequestID);
}

///用户退出请求
MDAPI_API int WINAPI ReqUserLogout(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcInvestorIDType INVESTOR_ID)
{
	CUstpFtdcReqUserLogoutField req;
	memset(&req,0,sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.UserID,INVESTOR_ID);
	return pUserApi->ReqUserLogout(&req,++iRequestID);
}

///订阅主题请求
MDAPI_API int WINAPI ReqSubscribeTopic(CUstpFtdcDisseminationField *pDissemination)
{
	return pUserApi->ReqSubscribeTopic(pDissemination, ++iRequestID);
}

///主题查询请求
MDAPI_API int WINAPI ReqQryTopic(CUstpFtdcDisseminationField *pDissemination)
{
	return pUserApi->ReqQryTopic(pDissemination, ++iRequestID);
}

///订阅合约的相关信息
MDAPI_API int WINAPI ReqSubMarketData(CUstpFtdcSpecificInstrumentField *pSpecificInstrument)
{
	return pUserApi->ReqSubMarketData(pSpecificInstrument, ++iRequestID);
}

///退订合约的相关信息
MDAPI_API int WINAPI ReqUnSubMarketData(CUstpFtdcSpecificInstrumentField *pSpecificInstrument)
{
	return pUserApi->ReqUnSubMarketData(pSpecificInstrument, ++iRequestID);
}

//============================================ 回调 函数注册 ===========================================

///当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
MDAPI_API void WINAPI RegOnFrontConnected(DefOnFrontConnected cb)
{
	_OnFrontConnected = cb;
}

///当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
MDAPI_API void WINAPI RegOnFrontDisconnected(DefOnFrontDisconnected cb)
{
	_OnFrontDisconnected = cb;
}

///心跳超时警告。当长时间未收到报文时，该方法被调用。
MDAPI_API void WINAPI RegOnHeartBeatWarning(DefOnHeartBeatWarning cb)
{
	_OnHeartBeatWarning = cb;
}

///报文回调开始通知。当API收到一个报文后，首先调用本方法，然后是各数据域的回调，最后是报文回调结束通知。
MDAPI_API void WINAPI RegPackageStart(DefOnPackageStart cb)	
{	
	_OnPackageStart = cb;
}

///报文回调结束通知。当API收到一个报文后，首先调用报文回调开始通知，然后是各数据域的回调，最后调用本方法。
MDAPI_API void WINAPI RegPackageEnd(DefOnPackageEnd cb)	
{
	_OnPackageEnd = cb;	
}

///错误应答
MDAPI_API void WINAPI RegRspError(DefOnRspError cb)
{
	_OnRspError = cb;
}

///风控前置系统用户登录应答
MDAPI_API void WINAPI RegRspUserLogin(DefOnRspUserLogin cb)
{
	_OnRspUserLogin = cb;
}

///用户退出应答
MDAPI_API void WINAPI RegRspUserLogout(DefOnRspUserLogout cb)
{
	_OnRspUserLogout = cb;
}

///订阅主题应答
MDAPI_API void WINAPI RegRspSubscribeTopic(DefOnRspSubscribeTopic cb)
{	
	_OnRspSubscribeTopic = cb;
}

///投资者持仓查询主题查询应答应答
MDAPI_API void WINAPI RegRspQryTopic(DefOnRspQryTopic cb)
{	
	_OnRspQryTopic = cb;
}

//深度行情通知
MDAPI_API void WINAPI RegRtnDepthMarketData(DefOnRtnDepthMarketData cb)
{
	_OnRtnDepthMarketData = cb;
}

//订阅合约的相关信息
MDAPI_API void WINAPI RegRspSubMarketData(DefOnRspSubMarketData cb)
{
	_OnRspSubMarketData = cb;
}

//退订合约的相关信息
MDAPI_API void WINAPI RegRspUnSubMarketData(DefOnRspUnSubMarketData cb)
{
	_OnRspUnSubMarketData = cb;
}

//======================================================================

void CMdSpi::OnFrontConnected()
{
	if(_OnFrontConnected!=NULL)
	{
		((DefOnFrontConnected)_OnFrontConnected)();
	}
}

void CMdSpi::OnFrontDisconnected(int nReason)
{
	if(_OnFrontDisconnected != NULL)
	{
		((DefOnFrontDisconnected)_OnFrontDisconnected)(nReason);
	}
}
		
void CMdSpi::OnHeartBeatWarning(int nTimeLapse)
{
	if(_OnHeartBeatWarning != NULL)
	{
		((DefOnHeartBeatWarning)_OnHeartBeatWarning)(nTimeLapse);
	}
}

void CMdSpi::OnPackageStart(int nTopicID, int nSequenceNo)
{}

void CMdSpi::OnPackageEnd(int nTopicID, int nSequenceNo)
{}

void CMdSpi::OnRspError(CUstpFtdcRspInfoField *pRspInfo,int nRequestID, bool bIsLast)
{
	if(_OnRspError != NULL)
	{
		((DefOnRspError)_OnRspError)(pRspInfo, nRequestID, bIsLast);
	}
}

void CMdSpi::OnRspUserLogin(CUstpFtdcRspUserLoginField *pRspUserLogin, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspUserLogin!=NULL)
	{
		((DefOnRspUserLogin)_OnRspUserLogin)(pRspUserLogin,pRspInfo,nRequestID,bIsLast);
	}
}

void CMdSpi::OnRspUserLogout(CUstpFtdcRspUserLogoutField *pUserLogout, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspUserLogout!=NULL)
	{
		((DefOnRspUserLogout)_OnRspUserLogout)(pUserLogout, pRspInfo, nRequestID, bIsLast);
	}
}

void CMdSpi::OnRspSubscribeTopic(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspSubscribeTopic!=NULL)
	{
		if(pDissemination == NULL)
		{
			CUstpFtdcDisseminationField req;
			memset(&req,0,sizeof(req));
			((DefOnRspSubscribeTopic)_OnRspSubscribeTopic)(&req, pRspInfo, nRequestID, bIsLast);
		}
		else
			((DefOnRspSubscribeTopic)_OnRspSubscribeTopic)(pDissemination, pRspInfo, nRequestID, bIsLast);
	}
}

///主题查询应答
void CMdSpi::OnRspQryTopic(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryTopic!=NULL)
	{
		if(pDissemination == NULL)
		{
			CUstpFtdcDisseminationField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryTopic)_OnRspQryTopic)(&req, pRspInfo, nRequestID, bIsLast);
		}
		else
			((DefOnRspQryTopic)_OnRspQryTopic)(pDissemination, pRspInfo, nRequestID, bIsLast);
	}
}

void CMdSpi::OnRtnDepthMarketData(CUstpFtdcDepthMarketDataField *pDepthMarketData)
{
	if(_OnRtnDepthMarketData!=NULL)
	{
		((DefOnRtnDepthMarketData)_OnRtnDepthMarketData)(pDepthMarketData);
	}
}

void CMdSpi::OnRspSubMarketData(CUstpFtdcSpecificInstrumentField *pSpecificInstrument, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspSubMarketData!=NULL)
	{
		((DefOnRspSubMarketData)_OnRspSubMarketData)(pSpecificInstrument,pRspInfo,nRequestID,bIsLast);
	}
}

void CMdSpi::OnRspUnSubMarketData(CUstpFtdcSpecificInstrumentField *pSpecificInstrument, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspUnSubMarketData!=NULL)
	{
		((DefOnRspUnSubMarketData)_OnRspUnSubMarketData)(pSpecificInstrument, pRspInfo,nRequestID,bIsLast);
	}
}
