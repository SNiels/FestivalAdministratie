using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FestivalLibAdmin.Model;
using FestivalSite.Helpers;

namespace FestivalSite.Controllers
{
    public class HomeController : Controller
    {
        public static Random Random { get; set; }
        static HomeController()
        {
            Random = new Random();
        }
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult BandsSliderPartial()
        {
            var bands =Band.GetBands();
            return PartialView("_BandsSliderPartial",(bands as IList<Band>).Shuffle().Take(5));
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public PartialViewResult OptredenCardsPartial(IEnumerable<Optreden> optredens)
        {
            if (optredens == null) return null;
            return PartialView("_OptredenCardsPartial",optredens);
        }
    }
}
