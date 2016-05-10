// MdApi.cpp : ���� DLL Ӧ�ó���ĵ���������
//
#include "stdafx.h"
#include "CtpMdUser.h"
#include <iostream>
//#include <vector>		//��̬����,֧�ָ�ֵ
using namespace std;

// ������
int iRequestID = 0;

//����
MDAPI_API void WINAPI Connect(char* FRONT_ADDR)
{
	// ��ʼ��UserApi
	pUserApi = CThostFtdcMdApi::CreateFtdcMdApi();			// ����UserApi
	CThostFtdcMdSpi* pUserSpi = new CMdSpi();
	pUserApi->RegisterSpi(pUserSpi);						// ע���¼���
	pUserApi->RegisterFront(FRONT_ADDR);					// connect
	pUserApi->Init();
	//pUserApi->Join();
}

///��ȡ��ǰ������:ֻ�е�¼�ɹ���,���ܵõ���ȷ�Ľ�����
MDAPI_API const char *GetTradingDay()
{
	return pUserApi->GetTradingDay();
}

MDAPI_API void WINAPI DisConnect()
{
	// �ͷ�UserApi
	if (pUserApi)
	{
		pUserApi->RegisterSpi(NULL);
		pUserApi->Release();
		pUserApi = NULL;
	}
	// �ͷ�UserSpiʵ��
	/*if (m_pUserSpiImpl)
	{
		delete m_pUserSpiImpl;
		m_pUserSpiImpl = NULL;
	}*/
}

//��¼
MDAPI_API int WINAPI ReqUserLogin(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID,TThostFtdcPasswordType PASSWORD)
{	
	CThostFtdcReqUserLoginField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.UserID, INVESTOR_ID);
	strcpy_s(req.Password, PASSWORD);
	return pUserApi->ReqUserLogin(&req, ++iRequestID);
}

///�ǳ�����
MDAPI_API int WINAPI ReqUserLogout(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID)
{
	CThostFtdcUserLogoutField req;
	memset(&req,0,sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.UserID,INVESTOR_ID);
	return pUserApi->ReqUserLogout(&req,++iRequestID);
}
//��������
MDAPI_API int WINAPI SubscribeMarketData(char* instrumentsID[],int nCount)
{
	return pUserApi->SubscribeMarketData(instrumentsID,nCount);
}
///�˶�����
MDAPI_API int WINAPI UnSubscribeMarketData(char *ppInstrumentID[], int nCount)
{
	return pUserApi->UnSubscribeMarketData(ppInstrumentID, nCount);
}
//����ѯ��
MDAPI_API int WINAPI SubscribeForQuoteRsp(char* instrumentsID[],int nCount)
{
	return pUserApi->SubscribeForQuoteRsp(instrumentsID,nCount);
}
///�˶�ѯ��
MDAPI_API int WINAPI UnSubscribeForQuoteRsp(char *ppInstrumentID[], int nCount)
{
	return pUserApi->UnSubscribeForQuoteRsp(ppInstrumentID, nCount);
}

//============================================ �ص� ����ע�� ===========================================

//����Ӧ��
MDAPI_API void WINAPI RegOnFrontConnected(DefOnFrontConnected cb)
{
	_OnFrontConnected=cb;
}

//���ӶϿ�
MDAPI_API void WINAPI RegOnFrontDisconnected(DefOnFrontDisconnected cb)
{
	_OnFrontDisconnected=cb;
}

//����
MDAPI_API void WINAPI RegOnHeartBeatWarning(DefOnHeartBeatWarning cb)
{
	_OnHeartBeatWarning = cb;
}

//��¼����Ӧ��
MDAPI_API void WINAPI RegRspUserLogin(DefOnRspUserLogin cb)
{
	_OnRspUserLogin=cb;
}

//�ǳ�����Ӧ��
MDAPI_API void WINAPI RegRspUserLogout(DefOnRspUserLogout cb)
{
	_OnRspUserLogout=cb;
}

///����Ӧ��
MDAPI_API void WINAPI RegRspError(DefOnRspError cb)
{
	_OnRspError= cb;
}

//��������Ӧ��
MDAPI_API void WINAPI RegRspSubMarketData(DefOnRspSubMarketData cb)
{
	_OnRspSubMarketData=cb;
}

//�˶�����Ӧ��
MDAPI_API void WINAPI RegRspUnSubMarketData(DefOnRspUnSubMarketData cb)
{
	_OnRspUnSubMarketData=cb;
}

//�������֪ͨ
MDAPI_API void WINAPI RegRtnDepthMarketData(DefOnRtnDepthMarketData cb)
{
	_OnRtnDepthMarketData=cb;
}

///����ѯ��Ӧ��
MDAPI_API void WINAPI RegRspSubForQuoteRsp(DefOnRspSubForQuoteRsp cb)
{
	_OnRspSubForQuoteRsp = cb;
}

///ȡ������ѯ��Ӧ��
MDAPI_API void WINAPI RegRspUnSubForQuoteRsp(DefOnRspUnSubForQuoteRsp cb)
{
	_OnRspUnSubForQuoteRsp = cb;
}

///ѯ��֪ͨ
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
