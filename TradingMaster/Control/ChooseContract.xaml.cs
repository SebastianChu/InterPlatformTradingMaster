using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TradingMaster.CodeSet;

namespace TradingMaster.Control
{
    /// <summary>
    /// ChooseContract.xaml 的交互逻辑
    /// </summary>
    public partial class ChooseContract : UserControl
    {
        /// <summary>
        /// 设置是否保存过，如果没有保存，则在关闭时需要重新载入之前保存的合约组数据
        /// </summary>
        public bool IsUpdated { get; set; }
        private ObservableCollection<Contract> _AreaCodeSet = new ObservableCollection<Contract>();
        private ObservableCollection<Contract> _ChooseSet;
        private string _NeedUpdatePriceCode = string.Empty;
        private string _CurrentContractType = string.Empty;

        public int PriceType { get; set; }

        public ChooseContract()
        {
            InitializeComponent();
            //CodeSet.Init();

            //chooseSet = UserCodeSetInstance.GetContractListByUserCodeSet(UserCodeSetInstance.GetUserCodeSetList()[0]);

            this.dgChooseCode.ItemsSource = _AreaCodeSet;
            //this.dgChoosedCode.ItemsSource = chooseSet;

            cbUserCodeSet.DisplayMemberPath = "Name";
            cbUserCodeSet.SelectedValuePath = "Id";
            cbUserCodeSet.ItemsSource = UserCodeSetInstance.GetUserCodeSetList();
            IsUpdated = false;

            //string selectedUserCodeSet = JYDataServer.getServerInstance().getMainWindow().userCodeSetBar.getSelectedUserCodeSetId();
            //cbUserCodeSet.SelectedValue = selectedUserCodeSet;
        }

        private void bt_add_Click(object sender, RoutedEventArgs e)
        {
            if (CommonUtil.IsValidCode(txtCode.Text))
            {
                Contract orderCodeIndo = CodeSetManager.GetContractInfo(txtCode.Text);
                if (!_ChooseSet.Contains(orderCodeIndo))
                {
                    _ChooseSet.Add(orderCodeIndo);
                }
                txtCode.Text = string.Empty;
                return;
            }

            AddContractFromChooseSet();
        }

        private void AddContractFromChooseSet()
        {
            List<Contract> selectedContract = new List<Contract>();

            foreach (object temp in dgChooseCode.SelectedItems)
            {
                selectedContract.Add(temp as Contract);
            }

            foreach (Contract temp in selectedContract)
            {
                if (!_ChooseSet.Contains(temp))
                {
                    _ChooseSet.Add(temp);
                }
            }
        }


        private void bt_del_Click(object sender, RoutedEventArgs e)
        {
            List<Contract> selectedContract = new List<Contract>();

            foreach (object temp in dgChoosedCode.SelectedItems)
            {
                selectedContract.Add(temp as Contract);
            }

            foreach (var temp in selectedContract)
            {
                _ChooseSet.Remove(temp);
            }
        }

        private void bt_moveUp_Click(object sender, RoutedEventArgs e)
        {
            //Contract moveTemp = dgChoosedCode.SelectedItem as Contract;
            //int index=chooseSet.IndexOf(moveTemp);
            //if (index>0)
            //{
            //    chooseSet.Move(index, index - 1);
            //}
            MoveSelectedItemsUpFromDataGrid();
        }

        private void MoveSelectedItemsUpFromDataGrid()
        {
            ObservableCollection<Contract> itemsource = dgChoosedCode.ItemsSource as ObservableCollection<Contract>;

            List<Contract> selectedItems = new List<Contract>();
            GetSelectedItems(itemsource, selectedItems);


            for (int i = 0; i < selectedItems.Count; i++)
            {
                int index = itemsource.IndexOf(selectedItems[i]);
                if (index == 0)
                {
                    continue;
                }
                else
                {
                    if (selectedItems.Contains(itemsource[index - 1]))
                    {
                        continue;
                    }
                    else
                    {
                        itemsource.Move(index, index - 1);
                    }
                }
            }
        }


