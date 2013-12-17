using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FestivalLibAdmin.Model;

namespace FestivalSite.ViewModels
{
    public class OptredensViewmodel
    {
        public OptredensViewmodel(Optreden optreden)
        {
            Optreden = optreden;
        }
        private Optreden Optreden { get; set; }

        private Stage Stage
        {
            get
            {
                if (Optreden != null) return Optreden.Stage;
                return null;
            }
        }
        public string StageName
        {
            get
            {
                if (Stage == null) return null;
                return Stage.Name;
            }
        }
    }
}