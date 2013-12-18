using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FestivalLibAdmin.Model;
using MvcSiteMapProvider;
using FestivalSite.Helpers;

namespace FestivalSite.DynamicNodeProvider
{
    public class DagDynamicNodeProvider:IDynamicNodeProvider
    {
        public bool AppliesTo(string providerName)
        {
            if (providerName == "FestivalSite.DynamicNodeProvider.DagDynamicNodeProvider,FestivalSite")
                return true;
            return false;
        }


        public IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            
            foreach (var lineUp in Festival.SingleFestival.ComputeLineUps().LineUps)
            {
                DynamicNode dynamicNode = new DynamicNode();
                dynamicNode.Title = lineUp.Dag.BeDayOfWeek().ToString();
                dynamicNode.ParentKey = "Dagen";
                dynamicNode.Action = "Details";
                dynamicNode.Controller = "Dag";
                dynamicNode.Key = "DagDetails_" + lineUp.Dag.BeDayOfWeek().ToString(); ;
                dynamicNode.RouteValues.Add("dag", lineUp.Dag.BeDayOfWeek().ToString());

                yield return dynamicNode;
            }
        }
    }
}