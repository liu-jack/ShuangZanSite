using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShuangZan.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
         //   routes.MapRoute(
         //          "t",
         //          "product.html",
         //          new { controller = "TestController", action = "Index", id = UrlParameter.Optional }
         //       );
         //   routes.MapRoute(
         //       name: "getnewspage",
         //       url: "{controller}/{action}/{key}.html",
         //       defaults: new { controller = "PersonalUser", action = "OrderDetail", id = UrlParameter.Optional }
         //   );
         
      
         //   routes.MapRoute(
         //    name: "Home",
         //    url: "{controller}/{action}.html",
         //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
         //);
         //   routes.MapRoute(
         // name: "Default2",
         // url: "{controller}/{action}/{id}.html",
         // defaults: new { controller = "home", action = "Index", id = UrlParameter.Optional }
         //);
            routes.MapRoute(
              "Root",
              "",
              new { controller = "Home", action = "Index", id = UrlParameter.Optional });//根目录匹配  
            routes.MapRoute(
               name: "Girl",
               url: "BeautyGirl/{action}/{id}/{tags}",
               defaults: new { controller = "BeautyGirl", action = "GirlDetails", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}