using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using TradingMaster.CodeSet;
using TradingMaster.JYData;

namespace TradingMaster.Control
{
    /// <summary>
    /// OptionQuotes.xaml 的交互逻辑
    /// </summary>
    public partial class OptionQuotes : UserControl
    {
        int _ColumnStartC = 0;
        int _ColumnEndC = 13 + 5;
        int _ColumnF = 14 + 5;
        int _ColumnStartP = 15 + 5;
        int _ColumnEndP = 28 + 5 * 2;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buyOrSell">三种值，""/"卖"/"买"，分别代表要下单、下卖单或者下买单</param>
        /// <param name="realData"></param>
        public delegate void RealDataMouseDoubleClickDelegate(object sender, MouseButtonEventArgs e);
        public delegate void RealDataMouseLeftButtonDownDelegate(string buyOrSell, RealData realData, double sJkpPrice, double sZjsPrice,
                         double fixPrice, double sNewPrice, double sHighPrice, double sLowPrice, double sZspPrice, double sJspPrice, double sJsjPrice, double sDrjjPrice, Boolean isBuySell);

        private XmlDocument _ProductDoc = null;
        private DataGridColumn _LastSelectedColumnForQuote;

        public event RealDataMouseLeftButtonDownDelegate RealDataMouseLeftButtonDown;
        public event RealDataMouseDoubleClickDelegate RealDataMouseDoubleClicked;

        public OptionQuotes()
        {
            InitializeComponent();

            foreach (var item in this.dgHangqing.Columns)
            {
                item.Width = DataGridLength.Auto;
            }
            dgHangqing.ColumnDisplayIndexChanged += new EventHandler<DataGridColumnEventArgs>(dgHangqing_ColumnDisplayIndexChanged);
            CbxGreekLetters_Unchecked(null, null);
        }

        public BackgroundDataServer OptionQuotesRealData;
        //public Dictionary<string, List<Contract>> CCFXOptionCodeDict = new Dictionary<string, List<Contract>>();
        //public Dictionary<string, List<Contract>> XDCEOptionCodeDict = new Dictionary<string, List<Contract>>();
        //public Dictionary<string, List<Contract>> XZCEOptionCodeDict = new Dictionary<string, List<Contract>>();
        public Dictionary<string, List<string>> OptionCodeDict = new Dictionary<string, List<string>>();
        public Dictionary<string, List<Contract>> OptionContractDict = new Dictionary<string, List<Contract>>();

        private MainWindow _MainWindow { get; set; }
        private RealData _LatestDisplayRealData = new RealData();
        private RealData _FuturesRealData = new RealData();
        private Dictionary<string, RealData> _BackupCodeDic = new Dictionary<string, RealData>();

        public void Init(MainWindow parent)
        {
            this.DataContext = parent;
            this._MainWindow = parent;
            ReleatedQutoes.DataContext = _FuturesRealData;
            //OptionQuotesRealData = new BackgroundDataServer();
            OptionQuotesRealData = new BackgroundDataServer(_BackupCodeDic);
            if (_ProductDoc == null)
            {
                _ProductDoc = new XmlDocument();
            }
            _ProductDoc.Load(CodeSetManager.OPTION_SETTINGS_FILE);

            foreach (Species optSpec in CodeSetManager.OptionSpecList)
            {
                if (optSpec.ExchangeCode == "CFFEX")//中金所
                {
                    GetCCFXOptionDic(optSpec.SpeciesCode);
                }
                else if (optSpec.ExchangeCode == "CZCE")//郑商所
                {
                    GetXZCEOptionDic(optSpec.SpeciesCode);
                }
                else if (optSpec.ExchangeCode == "DCE")//大商所
                {
                    GetXDCEOptionDic(optSpec.SpeciesCode);
                }
                else if (optSpec.ExchangeCode == "SHFE")//上期所
                {
                    GetXSGEOptionDic(optSpec.SpeciesCode);
                }
            }
            //GetCCFXOptionDic("IO");
            //GetCCFXOptionDic("HO");
            //GetXDCEOptionDic("m");
            //GetXZCEOptionDic("SR");
            //GetXZCEOptionDic("CF");
            //GetXSGEOptionDic("cu_o");
            //GetXSGEOptionDic("au_o");

            foreach (string fKey in OptionContractDict.Keys)
            {
                OptionContractDict[fKey].Sort(Contract.CompareByCode);
            }

            List<string> optionCodes = new List<string>();
            foreach (string futures in OptionCodeDict.Keys)
            {
                optionCodes.Add(GetRelatedOption(futures));
            }
            optionCodes.Sort(String.Compare);
            cbFutureCode.ItemsSource = optionCodes;

            LvOptQuotesPanel.Init(this._MainWindow);
        }

