using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Globalization;

namespace TradingMaster.CodeSet
{
    public class CodeSetManager
    {
        public static string OPTION_SETTINGS_FILE = System.AppDomain.CurrentDomain.BaseDirectory + "/setting/Products.xml";

        private static XmlDocument _ProductDoc = null;

        public static List<Contract> ContractList = new List<Contract>();

        public static List<Species> OptionSpecList = new List<Species>();

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

        /// <summary>
        /// 所有单腿品种信息
        /// </summary>
        /// /// <returns></returns>
        private static Dictionary<String, Species> _SpeciesDict = new Dictionary<string, Species>();
        public static Dictionary<String, Species> SpeciesDict
        {
            get { return _SpeciesDict; }
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
                Util.Log("Warning! Unknown ctpName：" + ctpName);
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
                Util.Log("Warning! Unknown IsoCode：" + isoCode);
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
            // Securities
            else if (ctpName == "SSE")
                return "上证所";
            else if (ctpName == "SZE")
                return "深交所";
            else
            {
                Util.Log("Warning! Unknown ctpName：" + ctpName);
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
            else
            {
                Util.Log("Warning! Unknown Exchange Name：" + exName);
                return "未知";
            }
        }

        public static Boolean GetHycsAndFluct(string code, out double hycs, out decimal fluct)
        {
            hycs = 0;
            fluct = 0;
            if (code == "")
            {
                return false;
            }
            Contract cInfo = GetContractInfo(code);
            if (cInfo != null)
            {
                string currKey = code + "_" + cInfo.ExchCode;
                foreach (string tempKey in ContractMap.Keys)
                {
                    if (tempKey == currKey)
                    {
                        hycs = cInfo.Hycs;
                        fluct = cInfo.Fluct;
                        break;
                    }
                }
                if (hycs == 0 && fluct == 0)
                {
                    GetHycsAndFluctFromSpecies(code, out hycs, out fluct);
                }
            }
            else
            {
                foreach (string tempKey in ContractMap.Keys)
                {
                    if (ContractMap[tempKey].BaseCode.StartsWith(code))
                    {
                        hycs = ContractMap[tempKey].Hycs;
                        fluct = ContractMap[tempKey].Fluct;
                        return true;
                    }
                }
                GetHycsAndFluctFromSpecies(code, out hycs, out fluct);
            }
            return false;
        }

        public static Boolean GetHycsAndFluctFromSpecies(string code, out double hycs, out decimal fluct)
        {
            hycs = 0;
            fluct = 0;
            int index = code.IndexOfAny("0123456789".ToCharArray());
            if (index < 0)
            {
                return false;
            }
            string specCode = code.Substring(0, index);
            List<Contract> tempLst = GetCodeListBySpecies(specCode);
            foreach (var item in tempLst)
            {
                hycs = item.Hycs > 0 ? item.Hycs : 0;
                fluct = item.Fluct > 0 ? item.Fluct : 0;
                return true;
            }
            return false;
        }

        public static string GetSpeciChineseName(string speciesCode)
        {
            if (SpeciesDict.ContainsKey(speciesCode))
            {
                return SpeciesDict[speciesCode].ChineseName;
            }
            return null;
        }

        public static List<Contract> GetCodeListBySpecies(string species)
        {
            List<Contract> contractCodeList = new List<Contract>();
            foreach (var item in SpeciesDict.Keys)
            {
                if (item.Equals(species))
                {
                    contractCodeList.AddRange(SpeciesDict[item].Codes.ToList());
                }
            }
            return contractCodeList;
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

        public static List<Contract> GetOptionContractListBySpecies(string species)
        {
            List<Contract> contractList = new List<Contract>();
            foreach (var item in SpeciesDict.Keys)
            {
                if (item.ToString().StartsWith(species))//if (item.Equals(species))
                {
                    foreach (Contract contract in SpeciesDict[item].Codes)
                    {
                        if (contract.ProductType.Contains("Option"))
                        {
                            contractList.Add(contract);
                        }
                    }
                }
            }
            return contractList;
        }

        public static List<string> GetCodeStringListBySpecies(string species)
        {
            List<string> contractCodeList = new List<string>();
            foreach (var item in SpeciesDict.Keys)
            {
                if (item.ToString().StartsWith(species))//if (item.Equals(species))
                {
                    foreach (Contract contract in SpeciesDict[item].Codes)
                    {
                        if (contract.ProductType.Contains("Option"))
                        {
                            contractCodeList.Add(contract.Code);
                        }
                    }
                }
            }
            return contractCodeList;
        }

        public static List<Species> GetAllInnerSpecies()
        {
            List<Species> innerSpeciesList = new List<Species>();
            foreach (string item in SpeciesDict.Keys)
            {
                innerSpeciesList.Add(SpeciesDict[item]);
            }
            return innerSpeciesList;
        }

        public static string GetValidSpeciesName(string SpeciesName)
        {
            foreach (var item in SpeciesDict.Keys)
            {
                if (SpeciesName.ToUpper().Equals(item.ToUpper()))
                {
                    return item;
                }
            }
            return null;
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
