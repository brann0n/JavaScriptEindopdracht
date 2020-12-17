using JavaScriptUNO.UnoBackend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JavaScriptUNO.Controllers
{
	public class SessionController : Controller
	{
		// GET: Session
		public ActionResult Index()
		{
			return View();
		}

		[IpRestriction]
		public ActionResult Stats()
		{
			return View(MvcApplication.Manager.Sessions);
		}

		[IpRestriction]
		public ActionResult StatDetails(string id)
		{
			return View(MvcApplication.Manager.Sessions.FirstOrDefault(n => n.GameId == id));
		}

		[IpRestriction]
		public async Task KillSession(string id)
        {
			var session = MvcApplication.Manager.Sessions.FirstOrDefault(n => n.GameId == id);

			if(session != null)
            {
				await session.KickGameAndPlayers();
				MvcApplication.Manager.EndGame(session);
			}		
		}

		public ActionResult Rules()
		{
			return View();
		}
	}
}