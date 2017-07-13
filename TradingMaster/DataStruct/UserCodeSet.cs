using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using TradingMaster.CodeSet;

namespace TradingMaster
{
    public class UserCodeSetInstance
    {
        public static int USER_CODE_SET_MAX_CODES = 20;
        public static string LASTCODEINFODIR = CommonUtil.GetSettingPath() + @"setting\";
        public static string LASTCODEINFOPATH = "CodeInfo.dat";
        public static string LASTGROUPCODEINFOPATH = "GroupCodeInfo.dat";
        public static readonly string ConfigFilePath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\setting\\userCodeSet.xml";

        private static ObservableCollection<UserCodeSet> _UserCodeSetList = null;
        //private static ObservableCollection<UserCodeSet> userCodeSetListForEdit = null;
        private static Dictionary<string, ObservableCollection<Contract>> _DicUserCodeSetCodeInfo = new Dictionary<string, ObservableCollection<Contract>>();

        private static Boolean _IsZlRequested = false;   //主力合约是否已经请求过了

        /// <summary>
        /// 获取合约组对应的合约
        /// </summary>
        /// <param name="userCodeSet"></param>
        /// <returns></returns>
        public static ObservableCollection<Contract> GetContractListByUserCodeSet(string userCodeSetId)
        {
            if (_UserCodeSetList == null)
            {
                GetUserCodeSetList();
            }
            return _DicUserCodeSetCodeInfo[userCodeSetId];
        }

        /// <summary>
        /// 从配置文件中读取合约组信息
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<UserCodeSet> GetUserCodeSetList()
        {
            if (_UserCodeSetList == null)
            {
                if (File.Exists(ConfigFilePath))
                {
                    try
                    {
                        //先从配置文件中读取合约组信息
                        _UserCodeSetList = (ObservableCollection<UserCodeSet>)(CommonUtil.RecoverObjectFromFile(typeof(ObservableCollection<UserCodeSet>), ConfigFilePath));
                    }
                    catch (System.Exception ex)
                    {
                        Util.Log(ex.ToString());
                    }
                }
                if (_UserCodeSetList == null)
                {
                    _UserCodeSetList = new ObservableCollection<UserCodeSet>();
                    UserCodeSet defaultSet = new UserCodeSet();
                    defaultSet.Name = "普通行情";
                    defaultSet.Id = Guid.NewGuid().ToString();
                    defaultSet.IsDefault = true;
                    defaultSet.FileName = LASTCODEINFOPATH;
                    defaultSet.IsArbitrage = false;
                    _UserCodeSetList.Add(defaultSet);

                    UserCodeSet GroupSet = new UserCodeSet();
                    GroupSet.Name = "组合行情";
                    GroupSet.Id = Guid.NewGuid().ToString();
                    GroupSet.IsDefault = true;
                    GroupSet.FileName = LASTGROUPCODEINFOPATH;
                    GroupSet.IsArbitrage = true;
                    _UserCodeSetList.Add(GroupSet);
                }

                //再读取各合约组的合约情况
                foreach (var item in _UserCodeSetList)
                {
                    if (File.Exists(LASTCODEINFODIR + item.FileName))
                    {
                        ObservableCollection<Contract> userCodes = new ObservableCollection<Contract>();
                        try
                        {
                            FileStream fs2 = new FileStream(LASTCODEINFODIR + item.FileName, FileMode.OpenOrCreate);
                            BinaryFormatter formatter = new BinaryFormatter();
                            ObservableCollection<Contract> temp = (ObservableCollection<Contract>)formatter.Deserialize(fs2);
                            foreach (Contract codeInfo in temp)
                            {
                                if (codeInfo != null && !CodeSetManager.IsOutOfDate(codeInfo.Code))
                                {
                                    userCodes.Add(codeInfo);
                                }
                            }
                            fs2.Close();
                        }
                        catch (System.Exception ex)
                        {
                            Util.Log_Error("exception: " + ex.Message);
                            Util.Log_Error("exception: " + ex.StackTrace);
                        }
                        //要对userCodes进行处理，保证无重复的内容。
                        Dictionary<Contract, Boolean> codeDict = new Dictionary<Contract, bool>();
                        for (int i = 0; i < userCodes.Count; i++)
                        {
                            if (userCodes[i] != null)
                            {
                                if (codeDict.ContainsKey(userCodes[i]))
                                {
                                    userCodes.RemoveAt(i);
                                    i -= 1;
                                }
                                else
                                {
                                    codeDict.Add(userCodes[i], true);
                                }
                            }
                        }
                        while (userCodes.Count > USER_CODE_SET_MAX_CODES && item.IsZhuli == false)
                        {
                            userCodes.RemoveAt(userCodes.Count - 1);
                        }
                        if (item.IsZhuli)
                        {
                            Util.Log("添加UserCodes到主力合约: userCodes的个数为" + userCodes.Count.ToString());
                            foreach (Contract codeInfo in userCodes)
                            {
                                if (codeInfo != null)
                                {
                                    //Util.Log("--CodeInfo为:" + codeInfo.ToString());
                                }
                                else
                                {
                                    //Util.Log("--CodeInfo为null");
                                }
                            }
                        }
                        _DicUserCodeSetCodeInfo.Add(item.Id, userCodes);
                    }
                    else
                    {
                        _DicUserCodeSetCodeInfo.Add(item.Id, new ObservableCollection<Contract>());
                    }
                }
            }

            //AddZhuliCodeTemp(userCodeSetList);

            return _UserCodeSetList;
        }

