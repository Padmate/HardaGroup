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
    public class VentureSupportController:BaseController
    {
        public ActionResult Index()
        {
            B_Image bImage = new B_Image();
            List<M_Image> bgImages = bImage.GetBGImagesByType(Common.Image_VentureSupportBG);
            ViewData["bgimages"] = bgImages;

            B_Module bModule = new B_Module();
            //获取当前国际化代码
            var culture = GlobalizationHelp.GetCurrentThreadCultureCode();
            
            //获取投资领域
            var investedFields = bModule.GetAllCultureDataByType(culture, Common.ModuleType_VentureSupport_InvestedFields);
            ViewData["investedFields"] = investedFields;
            //获取九大平台
            var ninePlatforms = bModule.GetAllCultureDataByType(culture, Common.ModuleType_VentureSupport_NineSupportingPlatforms);
            ViewData["ninePlatforms"] = ninePlatforms;
            //获取三大空间
            var threeSpaces = bModule.GetAllCultureDataByType(culture, Common.ModuleType_VentureSupport_ThreeSpaces);
            ViewData["threeSpaces"] = threeSpaces;

            return View();
        }

        public ActionResult ShowDetail(string type, string moduleUrlId)
        {

            if (string.IsNullOrEmpty(type) || !Common.Dic_ModuleType.ContainsKey(type.ToLower()))
                throw new HttpException(404, "");

            B_Module bModule = new B_Module();
            //获取当前culture
            var culture = GlobalizationHelp.GetCurrentThreadCultureCode();
            var module = bModule.GetModuleByModuleUrlIdAndCulture(moduleUrlId, culture);
            ViewData["module"] = module;

            if (module == null) throw new HttpException(404, "");



            return View();
        }

    }
}