using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FestivalLibAdmin.Model;
using MvcSiteMapProvider;

namespace FestivalSite.DynamicNodeProvider
{
    public class BandsNameDynamicNodeProvider : IDynamicNodeProvider

    {

        public bool AppliesTo(string providerName)
        {
            if(providerName=="FestivalSite.DynamicNodeProvider.BandsNameDynamicNodeProvider,FestivalSite")
            return true;
            return false;
        }


        public IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            foreach (var band in Festival.SingleFestival.Bands)
            {
                DynamicNode dynamicNode = new DynamicNode();
                dynamicNode.Title = band.Name;
                dynamicNode.ParentKey = "Bands";
                dynamicNode.Action = "Details";
                dynamicNode.Controller = "Band";
                dynamicNode.Key = "BandDetails_" + band.ID;
                dynamicNode.RouteValues.Add("name", band.Name);

                yield return dynamicNode;
            }
        }
    }
}