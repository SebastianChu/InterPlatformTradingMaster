using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using TradingMaster.CodeSet;

namespace TradingMaster.Control
{
    /// <summary>
    /// FuturesQuotes.xaml 的交互逻辑
    /// </summary>
    public partial class FuturesQuotes : UserControl
    {
        public FuturesQuotes()
        {
            InitializeComponent();
        }

        public DataGridCell DgCell;
        public BackgroundDataServer HQRealData;
        public BackgroundDataServer GroupHQRealData;
        private MainWindow _MainWindow { get; set; }
        private ObservableCollection<Contract> _CodeArray = new ObservableCollection<Contract>();
        private ObservableCollection<Contract> _GroupCodeArray = new ObservableCollection<Contract>();
        private List<Contract> _AddCodeList = new List<Contract>();        
        private List<Contract> _DelCodeList = new List<Contract>();
        private Dictionary<string, RealData> _BackupCodeDic = new Dictionary<string, RealData>();

        private ObservableCollection<Contract> _UserCodes = new ObservableCollection<Contract>();
        private ObservableCollection<Contract> _OldCodeArray = new ObservableCollection<Contract>();
        private ObservableCollection<Contract> _GroupOldCodeArray = new ObservableCollection<Contract>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buyOrSell">三种值，""/"卖"/"买"，分别代表要下单、下卖单或者下买单</param>
        /// <param name="realData"></param>
        public delegate void RealDataMouseDoubleClickDelegate(object sender, MouseButtonEventArgs e);
        public delegate void RealDataMouseLeftButtonDownDelegate(string buyOrSell, RealData realData, double sJkpPrice, double sZjsPrice,
                         double fixPrice, double sNewPrice, double sHighPrice, double sLowPrice, double sZspPrice, double sJspPrice, double sJsjPrice, double sDrjjPrice, bool isBuySell);

        public event RealDataMouseDoubleClickDelegate RealDataMouseDoubleClicked;
        public event RealDataMouseLeftButtonDownDelegate RealDataMouseLeftButtonDown;

        /// <summary>
        /// 当前合约组
        /// </summary>
        private Button defaultButton;
                                
        public void Init(MainWindow parent)
        {
            defaultButton = null;
            this.DataContext = parent;
            this._MainWindow = parent;
            dgRealData.DataContext = parent;



            HQRealData = new BackgroundDataServer(_MainWindow.RealDataCollection, _BackupCodeDic, _DelCodeList, _CodeArray, _OldCodeArray);
            GroupHQRealData = new BackgroundDataServer(_MainWindow.RealDataArbitrageCollection, _BackupCodeDic, _DelCodeList, _GroupCodeArray, _GroupOldCodeArray);
            InitHQData();

            //DataGridColumnBridgeUtils.LoadColumnsSettingFromFile(dgRealData, MainWindow.SettingDictionaryPath);
            dgRealData.SelectionChanged += new SelectionChangedEventHandler(grdRealData_SelectionChanged);
            dgGroupRealData.SelectionChanged += new SelectionChangedEventHandler(dgGroupRealData_SelectionChanged);

            LvQuotesPanel.Init(parent);

            ResetBlockButtons();
        }

        void grdRealData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //将不需要的行情去除掉
            if (dgRealData.SelectedItem != null)
            {
                DisplayRealData selectedRecord = dgRealData.SelectedItem as DisplayRealData;
                LvQuotesPanel.lblCode.Content = selectedRecord.Code;

                //RealData selectedData = new RealData();
                string tempKey = selectedRecord.Code + "_" + CodeSetManager.ExNameToCtp(selectedRecord.Market);
                if (_BackupCodeDic.ContainsKey(tempKey))
                {
                    LvQuotesPanel.SetLevelsQuotesByRealData(_BackupCodeDic[tempKey]);
                }

                //List<DisplayRealData> uselessDatas = new List<DisplayRealData>();
                //foreach (var item in mainWindow.RealDataCollection)
                //{
                //    if (!HQRealData.commObj.RequestingCodes.Contains(item.Code)
                //        && item.Code != mainWindow.uscNewOrderPanel.txtCode.Text.Trim()
                //        && item != selectedRecord)
                //    {
                //        uselessDatas.Add(item);
                //    }
                //}
                //foreach (var item in uselessDatas)
                //{
                //    mainWindow.RealDataCollection.Remove(item);
                //}
            }
        }

