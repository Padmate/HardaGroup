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

            B_Module bModule = new B_Module();
            B_News bNews = new B_News();
            //查找首页滚动的新闻
            var scrollNews = bNews.GetAllScrollNewsByCulture(culture);
            ViewData["scrollNews"] = scrollNews;
            //查找热点新闻
            var hotNews = bNews.GetAllHotNewsByCulture(culture);
            ViewData["hotNews"] = hotNews;
            //获取备用模块的内容
            var module1 = bModule.GetModuleByCultureAndType(culture, Common.ModuleType_Home_Module1);
            ViewData["module1"] = module1;

            //获取备用模块的内容
            var module2 = bModule.GetModuleByCultureAndType(culture, Common.ModuleType_Home_Module2);
            ViewData["module2"] = module2;

            //为什么选择华尔达
            var whychooseharda = bModule.GetModuleByCultureAndType(culture, Common.ModuleType_Home_WhyChooseHarda);
            ViewData["whychooseharda"] = whychooseharda;

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