// CtpTrade.cpp : ���� DLL Ӧ�ó���ĵ���������
//

#include "CtpTrader.h"

// ������
int iRequestID = 0;
//����
TRADEAPI_API void WINAPI Connect(char* FRONT_ADDR)
{
	// ��ʼ��UserApi
	pUserApi = CThostFtdcTraderApi::CreateFtdcTraderApi();			// ����UserApi
	CTraderSpi* pUserSpi = new CTraderSpi();
	pUserApi->RegisterSpi((CThostFtdcTraderSpi*)pUserSpi);			// ע���¼���
	pUserApi->SubscribePublicTopic(THOST_TERT_QUICK/*THOST_TERT_RESTART*/);					// ע�ṫ����
	pUserApi->SubscribePrivateTopic(THOST_TERT_QUICK/*THOST_TERT_RESTART*/);					// ע��˽����
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

//�����û���¼����
TRADEAPI_API int WINAPI ReqUserLogin(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcUserIDType USER_ID,TThostFtdcPasswordType PASSWORD)
{
	CThostFtdcReqUserLoginField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.UserID, USER_ID);
	strcpy_s(req.Password, PASSWORD);
	strcpy_s(req.UserProductInfo,"IPTM_v1.0");
	return pUserApi->ReqUserLogin(&req, ++iRequestID);
}

//���͵ǳ�����
TRADEAPI_API int WINAPI ReqUserLogout(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcUserIDType INVESTOR_ID)
{
	CThostFtdcUserLogoutField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.UserID, INVESTOR_ID);
	return pUserApi->ReqUserLogout(&req, ++iRequestID);
}

//�����û�����
TRADEAPI_API int WINAPI ReqUserPasswordUpdate(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcUserIDType USER_ID,TThostFtdcUserIDType OLD_PASSWORD,TThostFtdcPasswordType NEW_PASSWORD)
{
	CThostFtdcUserPasswordUpdateField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.UserID, USER_ID);
	strcpy_s(req.OldPassword,OLD_PASSWORD);
	strcpy_s(req.NewPassword,NEW_PASSWORD);
	return pUserApi->ReqUserPasswordUpdate(&req, ++iRequestID);
}

///�ʽ��˻������������
TRADEAPI_API int WINAPI ReqTradingAccountPasswordUpdate(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcAccountIDType ACCOUNT_ID,TThostFtdcUserIDType OLD_PASSWORD,TThostFtdcPasswordType NEW_PASSWORD)
{
	CThostFtdcTradingAccountPasswordUpdateField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.AccountID, ACCOUNT_ID);
	strcpy_s(req.NewPassword,NEW_PASSWORD);
	strcpy_s(req.OldPassword,OLD_PASSWORD);
	return pUserApi->ReqTradingAccountPasswordUpdate(&req, ++iRequestID);
}

//����¼������
TRADEAPI_API int WINAPI ReqOrderInsert(CThostFtdcInputOrderField *pOrder)
{
	strcpy_s(pOrder->BusinessUnit,"IPTM_v1.0");
	return pUserApi->ReqOrderInsert(pOrder, ++iRequestID);
}

//������������
TRADEAPI_API int WINAPI ReqOrderAction(CThostFtdcInputOrderActionField *pOrder)
{
	return pUserApi->ReqOrderAction(pOrder, ++iRequestID);
}

///��ѯ��󱨵���������
TRADEAPI_API int WINAPI ReqQueryMaxOrderVolume(CThostFtdcQueryMaxOrderVolumeField *pMaxOrderVolume)
{
	return pUserApi->ReqQueryMaxOrderVolume(pMaxOrderVolume, ++iRequestID);
}

//Ͷ���߽�����ȷ��
TRADEAPI_API int WINAPI ReqSettlementInfoConfirm(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID)
{
	CThostFtdcSettlementInfoConfirmField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.InvestorID, INVESTOR_ID);
	return pUserApi->ReqSettlementInfoConfirm(&req, ++iRequestID);
}

///�����ѯ����
TRADEAPI_API int WINAPI ReqQryOrder(CThostFtdcQryOrderField *pQryOrder)
{
	return pUserApi->ReqQryOrder(pQryOrder, ++iRequestID);
}

///�����ѯ�ɽ�
TRADEAPI_API int WINAPI ReqQryTrade(CThostFtdcQryTradeField *pQryTrade)
{
	return pUserApi->ReqQryTrade(pQryTrade, ++iRequestID);
}

//�����ѯͶ���ֲ߳�
TRADEAPI_API int WINAPI ReqQryInvestorPosition(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID,TThostFtdcInstrumentIDType INSTRUMENT_ID)
{
	CThostFtdcQryInvestorPositionField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.InvestorID, INVESTOR_ID);
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.InstrumentID, INSTRUMENT_ID);
	return pUserApi->ReqQryInvestorPosition(&req, ++iRequestID);
}

//�����ѯ�ʽ��˻�
TRADEAPI_API int WINAPI ReqQryTradingAccount(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID)
{
	CThostFtdcQryTradingAccountField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.InvestorID, INVESTOR_ID);
	return pUserApi->ReqQryTradingAccount(&req, ++iRequestID);
}

///�����ѯͶ����
TRADEAPI_API int WINAPI ReqQryInvestor(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID)
{
	CThostFtdcQryInvestorField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID ,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	return pUserApi->ReqQryInvestor(&req, ++iRequestID);
}

///�����ѯ���ױ���
TRADEAPI_API int WINAPI ReqQryTradingCode(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID,TThostFtdcClientIDType CLIENT_ID,TThostFtdcExchangeIDType	EXCHANGE_ID)
{
	CThostFtdcQryTradingCodeField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID ,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	if(CLIENT_ID != NULL)
		strcpy_s(req.ClientID,CLIENT_ID);
	if(EXCHANGE_ID != NULL)
		strcpy_s(req.ExchangeID,EXCHANGE_ID);
	return pUserApi->ReqQryTradingCode(&req, ++iRequestID);
}

///�����ѯ��Լ��֤����
TRADEAPI_API int WINAPI ReqQryInstrumentMarginRate(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID,TThostFtdcInstrumentIDType INSTRUMENT_ID,TThostFtdcHedgeFlagType HEDGE_FLAG)
{
	CThostFtdcQryInstrumentMarginRateField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID ,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.InstrumentID,INSTRUMENT_ID);
	if(HEDGE_FLAG != NULL)
		req.HedgeFlag = HEDGE_FLAG;						//*��*�ܲ���null�������в�ѯ
	return pUserApi->ReqQryInstrumentMarginRate(&req, ++iRequestID);
}

///�����ѯ��Լ��������
TRADEAPI_API int WINAPI ReqQryInstrumentCommissionRate(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID,TThostFtdcInstrumentIDType INSTRUMENT_ID)
{
	CThostFtdcQryInstrumentCommissionRateField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID ,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);	
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.InstrumentID,INSTRUMENT_ID);
	return pUserApi->ReqQryInstrumentCommissionRate(&req, ++iRequestID);
}

///�����ѯ��Ȩ��Լ��������
TRADEAPI_API int WINAPI ReqQryOptionInstrCommRate(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID,TThostFtdcInstrumentIDType INSTRUMENT_ID)
{
	CThostFtdcQryOptionInstrCommRateField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID ,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);	
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.InstrumentID,INSTRUMENT_ID);
	return pUserApi->ReqQryOptionInstrCommRate(&req, ++iRequestID);
}

///�����ѯ������
TRADEAPI_API int WINAPI ReqQryExchange(TThostFtdcExchangeIDType EXCHANGE_ID)
{
	CThostFtdcQryExchangeField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.ExchangeID,EXCHANGE_ID);
	return pUserApi->ReqQryExchange(&req, ++iRequestID);
}

///�����ѯ��Լ
TRADEAPI_API int WINAPI ReqQryInstrument(TThostFtdcInstrumentIDType INSTRUMENT_ID)
{
	CThostFtdcQryInstrumentField req;
	memset(&req, 0, sizeof(req));
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.InstrumentID, INSTRUMENT_ID);
	return pUserApi->ReqQryInstrument(&req, ++iRequestID);
}

///�����ѯ����
TRADEAPI_API int WINAPI ReqQryDepthMarketData(TThostFtdcInstrumentIDType INSTRUMENT_ID)
{
	CThostFtdcQryDepthMarketDataField req;
	memset(&req,0,sizeof(req));
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.InstrumentID, INSTRUMENT_ID);
	return pUserApi->ReqQryDepthMarketData(&req, ++iRequestID);
}

///�����ѯͶ���߽�����
TRADEAPI_API int WINAPI ReqQrySettlementInfo(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID,TThostFtdcDateType TRADING_DAY)
{
	CThostFtdcQrySettlementInfoField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID ,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);	
	if(TRADING_DAY != NULL)
		strcpy_s(req.TradingDay, TRADING_DAY);
	return pUserApi->ReqQrySettlementInfo(&req, ++iRequestID);
}

///��ѯ�ֲ���ϸ
TRADEAPI_API int WINAPI ReqQryInvestorPositionDetail(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID,TThostFtdcInstrumentIDType INSTRUMENT_ID)
{
	CThostFtdcQryInvestorPositionDetailField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.InstrumentID, INSTRUMENT_ID);
	return pUserApi->ReqQryInvestorPositionDetail(&req, ++iRequestID);
}

///�����ѯ�ͻ�֪ͨ
TRADEAPI_API int WINAPI ReqQryNotice(TThostFtdcBrokerIDType BROKERID)
{
	CThostFtdcQryNoticeField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKERID);
	return pUserApi->ReqQryNotice(&req, ++iRequestID);
}

///�����ѯ������Ϣȷ��
TRADEAPI_API int WINAPI ReqQrySettlementInfoConfirm(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID)
{
	CThostFtdcQrySettlementInfoConfirmField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	return pUserApi->ReqQrySettlementInfoConfirm(&req, ++iRequestID);
}

///�����ѯ**���**�ֲ���ϸ
TRADEAPI_API int WINAPI ReqQryInvestorPositionCombineDetail(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID,TThostFtdcInstrumentIDType INSTRUMENT_ID)
{
	CThostFtdcQryInvestorPositionCombineDetailField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.CombInstrumentID, INSTRUMENT_ID);
	return pUserApi->ReqQryInvestorPositionCombineDetail(&req, ++iRequestID);
}

///�����ѯ��֤����ϵͳ���͹�˾�ʽ��˻���Կ
TRADEAPI_API int WINAPI ReqQryCFMMCTradingAccountKey(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID)
{
	CThostFtdcQryCFMMCTradingAccountKeyField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	return pUserApi->ReqQryCFMMCTradingAccountKey(&req, ++iRequestID);
}

///�����ѯ����֪ͨ
TRADEAPI_API int WINAPI ReqQryTradingNotice(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID)
{
	CThostFtdcQryTradingNoticeField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	return pUserApi->ReqQryTradingNotice(&req, ++iRequestID);
}

///�����ѯ���͹�˾���ײ���
TRADEAPI_API int WINAPI ReqQryBrokerTradingParams(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID)
{
	CThostFtdcQryBrokerTradingParamsField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	return pUserApi->ReqQryBrokerTradingParams(&req, ++iRequestID);
}

///�����ѯ���͹�˾�����㷨
TRADEAPI_API int WINAPI ReqQryBrokerTradingAlgos(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcExchangeIDType EXCHANGE_ID,TThostFtdcInstrumentIDType INSTRUMENT_ID)
{
	CThostFtdcQryBrokerTradingAlgosField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	if(EXCHANGE_ID != NULL)
		strcpy_s(req.ExchangeID, EXCHANGE_ID);
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.InstrumentID, INSTRUMENT_ID);

	return pUserApi->ReqQryBrokerTradingAlgos(&req, ++iRequestID);
}

///Ԥ��¼������
TRADEAPI_API int WINAPI ReqParkedOrderInsert(CThostFtdcParkedOrderField *ParkedOrder)
{
	return pUserApi->ReqParkedOrderInsert(ParkedOrder, ++iRequestID);
}

