
#pragma once
#define TRADEAPI_API __declspec(dllexport)
#define WINAPI      __stdcall
#define WIN32_LEAN_AND_MEAN             //  �� Windows ͷ�ļ����ų�����ʹ�õ���Ϣ

#include "stdafx.h"
#include ".\api\USTPFtdcTraderApi.h"
#include ".\api\USTPFtdcUserApiDataType.h"
#include ".\api\USTPFtdcUserApiStruct.h"

// UserApi����
CUstpFtdcTraderApi* pUserApi;

//�ص�����
void* _OnFrontConnected;///���ͻ����뽻�׺�̨������ͨ������ʱ����δ��¼ǰ�����÷��������á�
void* _OnFrontDisconnected;///���ͻ����뽻�׺�̨ͨ�����ӶϿ�ʱ���÷��������á���������������API���Զ��������ӣ��ͻ��˿ɲ�������
void* _OnHeartBeatWarning;///������ʱ���档����ʱ��δ�յ�����ʱ���÷��������á�
void* _OnPackageStart;///���Ļص���ʼ֪ͨ����API�յ�һ�����ĺ����ȵ��ñ�������Ȼ���Ǹ�������Ļص�������Ǳ��Ļص�����֪ͨ��
void* _OnPackageEnd;///���Ļص�����֪ͨ����API�յ�һ�����ĺ����ȵ��ñ��Ļص���ʼ֪ͨ��Ȼ���Ǹ�������Ļص��������ñ�������
void* _OnRspError;///����Ӧ��
void* _OnRspUserLogin;///���ǰ��ϵͳ�û���¼Ӧ��
void* _OnRspUserLogout;///�û��˳�Ӧ��
void* _OnRspUserPasswordUpdate;///�û������޸�Ӧ��
void* _OnRspOrderInsert;///����¼��Ӧ��
void* _OnRspOrderAction;///��������Ӧ��
void* _OnRspQuoteInsert;///����¼��Ӧ��
void* _OnRspQuoteAction;///���۲���Ӧ��
void* _OnRspForQuote;///ѯ������Ӧ��
void* _OnRspMarginCombAction;///�ͻ��������Ӧ��
void* _OnRtnFlowMessageCancel;///����������֪ͨ
void* _OnRtnTrade;///�ɽ��ر�
void* _OnRtnOrder;///�����ر�
void* _OnErrRtnOrderInsert;///����¼�����ر�
void* _OnErrRtnOrderAction;///������������ر�
void* _OnRtnInstrumentStatus;///��Լ����״̬֪ͨ
void* _OnRtnInvestorAccountDeposit;///�˻������ر�
void* _OnRtnQuote;///���ۻر�
void* _OnErrRtnQuoteInsert;///����¼�����ر�
void* _OnRtnForQuote;///ѯ�ۻر�
void* _OnRtnMarginCombinationLeg;///������Ϲ���֪ͨ
void* _OnRtnMarginCombAction;///�ͻ��������ȷ��
void* _OnRspQryOrder;///������ѯӦ��
void* _OnRspQryTrade;///�ɽ�����ѯӦ��
void* _OnRspQryUserInvestor;///����Ͷ�����˻���ѯӦ��
void* _OnRspQryTradingCode;///���ױ����ѯӦ��
void* _OnRspQryInvestorAccount;///Ͷ�����ʽ��˻���ѯӦ��
void* _OnRspQryInstrument;///��Լ��ѯӦ��
void* _OnRspQryExchange;///��������ѯӦ��
void* _OnRspQryInvestorPosition;///Ͷ���ֲֲ߳�ѯӦ��
void* _OnRspSubscribeTopic;///��������Ӧ��
void* _OnRspQryComplianceParam;///�Ϲ������ѯӦ��
void* _OnRspQryTopic;///�����ѯӦ��
void* _OnRspQryInvestorFee;///Ͷ�����������ʲ�ѯӦ��
void* _OnRspQryInvestorMargin;///Ͷ���߱�֤���ʲ�ѯӦ��
void* _OnRspQryInvestorCombPosition;///���ױ�����ϳֲֲ�ѯӦ��
void* _OnRspQryInvestorLegPosition;///���ױ��뵥�ȳֲֲ�ѯӦ��
void* _OnRspQryExchangeRate;///������ʲ�ѯӦ��

