using HardaGroup.Entities;
using HardaGroup.Utility;
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

        public NewsScopeGlobalization GetNewsScopeGlobalizationByNewsScopeIdAndCulture(int newsScopeId, string culture)
        {
            var result = _dbContext.NewsScopeGlobalizations.Where(ag => ag.NewsScopeId == newsScopeId && ag.Culture == culture).FirstOrDefault();


            return result;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="newsScope"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<NewsScopeSearch> GetPageData(NewsScopeSearch newsScopeSearch, int skip, int limit)
        {
            /********************************************************* 
            
            select a.*,ab.* from dbo.Abouts a 
            left join dbo.AboutGlobalizations ab 
            on a.Id = ab.AboutId AND ab.Culture ='zh-cn'
            
            **********************************************************/
            var query = from ns in _dbContext.NewsScopes
                        join nsg in _dbContext.NewsScopeGlobalizations
                        on new { newsScopeId = ns.Id, culture = Common.Globalization_Chinese }
                        equals new { newsScopeId = nsg.NewsScopeId, culture = nsg.Culture }
                        into temp
                        from tt in temp.DefaultIfEmpty()
                        select new NewsScopeSearch
                        {
                            Id = ns.Id,
                            TypeCode = ns.TypeCode,
                            Sequence = ns.Sequence,
                            TypeName = tt == null ? "" : tt.TypeName,
                            Culture = tt == null ? "" : tt.Culture,

                        };

            //var query = _dbContext.Abouts.Include("AboutGlobalizations").Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(newsScopeSearch.TypeName))
                query = query.Where(a => a.TypeName.Contains(newsScopeSearch.TypeName));
            if (!string.IsNullOrEmpty(newsScopeSearch.TypeCode))
                query = query.Where(a => a.TypeCode.Contains(newsScopeSearch.TypeCode));
            #endregion

            var result = query.OrderBy(a => a.Sequence)
            .Skip(skip)
            .Take(limit)
            .ToList();

            return result;

           
        }

        public int GetPageDataTotalCount(NewsScopeSearch newsScopeSearch)
        {
            /********************************************************* 
            
            select a.*,ab.* from dbo.Abouts a 
            left join dbo.AboutGlobalizations ab 
            on a.Id = ab.AboutId AND ab.Culture ='zh-cn'
            
            **********************************************************/
            var query = from ns in _dbContext.NewsScopes
                        join nsg in _dbContext.NewsScopeGlobalizations
                        on new { newsScopeId = ns.Id, culture = Common.Globalization_Chinese }
                        equals new { newsScopeId = nsg.NewsScopeId, culture = nsg.Culture }
                        into temp
                        from tt in temp.DefaultIfEmpty()
                        select new NewsScopeSearch
                        {
                            Id = ns.Id,
                            TypeCode = ns.TypeCode,
                            Sequence = ns.Sequence,
                            TypeName = tt == null ? "" : tt.TypeName,
                            Culture = tt == null ? "" : tt.Culture,

                        };

            //var query = _dbContext.Abouts.Include("AboutGlobalizations").Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(newsScopeSearch.TypeName))
                query = query.Where(a => a.TypeName.Contains(newsScopeSearch.TypeName));
            if (!string.IsNullOrEmpty(newsScopeSearch.TypeCode))
                query = query.Where(a => a.TypeCode.Contains(newsScopeSearch.TypeCode));
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
        public NewsScope GetNewsScopeById(int id)
        {
            var newsScope = _dbContext.NewsScopes.Include("NewsScopeGlobalizations").FirstOrDefault(a => a.Id == id);
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
            //if (!string.IsNullOrEmpty(newsScope.TypeName))
            //    query = query.Where(a => a.TypeName == newsScope.TypeName);
            //if (!string.IsNullOrEmpty(newsScope.Culture))
            //    query = query.Where(a => a.Culture == newsScope.Culture);
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
        public int AddNewsScope(NewsScope newsScope)
        {
            _dbContext.NewsScopes.Add(newsScope);
            _dbContext.SaveChanges();
            return newsScope.Id;
        }

        public int EditNewsScope(int id, NewsScope model)
        {
            var newsScope = _dbContext.NewsScopes.FirstOrDefault(a => a.Id == id);

            newsScope.TypeCode = model.TypeCode;
            newsScope.Sequence = model.Sequence;

            var culture = model.NewsScopeGlobalizations.First().Culture;
            //先删除原来子数据
            var ag = _dbContext.NewsScopeGlobalizations
                .Where(a => a.NewsScopeId == newsScope.Id && a.Culture == culture);
            _dbContext.NewsScopeGlobalizations.RemoveRange(ag);

            //重新添加
            _dbContext.NewsScopeGlobalizations.AddRange(model.NewsScopeGlobalizations);

            _dbContext.SaveChanges();
            return newsScope.Id;
        }


        public void DeleteNewsScope(int id)
        {
            var newsScope = _dbContext.NewsScopes.Where(i => i.Id == id).FirstOrDefault();
            if (newsScope != null)
            {
                _dbContext.NewsScopes.Remove(newsScope);
                _dbContext.SaveChanges();
            }
        }

        public void BatchDeleteNewsScope(List<int> ids)
        {
            var newsScopes = _dbContext.NewsScopes.Where(i => ids.Contains(i.Id)).ToList();
            if (newsScopes.Count > 0)
            {
                _dbContext.NewsScopes.RemoveRange(newsScopes);
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="about"></param>
        /// <returns></returns>
        public int AddNewsScopeGlobalization(NewsScopeGlobalization newsScopeGlobalization)
        {
            var existData = _dbContext.NewsScopeGlobalizations
                .Where(ag => ag.NewsScopeId == newsScopeGlobalization.NewsScopeId && ag.Culture == newsScopeGlobalization.Culture);
            if (existData.Count() > 0)
            {
                _dbContext.NewsScopeGlobalizations.RemoveRange(existData);
            }
            //重新添加
            _dbContext.NewsScopeGlobalizations.Add(newsScopeGlobalization);

            _dbContext.SaveChanges();
            return newsScopeGlobalization.Id;
        }
    }
}
