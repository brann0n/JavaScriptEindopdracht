using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JavaScriptUNO.Controllers
{
    public class HostController : Controller
    {
        // GET: Host
        public string Index()
        {
            return "NO VALID SESSION PROVIDED";
        }

        public ActionResult Index(string id)
        {
            return View();
        }
    }
}