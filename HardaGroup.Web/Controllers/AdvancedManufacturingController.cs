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
            var introduction = this.GetIntroductionByCulture(culture);
            ViewData["introduction"] = introduction;
            //获取服务范围
            var serviceScope = bModule.GetAllCultureDataByType(culture,Common.ModuleType_Manufacturing_ServiceScope);
            ViewData["serviceScope"] = serviceScope;
            //获取服务特性
            var serviceFeatures = bModule.GetAllCultureDataByType(culture, Common.ModuleType_Manufacturing_ServiceFeatures);
            ViewData["serviceFeatures"] = serviceFeatures;

            return View();
        }

        /// <summary>
        /// 获取简介数据
        /// </summary>
        /// <returns></returns>
        private M_ModuleSearch GetIntroductionByCulture(string culture)
        {
            M_ModuleSearch result = new M_ModuleSearch();
            B_Module bModule = new B_Module();
            var introductionModules = bModule.GetModuleByType(Common.ModuleType_Manufacturing_Introduction);
            if(introductionModules.Count >0)
            {
                var introductionModule = introductionModules.First();
                
                //根据culture过滤数据，如果找不到数据则取中文数据
                var cultureGlobalizationData = introductionModule.ModuleGlobalizations
                    .Where(v => v.Culture == culture).FirstOrDefault();
                if(cultureGlobalizationData == null)
                {
                    cultureGlobalizationData = introductionModule.ModuleGlobalizations
                         .Where(v => v.Culture == Common.Globalization_Chinese).FirstOrDefault();
                }

                result.Id = introductionModule.Id;
                result.Content = cultureGlobalizationData.Content;
            }

            return result;
        
        }

        /// <summary>
        /// 制造简介
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveIntroduction()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_Module model = JsonHandle.DeserializeJsonToObject<M_Module>(strReqStream);

            model.ModuleURLId = "xxxxxxxxxx";
            model.ModuleGlobalizations.First().SubTitle = "智能制造简介";

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);

            message = model.ModuleGlobalizations.First().validate();
            if (!message.Success) return Json(message);

            var currentUser = this.GetCurrentUser();
            B_Module bModule = new B_Module(currentUser);

            string saveModuleId = string.Empty;
            //根据页面传入的moduleId判断是新增还是修改
            if(!string.IsNullOrEmpty(model.Id))
            {
                M_ModuleGlobalization mModuleGlobalization = new M_ModuleGlobalization();
                mModuleGlobalization = model.ModuleGlobalizations.First();
                mModuleGlobalization.ModuleId = model.Id;
                //修改
                message = bModule.DealModuleGlobalization(mModuleGlobalization);

                message.ReturnStrId = model.Id;

            }
            else
            {
                //校验必须先添加中文数据
                if(model.ModuleGlobalizations.First().Culture != Common.Globalization_Chinese)
                {
                    message.Success = false;
                    message.Content = "请先添加中文数据，再进行国际化操作。";
                    return Json(message);
                }
                //新增
                message = bModule.AddModule(model);
                message.ReturnStrId = message.ReturnId.ToString();
            }

            if (message.Success) message.Content = "保存成功";
            
            

            return Json(message);
        }

        /// <summary>
        /// 根据国际化代码加载 制造简介数据
        /// </summary>
        /// <param name="Culture"></param>
        /// <returns></returns>
        public ActionResult LoadIntroductionGlobalizationData(string Culture)
        {
            //
            Message message = new Message();
            message.Success = false;

            B_Module bModule = new B_Module();
            M_ModuleSearch searchModel = new M_ModuleSearch();
            searchModel.Type = Common.ModuleType_Manufacturing_Introduction;
            //根据类型查找数据
            var modules = bModule.GetModulesByMulitCondition(searchModel);

            if (modules.Count > 0)
            {
                var cultureModules = modules.Where(v => v.Culture == Culture).ToList() ;
                M_ModuleSearch result = new M_ModuleSearch();
                if(cultureModules.Count >0)
                {
                    result = cultureModules.First();
                }
                else
                {
                    result.Id = modules.First().Id;
                }
                message.Success = true;
                message.Content = JsonHandle.ToJson(result);
            }
            return Json(message);
        }
    }
}