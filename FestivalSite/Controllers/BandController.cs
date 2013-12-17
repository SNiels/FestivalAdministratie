using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FestivalLibAdmin.Model;
using FestivalSite.ViewModels;
using MvcSiteMapProvider.Web.Mvc.Filters;

namespace FestivalSite.Controllers
{
    public class BandController : Controller
    {
        //
        // GET: /Bands/

        public ActionResult Index()
        {
            return View(Band.GetBands());            
        }

        //
        // GET: /Bands/Details/5
        public ActionResult Details(string name)
        {
            if (name ==null) return RedirectToAction("Index");
            try
            {
                Band band = Band.GetByName(name);
                if(band==null) return RedirectToAction("Index");
                return View(band);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("Index");
        }

        public PartialViewResult OptredensPartial(string bandID)
        {
                var optredens =Optreden.GetOptredens().Where(optreden => optreden.BandID == bandID);
                if (optredens != null && optredens.Count() < 1)
                    optredens= null;
            
            return PartialView("_OptredensPartial",optredens);
        }

    }
}
