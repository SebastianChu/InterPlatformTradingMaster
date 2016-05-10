using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using TradingMaster.CodeSet;

namespace TradingMaster
{
    /// <summary>
    /// 使用Converter来控制涨跌的字体颜色变化
    /// </summary>
    public class PriceFluctuationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double v = CommonUtil.GetDoubleValue(value);

            if (v > 0)
            {
                return 1;
            }
            else if (v < 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 使用Converter来控制行情数据的显示
    /// 如果是0则不显示，如果是尽量不显示最后的0
    /// 关于最新价，买价，卖价等在CustomePriceFormatConverter处理
    /// </summary>
    public class PriceFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.ToString().Equals("NaN") || value.ToString().Equals("非数字"))
            {
                return CommonUtil.DefaultHQDisplayValue;
            }

            if (value is UInt64)
            {
                UInt64 v = (UInt64)value;
                if (v == 0)
                {
                    return CommonUtil.DefaultHQDisplayValue;
                }
                else
                {
                    return v.ToString();
                }
            }
            else if (value is uint)
            {
                uint v = (uint)value;
                if (v == 0)
                {
                    return CommonUtil.DefaultHQDisplayValue;
                }
                else
                {
                    return v.ToString();
                }
            }
            else if (value is Int64)
            {
                Int64 v = (Int64)value;
                if (v == 0)
                {
                    return CommonUtil.DefaultHQDisplayValue;
                }
                else
                {
                    return v.ToString();
                }
            }
            else if (value is UInt32)
            {
                UInt32 v = (UInt32)value;
                if (v == 0)
                {
                    return CommonUtil.DefaultHQDisplayValue;
                }
                else
                {
                    return v.ToString();
                }
            }
            else if (value is int)
            {
                int v = (int)value;
                if (v == 0)
                {
                    return CommonUtil.DefaultHQDisplayValue;
                }
                else
                {
                    return v.ToString();
                }
            }
            else if (value is short)
            {
                short v = (short)value;
                if (v == 0)
                {
                    return CommonUtil.DefaultHQDisplayValue;
                }
                else
                {
                    return v.ToString();
                }
            }
            else if (value is double)
            {
                double v = (double)value;
                if (v == 0)
                {
                    return CommonUtil.DefaultHQDisplayValue;
                }
                else
                {
                    if (parameter != null)
                    {
                        v = Math.Round(v, 4);
                        return v.ToString(parameter.ToString());
                    }

                    v = Math.Round(v, 2);

                    if (v - (int)v == 0)
                    {
                        return v.ToString("F0");
                    }
                    else if (v * 10 - (int)(v * 10) == 0)
                    {
                        return v.ToString("F1");
                    }
                    return v.ToString("F2");
                }
            }
            else if (value is Double)
            {
                Double v = (Double)value;
                if (v == 0)
                {
                    return CommonUtil.DefaultHQDisplayValue;
                }
                else
                {
                    //取2位小数
                    v = Math.Round(v, 2);
                    if (v - (int)v == 0)
                    {
                        return v.ToString("F0");
                    }
                    else if (v * 10 - (int)(v * 10) == 0)
                    {
                        return v.ToString("F1");
                    }
                    return v.ToString("F2");
                }
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 使用Converter来控制价格数据的显示
    /// 根据最少变动价格决定显示数据的格式
    /// </summary>
    public class MultiCustomePriceFormatConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //获得绑定的值
            object propertyValue = null;
            if (values.Length > 1)
            {
                propertyValue = values[1];
            }
            else
            {
                propertyValue = values[0];
            }

            if (propertyValue == null || propertyValue.ToString().Equals("NaN") || propertyValue.ToString().Equals("非数字"))
            {
                return CommonUtil.DefaultHQDisplayValue;
            }

            //获得最小变动单位
            string code = values[0] == null ? "" :values[0].ToString();
            if (code.StartsWith("TF") && (double)propertyValue!=0)
            {
                //Util.Log("TF");
            }


            double doubleValue = 0;
            if (propertyValue is double)
            {
                doubleValue = (double)propertyValue;
            }
            else if (propertyValue is Double)
            {
                doubleValue = double.Parse(propertyValue.ToString());
            }
            else if (propertyValue is string)
            {
                doubleValue = CommonUtil.GetDoubleValue(propertyValue);
            }
            
            if (doubleValue == 0)
            {
                return CommonUtil.DefaultHQDisplayValue;
            }
            else
            {
                return doubleValue.ToString(CommonUtil.GetPriceFormat(code));

            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 使用Converter来控制价格数据的显示
    /// 根据最少变动价格决定显示数据的格式
    /// </summary>
    public class ParameterFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //获得绑定的值
            object propertyValue = value;

            if (propertyValue == null || propertyValue.ToString().Equals("NaN") || propertyValue.ToString().Equals("非数字"))
            {
                return CommonUtil.DefaultHQDisplayValue;
            }

            double doubleValue = 0;
            if (propertyValue is double)
            {
                if (double.IsNaN((double)propertyValue) || double.IsInfinity((double)propertyValue) || (double)propertyValue >= double.MaxValue)
                {
                    return CommonUtil.DefaultHQDisplayValue;
                }
                else
                {
                    doubleValue = (double)propertyValue;
                }
            }
            else if (propertyValue is Double)
            {
                if (Double.IsNaN((Double)propertyValue) || Double.IsInfinity((Double)propertyValue) || (Double)propertyValue >= Double.MaxValue)
                {
                    return CommonUtil.DefaultHQDisplayValue;
                }
                else
                {
                    doubleValue = double.Parse(propertyValue.ToString());
                }
            }
            else if (propertyValue is string)
            {
                doubleValue = CommonUtil.GetDoubleValue(propertyValue);
            }

            if (doubleValue == 0)
            {
                return CommonUtil.DefaultHQDisplayValue;
            }
            else
            {
                return doubleValue;//.ToString(value.ToString());
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 在用户选择自选合约时，需要同时显示代码和名称
    /// </summary>
    public class CodeNamePairConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "";
            string code = value.ToString();
            if (CodeSetManager.GetContractInfo(code) != null)
            {
                return CodeSetManager.GetContractInfo(code).Name;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 用于资金查询时显示盯/浮的值
    /// </summary>
    public class MultiCapitalPairValue : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double value = CommonUtil.GetDoubleValue(values[0]);
            double value2 = CommonUtil.GetDoubleValue(values[1]);
            //超过两个时，技术的相加；偶数相加
            if (values.Length > 2)
            {
                value += CommonUtil.GetDoubleValue(values[2]);
                value2 += CommonUtil.GetDoubleValue(values[3]);
            }
            return value.ToString("F2") + "/" + value2.ToString("F2");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MultiValuePlusConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double value = 0;
            foreach (var item in values)
            {
                value += CommonUtil.GetDoubleValue(item);
            }
            return value;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 在预埋条件单中显示触发条件
    /// </summary>
    public class MultiValueTouchConditionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string condition = string.Empty;
            string touchMethod = values[0] == null ? "" : values[0].ToString();
            string touchMethod1 = values[1] == null ? "" : values[1].ToString();
            string touchMethod2 = values[2] == null ? "" : values[2].ToString();
            if (touchMethod.Contains("埋单"))
            {
                //condition = "手动发出";
                condition = touchMethod;
            }
            else if (touchMethod.Contains("自动"))
            {
                condition = "当重新进入交易状态时发出";
            }
            else
            {
                condition = touchMethod + touchMethod1 + touchMethod2 + "时发出";
            }

            return condition;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 埋单条件单类型
    /// </summary>
    public class MaiConditionOrderTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string orderType = string.Empty;

            string orderStatus = value == null ? "" : value.ToString();
            if (orderStatus.Contains("埋单"))
            {
                orderType = "预埋单(服务器)";
            }
            else if (orderStatus.Contains("自动"))
            {
                orderType = "自动单(服务器)";
            }
            else
            {
                orderType = "条件单(服务器)";
            }
            return orderType;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 埋单条件单状态
    /// </summary>
    public class MaiConditionOrderStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string newOrderStatus = "未发送";

            string orderStatus = value.ToString();
            if (orderStatus.Contains("删除")
                || orderStatus.Contains("撤单"))
            {
                newOrderStatus = "已撤单";
            }
            else if (orderStatus.Contains("拒绝"))
            {
                newOrderStatus = "已拒绝";
            }
            else if (!(orderStatus == "埋单"
                || orderStatus == "自动单"
                || orderStatus == "条件单" || orderStatus == "已报服务器"))
            {
                newOrderStatus = "已发送";
            }

            return newOrderStatus;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //public class MaiConditionOrderStatusForegroundConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        int newOrderStatus = 1;

    //        string orderStatus = value.ToString();
    //        if (!(orderStatus=="埋单"
    //            || orderStatus== "自动单"
    //            || orderStatus=="条件单"))
    //        {
    //            newOrderStatus = 2;
    //        }

    //        return newOrderStatus;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    /// <summary>
    /// 使用Converter来控制行情数据的字体颜色
    /// </summary>
    public class MultiPriceForegroundConverter : IMultiValueConverter
    {
        /// <summary>
        /// 1：涨；0：平；-1：跌；-2：非法值
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //获得绑定的值
            object propertyValue = values[1];

            if (values[0] == null || values[0].ToString().Equals("NaN") || values[0].ToString().Equals("非数字")
                || values[1] == null || values[1].ToString().Equals("NaN") || values[1].ToString().Equals("非数字"))
            {
                return -2;
            }

            double value = CommonUtil.GetDoubleValue(values[0]);
            double value2 = CommonUtil.GetDoubleValue(values[1]);
            if (value == 0)
            {
                return -2;
            }
            if (value > value2)
            {
                return 1;
            }
            else if (value < value2)
            {
                return -1;
            }
            else
            {
                return 0;
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 使用Converter来控制价格数据的显示
    /// 根据最少变动价格决定显示数据的格式
    /// 
    /// 当绑定属性的数值不会变时使用CustomePriceFormatConverter，如果属性的值会变必须使用MultiCustomePriceFormatConverter
    /// </summary>
    public class CustomePriceFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //获得绑定的值
            object propertyValue = CommonUtil.GetOrderProperty(value, parameter.ToString());

            if (propertyValue == null || propertyValue.ToString().Equals("NaN") || propertyValue.ToString().Equals("非数字"))
            {
                return CommonUtil.DefaultHQDisplayValue;
            }

            //获得最小变动单位
            string code = CommonUtil.GetOrderProperty(value, "Code").ToString();
            //int hycs = 0;
            double fluct = 0;
            //CodeGen.CodeSet.GetHycsAndFluct(code, out hycs, out fluct);

            double doubleValue = 0;
            if (propertyValue is double)
            {
                doubleValue = (double)propertyValue;
            }
            else if (propertyValue is Double)
            {
                doubleValue = double.Parse(propertyValue.ToString());
            }
            else if (propertyValue is string)
            {
                doubleValue = CommonUtil.GetDoubleValue(propertyValue);
            }


            if (doubleValue == 0)
            {
                return CommonUtil.DefaultHQDisplayValue;
            }
            else
            {
                //根据最小变动单位确定小数点位数，目前只有Au的fluct < 0.1；IF的fluct < 1；其他的都大于等于1
                if (fluct < 0.1)
                {
                    return doubleValue.ToString("F2");
                }
                else if (fluct < 1)
                {
                    return doubleValue.ToString("F1");
                }
                else
                {
                    return doubleValue.ToString("F0");
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Xaml中的DataGridTextColumn绑定是直接设置小数格式为F2时，0会显示成0，不是想要的0.00，因此需要转换
    /// </summary>
    public class DoubleValueFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value == null || value.ToString().Equals("NaN") || value.ToString().Equals("非数字"))
            {
                return CommonUtil.DefaultHQDisplayValue;
            }


            double doubleValue = 0.00;
            if (value is double)
            {
                doubleValue = (double)value;
            }
            else if (value is Double)
            {
                doubleValue = double.Parse(value.ToString());
            }
            else if (value is string)
            {
                doubleValue = CommonUtil.GetDoubleValue(value);
            }


            return doubleValue.ToString(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 使用Converter来控制买卖的字体颜色变化
    /// </summary>
    public class BuySellForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string text = value == null ? "" : value.ToString();

            if (text.Contains("买"))
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 使用Converter来控制开平仓的字体颜色变化
    /// </summary>
    public class KpForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string text = value.ToString();

            if (text.Contains("开仓"))
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 使用Converter来控制开平仓的字体颜色变化
    /// </summary>
    public class OrderStatusForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string text = value.ToString();

            if (text.Contains("失败"))
            {
                return 1;
            }
            else if (text.Contains("全部成交"))
            {
                return 2;
            }
            else if (text.Contains("未成交") || text.Contains("已撤") || text.Contains("未触发"))
            {
                return 3;
            }
            else if (text.Contains("拒绝"))
            {
                return 4;
            }
            else if (text.Contains("触发"))
            {
                return 5;
            }
            else
            {
                return 6;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 使用Converter来控制行权状态的字体颜色变化
    /// </summary>
    public class ExecStatusForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string text = value.ToString();

            if (text.Contains("失败"))
            {
                return 1;
            }
            else if (text.Contains("执行成功"))
            {
                return 2;
            }
            else if (text.Contains("已报") || text.Contains("已取消") || text.Contains("未报"))
            {
                return 3;
            }
            else if (text.Contains("拒绝"))
            {
                return 4;
            }
            else if (text.Contains("未执行"))
            {
                return 5;
            }
            else
            {
                return 6;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 使用Converter来控制持仓类型的字体颜色变化
    /// </summary>
    public class PositionTypeForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string text = value.ToString();

            if (text.Contains("今"))
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 使用Converter来控制持仓类型的字体颜色变化
    /// </summary>
    public class TradeHandCountForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int handCount = int.Parse(value.ToString());

            if (handCount > 0)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BuySellStringUpdated : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string buysellinfo = value == null ? "" :value.ToString();

            if (buysellinfo.Contains("买"))
                buysellinfo = "买";
            //return 1;
            else if (buysellinfo.Contains("卖"))
                buysellinfo = "  卖";
            //return 2;
            //else if (buysellinfo.Contains("平仓"))
            //    buysellinfo = "  平仓";
            return buysellinfo;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GroupOrderCodeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string code = value.ToString();
            //if (CodeGen.CodeSet.IsTaoli(code))
            //{
            //    string[] groupcode = code.Split(' ');
            //    string[] newcode = groupcode[1].Split('&');
            //    code = newcode[0] + "/" + newcode[1];
            //}
            return code;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HandValueUpdater_OF : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string buysellinfo = value.ToString();


            return buysellinfo + "手";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PriceValueUpdater_OF : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string buysellinfo = value.ToString();

            if (buysellinfo.Equals("0") == true)
            {
                return "";
            }

            return "于价格 " + buysellinfo;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TimeFormatChanger : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.ToString() != "000000")
            {
                return (value.ToString().Replace(":", "")).TrimStart('0');
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double v = 0;
            if (value == null) return "-";
            if (false == double.TryParse(value.ToString(), out v)) return "-";
            if (v == 0 || double.IsNaN(v)) return "-";
            return v.ToString("F2");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FOConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "-";
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    ///// <summary>
    ///// 在预埋条件单中显示触发条件
    ///// </summary>
    //public class MultiValueTouchConditionConverter : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        string condition = string.Empty;
    //        string touchMethod = values[0].ToString();
    //        if (touchMethod.Contains("埋单") || touchMethod.Contains("预置单"))
    //        {
    //            condition = "手动";
    //        }
    //        else if (touchMethod.Contains("自动"))
    //        {
    //            condition = "重新进入交易状态";
    //        }
    //        else
    //        {
    //            condition = values[0].ToString() + values[1].ToString() + values[2].ToString();
    //        }

    //        return condition;
    //    }

    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    ///// <summary>
    ///// 埋单条件单类型
    ///// </summary>
    //public class MaiConditionOrderTypeConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        string orderType = string.Empty;
    //        if (value == null) return orderType;

    //        string orderStatus = value.ToString();
    //        if (orderStatus.Contains("埋单") || orderStatus.Contains("预置单"))
    //        {
    //            orderType = "预置单";
    //        }
    //        else if (orderStatus.Contains("自动"))
    //        {
    //            orderType = "自动单";
    //        }
    //        else
    //        {
    //            orderType = "条件单";
    //        }
    //        return orderType;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    ///// <summary>
    ///// 埋单条件单状态
    ///// </summary>
    //public class MaiConditionOrderStatusConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        string newOrderStatus = "未触发";

    //        string orderStatus = value.ToString();

    //        if (orderStatus.Contains("删除"))
    //        {
    //            newOrderStatus = "已撤单";
    //        }
    //        else if (orderStatus.Contains("拒绝"))
    //        {
    //            newOrderStatus = "已拒绝";
    //        }
    //        else if (orderStatus.Contains("有效"))
    //        {
    //            newOrderStatus = "未触发";
    //        }
    //        else if (orderStatus.Contains("已过期"))
    //        {
    //            newOrderStatus = "已过期";
    //        }
    //        else if (orderStatus.Contains("触发成功"))
    //        {
    //            newOrderStatus = "触发成功";
    //        }
    //        else if (orderStatus.Contains("触发失败"))
    //        {
    //            newOrderStatus = "触发失败";
    //        }
    //        else if (!(orderStatus == "预置单"
    //            || orderStatus == "自动单"
    //            || orderStatus == "条件单" || orderStatus == "已报服务器"))
    //        {
    //            newOrderStatus = "已触发";
    //        }

    //        return newOrderStatus;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class ConditionToolTipConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string feedBackInfo = values[0] == null ? "" : values[0].ToString();
            string orderStatus = values[1] == null ? "" : values[1].ToString();
            //string toolTipString = "";

            string oper = "";
            string orderType = "";
            if (orderStatus.Contains("拒绝"))
            {
                oper = "拒绝";
                orderType = orderStatus.Replace(oper, "");
            }
            else if (orderStatus.Contains("删除"))
            {
                oper = "删除";
                orderType = orderStatus.Replace(oper, "");
            }
            else if ((orderStatus == "预置单" || orderStatus == "自动单" || orderStatus == "条件单") ||
                (orderStatus == "预置单有效" || orderStatus == "自动单有效" || orderStatus == "条件单有效"))
            {
                orderType = orderStatus;
                oper = "未触发";
            }
            else if (orderStatus.Contains("已过期"))
            {
                oper = "已过期";
                orderType = orderStatus.Replace(oper, "");

            }
            else if (orderStatus.Contains("触发成功"))
            {
                oper = "触发成功";
                orderType = orderStatus.Replace(oper, "");
            }
            else if (orderStatus.Contains("触发失败"))
            {
                oper = "触发失败";
                orderType = orderStatus.Replace(oper, "");
            }
            else if (orderStatus.Contains("触发"))
            {
                oper = "触发";
                orderType = orderStatus.Replace(oper, "");
            }

            switch (oper)
            {
                case "触发成功":
                    return "该" + orderType + "已经被触发，而且触发后下单成功";
                case "触发失败":
                    return "该" + orderType + "已经被触发，但是触发后下单失败：" + feedBackInfo;
                case "已过期":
                    return "该" + orderType + "已经过期";
                case "未触发":
                    return "该" + orderType + "还未被触发";
                case "拒绝":
                    //return "该" + orderType + "被服务器拒绝:" + feedBackInfo;
                    return "该" + orderType + "被服务器拒绝：" + feedBackInfo;
                case "删除":
                    return "该" + orderType + "已经被撤单";
                case "触发":
                    return "该" + orderType + "已经被触发";
            }
            return "已触发";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