///Ԥ�񳷵�¼������
TRADEAPI_API int WINAPI ReqParkedOrderAction(CThostFtdcParkedOrderActionField *ParkedOrderAction)
{
	return pUserApi->ReqParkedOrderAction(ParkedOrderAction, ++iRequestID);
}

///����ɾ��Ԥ��
TRADEAPI_API int WINAPI ReqRemoveParkedOrder(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID,TThostFtdcParkedOrderIDType ParkedOrder_ID)
{
	CThostFtdcRemoveParkedOrderField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	strcpy_s(req.ParkedOrderID,ParkedOrder_ID);
	return pUserApi->ReqRemoveParkedOrder(&req, ++iRequestID);
}

///����ɾ��Ԥ�񳷵�
TRADEAPI_API int WINAPI ReqRemoveParkedOrderAction(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID,TThostFtdcParkedOrderActionIDType ParkedOrderAction_ID)
{
	CThostFtdcRemoveParkedOrderActionField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	strcpy_s(req.ParkedOrderActionID,ParkedOrderAction_ID);
	return pUserApi->ReqRemoveParkedOrderAction(&req, ++iRequestID);
}

///�����ѯ����ǩԼ��ϵ
TRADEAPI_API int WINAPI ReqQryAccountregister(TThostFtdcBrokerIDType Broker_ID, TThostFtdcAccountIDType Account_ID)
{
	CThostFtdcQryAccountregisterField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,Broker_ID);
	strcpy_s(req.AccountID,Account_ID);
	return pUserApi->ReqQryAccountregister(&req, ++iRequestID);
}

///�����ѯת������
TRADEAPI_API int WINAPI ReqQryTransferBank(TThostFtdcBankIDType Bank_ID,	TThostFtdcBankBrchIDType BankBrch_ID)
{
	CThostFtdcQryTransferBankField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BankID,Bank_ID);
	strcpy_s(req.BankBrchID,BankBrch_ID);
	return pUserApi->ReqQryTransferBank(&req, ++iRequestID);
}

///�����ѯת����ˮ
TRADEAPI_API int WINAPI ReqQryTransferSerial(TThostFtdcBrokerIDType Broker_ID,TThostFtdcAccountIDType Account_ID,TThostFtdcBankIDType Bank_ID)
{ 
	CThostFtdcQryTransferSerialField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,Broker_ID);
	strcpy_s(req.AccountID,Account_ID);
	strcpy_s(req.BankID,Bank_ID);
	return pUserApi->ReqQryTransferSerial(&req, ++iRequestID);
}

///�����ѯǩԼ����
TRADEAPI_API int WINAPI ReqQryContractBank(TThostFtdcBrokerIDType Broker_ID,TThostFtdcBankIDType Bank_ID,	TThostFtdcBankBrchIDType BankBrch_ID)
{
	CThostFtdcQryContractBankField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,Broker_ID);
	if(Bank_ID != NULL)
		strcpy_s(req.BankID,Bank_ID);
	if(BankBrch_ID !=NULL)
		strcpy_s(req.BankBrchID,BankBrch_ID);
	return pUserApi->ReqQryContractBank(&req, ++iRequestID);
}

///�����ѯԤ��
TRADEAPI_API int WINAPI ReqQryParkedOrder(TThostFtdcBrokerIDType BrokerID,TThostFtdcInvestorIDType InvestorID,TThostFtdcInstrumentIDType InstrumentID,TThostFtdcExchangeIDType ExchangeID)
{
	CThostFtdcQryParkedOrderField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BrokerID);
	strcpy_s(req.InvestorID,InvestorID);
	if(InstrumentID != NULL)
		strcpy_s(req.InstrumentID,InstrumentID);
	if(ExchangeID != NULL)
		strcpy_s(req.ExchangeID,ExchangeID);
	return pUserApi->ReqQryParkedOrder(&req, ++iRequestID);
}

///�����ѯԤ�񳷵�
TRADEAPI_API int WINAPI ReqQryParkedOrderAction(TThostFtdcBrokerIDType BrokerID,TThostFtdcInvestorIDType InvestorID,TThostFtdcInstrumentIDType InstrumentID,TThostFtdcExchangeIDType ExchangeID)
{
	CThostFtdcQryParkedOrderActionField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BrokerID);
	strcpy_s(req.InvestorID,InvestorID);
	if(InstrumentID != NULL)
		strcpy_s(req.InstrumentID,InstrumentID);
	if(ExchangeID != NULL)
		strcpy_s(req.ExchangeID,ExchangeID);
	return pUserApi->ReqQryParkedOrderAction(&req, ++iRequestID);
}

///�ڻ����������ʽ�ת�ڻ�����
TRADEAPI_API int WINAPI ReqFromBankToFutureByFuture(CThostFtdcReqTransferField *ReqTransfer)
{
	return pUserApi->ReqFromBankToFutureByFuture(ReqTransfer, ++iRequestID);
}

///�ڻ������ڻ��ʽ�ת��������
TRADEAPI_API int WINAPI ReqFromFutureToBankByFuture(CThostFtdcReqTransferField *ReqTransfer)
{
	return pUserApi->ReqFromFutureToBankByFuture(ReqTransfer, ++iRequestID);
}

///�ڻ������ѯ�����������
TRADEAPI_API int WINAPI ReqQueryBankAccountMoneyByFuture(CThostFtdcReqQueryAccountField *ReqQueryAccount)
{
	return pUserApi->ReqQueryBankAccountMoneyByFuture(ReqQueryAccount, ++iRequestID);
}

///�����ѯ��Ȩ���׳ɱ�
TRADEAPI_API int WINAPI ReqQryOptionInstrTradeCost(CThostFtdcQryOptionInstrTradeCostField *pQryOptionInstrTradeCost)
{
	return pUserApi->ReqQryOptionInstrTradeCost(pQryOptionInstrTradeCost, ++iRequestID);
}

///�����ѯͶ����Ʒ��/��Ʒ�ֱ�֤��
TRADEAPI_API int WINAPI ReqQryInvestorProductGroupMargin(CThostFtdcQryInvestorProductGroupMarginField *pQryInvestorProductGroupMargin) 
{
	return pUserApi->ReqQryInvestorProductGroupMargin(pQryInvestorProductGroupMargin, ++iRequestID);
}

///�����ѯ������������֤����
TRADEAPI_API int WINAPI ReqQryExchangeMarginRateAdjust(CThostFtdcQryExchangeMarginRateAdjustField *pQryExchangeMarginRateAdjust)
{
	return pUserApi->ReqQryExchangeMarginRateAdjust(pQryExchangeMarginRateAdjust, ++iRequestID);
}

///�����ѯ����
TRADEAPI_API int WINAPI ReqQryExchangeRate(CThostFtdcQryExchangeRateField *pQryExchangeRate)
{
	return pUserApi->ReqQryExchangeRate(pQryExchangeRate, ++iRequestID);
}

///�����ѯ��Ʒ���ۻ���
TRADEAPI_API int WINAPI ReqQryProductExchRate(CThostFtdcQryProductExchRateField *pQryProductExchRate)
{
	return pUserApi->ReqQryProductExchRate(pQryProductExchRate, ++iRequestID);
}

///�����ѯѯ��
TRADEAPI_API int WINAPI ReqQryForQuote(CThostFtdcQryForQuoteField *pQryForQuote)
{
	return pUserApi->ReqQryForQuote(pQryForQuote, ++iRequestID);
}

///ѯ��¼������
TRADEAPI_API int WINAPI ReqForQuoteInsert(CThostFtdcInputForQuoteField *pInputForQuote)
{
	return pUserApi->ReqForQuoteInsert(pInputForQuote, ++iRequestID);
}

///�����ѯ����
TRADEAPI_API int WINAPI ReqQryQuote(CThostFtdcQryQuoteField *pQryQuote)
{
	return pUserApi->ReqQryQuote(pQryQuote, ++iRequestID);
}

///����¼������
TRADEAPI_API int WINAPI ReqQuoteInsert(CThostFtdcInputQuoteField *pInputQuote)
{
	return pUserApi->ReqQuoteInsert(pInputQuote, ++iRequestID);
}

///���۲�������
TRADEAPI_API int WINAPI ReqQuoteAction(CThostFtdcInputQuoteActionField *pInputQuoteAction)
{
	return pUserApi->ReqQuoteAction(pInputQuoteAction, ++iRequestID);
}

///�����ѯִ������
TRADEAPI_API int WINAPI ReqQryExecOrder(CThostFtdcQryExecOrderField *pQryExecOrder)
{
	return pUserApi->ReqQryExecOrder(pQryExecOrder, ++iRequestID);
}

///ִ������¼������
TRADEAPI_API int WINAPI ReqExecOrderInsert(CThostFtdcInputExecOrderField *pInputExecOrder)
{
	return pUserApi->ReqExecOrderInsert(pInputExecOrder, ++iRequestID);
}

///ִ�������������
TRADEAPI_API int WINAPI ReqExecOrderAction(CThostFtdcInputExecOrderActionField *pInputExecOrderAction)
{
	return pUserApi->ReqExecOrderAction(pInputExecOrderAction, ++iRequestID);
}

///�������¼������
TRADEAPI_API int WINAPI ReqCombActionInsert(CThostFtdcInputCombActionField *pInputCombAction)
{
	return pUserApi->ReqCombActionInsert(pInputCombAction, ++iRequestID);
}

///�����ѯ��Ϻ�Լ��ȫϵ��
TRADEAPI_API int WINAPI ReqQryCombInstrumentGuard(CThostFtdcQryCombInstrumentGuardField *pQryCombInstrumentGuard)
{
	return pUserApi->ReqQryCombInstrumentGuard(pQryCombInstrumentGuard, ++iRequestID);
}

///�����ѯ�������
TRADEAPI_API int WINAPI ReqQryCombAction(CThostFtdcQryCombActionField *pQryCombAction)
{
	return pUserApi->ReqQryCombAction(pQryCombAction, ++iRequestID);
}


///==================================== �ص����� =======================================///
TRADEAPI_API void WINAPI RegOnFrontConnected(DefOnFrontConnected cb)	///���ͻ����뽻�׺�̨������ͨ������ʱ����δ��¼ǰ�����÷��������á�
{	_OnFrontConnected = cb;	}

TRADEAPI_API void WINAPI RegOnFrontDisconnected(DefOnFrontDisconnected cb)	///���ͻ����뽻�׺�̨ͨ�����ӶϿ�ʱ���÷��������á���������������API���Զ��������ӣ��ͻ��˿ɲ�������
{	_OnFrontDisconnected = cb;	}

TRADEAPI_API void WINAPI RegOnHeartBeatWarning(DefOnHeartBeatWarning cb)	///������ʱ���档����ʱ��δ�յ�����ʱ���÷��������á�
{	_OnHeartBeatWarning = cb;	}

TRADEAPI_API void WINAPI RegRspUserLogin(DefOnRspUserLogin cb)///��¼������Ӧ
{	_OnRspUserLogin = cb;	}

TRADEAPI_API void WINAPI RegRspUserLogout(DefOnRspUserLogout cb)///�ǳ�������Ӧ
{	_OnRspUserLogout = cb;	}

TRADEAPI_API void WINAPI RegRspUserPasswordUpdate(DefOnRspUserPasswordUpdate cb)///�û��������������Ӧ
{	_OnRspUserPasswordUpdate = cb;	}

TRADEAPI_API void WINAPI RegRspTradingAccountPasswordUpdate(DefOnRspTradingAccountPasswordUpdate cb)///�ʽ��˻��������������Ӧ
{	_OnRspTradingAccountPasswordUpdate = cb;	}

TRADEAPI_API void WINAPI RegRspOrderInsert(DefOnRspOrderInsert cb)///����¼��������Ӧ
{	_OnRspOrderInsert = cb;	}

TRADEAPI_API void WINAPI RegRspParkedOrderInsert(DefOnRspParkedOrderInsert cb)///Ԥ��¼��������Ӧ
{	_OnRspParkedOrderInsert = cb;	}

TRADEAPI_API void WINAPI RegRspParkedOrderAction(DefOnRspParkedOrderAction cb)///Ԥ�񳷵�¼��������Ӧ
{	_OnRspParkedOrderAction = cb;	}

