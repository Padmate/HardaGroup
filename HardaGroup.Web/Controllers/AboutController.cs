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
    public class AboutController:BaseController
    {
        /// <summary>
        /// 关于华尔达
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string typecode)
        {
            B_Image bImage = new B_Image();
            List<M_Image> bgImages = bImage.GetBGImagesByType(Common.Image_AboutBG);
            ViewData["bgimages"] = bgImages;

            B_About bAbout = new B_About();

            //获取当前国际化代码
            var culture = GlobalizationHelp.GetCurrentThreadCultureCode();
            //根据当前国际化代码过滤数据
            List<M_AboutSearch> cultureDatas = bAbout.GetAllCultureData(culture);
            
            M_AboutSearch mAbout = null;
            if (cultureDatas.Count > 0)
            {
                mAbout = new M_AboutSearch();
                if (string.IsNullOrEmpty(typecode))
                {
                    //如果类型代码为空，则取默认第一条数据
                    mAbout = cultureDatas.FirstOrDefault();
                }
                else
                {
                    //根据当前typecode过滤数据
                    mAbout = cultureDatas.Where(a => a.TypeCode == typecode).FirstOrDefault();
                }
            }
            if (mAbout == null) throw new HttpException(404, "");

            ViewData["allDatas"] = cultureDatas;
            ViewData["currentData"] = mAbout;
            

            return View();
        }



        public ActionResult GetPageData()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_AboutSearch model = JsonHandle.UnJson<M_AboutSearch>(strReqStream);

            B_About bAbout = new B_About();
            var pageData = bAbout.GetPageData(model);
            var totalCount = bAbout.GetPageDataTotalCount(model);

            PageResult<M_AboutSearch> pageResult = new PageResult<M_AboutSearch>(totalCount, pageData);
            return Json(pageResult);
        }

        public ActionResult Detail(string id)
        {
            B_About bAbout = new B_About();

            Int32 aboutId = System.Convert.ToInt32(id);
            var about = bAbout.GetById(aboutId);

            var zhCNGlobalization = about.AboutGlobalizations.Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();

            var aboutSearch = new M_AboutSearch()
            {
                Id = about.Id,
                TypeCode = about.TypeCode,
                Sequence = about.Sequence,
                TypeName = zhCNGlobalization == null ? "" : zhCNGlobalization.TypeName,
                Content = zhCNGlobalization == null ? "" : zhCNGlobalization.Content,
                Culture = zhCNGlobalization == null ? "" : zhCNGlobalization.Culture
            };


            ViewData["AboutSearch"] = aboutSearch;
           
            return View();
        }

        public ActionResult Add()
        {

            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveAdd()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_About model = JsonHandle.DeserializeJsonToObject<M_About>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);
            //校验
            message = model.AboutGlobalizations.First().validate();
            if (!message.Success) return Json(message);

            B_About bAbout = new B_About();
            message = bAbout.AddAbout(model);

            return Json(message);
        }


        public ActionResult Edit(string id)
        {
            B_About bAbout = new B_About();

            Int32 aboutId = System.Convert.ToInt32(id);
            var about = bAbout.GetById(aboutId);

            //过滤国际化代码为中文的数据
            var zhCNGlobalization = about.AboutGlobalizations
                .Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();

            var aboutSearch = new M_AboutSearch() { 
                Id = about.Id,
                TypeCode = about.TypeCode,
                Sequence = about.Sequence,
                TypeName = zhCNGlobalization == null?"":zhCNGlobalization.TypeName,
                Content = zhCNGlobalization == null ? "" : zhCNGlobalization.Content,
                Culture = zhCNGlobalization == null ? "" : zhCNGlobalization.Culture
            };


            ViewData["AboutSearch"] = aboutSearch;

            return View();
        }

        // POST:
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveEdit()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_About model = JsonHandle.DeserializeJsonToObject<M_About>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);
            //校验
            message = model.AboutGlobalizations.First().validate();
            if (!message.Success) return Json(message);

            B_About bAbout = new B_About();
            message = bAbout.EditAbout(model);

            return Json(message);
        }

        [HttpPost]
        public ActionResult BachDeleteById()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            List<string> aboutIds = JsonHandle.DeserializeJsonToObject<List<string>>(strReqStream);

            List<int> ids = new List<int>();
            foreach (var aboutid in aboutIds)
            {
                ids.Add(System.Convert.ToInt32(aboutid));
            }
            Message message = new Message();
            B_About bAbout = new B_About();
            message = bAbout.BatchDeleteByIds(ids);
            return Json(message);
        }

        /// <summary>
        /// 语言国际化
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Globalization(string id)
        {
            B_About bAbout = new B_About();

            Int32 aboutId = System.Convert.ToInt32(id);
            var about = bAbout.GetById(aboutId);
            ViewData["about"] = about;

            //需要进行国际化的语言
            Dictionary<string, string> globalizationLanguage = Common.Dic_Globalization
                                                        .Where(g=>g.Key != Common.Globalization_Chinese)
                                                        .ToDictionary(g=>g.Key,g=>g.Value);

            ViewData["globalizationLanguage"] = globalizationLanguage;

            return View();
        }

        [HttpPost]
        public ActionResult LoadAboutGlobalizationData(string AboutId,string Culture)
        {
            Message message = new Message();
            M_AboutGlobalization result = new M_AboutGlobalization();
            if (string.IsNullOrEmpty(AboutId) || string.IsNullOrEmpty(Culture))
            {
                message.Success = false;
                message.Content = "获取数据失败，请重新尝试";
                return Json(message);
            }

            B_About bAbout = new B_About();
            result = bAbout.GetAboutGlobalizationByAboutIdAndCulture(AboutId,Culture);
            
            if(result == null)
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
        public ActionResult SaveGlobalization()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_AboutGlobalization model = JsonHandle.DeserializeJsonToObject<M_AboutGlobalization>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);

            B_About bAbout = new B_About();
            message = bAbout.DealAboutGlobalization(model);

            return Json(message);
        }
    }
}