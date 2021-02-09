using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opeapiShow
{
    class template
    {
        private int NSETTEMPLATE = 0x00000001;                      //没有初始化模板类
        private int INVALIDPARAM = 0x00000002;                     //不合法的参数
        private int INVALIDLICID = 0x00000003;                     //不合法的许可ID
        private int INVALIDTEMPID = 0x00000004;                     //不合法的模板ID

        private int tempID;                      //模板ID
        private string templateName;                //模板名称
        private int licenseId;                      //产品许可编号
        private int licenseForm;                    //许可形式(云，软)
        private bool isForever;                     //是否可永久使用
        private string licStartTimeType;               //时间限制类型
        private int startTime;                      //开始时间
        private int endTime;                        //结束时间
        private int spanTime;                       //时间跨度
        private int times;                          //使用次数
        private int licDuration;                    //许可的使用时长
        private int offlineDays;                    //软锁离线时长
        private int licBindLimit;                   //可绑定的设备数
        private int licBindMaxLimit;                //可累计绑定的设备数
        private int version;                        //许可版本
        private string pub;                         //公开数据区数据（16进制字符串）
        private string rom;                         //只读数据区数据（16进制字符串）
        private string raw;                         //读写数据区数据（16进制字符串）
        Dictionary<uint, string> moduleInfo;         //模块区数据（需转成JSON字符串）
        private string canModify;                      //发许可时，模板的许可限制是否允许临时修改
        private developer Dev;                      //开发商类
        private openAPI API;                        //openapi类

        //许可形式
        private const uint cloud = 1;
        private const uint slock = 2;

        public enum licForm
        {
            cloud = 1,                      //云锁
            slock = 2,                      //软锁
        }

        public string TemplateName
        {
            set { templateName = value; }
            get { return templateName; }
        }
        public int LicenseId
        {
            set { licenseId = value; }
            get { return licenseId; }
        }
        public int LicenseForm
        {
            set { licenseForm = value; }
            get { return licenseForm; }
        }
        public bool IsForever
        {
            set { isForever = value; }
            get { return isForever; }
        }
        public string LicStartTimeType
        {
            set { licStartTimeType = value; }
            get { return licStartTimeType; }
        }
        public int StartTime
        {
            set { startTime = value; }
            get { return startTime; }
        }
        public int Endtime
        {
            set { endTime = value; }
            get { return endTime; }
        }
        public int SpanTime
        {
            set { spanTime = value; }
            get { return spanTime; }
        }
        public int Times
        {
            set { times = value; }
            get { return times; }
        }
        public int LicDuration
        {
            set { licDuration = value; }
            get { return licDuration; }
        }
        public int OfflineDays
        {
            set { offlineDays = value; }
            get { return offlineDays; }
        }
        public int LicBindLimit
        {
            set { licBindLimit = value; }
            get { return licBindLimit; }
        }
        public int LicBindMaxLimit
        {
            set { licBindMaxLimit = value; }
            get { return licBindMaxLimit; }
        }
        public int Version
        {
            set { version = value; }
            get { return version; }
        }
        public string Pub
        {
            set { pub = value; }
            get { return pub; }
        }
        public string Rom
        {
            set { rom = value; }
            get { return rom; }
        }
        public string Raw
        {
            set { raw = value; }
            get { return raw; }
        }
        public string CanModify
        {
            set { canModify = value; }
            get { return canModify; }
        }

        public int TempID
        {
            set { tempID = value; }
            get { return tempID; }
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
        /// 构造函数，用于查找，删除，修改销售模板
        /// </summary>
        /// <param name="dev"></param>
        public template(developer dev)
        {
            tempID = 0;                        //模板ID
            templateName = "";                //模板名称
            licenseId = 0;                      //产品许可编号
            licenseForm = 0;                    //许可形式(云，软)
            isForever = false;                     //是否可永久使用
            licStartTimeType = "";               //时间限制类型
            startTime = 0;                      //开始时间
            endTime = 0;                        //结束时间
            spanTime = 0;                       //时间跨度
            times = 0;                          //使用次数
            licDuration = 0;                    //许可的使用时长
            offlineDays = 0;                    //软锁离线时长
            licBindLimit = 0;                   //可绑定的设备数
            licBindMaxLimit = 0;                //可累计绑定的设备数
            version = 0;                        //许可版本
            pub = "";                         //公开数据区数据（16进制字符串）
            rom = "";                         //只读数据区数据（16进制字符串）
            raw = "";                         //读写数据区数据（16进制字符串）   	
            moduleInfo = new Dictionary<uint, string>();         //模块区数据（需转成JSON字符串）
            canModify = "";                      //发许可时，模板的许可限制是否允许临时修改
            Dev = dev;                      //开发商类
            API = new openAPI();                        //openapi类
        }
        /// <summary>
        /// 构造函数 
        /// </summary>
        /// <param name="tempName">模板名称</param>
        /// <param name="licID">许可ID</param>
        /// <param name="licForm">许可形式</param>
        /// <param name="isForever">是否可以永久使用</param>
        /// <param name="dev">开发商类</param>
        public template(string tempName, int licID, int licForm, bool isForever, developer dev)
        {
            tempID = 0;                        //模板ID
            templateName = tempName;                //模板名称
            licenseId = licID;                      //产品许可编号
            licenseForm = licForm;                    //许可形式(云，软)
            this.isForever = isForever;                     //是否可永久使用
            licStartTimeType = "";               //时间限制类型
            startTime = 0;                      //开始时间
            endTime = 0;                        //结束时间
            spanTime = 0;                       //时间跨度
            times = 0;                          //使用次数
            licDuration = 0;                    //许可的使用时长
            offlineDays = 0;                    //软锁离线时长
            licBindLimit = 0;                   //可绑定的设备数
            licBindMaxLimit = 0;                //可累计绑定的设备数
            version = 0;                        //许可版本
            pub = "";                         //公开数据区数据（16进制字符串）
            rom = "";                         //只读数据区数据（16进制字符串）
            raw = "";                         //读写数据区数据（16进制字符串）   	
            moduleInfo = new Dictionary<uint, string>();         //模块区数据（需转成JSON字符串）
            canModify = "";                      //发许可时，模板的许可限制是否允许临时修改
            Dev = dev;                      //开发商类
            API = new openAPI();                        //openapi类
        }
        /// <summary>
        /// 创建销售模板
        /// </summary>
        /// <param name="desc">错误描述</param>
        /// <param name="tempID">生成的模板ID</param>
        /// <returns></returns>
        public int createTemp(ref string desc)
        {
            if (licenseId == 0)
                return INVALIDLICID;
            if (templateName == "" || licenseForm == 0)
                return NSETTEMPLATE;
            if (licenseForm == slock && (offlineDays == 0 || licBindLimit == 0 || licBindMaxLimit == 0))
                return NSETTEMPLATE;
            //拼接json数据
            StringBuilder jTemp = new StringBuilder();
            StringWriter sw = new StringWriter(jTemp);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("licenseId");
                writer.WriteValue(licenseId);
                writer.WritePropertyName("templateName");
                writer.WriteValue(templateName);
                if (licStartTimeType != "")
                {
                    writer.WritePropertyName("licStartTimeType");
                    writer.WriteValue(Convert.ToInt32(licStartTimeType));
                }
                if (canModify != "")
                {
                    writer.WritePropertyName("canModify");
                    writer.WriteValue(Convert.ToInt32(canModify));
                }
                
                writer.WritePropertyName("isForever");
                writer.WriteValue(isForever);
                //许可类型（云，软）
                writer.WritePropertyName("licenseForm");
                if (licenseForm == cloud)
                {
                    writer.WriteValue(cloud);
                }
                else if (licenseForm == slock)
                {
                    writer.WriteValue(slock);
                    writer.WritePropertyName("offlineDays");
                    writer.WriteValue(offlineDays);
                    writer.WritePropertyName("licBindLimit");
                    writer.WriteValue(licBindLimit);
                    writer.WritePropertyName("licBindMaxLimit");
                    writer.WriteValue(licBindMaxLimit);
                }
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
                if (startTime != 0)
                {
                    writer.WritePropertyName("startTime");
                    writer.WriteValue(startTime);
                }
                if (endTime != 0)
                {
                    writer.WritePropertyName("endTime");
                    writer.WriteValue(endTime);
                }
                if (spanTime != 0)
                {
                    writer.WritePropertyName("spanTime");
                    writer.WriteValue(spanTime);
                }
                if (licDuration != 0)
                {
                    writer.WritePropertyName("licDuration");
                    writer.WriteValue(licDuration);
                }
                if (version != 0)
                {
                    writer.WritePropertyName("version");
                    writer.WriteValue(version);
                }

                writer.WriteEndObject();
            }
            //调用公共算法类
            algorithm ALG = new algorithm();
            string tempJsonInfo = jTemp.ToString();
            string retJson = ALG.senseCloudRequest(API.AddTemplate, tempJsonInfo, Dev.Appid, Dev.Secret);
            //解析JSON返回值
            JObject jobj = JObject.Parse(retJson);
            int ret = Convert.ToInt32(jobj["code"].ToString());
            desc = jobj["desc"].ToString();
            if (ret == 0)
                tempID = Convert.ToInt32(jobj["data"]["templateId"].ToString());
            return ret;
        }
        /// <summary>
        /// 修改销售模板
        /// </summary>
        /// <param name="desc">错误描述</param>
        /// <returns></returns>
        public int modifyTemp(ref string desc)
        {
            if (tempID == 0)
                return INVALIDTEMPID;
            //拼接json数据
            StringBuilder jTemp = new StringBuilder();
            StringWriter sw = new StringWriter(jTemp);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("templateId");
                writer.WriteValue(tempID);
                if (templateName != "")
                {
                    writer.WritePropertyName("templateName");
                    writer.WriteValue(templateName);
                }
                if (LicStartTimeType != "")
                {
                    writer.WritePropertyName("licStartTimeType");
                    writer.WriteValue(licStartTimeType);
                }
                if (canModify != "")
                {
                    writer.WritePropertyName("canModify");
                    writer.WriteValue(Convert.ToInt32(canModify));
                }
                //许可类型（云，软）
                writer.WritePropertyName("licenseForm");
                if (licenseForm == cloud)
                {
                    writer.WriteValue(cloud);
                }
                else if (licenseForm == slock)
                {
                    writer.WriteValue(slock);
                    if (offlineDays != 0)
                    {
                        writer.WritePropertyName("offlineDays");
                        writer.WriteValue(offlineDays);
                    }
                    if (LicBindLimit != 0)
                    {
                        writer.WritePropertyName("licBindLimit");
                        writer.WriteValue(licBindLimit);
                    }
                    if (licBindMaxLimit != 0)
                    {
                        writer.WritePropertyName("licBindMaxLimit");
                        writer.WriteValue(licBindMaxLimit);
                    }
                }
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
                if (startTime != 0)
                {
                    writer.WritePropertyName("startTime");
                    writer.WriteValue(startTime);
                }
                if (endTime != 0)
                {
                    writer.WritePropertyName("endTime");
                    writer.WriteValue(endTime);
                }
                if (spanTime != 0)
                {
                    writer.WritePropertyName("spanTime");
                    writer.WriteValue(spanTime);
                }
                if (licDuration != 0)
                {
                    writer.WritePropertyName("licDuration");
                    writer.WriteValue(licDuration);
                }
                if (version != 0)
                {
                    writer.WritePropertyName("version");
                    writer.WriteValue(version);
                }

                writer.WriteEndObject();
            }
            //调用公共算法类
            algorithm ALG = new algorithm();
            string tempJsonInfo = jTemp.ToString();
            string retJson = ALG.senseCloudRequest(API.ModifyTemplate, tempJsonInfo, Dev.Appid, Dev.Secret);
            //解析JSON返回值
            JObject jobj = JObject.Parse(retJson);
            int ret = Convert.ToInt32(jobj["code"].ToString());
            desc = jobj["desc"].ToString();
            return ret;
        }
        /// <summary>
        /// 根据产品ID（许可ID）查找模板
        /// </summary>
        /// <param name="desc">错误描述</param>
        /// <returns></returns>
        public int findTempByProdID(ref string desc, ref string tempData)
        {
            if (licenseId == 0)
                return INVALIDLICID;
            //拼接json数据
            StringBuilder jTemp = new StringBuilder();
            StringWriter sw = new StringWriter(jTemp);
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
            string tempJsonInfo = jTemp.ToString();
            string retJson = ALG.senseCloudRequest(API.TemplateList, tempJsonInfo, Dev.Appid, Dev.Secret);
            //解析JSON返回值
            JObject jobj = JObject.Parse(retJson);
            int ret = Convert.ToInt32(jobj["code"].ToString());
            desc = jobj["desc"].ToString();
            if (ret == 0)
                tempData = jobj["data"].ToString();
            return ret;
        }
        /// <summary>
        /// 根据ID查找销售模板
        /// </summary>
        /// <param name="desc">错误描述</param>
        /// <returns></returns>
        public int findTempByTempID(ref string desc, ref string tempData)
        {
            if (tempID == 0)
                return INVALIDTEMPID;
            //拼接json数据
            StringBuilder jTemp = new StringBuilder();
            StringWriter sw = new StringWriter(jTemp);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("templateId");
                writer.WriteValue(tempID);
                writer.WriteEndObject();
            }
            //调用公共算法类
            algorithm ALG = new algorithm();
            string tempJsonInfo = jTemp.ToString();
            string retJson = ALG.senseCloudRequest(API.FindTemplate, tempJsonInfo, Dev.Appid, Dev.Secret);
            //解析JSON返回值
            JObject jobj = JObject.Parse(retJson);
            int ret = Convert.ToInt32(jobj["code"].ToString());
            desc = jobj["desc"].ToString();
            if (ret == 0)
                tempData = jobj["data"].ToString();
            return ret;
        }
        /// <summary>
        /// 删除指定ID的销售模板
        /// </summary>
        /// <param name="desc">错误描述</param>
        /// <returns></returns>
        public int delTemp(ref string desc)
        {
            if (tempID == 0)
                return INVALIDTEMPID;
            //拼接json数据
            StringBuilder jTemp = new StringBuilder();
            StringWriter sw = new StringWriter(jTemp);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("templateId");
                writer.WriteValue(tempID);
                writer.WriteEndObject();
            }
            //调用公共算法类
            algorithm ALG = new algorithm();
            string tempJsonInfo = jTemp.ToString();
            string retJson = ALG.senseCloudRequest(API.DeleteTemplate, tempJsonInfo, Dev.Appid, Dev.Secret);
            //解析JSON返回值
            JObject jobj = JObject.Parse(retJson);
            int ret = Convert.ToInt32(jobj["code"].ToString());
            desc = jobj["desc"].ToString();
            return ret;
        }

    }
}
