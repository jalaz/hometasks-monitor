using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace HometasksMonitoringPanel
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapMvcAttributeRoutes();
        }
    }
}