        void dgGroupRealData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //将不需要的行情去除掉
            if (dgGroupRealData.SelectedItem != null)
            {
                DisplayRealData selectedRecord = dgGroupRealData.SelectedItem as DisplayRealData;
                LvQuotesPanel.lblCode.Content = selectedRecord.Code;

                //RealData selectedData = new RealData();
                string tempKey = selectedRecord.Code + "_" + CodeSetManager.ExNameToCtp(selectedRecord.Market);
                if (_BackupCodeDic.ContainsKey(tempKey))
                {
                    LvQuotesPanel.SetLevelsQuotesByRealData(_BackupCodeDic[tempKey]);
                }

                //List<DisplayRealData> uselessDatas = new List<DisplayRealData>();
                //foreach (var item in mainWindow.RealDataArbitrageCollection)
                //{
                //    if (!GroupHQRealData.commObj.RequestingCodes.Contains(item.Code)
                //        && item.Code != mainWindow.uscNewOrderPanel.txtCode.Text.Trim()
                //        && item != selectedRecord)
                //    {
                //        uselessDatas.Add(item);
                //    }
                //}
                //foreach (var item in uselessDatas)
                //{
                //    mainWindow.RealDataArbitrageCollection.Remove(item);
                //}
            }
        }

        private void InitHQData()
        {
            
        }


        /// <summary>
        /// 显示自选行情设置窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowContract_Click(object sender, RoutedEventArgs e)
        {
            ChooseContract chooseContract = new ChooseContract();
            chooseContract.DataContext = this._MainWindow;
            Window codeSetWindow = CommonUtil.GetWindow("合约设置", chooseContract, _MainWindow);
            codeSetWindow.Closed += new EventHandler(codeSetWindow_Closed);
            codeSetWindow.ShowDialog();

        }

        void codeSetWindow_Closed(object sender, EventArgs e)
        {
            ChooseContract chooseContract = ((sender as Window).Content) as ChooseContract;
            if (chooseContract.IsUpdated == false)
            {
                UserCodeSetInstance.Reload();
            }
        }

        private void UserCodesChanged()
        {
            _UserCodes.Clear();
            foreach (var item in _CodeArray)
            {
                _UserCodes.Add(item);
            }
            //SaveUserCodes();
            //if (tabControl1.SelectedItem == tbiHQUser)
            //{
            //    RequestRealData();
            //}
        }


        private Boolean isZlCodeSetNow = false;

        /// <summary>
        /// 切换合约组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ChangeUserCodeSet(Boolean isZL,ObservableCollection<Contract> newCodes)
        {
            //this.tabControl1.IsEnabled = false;
            //ObservableCollection<Contract> newCodes = userCodes;
            isZlCodeSetNow = isZL;
            _CodeArray.Clear();

            foreach (var item in newCodes)
            {
                _CodeArray.Add(item);
            }

            
            this._MainWindow.MainWindow_ChangeUserCodeEvent(_CodeArray);
            RequestRealData(HQRealData);
        }

        public void ChangeUserGroupCodeSet(ObservableCollection<Contract> newCodes)
        {
            _GroupCodeArray.Clear();

            foreach (var item in newCodes)
            {
                _GroupCodeArray.Add(item);
            }

            RequestRealData(GroupHQRealData);
        }

        public void UnRequestRealData()
        {
            _CodeArray.Clear();
            RequestRealData(HQRealData);
            _GroupCodeArray.Clear();
            RequestRealData(GroupHQRealData);
        }

        private void RequestRealData(BackgroundDataServer hqRealData)
        {
            ObservableCollection<UserCodeSet> lstUserCodeSet = UserCodeSetInstance.GetUserCodeSetList();
            //先删除合约
            bool bChanged = false;
            _DelCodeList.Clear();
            foreach (Contract temp in hqRealData.m_oldCodeArray)
            {
                if (!hqRealData.m_codeArray.Contains(temp))
                {
                    _DelCodeList.Add(temp);
                }
            }
            if (_DelCodeList.Count > 0)
            {
                bChanged = true;
                hqRealData.AddContract(_DelCodeList.ToArray());
                hqRealData.UnRequest();
                try
                {
                    RealData tempData = null;
                    foreach (Contract temp in _DelCodeList)
                    {
                        string tempKey = temp.Code + "_" + temp.ExchCode;
                        if (_BackupCodeDic.ContainsKey(tempKey))
                        {
                            tempData = _BackupCodeDic[tempKey];
                            if (tempData != null)
                            {
                                DisplayRealData tempDisplayData = UpdateDisplayProperties(tempData);
                                if (hqRealData == HQRealData)
                                {
                                    //Util.Log("<<:" + tempData.ToString());
                                    _MainWindow.RealDataCollection.Remove(tempDisplayData);
                                }
                                else if (hqRealData == GroupHQRealData)
                                {
                                    _MainWindow.RealDataArbitrageCollection.Remove(tempDisplayData);
                                }
                                _BackupCodeDic.Remove(tempKey);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Util.Log_Error("exception: " + ex.Message);
                    Util.Log_Error("exception: " + ex.StackTrace);
                }
            }
            //在请求新增的合约
            _AddCodeList.Clear();
            foreach (Contract temp in hqRealData.m_codeArray)
            {
                if (!hqRealData.m_oldCodeArray.Contains(temp))
                {
                    _AddCodeList.Add(temp);
                }
            }

            if (_AddCodeList.Count > 0)
            {
                foreach (Contract temp in _AddCodeList)
                {
                    if (temp == null) continue; 
                    string tempKey = temp.Code + "_" + temp.ExchCode;
                    if (_BackupCodeDic.ContainsKey(tempKey) == false)
                    {
                        RealData tempData2 = new RealData();
                        //Contract tempContract = CodeSetManager.GetContractInfo(temp.Code);
                        tempData2.CodeInfo = CodeSetManager.GetContractInfo(temp.Code, temp.ExchCode);
                        if (tempData2.CodeInfo == null)
                        {
                            tempData2.CodeInfo = new Contract(temp.Code);
                        }
                        //tempData2.Name = tempContract.Name;
                        //decimal fluct = 0.00M;
                        //CodeSetManager.GetHycsAndFluct(tempData2.codeInfo.Code, out (double)tempData2.m_hycs, out fluct);
                        _BackupCodeDic.Add(tempKey, tempData2);
                    }
                }
                bChanged = true;
                hqRealData.AddContract(_AddCodeList.ToArray());
                hqRealData.GetRealData();
                hqRealData.Request();
            }

            DeepCopyInfo(hqRealData.m_codeArray, hqRealData.m_oldCodeArray);

            if (bChanged == false)
            {
                RealData tempData2 = null;
                _MainWindow.RealDataCollection.Clear();
                _MainWindow.RealDataArbitrageCollection.Clear();
                foreach (Contract temp in hqRealData.m_oldCodeArray)
                {
                    //Util.Log(this.defaultButton.Content + " 添加内容 Old-----");
                    string tempKey = temp.Code + "_" + temp.ExchCode;
                    if (_BackupCodeDic.ContainsKey(tempKey))
                    {
                        tempData2 = _BackupCodeDic[tempKey];
                        if (tempData2 != null)
                        {
                            DisplayRealData tempDisplayData = UpdateDisplayProperties(tempData2);
                            if (hqRealData == HQRealData)
                            {
                                //Util.Log(">>:" + tempData2.ToString());
                                _MainWindow.RealDataCollection.Add(tempDisplayData);
                            }
                            else if (hqRealData == GroupHQRealData)
                            {
                                if (tempData2.CodeInfo.Code.Contains("&"))
                                {
                                    //string[] tempType = tempData2.codeInfo.Code.Split(' ');
                                    //string[] tempGroup = tempType[1].Split('&');
                                    //tempData2.Code = tempGroup[0];
                                    //tempData2.Code2 = tempGroup[1];
                                }
                                _MainWindow.RealDataArbitrageCollection.Add(UpdateDisplayProperties(tempData2));
                            }
                        }
                    }
                }
            }
            else
            {
                RealData tempData2 = null;
                _MainWindow.RealDataCollection.Clear();
                _MainWindow.RealDataArbitrageCollection.Clear();
                if (hqRealData == HQRealData)
                {
                    //Util.Log(this.defaultButton.Content + " 添加内容-----");
                    foreach (Contract temp in _CodeArray)
                    {
                        if (temp != null)
                        {
                            string tempKey = temp.Code + "_" + temp.ExchCode;
                            if (_BackupCodeDic.ContainsKey(tempKey) == false)
                            {
                                tempData2 = new RealData();
                                //Contract tempContract = CodeSetManager.GetContractInfo(temp.Code);
                                tempData2.CodeInfo = CodeSetManager.GetContractInfo(temp.Code, temp.ExchCode);
                                if (tempData2.CodeInfo == null)
                                {
                                    tempData2.CodeInfo = new Contract(temp.Code);
                                }
                                //tempData2.Name = tempContract.Name;
                                //double fluct = 0;
                                //CodeSetManager.GetHycsAndFluct(tempData2.Code, out tempData2.m_hycs, out fluct);                                
                                _BackupCodeDic.Add(tempKey, tempData2);
                            }
                            else
                            {
                                tempData2 = _BackupCodeDic[tempKey];
                            }
                            DisplayRealData tempDisplayData = UpdateDisplayProperties(tempData2);
                            //Util.Log(">>:"+tempData2.ToString());
                            _MainWindow.RealDataCollection.Add(tempDisplayData);
                            //Util.Log("mainWindow.RealDataCollection.count=" + mainWindow.RealDataCollection.Count);
                        }
                    }
                }

                else if (hqRealData == GroupHQRealData)
                {
                    foreach (Contract temp in _GroupCodeArray)
                    {
                        if (temp != null)
                        {
                            string tempKey = temp.Code + "_" + temp.ExchCode;
                            if (_BackupCodeDic.ContainsKey(tempKey) == false)
                            {
                                tempData2 = new RealData();
                                tempData2.CodeInfo = CodeSetManager.GetContractInfo(temp.Code, temp.ExchCode);
                                if (tempData2.CodeInfo == null)
                                {
                                    tempData2.CodeInfo = new Contract(tempKey);
                                }
                                //decimal fluct = 0;
                                //CodeSetManager.GetHycsAndFluct(temp.Code, out tempData2.m_hycs, out fluct);
                                _BackupCodeDic.Add(tempKey, tempData2);
                            }
                            else
                            {
                                tempData2 = _BackupCodeDic[tempKey];
                            }
                            if (tempData2.CodeInfo.Code.Contains("&"))
                            {
                                //string[] tempType = tempData2.Code.Split(' ');
                                //string[] tempGroup = tempType[1].Split('&');
                                //tempData2.Code1 = tempGroup[0];
                                //tempData2.Code2 = tempGroup[1];
                            }
                            _MainWindow.RealDataArbitrageCollection.Add(UpdateDisplayProperties(tempData2));
                        }
                    }
                }
            }
            hqRealData.UpdateRealDataList();
        }


        private void DeepCopyInfo(ObservableCollection<Contract> srcCodeArray, ObservableCollection<Contract> desCodeArray)
        {
            if (srcCodeArray.Count == 0)
            {
                desCodeArray.Clear();
            }
            else
            {
                desCodeArray.Clear();
                foreach (Contract temp in srcCodeArray)
                {
                    desCodeArray.Add(temp);
                }
            }
        }

        /// <summary>
        /// 用户双击行情表格，通过MouseDoubleClicked实现对外处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdRealData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RealDataMouseDoubleClicked != null)
            {
                DataGridCell cell = CommonUtil.GetClickedCell(e);
                if (cell == null
                    || !(cell.Column == colBuyPrice || cell.Column == colBuyCount
                    || cell.Column == colSellPrice || cell.Column == colSellCount || cell.Column == colContract || cell.Column == colContractName))
                {
                    return;
                }
                RealDataMouseDoubleClicked(sender, e);
            }
        }

        private void grdGroupRealData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RealDataMouseDoubleClicked != null)
            {
                DataGridCell cell = CommonUtil.GetClickedCell(e);
                if (cell == null
                    || !(cell.Column == colGroupBuyPrice || cell.Column == colGroupBuyCount
                    || cell.Column == colGroupSellPrice || cell.Column == colGroupSellCount || cell.Column == colFirstContract || cell.Column == colSecondContract || cell.Column == colGroupContractName))
                {
                    return;
                }
                RealDataMouseDoubleClicked(sender, e);
            }
        }

        private void miExportData_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.ExportedData_Click(sender, e);
        }

        private void miAutoAdjustColumnWidth_Click(object sender, RoutedEventArgs e)
        {
            CommonUtil.AutoAdjustColumnWidth_Click(sender, e);
        }


        private void grdRealData_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DgCell = CommonUtil.GetClickedCell(e);
            if (DgCell == null)
            {
                return;
            }
            DisplayRealData record = DgCell.DataContext as DisplayRealData;
            GridHangQingData(record);

            DataGrid dg = sender as DataGrid;
            if (dg != null)
            {
                //dg.SelectedItems.Clear();
                //dg.SelectedItems.Add(record);
                dg.SelectedItem = record;
            }
        }

        private void GridHangQingData(DisplayRealData record)
        {
            string buySell = "";
            Boolean isBuySell = false;
            if (DgCell.Column == colBuyPrice || DgCell.Column == colBuyCount || DgCell.Column == colGroupBuyPrice || DgCell.Column == colGroupBuyCount)
            {
                buySell = "卖";
                isBuySell = true;
            }
            else if (DgCell.Column == colSellPrice || DgCell.Column == colSellCount || DgCell.Column == colGroupSellPrice || DgCell.Column == colGroupSellCount)
            {
                buySell = "买";
                isBuySell = true;
            }

            //点击最新价
            double sNewPrice = 0;
            if (DgCell.Column == colINewPrice || DgCell.Column == colGroupNewPrice)
            {
                sNewPrice = record.INewPrice;
            }

            //点击涨停价和跌停价时使用指定价下单
            double fixPrice = 0;
            if (DgCell.Column == colMaxPrice || DgCell.Column == colGroupMaxPrice)
            {
                buySell = "买";
                fixPrice = record.UpStopPrice;
            }
            else if (DgCell.Column == colMinPrice || DgCell.Column == colGroupMinPrice)
            {
                buySell = "卖";
                fixPrice = record.DownStopPrice;
            }
            //今开盘和昨结算
            double sJkpPrice = 0;
            double sZjsPrice = 0;
            if (DgCell.Column == colJkpPrice || DgCell.Column == colGroupJkpPrice)
            {
                sJkpPrice = record.Open;
            }
            else if (DgCell.Column == colZjsPrice || DgCell.Column == colGroupZjsPrice)
            {
                sZjsPrice = record.PrevSettleMent;
            }
            //点击最高价和最低价显示指定价
            double sHighPrice = 0;
            double sLowPrice = 0;
            if (DgCell.Column == colHightPrice || DgCell.Column == colGroupHightPrice)
            {
                sHighPrice = record.IMaxPrice;
            }
            else if (DgCell.Column == colLowPrice || DgCell.Column == colGroupLowPrice)
            {
                sLowPrice = record.IMinPrice;
            }
            //点击昨收盘和今收盘
            double sZspPrice = 0;
            double sJspPrice = 0;
            if (DgCell.Column == colZspPrice || DgCell.Column == colGroupZspPrice)
            {
                sZspPrice = record.PrevSettleMent;
            }
            else if (DgCell.Column == colJspPrice || DgCell.Column == colGroupJspPrice)
            {
                sJspPrice = record.IClose;
            }
            //点击结算价
            double sJsjPrice = 0;
            double sDrjjPrice = 0;
            if (DgCell.Column == colJsjPrice || DgCell.Column == colGroupJsjPrice)
            {
                sJsjPrice = record.ISettlementPrice;
            }
            else if (DgCell.Column == colDrjjPrice || DgCell.Column == colGroupDrjjPrice)
            {
                sDrjjPrice = record.AvgPrice;
            }

            RealData realRecord = new RealData();
            if (RealDataMouseLeftButtonDown != null && realRecord.UpdateRealProperties(record))
            {
                RealDataMouseLeftButtonDown(buySell, realRecord, fixPrice, sNewPrice, sJkpPrice, sZjsPrice, sHighPrice, sLowPrice, sZspPrice, sJspPrice, sJsjPrice, sDrjjPrice, isBuySell);
            }
        }

        /// <summary>
        /// 将持仓和下单板的合约行情显示出来
        /// </summary>
        /// <param name="externalHq"></param>
        public void AddExternalHqingData(DisplayRealData externalHq)
        {
            if (defaultButton != null && defaultButton.Content.ToString() == "主力合约") return;
            if (!HQRealData._CommObj.RequestingCodes.Contains(externalHq.Code))
            {
                DisplayRealData orderData = null;
                foreach (var item in _MainWindow.RealDataCollection)
                {
                    if (item.Code == externalHq.Code)
                    {
                        orderData = item;
                        break;
                    }
                }

                if (orderData == null)
                {
                    //Util.Log("添加其他行情数据:" + externalHq.ToString());
                    _MainWindow.RealDataCollection.Add(externalHq);
                    dgRealData.SelectedItem = externalHq;
                    dgRealData.ScrollIntoView(dgRealData.SelectedItem);
                }
                else
                {
                    orderData.INewPrice = externalHq.INewPrice;
                    orderData.StBuyPrice = externalHq.StBuyPrice;
                    orderData.I64Sum = externalHq.I64Sum;
                    orderData.StBuyCount = externalHq.StBuyCount;
                    orderData.StSellPrice = externalHq.StSellPrice;
                    orderData.StSellCount = externalHq.StSellCount;
                    orderData.ChiCangLiang = externalHq.ChiCangLiang;
                    orderData.Volumn = externalHq.Volumn;
                    orderData.CurrentHand = externalHq.CurrentHand;
                    orderData.Open = externalHq.Open;
                    orderData.ISettlementPrice = externalHq.ISettlementPrice;        //现结算
                    orderData.IClose = externalHq.IClose;
                    orderData.UpdateTime = externalHq.UpdateTime;
                }
            }
        }

        public void SelectDataByCode(string code)
        {
            DisplayRealData realData = null;
            foreach (var item in _MainWindow.RealDataCollection)
            {
                if (item.Code == code)
                {
                    realData = item;
                    break;
                }
            }
            if (realData != null)
            {
                dgRealData.SelectedItem = null;
                dgRealData.SelectedItem = realData;
                dgRealData.ScrollIntoView(dgRealData.SelectedItem);
            }
        }

        private void btnReportSetting_Click(object sender, RoutedEventArgs e)
        {
            ChooseContract chooseContract = new ChooseContract();
            chooseContract.DataContext = this._MainWindow;
            Window codeSetWindow = CommonUtil.GetWindow("合约设置", chooseContract, _MainWindow);
            codeSetWindow.Closed += new EventHandler(codeSetWindow_Closed);
            ObservableCollection<UserCodeSet> lstUserCodeSet = UserCodeSetInstance.GetUserCodeSetList();
            int i = 0;
            UserCodeSet userCodeSet = null;
            foreach (var item in lstUserCodeSet)
            {
                userCodeSet = item as UserCodeSet;
                if (defaultButton != null && defaultButton.Content.ToString() == userCodeSet.Name)
                {
                    break;
                }
                i += 1;
            }
            chooseContract.cbUserCodeSet.SelectedIndex = i;
            codeSetWindow.ShowDialog();
        }

        private void ReformBlockButtons()
        {
            ObservableCollection<UserCodeSet> userCodeSetList = UserCodeSetInstance.GetUserCodeSetList();
        }

        /// <summary>
        /// 重新更新板块按钮
        /// </summary>
        public void ResetBlockButtons()
        {
            stackGridBlock.Children.Clear();

            ObservableCollection<UserCodeSet> lstUserCodeSet = UserCodeSetInstance.GetUserCodeSetList();
            int i = 0;
            foreach (var item in lstUserCodeSet)
            {
                Button btn = new Button();
                
                btn.Content = item.Name;
                btn.Tag = item.Id;
                btn.Click += new RoutedEventHandler(btn_Click);
                btn.HorizontalAlignment = HorizontalAlignment.Left;
                btn.Margin = new Thickness(i*85, -1, 0, 0);
                btn.Width = 80;
                btn.Height = 23;
                stackGridBlock.Children.Add(btn);

                //btn.FontWeight = FontWeights.Normal;
                //btn.Foreground = new SolidColorBrush(Colors.Black);

                //btn.Style = (Style)FindResource("btnDefault");

                if (item.IsDefault && defaultButton == null)
                {
                    defaultButton = btn;
                }
                else if (defaultButton.Content.Equals( item.Name))
                {
                    defaultButton = btn;
                }
                i += 1;
            }

            //defaultButton.Style = (Style)FindResource("btnDefault_Selected");

            btn_Click(defaultButton,null);
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            //if (IsNeedNotice())
            //{
            //    //警告
            //    MessageBox.Show(Login.GetChangeCodeSetSleepMessage());
            //    return;
            //}

            ObservableCollection<UserCodeSet> lstUserCodeSet = UserCodeSetInstance.GetUserCodeSetList();
            string userCodeSetId = (sender as Button).Tag.ToString();

            foreach (var realItem in lstUserCodeSet)
            {
                if (realItem.Id == userCodeSetId)
                {
                    if (!realItem.IsArbitrage)  //单腿
                    {
                        if (dgGroupRealData.Visibility == Visibility.Visible)
                        {
                            ObservableCollection<Contract> newCodes = new ObservableCollection<Contract>();
                            ChangeUserGroupCodeSet(newCodes);
                        }
                        //if (realItem.IsZhuli)
                        //{
                        //    btnZLCodeSort.Visibility = Visibility.Visible;
                        //}
                        //else
                        //{
                        //    btnZLCodeSort.Visibility = Visibility.Hidden;
                        //}
                        ChangeUserCodeSet(false, UserCodeSetInstance.GetContractListByUserCodeSet(userCodeSetId));
                    }

                    else   //套利
                    {
                        if (dgRealData.Visibility == Visibility.Visible)
                        {
                            ObservableCollection<Contract> newCodes = new ObservableCollection<Contract>();
                            ChangeUserCodeSet(false,newCodes);
                        }
                        //RequestRealData(GroupHQRealData);
                        ChangeUserGroupCodeSet(UserCodeSetInstance.GetContractListByUserCodeSet(userCodeSetId));
                    }
                    break;
                }
            }

            foreach (var item in stackGridBlock.Children)
            {
                if (item is Button)
                {
                    Button btn = item as Button;
                    btn.IsEnabled = true;
                }
            }
            defaultButton = sender as Button;
            defaultButton.IsEnabled = false;

            
            foreach (var item in lstUserCodeSet)
            {
                if ((sender as Button).Content.ToString() == item.Name)
                {
                    if (item.IsArbitrage)
                    {
                        _MainWindow.uscHangqing.dgRealData.Visibility = Visibility.Hidden;
                        _MainWindow.uscHangqing.dgGroupRealData.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        _MainWindow.uscHangqing.dgRealData.Visibility = Visibility.Visible;
                        _MainWindow.uscHangqing.dgGroupRealData.Visibility = Visibility.Hidden;
                    }
                    break;
                }
            }
        }

        private void dgRealData_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DgCell == null)
                {
                    return;
                }
                DisplayRealData record = dgRealData.CurrentItem as DisplayRealData;
                GridHangQingData(record);
                _MainWindow.uscNewOrderPanel.NewOrderFocus();
            }
        }

        private void dgGroupRealData_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (DgCell == null)
                {
                    return;
                }
                DisplayRealData record = dgGroupRealData.CurrentItem as DisplayRealData;
                GridHangQingData(record);
                _MainWindow.uscNewOrderPanel.NewOrderFocus();
            }
        }

        public TextBlock GetVisualChild<t>(Visual parent) where t : Visual
        {
            TextBlock child = default(TextBlock);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as TextBlock;
                if (child == null)
                {
                    child = GetVisualChild<t>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        /// <summary>
        /// 买价，卖价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseEnter_Open(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            if (grid == null) return;
            TextBlock tp = grid.ToolTip as TextBlock;
            if (tp != null)
            {
                 int hand=1;
                if (_MainWindow.uscNewOrderPanel.iudNum.Value != null)
                {
                    hand=(int)_MainWindow.uscNewOrderPanel.iudNum.Value.Value;
                    if (hand == 0)
                    {
                        hand = 1;
                    }
                }

                foreach (Run run in tp.Inlines)
                {
                    if (run.Name == "hand")
                    {
                        run.Text = hand.ToString();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 买量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseEnter_Close_Buy(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            if (grid == null) return;
            TextBlock tp = grid.ToolTip as TextBlock;
            DisplayRealData displayRealData = grid.DataContext as DisplayRealData;
            //卖出平仓
            TradeDataClient jyDataServer = TradeDataClient.GetClientInstance();
            int todayPosCount;
            int lastPosCount;
            string code = displayRealData.Code;
            //查找多头的单子
            if (jyDataServer.GetPositionCount(code, true, out todayPosCount, out lastPosCount) == false)
            {
                grid.ToolTip = null;
                return;
            }
            if (todayPosCount == 0 && lastPosCount == 0)
            {
                grid.ToolTip = null;
                return;
            }
            if (tp == null)
            {
                grid.ToolTip = new TextBlock();
                tp = grid.ToolTip as TextBlock;
            }
            tp.Inlines.Clear();
            Run run = new Run();
            run.Text = "双击 ";
            tp.Inlines.Add(run);

            run = new Run();
            Binding binding1 = new Binding();
            binding1.Source = grid.DataContext;
            binding1.Mode = BindingMode.OneWay;
            binding1.Path = new PropertyPath("StBuyPrice");
            run.SetBinding(Run.TextProperty, binding1);
            tp.Inlines.Add(run);

            run = new Run();
            run.Text = " 卖出";
            run.Foreground = new SolidColorBrush(Color.FromRgb(0x13, 0xaf, 0x13));
            tp.Inlines.Add(run);

            Boolean bHasComma=false;
            if (CodeSetManager.IsCloseTodaySupport(code) && todayPosCount != 0)
            {
                //如果支持平今
                run = new Run();
                run.Text = "平今";
                run.FontSize = 13;
                run.Foreground = new SolidColorBrush(Color.FromRgb(0xff,0x00,0xff));
                tp.Inlines.Add(run);

                run = new Run();
                run.Text = " " + todayPosCount.ToString() + " ";
                run.Foreground = new SolidColorBrush(Color.FromRgb(0x13, 0xaf, 0x13));
                tp.Inlines.Add(run);

                bHasComma = true;
            }
            else
            {
                lastPosCount += todayPosCount;
            }
            if (lastPosCount != 0)
            {
                if (bHasComma)
                {
                    run = new Run();
                    run.Text = ",";
                    tp.Inlines.Add(run);
                }

                run = new Run();
                run.Text = " 平仓";
                run.FontSize = 13;
                run.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0xff));
                tp.Inlines.Add(run);

                run = new Run();
                run.Text = " " + lastPosCount.ToString() + " ";
                run.Foreground = new SolidColorBrush(Color.FromRgb(0x13, 0xaf, 0x13));
                tp.Inlines.Add(run);
            }

            run = new Run();
            run.Text ="手"+code;
            tp.FontWeight = FontWeights.Bold;
            tp.Inlines.Add(run);
        }


        /// <summary>
        /// 卖量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseEnter_Close_Sell(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;
            if (grid == null) return;
            TextBlock tp = grid.ToolTip as TextBlock;
            DisplayRealData displayRealData = grid.DataContext as DisplayRealData;
            //买入平仓
            TradeDataClient jyDataServer = TradeDataClient.GetClientInstance();
            int todayPosCount;
            int lastPosCount;
            string code = displayRealData.Code;
            if (jyDataServer.GetPositionCount(code, false, out todayPosCount, out lastPosCount) == false)
            {
                grid.ToolTip = null;
                return;
            }
            if (todayPosCount == 0 && lastPosCount == 0)
            {
                grid.ToolTip = null;
                return;
            }
            if (tp == null)
            {
                grid.ToolTip = new TextBlock();
                tp = grid.ToolTip as TextBlock;
            }
            tp.Inlines.Clear();
            Run run = new Run();
            run.Text = "双击 ";
            tp.Inlines.Add(run);

            run = new Run();
            Binding binding1 = new Binding();
            binding1.Source = grid.DataContext;
            binding1.Mode = BindingMode.OneWay;
            binding1.Path = new PropertyPath("StSellPrice");
            run.SetBinding(Run.TextProperty, binding1);
            tp.Inlines.Add(run);

            run = new Run();
            run.Text = " 买入";
            run.Foreground = new SolidColorBrush(Colors.Red);
            tp.Inlines.Add(run);

            Boolean bHasComma = false;
            if (CodeSetManager.IsCloseTodaySupport(code) && todayPosCount != 0)
            {
                //如果支持平今
                run = new Run();
                run.Text = "平今";
                run.FontSize = 13;
                run.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0xff));
                tp.Inlines.Add(run);

                run = new Run();
                run.Text = " " + todayPosCount.ToString() + " ";
                run.Foreground = new SolidColorBrush(Colors.Red);
                tp.Inlines.Add(run);

                bHasComma = true;
            }
            else
            {
                lastPosCount += todayPosCount;
            }
            if (lastPosCount != 0)
            {
                if (bHasComma)
                {
                    run = new Run();
                    run.Text = ",";
                    tp.Inlines.Add(run);
                }

                run = new Run();
                run.Text = " 平仓";
                run.FontSize = 13;
                run.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0xff));
                tp.Inlines.Add(run);

                run = new Run();
                run.Text = " " + lastPosCount.ToString() + " ";
                run.Foreground = new SolidColorBrush(Colors.Red);
                tp.Inlines.Add(run);
            }

            run = new Run();
            run.Text = "手" + code;
            tp.FontWeight = FontWeights.Bold;
            tp.Inlines.Add(run);
        }

        /// <summary>
        /// 全景式下单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void miFullViewOrder_Click(object sender, RoutedEventArgs e)
        {
            //if (dgRealData.SelectedItem == null) { return; }

            //RealData selectedItem = (RealData)dgRealData.SelectedItem;
            //if (!CommonUtil.isValidCode(selectedItem.Code))
            //{
            //    return;
            //}

            //FullViewOrderPanelNew fullViewOrderPanel = new FullViewOrderPanelNew();
            //fullViewOrderPanel.Init(this.mainWindow, selectedItem);
            //fullViewOrderPanel.miStyleSetting.Click += new RoutedEventHandler(mainWindow.StyleSetting_Click);
            //Window fullViewOrder = CommonUtil.GetWindow(selectedItem.Name + "-" + selectedItem.Code, fullViewOrderPanel, this.mainWindow);
            //fullViewOrder.Closed += new EventHandler(fullViewOrder_Closed);
            //if (mainWindow.LstFviewPanel == null) { mainWindow.LstFviewPanel = new List<Window>(); }
            //mainWindow.LstFviewPanel.Add(fullViewOrder);
            //fullViewOrder.Show();
        }

        /// <summary>
        /// 全景式下单—关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fullViewOrder_Closed(object sender, EventArgs e)
        {
            //FullViewOrderPanelNew fullViewOrderPanel = sender as FullViewOrderPanelNew;
            //if (fullViewOrderPanel == null)
            //{
            //    Window window = sender as Window;
            //    if (window == null)
            //    {
            //        Util.Log("关闭全景式下单窗口时错误一");
            //        return;
            //    }
            //    fullViewOrderPanel = window.Content as FullViewOrderPanelNew;
            //    if (fullViewOrderPanel == null)
            //    {
            //        Util.Log("关闭全景式下单窗口时错误二");
            //        return;
            //    }
            //}

            //JYDataServer dataServer = JYDataServer.getServerInstance();
            //dataServer.OnFilteredOrderDataChanged -= new JYDataServer.DelFilteredOrderDataChanged(fullViewOrderPanel.FullViewOrderPanel_OnFilteredOrderDataChanged);
            //dataServer.FullViewLotsEvent -= new JYDataServer.FullViewOrderPanelLots(fullViewOrderPanel.FullViewOrderPanel_FullViewLotsEvent);
        }

        private void btnZLCodeSort_Click(object sender, RoutedEventArgs e)
        {
            //对mainWindow.RealDataCollection进行排序
            //List<RealData> realDataList=new List<RealData>();
            //realDataList.AddRange(mainWindow.RealDataCollection);
            //realDataList.Sort(RealData.CompareByVolumn);
            //mainWindow.RealDataCollection.Clear();
            //foreach(RealData d in realDataList)
            //{
            //    mainWindow.RealDataCollection.Add(d);
            //}
        }

        private void ShowCodeInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        public DisplayRealData UpdateDisplayProperties(RealData realData)
        {
            DisplayRealData displayData = new DisplayRealData();
            try
            {
                displayData.Code = realData.CodeInfo.Code;
                if (realData.CodeInfo.ProductType != null && realData.CodeInfo.ProductType == "Combination" || displayData.Code.Contains("&"))
                {
                    if (realData.CodeInfo.Code.Contains(" "))
                    {
                        string[] tempType = realData.CodeInfo.Code.Split(' ');
                        string[] tempGroup = tempType[1].Split('&');
                        displayData.Code1 = tempGroup[0];
                        displayData.Code2 = tempGroup[1];
                    }
                    else
                    {
                        string[] tempGroup = realData.CodeInfo.Code.Split('&');
                        displayData.Code1 = tempGroup[0];
                        displayData.Code2 = tempGroup[1];
                    }
                }
                    
                displayData.Name = realData.CodeInfo.Name;
                displayData.ChiCangLiang = realData.Position;
                displayData.StBuyCount = realData.BidHand[0];
                displayData.StBuyPrice = realData.BidPrice[0];
                displayData.StSellCount = realData.AskHand[0];
                displayData.StSellPrice = realData.AskPrice[0];
                displayData.DownStopPrice = realData.LowerLimitPrice;
                displayData.I64Sum = (UInt64)realData.Sum;
                //this.ICurrentSum = realData.ICurrentSum ;
                displayData.ISettlementPrice = realData.SettlmentPrice;
                displayData.PrevSettleMent = realData.PrevSettlementPrice;
                displayData.UpStopPrice = realData.UpperLimitPrice;
                displayData.CurrentHand = realData.Hand;
                //this.I64Outside = realData.I64Outside;
                displayData.IClose = realData.ClosePrice;
                //this.IDealNumber = realData.IDealNumber;
                displayData.IMaxPrice = realData.MaxPrice;
                displayData.IMinPrice = realData.MinPrice;
                displayData.INewPrice = realData.NewPrice;

                displayData.Open = realData.OpenPrice;
                //this.NTime = realData.NTime;
                displayData.PrevClose = realData.PrevClose;
                //this.UsUpdateNumber = realData.UsUpdateNumber;
                displayData.Volumn = realData.Volumn;
                displayData.Market = CodeSetManager.CtpToExName(realData.CodeInfo.ExchCode);
                displayData.PreChicang = realData.PrevPosition;
                displayData.PrevClose = realData.PrevClose;
                displayData.UpdateTime = realData.UpdateTime;
            }
            catch (Exception ex)
            {
                Util.Log_Error("exception: " + ex.Message);
                Util.Log_Error("exception: " + ex.StackTrace);
            }
            return displayData;
        }

        private void CbxShowLevels_Checked(object sender, RoutedEventArgs e)
        {
            if (LvQuotesPanel != null)
            {
                LvQuotesPanel.Visibility = Visibility.Visible;
            }
        }

        private void CbxShowLevels_Unchecked(object sender, RoutedEventArgs e)
        {
            if (LvQuotesPanel != null)
            {
                LvQuotesPanel.Visibility = Visibility.Collapsed;
            }
        }
    }
}
