using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Presentation_
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Repo", action = "RepoView", id = UrlParameter.Optional }
            );          

            routes.MapRoute(
            name: "Presentation",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Presentation", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "Dash",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Dash", action = "Dash", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "Upload",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Dash", action = "Upload", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "Download",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Dash", action = "Download", id = UrlParameter.Optional }
            );

        }
    }
}
