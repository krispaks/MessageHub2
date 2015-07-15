using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MessageHub.Web.Controllers
{
    public class ChatController : Controller
    {
        [Authorize]
        public ActionResult Chat()
        {
            return View();
        }
    }
}