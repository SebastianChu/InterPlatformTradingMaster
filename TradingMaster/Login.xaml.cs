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
using System.Threading;
using System.IO;
using System.Globalization;
using TradingMaster.JYData;

namespace TradingMaster
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        //private bool paneShrinkflag = false;
        public static string USERNAME_FILEPATH
        {
            get
            {
                return CommonUtil.GetSettingPath() + @"Data\UserName_sim.dat";
            }
        }
        public static bool IsTerminated = false;

        private static string LOGON_SUCCESS = "登录成功";

        private string _StrAuthCode = string.Empty;
        //private GlobalCommonUtils commonUtil;
        private List<UserNameInfo> _UserNameList=null;
        private bool _IsRememeberUserName;
        //private ServerSettings serversettings = null;
        //private bool InitTag = false;
        //private bool ExecFlag = false;
        
        //public ObservableCollection<ServerData> ServerData = new ObservableCollection<ServerData>();
        //private bool isDataDirty = false;

        //private Boolean isShowNetworkSetting = false;

        /// <summary>
        /// 连接失败后依次重连的服务器
        /// </summary>
        //private List<ServerData> serverDatasReconnected;

        //#region Windows API Functions
        //// Hiding the Keyboard in the screen 
        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        //#endregion Windows API Functions

        private string _TradeBrokerID = "";
        private string _TradeServerAddr = "";
        private string _QuoteBrokerID = "";
        private string _QuoteServerAddr = "";  

        private static Login _LoginInstace = null;

        public static Login LoginInstace
        {
            get { return Login._LoginInstace; }
            set { Login._LoginInstace = value; }
        }

        private string _Title;
        public string TiTle
        {
            get { return _Title; }
            //set { title = value; }
        }

        public CheckBox CbMemberCount
        {
            get { return cb_memberCount; }
        }

        public PasswordBox PassWord
        {
            get { return pb_passWord; }
        }

        public void UpdatePassWord(Boolean isUpdateSuccess)
        {
            if (isUpdateSuccess)
            {
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        //PassWord.Password = TradeClientAppWrapper.TradeClientApp.newPassword;
                    });
                }
            }
        }

        public string GetAuthCode()
        {
            return _StrAuthCode;
        }

        //public ComboBox TbUserName
        public TextBox TbUserName
        {
            get { return tb_userName; }
        }
        
        
        public TextBlock TbStatusMessage
        {
            get { return StatusMessage; }
        }

        private Thread statusMsgThread = null;


        //public static string GetVesrion()
        //{
        //    String ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        //    string[] fs = ver.Split('.');
        //    if (fs.Length >= 2)
        //    {
        //        ver = fs[0] + "." + fs[1];
        //    }
        //    return ver;
        //}

        private void DelegateInitOver(Boolean hasError)
        {
            if (hasError == true)
            {
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        System.Windows.Forms.MessageBox.Show("无法从网络访问合约文件，程序即将关闭！\n请检查网络后重启该程序。", "错误", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        Environment.Exit(0);
                    });
                }
            }
        }

        private Guid _Guid;
        private System.Timers.Timer _LoginTicker = new System.Timers.Timer(20000);

        BACKENDTYPE _BackEnd = BACKENDTYPE.CTP;

        public Login()
        {
            //serverDatasReconnected = new List<SingleLogin.ServerData>();
            //UpgradeMng.Init();
            //CodeGen.CodeSet.Init(DelegateInitOver);
            
            InitializeComponent();
            //Boolean isSim = CommonUtil.IsSim();

            //title.Text = "实盘登录";
            _Title = "仿真交易登录";
            //StatusMessage.Foreground = new SolidColorBrush(Colors.White);

            _Guid = Guid.NewGuid();
            Util.Log("新建Login:" + _Guid.ToString());



            //System.Diagnostics.FileVersionInfo myFileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo("SingleLogin.dll");
            //title.Text += " " + "v" + GetVesrion();

            InitUserNameInfo();

            ShowAuthCode();
            ServerSettingInit();

            //根据实盘，仿真的结果选择不同的界面
            //rbSim.IsChecked=ServerStruct.IsSim();
            //rbReal.IsChecked=!ServerStruct.IsSim();

            _LoginInstace = this;

            //List<SimpleComboxItem> lstStyle = GetStyleList();
            //cbStyle.SelectedValuePath = "ID";
            //cbStyle.DisplayMemberPath = "Name";
            //cbStyle.ItemsSource = lstStyle;

            //cbStyle.SelectedValue = SingleLogin.Properties.Settings.Default.DefaultStyle;

            ResourceDictionary o = (ResourceDictionary)System.Windows.Application.LoadComponent(new Uri("/TradingMaster;component/Dictionary1.xaml", UriKind.Relative));
            System.Windows.Application.Current.Resources.MergedDictionaries.Add(o);

            //ShowNetworkSetting(false);
            _LoginTicker.Elapsed += new System.Timers.ElapsedEventHandler(LoginTicker_Elapsed);
        }

        void LoginTicker_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    if (this.Visibility == Visibility.Visible && !CtpDataServer.GetUserInstance().TradeServerLogOn)
                    {
                        SetStatusMessage("连接超时，服务器连接中断！", false);
                        _LoginTicker.Close();
                        TradeDataClient.GetClientInstance().RequestTradeData(tb_userName.Text.Trim(), _BackEnd, new RequestContent("RequestTradeDataDisConnect", new List<object>()));
                        bt_ok.IsEnabled = true;
                    }
                });
            }
        }

        private void InitUserNameInfo()
        {
            _UserNameList = ReadUserNameFile(USERNAME_FILEPATH, out _IsRememeberUserName);
            SetUserNameInfoToControl();
        }

        private void ThreadProc_GetMaxOperationCount()
        {
            Log("开始获取单笔最大可操作手数");
        }

        public string GetUserNameDatContent()
        {
            string ret = "";
            ret += cb_memberCount.IsChecked.ToString() + "\n";
            //foreach (string a in tb_userName.Items)
            //{
            //    if (tb_userName.SelectedItem != null)
            //    {
            //        if (tb_userName.SelectedItem.ToString() == a)
            //        {
            //            ret += ":";
            //        }
            //    }
            //    ret += a+"\n";
            //}
            //if (tb_userName.SelectedItem == null && cb_memberCount.IsChecked==true)
            if (tb_userName.Text.Trim() != "" && cb_memberCount.IsChecked == true)
            {
                ret += ":" + tb_userName.Text.Trim();
            }
            return ret;
        }


        /// <summary>
        /// 初始化用户名
        /// </summary>
        /// <param name="memeberUserName"></param>
        /// <param name="USERNAME"></param>
        private void SetUserNameInfoToControl()
        {
            cb_memberCount.IsChecked = _IsRememeberUserName;
            tb_userName.Text = "";//tb_userName.Items.Clear();
            foreach (UserNameInfo ui in _UserNameList)
            {
                //tb_userName.Items.Add(ui.userName);
                if (ui.selected == true)
                {
                    //tb_userName.SelectedItem = ui.userName;
                    tb_userName.Text = ui.userName;
                }
            }
        }

        private List<UserNameInfo> ReadUserNameFile(string fileName, out Boolean isRememberUserName)
        {
            if (File.Exists(fileName))
            {
                try
                {
                    using (StreamReader sr = File.OpenText(fileName))
                    {
                        string isRem = sr.ReadLine();
                        if (isRem.ToUpper() == "TRUE")
                        {
                            isRememberUserName = true;
                        }
                        else
                        {
                            isRememberUserName = false;
                        }

                        string line = "";
                        List < UserNameInfo > ret=new List<UserNameInfo>();
                        while ((line = sr.ReadLine()) != null)
                        {
                            UserNameInfo uif = UserNameInfo.Parse(line);
                            
                            if (uif != null)
                            {
                                if (uif.userName.ToLower() == "true" || uif.userName.ToLower() == "false") continue;
                                ret.Add(uif);
                            }
                        }
                        sr.Close();
                        return ret;
                    }
                }
                catch (Exception ex)
                {
                    Util.Log(ex.Message);
                    Util.Log(ex.StackTrace);
                    isRememberUserName = false;
                    return new List<UserNameInfo>();
                }                
            }
            else
            {
                isRememberUserName = false;
                return new List<UserNameInfo>();
            }


            
        }

        public static Boolean SaveToRiskCheckFile(string username)
        {
            String filename = System.AppDomain.CurrentDomain.BaseDirectory + "setting\\rcf.usr";
            FileStream fs = null;
            StreamReader sreader = null;
            StreamWriter sr = null; 
            

            try
            {
                fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                sreader = new StreamReader(fs);
                string text = sreader.ReadToEnd();

                sr = new StreamWriter(fs);
                sr.WriteLine(username);
            }
            catch (Exception ex)
            {
                Util.Log(ex.Message);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
                if (sreader != null)
                {
                    sreader.Close();
                }
            }
            return true;
        }

        private void bt_ok_Click(object sender, RoutedEventArgs e)
        {
            if (bt_ok.IsEnabled == false)
            {
                Util.Log("Login.bt_ok_Click bt_ok的状态为无效，无法登录");
                return;
            }
            if (pb_passWord.Password == "")
            {
                System.Windows.Forms.MessageBox.Show("请输入密码！", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }
            bt_ok.Focus();
            //Util.Log("按下登录按钮");


            //if (ServerStruct.IsSim()==false)
            {
                //if (IsNeedRisk(tb_userName.Text) == true && sender != null)
                //{
                //    risk = new Control.RiskWindow();
                //    risk.ShowDialog();
                //    if (risk.isChecked == false)
                //    {
                //        return;
                //    }
                //    else
                //    {
                //        SaveToRiskCheckFile(tb_userName.Text);
                //    }
                //}
            }



            if (this.Visibility == Visibility.Visible && AuthCode.Height.Value != 0)
            {
                if (tb_authcode.Text.Equals("") || tb_authcode.Text.Equals(string.Empty))
                {
                    System.Windows.Forms.MessageBox.Show("请输入验证码！", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    if (tb_authcode.Text.ToUpper().Equals(_StrAuthCode.ToUpper()))
                    {
                            //ServerStruct.SaveXml();
                        SetStatusMessage("连接中...",true);
                        bt_ok.IsEnabled = false;
                        //TODO
                        ServerStruct.ChangeSelectedServer();
                        ClientLogin();
                        return;
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("验证码输入错误，请重新输入验证码！", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        return;
                    }
                }
            }
                //ServerStruct.SaveXml();
            if (bt_ok.IsEnabled == true)
            {
                SetStatusMessage("连接中...",true);
                //TODO
                ServerStruct.ChangeSelectedServer();
                ClientLogin();
            }
        }

        /// <summary>
        /// 是否需要风险结束
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private Boolean IsNeedRisk(string username)
        {
            String filename = System.AppDomain.CurrentDomain.BaseDirectory + "setting\\rcf.usr";
            if (File.Exists(filename) == false) return true;
            FileStream fs = null;
            StreamReader sr = null;
            Boolean bRet = true;
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(fs);
                string[] name = sr.ReadToEnd().Split('\n');
                if (name != null)
                {
                    foreach (string s in name)
                    {
                        if (s.Replace("\r","").ToLower().Equals(username.ToLower()))
                        {
                            bRet = false;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.Log(ex.Message);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
                if (sr != null)
                {
                    sr.Close();
                }
            }
            return bRet;
        }

        //private static RiskWindow risk = null;

        private void ClientLogin()
        {
            Util.Log(DateTime.Now.ToString("HH:mm:ss.fff")+ " ClientLogin");

            foreach (char c in tb_userName.Text.ToCharArray())
            {
                if (c >= '0' && c <= '9')
                {
                }
                else
                {
                    SetStatusMessage("账号只能为数字!");
                    bt_ok.IsEnabled = true;
                    return;
                }
            }
            if (pb_passWord.Password == "")
            {
                SetStatusMessage("未输入密码");
                bt_ok.IsEnabled = true;
                return;
            }
            if (tb_userName.Text.Trim().Length > 12)
            {
                SetStatusMessage("账号过长，不合法");
                tb_userName.Text = "";
                pb_passWord.Password = "";
                bt_ok.IsEnabled = true;
                return;
            }
            Thread t = new Thread(delegate()
            {
                string username="";
                string password="";
                //int selValue = 0;
                //string projname = "";
                //BACKENDTYPE backEndType=BACKENDTYPE.HST2;
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        //selValue=(int)cbStyle.SelectedValue;
                        username=tb_userName.Text.Trim();
                        password=pb_passWord.Password;

                        //SimpleComboxItem selectItem = cbStyle.SelectedItem as SimpleComboxItem;
                        //projname = selectItem.Value;

                        //if (cbStyle.Text == "环球期货")
                        //{
                        //    backEndType = BACKENDTYPE.PATS;
                        //}
                        //else
                        //{
                            //backEndType = TradeClientApp.BackEndUser(tb_userName.Text);
                        //}

                    });
                }

                //if (selValue != SingleLogin.Properties.Settings.Default.DefaultStyle)
                //{
                //    SingleLogin.Properties.Settings.Default.DefaultStyle = selValue;
                //    SingleLogin.Properties.Settings.Default.Save();
                //}

                //Util.Log(DateTime.Now.ToString("HH:mm:ss.fff") + " 开始获取TradeClientAppWrapper");
                //TradeClientAppWrapper cliengApp = GetTradeClientApp(projname, backEndType);
                Util.Log(DateTime.Now.ToString("HH:mm:ss.fff") + " 开始连接:用户名:" + username);
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        if (rbSingleUser.IsChecked == true)
                        {
                            TradeDataClient.GetClientInstance().InitiateCounterApi(_BackEnd, username, password, _TradeBrokerID, _TradeServerAddr, _QuoteBrokerID, _QuoteServerAddr);
                        }
                    });
                }
                _LoginTicker.Start();
                //cliengApp.Connect(username, password, "version");
            });
            //Util.Log("开启连接线程");
            bt_ok.IsEnabled = false;
            t.Start();
        }

        //private Boolean IsCTPUser()
        //{
            //根据tb_userName.Text判断
            //return BACKENDTYPE.CTP== TradeClientApp.BackEndUser(tb_userName.Text);
        //}


        //private static BACKENDTYPE currBackEndType;

        static Dictionary<string, ResourceDictionary> dicResource = new Dictionary<string, ResourceDictionary>();


        //private TradeClientAppWrapper GetTradeClientApp(string clientProjectName,BACKENDTYPE backEndType)
        //{
        //    Util.Log("执行GetTradeClientApp");
            
        //    Assembly assembly = null;
           
        //    Util.Log("clientProjectName=" + clientProjectName);
        //    if (!dicResource.Keys.Contains<string>(clientProjectName))
        //    {
        //        Util.Log("资源字典中不存在" + clientProjectName);
        //        ResourceDictionary o = (ResourceDictionary)System.Windows.Application.LoadComponent(new Uri("/" + clientProjectName + ";component/Dictionary1.xaml", UriKind.Relative));
        //        Util.Log("开始调用MergedDictionaries.Add");
        //        System.Windows.Application.Current.Resources.MergedDictionaries.Add(o);
        //        Util.Log("往资源字典中加入o");
        //        dicResource.Add(clientProjectName, o);
        //    }

        //    Util.Log("开始反射，载入:" + CommonUtil.GetSettingPath() + clientProjectName + ".dll");
        //    assembly = Assembly.LoadFile(CommonUtil.GetSettingPath() + clientProjectName + ".dll");
        //    Util.Log("反射载入完毕");
        //    Type type = assembly.GetType(clientProjectName + ".JYDataServer");
        //    MethodInfo method = type.GetMethod("getServerInstanceAdv");

        //    currBackEndType = backEndType;

        //    Util.Log("开始调用getServerInstanceAdv");
        //    TradeClientAppWrapper cliengApp = (TradeClientAppWrapper)method.Invoke(null, new object[] { currBackEndType });//反射访问静态方法         
        //    Util.Log("得到了cliengApp");

        //    return cliengApp;
        //}

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IsTerminated = true;
                //ServerStruct.SaveXml();
            string fileContent = GetUserNameDatContent();
            CommonUtil.writeFile(Login.USERNAME_FILEPATH, fileContent);
            ClearToInit();
            if (TradeDataClient.GetClientInstance().IsTradeServerLogon(_BackEnd, tb_userName.Text.Trim()))
            {
                //JYDataServer.getServerInstance().AddToQryQueue(new CTPRequestContent("ClientLogOff", new List<object>()));
            }
            TradeDataClient.GetClientInstance().RequestTradeData(tb_userName.Text.Trim(), _BackEnd, new RequestContent("RequestTradeDataDisConnect", new List<object>()));
            SetStatusMessage("", false);
            //keybd_event(0x10, 0, 0x2, (UIntPtr)0);
            //Environment.Exit(0);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        public void ClearToInit()
        {
            this.pb_passWord.Clear();
            this.StatusMessage.Text = "";
            bt_ok.IsEnabled = true;
            if (Double.Parse(this.AuthCode.Height.ToString()) > 0)
            {
                this.AuthCode.Height = new GridLength(0);
                this.Height = this.ActualHeight - 50;
            }
        }

        private void bt_authcode_Click(object sender, RoutedEventArgs e)
        {
            ShowAuthCode();
        }

        public void CreateNewAuthCode()
        {
            tb_authcode.Text = string.Empty;
            ShowAuthCode();
        }

        private void ShowAuthCode()
        {
            Random random = new Random();
            this.sp_authcode.Background = new SolidColorBrush(Color.FromRgb(Convert.ToByte(random.Next(255)), Convert.ToByte(random.Next(255)), Convert.ToByte(random.Next(255))));
            this.authcodeImage.Source = getAuthCodeImage(Convert.ToInt32(authcodeImage.Width), Convert.ToInt32(authcodeImage.Height));
        }
        private char[] randAuthCode()
        {
            char[] authCode=new char[4] ;
            char[] authCodeSet ={'a','b','c','d','e','f','g','h','i','j','k','m','n','p','q','r','s','t','u','v','w','x','y','z',
                                 'A','B','C','D','E','F','G','H','I','J','K','M','N','P','Q','R','S','T','U','V','W','X','Y','Z',
                                 '1','2','3','4','5','6','7','8','9','0'};
            Random random = new Random();
            _StrAuthCode = string.Empty;
            for (int i = 0; i < 4;i++ )
            {
                authCode[i] = authCodeSet[random.Next(authCodeSet.Length)];
                _StrAuthCode = _StrAuthCode + authCode[i].ToString();
            }
            return authCode;
        }
        private ImageSource getAuthCodeImage(int width,int height)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            char[] strAuthCode = randAuthCode();
            drawingContext.DrawText(
                new FormattedText(strAuthCode[0].ToString(), CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight, new Typeface("Verdana"), 22, Brushes.Green),
            new Point(8, 0));

            drawingContext.DrawText(
               new FormattedText(strAuthCode[1].ToString(), CultureInfo.GetCultureInfo("en-us"),
                   FlowDirection.LeftToRight, new Typeface("Verdana"), 22, Brushes.Blue),
            new Point(28, 0));

            drawingContext.DrawText(
               new FormattedText(strAuthCode[2].ToString(), CultureInfo.GetCultureInfo("en-us"),
                   FlowDirection.LeftToRight, new Typeface("Verdana"), 22, Brushes.Red),
           new Point(50, 0));


            drawingContext.DrawText(
               new FormattedText(strAuthCode[3].ToString(), CultureInfo.GetCultureInfo("en-us"),
                   FlowDirection.LeftToRight, new Typeface("Verdana"), 22, Brushes.DeepSkyBlue),
           new Point(72, 0));
            //////////////////干扰线///////////////////////////////////////////
            Random random = new Random();
            for (int i = 0; i < 6;i++ )
            {
                Point startPoint = new Point(random.Next(width),random.Next(height-10));
                Point endPoint = new Point(random.Next(width),random.Next(height-4));
                SolidColorBrush brush=new SolidColorBrush(Color.FromRgb(Convert.ToByte(random.Next(255)),Convert.ToByte(random.Next(255)),Convert.ToByte(random.Next(255))));
                Pen linePen=new Pen(brush,1);
                drawingContext.DrawLine(linePen, startPoint, endPoint);
            }
            drawingContext.Close();
            // 利用RenderTargetBitmap对象，以保存图片

            RenderTargetBitmap renderBitmap = new RenderTargetBitmap((int)this.authcodeImage.Width, (int)this.authcodeImage.Height, 96, 96, PixelFormats.Pbgra32);
            renderBitmap.Render(drawingVisual);
            ImageSource source1 = BitmapFrame.Create(renderBitmap);
            return source1;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    (this.Parent as Window).Close();
                    break;
                case Key.Enter:
                    //bt_ok_Click(new Button(), new RoutedEventArgs());
                    break;
                default:
                    break;
            }
        }

        private void imageKeyBoard_Click(object sender, RoutedEventArgs e)
        {
            pb_passWord.Focus();
        }

        private void bt_cancel_Click(object sender, RoutedEventArgs e)
        {
            OnLogOut("");
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                (this.Parent as Window).DragMove();
            }
        }

        /// <summary>
        /// 登出时调用
        /// </summary>
        /// <param name="errMsg"></param>
        public void OnLogOut(string errMsg)
        {
            _LoginTicker.Close();
            if (errMsg == "")
            {
                errMsg = "连接失败，未知错误";
            }

            SetStatusMessage(errMsg);
            if (AuthCode.Height.Value == 0)
            {
                BitmapImage image = new BitmapImage();
                //if (SettingRow.Height.Value != 0)
                //{
                //    //显示设置信息
                //    image.BeginInit();
                //    image.UriSource = GetPicUri(1);
                //    image.EndInit();
                //}
                //else
                //{
                    //不显示设置信息
                    //image.BeginInit();
                    //image.UriSource = GetPicUri(2);
                    //image.EndInit();
                //}
                
                //image1.Source = image;

                AuthCode.Height = new GridLength(50);
                //LoginRow.Height = new GridLength(LoginRow.Height.Value + 36);
                this.Height = this.ActualHeight + 50;                
            }
            bt_ok.IsEnabled = true;
        }

        /// <summary>
        /// 登录成功后使用
        /// </summary>
        public void StopWatch()
        {
            _LoginTicker.Close();
            SetStatusMessage(LOGON_SUCCESS);
            bt_ok.IsEnabled = true;
        }

        /// <summary>
        /// 根据index显示图片的Uri，需要判断是否是模拟的
        /// 1.显示的是LoginBk_2
        /// 2.显示的是LoginBk_2_N
        /// 3.显示的是LoginBk_1
        /// 4.显示的是LoginBk_1_N
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        //private Uri GetPicUri(int index)
        //{
        //    if (index == 1)
        //    {
        //        //return new Uri("/SingleLogin;component/image/SLoginBk_2.jpg", UriKind.RelativeOrAbsolute);
        //        if (ServerStruct.IsSim() == true)
        //        {
        //            return new Uri("/SingleLogin;component/image/LoginBk_2.jpg", UriKind.RelativeOrAbsolute);
        //        }
        //        else
        //        {
        //            return new Uri("/SingleLogin;component/image/SLoginBk_2.jpg", UriKind.RelativeOrAbsolute);
        //        }
        //    }
        //    else if (index == 2)
        //    {
        //        //return new Uri("/SingleLogin;component/image/SLoginBk_2_N.jpg", UriKind.RelativeOrAbsolute);
        //        if (ServerStruct.IsSim() == true)
        //        {
        //            return new Uri("/SingleLogin;component/image/LoginBk_2_N.jpg", UriKind.RelativeOrAbsolute);
        //        }
        //        else
        //        {
        //            return new Uri("/SingleLogin;component/image/SLoginBk_2_N.jpg", UriKind.RelativeOrAbsolute);
        //        }
        //    }
        //    else if (index == 3)
        //    {
        //        //return new Uri("/SingleLogin;component/image/SLoginBk_1.jpg", UriKind.RelativeOrAbsolute);
        //        if (ServerStruct.IsSim() == true)
        //        {
        //            return new Uri("/SingleLogin;component/image/LoginBk_1.jpg", UriKind.RelativeOrAbsolute);
        //        }
        //        else
        //        {
        //            return new Uri("/SingleLogin;component/image/SLoginBk_1.jpg", UriKind.RelativeOrAbsolute);
        //        }
        //    }
        //    else if (index == 4)
        //    {
        //        //return new Uri("/SingleLogin;component/image/SLoginBk_1_N.jpg", UriKind.RelativeOrAbsolute);
        //        if (ServerStruct.IsSim() == true)
        //        {
        //            return new Uri("/SingleLogin;component/image/LoginBk_1_N.jpg", UriKind.RelativeOrAbsolute);
        //        }
        //        else
        //        {
        //            return new Uri("/SingleLogin;component/image/SLoginBk_1_N.jpg", UriKind.RelativeOrAbsolute);
        //        }
        //    }
        //    return null;
        //}

        /// <summary>
        /// 显示网络配置
        /// </summary>
        /// <param name="isShow"></param>
        //public void ShowNetworkSetting(Boolean isShow)
        //{
        //    if (isShow)
        //    {
        //        isShowNetworkSetting = true;
        //        UpdateBackgroundImage();
        //        SettingRow.Height = new GridLength(134);
        //        this.Height = this.Height + 134;
        //    }
        //    else
        //    {
        //        isShowNetworkSetting = false;
        //        UpdateBackgroundImage();
        //        SettingRow.Height = new GridLength(0);
        //        this.Height = this.Height - 134;
        //    }

            
        //}

        //private void UpdateBackgroundImage()
        //{
        //    int imgIndex = 1;
        //    if (isShowNetworkSetting)
        //    {
        //        if (AuthCode.Height.Value != 0)
        //        {
        //            imgIndex = 1;
        //        }
        //        else
        //        {
        //            imgIndex = 3;
        //        }
        //    }
        //    else
        //    {
        //        if (AuthCode.Height.Value != 0)
        //        {
        //            imgIndex = 2;
        //        }
        //        else
        //        {
        //            imgIndex = 4;
        //        }
        //    }

        //    //显示
        //    BitmapImage image = new BitmapImage();
        //    image.BeginInit();
        //    image.UriSource = GetPicUri(imgIndex);
        //    image.EndInit();
        //    image1.Source = image;

        //    SolidColorBrush deep;
        //    SolidColorBrush light;
        //    if (ServerStruct.IsSim() == true)
        //    {
        //         deep= new SolidColorBrush(Colors.Black);
        //         light= new SolidColorBrush(Color.FromArgb(0, 149, 163, 189));

        //         rbReal.Foreground = deep;
        //         rbSim.Foreground = deep;

        //         StatusMessage.Foreground = deep;
        //    }
        //    else
        //    {
        //        deep = new SolidColorBrush(Color.FromArgb(0, 149, 163, 189));
        //        light = new SolidColorBrush(Colors.White);

        //        rbReal.Foreground = light;
        //        rbSim.Foreground = light;

        //        StatusMessage.Foreground = light;
        //    }
        //    label2.Foreground = deep;
        //    label2_S.Foreground = light;

        //    label3.Foreground = deep;
        //    label3_S.Foreground = light;

        //    lbStyle.Foreground = deep;
        //    lbStyle_S.Foreground = light;

        //    tbAccount.Foreground = deep;
        //    tbAccount_S.Foreground = light;

        //    label10.Foreground = deep;
        //    label10_S.Foreground = light;

        //    label4.Foreground = deep;
        //    label4_S.Foreground = light;

        //    label5.Foreground = deep;
        //    label5_S.Foreground = light;

            

           
        //}

        /// <summary>
        /// 窗口关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void Window_Closing(object sender, CancelEventArgs e)
        //{
        //    if (this.isDataDirty)
        //    {
        //        string msg = "Data is dirty. Close without saving?";
        //        MessageBoxResult result =
        //          MessageBox.Show(
        //            msg,
        //            "Data App",
        //            MessageBoxButton.YesNo,
        //            MessageBoxImage.Warning);
        //        if (result == MessageBoxResult.No)
        //        {
        //            e.Cancel = true;
        //        }
        //    }
        //}

        //public void SetJYServerSelectedItem(ServerData sd)
        //{
        //    if (System.Windows.Application.Current != null)
        //    {
        //        System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
        //        {
        //            try
        //            {
        //                for (int i = 0; i < cb_JYServer.Items.Count; i++)
        //                {
        //                    if (cb_JYServer.Items[i] == sd)
        //                    {
        //                        cb_JYServer.SelectedIndex = i;
        //                        break;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Util.Log(ex.ToString());
        //                Util.Log(ex.StackTrace);
        //            }
        //        }
        //        );
        //    }
        //}

        //public void SetHQServerSelectedItem(ServerData sd)
        //{
        //    if (System.Windows.Application.Current != null)
        //    {
        //        System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
        //        {
        //            try
        //            {
        //                for (int i = 0; i < this.cb_HQServer .Items.Count; i++)
        //                {
        //                    if (cb_HQServer.Items[i] == sd)
        //                    {
        //                        cb_HQServer.SelectedIndex = i;
        //                        break;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Util.Log(ex.ToString());
        //                Util.Log(ex.StackTrace);
        //            }
        //        }
        //        );
        //    }
        //}

        //public void SetServerDisconnectedInfo()
        //{
        //    Util.Log("Login.SetServerDisconnectedInfo");
        //    if (serverDatasReconnected == null || serverDatasReconnected.Count==0)
        //    {
        //        serverDatasReconnected = new List<ServerData>();
        //        serverDatasReconnected.AddRange(ServerStruct.currentServerSet.JYserver);

        //        if (ServerStruct.currentServerSet.JYserver.Count > 1)
        //        {
        //            for (int i = 0; i < serverDatasReconnected.Count; i++)
        //            {
        //                ServerData current = ServerStruct.GetJYServer();
        //                if (serverDatasReconnected[i].ServerIPadd == current.ServerIPadd &&
        //                    serverDatasReconnected[i].ServerPort == current.ServerPort)
        //                {
        //                    serverDatasReconnected.RemoveAt(i);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < serverDatasReconnected.Count; i++)
        //        {
        //            ServerData current = ServerStruct.GetJYServer();
        //            if (serverDatasReconnected[i].ServerIPadd == current.ServerIPadd &&
        //                serverDatasReconnected[i].ServerPort == current.ServerPort)
        //            {
        //                serverDatasReconnected.RemoveAt(i);
        //            }
        //        }
        //    }
        //    if (serverDatasReconnected.Count != 0)
        //    {
        //        Util.Log("服务器连接失败，尝试连接下一个服务器...");
        //        SetStatusMessage( "服务器连接失败，尝试连接下一个服务器...");
        //        ServerStruct.Update_Selected(ServerStruct.SERVER_TYPE.JY,
        //            serverDatasReconnected[0].ServerName, true,true);
        //        bt_ok_Click(null, null);
        //    }
        //    else
        //    {
        //        SetStatusMessage( "服务器连接失败");
        //    }
        //}

        //public static int GetChangeStyleSleepTime()
        //{
        //    return Properties.Settings.Default.ChangeStyleSleepTime;
        //}

        //public static int GetChangeCodeSetSleepTime()
        //{
        //    return Properties.Settings.Default.ChangeCodeSetSleepTime;
        //}

        //public static string GetChangeCodeSetSleepMessage()
        //{
        //    return Properties.Settings.Default.ChangeCodeSetSleepMessage;
        //}

        public class StatusMsg
        {
            public string msg;
            public Boolean isLoading;
        }

        private JYData.SynQueue<StatusMsg> statusMsgSynQueue = new JYData.SynQueue<StatusMsg>();

        private void StatusMsgThreadProc()
        {
            while (true)
            {
                //Util.Log("guid=" + guid.ToString());
                //Util.Log("StatusMsg statusMsg");
                StatusMsg statusMsg = statusMsgSynQueue.Dequeue();
                //Util.Log("statusMsgSynQueue.Dequeue();");
                if (statusMsg.isLoading == false)
                {
                    SetStatusMessage_inner(statusMsg.msg);
                    //Util.Log("SetStatusMessage_inner(statusMsg.msg);");
                    if (statusMsg.msg.Contains(LOGON_SUCCESS))
                    {
                        break;
                    }
                }
                else
                {
                    int count=0;
                    String msg = statusMsg.msg;
                    //Util.Log("statusMsg.msg:" + statusMsg.msg);
                    while (true)
                    {
                        //Util.Log("guid=" + guid.ToString());
                        //Util.Log("SetStatusMessage_inner Count" + statusMsgSynQueue.Count().ToString());
                        if (statusMsgSynQueue.Count() != 0)
                        {
                            break;
                        }
                        msg =" "+ msg + ".";
                        count += 1;
                        SetStatusMessage_inner(msg);
                        //Util.Log("SetStatusMessage_inner(msg) msg:" + msg);
                        if (count == 10)
                        {
                            msg = statusMsg.msg;
                            //Util.Log(" count ="+count +" msg=" + msg);
                            count = 0;
                        }
                        Thread.Sleep(200);
                    }
                }
                //if (System.Windows.Application.Current != null)
                //{
                //    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                //    {
                //        if (this.tb_authcode.Visibility == Visibility.Visible)
                //        {
                //            this.CreateNewAuthCode();
                //        }
                //    });
                //}
               
            }
        }

        private void SetStatusMessage_inner(string msg)
        {
            if (System.Windows.Application.Current != null)
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    //bt_ok.IsEnabled = true; //Trial
                    msg = msg.Split('\n')[0].Split('\r')[0];
                    if (msg.Equals("Timed out waiting for logon response"))
                    {
                        return;
                    }
                    int length = msg.Length;
                    if (length > 32)
                    {
                        TbStatusMessage.Text = "";
                        MessageBox.Show(msg, "提示信息", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        TbStatusMessage.Text = msg;
                    }
                   
                });
            }
            
        }

        public void SetStatusMessage(string msg,bool isLoading=false)
        {
            Util.Log("开始调用 SetStatusMessage:msg=" + msg);
            Util.Log("guid=" + _Guid.ToString());
            if (statusMsgThread == null)
            {
                statusMsgThread = new Thread(StatusMsgThreadProc);
                statusMsgThread.IsBackground = true;
                statusMsgThread.Start();
            }
            StatusMsg s = new StatusMsg();
            s.msg = msg;
            s.isLoading = isLoading;

            statusMsgSynQueue.Enqueue(s, false);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bt_ok.IsEnabled = true;
            if (this.tb_authcode.Visibility == Visibility.Visible)
            {
                this.CreateNewAuthCode();
            }

            string commandLine = System.Environment.CommandLine;
            string[] fs = commandLine.Split('-');
            if (fs.Length == 2)
            {
                int id=0;
                if (int.TryParse(fs[1], out id) == true)
                {
                    //找到进程，删除
                    System.Diagnostics.Process myProcess = System.Diagnostics.Process.GetProcessById(id);
                    if (myProcess != null)
                    {
                        myProcess.Kill();
                    }
                }
            }
        }

        //private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    Environment.Exit(0);
        //}

        //private void Image_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    btnCloseImg.Source = new BitmapImage(new Uri("/SingleLogin;component/image/Close_O.png", UriKind.RelativeOrAbsolute)); 
        //}

        //private void Image_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    btnCloseImg.Source = new BitmapImage(new Uri("/SingleLogin;component/image/Close.png", UriKind.RelativeOrAbsolute)); 
        //}

        private void link_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.kiiik.com/applyfz.jsp");
        }

        public static void Log(string content)
        {
            string dateTime = DateTime.Now.ToString("yyyyMMdd hh:mm:ss.fff");
            string currentThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
            try
            {
                Util.Log("[" + dateTime + "]" + "[" + currentThreadID + "][" + System.Threading.Thread.CurrentThread.Priority.ToString() + "]" + content);
            }
            catch (Exception e)
            {
                Util.Log(e.Message);
                Util.Log(e.StackTrace);
            }
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            if (ServerSetting.Height.Value == 0)
            {
                ServerSetting.Height = new GridLength(120);
                this.Height += 120;
            }
            else
            {
                ServerSetting.Height = new GridLength(0);
                this.Height -= 120;
            }
        }

        private bool ServerSettingInit()
        {
            //读取XML文件
            if (!ServerStruct.LoadXml())
            {
                Log("ServerSettingInit:xml载入失败");
                MessageBox.Show("服务器信息文件载入失败，程序将登录默认服务器", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            //InitTag = true;
            ChangeServerInfoForComboBox();
            //InitTag = false;
            return true;
        }

        private void ChangeServerInfoForComboBox()
        {
            //ServerStruct.SetConnectServerForFile();

            cb_TradeServer.Items.Clear();
            //cb_HQServer.Items.Clear();
            foreach (ServerData jy in ServerStruct.CurrentServerSet.JYserver)
            {
                ComboBoxItem JYcbi = new ComboBoxItem();
                JYcbi.Content = jy.ServerName;
                //JYcbi.Content = jy.ServerIPadd + ":" + jy.ServerPort;
                cb_TradeServer.Items.Add(JYcbi);
                int FalseCount = 0;
                if (jy.ServerSelected == true)
                {
                    cb_TradeServer.SelectedIndex = cb_TradeServer.Items.Count - 1;
                }
                else
                {
                    FalseCount++;
                    if (FalseCount == cb_TradeServer.Items.Count)
                    {
                        cb_TradeServer.SelectedValue = null;
                    }
                }
            }
            foreach (ServerData hq in ServerStruct.CurrentServerSet.HQserver)
            {
                ComboBoxItem HQcbi = new ComboBoxItem();
                HQcbi.Content = hq.ServerName;
                cb_QuoteServer.Items.Add(HQcbi);
                int FalseCount = 0;
                if (hq.ServerSelected == true)
                {
                    cb_QuoteServer.SelectedIndex = cb_QuoteServer.Items.Count - 1;
                }
                else
                {
                    FalseCount++;
                    if (FalseCount == cb_QuoteServer.Items.Count)
                    {
                        cb_QuoteServer.SelectedValue = null;
                    }
                }
            }
        }

        private void cb_TradeServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (InitTag) return;
            //ExecFlag = true;
            foreach (ServerData jy in ServerStruct.CurrentServerSet.JYserver)
            {
                if (cb_TradeServer.SelectedValue != null && cb_TradeServer.SelectedValue is ComboBoxItem)
                {
                    if (((ComboBoxItem)cb_TradeServer.SelectedValue).Content.Equals(jy.ServerName))
                    {
                        ServerStruct.Update_Selected(ServerStruct.SERVER_TYPE.JY, jy.ServerName, true);
                        _TradeBrokerID = jy.BrokerID;
                        _TradeServerAddr = "tcp://" + jy.ServerIPadd + ":" + jy.ServerPort;
                        TradeServerInfo.Text = jy.ServerIPadd + ":" + jy.ServerPort;
                    }
                    else
                    {
                        ServerStruct.Update_Selected(ServerStruct.SERVER_TYPE.JY, jy.ServerName, false);
                    }

                }
            }
        }

        private void cb_QuoteServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (InitTag) return;
            //ExecFlag = true;
            foreach (ServerData hq in ServerStruct.CurrentServerSet.HQserver)
            {
                if (cb_QuoteServer.SelectedValue != null && cb_QuoteServer.SelectedValue is ComboBoxItem)
                {
                    if (((ComboBoxItem)cb_QuoteServer.SelectedValue).Content.Equals(hq.ServerName))
                    {
                        ServerStruct.Update_Selected(ServerStruct.SERVER_TYPE.HQ, hq.ServerName, true);
                        _QuoteBrokerID = hq.BrokerID;
                        _QuoteServerAddr = "tcp://" + hq.ServerIPadd + ":" + hq.ServerPort;
                        QuoteServerInfo.Text = hq.ServerIPadd + ":" + hq.ServerPort;
                    }
                    else
                    {
                        ServerStruct.Update_Selected(ServerStruct.SERVER_TYPE.HQ, hq.ServerName, false);
                    }

                }
            }
        }

        private void rbMultiUser_Checked(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MultiUserLogin multiUserWindow = new MultiUserLogin(this);
            multiUserWindow.Show();
        }

        private void rbCtp_Checked(object sender, RoutedEventArgs e)
        {
            _BackEnd = BACKENDTYPE.CTP;
        }

        private void rbFemas_Checked(object sender, RoutedEventArgs e)
        {
            _BackEnd = BACKENDTYPE.FEMAS;
        }

        //private void rbSim_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (rbReal.IsChecked==true)
        //    {
        //        //reg.Visibility = Visibility.Hidden;
        //        reg2.Visibility = Visibility.Hidden;
        //        title.Text = "实盘登录";
        //        title.Text += " " + "v" + GetVesrion();
        //        bt_ok.Content = "登  录";
        //        InitUserNameInfo();
        //        if (ServerStruct.IsSim() == true)
        //        {
        //            //选择了实盘，原来是模拟盘
        //            ServerStruct.SetToReal();
        //            ChangeServerInfoForComboBox();
        //            UpdateBackgroundImage();

        //        }
        //    }
        //    else if (rbSim.IsChecked == true)
        //    {
        //        //reg.Visibility = Visibility.Visible;
        //        reg2.Visibility = Visibility.Visible;
        //        title.Text = "仿真登录";
        //        title.Text += " " + "v" + GetVesrion();
        //        bt_ok.Content = "仿真登录";
        //        InitUserNameInfo();

        //        if (ServerStruct.IsSim() == false)
        //        {
        //            //选择了模拟盘，原来是实盘
        //            ServerStruct.SetToSim();
        //            ChangeServerInfoForComboBox();
        //            UpdateBackgroundImage();

        //        }
        //    }
        //}        
    }

    /// <summary>
    /// 用户名信息
    /// </summary>
    public class UserNameInfo
    {
        public string userName;
        public Boolean selected;

        private UserNameInfo(string aUserName, Boolean aSelectd)
        {
            userName = aUserName;
            selected = aSelectd;
        }

        /// <summary>
        /// 解析一行
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static UserNameInfo Parse(string line)
        {
            if (line == null || line == "") return null;
            string name = line;
            Boolean isSelected = false;
            if (line.StartsWith(":"))
            {
                name = name.Substring(1);
                isSelected = true;
            }
            return new UserNameInfo(name, isSelected);
        }


       
    }

}