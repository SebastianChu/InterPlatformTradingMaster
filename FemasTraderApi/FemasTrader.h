
#pragma once
#define TRADEAPI_API __declspec(dllexport)
#define WINAPI      __stdcall
#define WIN32_LEAN_AND_MEAN             //  从 Windows 头文件中排除极少使用的信息

#include "stdafx.h"
#include ".\api\USTPFtdcTraderApi.h"
#include ".\api\USTPFtdcUserApiDataType.h"
#include ".\api\USTPFtdcUserApiStruct.h"

// UserApi对象
CUstpFtdcTraderApi* pUserApi;

//回调函数
void* _OnFrontConnected;///当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
void* _OnFrontDisconnected;///当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
void* _OnHeartBeatWarning;///心跳超时警告。当长时间未收到报文时，该方法被调用。
void* _OnPackageStart;///报文回调开始通知。当API收到一个报文后，首先调用本方法，然后是各数据域的回调，最后是报文回调结束通知。
void* _OnPackageEnd;///报文回调结束通知。当API收到一个报文后，首先调用报文回调开始通知，然后是各数据域的回调，最后调用本方法。
void* _OnRspError;///错误应答
void* _OnRspUserLogin;///风控前置系统用户登录应答
void* _OnRspUserLogout;///用户退出应答
void* _OnRspUserPasswordUpdate;///用户密码修改应答
void* _OnRspOrderInsert;///报单录入应答
void* _OnRspOrderAction;///报单操作应答
void* _OnRspQuoteInsert;///报价录入应答
void* _OnRspQuoteAction;///报价操作应答
void* _OnRspForQuote;///询价请求应答
void* _OnRspMarginCombAction;///客户申请组合应答
void* _OnRtnFlowMessageCancel;///数据流回退通知
void* _OnRtnTrade;///成交回报
void* _OnRtnOrder;///报单回报
void* _OnErrRtnOrderInsert;///报单录入错误回报
void* _OnErrRtnOrderAction;///报单操作错误回报
void* _OnRtnInstrumentStatus;///合约交易状态通知
void* _OnRtnInvestorAccountDeposit;///账户出入金回报
void* _OnRtnQuote;///报价回报
void* _OnErrRtnQuoteInsert;///报价录入错误回报
void* _OnRtnForQuote;///询价回报
void* _OnRtnMarginCombinationLeg;///增加组合规则通知
void* _OnRtnMarginCombAction;///客户申请组合确认
void* _OnRspQryOrder;///报单查询应答
void* _OnRspQryTrade;///成交单查询应答
void* _OnRspQryUserInvestor;///可用投资者账户查询应答
void* _OnRspQryTradingCode;///交易编码查询应答
void* _OnRspQryInvestorAccount;///投资者资金账户查询应答
void* _OnRspQryInstrument;///合约查询应答
void* _OnRspQryExchange;///交易所查询应答
void* _OnRspQryInvestorPosition;///投资者持仓查询应答
void* _OnRspSubscribeTopic;///订阅主题应答
void* _OnRspQryComplianceParam;///合规参数查询应答
void* _OnRspQryTopic;///主题查询应答
void* _OnRspQryInvestorFee;///投资者手续费率查询应答
void* _OnRspQryInvestorMargin;///投资者保证金率查询应答
void* _OnRspQryInvestorCombPosition;///交易编码组合持仓查询应答
void* _OnRspQryInvestorLegPosition;///交易编码单腿持仓查询应答
void* _OnRspQryExchangeRate;///交叉汇率查询应答

///当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
typedef int (WINAPI *DefOnFrontConnected)();
	
///当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
typedef int (WINAPI *DefOnFrontDisconnected)(int nReason);
		
///心跳超时警告。当长时间未收到报文时，该方法被调用。
typedef int (WINAPI *DefOnHeartBeatWarning)(int nTimeLapse);
	
///报文回调开始通知。当API收到一个报文后，首先调用本方法，然后是各数据域的回调，最后是报文回调结束通知。
///@param nTopicID 主题代码（如私有流、公共流、行情流等）
///@param nSequenceNo 报文序号
typedef int (WINAPI *DefOnPackageStart)(int nTopicID, int nSequenceNo);
	
