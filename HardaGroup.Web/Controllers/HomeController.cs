using HardaGroup.Models;
using HardaGroup.Service;
using HardaGroup.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HardaGroup.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Default()
        {
            B_Image bImage = new B_Image();
            List<M_Image> bgImages = bImage.GetBGImagesByType(Common.Image_HomeBG);
            ViewData["bgimages"] = bgImages;

            //获取当前国际化代码
            var culture = GlobalizationHelp.GetCurrentThreadCultureCode();

            B_News bNews = new B_News();
            //查找首页滚动的新闻
            var scrollNews = bNews.GetAllScrollNewsByCulture(culture);
            ViewData["scrollNews"] = scrollNews;
            //查找热点新闻
            var hotNews = bNews.GetAllHotNewsByCulture(culture);
            ViewData["hotNews"] = hotNews;

            return View();
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ContactUs()
        {
            B_Image bImage = new B_Image();
            List<M_Image> bgImages = bImage.GetBGImagesByType(Common.Image_ContactUsBG);
            ViewData["bgimages"] = bgImages;

            return View();
        }

       
    }
}