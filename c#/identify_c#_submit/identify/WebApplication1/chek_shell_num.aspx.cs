using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class chek_shell_num : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string guid = Request["guid"];
            if (guid == null || guid.Length == 0)
            {
                Response.Write("服务器无对应session");
                return;
            }

            Session.Remove(Request["guid"]);
            if (Request.Form["shell_num"] == "337500000021")
            {
                Response.Write("加密锁号验证成功");
                Response.RedirectPermanent("/success.aspx");
            }
            else
            {
                Response.Write("加密锁号验证失败");
                Response.RedirectPermanent("/failed.aspx");

            }
        }
    }
}