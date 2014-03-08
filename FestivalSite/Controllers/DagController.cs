using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FestivalLibAdmin.Model;
using FestivalSite.Helpers;

namespace FestivalSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DagController : Controller
    {
        //
        // GET: /Dag/

        public ActionResult Index()
        {
            Festival.SingleFestival = Festival.GetFestival();
            Festival.SingleFestival.ComputeLineUps();
            var lineups = Festival.SingleFestival.LineUps;
            if (lineups == null || lineups.Count() < 1) return RedirectToAction("Index", "Home");
            return View(lineups);
        }

        //
        // GET: /Dag/Details/5

        public ActionResult Details(DayOfWeekBE dag)
        {
            Festival.SingleFestival = Festival.GetFestival();
            Festival.SingleFestival.ComputeLineUps();
            var lineups = Festival.SingleFestival.LineUps.Where(lineup => lineup.Dag.BeDayOfWeek() == dag);
            if(lineups == null||lineups.Count()<1) return RedirectToAction("Index");
            return View(lineups.First());
        }

        public PartialViewResult StageOptredensPerLineupPartial(LineUp lineup)
        {
            var optredens = Festival.SingleFestival.Optredens.Where(optreden=>optreden.From.Date==lineup.Dag);
            if (optredens == null || optredens.Count() < 1) return null;
            return PartialView("_StageOptredensPerLineupPartial", optredens);
        }
    }
}
