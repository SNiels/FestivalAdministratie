using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FestivalLibAdmin.Model;
using MvcSiteMapProvider;

namespace FestivalSite.DynamicNodeProvider
{
    public class StagesNameDynamicNodeProvider:IDynamicNodeProvider
    {
        public bool AppliesTo(string providerName)
        {
            if (providerName == "FestivalSite.DynamicNodeProvider.StagesNameDynamicNodeProvider,FestivalSite")
                return true;
            return false;
        }


        public IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            // Create a node for each album 
            foreach (var stage in Stage.GetStages())
            {
                DynamicNode dynamicNode = new DynamicNode();
                dynamicNode.Title = stage.Name;
                dynamicNode.ParentKey = "Stages";
                dynamicNode.Action = "Details";
                dynamicNode.Controller = "Stage";
                dynamicNode.Key = "StageDetails_" + stage.ID;
                dynamicNode.RouteValues.Add("name", stage.Name);

                yield return dynamicNode;
            }
        }
    }
}