
#pragma once
#define TRADEAPI_API __declspec(dllexport)
#define WINAPI      __stdcall
#define WIN32_LEAN_AND_MEAN             //  从 Windows 头文件中排除极少使用的信息

#include "stdafx.h"
#include ".\api\ThostFtdcTraderApi.h"
#include ".\api\ThostFtdcUserApiDataType.h"
#include ".\api\ThostFtdcUserApiStruct.h"

// UserApi对象
CThostFtdcTraderApi* pUserApi;

//回调函数
void* _OnFrontConnected;	///当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
void* _OnFrontDisconnected;	///当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
void* _OnHeartBeatWarning;	///心跳超时警告。当长时间未收到报文时，该方法被调用。
void* _OnRspUserLogin;///登录请求响应
void* _OnRspAuthenticate;///客户端认证响应
void* _OnRspUserLogout;///登出请求响应
void* _OnRspUserPasswordUpdate;///用户口令更新请求响应
void* _OnRspTradingAccountPasswordUpdate;///资金账户口令更新请求响应
void* _OnRspOrderInsert;///报单录入请求响应
void* _OnRspParkedOrderInsert;///预埋单录入请求响应
void* _OnRspParkedOrderAction;///预埋撤单录入请求响应
void* _OnRspOrderAction;///报单操作请求响应
void* _OnRspQueryMaxOrderVolume;///查询最大报单数量响应
void* _OnRspSettlementInfoConfirm;///投资者结算结果确认响应
void* _OnRspRemoveParkedOrder;///删除预埋单响应
void* _OnRspRemoveParkedOrderAction;///删除预埋撤单响应
void* _OnRspQryOrder;///请求查询报单响应
void* _OnRspQryTrade;///请求查询成交响应
void* _OnRspQryInvestorPosition;///请求查询投资者持仓响应
void* _OnRspQryTradingAccount;///请求查询资金账户响应
void* _OnRspQryInvestor;///请求查询投资者响应
void* _OnRspQryTradingCode;///请求查询交易编码响应
void* _OnRspQryInstrumentMarginRate;///请求查询合约保证金率响应
void* _OnRspQryInstrumentCommissionRate;///请求查询合约手续费率响应
void* _OnRspQryExchange;///请求查询交易所响应
void* _OnRspQryInstrument;///请求查询合约响应
void* _OnRspQryDepthMarketData;///请求查询行情响应
void* _OnRspQrySettlementInfo;///请求查询投资者结算结果响应
void* _OnRspQryTransferBank;///请求查询转帐银行响应
void* _OnRspQryInvestorPositionDetail;///请求查询投资者持仓明细响应
void* _OnRspQryNotice;///请求查询客户通知响应
void* _OnRspQrySettlementInfoConfirm;///请求查询结算信息确认响应
void* _OnRspQryInvestorPositionCombineDetail;///请求查询投资者持仓明细响应
void* _OnRspQryCFMMCTradingAccountKey;///查询保证金监管系统经纪公司资金账户密钥响应
void* _OnRspQryAccountregister;///请求查询银期签约关系响应
void* _OnRspQryTransferSerial;///请求查询转帐流水响应
void* _OnRspError;///错误应答
void* _OnRtnOrder;///报单通知
void* _OnRtnTrade;///成交通知
void* _OnErrRtnOrderInsert;///报单录入错误回报
void* _OnErrRtnOrderAction;///报单操作错误回报
void* _OnRtnInstrumentStatus;///合约交易状态通知
void* _OnRtnTradingNotice;///交易通知
void* _OnRtnErrorConditionalOrder;///提示条件单校验错误
void* _OnRspQryContractBank;///请求查询签约银行响应
void* _OnRspQryParkedOrder;///请求查询预埋单响应
void* _OnRspQryParkedOrderAction;///请求查询预埋撤单响应
void* _OnRspQryTradingNotice;///请求查询交易通知响应
void* _OnRspQryBrokerTradingParams;///请求查询经纪公司交易参数响应
void* _OnRspQryBrokerTradingAlgos;///请求查询经纪公司交易算法响应
void* _OnRtnFromBankToFutureByBank;///银行发起银行资金转期货通知
void* _OnRtnFromFutureToBankByBank;///银行发起期货资金转银行通知
void* _OnRtnRepealFromBankToFutureByBank;///银行发起冲正银行转期货通知
void* _OnRtnRepealFromFutureToBankByBank;///银行发起冲正期货转银行通知
void* _OnRtnFromBankToFutureByFuture;///期货发起银行资金转期货通知
void* _OnRtnFromFutureToBankByFuture;///期货发起期货资金转银行通知
void* _OnRtnRepealFromBankToFutureByFutureManual;///系统运行时期货端手工发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
void* _OnRtnRepealFromFutureToBankByFutureManual;///系统运行时期货端手工发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
void* _OnRtnQueryBankBalanceByFuture;///期货发起查询银行余额通知
void* _OnErrRtnBankToFutureByFuture;///期货发起银行资金转期货错误回报
void* _OnErrRtnFutureToBankByFuture;///期货发起期货资金转银行错误回报
void* _OnErrRtnRepealBankToFutureByFutureManual;///系统运行时期货端手工发起冲正银行转期货错误回报
void* _OnErrRtnRepealFutureToBankByFutureManual;///系统运行时期货端手工发起冲正期货转银行错误回报
void* _OnErrRtnQueryBankBalanceByFuture;///期货发起查询银行余额错误回报
void* _OnRtnRepealFromBankToFutureByFuture;///期货发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
void* _OnRtnRepealFromFutureToBankByFuture;///期货发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
void* _OnRspFromBankToFutureByFuture;///期货发起银行资金转期货应答
void* _OnRspFromFutureToBankByFuture;///期货发起期货资金转银行应答
void* _OnRspQueryBankAccountMoneyByFuture;///期货发起查询银行余额应答

