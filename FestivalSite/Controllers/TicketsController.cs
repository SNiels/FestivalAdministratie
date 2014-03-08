using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FestivalLibAdmin.Class.Model;
using FestivalLibAdmin.Model;

namespace FestivalSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TicketsController : Controller
    {
        //
        // GET: /Tickets/

        public PartialViewResult Reserveer()
        {
            ViewBag.Type = new SelectList(TicketType.GetTypes().Where(type=>type.AvailableTickets>0), "ID", "Name"); 
            
            TicketIndexViewModel vm = null;
            if (Session["TicketViewModel"] != null)
                vm =(TicketIndexViewModel) Session["TicketViewModel"];
            else vm = new TicketIndexViewModel();
            return PartialView("ReserveerPartial",vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Index(TicketIndexViewModel vm)
        {
            if(vm.IsValid)
            {
                Session["TicketViewModel"] = vm;
                if(User==null||!User.Identity.IsAuthenticated)
                    return RedirectToAction("Login");
                var holder = new UserProfile(){
                    ID=Convert.ToInt32(Membership.GetUser().ProviderUserKey)//error wordt opgevangen in de view, dit is een vs fout, als je je aanmeld, stopt met debuggen en opnieuw start ben je nog aangemeld maar geef dit een error.
                };
                if(new Ticket()
                {
                    Amount = vm.Amount,
                    Type = vm.TicketType,
                     TicketHolderProfile=holder
                }.Insert())
                {
                    Festival.SingleFestival.Tickets = Ticket.GetTickets();
                    return RedirectToAction("Index");
                } 
            }
            return View("Index");
        }

        public ActionResult MyTickets()
        {
            IEnumerable<Ticket> tickets = null;
            if (User.Identity.IsAuthenticated)
            {
                    tickets = Ticket.GetTicketsByUserID("" + Membership.GetUser().ProviderUserKey);
                if (tickets == null || tickets.Count() < 0) tickets = null;
            }
            return PartialView("MyTicketsPartial",tickets);
        }

        public ActionResult Index()
        {
            return View();
        }


    }
}
