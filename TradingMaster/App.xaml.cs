using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows;

namespace TradingMaster
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //#if DEBUG
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                //string s = System.AppDomain.CurrentDomain.BaseDirectory + "Log\\";
                //if (!Directory.Exists(s))
                //{
                //    Directory.CreateDirectory(s);
                //}
                //string file = s + DateTime.Now.ToString("yyyyMMdd HHmmss") + ".txt";
                //if (File.Exists(file))
                //{
                //    file = s + DateTime.Now.ToString("yyyyMMdd HHmmss") + "_1.txt";
                //}
                //FileStream fs = new FileStream(file, FileMode.Create);
                //StreamWriter ws = new StreamWriter(fs);
                //ws.AutoFlush = true;
                //Console.SetOut(ws);
                Util.Log("程序启动");
            }
            catch (Exception e)
            {
                Util.Log_Error("exception: " + e.Message);
            }

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            //#endif
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs ex)
        {
            //Util.Log("未处理的Exception:" + ex.ExceptionObject);
            Exception exception = (Exception)ex.ExceptionObject;
            Util.Log(string.Format("UnhandledException({0}): {1} ---> {2}",
                exception.GetType().Name, exception.Message, exception.InnerException.Message));
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (!HasWritePrevilege())
            {
                MessageBox.Show("请使用管理员权限打开程序。");
                Application.Current.Shutdown();
                return;
            }

            ////先判断Data目录下是否存在.zip文件，如果存在则解压
            //ExtractZip();

            //使程序按照单例方式运行
            Process process = Process.GetCurrentProcess();
            foreach (var item in Process.GetProcessesByName(process.ProcessName))
            {
                if (item.Id != process.Id)
                {
                    MessageBox.Show("您的程序已经启动。");
                    Application.Current.Shutdown();
                    return;
                }
            }

            Application currApp = Application.Current;
            currApp.StartupUri = new Uri("TradingMaster;component/Login.xaml", UriKind.RelativeOrAbsolute);
        }


        private bool HasWritePrevilege()
        {
            Version ver = System.Environment.OSVersion.Version;
            if (ver.Major < 6)//>=6时为vista，win7,2008
            {
                return true;
            }
            String appStartupPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            string programFiles = System.Environment.GetEnvironmentVariable("ProgramFiles");
            if (appStartupPath.IndexOf(programFiles) == 0)
            {
                WindowsIdentity id = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(id);
                if (principal.IsInRole(WindowsBuiltInRole.Administrator))
                {
                    return true;
                }
                return false;
            }
            else
            {
                return true;
            }
        }



        public Boolean ExtractZip()
        {
            //当前目录
            String appStartupPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            String appStartupPath2 = appStartupPath + "\\Data";
            if (System.IO.Directory.Exists(appStartupPath2))
            {
                string[] files = System.IO.Directory.GetFiles(appStartupPath2);
                string lastID = "";
                foreach (string file in files)
                {
                    if (file.EndsWith(".zip") == false) continue;


                    //if (messageBoxThread == null)
                    //{
                    //    messageBoxThread = new Thread(MessageBoxThreadProc);
                    //    messageBoxThread.IsBackground = true;
                    //    messageBoxThread.Start();
                    //}


                    string ver = UnZipFile(file, appStartupPath);
                    if (ver != "")
                    {
                        lastID = ver;
                    }

                    //删除该文件
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception e)
                    {
                        Util.Log("删除失败:" + e.Message);
                    }
                }
                WriteBackCurVersion(lastID);

            }
            //将最后一个Version写回文件
            return true;
        }

        private void WriteBackCurVersion(string ver)
        {
            if (ver == "") return;
            //去Setting目录下获取version.xml文件
            String appStartupPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            String appStartupPath2 = appStartupPath + "\\Setting\\version.txt";
            FileStream fs = new FileStream(appStartupPath2, FileMode.Create);
            byte[] v = System.Text.ASCIIEncoding.Default.GetBytes(ver);
            fs.Write(v, 0, v.Length);
            fs.Close();
        }

        private string GetCurVersion()
        {
            System.Diagnostics.FileVersionInfo myFileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo("SingleUtil.Login.dll");
            //去Setting目录下获取version.xml文件
            String appStartupPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            String appStartupPath2 = appStartupPath + "\\Setting\\version.txt";
            if (File.Exists(appStartupPath2) == false)
            {
                //不存在当前版本的信息
                return myFileVersion.FileVersion;
            }

            FileStream fss = new FileStream(appStartupPath2, FileMode.Open);
            StreamReader sr = new StreamReader(fss);
            string ret = sr.ReadLine();

            sr.Close();
            fss.Close();
            if (ret == null) return myFileVersion.FileVersion;
            string[] fs = ret.Split('.');
            if (fs.Length != 4) return myFileVersion.FileVersion;
            foreach (string v in fs)
            {
                int val = 0;
                if (int.TryParse(v, out val) == false) return myFileVersion.FileVersion;
                if (val < 0) return myFileVersion.FileVersion; ;
            }
            return ret;
        }

        private String UnZipFile(string file, string target)
        {
            String p = Path.GetFileName(file);
            if (p.StartsWith("DHU") == false) return "";
            //先得到版本号
            string[] v = file.Split('_');
            if (v.Length == 1) return "";
            string version = file.Substring(file.LastIndexOf('_') + 1);
            version = version.Replace(".zip", "");

            string curVersion = GetCurVersion();
            Util.Log("当前的版本号为:" + curVersion);

            if (curVersion.CompareTo(version) == 1)
            {
                //不小于
                Util.Log("当前的版本号大于" + version + ",返回");
                return "";
            }

            Util.Log("解压文件:" + file);
            ZipInputStream ssss = new ZipInputStream(File.OpenRead(file));
            ZipEntry theEntry;
            try
            {
                while ((theEntry = ssss.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(target);
                    string fileName = Path.GetFileName(theEntry.Name);
                    Util.Log("开始解压内容:" + fileName);
                    ////生成解压目录   
                    //Directory.CreateDirectory(directoryName);
                    if (fileName != String.Empty)
                    {
                        //解压文件到指定的目录   
                        FileStream streamWriter = null;
                        string filename = target + "\\" + theEntry.Name;
                        int index = 0;
                        while (index < 10)
                        {
                            try
                            {
                                streamWriter = File.Create(filename);
                                break;
                            }
                            catch (Exception ex2)
                            {
                                Util.Log("exception 解压文件" + filename + "时发生错误:" + ex2.Message);
                                index += 1;
                                Util.Log("index=" + index + " 睡眠500毫秒");
                                System.Threading.Thread.Sleep(500);
                            }
                        }

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = ssss.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                if (streamWriter != null)
                                {
                                    streamWriter.Write(data, 0, size);
                                }

                            }
                            else
                            {
                                break;
                            }
                        }
                        if (streamWriter != null)
                        {
                            streamWriter.Close();
                            Util.Log("成功解压内容:" + fileName);
                        }
                    }
                    else if (theEntry.Name.EndsWith("/"))
                    {
                        string folder = target + "//" + theEntry.Name;
                        if (Directory.Exists(folder) == false)
                        {
                            //不存在该目录
                            Directory.CreateDirectory(folder);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //解压出错
                Util.Log("解压失败:" + e.Message);
            }
            ssss.Close();
            return version;
        }

        //public static void Util.Log(string content)
        //{
        //    StackTrace st = new StackTrace(true);
        //    StackFrame sf = st.GetFrame(1);
        //    System.Reflection.MethodBase method = sf.GetMethod();
        //    string lastcallmethod = method.DeclaringType.ToString() + "." + method.Name;

        //    string dateTime = DateTime.Now.ToString("yyyyMMdd hh:mm:ss.fff");
        //    string currentThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId.ToString();
        //    try
        //    {
        //        Util.Log("[" + dateTime + "]" + "[" + currentThreadID + "][" + System.Threading.Thread.CurrentThread.Priority.ToString() + "][" + lastcallmethod + "]\t" + content);
        //        Util.Log("");

        //    }
        //    catch (Exception e)
        //    {
        //        Util.Log(e.Message);
        //    }
        //}
    }
}