void* _OnRspQryOptionInstrCommRate;///请求查询期权合约手续费响应
void* _OnRspQryOptionInstrTradeCost;///请求查询期权交易成本响应
void* _OnRspQryForQuote;///请求查询询价响应
void* _OnRspForQuoteInsert;///询价录入请求响应
void* _OnRspQryQuote;///请求查询报价响应
void* _OnRspQuoteInsert;///报价录入请求响应
void* _OnRspQuoteAction;///报价操作请求响应
void* _OnRspQryExecOrder;///请求查询执行宣告响应
void* _OnRtnExecOrder;///执行宣告通知
void* _OnRspExecOrderInsert;///执行宣告录入请求响应
void* _OnErrRtnExecOrderInsert;///执行宣告录入错误回报
void* _OnRspExecOrderAction;///执行宣告操作请求响应
void* _OnErrRtnExecOrderAction;///执行宣告操作错误回报

void* _OnRtnForQuoteRsp;///询价通知
void* _OnErrRtnForQuoteInsert;///询价录入错误回报
void* _OnRtnQuote;///报价通知
void* _OnErrRtnQuoteInsert;///报价录入错误回报
void* _OnErrRtnQuoteAction;///报价操作错误回报

void* _OnRspCombActionInsert;///申请组合录入请求响应
void* _OnRspQryCombInstrumentGuard;///请求查询组合合约安全系数响应
void* _OnRspQryCombAction;///请求查询申请组合响应
void* _OnRtnCombAction;///申请组合通知
void* _OnErrRtnCombActionInsert;///申请组合录入错误回报

void* _OnRspQryInvestorProductGroupMargin;///请求查询投资者品种/跨品种保证金响应
void* _OnRspQryExchangeMarginRateAdjust;///请求查询交易所调整保证金率响应
void* _OnRspQryExchangeRate;///请求查询汇率响应
void* _OnRspQryProductExchRate;///请求查询产品报价汇率

void* _OnRspBatchOrderAction;///批量报单操作请求响应
void* _OnRspQryProductGroup;///请求查询产品组
void* _OnRspQryMMInstrumentCommissionRate;///请求查询做市商合约手续费率响应
void* _OnRspQryMMOptionInstrCommRate;///请求查询做市商期权合约手续费响应
void* _OnRspQryInstrumentOrderCommRate;///请求查询报单手续费响应
void* _OnRtnBulletin;///交易所公告通知
void* _OnErrRtnBatchOrderAction;///批量报单操作错误回报