        /// <summary>
        /// 添加主力合约板块，代码暂不添加
        /// </summary>
        private static void AddZhuliCodeTemp(ObservableCollection<UserCodeSet> userCodeSets)
        {
            if (_IsZlRequested == true) return;
            //如果已经存在主力合约板块了，则返回
            string guid = System.Guid.NewGuid().ToString();
            UserCodeSet userCodeSet = GetZhuliCodeUserCodeSet();
            if (userCodeSet != null)
            {
                guid = userCodeSet.Id;
            }
            else
            {
                userCodeSet = new UserCodeSet();
                userCodeSet.IsArbitrage = false;    //非套利合约组
                userCodeSet.IsDefault = true;       //是默认的，不可被删除
                userCodeSet.IsZhuli = true;         //是主力合约

                userCodeSet.Id = guid;
                userCodeSet.Name = "主力合约";
                userCodeSet.FileName = "ZhuliCode.dat";

                ObservableCollection<Contract> codeInfoList = new ObservableCollection<Contract>();
                _DicUserCodeSetCodeInfo.Add(guid, codeInfoList);
                //SaveUserCode(userCodeSet);
                _UserCodeSetList.Insert(2, userCodeSet);
                //此时应该将userCodeSetList写回到文件中
                CommonUtil.SaveObjectToFile(_UserCodeSetList, ConfigFilePath);
            }
            //开一个线程请求主力合约
            Thread t = new Thread(ZhuliCodeThread);
            t.IsBackground = true;
            t.Start(guid);
        }

        private static void ZhuliCodeThread(object o)
        {
            //if (isZlRequested == true) return;
            //List<string> zhuliCode = new List<string>();
            //string guid = o as String;
            //try
            //{
            //    isZlRequested = true;
            //    zhuliCode = HQService.HQServ.GetZhuliCode();
            //    Util.Log("主力合约的guid为:" + guid);
            //    Util.Log("从WEB SERVICE得到主力合约的数量为:" + zhuliCode.Count.ToString());
            //    for (int i = 0; i < zhuliCode.Count; i++)
            //    {
            //        if (zhuliCode[i] != null)
            //        {
            //            Util.Log("主力Code:" + zhuliCode[i]);
            //        }
            //        else
            //        {
            //            Util.Log("主力Code:空");
            //        }
            //    }

            //    ObservableCollection<Contract> codeInfos = null;
            //    codeInfos = dicUserCodeSetCodeInfo[guid];

            //    Util.Log("从文件中存储的主力合约的数量为:" + codeInfos.Count.ToString());
            //    for (int i = 0; i < codeInfos.Count; i++)
            //    {
            //        if (codeInfos[i] != null)
            //        {
            //            Util.Log("Code:" + codeInfos[i].Code);
            //        }
            //        else
            //        {
            //            Util.Log("Code:空");
            //        }
            //    }


            //    //得到品种的顺序
            //    List<String> speciesSequence = new List<string>();      //品种顺序
            //    string date = "";
            //    ///将接收到的zhuliCode中的内容合并到codeInfos中去
            //    Dictionary<string, List<Contract>> codeDict = new Dictionary<string, List<Contract>>();
            //    //依次处理species
            //    foreach (string code in zhuliCode)
            //    {
            //        Contract codeInfo = CodeSetManager.GetContractInfo(code);
            //        string species = CodeSetManager.GetSpeciesName(code, out date);
            //        if (codeDict.ContainsKey(species))
            //        {
            //            codeDict[species].Add(codeInfo);
            //        }
            //        else
            //        {
            //            List<Contract> codeInfoList = new List<Contract>();
            //            codeInfoList.Add(codeInfo);
            //            codeDict[species] = codeInfoList;
            //        }
            //    }

            //    ObservableCollection<Contract> codeInfosNew = new ObservableCollection<Contract>();
            //    foreach (Contract code in codeInfos)
            //    {
            //        string species = CodeSetManager.GetSpeciesName(code.Code, out date);
            //        if (codeDict.ContainsKey(species) == true)
            //        {
            //            List<Contract> codeInfoList = codeDict[species];
            //            foreach (Contract ccc in codeInfoList)
            //            {
            //                codeInfosNew.Add(ccc);
            //            }
            //            codeDict.Remove(species);
            //        }
            //    }
            //    foreach (string key in codeDict.Keys)
            //    {
            //        List<Contract> codeInfoList = codeDict[key];
            //        foreach (Contract ccc in codeInfoList)
            //        {
            //            codeInfosNew.Add(ccc);
            //        }
            //    }

            //    codeInfos.Clear();
            //    Util.Log("codeInfosNew个数为:" + codeInfosNew.Count.ToString());
            //    foreach (Contract code in codeInfosNew)
            //    {
            //        if (code != null)
            //        {
            //            Util.Log("添加Code:" + code.ToString());
            //        }
            //        else
            //        {
            //            Util.Log("添加Code:null");
            //        }
            //        codeInfos.Add(code);
            //    }

            //    foreach (UserCodeSet usc in userCodeSetList)
            //    {
            //        if (usc.Id == guid)
            //        {
            //            SaveUserCode(usc);
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Util.Log("获取主力合约错误:" + ex.Message);
            //}
        }