TRADEAPI_API void WINAPI RegRspOrderAction(DefOnRspOrderAction cb)///��������������Ӧ
{	_OnRspOrderAction = cb;	}

TRADEAPI_API void WINAPI RegRspQueryMaxOrderVolume(DefOnRspQueryMaxOrderVolume cb)///��ѯ��󱨵�������Ӧ
{	_OnRspQueryMaxOrderVolume = cb;	}

TRADEAPI_API void WINAPI RegRspSettlementInfoConfirm(DefOnRspSettlementInfoConfirm cb)///Ͷ���߽�����ȷ����Ӧ
{	_OnRspSettlementInfoConfirm = cb;	}

TRADEAPI_API void WINAPI RegRspRemoveParkedOrder(DefOnRspRemoveParkedOrder cb)///ɾ��Ԥ����Ӧ
{	_OnRspRemoveParkedOrder = cb;	}

TRADEAPI_API void WINAPI RegRspRemoveParkedOrderAction(DefOnRspRemoveParkedOrderAction cb)///ɾ��Ԥ�񳷵���Ӧ
{	_OnRspRemoveParkedOrderAction = cb;	}

TRADEAPI_API void WINAPI RegRspQryOrder(DefOnRspQryOrder cb)///�����ѯ������Ӧ
{	_OnRspQryOrder = cb;	}

