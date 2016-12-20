using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TradingMaster.Control
{
    /// <summary>
    /// BankAmounttQry.xaml 的交互逻辑
    /// </summary>
    public partial class BankAmountQry : UserControl
    {
        private MainWindow _MainWindow { get; set; }

        static readonly string _NumberFormat = "#,##0.00";
        static readonly int _PadLeftNumber = 24;
        //static readonly int padRightNumber = 18;
        static readonly int _EqualMarkPadNumber = 46;
        //static readonly int minusMarkPadNumber = 45;

        public BankAmountQry(BankAcctDetail bankAcctDetail)
        {
            InitializeComponent();
            SetBankAmountDetails(bankAcctDetail);
        }

        public void Init(MainWindow parent)
        {
            this._MainWindow = parent;
        }

        public void SetBankAmountDetails(BankAcctDetail bankAcctDetail)
        {
            StringBuilder catpitalInfo = new StringBuilder();

            catpitalInfo.AppendLine("".PadRight(_EqualMarkPadNumber, '='));
            AddLine("银行账号：", bankAcctDetail.BankAccount, catpitalInfo, 6);
            AddLine("银行可用余额：", bankAcctDetail.BankUseAmount, catpitalInfo, 4);
            AddLine("银行可取余额：", bankAcctDetail.BankFetchAmount, catpitalInfo, 4);
            AddLine("币种：", bankAcctDetail.CurrencyID, catpitalInfo, 8);
            catpitalInfo.AppendLine("".PadRight(_EqualMarkPadNumber, '='));

            tb_AmountDetails.Text = catpitalInfo.ToString();
        }

        private void AddLine(string header, double value, StringBuilder catpitalInfo, int padRightNumber)
        {
            string newLine = header;
            for (int i = 0; i < padRightNumber; i++)
            {
                newLine = newLine + "　";
            }
            string numberValue = value.ToString(_NumberFormat);
            newLine = newLine + numberValue.PadLeft(_PadLeftNumber);
            catpitalInfo.AppendLine(newLine);
        }

        private void AddLine(string header, string value, StringBuilder catpitalInfo, int padRightNumber)
        {
            string newLine = header;
            for (int i = 0; i < padRightNumber; i++)
            {
                newLine = newLine + "　";
            }
            string numberValue = value;
            newLine = newLine + numberValue.PadLeft(_PadLeftNumber);
            catpitalInfo.AppendLine(newLine);
        }

        private void BtnAmtOK_Click(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Window)
            {
                (this.Parent as Window).Hide();
            }
        }
    }
}
