using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingMaster.CodeSet;

namespace TradingMaster
{
    /// <summary>
    /// 合约品种
    /// </summary>
    public class Species
    {
        protected string BaseCurrency = "CNY";
        /// <summary>
        /// 品种名字
        /// </summary>
        public string SpeciesCode { get; set; }
        ///// <summary>
        ///// 市场类型
        ///// </summary>
        //public int marketType;//TODO:待删除，该字段和全球鹰关联太紧密。应由ISO10383确定的交易所名和品种名唯一确定一个品种。
        /// <summary>
        /// 所有该品种的合约
        /// </summary>
        public List<Contract> Codes { get; set; }
        /// <summary>
        /// 品种的中文名称
        /// </summary>
        public string ChineseName { get; set; }
        /// <summary>
        /// 品种的类型
        /// </summary>
        public string ProductType { get; set; }
        /// <summary>
        /// 合约位数
        /// </summary>
        public int BitCnt { get; set; }
        /// <summary>
        /// 市场交易所标准代码
        /// </summary>
        public string ExchangeCode { get; set; }

        /// 根据品种名字和规则生成代码
        /// </summary>
        protected virtual void GenerateCodeInfo() { }

        public Species(string name)
        {
            SpeciesCode = name;
            //marketType = market;
            Codes = new List<Contract>();
        }

        public List<Contract> GetCodeInfos()
        {
            List<Contract> ret = new List<Contract>();
            if (Codes != null)
            {
                foreach (Contract ex in Codes)
                {
                    ret.Add(ex);
                }
            }
            return ret;
        }
    }
}
