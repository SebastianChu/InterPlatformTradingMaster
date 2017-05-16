// CtpTrade.cpp : 定义 DLL 应用程序的导出函数。
//

#include "CtpTrader.h"

// 请求编号
int iRequestID = 0;
//连接
TRADEAPI_API void WINAPI Connect(char* FRONT_ADDR)
{
	// 初始化UserApi
	pUserApi = CThostFtdcTraderApi::CreateFtdcTraderApi();			// 创建UserApi
	CTraderSpi* pUserSpi = new CTraderSpi();
	pUserApi->RegisterSpi((CThostFtdcTraderSpi*)pUserSpi);			// 注册事件类
	pUserApi->SubscribePublicTopic(THOST_TERT_QUICK/*THOST_TERT_RESTART*/);					// 注册公有流
	pUserApi->SubscribePrivateTopic(THOST_TERT_QUICK/*THOST_TERT_RESTART*/);					// 注册私有流
	pUserApi->RegisterFront(FRONT_ADDR);							// connect
	pUserApi->Init();
	//pUserApi->Join();
}

TRADEAPI_API const char *GetTradingDay()
{
	return pUserApi->GetTradingDay();
}

//断开
TRADEAPI_API void WINAPI DisConnect()
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

//发送用户登录请求
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

//发送登出请求
TRADEAPI_API int WINAPI ReqUserLogout(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcUserIDType INVESTOR_ID)
{
	CThostFtdcUserLogoutField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.UserID, INVESTOR_ID);
	return pUserApi->ReqUserLogout(&req, ++iRequestID);
}

//更新用户口令
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

///资金账户口令更新请求
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

//报单录入请求
TRADEAPI_API int WINAPI ReqOrderInsert(CThostFtdcInputOrderField *pOrder)
{
	strcpy_s(pOrder->BusinessUnit,"IPTM_v1.0");
	return pUserApi->ReqOrderInsert(pOrder, ++iRequestID);
}

//报单操作请求
TRADEAPI_API int WINAPI ReqOrderAction(CThostFtdcInputOrderActionField *pOrder)
{
	return pUserApi->ReqOrderAction(pOrder, ++iRequestID);
}

///查询最大报单数量请求
TRADEAPI_API int WINAPI ReqQueryMaxOrderVolume(CThostFtdcQueryMaxOrderVolumeField *pMaxOrderVolume)
{
	return pUserApi->ReqQueryMaxOrderVolume(pMaxOrderVolume, ++iRequestID);
}

//投资者结算结果确认
TRADEAPI_API int WINAPI ReqSettlementInfoConfirm(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID)
{
	CThostFtdcSettlementInfoConfirmField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.InvestorID, INVESTOR_ID);
	return pUserApi->ReqSettlementInfoConfirm(&req, ++iRequestID);
}

///请求查询报单
TRADEAPI_API int WINAPI ReqQryOrder(CThostFtdcQryOrderField *pQryOrder)
{
	return pUserApi->ReqQryOrder(pQryOrder, ++iRequestID);
}

///请求查询成交
TRADEAPI_API int WINAPI ReqQryTrade(CThostFtdcQryTradeField *pQryTrade)
{
	return pUserApi->ReqQryTrade(pQryTrade, ++iRequestID);
}

//请求查询投资者持仓
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

//请求查询资金账户
TRADEAPI_API int WINAPI ReqQryTradingAccount(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID)
{
	CThostFtdcQryTradingAccountField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID, BROKER_ID);
	strcpy_s(req.InvestorID, INVESTOR_ID);
	return pUserApi->ReqQryTradingAccount(&req, ++iRequestID);
}

///请求查询投资者
TRADEAPI_API int WINAPI ReqQryInvestor(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID)
{
	CThostFtdcQryInvestorField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID ,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	return pUserApi->ReqQryInvestor(&req, ++iRequestID);
}

///请求查询交易编码
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

///请求查询合约保证金率
TRADEAPI_API int WINAPI ReqQryInstrumentMarginRate(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID,TThostFtdcInstrumentIDType INSTRUMENT_ID,TThostFtdcHedgeFlagType HEDGE_FLAG)
{
	CThostFtdcQryInstrumentMarginRateField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID ,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.InstrumentID,INSTRUMENT_ID);
	if(HEDGE_FLAG != NULL)
		req.HedgeFlag = HEDGE_FLAG;						//*不*能采用null进行所有查询
	return pUserApi->ReqQryInstrumentMarginRate(&req, ++iRequestID);
}

///请求查询合约手续费率
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

///请求查询期权合约手续费率
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

///请求查询交易所
TRADEAPI_API int WINAPI ReqQryExchange(TThostFtdcExchangeIDType EXCHANGE_ID)
{
	CThostFtdcQryExchangeField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.ExchangeID,EXCHANGE_ID);
	return pUserApi->ReqQryExchange(&req, ++iRequestID);
}

///请求查询合约
TRADEAPI_API int WINAPI ReqQryInstrument(TThostFtdcInstrumentIDType INSTRUMENT_ID)
{
	CThostFtdcQryInstrumentField req;
	memset(&req, 0, sizeof(req));
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.InstrumentID, INSTRUMENT_ID);
	return pUserApi->ReqQryInstrument(&req, ++iRequestID);
}

///请求查询行情
TRADEAPI_API int WINAPI ReqQryDepthMarketData(TThostFtdcInstrumentIDType INSTRUMENT_ID)
{
	CThostFtdcQryDepthMarketDataField req;
	memset(&req,0,sizeof(req));
	if(INSTRUMENT_ID != NULL)
		strcpy_s(req.InstrumentID, INSTRUMENT_ID);
	return pUserApi->ReqQryDepthMarketData(&req, ++iRequestID);
}

