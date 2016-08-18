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

            //获取当前国际化代码
            var culture = GlobalizationHelp.GetCurrentThreadCultureCode();
            //根据当前国际化代码过滤数据
            B_NewsScope bNewScope = new B_NewsScope();
            M_NewsScope searchModel = new M_NewsScope();
            searchModel.Culture = culture;
            List<M_NewsScope> allNewsScope = bNewScope.GetByMulitCond(searchModel);

            M_NewsScope mNewsScope = new M_NewsScope();
            if (allNewsScope.Count > 0)
            {
                if (string.IsNullOrEmpty(typecode))
                {
                    //如果类型代码为空，则去默认第一条数据
                    mNewsScope = allNewsScope.FirstOrDefault();
                }
                else
                {
                    //根据当前typecode过滤数据
                    mNewsScope = allNewsScope.Where(a => a.TypeCode == typecode).FirstOrDefault();


                }
            }
            if (mNewsScope == null) throw new HttpException(404, "");
            ViewData["allNewsScope"] = allNewsScope;
            ViewData["currentNewsScope"] = mNewsScope;

            return View();
        }

        #region 新闻范围
        public ActionResult NewsScope()
        {
            return View();
        }

        public ActionResult GetScopePageData()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_NewsScope model = JsonHandle.UnJson<M_NewsScope>(strReqStream);

            B_NewsScope bNewsScope = new B_NewsScope();
            var pageData = bNewsScope.GetPageData(model);
            var totalCount = bNewsScope.GetPageDataTotalCount(model);

            PageResult<M_NewsScope> pageResult = new PageResult<M_NewsScope>(totalCount, pageData);
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

            B_NewsScope bNewsScope = new B_NewsScope();

            message = bNewsScope.AddNewsScope(model);

            return Json(message);
        }


        public ActionResult EditScope(string id)
        {
            B_NewsScope bNewsScope = new B_NewsScope();

            var contactScope = bNewsScope.GetById(id);

            ViewData["NewsScope"] = contactScope;

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

            B_NewsScope bNewsScope = new B_NewsScope();
           

            message = bNewsScope.EditNewsScope(model);

            return Json(message);
        }

        [HttpPost]
        public ActionResult BachDeleteScopeById()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            List<string> ids = JsonHandle.DeserializeJsonToObject<List<string>>(strReqStream);

            Message message = new Message();
            B_NewsScope bNewsScope = new B_NewsScope();
            message = bNewsScope.BatchDeleteByIds(ids);
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
            //根据当前国际化代码获取资讯模块
            var culture = GlobalizationHelp.GetCurrentThreadCultureCode();
            B_NewsScope bNewsScope = new B_NewsScope();
            M_NewsScope searchModel = new M_NewsScope();
            searchModel.Culture = culture;
            var allNewsScopes = bNewsScope.GetByMulitCond(searchModel);
            
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

            //根据当前国际化代码获取资讯模块
            var culture = GlobalizationHelp.GetCurrentThreadCultureCode();
            B_NewsScope bNewsScope = new B_NewsScope();
            M_NewsScope searchModel = new M_NewsScope();
            searchModel.Culture = culture;
            var allNewsScopes = bNewsScope.GetByMulitCond(searchModel);

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
            searchModel.Culture = culture;
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
            searchModel.Culture = culture;
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
            List<string> ids = JsonHandle.DeserializeJsonToObject<List<string>>(strReqStream);


            Message message = new Message();
            B_News bNews = new B_News();
            message = bNews.BatchDeleteByIds(ids);
            return Json(message);
        }

        #endregion
    }
}