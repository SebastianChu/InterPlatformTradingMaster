// CtpTrade.cpp : ���� DLL Ӧ�ó���ĵ���������
//

#include "FemasTrader.h"

// ������
int iRequestID = 0;
//����
TRADEAPI_API void WINAPI Connect(char* FRONT_ADDR)
{
	// ��ʼ��UserApi
	pUserApi = CUstpFtdcTraderApi::CreateFtdcTraderApi();			// ����UserApi
	CTraderSpi* pUserSpi = new CTraderSpi();
	pUserApi->RegisterSpi((CUstpFtdcTraderSpi*)pUserSpi);			// ע���¼���
	pUserApi->SubscribePublicTopic(USTP_TERT_QUICK/*USTP_TERT_RESTART*/);					// ע�ṫ����
	pUserApi->SubscribePrivateTopic(USTP_TERT_QUICK/*USTP_TERT_RESTART*/);					// ע��˽����
	pUserApi->RegisterFront(FRONT_ADDR);							// connect
	pUserApi->Init();
	//pUserApi->Join();
}

TRADEAPI_API const char *GetTradingDay()
{
	return pUserApi->GetTradingDay();
}

//�Ͽ�
TRADEAPI_API void WINAPI DisConnect()
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

//���ǰ��ϵͳ�û���¼����
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

//�û��˳�����
TRADEAPI_API int WINAPI ReqUserLogout(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcUserIDType INVESTOR_ID)
{
	CUstpFtdcReqUserLogoutField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.UserID, INVESTOR_ID);
	return pUserApi->ReqUserLogout(&req, ++iRequestID);
}

//�û������޸�����
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

//����¼������
TRADEAPI_API int WINAPI ReqOrderInsert(CUstpFtdcInputOrderField *pOrder)
{
	strcpy_s(pOrder->BusinessUnit,"IPTM_v1.0");
	return pUserApi->ReqOrderInsert(pOrder, ++iRequestID);
}

//������������
TRADEAPI_API int WINAPI ReqOrderAction(CUstpFtdcOrderActionField *pOrder)
{
	return pUserApi->ReqOrderAction(pOrder, ++iRequestID);
}

///����¼������
TRADEAPI_API int WINAPI ReqQuoteInsert(CUstpFtdcInputQuoteField *pInputQuote)
{
	return pUserApi->ReqQuoteInsert(pInputQuote, ++iRequestID);
}

///���۲�������
TRADEAPI_API int WINAPI ReqQuoteAction(CUstpFtdcQuoteActionField *pInputQuoteAction)
{
	return pUserApi->ReqQuoteAction(pInputQuoteAction, ++iRequestID);
}

///�ͻ�ѯ������
TRADEAPI_API int WINAPI ReqForQuote(CUstpFtdcReqForQuoteField *pReqForQuote)
{
	return pUserApi->ReqForQuote(pReqForQuote, ++iRequestID);
}

///�ͻ������������
TRADEAPI_API int WINAPI ReqMarginCombAction(CUstpFtdcInputMarginCombActionField *pInputMarginCombAction)
{
	return pUserApi->ReqMarginCombAction(pInputMarginCombAction, ++iRequestID);
}

///������ѯ����
TRADEAPI_API int WINAPI ReqQryOrder(CUstpFtdcQryOrderField *pQryOrder)
{
	return pUserApi->ReqQryOrder(pQryOrder, ++iRequestID);
}

///�ɽ�����ѯ����
TRADEAPI_API int WINAPI ReqQryTrade(CUstpFtdcQryTradeField *pQryTrade)
{
	return pUserApi->ReqQryTrade(pQryTrade, ++iRequestID);
}

///����Ͷ�����˻���ѯ����
TRADEAPI_API int WINAPI ReqQryUserInvestor(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcUserIDType USER_ID)
{
	CUstpFtdcQryUserInvestorField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID ,BROKER_ID);
	strcpy_s(req.UserID,USER_ID);
	return pUserApi->ReqQryUserInvestor(&req, ++iRequestID);
}