        /// <summary>
        /// 得到主力合约的UserCodeSet
        /// </summary>
        /// <returns></returns>
        public static UserCodeSet GetZhuliCodeUserCodeSet()
        {
            foreach (UserCodeSet item in _UserCodeSetList)
            {
                if (item.IsZhuli == true)
                {
                    return item;
                }
            }
            return null;
        }

        private static void SaveUserCode(UserCodeSet item)
        {
            FileStream fs = new FileStream(LASTCODEINFODIR + item.FileName, FileMode.OpenOrCreate);
            BinaryFormatter formatter = new BinaryFormatter();
            if (item.IsZhuli)
            {
                Util.Log("序列化主力合约到文件" + item.FileName);
            }
            formatter.Serialize(fs, _DicUserCodeSetCodeInfo[item.Id]);
            fs.Close();
        }


        /// <summary>
        /// 重新载入合约组信息
        /// </summary>
        public static void Reload()
        {
            _UserCodeSetList.Clear();
            _UserCodeSetList = null;
            _DicUserCodeSetCodeInfo.Clear();
            GetUserCodeSetList();
        }
        /// <summary>
        /// 添加一个合约组
        /// </summary>
        /// <param name="name"></param>
        public static void Add(string name)
        {
            ObservableCollection<UserCodeSet> userCodeSetList = GetUserCodeSetList();
            UserCodeSet userCodeSet = new UserCodeSet();
            userCodeSet.Name = name;
            userCodeSet.Id = Guid.NewGuid().ToString();
            userCodeSet.IsDefault = false;
            userCodeSet.FileName = Guid.NewGuid() + ".dat";
            userCodeSet.IsArbitrage = false;
            userCodeSetList.Add(userCodeSet);
            _DicUserCodeSetCodeInfo.Add(userCodeSet.Id, new ObservableCollection<Contract>());
        }


        public static void RemoveEmptyUserCodeSet()
        {
            List<UserCodeSet> emptyNameUserCodeSet = new List<UserCodeSet>();
            ObservableCollection<UserCodeSet> userCodeSetList = GetUserCodeSetList();
            foreach (var item in userCodeSetList)
            {
                if (string.IsNullOrEmpty(item.Name))
                {
                    emptyNameUserCodeSet.Add(item);
                }
            }
            foreach (var item in emptyNameUserCodeSet)
            {
                userCodeSetList.Remove(item);
                _DicUserCodeSetCodeInfo.Remove(item.Id);
            }
        }

        public static void SaveStyleToFile()
        {
            RemoveEmptyUserCodeSet();
            CommonUtil.SaveObjectToFile(_UserCodeSetList, ConfigFilePath);
            SaveUserCodes();
        }

        private static void SaveUserCodes()
        {
            foreach (var item in _UserCodeSetList)
            {
                try
                {
                    SaveUserCode(item);
                }
                catch (Exception ex)
                {
                    Util.Log(ex.ToString());
                }
            }
        }


    }

    public class UserCodeSet : INotifyPropertyChanged
    {

        /// <summary>
        /// 标示
        /// </summary>
        private string id;

        public string Id
        {
            get { return id; }
            set
            {
                id = value.Trim();
                OnPropertyChanged("Id");
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value.Trim();
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// 是否默认
        /// </summary>
        private bool isDefault;

        public bool IsDefault
        {
            get { return isDefault; }
            set
            {
                isDefault = value;
                OnPropertyChanged("IsDefault");
            }
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        /// <summary>
        /// 是否组合套利
        /// </summary>
        private bool isArbitrage;

        public bool IsArbitrage
        {
            get { return isArbitrage; }
            set
            {
                isArbitrage = value;
                OnPropertyChanged("IsArbitrage");
            }
        }

        private bool isZhuli;

        public bool IsZhuli
        {
            get { return isZhuli; }
            set { isZhuli = value; OnPropertyChanged("IsZhuli"); }
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

    }
}
