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
    public partial class Form4 : Form
    {
        static string pin = "12345678";
        JObject fileObject = new JObject();
        JObject licData = new JObject();
        

        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Int64 start_time = (dateTimePicker1.Value.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            fileObject["op"] = "reset";
            fileObject["not_before"] = start_time;
            textBox1.Text = JsonConvert.SerializeObject(fileObject);
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            Int64 end_time = (dateTimePicker2.Value.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            fileObject["not_after"] = end_time;
            textBox1.Text = JsonConvert.SerializeObject(fileObject);
        }

        private void button1_Click(object sender, EventArgs e)
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
                textBox1.Text = "打开控制锁失败.0x" + ret.ToString("X8");
                goto Final;

            }
            byte[] psd = System.Text.ASCIIEncoding.Default.GetBytes(pin);
            //验证pin码
            ret = Libd2c.master_pin_verify(device_handle, 0, psd, (UInt32)psd.Length);
            if (ret != 0)
            {
                textBox1.Text = "校验PIN码失败0x{0:X8}0x" + ret.ToString("X8");
                goto Final;

            }
            UInt32 CERT_SIZE = 4028;
            byte[] root_ca_cert = new byte[CERT_SIZE];
            //获得ca证书
            ret = Libd2c.master_get_ca_cert_ex(device_handle, Libd2c.CA_TYPE.PKI_CA_TYPE_ROOT, 1, root_ca_cert, CERT_SIZE, ref CERT_SIZE);
            if (ret != SSErrCode.SS_OK)
            {
                textBox1.Text = "获取控制锁证书失败。0x" + ret.ToString("X8");
                goto Final;
            }
            //生成一个d2c句柄
            ret = Libd2c.d2c_file_new(device_handle, ref d2cHandlePtr, Libd2c.SIGN_TYPE.SIGN_TYPE_SEED, root_ca_cert, CERT_SIZE);
            if (ret != SSErrCode.SS_OK)
            {
                textBox1.Text = "向d2c中添加内容失败。0x" + ret.ToString("X8");
                goto Final;
            }
            //将Jason添加到d2c句柄
            string strLicJson = JsonConvert.SerializeObject(fileObject);
            ret = Libd2c.d2c_add_pkg(d2cHandlePtr, strLicJson, "reset dongle");
            if (ret != SenseShield.SSErrCode.SS_OK)
            {
                textBox1.Text = "向d2c中添加内容失败。0x" + ret.ToString("X8");
                goto Final;
            }
            ret = Libd2c.d2c_get(d2cHandlePtr, null, 0, ref d2cSize);
            if (ret != SenseShield.SSErrCode.SS_ERROR_INSUFFICIENT_BUFFER)
            {
                textBox1.Text = "缓冲区不足。0x" + ret.ToString("X8");
                goto Final;
            }
Final:
            byte[] d2c_buf = new byte[d2cSize];
            string maked_lic_str = string.Empty;
            ret = Libd2c.d2c_get(d2cHandlePtr, d2c_buf, d2cSize, ref d2cSize);
            if (ret == SenseShield.SSErrCode.SS_OK)
            {
                maked_lic_str = Encoding.Default.GetString(d2c_buf);
                SenseShield.Program.WriteD2CToFile(textBox2.Text, "清空锁", maked_lic_str);
                textBox1.Text = "写出重置加密锁d2c文件成功";

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Form2 frm2 = new Form2();
            frm2.Show();
        }
    }
}
