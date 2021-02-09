using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace WebApplication1
{
    public class Global : System.Web.HttpApplication
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute(routeName: "Login", routeUrl: "login", physicalFile: "~/Login.aspx");
            routes.MapPageRoute(routeName: "WebForm1", routeUrl: "WebForm1", physicalFile: "~/WebForm1.aspx");
            routes.MapPageRoute(routeName: "success", routeUrl: "success", physicalFile: "~/success.aspx");
            routes.MapPageRoute(routeName: "failed", routeUrl: "failed", physicalFile: "~/failed.aspx");

            routes.MapPageRoute(routeName: "chek_shell_num", routeUrl: "chek_shell_num", physicalFile: "~/chek_shell_num.aspx");
            routes.MapPageRoute(routeName: "chek_pub_data", routeUrl: "chek_pub_data", physicalFile: "~/chek_pub_data.aspx");
            routes.RouteExistingFiles = true;
        }
        protected void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            RegisterRoutes(RouteTable.Routes);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码

        }

        void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码
            Console.Write("在出现未处理的错误时运行的代码");

        }

        void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码

        }

        void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
            // 或 SQLServer，则不会引发该事件。

        }
    }
}