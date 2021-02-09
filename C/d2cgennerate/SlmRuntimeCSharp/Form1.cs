using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using D2cDemo;
using System.Threading.Tasks;
using System.IO;
using cs_libd2c_demo;
using SenseShield;
using SLM_HANDLE_INDEX = System.UInt32;

namespace SlmRuntimeCSharp
{
    public partial class Form1 : Form
    {
        static string pin = "12345678";
        public const int DEVELOPER_ID_LENGTH = 8;
        public const int DEVICE_SN_LENGTH = 16;
        //许可选项定义
        UInt32 ID = 0;
        bool ch1_sta = false;
        bool ch2_sta = false;
        bool ch3_sta = false;
        bool ch4_sta = false;
        bool ch5_sta = false;
        string lock_sn = "";
        string lic_op = "";
        JObject licJsonObj = new JObject();
        JObject licData_raw = new JObject();
        JObject licData_rom = new JObject();
        JObject licData_pub = new JObject();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //许可id
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (0 != string.Compare("", textBox1.Text ))
            {
                ID = Convert.ToUInt32(textBox1.Text);
                licJsonObj["license_id"] = ID;
                textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
            }
        }
        //设置开始时间
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Int64 start_time = (dateTimePicker1.Value.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            licJsonObj["start_time"] = "=" + String.Format("{0:D}", start_time);
            //licJsonObj["start_time"] = start_time;
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }
        //设置结束时间
        private void dateTimePicker2_ValueChanged_1(object sender, EventArgs e)
        {
            Int64 end_time = (dateTimePicker2.Value.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            licJsonObj["end_time"] = "=" + String.Format("{0:D}", end_time);      //Unix时间戳
            //licJsonObj["end_time"] = end_time;
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }
        //账户类型
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (0 == string.Compare("单机锁许可", comboBox1.Text))
            {
                licJsonObj["concurrent_type"] = "process";           //并发类型
                licJsonObj["concurrent"] = "=0";                      //并发数
                textBox7.Enabled = false;
                comboBox3.Enabled = false;
            }
            else if (0 == string.Compare("网络锁许可", comboBox1.Text))
            {
                comboBox3.Enabled = true;
                textBox7.Enabled = true;
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);

        }
        //并发类型
        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            if (0 == string.Compare("进程", comboBox3.Text))
            {
                licJsonObj["concurrent_type"] = "process";   //并发类型
            }
            else if (0 == string.Compare("会话", comboBox3.Text))
            {
                licJsonObj["concurrent_type"] = "win_user_session";   //并发类型
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        //并发数
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            licJsonObj["concurrent"] = "=" + textBox7.Text;           //并发数
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }
        //锁号
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            lock_sn = textBox2.Text;
        }

        //许可操作
        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            licJsonObj = new JObject();
            if (0 == string.Compare("安装许可", comboBox2.Text))//暂不支持
                lic_op = "installlic";
            else if (0 == string.Compare("添加许可", comboBox2.Text))
            {
                
                lic_op = "addlic";
                comboBox2.Enabled = true;                        //许可操作
                textBox2.Enabled = true;                        //锁号
                textBox1.Enabled = true;                        //ID
                dateTimePicker1.Enabled = checkBox6.Checked;                 //开始时间
                dateTimePicker2.Enabled = checkBox7.Checked;                 //结束时间
                checkBox6.Enabled = true;
                checkBox7.Enabled = true;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox4.Enabled = true;                       //是否永久
                comboBox1.Enabled = true;                       //账号类型
                comboBox3.Enabled = false;                      //并发类型
                textBox7.Enabled = false;                        //并发数
                checkBox5.Enabled = true;                       //是否强制
                textBox11.Enabled = true;                       //许可版本
                textBox12.Enabled = checkBox9.Checked;                       //使用次数
                textBox13.Enabled = checkBox8.Checked;                       //时间跨度
                checkBox9.Enabled = true;
                checkBox8.Enabled = true;
                checkBox9.Checked = false;
                checkBox8.Checked = false;
                textBox14.Enabled = checkBox10.Checked;                       //模块
                checkBox10.Checked = false;
                checkBox10.Enabled = true;


                textBox1.Text = "";
                dateTimePicker1.Text = "";
                dateTimePicker2.Text = "";
                comboBox1.Text = "";
                comboBox2.Text = "";
                textBox7.Text = "";
                textBox11.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                textBox14.Text = "";

                checkBox1.Enabled = true;                       //公开区
                checkBox2.Enabled = true;                       //读写区
                checkBox3.Enabled = true;                       //只读区
                textBox8.Enabled = false;                       //偏移量
                textBox9.Enabled = false;
                textBox10.Enabled = false;
                textBox3.Enabled = false;                       //内容
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                checkBox1.Checked = false;                      //勾选初始化
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
            }
            else if (0 == string.Compare("激活许可", comboBox2.Text))//暂不支持
                lic_op = "activelic";
            else if (0 == string.Compare("更新许可", comboBox2.Text))
            {
                lic_op = "updatelic";
                comboBox2.Enabled = true;                        //许可操作
                textBox2.Enabled = true;                        //锁号
                textBox1.Enabled = true;                        //ID
                dateTimePicker1.Enabled = checkBox6.Checked;                 //开始时间
                dateTimePicker2.Enabled = checkBox7.Checked;                 //结束时间
                checkBox6.Enabled = true;
                checkBox7.Enabled = true;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox4.Enabled = false;                       //是否永久
                comboBox1.Enabled = false;                       //账号类型
                comboBox3.Enabled = false;                      //并发类型
                textBox7.Enabled = false;                        //并发数
                checkBox5.Enabled = false;                      //是否强制
                textBox11.Enabled = true;                       //许可版本
                textBox12.Enabled = checkBox9.Checked;                       //使用次数
                textBox13.Enabled = checkBox8.Checked;                       //时间跨度
                checkBox9.Enabled = true;
                checkBox8.Enabled = true;
                checkBox9.Checked = false;
                checkBox8.Checked = false;
                textBox14.Enabled = checkBox10.Checked;                       //模块
                checkBox10.Checked = false;
                checkBox10.Enabled = true;


                textBox1.Text = "";
                dateTimePicker1.Text = "";
                dateTimePicker2.Text = "";
                comboBox1.Text = "";
                comboBox2.Text = "";
                textBox7.Text = "";
                textBox11.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                textBox14.Text = "";


                checkBox1.Enabled = true;                       //公开区
                checkBox2.Enabled = true;                       //读写区
                checkBox3.Enabled = true;                       //只读区
                textBox8.Enabled = false;                       //偏移量
                textBox9.Enabled = false;
                textBox10.Enabled = false;
                textBox8.Text = "";
                textBox9.Text = "";
                textBox10.Text = "";
                textBox3.Enabled = false;                       //内容
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                checkBox1.Checked = false;                      //勾选初始化
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
            }
            else if (0 == string.Compare("删除许可", comboBox2.Text))
            {
                //界面上从左到右重上到下
                lic_op = "dellic";
                comboBox2.Enabled = true;                        //许可操作
                textBox2.Enabled = true;                        //锁号
                textBox1.Enabled = true;                        //ID
                dateTimePicker1.Enabled = false;                 //开始时间
                dateTimePicker2.Enabled = false;                 //结束时间
                checkBox6.Enabled = false;
                checkBox7.Enabled = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox4.Enabled = false;                       //是否永久
                comboBox1.Enabled = false;                       //账号类型
                comboBox2.Enabled = true;                        //许可操作
                textBox7.Enabled = false;                        //并发数
                checkBox5.Enabled = false;                       //是否强制
                textBox11.Enabled = false;                       //许可版本
                textBox12.Enabled = false;                       //使用次数
                textBox13.Enabled = false;                       //时间跨度
                checkBox9.Enabled = false;
                checkBox8.Enabled = false;
                checkBox9.Checked = false;
                checkBox8.Checked = false;
                textBox14.Enabled = false;                       //模块
                checkBox10.Checked = false;
                checkBox10.Enabled = false;


                textBox1.Text = "";
                dateTimePicker1.Text = "";
                dateTimePicker2.Text = "";
                comboBox1.Text = "";
                comboBox2.Text = "";
                textBox7.Text = "";
                textBox11.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                textBox14.Text = "";

                checkBox1.Enabled = false;                       //公开区
                checkBox2.Enabled = false;                       //读写区
                checkBox3.Enabled = false;                       //只读区
                textBox8.Enabled = false;                        //偏移量
                textBox9.Enabled = false;
                textBox10.Enabled = false;
                textBox8.Text = "";
                textBox9.Text = "";
                textBox10.Text = "";
                textBox3.Enabled = false;                        //内容
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                checkBox1.Checked = false;                      //勾选初始化
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
            }
            else if (0 == string.Compare("锁定所有许可", comboBox2.Text))
            {
                lic_op = "lockalllic";
                comboBox2.Enabled = true;                        //许可操作
                textBox2.Enabled = true;                        //锁号
                textBox1.Enabled = false;                        //ID
                dateTimePicker1.Enabled = false;                 //开始时间
                dateTimePicker2.Enabled = false;                 //结束时间
                checkBox6.Enabled = false;
                checkBox7.Enabled = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox4.Enabled = false;                       //是否永久
                comboBox1.Enabled = false;                       //账号类型
                comboBox3.Enabled = false;                        //并发类型
                textBox7.Enabled = false;                        //并发数
                checkBox5.Enabled = false;                       //是否强制
                textBox11.Enabled = false;                       //许可版本
                textBox12.Enabled = false;                       //使用次数
                textBox13.Enabled = false;                       //时间跨度
                checkBox9.Enabled = false;
                checkBox8.Enabled = false;
                checkBox9.Checked = false;
                checkBox8.Checked = false;
                textBox14.Enabled = false;                       //模块
                checkBox10.Checked = false;
                checkBox10.Enabled = false;



                textBox1.Text = "";
                dateTimePicker1.Text = "";
                dateTimePicker2.Text = "";
                comboBox1.Text = "";
                comboBox2.Text = "";
                textBox7.Text = "";
                textBox11.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                textBox14.Text = "";

                checkBox1.Enabled = false;                       //公开区
                checkBox2.Enabled = false;                       //读写区
                checkBox3.Enabled = false;                       //只读区
                textBox8.Enabled = false;                        //偏移量
                textBox9.Enabled = false;
                textBox10.Enabled = false;
                textBox8.Text = "";
                textBox9.Text = "";
                textBox10.Text = "";
                textBox3.Enabled = false;                        //内容
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                checkBox1.Checked = false;                      //勾选初始化
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
            }
            else if (0 == string.Compare("解锁所有许可", comboBox2.Text))
            {
                lic_op = "unlockalllic";
                comboBox2.Enabled = true;                        //许可操作
                textBox2.Enabled = true;                        //锁号
                textBox1.Enabled = false;                        //ID
                dateTimePicker1.Enabled = false;                 //开始时间
                dateTimePicker2.Enabled = false;                 //结束时间
                checkBox6.Enabled = false;
                checkBox7.Enabled = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox4.Enabled = false;                       //是否永久
                comboBox1.Enabled = false;                       //账号类型


                textBox7.Enabled = false;                        //并发数
                checkBox5.Enabled = false;                       //是否强制
                textBox11.Enabled = false;                       //许可版本
                textBox12.Enabled = false;                       //使用次数
                textBox13.Enabled = false;                       //时间跨度
                checkBox9.Enabled = false;
                checkBox8.Enabled = false;
                checkBox9.Checked = false;
                checkBox8.Checked = false;
                textBox14.Enabled = false;                       //模块
                checkBox10.Checked = false;
                checkBox10.Enabled = false;



                textBox1.Text = "";
                dateTimePicker1.Text = "";
                dateTimePicker2.Text = "";
                comboBox1.Text = "";
                comboBox2.Text = "";
                textBox7.Text = "";
                textBox11.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                textBox14.Text = "";

                checkBox1.Enabled = false;                       //公开区
                checkBox2.Enabled = false;                       //读写区
                checkBox3.Enabled = false;                       //只读区
                textBox8.Enabled = false;                        //偏移量
                textBox9.Enabled = false;
                textBox10.Enabled = false;
                textBox8.Text = "";
                textBox9.Text = "";
                textBox10.Text = "";
                textBox3.Enabled = false;                        //内容
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                checkBox1.Checked = false;                      //勾选初始化
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
            }
            else if (0 == string.Compare("删除所有许可", comboBox2.Text))
            {
                lic_op = "delalllic";
                comboBox2.Enabled = true;                        //许可操作
                comboBox2.Enabled = true;                        //许可操作
                textBox2.Enabled = true;                        //锁号
                textBox1.Enabled = false;                        //ID
                dateTimePicker1.Enabled = false;                 //开始时间
                dateTimePicker2.Enabled = false;                 //结束时间
                checkBox6.Enabled = false;
                checkBox7.Enabled = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox4.Enabled = false;                       //是否永久
                comboBox1.Enabled = false;                       //账号类型

                comboBox3.Enabled = false;                        //并发类型
                textBox7.Enabled = false;                        //并发数
                checkBox5.Enabled = false;                       //是否强制
                textBox11.Enabled = false;                       //许可版本
                textBox12.Enabled = false;                       //使用次数
                textBox13.Enabled = false;                       //时间跨度
                checkBox9.Enabled = false;
                checkBox8.Enabled = false;
                checkBox9.Checked = false;
                checkBox8.Checked = false;
                textBox14.Enabled = false;                       //模块
                checkBox10.Checked = false;
                checkBox10.Enabled = false;




                textBox1.Text = "";
                dateTimePicker1.Text = "";
                dateTimePicker2.Text = "";
                comboBox1.Text = "";
                comboBox2.Text = "";
                textBox7.Text = "";
                textBox11.Text = "";
                textBox12.Text = "";
                textBox13.Text = "";
                textBox14.Text = "";

                checkBox1.Enabled = false;                       //公开区
                checkBox2.Enabled = false;                       //读写区
                checkBox3.Enabled = false;                       //只读区
                textBox8.Enabled = false;                        //偏移量
                textBox9.Enabled = false;
                textBox10.Enabled = false;
                textBox8.Text = "";
                textBox9.Text = "";
                textBox10.Text = "";
                textBox3.Enabled = false;                        //内容
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                checkBox1.Checked = false;                      //勾选初始化
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
            }

            licJsonObj["op"] = lic_op;
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = !ch1_sta;
            textBox8.Enabled = !ch1_sta;
            textBox3.Enabled = !ch1_sta;
            ch1_sta = checkBox1.Checked;
            if (checkBox1.Checked)
            {
                licJsonObj["pub"] = licData_pub;
            }
            else
            {
                licJsonObj.Remove("pub");
                licData_pub = new JObject();
                textBox8.Text = "";
                textBox3.Text = "";
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            String a = null;
            Byte[] byte_content;
           //Byte[] len_bytes;
            Byte[] need_write;
            //int len = 0;
            if (checkBox1.Checked)
            {
                byte_content = System.Text.ASCIIEncoding.Default.GetBytes(textBox3.Text);
                //len = textBox3.Text.Length;
                //len_bytes = BitConverter.GetBytes(len);
                need_write = new byte[byte_content.Length];
                //len_bytes.CopyTo(need_write, 0);
                byte_content.CopyTo(need_write, 0);
                for (int i = 0; i < need_write.Length; i++)
                {
                    string b_str = "";
                    if (need_write[i] <= 15)
                        b_str += "0";
                    b_str += Convert.ToString(need_write[i], 16);
                    a = a + b_str;
                }
                licData_pub["data"] = a;
                licData_pub["resize"] = need_write.Length;  //数据区长度需要设计好， 应该设定为可能存储的最大字符串长度*2
                textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
            }

        }
        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (0 != string.Compare("", textBox8.Text))
            {
                licData_pub["offset"] = Convert.ToUInt32(textBox8.Text);
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void checkBox2_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = !ch2_sta;
            textBox9.Enabled = !ch2_sta;
            textBox4.Enabled = !ch2_sta;
            ch2_sta = checkBox2.Checked;
            if (checkBox2.Checked)
            {
                licJsonObj["raw"] = licData_raw;
            }
            else
            {
                licJsonObj.Remove("raw");
                licData_raw = new JObject();
                textBox9.Text = "";
                textBox4.Text = "";
            }
            
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            String a = null;
            Byte[] byte_content;
            //Byte[] len_bytes;
            Byte[] need_write;
            //int len = 0;
            if (checkBox2.Checked)
            {
                byte_content = System.Text.ASCIIEncoding.Default.GetBytes(textBox4.Text);
                //len = textBox4.Text.Length;
               // len_bytes = BitConverter.GetBytes(len);
                need_write = new byte[byte_content.Length];
                //len_bytes.CopyTo(need_write, 0);
                byte_content.CopyTo(need_write, 0);
                for (int i = 0; i < need_write.Length; i++)
                {
                    string b_str = "";
                    if (need_write[i] <= 15)
                        b_str += "0";
                    b_str += Convert.ToString(need_write[i], 16);
                    a = a + b_str;
                }
                licData_raw["data"] = a;
                licData_raw["resize"] = need_write.Length;  //数据区长度需要设计好， 应该设定为可能存储的最大字符串长度*2
                licJsonObj["raw"] = licData_raw;
                textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
            }
        }
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (0 != string.Compare("", textBox9.Text))
            {
                licData_raw["offset"] = Convert.ToUInt32(textBox9.Text);
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }
        private void checkBox3_Click(object sender, EventArgs e)
        {
            checkBox3.Checked = !ch3_sta;
            textBox10.Enabled = !ch3_sta;
            textBox5.Enabled = !ch3_sta;
            ch3_sta = checkBox3.Checked;
            if (checkBox3.Checked)
            {
                licJsonObj["rom"] = licData_rom;
            }
            else
            {
                licJsonObj.Remove("rom");
                licData_rom = new JObject();
                textBox10.Text = "";
                textBox5.Text = "";
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            String a = null;
            Byte[] byte_content;
            //Byte[] len_bytes;
            Byte[] need_write;
           // int len = 0;
            if (checkBox3.Checked)
            {
                byte_content = System.Text.ASCIIEncoding.Default.GetBytes(textBox5.Text);
                //len = textBox4.Text.Length;
                //len_bytes = BitConverter.GetBytes(len);
               // need_write = new byte[byte_content.Length + sizeof(int)];
                need_write = new byte[byte_content.Length];
                //len_bytes.CopyTo(need_write, 0);
                //byte_content.CopyTo(need_write, len_bytes.Length);
                byte_content.CopyTo(need_write, 0);
                for (int i = 0; i < need_write.Length; i++)
                {
                    string b_str = "";
                    if (need_write[i] <= 15)
                        b_str += "0";
                    b_str += Convert.ToString(need_write[i], 16);
                    a = a + b_str;
                }
                licData_rom["data"] = a;
                //licData_rom["offset"] = Convert.ToUInt32(textBox10.Text);
                licData_rom["resize"] = need_write.Length;  //数据区长度需要设计好， 应该设定为可能存储的最大字符串长度*2
                licJsonObj["rom"] = licData_rom;
                textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
            }

        }
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (0 != string.Compare("", textBox10.Text))
            {
                licData_rom["offset"] = Convert.ToUInt32(textBox10.Text);
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UInt32 ret = 0;
            UInt32 d2cSize = 0;
            IntPtr device_handle = IntPtr.Zero;
            IntPtr d2cHandlePtr = IntPtr.Zero;
            string license_str = "";
            byte[] cert3 = SenseShield.Program.get_lockp7b_from_db(textBox2.Text);//get_lockp7b_from_db("9733c801000702079da70011000b0013");
            //获得license_str*********************************************************************************************

            //打开控制锁
            ret = Libd2c.master_open(ref device_handle);
            if (ret != 0)
            {
                textBox6.Text = "打开控制锁失败.0x" + ret.ToString("X8") + "请检查控制锁是否插入或者被占用";
                goto Final;
               
            }
            byte[] psd = System.Text.ASCIIEncoding.Default.GetBytes(pin);
            ret = Libd2c.master_pin_verify(device_handle, 0, psd, (UInt32)psd.Length);
            if (ret != 0)
            {
                textBox6.Text = "校验PIN码失败0x{0:X8}0x" + ret.ToString("X8");
                goto Final;
                
            }
            //指定锁号签发许可
            byte[] accountIdBytes = SenseShield.Libd2cCommon.HexStrToBytes(textBox2.Text);//HexStrToBytes(textBox2.Text);
            UInt32 accountIdSize = (UInt32)(textBox2.Text.Length / 2);
            //生成d2cHandlePtr
            ret = Libd2c.d2c_lic_new(device_handle, ref d2cHandlePtr, Libd2c.ACCOUNT_TYPE.NONE, accountIdBytes, accountIdSize,
               cert3, (uint)cert3.Length);
            if (ret != 0)
            {
                textBox6.Text = "创建d2c失败。0x" + ret.ToString("X8");
                goto Final;
                
            }
            string strLicJson = JsonConvert.SerializeObject(licJsonObj);
            string strLicDesc = strLicJson;
            ret = Libd2c.d2c_add_lic(d2cHandlePtr, strLicJson, strLicDesc, null);
            if (ret != SSErrCode.SS_OK)
            {
                textBox6.Text = "向d2c中添加内容失败。0x" + ret.ToString("X8") + "请检查许可操作、锁号等参数是否正确";
                goto Final;
            }
            ret = Libd2c.d2c_get(d2cHandlePtr, null, 0, ref d2cSize);
            if (ret != 4)    //SS_ERROR_INSUFFICIENT_BUFFER = 4 缓冲区不足
            {
                textBox6.Text = "缓冲区不足。0x" + ret.ToString("X8");
                goto Final;
            }
        Final:
            byte []d2c_buf = new byte[d2cSize];
            ret = Libd2c.d2c_get(d2cHandlePtr, d2c_buf, d2cSize, ref d2cSize);
            if (0 == ret)
            {
                license_str = Encoding.Default.GetString(d2c_buf);
                //写出文件*****************************************************************************************************
                SenseShield.Program.WriteD2CToFile(comboBox2.Text, textBox1.Text, license_str);
                textBox6.Text = "写出文件成功。";
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Form2 frm2 = new Form2();
            frm2.Show();
            //SenseShield.Program.frm2.Show();
        }

        private void checkBox4_Click(object sender, EventArgs e)
        {
            checkBox4.Checked = !ch4_sta;
            dateTimePicker2.Enabled = ch4_sta;
            ch4_sta = checkBox4.Checked;
            //licJsonObj.Remove("end_time");
            if (checkBox4.Checked)
            {
                licJsonObj.Remove("start_time");
                licJsonObj.Remove("end_time");
                licJsonObj.Remove("span");
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                textBox13.Enabled = false;
                checkBox6.Enabled = false;
                checkBox7.Enabled = false;
                checkBox8.Enabled = false;
                checkBox9.Enabled = false;
            }
            else
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
                textBox13.Enabled = true;
                checkBox6.Enabled = true;
                checkBox7.Enabled = true;
                checkBox8.Enabled = true;
                checkBox9.Enabled = true;
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            checkBox5.Checked = !ch5_sta;
            ch5_sta = checkBox5.Checked;
            if (checkBox5.Checked)
            {
                licJsonObj["force"] = true;
            }
            else
            {
                licJsonObj["force"] = false;
                licJsonObj.Remove("force");
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (0 == string.Compare("", textBox11.Text))
            {
                licJsonObj["version"] = "=" + "0";
                licJsonObj.Remove("version");
            }
            else
            {
                //licJsonObj["version"] = "=" + textBox11.Text;
                licJsonObj["version"] = "=" + String.Format("{0:D}", textBox11.Text);
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (0 == string.Compare("", textBox12.Text))
            {
                licJsonObj["counter"] = "=" + "0";
                licJsonObj.Remove("counter");
            }
            else
            {
                //licJsonObj["version"] = "=" + textBox11.Text;
                licJsonObj["counter"] = "=" + String.Format("{0:D}", textBox12.Text);
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if (0 == string.Compare("", textBox13.Text))
            {
                licJsonObj["span"] = "=" + "0";
                licJsonObj.Remove("span");
            }
            else
            {   UInt32 time = 0;
                time  = 60 * 60 * 24 * Convert.ToUInt32(textBox13.Text);
                
                licJsonObj["span"] =  "=" + Convert.ToString(time);
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            string[] mod_arry = textBox14.Text.Split(' ');
            UInt32[] mod = new UInt32[mod_arry.Length];
            JArray md_arry = new JArray();
            int i;
            if (0 != string.Compare("", textBox14.Text))
            {
                for (i = 0; i < mod_arry.Length; i++)
                {
                    if (0 != string.Compare("", mod_arry[i]))
                    {
                        mod[i] = Convert.ToUInt32(mod_arry[i]);
                        md_arry.Add(mod[i]);
                    }

                }
                licJsonObj["module"] = md_arry;
            }
            else
            {
                licJsonObj.Remove("module");
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void checkBox6_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                dateTimePicker1.Enabled = true;
            }
            else
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker1.Text = "";
                licJsonObj.Remove("start_time");
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                dateTimePicker2.Enabled = true;
            }
            else
            {
                dateTimePicker2.Enabled = false;
                dateTimePicker2.Text = "";
                licJsonObj.Remove("end_time");
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked)
            {
                textBox12.Enabled = true;
            }
            else
            {
                textBox12.Enabled = false;
                textBox12.Text = "";
                licJsonObj.Remove("counter");

            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                textBox13.Enabled = true;
            }
            else 
            {
                textBox13.Enabled = false;
                textBox13.Text = "";
                licJsonObj.Remove("span");
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void checkBox10_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
            {
                textBox14.Enabled = true;
            }
            else
            {
                textBox14.Enabled = false;
                textBox14.Text = "";
                licJsonObj.Remove("module");
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }

        private void textBox14_MouseLeave(object sender, EventArgs e)
        {
            string[] mod_arry = textBox14.Text.Split(' ');
            UInt32[] mod = new UInt32[mod_arry.Length];
            JArray md_arry = new JArray();
            int i;
            if (0 != string.Compare("", textBox14.Text))
            {
                for (i = 0; i < mod_arry.Length; i++)
                {
                    if (0 != string.Compare("", mod_arry[i]))
                    {
                        mod[i] = Convert.ToUInt32(mod_arry[i]);
                        md_arry.Add(mod[i]);
                    }

                }
                licJsonObj["module"] = md_arry;
            }
            else
            {
                licJsonObj.Remove("module");
            }
            textBox6.Text = JsonConvert.SerializeObject(licJsonObj);
        }
    }
}
