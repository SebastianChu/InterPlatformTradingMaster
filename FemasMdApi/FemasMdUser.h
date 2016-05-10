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
#include ".\api\USTPFtdcMdUserApi.h"
#include ".\api\USTPFtdcUserApiDataType.h"
#include ".\api\USTPFtdcUserApiStruct.h"

// USER_API参数
CUstpFtdcMduserApi* pUserApi;

//回调函数
void* _OnFrontConnected;
void* _OnFrontDisconnected;
void* _OnHeartBeatWarning;
void* _OnPackageStart;
void* _OnPackageEnd;
void* _OnRspError;
void* _OnRspUserLogin;
void* _OnRspUserLogout;
void* _OnRspSubscribeTopic;
void* _OnRspQryTopic;
void* _OnRtnDepthMarketData;
void* _OnRspSubMarketData;
void* _OnRspUnSubMarketData;


///当客户端与交易后台建立起通信连接时（还未登录前），该方法被调用。
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

///报文回调开始通知。当API收到一个报文后，首先调用本方法，然后是各数据域的回调，最后是报文回调结束通知。
///@param nTopicID 主题代码（如私有流、公共流、行情流等）
///@param nSequenceNo 报文序号
typedef int (WINAPI *DefOnPackageStart)(int nTopicID, int nSequenceNo);
	
///报文回调结束通知。当API收到一个报文后，首先调用报文回调开始通知，然后是各数据域的回调，最后调用本方法。
///@param nTopicID 主题代码（如私有流、公共流、行情流等）
///@param nSequenceNo 报文序号
typedef int (WINAPI *DefOnPackageEnd)(int nTopicID, int nSequenceNo);

///错误应答
typedef int (WINAPI *DefOnRspError)(CUstpFtdcRspInfoField *pRspInfo,int nRequestID, bool bIsLast);

///风控前置系统用户登录应答
typedef int (WINAPI *DefOnRspUserLogin)(CUstpFtdcRspUserLoginField *pRspUserLogin,CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

//用户退出应答
typedef int (WINAPI *DefOnRspUserLogout)(CUstpFtdcRspUserLogoutField *pUserLogout, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

///订阅主题应答
typedef int (WINAPI *DefOnRspSubscribeTopic)(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///主题查询应答
typedef int (WINAPI *DefOnRspQryTopic)(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

//深度行情通知
typedef int (WINAPI *DefOnRtnDepthMarketData)(CUstpFtdcDepthMarketDataField *pDepthMarketData);

//订阅合约的相关信息
typedef int (WINAPI *DefOnRspSubMarketData)(CUstpFtdcSpecificInstrumentField *pSpecificInstrument, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

//退订合约的相关信息
typedef int (WINAPI *DefOnRspUnSubMarketData)(CUstpFtdcSpecificInstrumentField *pSpecificInstrument, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

// 此类是从 MdApi.dll 导出的
class CMdSpi : public CUstpFtdcMduserSpi
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

	///订阅主题应答
	virtual void OnRspSubscribeTopic(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///主题查询应答
	virtual void OnRspQryTopic(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///深度行情通知
	virtual void OnRtnDepthMarketData(CUstpFtdcDepthMarketDataField *pDepthMarketData) ;

	///订阅合约的相关信息
	virtual void OnRspSubMarketData(CUstpFtdcSpecificInstrumentField *pSpecificInstrument, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///退订合约的相关信息
	virtual void OnRspUnSubMarketData(CUstpFtdcSpecificInstrumentField *pSpecificInstrument, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

};