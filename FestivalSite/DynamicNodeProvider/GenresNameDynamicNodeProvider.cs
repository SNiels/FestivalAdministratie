using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FestivalLibAdmin.Model;
using MvcSiteMapProvider;

namespace FestivalSite.DynamicNodeProvider
{
    public class GenresNameDynamicNodeProvider:IDynamicNodeProvider
    {
        public bool AppliesTo(string providerName)
        {
            if (providerName == "FestivalSite.DynamicNodeProvider.GenresNameDynamicNodeProvider,FestivalSite")
                return true;
            return false;
        }


        public IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            // Create a node for each album 
            foreach (var genre in Genre.GetGenres())
            {
                DynamicNode dynamicNode = new DynamicNode();
                dynamicNode.Title = genre.Name;
                dynamicNode.ParentKey = "Genres";
                dynamicNode.Action = "Details";
                dynamicNode.Controller = "Genre";
                dynamicNode.Key = "GenreDetails_" + genre.ID;
                dynamicNode.RouteValues.Add("name", genre.Name);

                yield return dynamicNode;
            }
        }
    }
}