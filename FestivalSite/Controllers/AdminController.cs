using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FestivalLibAdmin.Model;

namespace FestivalSite.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Tickets()
        {
            return PartialView(TicketType.GetTypes());
        }

        public PartialViewResult Bands()
        {
            return PartialView(Band.GetBands());
        }
    }
}
