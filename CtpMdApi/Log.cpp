#include "StdAfx.h"
#include ".\log.h"

#include <log4cplus/configurator.h>

bool CLog::isInit=false;
Logger CLog::root=Logger::getRoot();

CLog::CLog(void)
{
}

CLog::~CLog(void)
{
}

void CLog::init()
{
	if (!isInit)
	{
#ifdef _DEBUG
		AllocConsole();
		freopen("CON", "w", stdout);
#endif
		PropertyConfigurator::doConfigure(LOG4CPLUS_TEXT("log4cplus.properties"));
		
		isInit=true;
	}
}