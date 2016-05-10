// MdApi.cpp : ���� DLL Ӧ�ó���ĵ���������
//
#include "stdafx.h"
#include "FemasMdUser.h"
#include <iostream>
//#include <vector>		//��̬����,֧�ָ�ֵ
using namespace std;

// ������
int iRequestID = 0;

//����
MDAPI_API void WINAPI Connect(char* FRONT_ADDR)
{
	// ��ʼ��UserApi
	pUserApi = CUstpFtdcMduserApi::CreateFtdcMduserApi();			// ����UserApi
	CUstpFtdcMduserSpi* pUserSpi = new CMdSpi();
	pUserApi->RegisterSpi(pUserSpi);						// ע���¼���
	pUserApi->SubscribeMarketDataTopic(100, USTP_TERT_QUICK);
	pUserApi->SubscribeMarketDataTopic(301, USTP_TERT_QUICK);
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

//��������
MDAPI_API int WINAPI SubMarketData(char* instrumentsID[],int nCount)
{
	return pUserApi->SubMarketData(instrumentsID,nCount);
}
///�˶�����
MDAPI_API int WINAPI UnSubMarketData(char *ppInstrumentID[], int nCount)
{
	return pUserApi->UnSubMarketData(ppInstrumentID, nCount);
}

///����������ʱʱ�䡣
///@param timeout ������ʱʱ��(��)  
MDAPI_API void WINAPI SetHeartbeatTimeout(unsigned int timeout)
{
	return pUserApi->SetHeartbeatTimeout(timeout);
}

//���ǰ��ϵͳ�û���¼����
MDAPI_API int WINAPI ReqUserLogin(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcInvestorIDType INVESTOR_ID,TUstpFtdcPasswordType PASSWORD)
{	
	CUstpFtdcReqUserLoginField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.UserID, INVESTOR_ID);
	strcpy_s(req.Password, PASSWORD);
	return pUserApi->ReqUserLogin(&req, ++iRequestID);
}

///�û��˳�����
MDAPI_API int WINAPI ReqUserLogout(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcInvestorIDType INVESTOR_ID)
{
	CUstpFtdcReqUserLogoutField req;
	memset(&req,0,sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.UserID,INVESTOR_ID);
	return pUserApi->ReqUserLogout(&req,++iRequestID);
}

///������������
MDAPI_API int WINAPI ReqSubscribeTopic(CUstpFtdcDisseminationField *pDissemination)
{
	return pUserApi->ReqSubscribeTopic(pDissemination, ++iRequestID);
}

///�����ѯ����
MDAPI_API int WINAPI ReqQryTopic(CUstpFtdcDisseminationField *pDissemination)
{
	return pUserApi->ReqQryTopic(pDissemination, ++iRequestID);
}

///���ĺ�Լ�������Ϣ
MDAPI_API int WINAPI ReqSubMarketData(CUstpFtdcSpecificInstrumentField *pSpecificInstrument)
{
	return pUserApi->ReqSubMarketData(pSpecificInstrument, ++iRequestID);
}

///�˶���Լ�������Ϣ
MDAPI_API int WINAPI ReqUnSubMarketData(CUstpFtdcSpecificInstrumentField *pSpecificInstrument)
{
	return pUserApi->ReqUnSubMarketData(pSpecificInstrument, ++iRequestID);
}

//============================================ �ص� ����ע�� ===========================================

///���ͻ����뽻�׺�̨������ͨ������ʱ����δ��¼ǰ�����÷��������á�
MDAPI_API void WINAPI RegOnFrontConnected(DefOnFrontConnected cb)
{
	_OnFrontConnected = cb;
}

///���ͻ����뽻�׺�̨ͨ�����ӶϿ�ʱ���÷��������á���������������API���Զ��������ӣ��ͻ��˿ɲ�������
MDAPI_API void WINAPI RegOnFrontDisconnected(DefOnFrontDisconnected cb)
{
	_OnFrontDisconnected = cb;
}

///������ʱ���档����ʱ��δ�յ�����ʱ���÷��������á�
MDAPI_API void WINAPI RegOnHeartBeatWarning(DefOnHeartBeatWarning cb)
{
	_OnHeartBeatWarning = cb;
}

///���Ļص���ʼ֪ͨ����API�յ�һ�����ĺ����ȵ��ñ�������Ȼ���Ǹ�������Ļص�������Ǳ��Ļص�����֪ͨ��
MDAPI_API void WINAPI RegPackageStart(DefOnPackageStart cb)	
{	
	_OnPackageStart = cb;
}

///���Ļص�����֪ͨ����API�յ�һ�����ĺ����ȵ��ñ��Ļص���ʼ֪ͨ��Ȼ���Ǹ�������Ļص��������ñ�������
MDAPI_API void WINAPI RegPackageEnd(DefOnPackageEnd cb)	
{
	_OnPackageEnd = cb;	
}

///����Ӧ��
MDAPI_API void WINAPI RegRspError(DefOnRspError cb)
{
	_OnRspError = cb;
}

///���ǰ��ϵͳ�û���¼Ӧ��
MDAPI_API void WINAPI RegRspUserLogin(DefOnRspUserLogin cb)
{
	_OnRspUserLogin = cb;
}

///�û��˳�Ӧ��
MDAPI_API void WINAPI RegRspUserLogout(DefOnRspUserLogout cb)
{
	_OnRspUserLogout = cb;
}

///��������Ӧ��
MDAPI_API void WINAPI RegRspSubscribeTopic(DefOnRspSubscribeTopic cb)
{	
	_OnRspSubscribeTopic = cb;
}

///Ͷ���ֲֲ߳�ѯ�����ѯӦ��Ӧ��
MDAPI_API void WINAPI RegRspQryTopic(DefOnRspQryTopic cb)
{	
	_OnRspQryTopic = cb;
}

//�������֪ͨ
MDAPI_API void WINAPI RegRtnDepthMarketData(DefOnRtnDepthMarketData cb)
{
	_OnRtnDepthMarketData = cb;
}

//���ĺ�Լ�������Ϣ
MDAPI_API void WINAPI RegRspSubMarketData(DefOnRspSubMarketData cb)
{
	_OnRspSubMarketData = cb;
}

//�˶���Լ�������Ϣ
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

///�����ѯӦ��
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
