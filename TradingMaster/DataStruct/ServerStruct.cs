using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

namespace TradingMaster
{
    public class ServerSettings
    {
        public List<ServerData> JYserver = new List<ServerData>();
        public List<ServerData> HQserver = new List<ServerData>();
        public ServerData JYServerDetails;
        public ServerData HQServerDetails;

        public Boolean BSim;

        public ServerSettings(Boolean sim)
        {
            BSim = sim;
        }

        public void Clear()
        {
            JYserver.Clear();
            HQserver.Clear();

        }
    }

    public class ServerStruct
    {
        public enum SERVER_TYPE { JY, HQ };
        public static XmlDocument ServerSettingDoc = new XmlDocument();

        ///以下两个变量用于测速
        private static AutoResetEvent _Ars = new AutoResetEvent(false);
        private static Boolean _IsConnectionSuccessful;

        //private static Boolean isTestOver = true;
        //private static Thread speedTestThread = null;
        //private static ServerSettings simServerSet=new ServerSettings(true);
        private static ServerSettings _RealServerSet=new ServerSettings(false);

        public static ServerSettings CurrentServerSet = null;

        //public static List<ServerData> JYserver = new List<ServerData>();
        //public static List<ServerData> HQserver = new List<ServerData>();
        //public static ServerData JYServerDetails;
        //public static ServerData HQServerDetails;

        public static ServerData GetJYServer()
        {
            if (CurrentServerSet == null) return null;
            return CurrentServerSet.JYServerDetails;
        }

        public static ServerData GetHQServer()
        {
            if (CurrentServerSet == null) return null;
            return CurrentServerSet.HQServerDetails;
        }

        //public static Boolean IsSim()
        //{
        //    if (currentServerSet == null) return true;
        //    return currentServerSet == simServerSet;
        //}

        //public static void SetToSim()
        //{
        //    currentServerSet = simServerSet;
        //    SetSimOrRealServer(true);
        //}

        //public static void SetToReal()
        //{
        //    currentServerSet = realServerSet;
        //    SetSimOrRealServer(false);

        //}

        public static bool LoadXml()
        {
            if (Directory.Exists(CommonUtil.GetSettingPath() + "setting") == false)
            {
                Directory.CreateDirectory(CommonUtil.GetSettingPath() + "setting");
            }
            //从Setting目录中读取服务器列表文件
            string settingFile = CommonUtil.GetSettingPath() + "setting/ServerSettings.xml";
            if (File.Exists(settingFile) == false)
            {
                return false;
            }
            //读取XML文件
            ServerSettingDoc.Load(settingFile);
            XmlNode node = ServerSettingDoc.SelectSingleNode("config");
            if (node == null)
            {
                return false;
            }

            //simServerSet.Clear();
            _RealServerSet.Clear();


            ServerSettings tempServerSet = null;

            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.Name.ToLower() != "servers") continue;

                tempServerSet = _RealServerSet;
                CurrentServerSet = tempServerSet;

                foreach (XmlNode serverNode in node2.ChildNodes)
                {
                    if (serverNode.Name.ToLower() != "server")
                    {
                        continue;
                    }
                    foreach (XmlNode temp2 in serverNode.ChildNodes)
                    {
                        if (temp2.Attributes["name"] == null || temp2.Attributes["ip"] == null ||
                            temp2.Attributes["port"] == null || temp2.Attributes["selected"] == null)
                        {
                            Util.Log("LoadXml 格式错误:" + temp2.OuterXml);
                            continue;
                        }
                        ServerData sdata
                                = new ServerData(temp2.Attributes["name"].InnerText, temp2.Attributes["brokerID"].InnerText, temp2.Attributes["ip"].InnerText, temp2.Attributes["port"].InnerText, temp2.Attributes["selected"].InnerText);
                        if (serverNode.Attributes["type"].InnerText == "JY")
                        {
                            tempServerSet.JYserver.Add(sdata);
                            if (sdata.ServerSelected)
                            {
                                ServerData JYSstruct = new ServerData(sdata.ServerName, sdata.BrokerID, sdata.ServerIPadd, sdata.ServerPort);
                                tempServerSet.JYServerDetails = JYSstruct;
                            }
                        }
                        else if (serverNode.Attributes["type"].InnerText == "HQ")
                        {
                            tempServerSet.HQserver.Add(sdata);
                            if (sdata.ServerSelected)
                            {
                                ServerData HQSstruct = new ServerData(sdata.ServerName, sdata.BrokerID, sdata.ServerIPadd, sdata.ServerPort);
                                tempServerSet.HQServerDetails = HQSstruct;
                            }
                        }
                    }
                }

            }

