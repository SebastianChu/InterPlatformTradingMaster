using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace TradingMaster
{
    public class Util
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public static readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        /// <summary>
        /// 判断文件是否被占用
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Boolean IsFileInUse(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Util.Log("文件不存在，未被占用");
                return false;
            }
            IntPtr vHandle = _lopen(fileName, OF_READWRITE | OF_SHARE_DENY_NONE);
            Boolean bret = false;
            if (vHandle == HFILE_ERROR)
            {
                Util.Log("文件被占用");
                bret = true;
            }
            CloseHandle(vHandle);
            return bret;
        }

        public static void PrintStackTrace()
        {
            StackTrace st = new StackTrace(true);
            for (int i = 1; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                System.Reflection.MethodBase method = sf.GetMethod();
                string lastcallmethod = method.DeclaringType.ToString() + "." + method.Name;
                Console.WriteLine("lastCallMethod:" + i + "=" + lastcallmethod);
            }
        }

        /// <summary>
        /// 得到本地某个dll中的版本号
        /// </summary>
        /// <param name="pluginDllFileName"></param>
        /// <returns></returns>
        public static string GetPluginVersionString(string pluginDllFileName)
        {
            if (String.IsNullOrEmpty(pluginDllFileName)) return "";
            Util.Log("获取文件版本号:" + pluginDllFileName);
            Assembly ass = Assembly.LoadFrom(pluginDllFileName);
            Version ver = ass.GetName().Version;
            return ver.ToString();
        }

        public static void Log(string content)
        {
            StackTrace st = new StackTrace(true);
            StackFrame sf = st.GetFrame(1);
            MethodBase method = sf.GetMethod();
            string lastcallmethod = string.Format("[{0}.{1}]", method.DeclaringType.ToString(), method.Name);
            ILog log = LogManager.GetLogger(lastcallmethod);
            log.Info(content);
        }

        public static string GetMD5(string content)
        {
            Util.Log("content=" + content);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(content));
            string str = System.BitConverter.ToString(hashData);
            str = str.Replace("-", "");
            return str;
        }

        /// <summary>
        /// 根据最小变动单位得到小数点后的位数
        /// </summary>
        /// <param name="fluct"></param>
        /// <returns></returns>
        public static string GetStringFormat(decimal fluct)
        {
            string sFluct = fluct.ToString();
            if (sFluct.IndexOf(".") < 0)
            {
                return "F0";
            }

            string decimalPart = sFluct.Substring(sFluct.LastIndexOf(".") + 1);
            while (decimalPart.Length > 0)
            {
                //去除末尾的0
                if (decimalPart[decimalPart.Length - 1] == '0')
                {
                    decimalPart = sFluct.Substring(0 + decimalPart.Length - 1);
                }
                else
                {
                    return "F" + decimalPart.Length;
                }
            }
            return "F0";
        }

        /// <summary>
        /// 根据某个dll得到其MD5值
        /// </summary>
        /// <param name="assemblyname"></param>
        /// <returns></returns>
        public static String GetMd5Identity(String assemblyname)
        {
            if (File.Exists(assemblyname) == false) return "error";
            Util.Log("为" + assemblyname + ".dll计算MD5值");
            FileStream fs = null;
            string str = "error";
            try
            {
                fs = new FileStream(assemblyname, FileMode.Open, FileAccess.Read, FileShare.Read);
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] hashData = md5.ComputeHash(fs);
                str = System.BitConverter.ToString(hashData);
                str = str.Replace("-", "");
            }
            catch (Exception ex)
            {
                Util.Log("exception" + ex.Message);
                Util.Log(ex.StackTrace);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
            Util.Log(assemblyname + ".dll的MD5值为:" + str);
            return str;
        }

        /// <summary>
        /// 进行DES加密。
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串。</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        public static string Encrypt(string pToEncrypt, string sKey)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                if (sKey.Length < 8)
                {
                    sKey = sKey.PadRight(8);
                }
                else if (sKey.Length > 8)
                {
                    sKey = sKey.Substring(0, 8);
                }
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>已解密的字符串。</returns>
        public static string Decrypt(string pToDecrypt, string sKey)
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                if (sKey.Length < 8)
                {
                    sKey = sKey.PadRight(8);
                }
                else if (sKey.Length > 8)
                {
                    sKey = sKey.Substring(0, 8);
                }
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        /// <summary>
        /// 解析FIX协议中的日期格式
        /// </summary>
        /// <param name="timeformat"></param>
        /// <returns></returns>
        public static DateTime ParseDateTime(string timeformat)
        {
            try
            {
                if (timeformat.IndexOf(".") == -1)
                {
                    timeformat = timeformat + ".000";
                }
                string[] fs = timeformat.Split('-');
                int date = int.Parse(fs[0]);
                string[] fs2 = fs[1].Split('.');
                int millisecond = int.Parse(fs2[1]);
                string[] fs3 = fs2[0].Split(':');
                int hour = int.Parse(fs3[0]);
                int minute = int.Parse(fs3[1]);
                int second = int.Parse(fs3[2]);
                int year = date / 10000;
                int month = date % 10000 / 100;
                int day = date % 100;
                return new DateTime(year, month, day, hour, minute, second, millisecond);
            }
            catch (Exception ex)
            {
                Util.Log("exception: " + ex.Message);
                Util.Log(ex.StackTrace);
            }
            return new DateTime();
        }

        /// <summary> 
        /// 在指定的字符串列表CnStr中检索符合拼音索引字符串 
        /// </summary> 
        /// <param name="CnStr">汉字字符串</param> 
        /// <returns>相对应的汉语拼音首字母串</returns> 
        public static string[] GetPinYinGroup(string cnStr)
        {
            //CnStr = Regex.Replace(CnStr, "\\s", "");
            string ReturnStr = GetPinYinString(cnStr);
            string[] strArray = ReturnStr.Split(",;".ToCharArray());
            return strArray;
        }

        //实现汉字转化为拼音
        private static string GetPinYinString(string hzStr)
        {
            int i, j, k, m;
            string tmpStr;
            string returnStr = "";  //返回最终结果的字符串
            string[] tmpArr;
            for (i = 0; i < hzStr.Length; i++)
            {   //处理汉字字符串,对每个汉字的首字母进行一次循环
                tmpStr = GetCharSpellCode((char)hzStr[i]);   //获取第i个汉字的拼音首字母,可能为1个或多个
                if (tmpStr.Length > 0)
                {   //汉字的拼音首字母存在的情况才进行操作
                    if (returnStr != "")
                    {   //不是第一个汉字
                        Regex regex = new Regex(",");
                        tmpArr = regex.Split(returnStr);
                        returnStr = "";
                        for (k = 0; k < tmpArr.Length; k++)
                        {
                            for (j = 0; j < tmpStr.Length; j++)    //对返回的每个首字母进行拼接
                            {
                                string charcode = tmpStr[j].ToString(); //取出第j个拼音字母
                                returnStr += tmpArr[k] + charcode + ",";
                            }
                        }
                        if (returnStr != "")
                            returnStr = returnStr.Substring(0, returnStr.Length - 1);
                    }
                    else
                    {   //构造第一个汉字返回结果
                        for (m = 0; m < tmpStr.Length - 1; m++)
                            returnStr += tmpStr[m] + ",";
                        returnStr += tmpStr[tmpStr.Length - 1];
                    }
                }
                else
                {
                    //returnStr;
                }
            }
            return returnStr;   //返回处理结果字符串，以，分隔每个拼音组合
        }

        /// <summary> 
        /// 获取单个汉字对应的拼音首字符字符串，
        /// </summary> 
        /// <param name="CnChar">单个汉字</param> 
        /// <returns>单个大写字母</returns> 
        private static string GetCharSpellCode(char strCh)
        {
            //此处收录了375个多音字
            string MultiPinyin = "19969:DZ,19975:WM,19988:QJ,20048:YL,20056:SC,20060:NM,20094:QG,20127:QJ,20167:QC,20193:YG,20250:KH,20256:ZC,20282:SC,20285:QJG,20291:TD,20314:YD,20340:NE,20375:TD,20389:YJ,20391:CZ,20415:PB,20446:YS,20447:SQ,20504:TC,20608:KG,20854:QJ,20857:ZC,20911:PF,20504:TC,20608:KG,20854:QJ,20857:ZC,20911:PF,20985:AW,21032:PB,21048:XQ,21049:SC,21089:YS,21119:JC,21242:SB,21273:SC,21305:YP,21306:QO,21330:ZC,21333:SDC,21345:QK,21378:CA,21397:SC,21414:XS,21442:SC,21477:JG,21480:TD,21484:ZS,21494:YX,21505:YX,21512:HG,21523:XH,21537:PB,21542:PF,21549:KH,21571:E,21574:DA,21588:TD,21589:O,21618:ZC,21621:KHA,21632:ZJ,21654:KG,21679:LKG,21683:KH,21710:A,21719:YH,21734:WOE,21769:A,21780:WN,21804:XH,21834:A,21899:ZD,21903:RN,21908:WO,21939:ZC,21956:SA,21964:YA,21970:TD,22003:A,22031:JG,22040:XS,22060:ZC,22066:ZC,22079:MH,22129:XJ,22179:XA,22237:NJ,22244:TD,22280:JQ,22300:YH,22313:XW,22331:YQ,22343:YJ,22351:PH,22395:DC,22412:TD,22484:PB,22500:PB,22534:ZD,22549:DH,22561:PB,22612:TD,22771:KQ,22831:HB,22841:JG,22855:QJ,22865:XQ,23013:ML,23081:WM,23487:SX,23558:QJ,23561:YW,23586:YW,23614:YW,23615:SN,23631:PB,23646:ZS,23663:ZT,23673:YG,23762:TD,23769:ZS,23780:QJ,23884:QK,24055:XH,24113:DC,24162:ZC,24191:GA,24273:QJ,24324:NL,24377:TD,24378:QJ,24439:PF,24554:ZS,24683:TD,24694:WE,24733:LK,24925:TN,25094:ZG,25100:XQ,25103:XH,25153:PB,25170:PB,25179:KG,25203:PB,25240:ZS,25282:FB,25303:NA,25324:KG,25341:ZY,25373:WZ,25375:XJ,25384:A,25457:A,25528:SD,25530:SC,25552:TD,25774:ZC,25874:ZC,26044:YW,26080:WM,26292:PB,26333:PB,26355:ZY,26366:CZ,26397:ZC,26399:QJ,26415:ZS,26451:SB,26526:ZC,26552:JG,26561:TD,26588:JG,26597:CZ,26629:ZS,26638:YL,26646:XQ,26653:KG,26657:XJ,26727:HG,26894:ZC,26937:ZS,26946:ZC,26999:KJ,27099:KJ,27449:YQ,27481:XS,27542:ZS,27663:ZS,27748:TS,27784:SC,27788:ZD,27795:TD,27812:O,27850:PB,27852:MB,27895:SL,27898:PL,27973:QJ,27981:KH,27986:HX,27994:XJ,28044:YC,28065:WG,28177:SM,28267:QJ,28291:KH,28337:ZQ,28463:TL,28548:DC,28601:TD,28689:PB,28805:JG,28820:QG,28846:PB,28952:TD,28975:ZC,29100:A,29325:QJ,29575:SL,29602:FB,30010:TD,30044:CX,30058:PF,30091:YSP,30111:YN,30229:XJ,30427:SC,30465:SX,30631:YQ,30655:QJ,30684:QJG,30707:SD,30729:XH,30796:LG,30917:PB,31074:NM,31085:JZ,31109:SC,31181:ZC,31192:MLB,31293:JQ,31400:YX,31584:YJ,31896:ZN,31909:ZY,31995:XJ,32321:PF,32327:ZY,32418:HG,32420:XQ,32421:HG,32438:LG,32473:GJ,32488:TD,32521:QJ,32527:PB,32562:ZSQ,32564:JZ,32735:ZD,32793:PB,33071:PF,33098:XL,33100:YA,33152:PB,33261:CX,33324:BP,33333:TD,33406:YA,33426:WM,33432:PB,33445:JG,33486:ZN,33493:TS,33507:QJ,33540:QJ,33544:ZC,33564:XQ,33617:YT,33632:QJ,33636:XH,33637:YX,33694:WG,33705:PF,33728:YW,33882:SR,34067:WM,34074:YW,34121:QJ,34255:ZC,34259:XL,34425:JH,34430:XH,34485:KH,34503:YS,34532:HG,34552:XS,34558:YE,34593:ZL,34660:YQ,34892:XH,34928:SC,34999:QJ,35048:PB,35059:SC,35098:ZC,35203:TQ,35265:JX,35299:JX,35782:SZ,35828:YS,35830:E,35843:TD,35895:YG,35977:MH,36158:JG,36228:QJ,36426:XQ,36466:DC,36710:JC,36711:ZYG,36767:PB,36866:SK,36951:YW,37034:YX,37063:XH,37218:ZC,37325:ZC,38063:PB,38079:TD,38085:QY,38107:DC,38116:TD,38123:YD,38224:HG,38241:XTC,38271:ZC,38415:YE,38426:KH,38461:YD,38463:AE,38466:PB,38477:XJ,38518:YT,38551:WK,38585:ZC,38704:XS,38739:LJ,38761:GJ,38808:SQ,39048:JG,39049:XJ,39052:HG,39076:CZ,39271:XT,39534:TD,39552:TD,39584:PB,39647:SB,39730:LG,39748:TPB,40109:ZQ,40479:ND,40516:HG,40536:HG,40583:QJ,40765:YQ,40784:QJ,40840:YK,40863:QJG,";
            string resStr = "";
            int i, j, uni;
            uni = (UInt16)strCh;
            if (uni > 40869 || uni < 19968)
            {
                return strCh.ToString();
            }
            //返回该字符在Unicode字符集中的编码值
            i = MultiPinyin.IndexOf(uni.ToString());
            //检查是否是多音字,是按多音字处理,不是就直接在strChineseFirstPY字符串中找对应的首字母
            if (i < 0)
            //获取非多音字汉字首字母
            {
                resStr = GetSinglePinYinInit(strCh.ToString());
            }
            else
            {   //获取多音字汉字首字母
                j = MultiPinyin.IndexOf(",", i);
                resStr = MultiPinyin.Substring(i + 6, j - i - 6);
            }
            return resStr;
        }


        /// <summary>
        /// 取单个字符的拼音声母
        /// </summary>
        /// <param name="c">要转换的单个汉字</param>
        /// <returns>拼音声母</returns>
        public static string GetSinglePinYinInit(string cnChar)
        {
            long iCnChar;
            byte[] ZW = System.Text.Encoding.Default.GetBytes(cnChar);
            //如果是字母，则直接返回 
            if (ZW.Length == 1)
            {
                return cnChar.ToUpper();
            }
            else
            {
                // get the array of byte from the single char 
                int i1 = (short)(ZW[0]);
                int i2 = (short)(ZW[1]);
                iCnChar = i1 * 256 + i2;
            }

            //expresstion 
            //table of the constant list 
            // 'A'; //45217..45252 
            // 'B'; //45253..45760 
            // 'C'; //45761..46317 
            // 'D'; //46318..46825 
            // 'E'; //46826..47009 
            // 'F'; //47010..47296 
            // 'G'; //47297..47613 
            // 'H'; //47614..48118 
            // 'J'; //48119..49061 
            // 'K'; //49062..49323 
            // 'L'; //49324..49895 
            // 'M'; //49896..50370 
            // 'N'; //50371..50613 
            // 'O'; //50614..50621 
            // 'P'; //50622..50905 
            // 'Q'; //50906..51386 
            // 'R'; //51387..51445 
            // 'S'; //51446..52217 
            // 'T'; //52218..52697 
            //没有U,V 
            // 'W'; //52698..52979 
            // 'X'; //52980..53640 
            // 'Y'; //53689..54480 
            // 'Z'; //54481..55289 

            // iCnChar match the constant 
            if ((iCnChar >= 45217) && (iCnChar <= 45252))
            {
                return "A";
            }
            else if ((iCnChar >= 45253) && (iCnChar <= 45760))
            {
                return "B";
            }
            else if ((iCnChar >= 45761) && (iCnChar <= 46317))
            {
                return "C";
            }
            else if ((iCnChar >= 46318) && (iCnChar <= 46825))
            {
                return "D";
            }
            else if ((iCnChar >= 46826) && (iCnChar <= 47009))
            {
                return "E";
            }
            else if ((iCnChar >= 47010) && (iCnChar <= 47296))
            {
                return "F";
            }
            else if ((iCnChar >= 47297) && (iCnChar <= 47613))
            {
                return "G";
            }
            else if ((iCnChar >= 47614) && (iCnChar <= 48118))
            {
                return "H";
            }
            else if ((iCnChar >= 48119) && (iCnChar <= 49061))
            {
                return "J";
            }
            else if ((iCnChar >= 49062) && (iCnChar <= 49323))
            {
                return "K";
            }
            else if ((iCnChar >= 49324) && (iCnChar <= 49895))
            {
                return "L";
            }
            else if ((iCnChar >= 49896) && (iCnChar <= 50370))
            {
                return "M";
            }

            else if ((iCnChar >= 50371) && (iCnChar <= 50613))
            {
                return "N";
            }
            else if ((iCnChar >= 50614) && (iCnChar <= 50621))
            {
                return "O";
            }
            else if ((iCnChar >= 50622) && (iCnChar <= 50905))
            {
                return "P";
            }
            else if ((iCnChar >= 50906) && (iCnChar <= 51386))
            {
                return "Q";
            }
            else if ((iCnChar >= 51387) && (iCnChar <= 51445))
            {
                return "R";
            }
            else if ((iCnChar >= 51446) && (iCnChar <= 52217))
            {
                return "S";
            }
            else if ((iCnChar >= 52218) && (iCnChar <= 52697))
            {
                return "T";
            }
            else if ((iCnChar >= 52698) && (iCnChar <= 52979))
            {
                return "W";
            }
            else if ((iCnChar >= 52980) && (iCnChar <= 53640))
            {
                return "X";
            }
            else if ((iCnChar >= 53689) && (iCnChar <= 54480))
            {
                return "Y";
            }
            else if ((iCnChar >= 54481) && (iCnChar <= 55289))
            {
                return "Z";
            }
            else return ("?");
        }

        // Encrypt the string.
        public static byte[] Encrypt(string PlainText, SymmetricAlgorithm key)
        {
            // Create a memory stream.
            MemoryStream ms = new MemoryStream();

            // Create a CryptoStream using the memory stream and the 
            // CSP DES key.  
            CryptoStream encStream = new CryptoStream(ms, key.CreateEncryptor(), CryptoStreamMode.Write);

            // Create a StreamWriter to write a string
            // to the stream.
            StreamWriter sw = new StreamWriter(encStream);

            // Write the plaintext to the stream.
            sw.WriteLine(PlainText);

            // Close the StreamWriter and CryptoStream.
            sw.Close();
            encStream.Close();

            // Get an array of bytes that represents
            // the memory stream.
            byte[] buffer = ms.ToArray();

            // Close the memory stream.
            ms.Close();

            // Return the encrypted byte array.
            return buffer;
        }

        // Decrypt the byte array.
        public static string Decrypt(byte[] CypherText, SymmetricAlgorithm key)
        {
            // Create a memory stream to the passed buffer.
            MemoryStream ms = new MemoryStream(CypherText);

            // Create a CryptoStream using the memory stream and the 
            // CSP DES key. 
            CryptoStream encStream = new CryptoStream(ms, key.CreateDecryptor(), CryptoStreamMode.Read);

            // Create a StreamReader for reading the stream.
            StreamReader sr = new StreamReader(encStream);

            // Read the stream as a string.
            string val = sr.ReadLine();

            // Close the streams.
            sr.Close();
            encStream.Close();
            ms.Close();

            return val;
        }

        /// <summary>
        /// 得到本机IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetCurIP()
        {
            try
            {
                //本机IP地址
                IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
                string myip = ipEntry.AddressList[0].ToString();
                if (ipEntry.AddressList.Length > 1)
                {
                    myip = ipEntry.AddressList[ipEntry.AddressList.Length - 1].ToString();
                }
                return myip;
            }
            catch (Exception)
            {
            }
            return "";

        }

        /// <summary>
        /// 得到本机MAC
        /// </summary>
        /// <returns></returns>
        public static string GetCurMac()
        {
            try
            {
                //本机MAC地址
                string macaddr = "";
                ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = query.Get();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        macaddr = mo["MacAddress"].ToString();
                    }
                    mo.Dispose();
                }
                return macaddr;
            }
            catch (Exception)
            {
            }
            return "";

        }

        /// <summary>
        /// 得到本机CPU序列号
        /// </summary>
        /// <returns></returns>
        public static string GetCurCPUNum()
        {
            try
            {
                //本机CPU序列号
                string cpunum = "";
                ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                ManagementObjectCollection moc = query.Get();
                foreach (ManagementObject mo in moc)
                {
                    if (mo["ProcessorId"] != null)
                    {
                        cpunum += mo["ProcessorId"].ToString();
                    }
                    mo.Dispose();
                }
                return cpunum;
            }
            catch (Exception)
            {
            }
            return "";

        }

        /// <summary>
        /// 得到本机硬盘序列号
        /// </summary>
        /// <returns></returns>
        public static string GetCurDiskNum()
        {
            try
            {
                string diskNum = "";
                ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                ManagementObjectCollection moc = query.Get();
                foreach (ManagementObject mo in moc)
                {
                    if (mo["Model"] != null)
                    {
                        diskNum += mo["Model"].ToString();
                    }
                    mo.Dispose();
                }
                return diskNum;
            }
            catch (Exception)
            {
            }
            return "";


        }

        /// <summary>
        /// 得到本机主板编号
        /// </summary>
        /// <returns></returns>
        public static string GetCurBoardNum()
        {
            try
            {
                string diskNum = "";
                ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                ManagementObjectCollection moc = query.Get();
                foreach (ManagementObject mo in moc)
                {
                    if (mo["SerialNumber"] != null)
                    {
                        diskNum += mo["SerialNumber"].ToString();
                    }
                    mo.Dispose();
                }
                return diskNum;
            }
            catch (Exception)
            {
            }
            return "";


        }
    }
}
