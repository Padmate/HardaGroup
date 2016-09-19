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
    public class JoinUsController:BaseController
    {
        public ActionResult Index()
        {
            B_Image bImage = new B_Image();
            List<M_Image> bgImages = bImage.GetBGImagesByType(Common.Image_JoinUsBG);
            ViewData["bgimages"] = bgImages;

            B_Module bModule = new B_Module();
            //获取当前国际化代码
            var culture = GlobalizationHelp.GetCurrentThreadCultureCode();

            //获取所有职位分类
            B_Job bJob = new B_Job();
            B_JobScope bJobScope = new B_JobScope();
            List<M_JobScopeSearch> allJobScopes = bJobScope.GetAllCultureData(culture);
            ViewData["allJobScopes"] = allJobScopes;
            ViewData["jsonlsJobScope"] = JsonHandle.ToJson(allJobScopes);

            //获取所有工作地点
            var locations = bJob.GetLocationsByCulture(culture);
            ViewData["locations"] = locations;

            //获取热门职位
            List<M_JobSearch> hotJob = bJob.GetAllHotJobByCulture(culture);
            ViewData["hotJobs"] = hotJob;

            //获取公司活动
            var companyActivities = bModule.GetAllCultureDataByType(culture, Common.ModuleType_Company_Activities);
            ViewData["companyActivities"] = companyActivities;

            return View();
        }

        /// <summary>
        /// 显示公司活动详细
        /// </summary>
        /// <param name="type"></param>
        /// <param name="moduleUrlId"></param>
        /// <returns></returns>
        public ActionResult ShowCompanyActivityDetail(string type, string moduleUrlId)
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

        #region 招聘Job
        public ActionResult Jobs()
        {
            return View();
        }



        #region 职位分类
        public ActionResult JobScope()
        {
            return View();
        }

        public ActionResult GetJobScopePageData()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_JobScopeSearch model = JsonHandle.UnJson<M_JobScopeSearch>(strReqStream);

            B_JobScope bJobScope = new B_JobScope();
            var pageData = bJobScope.GetPageData(model);
            var totalCount = bJobScope.GetPageDataTotalCount(model);

            PageResult<M_JobScopeSearch> pageResult = new PageResult<M_JobScopeSearch>(totalCount, pageData);
            return Json(pageResult);
        }

        public ActionResult AddJobScope()
        {
            return View();
        }

        // POST:
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveAddJobScope()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_JobScope model = JsonHandle.DeserializeJsonToObject<M_JobScope>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);
            message = model.JobScopeGlobalizations.First().validate();
            if (!message.Success) return Json(message);


            B_JobScope bJobScope = new B_JobScope();

            message = bJobScope.AddJobScope(model);

            return Json(message);
        }


        public ActionResult EditJobScope(string id)
        {
            B_JobScope bJobScope = new B_JobScope();

            var jobScope = bJobScope.GetById(id);

            //过滤国际化代码为中文的数据
            var zhCNGlobalization = jobScope.JobScopeGlobalizations
                .Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();

            var jobScopeSearch = new M_JobScopeSearch()
            {
                Id = jobScope.Id,
                TypeCode = jobScope.TypeCode,
                Sequence = jobScope.Sequence,
                TypeName = zhCNGlobalization == null ? "" : zhCNGlobalization.TypeName,
                Culture = zhCNGlobalization == null ? "" : zhCNGlobalization.Culture
            };


            ViewData["JobScopeSearch"] = jobScopeSearch;

            return View();
        }

        // POST:
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveEditJobScope()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_JobScope model = JsonHandle.DeserializeJsonToObject<M_JobScope>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);
            message = model.JobScopeGlobalizations.First().validate();
            if (!message.Success) return Json(message);

            B_JobScope bJobScope = new B_JobScope();
            message = bJobScope.EditJobScope(model);

            return Json(message);
        }

        [HttpPost]
        public ActionResult BachDeleteJobScopeById()
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
            B_JobScope bJobScope = new B_JobScope();
            message = bJobScope.BatchDeleteByIds(ids);
            return Json(message);
        }


        /// <summary>
        /// 国际化
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GlobalizationJobScope(string id)
        {
            B_JobScope bJobScope = new B_JobScope();

            var jobScope = bJobScope.GetById(id);

            ViewData["jobScope"] = jobScope;

            //需要进行国际化的语言
            Dictionary<string, string> globalizationLanguage = Common.Dic_Globalization
                                                        .Where(g => g.Key != Common.Globalization_Chinese)
                                                        .ToDictionary(g => g.Key, g => g.Value);

            ViewData["globalizationLanguage"] = globalizationLanguage;

            return View();
        }

        /// <summary>
        /// 根据JobScopeId和culture，查找数据
        /// </summary>
        /// <param name="JobScopeId"></param>
        /// <param name="Culture"></param>
        /// <returns></returns>
        public ActionResult LoadJobScopeGlobalizationData(string JobScopeId, string Culture)
        {
            Message message = new Message();
            M_JobScopeGlobalization result = new M_JobScopeGlobalization();
            if (string.IsNullOrEmpty(JobScopeId) || string.IsNullOrEmpty(Culture))
            {
                message.Success = false;
                message.Content = "获取数据失败，请重新尝试";
                return Json(message);
            }

            B_JobScope bJobScope = new B_JobScope();
            result = bJobScope.GetJobScopeGlobalizationByJobScopeIdAndCulture(JobScopeId, Culture);

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
        public ActionResult SaveJobScopeGlobalization()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_JobScopeGlobalization model = JsonHandle.DeserializeJsonToObject<M_JobScopeGlobalization>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);

            B_JobScope bJobScope = new B_JobScope();
            message = bJobScope.DealJobScopeGlobalization(model);

            return Json(message);
        }

        #endregion
        #region Job
        /// <summary>
        /// 查询分页数据 - BootstrapTable
        /// </summary>
        /// <returns></returns>
        public ActionResult GetJobPageData()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_JobSearch model = JsonHandle.UnJson<M_JobSearch>(strReqStream);

            B_Job bJob = new B_Job();
            var pageData = bJob.GetPageDataForBootstrapTable(model);
            var totalCount = bJob.GetPageDataTotalCount(model);

            PageResult<M_JobSearch> pageResult = new PageResult<M_JobSearch>(totalCount, pageData);
            return Json(pageResult);
        }

        /// <summary>
        /// 查询分页数据 - BootstrapPaginator
        /// </summary>
        /// <param name="type"></param>
        /// <param name="page">当前所在页数</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetJobPageDataForBootstrapPaginator(M_JobSearch searchModel)
        {
            //每页显示数据条数
            int limit = 5;
            searchModel.limit = limit;


            B_Job bJob = new B_Job();
            var pageData = bJob.GetPageDataForBootstrapPaginator(searchModel);
            var totalCount = bJob.GetPageDataTotalCount(searchModel);
            //总页数
            var totalPages = System.Convert.ToInt32(Math.Ceiling((double)totalCount / limit));

            PageResult<M_JobSearch> result = new PageResult<M_JobSearch>(totalCount, totalPages, pageData);
            return Json(result);
        }


        [HttpPost]
        public ActionResult GetJobById(string jobid)
        {
            B_Job bJob = new B_Job();

            var job = bJob.GetJobById(jobid); ;
            return Json(job);
        }


        public ActionResult JobAdd()
        {
            //取资讯模块
            B_JobScope bJobScope = new B_JobScope();
            var allJobScopes = bJobScope.GetAllZHCNData();

            ViewData["JobScope"] = allJobScopes;

            return View();

        }

        // POST:
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveJobAdd()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_Job model = JsonHandle.DeserializeJsonToObject<M_Job>(strReqStream);

            Message message = new Message();
            //校验model
            message = ValideJobData(model);
            if (!message.Success) return Json(message);

            message = model.JobGlobalizations.First().validate();
            if (!message.Success) return Json(message);

            var currentUser = this.GetCurrentUser();
            B_Job bJob = new B_Job(currentUser);
            message = bJob.AddJob(model);
            if (message.Success)
            {
                var jobId = message.ReturnId.ToString();
                var culture = model.JobGlobalizations.First().Culture;
                //根据jobId,查找刚插入的数据
                var jobGlobalization = bJob.GetJobGlobalizationByIdAndCulture(jobId, culture);

                //返回JobGlobalization对象
                message.Content = JsonHandle.ToJson(jobGlobalization);
            }

            return Json(message);
        }

        public ActionResult JobEdit(string jobId)
        {

            B_Job bJob = new B_Job();
            var job = bJob.GetJobById(jobId);

            //过滤国际化代码为中文的数据
            var zhCNGlobalization = job.JobGlobalizations
                .Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();

            var jobSearch = new M_JobSearch()
            {
                Id = job.Id,
                URLId = job.URLId,
                IsHot = job.IsHot,
                Pubtime = job.Pubtime,
                JobScopeId = job.JobScopeId,
                Location = zhCNGlobalization == null ? "" : zhCNGlobalization.Location,
                JobTitle = zhCNGlobalization == null ? "" : zhCNGlobalization.JobTitle,
                Description = zhCNGlobalization == null ? "" : zhCNGlobalization.Description,
                Content = zhCNGlobalization == null ? "" : zhCNGlobalization.Content,
                Culture = zhCNGlobalization == null ? "" : zhCNGlobalization.Culture
            };


            ViewData["JobSearch"] = jobSearch;


            //获取资讯模块
            B_JobScope bJobScope = new B_JobScope();
            var allJobScopes = bJobScope.GetAllZHCNData();

            ViewData["JobScope"] = allJobScopes;


            return View();
        }

        // POST:
        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public ActionResult SaveJobEdit()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_Job model = JsonHandle.DeserializeJsonToObject<M_Job>(strReqStream);

            Message message = new Message();
            //校验model
            message = ValideJobData(model);
            if (!message.Success) return Json(message);

            message = model.JobGlobalizations.First().validate();
            if (!message.Success) return Json(message);

            var currentUser = this.GetCurrentUser();
            B_Job bJob = new B_Job(currentUser);
            message = bJob.EditJob(model);

            if (message.Success)
            {
                var jobId = message.ReturnId.ToString();
                var culture = model.JobGlobalizations.First().Culture;
                //根据jobId,查找刚插入的数据
                var jobGlobalization = bJob.GetJobGlobalizationByIdAndCulture(jobId, culture);

                //返回JobGlobalization对象
                message.Content = JsonHandle.ToJson(jobGlobalization);
            }

            return Json(message);

        }

        public ActionResult JobShowDetail(string urlId)
        {

            B_Job bJob = new B_Job();
            //获取当前culture
            var culture = GlobalizationHelp.GetCurrentThreadCultureCode();
            var job = bJob.GetJobByJobUrlIdAndCulture(urlId, culture);
            ViewData["job"] = job;

            if (job == null) throw new HttpException(404, "");

            //获取职位分类
            B_JobScope bJobScope = new B_JobScope();
            var allJobScopes = bJobScope.GetAllZHCNData();

            ViewData["JobScope"] = allJobScopes;


            return View();
        }

        public ActionResult JobDetail(string jobId)
        {

            B_Job bJob = new B_Job();
            var job = bJob.GetJobById(jobId);

            //过滤国际化代码为中文的数据
            var zhCNGlobalization = job.JobGlobalizations
                .Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();

            var jobSearch = new M_JobSearch()
            {
                Id = job.Id,
                URLId = job.URLId,
                IsHot = job.IsHot,
                Pubtime = job.Pubtime,
                JobScopeId = job.JobScopeId,
                Location = zhCNGlobalization == null ? "" : zhCNGlobalization.Location,
                JobTitle = zhCNGlobalization == null ? "" : zhCNGlobalization.JobTitle,
                Description = zhCNGlobalization == null ? "" : zhCNGlobalization.Description,
                Content = zhCNGlobalization == null ? "" : zhCNGlobalization.Content,
                Culture = zhCNGlobalization == null ? "" : zhCNGlobalization.Culture
            };


            ViewData["JobSearch"] = jobSearch;


            //获取资讯模块
            B_JobScope bJobScope = new B_JobScope();
            var allJobScopes = bJobScope.GetAllZHCNData();

            ViewData["JobScope"] = allJobScopes;


            return View();
        }

        private Message ValideJobData(M_Job model)
        {
            Message message = new Message();
            message.Success = true;

            message = model.validate();
            if (!message.Success) return message;

            return message;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteJob(string JobId)
        {
            B_Job bJob = new B_Job();
            Message message = bJob.DeleteJob(JobId);

            return Json(message);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult BachDeleteJobById()
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
            B_Job bJob = new B_Job();
            message = bJob.BatchDeleteByIds(ids);
            return Json(message);
        }

        /// <summary>
        /// Job国际化
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        public ActionResult JobGlobalization(string jobId)
        {

            B_Job bJob = new B_Job();
            var job = bJob.GetJobById(jobId);


            ViewData["Job"] = job;


            //获取资讯模块
            B_JobScope bJobScope = new B_JobScope();
            var allJobScopes = bJobScope.GetAllZHCNData();

            ViewData["JobScope"] = allJobScopes;

            //需要进行国际化的语言
            Dictionary<string, string> globalizationLanguage = Common.Dic_Globalization
                                                        .Where(g => g.Key != Common.Globalization_Chinese)
                                                        .ToDictionary(g => g.Key, g => g.Value);

            ViewData["globalizationLanguage"] = globalizationLanguage;

            return View();
        }

        /// <summary>
        /// 根据JobId和culture，查找数据
        /// </summary>
        /// <param name="JobScopeId"></param>
        /// <param name="Culture"></param>
        /// <returns></returns>
        public ActionResult LoadJobGlobalizationData(string JobId, string Culture)
        {
            Message message = new Message();
            M_JobGlobalization result = new M_JobGlobalization();
            if (string.IsNullOrEmpty(JobId) || string.IsNullOrEmpty(Culture))
            {
                message.Success = false;
                message.Content = "获取数据失败，请重新尝试";
                return Json(message);
            }

            B_Job bJobScope = new B_Job();
            result = bJobScope.GetJobGlobalizationByJobIdAndCulture(JobId, Culture);

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
        public ActionResult SaveJobGlobalization()
        {
            StreamReader srRequest = new StreamReader(Request.InputStream);
            String strReqStream = srRequest.ReadToEnd();
            M_JobGlobalization model = JsonHandle.DeserializeJsonToObject<M_JobGlobalization>(strReqStream);

            Message message = new Message();
            //校验model
            message = model.validate();
            if (!message.Success) return Json(message);

            B_Job bJob = new B_Job();
            message = bJob.DealJobGlobalization(model);

            if (message.Success)
            {
                var jobGlobalizationId = message.ReturnId.ToString();
                var jobGlobalization = bJob.GetJobGlobalizationById(jobGlobalizationId);
                message.Content = JsonHandle.ToJson(jobGlobalization);
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
        #endregion

        #endregion

        
    }
}