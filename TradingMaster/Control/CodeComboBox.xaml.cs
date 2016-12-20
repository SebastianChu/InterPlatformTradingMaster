using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TradingMaster.CodeSet;

namespace TradingMaster.Control
{
    /// <summary>
    /// CodeComboBox.xaml 的交互逻辑
    /// </summary>
    public partial class CodeComboBox : UserControl
    {
        private List<SpeciesItem> _Species = null;
        private bool _IsPopupOpen;
        private Key _LastKey = Key.None;
        private Stack<string> _LastTextStack = new Stack<string>();
        private Boolean _MouseDownAccepted = false;

        public TextBox TxtBox_Inner = null;
        public event TextChangedEventHandler TextChanged = null;

        /// <summary>
        /// 名字和SpeciesItem的对应关系
        /// </summary>
        Dictionary<string, SpeciesItem> speciesNameDict = new Dictionary<string, SpeciesItem>();

        public string Text
        {
            get
            {
                if (TxtBox_Inner != null)
                {
                    return TxtBox_Inner.Text;
                }
                return "";
            }
            set
            {
                //if (txtBox_Inner != null)
                //{
                codeCBX.Text = value;
                //}
            }
        }

        /// <summary>
        /// 样式 styleTag
        /// </summary>
        public static readonly DependencyProperty styleTagProperty = DependencyProperty.Register("StyleTag", typeof(string), typeof(CodeComboBox), new PropertyMetadata(null, new PropertyChangedCallback(StyleTagChange)));
        public String StyleTag
        {
            get
            {
                return (string)GetValue(styleTagProperty);
            }
            set
            {
                SetValue(styleTagProperty, value);
            }
        }
        public static void StyleTagChange(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {

        }

        public CodeComboBox()
        {
            InitializeComponent();
            _IsPopupOpen = false;
        }

        /// <summary>
        /// 品种选择下拉列表选择了某个品种时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string content = btn.Tag.ToString();
                string speciesName = "";
                foreach (ComboBoxItem cbi in codeCBX.Items)
                {
                    //string[] fs = cbi.Tag.ToString().Split('.');

                    if (cbi.Tag.Equals(content))
                    {
                        speciesName = cbi.Tag.ToString();
                        break;
                    }
                }
                if (speciesName != null)
                {
                    codeCBX.Text = speciesName.Split('.')[0];
                }
            }
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            Util.Log("PopUpClosed.....");
            _IsPopupOpen = false;
        }

        private void Popup_Opened(object sender, EventArgs e)
        {
            Util.Log("PopUpOpened.....");
            _IsPopupOpen = true;

            string v = codeCBX.Text;
            string speName = GetValidSpeciesName(v);
            if (speName != null)
            {
            }
            else
            {
                //生成所有的品种名
                ReformSpeciesName();
            }
            //v = GetValidSpeciesName(v);
            //if (v == null)
            //{
            //    ReformSpeciesName();
            //}
            //else
            //{
            //    ReformContractsBySpeciesName(v,false);
            //}
        }

        /// <summary>
        /// 根据品种名字生成合约，仅在ComboBox中生成Item
        /// </summary>
        /// <param name="speciesName"></param>
        private string ReformContractsBySpeciesName(string speciesName)
        {
            while (codeCBX.Items.Count != 0)
            {
                codeCBX.Items.Clear();
            }
            _LastKey = Key.None;
            string zlCode = "";
            List<string> contracts = GetContractBySpeciesName(speciesName, out zlCode);
            ComboBoxItem cb1 = null;
            foreach (string ss in contracts)
            {
                cb1 = new ComboBoxItem();
                cb1.Content = ss;
                codeCBX.Items.Add(cb1);
                if (ss.Equals(zlCode))
                {
                    cb1.Tag = "ZL";//表示主力合约
                }
            }
            codeCBX.Tag = "FURTHER";
            return zlCode;
        }

        /// <summary>
        /// 根据品种名得到该品种的长度
        /// </summary>
        /// <param name="speciesName"></param>
        /// <returns></returns>
        private int GetContractLength(string speciesName)
        {
            string zlCode = "";
            List<string> contracts = GetContractBySpeciesName(speciesName, out zlCode);
            if (zlCode == "" && contracts.Count > 0)
            {
                return contracts[0].Length;
            }
            else
            {
                return zlCode.Length;
            }
        }

