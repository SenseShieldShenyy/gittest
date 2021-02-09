using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace openApiForm
{
    class product
    {
        private int NSETPRODUCT = 0x00000001;                      //未初始化产品
        private int INVALIDPARAM = 0x00000002;                     //不合法的参数
        private int INVALIDLICID = 0x00000003;                     //不合法的许可ID
        private int INVALIDLICNAME = 0x00000004;                    //不合法的产品名称
        //内容
        private uint licenseId;                                     //许可ID
        private string productName;                                 //产品名称
        private uint licenseForm;                                   //许可形式(需要JSON 类型拼接)
        private string pub;                                         //公开数据区
        private string raw;                                         //读写数据区
        private string rom;                                         //只读数据区
        private Dictionary<uint, string> moduleInfo;                 //模块信息（需要JSON类型拼接）
        private developer Dev;                                      //开发商类
        private openAPI API;                                        //API类
        //许可形式
        private const uint cloud = 1;
        private const uint slock = 2;
        private const uint cldAndSlk = 3;

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

        public string Pub
        {
            set { pub = value; }
            get { return pub; }
        }

        public string Raw
        {
            set { raw = value; }
            get { return pub; }
        }

        public string Rom
        {
            set { rom = value; }
            get { return rom; }
        }
        /// <summary>
        /// 将模块数据拼接成JSON字符串形式
        /// </summary>
        /// <returns></returns>
        private string module2Json()
        {
            //拼接json数据
            StringBuilder jModule = new StringBuilder();
            StringWriter sw = new StringWriter(jModule);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartArray();
                foreach (var item in moduleInfo)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("moduleId");
                    writer.WriteValue(item.Key);
                    writer.WritePropertyName("moduleName");
                    writer.WriteValue(item.Value);
                    writer.WriteEndObject();
                }
                writer.WriteEnd();
            }
            return jModule.ToString();
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
            moduleInfo = new Dictionary<uint, string>();
            Dev = dev;
            API = new openAPI();
            raw = "";
            rom = "";
            pub = "";
        }
        /// <summary>
        /// 构造函数，主要用于修改产品，删除产品，查询产品
        /// </summary>
        public product(developer dev)
        {
            licenseId = 0;
            productName = "";
            licenseForm = 0;
            moduleInfo = new Dictionary<uint, string>();
            API = new openAPI();
            Dev = dev;
            raw = "";
            rom = "";
            pub = "";
        }
        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="moduleID">模块ID</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public int addModules(uint moduleID, string moduleName)
        {
            if (moduleID == 0 || moduleName == "")
                return INVALIDPARAM;
            moduleInfo.Add(moduleID, moduleName);
            return 0;
        }
        public int creatProduct(ref string desc)
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
                else if (licenseForm == slock)
                    writer.WriteValue(slock);
                else if (licenseForm == cldAndSlk)
                {
                    writer.WriteValue(cloud);
                    writer.WriteValue(slock);
                }
                writer.WriteEnd();
                if (moduleInfo.Count != 0)
                {
                    string module = module2Json();
                    writer.WritePropertyName("modules");
                    writer.WriteValue(module);
                }
                if (raw != "")
                {
                    writer.WritePropertyName("raw");
                    writer.WriteValue(raw);
                }
                if (pub != "")
                {
                    writer.WritePropertyName("pub");
                    writer.WriteValue(pub);
                }
                if (rom != "")
                {
                    writer.WritePropertyName("rom");
                    writer.WriteValue(rom);
                }
                writer.WriteEndObject();
            }
            //调用公共算法类
            algorithm ALG = new algorithm();
            string proJsonInfo = jProduct.ToString();
            string retJson = ALG.senseCloudRequest(API.AddProduct, proJsonInfo, Dev.Appid, Dev.Secret);
            //解析JSON返回值
            JObject jobj = JObject.Parse(retJson);
            int ret = Convert.ToInt32(jobj["code"].ToString());
            desc = jobj["desc"].ToString();
            return ret;
        }
        public int modifyProduct(ref string desc)
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
                if (productName != "")
                {
                    writer.WritePropertyName("productName");
                    writer.WriteValue(productName);
                }
                writer.WritePropertyName("licenseForm");
                writer.WriteStartArray();
                if (licenseForm == cloud)
                    writer.WriteValue(cloud);
                else if (licenseForm == slock)
                    writer.WriteValue(slock);
                else if (licenseForm == cldAndSlk)
                {
                    writer.WriteValue(cloud);
                    writer.WriteValue(slock);
                }
                writer.WriteEnd();
                if (moduleInfo.Count != 0)
                {
                    string module = module2Json();
                    writer.WritePropertyName("modules");
                    writer.WriteValue(module);
                }
                if (raw != "")
                {
                    writer.WritePropertyName("raw");
                    writer.WriteValue(raw);
                }
                if (pub != "")
                {
                    writer.WritePropertyName("pub");
                    writer.WriteValue(pub);
                }
                if (rom != "")
                {
                    writer.WritePropertyName("rom");
                    writer.WriteValue(rom);
                }
                writer.WriteEndObject();
            }
            //调用公共算法类
            algorithm ALG = new algorithm();
            string proJsonInfo = jProduct.ToString();
            string retJson = ALG.senseCloudRequest(API.ModifyProduct, proJsonInfo, Dev.Appid, Dev.Secret);
            //解析JSON返回值
            JObject jobj = JObject.Parse(retJson);
            int ret = Convert.ToInt32(jobj["code"].ToString());
            desc = jobj["desc"].ToString();
            return ret;
        }
        public int findProductByID(ref string desc, ref string proData)
        {
            if (licenseId == 0)
                return INVALIDLICID;
            //拼接json数据
            StringBuilder jProduct = new StringBuilder();
            StringWriter sw = new StringWriter(jProduct);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("licenseId");
                writer.WriteValue(licenseId);
                writer.WriteEndObject();
            }
            //调用公共算法类
            algorithm ALG = new algorithm();
            string proJsonInfo = jProduct.ToString();
            string retJson = ALG.senseCloudRequest(API.GetProductInfo, proJsonInfo, Dev.Appid, Dev.Secret);
            //解析JSON返回值
            JObject jobj = JObject.Parse(retJson);
            int ret = Convert.ToInt32(jobj["code"].ToString());
            desc = jobj["desc"].ToString();
            if (ret == 0)
                proData = jobj["data"].ToString();
            return ret;
        }
        public int findProductByName(ref string desc, ref string proData)
        {
            if (productName == "")
                return INVALIDLICNAME;
            //拼接json数据
            StringBuilder jProduct = new StringBuilder();
            StringWriter sw = new StringWriter(jProduct);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("productName");
                writer.WriteValue(productName);
                writer.WriteEndObject();
            }
            //调用公共算法类
            algorithm ALG = new algorithm();
            string proJsonInfo = jProduct.ToString();
            string retJson = ALG.senseCloudRequest(API.ProductList, proJsonInfo, Dev.Appid, Dev.Secret);
            //解析JSON返回值
            JObject jobj = JObject.Parse(retJson);
            int ret = Convert.ToInt32(jobj["code"].ToString());
            desc = jobj["desc"].ToString();
            if (ret == 0)
                proData = jobj["data"].ToString();
            return ret;
        }
        public int delProduct(ref string desc)
        {
            if (licenseId == 0)
                return INVALIDLICID;
            //拼接json数据
            StringBuilder jProduct = new StringBuilder();
            StringWriter sw = new StringWriter(jProduct);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("licenseId");
                writer.WriteValue(licenseId);
                writer.WriteEndObject();
            }
            //调用公共算法类
            algorithm ALG = new algorithm();
            string proJsonInfo = jProduct.ToString();
            string retJson = ALG.senseCloudRequest(API.DeleteProduct, proJsonInfo, Dev.Appid, Dev.Secret);
            //解析JSON返回值
            JObject jobj = JObject.Parse(retJson);
            int ret = Convert.ToInt32(jobj["code"].ToString());
            desc = jobj["desc"].ToString();

            return ret;
        }
    }
}