///请求查询投资者结算结果
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

///查询持仓明细
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

///请求查询客户通知
TRADEAPI_API int WINAPI ReqQryNotice(TThostFtdcBrokerIDType BROKERID)
{
	CThostFtdcQryNoticeField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKERID);
	return pUserApi->ReqQryNotice(&req, ++iRequestID);
}

///请求查询结算信息确认
TRADEAPI_API int WINAPI ReqQrySettlementInfoConfirm(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID)
{
	CThostFtdcQrySettlementInfoConfirmField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	return pUserApi->ReqQrySettlementInfoConfirm(&req, ++iRequestID);
}

///请求查询**组合**持仓明细
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

///请求查询保证金监管系统经纪公司资金账户密钥
TRADEAPI_API int WINAPI ReqQryCFMMCTradingAccountKey(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID)
{
	CThostFtdcQryCFMMCTradingAccountKeyField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	return pUserApi->ReqQryCFMMCTradingAccountKey(&req, ++iRequestID);
}

///请求查询交易通知
TRADEAPI_API int WINAPI ReqQryTradingNotice(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID)
{
	CThostFtdcQryTradingNoticeField req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	return pUserApi->ReqQryTradingNotice(&req, ++iRequestID);
}

///请求查询经纪公司交易参数
TRADEAPI_API int WINAPI ReqQryBrokerTradingParams(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID)
{
	CThostFtdcQryBrokerTradingParamsField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	return pUserApi->ReqQryBrokerTradingParams(&req, ++iRequestID);
}

///请求查询经纪公司交易算法
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

///预埋单录入请求
TRADEAPI_API int WINAPI ReqParkedOrderInsert(CThostFtdcParkedOrderField *ParkedOrder)
{
	return pUserApi->ReqParkedOrderInsert(ParkedOrder, ++iRequestID);
}

///预埋撤单录入请求
TRADEAPI_API int WINAPI ReqParkedOrderAction(CThostFtdcParkedOrderActionField *ParkedOrderAction)
{
	return pUserApi->ReqParkedOrderAction(ParkedOrderAction, ++iRequestID);
}

///请求删除预埋单
TRADEAPI_API int WINAPI ReqRemoveParkedOrder(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID,TThostFtdcParkedOrderIDType ParkedOrder_ID)
{
	CThostFtdcRemoveParkedOrderField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	strcpy_s(req.ParkedOrderID,ParkedOrder_ID);
	return pUserApi->ReqRemoveParkedOrder(&req, ++iRequestID);
}

///请求删除预埋撤单
TRADEAPI_API int WINAPI ReqRemoveParkedOrderAction(TThostFtdcBrokerIDType BROKER_ID,TThostFtdcInvestorIDType INVESTOR_ID,TThostFtdcParkedOrderActionIDType ParkedOrderAction_ID)
{
	CThostFtdcRemoveParkedOrderActionField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,BROKER_ID);
	strcpy_s(req.InvestorID,INVESTOR_ID);
	strcpy_s(req.ParkedOrderActionID,ParkedOrderAction_ID);
	return pUserApi->ReqRemoveParkedOrderAction(&req, ++iRequestID);
}

///请求查询银期签约关系
TRADEAPI_API int WINAPI ReqQryAccountregister(TThostFtdcBrokerIDType Broker_ID, TThostFtdcAccountIDType Account_ID)
{
	CThostFtdcQryAccountregisterField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,Broker_ID);
	strcpy_s(req.AccountID,Account_ID);
	return pUserApi->ReqQryAccountregister(&req, ++iRequestID);
}

///请求查询转帐银行
TRADEAPI_API int WINAPI ReqQryTransferBank(TThostFtdcBankIDType Bank_ID,	TThostFtdcBankBrchIDType BankBrch_ID)
{
	CThostFtdcQryTransferBankField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BankID,Bank_ID);
	strcpy_s(req.BankBrchID,BankBrch_ID);
	return pUserApi->ReqQryTransferBank(&req, ++iRequestID);
}

///请求查询转帐流水
TRADEAPI_API int WINAPI ReqQryTransferSerial(TThostFtdcBrokerIDType Broker_ID,TThostFtdcAccountIDType Account_ID,TThostFtdcBankIDType Bank_ID)
{ 
	CThostFtdcQryTransferSerialField  req;
	memset(&req, 0, sizeof(req));
	strcpy_s(req.BrokerID,Broker_ID);
	strcpy_s(req.AccountID,Account_ID);
	strcpy_s(req.BankID,Bank_ID);
	return pUserApi->ReqQryTransferSerial(&req, ++iRequestID);
}

///请求查询签约银行
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

///请求查询预埋单
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

///请求查询预埋撤单
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

///期货发起银行资金转期货请求
TRADEAPI_API int WINAPI ReqFromBankToFutureByFuture(CThostFtdcReqTransferField *ReqTransfer)
{
	return pUserApi->ReqFromBankToFutureByFuture(ReqTransfer, ++iRequestID);
}

///期货发起期货资金转银行请求
TRADEAPI_API int WINAPI ReqFromFutureToBankByFuture(CThostFtdcReqTransferField *ReqTransfer)
{
	return pUserApi->ReqFromFutureToBankByFuture(ReqTransfer, ++iRequestID);
}

///期货发起查询银行余额请求
TRADEAPI_API int WINAPI ReqQueryBankAccountMoneyByFuture(CThostFtdcReqQueryAccountField *ReqQueryAccount)
{
	return pUserApi->ReqQueryBankAccountMoneyByFuture(ReqQueryAccount, ++iRequestID);
}

