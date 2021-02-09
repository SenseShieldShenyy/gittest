using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using D2cDemo;
using System.Threading.Tasks;
using System.IO;
using cs_libd2c_demo;
using SenseShield;
using SLM_HANDLE_INDEX = System.UInt32;

namespace SenseShield
{
    class Program
    {
        public static string pin = "12345678";
        public const int DEVELOPER_ID_LENGTH = 8;
        public const int DEVICE_SN_LENGTH = 16;
        //string file_patn = "./";
        //string filename = "h5first.evx";
        //许可选项定义


        //打印方式定义
        public static void WriteLineGreen(string s)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(s);
            Console.ResetColor();
        }
        public static void WriteLineRed(string s)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s);
            Console.ResetColor();
        }
        public static void WriteLineYellow(string s)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(s);
            Console.ResetColor();
        }
        public static byte[] StringToHex(string HexString)
        {
            byte[] returnBytes = new byte[HexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(HexString.Substring(i * 2, 2), 16);

            return returnBytes;
        }

        //回调函数信息提示
        public static uint handle_service_msg(uint message, UIntPtr wparam, UIntPtr lparam)
        {
            uint ret = SSErrCode.SS_OK;
            string StrMsg = string.Empty;
            char[] szmsg = new char[1024];
            char[] lock_sn = new char[DEVICE_SN_LENGTH];
            char[] szlock_sn = new char[DEVICE_SN_LENGTH];

            switch (message)
            {
                case SSDefine.SS_ANTI_INFORMATION:   // 信息提示
                    StrMsg = string.Format("SS_ANTI_INFORMATION is:0x{0:X8} wparam is %p", message, wparam);
                    WriteLineRed(StrMsg);
                    break;
                case SSDefine.SS_ANTI_WARNING:       // 警告
                    // 反调试检查。一旦发现如下消息，建议立即停止程序正常业务，防止程序被黑客调试。

                    switch ((uint)(wparam))
                    {
                        case SSDefine.SS_ANTI_PATCH_INJECT:
                            StrMsg = string.Format("信息类型=:0x{0:X8} 具体错误码= 0x{0:X8}", "注入", message, wparam);
                            WriteLineRed(StrMsg);
                            break;
                        case SSDefine.SS_ANTI_MODULE_INVALID:
                            StrMsg = string.Format("信息类型=:0x{0:X8} 具体错误码= 0x{0:X8}", "非法模块DLL", message, wparam);
                            WriteLineRed(StrMsg);
                            break;
                        case SSDefine.SS_ANTI_ATTACH_FOUND:
                            StrMsg = string.Format("信息类型=:0x{0:X8} 具体错误码= 0x{0:X8}", "附加调试", message, wparam);
                            WriteLineRed(StrMsg);
                            break;
                        case SSDefine.SS_ANTI_THREAD_INVALID:
                            StrMsg = string.Format("信息类型=:0x{0:X8} 具体错误码= 0x{0:X8}", "线程非法", message, wparam);
                            WriteLineRed(StrMsg);
                            break;
                        case SSDefine.SS_ANTI_THREAD_ERROR:
                            StrMsg = string.Format("信息类型=:0x{0:X8} 具体错误码= 0x{0:X8}", "线程错误", message, wparam);
                            WriteLineRed(StrMsg);
                            break;
                        case SSDefine.SS_ANTI_CRC_ERROR:
                            StrMsg = string.Format("信息类型=:0x{0:X8} 具体错误码= 0x{0:X8}", "内存模块 CRC 校验", message, wparam);
                            WriteLineRed(StrMsg);
                            break;
                        case SSDefine.SS_ANTI_DEBUGGER_FOUND:
                            StrMsg = string.Format("信息类型=:0x{0:X8} 具体错误码= 0x{0:X8}", "发现调试器", message, wparam);
                            WriteLineRed(StrMsg);
                            break;
                        default:
                            StrMsg = string.Format("信息类型=:0x{0:X8} 具体错误码= 0x{0:X8}", "其他未知错误", message, wparam);
                            WriteLineRed(StrMsg);
                            break;
                    }
                    break;
                case SSDefine.SS_ANTI_EXCEPTION:         // 异常
                    StrMsg = string.Format("SS_ANTI_EXCEPTION is :0x{0:X8} wparam is %p", message, wparam);
                    WriteLineRed(StrMsg); ;
                    break;
                case SSDefine.SS_ANTI_IDLE:              // 暂保留
                    StrMsg = string.Format("SS_ANTI_IDLE is :0x{0:X8} wparam is %p", message, wparam);
                    WriteLineRed(StrMsg);
                    break;
                case SSDefine.SS_MSG_SERVICE_START:      // 服务启动
                    StrMsg = string.Format("SS_MSG_SERVICE_START is :0x{0:X8} wparam is %p", message, wparam);
                    WriteLineRed(StrMsg);
                    break;
                case SSDefine.SS_MSG_SERVICE_STOP:       // 服务停止
                    StrMsg = string.Format("SS_MSG_SERVICE_STOP is :0x{0:X8} wparam is %p", message, wparam);
                    WriteLineRed(StrMsg);
                    break;
                case SSDefine.SS_MSG_LOCK_AVAILABLE:     // 锁可用（插入锁或SS启动时锁已初始化完成），wparam 代表锁号
                    // 锁插入消息，可以根据锁号查询锁内许可信息，实现自动登录软件等功能。
                    StrMsg = string.Format("{0},{0:x8}锁插入", DateTime.Now.ToString(), wparam);
                    WriteLineGreen(StrMsg);
                    break;
                case SSDefine.SS_MSG_LOCK_UNAVAILABLE:   // 锁无效（锁已拔出），wparam 代表锁号
                    // 锁拔出消息，对于只使用锁的应用程序，一旦加密锁拔出软件将无法继续使用，建议发现此消息提示用户保存数据，程序功能锁定等操作。
                    StrMsg = string.Format("{0},{0:x8}锁拔出", DateTime.Now.ToString(), wparam);
                    WriteLineRed(StrMsg);
                    break;
            }
            // 输出格式化后的消息内容
            //printf("%s\n", szmsg);
            return ret;
        }

        //main方法，测试主程序
        [STAThread]
        static void Main(string[] args)
        {
           

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SlmRuntimeCSharp.Form2 frm2 = new SlmRuntimeCSharp.Form2();
            Application.Run(frm2);
           

        }
//    }
//}
        //在main 放方法外定义其他方法
        //根据锁号获取证书
        //public static byte[] = get_lockp7b_from_db(string lock_sn)
        //{
        //    string p7b_from_db = "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";
        //    byte[] cert_p7b = Convert.FromBase64String(p7b_from_db);
        //    return cert_p7b;
        //}
        public static byte[] get_lockp7b_from_db(string lock_sn)
        {
            //这里需要链接数据库，根据锁号获取到数据库中对应的p7b, 并使用base64 解码成二进制。
            //因为无法连接数据库测试，这里直接使用以获取到的p7b数据
//            string p7b_from_db = @"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";
            string p7b_from_db = "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";

            byte[] cert_p7b = Convert.FromBase64String(p7b_from_db);
            return cert_p7b;
        }
        // 生成许可字符串
        public static UInt32 make_license(UInt32 license_id, UInt64 start_time, UInt64 end_time, string lock_type, string data_area_raw,
            string lock_sn, byte[] lock_cert, ref string maked_lic_str, string lic_op, string data_area)
        {
            UInt32 ret = 0;
            IntPtr device_handle = IntPtr.Zero;
            IntPtr d2cHandlePtr = IntPtr.Zero;
           // Libd2c.ACCOUNT_TYPE accountType = Libd2c.ACCOUNT_TYPE.NONE;
            string accountID = string.Empty;

            string licGUID = string.Empty;
            UInt32 d2cSize = 0;
            byte[] d2c_buf = null;
            IntPtr errMsgPtr = IntPtr.Zero;
            string errMsg = string.Empty;

            //打开控制锁
            ret = Libd2c.master_open(ref device_handle);
            if (ret != 0)
            {
                Console.WriteLine("打开控制锁失败。0x{0:X}", ret);
                return ret;
            }
            //验证控制所pin
            byte[] psd = System.Text.ASCIIEncoding.Default.GetBytes(pin);
            ret = Libd2c.master_pin_verify(device_handle, 0, psd, (UInt32)psd.Length);
            if (ret != 0)
            {
                Console.WriteLine("校验PIN码失败。0x{0:08X}", ret);
                return ret;
            }
            //指定锁号签发许可
            accountID = lock_sn;
            byte[] accountIdBytes = Libd2cCommon.HexStrToBytes(accountID);
            UInt32 accountIdSize = (UInt32)(accountID.Length / 2);
            //创建d2c
            ret = Libd2c.d2c_lic_new(device_handle, ref d2cHandlePtr, Libd2c.ACCOUNT_TYPE.NONE, accountIdBytes, accountIdSize,
                lock_cert, (uint)lock_cert.Length);
            if (ret != SenseShield.SSErrCode.SS_OK)
            {
                Console.WriteLine("创建d2c失败。0x{0:08X}", ret);
            }
            JObject licJsonObj = make_license_json(license_id, start_time, end_time, lock_type, data_area_raw, lic_op, data_area);
            string strLicJson = JsonConvert.SerializeObject(licJsonObj);
            string strLicDesc = strLicJson;
            //往d2c中添加许可
            ret = Libd2c.d2c_add_lic(d2cHandlePtr, strLicJson, strLicDesc, null);
            if (ret != SenseShield.SSErrCode.SS_OK)
            {
                Console.WriteLine("向d2c中添加内容失败。0x{0:08X}", ret);
                return ret;
            }
            WriteLineGreen(strLicJson);
            ret = Libd2c.d2c_get(d2cHandlePtr, null, 0, ref d2cSize);
            if (ret != SenseShield.SSErrCode.SS_ERROR_INSUFFICIENT_BUFFER)
            {
                Console.WriteLine("缓冲区不足。0x{0:08X}", ret);
                return ret;
            }
            d2c_buf = new byte[d2cSize];
            ret = Libd2c.d2c_get(d2cHandlePtr, d2c_buf, d2cSize, ref d2cSize);
            if (ret == SenseShield.SSErrCode.SS_OK)
            {
                maked_lic_str = Encoding.Default.GetString(d2c_buf);
            }

            return ret;
        }
        //生成写文件字符串  绑定的许可可以先填0吧  
        public static UInt32 make_file(string lic_op, string file_name, byte[] file_buffer, UInt32 bind_lic,ref string maked_lic_str)
        {
            UInt32 ret = 0;
            IntPtr device_handle = IntPtr.Zero;
            IntPtr d2cHandlePtr = IntPtr.Zero;
            // Libd2c.ACCOUNT_TYPE accountType = Libd2c.ACCOUNT_TYPE.NONE;
            string accountID = string.Empty;

            string licGUID = string.Empty;
            UInt32 d2cSize = 0;
            byte[] d2c_buf = null;
            IntPtr errMsgPtr = IntPtr.Zero;
            string errMsg = string.Empty;

            UInt32 CERT_SIZE =2048;
            byte[] root_ca_cert = null;

            //打开控制锁
            ret = Libd2c.master_open(ref device_handle);
            if (ret != 0)
            {
                Console.WriteLine("打开控制锁失败。0x{0:X}", ret);
                return ret;
            }
            //验证控制所pin
            byte[] psd = System.Text.ASCIIEncoding.Default.GetBytes(pin);
            ret = Libd2c.master_pin_verify(device_handle, 0, psd, (UInt32)psd.Length);
            if (ret != 0)
            {
                Console.WriteLine("校验PIN码失败。0x{0:08X}", ret);
                return ret;
            }
            //将字节转化16进制字符串
            string str_buff = null;
            foreach (byte b in file_buffer)
            {
                if (b <= 15)
                    str_buff += "0";
                str_buff += Convert.ToString(b, 16);
            }
            JObject fileObject = new JObject();
            fileObject["op"] = lic_op;
            fileObject["filename"] = file_name;
            fileObject["filetype"] = "evx";
            fileObject["filebuffer"] = str_buff;
            fileObject["fileoffset"] = 0;
            //fileObject["bind_lic"] = bind_lic;

            string strLicJson = JsonConvert.SerializeObject(fileObject);
            string strLicDesc = strLicJson;
            //或得控制锁得CA证书
            root_ca_cert = new byte[CERT_SIZE];
            ret = Libd2c.master_get_ca_cert_ex(device_handle,Libd2c.CA_TYPE.PKI_CA_TYPE_ROOT,1,root_ca_cert,CERT_SIZE,ref CERT_SIZE);
            if (ret != SSErrCode.SS_OK)
            {
                WriteLineRed("获取控制锁证书失败");
            }
            //生成一个d2c句柄
            ret = Libd2c.d2c_file_new(device_handle, ref d2cHandlePtr, Libd2c.SIGN_TYPE.SIGN_TYPE_SEED, root_ca_cert, CERT_SIZE);
            //往d2c中添加许可
            //ret = Libd2c.d2c_add_lic(d2cHandlePtr, strLicJson, strLicDesc, null);

            ret = Libd2c.d2c_add_pkg(d2cHandlePtr, strLicJson, "seed_file_hello_sample");
            WriteLineGreen(strLicJson);
            if (ret != SenseShield.SSErrCode.SS_OK)
            {
                Console.WriteLine("向d2c中添加内容失败。0x{0:08X}", ret);
                return ret;
            }
            ret = Libd2c.d2c_get(d2cHandlePtr, null, 0, ref d2cSize);
            if (ret != SenseShield.SSErrCode.SS_ERROR_INSUFFICIENT_BUFFER)
            {
                Console.WriteLine("缓冲区不足。0x{0:08X}", ret);
                return ret;
            }
            d2c_buf = new byte[d2cSize];
            ret = Libd2c.d2c_get(d2cHandlePtr, d2c_buf, d2cSize, ref d2cSize);
            if (ret == SenseShield.SSErrCode.SS_OK)
            {
                maked_lic_str = Encoding.Default.GetString(d2c_buf);
            }
            return ret;
        }
        //生成清空锁的d2c
        public static UInt32 make_clear_d2c(string lic_op, double start_time,double end_time,ref string maked_lic_str)
        {
            UInt32 ret = 0;
            IntPtr device_handle = IntPtr.Zero;
            IntPtr d2cHandlePtr = IntPtr.Zero;
            UInt32 d2cSize = 0;
            byte[] d2c_buf = null;

            UInt32 CERT_SIZE = 4096;
            byte[] root_ca_cert = new byte[CERT_SIZE];

            //打开控制锁
            ret = Libd2c.master_open(ref device_handle);
            if (ret != 0)
            {
                Console.WriteLine("打开控制锁失败。0x{0:X}", ret);
                return ret;
            }
            //验证控制所pin
            byte[] psd = System.Text.ASCIIEncoding.Default.GetBytes(pin);
            ret = Libd2c.master_pin_verify(device_handle, 0, psd, (UInt32)psd.Length);
            if (ret != 0)
            {
                Console.WriteLine("校验PIN码失败。0x{0:08X}", ret);
                return ret;
            }
                /*       重置锁：
    *       {
    *           "op":           "reset"             操作类型：重置锁
    *           "not_before":   UTC 时间（有效开始时间）
    *           "not_after":    UTC 时间（有效终止时间），升级包在起止时间范围内（包含起止的那一秒）可重复使用。
    *       }
                 */
            //生成jasion
            JObject fileObject = new JObject();
            fileObject["op"] = "reset";
            fileObject["not_before"] = 1607665408;     //Unix时间戳
            fileObject["not_after"] = 1607666008;      //Unix时间戳

            //licJsonObj["start_time"] = "=" + String.Format("{0:D}", start_time);    //Unix时间戳
            //licJsonObj["end_time"] = "=" + String.Format("{0:D}", end_time);      //Unix时间戳
            string strLicJson = JsonConvert.SerializeObject(fileObject);
            string strLicDesc = strLicJson;

            //获取控制锁证书
            ret = Libd2c.master_get_ca_cert_ex(device_handle, Libd2c.CA_TYPE.PKI_CA_TYPE_ROOT, 1, root_ca_cert, CERT_SIZE, ref CERT_SIZE);
            if (ret != SenseShield.SSErrCode.SS_OK)
            {
                Console.WriteLine("缓冲区不足。0x{0:08X}", ret);
                return ret;
            }
            //生成一个d2c句柄
            ret = Libd2c.d2c_file_new(device_handle, ref d2cHandlePtr, Libd2c.SIGN_TYPE.SIGN_TYPE_SEED, root_ca_cert, CERT_SIZE);
            if (ret != SenseShield.SSErrCode.SS_OK)
            {
                WriteLineRed("生成d2c句柄失败");
                return ret;
            }
            //将jason 添加到d2c句柄
            ret = Libd2c.d2c_add_pkg(d2cHandlePtr, strLicJson, "reset dongle");
            WriteLineGreen(strLicJson);
            if (ret != SenseShield.SSErrCode.SS_OK)
            {
                Console.WriteLine("向d2c中添加内容失败。0x{0:08X}", ret);
                return ret;
            }
            ret = Libd2c.d2c_get(d2cHandlePtr, null, 0, ref d2cSize);
            if (ret != SenseShield.SSErrCode.SS_ERROR_INSUFFICIENT_BUFFER)
            {
                Console.WriteLine("缓冲区不足。0x{0:08X}", ret);
                return ret;
            }
            d2c_buf = new byte[d2cSize];
            ret = Libd2c.d2c_get(d2cHandlePtr, d2c_buf, d2cSize, ref d2cSize);
            if (ret == SenseShield.SSErrCode.SS_OK)
            {
                maked_lic_str = Encoding.Default.GetString(d2c_buf);
            }
            return ret;
        }
        //生成许可jasion
        static JObject make_license_json(UInt32 license_id, UInt64 start_time, UInt64 end_time, string lock_type, string data_area_raw, string lic_op, string data_area)
        {
            JObject licJsonObj = new JObject();
            JObject licData    = new JObject();
            UInt32  ret        = 0;
            //licJsonObj["op"]         = "addlic";                                             //控制许可的增加更新删除
            licJsonObj["op"] = lic_op;  
            licJsonObj["license_id"] = license_id;
            licJsonObj["force"]      = true;
            licJsonObj["start_time"] = "=" + String.Format("{0:D}", start_time);    //Unix时间戳
            licJsonObj["end_time"] = "=" + String.Format("{0:D}", end_time);      //Unix时间戳
            //控制单机网络锁
             if (lock_type == "0")
            {//单机锁
                licJsonObj["concurrent_type"] = "process";           //并发类型
                licJsonObj["concurrent"] = "=0";                      //并发数
            }
            else
            {//网络锁
                licJsonObj["concurrent_type"] = "win_user_session";   //并发类型
                licJsonObj["concurrent"] = "=" + lock_type;           //并发数
            }
            //数据区操作
            if (data_area_raw.Length != 0)
            {
                String a = null;
                Byte[] byte_content = System.Text.ASCIIEncoding.Default.GetBytes(data_area_raw);
                int len = data_area_raw.Length;
                Byte[] len_bytes = BitConverter.GetBytes(len);
                Byte[] need_write = new byte[byte_content.Length + sizeof(int)];

                len_bytes.CopyTo(need_write,0);
                byte_content.CopyTo(need_write,len_bytes.Length);
                for (int i = 0; i < need_write.Length; i++)
                {
                    string b_str = "";
                    if (need_write[i] <= 15)
                        b_str += "0";
                    b_str += Convert.ToString(need_write[i], 16);// 转换成16进制字符串
                    a = a + b_str;
                }
                licData["data"]     = a;
                licData["offset"]   = 0;
                licData["resize"]   = need_write.Length;
                licJsonObj[data_area] = licData;                                                                                  //指定写数据的区域
            }
            return licJsonObj;
        }
        public static void WriteD2CToFile(string licOper, string licID,string d2c)
        {
            string filename = string.Format("{0}{1}.d2c", licOper, licID);
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(d2c);
            sw.Close();
            fs.Close();
            WriteLineGreen("成功生成远程升级包！");
        }
        public static byte[] ReadFile2String(string file_path,UInt32 len)
        {
            byte[] filebuffer = new byte[len];
            if (File.Exists(file_path))
            {
                filebuffer = File.ReadAllBytes(file_path);
            }
            return filebuffer;
        }
    }
    //定义一个类方法，将字符串转为byte
    class Libd2cCommon
    {
        public static byte[] HexStrToBytes(string hexstring)
        {
            if (hexstring == string.Empty || hexstring.Length % 2 != 0)
                return null;
            //if (hexstring.Length % 2 != 0)
            //    return null;
            int byteLen = hexstring.Length / 2;
            byte[] returnBytes = new byte[byteLen];
            for (int i = 0; i < byteLen; i++)
            {
                returnBytes[i] = Convert.ToByte(hexstring.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }
    }
}
