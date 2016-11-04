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
    public class ModuleController:BaseController
    {

        string _imageVirtualDirectory = SystemConfig.Init.PathConfiguration["moduleThumbnailsVirtualDirectory"].ToString();

        /// <summary>
        /// 查询分页数据 - BootstrapTable
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPageData()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_ModuleSearch model = JsonHandle.UnJson<M_ModuleSearch>(strReqStream);

            B_Module bModule = new B_Module();
            var pageData = bModule.GetPageDataForBootstrapTable(model);
            var totalCount = bModule.GetPageDataTotalCount(model);

            PageResult<M_ModuleSearch> pageResult = new PageResult<M_ModuleSearch>(totalCount, pageData);
            return Json(pageResult);
        }

        /// <summary>
        /// 查询分页数据 - BootstrapPaginator
        /// </summary>
        /// <param name="type"></param>
        /// <param name="page">当前所在页数</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPageDataForBootstrapPaginator(M_ModuleSearch searchModel)
        {
            //每页显示数据条数
            int limit = 5;
            searchModel.limit = limit;


            B_Module bModule = new B_Module();
            var pageData = bModule.GetPageDataForBootstrapPaginator(searchModel);
            var totalCount = bModule.GetPageDataTotalCount(searchModel);
            //总页数
            var totalPages = System.Convert.ToInt32(Math.Ceiling((double)totalCount / limit));

            PageResult<M_ModuleSearch> result = new PageResult<M_ModuleSearch>(totalCount, totalPages, pageData);
            return Json(result);
        }


        [HttpPost]
        public ActionResult GetModuleById(string moduleid)
        {
            B_Module bModule = new B_Module();

            var module = bModule.GetModuleById(moduleid); ;
            return Json(module);
        }

        /// <summary>
        /// 上传缩略图
        /// </summary>
        /// <param name="model"></param>
        /// <param name="thumbnails"></param>
        /// <param name="ReturnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult UploadThumbnailsImage(string moduleGlobalizationId, string culture, HttpPostedFileBase file)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "图片上传成功";

            //如果上传文件不为空
            if (file != null)
            {
                B_Module bModule = new B_Module();
                B_Image bImage = new B_Image();
                var moduleGlobalization = bModule.GetModuleGlobalizationById(moduleGlobalizationId);

                if (moduleGlobalization.Image != null)
                {
                    //删除原来的图片
                    message = bImage.DeleteImage(System.Convert.ToInt32(moduleGlobalization.Image.Id));
                    if (!message.Success) return Json(message);
                }
                //上传新图标
                message = bImage.AddCurrentCltureImage(file, _imageVirtualDirectory, Common.Module_Thumbnails, culture);
                if (!message.Success) return Json(message);
                int imageId = System.Convert.ToInt32(message.ReturnId);
                //更新新图标id到数据库
                message = bModule.UpdateImageId(moduleGlobalizationId, imageId);

            }

            return Json(message);
        }

        public ActionResult Add()
        {

            return View();

        }

        // POST:
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveAdd()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_Module model = JsonHandle.DeserializeJsonToObject<M_Module>(strReqStream);

            Message message = new Message();
            //校验model
            message = ValideData(model);
            if (!message.Success) return Json(message);


            var currentUser = this.GetCurrentUser();
            B_Module bModule = new B_Module(currentUser);
            message = bModule.AddModule(model);
            if (message.Success)
            {
                var moduleId = message.ReturnId.ToString();
                var culture = model.ModuleGlobalizations.First().Culture;
                //根据moduleId,查找刚插入的数据
                var moduleGlobalization = bModule.GetModuleGlobalizationByIdAndCulture(moduleId, culture);

                //返回ModuleGlobalization对象
                message.Content = JsonHandle.ToJson(moduleGlobalization);
            }

            return Json(message);
        }

        public ActionResult Edit(string moduleId)
        {

            B_Module bModule = new B_Module();
            var module = bModule.GetModuleById(moduleId);

            //过滤国际化代码为中文的数据
            var zhCNGlobalization = module.ModuleGlobalizations
                .Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();

            var moduleSearch = new M_ModuleSearch()
            {
                Id = module.Id,
                ModuleURLId = module.ModuleURLId,
                Sequence = module.Sequence,
                Type = module.Type,
                Title = zhCNGlobalization == null ? "" : zhCNGlobalization.Title,
                SubTitle = zhCNGlobalization == null ? "" : zhCNGlobalization.SubTitle,
                Description = zhCNGlobalization == null ? "" : zhCNGlobalization.Description,
                IsHref = (zhCNGlobalization == null || string.IsNullOrEmpty(zhCNGlobalization.Href))?false :true,
                Href = zhCNGlobalization == null ? "" : zhCNGlobalization.Href,
                Content = zhCNGlobalization == null ? "" : zhCNGlobalization.Content,
                Culture = zhCNGlobalization == null ? "" : zhCNGlobalization.Culture,
                Image = zhCNGlobalization == null ? null : zhCNGlobalization.Image,
                ImageClass = zhCNGlobalization == null ? "" : zhCNGlobalization.ImageClass

            };


            ViewData["ModuleSearch"] = moduleSearch;


            return View();
        }

        // POST:
        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveEdit()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_Module model = JsonHandle.DeserializeJsonToObject<M_Module>(strReqStream);

            Message message = new Message();
            //校验model
            message = ValideData(model);
            if (!message.Success) return Json(message);


            var currentUser = this.GetCurrentUser();
            B_Module bModule = new B_Module(currentUser);
            message = bModule.EditModule(model);

            if (message.Success)
            {
                var moduleId = message.ReturnId.ToString();
                var culture = model.ModuleGlobalizations.First().Culture;
                //根据moduleId,查找刚插入的数据
                var moduleGlobalization = bModule.GetModuleGlobalizationByIdAndCulture(moduleId, culture);

                //返回ModuleGlobalization对象
                message.Content = JsonHandle.ToJson(moduleGlobalization);
            }

            return Json(message);

        }

        public ActionResult ShowDetail(string type,string moduleUrlId)
        {

            if(string.IsNullOrEmpty(type) || !Common.Dic_ModuleType.ContainsKey(type.ToLower()))
                throw new HttpException(404, "");

            B_Module bModule = new B_Module();
            //获取当前culture
            var culture = GlobalizationHelp.GetCurrentThreadCultureCode();
            var module = bModule.GetModuleByModuleUrlIdAndCulture(moduleUrlId, culture);
            ViewData["module"] = module;

            if (module == null) throw new HttpException(404, "");



            return View();
        }

        public ActionResult Detail(string moduleId)
        {

            B_Module bModule = new B_Module();
            var module = bModule.GetModuleById(moduleId);

            //过滤国际化代码为中文的数据
            var zhCNGlobalization = module.ModuleGlobalizations
                .Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();

            var moduleSearch = new M_ModuleSearch()
            {
                Id = module.Id,
                ModuleURLId = module.ModuleURLId,
                Sequence = module.Sequence,
                Type = module.Type,
                Title = zhCNGlobalization == null ? "" : zhCNGlobalization.Title,
                SubTitle = zhCNGlobalization == null ? "" : zhCNGlobalization.SubTitle,
                Description = zhCNGlobalization == null ? "" : zhCNGlobalization.Description,
                Href = zhCNGlobalization == null ? "" : zhCNGlobalization.Href,
                Content = zhCNGlobalization == null ? "" : zhCNGlobalization.Content,
                Culture = zhCNGlobalization == null ? "" : zhCNGlobalization.Culture,
                Image = zhCNGlobalization == null ? null : zhCNGlobalization.Image,
                ImageClass = zhCNGlobalization == null ? "" : zhCNGlobalization.ImageClass
            };


            ViewData["ModuleSearch"] = moduleSearch;

            return View();
        }

        private Message ValideData(M_Module model)
        {
            Message message = new Message();
            message.Success = true;

            message = model.validate();
            if (!message.Success) return message;

            message = model.ModuleGlobalizations.First().validate();
            if (!message.Success) return message;

            message = this.ValidateHrefContent(model.ModuleGlobalizations.First());


            return message;
        }

        private Message ValidateHrefContent(M_ModuleGlobalization model)
        {
            Message message = new Message();
            message.Success = true;

            var isHref = model.IsHref;
            var href = model.Href;
            var content = model.Content;
            if (isHref && string.IsNullOrEmpty(href))
            {
                message.Success = false;
                message.Content = "链接不能为空";
                return message;
            }
            else if (!isHref && string.IsNullOrEmpty(content))
            {
                message.Success = false;
                message.Content = "内容不能为空";
                return message;
            }

            return message;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string ModuleId)
        {
            B_Module bModule = new B_Module();
            Message message = bModule.DeleteModule(ModuleId);

            return Json(message);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult BachDeleteById()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            List<string> strIds = JsonHandle.DeserializeJsonToObject<List<string>>(strReqStream);

            List<int> ids = new List<int>();
            foreach (var aboutid in strIds)
            {
                ids.Add(System.Convert.ToInt32(aboutid));
            }


            Message message = new Message();
            B_Module bModule = new B_Module();
            message = bModule.BatchDeleteByIds(ids);
            return Json(message);
        }

        /// <summary>
        /// Module国际化
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public ActionResult Globalization(string moduleId)
        {

            B_Module bModule = new B_Module();
            var module = bModule.GetModuleById(moduleId);


            ViewData["Module"] = module;

            //需要进行国际化的语言
            Dictionary<string, string> globalizationLanguage = Common.Dic_Globalization
                                                        .Where(g => g.Key != Common.Globalization_Chinese)
                                                        .ToDictionary(g => g.Key, g => g.Value);

            ViewData["globalizationLanguage"] = globalizationLanguage;

            return View();
        }

        /// <summary>
        /// 根据ModuleId和culture，查找数据
        /// </summary>
        /// <param name="ModuleScopeId"></param>
        /// <param name="Culture"></param>
        /// <returns></returns>
        public ActionResult LoadModuleGlobalizationData(string ModuleId, string Culture)
        {
            Message message = new Message();
            M_ModuleGlobalization result = new M_ModuleGlobalization();
            if (string.IsNullOrEmpty(ModuleId) || string.IsNullOrEmpty(Culture))
            {
                message.Success = false;
                message.Content = "获取数据失败，请重新尝试";
                return Json(message);
            }

            B_Module bModuleScope = new B_Module();
            result = bModuleScope.GetModuleGlobalizationByModuleIdAndCulture(ModuleId, Culture);

            if (result == null)
            {
                message.Success = false;
                message.Content = "获取数据失败，请重新尝试";
                return Json(message);
            }
            message.Success = true;
            message.Content = JsonHandle.ToJson(result); ;

            return Json(message);
        }

       

        // POST:
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveModuleGlobalization()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_ModuleGlobalization model = JsonHandle.DeserializeJsonToObject<M_ModuleGlobalization>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);

            message = this.ValidateHrefContent(model);
            if (!message.Success) return Json(message);


            B_Module bModule = new B_Module();
            message = bModule.DealModuleGlobalization(model);

            if (message.Success)
            {
                var moduleGlobalizationId = message.ReturnId.ToString();
                var moduleGlobalization = bModule.GetModuleGlobalizationById(moduleGlobalizationId);
                message.Content = JsonHandle.ToJson(moduleGlobalization);
            }

            return Json(message);
        }


        public ActionResult GenerateNewURLId()
        {
            Message message = new Message();
            message.Success = true;

            try
            {
                message.ReturnStrId = Guid.NewGuid().ToString();


            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "生成唯一标识失败：" + e.Message;
            }

            return Json(message);
        }

        #region 单页模块
       

        /// <summary>
        /// 保存模块
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveSingleModule()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_Module model = JsonHandle.DeserializeJsonToObject<M_Module>(strReqStream);

            Message message = new Message();

            if(string.IsNullOrEmpty(model.Type) || !Common.Dic_ModuleType.ContainsKey(model.Type))
            {
                message.Success = false;
                message.Content = "获取模块类型失败。";
                return Json(message);
            }
            model.ModuleURLId = Guid.NewGuid().ToString();
            model.ModuleGlobalizations.First().SubTitle = Common.Dic_ModuleType[model.Type];

            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);

            message = model.ModuleGlobalizations.First().validate();
            if (!message.Success) return Json(message);

            var currentUser = this.GetCurrentUser();
            B_Module bModule = new B_Module(currentUser);

            string saveModuleId = string.Empty;
            //根据页面传入的moduleId判断是新增还是修改
            if (!string.IsNullOrEmpty(model.Id))
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
                if (model.ModuleGlobalizations.First().Culture != Common.Globalization_Chinese)
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
        /// 根据国际化代码加载数据
        /// </summary>
        /// <param name="Culture"></param>
        /// <returns></returns>
        public ActionResult LoadModuleGlobalizationDataByType(string Culture,string moduletype)
        {
            //
            Message message = new Message();
            message.Success = false;

            B_Module bModule = new B_Module();
            M_ModuleSearch searchModel = new M_ModuleSearch();
            searchModel.Type = moduletype;
            //根据类型查找数据
            var modules = bModule.GetModulesByMulitCondition(searchModel);

            if (modules.Count > 0)
            {
                var cultureModules = modules.Where(v => v.Culture == Culture).ToList();
                M_ModuleSearch result = new M_ModuleSearch();
                if (cultureModules.Count > 0)
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

        #endregion
    }
}