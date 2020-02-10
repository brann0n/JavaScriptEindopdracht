using JavaScriptUNO.Models;
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
        public ActionResult Index(string id)
        {
            ServerGameSession game = MvcApplication.Manager.FindSession(id);

            if(game != null)
            {
                return View(game);
            }
            else
            {
                return View("Error");
            }
        }
    }
}