TRADEAPI_API void WINAPI RegRspQryTrade(DefOnRspQryTrade cb)///�����ѯ�ɽ���Ӧ
{	_OnRspQryTrade = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorPosition(DefOnRspQryInvestorPosition cb)///�����ѯͶ���ֲ߳���Ӧ
{	_OnRspQryInvestorPosition = cb;	}

TRADEAPI_API void WINAPI RegRspQryTradingAccount(DefOnRspQryTradingAccount cb)///�����ѯ�ʽ��˻���Ӧ
{	_OnRspQryTradingAccount = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestor(DefOnRspQryInvestor cb)///�����ѯͶ������Ӧ
{	_OnRspQryInvestor = cb;	}

TRADEAPI_API void WINAPI RegRspQryTradingCode(DefOnRspQryTradingCode cb)///�����ѯ���ױ�����Ӧ
{	_OnRspQryTradingCode = cb;	}

TRADEAPI_API void WINAPI RegRspQryInstrumentMarginRate(DefOnRspQryInstrumentMarginRate cb)///�����ѯ��Լ��֤������Ӧ
{	_OnRspQryInstrumentMarginRate = cb;	}

TRADEAPI_API void WINAPI RegRspQryInstrumentCommissionRate(DefOnRspQryInstrumentCommissionRate cb)///�����ѯ��Լ����������Ӧ
{	_OnRspQryInstrumentCommissionRate = cb;	}

TRADEAPI_API void WINAPI RegRspQryExchange(DefOnRspQryExchange cb)///�����ѯ��������Ӧ
{	_OnRspQryExchange = cb;	}

TRADEAPI_API void WINAPI RegRspQryInstrument(DefOnRspQryInstrument cb)///�����ѯ��Լ��Ӧ
{	_OnRspQryInstrument = cb;	}

TRADEAPI_API void WINAPI RegRspQryDepthMarketData(DefOnRspQryDepthMarketData cb)///�����ѯ������Ӧ
{	_OnRspQryDepthMarketData = cb;	}

TRADEAPI_API void WINAPI RegRspQrySettlementInfo(DefOnRspQrySettlementInfo cb)///�����ѯͶ���߽�������Ӧ
{	_OnRspQrySettlementInfo = cb;	}

TRADEAPI_API void WINAPI RegRspQryTransferBank(DefOnRspQryTransferBank cb)///�����ѯת��������Ӧ
{	_OnRspQryTransferBank = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorPositionDetail(DefOnRspQryInvestorPositionDetail cb)///�����ѯͶ���ֲ߳���ϸ��Ӧ
{	_OnRspQryInvestorPositionDetail = cb;	}

TRADEAPI_API void WINAPI RegRspQryNotice(DefOnRspQryNotice cb)///�����ѯ�ͻ�֪ͨ��Ӧ
{	_OnRspQryNotice = cb;	}

TRADEAPI_API void WINAPI RegRspQrySettlementInfoConfirm(DefOnRspQrySettlementInfoConfirm cb)///�����ѯ������Ϣȷ����Ӧ
{	_OnRspQrySettlementInfoConfirm = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorPositionCombineDetail(DefOnRspQryInvestorPositionCombineDetail cb)///�����ѯͶ���ֲ߳���ϸ��Ӧ
{	_OnRspQryInvestorPositionCombineDetail = cb;	}

TRADEAPI_API void WINAPI RegRspQryCFMMCTradingAccountKey(DefOnRspQryCFMMCTradingAccountKey cb)///��ѯ��֤����ϵͳ���͹�˾�ʽ��˻���Կ��Ӧ
{	_OnRspQryCFMMCTradingAccountKey = cb;	}

TRADEAPI_API void WINAPI RegRspQryAccountregister(DefOnRspQryAccountregister cb) ///�����ѯ����ǩԼ��ϵ��Ӧ
{	_OnRspQryAccountregister = cb;	}

TRADEAPI_API void WINAPI RegRspQryTransferSerial(DefOnRspQryTransferSerial cb)///�����ѯת����ˮ��Ӧ
{	_OnRspQryTransferSerial = cb;	}

TRADEAPI_API void WINAPI RegRspError(DefOnRspError cb)///����Ӧ��
{	_OnRspError = cb;	}

TRADEAPI_API void WINAPI RegRtnOrder(DefOnRtnOrder cb)///����֪ͨ
{	_OnRtnOrder = cb;	}

TRADEAPI_API void WINAPI RegRtnTrade(DefOnRtnTrade cb)///�ɽ�֪ͨ
{	_OnRtnTrade = cb;	}

TRADEAPI_API void WINAPI RegErrRtnOrderInsert(DefOnErrRtnOrderInsert cb)///����¼�����ر�
{	_OnErrRtnOrderInsert = cb;	}

TRADEAPI_API void WINAPI RegErrRtnOrderAction(DefOnErrRtnOrderAction cb)///������������ر�
{	_OnErrRtnOrderAction = cb;	}

TRADEAPI_API void WINAPI RegRtnInstrumentStatus(DefOnRtnInstrumentStatus cb)///��Լ����״̬֪ͨ
{	_OnRtnInstrumentStatus = cb;	}

TRADEAPI_API void WINAPI RegRtnTradingNotice(DefOnRtnTradingNotice cb)///����֪ͨ
{	_OnRtnTradingNotice = cb;	}

TRADEAPI_API void WINAPI RegRtnErrorConditionalOrder(DefOnRtnErrorConditionalOrder cb)///��ʾ������У�����
{	_OnRtnErrorConditionalOrder = cb;	}

TRADEAPI_API void WINAPI RegRspQryContractBank(DefOnRspQryContractBank cb)///�����ѯǩԼ������Ӧ
{	_OnRspQryContractBank = cb;	}

TRADEAPI_API void WINAPI RegRspQryParkedOrder(DefOnRspQryParkedOrder cb)///�����ѯԤ����Ӧ
{	_OnRspQryParkedOrder = cb;	}

TRADEAPI_API void WINAPI RegRspQryParkedOrderAction(DefOnRspQryParkedOrderAction cb)///�����ѯԤ�񳷵���Ӧ
{	_OnRspQryParkedOrderAction = cb;	}

TRADEAPI_API void WINAPI RegRspQryTradingNotice(DefOnRspQryTradingNotice cb)///�����ѯ����֪ͨ��Ӧ
{	_OnRspQryTradingNotice = cb;	}

TRADEAPI_API void WINAPI RegRspQryBrokerTradingParams(DefOnRspQryBrokerTradingParams cb)///�����ѯ���͹�˾���ײ�����Ӧ
{	_OnRspQryBrokerTradingParams = cb;	}

TRADEAPI_API void WINAPI RegRspQryBrokerTradingAlgos(DefOnRspQryBrokerTradingAlgos cb)///�����ѯ���͹�˾�����㷨��Ӧ
{	_OnRspQryBrokerTradingAlgos = cb;	}

TRADEAPI_API void WINAPI RegRtnFromBankToFutureByBank(DefOnRtnFromBankToFutureByBank cb)///���з��������ʽ�ת�ڻ�֪ͨ
{	_OnRtnFromBankToFutureByBank = cb;	}

TRADEAPI_API void WINAPI RegRtnFromFutureToBankByBank(DefOnRtnFromFutureToBankByBank cb)///���з����ڻ��ʽ�ת����֪ͨ
{	_OnRtnFromFutureToBankByBank = cb;	}

TRADEAPI_API void WINAPI RegRtnRepealFromBankToFutureByBank(DefOnRtnRepealFromBankToFutureByBank cb)///���з����������ת�ڻ�֪ͨ
{	_OnRtnRepealFromBankToFutureByBank = cb;	}

TRADEAPI_API void WINAPI RegRtnRepealFromFutureToBankByBank(DefOnRtnRepealFromFutureToBankByBank cb)///���з�������ڻ�ת����֪ͨ
{	_OnRtnRepealFromFutureToBankByBank = cb;	}

TRADEAPI_API void WINAPI RegRtnFromBankToFutureByFuture(DefOnRtnFromBankToFutureByFuture cb)///�ڻ����������ʽ�ת�ڻ�֪ͨ
{	_OnRtnFromBankToFutureByFuture = cb;	}

TRADEAPI_API void WINAPI RegRtnFromFutureToBankByFuture(DefOnRtnFromFutureToBankByFuture cb)///�ڻ������ڻ��ʽ�ת����֪ͨ
{	_OnRtnFromFutureToBankByFuture = cb;	}

TRADEAPI_API void WINAPI RegRtnRepealFromBankToFutureByFutureManual(DefOnRtnRepealFromBankToFutureByFutureManual cb)///ϵͳ����ʱ�ڻ����ֹ������������ת�ڻ��������д�����Ϻ��̷��ص�֪ͨ
{	_OnRtnRepealFromBankToFutureByFutureManual = cb;	}

TRADEAPI_API void WINAPI RegRtnRepealFromFutureToBankByFutureManual(DefOnRtnRepealFromFutureToBankByFutureManual cb)///ϵͳ����ʱ�ڻ����ֹ���������ڻ�ת�����������д�����Ϻ��̷��ص�֪ͨ
{	_OnRtnRepealFromFutureToBankByFutureManual = cb;	}

TRADEAPI_API void WINAPI RegRtnQueryBankBalanceByFuture(DefOnRtnQueryBankBalanceByFuture cb)///�ڻ������ѯ�������֪ͨ
{	_OnRtnQueryBankBalanceByFuture = cb;	}

TRADEAPI_API void WINAPI RegErrRtnBankToFutureByFuture(DefOnErrRtnBankToFutureByFuture cb)///�ڻ����������ʽ�ת�ڻ�����ر�
{	_OnErrRtnBankToFutureByFuture = cb;	}

TRADEAPI_API void WINAPI RegErrRtnFutureToBankByFuture(DefOnErrRtnFutureToBankByFuture cb)///�ڻ������ڻ��ʽ�ת���д���ر�
{	_OnErrRtnFutureToBankByFuture = cb;	}

TRADEAPI_API void WINAPI RegErrRtnRepealBankToFutureByFutureManual(DefOnErrRtnRepealBankToFutureByFutureManual cb)///ϵͳ����ʱ�ڻ����ֹ������������ת�ڻ�����ر�
{	_OnErrRtnRepealBankToFutureByFutureManual = cb;	}

TRADEAPI_API void WINAPI RegErrRtnRepealFutureToBankByFutureManual(DefOnErrRtnRepealFutureToBankByFutureManual cb)///ϵͳ����ʱ�ڻ����ֹ���������ڻ�ת���д���ر�
{	_OnErrRtnRepealFutureToBankByFutureManual = cb;	}

TRADEAPI_API void WINAPI RegErrRtnQueryBankBalanceByFuture(DefOnErrRtnQueryBankBalanceByFuture cb)///�ڻ������ѯ����������ر�
{	_OnErrRtnQueryBankBalanceByFuture = cb;	}

TRADEAPI_API void WINAPI RegRtnRepealFromBankToFutureByFuture(DefOnRtnRepealFromBankToFutureByFuture cb)///�ڻ������������ת�ڻ��������д�����Ϻ��̷��ص�֪ͨ
{	_OnRtnRepealFromBankToFutureByFuture = cb;	}

TRADEAPI_API void WINAPI RegRtnRepealFromFutureToBankByFuture(DefOnRtnRepealFromFutureToBankByFuture cb)///�ڻ���������ڻ�ת�����������д�����Ϻ��̷��ص�֪ͨ
{	_OnRtnRepealFromFutureToBankByFuture = cb;	}

TRADEAPI_API void WINAPI RegRspFromBankToFutureByFuture(DefOnRspFromBankToFutureByFuture cb)///�ڻ����������ʽ�ת�ڻ�Ӧ��
{	_OnRspFromBankToFutureByFuture = cb;	}

TRADEAPI_API void WINAPI RegRspFromFutureToBankByFuture(DefOnRspFromFutureToBankByFuture cb)///�ڻ������ڻ��ʽ�ת����Ӧ��
{	_OnRspFromFutureToBankByFuture = cb;	}

TRADEAPI_API void WINAPI RegRspQueryBankAccountMoneyByFuture(DefOnRspQueryBankAccountMoneyByFuture cb)///�ڻ������ѯ�������Ӧ��
{	_OnRspQueryBankAccountMoneyByFuture = cb;	}

TRADEAPI_API void WINAPI RegRspQryOptionInstrCommRate(DefOnRspQryOptionInstrCommRate cb)///�����ѯ��Ȩ��Լ��������Ӧ
{	_OnRspQryOptionInstrCommRate = cb;	}

TRADEAPI_API void WINAPI RegRspQryOptionInstrTradeCost(DefOnRspQryOptionInstrTradeCost cb)///�����ѯ��Ȩ���׳ɱ���Ӧ
{	_OnRspQryOptionInstrTradeCost = cb;	}

TRADEAPI_API void WINAPI RegRspQryForQuote(DefOnRspQryForQuote cb)///�����ѯѯ����Ӧ
{	_OnRspQryForQuote = cb;	}

TRADEAPI_API void WINAPI RegRtnForQuoteRsp(DefOnRtnForQuoteRsp cb)///ѯ��֪ͨ
{	_OnRtnForQuoteRsp = cb;	}

TRADEAPI_API void WINAPI RegRspForQuoteInsert(DefOnRspForQuoteInsert cb)///ѯ��¼��������Ӧ
{	_OnRspForQuoteInsert = cb;	}

TRADEAPI_API void WINAPI RegErrRtnForQuoteInsert(DefOnErrRtnForQuoteInsert cb)///ѯ��¼�����ر�
{	_OnErrRtnForQuoteInsert = cb;	}

TRADEAPI_API void WINAPI RegRspQryQuote(DefOnRspQryQuote cb)///�����ѯ������Ӧ
{	_OnRspQryQuote = cb;	}

TRADEAPI_API void WINAPI RegRtnQuote(DefOnRtnQuote cb)///����֪ͨ
{	_OnRtnQuote = cb;	}

TRADEAPI_API void WINAPI RegRspQuoteInsert(DefOnRspQuoteInsert cb)///����¼��������Ӧ
{	_OnRspQuoteInsert = cb;	}

TRADEAPI_API void WINAPI RegErrRtnQuoteInsert(DefOnErrRtnQuoteInsert cb)///����¼�����ر�
{	_OnErrRtnQuoteInsert = cb;	}

TRADEAPI_API void WINAPI RegRspQuoteAction(DefOnRspQuoteAction cb)///���۲���������Ӧ
{	_OnRspQuoteAction = cb;	}

TRADEAPI_API void WINAPI RegErrRtnQuoteAction(DefOnErrRtnQuoteAction cb)///���۲�������ر�
{	_OnErrRtnQuoteAction = cb;	}

TRADEAPI_API void WINAPI RegRspQryExecOrder(DefOnRspQryExecOrder cb)///�����ѯִ��������Ӧ
{	_OnRspQryExecOrder = cb;	}

TRADEAPI_API void WINAPI RegRtnExecOrder(DefOnRtnExecOrder cb)///ִ������֪ͨ
{	_OnRtnExecOrder = cb;	}

TRADEAPI_API void WINAPI RegRspExecOrderInsert(DefOnRspExecOrderInsert cb)///ִ������¼��������Ӧ
{	_OnRspExecOrderInsert = cb;	}

TRADEAPI_API void WINAPI RegErrRtnExecOrderInsert(DefOnErrRtnExecOrderInsert cb)///ִ������¼�����ر�
{	_OnErrRtnExecOrderInsert = cb;	}

TRADEAPI_API void WINAPI RegRspExecOrderAction(DefOnRspExecOrderAction cb)///ִ���������������Ӧ
{	_OnRspExecOrderAction = cb;	}

TRADEAPI_API void WINAPI RegErrRtnExecOrderAction(DefOnErrRtnExecOrderAction cb)///ִ�������������ر�
{	_OnErrRtnExecOrderAction = cb;	}

TRADEAPI_API void WINAPI RegRspCombActionInsert(DefOnRspCombActionInsert cb)///�������¼��������Ӧ
{	_OnRspCombActionInsert = cb;	}

TRADEAPI_API void WINAPI RegRspQryCombInstrumentGuard(DefOnRspQryCombInstrumentGuard cb)///�����ѯ��Ϻ�Լ��ȫϵ����Ӧ
{	_OnRspQryCombInstrumentGuard = cb;	}

TRADEAPI_API void WINAPI RegRspQryCombAction(DefOnRspQryCombAction cb)///�����ѯ���������Ӧ
{	_OnRspQryCombAction = cb;	}

TRADEAPI_API void WINAPI RegRtnCombAction(DefOnRtnCombAction cb) ///�������֪ͨ
{	_OnRtnCombAction = cb;	}

TRADEAPI_API void WINAPI RegErrRtnCombActionInsert(DefOnErrRtnCombActionInsert cb) ///�������¼�����ر�
{	_OnErrRtnCombActionInsert = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorProductGroupMargin(DefOnRspQryInvestorProductGroupMargin cb) ///�����ѯͶ����Ʒ��/��Ʒ�ֱ�֤����Ӧ
{	_OnRspQryInvestorProductGroupMargin = cb;	}

TRADEAPI_API void WINAPI RegRspQryExchangeMarginRateAdjust(DefOnRspQryExchangeMarginRateAdjust cb) ///�����ѯ������������֤������Ӧ
{	_OnRspQryExchangeMarginRateAdjust = cb;	}

TRADEAPI_API void WINAPI RegRspQryExchangeRate(DefOnRspQryExchangeRate cb) ///�����ѯ������Ӧ
{	_OnRspQryExchangeRate = cb;	}

TRADEAPI_API void WINAPI RegRspQryProductExchRate(DefOnRspQryProductExchRate cb) ///�����ѯ��Ʒ���ۻ���
{	_OnRspQryProductExchRate = cb;	}

TRADEAPI_API void WINAPI RegRspBatchOrderAction(DefOnRspBatchOrderAction cb) ///�����ѯ������Ӧ
{	_OnRspBatchOrderAction = cb;		}

TRADEAPI_API void WINAPI RegRspQryProductGroup(DefOnRspQryProductGroup cb) ///�����ѯ������Ӧ
{	_OnRspQryProductGroup = cb;		}

TRADEAPI_API void WINAPI RegRspQryMMInstrumentCommissionRate(DefOnRspQryMMInstrumentCommissionRate cb) ///�����ѯ������Ӧ
{	_OnRspQryMMInstrumentCommissionRate = cb;		}

TRADEAPI_API void WINAPI RegRspQryMMOptionInstrCommRate(DefOnRspQryMMOptionInstrCommRate cb) ///�����ѯ������Ӧ
{	_OnRspQryMMOptionInstrCommRate = cb;		}

TRADEAPI_API void WINAPI RegRspQryInstrumentOrderCommRate(DefOnRspQryInstrumentOrderCommRate cb) ///�����ѯ������Ӧ
{	_OnRspQryInstrumentOrderCommRate = cb;		}

TRADEAPI_API void WINAPI RegRtnBulletin(DefOnRtnBulletin cb) ///�����ѯ������Ӧ
{	_OnRtnBulletin = cb;		}

TRADEAPI_API void WINAPI RegErrRtnBatchOrderAction(DefOnErrRtnBatchOrderAction cb) ///�����ѯ������Ӧ
{	_OnErrRtnBatchOrderAction = cb;		}

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

	///������ʱ���档����ʱ��δ�յ�����ʱ���÷��������á�  @param nTimeLapse �����ϴν��ձ��ĵ�ʱ��
void CTraderSpi::OnHeartBeatWarning(int nTimeLapse)
{
	if(_OnHeartBeatWarning != NULL)
	{
		((DefOnHeartBeatWarning)_OnHeartBeatWarning)(nTimeLapse);
	}
}

///�ͻ�����֤��Ӧ
void CTraderSpi::OnRspAuthenticate(CThostFtdcRspAuthenticateField *pRspAuthenticateField, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///��¼������Ӧ
void CTraderSpi::OnRspUserLogin(CThostFtdcRspUserLoginField *pRspUserLogin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspUserLogin!=NULL)
	{
		if(pRspUserLogin == NULL)
		{
			CThostFtdcRspUserLoginField req;
			memset(&req,0,sizeof(req));
			((DefOnRspUserLogin)_OnRspUserLogin)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspUserLogin)_OnRspUserLogin)(pRspUserLogin,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�ǳ�������Ӧ
void CTraderSpi::OnRspUserLogout(CThostFtdcUserLogoutField *pUserLogout, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspUserLogout!=NULL)
	{
		if(pUserLogout == NULL)
		{
			CThostFtdcUserLogoutField req;
			memset(&req,0,sizeof(req));
			((DefOnRspUserLogout)_OnRspUserLogout)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspUserLogout)_OnRspUserLogout)(pUserLogout,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�û��������������Ӧ
void CTraderSpi::OnRspUserPasswordUpdate(CThostFtdcUserPasswordUpdateField *pUserPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspUserPasswordUpdate!=NULL)
	{
		if(pUserPasswordUpdate == NULL)
		{
			CThostFtdcUserPasswordUpdateField req;
			memset(&req,0,sizeof(req));
			((DefOnRspUserPasswordUpdate)_OnRspUserPasswordUpdate)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspUserPasswordUpdate)_OnRspUserPasswordUpdate)(pUserPasswordUpdate,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�ʽ��˻��������������Ӧ
void CTraderSpi::OnRspTradingAccountPasswordUpdate(CThostFtdcTradingAccountPasswordUpdateField *pTradingAccountPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspTradingAccountPasswordUpdate!=NULL)
	{
		if(pTradingAccountPasswordUpdate == NULL)
		{
			CThostFtdcTradingAccountPasswordUpdateField req;
			memset(&req,0,sizeof(req));
			((DefOnRspTradingAccountPasswordUpdate)_OnRspTradingAccountPasswordUpdate)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspTradingAccountPasswordUpdate)_OnRspTradingAccountPasswordUpdate)(pTradingAccountPasswordUpdate,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///����¼��������Ӧ
void CTraderSpi::OnRspOrderInsert(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspOrderInsert!=NULL)
	{
		if(pInputOrder == NULL)
		{
			CThostFtdcInputOrderField req;
			memset(&req,0,sizeof(req));
			((DefOnRspOrderInsert)_OnRspOrderInsert)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspOrderInsert)_OnRspOrderInsert)(pInputOrder,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///Ԥ��¼��������Ӧ
void CTraderSpi::OnRspParkedOrderInsert(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspParkedOrderInsert!=NULL)
	{
		if(pParkedOrder == NULL)
		{
			CThostFtdcParkedOrderField req;
			memset(&req,0,sizeof(req));
			((DefOnRspParkedOrderInsert)_OnRspParkedOrderInsert)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspParkedOrderInsert)_OnRspParkedOrderInsert)(pParkedOrder,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///Ԥ�񳷵�¼��������Ӧ
void CTraderSpi::OnRspParkedOrderAction(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspParkedOrderAction!=NULL)
	{
		if(pParkedOrderAction == NULL)
		{
			CThostFtdcParkedOrderActionField req;
			memset(&req,0,sizeof(req));
			((DefOnRspParkedOrderAction)_OnRspParkedOrderAction)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspParkedOrderAction)_OnRspParkedOrderAction)(pParkedOrderAction,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///��������������Ӧ
void CTraderSpi::OnRspOrderAction(CThostFtdcInputOrderActionField *pInputOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspOrderAction!=NULL)
	{
		if(pInputOrderAction == NULL)
		{
			CThostFtdcInputOrderActionField req;
			memset(&req,0,sizeof(req));
			((DefOnRspOrderAction)_OnRspOrderAction)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspOrderAction)_OnRspOrderAction)(pInputOrderAction,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///��ѯ��󱨵�������Ӧ
void CTraderSpi::OnRspQueryMaxOrderVolume(CThostFtdcQueryMaxOrderVolumeField *pQueryMaxOrderVolume, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQueryMaxOrderVolume!=NULL)
	{
		if(pQueryMaxOrderVolume == NULL)
		{
			CThostFtdcQueryMaxOrderVolumeField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQueryMaxOrderVolume)_OnRspQueryMaxOrderVolume)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQueryMaxOrderVolume)_OnRspQueryMaxOrderVolume)(pQueryMaxOrderVolume,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///Ͷ���߽�����ȷ����Ӧ
void CTraderSpi::OnRspSettlementInfoConfirm(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspSettlementInfoConfirm!=NULL)
	{
		if(pSettlementInfoConfirm == NULL)
		{
			CThostFtdcSettlementInfoConfirmField req;
			memset(&req,0,sizeof(req));
			((DefOnRspSettlementInfoConfirm)_OnRspSettlementInfoConfirm)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspSettlementInfoConfirm)_OnRspSettlementInfoConfirm)(pSettlementInfoConfirm,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///ɾ��Ԥ����Ӧ
void CTraderSpi::OnRspRemoveParkedOrder(CThostFtdcRemoveParkedOrderField *pRemoveParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspRemoveParkedOrder!=NULL)
	{
		if(pRemoveParkedOrder == NULL)
		{
			CThostFtdcRemoveParkedOrderField req;
			memset(&req,0,sizeof(req));
			((DefOnRspRemoveParkedOrder)_OnRspRemoveParkedOrder)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspRemoveParkedOrder)_OnRspRemoveParkedOrder)(pRemoveParkedOrder,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///ɾ��Ԥ�񳷵���Ӧ
void CTraderSpi::OnRspRemoveParkedOrderAction(CThostFtdcRemoveParkedOrderActionField *pRemoveParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspRemoveParkedOrderAction!=NULL)
	{
		if(pRemoveParkedOrderAction == NULL)
		{
			CThostFtdcRemoveParkedOrderActionField req;
			memset(&req,0,sizeof(req));
			((DefOnRspRemoveParkedOrderAction)_OnRspRemoveParkedOrderAction)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspRemoveParkedOrderAction)_OnRspRemoveParkedOrderAction)(pRemoveParkedOrderAction,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ������Ӧ
void CTraderSpi::OnRspQryOrder(CThostFtdcOrderField *pOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryOrder!=NULL)
	{
		if(pOrder == NULL)
		{
			CThostFtdcOrderField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryOrder)_OnRspQryOrder)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryOrder)_OnRspQryOrder)(pOrder,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ�ɽ���Ӧ
void CTraderSpi::OnRspQryTrade(CThostFtdcTradeField *pTrade, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryTrade!=NULL)
	{
		if(pTrade == NULL)
		{
			CThostFtdcTradeField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryTrade)_OnRspQryTrade)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryTrade)_OnRspQryTrade)(pTrade,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯͶ���ֲ߳���Ӧ
void CTraderSpi::OnRspQryInvestorPosition(CThostFtdcInvestorPositionField *pInvestorPosition, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryInvestorPosition!=NULL)
	{
		if(pInvestorPosition == NULL)
		{
			CThostFtdcInvestorPositionField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryInvestorPosition)_OnRspQryInvestorPosition)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryInvestorPosition)_OnRspQryInvestorPosition)(pInvestorPosition,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ�ʽ��˻���Ӧ
void CTraderSpi::OnRspQryTradingAccount(CThostFtdcTradingAccountField *pTradingAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryTradingAccount!=NULL)
	{
		if(pTradingAccount == NULL)
		{
			CThostFtdcTradingAccountField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryTradingAccount)_OnRspQryTradingAccount)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryTradingAccount)_OnRspQryTradingAccount)(pTradingAccount,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯͶ������Ӧ
void CTraderSpi::OnRspQryInvestor(CThostFtdcInvestorField *pInvestor, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryInvestor!=NULL)
	{
		if(pInvestor == NULL)
		{
			CThostFtdcInvestorField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryInvestor)_OnRspQryInvestor)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryInvestor)_OnRspQryInvestor)(pInvestor,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ���ױ�����Ӧ
void CTraderSpi::OnRspQryTradingCode(CThostFtdcTradingCodeField *pTradingCode, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryTradingCode!=NULL)
	{
		if(pTradingCode == NULL)
		{
			CThostFtdcTradingCodeField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryTradingCode)_OnRspQryTradingCode)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryTradingCode)_OnRspQryTradingCode)(pTradingCode,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ��Լ��֤������Ӧ
void CTraderSpi::OnRspQryInstrumentMarginRate(CThostFtdcInstrumentMarginRateField *pInstrumentMarginRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryInstrumentMarginRate!=NULL)
	{
		if(pInstrumentMarginRate == NULL)
		{
			CThostFtdcInstrumentMarginRateField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryInstrumentMarginRate)_OnRspQryInstrumentMarginRate)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryInstrumentMarginRate)_OnRspQryInstrumentMarginRate)(pInstrumentMarginRate,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ��Լ����������Ӧ
void CTraderSpi::OnRspQryInstrumentCommissionRate(CThostFtdcInstrumentCommissionRateField *pInstrumentCommissionRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryInstrumentCommissionRate!=NULL)
	{
		if(pInstrumentCommissionRate == NULL)
		{
			CThostFtdcInstrumentCommissionRateField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryInstrumentCommissionRate)_OnRspQryInstrumentCommissionRate)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryInstrumentCommissionRate)_OnRspQryInstrumentCommissionRate)(pInstrumentCommissionRate,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ��������Ӧ
void CTraderSpi::OnRspQryExchange(CThostFtdcExchangeField *pExchange, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryExchange!=NULL)
	{
		if(pExchange == NULL)
		{
			CThostFtdcExchangeField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryExchange)_OnRspQryExchange)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryExchange)_OnRspQryExchange)(pExchange,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ��Ʒ��Ӧ
void CTraderSpi::OnRspQryProduct(CThostFtdcProductField *pProduct, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ��Լ��Ӧ
void CTraderSpi::OnRspQryInstrument(CThostFtdcInstrumentField *pInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryInstrument!=NULL)
	{
		if(pInstrument == NULL)
		{
			CThostFtdcInstrumentField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryInstrument)_OnRspQryInstrument)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryInstrument)_OnRspQryInstrument)(pInstrument,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ������Ӧ
void CTraderSpi::OnRspQryDepthMarketData(CThostFtdcDepthMarketDataField *pDepthMarketData, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryDepthMarketData!=NULL)
	{
		if(pDepthMarketData == NULL)
		{
			CThostFtdcDepthMarketDataField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryDepthMarketData)_OnRspQryDepthMarketData)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryDepthMarketData)_OnRspQryDepthMarketData)(pDepthMarketData,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯͶ���߽�������Ӧ
void CTraderSpi::OnRspQrySettlementInfo(CThostFtdcSettlementInfoField *pSettlementInfo, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQrySettlementInfo!=NULL)
	{
		if(pSettlementInfo == NULL)
		{
			CThostFtdcSettlementInfoField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQrySettlementInfo)_OnRspQrySettlementInfo)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQrySettlementInfo)_OnRspQrySettlementInfo)(pSettlementInfo,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯת��������Ӧ
void CTraderSpi::OnRspQryTransferBank(CThostFtdcTransferBankField *pTransferBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryTransferBank!=NULL)
	{
		if(pTransferBank == NULL)
		{
			CThostFtdcTransferBankField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryTransferBank)_OnRspQryTransferBank)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryTransferBank)_OnRspQryTransferBank)(pTransferBank,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯͶ���ֲ߳���ϸ��Ӧ
void CTraderSpi::OnRspQryInvestorPositionDetail(CThostFtdcInvestorPositionDetailField *pInvestorPositionDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryInvestorPositionDetail!=NULL)
	{
		if(pInvestorPositionDetail == NULL)
		{
			CThostFtdcInvestorPositionDetailField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryInvestorPositionDetail)_OnRspQryInvestorPositionDetail)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryInvestorPositionDetail)_OnRspQryInvestorPositionDetail)(pInvestorPositionDetail,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ�ͻ�֪ͨ��Ӧ
void CTraderSpi::OnRspQryNotice(CThostFtdcNoticeField *pNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryNotice!=NULL)
	{
		if(pNotice == NULL)
		{
			CThostFtdcNoticeField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryNotice)_OnRspQryNotice)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryNotice)_OnRspQryNotice)(pNotice,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ������Ϣȷ����Ӧ
void CTraderSpi::OnRspQrySettlementInfoConfirm(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQrySettlementInfoConfirm!=NULL)
	{
		if(pSettlementInfoConfirm == NULL)
		{
			CThostFtdcSettlementInfoConfirmField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQrySettlementInfoConfirm)_OnRspQrySettlementInfoConfirm)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQrySettlementInfoConfirm)_OnRspQrySettlementInfoConfirm)(pSettlementInfoConfirm,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯͶ���ֲ߳���ϸ��Ӧ
void CTraderSpi::OnRspQryInvestorPositionCombineDetail(CThostFtdcInvestorPositionCombineDetailField *pInvestorPositionCombineDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryInvestorPositionCombineDetail!=NULL)
	{
		if(pInvestorPositionCombineDetail == NULL)
		{
			CThostFtdcInvestorPositionCombineDetailField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryInvestorPositionCombineDetail)_OnRspQryInvestorPositionCombineDetail)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryInvestorPositionCombineDetail)_OnRspQryInvestorPositionCombineDetail)(pInvestorPositionCombineDetail,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///��ѯ��֤����ϵͳ���͹�˾�ʽ��˻���Կ��Ӧ
void CTraderSpi::OnRspQryCFMMCTradingAccountKey(CThostFtdcCFMMCTradingAccountKeyField *pCFMMCTradingAccountKey, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryCFMMCTradingAccountKey!=NULL)
	{
		if(pCFMMCTradingAccountKey == NULL)
		{
			CThostFtdcCFMMCTradingAccountKeyField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryCFMMCTradingAccountKey)_OnRspQryCFMMCTradingAccountKey)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryCFMMCTradingAccountKey)_OnRspQryCFMMCTradingAccountKey)(pCFMMCTradingAccountKey,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///��֤���������û�����
void CTraderSpi::OnRtnCFMMCTradingAccountToken(CThostFtdcCFMMCTradingAccountTokenField *pCFMMCTradingAccountToken) {};

///����������������ر�
void CTraderSpi::OnErrRtnBatchOrderAction(CThostFtdcBatchOrderActionField *pBatchOrderAction, CThostFtdcRspInfoField *pRspInfo) {};

///�����ѯת��������Ӧ
void CTraderSpi::OnRspQryAccountregister(CThostFtdcAccountregisterField *pAccountregister, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryAccountregister!=NULL)
	{
		if(pAccountregister == NULL)
		{
			CThostFtdcAccountregisterField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryAccountregister)_OnRspQryAccountregister)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryAccountregister)_OnRspQryAccountregister)(pAccountregister,repareInfo(pRspInfo), nRequestID, bIsLast);

	}
}

