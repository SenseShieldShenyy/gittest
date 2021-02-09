using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using opeapiShow;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        string appid = "044b79df99e245f8ac547f892b197b59";
        string secret = "fdfee335c3b34bfe991dffb6016a4d31";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            developer dev = new developer(appid, secret);
            product pro = new product(dev);
            string desc = "";
        }
    }
}
