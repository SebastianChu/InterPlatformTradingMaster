// 下列 ifdef 块是创建使从 DLL 导出更简单的
// 宏的标准方法。此 DLL 中的所有文件都是用命令行上定义的 MDAPI_EXPORTS
// 符号编译的。在使用此 DLL 的
// 任何其他项目上不应定义此符号。这样，源文件中包含此文件的任何其他项目都会将
// MDAPI_API 函数视为是从 DLL 导入的，而此 DLL 则将用此宏定义的
// 符号视为是被导出的。
//#ifdef MDAPI_EXPORTS
//#define MDAPI_API __declspec(dllexport)
//#else
//#define MDAPI_API __declspec(dllimport)
//#endif
//#include ".\api\ThostFtdcMdApi.h"

#pragma once
#define MDAPI_API __declspec(dllexport)
#define WINAPI      __stdcall
#define WIN32_LEAN_AND_MEAN             //  从 Windows 头文件中排除极少使用的信息

#include "stdafx.h"
#include ".\api\ThostFtdcMdApi.h"
#include ".\api\ThostFtdcUserApiDataType.h"
#include ".\api\ThostFtdcUserApiStruct.h"

// USER_API参数
CThostFtdcMdApi* pUserApi;

//回调函数
void* _OnFrontConnected;
void* _OnFrontDisconnected;
void* _OnHeartBeatWarning;
void* _OnRspUserLogin;
void* _OnRspUserLogout;
void* _OnRspSubMarketData;
void* _OnRspUnSubMarketData;
void* _OnRtnDepthMarketData;
void* _OnRspSubForQuoteRsp;
void* _OnRspUnSubForQuoteRsp;
void* _OnRtnForQuoteRsp;
void* _OnRspError;

///连接
typedef int (WINAPI *DefOnFrontConnected)(void);

///当客户端与交易后台通信连接断开时，该方法被调用。当发生这个情况后，API会自动重新连接，客户端可不做处理。
///@param nReason 错误原因
///        0x1001 网络读失败
///        0x1002 网络写失败
///        0x2001 接收心跳超时
///        0x2002 发送心跳失败
///        0x2003 收到错误报文
typedef int (WINAPI *DefOnFrontDisconnected)(int nReason);

///心跳超时警告。当长时间未收到报文时，该方法被调用。
///@param nTimeLapse 距离上次接收报文的时间
typedef int (WINAPI *DefOnHeartBeatWarning)(int nTimeLapse);

///登录请求响应
typedef int (WINAPI *DefOnRspUserLogin)(CThostFtdcRspUserLoginField *pRspUserLogin,CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

//登出请求响应
typedef int (WINAPI *DefOnRspUserLogout)(CThostFtdcUserLogoutField *pUserLogout, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

///错误应答
typedef int (WINAPI *DefOnRspError)(CThostFtdcRspInfoField *pRspInfo,int nRequestID, bool bIsLast);

//订阅行情应答
typedef int (WINAPI *DefOnRspSubMarketData)(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

//取消行情应答
typedef int (WINAPI *DefOnRspUnSubMarketData)(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

//深度行情通知
typedef int (WINAPI *DefOnRtnDepthMarketData)(CThostFtdcDepthMarketDataField *pDepthMarketData);

///订阅询价应答
typedef int (WINAPI *DefOnRspSubForQuoteRsp)(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

///取消订阅询价应答
typedef int (WINAPI *DefOnRspUnSubForQuoteRsp)(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

///询价通知
typedef int (WINAPI *DefOnRtnForQuoteRsp)(CThostFtdcForQuoteRspField *pForQuoteRsp);

// 此类是从 MdApi.dll 导出的
class CMdSpi : public CThostFtdcMdSpi
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
	
	///登录请求响应
	virtual void OnRspUserLogin(CThostFtdcRspUserLoginField *pRspUserLogin,	CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///登出请求响应
	virtual void OnRspUserLogout(CThostFtdcUserLogoutField *pUserLogout, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///错误应答
	virtual void OnRspError(CThostFtdcRspInfoField *pRspInfo,int nRequestID, bool bIsLast);

	///订阅行情应答
	virtual void OnRspSubMarketData(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///取消订阅行情应答
	virtual void OnRspUnSubMarketData(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///深度行情通知
	virtual void OnRtnDepthMarketData(CThostFtdcDepthMarketDataField *pDepthMarketData);

	///订阅询价应答
	virtual void OnRspSubForQuoteRsp(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///取消订阅询价应答
	virtual void OnRspUnSubForQuoteRsp(CThostFtdcSpecificInstrumentField *pSpecificInstrument, CThostFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

	///询价通知
	virtual void OnRtnForQuoteRsp(CThostFtdcForQuoteRspField *pForQuoteRsp);

};