///�����ѯת����ˮ��Ӧ
void CTraderSpi::OnRspQryTransferSerial(CThostFtdcTransferSerialField *pTransferSerial, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryTransferSerial!=NULL)
	{
		if(pTransferSerial == NULL)
		{
			CThostFtdcTransferSerialField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryTransferSerial)_OnRspQryTransferSerial)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryTransferSerial)_OnRspQryTransferSerial)(pTransferSerial,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///����Ӧ��
void CTraderSpi::OnRspError(CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspError!=NULL)
	{
		if(pRspInfo == NULL)
		{
			CThostFtdcRspInfoField req;
			memset(&req,0,sizeof(req));
			((DefOnRspError)_OnRspError)(&req, nRequestID, bIsLast);
		}
		else
			((DefOnRspError)_OnRspError)(pRspInfo,nRequestID, bIsLast);
	}
}

///����֪ͨ
void CTraderSpi::OnRtnOrder(CThostFtdcOrderField *pOrder) 
{
	if(_OnRtnOrder!=NULL)
	{
		if(pOrder == NULL)
		{
			CThostFtdcOrderField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnOrder)_OnRtnOrder)(&req);
		}
		else
			((DefOnRtnOrder)_OnRtnOrder)(pOrder);
	}
}

///�ɽ�֪ͨ
void CTraderSpi::OnRtnTrade(CThostFtdcTradeField *pTrade) 
{
	if(_OnRtnTrade!=NULL)
	{
		if(pTrade == NULL)
		{
			CThostFtdcTradeField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnTrade)_OnRtnTrade)(&req);
		}
		else
			((DefOnRtnTrade)_OnRtnTrade)(pTrade);
	}
}

