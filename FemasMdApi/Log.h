#pragma once

#define LOG4CPLUS_STATIC

#include <log4cplus/logger.h>

using namespace log4cplus;

#define LOG_FATAL(msg)  LOG4CPLUS_FATAL(CLog::root, msg)   
#define LOG_ERROR(msg)  LOG4CPLUS_ERROR(CLog::root, msg)           
#define LOG_WARN(msg)   LOG4CPLUS_WARN(CLog::root,msg)
#define LOG_INFO(msg)   LOG4CPLUS_INFO(CLog::root, msg)
#define LOG_DEBUG(msg)  LOG4CPLUS_DEBUG(CLog::root, msg)

class CLog
{
public:
	static void init();

	static Logger root;
private:
	static bool isInit;
	
private:
	CLog(void);
	~CLog(void);
};