        /// <summary>
        /// 生成品种名字
        /// </summary>
        private void ReformSpeciesName()
        {
            if (_Species == null)
            {
                _Species = GetAllSpecies();
            }
            _LastKey = Key.None;
            codeCBX.Tag = "";
            //填入主力合约的名字
            while (codeCBX.Items.Count != 0)
            {
                codeCBX.Items.Clear();
            }
            ComboBoxItem cbi = null;
            foreach (SpeciesItem item in _Species)
            {
                cbi = new ComboBoxItem();
                cbi.Tag = item.code + "." + item.otherCode;
                Util.Log("ComboBox 添加品种名:" + cbi.Name);
                cbi.Content = item.name;
                codeCBX.Items.Add(cbi);
            }
        }

        private void PART_EditableTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Util.Log("\nPART_EditableTextBox_TextChanged开始");
            TextBox txtBox = sender as TextBox;
            if (txtBox == null) return;
            TxtBox_Inner = txtBox;
            string content = txtBox.Text;
            string speciesName = GetValidSpeciesName(content);
            Util.Log("content=" + content);
            Util.Log("speciesName=" + speciesName);
            Util.Log("lastTextStack.count=" + _LastTextStack.Count);
            if (_LastTextStack.Count > 0)
            {
                Util.Log("lastTextStack[0]=" + _LastTextStack.ToArray()[0]);
            }



            if (content == "" && _LastTextStack.Count == 1)
            {
                string lastCodeText = _LastTextStack.ToArray()[0];
                int contractLength = GetContractLength(GetValidSpeciesName(lastCodeText));
                Util.Log("合约长度=" + contractLength);
                if (_LastKey == Key.Delete || _LastKey == Key.Back || contractLength == lastCodeText.Length)
                {
                    Util.Log("END lastKey=" + _LastKey.ToString() + "..............");
                    txtBox.Text = lastCodeText;
                    return;
                }
            }

            if (content == "" && _LastTextStack.Count == 0 && (_LastKey == Key.Delete || _LastKey == Key.Back))
            {
                ReformSpeciesName();
                if (_IsPopupOpen == false)
                {
                    codeCBX.IsDropDownOpen = true;
                }
            }
            _LastTextStack.Push(content);
            string zlCode = "";
            if (speciesName != null)
            {
                zlCode = ReformContractsBySpeciesName(speciesName);
                Util.Log("主力合约为:" + zlCode);
                if (_IsPopupOpen == false)
                {
                    codeCBX.IsDropDownOpen = true;
                }

                if (_LastKey != Key.Delete && _LastKey != Key.Back && content.Length != zlCode.Length)
                {
                    if (zlCode != "")
                    {
                        Util.Log("设置主力合约:" + zlCode);
                        txtBox.Text = zlCode;
                    }
                    else
                    {

                        Util.Log("未发现主力合约，默认选择第一个" + zlCode);
                        ComboBoxItem ci = codeCBX.Items[0] as ComboBoxItem;
                        if (content.Length != ci.Content.ToString().Length)
                        {
                            txtBox.Text = ci.Content.ToString();
                        }
                    }
                }
                string[] lastTexts = _LastTextStack.ToArray();
                if (lastTexts.Length >= 2 && lastTexts[0].Length >= lastTexts[1].Length && _LastKey != Key.Delete && _LastKey != Key.Back)
                {
                    txtBox.Select(lastTexts[1].Length, lastTexts[0].Length - lastTexts[1].Length);
                }
            }
            _LastTextStack.Pop();
            if (_LastTextStack.Count == 0)
            {
                if (txtBox.Text.Length != zlCode.Length)
                {
                    txtBox.Select(txtBox.Text.Length, 0);
                }
            }
            Util.Log("PART_EditableTextBox_TextChanged结束\n");
            if (_LastTextStack.Count == 0)
            {
                if (TextChanged != null)
                {
                    TextChanged(this, e);
                }
            }

