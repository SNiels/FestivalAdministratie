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
        //public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        //{
        //        // Create a node for each album 
        //        foreach (var band in Band.GetBands())
        //        {
        //            DynamicNode dynamicNode = new DynamicNode();
        //            dynamicNode.Title = band.Name;
        //            dynamicNode.ParentKey = "Bands";
        //            dynamicNode.Action = "Details";
        //            dynamicNode.Controller = "Band";
        //            dynamicNode.Key = "BandDetails_" + band.ID;
        //            dynamicNode.RouteValues.Add("name", band.Name);

        //            yield return dynamicNode;
        //        }
        //}

        public bool AppliesTo(string providerName)
        {
            if(providerName=="FestivalSite.DynamicNodeProvider.BandsNameDynamicNodeProvider,FestivalSite")
            return true;
            return false;
        }


        public IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            // Create a node for each album 
            foreach (var band in Band.GetBands())
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