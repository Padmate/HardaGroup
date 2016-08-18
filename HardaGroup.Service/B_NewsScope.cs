using HardaGroup.DataAccess;
using HardaGroup.Entities;
using HardaGroup.Models;
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
            var newsScope = _dNewsScope.GetNewsScopeById(id);
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
                TypeCode = newsScope.TypeCode,
                TypeName = newsScope.TypeName,
                Culture = newsScope.Culture

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
        /// 获取分页数据
        /// </summary>
        /// <param name="newsScope"></param>
        /// <returns></returns>
        public List<M_NewsScope> GetPageData(M_NewsScope newsScope)
        {
            NewsScope searchModel = new NewsScope()
            {
                TypeCode = newsScope.TypeCode,
                Culture = newsScope.Culture
            };

            var offset = newsScope.offset;
            var limit = newsScope.limit;


            var pageResult = _dNewsScope.GetPageData(searchModel, offset, limit);
            var result = pageResult.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取分页数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetPageDataTotalCount(M_NewsScope newsScope)
        {
            NewsScope searchModel = new NewsScope()
            {
                TypeCode = newsScope.TypeCode,
                Culture = newsScope.Culture
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
                //在当前国际化语言中只能有唯一的TypeCode
                var search = new NewsScope() { TypeCode = model.TypeCode, Culture = model.Culture };
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
                    TypeName = model.TypeName,
                    Culture = model.Culture,
                    Sequence = string.IsNullOrEmpty(model.Sequence) ? 0 : System.Convert.ToInt32(model.Sequence),
                };

                message.ReturnStrId = _dNewsScope.AddNewsScope(newsScope);

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
                //在当前国际化语言中只能有唯一的TypeCode
                var search = new NewsScope() { TypeCode = model.TypeCode, Culture = model.Culture };
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
                    TypeName = model.TypeName,
                    Culture = model.Culture,
                    Sequence = string.IsNullOrEmpty(model.Sequence) ? 0 : System.Convert.ToInt32(model.Sequence),
                };

                message.ReturnStrId = _dNewsScope.EditNewsScope(model.Id, newsScope);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "模块修改失败，异常：" + e.Message;
            }
            return message;
        }

        public Message BatchDeleteByIds(List<string> ids)
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
    }
}
