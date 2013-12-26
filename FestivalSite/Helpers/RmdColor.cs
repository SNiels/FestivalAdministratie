using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FestivalSite.Helpers
{
    public static class RmdColor
    {
        private static Random Random = new Random();
        public static string GetRandomColor()
        {
            return "#" + Random.Next(255).ToString("X") + Random.Next(255).ToString("X") + Random.Next(255).ToString("#");
        }
    }
}