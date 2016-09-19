using HardaGroup.Entities;
using HardaGroup.Utility;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess
{
    public class D_Job
    {
        HardaDBContext _dbContext = new HardaDBContext();

        public List<string> GetLocationsByCulture(string culture)
        {
            var sql = @"select Location from dbo.JobGlobalizations where Culture=@culture group by Location";

            var args = new DbParameter[] {
                  new SqlParameter {ParameterName = "culture", Value = culture}};
            var locations = _dbContext.Database.SqlQuery<string>(sql, args);

            return locations.ToList();

        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="job"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<JobSearch> GetPageData(JobSearch jobSearch, int skip, int limit)
        {
            //根据国际化代码获取数据，
            var culture = jobSearch.Culture;
            if (string.IsNullOrEmpty(culture))
            {
                culture = Common.Globalization_Chinese;
            }
            var query = from a in _dbContext.Jobs
                        join ag in _dbContext.JobGlobalizations
                        on new { jobId = a.Id, culture = culture }
                        equals new { jobId = ag.JobId, culture = ag.Culture }
                        into temp
                        from tt in temp.DefaultIfEmpty()
                        //左链接国际化代码为中文的数据
                        join zhcndata in _dbContext.JobGlobalizations
                        on new { jobId = a.Id, culture = Common.Globalization_Chinese }
                        equals new { jobId = zhcndata.JobId, culture = zhcndata.Culture }
                        into tempzhcndata
                        from ttzhcndata in tempzhcndata.DefaultIfEmpty()

                        select new JobSearch
                        {
                            Id = a.Id,
                            JobScopeId = a.JobScopeId,
                            Pubtime = a.Pubtime,
                            CreateDate = a.CreateDate,
                            Creator = a.Creator,
                            ModifiedDate = a.ModifiedDate,
                            Modifier = a.Modifier,
                            URLId = a.URLId,
                            IsHot = a.IsHot,
                            //如果当前culture获取不到数据，则默认取中文数据
                            Location = tt == null ? ttzhcndata.Location : tt.Location,
                            JobTitle = tt == null ? ttzhcndata.JobTitle : tt.JobTitle,
                            Content = tt == null ? ttzhcndata.Content : tt.Content,
                            Culture = tt == null ? ttzhcndata.Culture : tt.Culture,
                            Description = tt == null ? ttzhcndata.Description : tt.Description,

                        };

            #region　条件过滤
            if (!string.IsNullOrEmpty(jobSearch.Location))
                query = query.Where(a => a.Location.Contains(jobSearch.Location));
            if (!string.IsNullOrEmpty(jobSearch.JobTitle))
                query = query.Where(a => a.JobTitle.Contains(jobSearch.JobTitle));
            if (jobSearch.JobScopeId != null)
                query = query.Where(a => a.JobScopeId == jobSearch.JobScopeId);
            if (!string.IsNullOrEmpty(jobSearch.IsHotSearch))
            {
                var isHot = jobSearch.IsHotSearch == Common.Yes ? true : false;
                query = query.Where(a => a.IsHot == isHot);
            }
            
            #endregion

            var result = query.OrderByDescending(a => a.Pubtime)
            .Skip(skip)
            .Take(limit)
            .ToList();

            return result;

        }

        public int GetPageDataTotalCount(JobSearch jobSearch)
        {
            var query = from a in _dbContext.Jobs
                        join ag in _dbContext.JobGlobalizations
                        on new { jobId = a.Id, culture = Common.Globalization_Chinese }
                        equals new { jobId = ag.JobId, culture = ag.Culture }
                        into temp
                        from tt in temp.DefaultIfEmpty()
                        select new JobSearch
                        {
                            Id = a.Id,
                            JobScopeId = a.JobScopeId,
                            Pubtime = a.Pubtime,
                            CreateDate = a.CreateDate,
                            Creator = a.Creator,
                            ModifiedDate = a.ModifiedDate,
                            Modifier = a.Modifier,
                            URLId = a.URLId,
                            IsHot = a.IsHot,
                            Location = tt == null ? "" : tt.Location,
                            JobTitle = tt == null ? "" : tt.JobTitle,
                            Content = tt == null ? "" : tt.Content,
                            Culture = tt == null ? "" : tt.Culture,
                            Description = tt == null ? "" : tt.Description,

                        };

            #region　条件过滤
            if (!string.IsNullOrEmpty(jobSearch.Location))
                query = query.Where(a => a.Location.Contains(jobSearch.Location));
            if (!string.IsNullOrEmpty(jobSearch.JobTitle))
                query = query.Where(a => a.JobTitle.Contains(jobSearch.JobTitle));
            if (jobSearch.JobScopeId != null)
                query = query.Where(a => a.JobScopeId == jobSearch.JobScopeId);
            if (!string.IsNullOrEmpty(jobSearch.IsHotSearch))
            {
                var isHot = jobSearch.IsHotSearch == Common.Yes ? true : false;
                query = query.Where(a => a.IsHot == isHot);
            }
            
            #endregion

            var result = query.ToList().Count();

            return result;
        }

        /// <summary>
        /// 根据ID查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Job GetJobById(int id)
        {
            var job = _dbContext.Jobs.Include("JobScope").Include("JobGlobalizations").FirstOrDefault(a => a.Id == id);
            return job;
        }

        /// <summary>
        /// 根据id找找上一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Job GetPreviousDataByURLId(string jobUrlId)
        {
            var currentData = _dbContext.Jobs.Include("JobGlobalizations").FirstOrDefault(a => a.URLId == jobUrlId);
            var previousData = _dbContext.Jobs
                .Where(a => a.Pubtime > currentData.Pubtime)
                .OrderBy(a => a.Pubtime)
                .FirstOrDefault();

            return previousData;
        }

        /// <summary>
        /// 根据id查找下一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Job GetNextDataByURLId(string jobUrlId)
        {
            var currentData = _dbContext.Jobs.Include("JobGlobalizations").FirstOrDefault(a => a.URLId == jobUrlId);
            var nextData = _dbContext.Jobs
                .Where(a => a.Pubtime < currentData.Pubtime)
                .OrderByDescending(a => a.Pubtime)
                .FirstOrDefault();

            return nextData;
        }

        public Job GetJobByJobUrlId(string jobUrlId)
        {
            var job = _dbContext.Jobs.Include("JobScope")
                .Include("JobGlobalizations")
                .FirstOrDefault(a => a.URLId == jobUrlId);
            return job;
        }

        public List<Job> GetAll()
        {
            var jobs = _dbContext.Jobs.Include("JobScope")
                .ToList();

            return jobs;
        }


        /// <summary>
        /// 获取所有热门职位
        /// </summary>
        /// <returns></returns>
        public List<Job> GetAllHotJob()
        {
            var result = _dbContext.Jobs.Include("JobScope").Include("JobGlobalizations")
                .Where(a => a.IsHot == true)
                .OrderByDescending(a => a.Pubtime)
                .ToList();
            return result;
        }

        public List<Job> GetByMulitCondition(Job searchModel)
        {
            var query = _dbContext.Jobs.Include("JobGlobalizations").Where(n => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(searchModel.URLId))
                query = query.Where(a => a.URLId == searchModel.URLId);
            #endregion

            var result = query.OrderByDescending(a => a.Pubtime)
            .ToList();

            return result;
        }



        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public int AddJob(Job job)
        {
            _dbContext.Jobs.Add(job);
            _dbContext.SaveChanges();
            return job.Id;
        }

        public int EditJob(int id, Job model)
        {
            var job = _dbContext.Jobs.FirstOrDefault(a => a.Id == id);

            job.JobScopeId = model.JobScopeId;
            job.ModifiedDate = model.ModifiedDate;
            job.Modifier = model.Modifier;
            job.Pubtime = model.Pubtime;
            job.URLId = model.URLId;
            job.IsHot = model.IsHot;

            var culture = model.JobGlobalizations.First().Culture;
            //先删除原来子数据
            var ag = _dbContext.JobGlobalizations
                .Where(a => a.JobId == job.Id && a.Culture == culture);

            _dbContext.JobGlobalizations.RemoveRange(ag);

            _dbContext.JobGlobalizations.AddRange(model.JobGlobalizations);

            _dbContext.SaveChanges();
            return job.Id;
        }


        public void DeleteJob(int id)
        {
            var job = _dbContext.Jobs.Where(i => i.Id == id).FirstOrDefault();
            if (job != null)
            {
                _dbContext.Jobs.Remove(job);
                _dbContext.SaveChanges();
            }
        }

        public void BatchDeleteJob(List<int> ids)
        {
            var jobs = _dbContext.Jobs.Where(i => ids.Contains(i.Id)).ToList();
            if (jobs.Count > 0)
            {
                _dbContext.Jobs.RemoveRange(jobs);
                _dbContext.SaveChanges();
            }
        }

        #region JobGlobalization

        public JobGlobalization GetJobGlobalizationById(int jobGlobalizationId)
        {
            var jobGlobalization = _dbContext.JobGlobalizations.FirstOrDefault(a => a.Id == jobGlobalizationId);
            return jobGlobalization;
        }

        public List<JobGlobalization> GetJobGlobalizationByJobId(int jobId)
        {
            var jobGlobalization = _dbContext.JobGlobalizations.Where(a => a.JobId == jobId).ToList();
            return jobGlobalization;
        }

       

        public JobGlobalization GetJobGlobalizationByJobIdAndCulture(int jobId, string culture)
        {
            var result = _dbContext.JobGlobalizations.Where(ag => ag.JobId == jobId && ag.Culture == culture).FirstOrDefault();


            return result;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="about"></param>
        /// <returns></returns>
        public int AddJobGlobalization(JobGlobalization jobGlobalization)
        {
            var existData = _dbContext.JobGlobalizations
                .Where(ag => ag.JobId == jobGlobalization.JobId && ag.Culture == jobGlobalization.Culture);
            if (existData.Count() > 0)
            {
                _dbContext.JobGlobalizations.RemoveRange(existData);
            }
            //重新添加
            _dbContext.JobGlobalizations.Add(jobGlobalization);

            _dbContext.SaveChanges();
            return jobGlobalization.Id;
        }
        #endregion
    }
}