            //if (tempServerSet!=null && (tempServerSet.JYServerDetails == null || tempServerSet.HQServerDetails == null))
            //{
            //    //给交易服务器测速
            //    SpeedTest(true);
            //}

            //LoadHQInfo();
            
            return true;
        }

        /// <summary>
        /// 测速
        /// </summary>
        /// <returns></returns>
        //public static Boolean SpeedTest(Boolean needAutoSelect)
        //{
        //    if (isTestOver == false) return false;
        //    speedTestThread = new Thread((SpeedTestProc));
        //    speedTestThread.IsBackground = true;
        //    speedTestThread.Start(needAutoSelect);
        //    return true;
        //}


        //public static Boolean StopSpeedTest()
        //{
        //    if (speedTestThread == null) return false;
        //    if (speedTestThread.IsAlive == false) return false;
        //    speedTestThread.Abort();
        //    isTestOver = true;
        //    speedTestThread = null;
        //    ServerSettings.g_svrSetting.SetTestButtonEnabled(true);
        //    return true;
        //}

        //public static void SpeedTestProc(object o)
        //{
        //    try
        //    {
        //        isTestOver = false;
        //        ///先清空
        //        foreach (ServerData sd in currentServerSet.JYserver)
        //        {
        //            sd.ServerSpeed = -100;
        //        }

        //        foreach (ServerData sd in currentServerSet.HQserver)
        //        {
        //            sd.ServerSpeed = -100;
        //        }

        //        //先测交易的
        //        int minSpeed = 99999;
        //        ServerData minServerData = null;

        //        foreach (ServerData sd in currentServerSet.JYserver)
        //        {
        //            string ip = sd.ServerIPadd;
        //            int port = 0;
        //            if (int.TryParse(sd.ServerPort, out port) == false)
        //            {
        //                continue;
        //            }
        //            sd.ServerSpeed = TestServerSpeed(ip, port);
        //            if (sd.ServerSpeed != -1)
        //            {
        //                if (minSpeed > sd.ServerSpeed)
        //                {
        //                    minSpeed = sd.ServerSpeed;
        //                    minServerData = sd;
        //                }
        //            }
        //            if ((Boolean)o == true)
        //            {
        //                sd.ServerSelected = false;
        //            }
        //        }
        //        if ((Boolean)o == true)
        //        {
        //            if (minServerData != null)
        //            {
        //                minServerData.ServerSelected = true;
        //                Login.LoginInstace.SetJYServerSelectedItem(minServerData);
        //            }
        //        }


        //        minSpeed = 99999;
        //        minServerData = null;
        //        //再测行情的
        //        foreach (ServerData sd in currentServerSet.HQserver)
        //        {
        //            string ip = sd.ServerIPadd;
        //            int port = 0;
        //            if (int.TryParse(sd.ServerPort, out port) == false)
        //            {
        //                continue;
        //            }
        //            sd.ServerSpeed = TestServerSpeed(ip, port);
        //            if (sd.ServerSpeed != -1)
        //            {
        //                if (minSpeed > sd.ServerSpeed)
        //                {
        //                    minSpeed = sd.ServerSpeed;
        //                    minServerData = sd;
        //                }
        //            }

        //            if ((Boolean)o == true)
        //            {
        //                sd.ServerSelected = false;
        //            }
        //        }
        //        if ((Boolean)o == true)
        //        {
        //            if (minServerData != null)
        //            {
        //                minServerData.ServerSelected = true;
        //                Login.LoginInstace.SetHQServerSelectedItem(minServerData);
        //            }

        //            //将更改后的结果存到XML
        //            ChangeSelectedServer();
        //        }
              
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    isTestOver = true;

        //    if (ServerSettings.g_svrSetting != null)
        //    {
        //        ServerSettings.g_svrSetting.SetTestButtonEnabled(true);
        //    }
            
        //}

        /// <summary>
        /// 测试服务器速度,－1表示无法连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public static int TestServerSpeed(string ip, int port)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            long lastTick = DateTime.Now.Ticks;
            _IsConnectionSuccessful = false;
            _Ars.Reset();
            try
            {
                s.BeginConnect(IPAddress.Parse(ip), port, new AsyncCallback(CallBackMethod), s);
                if (_Ars.WaitOne(1000, false))
                {
                    if (_IsConnectionSuccessful == true)
                    {
                        long ret = (DateTime.Now.Ticks - lastTick)/10000;
                        return (int)ret;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log(ex.Message);
            }
            return -1;

        }

        private static void CallBackMethod(IAsyncResult asyncResult)
        {
            try
            {
                Socket s = asyncResult.AsyncState as Socket;
                if (s != null)
                {
                    s.Close();
                    _IsConnectionSuccessful = true;
                    _Ars.Set();
                }
            }
            catch (Exception ex)
            {
                Util.Log(ex.Message);
            }
        }

        public static bool SaveXml()
        {
            try
            {
                if (Directory.Exists(CommonUtil.GetSettingPath() + "setting") == false)
                {
                    Directory.CreateDirectory(CommonUtil.GetSettingPath() + "setting");
                }
                string settingFile = CommonUtil.GetSettingPath() + "setting/ServerSettings.xml";
                ServerSettingDoc.Save(settingFile);
            }
            catch (System.Exception ex)
            {
                Util.Log(ex.Message);
                return false;
            }

            return true;
        }

        //public static bool AddServerElement(SERVER_TYPE serverType, string serverName, string ip, string port)
        //{
        //    ServerData sdata = new ServerData(serverName, ip, port);
        //    if (serverType == SERVER_TYPE.JY)
        //    {
        //        foreach (ServerData serverData in currentServerSet.JYserver)
        //        {
        //            if (serverData.ServerName.Equals(sdata.ServerName))
        //            {
        //                return false;
        //            }
        //        }
        //        currentServerSet.JYserver.Add(sdata);
        //        ServerSettingDoc.GetElementsByTagName("server")[0].AppendChild(sdata.toXmlElement());
        //    }
        //    else if (serverType == SERVER_TYPE.HQ)
        //    {
        //        foreach (ServerData serverData in currentServerSet.HQserver)
        //        {
        //            if (serverData.ServerName.Equals(sdata.ServerName))
        //            {
        //                return false;
        //            }
        //        }
        //        currentServerSet.HQserver.Add(sdata);
        //        ServerSettingDoc.GetElementsByTagName("server")[1].AppendChild(sdata.toXmlElement());
        //    }
        //    //对sdata进行测速
        //    return true;
        //}

        //public static bool ContainsServer(SERVER_TYPE serverType, string serverName)
        //{
        //    if (serverType == SERVER_TYPE.JY)
        //    {
        //        foreach (ServerData serverData in currentServerSet.JYserver)
        //        {
        //            if (serverData.ServerName.Equals(serverName))
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    else if (serverType == SERVER_TYPE.HQ)
        //    {
        //        foreach (ServerData serverData in currentServerSet.HQserver)
        //        {
        //            if (serverData.ServerName.Equals(serverName))
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        //public static bool RemoveServerElement(SERVER_TYPE serverType, string serverName)
        //{
        //    if (serverType == SERVER_TYPE.JY)
        //    {
        //        foreach (ServerData serverData in currentServerSet.JYserver)
        //        {
        //            if (serverData.ServerName.Equals(serverName))
        //            {
        //                currentServerSet.JYserver.Remove(serverData);

        //                XmlNode serverNode=ServerSettingDoc.GetElementsByTagName("server")[0];
        //                foreach(XmlNode hostNode in serverNode.ChildNodes)
        //                {
        //                    if (serverName.Equals(hostNode.Attributes["name"].Value))
        //                    {
        //                        serverNode.RemoveChild(hostNode);
        //                        break;
        //                    }
        //                }
        //                return true;
        //            }
        //        }
        //    }
        //    else if (serverType == SERVER_TYPE.HQ)
        //    {
        //        foreach (ServerData serverData in currentServerSet.HQserver)
        //        {
        //            if (serverData.ServerName.Equals(serverName))
        //            {
        //                currentServerSet.HQserver.Remove(serverData);
        //                XmlNode serverNode = ServerSettingDoc.GetElementsByTagName("server")[1];
        //                foreach (XmlNode hostNode in serverNode.ChildNodes)
        //                {
        //                    if (serverName.Equals(hostNode.Attributes["name"].Value))
        //                    {
        //                        serverNode.RemoveChild(hostNode);
        //                        break;
        //                    }
        //                }
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        /// <summary>
        /// 更改选择的服务器
        /// </summary>
        /// <returns></returns>
        public static bool ChangeSelectedServer()
        {
            foreach (ServerData serverData in CurrentServerSet.JYserver)
            {
                XmlNode serverNode = ServerSettingDoc.GetElementsByTagName("server")[0];
                foreach (XmlNode hostNode in serverNode.ChildNodes)
                {
                    //if (serverName.Equals(hostNode.Attributes["name"].Value))
                    if (hostNode.Attributes["name"].Value.ToString() == serverData.ServerName)
                    {
                        hostNode.Attributes["selected"].Value = serverData.ServerSelected.ToString();
                        break;
                    }
                }
            }

            foreach (ServerData serverData in CurrentServerSet.HQserver)
            {
                XmlNode serverNode = ServerSettingDoc.GetElementsByTagName("server")[1];
                foreach (XmlNode hostNode in serverNode.ChildNodes)
                {
                    if (hostNode.Attributes["name"].Value.ToString() == serverData.ServerName)
                    {
                        hostNode.Attributes["selected"].Value = serverData.ServerSelected.ToString();
                        break;
                    }
                }
            }
            SaveXml();
            return true;
        }

        public static bool ModifyServerElement(SERVER_TYPE serverType, string original_name, string serverName, string ip, string port)
        {
            if (serverType == SERVER_TYPE.JY)
            {
                foreach (ServerData serverData in CurrentServerSet.JYserver)
                {
                    if (serverData.ServerName.Equals(original_name))
                    {
                        serverData.ServerName = serverName;
                        serverData.ServerIPadd = ip;
                        serverData.ServerPort = port;

                        XmlNode serverNode=ServerSettingDoc.GetElementsByTagName("server")[0];
                        foreach(XmlNode hostNode in serverNode.ChildNodes)
                        {
                            //if (serverName.Equals(hostNode.Attributes["name"].Value))
                            if (hostNode.Attributes["name"].Value.ToString() == original_name)
                            {
                                hostNode.Attributes["name"].Value = serverName;
                                hostNode.Attributes["ip"].Value = ip;
                                hostNode.Attributes["port"].Value = port;
                                break;
                            }
                        }
                        return true;
                    }
                }
            }
            else if (serverType == SERVER_TYPE.HQ)
            {
                foreach (ServerData serverData in CurrentServerSet.HQserver)
                {
                    if (serverData.ServerName.Equals(original_name))
                    {
                        serverData.ServerName = serverName;
                        serverData.ServerIPadd = ip;
                        serverData.ServerPort = port;

                        XmlNode serverNode = ServerSettingDoc.GetElementsByTagName("server")[1];
                        foreach (XmlNode hostNode in serverNode.ChildNodes)
                        {
                            if (hostNode.Attributes["name"].Value.ToString() == original_name)
                            {
                                hostNode.Attributes["name"].Value = serverName;
                                hostNode.Attributes["ip"].Value = ip;
                                hostNode.Attributes["port"].Value = port;
                                break;
                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 设置实盘或者模拟服务器
        /// </summary>
        /// <param name="isSim"></param>
        //private static void SetSimOrRealServer(Boolean aIsSimServer)
        //{
        //    XmlNode serverNode = ServerSettingDoc.GetElementsByTagName("config")[0];
        //    foreach (XmlNode hostNode in serverNode.ChildNodes)
        //    {
        //        if (hostNode.Attributes["type"]!=null && hostNode.Attributes["type"].Value.ToString().ToLower() == "real")
        //        {
        //            if (hostNode.Attributes["selected"] != null)
        //            {
        //                hostNode.Attributes["selected"].Value = (!aIsSimServer).ToString();
        //            }

        //        }
        //        else if (hostNode.Attributes["type"] != null && hostNode.Attributes["type"].Value.ToString().ToLower() == "sim")
        //        {
        //            if (hostNode.Attributes["selected"] != null)
        //            {
        //                hostNode.Attributes["selected"].Value = (aIsSimServer).ToString();
        //            }

        //        }
        //    }
        //    //SaveXml();
        //}       

        private static XmlNode GetServerNodeByAttribute(XmlDocument xmlDoc, Boolean isJy)
        {
            if (xmlDoc == null) return null;
            foreach (XmlNode nnode in xmlDoc.GetElementsByTagName("servers"))
            {
                //if ((nnode.Attributes["type"].Value.ToString().ToLower() == "real" && isReal) ||
                //    (nnode.Attributes["type"].Value.ToString().ToLower() == "sim" && !isReal))
                //{
                    foreach (XmlNode nnode2 in nnode.ChildNodes)
                    {
                        if ((nnode2.Attributes["type"].Value.ToString().ToLower() == "jy" && isJy) ||
                            (nnode2.Attributes["type"].Value.ToString().ToLower() == "hq" && !isJy))
                        {
                            return nnode2;
                        }
                    }
                //}
            }
            return null;
        }

        public static bool Update_Selected(SERVER_TYPE serverType, string sname, bool selected, bool isClearAll = false)
        {
            if (serverType == SERVER_TYPE.JY)
            {
                if (isClearAll == true)
                {
                    foreach (ServerData serverData in CurrentServerSet.JYserver)
                    {
                        serverData.ServerSelected = !selected;
                    }
                }
                foreach (ServerData serverData in CurrentServerSet.JYserver)
                {
                    if (serverData.ServerName.Equals(sname))
                    {
                        serverData.ServerSelected = selected;
                        if (selected)
                        {
                            ServerData JYSstruct = new ServerData(serverData.ServerName, serverData.BrokerID, serverData.ServerIPadd, serverData.ServerPort);
                            CurrentServerSet.JYServerDetails = JYSstruct;
                        }
                        XmlNode serverNode = GetServerNodeByAttribute(ServerSettingDoc, true);
                        if (serverNode == null) continue;

                        foreach (XmlNode hostNode in serverNode.ChildNodes)
                        {
                            if (hostNode.Attributes["name"].Value.ToString() == sname)
                            {
                                //Util.Log("Change JY Selected Server:" + hostNode.Attributes["name"].Value + " selected=" + selected+" sname="+sname);
                                hostNode.Attributes["selected"].Value = selected.ToString();
                            }
                            else if (selected == true)
                            {
                                hostNode.Attributes["selected"].Value = (!selected).ToString();
                            }
                        }
                        //if (selected == true)
                        //{
                        //    SetConnectServerForFile();
                        //}
                        return true;
                    }
                }
            }
            else if (serverType == SERVER_TYPE.HQ)
            {
                if (isClearAll == true)
                {
                    foreach (ServerData serverData in CurrentServerSet.HQserver)
                    {
                        serverData.ServerSelected = !selected;
                    }
                }

                foreach (ServerData serverData in CurrentServerSet.HQserver)
                {
                    if (serverData.ServerName.Equals(sname))
                    {
                        serverData.ServerSelected = selected;
                        if (selected)
                        {
                            ServerData HQSstruct = new ServerData(serverData.ServerName, serverData.BrokerID, serverData.ServerIPadd, serverData.ServerPort);
                            CurrentServerSet.HQServerDetails = HQSstruct;
                        }
                        XmlNode serverNode = GetServerNodeByAttribute(ServerSettingDoc, false);
                        if (serverNode == null) continue;


                        foreach (XmlNode hostNode in serverNode.ChildNodes)
                        {
                            if (hostNode.Attributes["name"].Value.ToString() == sname)
                            {
                                //Util.Log("Change HQ Selected Server:" + hostNode.Attributes["name"].Value + " selected=" + selected + " sname=" + sname);
                                hostNode.Attributes["selected"].Value = selected.ToString();
                            }
                            else if (selected == true)
                            {
                                hostNode.Attributes["selected"].Value = (!selected).ToString();
                            }
                        }
                        //if (selected == true)
                        //{
                        //    SetConnectServerForFile();
                        //}
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public class ServerData : INotifyPropertyChanged
    {
        public ServerData(string name, string broker, string ipadd, string port)
        {
            ServerName = name;
            BrokerID = broker;
            ServerIPadd = ipadd;
            ServerPort = port;
            ServerSpeed = -100;
        }

        public ServerData(string name, string broker, string ipadd, string port,string selected)
        {
            ServerName = name;
            BrokerID = broker;
            ServerIPadd = ipadd;
            ServerPort = port;
            
            if (selected.ToLower().Equals("true"))
            {
                serverselected = true;
            }
            else
            {
                serverselected = false;
            }

            ServerSpeed = -100;
        }

        private string brokerID;
        public string BrokerID
        {
            get { return brokerID; }
            set { brokerID = value; OnPropertyChanged("BrokerID"); }
        }

        private string servername;
        public string ServerName
        {
            get { return servername; }
            set { servername = value; OnPropertyChanged("ServerName"); }
        }

        private string serveripadd;
        public string ServerIPadd
        {
            get { return serveripadd; }
            set { serveripadd = value; OnPropertyChanged("ServerIPadd"); }
        }

        private string serverport;
        public string ServerPort
        {
            get { return serverport; }
            set { serverport = value; OnPropertyChanged("ServerPort"); }
        }

        private bool serverselected;
        public bool ServerSelected
        {
            get { return serverselected; }
            set { serverselected = value; OnPropertyChanged("ServerSelected"); }
        }

        /// <summary>
        /// 服务器速度
        /// </summary>
        private int serverSpeed;
        public int ServerSpeed
        {
            get { return serverSpeed; }
            set { serverSpeed = value; OnPropertyChanged("ServerSpeed"); }
        }
    
        /// <summary>
        /// 是否是模拟的
        /// </summary>
        private Boolean isSim;
        public Boolean IsSim
        {
            get { return isSim; }
            set { isSim = value; OnPropertyChanged("IsSim"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        public XmlElement toXmlElement()
        {
            XmlElement element=ServerStruct.ServerSettingDoc.CreateElement("host");
            element.SetAttribute("name", servername);
            element.SetAttribute("ip", serveripadd);
            element.SetAttribute("port", serverport);
            element.SetAttribute("selected", serverselected.ToString());
            return element;
        }
    }
}
