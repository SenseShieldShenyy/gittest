using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class chek_pub_data : System.Web.UI.Page
    {
        public static string account = "";
        public static string pwd = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string guid = Request["guid"];
            string pub_data = "";
            byte[] pub_data_byte;
            byte[] new_pub_data_byte;
            string temp = "";
            
            if (guid == null || guid.Length == 0)
            {
                Response.Write("服务器无对应session");
                return;
            }
            Session.Remove(Request["guid"]);
            if (Request.Form["pub_data"] != null)
            {
                pub_data = Request.Form["pub_data"];
                pub_data_byte = Convert.FromBase64String(Request.Form["pub_data"]);
                pub_data = BitConverter.ToString(pub_data_byte);
                new_pub_data_byte = new byte[2 * pub_data_byte.Length];
                for (int i = 0; i < pub_data_byte.Length; i++)
                {
                    new_pub_data_byte[2 * i] = pub_data_byte[i];
                    new_pub_data_byte[2 * i + 1] = 0;
                }
                for (int i = 0; i < new_pub_data_byte.Length; i += 2)
                {
                    temp += BitConverter.ToChar(new_pub_data_byte, i);
                    if (i < 37)
                    {
                        account += BitConverter.ToChar(new_pub_data_byte, i);
                    }
                    else
                    {
                        pwd += BitConverter.ToChar(new_pub_data_byte, i);
                    }
                }

            }
            TextBox1.Text = account;
            //TextBox2.Visible = false;
            TextBox2.Text = pwd;

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if(pwd == "1234567890" && account == "shenyy@sense.com.cn")
            {
                Response.Write("加密锁号验证成功");
                Response.RedirectPermanent("/success.aspx");
            }
            
        }
    }
}