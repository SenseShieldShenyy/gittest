using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace openAPI
{
    public partial class CreatProduct : Form
    {
        string appId = "";
        string psd = "";

        public CreatProduct()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            appId = textBox1.Text;

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            psd = textBox2.Text;
        }

        
    }
}
