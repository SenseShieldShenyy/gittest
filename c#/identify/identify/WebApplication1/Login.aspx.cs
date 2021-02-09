using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WebApplication1.Account
{
    public partial class Login : System.Web.UI.Page
    {
        public string hash = "";
        public string guid = "";

        //设置前端与加密锁交互使用的请求协议， 需要用户端WebServer(分为http/https版本)支持
        public string request_schema = "http://";
        //public string request_schema = "https://";

        public string checkpath = "WebForm1"; //验证请求地方

        public int need_pub = 0; //是否需要前端js读取锁内许可的公开数据区
        public int licid = 0;    //读取的公开区数据所在的许可ID
        protected void Page_Load(object sender, EventArgs e)
        {
            //根据guid，计算session中的hash值
            guid = Guid.NewGuid().ToString();
            DateTime time = DateTime.UtcNow;

            string timestamp = Convert.ToString(Math.Floor((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds));
            Random rd = new Random();
            string key = rd.NextDouble().ToString() + '+' + timestamp;
            byte[] valuebytes = Encoding.UTF8.GetBytes(key);
            hash = BitConverter.ToString(MD5.Create().ComputeHash(valuebytes)).Replace("-", "");

            need_pub = LockinfoConfig.need_pub_data();

            
            Session.Add(guid, key);
        }

        ///// <summary>
        ///// 从数据库中获取指定加密锁的证书链
        ///// </summary>
        ///// <param name="shell_num">外壳号</param>
        ///// <param name="sn">锁内sn码</param>
        ///// <param name="name">用户信息</param>
        ///// <returns>锁证书链</returns>
        //protected string getDeviceCertsFromDb(string shell_num, string sn, string person_info)
        //{
        //    //该证书链只用于临时测试，请修改成从数据库获取
        //    string device_certs = "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";




        //    return device_certs;
        //}

        ///// <summary>
        ///// 验证使用加密锁签名的数据是否正确。
        ///// </summary>
        ///// <param name="signdata">身份认证前端js中使用slm_ctrl_by_device_sign签名的使用base64解码后数据</param>
        ///// <param name="rawdata">加密前的原始数据的byte[]数组</param>
        ///// <param name="cert_list">从数据库中获取到的锁的证书链</param>
        ///// <returns></returns>
        //protected bool verifyDevice(byte[] signdata, byte[] signature, string cert_list)
        //{
        //    bool result = false;
        //    byte[] certs_byte = Convert.FromBase64String(cert_list);
        //    X509Certificate2Collection Collection = new X509Certificate2Collection();
        //    Collection.Import(Convert.FromBase64String(cert_list));
        //    RSACryptoServiceProvider rsa;

        //    for (int i = 0; i < Collection.Count; i++)
        //    {
        //        if (Collection[i].Subject.StartsWith("CN=DEVICEID"))
        //        {
        //            rsa = (RSACryptoServiceProvider)Collection[i].PublicKey.Key;
        //            result = rsa.VerifyData(signdata, SHA1.Create(), signature);
        //            break;
        //        }
        //    }

        //    return result;
        //}
    }
}