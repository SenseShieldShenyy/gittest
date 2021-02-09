using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace openAPI
{
    class algorithm
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public string getTime()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ttp = Convert.ToInt64(ts.TotalMilliseconds).ToString();
            return ttp;
        }
        /// <summary>
        /// 获取UUID
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 字节数组转hex字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>hex字符串</returns>
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

        /// <summary>
        /// HTTP POST 调用.
        /// </summary>
        /// <param name="url">接口地址.</param>
        /// <param name="appId">appId</param>
        /// <param name="timestamp">时间戳，1970年01月01日00时00分00秒起至现在的总毫秒数，和服务器相差时间超过允许值，API将返回授权失败.</param>
        /// <param name="uuid">唯一识别码，用于一段时间的防重放.</param>
        /// <param name="signData">hmac-sha256签名后的数据.</param>
        /// <param name="data">要POST的数据字节数组.</param>
        /// <returns>返回数据的字符串格式.</returns>
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
                //发送数据
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
        /// <summary>
        /// 向深思云平台发送请求
        /// </summary>
        /// <returns></returns>
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
}
