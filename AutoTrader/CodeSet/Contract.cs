﻿using System;
using System.Runtime.InteropServices;

namespace AutoTrader
{
    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public class Contract
    {
        /// <summary>
        /// 代码
        /// </summary>
        private string _Code;
        /// <summary>
        /// 代码
        /// </summary>
        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        /// <summary>
        /// 名字
        /// </summary>
        private string _Name;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// 交易所代码
        /// </summary>
        private string _ExchangeId;
        /// /// <summary>
        /// 交易所代码
        /// </summary>
        public string ExchangeId
        {
            get { return _ExchangeId; }
            set { _ExchangeId = value; }
        }

        /// <summary>
        /// 基础合约代码
        /// </summary>
        private string _BaseCode;
        /// <summary>
        /// 基础合约代码
        /// </summary>
        public string BaseCode
        {
            get { return _BaseCode; }
            set { _BaseCode = value; }
        }

        /// /// <summary>
        /// 交易所
        /// </summary>
        public string ExchName
        {
            get { return CodeSetManager.CtpToExName(_ExchangeId); }
        }

        /// <summary>
        /// 上市日 
        /// </summary>
        private string _OpenDate;
        /// <summary>
        /// 上市日 
        /// </summary>
        public string OpenDate
        {
            get { return _OpenDate; }
            set { _OpenDate = value; }
        }

        /// <summary>
        /// 到期日 
        /// </summary>
        private string _ExpireDate;
        /// <summary>
        /// 到期日 
        /// </summary>
        public string ExpireDate
        {
            get { return _ExpireDate; }
            set { _ExpireDate = value; }
        }

        /// <summary>
        /// 拼音缩写
        /// </summary>
        private string[] _Spell;
        /// <summary>
        /// 拼音缩写
        /// </summary>
        public string[] Spell
        {
            get { return _Spell; }
            set { _Spell = value; }
        }

        /// <summary>
        /// 合约乘数
        /// </summary>
        private int _VolumeMultiple;
        /// <summary>
        /// 合约乘数
        /// </summary>
        public int VolumeMultiple
        {
            get { return _VolumeMultiple; }
            set { _VolumeMultiple = value; }
        }

        /// <summary>
        /// 最小变动单位
        /// </summary>
        private double _PriceTick;
        /// <summary>
        /// 最小变动单位
        /// </summary>
        public double PriceTick
        {
            get { return _PriceTick; }
            set { _PriceTick = value; }
        }

        /// <summary>
        /// 期权类型
        /// </summary>
        private string _OptionType;
        /// <summary>
        /// 期权类型
        /// </summary>
        public string OptionType
        {
            get { return _OptionType; }
            set { _OptionType = value; }
        }

        /// <summary>
        /// 产品类型
        /// </summary>
        private string _ProductType;
        /// <summary>
        /// 产品类型
        /// </summary>
        public string ProductType
        {
            get { return _ProductType; }
            set { _ProductType = value; }
        }

        /// <summary>
        /// 品种名
        /// </summary>
        private string _SpeciesCode;
        /// <summary>
        /// 品种名
        /// </summary>
        public string ProductID
        {
            get { return _SpeciesCode; }
            set { _SpeciesCode = value; }
        }

        /// <summary>
        /// 执行价
        /// </summary>
        private double _Strike;
        /// <summary>
        /// 执行价
        /// </summary>
        public double Strike
        {
            get { return _Strike; }
            set { _Strike = value; }
        }

        /// <summary>
        /// 交易所多头保证金率
        /// </summary>
        private double _LongMarginRatio;
        /// <summary>
        /// 交易所多头保证金率
        /// </summary>
        public double LongMarginRatio
        {
            get { return _LongMarginRatio; }
            set { _LongMarginRatio = value; }
        }

        /// <summary>
        /// 交易所空头保证金率
        /// </summary>
        private double _ShortMarginRatio;
        /// <summary>
        /// 交易所空头保证金率
        /// </summary>
        public double ShortMarginRatio
        {
            get { return _ShortMarginRatio; }
            set { _ShortMarginRatio = value; }
        }

        /// <summary>
        /// 是否采用单边保证金算法
        /// </summary>
        private bool _IsMaxMarginSingleSide;
        /// <summary>
        /// 是否采用单边保证金算法
        /// </summary>
        public bool IsMaxMarginSingleSide
        {
            get { return _IsMaxMarginSingleSide; }
            set { _IsMaxMarginSingleSide = value; }
        }

        /// <summary>
        /// 市价单最大下单量
        /// </summary>
        private int _MaxMarketOrderVolume;
        /// <summary>
        /// 市价单最大下单量
        /// </summary>
        public int MaxMarketOrderVolume
        {
            get { return _MaxMarketOrderVolume; }
            set { _MaxMarketOrderVolume = value; }
        }

        /// <summary>
        /// 限价单最大下单量
        /// </summary>
        private int _MaxLimitOrderVolume;
        /// <summary>
        /// 限价单最大下单量
        /// </summary>
        public int MaxLimitOrderVolume
        {
            get { return _MaxLimitOrderVolume; }
            set { _MaxLimitOrderVolume = value; }
        }

        public override string ToString()
        {
            return "[" + _Code + "][" + _Name + "][" + (_ExchangeId != null ? CodeSetManager.CtpToExName(_ExchangeId) : "") + "]";
        }

        public static int CompareByCode(Contract t1, Contract t2)
        {
            try
            {
                if (t1 == null && t2 == null) return 0;
                if (t1 == null) return -1;
                if (t2 == null) return 1;
                return t1._Code.CompareTo(t2._Code);
            }
            catch (Exception ex)
            {
                Util.WriteInfo("exception" + ex.Message);
                Util.WriteError(ex.StackTrace);
                return 0;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            //if (ReferenceEquals(this, obj))
            //{
            //    return false;
            //}
            if (this.GetType() != obj.GetType())
            {
                return false;
            }
            if (this.GetHashCode() != obj.GetHashCode())
            {
                return false;
            }
            Contract other = obj as Contract;
            if (other == null || ExchangeId == null) return base.Equals(obj);
            return (ExchangeId.Equals(other.ExchangeId)) && Code.Equals(other.Code);// && Hycs.Equals(other.Hycs) && Fluct.Equals(other.Fluct);
        }

        public override int GetHashCode()
        {
            if (Code == null)
            {
                return base.GetHashCode();
            }
            int hashCode = Code.GetHashCode();
            if (ExchangeId != null && Code.GetHashCode() != ExchangeId.GetHashCode())
            {
                hashCode ^= ExchangeId.GetHashCode();
            }
            return Code.GetHashCode();
        }

        public static bool operator ==(Contract leftHandSide, Contract rightHandSide)
        {
            if (ReferenceEquals(leftHandSide, null))
            {
                return ReferenceEquals(rightHandSide, null);
            }
            return leftHandSide.Equals(rightHandSide);
        }

        public static bool operator !=(Contract leftHandSide, Contract rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }
    }
}
