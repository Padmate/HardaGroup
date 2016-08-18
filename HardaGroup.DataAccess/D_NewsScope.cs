using HardaGroup.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess
{
    public class D_NewsScope
    {
        HardaDBContext _dbContext = new HardaDBContext();

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="newsScope"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<NewsScope> GetPageData(NewsScope newsScope, int skip, int limit)
        {
            var query = _dbContext.NewsScopes.Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(newsScope.TypeCode))
                query = query.Where(a => a.TypeCode.Contains(newsScope.TypeCode));

            if (!string.IsNullOrEmpty(newsScope.TypeName))
                query = query.Where(a => a.TypeName.Contains(newsScope.TypeName));

            //根据国际化代码过滤数据
            if (!string.IsNullOrEmpty(newsScope.Culture))
                query = query.Where(a => a.Culture.Contains(newsScope.Culture));

            #endregion

            var result = query.OrderBy(a => new { a.Sequence })
            .Skip(skip)
            .Take(limit)
            .ToList();

            return result;
        }

        public int GetPageDataTotalCount(NewsScope newsScope)
        {
            var query = _dbContext.NewsScopes.Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(newsScope.TypeCode))
                query = query.Where(a => a.TypeCode.Contains(newsScope.TypeCode));

            if (!string.IsNullOrEmpty(newsScope.TypeName))
                query = query.Where(a => a.TypeName.Contains(newsScope.TypeName));

            if (!string.IsNullOrEmpty(newsScope.Culture))
                query = query.Where(a => a.Culture.Contains(newsScope.Culture));

            #endregion

            var result = query.ToList().Count();

            return result;
        }

        public List<NewsScope> GetAll()
        {
            var result = _dbContext.NewsScopes.OrderBy(a => a.Sequence).ToList();
            return result;
        }

        /// <summary>
        /// 根据ID查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NewsScope GetNewsScopeById(string id)
        {
            var newsScope = _dbContext.NewsScopes.FirstOrDefault(a => a.Id.ToString() == id);
            return newsScope;
        }

        /// <summary>
        /// 通过查询条件过滤数据
        /// </summary>
        /// <param name="about"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<NewsScope> GetByMulitCondition(NewsScope newsScope)
        {
            var query = _dbContext.NewsScopes.Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(newsScope.TypeCode))
                query = query.Where(a => a.TypeCode == newsScope.TypeCode);
            if (!string.IsNullOrEmpty(newsScope.TypeName))
                query = query.Where(a => a.TypeName == newsScope.TypeName);
            if (!string.IsNullOrEmpty(newsScope.Culture))
                query = query.Where(a => a.Culture == newsScope.Culture);
            #endregion

            var result = query.OrderBy(a => a.Sequence)
            .ToList();

            return result;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="newsScope"></param>
        /// <returns></returns>
        public string AddNewsScope(NewsScope newsScope)
        {
            _dbContext.NewsScopes.Add(newsScope);
            _dbContext.SaveChanges();
            return newsScope.Id.ToString();
        }

        public string EditNewsScope(string id, NewsScope model)
        {
            var newsScope = _dbContext.NewsScopes.FirstOrDefault(a => a.Id.ToString() == id);

            newsScope.TypeCode = model.TypeCode;
            newsScope.TypeName = model.TypeName;
            newsScope.Culture = model.Culture;
            newsScope.Sequence = model.Sequence;

            _dbContext.SaveChanges();
            return newsScope.Id.ToString();
        }


        public void DeleteNewsScope(string id)
        {
            var newsScope = _dbContext.NewsScopes.Where(i => i.Id.ToString() == id).FirstOrDefault();
            if (newsScope != null)
            {
                _dbContext.NewsScopes.Remove(newsScope);
                _dbContext.SaveChanges();
            }
        }

        public void BatchDeleteNewsScope(List<string> ids)
        {
            var newsScopes = _dbContext.NewsScopes.Where(i => ids.Contains(i.Id.ToString())).ToList();
            if (newsScopes.Count > 0)
            {
                _dbContext.NewsScopes.RemoveRange(newsScopes);
                _dbContext.SaveChanges();
            }
        }
    }
}
