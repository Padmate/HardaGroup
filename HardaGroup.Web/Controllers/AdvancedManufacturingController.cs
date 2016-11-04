using HardaGroup.Models;
using HardaGroup.Service;
using HardaGroup.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HardaGroup.Web.Controllers
{
    public class AdvancedManufacturingController:BaseController
    {
        /// <summary>
        /// 智能制造
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(){

            B_Image bImage = new B_Image();
            List<M_Image> bgImages = bImage.GetBGImagesByType(Common.Image_AdvancedManufacturingBG);
            ViewData["bgimages"] = bgImages;

            B_Module bModule = new B_Module();
            //获取当前国际化代码
            var culture = GlobalizationHelp.GetCurrentThreadCultureCode();
            //获取简介
            var introduction = bModule.GetModuleByCultureAndType(culture, Common.ModuleType_Manufacturing_Introduction);
            ViewData["introduction"] = introduction;
            //获取服务范围
            var serviceScope = bModule.GetAllCultureDataByType(culture,Common.ModuleType_Manufacturing_ServiceScope);
            ViewData["serviceScope"] = serviceScope;
            //获取服务特性
            var serviceFeatures = bModule.GetAllCultureDataByType(culture, Common.ModuleType_Manufacturing_ServiceFeatures);
            ViewData["serviceFeatures"] = serviceFeatures;

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