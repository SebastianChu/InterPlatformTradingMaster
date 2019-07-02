
#pragma once
#define TRADEAPI_API __declspec(dllexport)
#define WINAPI      __stdcall
#define WIN32_LEAN_AND_MEAN             //  �� Windows ͷ�ļ����ų�����ʹ�õ���Ϣ

#include "stdafx.h"
#include ".\api\ThostFtdcTraderApi.h"
#include ".\api\ThostFtdcUserApiDataType.h"
#include ".\api\ThostFtdcUserApiStruct.h"

// UserApi����
CThostFtdcTraderApi* pUserApi;

//�ص�����
void* _OnFrontConnected;	///���ͻ����뽻�׺�̨������ͨ������ʱ����δ��¼ǰ�����÷��������á�
void* _OnFrontDisconnected;	///���ͻ����뽻�׺�̨ͨ�����ӶϿ�ʱ���÷��������á���������������API���Զ��������ӣ��ͻ��˿ɲ�������
void* _OnHeartBeatWarning;	///������ʱ���档����ʱ��δ�յ�����ʱ���÷��������á�
void* _OnRspUserLogin;///��¼������Ӧ
void* _OnRspAuthenticate;///�ͻ�����֤��Ӧ
void* _OnRspUserLogout;///�ǳ�������Ӧ
void* _OnRspUserPasswordUpdate;///�û��������������Ӧ
void* _OnRspTradingAccountPasswordUpdate;///�ʽ��˻��������������Ӧ
void* _OnRspOrderInsert;///����¼��������Ӧ
void* _OnRspParkedOrderInsert;///Ԥ��¼��������Ӧ
void* _OnRspParkedOrderAction;///Ԥ�񳷵�¼��������Ӧ
void* _OnRspOrderAction;///��������������Ӧ
void* _OnRspQueryMaxOrderVolume;///��ѯ��󱨵�������Ӧ
void* _OnRspSettlementInfoConfirm;///Ͷ���߽�����ȷ����Ӧ
void* _OnRspRemoveParkedOrder;///ɾ��Ԥ����Ӧ
void* _OnRspRemoveParkedOrderAction;///ɾ��Ԥ�񳷵���Ӧ
void* _OnRspQryOrder;///�����ѯ������Ӧ
void* _OnRspQryTrade;///�����ѯ�ɽ���Ӧ
void* _OnRspQryInvestorPosition;///�����ѯͶ���ֲ߳���Ӧ
void* _OnRspQryTradingAccount;///�����ѯ�ʽ��˻���Ӧ
void* _OnRspQryInvestor;///�����ѯͶ������Ӧ
void* _OnRspQryTradingCode;///�����ѯ���ױ�����Ӧ
void* _OnRspQryInstrumentMarginRate;///�����ѯ��Լ��֤������Ӧ
void* _OnRspQryInstrumentCommissionRate;///�����ѯ��Լ����������Ӧ
void* _OnRspQryExchange;///�����ѯ��������Ӧ
void* _OnRspQryInstrument;///�����ѯ��Լ��Ӧ
void* _OnRspQryDepthMarketData;///�����ѯ������Ӧ
void* _OnRspQrySettlementInfo;///�����ѯͶ���߽�������Ӧ
void* _OnRspQryTransferBank;///�����ѯת��������Ӧ
void* _OnRspQryInvestorPositionDetail;///�����ѯͶ���ֲ߳���ϸ��Ӧ
void* _OnRspQryNotice;///�����ѯ�ͻ�֪ͨ��Ӧ
void* _OnRspQrySettlementInfoConfirm;///�����ѯ������Ϣȷ����Ӧ
void* _OnRspQryInvestorPositionCombineDetail;///�����ѯͶ���ֲ߳���ϸ��Ӧ
void* _OnRspQryCFMMCTradingAccountKey;///��ѯ��֤����ϵͳ���͹�˾�ʽ��˻���Կ��Ӧ
void* _OnRspQryAccountregister;///�����ѯ����ǩԼ��ϵ��Ӧ
void* _OnRspQryTransferSerial;///�����ѯת����ˮ��Ӧ
void* _OnRspError;///����Ӧ��
void* _OnRtnOrder;///����֪ͨ
void* _OnRtnTrade;///�ɽ�֪ͨ
void* _OnErrRtnOrderInsert;///����¼�����ر�
void* _OnErrRtnOrderAction;///������������ر�
void* _OnRtnInstrumentStatus;///��Լ����״̬֪ͨ
void* _OnRtnTradingNotice;///����֪ͨ
void* _OnRtnErrorConditionalOrder;///��ʾ������У�����
void* _OnRspQryContractBank;///�����ѯǩԼ������Ӧ
void* _OnRspQryParkedOrder;///�����ѯԤ����Ӧ
void* _OnRspQryParkedOrderAction;///�����ѯԤ�񳷵���Ӧ
void* _OnRspQryTradingNotice;///�����ѯ����֪ͨ��Ӧ
void* _OnRspQryBrokerTradingParams;///�����ѯ���͹�˾���ײ�����Ӧ
void* _OnRspQryBrokerTradingAlgos;///�����ѯ���͹�˾�����㷨��Ӧ
void* _OnRtnFromBankToFutureByBank;///���з��������ʽ�ת�ڻ�֪ͨ
void* _OnRtnFromFutureToBankByBank;///���з����ڻ��ʽ�ת����֪ͨ
void* _OnRtnRepealFromBankToFutureByBank;///���з����������ת�ڻ�֪ͨ
void* _OnRtnRepealFromFutureToBankByBank;///���з�������ڻ�ת����֪ͨ
void* _OnRtnFromBankToFutureByFuture;///�ڻ����������ʽ�ת�ڻ�֪ͨ
void* _OnRtnFromFutureToBankByFuture;///�ڻ������ڻ��ʽ�ת����֪ͨ
void* _OnRtnRepealFromBankToFutureByFutureManual;///ϵͳ����ʱ�ڻ����ֹ������������ת�ڻ��������д�����Ϻ��̷��ص�֪ͨ
void* _OnRtnRepealFromFutureToBankByFutureManual;///ϵͳ����ʱ�ڻ����ֹ���������ڻ�ת�����������д�����Ϻ��̷��ص�֪ͨ
void* _OnRtnQueryBankBalanceByFuture;///�ڻ������ѯ�������֪ͨ
void* _OnErrRtnBankToFutureByFuture;///�ڻ����������ʽ�ת�ڻ�����ر�
void* _OnErrRtnFutureToBankByFuture;///�ڻ������ڻ��ʽ�ת���д���ر�
void* _OnErrRtnRepealBankToFutureByFutureManual;///ϵͳ����ʱ�ڻ����ֹ������������ת�ڻ�����ر�
void* _OnErrRtnRepealFutureToBankByFutureManual;///ϵͳ����ʱ�ڻ����ֹ���������ڻ�ת���д���ر�
void* _OnErrRtnQueryBankBalanceByFuture;///�ڻ������ѯ����������ر�
void* _OnRtnRepealFromBankToFutureByFuture;///�ڻ������������ת�ڻ��������д�����Ϻ��̷��ص�֪ͨ
void* _OnRtnRepealFromFutureToBankByFuture;///�ڻ���������ڻ�ת�����������д�����Ϻ��̷��ص�֪ͨ
void* _OnRspFromBankToFutureByFuture;///�ڻ����������ʽ�ת�ڻ�Ӧ��
void* _OnRspFromFutureToBankByFuture;///�ڻ������ڻ��ʽ�ת����Ӧ��
void* _OnRspQueryBankAccountMoneyByFuture;///�ڻ������ѯ�������Ӧ��

