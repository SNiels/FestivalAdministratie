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

        public IEnumerable<object> Get(string request)
        {
            try
            {
                if (request == null) throw new HttpResponseException(Request.CreateResponse(System.Net.HttpStatusCode.NotFound));
                Festival.SingleFestival = Festival.GetFestival();

                Festival.SingleFestival.ComputeLineUps();
                switch (request.ToLower())
                {
                    case "bands":
                        return Festival.SingleFestival.Bands;
                    case "optredens":
                        return Festival.SingleFestival.Optredens;
                    case "stages":
                        return Festival.SingleFestival.Stages;
                    case "genres":
                        return Festival.SingleFestival.Genres;
                    default: throw new HttpResponseException(Request.CreateResponse(System.Net.HttpStatusCode.NotFound));
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(System.Net.HttpStatusCode.NotFound));
            }
        }

        public IEnumerable<object> Get(string request,string id)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(id)) throw new HttpResponseException(Request.CreateResponse(System.Net.HttpStatusCode.NotFound));
                Festival.SingleFestival = Festival.GetFestival();

                Festival.SingleFestival.ComputeLineUps();
                switch (request.ToLower())
                {
                    case "bands":
                        var bands = Festival.SingleFestival.Bands.Where(band => band.ID == id);
                        if (bands == null || bands.Count() < 1) throw new HttpResponseException(Request.CreateResponse(System.Net.HttpStatusCode.NotFound));
                        return bands;
                    case "optredens":
                        var optredens = Festival.SingleFestival.Optredens.Where(optreden => optreden.ID == id);
                        if (optredens == null || optredens.Count() < 1) throw new HttpResponseException(Request.CreateResponse(System.Net.HttpStatusCode.NotFound));
                        return optredens;
                    case "stages":
                        var stages = Festival.SingleFestival.Stages.Where(stage => stage.ID == id);
                        if (stages == null || stages.Count() < 1) throw new HttpResponseException(Request.CreateResponse(System.Net.HttpStatusCode.NotFound));
                        return stages;
                    case "genres":
                        var genres = Festival.SingleFestival.Genres.Where(genre => genre.ID == id);
                        if (genres == null || genres.Count() < 1) throw new HttpResponseException(Request.CreateResponse(System.Net.HttpStatusCode.NotFound));
                        return genres;
                    default: throw new HttpResponseException(Request.CreateResponse(System.Net.HttpStatusCode.NotFound));
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateResponse(System.Net.HttpStatusCode.NotFound));
            }
        }
    }
}