///请求查询期权交易成本
TRADEAPI_API int WINAPI ReqQryOptionInstrTradeCost(CThostFtdcQryOptionInstrTradeCostField *pQryOptionInstrTradeCost)
{
	return pUserApi->ReqQryOptionInstrTradeCost(pQryOptionInstrTradeCost, ++iRequestID);
}

///请求查询投资者品种/跨品种保证金
TRADEAPI_API int WINAPI ReqQryInvestorProductGroupMargin(CThostFtdcQryInvestorProductGroupMarginField *pQryInvestorProductGroupMargin) 
{
	return pUserApi->ReqQryInvestorProductGroupMargin(pQryInvestorProductGroupMargin, ++iRequestID);
}

///请求查询交易所调整保证金率
TRADEAPI_API int WINAPI ReqQryExchangeMarginRateAdjust(CThostFtdcQryExchangeMarginRateAdjustField *pQryExchangeMarginRateAdjust)
{
	return pUserApi->ReqQryExchangeMarginRateAdjust(pQryExchangeMarginRateAdjust, ++iRequestID);
}

///请求查询汇率
TRADEAPI_API int WINAPI ReqQryExchangeRate(CThostFtdcQryExchangeRateField *pQryExchangeRate)
{
	return pUserApi->ReqQryExchangeRate(pQryExchangeRate, ++iRequestID);
}

///请求查询产品报价汇率
TRADEAPI_API int WINAPI ReqQryProductExchRate(CThostFtdcQryProductExchRateField *pQryProductExchRate)
{
	return pUserApi->ReqQryProductExchRate(pQryProductExchRate, ++iRequestID);
}

///请求查询询价
TRADEAPI_API int WINAPI ReqQryForQuote(CThostFtdcQryForQuoteField *pQryForQuote)
{
	return pUserApi->ReqQryForQuote(pQryForQuote, ++iRequestID);
}

///询价录入请求
TRADEAPI_API int WINAPI ReqForQuoteInsert(CThostFtdcInputForQuoteField *pInputForQuote)
{
	return pUserApi->ReqForQuoteInsert(pInputForQuote, ++iRequestID);
}

///请求查询报价
TRADEAPI_API int WINAPI ReqQryQuote(CThostFtdcQryQuoteField *pQryQuote)
{
	return pUserApi->ReqQryQuote(pQryQuote, ++iRequestID);
}

///报价录入请求
TRADEAPI_API int WINAPI ReqQuoteInsert(CThostFtdcInputQuoteField *pInputQuote)
{
	return pUserApi->ReqQuoteInsert(pInputQuote, ++iRequestID);
}

///报价操作请求
TRADEAPI_API int WINAPI ReqQuoteAction(CThostFtdcInputQuoteActionField *pInputQuoteAction)
{
	return pUserApi->ReqQuoteAction(pInputQuoteAction, ++iRequestID);
}

///请求查询执行宣告
TRADEAPI_API int WINAPI ReqQryExecOrder(CThostFtdcQryExecOrderField *pQryExecOrder)
{
	return pUserApi->ReqQryExecOrder(pQryExecOrder, ++iRequestID);
}

///执行宣告录入请求
TRADEAPI_API int WINAPI ReqExecOrderInsert(CThostFtdcInputExecOrderField *pInputExecOrder)
{
	return pUserApi->ReqExecOrderInsert(pInputExecOrder, ++iRequestID);
}

///执行宣告操作请求
TRADEAPI_API int WINAPI ReqExecOrderAction(CThostFtdcInputExecOrderActionField *pInputExecOrderAction)
{
	return pUserApi->ReqExecOrderAction(pInputExecOrderAction, ++iRequestID);
}

///申请组合录入请求
TRADEAPI_API int WINAPI ReqCombActionInsert(CThostFtdcInputCombActionField *pInputCombAction)
{
	return pUserApi->ReqCombActionInsert(pInputCombAction, ++iRequestID);
}

///请求查询组合合约安全系数
TRADEAPI_API int WINAPI ReqQryCombInstrumentGuard(CThostFtdcQryCombInstrumentGuardField *pQryCombInstrumentGuard)
{
	return pUserApi->ReqQryCombInstrumentGuard(pQryCombInstrumentGuard, ++iRequestID);
}

///请求查询申请组合
TRADEAPI_API int WINAPI ReqQryCombAction(CThostFtdcQryCombActionField *pQryCombAction)
{
	return pUserApi->ReqQryCombAction(pQryCombAction, ++iRequestID);
}


///==================================== 回调函数 =======================================///
TRADEAPI_API void WINAPI RegOnFrontConnected(DefOnFrontConnected cb)	///当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
{	_OnFrontConnected = cb;	}

TRADEAPI_API void WINAPI RegOnFrontDisconnected(DefOnFrontDisconnected cb)	///当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
{	_OnFrontDisconnected = cb;	}

TRADEAPI_API void WINAPI RegOnHeartBeatWarning(DefOnHeartBeatWarning cb)	///心跳超时警告。当长时间未收到报文时，该方法被调用。
{	_OnHeartBeatWarning = cb;	}

TRADEAPI_API void WINAPI RegRspUserLogin(DefOnRspUserLogin cb)///登录请求响应
{	_OnRspUserLogin = cb;	}

TRADEAPI_API void WINAPI RegRspUserLogout(DefOnRspUserLogout cb)///登出请求响应
{	_OnRspUserLogout = cb;	}

