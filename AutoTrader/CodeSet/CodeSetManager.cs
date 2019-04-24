using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace AutoTrader
{
    public class CodeSetManager
    {
        public static string OPTION_SETTINGS_FILE = System.AppDomain.CurrentDomain.BaseDirectory + "/setting/Products.xml";

        private static XmlDocument _ProductDoc = null;

        public static List<Contract> ContractList = new List<Contract>();

        /// <summary>
        /// 某个品种，某个操作的最大操作手数
        /// </summary>
        public static Dictionary<Contract, int> MaxOperationHandCountDict = new Dictionary<Contract, int>();
        //public static Dictionary<Contract, Dictionary<PosEffect, int>> MaxOperationHandCountDict = new Dictionary<Contract, Dictionary<PosEffect, int>>();

        /// <summary>
        /// 所有合约信息，关键字：代码 + 交易所
        /// </summary>
        /// /// <returns></returns>
        private static Dictionary<string, Contract> _ContractMap = new Dictionary<string, Contract>();
        public static Dictionary<string, Contract> ContractMap
        {
            get { return _ContractMap; }
            set { _ContractMap = value; }
        }

        private static List<string> _LstOptionCodes = new List<string>();
        public static List<string> LstOptionCodes
        {
            get { return CodeSetManager._LstOptionCodes; }
            set { CodeSetManager._LstOptionCodes = value; }
        }

        public static Contract GetContractInfo(string code)
        {
            foreach (string contractKey in _ContractMap.Keys)
            {
                if (contractKey.StartsWith(code + "_"))
                {
                    return _ContractMap[contractKey];
                }
            }
            return null;
        }

        public static Contract GetContractInfo(string code, string exchangeID)
        {
            string tempKey = code + "_" + exchangeID;
            if (String.IsNullOrWhiteSpace(exchangeID) || exchangeID.Contains("未知"))
            {
                return GetContractInfo(code);
            }
            if (_ContractMap.ContainsKey(tempKey))
            {
                return _ContractMap[tempKey];
            }
            else
            {
                return null;
            }
        }

        //public static bool IsCloseTodaySupport(string code)
        //{
        //    Contract cInfo = null;
        //    foreach (Contract item in ContractList)
        //    {
        //        if (item.Code == code)
        //        {
        //            cInfo = item;
        //            break;
        //        }
        //    }
        //    if (cInfo == null)
        //        return false;

        //    if (cInfo.ExchCode.Contains("XSGE") || cInfo.ExchCode.Contains("SHFE"))
        //    {
        //        return true;
        //    }
        //    else
        //        return false;
        //}

        public static bool IsCloseTodaySupport(string code)
        {
            foreach (string tempKey in ContractMap.Keys)
            {
                if (tempKey == code + "_SHFE" || tempKey == code + "_XSGE")
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsCffexCloseRule(string code)
        {
            foreach (string tempKey in ContractMap.Keys)
            {
                if (tempKey == code + "_CFFEX" || tempKey == code + "_CFFX")
                {
                    return true;
                }
            }
            return false;
        }

        public static string CtpToIsoCode(string ctpName)
        {
            if (ctpName == "SHFE")
                return "XSGE";
            else if (ctpName == "CZCE")
                return "XZCE";
            else if (ctpName == "DCE")
                return "XDCE";
            else if (ctpName == "CFFEX")
                return "CCFX";
            // Securities
            else if (ctpName == "SSE")
                return "XSHG";
            else if (ctpName == "SZE")
                return "XSHE";
            else
            {
                Util.WriteInfo("Warning! Unknown ctpName：" + ctpName);
                return "未知";
            }
        }

        public static string IsoCodeToCtp(string isoCode)
        {
            if (isoCode == "XSGE")
                return "SHFE";
            else if (isoCode == "XZCE")
                return "CZCE";
            else if (isoCode == "XDCE")
                return "DCE";
            else if (isoCode == "CCFX")
                return "CFFEX";
            // Securities
            else if (isoCode == "XSHG")
                return "SSE";
            else if (isoCode == "XSHE")
                return "SZE";
            else
            {
                Util.WriteInfo("Warning! Unknown IsoCode：" + isoCode);
                return "未知";
            }
        }

        public static string CtpToExName(string ctpName)
        {
            if (ctpName == "SHFE")
                return "上期所";
            else if (ctpName == "CZCE")
                return "郑商所";
            else if (ctpName == "DCE")
                return "大商所";
            else if (ctpName == "CFFEX")
                return "中金所";
            else if (ctpName == "INE")
                return "上期能源";
            // Securities
            else if (ctpName == "SSE")
                return "上证所";
            else if (ctpName == "SZE")
                return "深交所";
            else
            {
                Util.WriteInfo("Warning! Unknown ctpName：" + ctpName);
                return "未知";
            }
        }

        public static string FemasToExName(string ctpName)
        {
            if (ctpName == "SHFE")
                return "上期所";
            else if (ctpName == "CZCE")
                return "郑商所";
            else if (ctpName == "DCE")
                return "大商所";
            else if (ctpName == "CFFEX")
                return "中金所";
            // Securities
            else if (ctpName == "SSE")
                return "上证所";
            else if (ctpName == "SZE")
                return "深交所";
            else
            {
                Util.WriteInfo("Warning! Unknown ctpName：" + ctpName);
                return "未知";
            }
        }


        public static string ExNameToCtp(string exName)
        {
            if (exName == "上期所")
                return "SHFE";
            else if (exName == "郑商所")
                return "CZCE";
            else if (exName == "大商所")
                return "DCE";
            else if (exName == "中金所")
                return "CFFEX";
            // Securities
            else if (exName == "上证所")
                return "SSE";
            else if (exName == "深交所")
                return "SZE";
            else if (exName == "上期能源")
                return "INE";
            else
            {
                Util.WriteInfo("Warning! Unknown Exchange Name：" + exName);
                return "未知";
            }
        }

        public static string ExNameToFemas(string exName)
        {
            if (exName == "上期所")
                return "SHFE";
            else if (exName == "郑商所")
                return "CZCE";
            else if (exName == "大商所")
                return "DCE";
            else if (exName == "中金所")
                return "CFFEX";
            // Securities
            else if (exName == "上证所")
                return "SSE";
            else if (exName == "深交所")
                return "SZE";
            else
            {
                Util.WriteInfo("Warning! Unknown Exchange Name：" + exName);
                return "未知";
            }
        }

        public static List<Contract> GetMarketCodeList(string marketCode)
        {
            List<Contract> marketContractList = new List<Contract>();
            foreach (string tempKey in ContractMap.Keys)
            {
                if (tempKey.EndsWith(marketCode))
                {
                    marketContractList.Add(ContractMap[tempKey]);
                }
            }
            marketContractList.Sort(Contract.CompareByCode);
            return marketContractList;
        }

        public static string GetOptionUnderlyingCode(Contract option, string exchange)
        {
            string underlyingCode = "";
            if (exchange == "CFFEX")
            {
                if (_ProductDoc == null)
                {
                    _ProductDoc = new XmlDocument();
                    _ProductDoc.Load(CodeSetManager.OPTION_SETTINGS_FILE);
                }
                XmlNodeList nodeLst = _ProductDoc.SelectNodes("markets/market/product");
                foreach (XmlNode node in nodeLst)
                {
                    if (option.Code.Contains(node.Attributes["code"].Value.ToString()) && node.Attributes["RelatedCode"] != null)
                    {
                        underlyingCode = option.BaseCode.Replace(node.Attributes["code"].Value.ToString(), node.Attributes["RelatedCode"].Value.ToString());
                        break;
                    }
                }
                //if (underlyingCode.StartsWith("IO"))
                //{
                //    underlyingCode = option.BaseCode.Replace("IO","IF");
                //}
                //else if (underlyingCode.StartsWith("HO"))
                //{
                //    underlyingCode = option.BaseCode.Replace("HO", "IH");
                //}
            }
            else
            {
                underlyingCode = option.BaseCode;
            }
            return underlyingCode;
        }

        public static int GetContractRemainingDays(Contract codeItem)
        {
            TimeSpan remainingDate = DateTime.ParseExact(codeItem.ExpireDate, "yyyyMMdd", CultureInfo.CurrentCulture) - DateTime.Now;
            return remainingDate.Days;
        }

        public static bool IsOutOfDate(string contract)
        {
            return false;
        }
    }
}