void* _OnRspQryOptionInstrCommRate;///�����ѯ��Ȩ��Լ��������Ӧ
void* _OnRspQryOptionInstrTradeCost;///�����ѯ��Ȩ���׳ɱ���Ӧ
void* _OnRspQryForQuote;///�����ѯѯ����Ӧ
void* _OnRspForQuoteInsert;///ѯ��¼��������Ӧ
void* _OnRspQryQuote;///�����ѯ������Ӧ
void* _OnRspQuoteInsert;///����¼��������Ӧ
void* _OnRspQuoteAction;///���۲���������Ӧ
void* _OnRspQryExecOrder;///�����ѯִ��������Ӧ
void* _OnRtnExecOrder;///ִ������֪ͨ
void* _OnRspExecOrderInsert;///ִ������¼��������Ӧ
void* _OnErrRtnExecOrderInsert;///ִ������¼�����ر�
void* _OnRspExecOrderAction;///ִ���������������Ӧ
void* _OnErrRtnExecOrderAction;///ִ�������������ر�

void* _OnRtnForQuoteRsp;///ѯ��֪ͨ
void* _OnErrRtnForQuoteInsert;///ѯ��¼�����ر�
void* _OnRtnQuote;///����֪ͨ
void* _OnErrRtnQuoteInsert;///����¼�����ر�
void* _OnErrRtnQuoteAction;///���۲�������ر�

void* _OnRspCombActionInsert;///�������¼��������Ӧ
void* _OnRspQryCombInstrumentGuard;///�����ѯ��Ϻ�Լ��ȫϵ����Ӧ
void* _OnRspQryCombAction;///�����ѯ���������Ӧ
void* _OnRtnCombAction;///�������֪ͨ
void* _OnErrRtnCombActionInsert;///�������¼�����ر�

void* _OnRspQryInvestorProductGroupMargin;///�����ѯͶ����Ʒ��/��Ʒ�ֱ�֤����Ӧ
void* _OnRspQryExchangeMarginRateAdjust;///�����ѯ������������֤������Ӧ
void* _OnRspQryExchangeRate;///�����ѯ������Ӧ
void* _OnRspQryProductExchRate;///�����ѯ��Ʒ���ۻ���

void* _OnRspBatchOrderAction;///������������������Ӧ
void* _OnRspQryProductGroup;///�����ѯ��Ʒ��
void* _OnRspQryMMInstrumentCommissionRate;///�����ѯ�����̺�Լ����������Ӧ
void* _OnRspQryMMOptionInstrCommRate;///�����ѯ��������Ȩ��Լ��������Ӧ
void* _OnRspQryInstrumentOrderCommRate;///�����ѯ������������Ӧ
void* _OnRtnBulletin;///����������֪ͨ
void* _OnErrRtnBatchOrderAction;///����������������ر�

