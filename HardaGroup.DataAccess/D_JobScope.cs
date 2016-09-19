using HardaGroup.Entities;
using HardaGroup.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess
{
    public class D_JobScope
    {
        HardaDBContext _dbContext = new HardaDBContext();

        public JobScopeGlobalization GetJobScopeGlobalizationByJobScopeIdAndCulture(int jobScopeId, string culture)
        {
            var result = _dbContext.JobScopeGlobalizations.Where(ag => ag.JobScopeId == jobScopeId && ag.Culture == culture).FirstOrDefault();


            return result;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="jobScope"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<JobScopeSearch> GetPageData(JobScopeSearch jobScopeSearch, int skip, int limit)
        {

            var query = from ns in _dbContext.JobScopes
                        join nsg in _dbContext.JobScopeGlobalizations
                        on new { jobScopeId = ns.Id, culture = Common.Globalization_Chinese }
                        equals new { jobScopeId = nsg.JobScopeId, culture = nsg.Culture }
                        into temp
                        from tt in temp.DefaultIfEmpty()
                        select new JobScopeSearch
                        {
                            Id = ns.Id,
                            TypeCode = ns.TypeCode,
                            Sequence = ns.Sequence,
                            TypeName = tt == null ? "" : tt.TypeName,
                            Culture = tt == null ? "" : tt.Culture,

                        };

            //var query = _dbContext.Abouts.Include("AboutGlobalizations").Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(jobScopeSearch.TypeName))
                query = query.Where(a => a.TypeName.Contains(jobScopeSearch.TypeName));
            if (!string.IsNullOrEmpty(jobScopeSearch.TypeCode))
                query = query.Where(a => a.TypeCode.Contains(jobScopeSearch.TypeCode));
            #endregion

            var result = query.OrderBy(a => a.Sequence)
            .Skip(skip)
            .Take(limit)
            .ToList();

            return result;


        }

        public int GetPageDataTotalCount(JobScopeSearch jobScopeSearch)
        {

            var query = from ns in _dbContext.JobScopes
                        join nsg in _dbContext.JobScopeGlobalizations
                        on new { jobScopeId = ns.Id, culture = Common.Globalization_Chinese }
                        equals new { jobScopeId = nsg.JobScopeId, culture = nsg.Culture }
                        into temp
                        from tt in temp.DefaultIfEmpty()
                        select new JobScopeSearch
                        {
                            Id = ns.Id,
                            TypeCode = ns.TypeCode,
                            Sequence = ns.Sequence,
                            TypeName = tt == null ? "" : tt.TypeName,
                            Culture = tt == null ? "" : tt.Culture,

                        };

            //var query = _dbContext.Abouts.Include("AboutGlobalizations").Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(jobScopeSearch.TypeName))
                query = query.Where(a => a.TypeName.Contains(jobScopeSearch.TypeName));
            if (!string.IsNullOrEmpty(jobScopeSearch.TypeCode))
                query = query.Where(a => a.TypeCode.Contains(jobScopeSearch.TypeCode));
            #endregion

            var result = query.ToList().Count();

            return result;
        }

        public List<JobScope> GetAll()
        {
            var result = _dbContext.JobScopes.OrderBy(a => a.Sequence).ToList();
            return result;
        }



        /// <summary>
        /// 根据ID查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JobScope GetJobScopeById(int id)
        {
            var jobScope = _dbContext.JobScopes.Include("JobScopeGlobalizations").FirstOrDefault(a => a.Id == id);
            return jobScope;
        }

        /// <summary>
        /// 通过查询条件过滤数据
        /// </summary>
        /// <param name="about"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<JobScope> GetByMulitCondition(JobScope jobScope)
        {
            var query = _dbContext.JobScopes.Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(jobScope.TypeCode))
                query = query.Where(a => a.TypeCode == jobScope.TypeCode);
            //if (!string.IsNullOrEmpty(jobScope.TypeName))
            //    query = query.Where(a => a.TypeName == jobScope.TypeName);
            //if (!string.IsNullOrEmpty(jobScope.Culture))
            //    query = query.Where(a => a.Culture == jobScope.Culture);
            #endregion

            var result = query.OrderBy(a => a.Sequence)
            .ToList();

            return result;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="jobScope"></param>
        /// <returns></returns>
        public int AddJobScope(JobScope jobScope)
        {
            _dbContext.JobScopes.Add(jobScope);
            _dbContext.SaveChanges();
            return jobScope.Id;
        }

        public int EditJobScope(int id, JobScope model)
        {
            var jobScope = _dbContext.JobScopes.FirstOrDefault(a => a.Id == id);

            jobScope.TypeCode = model.TypeCode;
            jobScope.Sequence = model.Sequence;

            var culture = model.JobScopeGlobalizations.First().Culture;
            //先删除原来子数据
            var ag = _dbContext.JobScopeGlobalizations
                .Where(a => a.JobScopeId == jobScope.Id && a.Culture == culture);
            _dbContext.JobScopeGlobalizations.RemoveRange(ag);

            //重新添加
            _dbContext.JobScopeGlobalizations.AddRange(model.JobScopeGlobalizations);

            _dbContext.SaveChanges();
            return jobScope.Id;
        }


        public void DeleteJobScope(int id)
        {
            var jobScope = _dbContext.JobScopes.Where(i => i.Id == id).FirstOrDefault();
            if (jobScope != null)
            {
                _dbContext.JobScopes.Remove(jobScope);
                _dbContext.SaveChanges();
            }
        }

        public void BatchDeleteJobScope(List<int> ids)
        {
            var jobScopes = _dbContext.JobScopes.Where(i => ids.Contains(i.Id)).ToList();
            if (jobScopes.Count > 0)
            {
                _dbContext.JobScopes.RemoveRange(jobScopes);
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="about"></param>
        /// <returns></returns>
        public int AddJobScopeGlobalization(JobScopeGlobalization jobScopeGlobalization)
        {
            var existData = _dbContext.JobScopeGlobalizations
                .Where(ag => ag.JobScopeId == jobScopeGlobalization.JobScopeId && ag.Culture == jobScopeGlobalization.Culture);
            if (existData.Count() > 0)
            {
                _dbContext.JobScopeGlobalizations.RemoveRange(existData);
            }
            //重新添加
            _dbContext.JobScopeGlobalizations.Add(jobScopeGlobalization);

            _dbContext.SaveChanges();
            return jobScopeGlobalization.Id;
        }
    }
}
