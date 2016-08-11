using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HardaGroup.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //全球化路由
            routes.MapRoute(
                "Globalization", // 路由名称
                "{language}/{controller}/{action}/{id}", // 带有参数的 URL
                new { language = "zh-cn", controller = "Home", action = "Default", id = UrlParameter.Optional }, // 参数默认值
                new { language = "(?i)^(zh-cn|en-us)$" }    //参数只匹配zh-cn和en-us，不区分大小写
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Default", id = UrlParameter.Optional }
            );
        }
    }
}