TRADEAPI_API void WINAPI RegRspUserPasswordUpdate(DefOnRspUserPasswordUpdate cb)///用户口令更新请求响应
{	_OnRspUserPasswordUpdate = cb;	}

TRADEAPI_API void WINAPI RegRspTradingAccountPasswordUpdate(DefOnRspTradingAccountPasswordUpdate cb)///资金账户口令更新请求响应
{	_OnRspTradingAccountPasswordUpdate = cb;	}

TRADEAPI_API void WINAPI RegRspOrderInsert(DefOnRspOrderInsert cb)///报单录入请求响应
{	_OnRspOrderInsert = cb;	}

TRADEAPI_API void WINAPI RegRspParkedOrderInsert(DefOnRspParkedOrderInsert cb)///预埋单录入请求响应
{	_OnRspParkedOrderInsert = cb;	}

TRADEAPI_API void WINAPI RegRspParkedOrderAction(DefOnRspParkedOrderAction cb)///预埋撤单录入请求响应
{	_OnRspParkedOrderAction = cb;	}

TRADEAPI_API void WINAPI RegRspOrderAction(DefOnRspOrderAction cb)///报单操作请求响应
{	_OnRspOrderAction = cb;	}

TRADEAPI_API void WINAPI RegRspQueryMaxOrderVolume(DefOnRspQueryMaxOrderVolume cb)///查询最大报单数量响应
{	_OnRspQueryMaxOrderVolume = cb;	}

TRADEAPI_API void WINAPI RegRspSettlementInfoConfirm(DefOnRspSettlementInfoConfirm cb)///投资者结算结果确认响应
{	_OnRspSettlementInfoConfirm = cb;	}

TRADEAPI_API void WINAPI RegRspRemoveParkedOrder(DefOnRspRemoveParkedOrder cb)///删除预埋单响应
{	_OnRspRemoveParkedOrder = cb;	}

TRADEAPI_API void WINAPI RegRspRemoveParkedOrderAction(DefOnRspRemoveParkedOrderAction cb)///删除预埋撤单响应
{	_OnRspRemoveParkedOrderAction = cb;	}

TRADEAPI_API void WINAPI RegRspQryOrder(DefOnRspQryOrder cb)///请求查询报单响应
{	_OnRspQryOrder = cb;	}

