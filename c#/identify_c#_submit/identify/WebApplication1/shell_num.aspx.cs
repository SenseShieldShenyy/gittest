using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;


namespace WebApplication1
{
    public partial class shell_num : System.Web.UI.Page
    {
        public string request_schema = "http://";
        public string checkpath = "chek_shell_num"; //验证请求地方
        public string guid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            guid = Guid.NewGuid().ToString();
        }
    }
}