///���ױ����ѯ����
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

//Ͷ�����ʽ��˻���ѯ����
TRADEAPI_API int WINAPI ReqQryInvestorAccount(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcInvestorIDType INVESTOR_ID,TUstpFtdcUserIDType USER_ID)
{
	CUstpFtdcQryInvestorAccountField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.InvestorID, INVESTOR_ID);
	strcpy_s(req.UserID, USER_ID);
	return pUserApi->ReqQryInvestorAccount(&req, ++iRequestID);
}

///��Լ��ѯ����
TRADEAPI_API int WINAPI ReqQryInstrument(TUstpFtdcInstrumentIDType INSTRUMENT_ID)
{
	CUstpFtdcQryInstrumentField req;
	memset(&req, 0, sizeof(req));
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.InstrumentID, INSTRUMENT_ID);
	return pUserApi->ReqQryInstrument(&req, ++iRequestID);
}

///��������ѯ����
TRADEAPI_API int WINAPI ReqQryExchange(TUstpFtdcExchangeIDType EXCHANGE_ID)
{
	CUstpFtdcQryExchangeField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.ExchangeID,EXCHANGE_ID);
	return pUserApi->ReqQryExchange(&req, ++iRequestID);
}

//Ͷ���ֲֲ߳�ѯ����
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

///������������
TRADEAPI_API int WINAPI ReqSubscribeTopic(CUstpFtdcDisseminationField *pDissemination)
{
	return pUserApi->ReqSubscribeTopic(pDissemination, ++iRequestID);
}

///�Ϲ������ѯ����
TRADEAPI_API int WINAPI ReqQryComplianceParam(CUstpFtdcQryComplianceParamField *pQryComplianceParam)
{
	return pUserApi->ReqQryComplianceParam(pQryComplianceParam, ++iRequestID);
}

///�����ѯ����
TRADEAPI_API int WINAPI ReqQryTopic(CUstpFtdcDisseminationField *pDissemination)
{
	return pUserApi->ReqQryTopic(pDissemination, ++iRequestID);
}

///Ͷ�����������ʲ�ѯ����
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

///Ͷ���߱�֤���ʲ�ѯ����
TRADEAPI_API int WINAPI ReqQryInvestorMargin(TUstpFtdcBrokerIDType BROKER_ID,TUstpFtdcInvestorIDType INVESTOR_ID,TUstpFtdcUserIDType USER_ID,TUstpFtdcInstrumentIDType INSTRUMENT_ID,TUstpFtdcExchangeIDType EXCHANGE_ID)
{
	CUstpFtdcQryInvestorMarginField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID ,BROKER_ID);
	if(EXCHANGE_ID != NULL)
		strcpy_s(req.ExchangeID,EXCHANGE_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.InstrumentID,INSTRUMENT_ID);//*��*�ܲ���null�������в�ѯ
	strcpy_s(req.UserID, USER_ID);
	return pUserApi->ReqQryInvestorMargin(&req, ++iRequestID);
}

///���ױ�����ϳֲֲ�ѯ����
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

///���ױ��뵥�ȳֲֲ�ѯ����
TRADEAPI_API int WINAPI ReqQryInvestorLegPosition(CUstpFtdcQryInvestorLegPositionField *pQryInvestorLegPosition)
{
	return pUserApi->ReqQryInvestorLegPosition(pQryInvestorLegPosition, ++iRequestID);
}

///�����ѯ����
TRADEAPI_API int WINAPI ReqQryExchangeRate(CUstpFtdcQryExchangeRateField *pQryExchangeRate)
{
	return pUserApi->ReqQryExchangeRate(pQryExchangeRate, ++iRequestID);
}

