// MdApi.cpp : 定义 DLL 应用程序的导出函数。
//
#include "stdafx.h"
#include "CtpMdUser.h"
#include <iostream>
//#include <vector>		//动态数组,支持赋值
using namespace std;

// 请求编号
int iRequestID = 0;

//连接
MDAPI_API void WINAPI Connect(char* FRONT_ADDR)
{
	// 初始化UserApi
	pUserApi = CThostFtdcMdApi::CreateFtdcMdApi();			// 创建UserApi
	CThostFtdcMdSpi* pUserSpi = new CMdSpi();
	pUserApi->RegisterSpi(pUserSpi);						// 注册事件类
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

//登录
MDAPI_API int WINAPI ReqUserLogin(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID,TThostFtdcPasswordType PASSWORD)
{	
	CThostFtdcReqUserLoginField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.UserID, INVESTOR_ID);
	strcpy_s(req.Password, PASSWORD);
	return pUserApi->ReqUserLogin(&req, ++iRequestID);
}

///登出请求
MDAPI_API int WINAPI ReqUserLogout(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID)
{
	CThostFtdcUserLogoutField req;
	memset(&req,0,sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.UserID,INVESTOR_ID);
	return pUserApi->ReqUserLogout(&req,++iRequestID);
}
//订阅行情
MDAPI_API int WINAPI SubscribeMarketData(char* instrumentsID[],int nCount)
{
	return pUserApi->SubscribeMarketData(instrumentsID,nCount);
}
///退订行情
MDAPI_API int WINAPI UnSubscribeMarketData(char *ppInstrumentID[], int nCount)
{
	return pUserApi->UnSubscribeMarketData(ppInstrumentID, nCount);
}
//订阅询价
MDAPI_API int WINAPI SubscribeForQuoteRsp(char* instrumentsID[],int nCount)
{
	return pUserApi->SubscribeForQuoteRsp(instrumentsID,nCount);
}
///退订询价
MDAPI_API int WINAPI UnSubscribeForQuoteRsp(char *ppInstrumentID[], int nCount)
{
	return pUserApi->UnSubscribeForQuoteRsp(ppInstrumentID, nCount);
}

//============================================ 回调 函数注册 ===========================================

//连接应答
MDAPI_API void WINAPI RegOnFrontConnected(DefOnFrontConnected cb)
{
	_OnFrontConnected=cb;
}

//连接断开
MDAPI_API void WINAPI RegOnFrontDisconnected(DefOnFrontDisconnected cb)
{
	_OnFrontDisconnected=cb;
}

//心跳
MDAPI_API void WINAPI RegOnHeartBeatWarning(DefOnHeartBeatWarning cb)
{
	_OnHeartBeatWarning = cb;
}

//登录请求应答
MDAPI_API void WINAPI RegRspUserLogin(DefOnRspUserLogin cb)
{
	_OnRspUserLogin=cb;
}

//登出请求应答
MDAPI_API void WINAPI RegRspUserLogout(DefOnRspUserLogout cb)
{
	_OnRspUserLogout=cb;
}

///错误应答
MDAPI_API void WINAPI RegRspError(DefOnRspError cb)
{
	_OnRspError= cb;
}

//订阅行情应答
MDAPI_API void WINAPI RegRspSubMarketData(DefOnRspSubMarketData cb)
{
	_OnRspSubMarketData=cb;
}

//退订行情应答
MDAPI_API void WINAPI RegRspUnSubMarketData(DefOnRspUnSubMarketData cb)
{
	_OnRspUnSubMarketData=cb;
}

//深度行情通知
MDAPI_API void WINAPI RegRtnDepthMarketData(DefOnRtnDepthMarketData cb)
{
	_OnRtnDepthMarketData=cb;
}

///订阅询价应答
MDAPI_API void WINAPI RegRspSubForQuoteRsp(DefOnRspSubForQuoteRsp cb)
{
	_OnRspSubForQuoteRsp = cb;
}

///取消订阅询价应答
MDAPI_API void WINAPI RegRspUnSubForQuoteRsp(DefOnRspUnSubForQuoteRsp cb)
{
	_OnRspUnSubForQuoteRsp = cb;
}

///询价通知
MDAPI_API void WINAPI RegRtnForQuoteRsp(DefOnRtnForQuoteRsp cb)
{
	_OnRtnForQuoteRsp = cb;
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
	//cerr << "--->>> " << __FUNCTION__ << endl;
	//cerr << "--->>> Reason = " << nReason << endl;
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

void CMdSpi::OnRspUserLogin(CThostFtdcRspUserLoginField *pRspUserLogin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspUserLogin!=NULL)
	{
		((DefOnRspUserLogin)_OnRspUserLogin)(pRspUserLogin,pRspInfo,nRequestID,bIsLast);
	}
}

void CMdSpi::OnRspUserLogout(CThostFtdcUserLogoutField *pUserLogout, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspUserLogout!=NULL)
	{
		((DefOnRspUserLogout)_OnRspUserLogout)(pUserLogout, pRspInfo, nRequestID, bIsLast);
	}
}

void CMdSpi::OnRspError(CThostFtdcRspInfoField *pRspInfo,int nRequestID, bool bIsLast)
{
	if(_OnRspError != NULL)
	{
		((DefOnRspError)_OnRspError)(pRspInfo, nRequestID, bIsLast);
	}
}

void CMdSpi::OnRspSubMarketData(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspSubMarketData!=NULL)
	{
		((DefOnRspSubMarketData)_OnRspSubMarketData)(pSpecificInstrument,pRspInfo,nRequestID,bIsLast);
	}
}

void CMdSpi::OnRspUnSubMarketData(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspUnSubMarketData!=NULL)
	{
		((DefOnRspUnSubMarketData)_OnRspUnSubMarketData)(pSpecificInstrument, pRspInfo,nRequestID,bIsLast);
	}
}

void CMdSpi::OnRtnDepthMarketData(CThostFtdcDepthMarketDataField *pDepthMarketData)
{
	if(_OnRtnDepthMarketData!=NULL)
	{
		((DefOnRtnDepthMarketData)_OnRtnDepthMarketData)(pDepthMarketData);
	}
}

void CMdSpi::OnRspSubForQuoteRsp(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspSubForQuoteRsp!=NULL)
	{
		((DefOnRspSubForQuoteRsp)_OnRspSubForQuoteRsp)(pSpecificInstrument, pRspInfo,nRequestID,bIsLast);
	}
}

void CMdSpi::OnRspUnSubForQuoteRsp(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspUnSubForQuoteRsp!=NULL)
	{
		((DefOnRspUnSubForQuoteRsp)_OnRspUnSubForQuoteRsp)(pSpecificInstrument, pRspInfo,nRequestID,bIsLast);
	}
}

void CMdSpi::OnRtnForQuoteRsp(CThostFtdcForQuoteRspField *pForQuoteRsp)
{
	if(_OnRtnForQuoteRsp!=NULL)
	{
		((DefOnRtnForQuoteRsp)_OnRtnForQuoteRsp)(pForQuoteRsp);
	}
}