            //Util.Log("\n\n");
            //Util.Log("PART_EditableTextBox_TextChanged....txtBox_Inner.width=" + txtBox_Inner.Width + " actualWidth=" + txtBox_Inner.ActualWidth);
            //string content = txtBox.Text;
            //Util.Log("content=" + content + " lastText=" + lastText);

            //string lastTextTemp = content;
            //codeCBX.Text = content;
            //string speciesName = GetValidSpeciesName(content);
            //Util.Log("speciesName=" + speciesName);
            //if (speciesName != null)
            //{
            //    string zlCode = "";
            //    if (lastText.Length < content.Length)
            //    //if(lastKey!=Key.Delete && lastKey!=Key.Back)
            //    {
            //        lastText = lastTextTemp;
            //        zlCode = ReformContractsBySpeciesName(speciesName, content, true);
            //    }
            //    lastText = lastTextTemp;
            //    //正确的品种名
            //    if (codeCBX.IsDropDownOpen == false)
            //    {
            //        if (zlCode!="" && zlCode.Length != content.Length)
            //        {
            //            Util.Log("IsDropDownOpen=true");
            //            codeCBX.IsDropDownOpen = true;
            //        }
            //    }
            //    //txtBox.Text = codeCBX.Text;
            //    //content = codeCBX.Text;
            //    ////同时选择最后的到期时间的字符串
            //    //Util.Log("开始选择字符串 Content=" + content + " speciesName=" + speciesName+" zlCode="+zlCode);
            //    //if (zlCode != "")
            //    //{
            //    //    if (zlCode.ToLower().Equals(content.ToLower()))
            //    //    {
            //    //        Util.Log("1 txtBox.Select(" + speciesName.Length.ToString() + "," + (content.Length - speciesName.Length).ToString());
            //    //        if (content.Length - speciesName.Length >= 0)
            //    //        {
            //    //            txtBox.Select(speciesName.Length, content.Length - speciesName.Length);
            //    //        }
            //    //    }
            //    //    else
            //    //    {
            //    //        Util.Log("2 txtBox.Select(" + content.Length.ToString() + "," + (zlCode.Length - content.Length).ToString());
            //    //        if (zlCode.Length - content.Length >= 0)
            //    //        {
            //    //            txtBox.Select(content.Length, zlCode.Length - content.Length);
            //    //        }
            //    //    }
            //    //}
            //}


