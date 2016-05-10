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
using TradingMaster.CodeSet;

namespace TradingMaster.Control
{
    /// <summary>
    /// OptionExecPanel.xaml 的交互逻辑
    /// </summary>
    public partial class OptionExecPanel : UserControl
    {
        public OptionExecPanel()
        {
            InitializeComponent();
        }

        public void Init(MainWindow parent)
        {
            this._MainWindow = parent;
        }

        private MainWindow _MainWindow { get; set; }

        public TradeClientStyleUI CurrentClientStyleUI
        {
            get { return CommonStaticMemeber.CurrentClientStyleUI; }
        }

        private void txtCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Util.Log("txtCode_TextChanged starts.");
            txtCode.SelectionStart = txtCode.Text.Length;
            int firstNumberIndiex = txtCode.Text.IndexOfAny("0123456789".ToCharArray());
            string SpeciesName = txtCode.Text;
            string SpeciesDate = string.Empty;
            if (firstNumberIndiex >= 0)
            {
                SpeciesName = txtCode.Text.Substring(0, firstNumberIndiex);
                SpeciesDate = txtCode.Text.Substring(firstNumberIndiex, txtCode.Text.Length - firstNumberIndiex);
                //}

                //用户可能不区分大小写输入合约
                string validSpeciesName = CodeSetManager.GetValidSpeciesName(SpeciesName);
                if (validSpeciesName != null && validSpeciesName != SpeciesName)
                {
                    txtCode.Text = validSpeciesName + SpeciesDate;
                    return;
                }
            }
            if (CommonUtil.IsValidCode(txtCode.Text.Trim()))
            {
                Contract newCode = CodeSetManager.GetContractInfo(txtCode.Text.Trim());
                txtCodeName.Text = newCode.Name;

                //iudNum.Value = DefaultCodeHandInstance.GetDefaultCodeHand(txtCode.Text);
            }      
        }

