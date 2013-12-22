using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FestivalLibAdmin.Model;

namespace FestivalSite
{
    public class TicketIndexViewModel
    {
        public TicketIndexViewModel()
        {
            Amount = 1;
        }
        [Required(ErrorMessage = "Gelieve het aantal tickets in te geven")]
        [Range(1, int.MaxValue, ErrorMessage = "Het aantal tickets moet minstens 1 zijn")]
        [Display(Name = "Aantal personen", Order = 0, GroupName = "Bestelling", Prompt = "Bv. 5")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public int Amount { get; set; }
        [Required(ErrorMessage = "Gelieve een ticket type in te geven")]
        [Display(Name = "Type", Order = 1, Description = "Het type ticket", GroupName = "Bestelling")]
        public string Type { get; set; }

        public TicketType TicketType
        {
            get
            {
                var list = TicketType.GetTypes().Where(type => type.ID == Type) ;
                if (list == null || list.Count() < 1) return null;
                return list.First();
            }
        }

        public bool IsValid
        {
            get
            {
                if (Amount > 0 && TicketType != null&&TicketType.AvailableTickets-Amount>=0) return true;
                return false;
            }
        }
    }
}