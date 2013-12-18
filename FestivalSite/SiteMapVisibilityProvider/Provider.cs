using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcSiteMapProvider;

namespace FestivalSite.SiteMapVisibilityProvider
{
    public class Provider:ISiteMapNodeVisibilityProvider
    {
        public bool AppliesTo(string providerName)
        {
            return true;
        }

        public bool IsVisible(ISiteMapNode node, IDictionary<string, object> sourceMetadata)
        {
            //var mvcNode = node as MvcSiteMapNode;
            //if (mvcNode == null)
            //{
            //    return true;
            //}
            //string visibility = mvcNode["visibility"];
            //if (string.IsNullOrEmpty(visibility))
            //{
            //    return true;
            //}
            //visibility = visibility.Trim();

            //switch (visibility)
            //{
            //    case "false":
            //        return false;
            //}

            //return true;
            return true;
        }
    }
}