﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessageHub.Web.Controllers
{

	public class MessageController : Controller
	{
		[Authorize]
		public ActionResult Index()
		{
			return View();
		}
	}
}
