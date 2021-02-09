using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace testApplication
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("Browser name and version " + Request.Browser.Type + "<br>");
            Response.Write("Browser name" + Request.Browser.Browser + "<br>");
            Response.Write("Browser platform" + Request.Browser.Platform + "<br>");
            Response.Write("Client IP address" + Request.UserHostAddress + "br");
            Response.Write("Current request URL " + Request.Url + "<br>");
            Response.Write("Current request vitual " + Request.Path + "<br>");
            Response.Write("Current PhysicalPath" + Request.PhysicalPath + "<br>");
        }
    }
}