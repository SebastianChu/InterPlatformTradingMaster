using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TradingMaster.Control
{
    /// <summary>
    /// 由于接口原因，本界面和Q7不同，不显示交割保证金
    /// </summary>
    public partial class CapitalDetail : UserControl
    {
        static readonly string _NumberFormat = "#,##0.00";
        static readonly int _PadLeftNumber = 24;
        //static readonly int padRightNumber = 18;
        static readonly int _EqualMarkPadNumber = 45;
        static readonly int _MinusMarkPadNumber = 45;


        public CapitalDetail()
        {
            InitializeComponent();
            //txtCapital.Language
        }

        public TradeClientStyleUI CurrentClientStyleUI
        {
            get { return CommonStaticMemeber.CurrentClientStyleUI; }
        }

        public void SetJYRealData(JYRealData jyRealData)
        {
            StringBuilder catpitalInfo = new StringBuilder();
            catpitalInfo.AppendLine("".PadRight(_EqualMarkPadNumber, '='));

            AddLine("　上次结算准备金：", jyRealData.YesterdayEquity, catpitalInfo, 0);
            AddLine("－上次信用额度：", jyRealData.LastCredit, catpitalInfo, 1);
            AddLine("－上次质押额度：", jyRealData.LastMortage, catpitalInfo, 1);
            AddLine("＋质押金额：", jyRealData.Mortage, catpitalInfo, 3);
            AddLine("－今日出金：", jyRealData.OutMoney, catpitalInfo, 3);
            AddLine("＋今日入金：", jyRealData.InMoney, catpitalInfo, 3);

            catpitalInfo.AppendLine("".PadRight(_MinusMarkPadNumber, '-'));

            AddLine("＝静态权益：", jyRealData.StaticEquity, catpitalInfo, 3);
            AddLine("＋平仓盈亏：", jyRealData.Dspy, catpitalInfo, 3);
            AddLine("＋持仓盈亏：", jyRealData.Dsfy, catpitalInfo, 3);
            AddLine("＋权利金：", jyRealData.Royalty, catpitalInfo, 4);
            AddLine("－手续费：", jyRealData.Charge, catpitalInfo, 4);
            catpitalInfo.AppendLine("".PadRight(_MinusMarkPadNumber, '-'));

            AddLine("＝动态权益：", jyRealData.DynamicEquity, catpitalInfo, 3);
            AddLine("－占用保证金：", jyRealData.Bond, catpitalInfo, 2);
            AddLine("－冻结保证金：", jyRealData.FrozenMargin, catpitalInfo, 2);
            AddLine("－冻结手续费：", jyRealData.FrozenCommision, catpitalInfo, 2);
            AddLine("－交割保证金：", jyRealData.DeliveryMargin, catpitalInfo, 2);
            AddLine("－冻结权利金：", jyRealData.FrozenRoyalty, catpitalInfo, 2);
            AddLine("＋信用金额：", jyRealData.Credit, catpitalInfo, 3);
            ///只有当持仓盈利的时候，不能用做可用。即浮盈不能开新仓，对于上海辖区。
            AddLine("－持仓盈利：", jyRealData.Dsfy > 0 ? jyRealData.Dsfy : 0, catpitalInfo, 3);
            catpitalInfo.AppendLine("".PadRight(_MinusMarkPadNumber, '-'));

            AddLine("＝可用资金：", jyRealData.CaculatedAvailable, catpitalInfo, 3);

            catpitalInfo.AppendLine("".PadRight(_EqualMarkPadNumber, '='));

            AddLine("　保底资金：", jyRealData.Reserve, catpitalInfo, 3);
            AddLine("　可取资金：", jyRealData.Fetchable, catpitalInfo, 3);       
            catpitalInfo.AppendLine("".PadRight(_EqualMarkPadNumber, '='));

            //AddLine("　权利金收付：", jyRealData.Premium, catpitalInfo, 2);
            //AddLine("＋期权市值：", jyRealData.OptionMarketCap, catpitalInfo, 3);
            //catpitalInfo.AppendLine("".PadRight(minusMarkPadNumber, '-'));
            //AddLine("　期权盈亏：", jyRealData.OptionProfit, catpitalInfo, 3);
            //catpitalInfo.AppendLine("".PadRight(equalMarkPadNumber, '='));

            AddLine("　期权市值：", jyRealData.OptionMarketCap, catpitalInfo, 3);
            AddLine("＋动态权益：", jyRealData.DynamicEquity, catpitalInfo, 3);            
            catpitalInfo.AppendLine("".PadRight(_MinusMarkPadNumber, '-'));
            AddLine("＝账户市值：", jyRealData.AccountCap, catpitalInfo, 3);
            catpitalInfo.AppendLine("".PadRight(_EqualMarkPadNumber, '='));

            txtCapital.Text = catpitalInfo.ToString();
        }

        private void AddLine(string header, double value, StringBuilder catpitalInfo, int padRightNumber)
        {
            string newLine = header;
            for (int i = 0; i < padRightNumber; i++)
            {
                newLine = newLine + "　";
            }
            string numberValue = value.ToString(_NumberFormat);

            //int padLeft = padLeftNumber - numberValue.Length;
            //newLine = newLine + numberValue.PadLeft(padLeft);
            newLine = newLine + numberValue.PadLeft(_PadLeftNumber);
            catpitalInfo.AppendLine(newLine);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Window)
            {
                (this.Parent as Window).Close();
            }
        }

        //private void btnOk2_Click(object sender, RoutedEventArgs e)
        //{
        //    System.Windows.Forms.FontDialog fontDialog = new System.Windows.Forms.FontDialog();

        //    if (fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        txtCapital.FontFamily = new FontFamily(fontDialog.Font.Name);
        //        txtCapital.FontSize = fontDialog.Font.Size;
        //        txtCapital.FontWeight = fontDialog.Font.Bold ? FontWeights.Bold : FontWeights.Normal;
        //        txtCapital.FontStyle = fontDialog.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
        //    }
        //}

    }
}