///==================================== �ص����� =======================================///
TRADEAPI_API void WINAPI RegOnFrontConnected(DefOnFrontConnected cb)	///���ͻ����뽻�׺�̨������ͨ������ʱ����δ��¼ǰ�����÷��������á�
{	_OnFrontConnected = cb;	}

TRADEAPI_API void WINAPI RegOnFrontDisconnected(DefOnFrontDisconnected cb)	///���ͻ����뽻�׺�̨ͨ�����ӶϿ�ʱ���÷��������á���������������API���Զ��������ӣ��ͻ��˿ɲ�������
{	_OnFrontDisconnected = cb;	}

TRADEAPI_API void WINAPI RegOnHeartBeatWarning(DefOnHeartBeatWarning cb)	///������ʱ���档����ʱ��δ�յ�����ʱ���÷��������á�
{	_OnHeartBeatWarning = cb;	}

TRADEAPI_API void WINAPI RegPackageStart(DefOnPackageStart cb)	///������ʱ���档����ʱ��δ�յ�����ʱ���÷��������á�
{	_OnPackageStart = cb;	}

TRADEAPI_API void WINAPI RegPackageEnd(DefOnPackageEnd cb)	///������ʱ���档����ʱ��δ�յ�����ʱ���÷��������á�
{	_OnPackageEnd = cb;	}

TRADEAPI_API void WINAPI RegRspError(DefOnRspError cb)///����Ӧ��
{	_OnRspError = cb;	}

TRADEAPI_API void WINAPI RegRspUserLogin(DefOnRspUserLogin cb)///���ǰ��ϵͳ�û���¼Ӧ��
{	_OnRspUserLogin = cb;	}

TRADEAPI_API void WINAPI RegRspUserLogout(DefOnRspUserLogout cb)///�û��˳�Ӧ��
{	_OnRspUserLogout = cb;	}

TRADEAPI_API void WINAPI RegRspUserPasswordUpdate(DefOnRspUserPasswordUpdate cb)///�û������޸�Ӧ��
{	_OnRspUserPasswordUpdate = cb;	}

TRADEAPI_API void WINAPI RegRspOrderInsert(DefOnRspOrderInsert cb)///����¼��Ӧ��
{	_OnRspOrderInsert = cb;	}

TRADEAPI_API void WINAPI RegRspOrderAction(DefOnRspOrderAction cb)///��������Ӧ��
{	_OnRspOrderAction = cb;	}

TRADEAPI_API void WINAPI RegRspQuoteInsert(DefOnRspQuoteInsert cb)///����¼��Ӧ��
{	_OnRspQuoteInsert = cb;	}

TRADEAPI_API void WINAPI RegRspQuoteAction(DefOnRspQuoteAction cb)///���۲���Ӧ��
{	_OnRspQuoteAction = cb;	}

TRADEAPI_API void WINAPI RegRspForQuote(DefOnRspForQuote cb)///ѯ������Ӧ��
{	_OnRspForQuote = cb;	}

TRADEAPI_API void WINAPI RegRspMarginCombAction(DefOnRspMarginCombAction cb)///�ͻ��������Ӧ��
{	_OnRspMarginCombAction = cb;	}

TRADEAPI_API void WINAPI RegRtnFlowMessageCancel(DefOnRtnFlowMessageCancel cb)///����������֪ͨ
{	_OnRtnFlowMessageCancel = cb;	}

TRADEAPI_API void WINAPI RegRtnTrade(DefOnRtnTrade cb)///�ɽ�֪ͨ
{	_OnRtnTrade = cb;	}

TRADEAPI_API void WINAPI RegRtnOrder(DefOnRtnOrder cb)///����֪ͨ
{	_OnRtnOrder = cb;	}

TRADEAPI_API void WINAPI RegErrRtnOrderInsert(DefOnErrRtnOrderInsert cb)///����¼�����ر�
{	_OnErrRtnOrderInsert = cb;	}