///����¼�����ر�
void CTraderSpi::OnErrRtnOrderInsert(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo) 
{
	if(_OnErrRtnOrderInsert!=NULL)
	{
		if(pInputOrder == NULL)
		{
			CThostFtdcInputOrderField req;
			memset(&req,0,sizeof(req));
			((DefOnErrRtnOrderInsert)_OnErrRtnOrderInsert)(&req,repareInfo(pRspInfo));
		}
		else
			((DefOnErrRtnOrderInsert)_OnErrRtnOrderInsert)(pInputOrder,repareInfo(pRspInfo));
	}
}

///������������ر�
void CTraderSpi::OnErrRtnOrderAction(CThostFtdcOrderActionField *pOrderAction, CThostFtdcRspInfoField *pRspInfo) 
{
	if(_OnErrRtnOrderAction!=NULL)
	{
		if(pOrderAction == NULL)
		{
			CThostFtdcOrderActionField req;
			memset(&req,0,sizeof(req));
			((DefOnErrRtnOrderAction)_OnErrRtnOrderAction)(&req,repareInfo(pRspInfo));
		}
		else
			((DefOnErrRtnOrderAction)_OnErrRtnOrderAction)(pOrderAction,repareInfo(pRspInfo));
	}
}

///��Լ����״̬֪ͨ
void CTraderSpi::OnRtnInstrumentStatus(CThostFtdcInstrumentStatusField *pInstrumentStatus) 
{
	if(_OnRtnInstrumentStatus!=NULL)
	{
		if(pInstrumentStatus == NULL)
		{
			CThostFtdcInstrumentStatusField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnInstrumentStatus)_OnRtnInstrumentStatus)(&req);
		}
		else
			((DefOnRtnInstrumentStatus)_OnRtnInstrumentStatus)(pInstrumentStatus);
	}
}

///����������֪ͨ
void CTraderSpi::OnRtnBulletin(CThostFtdcBulletinField *pBulletin) {};

///����֪ͨ
void CTraderSpi::OnRtnTradingNotice(CThostFtdcTradingNoticeInfoField *pTradingNoticeInfo) 
{
	if(_OnRtnTradingNotice!=NULL)
	{
		if(pTradingNoticeInfo == NULL)
		{
			CThostFtdcTradingNoticeInfoField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnTradingNotice)_OnRtnTradingNotice)(&req);
		}
		else
			((DefOnRtnTradingNotice)_OnRtnTradingNotice)(pTradingNoticeInfo);
	}
}

///��ʾ������У�����
void CTraderSpi::OnRtnErrorConditionalOrder(CThostFtdcErrorConditionalOrderField *pErrorConditionalOrder) 
{
	if(_OnRtnErrorConditionalOrder!=NULL)
	{
		if(pErrorConditionalOrder == NULL)
		{
			CThostFtdcErrorConditionalOrderField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnErrorConditionalOrder)_OnRtnErrorConditionalOrder)(&req);
		}
		else
			((DefOnRtnErrorConditionalOrder)_OnRtnErrorConditionalOrder)(pErrorConditionalOrder);
	}
}

///�����ѯǩԼ������Ӧ
void CTraderSpi::OnRspQryContractBank(CThostFtdcContractBankField *pContractBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryContractBank!=NULL)
	{
		if(pContractBank == NULL)
		{
			CThostFtdcContractBankField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryContractBank)_OnRspQryContractBank)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryContractBank)_OnRspQryContractBank)(pContractBank,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯԤ����Ӧ
void CTraderSpi::OnRspQryParkedOrder(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryParkedOrder!=NULL)
	{
		if(pParkedOrder == NULL)
		{
			CThostFtdcParkedOrderField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryParkedOrder)_OnRspQryParkedOrder)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryParkedOrder)_OnRspQryParkedOrder)(pParkedOrder,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯԤ�񳷵���Ӧ
void CTraderSpi::OnRspQryParkedOrderAction(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryParkedOrderAction!=NULL)
	{
		if(pParkedOrderAction == NULL)
		{
			CThostFtdcParkedOrderActionField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryParkedOrderAction)_OnRspQryParkedOrderAction)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryParkedOrderAction)_OnRspQryParkedOrderAction)(pParkedOrderAction,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ����֪ͨ��Ӧ
void CTraderSpi::OnRspQryTradingNotice(CThostFtdcTradingNoticeField *pTradingNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryTradingNotice!=NULL)
	{
		if(pTradingNotice == NULL)
		{
			CThostFtdcTradingNoticeField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryTradingNotice)_OnRspQryTradingNotice)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryTradingNotice)_OnRspQryTradingNotice)(pTradingNotice,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ���͹�˾���ײ�����Ӧ
void CTraderSpi::OnRspQryBrokerTradingParams(CThostFtdcBrokerTradingParamsField *pBrokerTradingParams, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryBrokerTradingParams!=NULL)
	{
		if(pBrokerTradingParams == NULL)
		{
			CThostFtdcBrokerTradingParamsField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryBrokerTradingParams)_OnRspQryBrokerTradingParams)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryBrokerTradingParams)_OnRspQryBrokerTradingParams)(pBrokerTradingParams,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ���͹�˾�����㷨��Ӧ
void CTraderSpi::OnRspQryBrokerTradingAlgos(CThostFtdcBrokerTradingAlgosField *pBrokerTradingAlgos, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQryBrokerTradingAlgos!=NULL)
	{
		if(pBrokerTradingAlgos == NULL)
		{
			CThostFtdcBrokerTradingAlgosField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryBrokerTradingAlgos)_OnRspQryBrokerTradingAlgos)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryBrokerTradingAlgos)_OnRspQryBrokerTradingAlgos)(pBrokerTradingAlgos,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ��������û�����
void CTraderSpi::OnRspQueryCFMMCTradingAccountToken(CThostFtdcQueryCFMMCTradingAccountTokenField *pQueryCFMMCTradingAccountToken, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///���з��������ʽ�ת�ڻ�֪ͨ
void CTraderSpi::OnRtnFromBankToFutureByBank(CThostFtdcRspTransferField *pRspTransfer) 
{
	if(_OnRtnFromBankToFutureByBank!=NULL)
	{
		if(pRspTransfer == NULL)
		{
			CThostFtdcRspTransferField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnFromBankToFutureByBank)_OnRtnFromBankToFutureByBank)(&req);
		}
		else
			((DefOnRtnFromBankToFutureByBank)_OnRtnFromBankToFutureByBank)(pRspTransfer);
	}
}

///���з����ڻ��ʽ�ת����֪ͨ
void CTraderSpi::OnRtnFromFutureToBankByBank(CThostFtdcRspTransferField *pRspTransfer) 
{
	if(_OnRtnFromFutureToBankByBank!=NULL)
	{
		if(pRspTransfer == NULL)
		{
			CThostFtdcRspTransferField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnFromFutureToBankByBank)_OnRtnFromFutureToBankByBank)(&req);
		}
		else
			((DefOnRtnFromFutureToBankByBank)_OnRtnFromFutureToBankByBank)(pRspTransfer);
	}
}

///���з����������ת�ڻ�֪ͨ
void CTraderSpi::OnRtnRepealFromBankToFutureByBank(CThostFtdcRspRepealField *pRspRepeal) 
{
	if(_OnRtnRepealFromBankToFutureByBank!=NULL)
	{
		if(pRspRepeal == NULL)
		{
			CThostFtdcRspRepealField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnRepealFromBankToFutureByBank)_OnRtnRepealFromBankToFutureByBank)(&req);
		}
		else
			((DefOnRtnRepealFromBankToFutureByBank)_OnRtnRepealFromBankToFutureByBank)(pRspRepeal);
	}
}

///���з�������ڻ�ת����֪ͨ
void CTraderSpi::OnRtnRepealFromFutureToBankByBank(CThostFtdcRspRepealField *pRspRepeal) 
{
	if(_OnRtnRepealFromFutureToBankByBank!=NULL)
	{
		if(pRspRepeal == NULL)
		{
			CThostFtdcRspRepealField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnRepealFromFutureToBankByBank)_OnRtnRepealFromFutureToBankByBank)(&req);
		}
		else
			((DefOnRtnRepealFromFutureToBankByBank)_OnRtnRepealFromFutureToBankByBank)(pRspRepeal);
	}
}

///�ڻ����������ʽ�ת�ڻ�֪ͨ
void CTraderSpi::OnRtnFromBankToFutureByFuture(CThostFtdcRspTransferField *pRspTransfer) 
{
	if(_OnRtnFromBankToFutureByFuture!=NULL)
	{
		if(pRspTransfer == NULL)
		{
			CThostFtdcRspTransferField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnFromBankToFutureByFuture)_OnRtnFromBankToFutureByFuture)(&req);
		}
		else
			((DefOnRtnFromBankToFutureByFuture)_OnRtnFromBankToFutureByFuture)(pRspTransfer);
	}
}

///�ڻ������ڻ��ʽ�ת����֪ͨ
void CTraderSpi::OnRtnFromFutureToBankByFuture(CThostFtdcRspTransferField *pRspTransfer) 
{
	if(_OnRtnFromFutureToBankByFuture!=NULL)
	{
		if(pRspTransfer == NULL)
		{
			CThostFtdcRspTransferField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnFromFutureToBankByFuture)_OnRtnFromFutureToBankByFuture)(&req);
		}
		else
			((DefOnRtnFromFutureToBankByFuture)_OnRtnFromFutureToBankByFuture)(pRspTransfer);
	}
}

///ϵͳ����ʱ�ڻ����ֹ������������ת�ڻ��������д�����Ϻ��̷��ص�֪ͨ
void CTraderSpi::OnRtnRepealFromBankToFutureByFutureManual(CThostFtdcRspRepealField *pRspRepeal) 
{
	if(_OnRtnRepealFromBankToFutureByFutureManual!=NULL)
	{
		if(pRspRepeal == NULL)
		{
			CThostFtdcRspRepealField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnRepealFromBankToFutureByFutureManual)_OnRtnRepealFromBankToFutureByFutureManual)(&req);
		}
		else
			((DefOnRtnRepealFromBankToFutureByFutureManual)_OnRtnRepealFromBankToFutureByFutureManual)(pRspRepeal);
	}
}

