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
    public class NewsController:BaseController
    {
        public ActionResult Index(string typecode)
        {
            B_Image bImage = new B_Image();
            List<M_Image> bgImages = bImage.GetBGImagesByType(Common.Image_PressBG);
            ViewData["bgimages"] = bgImages;

            

            //获取所有模块shuj
            B_NewsScope bNewScope = new B_NewsScope();
            List<M_NewsScope> allNewsScope = bNewScope.GetAllData();

            List<M_NewsScopeSearch> currentList = new List<M_NewsScopeSearch>();
            //获取当前国际化代码
            var culture = GlobalizationHelp.GetCurrentThreadCultureCode();
            foreach(var newsScope in allNewsScope)
            {
                //根据国际化代码过滤数据，如果没有当前国际化代码的数据，则取中文数据
                var defaultGlobalizationData = newsScope.NewsScopeGlobalizations.Where(ag => ag.Culture == culture).FirstOrDefault();
                if (defaultGlobalizationData == null)
                {
                    defaultGlobalizationData = newsScope.NewsScopeGlobalizations.Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();
                }

                M_NewsScopeSearch currentData = new M_NewsScopeSearch();
                currentData.Id = newsScope.Id;
                currentData.TypeCode = newsScope.TypeCode;
                currentData.TypeName = defaultGlobalizationData.TypeName;

                currentList.Add(currentData);
            }

            M_NewsScopeSearch mNewsScope = new M_NewsScopeSearch();
            if (currentList.Count > 0)
            {
                if (string.IsNullOrEmpty(typecode))
                {
                    //如果类型代码为空，则取默认第一条数据
                    mNewsScope = currentList.FirstOrDefault();
                }
                else
                {
                    //根据当前typecode过滤数据
                    mNewsScope = currentList.Where(a => a.TypeCode == typecode).FirstOrDefault();


                }
            }
            if (mNewsScope == null) throw new HttpException(404, "");
            ViewData["allNewsScope"] = currentList;
            ViewData["currentNewsScope"] = mNewsScope;

            return View();
        }

        #region 资讯模块
        public ActionResult NewsScope()
        {
            return View();
        }

        public ActionResult GetScopePageData()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_NewsScopeSearch model = JsonHandle.UnJson<M_NewsScopeSearch>(strReqStream);

            B_NewsScope bNewsScope = new B_NewsScope();
            var pageData = bNewsScope.GetPageData(model);
            var totalCount = bNewsScope.GetPageDataTotalCount(model);

            PageResult<M_NewsScopeSearch> pageResult = new PageResult<M_NewsScopeSearch>(totalCount, pageData);
            return Json(pageResult);
        }

        public ActionResult AddScope()
        {
            return View();
        }

        // POST:
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveAddScope()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_NewsScope model = JsonHandle.DeserializeJsonToObject<M_NewsScope>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);
            message = model.NewsScopeGlobalizations.First().validate();
            if (!message.Success) return Json(message);


            B_NewsScope bNewsScope = new B_NewsScope();

            message = bNewsScope.AddNewsScope(model);

            return Json(message);
        }


        public ActionResult EditScope(string id)
        {
            B_NewsScope bNewsScope = new B_NewsScope();

            var newsScope = bNewsScope.GetById(id);

            //过滤国际化代码为中文的数据
            var zhCNGlobalization = newsScope.NewsScopeGlobalizations
                .Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();

            var newsScopeSearch = new M_NewsScopeSearch()
            {
                Id = newsScope.Id,
                TypeCode = newsScope.TypeCode,
                Sequence = newsScope.Sequence,
                TypeName = zhCNGlobalization == null ? "" : zhCNGlobalization.TypeName,
                Culture = zhCNGlobalization == null ? "" : zhCNGlobalization.Culture
            };


            ViewData["NewsScopeSearch"] = newsScopeSearch;

            return View();
        }

        // POST:
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveEditScope()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_NewsScope model = JsonHandle.DeserializeJsonToObject<M_NewsScope>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);
            message = model.NewsScopeGlobalizations.First().validate();
            if (!message.Success) return Json(message);

            B_NewsScope bNewsScope = new B_NewsScope();
            message = bNewsScope.EditNewsScope(model);

            return Json(message);
        }

        [HttpPost]
        public ActionResult BachDeleteScopeById()
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
            B_NewsScope bNewsScope = new B_NewsScope();
            message = bNewsScope.BatchDeleteByIds(ids);
            return Json(message);
        }


        /// <summary>
        /// 国际化
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GlobalizationNewsScope(string id)
        {
            B_NewsScope bNewsScope = new B_NewsScope();

            var newsScope = bNewsScope.GetById(id);

            ViewData["newsScope"] = newsScope;

            //需要进行国际化的语言
            Dictionary<string, string> globalizationLanguage = Common.Dic_Globalization
                                                        .Where(g => g.Key != Common.Globalization_Chinese)
                                                        .ToDictionary(g => g.Key, g => g.Value);

            ViewData["globalizationLanguage"] = globalizationLanguage;

            return View();
        }

        /// <summary>
        /// 根据NewsScopeId和culture，查找数据
        /// </summary>
        /// <param name="NewsScopeId"></param>
        /// <param name="Culture"></param>
        /// <returns></returns>
        public ActionResult LoadNewsScopeGlobalizationData(string NewsScopeId,string Culture)
        {
            Message message = new Message();
            M_NewsScopeGlobalization result = new M_NewsScopeGlobalization();
            if (string.IsNullOrEmpty(NewsScopeId) || string.IsNullOrEmpty(Culture))
            {
                message.Success = false;
                message.Content = "获取数据失败，请重新尝试";
                return Json(message);
            }

            B_NewsScope bNewsScope = new B_NewsScope();
            result = bNewsScope.GetNewsScopeGlobalizationByNewsScopeIdAndCulture(NewsScopeId, Culture);

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
        public ActionResult SaveGlobalization()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_NewsScopeGlobalization model = JsonHandle.DeserializeJsonToObject<M_NewsScopeGlobalization>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);

            B_NewsScope bNewsScope = new B_NewsScope();
            message = bNewsScope.DealAboutGlobalization(model);

            return Json(message);
        }

        #endregion
        #region News
        string _imageVirtualDirectory = SystemConfig.Init.PathConfiguration["newsThumbnailsVirtualDirectory"].ToString();

        /// <summary>
        /// 查询分页数据 - BootstrapTable
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPageData()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_News model = JsonHandle.UnJson<M_News>(strReqStream);

            B_News bNews = new B_News();
            var pageData = bNews.GetPageDataForBootstrapTable(model);
            var totalCount = bNews.GetPageDataTotalCount(model);

            PageResult<M_News> pageResult = new PageResult<M_News>(totalCount, pageData);
            return Json(pageResult);
        }

        /// <summary>
        /// 查询分页数据 - BootstrapPaginator
        /// </summary>
        /// <param name="type"></param>
        /// <param name="page">当前所在页数</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPageDataForBootstrapPaginator(M_News searchModel)
        {
            //每页显示数据条数
            int limit = 5;
            searchModel.limit = limit;

            B_News bNews = new B_News();
            var pageData = bNews.GetPageDataForBootstrapPaginator(searchModel);
            var totalCount = bNews.GetPageDataTotalCount(searchModel);
            //总页数
            var totalPages = System.Convert.ToInt32(Math.Ceiling((double)totalCount / limit));

            PageResult<M_News> result = new PageResult<M_News>(totalCount, totalPages, pageData);
            return Json(result);
        }


        [HttpPost]
        public ActionResult GetNewsById(string newsid)
        {
            B_News bNews = new B_News();

            var news = bNews.GetNewsById(newsid); ;
            return Json(news);
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
        public ActionResult UploadThumbnailsImage(string newsId,string culture ,HttpPostedFileBase file)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "图片上传成功";

            //如果上传文件不为空
            if (file != null)
            {
                B_News bNews = new B_News();
                B_Image bImage = new B_Image();
                var news = bNews.GetNewsById(newsId);

                if (news.Image != null)
                {
                    //删除原来的图片
                    message = bImage.DeleteImage(System.Convert.ToInt32(news.Image.Id));
                    if (!message.Success) return Json(message);
                }
                //上传新图标
                message = bImage.AddCurrentCltureImage(file, _imageVirtualDirectory, Common.News_Thumbnails,culture);
                if (!message.Success) return Json(message);
                int imageId = System.Convert.ToInt32(message.ReturnId);
                //更新新图标id到数据库
                message = bNews.UpdateImageId(newsId, imageId);

            }

            return Json(message);
        }

        public ActionResult Add()
        {
            //取资讯模块
            B_NewsScope bNewsScope = new B_NewsScope();
            var allNewsScopes = bNewsScope.GetAllZHCNData();
            
            ViewData["NewsScope"] = allNewsScopes;

            return View();

        }

        // POST:
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveAdd()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_News model = JsonHandle.DeserializeJsonToObject<M_News>(strReqStream);

            Message message = new Message();
            //校验model
            message = ValideData(model);
            if (!message.Success) return Json(message);

            var currentUser = this.GetCurrentUser();
            B_News bNews = new B_News(currentUser);
            message = bNews.AddNews(model);

            return Json(message);
        }

        public ActionResult Edit(string newsId)
        {

            B_News bNews = new B_News();
            var news = bNews.GetNewsById(newsId);
            ViewData["news"] = news;

            //取资讯模块
            B_NewsScope bNewsScope = new B_NewsScope();
            var allNewsScopes = bNewsScope.GetAllZHCNData();

            ViewData["NewsScope"] = allNewsScopes;


            return View();
        }

        public ActionResult ShowDetail(string newsId)
        {

            B_News bNews = new B_News();
            var news = bNews.GetNewsById(newsId);
            ViewData["news"] = news;

            //根据当前国际化代码获取资讯模块
            var culture = GlobalizationHelp.GetCurrentThreadCultureCode();
            B_NewsScope bNewsScope = new B_NewsScope();
            M_NewsScope searchModel = new M_NewsScope();
            //searchModel.Culture = culture;
            var allNewsScopes = bNewsScope.GetByMulitCond(searchModel);

            ViewData["NewsScope"] = allNewsScopes;


            return View();
        }

        public ActionResult Detail(string newsId)
        {

            B_News bNews = new B_News();
            var news = bNews.GetNewsById(newsId);
            ViewData["news"] = news;

            //根据当前国际化代码获取资讯模块
            var culture = GlobalizationHelp.GetCurrentThreadCultureCode();
            B_NewsScope bNewsScope = new B_NewsScope();
            M_NewsScope searchModel = new M_NewsScope();
            //searchModel.Culture = culture;
            var allNewsScopes = bNewsScope.GetByMulitCond(searchModel);

            ViewData["NewsScope"] = allNewsScopes;


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
            M_News model = JsonHandle.DeserializeJsonToObject<M_News>(strReqStream);

            Message message = new Message();
            //校验model
            message = ValideData(model);
            if (!message.Success) return Json(message);

            var currentUser = this.GetCurrentUser();
            B_News bNews = new B_News(currentUser);
            message = bNews.EditNews(model);

            return Json(message);

        }


        private Message ValideData(M_News model)
        {
            Message message = new Message();
            message.Success = true;

            message = model.validate();
            if (!message.Success) return message;

            return message;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string NewsId)
        {
            B_News bNews = new B_News();
            Message message = bNews.DeleteNews(NewsId);

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
            B_News bNews = new B_News();
            message = bNews.BatchDeleteByIds(ids);
            return Json(message);
        }

        #endregion
    }
}