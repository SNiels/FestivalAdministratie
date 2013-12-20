using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FestivalLibAdmin.Model;
using Helper;

namespace FestivalSite.Controllers
{
    public class GenreController : Controller
    {
        //
        // GET: /Genre/

        public ActionResult Index()
        {
            Festival.SingleFestival = Festival.GetFestival();
            return View(Festival.SingleFestival.Genres);
        }

        //
        // GET: /Genre/Details/5

        public ActionResult Details(string name)
        {
            var genres = Festival.SingleFestival.Genres.Where(genre => genre.Name == name); //Genre.GetGenresByQuery("SELECT * From Genres WHERE Name=@Name", Database.CreateParameter("@Name", name));
            if (genres == null || genres.Count() < 1) return RedirectToAction("Index");
            return View(genres.First());
        }

        public PartialViewResult OptredensByGenrePartial(string genreID)
        {
            var optredens = Optreden.GetOptredensByGenreID(genreID);
            if (optredens == null || optredens.Count() < 1) return null;
            return PartialView("_OptredenCardsPartial", optredens);
        }
    }
}
