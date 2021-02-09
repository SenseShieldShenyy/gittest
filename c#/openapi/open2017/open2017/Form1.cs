using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using opeapiShow;


namespace open2017
{
    public partial class Form1 : Form
    {

        //查询深思开发者平台中appid/secret
        JObject licJsonObj = new JObject();
        
        string[] modid_arry;
        string[] modnm_arry;
        string appid = "044b79df99e245f8ac547f892b197b59";
        string secret = "fdfee335c3b34bfe991dffb6016a4d31";
        UInt16 MAX_PRO_NAME = 64;
        UInt16 MAX_DSP_NAME = 16;
        bool bt1_State = false;
        bool bt2_State = false;
        bool bt3_State = false;
        bool bt4_State = false;
        bool bt5_State = false;

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            textBox9.Enabled = false;
            textBox10.Enabled = false;
            textBox11.Enabled = false;
            textBox12.Enabled = false;
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            checkBox3.Enabled = false;
            checkBox4.Enabled = false;
            checkBox5.Enabled = false;
            checkBox6.Enabled = false;
            comboBox1.Enabled = false;
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if(textBox3.Text.Length> MAX_PRO_NAME)
            {
                textBox8.Text = "产品名称长度不得超过64个字符";
            }
            else if (0 != string.Compare("", textBox3.Text))
            {
                licJsonObj["productName"] = textBox3.Text;
                textBox7.Text = JsonConvert.SerializeObject(licJsonObj);
            }
        }
        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (textBox12.Text.Length > MAX_DSP_NAME)
            {
                textBox8.Text = "显示名称长度不得超过64个字符";
            }

