using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FestivalLibAdmin.Model;
using FestivalSite.Helpers;
using QDFeedParser;

namespace FestivalSite.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            Festival.SingleFestival = Festival.GetFestival();
            return View();
        }

        public PartialViewResult BandsSliderPartial()
        {
            var bands =Band.GetBands();
            return PartialView("_BandsSliderPartial",bands.OrderByDescending(band=>band.Popularity).Take(5));
        }

        public PartialViewResult RssFeed()
        {
            IFeedFactory factory = new FileSystemFeedFactory();
            IFeed feed = factory.CreateFeed(new Uri(Server.MapPath(@"~/Content/rss.xml")));
            return PartialView("_RssFeedPartial", feed);
        }

        public PartialViewResult OptredenCardsPartial(IEnumerable<Optreden> optredens)
        {
            if (optredens == null) return null;
            return PartialView("_OptredenCardsPartial",optredens);
        }
    }
}
