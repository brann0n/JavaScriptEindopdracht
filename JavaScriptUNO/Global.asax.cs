using JavaScriptUNO.UnoBackend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JavaScriptUNO
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static SessionManager Manager;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Manager = new SessionManager();
        }
    }
}
