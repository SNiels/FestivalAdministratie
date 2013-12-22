using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FestivalLibAdmin.Model;
using MvcSiteMapProvider.Web.Mvc.Filters;

namespace FestivalSite.Controllers
{
    public class BandController : Controller
    {
        //
        // GET: /Bands/

        public ActionResult Index()
        {
            Festival.SingleFestival = Festival.GetFestival();
            return View(Festival.SingleFestival.Bands);            
        }

        //
        // GET: /Bands/Details/5
        public ActionResult Details(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return RedirectToAction("Index");
            if (name == "Genres") return RedirectToAction("Index", "Genre", null);
            try
            {
                Band band = Band.GetByName(name);
                if(band==null) return RedirectToAction("Index");
                band.Visit();
                return View(band);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Genres()
        {
            return RedirectToAction("Index", "Genre", null);
        }

        public PartialViewResult OptredensPartial(string bandID)
        {
                var optredens =Festival.SingleFestival.Optredens.Where(optreden => optreden.BandID == bandID);
                if (optredens == null || optredens.Count() < 1)
                    return null;
            
            return PartialView("_OptredensPartial",optredens);
        }

        public PartialViewResult GenresPartial(string bandID)
        {
            var genres = Genre.GetGenresByBandId(bandID);
            if (genres == null || genres.Count() < 1)
                return null;
            return PartialView("_GenresPartial", genres);
        }

    }
}
