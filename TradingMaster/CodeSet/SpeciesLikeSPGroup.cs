using System;
using System.Collections.Generic;
using TradingMaster.CodeSet;

namespace TradingMaster
{
    public class SpeciesLikeSPGroup : Species
    {
        public SpeciesLikeSPGroup(string name, Species baseSpecies, string baseCurrency, string codeTitle)
            : base(name)
        {
            this.BaseCurrency = baseCurrency;
            GenerateCodeInfo(baseSpecies, codeTitle);
        }

        /// <summary>
        /// 生成品种代码
        /// </summary>
        protected void GenerateCodeInfo(Species baseSpecies, string codeTitle)
        {
            if (Codes == null) Codes = new List<Contract>();
            for (int i = 0; i < baseSpecies.Codes.Count - 1; i++)
            {
                for (int j = i + 1; j < baseSpecies.Codes.Count; j++)
                {
                    Contract codeInfo = new Contract(codeTitle + " " + baseSpecies.Codes[i].Code + "&" + baseSpecies.Codes[j].Code, baseSpecies.Codes[i].Hycs, baseSpecies.Codes[i].Fluct);
                    codeInfo.Name = baseSpecies.Codes[i].Name + "/" + baseSpecies.Codes[j].Name;
                    //codeInfo.TradingTime = baseSpecies.codes[i].TradingTime;
                    //codeInfo.TimeZone = baseSpecies.codes[i].TimeZone;
                    Codes.Add(codeInfo);

                    //添加合约信息
                    //SpeciesCodeManager.GetInstance().AddSpeicesCode(new SpeciesCode()
                    //{
                    //    MarketType = (ushort)marketType,
                    //    Month = "",
                    //    MonthCode = "",
                    //    Year = "",
                    //    YearCode = "",
                    //    Code = codeInfo.Code,
                    //    Species = speciesName,
                    //    Currency = baseCurrency
                    //});

                }
            }
        }

        public SpeciesLikeSPGroup(string name, string name2, Species baseSpecies, Species baseSpecies2, string codeTitle)
            : base(name)
        {
            GenerateSPCCodeInfo(baseSpecies, baseSpecies2, codeTitle);
        }

        /// <summary>
        /// 生成跨品种代码
        /// </summary>
        public void GenerateSPCCodeInfo(Species baseSpecies, Species baseSpecies2, string codeTitle)
        {
            if (Codes == null) Codes = new List<Contract>();
            for (int i = 0, j = 0; i < Math.Min(baseSpecies.Codes.Count, baseSpecies2.Codes.Count) && j < Math.Min(baseSpecies.Codes.Count, baseSpecies2.Codes.Count);)
            {
                int tempcount1 = baseSpecies.Codes[i].Code.IndexOfAny("0123456789".ToCharArray());
                string tempcode1 = baseSpecies.Codes[i].Code.Substring(tempcount1);
                int tempcount2 = baseSpecies2.Codes[j].Code.IndexOfAny("0123456789".ToCharArray());
                string tempcode2 = baseSpecies2.Codes[j].Code.Substring(tempcount2);
                if (tempcode1 == tempcode2)
                {
                    //CodeInfo codeInfo = new CodeInfo(marketType, codeTitle + " " + baseSpecies.codes[i].codeInfo.StrCode + "&" + baseSpecies2.codes[j].codeInfo.StrCode, baseCurrency);
                    Contract codeInfo = new Contract(codeTitle + " " + baseSpecies.Codes[i].Code + "&" + baseSpecies2.Codes[j].Code, baseSpecies.Codes[i].Hycs, baseSpecies.Codes[i].Fluct);
                    codeInfo.Name = baseSpecies.Codes[i].Name + "/" + baseSpecies2.Codes[j].Name;
                    //codeInfo.TradingTime = baseSpecies.codes[i].TradingTime;
                    //codeInfo.TimeZone = baseSpecies.codes[i].TimeZone;
                    Codes.Add(codeInfo);

                    //添加合约信息
                    //SpeciesCodeManager.GetInstance().AddSpeicesCode(new SpeciesCode()
                    //{
                    //    //MarketType = (ushort)marketType,
                    //    Month = "",
                    //    MonthCode = "",
                    //    Year = "",
                    //    YearCode = "",
                    //    Code = codeInfo.Code,
                    //    Species = speciesName,
                    //    Currency = baseCurrency
                    //});

                    i++;
                    j++;
                }
                else if (int.Parse(tempcode1) > int.Parse(tempcode2))
                {
                    j++;
                }
                else if (int.Parse(tempcode1) < int.Parse(tempcode2))
                {
                    i++;
                }

            }
        }


    }
}
