using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows;

namespace TradingMaster
{
    public class CommonStaticMemeber
    {

        private static string DEFAULT_STYLE_FILE_PATH = System.AppDomain.CurrentDomain.BaseDirectory + "/setting/defaultStyle.xml";
        private static string STYLE_FILE_PATH = System.AppDomain.CurrentDomain.BaseDirectory + "/setting/style.xml";

        private static TradeClientStyleUI _CurrentClientStyleUI = null;

        public static TradeClientStyleUI CurrentClientStyleUI
        {
            get
            {
                if (_CurrentClientStyleUI == null)
                {
                    _CurrentClientStyleUI = new TradeClientStyleUI();
                }
                return _CurrentClientStyleUI;
            }
            set { _CurrentClientStyleUI = value; }
        }


        public static void SaveStyleToFile()
        {
            TradeClientStyle style = new TradeClientStyle();

            style.InitValueFromCommonStaticMemeber();
            XmlSerializer ser = new XmlSerializer(typeof(TradeClientStyle));
            TextWriter writer = new StreamWriter(STYLE_FILE_PATH);
            ser.Serialize(writer, style);
            writer.Close();
        }

        public static void LoadStyleFromUserFile()
        {
            LoadStyleFromFile(STYLE_FILE_PATH);
        }

        public static void LoadDefaultStyle()
        {
            LoadStyleFromFile(DEFAULT_STYLE_FILE_PATH);
        }

        private static void LoadStyleFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                XmlSerializer ser = new XmlSerializer(typeof(TradeClientStyle));
                System.IO.FileStream fs = new System.IO.FileStream(filePath, FileMode.Open);
                System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(fs);
                TradeClientStyle style = null;
                try
                {
                    style = (TradeClientStyle)(ser.Deserialize(reader));
                    reader.Close();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    Util.Log("exception: " + ex.Message);
                    Util.Log(ex.StackTrace);
                }