        private void MoveSelectedItemsDownFromDataGrid()
        {
            ObservableCollection<Contract> itemsource = dgChoosedCode.ItemsSource as ObservableCollection<Contract>;

            List<Contract> selectedItems = new List<Contract>();
            GetSelectedItems(itemsource, selectedItems);


            for (int i = selectedItems.Count - 1; i >= 0; i--)
            {
                int index = itemsource.IndexOf(selectedItems[i]);
                if (index == itemsource.Count - 1)
                {
                    continue;
                }
                else
                {
                    if (selectedItems.Contains(itemsource[index + 1]))
                    {
                        continue;
                    }
                    else
                    {
                        itemsource.Move(index, index + 1);
                    }
                }
            }
        }
        private void GetSelectedItems(ObservableCollection<Contract> itemsource, List<Contract> selectedItems)
        {
            //获得选中项，并且要求选中项按照datagrid中的状态排序
            foreach (var item in dgChoosedCode.SelectedItems)
            {
                if (selectedItems.Count == 0)
                {
                    selectedItems.Add(item as Contract);
                    continue;
                }
                bool hasInserted = false;
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    if (itemsource.IndexOf(item as Contract) < itemsource.IndexOf(selectedItems[i]))
                    {
                        selectedItems.Insert(i, (item as Contract));
                        hasInserted = true;
                        break;
                    }
                }
                if (!hasInserted)
                {
                    selectedItems.Add(item as Contract);
                }
            }
        }

        private void bt_moveDown_Click(object sender, RoutedEventArgs e)
        {
            MoveSelectedItemsDownFromDataGrid();
        }

