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
    public partial class Form3 : Form
    {
        static string pin = "12345678";
        JObject fileObject = new JObject();
        JObject licData = new JObject();
        string filepath = string.Empty;
        string str_buff = null;


        public Form3()
        {
            InitializeComponent();
        }

        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {

                    }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            //openFileDialog1.Filter = "txt files(*evx)|(*evd)|(*key)|All files(*,*)";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                byte[] file_buffer = new byte[65536];
                filepath = openFileDialog1.FileName;
                file_buffer = File.ReadAllBytes(openFileDialog1.FileName);
                
                foreach (byte b in file_buffer)
                {
                    if (b <= 15)
                        str_buff += "0";
                    str_buff += Convert.ToString(b, 16);
                }
                fileObject["filebuffer"] = str_buff;
                fileObject["filename"] = openFileDialog1.SafeFileName;
                textBox4.Text = openFileDialog1.SafeFileName;
                textBox3.Text = JsonConvert.SerializeObject(fileObject);
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            //string lic_op = string.Empty;
            if (0 == string.Compare("添加文件", comboBox1.Text))
            {
                fileObject["op"] = "addfile";
                comboBox2.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button1.Enabled = true;
            }
            else if (0 == string.Compare("更新文件", comboBox1.Text))
            {
                fileObject["op"] = "updatefile";
                comboBox2.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button1.Enabled = true;
            }
            else if (0 == string.Compare("删除文件", comboBox1.Text))
            {
                fileObject["op"] = "delfile";
                comboBox2.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                button1.Enabled = false;
            }
            textBox3.Text = JsonConvert.SerializeObject(fileObject);
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            if (0 == string.Compare("evx", comboBox2.Text))
                fileObject["filetype"] = "evx";
            else if (0 == string.Compare("evd", comboBox2.Text))
                fileObject["filetype"] = "evd";
            else if (0 == string.Compare("key", comboBox2.Text))
                fileObject["filetype"] = "key";
            textBox3.Text = JsonConvert.SerializeObject(fileObject);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (0 != string.Compare("", textBox1.Text))
            {
                fileObject["offset"] = Convert.ToInt32(textBox1.Text);
                textBox3.Text = JsonConvert.SerializeObject(fileObject);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            UInt32 ret = 0;
            UInt32 d2cSize = 0;
            IntPtr device_handle = IntPtr.Zero;
            IntPtr d2cHandlePtr = IntPtr.Zero;
            byte[] cert3 = SenseShield.Program.get_lockp7b_from_db(textBox2.Text);//get_lockp7b_from_db("9733c801000702079da70011000b0013");
            //获得license_str*********************************************************************************************

            //打开控制锁
            ret = Libd2c.master_open(ref device_handle);
            if (ret != 0)
            {
                textBox3.Text = "打开控制锁失败.0x" + ret.ToString("X8");
                goto Final;

            }
            byte[] psd = System.Text.ASCIIEncoding.Default.GetBytes(pin);
            ret = Libd2c.master_pin_verify(device_handle, 0, psd, (UInt32)psd.Length);
            if (ret != 0)
            {
                Console.WriteLine("校验PIN码失败。0x{0:08X}", ret);

            }
            UInt32 CERT_SIZE = 4028;
            byte[] root_ca_cert = new byte[CERT_SIZE];
            ret = Libd2c.master_get_ca_cert_ex(device_handle, Libd2c.CA_TYPE.PKI_CA_TYPE_ROOT, 1, root_ca_cert, CERT_SIZE, ref CERT_SIZE);
            if (ret != SSErrCode.SS_OK)
            {
                textBox3.Text = "校验PIN码失败0x{0:X8}0x" + ret.ToString("X8");
                goto Final;
            }
            ret = Libd2c.d2c_file_new(device_handle, ref d2cHandlePtr, Libd2c.SIGN_TYPE.SIGN_TYPE_SEED, root_ca_cert, CERT_SIZE);

            string strLicJson = JsonConvert.SerializeObject(fileObject);
            ret = Libd2c.d2c_add_pkg(d2cHandlePtr, strLicJson, "seed_file_hello_sample");
            Console.WriteLine(strLicJson);
            if (ret != SenseShield.SSErrCode.SS_OK)
            {
                textBox3.Text = "创建d2c失败。0x" + ret.ToString("X8");
                goto Final;
            }
            ret = Libd2c.d2c_get(d2cHandlePtr, null, 0, ref d2cSize);
            if (ret != SenseShield.SSErrCode.SS_ERROR_INSUFFICIENT_BUFFER)
            {
                textBox3.Text = "缓冲区不足。0x" + ret.ToString("X8");
                goto Final;
            }
Final:
            byte[] d2c_buf = new byte[d2cSize];
            string maked_lic_str = string.Empty;
            ret = Libd2c.d2c_get(d2cHandlePtr, d2c_buf, d2cSize, ref d2cSize);
            if (ret == SenseShield.SSErrCode.SS_OK)
            {
                maked_lic_str = Encoding.Default.GetString(d2c_buf);
                SenseShield.Program.WriteD2CToFile("种子码签发", comboBox1.Text, maked_lic_str);
                //WriteD2CToFile("9733c801000702079da70011000b0013", textBox1.Text, license_str);
                //Console.WriteLine("写出文件成功");
                textBox3.Text = "写出d2c文件成功";
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Form2 frm2 = new Form2();
            frm2.Show();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            fileObject["filename"] = textBox4.Text;
            textBox3.Text = JsonConvert.SerializeObject(fileObject);

        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            //这边是要返回一个jason结构
            string[] lic_arry = textBox2.Text.Split(' ');
            UInt32[] lic = new UInt32[lic_arry.Length];
            JArray li_arry = new JArray();
            int i;
            if (0 != string.Compare("", textBox2.Text))
            {
                for (i = 0; i < lic_arry.Length; i++)
                {
                    if (0 != string.Compare("", lic_arry[i]))
                    {
                        lic[i] = Convert.ToUInt32(lic_arry[i]);
                        li_arry.Add(lic[i]);
                    }

                }
                fileObject["bind_lic"] = li_arry;
            }
            else
            {
                fileObject.Remove("bind_lic");
            }
            textBox3.Text = JsonConvert.SerializeObject(fileObject);
        }

    }
}
