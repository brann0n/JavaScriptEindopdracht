﻿using JavaScriptUNO.UnoBackend;
using System;
using System.Collections.Generic;
using System.Linq;
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

		public ActionResult Rules()
		{
			return View();
		}
	}
}