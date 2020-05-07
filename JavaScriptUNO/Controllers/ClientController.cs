using JavaScriptUNO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JavaScriptUNO.Controllers
{
    public class ClientController : Controller
    {
        // GET: Client
        public ActionResult Index(string id)
        {
            ServerGameSession game = MvcApplication.Manager.FindSessionByClientId(id);

            if (game != null)
            {
				ClientGameSession session = new ClientGameSession();
				session.ClientId = id;
				session.GameId = game.GameId;
				session.GameName = game.GameName;
                return View(session);
            }
            else
            {
                return View("Error");
            }
        }
    }
}