///���ͻ����뽻�׺�̨������ͨ������ʱ����δ��¼ǰ�����÷��������á�
typedef int (WINAPI *DefOnFrontConnected)();
	
///���ͻ����뽻�׺�̨ͨ�����ӶϿ�ʱ���÷��������á���������������API���Զ��������ӣ��ͻ��˿ɲ�������
typedef int (WINAPI *DefOnFrontDisconnected)(int nReason);
		
///������ʱ���档����ʱ��δ�յ�����ʱ���÷��������á�
typedef int (WINAPI *DefOnHeartBeatWarning)(int nTimeLapse);
	
///���Ļص���ʼ֪ͨ����API�յ�һ�����ĺ����ȵ��ñ�������Ȼ���Ǹ�������Ļص�������Ǳ��Ļص�����֪ͨ��
///@param nTopicID ������루��˽���������������������ȣ�
///@param nSequenceNo �������
typedef int (WINAPI *DefOnPackageStart)(int nTopicID, int nSequenceNo);
	
///���Ļص�����֪ͨ����API�յ�һ�����ĺ����ȵ��ñ��Ļص���ʼ֪ͨ��Ȼ���Ǹ�������Ļص��������ñ�������
///@param nTopicID ������루��˽���������������������ȣ�
///@param nSequenceNo �������
typedef int (WINAPI *DefOnPackageEnd)(int nTopicID, int nSequenceNo);

