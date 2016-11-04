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
    public class SolutionsController:BaseController
    {
        /// <summary>
        /// 组件产品
        /// </summary>
        /// <returns></returns>
        public ActionResult PartsAndModules ()
        {
            B_Image bImage = new B_Image();
            List<M_Image> bgImages = bImage.GetBGImagesByType(Common.Image_SolutionsBG_PM);
            ViewData["bgimages"] = bgImages;

            B_Module bModule = new B_Module();
            //获取当前国际化代码
            var culture = GlobalizationHelp.GetCurrentThreadCultureCode();
            //获取组件产品内容
            var content = bModule.GetModuleByCultureAndType(culture, Common.ModuleType_Solutions_PartsAndModules);
            ViewData["content"] = content;

            return View();
        }

        /// <summary>
        /// 研发
        /// </summary>
        /// <returns></returns>
        public ActionResult ResearchAndDevelopment()
        {
            B_Image bImage = new B_Image();
            List<M_Image> bgImages = bImage.GetBGImagesByType(Common.Image_SolutionsBG_RD);
            ViewData["bgimages"] = bgImages;

            B_Module bModule = new B_Module();
            //获取当前国际化代码
            var culture = GlobalizationHelp.GetCurrentThreadCultureCode();
            //获取简介
            var introduction = bModule.GetModuleByCultureAndType(culture, Common.ModuleType_Solutions_RDIntroduction);
            ViewData["introduction"] = introduction;
            //设计服务范围
            var designScopes = bModule.GetAllCultureDataByType(culture, Common.ModuleType_Solutions_DesignScopes);
            ViewData["designscopes"] = designScopes;
            //产品解决方案
            var productSolutions = bModule.GetAllCultureDataByType(culture, Common.ModuleType_Solutions_ProductSolution);
            ViewData["productsolutions"] = productSolutions;
            //研发流程与管理
            var workflow = bModule.GetAllCultureDataByType(culture, Common.ModuleType_Solutions_Workflow);
            ViewData["workflow"] = workflow;

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