///ϵͳ����ʱ�ڻ����ֹ���������ڻ�ת�����������д�����Ϻ��̷��ص�֪ͨ
void CTraderSpi::OnRtnRepealFromFutureToBankByFutureManual(CThostFtdcRspRepealField *pRspRepeal) 
{
	if(_OnRtnRepealFromFutureToBankByFutureManual!=NULL)
	{
		if(pRspRepeal == NULL)
		{
			CThostFtdcRspRepealField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnRepealFromFutureToBankByFutureManual)_OnRtnRepealFromFutureToBankByFutureManual)(&req);
		}
		else
			((DefOnRtnRepealFromFutureToBankByFutureManual)_OnRtnRepealFromFutureToBankByFutureManual)(pRspRepeal);
	}
}

///�ڻ������ѯ�������֪ͨ
void CTraderSpi::OnRtnQueryBankBalanceByFuture(CThostFtdcNotifyQueryAccountField *pNotifyQueryAccount) 
{
	if(_OnRtnQueryBankBalanceByFuture!=NULL)
	{
		if(pNotifyQueryAccount == NULL)
		{
			CThostFtdcNotifyQueryAccountField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnQueryBankBalanceByFuture)_OnRtnQueryBankBalanceByFuture)(&req);
		}
		else
			((DefOnRtnQueryBankBalanceByFuture)_OnRtnQueryBankBalanceByFuture)(pNotifyQueryAccount);
	}
}

///�ڻ����������ʽ�ת�ڻ�����ر�
void CTraderSpi::OnErrRtnBankToFutureByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo) 
{
	if(_OnErrRtnBankToFutureByFuture!=NULL)
	{
		if(pReqTransfer == NULL)
		{
			CThostFtdcReqTransferField req;
			memset(&req,0,sizeof(req));
			((DefOnErrRtnBankToFutureByFuture)_OnErrRtnBankToFutureByFuture)(&req,repareInfo(pRspInfo));
		}
		else
			((DefOnErrRtnBankToFutureByFuture)_OnErrRtnBankToFutureByFuture)(pReqTransfer,repareInfo(pRspInfo));
	}
}

///�ڻ������ڻ��ʽ�ת���д���ر�
void CTraderSpi::OnErrRtnFutureToBankByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo) 
{
	if(_OnErrRtnFutureToBankByFuture!=NULL)
	{
		if(pReqTransfer == NULL)
		{
			CThostFtdcReqTransferField req;
			memset(&req,0,sizeof(req));
			((DefOnErrRtnFutureToBankByFuture)_OnErrRtnFutureToBankByFuture)(&req,repareInfo(pRspInfo));
		}
		else
			((DefOnErrRtnFutureToBankByFuture)_OnErrRtnFutureToBankByFuture)(pReqTransfer,repareInfo(pRspInfo));
	}
}

///ϵͳ����ʱ�ڻ����ֹ������������ת�ڻ�����ر�
void CTraderSpi::OnErrRtnRepealBankToFutureByFutureManual(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo) 
{
	if(_OnErrRtnRepealBankToFutureByFutureManual!=NULL)
	{
		if(pReqRepeal == NULL)
		{
			CThostFtdcReqRepealField req;
			memset(&req,0,sizeof(req));
			((DefOnErrRtnRepealBankToFutureByFutureManual)_OnErrRtnRepealBankToFutureByFutureManual)(&req,repareInfo(pRspInfo));
		}
		else
			((DefOnErrRtnRepealBankToFutureByFutureManual)_OnErrRtnRepealBankToFutureByFutureManual)(pReqRepeal,repareInfo(pRspInfo));
	}
}

///ϵͳ����ʱ�ڻ����ֹ���������ڻ�ת���д���ر�
void CTraderSpi::OnErrRtnRepealFutureToBankByFutureManual(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo) 
{
	if(_OnErrRtnRepealFutureToBankByFutureManual!=NULL)
	{
		if(pReqRepeal == NULL)
		{
			CThostFtdcReqRepealField req;
			memset(&req,0,sizeof(req));
			((DefOnErrRtnRepealFutureToBankByFutureManual)_OnErrRtnRepealFutureToBankByFutureManual)(&req,repareInfo(pRspInfo));
		}
		else
			((DefOnErrRtnRepealFutureToBankByFutureManual)_OnErrRtnRepealFutureToBankByFutureManual)(pReqRepeal,repareInfo(pRspInfo));
	}
}

///�ڻ������ѯ����������ر�
void CTraderSpi::OnErrRtnQueryBankBalanceByFuture(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo) 
{
	if(_OnErrRtnQueryBankBalanceByFuture!=NULL)
	{
		if(pReqQueryAccount == NULL)
		{
			CThostFtdcReqQueryAccountField req;
			memset(&req,0,sizeof(req));
			((DefOnErrRtnQueryBankBalanceByFuture)_OnErrRtnQueryBankBalanceByFuture)(&req,repareInfo(pRspInfo));
		}
		else
			((DefOnErrRtnQueryBankBalanceByFuture)_OnErrRtnQueryBankBalanceByFuture)(pReqQueryAccount,repareInfo(pRspInfo));
	}
}

///�ڻ������������ת�ڻ��������д�����Ϻ��̷��ص�֪ͨ
void CTraderSpi::OnRtnRepealFromBankToFutureByFuture(CThostFtdcRspRepealField *pRspRepeal) 
{
	if(_OnRtnRepealFromBankToFutureByFuture!=NULL)
	{
		if(pRspRepeal == NULL)
		{
			CThostFtdcRspRepealField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnRepealFromBankToFutureByFuture)_OnRtnRepealFromBankToFutureByFuture)(&req);
		}
		else
			((DefOnRtnRepealFromBankToFutureByFuture)_OnRtnRepealFromBankToFutureByFuture)(pRspRepeal);
	}
}

///�ڻ���������ڻ�ת�����������д�����Ϻ��̷��ص�֪ͨ
void CTraderSpi::OnRtnRepealFromFutureToBankByFuture(CThostFtdcRspRepealField *pRspRepeal) 
{
	if(_OnRtnRepealFromFutureToBankByFuture!=NULL)
	{
		if(pRspRepeal == NULL)
		{
			CThostFtdcRspRepealField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnRepealFromFutureToBankByFuture)_OnRtnRepealFromFutureToBankByFuture)(&req);
		}
		else
			((DefOnRtnRepealFromFutureToBankByFuture)_OnRtnRepealFromFutureToBankByFuture)(pRspRepeal);
	}
}

///�ڻ����������ʽ�ת�ڻ�Ӧ��
void CTraderSpi::OnRspFromBankToFutureByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspFromBankToFutureByFuture!=NULL)
	{
		if(pReqTransfer == NULL)
		{
			CThostFtdcReqTransferField req;
			memset(&req,0,sizeof(req));
			((DefOnRspFromBankToFutureByFuture)_OnRspFromBankToFutureByFuture)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspFromBankToFutureByFuture)_OnRspFromBankToFutureByFuture)(pReqTransfer,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�ڻ������ڻ��ʽ�ת����Ӧ��
void CTraderSpi::OnRspFromFutureToBankByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspFromFutureToBankByFuture!=NULL)
	{
		if(pReqTransfer == NULL)
		{
			CThostFtdcReqTransferField req;
			memset(&req,0,sizeof(req));
			((DefOnRspFromFutureToBankByFuture)_OnRspFromFutureToBankByFuture)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspFromFutureToBankByFuture)_OnRspFromFutureToBankByFuture)(pReqTransfer,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�ڻ������ѯ�������Ӧ��
void CTraderSpi::OnRspQueryBankAccountMoneyByFuture(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) 
{
	if(_OnRspQueryBankAccountMoneyByFuture!=NULL)
	{
		if(pReqQueryAccount == NULL)
		{
			CThostFtdcReqQueryAccountField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQueryBankAccountMoneyByFuture)_OnRspQueryBankAccountMoneyByFuture)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQueryBankAccountMoneyByFuture)_OnRspQueryBankAccountMoneyByFuture)(pReqQueryAccount,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///���з������ڿ���֪ͨ
void CTraderSpi::OnRtnOpenAccountByBank(CThostFtdcOpenAccountField *pOpenAccount)
{}

///���з�����������֪ͨ
void CTraderSpi::OnRtnCancelAccountByBank(CThostFtdcCancelAccountField *pCancelAccount)
{}

///���з����������˺�֪ͨ
void CTraderSpi::OnRtnChangeAccountByBank(CThostFtdcChangeAccountField *pChangeAccount)
{}

///�����ѯ��Ȩ��Լ��������Ӧ
void CTraderSpi::OnRspQryOptionInstrCommRate(CThostFtdcOptionInstrCommRateField *pOptionInstrCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryOptionInstrCommRate!=NULL)
	{
		if(pOptionInstrCommRate == NULL)
		{
			CThostFtdcOptionInstrCommRateField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryOptionInstrCommRate)_OnRspQryOptionInstrCommRate)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryOptionInstrCommRate)_OnRspQryOptionInstrCommRate)(pOptionInstrCommRate,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ��Ȩ���׳ɱ���Ӧ
void CTraderSpi::OnRspQryOptionInstrTradeCost(CThostFtdcOptionInstrTradeCostField *pOptionInstrTradeCost, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryOptionInstrTradeCost!=NULL)
	{
		if(pOptionInstrTradeCost == NULL)
		{
			CThostFtdcOptionInstrTradeCostField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryOptionInstrTradeCost)_OnRspQryOptionInstrTradeCost)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryOptionInstrTradeCost)_OnRspQryOptionInstrTradeCost)(pOptionInstrTradeCost,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ�ֵ��۵���Ϣ��Ӧ
void CTraderSpi::OnRspQryEWarrantOffset(CThostFtdcEWarrantOffsetField *pEWarrantOffset, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) {};

///�����ѯͶ����Ʒ��/��Ʒ�ֱ�֤����Ӧ
void CTraderSpi::OnRspQryInvestorProductGroupMargin(CThostFtdcInvestorProductGroupMarginField *pInvestorProductGroupMargin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryInvestorProductGroupMargin!=NULL)
	{
		if(pInvestorProductGroupMargin == NULL)
		{
			CThostFtdcInvestorProductGroupMarginField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryInvestorProductGroupMargin)_OnRspQryInvestorProductGroupMargin)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryInvestorProductGroupMargin)_OnRspQryInvestorProductGroupMargin)(pInvestorProductGroupMargin,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ��������֤������Ӧ
void CTraderSpi::OnRspQryExchangeMarginRate(CThostFtdcExchangeMarginRateField *pExchangeMarginRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ������������֤������Ӧ
void CTraderSpi::OnRspQryExchangeMarginRateAdjust(CThostFtdcExchangeMarginRateAdjustField *pExchangeMarginRateAdjust, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryExchangeMarginRateAdjust!=NULL)
	{
		if(pExchangeMarginRateAdjust == NULL)
		{
			CThostFtdcExchangeMarginRateAdjustField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryExchangeMarginRateAdjust)_OnRspQryExchangeMarginRateAdjust)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryExchangeMarginRateAdjust)_OnRspQryExchangeMarginRateAdjust)(pExchangeMarginRateAdjust,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}


///�����ѯ��Ʒ��
void CTraderSpi::OnRspQryProductGroup(CThostFtdcProductGroupField *pProductGroup, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) {};

///�����ѯ�����̺�Լ����������Ӧ
void CTraderSpi::OnRspQryMMInstrumentCommissionRate(CThostFtdcMMInstrumentCommissionRateField *pMMInstrumentCommissionRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) {};

///�����ѯ��������Ȩ��Լ��������Ӧ
void CTraderSpi::OnRspQryMMOptionInstrCommRate(CThostFtdcMMOptionInstrCommRateField *pMMOptionInstrCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) {};

///�����ѯ������������Ӧ
void CTraderSpi::OnRspQryInstrumentOrderCommRate(CThostFtdcInstrumentOrderCommRateField *pInstrumentOrderCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) {};