            else if (0 != string.Compare("", textBox12.Text))
            {
                licJsonObj["displayName"] = textBox12.Text;
                textBox7.Text = JsonConvert.SerializeObject(licJsonObj);
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (0 != string.Compare("", textBox4.Text))
            {
                licJsonObj["licenseId"] = Convert.ToUInt32(textBox4.Text);
                textBox7.Text = JsonConvert.SerializeObject(licJsonObj);
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            JArray jForm = new JArray();
            if (0 != string.Compare("", comboBox1.Text))
            {
                if (0 == String.Compare(comboBox1.Text, "云锁"))
                {
                    jForm.Add(1);
                }
                else if (0 == String.Compare(comboBox1.Text, "软锁"))
                {
                    jForm.Add(2);

                }
                else if (0 == String.Compare(comboBox1.Text, "硬件锁"))
                {
                    jForm.Add(4);

                }
                else if (0 == String.Compare(comboBox1.Text, "云软锁"))
                {
                    jForm.Add(1);
                    jForm.Add(2);
                }
                else if (0 == String.Compare(comboBox1.Text, "云硬锁"))
                {
                    jForm.Add(1);
                    jForm.Add(4);
                }
                else if (0 == String.Compare(comboBox1.Text, "软硬锁"))
                {
                    jForm.Add(2);
                    jForm.Add(4);
                }
                else if (0 == String.Compare(comboBox1.Text, "云软硬锁"))
                {
                    jForm.Add(1);
                    jForm.Add(2);
                    jForm.Add(4);
                }
            }
            licJsonObj["licenseForm"] = jForm;
            textBox7.Text = JsonConvert.SerializeObject(licJsonObj);
        }



        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            modid_arry = textBox5.Text.Split(' ');
            JArray jMod_ary = new JArray();
            JObject jMod = new JObject();
            for (int i = 0; i < modid_arry.Length; i++)
            {
                if ("" != modid_arry[i])
                {
                    jMod["moduleId"] = modid_arry[i];
                    jMod_ary.Add(jMod);
                }
                //jMod["moduleId"] = modid_arry[i];
                //jMod_ary.Add(jMod);
            }
            licJsonObj["modules"] = jMod_ary;
            if ("" == textBox5.Text)
            {
                licJsonObj.Remove("modules");
            }
            textBox7.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            modid_arry = textBox5.Text.Split(' ');
            modnm_arry = textBox6.Text.Split(' ');
            JArray jMod_ary = new JArray();
           
            for (int i = 0; i < modid_arry.Length; i++)
            {
                if ("" != modid_arry[i])
                {
                    JObject jMod = new JObject();
                    jMod["moduleId"] = modid_arry[i];
                    if (i < modnm_arry.Length && "" != modnm_arry[i])
                    {
                        jMod["moduleName"] = modnm_arry[i];
                    }
                    jMod_ary.Add(jMod);
                }
                
                //jMod["moduleId"] = modid_arry[i];
                //jMod_ary.Add(jMod);
            }
            
            licJsonObj["modules"] = jMod_ary;
            if ("" == textBox5.Text)
            {
                licJsonObj.Remove("modules");
            }
            textBox7.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (bt1_State)
            {
                openAPI API = new openAPI();
                string retJson = algorithm.senseCloudRequest(API.AddProduct, JsonConvert.SerializeObject(licJsonObj), appid, secret);
                JObject jobj = JObject.Parse(retJson);
                int ret = Convert.ToInt32(jobj["code"].ToString());
                string desc = jobj["desc"].ToString();
                Console.WriteLine("创建产品 ret = {0}, desc = {1}", ret, desc);
                textBox8.Text = "创建产品 ret = " + ret.ToString() + "; desc =" + desc;
            }
            else
            {

                textBox4.Enabled = true;
                checkBox5.Enabled = true;
                checkBox6.Enabled = true;
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
                checkBox4.Enabled = true;
                comboBox1.Enabled = true;
                textBox7.Enabled = true;
                textBox8.Enabled = true;
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                bt1_State = true;
                bt2_State = false;
                bt3_State = false;
                bt4_State = false;
                bt5_State = false;
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (bt2_State)
            {
                if (textBox4.Text.Length > 0)
                {
                    JObject dellicJsonObj = new JObject();
                    dellicJsonObj["licenseId"] = Convert.ToUInt32(textBox4.Text);
                    openAPI API = new openAPI();
                    string retJson = algorithm.senseCloudRequest(API.DeleteProduct, JsonConvert.SerializeObject(dellicJsonObj), appid, secret);
                    JObject jobj = JObject.Parse(retJson);
                    int ret = Convert.ToInt32(jobj["code"].ToString());
                    string desc = jobj["desc"].ToString();
                    textBox8.Text = "删除产品 ret = " + ret.ToString() + "; desc =" + desc;
                    Console.WriteLine("删除产品 ret = {0}, desc = {1}", ret, desc);
                }
            }
            else
            {

                textBox4.Enabled = true;
                checkBox5.Enabled = true;
                checkBox6.Enabled = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                checkBox4.Enabled = false;
                comboBox1.Enabled = false;
                textBox7.Enabled = true;
                textBox8.Enabled = true;
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = true;
                checkBox6.Checked = false;
                bt1_State = false;
                bt2_State = true;
                bt3_State = false;
                bt4_State = false;
                bt5_State = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (bt3_State)
            {
                if (textBox4.Text.Length > 0)
                {
                    JObject findJasonObj = new JObject();
                    findJasonObj["licenseId"] = Convert.ToUInt32(textBox4.Text);
                    openAPI API = new openAPI();
                    string retJson = algorithm.senseCloudRequest(API.GetProductInfo, JsonConvert.SerializeObject(findJasonObj), appid, secret);
                    JObject jobj = JObject.Parse(retJson);
                    int ret = Convert.ToInt32(jobj["code"].ToString());
                    string desc = jobj["data"].ToString();
                    textBox8.Text = "查找产品 ret = " + ret.ToString() + ";\r\n desc =" + desc;
                }
            }
            else
            {
                textBox4.Enabled = true;
                checkBox5.Enabled = true;
                checkBox6.Enabled = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                checkBox4.Enabled = false;
                comboBox1.Enabled = false;
                textBox7.Enabled = true;
                textBox8.Enabled = true;
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                bt1_State = false;
                bt2_State = false;
                bt3_State = true;
                bt4_State = false;
                bt5_State = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (bt4_State)
            {
                JObject findJasonObj = new JObject();
                findJasonObj["productName"] = textBox3.Text;
                openAPI API = new openAPI();
                string retJson = algorithm.senseCloudRequest(API.ProductList, JsonConvert.SerializeObject(findJasonObj), appid, secret);
                JObject jobj = JObject.Parse(retJson);
                int ret = Convert.ToInt32(jobj["code"].ToString());
                string desc = jobj["data"].ToString();
                textBox8.Text = "查找产品 ret = " + ret.ToString() + ";\r\n desc =" + desc;
            }
            {

                textBox4.Enabled = false;
                checkBox5.Enabled = true;
                checkBox6.Enabled = false;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                checkBox4.Enabled = false;
                comboBox1.Enabled = false;
                textBox7.Enabled = true;
                textBox8.Enabled = true;
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = true;
                checkBox6.Checked = false;
                bt1_State = false;
                bt2_State = false;
                bt3_State = false;
                bt4_State = true;
                bt5_State = false;
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            Byte[] byte_content = System.Text.ASCIIEncoding.Default.GetBytes(textBox9.Text); 
            licJsonObj["pub"] = algorithm.ToHexString(byte_content);
            
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            Byte[]  byte_content = System.Text.ASCIIEncoding.Default.GetBytes(textBox10.Text);
            licJsonObj["raw"] = algorithm.ToHexString(byte_content);
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            Byte[] byte_content = System.Text.ASCIIEncoding.Default.GetBytes(textBox11.Text);
            licJsonObj["rom"] = algorithm.ToHexString(byte_content);
        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (bt5_State)
            {
                openAPI API = new openAPI();
                string retJson = algorithm.senseCloudRequest(API.ModifyProduct, JsonConvert.SerializeObject(licJsonObj), appid, secret);
                JObject jobj = JObject.Parse(retJson);
                int ret = Convert.ToInt32(jobj["code"].ToString());
                string desc = jobj["desc"].ToString();
                Console.WriteLine("修改产品 ret = {0}, desc = {1}", ret, desc);
                textBox8.Text = "修改产品 ret = " + ret.ToString() + "; desc =" + desc;
            }
            else
            {
 
                textBox4.Enabled = true;
                checkBox5.Enabled = true;
                checkBox6.Enabled = true;
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
                checkBox4.Enabled = true;
                comboBox1.Enabled = true;
                textBox7.Enabled = true;
                textBox8.Enabled = true;
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                bt1_State = false;
                bt2_State = false;
                bt3_State = false;
                bt4_State = false;
                bt5_State = true;
            }
        }
        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            textBox5.Enabled = checkBox1.Checked;
            textBox6.Enabled = checkBox1.Checked;

        }
        private void checkBox5_CheckStateChanged(object sender, EventArgs e)
        {
            textBox3.Enabled = checkBox5.Checked;
        }

        private void checkBox6_CheckStateChanged(object sender, EventArgs e)
        {
            textBox12.Enabled = checkBox6.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            textBox9.Enabled = checkBox2.Checked;
        }

        private void checkBox3_CheckStateChanged(object sender, EventArgs e)
        {
            textBox10.Enabled = checkBox3.Checked;
        }

        private void checkBox4_CheckStateChanged(object sender, EventArgs e)
        {
            textBox11.Enabled = checkBox4.Checked;
        }
    }
}