///����Ӧ��
typedef int (WINAPI *DefOnRspError)(CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///���ǰ��ϵͳ�û���¼Ӧ��
typedef int (WINAPI *DefOnRspUserLogin)(CUstpFtdcRspUserLoginField *pRspUserLogin, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///�û��˳�Ӧ��
typedef int (WINAPI *DefOnRspUserLogout)(CUstpFtdcRspUserLogoutField *pRspUserLogout, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///�û������޸�Ӧ��
typedef int (WINAPI *DefOnRspUserPasswordUpdate)(CUstpFtdcUserPasswordUpdateField *pUserPasswordUpdate, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///����¼��Ӧ��
typedef int (WINAPI *DefOnRspOrderInsert)(CUstpFtdcInputOrderField *pInputOrder, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///��������Ӧ��
typedef int (WINAPI *DefOnRspOrderAction)(CUstpFtdcOrderActionField *pOrderAction, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///����¼��Ӧ��
typedef int (WINAPI *DefOnRspQuoteInsert)(CUstpFtdcInputQuoteField *pInputQuote, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///���۲���Ӧ��
typedef int (WINAPI *DefOnRspQuoteAction)(CUstpFtdcQuoteActionField *pQuoteAction, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///ѯ������Ӧ��
typedef int (WINAPI *DefOnRspForQuote)(CUstpFtdcReqForQuoteField *pReqForQuote, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///�ͻ��������Ӧ��
typedef int (WINAPI *DefOnRspMarginCombAction)(CUstpFtdcInputMarginCombActionField *pInputMarginCombAction, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///����������֪ͨ
typedef int (WINAPI *DefOnRtnFlowMessageCancel)(CUstpFtdcFlowMessageCancelField *pFlowMessageCancel) ;

///�ɽ��ر�
typedef int (WINAPI *DefOnRtnTrade)(CUstpFtdcTradeField *pTrade) ;

///�����ر�
typedef int (WINAPI *DefOnRtnOrder)(CUstpFtdcOrderField *pOrder) ;

///����¼�����ر�
typedef int (WINAPI *DefOnErrRtnOrderInsert)(CUstpFtdcInputOrderField *pInputOrder, CUstpFtdcRspInfoField *pRspInfo) ;

///������������ر�
typedef int (WINAPI *DefOnErrRtnOrderAction)(CUstpFtdcOrderActionField *pOrderAction, CUstpFtdcRspInfoField *pRspInfo) ;

///��Լ����״̬֪ͨ
typedef int (WINAPI *DefOnRtnInstrumentStatus)(CUstpFtdcInstrumentStatusField *pInstrumentStatus) ;

///�˻������ر�
typedef int (WINAPI *DefOnRtnInvestorAccountDeposit)(CUstpFtdcInvestorAccountDepositResField *pInvestorAccountDepositRes) ;

///���ۻر�
typedef int (WINAPI *DefOnRtnQuote)(CUstpFtdcRtnQuoteField *pRtnQuote) ;

///����¼�����ر�
typedef int (WINAPI *DefOnErrRtnQuoteInsert)(CUstpFtdcInputQuoteField *pInputQuote, CUstpFtdcRspInfoField *pRspInfo) ;

///ѯ�ۻر�
typedef int (WINAPI *DefOnRtnForQuote)(CUstpFtdcReqForQuoteField *pReqForQuote) ;

///������Ϲ���֪ͨ
typedef int (WINAPI *DefOnRtnMarginCombinationLeg)(CUstpFtdcMarginCombinationLegField *pMarginCombinationLeg) ;

///�ͻ��������ȷ��
typedef int (WINAPI *DefOnRtnMarginCombAction)(CUstpFtdcInputMarginCombActionField *pInputMarginCombAction) ;

///������ѯӦ��
typedef int (WINAPI *DefOnRspQryOrder)(CUstpFtdcOrderField *pOrder, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///�ɽ�����ѯӦ��
typedef int (WINAPI *DefOnRspQryTrade)(CUstpFtdcTradeField *pTrade, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///����Ͷ�����˻���ѯӦ��
typedef int (WINAPI *DefOnRspQryUserInvestor)(CUstpFtdcRspUserInvestorField *pRspUserInvestor, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///���ױ����ѯӦ��
typedef int (WINAPI *DefOnRspQryTradingCode)(CUstpFtdcRspTradingCodeField *pRspTradingCode, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///Ͷ�����ʽ��˻���ѯӦ��
typedef int (WINAPI *DefOnRspQryInvestorAccount)(CUstpFtdcRspInvestorAccountField *pRspInvestorAccount, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///��Լ��ѯӦ��
typedef int (WINAPI *DefOnRspQryInstrument)(CUstpFtdcRspInstrumentField *pRspInstrument, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///��������ѯӦ��
typedef int (WINAPI *DefOnRspQryExchange)(CUstpFtdcRspExchangeField *pRspExchange, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///Ͷ���ֲֲ߳�ѯӦ��
typedef int (WINAPI *DefOnRspQryInvestorPosition)(CUstpFtdcRspInvestorPositionField *pRspInvestorPosition, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///��������Ӧ��
typedef int (WINAPI *DefOnRspSubscribeTopic)(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///�Ϲ������ѯӦ��
typedef int (WINAPI *DefOnRspQryComplianceParam)(CUstpFtdcRspComplianceParamField *pRspComplianceParam, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///�����ѯӦ��
typedef int (WINAPI *DefOnRspQryTopic)(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///Ͷ�����������ʲ�ѯӦ��
typedef int (WINAPI *DefOnRspQryInvestorFee)(CUstpFtdcInvestorFeeField *pInvestorFee, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///Ͷ���߱�֤���ʲ�ѯӦ��
typedef int (WINAPI *DefOnRspQryInvestorMargin)(CUstpFtdcInvestorMarginField *pInvestorMargin, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///���ױ�����ϳֲֲ�ѯӦ��
typedef int (WINAPI *DefOnRspQryInvestorCombPosition)(CUstpFtdcRspInvestorCombPositionField *pRspInvestorCombPosition, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///���ױ��뵥�ȳֲֲ�ѯӦ��
typedef int (WINAPI *DefOnRspQryInvestorLegPosition)(CUstpFtdcRspInvestorLegPositionField *pRspInvestorLegPosition, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///������ʲ�ѯӦ��
typedef int (WINAPI *DefOnRspQryExchangeRate)(CUstpFtdcRspExchangeRateField *pRspExchangeRate, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

class CTraderSpi : public CUstpFtdcTraderSpi
{
public:
	///���ͻ����뽻�׺�̨������ͨ������ʱ����δ��¼ǰ�����÷��������á�
	virtual void OnFrontConnected();
	
	///���ͻ����뽻�׺�̨ͨ�����ӶϿ�ʱ���÷��������á���������������API���Զ��������ӣ��ͻ��˿ɲ�������
	///@param nReason ����ԭ��
	///        0x1001 �����ʧ��
	///        0x1002 ����дʧ��
	///        0x2001 ����������ʱ
	///        0x2002 ��������ʧ��
	///        0x2003 �յ�������
	virtual void OnFrontDisconnected(int nReason);
		
	///������ʱ���档����ʱ��δ�յ�����ʱ���÷��������á�
	///@param nTimeLapse �����ϴν��ձ��ĵ�ʱ��
	virtual void OnHeartBeatWarning(int nTimeLapse);
	
	///���Ļص���ʼ֪ͨ����API�յ�һ�����ĺ����ȵ��ñ�������Ȼ���Ǹ�������Ļص�������Ǳ��Ļص�����֪ͨ��
	///@param nTopicID ������루��˽���������������������ȣ�
	///@param nSequenceNo �������
	virtual void OnPackageStart(int nTopicID, int nSequenceNo);
	
	///���Ļص�����֪ͨ����API�յ�һ�����ĺ����ȵ��ñ��Ļص���ʼ֪ͨ��Ȼ���Ǹ�������Ļص��������ñ�������
	///@param nTopicID ������루��˽���������������������ȣ�
	///@param nSequenceNo �������
	virtual void OnPackageEnd(int nTopicID, int nSequenceNo);

	///����Ӧ��
	virtual void OnRspError(CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///���ǰ��ϵͳ�û���¼Ӧ��
	virtual void OnRspUserLogin(CUstpFtdcRspUserLoginField *pRspUserLogin, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///�û��˳�Ӧ��
	virtual void OnRspUserLogout(CUstpFtdcRspUserLogoutField *pRspUserLogout, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///�û������޸�Ӧ��
	virtual void OnRspUserPasswordUpdate(CUstpFtdcUserPasswordUpdateField *pUserPasswordUpdate, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///����¼��Ӧ��
	virtual void OnRspOrderInsert(CUstpFtdcInputOrderField *pInputOrder, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///��������Ӧ��
	virtual void OnRspOrderAction(CUstpFtdcOrderActionField *pOrderAction, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///����¼��Ӧ��
	virtual void OnRspQuoteInsert(CUstpFtdcInputQuoteField *pInputQuote, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///���۲���Ӧ��
	virtual void OnRspQuoteAction(CUstpFtdcQuoteActionField *pQuoteAction, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///ѯ������Ӧ��
	virtual void OnRspForQuote(CUstpFtdcReqForQuoteField *pReqForQuote, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///�ͻ��������Ӧ��
	virtual void OnRspMarginCombAction(CUstpFtdcInputMarginCombActionField *pInputMarginCombAction, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///����������֪ͨ
	virtual void OnRtnFlowMessageCancel(CUstpFtdcFlowMessageCancelField *pFlowMessageCancel) ;

	///�ɽ��ر�
	virtual void OnRtnTrade(CUstpFtdcTradeField *pTrade) ;

	///�����ر�
	virtual void OnRtnOrder(CUstpFtdcOrderField *pOrder) ;

	///����¼�����ر�
	virtual void OnErrRtnOrderInsert(CUstpFtdcInputOrderField *pInputOrder, CUstpFtdcRspInfoField *pRspInfo) ;

	///������������ر�
	virtual void OnErrRtnOrderAction(CUstpFtdcOrderActionField *pOrderAction, CUstpFtdcRspInfoField *pRspInfo) ;

	///��Լ����״̬֪ͨ
	virtual void OnRtnInstrumentStatus(CUstpFtdcInstrumentStatusField *pInstrumentStatus) ;

	///�˻������ر�
	virtual void OnRtnInvestorAccountDeposit(CUstpFtdcInvestorAccountDepositResField *pInvestorAccountDepositRes) ;

	///���ۻر�
	virtual void OnRtnQuote(CUstpFtdcRtnQuoteField *pRtnQuote) ;

	///����¼�����ر�
	virtual void OnErrRtnQuoteInsert(CUstpFtdcInputQuoteField *pInputQuote, CUstpFtdcRspInfoField *pRspInfo) ;

	///ѯ�ۻر�
	virtual void OnRtnForQuote(CUstpFtdcReqForQuoteField *pReqForQuote) ;

	///������Ϲ���֪ͨ
	virtual void OnRtnMarginCombinationLeg(CUstpFtdcMarginCombinationLegField *pMarginCombinationLeg) ;

	///�ͻ��������ȷ��
	virtual void OnRtnMarginCombAction(CUstpFtdcInputMarginCombActionField *pInputMarginCombAction) ;

	///������ѯӦ��
	virtual void OnRspQryOrder(CUstpFtdcOrderField *pOrder, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///�ɽ�����ѯӦ��
	virtual void OnRspQryTrade(CUstpFtdcTradeField *pTrade, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///����Ͷ�����˻���ѯӦ��
	virtual void OnRspQryUserInvestor(CUstpFtdcRspUserInvestorField *pRspUserInvestor, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///���ױ����ѯӦ��
	virtual void OnRspQryTradingCode(CUstpFtdcRspTradingCodeField *pRspTradingCode, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///Ͷ�����ʽ��˻���ѯӦ��
	virtual void OnRspQryInvestorAccount(CUstpFtdcRspInvestorAccountField *pRspInvestorAccount, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///��Լ��ѯӦ��
	virtual void OnRspQryInstrument(CUstpFtdcRspInstrumentField *pRspInstrument, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///��������ѯӦ��
	virtual void OnRspQryExchange(CUstpFtdcRspExchangeField *pRspExchange, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///Ͷ���ֲֲ߳�ѯӦ��
	virtual void OnRspQryInvestorPosition(CUstpFtdcRspInvestorPositionField *pRspInvestorPosition, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///��������Ӧ��
	virtual void OnRspSubscribeTopic(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///�Ϲ������ѯӦ��
	virtual void OnRspQryComplianceParam(CUstpFtdcRspComplianceParamField *pRspComplianceParam, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///�����ѯӦ��
	virtual void OnRspQryTopic(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///Ͷ�����������ʲ�ѯӦ��
	virtual void OnRspQryInvestorFee(CUstpFtdcInvestorFeeField *pInvestorFee, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///Ͷ���߱�֤���ʲ�ѯӦ��
	virtual void OnRspQryInvestorMargin(CUstpFtdcInvestorMarginField *pInvestorMargin, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///���ױ�����ϳֲֲ�ѯӦ��
	virtual void OnRspQryInvestorCombPosition(CUstpFtdcRspInvestorCombPositionField *pRspInvestorCombPosition, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///���ױ��뵥�ȳֲֲ�ѯӦ��
	virtual void OnRspQryInvestorLegPosition(CUstpFtdcRspInvestorLegPositionField *pRspInvestorLegPosition, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///������ʲ�ѯӦ��
	virtual void OnRspQryExchangeRate(CUstpFtdcRspExchangeRateField *pRspExchangeRate, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

private:
	CUstpFtdcRspInfoField* repareInfo(CUstpFtdcRspInfoField *pRspInfo);
	
	// �Ƿ��յ��ɹ�����Ӧ
	bool IsErrorRspInfo(CUstpFtdcRspInfoField *pRspInfo);
	
	// �Ƿ��ҵı����ر�
	bool IsMyOrder(CUstpFtdcOrderField *pOrder);
	
	// �Ƿ����ڽ��׵ı���
	bool IsTradingOrder(CUstpFtdcOrderField *pOrder);
};