TRADEAPI_API void WINAPI RegErrRtnOrderAction(DefOnErrRtnOrderAction cb)///������������ر�
{	_OnErrRtnOrderAction = cb;	}

TRADEAPI_API void WINAPI RegRtnInstrumentStatus(DefOnRtnInstrumentStatus cb)///��Լ����״̬֪ͨ
{	_OnRtnInstrumentStatus = cb;	}

TRADEAPI_API void WINAPI RegRtnInvestorAccountDeposit(DefOnRtnInvestorAccountDeposit cb)///�˻������ر�
{	_OnRtnInvestorAccountDeposit = cb;	}

TRADEAPI_API void WINAPI RegRtnQuote(DefOnRtnQuote cb)///���ۻر�
{	_OnRtnQuote = cb;	}

TRADEAPI_API void WINAPI RegErrRtnQuoteInsert(DefOnErrRtnQuoteInsert cb)///����¼�����ر�
{	_OnErrRtnQuoteInsert = cb;	}

TRADEAPI_API void WINAPI RegRtnForQuote(DefOnRtnForQuote cb)///ѯ�ۻر�
{	_OnRtnForQuote = cb;	}

TRADEAPI_API void WINAPI RegRtnMarginCombinationLeg(DefOnRtnMarginCombinationLeg cb)///������Ϲ���֪ͨ
{	_OnRtnMarginCombinationLeg = cb;	}

TRADEAPI_API void WINAPI RegRtnMarginCombAction(DefOnRtnMarginCombAction cb)///�ͻ��������ȷ��
{	_OnRtnMarginCombAction = cb;	}

TRADEAPI_API void WINAPI RegRspQryOrder(DefOnRspQryOrder cb)///������ѯӦ��
{	_OnRspQryOrder = cb;	}

TRADEAPI_API void WINAPI RegRspQryTrade(DefOnRspQryTrade cb)///�ɽ�����ѯӦ��
{	_OnRspQryTrade = cb;	}

TRADEAPI_API void WINAPI RegRspQryUserInvestor(DefOnRspQryUserInvestor cb)///����Ͷ�����˻���ѯӦ��
{	_OnRspQryUserInvestor = cb;	}

