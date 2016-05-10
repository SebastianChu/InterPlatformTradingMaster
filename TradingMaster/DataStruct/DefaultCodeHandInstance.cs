using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace TradingMaster
{
    public class DefaultCodeHandInstance
    {
        public static readonly string ConfigFilePath = "setting/q7/defaultCodeHand.xml";

        private static ObservableCollection<DefaultCodeHand> _DefaultCodeHandList = null;
        private static int _DefaultCodeHandCount = 1;

        /// <summary>
        /// 从配置文件中读取默认手数信息
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<DefaultCodeHand> GetDefaultCodeHandList()
        {
            if (_DefaultCodeHandList == null)
            {
                if (File.Exists(ConfigFilePath))
                {
                    try
                    {
                        //先从配置文件中读取默认手数信息
                        _DefaultCodeHandList = (ObservableCollection<DefaultCodeHand>)(CommonUtil.RecoverObjectFromFile(typeof(ObservableCollection<DefaultCodeHand>), ConfigFilePath));
                    }
                    catch (Exception ex)
                    {
                        Util.Log(ex.ToString());
                    }
                }
                if (_DefaultCodeHandList == null)
                {
                    _DefaultCodeHandList = new ObservableCollection<DefaultCodeHand>();
                }
            }

            return _DefaultCodeHandList;
        }


        public static int GetDefaultCodeHand(string code)
        {
            //如果有默认合约则直接处理合约
            ObservableCollection<DefaultCodeHand> codeHandList = GetDefaultCodeHandList();
            foreach (var item in codeHandList)
            {
                if (code == item.Name)
                {
                    return item.DefaultHand;
                }
            }

            //没有合约则查看是否有品种
            int firstNumberIndiex = code.IndexOfAny("0123456789".ToCharArray());
            string SpeciesName = code;
            if (firstNumberIndiex >= 0)
            {
                SpeciesName = code.Substring(0, firstNumberIndiex);
            }

            //品种必须完全一致才可
            foreach (var item in codeHandList)
            {
                if (SpeciesName == item.Name)
                {
                    return item.DefaultHand;
                }
            }

            return _DefaultCodeHandCount;
        }

        /// <summary>
        /// 重新载入默认手数信息
        /// </summary>
        public static void Reload()
        {
            _DefaultCodeHandList.Clear();
            _DefaultCodeHandList = null;
            GetDefaultCodeHandList();
        }
        /// <summary>
        /// 添加一个默认手数
        /// </summary>
        /// <param name="name"></param>
        public static void Add(string name)
        {
            ObservableCollection<DefaultCodeHand> codeHandList = GetDefaultCodeHandList();
            DefaultCodeHand codeHand = new DefaultCodeHand();
            codeHand.Name = name;
            codeHand.DefaultHand = _DefaultCodeHandCount;
            codeHandList.Add(codeHand);
        }


        public static void RemoveEmptyDefaultCodeHand()
        {
            List<DefaultCodeHand> emptyNameDefaultCodeHand = new List<DefaultCodeHand>();
            ObservableCollection<DefaultCodeHand> codeHandList = GetDefaultCodeHandList();
            foreach (var item in codeHandList)
            {
                if (string.IsNullOrEmpty(item.Name))
                {
                    emptyNameDefaultCodeHand.Add(item);
                }
            }
            foreach (var item in emptyNameDefaultCodeHand)
            {
                codeHandList.Remove(item);
            }
        }

        public static void SaveStyleToFile()
        {
            RemoveEmptyDefaultCodeHand();
            CommonUtil.SaveObjectToFile(_DefaultCodeHandList, ConfigFilePath);
        }
    }

    public class DefaultCodeHand : INotifyPropertyChanged
    {
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
        /// 手数
        /// </summary>
        private int defaultHand;

        public int DefaultHand
        {
            get { return defaultHand; }
            set
            {
                defaultHand = value;
                OnPropertyChanged("DefaultHand");
            }
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