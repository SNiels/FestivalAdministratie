using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FestivalLibAdmin.Model;

namespace FestivalSite.Controllers
{
    public class StageController : Controller
    {
        //
        // GET: /Stage/

        public ActionResult Index()
        {
            Festival.SingleFestival = Festival.GetFestival();
            return View(Festival.SingleFestival.Stages.OrderBy(stage=>stage.StageNumber));
        }

        //
        // GET: /Stage/Details/5

        public ActionResult Details(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return RedirectToAction("Index");
            Festival.SingleFestival = Festival.GetFestival();
                Festival.SingleFestival.ComputeLineUps();
                var stages = Festival.SingleFestival.Stages.Where(stager => stager.Name == name);
                if (stages == null || stages.Count() < 1) return RedirectToAction("Index");
            return View(stages.First());
        }

        //
        // GET: /Stage/Create

        public PartialViewResult StageOptredensPerLineupPartial(Stage stage)
        {
                Festival.SingleFestival = Festival.GetFestival();
                Festival.SingleFestival.ComputeLineUps();
            return PartialView("_StageOptredensPerLineupPartial",Festival.SingleFestival.Optredens.Where(optreden=>optreden.Stage.ID==stage.ID));
        }
    }
}
