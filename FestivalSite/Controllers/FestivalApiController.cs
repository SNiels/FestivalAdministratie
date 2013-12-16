using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FestivalLibAdmin.Model;

namespace FestivalSite.Controllers
{
    public class FestivalController : ApiController
    {
        // GET api/<controller>
        public Festival Get()
        {
            //Festival.SingleFestival = Festival.GetFestival();
            //Festival.SingleFestival.ComputeLineUps();
            //return Festival.SingleFestival;
            try
            {
                //Festival.SingleFestival = Festival.GetFestival();
                //Festival.SingleFestival.ComputeLineUps();
                return Festival.GetFestival();
            }
            catch (Exception)
            {
                 throw new HttpResponseException(Request.CreateResponse(System.Net.HttpStatusCode.NotFound));
            }
            //if (Festival.SingleFestival == null) throw new HttpResponseException(Request.CreateResponse(System.Net.HttpStatusCode.NotFound));

            
        }
    }
}