        private void rbExec_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                rbAbandon.Focus();
                rbAbandon.IsChecked = true;
                rbExec.IsChecked = false;
            }
            e.Handled = true;
        }

        private void rbAbandon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                rbExec.Focus();
                rbExec.IsChecked = true;
                rbAbandon.IsChecked = false;
            }
            e.Handled = true;
        }

        public void SetExecOrderInfoByPositionData(string buyOrSell, string kp, int num, RealData realData)
        {
            if (buyOrSell == "卖" && realData.CodeInfo != null && realData.CodeInfo.ProductType.Contains("Option") )
            {
                if (realData.CodeInfo != null && realData.CodeInfo.Code != null)
                {
                    if (kp == "平仓")
                    {
                        rbPingcang.IsChecked = true;
                    }
                    else if (kp == "平今")
                    {
                        rbPingjin.IsChecked = true;
                    }

                    txtCode.Text = realData.CodeInfo.Code;
                    iudNum.Value = num;
                    SetHandCount("买");
                }
                else
                {
                    Util.Log("Warning! realData.CodeInfo or its code is null!");
                }
                
            }
        }

        public void SetHandCount(string buySell)
        {
            tbHandCount.Text = "0";
            //if (rbPingcang.IsChecked == true)
            //{
            //    foreach (var item in _MainWindow.PositionCollection_Total)
            //    {
            //        if (txtCode.Text == item.Code && item.BuySell.Contains(buySell))
            //        {
            //            if (CodeSetManager.IsCloseTodaySupport(item.Code))
            //            {
            //                tbHandCount.Text = item.YesterdayPosition.ToString(); //(item.YesterdayPosition - item.FreezeCount).ToString();
            //            }
            //            else
            //            {
            //                tbHandCount.Text = item.TotalPosition.ToString(); //(item.TotalPosition - item.FreezeCount).ToString();
            //            }
            //        }
            //    }
            //}
            //else if (rbPingjin.IsChecked == true)
            {
                foreach (var item in _MainWindow.PositionCollection_Total)
                {
                    if (txtCode.Text == item.Code && item.BuySell.Contains(buySell))
                    {
                        if (CodeSetManager.IsCloseTodaySupport(item.Code))
                        {
                            tbHandCount.Text = item.TodayPosition.ToString(); //(item.TodayPosition - item.FreezeCount).ToString();
                        }
                        else
                        {
                            tbHandCount.Text = item.TotalPosition.ToString(); //(item.TotalPosition - item.FreezeCount).ToString();
                        }
                    }
                }
            }
        }

        private void Exec_Click(object sender, RoutedEventArgs e)
        {
            string verifyMessage = execOrderDataVerification();
            if (verifyMessage == string.Empty)
            {
                Contract optContract = CodeSetManager.GetContractInfo(txtCode.Text.Trim());
                if (optContract == null)
                {
                    Util.Log("Warning! Cannot find contract info for " + txtCode.Text.Trim());
                    return;
                }
                bool isExec = rbExec.IsChecked == true ? true : false;
                string order = rbExec.IsChecked == true ? "行权" : "弃权";
                string strCode = optContract.Code;
                int handCount = CommonUtil.GetIntValue(iudNum.Value.Value);
                EnumThostOffsetFlagType openClose = EnumThostOffsetFlagType.Close;
                string kp = "平仓";
                if (CodeSetManager.IsCloseTodaySupport(optContract.Code))
                {
                    if (rbPingjin.IsChecked == true)
                    {
                        openClose = EnumThostOffsetFlagType.CloseToday;
                        kp = "平今";
                    }

                    else if (rbPingcang.IsChecked == true)
                    {
                        openClose = EnumThostOffsetFlagType.CloseYesterday;
                        kp = "平仓";
                    }
                }
                else if (rbPingcang.IsChecked == true)
                {
                    openClose = EnumThostOffsetFlagType.Close;
                }

                if (TradingMaster.Properties.Settings.Default.ConfirmBeforeSendNewOrder == true)
                {
                    CheckableMessageBox messageBox = new CheckableMessageBox();
                    messageBox.tbMessage.Text = string.Format("{0}：{1} {2} {3}手", order, kp, strCode, handCount);
                    string windowTitle = string.Format("确认{0}：{1} {2} {3}手", order, kp, strCode, handCount);
                    Window confirmWindow = CommonUtil.GetWindow(windowTitle, messageBox, _MainWindow);

                    if (confirmWindow.ShowDialog() == true)
                    {
                        if (handCount > 0)
                        {
                            TradeDataClient.GetClientInstance().RequestOrder("", BACKENDTYPE.CTP, new RequestContent("NewExecOrder", new List<object>() { strCode, handCount, optContract.ExchName, isExec, openClose }));
                        }
                    }
                }
                else
                {
                    if (!CodeSetManager.IsCloseTodaySupport(strCode) && openClose == EnumThostOffsetFlagType.CloseToday)
                    {
                        openClose = EnumThostOffsetFlagType.Close;
                    }
                    if (handCount > 0)
                    {
                        TradeDataClient.GetClientInstance().RequestOrder("", BACKENDTYPE.CTP, new RequestContent("NewExecOrder", new List<object>() { strCode, handCount, optContract.ExchName, isExec, openClose }));
                    }
                }
            }
        }

        private string execOrderDataVerification()
        {
            string verifyMessage = string.Empty;

            if (txtCode.Text == "")
            {
                verifyMessage = verifyMessage + "合约代码不能为空!\n";
            }
            else if (!CommonUtil.IsValidCode(txtCode.Text))
            {
                verifyMessage = verifyMessage + "合约代码不合法!\n";
            }
            else if (iudNum.Value == null || iudNum.Value.Value == 0)
            {
                verifyMessage = verifyMessage + "委托数量不能为空或为0!\n";
            }
            return verifyMessage;
        }

        private void rbPingjin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                rbPingcang.Focus();
                rbPingcang.IsChecked = true;
                rbPingjin.IsChecked = false;
            }
            e.Handled = true;
        }

        private void rbPingcang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                rbPingjin.Focus();
                rbPingjin.IsChecked = true;
                rbPingcang.IsChecked = false;
            }
            e.Handled = true;
        }
    }
}