TRADEAPI_API void WINAPI RegRspQryTrade(DefOnRspQryTrade cb)///请求查询成交响应
{	_OnRspQryTrade = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorPosition(DefOnRspQryInvestorPosition cb)///请求查询投资者持仓响应
{	_OnRspQryInvestorPosition = cb;	}

TRADEAPI_API void WINAPI RegRspQryTradingAccount(DefOnRspQryTradingAccount cb)///请求查询资金账户响应
{	_OnRspQryTradingAccount = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestor(DefOnRspQryInvestor cb)///请求查询投资者响应
{	_OnRspQryInvestor = cb;	}

TRADEAPI_API void WINAPI RegRspQryTradingCode(DefOnRspQryTradingCode cb)///请求查询交易编码响应
{	_OnRspQryTradingCode = cb;	}

TRADEAPI_API void WINAPI RegRspQryInstrumentMarginRate(DefOnRspQryInstrumentMarginRate cb)///请求查询合约保证金率响应
{	_OnRspQryInstrumentMarginRate = cb;	}

TRADEAPI_API void WINAPI RegRspQryInstrumentCommissionRate(DefOnRspQryInstrumentCommissionRate cb)///请求查询合约手续费率响应
{	_OnRspQryInstrumentCommissionRate = cb;	}

TRADEAPI_API void WINAPI RegRspQryExchange(DefOnRspQryExchange cb)///请求查询交易所响应
{	_OnRspQryExchange = cb;	}

TRADEAPI_API void WINAPI RegRspQryInstrument(DefOnRspQryInstrument cb)///请求查询合约响应
{	_OnRspQryInstrument = cb;	}

TRADEAPI_API void WINAPI RegRspQryDepthMarketData(DefOnRspQryDepthMarketData cb)///请求查询行情响应
{	_OnRspQryDepthMarketData = cb;	}

TRADEAPI_API void WINAPI RegRspQrySettlementInfo(DefOnRspQrySettlementInfo cb)///请求查询投资者结算结果响应
{	_OnRspQrySettlementInfo = cb;	}

TRADEAPI_API void WINAPI RegRspQryTransferBank(DefOnRspQryTransferBank cb)///请求查询转帐银行响应
{	_OnRspQryTransferBank = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorPositionDetail(DefOnRspQryInvestorPositionDetail cb)///请求查询投资者持仓明细响应
{	_OnRspQryInvestorPositionDetail = cb;	}

TRADEAPI_API void WINAPI RegRspQryNotice(DefOnRspQryNotice cb)///请求查询客户通知响应
{	_OnRspQryNotice = cb;	}

TRADEAPI_API void WINAPI RegRspQrySettlementInfoConfirm(DefOnRspQrySettlementInfoConfirm cb)///请求查询结算信息确认响应
{	_OnRspQrySettlementInfoConfirm = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorPositionCombineDetail(DefOnRspQryInvestorPositionCombineDetail cb)///请求查询投资者持仓明细响应
{	_OnRspQryInvestorPositionCombineDetail = cb;	}

TRADEAPI_API void WINAPI RegRspQryCFMMCTradingAccountKey(DefOnRspQryCFMMCTradingAccountKey cb)///查询保证金监管系统经纪公司资金账户密钥响应
{	_OnRspQryCFMMCTradingAccountKey = cb;	}

TRADEAPI_API void WINAPI RegRspQryAccountregister(DefOnRspQryAccountregister cb) ///请求查询银期签约关系响应
{	_OnRspQryAccountregister = cb;	}

TRADEAPI_API void WINAPI RegRspQryTransferSerial(DefOnRspQryTransferSerial cb)///请求查询转帐流水响应
{	_OnRspQryTransferSerial = cb;	}

TRADEAPI_API void WINAPI RegRspError(DefOnRspError cb)///错误应答
{	_OnRspError = cb;	}

TRADEAPI_API void WINAPI RegRtnOrder(DefOnRtnOrder cb)///报单通知
{	_OnRtnOrder = cb;	}

TRADEAPI_API void WINAPI RegRtnTrade(DefOnRtnTrade cb)///成交通知
{	_OnRtnTrade = cb;	}

TRADEAPI_API void WINAPI RegErrRtnOrderInsert(DefOnErrRtnOrderInsert cb)///报单录入错误回报
{	_OnErrRtnOrderInsert = cb;	}

TRADEAPI_API void WINAPI RegErrRtnOrderAction(DefOnErrRtnOrderAction cb)///报单操作错误回报
{	_OnErrRtnOrderAction = cb;	}

TRADEAPI_API void WINAPI RegRtnInstrumentStatus(DefOnRtnInstrumentStatus cb)///合约交易状态通知
{	_OnRtnInstrumentStatus = cb;	}

TRADEAPI_API void WINAPI RegRtnTradingNotice(DefOnRtnTradingNotice cb)///交易通知
{	_OnRtnTradingNotice = cb;	}

TRADEAPI_API void WINAPI RegRtnErrorConditionalOrder(DefOnRtnErrorConditionalOrder cb)///提示条件单校验错误
{	_OnRtnErrorConditionalOrder = cb;	}

TRADEAPI_API void WINAPI RegRspQryContractBank(DefOnRspQryContractBank cb)///请求查询签约银行响应
{	_OnRspQryContractBank = cb;	}

TRADEAPI_API void WINAPI RegRspQryParkedOrder(DefOnRspQryParkedOrder cb)///请求查询预埋单响应
{	_OnRspQryParkedOrder = cb;	}

TRADEAPI_API void WINAPI RegRspQryParkedOrderAction(DefOnRspQryParkedOrderAction cb)///请求查询预埋撤单响应
{	_OnRspQryParkedOrderAction = cb;	}

TRADEAPI_API void WINAPI RegRspQryTradingNotice(DefOnRspQryTradingNotice cb)///请求查询交易通知响应
{	_OnRspQryTradingNotice = cb;	}

TRADEAPI_API void WINAPI RegRspQryBrokerTradingParams(DefOnRspQryBrokerTradingParams cb)///请求查询经纪公司交易参数响应
{	_OnRspQryBrokerTradingParams = cb;	}

TRADEAPI_API void WINAPI RegRspQryBrokerTradingAlgos(DefOnRspQryBrokerTradingAlgos cb)///请求查询经纪公司交易算法响应
{	_OnRspQryBrokerTradingAlgos = cb;	}

TRADEAPI_API void WINAPI RegRtnFromBankToFutureByBank(DefOnRtnFromBankToFutureByBank cb)///银行发起银行资金转期货通知
{	_OnRtnFromBankToFutureByBank = cb;	}

TRADEAPI_API void WINAPI RegRtnFromFutureToBankByBank(DefOnRtnFromFutureToBankByBank cb)///银行发起期货资金转银行通知
{	_OnRtnFromFutureToBankByBank = cb;	}

TRADEAPI_API void WINAPI RegRtnRepealFromBankToFutureByBank(DefOnRtnRepealFromBankToFutureByBank cb)///银行发起冲正银行转期货通知
{	_OnRtnRepealFromBankToFutureByBank = cb;	}

TRADEAPI_API void WINAPI RegRtnRepealFromFutureToBankByBank(DefOnRtnRepealFromFutureToBankByBank cb)///银行发起冲正期货转银行通知
{	_OnRtnRepealFromFutureToBankByBank = cb;	}

TRADEAPI_API void WINAPI RegRtnFromBankToFutureByFuture(DefOnRtnFromBankToFutureByFuture cb)///期货发起银行资金转期货通知
{	_OnRtnFromBankToFutureByFuture = cb;	}

TRADEAPI_API void WINAPI RegRtnFromFutureToBankByFuture(DefOnRtnFromFutureToBankByFuture cb)///期货发起期货资金转银行通知
{	_OnRtnFromFutureToBankByFuture = cb;	}

TRADEAPI_API void WINAPI RegRtnRepealFromBankToFutureByFutureManual(DefOnRtnRepealFromBankToFutureByFutureManual cb)///系统运行时期货端手工发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
{	_OnRtnRepealFromBankToFutureByFutureManual = cb;	}

TRADEAPI_API void WINAPI RegRtnRepealFromFutureToBankByFutureManual(DefOnRtnRepealFromFutureToBankByFutureManual cb)///系统运行时期货端手工发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
{	_OnRtnRepealFromFutureToBankByFutureManual = cb;	}

TRADEAPI_API void WINAPI RegRtnQueryBankBalanceByFuture(DefOnRtnQueryBankBalanceByFuture cb)///期货发起查询银行余额通知
{	_OnRtnQueryBankBalanceByFuture = cb;	}

TRADEAPI_API void WINAPI RegErrRtnBankToFutureByFuture(DefOnErrRtnBankToFutureByFuture cb)///期货发起银行资金转期货错误回报
{	_OnErrRtnBankToFutureByFuture = cb;	}

TRADEAPI_API void WINAPI RegErrRtnFutureToBankByFuture(DefOnErrRtnFutureToBankByFuture cb)///期货发起期货资金转银行错误回报
{	_OnErrRtnFutureToBankByFuture = cb;	}

TRADEAPI_API void WINAPI RegErrRtnRepealBankToFutureByFutureManual(DefOnErrRtnRepealBankToFutureByFutureManual cb)///系统运行时期货端手工发起冲正银行转期货错误回报
{	_OnErrRtnRepealBankToFutureByFutureManual = cb;	}

TRADEAPI_API void WINAPI RegErrRtnRepealFutureToBankByFutureManual(DefOnErrRtnRepealFutureToBankByFutureManual cb)///系统运行时期货端手工发起冲正期货转银行错误回报
{	_OnErrRtnRepealFutureToBankByFutureManual = cb;	}

TRADEAPI_API void WINAPI RegErrRtnQueryBankBalanceByFuture(DefOnErrRtnQueryBankBalanceByFuture cb)///期货发起查询银行余额错误回报
{	_OnErrRtnQueryBankBalanceByFuture = cb;	}

TRADEAPI_API void WINAPI RegRtnRepealFromBankToFutureByFuture(DefOnRtnRepealFromBankToFutureByFuture cb)///期货发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
{	_OnRtnRepealFromBankToFutureByFuture = cb;	}

TRADEAPI_API void WINAPI RegRtnRepealFromFutureToBankByFuture(DefOnRtnRepealFromFutureToBankByFuture cb)///期货发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
{	_OnRtnRepealFromFutureToBankByFuture = cb;	}

TRADEAPI_API void WINAPI RegRspFromBankToFutureByFuture(DefOnRspFromBankToFutureByFuture cb)///期货发起银行资金转期货应答
{	_OnRspFromBankToFutureByFuture = cb;	}

TRADEAPI_API void WINAPI RegRspFromFutureToBankByFuture(DefOnRspFromFutureToBankByFuture cb)///期货发起期货资金转银行应答
{	_OnRspFromFutureToBankByFuture = cb;	}

TRADEAPI_API void WINAPI RegRspQueryBankAccountMoneyByFuture(DefOnRspQueryBankAccountMoneyByFuture cb)///期货发起查询银行余额应答
{	_OnRspQueryBankAccountMoneyByFuture = cb;	}

TRADEAPI_API void WINAPI RegRspQryOptionInstrCommRate(DefOnRspQryOptionInstrCommRate cb)///请求查询期权合约手续费响应
{	_OnRspQryOptionInstrCommRate = cb;	}

TRADEAPI_API void WINAPI RegRspQryOptionInstrTradeCost(DefOnRspQryOptionInstrTradeCost cb)///请求查询期权交易成本响应
{	_OnRspQryOptionInstrTradeCost = cb;	}

TRADEAPI_API void WINAPI RegRspQryForQuote(DefOnRspQryForQuote cb)///请求查询询价响应
{	_OnRspQryForQuote = cb;	}

TRADEAPI_API void WINAPI RegRtnForQuoteRsp(DefOnRtnForQuoteRsp cb)///询价通知
{	_OnRtnForQuoteRsp = cb;	}

TRADEAPI_API void WINAPI RegRspForQuoteInsert(DefOnRspForQuoteInsert cb)///询价录入请求响应
{	_OnRspForQuoteInsert = cb;	}

TRADEAPI_API void WINAPI RegErrRtnForQuoteInsert(DefOnErrRtnForQuoteInsert cb)///询价录入错误回报
{	_OnErrRtnForQuoteInsert = cb;	}

TRADEAPI_API void WINAPI RegRspQryQuote(DefOnRspQryQuote cb)///请求查询报价响应
{	_OnRspQryQuote = cb;	}

TRADEAPI_API void WINAPI RegRtnQuote(DefOnRtnQuote cb)///报价通知
{	_OnRtnQuote = cb;	}

TRADEAPI_API void WINAPI RegRspQuoteInsert(DefOnRspQuoteInsert cb)///报价录入请求响应
{	_OnRspQuoteInsert = cb;	}

TRADEAPI_API void WINAPI RegErrRtnQuoteInsert(DefOnErrRtnQuoteInsert cb)///报价录入错误回报
{	_OnErrRtnQuoteInsert = cb;	}

TRADEAPI_API void WINAPI RegRspQuoteAction(DefOnRspQuoteAction cb)///报价操作请求响应
{	_OnRspQuoteAction = cb;	}

TRADEAPI_API void WINAPI RegErrRtnQuoteAction(DefOnErrRtnQuoteAction cb)///报价操作错误回报
{	_OnErrRtnQuoteAction = cb;	}

TRADEAPI_API void WINAPI RegRspQryExecOrder(DefOnRspQryExecOrder cb)///请求查询执行宣告响应
{	_OnRspQryExecOrder = cb;	}

TRADEAPI_API void WINAPI RegRtnExecOrder(DefOnRtnExecOrder cb)///执行宣告通知
{	_OnRtnExecOrder = cb;	}

TRADEAPI_API void WINAPI RegRspExecOrderInsert(DefOnRspExecOrderInsert cb)///执行宣告录入请求响应
{	_OnRspExecOrderInsert = cb;	}

TRADEAPI_API void WINAPI RegErrRtnExecOrderInsert(DefOnErrRtnExecOrderInsert cb)///执行宣告录入错误回报
{	_OnErrRtnExecOrderInsert = cb;	}

TRADEAPI_API void WINAPI RegRspExecOrderAction(DefOnRspExecOrderAction cb)///执行宣告操作请求响应
{	_OnRspExecOrderAction = cb;	}

TRADEAPI_API void WINAPI RegErrRtnExecOrderAction(DefOnErrRtnExecOrderAction cb)///执行宣告操作错误回报
{	_OnErrRtnExecOrderAction = cb;	}

TRADEAPI_API void WINAPI RegRspCombActionInsert(DefOnRspCombActionInsert cb)///申请组合录入请求响应
{	_OnRspCombActionInsert = cb;	}

TRADEAPI_API void WINAPI RegRspQryCombInstrumentGuard(DefOnRspQryCombInstrumentGuard cb)///请求查询组合合约安全系数响应
{	_OnRspQryCombInstrumentGuard = cb;	}

TRADEAPI_API void WINAPI RegRspQryCombAction(DefOnRspQryCombAction cb)///请求查询申请组合响应
{	_OnRspQryCombAction = cb;	}

TRADEAPI_API void WINAPI RegRtnCombAction(DefOnRtnCombAction cb) ///申请组合通知
{	_OnRtnCombAction = cb;	}

TRADEAPI_API void WINAPI RegErrRtnCombActionInsert(DefOnErrRtnCombActionInsert cb) ///申请组合录入错误回报
{	_OnErrRtnCombActionInsert = cb;	}

TRADEAPI_API void WINAPI RegRspQryInvestorProductGroupMargin(DefOnRspQryInvestorProductGroupMargin cb) ///请求查询投资者品种/跨品种保证金响应
{	_OnRspQryInvestorProductGroupMargin = cb;	}

TRADEAPI_API void WINAPI RegRspQryExchangeMarginRateAdjust(DefOnRspQryExchangeMarginRateAdjust cb) ///请求查询交易所调整保证金率响应
{	_OnRspQryExchangeMarginRateAdjust = cb;	}

TRADEAPI_API void WINAPI RegRspQryExchangeRate(DefOnRspQryExchangeRate cb) ///请求查询汇率响应
{	_OnRspQryExchangeRate = cb;	}

TRADEAPI_API void WINAPI RegRspQryProductExchRate(DefOnRspQryProductExchRate cb) ///请求查询产品报价汇率
{	_OnRspQryProductExchRate = cb;	}

TRADEAPI_API void WINAPI RegRspBatchOrderAction(DefOnRspBatchOrderAction cb) ///请求查询汇率响应
{	_OnRspBatchOrderAction = cb;		}

TRADEAPI_API void WINAPI RegRspQryProductGroup(DefOnRspQryProductGroup cb) ///请求查询汇率响应
{	_OnRspQryProductGroup = cb;		}

TRADEAPI_API void WINAPI RegRspQryMMInstrumentCommissionRate(DefOnRspQryMMInstrumentCommissionRate cb) ///请求查询汇率响应
{	_OnRspQryMMInstrumentCommissionRate = cb;		}

TRADEAPI_API void WINAPI RegRspQryMMOptionInstrCommRate(DefOnRspQryMMOptionInstrCommRate cb) ///请求查询汇率响应
{	_OnRspQryMMOptionInstrCommRate = cb;		}

TRADEAPI_API void WINAPI RegRspQryInstrumentOrderCommRate(DefOnRspQryInstrumentOrderCommRate cb) ///请求查询汇率响应
{	_OnRspQryInstrumentOrderCommRate = cb;		}

TRADEAPI_API void WINAPI RegRtnBulletin(DefOnRtnBulletin cb) ///请求查询汇率响应
{	_OnRtnBulletin = cb;		}

TRADEAPI_API void WINAPI RegErrRtnBatchOrderAction(DefOnErrRtnBatchOrderAction cb) ///请求查询汇率响应
{	_OnErrRtnBatchOrderAction = cb;		}

// 请求编号
//extern int iRequestID;

///当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
void CTraderSpi::OnFrontConnected()
{
	if(_OnFrontConnected!=NULL)
	{
		((DefOnFrontConnected)_OnFrontConnected)();
	}
}

///当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
void CTraderSpi::OnFrontDisconnected(int nReason)
{
	if(_OnFrontDisconnected != NULL)
	{
		((DefOnFrontDisconnected)_OnFrontDisconnected)(nReason);
	}
}

	///心跳超时警告。当长时间未收到报文时，该方法被调用。  @param nTimeLapse 距离上次接收报文的时间
void CTraderSpi::OnHeartBeatWarning(int nTimeLapse)
{
	if(_OnHeartBeatWarning != NULL)
	{
		((DefOnHeartBeatWarning)_OnHeartBeatWarning)(nTimeLapse);
	}
}

///客户端认证响应
void CTraderSpi::OnRspAuthenticate(CThostFtdcRspAuthenticateField *pRspAuthenticateField, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///登录请求响应
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

///登出请求响应
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

///用户口令更新请求响应
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

///资金账户口令更新请求响应
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

///报单录入请求响应
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

///预埋单录入请求响应
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

///预埋撤单录入请求响应
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

///报单操作请求响应
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

///查询最大报单数量响应
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

///投资者结算结果确认响应
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

///删除预埋单响应
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

///删除预埋撤单响应
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

///请求查询报单响应
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

///请求查询成交响应
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

///请求查询投资者持仓响应
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

///请求查询资金账户响应
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

///请求查询投资者响应
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

///请求查询交易编码响应
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

///请求查询合约保证金率响应
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

///请求查询合约手续费率响应
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

///请求查询交易所响应
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

///请求查询产品响应
void CTraderSpi::OnRspQryProduct(CThostFtdcProductField *pProduct, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询合约响应
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

///请求查询行情响应
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

///请求查询投资者结算结果响应
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

///请求查询转帐银行响应
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

///请求查询投资者持仓明细响应
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

///请求查询客户通知响应
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

///请求查询结算信息确认响应
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

///请求查询投资者持仓明细响应
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

///查询保证金监管系统经纪公司资金账户密钥响应
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

///保证金监控中心用户令牌
void CTraderSpi::OnRtnCFMMCTradingAccountToken(CThostFtdcCFMMCTradingAccountTokenField *pCFMMCTradingAccountToken) {};

///批量报单操作错误回报
void CTraderSpi::OnErrRtnBatchOrderAction(CThostFtdcBatchOrderActionField *pBatchOrderAction, CThostFtdcRspInfoField *pRspInfo) {};

///请求查询转帐银行响应
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

///请求查询转帐流水响应
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

///错误应答
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

///报单通知
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

///成交通知
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

///报单录入错误回报
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

///报单操作错误回报
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

///合约交易状态通知
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

///交易所公告通知
void CTraderSpi::OnRtnBulletin(CThostFtdcBulletinField *pBulletin) {};

///交易通知
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

///提示条件单校验错误
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

///请求查询签约银行响应
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

///请求查询预埋单响应
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

///请求查询预埋撤单响应
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

///请求查询交易通知响应
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

///请求查询经纪公司交易参数响应
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

///请求查询经纪公司交易算法响应
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

///请求查询监控中心用户令牌
void CTraderSpi::OnRspQueryCFMMCTradingAccountToken(CThostFtdcQueryCFMMCTradingAccountTokenField *pQueryCFMMCTradingAccountToken, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///银行发起银行资金转期货通知
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

///银行发起期货资金转银行通知
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

///银行发起冲正银行转期货通知
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

///银行发起冲正期货转银行通知
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

///期货发起银行资金转期货通知
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

///期货发起期货资金转银行通知
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

///系统运行时期货端手工发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
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

///系统运行时期货端手工发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
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

///期货发起查询银行余额通知
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

///期货发起银行资金转期货错误回报
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

///期货发起期货资金转银行错误回报
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

///系统运行时期货端手工发起冲正银行转期货错误回报
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

///系统运行时期货端手工发起冲正期货转银行错误回报
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

///期货发起查询银行余额错误回报
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

///期货发起冲正银行转期货请求，银行处理完毕后报盘发回的通知
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

///期货发起冲正期货转银行请求，银行处理完毕后报盘发回的通知
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

///期货发起银行资金转期货应答
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

///期货发起期货资金转银行应答
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

///期货发起查询银行余额应答
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

///银行发起银期开户通知
void CTraderSpi::OnRtnOpenAccountByBank(CThostFtdcOpenAccountField *pOpenAccount)
{}

///银行发起银期销户通知
void CTraderSpi::OnRtnCancelAccountByBank(CThostFtdcCancelAccountField *pCancelAccount)
{}

///银行发起变更银行账号通知
void CTraderSpi::OnRtnChangeAccountByBank(CThostFtdcChangeAccountField *pChangeAccount)
{}

///请求查询期权合约手续费响应
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

///请求查询期权交易成本响应
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

///请求查询仓单折抵信息响应
void CTraderSpi::OnRspQryEWarrantOffset(CThostFtdcEWarrantOffsetField *pEWarrantOffset, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) {};

///请求查询投资者品种/跨品种保证金响应
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

///请求查询交易所保证金率响应
void CTraderSpi::OnRspQryExchangeMarginRate(CThostFtdcExchangeMarginRateField *pExchangeMarginRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询交易所调整保证金率响应
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


///请求查询产品组
void CTraderSpi::OnRspQryProductGroup(CThostFtdcProductGroupField *pProductGroup, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) {};

///请求查询做市商合约手续费率响应
void CTraderSpi::OnRspQryMMInstrumentCommissionRate(CThostFtdcMMInstrumentCommissionRateField *pMMInstrumentCommissionRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) {};

///请求查询做市商期权合约手续费响应
void CTraderSpi::OnRspQryMMOptionInstrCommRate(CThostFtdcMMOptionInstrCommRateField *pMMOptionInstrCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) {};

///请求查询报单手续费响应
void CTraderSpi::OnRspQryInstrumentOrderCommRate(CThostFtdcInstrumentOrderCommRateField *pInstrumentOrderCommRate, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) {};

///请求查询汇率响应
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

///请求查询二级代理操作员银期权限响应
void CTraderSpi::OnRspQrySecAgentACIDMap(CThostFtdcSecAgentACIDMapField *pSecAgentACIDMap, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast)
{}

///请求查询产品报价汇率
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

///请求查询询价响应
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

///询价通知
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

///询价录入请求响应
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

///询价录入错误回报
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

///请求查询报价响应
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

///报价通知
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

///报价录入请求响应
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

///报价录入错误回报
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

///报价操作请求响应
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

///报价操作错误回报
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


///请求查询执行宣告响应
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

///执行宣告通知
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

///执行宣告录入请求响应
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

///执行宣告录入错误回报
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


///执行宣告操作请求响应
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

///执行宣告操作错误回报
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

///批量报单操作请求响应
void CTraderSpi::OnRspBatchOrderAction(CThostFtdcInputBatchOrderActionField *pInputBatchOrderAction, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) {};

///申请组合录入请求响应
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

///请求查询组合合约安全系数响应
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

///请求查询申请组合响应
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

///申请组合通知
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

///申请组合录入错误回报
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

//针对收到空反馈的处理
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

