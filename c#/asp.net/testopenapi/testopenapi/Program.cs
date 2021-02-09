using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;

namespace testopenapi
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
    class developer
    {
        private string appid = "";
        private string secret = "";

        public developer(string appid, string secret)
        {
            this.appid = appid;
            this.secret = secret;
        }

        public string Appid
        {
            get { return appid; }
        }

        public string Secret
        {
            get { return secret; }
        }
    }
    class openAPI
    {
        //通用地址
        private string httpsUrl = "https://openapi.senseyun.com";
        private string auth = "https://openapi.senseyun.com/v2/sv/";
        //功能地址
        private string addProduct = "addProduct";                    //添加产品
        public string HttpsUrl
        {
            get
            {
                return httpsUrl;
            }
        }
        //拼接
        public string AddProduct
        {
            get
            {
                return auth + addProduct;
            }
        }
    }
    class algorithm 
    {
        public string getTime()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ttp = Convert.ToInt64(ts.TotalMilliseconds).ToString();
            return ttp;
        }
        public string getUUID()
        {
            string uuid = Guid.NewGuid().ToString();
            return uuid;
        }
        public string getSignData(byte[] allData, string secret)
        {
            HMACSHA256 hmacsha256 = new HMACSHA256();
            hmacsha256.Key = Encoding.UTF8.GetBytes(secret);
            byte[] hashBytes = hmacsha256.ComputeHash(allData);
            return Convert.ToBase64String(hashBytes);
        }
        public static string ToHexString(byte[] bytes)
            {
                string hexString = string.Empty;
                if (bytes != null)
                {
                    StringBuilder strB = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        strB.Append(bytes[i].ToString("X2"));
                    }
                    hexString = strB.ToString();
                }
                return hexString;
            }
        public static string sendHttpPost(String url, String appId, String timestamp,
            String uuid, String signData, byte[] data)
        {
            Stream smRequest = null;
            Stream smResponse = null;
            StreamReader reader = null;
            HttpWebRequest request = null;
            try 
            {
                if (url.Substring(0, 5).ToLower().Equals("https"))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                        new System.Net.Security.RemoteCertificateValidationCallback(checkValidationResult);
                    request = WebRequest.Create(url) as HttpWebRequest;
                    request.ProtocolVersion = HttpVersion.Version11;
                }
                else
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                }
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("SenseAppID", appId);
                request.Headers.Add("SenseTimestamp", timestamp);
                request.Headers.Add("SenseNonce", uuid);
                request.Headers.Add("SenseSign", signData);
                try
                {
                    smRequest = request.GetRequestStream();
                    smRequest.Write(data, 0, data.Length);
                }
                catch (Exception ex)
                {
                    throw new Exception("发生异常：", ex);
                }
                finally
                {
                    if (smRequest != null)
                    {
                        smRequest.Close();
                    }
                }
                HttpWebResponse repsonse = request.GetResponse() as HttpWebResponse;
                //获得执行结果
                smResponse = repsonse.GetResponseStream();
                reader = new StreamReader(smResponse, Encoding.UTF8);
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("发生异常：", ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (smResponse != null)
                {
                    smResponse.Close();
                }
            }

        }
        public static bool checkValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //总是接受
            return true;
        }
        public string senseCloudRequest(string url, string JsonInfo, string appid, string secret)
        {
            string ttp = getTime();
            string uuid = getUUID();
            openAPI api = new openAPI();
            string addUserFormat = url.Substring(api.HttpsUrl.Length);
            //将数据转成字节数组形式，在网络上传递使用
            byte[] ttpBytes = Encoding.UTF8.GetBytes(ttp);
            byte[] uuidBytes = Encoding.UTF8.GetBytes(uuid);
            byte[] resourceURI = Encoding.UTF8.GetBytes(addUserFormat);
            byte[] JsonInfoBytes = Encoding.UTF8.GetBytes(JsonInfo);
            byte[] allData = new byte[ttpBytes.Length + uuidBytes.Length + resourceURI.Length + JsonInfoBytes.Length];
            //拼接字节数组
            ttpBytes.CopyTo(allData, 0);
            uuidBytes.CopyTo(allData, ttpBytes.Length);
            resourceURI.CopyTo(allData, ttpBytes.Length + uuidBytes.Length);
            JsonInfoBytes.CopyTo(allData, ttpBytes.Length + uuidBytes.Length + resourceURI.Length);
            //签名
            String signData = getSignData(allData, secret);
            //请求数据
            return sendHttpPost(url, appid, ttp, uuid, signData, JsonInfoBytes);
        }
    }
    class product
    {
        //常见错误码
        private int NSETPRODUCT = 0x00000001;                      //未初始化产品
        private int INVALIDPARAM = 0x00000002;                     //不合法的参数
        private int INVALIDLICID = 0x00000003;                     //不合法的许可ID
        private int INVALIDLICNAME = 0x00000004;                    //不合法的产品名称
        //属性
        private uint licenseId;                                     //许可ID
        private string productName;                                 //产品名称
        private uint licenseForm;                                   //许可形式(需要JSON 类型拼接)
        private developer Dev;                                      //开发商类
        private openAPI API;                                        //API类
        private const uint cloud = 1;

        public enum licForm
        {
            cloud = 1,                      //云锁
            slock = 2,                      //软锁
            cldAndSlk = 3                   //云/软锁同时建立
        }

        public uint LicenseId
        {
            set { licenseId = value; }
            get { return licenseId; }
        }

        public string ProductName
        {
            set { productName = value; }
            get { return productName; }
        }

        public uint LicenseForm
        {
            set { licenseForm = value; }
            get { return licenseForm; }
        }

        /// <summary>
        /// 构造函数,主要用于创建产品
        /// </summary>
        /// <param name="licID">设定的许可ID</param>
        /// <param name="procName">产品名称</param>
        /// <param name="licForm">许可模式（云，软，云and软）</param>
        public product(uint licID, string procName, uint licForm, developer dev)
        {
            licenseId = licID;
            productName = procName;
            licenseForm = licForm;
            Dev = dev;
            API = new openAPI();
        }
        public int creatproduct(ref  string desc)
        {
            if (licenseId == 0)
                return INVALIDLICID;
            if (productName == "" || licenseForm == 0)
                return NSETPRODUCT;
            //拼接json数据
            StringBuilder jProduct = new StringBuilder();
            StringWriter sw = new StringWriter(jProduct);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("licenseId");
                writer.WriteValue(licenseId);
                writer.WritePropertyName("productName");
                writer.WriteValue(productName);
                //许可类型（云，软，云and软）
                writer.WritePropertyName("licenseForm");
                writer.WriteStartArray();
                if (licenseForm == cloud)
                    writer.WriteValue(cloud);
                writer.WriteEnd();
                writer.WriteEndObject();
            }
            algorithm ALG = new algorithm();
        }
    }
}
