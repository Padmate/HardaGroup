using HardaGroup.DataAccess;
using HardaGroup.Entities;
using HardaGroup.Models;
using HardaGroup.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Service
{
    public class B_Job
    {
        D_Job _dJob = new D_Job();

        public B_Job()
        {

        }

        M_User _currentUser;
        string _mapPath;
        public B_Job(M_User currentUser)
        {
            _currentUser = currentUser;

        }

        public B_Job(M_User currentUser, string mapPath)
        {
            _currentUser = currentUser;
            _mapPath = mapPath;

        }

        public List<string> GetLocationsByCulture(string culture)
        {
            return _dJob.GetLocationsByCulture(culture);
        }

        /// <summary>
        /// 获取分页数据 - BootstrapTable
        /// </summary>
        /// <param name="about"></param>
        /// <returns></returns>
        public List<M_JobSearch> GetPageDataForBootstrapTable(M_JobSearch job)
        {
            JobSearch searchModel = new JobSearch()
            {

                Location = job.Location,
                JobTitle = job.JobTitle,
                Culture = job.Culture,
                IsHotSearch = job.IsHotSearch

            };

            if (string.IsNullOrEmpty(job.JobScopeId))
            {
                searchModel.JobScopeId = null;
            }
            else
            {
                searchModel.JobScopeId = System.Convert.ToInt32(job.JobScopeId);
            }

            var offset = job.offset;
            var limit = job.limit;


            var pageResult = _dJob.GetPageData(searchModel, offset, limit);
            var result = pageResult.Select(a => ConverSearchEntityToModel(a)).ToList();

            return result;
        }


        /// <summary>
        /// 获取分页数据 - bootstrap-paginator
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public List<M_JobSearch> GetPageDataForBootstrapPaginator(M_JobSearch job)
        {

            JobSearch searchModel = new JobSearch()
            {
                Location = job.Location,
                JobTitle = job.JobTitle,
                Culture = job.Culture,
                IsHotSearch = job.IsHotSearch
            };
            if (string.IsNullOrEmpty(job.JobScopeId))
            {
                searchModel.JobScopeId = null;
            }
            else
            {
                searchModel.JobScopeId = System.Convert.ToInt32(job.JobScopeId);
            }

            var currentPage = job.page;
            var limit = job.limit;

            //page:第一页表示从第0条数据开始索引
            Int32 skip = System.Convert.ToInt32((currentPage - 1) * limit);

            B_Image bImage = new B_Image();
            var pageResult = _dJob.GetPageData(searchModel, skip, limit);
            var result = pageResult.Select(a => ConverSearchEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取分页数据总条数 
        /// </summary>
        /// <returns></returns>
        public int GetPageDataTotalCount(M_JobSearch job)
        {

            JobSearch searchModel = new JobSearch()
            {
                Location = job.Location,
                JobTitle = job.JobTitle,
                Culture = job.Culture,
                IsHotSearch = job.IsHotSearch
            };
            if (string.IsNullOrEmpty(job.JobScopeId))
            {
                searchModel.JobScopeId = null;
            }
            else
            {
                searchModel.JobScopeId = System.Convert.ToInt32(job.JobScopeId);
            }

            var totalCount = _dJob.GetPageDataTotalCount(searchModel);
            return totalCount;
        }


        public M_Job GetJobById(string newsId)
        {
            B_Image bImage = new B_Image();

            int id = System.Convert.ToInt32(newsId);
            var job = _dJob.GetJobById(id);
            var result = ConverEntityToModel(job);
            return result;
        }

        public List<M_JobSearch> GetAllHotJobByCulture(string culture)
        {
            var allJob = _dJob.GetAllHotJob();

            B_Image bImage = new B_Image();
            List<M_JobSearch> cultureDatas = new List<M_JobSearch>();
            foreach (var job in allJob)
            {
                //根据国际化代码过滤数据，如果没有当前国际化代码的数据，则取中文数据
                var defaultGlobalizationData = job.JobGlobalizations.Where(ag => ag.Culture == culture).FirstOrDefault();
                if (defaultGlobalizationData == null)
                {
                    defaultGlobalizationData = job.JobGlobalizations.Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();
                }

                M_JobSearch cultureData = new M_JobSearch();
                cultureData.Id = job.Id.ToString();
                cultureData.URLId = job.URLId;
                cultureData.IsHot = job.IsHot;
                cultureData.Location = defaultGlobalizationData.Location;
                cultureData.JobTitle = defaultGlobalizationData.JobTitle;
                cultureData.Description = defaultGlobalizationData.Description;
                cultureData.Content = defaultGlobalizationData.Content;
                cultureData.CreateDate = job.CreateDate;
                cultureData.Creator = job.Creator;
                cultureData.ModifiedDate = job.ModifiedDate;
                cultureData.Modifier = job.Modifier;
                cultureData.Pubtime = job.Pubtime;
                cultureData.JobScopeId = job.JobScopeId.ToString();
                cultureData.ScopeTypeCode = job.JobScope.TypeCode;

                cultureDatas.Add(cultureData);
            }

            return cultureDatas;

        }

        /// <summary>
        /// 根据当前id获取上一条数据的id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public M_JobSearch GetPreviousIdByJobUrlIdAndCulture(string newsUrlId, string culture)
        {
            B_Image bImage = new B_Image();

            var job = _dJob.GetPreviousDataByURLId(newsUrlId);

            if (job == null) return null;
            M_JobSearch cultureData = new M_JobSearch();

            //根据国际化代码过滤数据，如果没有当前国际化代码的数据，则取中文数据
            var defaultGlobalizationData = job.JobGlobalizations.Where(ag => ag.Culture == culture).FirstOrDefault();
            if (defaultGlobalizationData == null)
            {
                defaultGlobalizationData = job.JobGlobalizations.Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();
            }

            cultureData.Id = job.Id.ToString();
            cultureData.URLId = job.URLId;
            cultureData.IsHot = job.IsHot;
            cultureData.Location = defaultGlobalizationData.Location;
            cultureData.JobTitle = defaultGlobalizationData.JobTitle;
            cultureData.Description = defaultGlobalizationData.Description;
            cultureData.Content = defaultGlobalizationData.Content;
            cultureData.CreateDate = job.CreateDate;
            cultureData.Creator = job.Creator;
            cultureData.ModifiedDate = job.ModifiedDate;
            cultureData.Modifier = job.Modifier;
            cultureData.Pubtime = job.Pubtime;
            cultureData.JobScopeId = job.JobScopeId.ToString();

            return cultureData;
        }

        /// <summary>
        /// 根据当前id获取下一条数据的id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public M_JobSearch GetNextIdByCurrentJobUrlIdAndCulture(string newsUrlId, string culture)
        {
            B_Image bImage = new B_Image();

            var job = _dJob.GetNextDataByURLId(newsUrlId);
            if (job == null) return null;
            M_JobSearch cultureData = new M_JobSearch();

            //根据国际化代码过滤数据，如果没有当前国际化代码的数据，则取中文数据
            var defaultGlobalizationData = job.JobGlobalizations.Where(ag => ag.Culture == culture).FirstOrDefault();
            if (defaultGlobalizationData == null)
            {
                defaultGlobalizationData = job.JobGlobalizations.Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();
            }

            cultureData.Id = job.Id.ToString();
            cultureData.URLId = job.URLId;
            cultureData.IsHot = job.IsHot;
            cultureData.Location = defaultGlobalizationData.Location;
            cultureData.JobTitle = defaultGlobalizationData.JobTitle;
            cultureData.Description = defaultGlobalizationData.Description;
            cultureData.Content = defaultGlobalizationData.Content;
            cultureData.CreateDate = job.CreateDate;
            cultureData.Creator = job.Creator;
            cultureData.ModifiedDate = job.ModifiedDate;
            cultureData.Modifier = job.Modifier;
            cultureData.Pubtime = job.Pubtime;
            cultureData.JobScopeId = job.JobScopeId.ToString();

            return cultureData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public M_JobSearch GetJobByJobUrlIdAndCulture(string newsUrlId, string culture)
        {
            B_Image bImage = new B_Image();
            var job = _dJob.GetJobByJobUrlId(newsUrlId);

            if (job == null) return null;
            M_JobSearch cultureData = new M_JobSearch();

            //根据国际化代码过滤数据，如果没有当前国际化代码的数据，则取中文数据
            var defaultGlobalizationData = job.JobGlobalizations.Where(ag => ag.Culture == culture).FirstOrDefault();
            if (defaultGlobalizationData == null)
            {
                defaultGlobalizationData = job.JobGlobalizations.Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();
            }

            cultureData.Id = job.Id.ToString();
            cultureData.URLId = job.URLId;
            cultureData.IsHot = job.IsHot;
            cultureData.Location = defaultGlobalizationData.Location;
            cultureData.JobTitle = defaultGlobalizationData.JobTitle;
            cultureData.Description = defaultGlobalizationData.Description;
            cultureData.Content = defaultGlobalizationData.Content;
            cultureData.CreateDate = job.CreateDate;
            cultureData.Creator = job.Creator;
            cultureData.ModifiedDate = job.ModifiedDate;
            cultureData.Modifier = job.Modifier;
            cultureData.Pubtime = job.Pubtime;
            cultureData.JobScopeId = job.JobScopeId.ToString();

            return cultureData;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// 
        /// <returns></returns>
        public Message AddJob(M_Job model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "新增成功";

            try
            {
                //只能有唯一的JobUrlId
                var search = new Job() { URLId = model.URLId };
                var data = _dJob.GetByMulitCondition(search);
                if (data.Count > 0)
                {
                    message.Success = false;
                    message.Content = "系统中已存在标识为'" + model.URLId + "'的数据,不能重复添加。";
                    return message;

                }

                //新增
                var job = new Job()
                {

                    URLId = model.URLId,
                    IsHot = model.IsHot,
                    CreateDate = DateTime.Now,
                    Creator = _currentUser.UserName,
                    Pubtime = model.Pubtime,
                    JobScopeId = System.Convert.ToInt32(model.JobScopeId),
                    JobGlobalizations = model.JobGlobalizations.Select(ng => new JobGlobalization()
                    {
                        Location = ng.Location,
                        JobTitle = ng.JobTitle,
                        Description = ng.Description,
                        Content = ng.Content,
                        Culture = ng.Culture

                    }).ToList()

                };

                int newsId = _dJob.AddJob(job);
                message.ReturnId = newsId;

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "新增失败，异常：" + e.Message;
            }
            return message;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Message EditJob(M_Job model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "修改成功";

            try
            {
                var newsId = System.Convert.ToInt32(model.Id);
                var culture = model.JobGlobalizations.First().Culture;

                //只能有唯一的JobUrlId
                var search = new Job() { URLId = model.URLId };
                var data = _dJob.GetByMulitCondition(search);
                if (data.Where(a => a.Id != System.Convert.ToInt32(model.Id)).Count() > 0)
                {
                    message.Success = false;
                    message.Content = "系统中已存在标识为'" + model.URLId + "'的数据,不能重复添加。";
                    return message;

                }
                //根据newsId和culture，查找已存在的数据
                var oldData = _dJob.GetJobGlobalizationByJobIdAndCulture(newsId, culture);

                var job = new Job()
                {
                    URLId = model.URLId,
                    IsHot = model.IsHot,
                    ModifiedDate = DateTime.Now,
                    Modifier = _currentUser.UserName,
                    Pubtime = model.Pubtime,
                    JobScopeId = System.Convert.ToInt32(model.JobScopeId),
                    JobGlobalizations = model.JobGlobalizations.Select(ng => new JobGlobalization()
                    {
                        JobId = newsId,
                        Location = ng.Location,
                        JobTitle = ng.JobTitle,
                        Description = ng.Description,
                        Content = ng.Content,
                        Culture = ng.Culture

                    }).ToList()
                };

                message.ReturnId = _dJob.EditJob(System.Convert.ToInt32(model.Id), job);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "修改失败，异常：" + e.Message;
            }
            return message;
        }

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Message DeleteJob(string newsId)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "删除成功";
            try
            {
                var id = System.Convert.ToInt32(newsId);
                B_Image bImage = new B_Image();
                //删除图标
                var job = _dJob.GetJobById(id);
                //if (job.ImageId != null)
                //    bImage.DeleteImage(System.Convert.ToInt32(job.ImageId));

                _dJob.DeleteJob(id);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "删除失败，异常：" + e.Message;
            }
            return message;
        }

        /// <summary>
        /// 根据ID批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Message BatchDeleteByIds(List<Int32> ids)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "删除成功";

            try
            {

                _dJob.BatchDeleteJob(ids);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "删除失败：" + e.Message;
            }

            return message;
        }

        private M_Job ConverEntityToModel(Job job)
        {
            if (job == null) return null;

            B_Image bImage = new B_Image();

            var model = new M_Job()
            {
                Id = job.Id.ToString(),
                URLId = job.URLId,
                IsHot = job.IsHot,
                CreateDate = job.CreateDate,
                Creator = job.Creator,
                ModifiedDate = job.ModifiedDate,
                Modifier = job.Modifier,
                Pubtime = job.Pubtime,
                JobScopeId = job.JobScopeId.ToString(),
                JobGlobalizations = job.JobGlobalizations.Select(ng => new M_JobGlobalization()
                {
                    Location = ng.Location,
                    JobTitle = ng.JobTitle,
                    Description = ng.Description,
                    Content = ng.Content,
                    Culture = ng.Culture
                }).ToList()
            };
            return model;
        }

        private M_JobSearch ConverSearchEntityToModel(JobSearch job)
        {
            if (job == null) return null;


            var model = new M_JobSearch()
            {
                Id = job.Id.ToString(),
                URLId = job.URLId,
                IsHot = job.IsHot,
                Location = job.Location,
                JobTitle = job.JobTitle,
                Description = job.Description,
                Content = job.Content,
                CreateDate = job.CreateDate,
                Creator = job.Creator,
                ModifiedDate = job.ModifiedDate,
                Modifier = job.Modifier,
                Pubtime = job.Pubtime,
                JobScopeId = job.JobScopeId.ToString()
            };
            return model;
        }

        #region JobGlobalization
        public M_JobGlobalization GetJobGlobalizationByJobIdAndCulture(string newsId, string culture)
        {
            int id = System.Convert.ToInt32(newsId);
            var newsGlobalization = _dJob.GetJobGlobalizationByJobIdAndCulture(id, culture);
            var result = ConverJobGlobalizationEntityToModel(newsGlobalization);
            return result;
        }

        public M_JobGlobalization GetJobGlobalizationById(string newsGlobalizationId)
        {

            int id = System.Convert.ToInt32(newsGlobalizationId);
            var newsGlobalization = _dJob.GetJobGlobalizationById(id);
            var result = ConverJobGlobalizationEntityToModel(newsGlobalization);
            return result;
        }

        public M_JobGlobalization GetJobGlobalizationByIdAndCulture(string newsGlobalizationId, string culture)
        {

            int id = System.Convert.ToInt32(newsGlobalizationId);
            var newsGlobalization = _dJob.GetJobGlobalizationByJobIdAndCulture(id, culture);
            var result = ConverJobGlobalizationEntityToModel(newsGlobalization);
            return result;
        }

        /// <summary>
        /// 处理国际化数据
        /// </summary>
        /// <param name="model"></param>
        /// 
        /// <returns></returns>
        public Message DealJobGlobalization(M_JobGlobalization model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "新增成功";

            try
            {
                var newsId = System.Convert.ToInt32(model.JobId);
                //根据modulesId和culture，查找已存在的数据
                var oldData = _dJob.GetJobGlobalizationByJobIdAndCulture(newsId, model.Culture);

                //新增
                var newsGlobalization = new JobGlobalization()
                {
                    JobId = System.Convert.ToInt32(model.JobId),
                    Location = model.Location,
                    JobTitle = model.JobTitle,
                    Description = model.Description,
                    Content = model.Content,
                    Culture = model.Culture
                };

                message.ReturnId = _dJob.AddJobGlobalization(newsGlobalization);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "新增失败，异常：" + e.Message;
            }
            return message;
        }



        private M_JobGlobalization ConverJobGlobalizationEntityToModel(JobGlobalization newsGlobalization)
        {
            if (newsGlobalization == null) return null;

            B_Image bImage = new B_Image();

            var model = new M_JobGlobalization()
            {
                Id = newsGlobalization.Id.ToString(),
                Location = newsGlobalization.Location,
                JobTitle = newsGlobalization.JobTitle,
                Description = newsGlobalization.Description,
                Content = newsGlobalization.Content,
                Culture = newsGlobalization.Culture

            };
            return model;
        }

        #endregion

    }
}
