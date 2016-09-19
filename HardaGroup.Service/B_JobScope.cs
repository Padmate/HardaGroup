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
    public class B_JobScope
    {
        D_JobScope _dJobScope = new D_JobScope();

        public M_JobScope GetById(string id)
        {
            var jobScopeId = System.Convert.ToInt32(id);

            var jobScope = _dJobScope.GetJobScopeById(jobScopeId);
            var result = ConverEntityToModel(jobScope);
            return result;
        }

        /// <summary>
        /// 根据条件过滤数据
        /// </summary>
        /// <param name="about"></param>
        /// <returns></returns>
        public List<M_JobScope> GetByMulitCond(M_JobScope jobScope)
        {
            var searchModel = new JobScope()
            {
                TypeCode = jobScope.TypeCode

            };
            var abouts = _dJobScope.GetByMulitCondition(searchModel);
            var result = abouts.Select(a => ConverEntityToModel(a)).ToList();
            return result;
        }

        public List<M_JobScope> GetAllData()
        {
            var jobScopes = _dJobScope.GetAll();
            var result = jobScopes.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取所有的中文信息列表
        /// </summary>
        /// <returns></returns>
        public List<M_JobScopeSearch> GetAllZHCNData()
        {
            var jobScopes = _dJobScope.GetAll();

            List<M_JobScopeSearch> result = new List<M_JobScopeSearch>();
            foreach (var scope in jobScopes)
            {
                M_JobScopeSearch jobScopeSearch = new M_JobScopeSearch();
                var zhcnData = scope.JobScopeGlobalizations
                    .Where(ag => ag.Culture == Common.Globalization_Chinese)
                    .FirstOrDefault();

                jobScopeSearch.Id = scope.Id.ToString();
                jobScopeSearch.TypeCode = scope.TypeCode;
                jobScopeSearch.TypeName = zhcnData == null ? "" : zhcnData.TypeName;
                jobScopeSearch.Culture = zhcnData == null ? Common.Globalization_Chinese : zhcnData.Culture;

                result.Add(jobScopeSearch);
            }
            return result;
        }

        /// <summary>
        /// 根据culture获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<M_JobScopeSearch> GetAllCultureData(string culture)
        {
            var allDatas = this.GetAllData();
            List<M_JobScopeSearch> cultureDatas = new List<M_JobScopeSearch>();
            foreach (var jobScope in allDatas)
            {
                //根据国际化代码过滤数据，如果没有当前国际化代码的数据，则取中文数据
                var defaultGlobalizationData = jobScope.JobScopeGlobalizations.Where(ag => ag.Culture == culture).FirstOrDefault();
                if (defaultGlobalizationData == null)
                {
                    defaultGlobalizationData = jobScope.JobScopeGlobalizations.Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();
                }

                M_JobScopeSearch currentData = new M_JobScopeSearch();
                currentData.Id = jobScope.Id;
                currentData.TypeCode = jobScope.TypeCode;
                currentData.TypeName = defaultGlobalizationData.TypeName;

                cultureDatas.Add(currentData);
            }

            return cultureDatas;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="jobScope"></param>
        /// <returns></returns>
        public List<M_JobScopeSearch> GetPageData(M_JobScopeSearch jobScope)
        {
            JobScopeSearch searchModel = new JobScopeSearch()
            {
                TypeCode = jobScope.TypeCode,
                TypeName = jobScope.TypeName
            };

            var offset = jobScope.offset;
            var limit = jobScope.limit;


            var pageResult = _dJobScope.GetPageData(searchModel, offset, limit);
            var result = pageResult.Select(a => ConverSearchEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取分页数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetPageDataTotalCount(M_JobScopeSearch jobScope)
        {
            JobScopeSearch searchModel = new JobScopeSearch()
            {
                TypeCode = jobScope.TypeCode,
                TypeName = jobScope.TypeName
            };

            var totalCount = _dJobScope.GetPageDataTotalCount(searchModel);
            return totalCount;
        }

        private M_JobScope ConverEntityToModel(JobScope jobScope)
        {
            if (jobScope == null) return null;

            var model = new M_JobScope()
            {
                Id = jobScope.Id.ToString(),
                TypeCode = jobScope.TypeCode,
                Sequence = jobScope.Sequence.ToString(),
                JobScopeGlobalizations = jobScope.JobScopeGlobalizations
                                    .Select(nsg => new M_JobScopeGlobalization()
                                    {
                                        Id = nsg.Id.ToString(),
                                        TypeName = nsg.TypeName,
                                        Culture = nsg.Culture,

                                    }).ToList()

            };
            return model;
        }

        private M_JobScopeSearch ConverSearchEntityToModel(JobScopeSearch jobScope)
        {
            if (jobScope == null) return null;

            var model = new M_JobScopeSearch()
            {
                Id = jobScope.Id.ToString(),
                TypeCode = jobScope.TypeCode,
                TypeName = jobScope.TypeName,
                Culture = jobScope.Culture,
                Sequence = jobScope.Sequence.ToString(),
            };
            return model;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// 
        /// <returns></returns>
        public Message AddJobScope(M_JobScope model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "模块新增成功";

            try
            {
                //只能有唯一的TypeCode
                var search = new JobScope() { TypeCode = model.TypeCode };
                var data = _dJobScope.GetByMulitCondition(search);
                if (data.Count > 0)
                {
                    message.Success = false;
                    message.Content = "系统中已存在代码为'" + model.TypeCode + "'的数据,不能重复添加。";
                    return message;

                }

                //新增
                var jobScope = new JobScope()
                {
                    TypeCode = model.TypeCode,
                    Sequence = string.IsNullOrEmpty(model.Sequence) ? 0 : System.Convert.ToInt32(model.Sequence),
                    JobScopeGlobalizations = model.JobScopeGlobalizations
                                    .Select(nsg => new JobScopeGlobalization()
                                    {
                                        TypeName = nsg.TypeName,
                                        Culture = nsg.Culture,

                                    }).ToList()
                };

                message.ReturnId = _dJobScope.AddJobScope(jobScope);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "模块新增失败，异常：" + e.Message;
            }
            return message;
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Message EditJobScope(M_JobScope model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "模块修改成功";

            try
            {
                //只能有唯一的TypeCode
                var search = new JobScope() { TypeCode = model.TypeCode };
                var data = _dJobScope.GetByMulitCondition(search);
                if (data.Where(a => a.Id.ToString() != model.Id).Count() > 0)
                {
                    message.Success = false;
                    message.Content = "系统中已存在代码为'" + model.TypeCode + "'的数据,不能重复添加。";
                    return message;

                }

                var jobScope = new JobScope()
                {
                    TypeCode = model.TypeCode,
                    Sequence = string.IsNullOrEmpty(model.Sequence) ? 0 : System.Convert.ToInt32(model.Sequence),
                    JobScopeGlobalizations = model.JobScopeGlobalizations
                                    .Select(nsg => new JobScopeGlobalization()
                                    {
                                        JobScopeId = System.Convert.ToInt32(nsg.JobScopeId),
                                        TypeName = nsg.TypeName,
                                        Culture = nsg.Culture,

                                    }).ToList()
                };

                message.ReturnId = _dJobScope.EditJobScope(System.Convert.ToInt32(model.Id), jobScope);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "模块修改失败，异常：" + e.Message;
            }
            return message;
        }

        public Message BatchDeleteByIds(List<int> ids)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "模块删除成功";

            try
            {
                _dJobScope.BatchDeleteJobScope(ids);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "模块删除失败：" + e.Message;
            }

            return message;
        }

        /// <summary>
        /// 根据条件过滤数据
        /// </summary>
        /// <param name="about"></param>
        /// <returns></returns>
        public M_JobScopeGlobalization GetJobScopeGlobalizationByJobScopeIdAndCulture(string jobScopeId, string culture)
        {

            M_JobScopeGlobalization jobScopeGlobalization = null;
            var id = System.Convert.ToInt32(jobScopeId);
            var result = _dJobScope.GetJobScopeGlobalizationByJobScopeIdAndCulture(id, culture);

            if (result != null)
            {
                jobScopeGlobalization = new M_JobScopeGlobalization();
                jobScopeGlobalization.Id = result.Id.ToString();
                jobScopeGlobalization.TypeName = result.TypeName;
                jobScopeGlobalization.Culture = result.Culture;
                jobScopeGlobalization.JobScopeId = result.JobScopeId.ToString();

            }
            return jobScopeGlobalization;
        }

        /// <summary>
        /// 处理国际化数据
        /// </summary>
        /// <param name="model"></param>
        /// 
        /// <returns></returns>
        public Message DealJobScopeGlobalization(M_JobScopeGlobalization model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "新增成功";

            try
            {

                //新增
                var jobScopeGlobalization = new JobScopeGlobalization()
                {
                    JobScopeId = System.Convert.ToInt32(model.JobScopeId),
                    TypeName = model.TypeName,
                    Culture = model.Culture
                };

                message.ReturnId = _dJobScope.AddJobScopeGlobalization(jobScopeGlobalization);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "新增失败，异常：" + e.Message;
            }
            return message;
        }
    }
}