///报文回调结束通知。当API收到一个报文后，首先调用报文回调开始通知，然后是各数据域的回调，最后调用本方法。
///@param nTopicID 主题代码（如私有流、公共流、行情流等）
///@param nSequenceNo 报文序号
typedef int (WINAPI *DefOnPackageEnd)(int nTopicID, int nSequenceNo);

///错误应答
typedef int (WINAPI *DefOnRspError)(CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///风控前置系统用户登录应答
typedef int (WINAPI *DefOnRspUserLogin)(CUstpFtdcRspUserLoginField *pRspUserLogin, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///用户退出应答
typedef int (WINAPI *DefOnRspUserLogout)(CUstpFtdcRspUserLogoutField *pRspUserLogout, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///用户密码修改应答
typedef int (WINAPI *DefOnRspUserPasswordUpdate)(CUstpFtdcUserPasswordUpdateField *pUserPasswordUpdate, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///报单录入应答
typedef int (WINAPI *DefOnRspOrderInsert)(CUstpFtdcInputOrderField *pInputOrder, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///报单操作应答
typedef int (WINAPI *DefOnRspOrderAction)(CUstpFtdcOrderActionField *pOrderAction, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///报价录入应答
typedef int (WINAPI *DefOnRspQuoteInsert)(CUstpFtdcInputQuoteField *pInputQuote, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///报价操作应答
typedef int (WINAPI *DefOnRspQuoteAction)(CUstpFtdcQuoteActionField *pQuoteAction, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///询价请求应答
typedef int (WINAPI *DefOnRspForQuote)(CUstpFtdcReqForQuoteField *pReqForQuote, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///客户申请组合应答
typedef int (WINAPI *DefOnRspMarginCombAction)(CUstpFtdcInputMarginCombActionField *pInputMarginCombAction, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///数据流回退通知
typedef int (WINAPI *DefOnRtnFlowMessageCancel)(CUstpFtdcFlowMessageCancelField *pFlowMessageCancel) ;

///成交回报
typedef int (WINAPI *DefOnRtnTrade)(CUstpFtdcTradeField *pTrade) ;

///报单回报
typedef int (WINAPI *DefOnRtnOrder)(CUstpFtdcOrderField *pOrder) ;

///报单录入错误回报
typedef int (WINAPI *DefOnErrRtnOrderInsert)(CUstpFtdcInputOrderField *pInputOrder, CUstpFtdcRspInfoField *pRspInfo) ;

///报单操作错误回报
typedef int (WINAPI *DefOnErrRtnOrderAction)(CUstpFtdcOrderActionField *pOrderAction, CUstpFtdcRspInfoField *pRspInfo) ;

///合约交易状态通知
typedef int (WINAPI *DefOnRtnInstrumentStatus)(CUstpFtdcInstrumentStatusField *pInstrumentStatus) ;

///账户出入金回报
typedef int (WINAPI *DefOnRtnInvestorAccountDeposit)(CUstpFtdcInvestorAccountDepositResField *pInvestorAccountDepositRes) ;

///报价回报
typedef int (WINAPI *DefOnRtnQuote)(CUstpFtdcRtnQuoteField *pRtnQuote) ;

///报价录入错误回报
typedef int (WINAPI *DefOnErrRtnQuoteInsert)(CUstpFtdcInputQuoteField *pInputQuote, CUstpFtdcRspInfoField *pRspInfo) ;

///询价回报
typedef int (WINAPI *DefOnRtnForQuote)(CUstpFtdcReqForQuoteField *pReqForQuote) ;

///增加组合规则通知
typedef int (WINAPI *DefOnRtnMarginCombinationLeg)(CUstpFtdcMarginCombinationLegField *pMarginCombinationLeg) ;

///客户申请组合确认
typedef int (WINAPI *DefOnRtnMarginCombAction)(CUstpFtdcInputMarginCombActionField *pInputMarginCombAction) ;

///报单查询应答
typedef int (WINAPI *DefOnRspQryOrder)(CUstpFtdcOrderField *pOrder, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///成交单查询应答
typedef int (WINAPI *DefOnRspQryTrade)(CUstpFtdcTradeField *pTrade, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///可用投资者账户查询应答
typedef int (WINAPI *DefOnRspQryUserInvestor)(CUstpFtdcRspUserInvestorField *pRspUserInvestor, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///交易编码查询应答
typedef int (WINAPI *DefOnRspQryTradingCode)(CUstpFtdcRspTradingCodeField *pRspTradingCode, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///投资者资金账户查询应答
typedef int (WINAPI *DefOnRspQryInvestorAccount)(CUstpFtdcRspInvestorAccountField *pRspInvestorAccount, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///合约查询应答
typedef int (WINAPI *DefOnRspQryInstrument)(CUstpFtdcRspInstrumentField *pRspInstrument, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///交易所查询应答
typedef int (WINAPI *DefOnRspQryExchange)(CUstpFtdcRspExchangeField *pRspExchange, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///投资者持仓查询应答
typedef int (WINAPI *DefOnRspQryInvestorPosition)(CUstpFtdcRspInvestorPositionField *pRspInvestorPosition, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///订阅主题应答
typedef int (WINAPI *DefOnRspSubscribeTopic)(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///合规参数查询应答
typedef int (WINAPI *DefOnRspQryComplianceParam)(CUstpFtdcRspComplianceParamField *pRspComplianceParam, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///主题查询应答
typedef int (WINAPI *DefOnRspQryTopic)(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///投资者手续费率查询应答
typedef int (WINAPI *DefOnRspQryInvestorFee)(CUstpFtdcInvestorFeeField *pInvestorFee, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///投资者保证金率查询应答
typedef int (WINAPI *DefOnRspQryInvestorMargin)(CUstpFtdcInvestorMarginField *pInvestorMargin, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///交易编码组合持仓查询应答
typedef int (WINAPI *DefOnRspQryInvestorCombPosition)(CUstpFtdcRspInvestorCombPositionField *pRspInvestorCombPosition, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///交易编码单腿持仓查询应答
typedef int (WINAPI *DefOnRspQryInvestorLegPosition)(CUstpFtdcRspInvestorLegPositionField *pRspInvestorLegPosition, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///交叉汇率查询应答
typedef int (WINAPI *DefOnRspQryExchangeRate)(CUstpFtdcRspExchangeRateField *pRspExchangeRate, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

class CTraderSpi : public CUstpFtdcTraderSpi
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
	
	///报文回调开始通知。当API收到一个报文后，首先调用本方法，然后是各数据域的回调，最后是报文回调结束通知。
	///@param nTopicID 主题代码（如私有流、公共流、行情流等）
	///@param nSequenceNo 报文序号
	virtual void OnPackageStart(int nTopicID, int nSequenceNo);
	
	///报文回调结束通知。当API收到一个报文后，首先调用报文回调开始通知，然后是各数据域的回调，最后调用本方法。
	///@param nTopicID 主题代码（如私有流、公共流、行情流等）
	///@param nSequenceNo 报文序号
	virtual void OnPackageEnd(int nTopicID, int nSequenceNo);

	///错误应答
	virtual void OnRspError(CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///风控前置系统用户登录应答
	virtual void OnRspUserLogin(CUstpFtdcRspUserLoginField *pRspUserLogin, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///用户退出应答
	virtual void OnRspUserLogout(CUstpFtdcRspUserLogoutField *pRspUserLogout, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///用户密码修改应答
	virtual void OnRspUserPasswordUpdate(CUstpFtdcUserPasswordUpdateField *pUserPasswordUpdate, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///报单录入应答
	virtual void OnRspOrderInsert(CUstpFtdcInputOrderField *pInputOrder, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///报单操作应答
	virtual void OnRspOrderAction(CUstpFtdcOrderActionField *pOrderAction, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///报价录入应答
	virtual void OnRspQuoteInsert(CUstpFtdcInputQuoteField *pInputQuote, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///报价操作应答
	virtual void OnRspQuoteAction(CUstpFtdcQuoteActionField *pQuoteAction, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///询价请求应答
	virtual void OnRspForQuote(CUstpFtdcReqForQuoteField *pReqForQuote, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///客户申请组合应答
	virtual void OnRspMarginCombAction(CUstpFtdcInputMarginCombActionField *pInputMarginCombAction, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///数据流回退通知
	virtual void OnRtnFlowMessageCancel(CUstpFtdcFlowMessageCancelField *pFlowMessageCancel) ;

	///成交回报
	virtual void OnRtnTrade(CUstpFtdcTradeField *pTrade) ;

	///报单回报
	virtual void OnRtnOrder(CUstpFtdcOrderField *pOrder) ;

	///报单录入错误回报
	virtual void OnErrRtnOrderInsert(CUstpFtdcInputOrderField *pInputOrder, CUstpFtdcRspInfoField *pRspInfo) ;

	///报单操作错误回报
	virtual void OnErrRtnOrderAction(CUstpFtdcOrderActionField *pOrderAction, CUstpFtdcRspInfoField *pRspInfo) ;

	///合约交易状态通知
	virtual void OnRtnInstrumentStatus(CUstpFtdcInstrumentStatusField *pInstrumentStatus) ;

	///账户出入金回报
	virtual void OnRtnInvestorAccountDeposit(CUstpFtdcInvestorAccountDepositResField *pInvestorAccountDepositRes) ;

	///报价回报
	virtual void OnRtnQuote(CUstpFtdcRtnQuoteField *pRtnQuote) ;

	///报价录入错误回报
	virtual void OnErrRtnQuoteInsert(CUstpFtdcInputQuoteField *pInputQuote, CUstpFtdcRspInfoField *pRspInfo) ;

	///询价回报
	virtual void OnRtnForQuote(CUstpFtdcReqForQuoteField *pReqForQuote) ;

	///增加组合规则通知
	virtual void OnRtnMarginCombinationLeg(CUstpFtdcMarginCombinationLegField *pMarginCombinationLeg) ;

	///客户申请组合确认
	virtual void OnRtnMarginCombAction(CUstpFtdcInputMarginCombActionField *pInputMarginCombAction) ;

	///报单查询应答
	virtual void OnRspQryOrder(CUstpFtdcOrderField *pOrder, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///成交单查询应答
	virtual void OnRspQryTrade(CUstpFtdcTradeField *pTrade, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///可用投资者账户查询应答
	virtual void OnRspQryUserInvestor(CUstpFtdcRspUserInvestorField *pRspUserInvestor, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///交易编码查询应答
	virtual void OnRspQryTradingCode(CUstpFtdcRspTradingCodeField *pRspTradingCode, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///投资者资金账户查询应答
	virtual void OnRspQryInvestorAccount(CUstpFtdcRspInvestorAccountField *pRspInvestorAccount, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///合约查询应答
	virtual void OnRspQryInstrument(CUstpFtdcRspInstrumentField *pRspInstrument, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///交易所查询应答
	virtual void OnRspQryExchange(CUstpFtdcRspExchangeField *pRspExchange, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///投资者持仓查询应答
	virtual void OnRspQryInvestorPosition(CUstpFtdcRspInvestorPositionField *pRspInvestorPosition, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///订阅主题应答
	virtual void OnRspSubscribeTopic(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///合规参数查询应答
	virtual void OnRspQryComplianceParam(CUstpFtdcRspComplianceParamField *pRspComplianceParam, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///主题查询应答
	virtual void OnRspQryTopic(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///投资者手续费率查询应答
	virtual void OnRspQryInvestorFee(CUstpFtdcInvestorFeeField *pInvestorFee, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///投资者保证金率查询应答
	virtual void OnRspQryInvestorMargin(CUstpFtdcInvestorMarginField *pInvestorMargin, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///交易编码组合持仓查询应答
	virtual void OnRspQryInvestorCombPosition(CUstpFtdcRspInvestorCombPositionField *pRspInvestorCombPosition, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///交易编码单腿持仓查询应答
	virtual void OnRspQryInvestorLegPosition(CUstpFtdcRspInvestorLegPositionField *pRspInvestorLegPosition, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///交叉汇率查询应答
	virtual void OnRspQryExchangeRate(CUstpFtdcRspExchangeRateField *pRspExchangeRate, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

private:
	CUstpFtdcRspInfoField* repareInfo(CUstpFtdcRspInfoField *pRspInfo);
	
	// 是否收到成功的响应
	bool IsErrorRspInfo(CUstpFtdcRspInfoField *pRspInfo);
	
	// 是否我的报单回报
	bool IsMyOrder(CUstpFtdcOrderField *pOrder);
	
	// 是否正在交易的报单
	bool IsTradingOrder(CUstpFtdcOrderField *pOrder);
};