        private void GetCCFXOptionDic(string optionSpec)//(string futuresSpec, string optionSpec)
        {
            try
            {
                //List<string> futuresCodes = CodeSetManager.GetCodeStringListBySpecies(futuresSpec);
                List<string> optionCodes = CodeSetManager.GetCodeStringListBySpecies(optionSpec);
                string futuresSpec = GetRelatedFutures(optionSpec);
                foreach (string item in optionCodes)
                {
                    int firstIndex = item.IndexOfAny(new char[] { '-' });
                    if (firstIndex <= 0)
                    {
                        Util.Log("Warning! Option Code is illegal: " + item);
                        continue;
                    }
                    string futureKey = item.Replace(optionSpec, futuresSpec).Substring(0, firstIndex);
                    //string futureKey = item.Code.Substring(0, firstIndex);

                    if (!OptionCodeDict.ContainsKey(futureKey))
                    {
                        OptionCodeDict.Add(futureKey, new List<string>());
                    }
                    if (!OptionContractDict.ContainsKey(futureKey))
                    {
                        OptionContractDict.Add(futureKey, new List<Contract>());
                    }
                    OptionCodeDict[futureKey].Add(item);
                    OptionContractDict[futureKey].Add(CodeSetManager.GetContractInfo(item));
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        private void GetXDCEOptionDic(string spec)
        {
            try
            {
                List<Contract> dceCodes = CodeSetManager.GetOptionContractListBySpecies(spec);
                foreach (var item in dceCodes)
                {
                    //int firstIndex = item.IndexOfAny(new char[] { '-' });
                    //if (firstIndex <= 0)
                    //{
                    //    Util.Log("Warning! Option Code is illegal: " + item);
                    //    continue;
                    //}
                    //string futureKey = item.Substring(0, firstIndex);

                    string futureKey = item.BaseCode;
                    if (String.IsNullOrEmpty(futureKey))//(firstIndex <= 0)
                    {
                        Util.Log("Warning! Option Code is illegal: " + item);
                        continue;
                    }

                    if (!OptionCodeDict.ContainsKey(futureKey))
                    {
                        OptionCodeDict.Add(futureKey, new List<string>());
                    }
                    if (!OptionContractDict.ContainsKey(futureKey))
                    {
                        OptionContractDict.Add(futureKey, new List<Contract>());
                    }
                    OptionCodeDict[futureKey].Add(item.Code);
                    OptionContractDict[futureKey].Add(item as Contract);
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        private void GetXZCEOptionDic(string spec)
        {
            try
            {
                List<Contract> zceCodes = CodeSetManager.GetOptionContractListBySpecies(spec);
                foreach (var item in zceCodes)
                {
                    string futureKey = item.BaseCode;
                    if (String.IsNullOrEmpty(futureKey))//(firstIndex <= 0)
                    {
                        Util.Log("Warning! Option Code is illegal: " + item);
                        continue;
                    }

                    if (!OptionCodeDict.ContainsKey(futureKey))
                    {
                        OptionCodeDict.Add(futureKey, new List<string>());
                    }
                    if (!OptionContractDict.ContainsKey(futureKey))
                    {
                        OptionContractDict.Add(futureKey, new List<Contract>());
                    }
                    OptionCodeDict[futureKey].Add(item.Code);
                    OptionContractDict[futureKey].Add(item as Contract);
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        private void GetXSGEOptionDic(string spec)
        {
            try
            {
                List<Contract> sgeCodes = CodeSetManager.GetOptionContractListBySpecies(spec);
                foreach (var item in sgeCodes)
                {
                    //int firstIndex = item.IndexOfAny(new char[] { 'P', 'C' });
                    //if (firstIndex < 0)
                    //{
                    //    Util.Log("Warning! Option Code is illegal: " + item);
                    //    continue;
                    //}
                    //string futureKey = item.Substring(0, firstIndex);

                    string futureKey = item.BaseCode;
                    if (String.IsNullOrEmpty(futureKey))//(firstIndex <= 0)
                    {
                        Util.Log("Warning! Option Code is illegal: " + item);
                        continue;
                    }

                    if (!OptionCodeDict.ContainsKey(futureKey))
                    {
                        OptionCodeDict.Add(futureKey, new List<string>());
                    }
                    if (!OptionContractDict.ContainsKey(futureKey))
                    {
                        OptionContractDict.Add(futureKey, new List<Contract>());
                    }
                    OptionCodeDict[futureKey].Add(item.Code);
                    OptionContractDict[futureKey].Add(item as Contract);
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        public string GetRelatedFutures(string option)
        {
            if (_ProductDoc == null)
            {
                Util.Log("Error! Products.xml has not been loaded successfully.");
                return String.Empty;
            }
            try
            {
                XmlNodeList nodeLst = _ProductDoc.SelectNodes("markets/market/product");
                foreach (XmlNode node in nodeLst)
                {
                    if (option.Contains(node.Attributes["code"].Value.ToString()) && node.Attributes["RelatedCode"] != null)
                    {
                        return option.Replace(node.Attributes["code"].Value.ToString(), node.Attributes["RelatedCode"].Value.ToString());
                    }
                }
                //Temp:
                //if (option.Contains("IO"))
                //{
                //    return option.Replace("IO","IF");
                //}
                //if (option.Contains("HO"))
                //{
                //    return option.Replace("HO", "IH");
                //}
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
            return option;
        }

        public string GetRelatedOption(string futures)
        {
            if (_ProductDoc == null)
            {
                Util.Log("Error! Products.xml has not been loaded successfully.");
                return String.Empty;
            }
            try
            {
                XmlNodeList nodeLst = _ProductDoc.SelectNodes("markets/market/product");
                foreach (XmlNode node in nodeLst)
                {
                    if (node.Attributes["RelatedCode"] != null && futures.Contains(node.Attributes["RelatedCode"].Value.ToString()))
                    {
                        return futures.Replace(node.Attributes["RelatedCode"].Value.ToString(), node.Attributes["code"].Value.ToString());
                    }
                }
                //if (futures.Contains("IF"))
                //{
                //    return futures.Replace("IF", "IO");
                //}
                //if (futures.Contains("IH"))
                //{
                //    return futures.Replace("IH", "HO");
                //}
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
            return futures;
        }

        private void Resize()
        {
            double headerCWidth = 0;
            for (int i = _ColumnStartC; i <= _ColumnEndC; i++)
            {
                if (dgHangqing.Columns[i].Visibility == System.Windows.Visibility.Visible)
                {
                    headerCWidth += dgHangqing.Columns[i].ActualWidth;
                }
            }
            colHeaderC.Width = headerCWidth;

            colHeaderF.Width = dgHangqing.Columns[_ColumnF].ActualWidth;

            double headerPWidth = 0;
            for (int i = _ColumnStartP; i <= _ColumnEndP; i++)
            {
                if (dgHangqing.Columns[i].Visibility == System.Windows.Visibility.Visible)
                {
                    headerPWidth += dgHangqing.Columns[i].ActualWidth;
                }
            }
            colHeaderP.Width = headerPWidth;
        }

        private void dgHeader_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Resize();
        }

        private void dgHangqing_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Resize();
        }

        private void dgHangqing_LayoutUpdated(object sender, EventArgs e)
        {
            Resize();
        }

        private void cbFutureCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cbFutureCode.SelectedItem != null)
                {
                    Contract ci = CodeSetManager.GetContractInfo(GetRelatedFutures(cbFutureCode.SelectedItem.ToString()));
                    if (ci == null)
                    {
                        ci = new Contract(GetRelatedFutures(cbFutureCode.SelectedItem.ToString()));
                    }
                    titleCode.Content = ci.Code;

                    UpdateOptionDataCollectionByCode(ci);
                }
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log("exception: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="code">期权合约代码:IO1309</param>
        public void UpdateOptionDataCollectionByCode(Contract fCode)
        {
            _MainWindow.OptionRealDataCollection.Clear();
            RealData realData = null;

            //foreach (var item in mainWindow.RealDataCollection)
            //{
            //    if (item.Code == code.Replace("IO", "IF"))
            //    {
            //        displayRealData = item;
            //    }
            //}

            if (realData == null)
            {
                //TODO
                realData = new RealData();
                realData.CodeInfo.Code = fCode.Code;
            }
            _LatestDisplayRealData.CopyProperties(realData);
            UpdateOptionRelatedDataByRealData(realData);

            dgHangqing.Columns[_ColumnF].Header = "C←" + GetRelatedOption(fCode.Code) + "→P";

            Dictionary<string, OptionRealData> dicOptionRealData = new Dictionary<string, OptionRealData>();
            List<Contract> contractLst = new List<Contract>();
            contractLst.Add(fCode);

            //foreach (string item in CodeSetManager.LstOptionCodes)
            if (OptionContractDict.ContainsKey(fCode.Code))
            {
                foreach (Contract item in OptionContractDict[fCode.Code])
                {
                    if (dicOptionRealData.ContainsKey(item.Code.Replace("P", "C")))
                    {
                        continue;
                    }
                    OptionRealData ordRealData = new OptionRealData();
                    int lastIndex = item.Code.LastIndexOfAny(new char[] { '-', 'C', 'P' });
                    ordRealData.ExecutePrice = CommonUtil.GetDoubleValue(item.Code.Substring(lastIndex + 1));
                    if (fCode.ProductType == null)//Temp Judgement
                    {
                        ordRealData.Code = item.BaseCode;
                    }
                    else
                    {
                        ordRealData.Code = fCode.Code;
                    }
                    if (item.Code.Contains("-"))
                    {
                        ordRealData.Code_C = item.Code.Replace("-P-", "-C-");
                        ordRealData.Code_P = item.Code.Replace("-C-", "-P-");
                    }
                    else
                    {
                        int tempFirstIndex = item.Code.IndexOf(item.BaseCode);
                        string optInfoString = item.Code.Substring(tempFirstIndex + item.BaseCode.Length);
                        ordRealData.Code_C = item.BaseCode + optInfoString.Replace("P", "C");
                        ordRealData.Code_P = item.BaseCode + optInfoString.Replace("C", "P");
                    }
                    ordRealData.Market = item.ExchName;
                    dicOptionRealData.Add(ordRealData.Code_C, ordRealData);
                    _MainWindow.OptionRealDataCollection.Add(ordRealData);

                    if (ordRealData.Code_C != null && ordRealData.Market != null)
                    {
                        string tempKey = ordRealData.Code_C + "_" + CodeSetManager.ExNameToCtp(ordRealData.Market);
                        if (!_BackupCodeDic.ContainsKey(tempKey))
                        {
                            RealData tempData = new RealData();
                            tempData.CodeInfo = CodeSetManager.GetContractInfo(ordRealData.Code_C, CodeSetManager.ExNameToCtp(ordRealData.Market));
                            if (tempData.CodeInfo == null)
                            {
                                tempData.CodeInfo = new Contract(ordRealData.Code_C);
                            }
                            _BackupCodeDic.Add(tempKey, tempData);
                        }
                    }
                    if (ordRealData.Code_P != null && ordRealData.Market != null)
                    {
                        string tempKey = ordRealData.Code_P + "_" + CodeSetManager.ExNameToCtp(ordRealData.Market);
                        if (!_BackupCodeDic.ContainsKey(tempKey))
                        {
                            RealData tempData = new RealData();
                            tempData.CodeInfo = CodeSetManager.GetContractInfo(ordRealData.Code_P, CodeSetManager.ExNameToCtp(ordRealData.Market));
                            if (tempData.CodeInfo == null)
                            {
                                tempData.CodeInfo = new Contract(ordRealData.Code_P);
                            }
                            _BackupCodeDic.Add(tempKey, tempData);
                        }
                    }
                }
                contractLst.AddRange(OptionContractDict[fCode.Code]);
            }

            //Get Saved RealData from BackgroundServer
            foreach (Contract cItem in contractLst)
            {
                RealData optRealData = DataContainer.GetRealDataFromContainer(cItem);
                if (optRealData != null)
                {
                    OptionQuotesRealData.SavedDataInit(optRealData);
                }
            }

            OptionQuotesRealData.AddContract(contractLst.ToArray());
            //OptionQuotesRealData.GetRealData();
            OptionQuotesRealData.Request();
            //lock (CtpDataServer.ServerLock)
            {
                OptionQuotesRealData.RemoveUselessRequest();
            }
            OptionQuotesRealData.UpdateRealDataList();
        }

        public void UpdateOptionRelatedDataByRealData(RealData realData)
        {
            if (cbFutureCode.SelectedValue != null && realData != null
                && GetRelatedFutures(cbFutureCode.SelectedValue.ToString()) == realData.CodeInfo.Code)
            //&& cbFutureCode.SelectedValue.ToString().Replace("IO", "IF") == displayRealData.codeInfo.Code)
            {
                _LatestDisplayRealData.CopyProperties(realData);
                _FuturesRealData.CopyProperties(realData);

                string priceFormat = CommonUtil.GetPriceFormat(realData.CodeInfo.Code);
                tbNewPrice.Text = realData.NewPrice.ToString(priceFormat);
                tbBuyPrice.Content = realData.BidPrice[0].ToString(priceFormat);
                tbBuyCount.Content = realData.BidHand[0].ToString("F0");
                tbSellPrice.Content = realData.AskPrice[0].ToString(priceFormat);
                tbSellCount.Content = realData.AskHand[0].ToString("F0");
                tbFlex.Text = (realData.NewPrice - realData.PrevSettlementPrice).ToString(priceFormat);
                tbChengjiao.Text = realData.Volumn.ToString("F0");
                tbPosition.Text = realData.Position.ToString("F0");
                //tbHighPrice.Text = displayRealData.maxPrice.ToString(priceFormat);
                //tbLowPrice.Text = displayRealData.minPrice.ToString(priceFormat);
                //tbUpStop.Text = realData.UpperLimitPrice.ToString(priceFormat);
                //tbDownStop.Text = realData.LowerLimitPrice.ToString(priceFormat);
                tbPreSettlement.Text = realData.PrevSettlementPrice.ToString(priceFormat);
            }
        }

        private void dgHangqing_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RealDataMouseDoubleClicked != null)
            {
                DataGridCell cell = CommonUtil.GetClickedCell(e);
                if (cell == null ||
                 !(cell.Column == colBuyCount_C || cell.Column == colBuyCount_P
                || cell.Column == colBuyPrice_C || cell.Column == colBuyPrice_P
                || cell.Column == colSellPrice_C || cell.Column == colSellPrice_P
                || cell.Column == colSellCount_C || cell.Column == colSellCount_P
                   ))
                {
                    return;
                }
                if (dgHangqing.SelectedItem != null)
                {
                    OptionRealData selectedRecord = dgHangqing.SelectedItem as OptionRealData;
                }
                RealDataMouseDoubleClicked(sender, e);
            }
        }

        private void ReleatedQutoes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RealDataMouseDoubleClicked != null)
            {
                RealDataMouseDoubleClicked(sender, e);
            }
        }

        private void dgHangqing_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = CommonUtil.GetClickedCell(e);
            if (cell == null)
            {
                return;
            }

            OptionRealData record = cell.DataContext as OptionRealData;
            RealData record2 = null;

            if (cell.Column.DisplayIndex < _ColumnF)
            {
                LvOptQuotesPanel.lblCode.Content = record.Code_C;
                record2 = record.GetOptRealData_C();

                string tempKey = record.Code_C + "_" + CodeSetManager.ExNameToCtp(record.Market);
                if (_BackupCodeDic.ContainsKey(tempKey))
                {
                    LvOptQuotesPanel.SetLevelsQuotesByRealData(_BackupCodeDic[tempKey]);
                }

            }
            else if (cell.Column.DisplayIndex > _ColumnF)
            {
                LvOptQuotesPanel.lblCode.Content = record.Code_P;
                record2 = record.GetOptRealData_P();

                string tempKey = record.Code_P + "_" + CodeSetManager.ExNameToCtp(record.Market);
                if (_BackupCodeDic.ContainsKey(tempKey))
                {
                    LvOptQuotesPanel.SetLevelsQuotesByRealData(_BackupCodeDic[tempKey]);
                }
            }

            _LastSelectedColumnForQuote = cell.Column;
            string buySell = "";
            Boolean isBuySell = false;
            if (cell.Column == colBuyPrice_C || cell.Column == colBuyCount_C
                || cell.Column == colBuyPrice_P || cell.Column == colBuyCount_P)
            {
                buySell = "卖";
                isBuySell = true;

            }
            else if (cell.Column == colSellPrice_C || cell.Column == colSellCount_C
                || cell.Column == colSellPrice_P || cell.Column == colSellCount_P)
            {
                buySell = "买";
                isBuySell = true;
            }
            else if (cell.Column == colExecutePrice)
            {
                record2 = _LatestDisplayRealData;
                return;
            }

            //点击最新价
            double sNewPrice = 0;
            if (cell.Column == colNewPrice_C)
            {
                sNewPrice = record.INewPrice_C;
            }
            else if (cell.Column == colNewPrice_P)
            {
                sNewPrice = record.INewPrice_P;
            }
            else if (cell.Column == colExecutePrice)
            {
                sNewPrice = record.ExecutePrice;
            }

            ////点击涨停价和跌停价时使用指定价下单
            double fixPrice = 0;
            //if (cell.Column == colMaxPrice || cell.Column == colGroupMaxPrice)
            //{
            //    buySell = "买";
            //    fixPrice = record.UpStopPrice;
            //}
            //else if (cell.Column == colMinPrice || cell.Column == colGroupMinPrice)
            //{
            //    buySell = "卖";
            //    fixPrice = record.DownStopPrice;
            //}
            //今开盘和昨结算
            double sJkpPrice = 0;
            double sZjsPrice = 0;
            //if (cell.Column == colJkpPrice || cell.Column == colGroupJkpPrice)
            //{
            //    sJkpPrice = record.Open;
            //}
            //else if (cell.Column == colZjsPrice || cell.Column == colGroupZjsPrice)
            //{
            //    sZjsPrice = record.PrevSettleMent;
            //}
            //点击最高价和最低价显示指定价
            double sHighPrice = 0;
            double sLowPrice = 0;
            //if (cell.Column == colHightPrice || cell.Column == colGroupHightPrice)
            //{
            //    sHighPrice = record.IMaxPrice;
            //}
            //else if (cell.Column == colLowPrice || cell.Column == colGroupLowPrice)
            //{
            //    sLowPrice = record.IMinPrice;
            //}
            //点击昨收盘和今收盘
            double sZspPrice = 0;
            double sJspPrice = 0;
            //if (cell.Column == colZspPrice || cell.Column == colGroupZspPrice)
            //{
            //    sZspPrice = record.PrevSettleMent;
            //}
            //else if (cell.Column == colJspPrice || cell.Column == colGroupJspPrice)
            //{
            //    sJspPrice = record.IClose;
            //}
            //点击结算价
            double sJsjPrice = 0;
            double sDrjjPrice = 0;
            //if (cell.Column == colJsjPrice || cell.Column == colGroupJsjPrice)
            //{
            //    sJsjPrice = record.ISettlementPrice;
            //}
            //else if (cell.Column == colDrjjPrice || cell.Column == colGroupDrjjPrice)
            //{
            //    sDrjjPrice = record.AvgPrice;
            //}

            DataGrid dg = sender as DataGrid;
            if (dg != null)
            {
                ObservableCollection<OptionQuotes> posInfoTotalList = dg.ItemsSource as ObservableCollection<OptionQuotes>;
                dg.SelectedItems.Clear();
                dg.SelectedItems.Add(record);
            }


            if (RealDataMouseLeftButtonDown != null)
            {
                RealDataMouseLeftButtonDown(buySell, record2, fixPrice, sNewPrice, sJkpPrice, sZjsPrice, sHighPrice, sLowPrice, sZspPrice, sJspPrice, sJsjPrice, sDrjjPrice, isBuySell);
            }
        }

        private void ReleatedQutoes_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Label newLb = sender as Label;

            //OptionRealData record = cell.DataContext as OptionRealData;
            RealData record = new RealData();
            record.CodeInfo = CodeSetManager.GetContractInfo(titleCode.Content.ToString());
            record.NewPrice = Double.Parse(tbNewPrice.Text);
            record.BidPrice[0] = Double.Parse(tbBuyPrice.Content.ToString());
            record.AskPrice[0] = Double.Parse(tbSellPrice.Content.ToString());

            string buySell = "";
            Boolean isBuySell = false;
            if (newLb.Name.Contains("Buy"))
            {
                buySell = "卖";
                isBuySell = true;

            }
            else if (newLb.Name.Contains("Sell"))
            {
                buySell = "买";
                isBuySell = true;
            }
            else
            {
                record = _LatestDisplayRealData;
                return;
            }

            //点击最新价
            double sNewPrice = 0;
            //if (newTb.Name.Contains("New"))
            //{
            //    sNewPrice = record.newprice;
            //}

            //点击涨停价和跌停价时使用指定价下单
            double fixPrice = 0;

            //今开盘和昨结算
            double sJkpPrice = 0;
            double sZjsPrice = 0;

            //点击最高价和最低价显示指定价
            double sHighPrice = 0;
            double sLowPrice = 0;

            //点击昨收盘和今收盘
            double sZspPrice = 0;
            double sJspPrice = 0;

            //点击结算价
            double sJsjPrice = 0;
            double sDrjjPrice = 0;


            if (RealDataMouseLeftButtonDown != null)
            {
                RealDataMouseLeftButtonDown(buySell, record, fixPrice, sNewPrice, sJkpPrice, sZjsPrice, sHighPrice, sLowPrice, sZspPrice, sJspPrice, sJsjPrice, sDrjjPrice, isBuySell);
            }
        }

        /// <summary>
        /// 买价，卖价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseEnter_Open(object sender, MouseEventArgs e)
        {

        }

        /// <summary>
        /// 买量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseEnter_Close_Buy_Call(object sender, MouseEventArgs e)
        {
            //Grid grid = sender as Grid;
            //if (grid == null) return;
            //TextBlock tp = grid.ToolTip as TextBlock;
            //OptionRealData displayRealData = grid.DataContext as OptionRealData;
            ////卖出平仓
            //JYDataServer jyDataServer = JYDataServer.getServerInstance();
            //int todayPosCount;
            //int lastPosCount;
            //string code = displayRealData.Code_C;
            ////查找多头的单子
            //if (jyDataServer.GetPositionCount(code, true, out todayPosCount, out lastPosCount) == false)
            //{
            //    grid.ToolTip = null;
            //    return;
            //}
            //if (todayPosCount == 0 && lastPosCount == 0)
            //{
            //    grid.ToolTip = null;
            //    return;
            //}
            //if (tp == null)
            //{
            //    grid.ToolTip = new TextBlock();
            //    tp = grid.ToolTip as TextBlock;
            //}
            //tp.Inlines.Clear();
            //Run run = new Run();
            //run.Text = "双击 ";
            //tp.Inlines.Add(run);

            //run = new Run();
            //Binding binding1 = new Binding();
            //binding1.Source = grid.DataContext;
            //binding1.Mode = BindingMode.OneWay;
            //binding1.Path = new PropertyPath("StBuyPrice_C");
            //run.SetBinding(Run.TextProperty, binding1);
            //tp.Inlines.Add(run);

            //run = new Run();
            //run.Text = " 卖出";
            //run.Foreground = new SolidColorBrush(Color.FromRgb(0x13, 0xaf, 0x13));
            //tp.Inlines.Add(run);

            //Boolean bHasComma = false;
            //if (CodeSetManager.IsCloseTodaySupport(code) && todayPosCount != 0)
            //{
            //    //如果支持平今
            //    run = new Run();
            //    run.Text = "平今";
            //    run.FontSize = 13;
            //    run.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0xff));
            //    tp.Inlines.Add(run);

            //    run = new Run();
            //    run.Text = " " + todayPosCount.ToString() + " ";
            //    run.Foreground = new SolidColorBrush(Color.FromRgb(0x13, 0xaf, 0x13));
            //    tp.Inlines.Add(run);

            //    bHasComma = true;
            //}
            //else
            //{
            //    lastPosCount += todayPosCount;
            //}
            //if (lastPosCount != 0)
            //{
            //    if (bHasComma)
            //    {
            //        run = new Run();
            //        run.Text = ",";
            //        tp.Inlines.Add(run);
            //    }

            //    run = new Run();
            //    run.Text = " 平仓";
            //    run.FontSize = 13;
            //    run.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0xff));
            //    tp.Inlines.Add(run);

            //    run = new Run();
            //    run.Text = " " + lastPosCount.ToString() + " ";
            //    run.Foreground = new SolidColorBrush(Color.FromRgb(0x13, 0xaf, 0x13));
            //    tp.Inlines.Add(run);
            //}

            //run = new Run();
            //run.Text = "手" + code;
            //tp.FontWeight = FontWeights.Bold;
            //tp.Inlines.Add(run);
        }

        private void Grid_MouseEnter_Close_Buy_Put(object sender, MouseEventArgs e)
        {
            //Grid grid = sender as Grid;
            //if (grid == null) return;
            //TextBlock tp = grid.ToolTip as TextBlock;
            //OptionRealData displayRealData = grid.DataContext as OptionRealData;
            ////卖出平仓
            //JYDataServer jyDataServer = JYDataServer.getServerInstance();
            //int todayPosCount;
            //int lastPosCount;
            //string code = displayRealData.Code_P;
            ////查找多头的单子
            //if (jyDataServer.GetPositionCount(code, true, out todayPosCount, out lastPosCount) == false)
            //{
            //    grid.ToolTip = null;
            //    return;
            //}
            //if (todayPosCount == 0 && lastPosCount == 0)
            //{
            //    grid.ToolTip = null;
            //    return;
            //}
            //if (tp == null)
            //{
            //    grid.ToolTip = new TextBlock();
            //    tp = grid.ToolTip as TextBlock;
            //}
            //tp.Inlines.Clear();
            //Run run = new Run();
            //run.Text = "双击 ";
            //tp.Inlines.Add(run);

            //run = new Run();
            //Binding binding1 = new Binding();
            //binding1.Source = grid.DataContext;
            //binding1.Mode = BindingMode.OneWay;
            //binding1.Path = new PropertyPath("StBuyPrice_P");
            //run.SetBinding(Run.TextProperty, binding1);
            //tp.Inlines.Add(run);

            //run = new Run();
            //run.Text = " 卖出";
            //run.Foreground = new SolidColorBrush(Color.FromRgb(0x13, 0xaf, 0x13));
            //tp.Inlines.Add(run);

            //Boolean bHasComma = false;
            //if (CodeSetManager.IsCloseTodaySupport(code) && todayPosCount != 0)
            //{
            //    //如果支持平今
            //    run = new Run();
            //    run.Text = "平今";
            //    run.FontSize = 13;
            //    run.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0xff));
            //    tp.Inlines.Add(run);

            //    run = new Run();
            //    run.Text = " " + todayPosCount.ToString() + " ";
            //    run.Foreground = new SolidColorBrush(Color.FromRgb(0x13, 0xaf, 0x13));
            //    tp.Inlines.Add(run);

            //    bHasComma = true;
            //}
            //else
            //{
            //    lastPosCount += todayPosCount;
            //}
            //if (lastPosCount != 0)
            //{
            //    if (bHasComma)
            //    {
            //        run = new Run();
            //        run.Text = ",";
            //        tp.Inlines.Add(run);
            //    }

            //    run = new Run();
            //    run.Text = " 平仓";
            //    run.FontSize = 13;
            //    run.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0xff));
            //    tp.Inlines.Add(run);

            //    run = new Run();
            //    run.Text = " " + lastPosCount.ToString() + " ";
            //    run.Foreground = new SolidColorBrush(Color.FromRgb(0x13, 0xaf, 0x13));
            //    tp.Inlines.Add(run);
            //}

            //run = new Run();
            //run.Text = "手" + code;
            //tp.FontWeight = FontWeights.Bold;
            //tp.Inlines.Add(run);
        }

        /// 卖量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseEnter_Close_Sell_Call(object sender, MouseEventArgs e)
        {
            //Grid grid = sender as Grid;
            //if (grid == null) return;
            //TextBlock tp = grid.ToolTip as TextBlock;
            //OptionRealData displayRealData = grid.DataContext as OptionRealData;
            ////买入平仓
            //JYDataServer jyDataServer = JYDataServer.getServerInstance();
            //int todayPosCount;
            //int lastPosCount;
            //string code = displayRealData.Code_C;
            //if (jyDataServer.GetPositionCount(code, false, out todayPosCount, out lastPosCount) == false)
            //{
            //    grid.ToolTip = null;
            //    return;
            //}
            //if (todayPosCount == 0 && lastPosCount == 0)
            //{
            //    grid.ToolTip = null;
            //    return;
            //}
            //if (tp == null)
            //{
            //    grid.ToolTip = new TextBlock();
            //    tp = grid.ToolTip as TextBlock;
            //}
            //tp.Inlines.Clear();
            //Run run = new Run();
            //run.Text = "双击 ";
            //tp.Inlines.Add(run);

            //run = new Run();
            //Binding binding1 = new Binding();
            //binding1.Source = grid.DataContext;
            //binding1.Mode = BindingMode.OneWay;
            //binding1.Path = new PropertyPath("StSellPrice_C");
            //run.SetBinding(Run.TextProperty, binding1);
            //tp.Inlines.Add(run);

            //run = new Run();
            //run.Text = " 买入";
            //run.Foreground = new SolidColorBrush(Colors.Red);
            //tp.Inlines.Add(run);

            //Boolean bHasComma = false;
            //if (CodeSetManager.IsCloseTodaySupport(code) && todayPosCount != 0)
            //{
            //    //如果支持平今
            //    run = new Run();
            //    run.Text = "平今";
            //    run.FontSize = 13;
            //    run.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0xff));
            //    tp.Inlines.Add(run);

            //    run = new Run();
            //    run.Text = " " + todayPosCount.ToString() + " ";
            //    run.Foreground = new SolidColorBrush(Colors.Red);
            //    tp.Inlines.Add(run);

            //    bHasComma = true;
            //}
            //else
            //{
            //    lastPosCount += todayPosCount;
            //}
            //if (lastPosCount != 0)
            //{
            //    if (bHasComma)
            //    {
            //        run = new Run();
            //        run.Text = ",";
            //        tp.Inlines.Add(run);
            //    }

            //    run = new Run();
            //    run.Text = " 平仓";
            //    run.FontSize = 13;
            //    run.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0xff));
            //    tp.Inlines.Add(run);

            //    run = new Run();
            //    run.Text = " " + lastPosCount.ToString() + " ";
            //    run.Foreground = new SolidColorBrush(Colors.Red);
            //    tp.Inlines.Add(run);
            //}

            //run = new Run();
            //run.Text = "手" + code;
            //tp.FontWeight = FontWeights.Bold;
            //tp.Inlines.Add(run);
        }

        private void Grid_MouseEnter_Close_Sell_Put(object sender, MouseEventArgs e)
        {
            //Grid grid = sender as Grid;
            //if (grid == null) return;
            //TextBlock tp = grid.ToolTip as TextBlock;
            //OptionRealData displayRealData = grid.DataContext as OptionRealData;
            ////买入平仓
            //JYDataServer jyDataServer = JYDataServer.getServerInstance();
            //int todayPosCount;
            //int lastPosCount;
            //string code = displayRealData.Code_P;
            //if (jyDataServer.GetPositionCount(code, false, out todayPosCount, out lastPosCount) == false)
            //{
            //    grid.ToolTip = null;
            //    return;
            //}
            //if (todayPosCount == 0 && lastPosCount == 0)
            //{
            //    grid.ToolTip = null;
            //    return;
            //}
            //if (tp == null)
            //{
            //    grid.ToolTip = new TextBlock();
            //    tp = grid.ToolTip as TextBlock;
            //}
            //tp.Inlines.Clear();
            //Run run = new Run();
            //run.Text = "双击 ";
            //tp.Inlines.Add(run);

            //run = new Run();
            //Binding binding1 = new Binding();
            //binding1.Source = grid.DataContext;
            //binding1.Mode = BindingMode.OneWay;
            //binding1.Path = new PropertyPath("StSellPrice_P");
            //run.SetBinding(Run.TextProperty, binding1);
            //tp.Inlines.Add(run);

            //run = new Run();
            //run.Text = " 买入";
            //run.Foreground = new SolidColorBrush(Colors.Red);
            //tp.Inlines.Add(run);

            //Boolean bHasComma = false;
            //if (CodeSetManager.IsCloseTodaySupport(code) && todayPosCount != 0)
            //{
            //    //如果支持平今
            //    run = new Run();
            //    run.Text = "平今";
            //    run.FontSize = 13;
            //    run.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0xff));
            //    tp.Inlines.Add(run);

            //    run = new Run();
            //    run.Text = " " + todayPosCount.ToString() + " ";
            //    run.Foreground = new SolidColorBrush(Colors.Red);
            //    tp.Inlines.Add(run);

            //    bHasComma = true;
            //}
            //else
            //{
            //    lastPosCount += todayPosCount;
            //}
            //if (lastPosCount != 0)
            //{
            //    if (bHasComma)
            //    {
            //        run = new Run();
            //        run.Text = ",";
            //        tp.Inlines.Add(run);
            //    }

            //    run = new Run();
            //    run.Text = " 平仓";
            //    run.FontSize = 13;
            //    run.Foreground = new SolidColorBrush(Color.FromRgb(0xff, 0x00, 0xff));
            //    tp.Inlines.Add(run);

            //    run = new Run();
            //    run.Text = " " + lastPosCount.ToString() + " ";
            //    run.Foreground = new SolidColorBrush(Colors.Red);
            //    tp.Inlines.Add(run);
            //}

            //run = new Run();
            //run.Text = "手" + code;
            //tp.FontWeight = FontWeights.Bold;
            //tp.Inlines.Add(run);
        }

        private void dgHangqing_ColumnDisplayIndexChanged(object sender, DataGridColumnEventArgs e)
        {
            dgHangqing.ColumnDisplayIndexChanged -= new EventHandler<DataGridColumnEventArgs>(dgHangqing_ColumnDisplayIndexChanged);

            var sortedColumns = from item in dgHangqing.Columns
                                orderby item.DisplayIndex ascending
                                select item;

            List<DataGridColumn> lstColumnSortByDisplayIndex = new List<DataGridColumn>();

            foreach (var item in sortedColumns)
            {
                lstColumnSortByDisplayIndex.Add(item);
            }

            DataGridColumn c = e.Column;
            int columnIndex = 0;

            for (int i = 0; i < dgHangqing.Columns.Count; i++)
            {
                if (dgHangqing.Columns[i] == c)
                {
                    columnIndex = i;
                }
            }

            if (columnIndex < _ColumnF && c.DisplayIndex >= _ColumnF)
            {
                c.DisplayIndex = _ColumnEndC;
            }
            else if (columnIndex == _ColumnF && c.DisplayIndex < _ColumnF)
            {
                c.DisplayIndex = _ColumnF;
            }
            else if (columnIndex == _ColumnF && c.DisplayIndex > _ColumnF)
            {
                c.DisplayIndex = _ColumnF;
            }
            if (columnIndex > _ColumnF && c.DisplayIndex <= _ColumnF)
            {
                c.DisplayIndex = _ColumnF + 1;
                colExecutePrice.DisplayIndex = _ColumnF;
            }

            dgHangqing.ColumnDisplayIndexChanged += new EventHandler<DataGridColumnEventArgs>(dgHangqing_ColumnDisplayIndexChanged);

        }

        private void dgHangqing_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = false;
        }

        //同步两个datagrid的滚动框，但是不知道为什么scrollh.ScrollToHorizontalOffset(scroll.HorizontalOffset);没有效果
        //最终在两个datagrid外面套一个scrollview来解决
        private void dgHangqing_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dgHangqing); i++)
            {
                if (VisualTreeHelper.GetChild(dgHangqing, i) is Border)
                {
                    Border b = VisualTreeHelper.GetChild(dgHangqing, i) as Border;

                    if (VisualTreeHelper.GetChild(b, i) is ScrollViewer)
                    {
                        ScrollViewer scroll =
                            (ScrollViewer)(VisualTreeHelper.GetChild(b, i));
                        //Util.Log(scroll.VerticalAlignment.ToString());

                        for (int j = 0; j < VisualTreeHelper.GetChildrenCount(dgHeader); j++)
                        {
                            Border bh = VisualTreeHelper.GetChild(dgHeader, j) as Border;
                            if (VisualTreeHelper.GetChild(bh, j) is ScrollViewer)
                            {
                                ScrollViewer scrollh =
                                    (ScrollViewer)(VisualTreeHelper.GetChild(bh, j));
                                double position = scroll.HorizontalOffset;
                                scrollh.ScrollToHorizontalOffset(position);
                                scrollh.UpdateLayout();
                                dgHeader.UpdateLayout();
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void CbxShowLevels_Checked(object sender, RoutedEventArgs e)
        {
            if (LvOptQuotesPanel != null)
            {
                LvOptQuotesPanel.Visibility = Visibility.Visible;
            }
        }

        private void CbxShowLevels_Unchecked(object sender, RoutedEventArgs e)
        {
            if (LvOptQuotesPanel != null)
            {
                LvOptQuotesPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void CbxGreekLetters_Checked(object sender, RoutedEventArgs e)
        {
            colThetaC.Visibility = colVegaC.Visibility = colGammaC.Visibility = colDeltaC.Visibility = colSigmaC.Visibility = Visibility.Visible;
            colThetaP.Visibility = colVegaP.Visibility = colGammaP.Visibility = colDeltaP.Visibility = colSigmaP.Visibility = Visibility.Visible;
        }

        private void CbxGreekLetters_Unchecked(object sender, RoutedEventArgs e)
        {
            colThetaC.Visibility = colVegaC.Visibility = colGammaC.Visibility = colDeltaC.Visibility = colSigmaC.Visibility = Visibility.Collapsed;
            colThetaP.Visibility = colVegaP.Visibility = colGammaP.Visibility = colDeltaP.Visibility = colSigmaP.Visibility = Visibility.Collapsed;
        }

        private void BtnReqQuote_Click(object sender, RoutedEventArgs e)
        {
            if (dgHangqing.SelectedItem != null && _LastSelectedColumnForQuote != null)
            {
                OptionRealData optRecord = dgHangqing.SelectedItem as OptionRealData;
                string code = "";

                if (_LastSelectedColumnForQuote.DisplayIndex < _ColumnF)
                {
                    code = optRecord.Code_C;
                }
                else if (_LastSelectedColumnForQuote.DisplayIndex > _ColumnF)
                {
                    code = optRecord.Code_P;
                }
                else
                {
                    return;
                }
                TradeDataClient.GetClientInstance().RequestOrder("", BACKENDTYPE.CTP, new RequestContent("NewQryQuote", new List<object>() { code }));
            }
        }

        private void dgHangqing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (dgHangqing.SelectedItem != null)
            //{
            //    OptionRealData optRecord = dgHangqing.SelectedItem as OptionRealData;

            //    if (_LastSelectedColumnForQuote.DisplayIndex < _ColumnF)
            //    {
            //        LvOptQuotesPanel.lblCode.Content = optRecord.Code_C;
            //        RealData selectedData = new RealData();
            //        if (selectedData.UpdateCallOptionRealProperties(optRecord))
            //        {
            //            LvOptQuotesPanel.SetLevelsQuotesByRealData(selectedData);
            //        }
            //    }
            //    else if (_LastSelectedColumnForQuote.DisplayIndex > _ColumnF)
            //    {
            //        LvOptQuotesPanel.lblCode.Content = optRecord.Code_P;
            //        RealData selectedData = new RealData();
            //        if (selectedData.UpdatePutOptionRealProperties(optRecord))
            //        {
            //            LvOptQuotesPanel.SetLevelsQuotesByRealData(selectedData);
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
        }

    }
}