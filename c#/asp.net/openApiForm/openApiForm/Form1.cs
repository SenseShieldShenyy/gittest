using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace openApiForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string appid = textBox1.Text;
            string psd = textBox2.Text;
            developer Dev =new developer(appid, psd);
            product pro = new product(Dev);
            string desc = "";
            pro.LicenseId = Convert.ToUInt32(textBox4.Text);
            pro.ProductName = textBox3.Text;
            if (comboBox1.Text == "云锁")
            {
                pro.LicenseForm = 1;
            }
            pro.addModules(Convert.ToUInt32(textBox5.Text), textBox6.Text);
            int ret = pro.creatProduct(ref desc);
            textBox7.Text = "创建产品 ret = " + ret.ToString() + "desc =" + desc;
            Console.WriteLine(ret.ToString());

        }

        
    }
}
