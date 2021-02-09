using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class get_pubdata : System.Web.UI.Page
    {
        public string request_schema = "http://";
        public string checkpath = "chek_pub_data"; //验证请求地方
        public string guid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            guid = Guid.NewGuid().ToString();
        }
    }
}