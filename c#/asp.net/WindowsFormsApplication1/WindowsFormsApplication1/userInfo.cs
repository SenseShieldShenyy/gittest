using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace opeapiShow
{
    //用户信息类
    class userInfo
    {
        private int NINITDEV = 0x00000001;              //未初始化开发商类
        private int NSETUSERINFO = 0x00000002;          //未初始化用户类
        private int NUSERID = 0x00000003;               //没有设置userid

        //必填项
        private string userAccount = "";
        private string passwd = "";
        private string cellphone = "";
        private string nickname = "";
        //选填项
        private string sex = "";
        private string tel = "";
        private string address = "";
        private string desc = "";
        private string shadowAccount = "";
        private string userID = "";
        private string email = "";
        private int currentPage = 0;
        private int pageSize = 0;
        //默认账号
        private string defAccount = "zhangcq";
        private string defPsd = "123456";
        private string defCellphone = "18700000000";
        private string defNickName = "zhangcqtest";

        private developer Dev;
        private openAPI API;

        public string UserAccount
        {
            set { userAccount = value; }
            get { return userAccount; }
        }

        public string Passwd
        {
            set { passwd = value; }
            get { return passwd; }
        }

        public string Cellphone
        {
            set { cellphone = value; }
            get { return cellphone; }
        }

        public string Nickname
        {
            set { nickname = value; }
            get { return nickname; }
        }

        public string Sex
        {
            set { sex = value; }
            get { return sex; }
        }

        public string Tel
        {
            set { tel = value; }
            get { return tel; }
        }

        public string Address
        {
            set { address = value; }
            get { return address; }
        }

        public string Desc
        {
            set { desc = value; }
            get { return desc; }
        }

        public string ShadowAccount
        {
            get
            {
                return shadowAccount;
            }
        }

        public string UserID
        {
            set { userID = value; }
            get
            {
                return userID;
            }
        }

        public string Email
        {
            set { email = value; }
            get { return email; }
        }

        /// <summary>
        /// 构造函数，用于创建默认账户，删除账户，查找用户等
        /// </summary>
        /// <param name="dev">开发商类</param>
        public userInfo(developer dev)
        {
            userAccount = "";
            passwd = "";
            cellphone = "";
            nickname = "";
            API = new openAPI();
            Dev = dev;

            sex = "";
            tel = "";
            address = "";
            desc = "";
            shadowAccount = "";
            userID = "";
        }
        /// <summary>
        /// 构造函数,用于在数据库中取出影子账号修改密码
        /// </summary>
        /// <param name="account">影子账号</param>
        /// <param name="psd">新密码</param>
        /// <param name="Devloper">开发商信息</param>
        public userInfo(string account, string psd, developer Developer)
        {
            userAccount = "";
            nickname = "";
            API = new openAPI();
            sex = "";
            tel = "";
            address = "";
            desc = "";
            userID = "";
            cellphone = "";

            shadowAccount = account;
            passwd = psd;
            Dev = Developer;
        }
        /// <summary>
        /// 构造函数，用于创建新的普通账户或影子账户
        /// </summary>
        /// <param name="nick">用户昵称</param>
        /// <param name="name">用户名（邮箱）</param>
        /// <param name="password">用户密码</param>
        /// <param name="phone">用户手机号</param>
        /// <param name="Developer">开发商信息</param>
        public userInfo(string nick, string name, string password, string phone, developer Developer)
        {
            userAccount = name;
            passwd = password;
            cellphone = phone;
            nickname = nick;
            Dev = Developer;
            API = new openAPI();

            sex = "";
            tel = "";
            address = "";
            desc = "";
            shadowAccount = "";
            userID = "";
        }
        /// <summary>
        /// 创建默认用户，用来发布批量使用的许可
        /// 此处创建的为影子账号，避免出现重复
        /// </summary>
        /// <returns></returns>
        public int createDefUser(ref string desc)
        {
            nickname = defNickName;
            this.userAccount = defAccount;
            passwd = defPsd;
            cellphone = defCellphone; 
            
            return addShadowUser(ref desc);
        }
        //创建用户
        public int addUser(ref string desc)
        {
            if (Dev.Appid == "" || Dev.Secret == "")
                return NINITDEV;
            if (nickname == "" || userAccount == "" || passwd == "" || cellphone == "")
                return NSETUSERINFO;
            //拼接JSON字符串
            StringBuilder jUser = new StringBuilder();
            StringWriter sw = new StringWriter(jUser);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("nickname");
                writer.WriteValue(nickname);
                writer.WritePropertyName("userAccount");
                writer.WriteValue(userAccount);
                writer.WritePropertyName("passwd");
                writer.WriteValue(passwd);
                writer.WritePropertyName("cellphone");
                writer.WriteValue(cellphone);
                if (sex != "")
                {
                    writer.WritePropertyName("sex");
                    writer.WriteValue(sex);
                }
                if (tel != "")
                {
                    writer.WritePropertyName("tel");
                    writer.WriteValue(tel);
                }
                if (address != "")
                {
                    writer.WritePropertyName("address");
                    writer.WriteValue(address);
                }
                if (desc != "")
                {
                    writer.WritePropertyName("desc");
                    writer.WriteValue(this.desc);
                }
                writer.WriteEndObject();
            }
            //调用公共算法类
            algorithm ALG = new algorithm();
            string userJsonInfo = jUser.ToString();
            string retJsonInfo = ALG.senseCloudRequest(API.AddUser, userJsonInfo, Dev.Appid, Dev.Secret);
            //分析JSON数据
            JObject jobj = JObject.Parse(retJsonInfo);
            int ret = Convert.ToInt32(jobj["code"].ToString());
            desc = jobj["desc"].ToString();
            if (ret == 0)
                userID = jobj["data"]["userId"].ToString();
            return ret;
        }
        //创建影子账户
        public int addShadowUser(ref string desc)
        {
            if (Dev.Appid == "" || Dev.Secret == "")
                return NINITDEV;
            if (userAccount == "" || passwd == "")
                return NSETUSERINFO;
            //拼接json数据
            StringBuilder jShadow = new StringBuilder();
            StringWriter sw = new StringWriter(jShadow);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("shadowAccount");
                writer.WriteValue(userAccount);
                writer.WritePropertyName("passwd");
                writer.WriteValue(passwd);
                writer.WriteEndObject();
            }
            //调用公共算法类
            algorithm ALG = new algorithm();
            string userJsonInfo = jShadow.ToString();
            string retJson = ALG.senseCloudRequest(API.AddShadowUser, userJsonInfo, Dev.Appid, Dev.Secret);
            //分析JSON数据
            JObject Jobj = JObject.Parse(retJson);
            int ret = Convert.ToInt32(Jobj["code"].ToString());
            desc = Jobj["desc"].ToString();
            if(ret == 0)
            {
                shadowAccount = Jobj["data"]["userAccount"].ToString();
                userID = Jobj["data"]["userId"].ToString();
            }
                
            return 0;
        }
        //修改影子账户密码
        public int modifyUserPasswd(string psd, ref string desc)
        {
            if (Dev.Appid == "" || Dev.Secret == "")
                return NINITDEV;
            if (shadowAccount == "" || passwd == "")
                return NSETUSERINFO;
            //拼接json数据
            StringBuilder jShadow = new StringBuilder();
            StringWriter sw = new StringWriter(jShadow);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("userAccount");
                writer.WriteValue(shadowAccount);
                writer.WritePropertyName("passwd");
                writer.WriteValue(psd);
                writer.WriteEndObject();
            }
            //调用公共算法类
            algorithm ALG = new algorithm();
            string userJsonInfo = jShadow.ToString();
            string retJson = ALG.senseCloudRequest(API.ModifyUserPasswd, userJsonInfo, Dev.Appid, Dev.Secret);
            //解析JSON返回值
            JObject jobj = JObject.Parse(retJson);
            int ret = Convert.ToInt32(jobj["code"].ToString());
            desc = jobj["desc"].ToString();
            return ret;
        }
        //删除影子账户
        public int delUser(string userid, ref string desc)
        {
            if (Dev.Appid == "" || Dev.Secret == "")
                return NINITDEV;
            if (userid == "")
                return NUSERID;
            //拼接json数据
            StringBuilder jShadow = new StringBuilder();
            StringWriter sw = new StringWriter(jShadow);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("userId");
                writer.WriteValue(userid);
                writer.WriteEndObject();
            }
            //调用公共算法类
            algorithm ALG = new algorithm();
            string userJsonInfo = jShadow.ToString();
            string retJson = ALG.senseCloudRequest(API.DeleteUser, userJsonInfo, Dev.Appid, Dev.Secret);
            //解析JSON返回值
            JObject jobj = JObject.Parse(retJson);
            int ret = Convert.ToInt32(jobj["code"].ToString());
            desc = jobj["desc"].ToString();
            return ret;
        }
        //查找用户,需要设置至少emial,nickname,cellphone,currentPage,pagesize一项
        public int findUser(ref string desc, ref string usersData)
        {
            if (Dev.Appid == "" || Dev.Secret == "")
                return NINITDEV;
            if (email == "" && nickname == "" && cellphone == "" && currentPage == 0 && pageSize == 0)
                return NSETUSERINFO;
            //拼接json数据
            StringBuilder jShadow = new StringBuilder();
            StringWriter sw = new StringWriter(jShadow);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                if (email != "")
                {
                    writer.WritePropertyName("email");
                    writer.WriteValue(email);
                }
                if (nickname != "")
                {
                    writer.WritePropertyName("nickname");
                    writer.WriteValue(nickname);
                }
                if (cellphone != "")
                {
                    writer.WritePropertyName("cellphone");
                    writer.WriteValue(cellphone);
                }
                if (currentPage != 0)
                {
                    writer.WritePropertyName("currentPage");
                    writer.WriteValue(currentPage);
                }
                if (pageSize != 0)
                {
                    writer.WritePropertyName("pageSize");
                    writer.WriteValue(pageSize);
                }
                writer.WriteEndObject();
            }
            //调用公共算法类
            algorithm ALG = new algorithm();
            string userJsonInfo = jShadow.ToString();
            string retJson = ALG.senseCloudRequest(API.FindUsers, userJsonInfo, Dev.Appid, Dev.Secret);
            //解析JSON返回值
            JObject jobj = JObject.Parse(retJson);
            int ret = Convert.ToInt32(jobj["code"].ToString());
            desc = jobj["desc"].ToString();
            if (ret == 0)
                usersData = jobj["data"].ToString();
            return ret;
        }
        //修改影子账户信息
        public int modifyUser(ref string desc)
        {

            if (Dev.Appid == "" || Dev.Secret == "")
                return NINITDEV;
            if (userID == "")
                return NSETUSERINFO;
            //拼接json数据
            StringBuilder jShadow = new StringBuilder();
            StringWriter sw = new StringWriter(jShadow);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();
                writer.WritePropertyName("userId");
                writer.WriteValue(userID);
                if (nickname != "")
                {
                    writer.WritePropertyName("nickname");
                    writer.WriteValue(nickname);
                }
                if (cellphone != "")
                {
                    writer.WritePropertyName("cellphone");
                    writer.WriteValue(cellphone);
                }
                if (sex != "")
                {
                    writer.WritePropertyName("sex");
                    writer.WriteValue(sex);
                }
                if (tel != "")
                {
                    writer.WritePropertyName("tel");
                    writer.WriteValue(tel);
                }
                if (address != "")
                {
                    writer.WritePropertyName("address");
                    writer.WriteValue(address);
                }
                if (desc != "")
                {
                    writer.WritePropertyName("desc");
                    writer.WriteValue(this.desc);
                }
                writer.WriteEndObject();
            }
            //调用公共算法类
            algorithm ALG = new algorithm();
            string userJsonInfo = jShadow.ToString();
            string retJson = ALG.senseCloudRequest(API.ModifyUser, userJsonInfo, Dev.Appid, Dev.Secret);
            //解析JSON返回值
            JObject jobj = JObject.Parse(retJson);
            int ret = Convert.ToInt32(jobj["code"].ToString());
            desc = jobj["desc"].ToString();
            return ret;
        }

    }
}