///当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
typedef int (WINAPI *DefOnFrontConnected)();
///当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
typedef int (WINAPI *DefOnFrontDisconnected)(int nReason);
///心跳超时警告。当长时间未收到报文时，该方法被调用。
typedef int (WINAPI *DefOnHeartBeatWarning)(int nTimeLapse);
///登录请求响应
typedef int (WINAPI *DefOnRspAuthenticate)(CThostFtdcRspAuthenticateField *pRspAuthenticateField, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///登录请求响应
typedef int (WINAPI *DefOnRspUserLogin)(CThostFtdcRspUserLoginField *pRspUserLogin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///登出请求响应
typedef int (WINAPI *DefOnRspUserLogout)(CThostFtdcUserLogoutField *pUserLogout, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///用户口令更新请求响应
typedef int (WINAPI *DefOnRspUserPasswordUpdate)(CThostFtdcUserPasswordUpdateField *pUserPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///资金账户口令更新请求响应
typedef int (WINAPI *DefOnRspTradingAccountPasswordUpdate)(CThostFtdcTradingAccountPasswordUpdateField *pTradingAccountPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///报单录入请求响应
typedef int (WINAPI *DefOnRspOrderInsert)(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///预埋单录入请求响应
typedef int (WINAPI *DefOnRspParkedOrderInsert)(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///预埋撤单录入请求响应
typedef int (WINAPI *DefOnRspParkedOrderAction)(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///报单操作请求响应
typedef int (WINAPI *DefOnRspOrderAction)(CThostFtdcInputOrderActionField *pInputOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///查询最大报单数量响应
typedef int (WINAPI *DefOnRspQueryMaxOrderVolume)(CThostFtdcQueryMaxOrderVolumeField *pQueryMaxOrderVolume, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///投资者结算结果确认响应
typedef int (WINAPI *DefOnRspSettlementInfoConfirm)(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///删除预埋单响应
typedef int (WINAPI *DefOnRspRemoveParkedOrder)(CThostFtdcRemoveParkedOrderField *pRemoveParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///删除预埋撤单响应
typedef int (WINAPI *DefOnRspRemoveParkedOrderAction)(CThostFtdcRemoveParkedOrderActionField *pRemoveParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询报单响应
typedef int (WINAPI *DefOnRspQryOrder)(CThostFtdcOrderField *pOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询成交响应
typedef int (WINAPI *DefOnRspQryTrade)(CThostFtdcTradeField *pTrade, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询投资者持仓响应
typedef int (WINAPI *DefOnRspQryInvestorPosition)(CThostFtdcInvestorPositionField *pInvestorPosition, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询资金账户响应
typedef int (WINAPI *DefOnRspQryTradingAccount)(CThostFtdcTradingAccountField *pTradingAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询投资者响应
typedef int (WINAPI *DefOnRspQryInvestor)(CThostFtdcInvestorField *pInvestor, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询交易编码响应
typedef int (WINAPI *DefOnRspQryTradingCode)(CThostFtdcTradingCodeField *pTradingCode, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询合约保证金率响应
typedef int (WINAPI *DefOnRspQryInstrumentMarginRate)(CThostFtdcInstrumentMarginRateField *pInstrumentMarginRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询合约手续费率响应
typedef int (WINAPI *DefOnRspQryInstrumentCommissionRate)(CThostFtdcInstrumentCommissionRateField *pInstrumentCommissionRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询期权合约手续费率响应
typedef int (WINAPI *DefOnRspQryQryOptionInstrCommRate)(CThostFtdcOptionInstrCommRateField *pOptionInstrCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询交易所响应
typedef int (WINAPI *DefOnRspQryExchange)(CThostFtdcExchangeField *pExchange, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询合约响应
typedef int (WINAPI *DefOnRspQryInstrument)(CThostFtdcInstrumentField *pInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询行情响应
typedef int (WINAPI *DefOnRspQryDepthMarketData)(CThostFtdcDepthMarketDataField *pDepthMarketData, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询投资者结算结果响应
typedef int (WINAPI *DefOnRspQrySettlementInfo)(CThostFtdcSettlementInfoField *pSettlementInfo, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询银期签约关系响应
typedef int (WINAPI *DefOnRspQryAccountregister)(CThostFtdcAccountregisterField *pAccountregister, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询转帐银行响应
typedef int (WINAPI *DefOnRspQryTransferBank)(CThostFtdcTransferBankField *pTransferBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询投资者持仓明细响应
typedef int (WINAPI *DefOnRspQryInvestorPositionDetail)(CThostFtdcInvestorPositionDetailField *pInvestorPositionDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询客户通知响应
typedef int (WINAPI *DefOnRspQryNotice)(CThostFtdcNoticeField *pNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询结算信息确认响应
typedef int (WINAPI *DefOnRspQrySettlementInfoConfirm)(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询投资者持仓明细响应
typedef int (WINAPI *DefOnRspQryInvestorPositionCombineDetail)(CThostFtdcInvestorPositionCombineDetailField *pInvestorPositionCombineDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///查询保证金监管系统经纪公司资金账户密钥响应
typedef int (WINAPI *DefOnRspQryCFMMCTradingAccountKey)(CThostFtdcCFMMCTradingAccountKeyField *pCFMMCTradingAccountKey, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询转帐流水响应
typedef int (WINAPI *DefOnRspQryTransferSerial)(CThostFtdcTransferSerialField *pTransferSerial, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///错误应答
typedef int (WINAPI *DefOnRspError)(CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///报单通知
typedef int (WINAPI *DefOnRtnOrder)(CThostFtdcOrderField *pOrder) ;
///成交通知
typedef int (WINAPI *DefOnRtnTrade)(CThostFtdcTradeField *pTrade) ;
///报单录入错误回报
typedef int (WINAPI *DefOnErrRtnOrderInsert)(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo) ;
///报单操作错误回报
typedef int (WINAPI *DefOnErrRtnOrderAction)(CThostFtdcOrderActionField *pOrderAction, CThostFtdcRspInfoField *pRspInfo) ;
///合约交易状态通知
typedef int (WINAPI *DefOnRtnInstrumentStatus)(CThostFtdcInstrumentStatusField *pInstrumentStatus) ;
///交易通知
typedef int (WINAPI *DefOnRtnTradingNotice)(CThostFtdcTradingNoticeInfoField *pTradingNoticeInfo) ;
///提示条件单校验错误
typedef int (WINAPI *DefOnRtnErrorConditionalOrder)(CThostFtdcErrorConditionalOrderField *pErrorConditionalOrder) ;
///请求查询签约银行响应
typedef int (WINAPI *DefOnRspQryContractBank)(CThostFtdcContractBankField *pContractBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询预埋单响应
typedef int (WINAPI *DefOnRspQryParkedOrder)(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询预埋撤单响应
typedef int (WINAPI *DefOnRspQryParkedOrderAction)(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询交易通知响应
typedef int (WINAPI *DefOnRspQryTradingNotice)(CThostFtdcTradingNoticeField *pTradingNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询经纪公司交易参数响应
typedef int (WINAPI *DefOnRspQryBrokerTradingParams)(CThostFtdcBrokerTradingParamsField *pBrokerTradingParams, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///请求查询经纪公司交易算法响应
typedef int (WINAPI *DefOnRspQryBrokerTradingAlgos)(CThostFtdcBrokerTradingAlgosField *pBrokerTradingAlgos, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///银行发起银行资金转期货通知
typedef int (WINAPI *DefOnRtnFromBankToFutureByBank)(CThostFtdcRspTransferField *pRspTransfer) ;
///银行发起期货资金转银行通知
typedef int (WINAPI *DefOnRtnFromFutureToBankByBank)(CThostFtdcRspTransferField *pRspTransfer) ;
///银行发起冲正银行转期货通知
typedef int (WINAPI *DefOnRtnRepealFromBankToFutureByBank)(CThostFtdcRspRepealField *pRspRepeal) ;
///银行发起冲正期货转银行通知
typedef int (WINAPI *DefOnRtnRepealFromFutureToBankByBank)(CThostFtdcRspRepealField *pRspRepeal) ;
///期货发起银行资金转期货通知
typedef int (WINAPI *DefOnRtnFromBankToFutureByFuture)(CThostFtdcRspTransferField *pRspTransfer) ;
///期货发起期货资金转银行通知
typedef int (WINAPI *DefOnRtnFromFutureToBankByFuture)(CThostFtdcRspTransferField *pRspTransfer) ;
///系统运行时期货端手工发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
typedef int (WINAPI *DefOnRtnRepealFromBankToFutureByFutureManual)(CThostFtdcRspRepealField *pRspRepeal) ;
///系统运行时期货端手工发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
typedef int (WINAPI *DefOnRtnRepealFromFutureToBankByFutureManual)(CThostFtdcRspRepealField *pRspRepeal) ;
///期货发起查询银行余额通知
typedef int (WINAPI *DefOnRtnQueryBankBalanceByFuture)(CThostFtdcNotifyQueryAccountField *pNotifyQueryAccount) ;
///期货发起银行资金转期货错误回报
typedef int (WINAPI *DefOnErrRtnBankToFutureByFuture)(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo) ;
///期货发起期货资金转银行错误回报
typedef int (WINAPI *DefOnErrRtnFutureToBankByFuture)(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo) ;
///系统运行时期货端手工发起冲正银行转期货错误回报
typedef int (WINAPI *DefOnErrRtnRepealBankToFutureByFutureManual)(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo) ;
///系统运行时期货端手工发起冲正期货转银行错误回报
typedef int (WINAPI *DefOnErrRtnRepealFutureToBankByFutureManual)(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo) ;
///期货发起查询银行余额错误回报
typedef int (WINAPI *DefOnErrRtnQueryBankBalanceByFuture)(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo) ;
///期货发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
typedef int (WINAPI *DefOnRtnRepealFromBankToFutureByFuture)(CThostFtdcRspRepealField *pRspRepeal) ;
///期货发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
typedef int (WINAPI *DefOnRtnRepealFromFutureToBankByFuture)(CThostFtdcRspRepealField *pRspRepeal) ;
///期货发起银行资金转期货应答
typedef int (WINAPI *DefOnRspFromBankToFutureByFuture)(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///期货发起期货资金转银行应答
typedef int (WINAPI *DefOnRspFromFutureToBankByFuture)(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;
///期货发起查询银行余额应答
typedef int (WINAPI *DefOnRspQueryBankAccountMoneyByFuture)(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///请求查询期权合约手续费响应
typedef int (WINAPI *DefOnRspQryOptionInstrCommRate)(CThostFtdcOptionInstrCommRateField *pOptionInstrCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///请求查询期权交易成本响应
typedef int (WINAPI *DefOnRspQryOptionInstrTradeCost)(CThostFtdcOptionInstrTradeCostField *pOptionInstrTradeCost, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///请求查询询价响应
typedef int (WINAPI *DefOnRspQryForQuote)(CThostFtdcForQuoteField *pForQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///询价通知
typedef int (WINAPI *DefOnRtnForQuoteRsp)(CThostFtdcForQuoteRspField *pForQuoteRsp);
///询价录入请求响应
typedef int (WINAPI *DefOnRspForQuoteInsert)(CThostFtdcInputForQuoteField *pInputForQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///询价录入错误回报
typedef int (WINAPI *DefOnErrRtnForQuoteInsert)(CThostFtdcInputForQuoteField *pInputForQuote, CThostFtdcRspInfoField *pRspInfo);
///请求查询报价响应
typedef int (WINAPI *DefOnRspQryQuote)(CThostFtdcQuoteField *pQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///报价通知
typedef int (WINAPI *DefOnRtnQuote)(CThostFtdcQuoteField *pQuote);
///报价录入请求响应
typedef int (WINAPI *DefOnRspQuoteInsert)(CThostFtdcInputQuoteField *pInputQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///报价录入错误回报
typedef int (WINAPI *DefOnErrRtnQuoteInsert)(CThostFtdcInputQuoteField *pInputQuote, CThostFtdcRspInfoField *pRspInfo);
///报价操作请求响应
typedef int (WINAPI *DefOnRspQuoteAction)(CThostFtdcInputQuoteActionField *pInputQuoteAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///报价操作错误回报
typedef int (WINAPI *DefOnErrRtnQuoteAction)(CThostFtdcQuoteActionField *pQuoteAction, CThostFtdcRspInfoField *pRspInfo);
///请求查询执行宣告响应
typedef int (WINAPI *DefOnRspQryExecOrder)(CThostFtdcExecOrderField *pExecOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///执行宣告通知
typedef int (WINAPI *DefOnRtnExecOrder)(CThostFtdcExecOrderField *pExecOrder);
///执行宣告录入请求响应
typedef int (WINAPI *DefOnRspExecOrderInsert)(CThostFtdcInputExecOrderField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///执行宣告录入错误回报
typedef int (WINAPI *DefOnErrRtnExecOrderInsert)(CThostFtdcInputExecOrderField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo);
///执行宣告操作请求响应
typedef int (WINAPI *DefOnRspExecOrderAction)(CThostFtdcInputExecOrderActionField *pInputExecOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///执行宣告操作错误回报
typedef int (WINAPI *DefOnErrRtnExecOrderAction)(CThostFtdcExecOrderActionField *pExecOrderAction, CThostFtdcRspInfoField *pRspInfo);
///申请组合录入请求响应
typedef int (WINAPI *DefOnRspCombActionInsert) (CThostFtdcInputCombActionField *pInputCombAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///请求查询组合合约安全系数响应
typedef int (WINAPI *DefOnRspQryCombInstrumentGuard) (CThostFtdcCombInstrumentGuardField *pCombInstrumentGuard, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///请求查询申请组合响应
typedef int (WINAPI *DefOnRspQryCombAction) (CThostFtdcCombActionField *pCombAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///申请组合通知
typedef int (WINAPI *DefOnRtnCombAction) (CThostFtdcCombActionField *pCombAction);
///申请组合录入错误回报
typedef int (WINAPI *DefOnErrRtnCombActionInsert) (CThostFtdcInputCombActionField *pInputCombAction, CThostFtdcRspInfoField *pRspInfo);
///请求查询投资者品种/跨品种保证金响应
typedef int (WINAPI *DefOnRspQryInvestorProductGroupMargin)(CThostFtdcInvestorProductGroupMarginField *pInvestorProductGroupMargin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///请求查询交易所调整保证金率响应
typedef int (WINAPI *DefOnRspQryExchangeMarginRateAdjust)(CThostFtdcExchangeMarginRateAdjustField *pExchangeMarginRateAdjust, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///请求查询汇率响应
typedef int (WINAPI *DefOnRspQryExchangeRate)(CThostFtdcExchangeRateField *pExchangeRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///请求查询产品报价汇率
typedef int (WINAPI *DefOnRspQryProductExchRate)(CThostFtdcProductExchRateField *pProductExchRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///批量报单操作请求响应
typedef int (WINAPI *DefOnRspBatchOrderAction)(CThostFtdcInputBatchOrderActionField *pInputBatchOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///请求查询产品组
typedef int (WINAPI *DefOnRspQryProductGroup)(CThostFtdcProductGroupField *pProductGroup, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///请求查询做市商合约手续费率响应
typedef int (WINAPI *DefOnRspQryMMInstrumentCommissionRate)(CThostFtdcMMInstrumentCommissionRateField *pMMInstrumentCommissionRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///请求查询做市商期权合约手续费响应
typedef int (WINAPI *DefOnRspQryMMOptionInstrCommRate)(CThostFtdcMMOptionInstrCommRateField *pMMOptionInstrCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///请求查询报单手续费响应
typedef int (WINAPI *DefOnRspQryInstrumentOrderCommRate)(CThostFtdcInstrumentOrderCommRateField *pInstrumentOrderCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);
///交易所公告通知
typedef int (WINAPI *DefOnRtnBulletin)(CThostFtdcBulletinField *pBulletin);
///批量报单操作错误回报
typedef int (WINAPI *DefOnErrRtnBatchOrderAction)(CThostFtdcBatchOrderActionField *pBatchOrderAction, CThostFtdcRspInfoField *pRspInfo);

class CTraderSpi : public CThostFtdcTraderSpi
{
public:
	///当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
	virtual void OnFrontConnected();

	///当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
	///@param nReason 错误原因
	///        0x1001 网络读失败
	///        0x1002 网络写失败
	///        0x2001 接收心跳超时
	///        0x2002 发送心跳失败
	///        0x2003 收到错误报文
	virtual void OnFrontDisconnected(int nReason);

	///心跳超时警告。当长时间未收到报文时，该方法被调用。
	///@param nTimeLapse 距离上次接收报文的时间
	virtual void OnHeartBeatWarning(int nTimeLapse);

	///客户端认证响应
	virtual void OnRspAuthenticate(CThostFtdcRspAuthenticateField *pRspAuthenticateField, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///登录请求响应
	virtual void OnRspUserLogin(CThostFtdcRspUserLoginField *pRspUserLogin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///登出请求响应
	virtual void OnRspUserLogout(CThostFtdcUserLogoutField *pUserLogout, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///用户口令更新请求响应
	virtual void OnRspUserPasswordUpdate(CThostFtdcUserPasswordUpdateField *pUserPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///资金账户口令更新请求响应
	virtual void OnRspTradingAccountPasswordUpdate(CThostFtdcTradingAccountPasswordUpdateField *pTradingAccountPasswordUpdate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///报单录入请求响应
	virtual void OnRspOrderInsert(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///预埋单录入请求响应
	virtual void OnRspParkedOrderInsert(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///预埋撤单录入请求响应
	virtual void OnRspParkedOrderAction(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///报单操作请求响应
	virtual void OnRspOrderAction(CThostFtdcInputOrderActionField *pInputOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///查询最大报单数量响应
	virtual void OnRspQueryMaxOrderVolume(CThostFtdcQueryMaxOrderVolumeField *pQueryMaxOrderVolume, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///投资者结算结果确认响应
	virtual void OnRspSettlementInfoConfirm(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///删除预埋单响应
	virtual void OnRspRemoveParkedOrder(CThostFtdcRemoveParkedOrderField *pRemoveParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///删除预埋撤单响应
	virtual void OnRspRemoveParkedOrderAction(CThostFtdcRemoveParkedOrderActionField *pRemoveParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///执行宣告录入请求响应
	virtual void OnRspExecOrderInsert(CThostFtdcInputExecOrderField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///执行宣告操作请求响应
	virtual void OnRspExecOrderAction(CThostFtdcInputExecOrderActionField *pInputExecOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///询价录入请求响应
	virtual void OnRspForQuoteInsert(CThostFtdcInputForQuoteField *pInputForQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///报价录入请求响应
	virtual void OnRspQuoteInsert(CThostFtdcInputQuoteField *pInputQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///报价操作请求响应
	virtual void OnRspQuoteAction(CThostFtdcInputQuoteActionField *pInputQuoteAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///批量报单操作请求响应
	virtual void OnRspBatchOrderAction(CThostFtdcInputBatchOrderActionField *pInputBatchOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///申请组合录入请求响应
	virtual void OnRspCombActionInsert(CThostFtdcInputCombActionField *pInputCombAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询报单响应
	virtual void OnRspQryOrder(CThostFtdcOrderField *pOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询成交响应
	virtual void OnRspQryTrade(CThostFtdcTradeField *pTrade, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询投资者持仓响应
	virtual void OnRspQryInvestorPosition(CThostFtdcInvestorPositionField *pInvestorPosition, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询资金账户响应
	virtual void OnRspQryTradingAccount(CThostFtdcTradingAccountField *pTradingAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询投资者响应
	virtual void OnRspQryInvestor(CThostFtdcInvestorField *pInvestor, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询交易编码响应
	virtual void OnRspQryTradingCode(CThostFtdcTradingCodeField *pTradingCode, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询合约保证金率响应
	virtual void OnRspQryInstrumentMarginRate(CThostFtdcInstrumentMarginRateField *pInstrumentMarginRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询合约手续费率响应
	virtual void OnRspQryInstrumentCommissionRate(CThostFtdcInstrumentCommissionRateField *pInstrumentCommissionRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询交易所响应
	virtual void OnRspQryExchange(CThostFtdcExchangeField *pExchange, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询产品响应
	virtual void OnRspQryProduct(CThostFtdcProductField *pProduct, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询合约响应
	virtual void OnRspQryInstrument(CThostFtdcInstrumentField *pInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询行情响应
	virtual void OnRspQryDepthMarketData(CThostFtdcDepthMarketDataField *pDepthMarketData, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询投资者结算结果响应
	virtual void OnRspQrySettlementInfo(CThostFtdcSettlementInfoField *pSettlementInfo, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询转帐银行响应
	virtual void OnRspQryTransferBank(CThostFtdcTransferBankField *pTransferBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询投资者持仓明细响应
	virtual void OnRspQryInvestorPositionDetail(CThostFtdcInvestorPositionDetailField *pInvestorPositionDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询客户通知响应
	virtual void OnRspQryNotice(CThostFtdcNoticeField *pNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询结算信息确认响应
	virtual void OnRspQrySettlementInfoConfirm(CThostFtdcSettlementInfoConfirmField *pSettlementInfoConfirm, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询投资者持仓明细响应
	virtual void OnRspQryInvestorPositionCombineDetail(CThostFtdcInvestorPositionCombineDetailField *pInvestorPositionCombineDetail, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///查询保证金监管系统经纪公司资金账户密钥响应
	virtual void OnRspQryCFMMCTradingAccountKey(CThostFtdcCFMMCTradingAccountKeyField *pCFMMCTradingAccountKey, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询仓单折抵信息响应
	virtual void OnRspQryEWarrantOffset(CThostFtdcEWarrantOffsetField *pEWarrantOffset, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询投资者品种/跨品种保证金响应
	virtual void OnRspQryInvestorProductGroupMargin(CThostFtdcInvestorProductGroupMarginField *pInvestorProductGroupMargin, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询交易所保证金率响应
	virtual void OnRspQryExchangeMarginRate(CThostFtdcExchangeMarginRateField *pExchangeMarginRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询交易所调整保证金率响应
	virtual void OnRspQryExchangeMarginRateAdjust(CThostFtdcExchangeMarginRateAdjustField *pExchangeMarginRateAdjust, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询汇率响应
	virtual void OnRspQryExchangeRate(CThostFtdcExchangeRateField *pExchangeRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询二级代理操作员银期权限响应
	virtual void OnRspQrySecAgentACIDMap(CThostFtdcSecAgentACIDMapField *pSecAgentACIDMap, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询产品报价汇率
	virtual void OnRspQryProductExchRate(CThostFtdcProductExchRateField *pProductExchRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询产品组
	virtual void OnRspQryProductGroup(CThostFtdcProductGroupField *pProductGroup, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询做市商合约手续费率响应
	virtual void OnRspQryMMInstrumentCommissionRate(CThostFtdcMMInstrumentCommissionRateField *pMMInstrumentCommissionRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询做市商期权合约手续费响应
	virtual void OnRspQryMMOptionInstrCommRate(CThostFtdcMMOptionInstrCommRateField *pMMOptionInstrCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询报单手续费响应
	virtual void OnRspQryInstrumentOrderCommRate(CThostFtdcInstrumentOrderCommRateField *pInstrumentOrderCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询期权交易成本响应
	virtual void OnRspQryOptionInstrTradeCost(CThostFtdcOptionInstrTradeCostField *pOptionInstrTradeCost, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询期权合约手续费响应
	virtual void OnRspQryOptionInstrCommRate(CThostFtdcOptionInstrCommRateField *pOptionInstrCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询执行宣告响应
	virtual void OnRspQryExecOrder(CThostFtdcExecOrderField *pExecOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询询价响应
	virtual void OnRspQryForQuote(CThostFtdcForQuoteField *pForQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询报价响应
	virtual void OnRspQryQuote(CThostFtdcQuoteField *pQuote, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询组合合约安全系数响应
	virtual void OnRspQryCombInstrumentGuard(CThostFtdcCombInstrumentGuardField *pCombInstrumentGuard, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询申请组合响应
	virtual void OnRspQryCombAction(CThostFtdcCombActionField *pCombAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询转帐流水响应
	virtual void OnRspQryTransferSerial(CThostFtdcTransferSerialField *pTransferSerial, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询银期签约关系响应
	virtual void OnRspQryAccountregister(CThostFtdcAccountregisterField *pAccountregister, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///错误应答
	virtual void OnRspError(CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///报单通知
	virtual void OnRtnOrder(CThostFtdcOrderField *pOrder);

	///成交通知
	virtual void OnRtnTrade(CThostFtdcTradeField *pTrade);

	///报单录入错误回报
	virtual void OnErrRtnOrderInsert(CThostFtdcInputOrderField *pInputOrder, CThostFtdcRspInfoField *pRspInfo);

	///报单操作错误回报
	virtual void OnErrRtnOrderAction(CThostFtdcOrderActionField *pOrderAction, CThostFtdcRspInfoField *pRspInfo);

	///合约交易状态通知
	virtual void OnRtnInstrumentStatus(CThostFtdcInstrumentStatusField *pInstrumentStatus);

	///交易所公告通知
	virtual void OnRtnBulletin(CThostFtdcBulletinField *pBulletin);

	///交易通知
	virtual void OnRtnTradingNotice(CThostFtdcTradingNoticeInfoField *pTradingNoticeInfo);

	///提示条件单校验错误
	virtual void OnRtnErrorConditionalOrder(CThostFtdcErrorConditionalOrderField *pErrorConditionalOrder);

	///执行宣告通知
	virtual void OnRtnExecOrder(CThostFtdcExecOrderField *pExecOrder);

	///执行宣告录入错误回报
	virtual void OnErrRtnExecOrderInsert(CThostFtdcInputExecOrderField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo);

	///执行宣告操作错误回报
	virtual void OnErrRtnExecOrderAction(CThostFtdcExecOrderActionField *pExecOrderAction, CThostFtdcRspInfoField *pRspInfo);

	///询价录入错误回报
	virtual void OnErrRtnForQuoteInsert(CThostFtdcInputForQuoteField *pInputExecOrder, CThostFtdcRspInfoField *pRspInfo);

	///报价通知
	virtual void OnRtnQuote(CThostFtdcQuoteField *pQuote);

	///报价录入错误回报
	virtual void OnErrRtnQuoteInsert(CThostFtdcInputQuoteField *pInputQuote, CThostFtdcRspInfoField *pRspInfo);

	///报价操作错误回报
	virtual void OnErrRtnQuoteAction(CThostFtdcQuoteActionField *pQuoteAction, CThostFtdcRspInfoField *pRspInfo);

	///询价通知
	virtual void OnRtnForQuoteRsp(CThostFtdcForQuoteRspField *pForQuoteRsp);

	///保证金监控中心用户令牌
	virtual void OnRtnCFMMCTradingAccountToken(CThostFtdcCFMMCTradingAccountTokenField *pCFMMCTradingAccountToken);

	///批量报单操作错误回报
	virtual void OnErrRtnBatchOrderAction(CThostFtdcBatchOrderActionField *pBatchOrderAction, CThostFtdcRspInfoField *pRspInfo);

	///申请组合通知
	virtual void OnRtnCombAction(CThostFtdcCombActionField *pCombAction);

	///申请组合录入错误回报
	virtual void OnErrRtnCombActionInsert(CThostFtdcInputCombActionField *pInputCombAction, CThostFtdcRspInfoField *pRspInfo);

	///请求查询签约银行响应
	virtual void OnRspQryContractBank(CThostFtdcContractBankField *pContractBank, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询预埋单响应
	virtual void OnRspQryParkedOrder(CThostFtdcParkedOrderField *pParkedOrder, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询预埋撤单响应
	virtual void OnRspQryParkedOrderAction(CThostFtdcParkedOrderActionField *pParkedOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询交易通知响应
	virtual void OnRspQryTradingNotice(CThostFtdcTradingNoticeField *pTradingNotice, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询经纪公司交易参数响应
	virtual void OnRspQryBrokerTradingParams(CThostFtdcBrokerTradingParamsField *pBrokerTradingParams, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询经纪公司交易算法响应
	virtual void OnRspQryBrokerTradingAlgos(CThostFtdcBrokerTradingAlgosField *pBrokerTradingAlgos, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///请求查询监控中心用户令牌
	virtual void OnRspQueryCFMMCTradingAccountToken(CThostFtdcQueryCFMMCTradingAccountTokenField *pQueryCFMMCTradingAccountToken, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///银行发起银行资金转期货通知
	virtual void OnRtnFromBankToFutureByBank(CThostFtdcRspTransferField *pRspTransfer);

	///银行发起期货资金转银行通知
	virtual void OnRtnFromFutureToBankByBank(CThostFtdcRspTransferField *pRspTransfer);

	///银行发起冲正银行转期货通知
	virtual void OnRtnRepealFromBankToFutureByBank(CThostFtdcRspRepealField *pRspRepeal);

	///银行发起冲正期货转银行通知
	virtual void OnRtnRepealFromFutureToBankByBank(CThostFtdcRspRepealField *pRspRepeal);

	///期货发起银行资金转期货通知
	virtual void OnRtnFromBankToFutureByFuture(CThostFtdcRspTransferField *pRspTransfer);

	///期货发起期货资金转银行通知
	virtual void OnRtnFromFutureToBankByFuture(CThostFtdcRspTransferField *pRspTransfer);

	///系统运行时期货端手工发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
	virtual void OnRtnRepealFromBankToFutureByFutureManual(CThostFtdcRspRepealField *pRspRepeal);

	///系统运行时期货端手工发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
	virtual void OnRtnRepealFromFutureToBankByFutureManual(CThostFtdcRspRepealField *pRspRepeal);

	///期货发起查询银行余额通知
	virtual void OnRtnQueryBankBalanceByFuture(CThostFtdcNotifyQueryAccountField *pNotifyQueryAccount);

	///期货发起银行资金转期货错误回报
	virtual void OnErrRtnBankToFutureByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo);

	///期货发起期货资金转银行错误回报
	virtual void OnErrRtnFutureToBankByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo);

	///系统运行时期货端手工发起冲正银行转期货错误回报
	virtual void OnErrRtnRepealBankToFutureByFutureManual(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo);

	///系统运行时期货端手工发起冲正期货转银行错误回报
	virtual void OnErrRtnRepealFutureToBankByFutureManual(CThostFtdcReqRepealField *pReqRepeal, CThostFtdcRspInfoField *pRspInfo);

	///期货发起查询银行余额错误回报
	virtual void OnErrRtnQueryBankBalanceByFuture(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo);

	///期货发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
	virtual void OnRtnRepealFromBankToFutureByFuture(CThostFtdcRspRepealField *pRspRepeal);

	///期货发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
	virtual void OnRtnRepealFromFutureToBankByFuture(CThostFtdcRspRepealField *pRspRepeal);

	///期货发起银行资金转期货应答
	virtual void OnRspFromBankToFutureByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///期货发起期货资金转银行应答
	virtual void OnRspFromFutureToBankByFuture(CThostFtdcReqTransferField *pReqTransfer, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///期货发起查询银行余额应答
	virtual void OnRspQueryBankAccountMoneyByFuture(CThostFtdcReqQueryAccountField *pReqQueryAccount, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///银行发起银期开户通知
	virtual void OnRtnOpenAccountByBank(CThostFtdcOpenAccountField *pOpenAccount);

	///银行发起银期销户通知
	virtual void OnRtnCancelAccountByBank(CThostFtdcCancelAccountField *pCancelAccount);

	///银行发起变更银行账号通知
	virtual void OnRtnChangeAccountByBank(CThostFtdcChangeAccountField *pChangeAccount);

private:
	CThostFtdcRspInfoField* repareInfo(CThostFtdcRspInfoField *pRspInfo);
	
	// 是否收到成功的响应
	bool IsErrorRspInfo(CThostFtdcRspInfoField *pRspInfo);
	
	// 是否我的报单回报
	bool IsMyOrder(CThostFtdcOrderField *pOrder);
	
	// 是否正在交易的报单
	bool IsTradingOrder(CThostFtdcOrderField *pOrder);
};