TRADEAPI_API void WINAPI RegRspQryTradingCode(DefOnRspQryTradingCode cb)///���ױ����ѯӦ��
{	_OnRspQryTradingCode = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorAccount(DefOnRspQryInvestorAccount cb)///Ͷ�����ʽ��˻���ѯӦ��
{	_OnRspQryInvestorAccount = cb;	}

TRADEAPI_API void WINAPI RegRspQryInstrument(DefOnRspQryInstrument cb)///��Լ��ѯӦ��
{	_OnRspQryInstrument = cb;	}

TRADEAPI_API void WINAPI RegRspQryExchange(DefOnRspQryExchange cb)///��������ѯӦ��
{	_OnRspQryExchange = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorPosition(DefOnRspQryInvestorPosition cb)///Ͷ���ֲֲ߳�ѯӦ��
{	_OnRspQryInvestorPosition = cb;	}

TRADEAPI_API void WINAPI RegRspSubscribeTopic(DefOnRspSubscribeTopic cb)///��������Ӧ��
{	_OnRspSubscribeTopic = cb;	}

TRADEAPI_API void WINAPI RegRspQryComplianceParam(DefOnRspQryComplianceParam cb)///�Ϲ������ѯӦ��
{	_OnRspQryComplianceParam = cb;	}

TRADEAPI_API void WINAPI RegRspQryTopic(DefOnRspQryTopic cb)///Ͷ���ֲֲ߳�ѯ�����ѯӦ��Ӧ��
{	_OnRspQryTopic = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorFee(DefOnRspQryInvestorFee cb)///Ͷ�����������ʲ�ѯӦ��
{	_OnRspQryInvestorFee = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorMargin(DefOnRspQryInvestorMargin cb)///Ͷ���߱�֤���ʲ�ѯӦ��
{	_OnRspQryInvestorMargin = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorCombPosition(DefOnRspQryInvestorCombPosition cb)///���ױ�����ϳֲֲ�ѯӦ��
{	_OnRspQryInvestorCombPosition = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorLegPosition(DefOnRspQryInvestorLegPosition cb)///���ױ��뵥�ȳֲֲ�ѯӦ��
{	_OnRspQryInvestorLegPosition = cb;	}

TRADEAPI_API void WINAPI RegRspQryExchangeRate(DefOnRspQryExchangeRate cb) ///������ʲ�ѯӦ��
{	_OnRspQryExchangeRate = cb;	}


// ������
//extern int iRequestID;

///���ͻ����뽻�׺�̨������ͨ������ʱ����δ��¼ǰ�����÷��������á�
void CTraderSpi::OnFrontConnected()
{
	if(_OnFrontConnected!=NULL)
	{
		((DefOnFrontConnected)_OnFrontConnected)();
	}
}

///���ͻ����뽻�׺�̨ͨ�����ӶϿ�ʱ���÷��������á���������������API���Զ��������ӣ��ͻ��˿ɲ�������
void CTraderSpi::OnFrontDisconnected(int nReason)
{
	if(_OnFrontDisconnected != NULL)
	{
		((DefOnFrontDisconnected)_OnFrontDisconnected)(nReason);
	}
}

///������ʱ���档����ʱ��δ�յ�����ʱ���÷��������á�
void CTraderSpi::OnHeartBeatWarning(int nTimeLapse)
{
	if(_OnHeartBeatWarning != NULL)
	{
		((DefOnHeartBeatWarning)_OnHeartBeatWarning)(nTimeLapse);
	}
}

///���Ļص���ʼ֪ͨ����API�յ�һ�����ĺ����ȵ��ñ�������Ȼ���Ǹ�������Ļص�������Ǳ��Ļص�����֪ͨ��
void CTraderSpi::OnPackageStart(int nTopicID, int nSequenceNo)
{}

//���Ļص�����֪ͨ����API�յ�һ�����ĺ����ȵ��ñ��Ļص���ʼ֪ͨ��Ȼ���Ǹ�������Ļص��������ñ�������
void CTraderSpi::OnPackageEnd(int nTopicID, int nSequenceNo)
{}

///����Ӧ��
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

///���ǰ��ϵͳ�û���¼Ӧ��
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

///�û��˳�Ӧ��
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

///�û������޸�Ӧ��
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

///����¼��Ӧ��
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

///��������Ӧ��
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

///����¼��Ӧ��
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

///���۲���Ӧ��
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

///ѯ������Ӧ��
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

///�ͻ��������Ӧ��
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

///����������֪ͨ
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

///�ɽ��ر�
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

///�����ر�
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
///����¼�����ر�
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

///������������ر�
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

///��Լ����״̬֪ͨ
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

///�˻������ر�
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

///���ۻر�
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

///����¼�����ر�
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

///ѯ�ۻر�
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

///������Ϲ���֪ͨ
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

///�ͻ��������ȷ��
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

///������ѯӦ��
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

///�ɽ�����ѯӦ��
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


///����Ͷ�����˻���ѯӦ��
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

///���ױ����ѯӦ��
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

///Ͷ�����ʽ��˻���ѯӦ��
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

///��Լ��ѯӦ��
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

///��������ѯӦ��
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


///�����ѯͶ���ֲ߳���Ӧ
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

///��������Ӧ��
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

///�Ϲ������ѯӦ��
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

///�����ѯӦ��
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

///Ͷ�����������ʲ�ѯӦ��
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

///Ͷ���߱�֤���ʲ�ѯӦ��
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

///���ױ�����ϳֲֲ�ѯӦ��
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

///���ױ��뵥�ȳֲֲ�ѯӦ��
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

///������ʲ�ѯӦ��
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

//����յ��շ����Ĵ���
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