///�����ѯ������Ӧ
void CTraderSpi::OnRspQryExchangeRate(CThostFtdcExchangeRateField *pExchangeRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryExchangeRate!=NULL)
	{
		if(pExchangeRate == NULL)
		{
			CThostFtdcExchangeRateField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryExchangeRate)_OnRspQryExchangeRate)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryExchangeRate)_OnRspQryExchangeRate)(pExchangeRate,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ�����������Ա����Ȩ����Ӧ
void CTraderSpi::OnRspQrySecAgentACIDMap(CThostFtdcSecAgentACIDMapField *pSecAgentACIDMap, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///�����ѯ��Ʒ���ۻ���
void CTraderSpi::OnRspQryProductExchRate(CThostFtdcProductExchRateField *pProductExchRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryProductExchRate!=NULL)
	{
		if(pProductExchRate == NULL)
		{
			CThostFtdcProductExchRateField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryProductExchRate)_OnRspQryProductExchRate)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryProductExchRate)_OnRspQryProductExchRate)(pProductExchRate,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯѯ����Ӧ
void CTraderSpi::OnRspQryForQuote(CThostFtdcForQuoteField *pForQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryForQuote!=NULL)
	{
		if(pForQuote == NULL)
		{
			CThostFtdcForQuoteField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryForQuote)_OnRspQryForQuote)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryForQuote)_OnRspQryForQuote)(pForQuote,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///ѯ��֪ͨ
void CTraderSpi::OnRtnForQuoteRsp(CThostFtdcForQuoteRspField *pForQuoteRsp)
{
	if(_OnRtnForQuoteRsp!=NULL)
	{
		if(pForQuoteRsp == NULL)
		{
			CThostFtdcForQuoteRspField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnForQuoteRsp)_OnRtnForQuoteRsp)(&req);
		}
		else
			((DefOnRtnForQuoteRsp)_OnRtnForQuoteRsp)(pForQuoteRsp);
	}
}

///ѯ��¼��������Ӧ
void CTraderSpi::OnRspForQuoteInsert(CThostFtdcInputForQuoteField *pInputForQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryForQuote!=NULL)
	{
		if(pInputForQuote == NULL)
		{
			CThostFtdcInputForQuoteField req;
			memset(&req,0,sizeof(req));
			((DefOnRspForQuoteInsert)_OnRspForQuoteInsert)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspForQuoteInsert)_OnRspForQuoteInsert)(pInputForQuote,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///ѯ��¼�����ر�
void CTraderSpi::OnErrRtnForQuoteInsert(CThostFtdcInputForQuoteField *pInputForQuote, CThostFtdcRspInfoField *pRspInfo)
{
	if(_OnErrRtnForQuoteInsert!=NULL)
	{
		if(pInputForQuote == NULL)
		{
			CThostFtdcInputForQuoteField req;
			memset(&req,0,sizeof(req));
			((DefOnErrRtnForQuoteInsert)_OnErrRtnForQuoteInsert)(&req,repareInfo(pRspInfo));
		}
		else
			((DefOnErrRtnForQuoteInsert)_OnErrRtnForQuoteInsert)(pInputForQuote,repareInfo(pRspInfo));
	}
}

///�����ѯ������Ӧ
void CTraderSpi::OnRspQryQuote(CThostFtdcQuoteField *pQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryForQuote!=NULL)
	{
		if(pQuote == NULL)
		{
			CThostFtdcQuoteField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryQuote)_OnRspQryQuote)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryQuote)_OnRspQryQuote)(pQuote,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///����֪ͨ
void CTraderSpi::OnRtnQuote(CThostFtdcQuoteField *pQuote)
{
	if(_OnRtnQuote!=NULL)
	{
		if(pQuote == NULL)
		{
			CThostFtdcQuoteField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnQuote)_OnRtnQuote)(&req);
		}
		else
			((DefOnRtnQuote)_OnRtnQuote)(pQuote);
	}
}

///����¼��������Ӧ
void CTraderSpi::OnRspQuoteInsert(CThostFtdcInputQuoteField *pInputQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQuoteInsert!=NULL)
	{
		if(pInputQuote == NULL)
		{
			CThostFtdcInputQuoteField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQuoteInsert)_OnRspQuoteInsert)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQuoteInsert)_OnRspQuoteInsert)(pInputQuote,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///����¼�����ر�
void CTraderSpi::OnErrRtnQuoteInsert(CThostFtdcInputQuoteField *pInputQuote, CThostFtdcRspInfoField *pRspInfo)
{
	if(_OnErrRtnQuoteInsert!=NULL)
	{
		if(pInputQuote == NULL)
		{
			CThostFtdcInputQuoteField req;
			memset(&req,0,sizeof(req));
			((DefOnErrRtnQuoteInsert)_OnErrRtnQuoteInsert)(&req,repareInfo(pRspInfo));
		}
		else
			((DefOnErrRtnQuoteInsert)_OnErrRtnQuoteInsert)(pInputQuote,repareInfo(pRspInfo));
	}
}

///���۲���������Ӧ
void CTraderSpi::OnRspQuoteAction(CThostFtdcInputQuoteActionField *pInputQuoteAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQuoteAction!=NULL)
	{
		if(pInputQuoteAction == NULL)
		{
			CThostFtdcInputQuoteActionField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQuoteAction)_OnRspQuoteAction)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQuoteAction)_OnRspQuoteAction)(pInputQuoteAction,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///���۲�������ر�
void CTraderSpi::OnErrRtnQuoteAction(CThostFtdcQuoteActionField *pQuoteAction, CThostFtdcRspInfoField *pRspInfo)
{
	if(_OnErrRtnQuoteAction!=NULL)
	{
		if(pQuoteAction == NULL)
		{
			CThostFtdcQuoteActionField req;
			memset(&req,0,sizeof(req));
			((DefOnErrRtnQuoteAction)_OnErrRtnQuoteAction)(&req,repareInfo(pRspInfo));
		}
		else
			((DefOnErrRtnQuoteAction)_OnErrRtnQuoteAction)(pQuoteAction,repareInfo(pRspInfo));
	}
}


///�����ѯִ��������Ӧ
void CTraderSpi::OnRspQryExecOrder(CThostFtdcExecOrderField *pExecOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryExecOrder!=NULL)
	{
		if(pExecOrder == NULL)
		{
			CThostFtdcExecOrderField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryExecOrder)_OnRspQryExecOrder)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryExecOrder)_OnRspQryExecOrder)(pExecOrder,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///ִ������֪ͨ
void CTraderSpi::OnRtnExecOrder(CThostFtdcExecOrderField *pExecOrder)
{
	if(_OnRtnExecOrder!=NULL)
	{
		if(pExecOrder == NULL)
		{
			CThostFtdcExecOrderField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnExecOrder)_OnRtnExecOrder)(&req);
		}
		else
			((DefOnRtnExecOrder)_OnRtnExecOrder)(pExecOrder);
	}
}

///ִ������¼��������Ӧ
void CTraderSpi::OnRspExecOrderInsert(CThostFtdcInputExecOrderField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspExecOrderInsert!=NULL)
	{
		if(pInputExecOrder == NULL)
		{
			CThostFtdcInputExecOrderField req;
			memset(&req,0,sizeof(req));
			((DefOnRspExecOrderInsert)_OnRspExecOrderInsert)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspExecOrderInsert)_OnRspExecOrderInsert)(pInputExecOrder,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///ִ������¼�����ر�
void CTraderSpi::OnErrRtnExecOrderInsert(CThostFtdcInputExecOrderField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo)
{
	if(_OnErrRtnExecOrderInsert!=NULL)
	{
		if(pInputExecOrder == NULL)
		{
			CThostFtdcInputExecOrderField req;
			memset(&req,0,sizeof(req));
			((DefOnErrRtnExecOrderInsert)_OnErrRtnExecOrderInsert)(&req,repareInfo(pRspInfo));
		}
		else
			((DefOnErrRtnExecOrderInsert)_OnErrRtnExecOrderInsert)(pInputExecOrder,repareInfo(pRspInfo));
	}
}


///ִ���������������Ӧ
void CTraderSpi::OnRspExecOrderAction(CThostFtdcInputExecOrderActionField *pInputExecOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspExecOrderAction!=NULL)
	{
		if(pInputExecOrderAction == NULL)
		{
			CThostFtdcInputExecOrderActionField req;
			memset(&req,0,sizeof(req));
			((DefOnRspExecOrderAction)_OnRspExecOrderAction)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspExecOrderAction)_OnRspExecOrderAction)(pInputExecOrderAction,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///ִ�������������ر�
void CTraderSpi::OnErrRtnExecOrderAction(CThostFtdcExecOrderActionField *pExecOrderAction, CThostFtdcRspInfoField *pRspInfo)
{
	if(_OnErrRtnExecOrderAction!=NULL)
	{
		if(pExecOrderAction == NULL)
		{
			CThostFtdcExecOrderActionField req;
			memset(&req,0,sizeof(req));
			((DefOnErrRtnExecOrderAction)_OnErrRtnExecOrderAction)(&req,repareInfo(pRspInfo));
		}
		else
			((DefOnErrRtnExecOrderAction)_OnErrRtnExecOrderAction)(pExecOrderAction,repareInfo(pRspInfo));
	}
}

///������������������Ӧ
void CTraderSpi::OnRspBatchOrderAction(CThostFtdcInputBatchOrderActionField *pInputBatchOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) {};

///�������¼��������Ӧ
void CTraderSpi::OnRspCombActionInsert(CThostFtdcInputCombActionField *pInputCombAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspCombActionInsert!=NULL)
	{
		if(pInputCombAction == NULL)
		{
			CThostFtdcInputCombActionField req;
			memset(&req,0,sizeof(req));
			((DefOnRspCombActionInsert)_OnRspCombActionInsert)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspCombActionInsert)_OnRspCombActionInsert)(pInputCombAction,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ��Ϻ�Լ��ȫϵ����Ӧ
void CTraderSpi::OnRspQryCombInstrumentGuard(CThostFtdcCombInstrumentGuardField *pCombInstrumentGuard, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryCombInstrumentGuard!=NULL)
	{
		if(pCombInstrumentGuard == NULL)
		{
			CThostFtdcCombInstrumentGuardField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryCombInstrumentGuard)_OnRspQryCombInstrumentGuard)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryCombInstrumentGuard)_OnRspQryCombInstrumentGuard)(pCombInstrumentGuard,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�����ѯ���������Ӧ
void CTraderSpi::OnRspQryCombAction(CThostFtdcCombActionField *pCombAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{
	if(_OnRspQryCombAction!=NULL)
	{
		if(pCombAction == NULL)
		{
			CThostFtdcCombActionField req;
			memset(&req,0,sizeof(req));
			((DefOnRspQryCombAction)_OnRspQryCombAction)(&req,repareInfo(pRspInfo), nRequestID, bIsLast);
		}
		else
			((DefOnRspQryCombAction)_OnRspQryCombAction)(pCombAction,repareInfo(pRspInfo), nRequestID, bIsLast);
	}
}

///�������֪ͨ
void CTraderSpi::OnRtnCombAction(CThostFtdcCombActionField *pCombAction)
{
	if(_OnRtnCombAction!=NULL)
	{
		if(pCombAction == NULL)
		{
			CThostFtdcCombActionField req;
			memset(&req,0,sizeof(req));
			((DefOnRtnCombAction)_OnRtnCombAction)(&req);
		}
		else
			((DefOnRtnCombAction)_OnRtnCombAction)(pCombAction);
	}
}

///�������¼�����ر�
void CTraderSpi::OnErrRtnCombActionInsert(CThostFtdcInputCombActionField *pInputCombAction, CThostFtdcRspInfoField *pRspInfo)
{
	if(_OnErrRtnCombActionInsert!=NULL)
	{
		if(pInputCombAction == NULL)
		{
			CThostFtdcInputCombActionField req;
			memset(&req,0,sizeof(req));
			((DefOnErrRtnCombActionInsert)_OnErrRtnCombActionInsert)(&req,repareInfo(pRspInfo));
		}
		else
			((DefOnErrRtnCombActionInsert)_OnErrRtnCombActionInsert)(pInputCombAction,repareInfo(pRspInfo));
	}
}

//����յ��շ����Ĵ���
CThostFtdcRspInfoField rif;
CThostFtdcRspInfoField* CTraderSpi::repareInfo(CThostFtdcRspInfoField *pRspInfo)
{
	if(pRspInfo==NULL)
	{
		memset(&rif,0,sizeof(rif));
		return &rif;
	}
	else
		return pRspInfo;
}