                style.UpdateToCommonStaticMemeber();
            }
        }
    }

    public class TradeClientStyleUI : TradeClientStyle
    {
        FontFamilyConverter converter = new FontFamilyConverter();

        public Brush ChangeColorBrush
        {
            get { return new SolidColorBrush(ChangeColor); }
        }

        public Brush CodeColorBrush
        {
            get { return new SolidColorBrush(CodeColor); }
        }

        public Brush HQOddColorBrush
        {
            get { return new SolidColorBrush(HQOddColor); }
        }

        public Brush HQEvenColorBrush
        {
            get { return new SolidColorBrush(HQEvenColor); }
        }


        public Brush OtherOddColorBrush
        {
            get { return new SolidColorBrush(OtherOddColor); }
        }

        public Brush OtherEvenColorBrush
        {
            get { return new SolidColorBrush(OtherEvenColor); }
        }

        public Brush UpColorBrush
        {
            get { return new SolidColorBrush(UpColor); }
        }

        public Brush DownColorBrush
        {
            get { return new SolidColorBrush(DownColor); }
        }

        public Brush HQContentForegroundBrush
        {
            get { return new SolidColorBrush(LabelHQContentForeground); }
        }

        public Brush HQContentBackgroundBrush
        {
            get { return new SolidColorBrush(LabelHQContentBackground); }
        }

        public Brush HQHeaderForegroundBrush
        {
            get { return new SolidColorBrush(LabelHQHeaderForeground); }
        }

        public Brush HQHeaderBackgroundBrush
        {
            get { return new SolidColorBrush(LabelHQHeaderBackground); }
        }

        public Brush HQSelectedForegroundBrush
        {
            get { return new SolidColorBrush(LabelHQSelectedForeground); }
        }

        public Brush HQSelectedBackgroundBrush
        {
            get
            {

                return GetSelectBackGroundBrush(LabelHQSelectedBackground);
            }
        }

        private Brush GetSelectBackGroundBrush(Color color)
        {
            GradientStopCollection stops = new GradientStopCollection();
            GradientStop stop = new GradientStop(Colors.White, -0.2);
            stops.Add(stop);

            stop = new GradientStop(color, 0.5);
            stops.Add(stop);

            stop = new GradientStop(Colors.White, 1.8);
            stops.Add(stop);

            Point startPoint = new Point(0.5, 0);
            Point endPoint = new Point(0.5, 1);
            LinearGradientBrush brush = new LinearGradientBrush(stops, startPoint, endPoint);

            return brush;
        }

        public FontFamily HQFontFamilyUI
        {
            get { return converter.ConvertFromString(HQFontFamily) as FontFamily; }
        }

        public FontStyle HQFontStyleUI
        {
            get { return HQFontStyle == "Italic" ? FontStyles.Italic : FontStyles.Normal; }
        }

        public FontWeight HQFontWeightUI
        {
            get { return HQFontWeight == "Bold" ? FontWeights.Bold : FontWeights.Normal; }
        }


        public FontFamily HQHeaderFontFamilyUI
        {
            get { return converter.ConvertFromString(HQHeaderFontFamily) as FontFamily; }
        }

        public FontStyle HQHeaderFontStyleUI
        {
            get { return HQHeaderFontStyle == "Italic" ? FontStyles.Italic : FontStyles.Normal; }
        }

        public FontWeight HQHeaderFontWeightUI
        {
            get { return HQHeaderFontWeight == "Bold" ? FontWeights.Bold : FontWeights.Normal; }
        }


        public Brush OtherContentForegroundBrush
        {
            get { return new SolidColorBrush(LabelOtherContentForeground); }
        }

        public Brush OtherContentBackgroundBrush
        {
            get { return new SolidColorBrush(LabelOtherContentBackground); }
        }

        public Brush OtherHeaderForegroundBrush
        {
            get { return new SolidColorBrush(LabelOtherHeaderForeground); }
        }

        public Brush OtherHeaderBackgroundBrush
        {
            get { return new SolidColorBrush(LabelOtherHeaderBackground); }
        }

        public Brush OtherSelectedForegroundBrush
        {
            get { return new SolidColorBrush(LabelOtherSelectedForeground); }
        }


        public Brush OtherSelectedBackgroundBrush
        {
            get { return GetSelectBackGroundBrush(LabelOtherSelectedBackground); }
        }

        public FontFamily OtherFontFamilyUI
        {
            get { return converter.ConvertFromString(OtherFontFamily) as FontFamily; }
        }

        public FontStyle OtherFontStyleUI
        {
            get { return OtherFontStyle == "Italic" ? FontStyles.Italic : FontStyles.Normal; }
        }

        public FontWeight OtherFontWeightUI
        {
            get { return OtherFontWeight == "Bold" ? FontWeights.Bold : FontWeights.Normal; }
        }


        public FontFamily OtherHeaderFontFamilyUI
        {
            get { return converter.ConvertFromString(OtherHeaderFontFamily) as FontFamily; }
        }

        public FontStyle OtherHeaderFontStyleUI
        {
            get { return OtherHeaderFontStyle == "Italic" ? FontStyles.Italic : FontStyles.Normal; }
        }

        public FontWeight OtherHeaderFontWeightUI
        {
            get { return OtherHeaderFontWeight == "Bold" ? FontWeights.Bold : FontWeights.Normal; }
        }



        public double HQRowHeightValue
        {
            get { return Convert.ToDouble(HQRowHeight); }
        }

        public double OtherRowHeightValue
        {
            get { return Convert.ToDouble(OtherRowHeight); }
        }

        public Brush OtherCellBorderBrush
        {
            get { return new SolidColorBrush(OtherCellBorder); }
        }
    }

    public class TradeClientStyle : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        private int cb_consultIndex;
        public int Cb_consultIndex
        {
            get { return cb_consultIndex; }
            set
            {
                cb_consultIndex = value;
                OnPropertyChanged("Cb_consultIndex");
            }
        }

        #region hq

        private Color codeColor;
        public Color CodeColor
        {
            get { return codeColor; }
            set
            {
                codeColor = value;
                OnPropertyChanged("CodeColor");
                OnPropertyChanged("CodeColorBrush");
            }
        }


        private Color hQOddColor;
        public Color HQOddColor
        {
            get { return hQOddColor; }
            set
            {
                hQOddColor = value;
                OnPropertyChanged("HQOddColor");
                OnPropertyChanged("HQOddColorBrush");
            }
        }


        private Color hQEvenColor;
        public Color HQEvenColor
        {
            get { return hQEvenColor; }
            set
            {
                hQEvenColor = value;
                OnPropertyChanged("HQEvenColor");
                OnPropertyChanged("HQEvenColorBrush");
            }
        }


        private Color changeColor;
        public Color ChangeColor
        {
            get { return changeColor; }
            set
            {
                changeColor = value;
                OnPropertyChanged("ChangeColor");
                OnPropertyChanged("ChangeColorBrush");
            }
        }

        private Color upColor { get; set; }
        public Color UpColor
        {
            get { return upColor; }
            set
            {
                upColor = value;
                OnPropertyChanged("UpColor");
                OnPropertyChanged("UpColorBrush");
                OnPropertyChanged("OtherCellUpColor");
            }
        }

        private Color downColor { get; set; }
        public Color DownColor
        {
            get { return downColor; }
            set
            {
                downColor = value;
                OnPropertyChanged("DownColor");
                OnPropertyChanged("DownColorBrush");
            }
        }

        private Color labelHQContentForeground;
        public Color LabelHQContentForeground
        {
            get { return labelHQContentForeground; }
            set
            {
                labelHQContentForeground = value;
                OnPropertyChanged("LabelHQContentForeground");
                OnPropertyChanged("HQContentForegroundBrush");
            }
        }

        private Color labelHQContentBackground;
        public Color LabelHQContentBackground
        {
            get { return labelHQContentBackground; }
            set
            {
                labelHQContentBackground = value;
                OnPropertyChanged("LabelHQContentBackground");
                OnPropertyChanged("HQContentBackgroundBrush");

            }
        }

        private Color labelHQHeaderForeground;
        public Color LabelHQHeaderForeground
        {
            get { return labelHQHeaderForeground; }
            set
            {
                labelHQHeaderForeground = value;
                OnPropertyChanged("LabelHQHeaderForeground");
                OnPropertyChanged("HQHeaderForegroundBrush");
            }
        }

        private Color labelHQHeaderBackground;
        public Color LabelHQHeaderBackground
        {
            get { return labelHQHeaderBackground; }
            set
            {
                labelHQHeaderBackground = value;
                OnPropertyChanged("LabelHQHeaderBackground");
                OnPropertyChanged("HQHeaderBackgroundBrush");
            }
        }

        private Color labelHQSelectedForeground;
        public Color LabelHQSelectedForeground
        {
            get { return labelHQSelectedForeground; }
            set
            {
                labelHQSelectedForeground = value;
                OnPropertyChanged("LabelHQSelectedForeground");
                OnPropertyChanged("HQSelectedForegroundBrush");
            }
        }

        private Color labelHQSelectedBackground { get; set; }
        public Color LabelHQSelectedBackground
        {
            get { return labelHQSelectedBackground; }
            set
            {
                labelHQSelectedBackground = value;
                OnPropertyChanged("LabelHQSelectedBackground");
                OnPropertyChanged("HQSelectedBackgroundBrush");
            }
        }

        private string hqFontFamily;
        public string HQFontFamily
        {
            get { return hqFontFamily; }
            set
            {
                hqFontFamily = value;
                OnPropertyChanged("HQFontFamily");
                OnPropertyChanged("HQFontFamilyUI");
            }
        }

        private double hqFontSize;
        public double HQFontSize
        {
            get { return hqFontSize; }
            set
            {
                hqFontSize = value;
                OnPropertyChanged("HQFontSize");
            }
        }

        private string hqFontStyle;
        public string HQFontStyle
        {
            get { return hqFontStyle; }
            set
            {
                hqFontStyle = value;
                OnPropertyChanged("HQFontStyle");
                OnPropertyChanged("HQFontStyleUI");
            }
        }

        private string hqFontWeight;
        public string HQFontWeight
        {
            get { return hqFontWeight; }
            set
            {
                hqFontWeight = value;
                OnPropertyChanged("HQFontWeight");
                OnPropertyChanged("HQFontWeightUI");
            }
        }

        private string hqHeaderFontFamily;
        public string HQHeaderFontFamily
        {
            get { return hqHeaderFontFamily; }
            set
            {
                hqHeaderFontFamily = value;
                OnPropertyChanged("HQHeaderFontFamily");
                OnPropertyChanged("HQHeaderFontFamilyUI");
            }
        }

        private double hqHeaderFontSize;
        public double HQHeaderFontSize
        {
            get { return hqHeaderFontSize; }
            set
            {
                hqHeaderFontSize = value;
                OnPropertyChanged("HQHeaderFontSize");
            }
        }

        private string hqHeaderFontStyle;
        public string HQHeaderFontStyle
        {
            get { return hqHeaderFontStyle; }
            set
            {
                hqHeaderFontStyle = value;
                OnPropertyChanged("HQHeaderFontStyle");
                OnPropertyChanged("HQHeaderFontStyleUI");
            }
        }

        private string hqHeaderFontWeight;
        public string HQHeaderFontWeight
        {
            get { return hqHeaderFontWeight; }
            set
            {
                hqHeaderFontWeight = value;
                OnPropertyChanged("HQHeaderFontWeight");
                OnPropertyChanged("HQHeaderFontWeightUI");
            }
        }
        #endregion

        #region 其他表格

        private Color labelOtherContentForeground;
        public Color LabelOtherContentForeground
        {
            get { return labelOtherContentForeground; }
            set
            {
                labelOtherContentForeground = value;
                OnPropertyChanged("LabelOtherContentForeground");
                OnPropertyChanged("OtherContentForegroundBrush");
            }
        }

        private Color labelOtherContentBackground;
        public Color LabelOtherContentBackground
        {
            get { return labelOtherContentBackground; }
            set
            {
                labelOtherContentBackground = value;
                OnPropertyChanged("LabelOtherContentBackground");
                OnPropertyChanged("OtherContentBackgroundBrush");
            }
        }

        private Color labelOtherHeaderForeground;
        public Color LabelOtherHeaderForeground
        {
            get { return labelOtherHeaderForeground; }
            set
            {
                labelOtherHeaderForeground = value;
                OnPropertyChanged("LabelOtherHeaderForeground");
                OnPropertyChanged("OtherHeaderForegroundBrush");
            }
        }

        private Color labelOtherHeaderBackground;
        public Color LabelOtherHeaderBackground
        {
            get { return labelOtherHeaderBackground; }
            set
            {
                labelOtherHeaderBackground = value;
                OnPropertyChanged("LabelOtherHeaderBackground");
                OnPropertyChanged("OtherHeaderBackgroundBrush");
            }
        }
        private Color labelOtherSelectedForeground;
        public Color LabelOtherSelectedForeground
        {
            get { return labelOtherSelectedForeground; }
            set
            {
                labelOtherSelectedForeground = value;
                OnPropertyChanged("LabelOtherSelectedForeground");
                OnPropertyChanged("OtherSelectedForegroundBrush");
            }
        }

        private Color labelOtherSelectedBackground;
        public Color LabelOtherSelectedBackground
        {
            get { return labelOtherSelectedBackground; }
            set
            {
                labelOtherSelectedBackground = value;
                OnPropertyChanged("LabelOtherSelectedBackground");
                OnPropertyChanged("OtherSelectedBackgroundBrush");
            }
        }

        private string otherFontFamily;
        public string OtherFontFamily
        {
            get { return otherFontFamily; }
            set
            {
                otherFontFamily = value;
                OnPropertyChanged("OtherFontFamily");
                OnPropertyChanged("OtherFontFamilyUI");
            }
        }

        private double otherFontSize;
        public double OtherFontSize
        {
            get { return otherFontSize; }
            set
            {
                otherFontSize = value;
                OnPropertyChanged("OtherFontSize");
            }
        }

        private string otherFontStyle;
        public string OtherFontStyle
        {
            get { return otherFontStyle; }
            set
            {
                otherFontStyle = value;
                OnPropertyChanged("OtherFontStyle");
                OnPropertyChanged("OtherFontStyleUI");
            }
        }

        private string otherFontWeight;
        public string OtherFontWeight
        {
            get { return otherFontWeight; }
            set
            {
                otherFontWeight = value;
                OnPropertyChanged("OtherFontWeight");
                OnPropertyChanged("OtherFontWeightUI");
            }
        }

        private string hqRowHeight;
        public string HQRowHeight
        {
            get { return hqRowHeight; }
            set
            {
                hqRowHeight = value;
                OnPropertyChanged("HQRowHeight");
                OnPropertyChanged("HQRowHeightValue");
            }
        }
        private string otherRowHeight;
        public string OtherRowHeight
        {
            get { return otherRowHeight; }
            set
            {
                otherRowHeight = value;
                OnPropertyChanged("OtherRowHeight");
                OnPropertyChanged("OtherRowHeightValue");
            }
        }


        private string otherHeaderFontFamily;
        public string OtherHeaderFontFamily
        {
            get { return otherHeaderFontFamily; }
            set
            {
                otherHeaderFontFamily = value;
                OnPropertyChanged("OtherHeaderFontFamily");
                OnPropertyChanged("OtherHeaderFontFamilyUI");
            }
        }

        private double otherHeaderFontSize;
        public double OtherHeaderFontSize
        {
            get { return otherHeaderFontSize; }
            set
            {
                otherHeaderFontSize = value;
                OnPropertyChanged("OtherHeaderFontSize");
            }
        }

        private string otherHeaderFontStyle;
        public string OtherHeaderFontStyle
        {
            get { return otherHeaderFontStyle; }
            set
            {
                otherHeaderFontStyle = value;
                OnPropertyChanged("OtherHeaderFontStyle");
                OnPropertyChanged("OtherHeaderFontStyleUI");
            }
        }

        private string otherHeaderFontWeight;
        public string OtherHeaderFontWeight
        {
            get { return otherHeaderFontWeight; }
            set
            {
                otherHeaderFontWeight = value;
                OnPropertyChanged("OtherHeaderFontWeight");
                OnPropertyChanged("OtherHeaderFontWeightUI");
            }
        }

        private Color otherCellBorder;
        public Color OtherCellBorder
        {
            get { return otherCellBorder; }
            set
            {
                otherCellBorder = value;
                OnPropertyChanged("OtherCellBorderBrush");
                OnPropertyChanged("OtherCellBorder");
            }
        }

        private Color otherOddColor;
        public Color OtherOddColor
        {
            get { return otherOddColor; }
            set
            {
                otherOddColor = value;
                OnPropertyChanged("OtherOddColor");
                OnPropertyChanged("OtherOddColorBrush");
            }
        }

        private Color otherEvenColor;
        public Color OtherEvenColor
        {
            get { return otherEvenColor; }
            set
            {
                otherEvenColor = value;
                OnPropertyChanged("OtherEvenColor");
                OnPropertyChanged("OtherEvenColorBrush");
            }
        }


        #endregion

        public void InitValueFromCommonStaticMemeber()
        {
            DeepCopy(CommonStaticMemeber.CurrentClientStyleUI);
        }

        public void DeepCopy(TradeClientStyle style)
        {
            this.Cb_consultIndex = style.Cb_consultIndex;
            this.ChangeColor = style.ChangeColor;
            this.UpColor = style.UpColor;
            this.DownColor = style.DownColor;
            this.LabelHQContentForeground = style.LabelHQContentForeground;
            this.LabelHQContentBackground = style.LabelHQContentBackground;
            this.LabelHQHeaderForeground = style.LabelHQHeaderForeground;
            this.LabelHQHeaderBackground = style.LabelHQHeaderBackground;
            this.LabelHQSelectedForeground = style.LabelHQSelectedForeground;
            this.LabelHQSelectedBackground = style.LabelHQSelectedBackground;
            this.HQOddColor = style.HQOddColor;
            this.HQEvenColor = style.HQEvenColor;
            this.CodeColor = style.CodeColor;

            this.HQFontFamily = style.HQFontFamily;
            this.HQFontSize = style.HQFontSize;
            this.HQFontStyle = style.HQFontStyle;
            this.HQFontWeight = style.HQFontWeight;

            this.HQHeaderFontFamily = style.HQHeaderFontFamily;
            this.HQHeaderFontSize = style.HQHeaderFontSize;
            this.HQHeaderFontStyle = style.HQHeaderFontStyle;
            this.HQHeaderFontWeight = style.HQHeaderFontWeight;


            this.LabelOtherContentForeground = style.LabelOtherContentForeground;
            this.LabelOtherContentBackground = style.LabelOtherContentBackground;
            this.LabelOtherHeaderForeground = style.LabelOtherHeaderForeground;
            this.LabelOtherHeaderBackground = style.LabelOtherHeaderBackground;
            this.LabelOtherSelectedForeground = style.LabelOtherSelectedForeground;
            this.LabelOtherSelectedBackground = style.LabelOtherSelectedBackground;
            this.OtherOddColor = style.OtherOddColor;
            this.OtherEvenColor = style.OtherEvenColor;

            this.OtherFontFamily = style.OtherFontFamily;
            this.OtherFontSize = style.OtherFontSize;
            this.OtherFontStyle = style.OtherFontStyle;
            this.OtherFontWeight = style.OtherFontWeight;

            this.OtherHeaderFontFamily = style.OtherHeaderFontFamily;
            this.OtherHeaderFontSize = style.OtherHeaderFontSize;
            this.OtherHeaderFontStyle = style.OtherHeaderFontStyle;
            this.OtherHeaderFontWeight = style.OtherHeaderFontWeight;

            this.HQRowHeight = style.HQRowHeight;
            this.OtherRowHeight = style.OtherRowHeight;

            this.OtherCellBorder = style.OtherCellBorder;
        }

        public void UpdateToCommonStaticMemeber()
        {
            CommonStaticMemeber.CurrentClientStyleUI.DeepCopy(this);
        }
    }
}
