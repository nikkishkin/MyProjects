using System.Web.Mvc;
using System.Web.Routing;

namespace TaskOperator.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("*.sample");

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
            //routes.IgnoreRoute("{*allsample}", new { allaspx = @".*\.sample(/.*)?" });
            routes.IgnoreRoute("{handler}.sample");

            routes.IgnoreRoute("{handler}.validate");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                 "Delete task",
                 "{controller}/{action}/{taskId}/{pageNumber}",
                 new
                 {
                     controller = "Home", 
                     action = "Index",
                     taskId = UrlParameter.Optional, 
                     pageNumber = UrlParameter.Optional
                 }
            );
        }
    }
}