        private void cb_area_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = cb_area.SelectedIndex;
            switch (selectedIndex)
            {
                case 1:
                    GetContract("DCE");
                    break;
                case 2:
                    GetContract("SHFE");
                    break;
                case 3:
                    GetContract("CFFEX");
                    break;
                default:
                    GetContract("CZCE");
                    break;
            }
        }

        private void GetContract(string market)
        {
            Contract[] codeArray = CodeSet.CodeSetManager.GetMarketCodeList(market).ToArray();
            if (_AreaCodeSet != null)
            {
                _AreaCodeSet.Clear();
            }
            foreach (Contract temp in codeArray)
            {
                if (temp.ProductType == _CurrentContractType)
                {
                    _AreaCodeSet.Add(temp);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetContract("CZCE");
        }

        private void bt_ok_Click(object sender, RoutedEventArgs e)
        {
            //save all
            Save();
            (this.Parent as Window).Close();
        }

        public bool Save()
        {
            try
            {
                UserCodeSetInstance.SaveStyleToFile();
                UserCodeSet userCodeSet = cbUserCodeSet.SelectedItem as UserCodeSet;
                string userCodeSetId = null;
                if (userCodeSet != null)
                {
                    userCodeSetId = userCodeSet.Id;
                }
                TradeDataClient.GetClientInstance().getMainWindow().uscHangqing.ResetBlockButtons();
                //JYDataServer.getServerInstance().getMainWindow().userCodeSetBar.ResetUserCodeSetButtons(userCodeSetId);
                IsUpdated = true;
                return true;
            }
            catch (Exception ex)
            {
                Util.Log(ex.ToString());
                return false;
            }
        }
        private void bt_cancle_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Window).Close();
        }

        private void dgChoosedCodet_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            bt_del_Click(null, null);
        }

        private void dgChooseCode_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddContractFromChooseSet();
        }

        private void bt_SetCodeSet_Click(object sender, RoutedEventArgs e)
        {
            //UserCodeSetEdit editControl = new UserCodeSetEdit();
            //editControl.DataContext = this.DataContext;
            //Window window = CommonUtil.GetWindow("", editControl, this.Parent as Window);
            //window.Closed += new EventHandler(userCodeSetEditWindow_Closed);
            //window.ShowDialog();
        }

        void userCodeSetEditWindow_Closed(object sender, EventArgs e)
        {
            UserCodeSetInstance.RemoveEmptyUserCodeSet();
        }

        private void cbUserCodeSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbUserCodeSet.SelectedItem != null)
            {
                _ChooseSet = UserCodeSetInstance.GetContractListByUserCodeSet((cbUserCodeSet.SelectedItem as UserCodeSet).Id);
                this.dgChoosedCode.ItemsSource = _ChooseSet;
                switch (cbUserCodeSet.SelectedIndex)
                {
                    case 0:
                        _CurrentContractType = "Futures";
                        break;
                    case 1:
                        _CurrentContractType = "Combination";
                        break;
                }
                int i = cb_area.SelectedIndex;
                cb_area.SelectedItem = null;
                cb_area.SelectedIndex = i;
            }
        }

        private void txtCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtCode.SelectionStart = txtCode.Text.Length;
            int firstNumberIndiex = txtCode.Text.IndexOfAny("0123456789".ToCharArray());
            string SpeciesName = txtCode.Text;
            string SpeciesDate = string.Empty;
            if (firstNumberIndiex >= 0)
            {
                SpeciesName = txtCode.Text.Substring(0, firstNumberIndiex);
                SpeciesDate = txtCode.Text.Substring(firstNumberIndiex, txtCode.Text.Length - firstNumberIndiex);

                //用户可能不区分大小写输入合约
                string validSpeciesName = CodeSetManager.GetValidSpeciesName(SpeciesName);
                if (validSpeciesName != null && validSpeciesName != SpeciesName)
                {
                    txtCode.Text = validSpeciesName + SpeciesDate;
                    return;
                }
            }
        }


        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;
            if (image == null) return;
            if (image.Name == "img_add")
            {
                bt_add_Click(null, null);
            }
            else if (image.Name == "img_remove")
            {
                bt_del_Click(null, null);
            }
            else if (image.Name == "img_up")
            {
                bt_moveUp_Click(null, null);
            }
            else if (image.Name == "img_down")
            {
                bt_moveDown_Click(null, null);
            }
        }

        private void img_add_MouseEnter(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            if (image == null) return;
            if (image.Name == "img_add")
            {
                img_add.Source = CommonUtil.getImageSource(new Uri("/TradingMaster;component/image/btn_left_s.png", UriKind.RelativeOrAbsolute));
            }
            else if (image.Name == "img_remove")
            {
                img_remove.Source = CommonUtil.getImageSource(new Uri("/TradingMaster;component/image/btn_right_s.png", UriKind.RelativeOrAbsolute));
            }
            else if (image.Name == "img_up")
            {
                img_up.Source = CommonUtil.getImageSource(new Uri("/TradingMaster;component/image/btn_up_s.png", UriKind.RelativeOrAbsolute));
            }
            else if (image.Name == "img_down")
            {
                img_down.Source = CommonUtil.getImageSource(new Uri("/TradingMaster;component/image/btn_down_s.png", UriKind.RelativeOrAbsolute));
            }
        }

        private void img_add_MouseLeave(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            if (image == null) return;
            if (image.Name == "img_add")
            {
                img_add.Source = CommonUtil.getImageSource(new Uri("/TradingMaster;component/image/btn_left.png", UriKind.RelativeOrAbsolute));
            }
            else if (image.Name == "img_remove")
            {
                img_remove.Source = CommonUtil.getImageSource(new Uri("/TradingMaster;component/image/btn_right.png", UriKind.RelativeOrAbsolute));
            }
            else if (image.Name == "img_up")
            {
                img_up.Source = CommonUtil.getImageSource(new Uri("/TradingMaster;component/image/btn_up.png", UriKind.RelativeOrAbsolute));
            }
            else if (image.Name == "img_down")
            {
                img_down.Source = CommonUtil.getImageSource(new Uri("/TradingMaster;component/image/btn_down.png", UriKind.RelativeOrAbsolute));
            }
        }
    }
}