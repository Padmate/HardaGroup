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
    public class B_NewsScope
    {
        D_NewsScope _dNewsScope = new D_NewsScope();

        public M_NewsScope GetById(string id)
        {
            var newsScopeId = System.Convert.ToInt32(id);

            var newsScope = _dNewsScope.GetNewsScopeById(newsScopeId);
            var result = ConverEntityToModel(newsScope);
            return result;
        }

        /// <summary>
        /// 根据条件过滤数据
        /// </summary>
        /// <param name="about"></param>
        /// <returns></returns>
        public List<M_NewsScope> GetByMulitCond(M_NewsScope newsScope)
        {
            var searchModel = new NewsScope()
            {
                TypeCode = newsScope.TypeCode

            };
            var abouts = _dNewsScope.GetByMulitCondition(searchModel);
            var result = abouts.Select(a => ConverEntityToModel(a)).ToList();
            return result;
        }

        public List<M_NewsScope> GetAllData()
        {
            var newsScopes = _dNewsScope.GetAll();
            var result = newsScopes.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取所有的中文信息列表
        /// </summary>
        /// <returns></returns>
        public List<M_NewsScopeSearch> GetAllZHCNData()
        {
            var newsScopes = _dNewsScope.GetAll();

            List<M_NewsScopeSearch> result = new List<M_NewsScopeSearch>();
            foreach(var scope in newsScopes)
            {
                M_NewsScopeSearch newsScopeSearch = new M_NewsScopeSearch();
                var zhcnData = scope.NewsScopeGlobalizations
                    .Where(ag => ag.Culture == Common.Globalization_Chinese)
                    .FirstOrDefault();

                newsScopeSearch.Id = scope.Id.ToString();
                newsScopeSearch.TypeCode = scope.TypeCode;
                newsScopeSearch.TypeName = zhcnData == null ? "" : zhcnData.TypeName;
                newsScopeSearch.Culture = zhcnData == null ? Common.Globalization_Chinese : zhcnData.Culture;

                result.Add(newsScopeSearch);
            }
            return result;
        }

        /// <summary>
        /// 根据culture获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<M_NewsScopeSearch> GetAllCultureData(string culture)
        {
            var allDatas = this.GetAllData();
            List<M_NewsScopeSearch> cultureDatas = new List<M_NewsScopeSearch>();
            foreach (var newsScope in allDatas)
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

                cultureDatas.Add(currentData);
            }

            return cultureDatas;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="newsScope"></param>
        /// <returns></returns>
        public List<M_NewsScopeSearch> GetPageData(M_NewsScopeSearch newsScope)
        {
            NewsScopeSearch searchModel = new NewsScopeSearch()
            {
                TypeCode = newsScope.TypeCode,
                TypeName = newsScope.TypeName
            };

            var offset = newsScope.offset;
            var limit = newsScope.limit;


            var pageResult = _dNewsScope.GetPageData(searchModel, offset, limit);
            var result = pageResult.Select(a => ConverSearchEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取分页数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetPageDataTotalCount(M_NewsScopeSearch newsScope)
        {
            NewsScopeSearch searchModel = new NewsScopeSearch()
            {
                TypeCode = newsScope.TypeCode,
                TypeName = newsScope.TypeName
            };

            var totalCount = _dNewsScope.GetPageDataTotalCount(searchModel);
            return totalCount;
        }

        private M_NewsScope ConverEntityToModel(NewsScope newsScope)
        {
            if (newsScope == null) return null;

            var model = new M_NewsScope()
            {
                Id = newsScope.Id.ToString(),
                TypeCode = newsScope.TypeCode,
                Sequence = newsScope.Sequence.ToString(),
                NewsScopeGlobalizations = newsScope.NewsScopeGlobalizations
                                    .Select(nsg => new M_NewsScopeGlobalization() { 
                                        Id = nsg.Id.ToString(),
                                        TypeName = nsg.TypeName,
                                        Culture = nsg.Culture,
                                       
                                    }).ToList()

            };
            return model;
        }

        private M_NewsScopeSearch ConverSearchEntityToModel(NewsScopeSearch newsScope)
        {
            if (newsScope == null) return null;

            var model = new M_NewsScopeSearch()
            {
                Id = newsScope.Id.ToString(),
                TypeCode = newsScope.TypeCode,
                TypeName = newsScope.TypeName,
                Culture = newsScope.Culture,
                Sequence = newsScope.Sequence.ToString(),
            };
            return model;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// 
        /// <returns></returns>
        public Message AddNewsScope(M_NewsScope model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "模块新增成功";

            try
            {
                //只能有唯一的TypeCode
                var search = new NewsScope() { TypeCode = model.TypeCode };
                var data = _dNewsScope.GetByMulitCondition(search);
                if (data.Count > 0)
                {
                    message.Success = false;
                    message.Content = "系统中已存在代码为'" + model.TypeCode + "'的数据,不能重复添加。";
                    return message;

                }

                //新增
                var newsScope = new NewsScope()
                {
                    TypeCode = model.TypeCode,
                    Sequence = string.IsNullOrEmpty(model.Sequence) ? 0 : System.Convert.ToInt32(model.Sequence),
                    NewsScopeGlobalizations = model.NewsScopeGlobalizations
                                    .Select(nsg => new NewsScopeGlobalization()
                                    {
                                        TypeName = nsg.TypeName,
                                        Culture = nsg.Culture,

                                    }).ToList()
                };

                message.ReturnId = _dNewsScope.AddNewsScope(newsScope);

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
        public Message EditNewsScope(M_NewsScope model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "模块修改成功";

            try
            {
                //只能有唯一的TypeCode
                var search = new NewsScope() { TypeCode = model.TypeCode};
                var data = _dNewsScope.GetByMulitCondition(search);
                if (data.Where(a => a.Id.ToString() != model.Id).Count() > 0)
                {
                    message.Success = false;
                    message.Content = "系统中已存在代码为'" + model.TypeCode + "'的数据,不能重复添加。";
                    return message;

                }

                var newsScope = new NewsScope()
                {
                    TypeCode = model.TypeCode,
                    Sequence = string.IsNullOrEmpty(model.Sequence) ? 0 : System.Convert.ToInt32(model.Sequence),
                    NewsScopeGlobalizations = model.NewsScopeGlobalizations
                                    .Select(nsg => new NewsScopeGlobalization()
                                    {
                                        NewsScopeId = System.Convert.ToInt32(nsg.NewsScopeId),
                                        TypeName = nsg.TypeName,
                                        Culture = nsg.Culture,

                                    }).ToList()
                };

                message.ReturnId = _dNewsScope.EditNewsScope(System.Convert.ToInt32(model.Id), newsScope);

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
                _dNewsScope.BatchDeleteNewsScope(ids);

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
        public M_NewsScopeGlobalization GetNewsScopeGlobalizationByNewsScopeIdAndCulture(string newsScopeId, string culture)
        {

            M_NewsScopeGlobalization newsScopeGlobalization = null;
            var id = System.Convert.ToInt32(newsScopeId);
            var result = _dNewsScope.GetNewsScopeGlobalizationByNewsScopeIdAndCulture(id, culture);

            if (result != null)
            {
                newsScopeGlobalization = new M_NewsScopeGlobalization();
                newsScopeGlobalization.Id = result.Id.ToString();
                newsScopeGlobalization.TypeName = result.TypeName;
                newsScopeGlobalization.Culture = result.Culture;
                newsScopeGlobalization.NewsScopeId = result.NewsScopeId.ToString();

            }
            return newsScopeGlobalization;
        }

        /// <summary>
        /// 处理国际化数据
        /// </summary>
        /// <param name="model"></param>
        /// 
        /// <returns></returns>
        public Message DealNewsScopeGlobalization(M_NewsScopeGlobalization model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "新增成功";

            try
            {

                //新增
                var newsScopeGlobalization = new NewsScopeGlobalization()
                {
                    NewsScopeId = System.Convert.ToInt32(model.NewsScopeId),
                    TypeName = model.TypeName,
                    Culture = model.Culture
                };

                message.ReturnId = _dNewsScope.AddNewsScopeGlobalization(newsScopeGlobalization);

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