///���ͻ����뽻�׺�̨������ͨ������ʱ����δ��¼ǰ�����÷��������á�
typedef int (WINAPI *DefOnFrontConnected)();
///���ͻ����뽻�׺�̨ͨ�����ӶϿ�ʱ���÷��������á���������������API���Զ��������ӣ��ͻ��˿ɲ�������
typedef int (WINAPI *DefOnFrontDisconnected)(int nReason);
///������ʱ���档����ʱ��δ�յ�����ʱ���÷��������á�
typedef int (WINAPI *DefOnHeartBeatWarning)(int nTimeLapse);
///��¼������Ӧ
typedef int (WINAPI *DefOnRspAuthenticate)(CThostFtdcRspAuthenticateField *pRspAuthenticateField, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///��¼������Ӧ
typedef int (WINAPI *DefOnRspUserLogin)(CThostFtdcRspUserLoginField *pRspUserLogin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�ǳ�������Ӧ
typedef int (WINAPI *DefOnRspUserLogout)(CThostFtdcUserLogoutField *pUserLogout, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�û��������������Ӧ
typedef int (WINAPI *DefOnRspUserPasswordUpdate)(CThostFtdcUserPasswordUpdateField *pUserPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�ʽ��˻��������������Ӧ
typedef int (WINAPI *DefOnRspTradingAccountPasswordUpdate)(CThostFtdcTradingAccountPasswordUpdateField *pTradingAccountPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///����¼��������Ӧ
typedef int (WINAPI *DefOnRspOrderInsert)(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///Ԥ��¼��������Ӧ
typedef int (WINAPI *DefOnRspParkedOrderInsert)(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///Ԥ�񳷵�¼��������Ӧ
typedef int (WINAPI *DefOnRspParkedOrderAction)(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///��������������Ӧ
typedef int (WINAPI *DefOnRspOrderAction)(CThostFtdcInputOrderActionField *pInputOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///��ѯ��󱨵�������Ӧ
typedef int (WINAPI *DefOnRspQueryMaxOrderVolume)(CThostFtdcQueryMaxOrderVolumeField *pQueryMaxOrderVolume, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///Ͷ���߽�����ȷ����Ӧ
typedef int (WINAPI *DefOnRspSettlementInfoConfirm)(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///ɾ��Ԥ����Ӧ
typedef int (WINAPI *DefOnRspRemoveParkedOrder)(CThostFtdcRemoveParkedOrderField *pRemoveParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///ɾ��Ԥ�񳷵���Ӧ
typedef int (WINAPI *DefOnRspRemoveParkedOrderAction)(CThostFtdcRemoveParkedOrderActionField *pRemoveParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯ������Ӧ
typedef int (WINAPI *DefOnRspQryOrder)(CThostFtdcOrderField *pOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯ�ɽ���Ӧ
typedef int (WINAPI *DefOnRspQryTrade)(CThostFtdcTradeField *pTrade, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯͶ���ֲ߳���Ӧ
typedef int (WINAPI *DefOnRspQryInvestorPosition)(CThostFtdcInvestorPositionField *pInvestorPosition, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯ�ʽ��˻���Ӧ
typedef int (WINAPI *DefOnRspQryTradingAccount)(CThostFtdcTradingAccountField *pTradingAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯͶ������Ӧ
typedef int (WINAPI *DefOnRspQryInvestor)(CThostFtdcInvestorField *pInvestor, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯ���ױ�����Ӧ
typedef int (WINAPI *DefOnRspQryTradingCode)(CThostFtdcTradingCodeField *pTradingCode, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯ��Լ��֤������Ӧ
typedef int (WINAPI *DefOnRspQryInstrumentMarginRate)(CThostFtdcInstrumentMarginRateField *pInstrumentMarginRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯ��Լ����������Ӧ
typedef int (WINAPI *DefOnRspQryInstrumentCommissionRate)(CThostFtdcInstrumentCommissionRateField *pInstrumentCommissionRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯ��Ȩ��Լ����������Ӧ
typedef int (WINAPI *DefOnRspQryQryOptionInstrCommRate)(CThostFtdcOptionInstrCommRateField *pOptionInstrCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯ��������Ӧ
typedef int (WINAPI *DefOnRspQryExchange)(CThostFtdcExchangeField *pExchange, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯ��Լ��Ӧ
typedef int (WINAPI *DefOnRspQryInstrument)(CThostFtdcInstrumentField *pInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯ������Ӧ
typedef int (WINAPI *DefOnRspQryDepthMarketData)(CThostFtdcDepthMarketDataField *pDepthMarketData, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯͶ���߽�������Ӧ
typedef int (WINAPI *DefOnRspQrySettlementInfo)(CThostFtdcSettlementInfoField *pSettlementInfo, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯ����ǩԼ��ϵ��Ӧ
typedef int (WINAPI *DefOnRspQryAccountregister)(CThostFtdcAccountregisterField *pAccountregister, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯת��������Ӧ
typedef int (WINAPI *DefOnRspQryTransferBank)(CThostFtdcTransferBankField *pTransferBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯͶ���ֲ߳���ϸ��Ӧ
typedef int (WINAPI *DefOnRspQryInvestorPositionDetail)(CThostFtdcInvestorPositionDetailField *pInvestorPositionDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯ�ͻ�֪ͨ��Ӧ
typedef int (WINAPI *DefOnRspQryNotice)(CThostFtdcNoticeField *pNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯ������Ϣȷ����Ӧ
typedef int (WINAPI *DefOnRspQrySettlementInfoConfirm)(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯͶ���ֲ߳���ϸ��Ӧ
typedef int (WINAPI *DefOnRspQryInvestorPositionCombineDetail)(CThostFtdcInvestorPositionCombineDetailField *pInvestorPositionCombineDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///��ѯ��֤����ϵͳ���͹�˾�ʽ��˻���Կ��Ӧ
typedef int (WINAPI *DefOnRspQryCFMMCTradingAccountKey)(CThostFtdcCFMMCTradingAccountKeyField *pCFMMCTradingAccountKey, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯת����ˮ��Ӧ
typedef int (WINAPI *DefOnRspQryTransferSerial)(CThostFtdcTransferSerialField *pTransferSerial, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///����Ӧ��
typedef int (WINAPI *DefOnRspError)(CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///����֪ͨ
typedef int (WINAPI *DefOnRtnOrder)(CThostFtdcOrderField *pOrder) ;
///�ɽ�֪ͨ
typedef int (WINAPI *DefOnRtnTrade)(CThostFtdcTradeField *pTrade) ;
///����¼�����ر�
typedef int (WINAPI *DefOnErrRtnOrderInsert)(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo) ;
///������������ر�
typedef int (WINAPI *DefOnErrRtnOrderAction)(CThostFtdcOrderActionField *pOrderAction, CThostFtdcRspInfoField *pRspInfo) ;
///��Լ����״̬֪ͨ
typedef int (WINAPI *DefOnRtnInstrumentStatus)(CThostFtdcInstrumentStatusField *pInstrumentStatus) ;
///����֪ͨ
typedef int (WINAPI *DefOnRtnTradingNotice)(CThostFtdcTradingNoticeInfoField *pTradingNoticeInfo) ;
///��ʾ������У�����
typedef int (WINAPI *DefOnRtnErrorConditionalOrder)(CThostFtdcErrorConditionalOrderField *pErrorConditionalOrder) ;
///�����ѯǩԼ������Ӧ
typedef int (WINAPI *DefOnRspQryContractBank)(CThostFtdcContractBankField *pContractBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯԤ����Ӧ
typedef int (WINAPI *DefOnRspQryParkedOrder)(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯԤ�񳷵���Ӧ
typedef int (WINAPI *DefOnRspQryParkedOrderAction)(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯ����֪ͨ��Ӧ
typedef int (WINAPI *DefOnRspQryTradingNotice)(CThostFtdcTradingNoticeField *pTradingNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯ���͹�˾���ײ�����Ӧ
typedef int (WINAPI *DefOnRspQryBrokerTradingParams)(CThostFtdcBrokerTradingParamsField *pBrokerTradingParams, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�����ѯ���͹�˾�����㷨��Ӧ
typedef int (WINAPI *DefOnRspQryBrokerTradingAlgos)(CThostFtdcBrokerTradingAlgosField *pBrokerTradingAlgos, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///���з��������ʽ�ת�ڻ�֪ͨ
typedef int (WINAPI *DefOnRtnFromBankToFutureByBank)(CThostFtdcRspTransferField *pRspTransfer) ;
///���з����ڻ��ʽ�ת����֪ͨ
typedef int (WINAPI *DefOnRtnFromFutureToBankByBank)(CThostFtdcRspTransferField *pRspTransfer) ;
///���з����������ת�ڻ�֪ͨ
typedef int (WINAPI *DefOnRtnRepealFromBankToFutureByBank)(CThostFtdcRspRepealField *pRspRepeal) ;
///���з�������ڻ�ת����֪ͨ
typedef int (WINAPI *DefOnRtnRepealFromFutureToBankByBank)(CThostFtdcRspRepealField *pRspRepeal) ;
///�ڻ����������ʽ�ת�ڻ�֪ͨ
typedef int (WINAPI *DefOnRtnFromBankToFutureByFuture)(CThostFtdcRspTransferField *pRspTransfer) ;
///�ڻ������ڻ��ʽ�ת����֪ͨ
typedef int (WINAPI *DefOnRtnFromFutureToBankByFuture)(CThostFtdcRspTransferField *pRspTransfer) ;
///ϵͳ����ʱ�ڻ����ֹ������������ת�ڻ��������д�����Ϻ��̷��ص�֪ͨ
typedef int (WINAPI *DefOnRtnRepealFromBankToFutureByFutureManual)(CThostFtdcRspRepealField *pRspRepeal) ;
///ϵͳ����ʱ�ڻ����ֹ���������ڻ�ת�����������д�����Ϻ��̷��ص�֪ͨ
typedef int (WINAPI *DefOnRtnRepealFromFutureToBankByFutureManual)(CThostFtdcRspRepealField *pRspRepeal) ;
///�ڻ������ѯ�������֪ͨ
typedef int (WINAPI *DefOnRtnQueryBankBalanceByFuture)(CThostFtdcNotifyQueryAccountField *pNotifyQueryAccount) ;
///�ڻ����������ʽ�ת�ڻ�����ر�
typedef int (WINAPI *DefOnErrRtnBankToFutureByFuture)(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo) ;
///�ڻ������ڻ��ʽ�ת���д���ر�
typedef int (WINAPI *DefOnErrRtnFutureToBankByFuture)(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo) ;
///ϵͳ����ʱ�ڻ����ֹ������������ת�ڻ�����ر�
typedef int (WINAPI *DefOnErrRtnRepealBankToFutureByFutureManual)(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo) ;
///ϵͳ����ʱ�ڻ����ֹ���������ڻ�ת���д���ر�
typedef int (WINAPI *DefOnErrRtnRepealFutureToBankByFutureManual)(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo) ;
///�ڻ������ѯ����������ر�
typedef int (WINAPI *DefOnErrRtnQueryBankBalanceByFuture)(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo) ;
///�ڻ������������ת�ڻ��������д�����Ϻ��̷��ص�֪ͨ
typedef int (WINAPI *DefOnRtnRepealFromBankToFutureByFuture)(CThostFtdcRspRepealField *pRspRepeal) ;
///�ڻ���������ڻ�ת�����������д�����Ϻ��̷��ص�֪ͨ
typedef int (WINAPI *DefOnRtnRepealFromFutureToBankByFuture)(CThostFtdcRspRepealField *pRspRepeal) ;
///�ڻ����������ʽ�ת�ڻ�Ӧ��
typedef int (WINAPI *DefOnRspFromBankToFutureByFuture)(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�ڻ������ڻ��ʽ�ת����Ӧ��
typedef int (WINAPI *DefOnRspFromFutureToBankByFuture)(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///�ڻ������ѯ�������Ӧ��
typedef int (WINAPI *DefOnRspQueryBankAccountMoneyByFuture)(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///�����ѯ��Ȩ��Լ��������Ӧ
typedef int (WINAPI *DefOnRspQryOptionInstrCommRate)(CThostFtdcOptionInstrCommRateField *pOptionInstrCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///�����ѯ��Ȩ���׳ɱ���Ӧ
typedef int (WINAPI *DefOnRspQryOptionInstrTradeCost)(CThostFtdcOptionInstrTradeCostField *pOptionInstrTradeCost, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///�����ѯѯ����Ӧ
typedef int (WINAPI *DefOnRspQryForQuote)(CThostFtdcForQuoteField *pForQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///ѯ��֪ͨ
typedef int (WINAPI *DefOnRtnForQuoteRsp)(CThostFtdcForQuoteRspField *pForQuoteRsp);
///ѯ��¼��������Ӧ
typedef int (WINAPI *DefOnRspForQuoteInsert)(CThostFtdcInputForQuoteField *pInputForQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///ѯ��¼�����ر�
typedef int (WINAPI *DefOnErrRtnForQuoteInsert)(CThostFtdcInputForQuoteField *pInputForQuote, CThostFtdcRspInfoField *pRspInfo);
///�����ѯ������Ӧ
typedef int (WINAPI *DefOnRspQryQuote)(CThostFtdcQuoteField *pQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///����֪ͨ
typedef int (WINAPI *DefOnRtnQuote)(CThostFtdcQuoteField *pQuote);
///����¼��������Ӧ
typedef int (WINAPI *DefOnRspQuoteInsert)(CThostFtdcInputQuoteField *pInputQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///����¼�����ر�
typedef int (WINAPI *DefOnErrRtnQuoteInsert)(CThostFtdcInputQuoteField *pInputQuote, CThostFtdcRspInfoField *pRspInfo);
///���۲���������Ӧ
typedef int (WINAPI *DefOnRspQuoteAction)(CThostFtdcInputQuoteActionField *pInputQuoteAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///���۲�������ر�
typedef int (WINAPI *DefOnErrRtnQuoteAction)(CThostFtdcQuoteActionField *pQuoteAction, CThostFtdcRspInfoField *pRspInfo);
///�����ѯִ��������Ӧ
typedef int (WINAPI *DefOnRspQryExecOrder)(CThostFtdcExecOrderField *pExecOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///ִ������֪ͨ
typedef int (WINAPI *DefOnRtnExecOrder)(CThostFtdcExecOrderField *pExecOrder);
///ִ������¼��������Ӧ
typedef int (WINAPI *DefOnRspExecOrderInsert)(CThostFtdcInputExecOrderField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///ִ������¼�����ر�
typedef int (WINAPI *DefOnErrRtnExecOrderInsert)(CThostFtdcInputExecOrderField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo);
///ִ���������������Ӧ
typedef int (WINAPI *DefOnRspExecOrderAction)(CThostFtdcInputExecOrderActionField *pInputExecOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///ִ�������������ر�
typedef int (WINAPI *DefOnErrRtnExecOrderAction)(CThostFtdcExecOrderActionField *pExecOrderAction, CThostFtdcRspInfoField *pRspInfo);
///�������¼��������Ӧ
typedef int (WINAPI *DefOnRspCombActionInsert) (CThostFtdcInputCombActionField *pInputCombAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///�����ѯ��Ϻ�Լ��ȫϵ����Ӧ
typedef int (WINAPI *DefOnRspQryCombInstrumentGuard) (CThostFtdcCombInstrumentGuardField *pCombInstrumentGuard, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///�����ѯ���������Ӧ
typedef int (WINAPI *DefOnRspQryCombAction) (CThostFtdcCombActionField *pCombAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///�������֪ͨ
typedef int (WINAPI *DefOnRtnCombAction) (CThostFtdcCombActionField *pCombAction);
///�������¼�����ر�
typedef int (WINAPI *DefOnErrRtnCombActionInsert) (CThostFtdcInputCombActionField *pInputCombAction, CThostFtdcRspInfoField *pRspInfo);
///�����ѯͶ����Ʒ��/��Ʒ�ֱ�֤����Ӧ
typedef int (WINAPI *DefOnRspQryInvestorProductGroupMargin)(CThostFtdcInvestorProductGroupMarginField *pInvestorProductGroupMargin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///�����ѯ������������֤������Ӧ
typedef int (WINAPI *DefOnRspQryExchangeMarginRateAdjust)(CThostFtdcExchangeMarginRateAdjustField *pExchangeMarginRateAdjust, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///�����ѯ������Ӧ
typedef int (WINAPI *DefOnRspQryExchangeRate)(CThostFtdcExchangeRateField *pExchangeRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///�����ѯ��Ʒ���ۻ���
typedef int (WINAPI *DefOnRspQryProductExchRate)(CThostFtdcProductExchRateField *pProductExchRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///������������������Ӧ
typedef int (WINAPI *DefOnRspBatchOrderAction)(CThostFtdcInputBatchOrderActionField *pInputBatchOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///�����ѯ��Ʒ��
typedef int (WINAPI *DefOnRspQryProductGroup)(CThostFtdcProductGroupField *pProductGroup, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///�����ѯ�����̺�Լ����������Ӧ
typedef int (WINAPI *DefOnRspQryMMInstrumentCommissionRate)(CThostFtdcMMInstrumentCommissionRateField *pMMInstrumentCommissionRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///�����ѯ��������Ȩ��Լ��������Ӧ
typedef int (WINAPI *DefOnRspQryMMOptionInstrCommRate)(CThostFtdcMMOptionInstrCommRateField *pMMOptionInstrCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///�����ѯ������������Ӧ
typedef int (WINAPI *DefOnRspQryInstrumentOrderCommRate)(CThostFtdcInstrumentOrderCommRateField *pInstrumentOrderCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///����������֪ͨ
typedef int (WINAPI *DefOnRtnBulletin)(CThostFtdcBulletinField *pBulletin);
///����������������ر�
typedef int (WINAPI *DefOnErrRtnBatchOrderAction)(CThostFtdcBatchOrderActionField *pBatchOrderAction, CThostFtdcRspInfoField *pRspInfo);

class CTraderSpi : public CThostFtdcTraderSpi
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

	///�ͻ�����֤��Ӧ
	virtual void OnRspAuthenticate(CThostFtdcRspAuthenticateField *pRspAuthenticateField, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///��¼������Ӧ
	virtual void OnRspUserLogin(CThostFtdcRspUserLoginField *pRspUserLogin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�ǳ�������Ӧ
	virtual void OnRspUserLogout(CThostFtdcUserLogoutField *pUserLogout, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�û��������������Ӧ
	virtual void OnRspUserPasswordUpdate(CThostFtdcUserPasswordUpdateField *pUserPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�ʽ��˻��������������Ӧ
	virtual void OnRspTradingAccountPasswordUpdate(CThostFtdcTradingAccountPasswordUpdateField *pTradingAccountPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///����¼��������Ӧ
	virtual void OnRspOrderInsert(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///Ԥ��¼��������Ӧ
	virtual void OnRspParkedOrderInsert(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///Ԥ�񳷵�¼��������Ӧ
	virtual void OnRspParkedOrderAction(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///��������������Ӧ
	virtual void OnRspOrderAction(CThostFtdcInputOrderActionField *pInputOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///��ѯ��󱨵�������Ӧ
	virtual void OnRspQueryMaxOrderVolume(CThostFtdcQueryMaxOrderVolumeField *pQueryMaxOrderVolume, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///Ͷ���߽�����ȷ����Ӧ
	virtual void OnRspSettlementInfoConfirm(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///ɾ��Ԥ����Ӧ
	virtual void OnRspRemoveParkedOrder(CThostFtdcRemoveParkedOrderField *pRemoveParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///ɾ��Ԥ�񳷵���Ӧ
	virtual void OnRspRemoveParkedOrderAction(CThostFtdcRemoveParkedOrderActionField *pRemoveParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///ִ������¼��������Ӧ
	virtual void OnRspExecOrderInsert(CThostFtdcInputExecOrderField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///ִ���������������Ӧ
	virtual void OnRspExecOrderAction(CThostFtdcInputExecOrderActionField *pInputExecOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///ѯ��¼��������Ӧ
	virtual void OnRspForQuoteInsert(CThostFtdcInputForQuoteField *pInputForQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///����¼��������Ӧ
	virtual void OnRspQuoteInsert(CThostFtdcInputQuoteField *pInputQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///���۲���������Ӧ
	virtual void OnRspQuoteAction(CThostFtdcInputQuoteActionField *pInputQuoteAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///������������������Ӧ
	virtual void OnRspBatchOrderAction(CThostFtdcInputBatchOrderActionField *pInputBatchOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�������¼��������Ӧ
	virtual void OnRspCombActionInsert(CThostFtdcInputCombActionField *pInputCombAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ������Ӧ
	virtual void OnRspQryOrder(CThostFtdcOrderField *pOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ�ɽ���Ӧ
	virtual void OnRspQryTrade(CThostFtdcTradeField *pTrade, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯͶ���ֲ߳���Ӧ
	virtual void OnRspQryInvestorPosition(CThostFtdcInvestorPositionField *pInvestorPosition, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ�ʽ��˻���Ӧ
	virtual void OnRspQryTradingAccount(CThostFtdcTradingAccountField *pTradingAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯͶ������Ӧ
	virtual void OnRspQryInvestor(CThostFtdcInvestorField *pInvestor, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ���ױ�����Ӧ
	virtual void OnRspQryTradingCode(CThostFtdcTradingCodeField *pTradingCode, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ��Լ��֤������Ӧ
	virtual void OnRspQryInstrumentMarginRate(CThostFtdcInstrumentMarginRateField *pInstrumentMarginRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ��Լ����������Ӧ
	virtual void OnRspQryInstrumentCommissionRate(CThostFtdcInstrumentCommissionRateField *pInstrumentCommissionRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ��������Ӧ
	virtual void OnRspQryExchange(CThostFtdcExchangeField *pExchange, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ��Ʒ��Ӧ
	virtual void OnRspQryProduct(CThostFtdcProductField *pProduct, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ��Լ��Ӧ
	virtual void OnRspQryInstrument(CThostFtdcInstrumentField *pInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ������Ӧ
	virtual void OnRspQryDepthMarketData(CThostFtdcDepthMarketDataField *pDepthMarketData, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯͶ���߽�������Ӧ
	virtual void OnRspQrySettlementInfo(CThostFtdcSettlementInfoField *pSettlementInfo, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯת��������Ӧ
	virtual void OnRspQryTransferBank(CThostFtdcTransferBankField *pTransferBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯͶ���ֲ߳���ϸ��Ӧ
	virtual void OnRspQryInvestorPositionDetail(CThostFtdcInvestorPositionDetailField *pInvestorPositionDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ�ͻ�֪ͨ��Ӧ
	virtual void OnRspQryNotice(CThostFtdcNoticeField *pNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ������Ϣȷ����Ӧ
	virtual void OnRspQrySettlementInfoConfirm(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯͶ���ֲ߳���ϸ��Ӧ
	virtual void OnRspQryInvestorPositionCombineDetail(CThostFtdcInvestorPositionCombineDetailField *pInvestorPositionCombineDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///��ѯ��֤����ϵͳ���͹�˾�ʽ��˻���Կ��Ӧ
	virtual void OnRspQryCFMMCTradingAccountKey(CThostFtdcCFMMCTradingAccountKeyField *pCFMMCTradingAccountKey, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ�ֵ��۵���Ϣ��Ӧ
	virtual void OnRspQryEWarrantOffset(CThostFtdcEWarrantOffsetField *pEWarrantOffset, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯͶ����Ʒ��/��Ʒ�ֱ�֤����Ӧ
	virtual void OnRspQryInvestorProductGroupMargin(CThostFtdcInvestorProductGroupMarginField *pInvestorProductGroupMargin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ��������֤������Ӧ
	virtual void OnRspQryExchangeMarginRate(CThostFtdcExchangeMarginRateField *pExchangeMarginRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ������������֤������Ӧ
	virtual void OnRspQryExchangeMarginRateAdjust(CThostFtdcExchangeMarginRateAdjustField *pExchangeMarginRateAdjust, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ������Ӧ
	virtual void OnRspQryExchangeRate(CThostFtdcExchangeRateField *pExchangeRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ�����������Ա����Ȩ����Ӧ
	virtual void OnRspQrySecAgentACIDMap(CThostFtdcSecAgentACIDMapField *pSecAgentACIDMap, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ��Ʒ���ۻ���
	virtual void OnRspQryProductExchRate(CThostFtdcProductExchRateField *pProductExchRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ��Ʒ��
	virtual void OnRspQryProductGroup(CThostFtdcProductGroupField *pProductGroup, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ�����̺�Լ����������Ӧ
	virtual void OnRspQryMMInstrumentCommissionRate(CThostFtdcMMInstrumentCommissionRateField *pMMInstrumentCommissionRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ��������Ȩ��Լ��������Ӧ
	virtual void OnRspQryMMOptionInstrCommRate(CThostFtdcMMOptionInstrCommRateField *pMMOptionInstrCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ������������Ӧ
	virtual void OnRspQryInstrumentOrderCommRate(CThostFtdcInstrumentOrderCommRateField *pInstrumentOrderCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ��Ȩ���׳ɱ���Ӧ
	virtual void OnRspQryOptionInstrTradeCost(CThostFtdcOptionInstrTradeCostField *pOptionInstrTradeCost, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ��Ȩ��Լ��������Ӧ
	virtual void OnRspQryOptionInstrCommRate(CThostFtdcOptionInstrCommRateField *pOptionInstrCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯִ��������Ӧ
	virtual void OnRspQryExecOrder(CThostFtdcExecOrderField *pExecOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯѯ����Ӧ
	virtual void OnRspQryForQuote(CThostFtdcForQuoteField *pForQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ������Ӧ
	virtual void OnRspQryQuote(CThostFtdcQuoteField *pQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ��Ϻ�Լ��ȫϵ����Ӧ
	virtual void OnRspQryCombInstrumentGuard(CThostFtdcCombInstrumentGuardField *pCombInstrumentGuard, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ���������Ӧ
	virtual void OnRspQryCombAction(CThostFtdcCombActionField *pCombAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯת����ˮ��Ӧ
	virtual void OnRspQryTransferSerial(CThostFtdcTransferSerialField *pTransferSerial, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ����ǩԼ��ϵ��Ӧ
	virtual void OnRspQryAccountregister(CThostFtdcAccountregisterField *pAccountregister, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///����Ӧ��
	virtual void OnRspError(CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///����֪ͨ
	virtual void OnRtnOrder(CThostFtdcOrderField *pOrder);

	///�ɽ�֪ͨ
	virtual void OnRtnTrade(CThostFtdcTradeField *pTrade);

	///����¼�����ر�
	virtual void OnErrRtnOrderInsert(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo);

	///������������ر�
	virtual void OnErrRtnOrderAction(CThostFtdcOrderActionField *pOrderAction, CThostFtdcRspInfoField *pRspInfo);

	///��Լ����״̬֪ͨ
	virtual void OnRtnInstrumentStatus(CThostFtdcInstrumentStatusField *pInstrumentStatus);

	///����������֪ͨ
	virtual void OnRtnBulletin(CThostFtdcBulletinField *pBulletin);

	///����֪ͨ
	virtual void OnRtnTradingNotice(CThostFtdcTradingNoticeInfoField *pTradingNoticeInfo);

	///��ʾ������У�����
	virtual void OnRtnErrorConditionalOrder(CThostFtdcErrorConditionalOrderField *pErrorConditionalOrder);

	///ִ������֪ͨ
	virtual void OnRtnExecOrder(CThostFtdcExecOrderField *pExecOrder);

	///ִ������¼�����ر�
	virtual void OnErrRtnExecOrderInsert(CThostFtdcInputExecOrderField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo);

	///ִ�������������ر�
	virtual void OnErrRtnExecOrderAction(CThostFtdcExecOrderActionField *pExecOrderAction, CThostFtdcRspInfoField *pRspInfo);

	///ѯ��¼�����ر�
	virtual void OnErrRtnForQuoteInsert(CThostFtdcInputForQuoteField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo);

	///����֪ͨ
	virtual void OnRtnQuote(CThostFtdcQuoteField *pQuote);

	///����¼�����ر�
	virtual void OnErrRtnQuoteInsert(CThostFtdcInputQuoteField *pInputQuote, CThostFtdcRspInfoField *pRspInfo);

	///���۲�������ر�
	virtual void OnErrRtnQuoteAction(CThostFtdcQuoteActionField *pQuoteAction, CThostFtdcRspInfoField *pRspInfo);

	///ѯ��֪ͨ
	virtual void OnRtnForQuoteRsp(CThostFtdcForQuoteRspField *pForQuoteRsp);

	///��֤���������û�����
	virtual void OnRtnCFMMCTradingAccountToken(CThostFtdcCFMMCTradingAccountTokenField *pCFMMCTradingAccountToken);

	///����������������ر�
	virtual void OnErrRtnBatchOrderAction(CThostFtdcBatchOrderActionField *pBatchOrderAction, CThostFtdcRspInfoField *pRspInfo);

	///�������֪ͨ
	virtual void OnRtnCombAction(CThostFtdcCombActionField *pCombAction);

	///�������¼�����ر�
	virtual void OnErrRtnCombActionInsert(CThostFtdcInputCombActionField *pInputCombAction, CThostFtdcRspInfoField *pRspInfo);

	///�����ѯǩԼ������Ӧ
	virtual void OnRspQryContractBank(CThostFtdcContractBankField *pContractBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯԤ����Ӧ
	virtual void OnRspQryParkedOrder(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯԤ�񳷵���Ӧ
	virtual void OnRspQryParkedOrderAction(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ����֪ͨ��Ӧ
	virtual void OnRspQryTradingNotice(CThostFtdcTradingNoticeField *pTradingNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ���͹�˾���ײ�����Ӧ
	virtual void OnRspQryBrokerTradingParams(CThostFtdcBrokerTradingParamsField *pBrokerTradingParams, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ���͹�˾�����㷨��Ӧ
	virtual void OnRspQryBrokerTradingAlgos(CThostFtdcBrokerTradingAlgosField *pBrokerTradingAlgos, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�����ѯ��������û�����
	virtual void OnRspQueryCFMMCTradingAccountToken(CThostFtdcQueryCFMMCTradingAccountTokenField *pQueryCFMMCTradingAccountToken, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///���з��������ʽ�ת�ڻ�֪ͨ
	virtual void OnRtnFromBankToFutureByBank(CThostFtdcRspTransferField *pRspTransfer);

	///���з����ڻ��ʽ�ת����֪ͨ
	virtual void OnRtnFromFutureToBankByBank(CThostFtdcRspTransferField *pRspTransfer);

	///���з����������ת�ڻ�֪ͨ
	virtual void OnRtnRepealFromBankToFutureByBank(CThostFtdcRspRepealField *pRspRepeal);

	///���з�������ڻ�ת����֪ͨ
	virtual void OnRtnRepealFromFutureToBankByBank(CThostFtdcRspRepealField *pRspRepeal);

	///�ڻ����������ʽ�ת�ڻ�֪ͨ
	virtual void OnRtnFromBankToFutureByFuture(CThostFtdcRspTransferField *pRspTransfer);

	///�ڻ������ڻ��ʽ�ת����֪ͨ
	virtual void OnRtnFromFutureToBankByFuture(CThostFtdcRspTransferField *pRspTransfer);

	///ϵͳ����ʱ�ڻ����ֹ������������ת�ڻ��������д�����Ϻ��̷��ص�֪ͨ
	virtual void OnRtnRepealFromBankToFutureByFutureManual(CThostFtdcRspRepealField *pRspRepeal);

	///ϵͳ����ʱ�ڻ����ֹ���������ڻ�ת�����������д�����Ϻ��̷��ص�֪ͨ
	virtual void OnRtnRepealFromFutureToBankByFutureManual(CThostFtdcRspRepealField *pRspRepeal);

	///�ڻ������ѯ�������֪ͨ
	virtual void OnRtnQueryBankBalanceByFuture(CThostFtdcNotifyQueryAccountField *pNotifyQueryAccount);

	///�ڻ����������ʽ�ת�ڻ�����ر�
	virtual void OnErrRtnBankToFutureByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo);

	///�ڻ������ڻ��ʽ�ת���д���ر�
	virtual void OnErrRtnFutureToBankByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo);

	///ϵͳ����ʱ�ڻ����ֹ������������ת�ڻ�����ر�
	virtual void OnErrRtnRepealBankToFutureByFutureManual(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo);

	///ϵͳ����ʱ�ڻ����ֹ���������ڻ�ת���д���ر�
	virtual void OnErrRtnRepealFutureToBankByFutureManual(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo);

	///�ڻ������ѯ����������ر�
	virtual void OnErrRtnQueryBankBalanceByFuture(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo);

	///�ڻ������������ת�ڻ��������д�����Ϻ��̷��ص�֪ͨ
	virtual void OnRtnRepealFromBankToFutureByFuture(CThostFtdcRspRepealField *pRspRepeal);

	///�ڻ���������ڻ�ת�����������д�����Ϻ��̷��ص�֪ͨ
	virtual void OnRtnRepealFromFutureToBankByFuture(CThostFtdcRspRepealField *pRspRepeal);

	///�ڻ����������ʽ�ת�ڻ�Ӧ��
	virtual void OnRspFromBankToFutureByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�ڻ������ڻ��ʽ�ת����Ӧ��
	virtual void OnRspFromFutureToBankByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///�ڻ������ѯ�������Ӧ��
	virtual void OnRspQueryBankAccountMoneyByFuture(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///���з������ڿ���֪ͨ
	virtual void OnRtnOpenAccountByBank(CThostFtdcOpenAccountField *pOpenAccount);

	///���з�����������֪ͨ
	virtual void OnRtnCancelAccountByBank(CThostFtdcCancelAccountField *pCancelAccount);

	///���з����������˺�֪ͨ
	virtual void OnRtnChangeAccountByBank(CThostFtdcChangeAccountField *pChangeAccount);

private:
	CThostFtdcRspInfoField* repareInfo(CThostFtdcRspInfoField *pRspInfo);
	
	// �Ƿ��յ��ɹ�����Ӧ
	bool IsErrorRspInfo(CThostFtdcRspInfoField *pRspInfo);
	
	// �Ƿ��ҵı����ر�
	bool IsMyOrder(CThostFtdcOrderField *pOrder);
	
	// �Ƿ����ڽ��׵ı���
	bool IsTradingOrder(CThostFtdcOrderField *pOrder);
};


