// ���� ifdef ���Ǵ���ʹ�� DLL �������򵥵�
// ��ı�׼�������� DLL �е������ļ��������������϶���� MDAPI_EXPORTS
// ���ű���ġ���ʹ�ô� DLL ��
// �κ�������Ŀ�ϲ�Ӧ����˷��š�������Դ�ļ��а������ļ����κ�������Ŀ���Ὣ
// MDAPI_API ������Ϊ�Ǵ� DLL ����ģ����� DLL ���ô˺궨���
// ������Ϊ�Ǳ������ġ�
//#ifdef MDAPI_EXPORTS
//#define MDAPI_API __declspec(dllexport)
//#else
//#define MDAPI_API __declspec(dllimport)
//#endif
//#include ".\api\ThostFtdcMdApi.h"

#pragma once
#define MDAPI_API __declspec(dllexport)
#define WINAPI      __stdcall
#define WIN32_LEAN_AND_MEAN             //  �� Windows ͷ�ļ����ų�����ʹ�õ���Ϣ

#include "stdafx.h"
#include ".\api\USTPFtdcMdUserApi.h"
#include ".\api\USTPFtdcUserApiDataType.h"
#include ".\api\USTPFtdcUserApiStruct.h"

// USER_API����
CUstpFtdcMduserApi* pUserApi;

//�ص�����
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


///���ͻ����뽻�׺�̨������ͨ������ʱ����δ��¼ǰ�����÷��������á�
typedef int (WINAPI *DefOnFrontConnected)(void);
	
///���ͻ����뽻�׺�̨ͨ�����ӶϿ�ʱ���÷��������á���������������API���Զ��������ӣ��ͻ��˿ɲ�������
///@param nReason ����ԭ��
///        0x1001 �����ʧ��
///        0x1002 ����дʧ��
///        0x2001 ����������ʱ
///        0x2002 ��������ʧ��
///        0x2003 �յ�������
typedef int (WINAPI *DefOnFrontDisconnected)(int nReason);

///������ʱ���档����ʱ��δ�յ�����ʱ���÷��������á�
	///@param nTimeLapse �����ϴν��ձ��ĵ�ʱ��
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
typedef int (WINAPI *DefOnRspError)(CUstpFtdcRspInfoField *pRspInfo,int nRequestID, bool bIsLast);

///���ǰ��ϵͳ�û���¼Ӧ��
typedef int (WINAPI *DefOnRspUserLogin)(CUstpFtdcRspUserLoginField *pRspUserLogin,CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

//�û��˳�Ӧ��
typedef int (WINAPI *DefOnRspUserLogout)(CUstpFtdcRspUserLogoutField *pUserLogout, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

///��������Ӧ��
typedef int (WINAPI *DefOnRspSubscribeTopic)(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

///�����ѯӦ��
typedef int (WINAPI *DefOnRspQryTopic)(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

//�������֪ͨ
typedef int (WINAPI *DefOnRtnDepthMarketData)(CUstpFtdcDepthMarketDataField *pDepthMarketData);

//���ĺ�Լ�������Ϣ
typedef int (WINAPI *DefOnRspSubMarketData)(CUstpFtdcSpecificInstrumentField *pSpecificInstrument, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

//�˶���Լ�������Ϣ
typedef int (WINAPI *DefOnRspUnSubMarketData)(CUstpFtdcSpecificInstrumentField *pSpecificInstrument, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast);

// �����Ǵ� MdApi.dll ������
class CMdSpi : public CUstpFtdcMduserSpi
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

	///��������Ӧ��
	virtual void OnRspSubscribeTopic(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///�����ѯӦ��
	virtual void OnRspQryTopic(CUstpFtdcDisseminationField *pDissemination, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///�������֪ͨ
	virtual void OnRtnDepthMarketData(CUstpFtdcDepthMarketDataField *pDepthMarketData) ;

	///���ĺ�Լ�������Ϣ
	virtual void OnRspSubMarketData(CUstpFtdcSpecificInstrumentField *pSpecificInstrument, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

	///�˶���Լ�������Ϣ
	virtual void OnRspUnSubMarketData(CUstpFtdcSpecificInstrumentField *pSpecificInstrument, CUstpFtdcRspInfoField *pRspInfo, int nRequestID, bool bIsLast) ;

};