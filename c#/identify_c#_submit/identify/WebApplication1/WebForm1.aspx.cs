using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string pub_data_str = "";
            string guid = Request["guid"];
            if (guid == null || guid.Length == 0)
            {
                Response.Write("服务器无对应session");
                return;
            }

            string key_value = (string)Session[guid];
            if (key_value == null || key_value.Length == 0)
            {
                Response.Write("服务器无对应session");
                return;
            }

            Session.Remove(Request["guid"]);

            byte[] valuebytes = Encoding.UTF8.GetBytes(key_value);
            string hash = BitConverter.ToString(MD5.Create().ComputeHash(valuebytes)).Replace("-", "");
            string num = Request.Form["shell_num"];
            if (Request.Form["shell_num"] != null)
            {
                string base64_sign_data = Request.Form["verify_data"];
                string shell_num = Request.Form["shell_num"];
                string lock_sn = Request.Form["lock_sn"];

                if (LockinfoConfig.need_pub_data() > 0)
                {
                    string pub_data_size = Request.Form["pub_data_size"];
                    byte[] pub_data = Convert.FromBase64String(Request.Form["pub_data"]);

                    //pub_data如果存储的是字符串，在将byte数组转换成字符串时，需要考虑在存储时
                    //使用的格式（开发商管理工具不识别任何格式，直接将数据分隔成byte[]数组储存。
                    //我这里使用UTF8解码是因为存储时使用的UTF8编码。
                    pub_data_str = Encoding.UTF8.GetString(pub_data);
                }
                byte[] signdata = Encoding.UTF8.GetBytes("SENSELOCK" + hash);//signdata 长度为41字节

                if (verifyDevice(signdata, Convert.FromBase64String(base64_sign_data), getDeviceCertsFromDb(shell_num, lock_sn, pub_data_str)))
                {
                    Response.RedirectPermanent("/success.aspx");
                }
                else
                {
                    Response.RedirectPermanent("/failed.aspx");
                }
            }
            else
            {
                Response.Write("未包含指定参数");
                return;
            }
        }

        protected string getDeviceCertsFromDb(string shell_num, string sn, string person_info)
        {
            //该证书链只用于临时测试，请修改成从数据库获取
            string device_certs = "MIIL9AYJKoZIhvcNAQcCoIIL5TCCC+ECAQExADALBgkqhkiG9w0BBwGgggvJMIIDKzCCAhOgAwI" +
                "BAgIDBldHMA0GCSqGSIb3DQEBDQUAMCMxITAfBgNVBAMMGFZpcmJveCBEZXZpY2UgQ0EgMUIgMTAwMTAeFw0yMDA1MDkxM" +
                "DM5MTVaFw00MDA1MDQxMDM5MTVaMIGNMQswCQYDVQQGEwJDTjEQMA4GA1UECAwHQmVpamluZzEQMA4GA1UEBwwHQmVpamlu" +
                "ZzESMBAGA1UECgwJU2Vuc2Vsb2NrMRIwEAYDVQQLDAlTZW5zZWxvY2sxMjAwBgNVBAMMKURFVklDRUlELTk3MzNDODAxMDAw" +
                "NzAyMDc5REE3MDAxMTAwMEIwMDEzMIIBITANBgkqhkiG9w0BAQEFAAOCAQ4AMIIBCQKCAQBxYjq3FA2JWKcOGrAopJuptnfQP" +
                "eT3mGNqIqNfBZFfiZtRJz3K8iGOtVjlB2WDmh3N+EE5Js+JPYR4D7ypBHe44EFuh8E70gS0hpB3CgCZVwalH7z790MIRNImYG4" +
                "IrJ6dkEjSYGCQFM3jNPWNRZTjVVLB0aIbyjOSh2QQzzCC4biOpXbguH8DRFM+c3hu0VSNkb4BXhPhLQ6VSCMpjRVWWy+0iSp/o" +
                "v3qoBVVfprAgB9JjDm5Oi1Dms37itnOB2mUrFbHQKj//NY77O3bYcdlgzVrhPEDVGxIBq8sKWvGe2Nlhxvn1SHRRLqfBf4Ieh4" +
                "5K94f8BYNBlVsg17iMaabAgMBAAEwDQYJKoZIhvcNAQENBQADggEBAG3akLpCBBhDssnXLs6dbBUzaXLZz3Fk/LlsLKkXIB9TE6" +
                "xY/k6YXQsKOxHGMWz+2ASvRozgkI1DZtHcnmBMFWw1iugelxiJAbRKOWOfnh0ZN1TYFJLjeGF2VP7WeHprXN3X7/C2eAqiB2PYE" +
                "geB6jxlhreXIHQXIydRkpKvNIhhtVWu5AWCtBrmwF5hcy5dvl/E7YBlK92TjknQgHOfgou95mVUssI+Z8cNP42GncPcxXPKgfkFUt" +
                "SoVUuyVkEOa0FQEdX9n7mK/OJFl7W6eQhRTwwE+6J4CExJ4b8kc0glcxbi9j6pnLQCBNVc6dnPHx2LeSKiN5Jsq2WyW5m/+6MwggPN" +
                "MIIBtaADAgECAgEEMA0GCSqGSIb3DQEBDQUAMBwxGjAYBgNVBAMMEVZpcmJveCBSb290IENBIFYxMB4XDTE3MTIxMjA3MDQyMloXDT" +
                "M3MTIwNzA3MDQyMlowIzEhMB8GA1UEAwwYVmlyYm94IERldmljZSBDQSAxQiAxMDAxMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMI" +
                "IBCgKCAQEAg2+m8znOf+ITEt0RtkmWTkvSsrlMJnzM+6OZzI7XWxnZRDrghYDofAhc6VDjfJSspGFodsYjWu1Gz/Vdv/wQBbX2bFW" +
                "9OBMDSoVajVjde0OlSdNpQRQxkeD5PJh7k9mKfGVYKyM3ApZ7k6KHr1MQ6CLnR3T4mDhESgQMxqBR920jGnxu3nuQXUfqGW/rfO0rS" +
                "pHDNbVfuYjlyAPRWQlR6IygT8v5O7YVqUX08uLpYsrBAa2UbzPYlHZeAtmRxygp3MYtFb3+33yNMFjj6dJ2s3AtNo6qCwn46v9gaBw" +
                "3hCu9Uw38Imh6V1Got82o1lWAVypUVdLBXDPssJi41ZoZNQIDAQABoxMwETAPBgNVHRMBAf8EBTADAQH/MA0GCSqGSIb3DQEBDQUA" +
                "A4ICAQAJLxiMJ5isiOv9DFcxgN9Ij2TJnHpaa2zGLUOjbfZyFC1BO/Ol4q9dLLvxQ4FdGL1tNMArcHLehRZ1I9LnA2ZiskG3AT8yPi" +
                "8s9LdSjG/OrIr7fwAkGmD/syojaQkRiyoztfi/SlLozoOaxt4QoDsh8WiQh2nUk832zg5yK6K4RAE1LGYDktdyxHTBnI4wT8YN3Mo" +
                "qkEguwH92VWi7BvDtpUVXBuSYrAnk0HoOawomlVXNEA55xH6P6SE6ebpcu+az2awGYAiHDoHXHu4P7YylAejPVo+zYm3coL1J+Ilh3U" +
                "9XYpZ4Qq5pq/MfUFt06dhW7WkvbhdAapq2mpchAgcnduQRwZ0uiTijnMS/Fpl83m2WhYshHkzTbCPc6QlJHKhs0bbig+vcfc7dJM9dXlo" +
                "F14hZV/O3/I0AtND3QCOXSd6DSb8nUxwxFHKDz2b1jIiBQjHw8nB/wZ/RgaZLxRfvDqEyxq20Z8j5Wt8qBh9HOFRcxgOhtgn0Nkvj9zL" +
                "aOLEOmZpQE827ijHsUtBLJp9N7+xG8kRSfddmTRps22Scr/sS67G8t7nsiEpMJKnbBTvPiH+w/JIRIeonNf3YptnhgAoBdI7p24d63aQ" +
                "gXBXpN9Ij2uPLvm61nA6nRYkhU7o6nyPzjLw/MhAoC17HQalTyWB6zxmYT/7CPBA133wq8jCCBMUwggKtoAMCAQICAQAwDQYJKoZIhvc" +
                "NAQENBQAwHDEaMBgGA1UEAwwRVmlyYm94IFJvb3QgQ0EgVjEwHhcNMTcxMjA4MDU1NTQ3WhcNMzcxMjAzMDU1NTQ3WjAcMRowGAYDVQQ" +
                "DDBFWaXJib3ggUm9vdCBDQSBWMTCCAiEwDQYJKoZIhvcNAQEBBQADggIOADCCAgkCggIAcQU28Fk3G/9BKJcoQZAOum+stKAmjhFAPTu" +
                "K6Gx8yfgF988Y+FogJsLkxlb/96FISS4zviI0ujXh15c7oGNMZEGw6bVKKgng/5XuCdW4goexQG4PEWcWhhBBKA222z0TQv6aIJVWZy0" +
                "i+dl13YT+JuvTVe/PUHduFB40pxfbMBb7UNfMgnjr1aTURHIkSpCIRrX1BLSkL9LGF43Ax0c25ActrPcLqgA9poukq0njo4KRTWAr2qU" +
                "tzk4CND50EUBc86F2ZQFVwZ+SZS5wYBvk15ThF7T2q/AcdusaNx/XSsaY86Js5Gyzr6NXmxMDfjnyry6C0Y5fcSDmsTsa8YZ2oeFsddD" +
                "DIlz1ZOOoaM0R1Cyj1LTakj2gn58FjAFVOB0E/8rBdeNu5TCrLSXmiS4qJmiY+oWUAr5fNzBlSj94cGysTH+kbM7c7BfArX98Q48jypXZq" +
                "uZz2UPrlf6u5rBHsW63NiI5pBujiP0/yhRqhv/DCW+Fl5FD5PBhUXBy1zdnvshURVxuycQFo4p8mrmrdh6whQI6oqRVpPzM5zA+1bs6J" +
                "EOVp1SqclpYBR917A0wb+oF9+Y7fwHnFlQJADgZRmf6ypiiZgwdMR++hU2zoJ9huoCOQ97ARWWM5pYIqINSRTnIAS6/344HjoK2LUw6j" +
                "TlN+G/dMJRnWkuHSwoVS2UCAwEAAaMTMBEwDwYDVR0TAQH/BAUwAwEB/zANBgkqhkiG9w0BAQ0FAAOCAgEAWb9R1Vl2OG7VlWIDN7Bfd" +
                "7+MSUwrxeoqb93pMNbq40gclx0mmnKWOlVYhr1ILL+agkSdoTYpM+hXCkPT2sPb0Pkwb/WZjhENF0i3U+Hjl+GzRbsNo00pu/B8ga8bO" +
                "L2HSehbFn0khwsxahDpk/SbPpZ1znYem0YW23qT70d2+s0Q7a0oRqYpHIEAhqm4XqlRdmGc4MbI2JMtFCMQ+IYvXRSfnVp5x0H/nlwJm" +
                "2uuXr49Ia4FziUjPdraCSu5kpuaQjXvArcnRvAPTSzrQM/5iRfFPUrrGI8D+at39BC0IDoJP3edxKnAt6Kr5faYMsqwxYO/QIfOfO0so" +
                "JNsG/5mBfpCTuEaajzWetNSoNyT2xgVQfHQJeYMtWSkeJdycecuokcBQX6u2j+eJQtnOL7EgcGU8PirKsf0NM60BIZql/76WfZKUR+9A" +
                "txILFE2AGYLV1w8+mUh3w4gXCDqPJX3ZVgLo8vZcfEWC4IKxzXWa8UGBojvcWZPBguvHvUkWHONpbQEPMT0gFDGK+44XbZT2mjTJSn/o" +
                "bW5pzxNlsy9mXQAvWNF8PGZMh1py46oZXii9MKhEZNlgHGem61qhehhsGTmpM0ROckNrggs9Tp3oM6muTWgbTJg1UPW0iGjiAQA9O1U7" +
                "oNUkGVtcbUYiuDmTprtNJCZYob5pO9Z7A5QTOIxAA==";




            return device_certs;
        }

        protected bool verifyDevice(byte[] signdata, byte[] signature, string cert_list)
        {
            bool result = false;
            byte[] certs_byte = Convert.FromBase64String(cert_list);
            X509Certificate2Collection Collection = new X509Certificate2Collection();
            Collection.Import(Convert.FromBase64String(cert_list));
            RSACryptoServiceProvider rsa;

            for (int i = 0; i < Collection.Count; i++)
            {
                if (Collection[i].Subject.StartsWith("CN=DEVICEID"))
                {
                    rsa = (RSACryptoServiceProvider)Collection[i].PublicKey.Key;
                    result = rsa.VerifyData(signdata, SHA1.Create(), signature);
                    break;
                }
            }

            return result;
        }
    }
}