            //if (txtBox != null)
            //{
            //    string s = txtBox.Text;
            //    Util.Log("内容变化:" + s);
            //    string v = GetValidSpeciesName(s);
            //    Util.Log("正确的品种名:" + v);
            //    if (v != null)  //显示合约
            //    {
            //        if (codeCBX.Tag == null || codeCBX.Tag.ToString() != "FURTHER")
            //        {
            //            if (codeCBX.IsDropDownOpen == false)
            //            {
            //                codeCBX.IsDropDownOpen = true;
            //            }
            //            ReformContractsBySpeciesName(v, true);     //根据品种名字显示所有的合约，且默认选择主力合约
            //        }
            //    }
            //    else
            //    {
            //        if (codeCBX.Tag != null && codeCBX.Tag.ToString() == "FURTHER")
            //        {
            //            ReformSpeciesName();
            //        }
            //    }
            //}
        }



        /// <summary>
        /// 得到正确的品种名
        /// 判断speciesName是否正确（自动转换大小写），如果正确，则返回正确的（转换了大小写的）品种名，否则返回null
        /// </summary>
        /// <param name="speciesName"></param>
        /// <returns></returns>
        private string GetValidSpeciesName(string speciesName)
        {
            if (_Species == null)
            {
                _Species = GetAllSpecies();
            }
            if (speciesName == "" || speciesName == null) return null;
            char[] speciesArr = speciesName.ToCharArray();
            string realSpeciesName = "";
            foreach (char c in speciesArr)
            {
                if (c >= '0' && c <= '9')
                {
                    break;
                }
                else
                {
                    realSpeciesName += c;
                }
            }

            foreach (SpeciesItem si in _Species)
            {
                if (si.code.Equals(realSpeciesName.ToLower()))
                {
                    return realSpeciesName.ToLower();
                }
                else if (si.code.Equals(realSpeciesName.ToUpper()))
                {
                    return realSpeciesName.ToUpper();
                }
                if (si.otherCode.Equals(realSpeciesName.ToLower()))
                {
                    return realSpeciesName.ToLower();
                }
                else if (si.otherCode.Equals(realSpeciesName.ToUpper()))
                {
                    return realSpeciesName.ToUpper();
                }
            }
            return null;
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            if (e.OriginalSource == this)
            {
                codeCBX.Focus();
            }
        }

        private void PART_EditableTextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Util.Log("DOWN codeCBX.IsDropDownOpen=" + codeCBX.IsDropDownOpen);
            if (_IsPopupOpen == false)
            {
                codeCBX.IsDropDownOpen = true;
            }
            else
            {
                codeCBX.IsDropDownOpen = false;
            }
            _MouseDownAccepted = true;
        }

        private void PART_EditableTextBox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Util.Log("UP codeCBX.IsDropDownOpen=" + codeCBX.IsDropDownOpen);
            if (_MouseDownAccepted == true) return;
            if (_IsPopupOpen == false)
            {
                codeCBX.IsDropDownOpen = true;
            }
            else
            {
                codeCBX.IsDropDownOpen = false;
            }
        }


        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            //if (codeCBX.IsFocused == false)
            //{
            //    e.Handled = true;
            //    codeCBX.Focus();
            //}
            //if (isPopupOpen == false)
            //{
            //    codeCBX.IsDropDownOpen = true;
            //}
            //else
            //{
            //    codeCBX.IsDropDownOpen = false;
            //}
        }

        private void PART_EditableTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            _LastKey = e.Key;
        }
        #region "TODO"
        /// <summary>
        /// 根据品种名得到其对应的合约
        /// </summary>
        /// <param name="species"></param>
        private List<string> GetContractBySpeciesName(string species, out string ZLCode)
        {
            //先得到中文名
            string chineseName = CodeSetManager.GetSpeciChineseName(species);
            string otherCode = "";
            if (speciesNameDict.ContainsKey(chineseName))
            {
                SpeciesItem si = speciesNameDict[chineseName];
                if (species.Equals(si.code))
                {
                    otherCode = si.otherCode;
                }
                else if (species.Equals(si.otherCode))
                {
                    otherCode = si.code; ;
                }
            }

            List<Contract> codeInfos = CodeSetManager.GetCodeListBySpecies(species);
            List<Contract> codeInfo2 = CodeSetManager.GetCodeListBySpecies(otherCode);
            if (codeInfo2 != null)
            {
                codeInfos.AddRange(codeInfo2);
            }

            //List<string> zhuliCodes = HQService.HQServ.GetZhuliCode();
            ZLCode = "";
            //foreach (String codeInfo in zhuliCodes)
            //{
            //    string name = GetValidSpeciesName(codeInfo);
            //    if (name.Equals(species))
            //    {
            //        ZLCode = codeInfo;
            //        break;
            //    }
            //}
            List<string> ret = new List<string>();
            if (codeInfos != null)
            {
                foreach (Contract cif in codeInfos)
                {
                    ret.Add(cif.Code);
                }
            }
            return ret;
        }
        /// <summary>
        /// 得到所有的品种
        /// </summary>
        /// <returns></returns>
        private List<SpeciesItem> GetAllSpecies()
        {
            List<SpeciesItem> ret = new List<SpeciesItem>();
            SpeciesItem si = null;
            List<Species> speciesList = null;
            speciesList = CodeSetManager.GetAllInnerSpecies();// CodeSet.GetListSpeciesByMarket(HSMarketDataType.FUTURES_MARKET | HSMarketDataType.CFFEX_BOURSE);
            //foreach (Species s in speciesList)
            //{
            //    si = null;
            //    string chnName=CodeGen.CodeSet.GetSpeciChineseName(s.speciesName);
            //    if (speciesNameDict.ContainsKey(chnName))
            //    {
            //        si = speciesNameDict[chnName];
            //        si.otherCode = s.speciesName;
            //    }
            //    else
            //    {
            //        si = new SpeciesItem();
            //        si.code = s.speciesName;
            //        si.name = chnName;
            //        ret.Add(si);
            //        speciesNameDict[chnName] = si;
            //    }
            //}

            //speciesList=CodeSet.GetListSpeciesByMarket(HSMarketDataType.FUTURES_MARKET | HSMarketDataType.SHANGHAI_BOURSE);
            //foreach (Species s in speciesList)
            //{
            //    si = null;
            //    string chnName = CodeGen.CodeSet.GetSpeciChineseName(s.speciesName);
            //    if (speciesNameDict.ContainsKey(chnName))
            //    {
            //        si = speciesNameDict[chnName];
            //        si.otherCode = s.speciesName;
            //    }
            //    else
            //    {
            //        si = new SpeciesItem();
            //        si.name = chnName;
            //        si.code = s.speciesName;
            //        ret.Add(si);
            //        speciesNameDict[chnName] = si;
            //    }
            //}

            //speciesList = CodeSet.GetListSpeciesByMarket(HSMarketDataType.FUTURES_MARKET | HSMarketDataType.ZHENGZHOU_BOURSE);
            //foreach (Species s in speciesList)
            //{
            //    si = null;
            //    string chnName = CodeGen.CodeSet.GetSpeciChineseName(s.speciesName);
            //    if (speciesNameDict.ContainsKey(chnName))
            //    {
            //        si = speciesNameDict[chnName];
            //        si.otherCode = s.speciesName;
            //    }
            //    else
            //    {
            //        si = new SpeciesItem();
            //        si.code = s.speciesName;
            //        si.name = chnName;
            //        ret.Add(si);
            //        speciesNameDict[chnName] = si;
            //    }
            //}

            //speciesList = CodeSet.GetListSpeciesByMarket(HSMarketDataType.FUTURES_MARKET | HSMarketDataType.DALIAN_BOURSE);
            foreach (Species s in speciesList)
            {
                //Todo:
                //if (s.SpeciesCode.Contains("SP") || s.SpeciesCode.Contains("efp") || s.SpeciesCode.Contains("_o"))
                if (s.SpeciesCode.Contains("efp") || s.SpeciesCode.StartsWith("sc") || s.ProductType == "Combination" || s.ProductType.Contains("Option") || s.SpeciesCode == "CVX")
                    continue;

                si = null;
                string chnName = CodeSetManager.GetSpeciChineseName(s.SpeciesCode);
                if (speciesNameDict.ContainsKey(chnName))
                {
                    si = speciesNameDict[chnName];
                    si.otherCode = s.SpeciesCode;
                }
                else
                {
                    si = new SpeciesItem();
                    si.code = s.SpeciesCode;
                    si.name = chnName;
                    ret.Add(si);
                    speciesNameDict[chnName] = si;
                }
            }
            return ret;
        }
        #endregion

        public int SelectionStart
        {
            set
            {
                if (TxtBox_Inner != null)
                {
                    TxtBox_Inner.SelectionStart = value;
                }
            }
        }

        private double extendsWidth = 0;
        /// <summary>
        /// 
        /// </summary>
        public double ExtendsWidth
        {
            get
            {
                return extendsWidth;
            }
            set
            {
                extendsWidth = value;
            }
        }

        public bool IsDropDownOpen
        {
            set
            {
                codeCBX.IsDropDownOpen = value;
            }
            get
            {
                return codeCBX.IsDropDownOpen;
            }
        }

        public void ClearCodeCBXTag()
        {
            ReformSpeciesName();
            //codeCBX.Tag = string.Empty;
        }
    }

    /// <summary>
    /// 品种
    /// </summary>
    public class SpeciesItem
    {
        public string name = "";
        public string code = "";
        public string otherCode = "";

        public override string ToString()
        {
            return name + " " + code + " " + otherCode;
        }
    }

    public class MinusValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int v = int.Parse(value.ToString());
            return v - 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ButtonWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int v = int.Parse(value.ToString());
            return (v - 2) / 3 - 2;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class FontSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s = value.ToString();
            if (s.Length >= 3)
            {
                char c = s.ToCharArray()[0];
                if (c >= 'A' && c <= 'Z') return 13;
                if (c >= 'a' && c <= 'z') return 13;
                return 11;
            }
            return 13;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
