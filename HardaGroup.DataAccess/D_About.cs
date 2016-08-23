using HardaGroup.Entities;
using HardaGroup.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess
{
    public class D_About
    {
        HardaDBContext _dbContext = new HardaDBContext();

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="about"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<AboutSearch> GetPageData(AboutSearch aboutSearch, int skip, int limit)
        {
            /********************************************************* 
            
            select a.*,ab.* from dbo.Abouts a 
            left join dbo.AboutGlobalizations ab 
            on a.Id = ab.AboutId AND ab.Culture ='zh-cn'
            
            **********************************************************/
            var query = from a in _dbContext.Abouts
                              join ag in _dbContext.AboutGlobalizations 
                              on new { aboutId = a.Id ,culture = Common.Globalization_Chinese}
                              equals new { aboutId = ag.AboutId,culture=ag.Culture}
                              into temp
                              from tt in temp.DefaultIfEmpty()
                              select new AboutSearch
                              {
                                  Id = a.Id,
                                  TypeCode= a.TypeCode,
                                  Sequence = a.Sequence,
                                  TypeName = tt == null?"":tt.TypeName,
                                  Content = tt == null ? "" : tt.Content,
                                  Culture = tt == null ? "" : tt.Culture,
                                   
                              };

            //var query = _dbContext.Abouts.Include("AboutGlobalizations").Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(aboutSearch.TypeName))
                query = query.Where(a => a.TypeName.Contains(aboutSearch.TypeName));
            if (!string.IsNullOrEmpty(aboutSearch.TypeCode))
                query = query.Where(a => a.TypeCode.Contains(aboutSearch.TypeCode));
            #endregion

            var result = query.OrderBy(a => a.Sequence)
            .Skip(skip)
            .Take(limit)
            .ToList();

            return result;
        }

        public int GetPageDataTotalCount(AboutSearch aboutSearch)
        {
            /********************************************************* 
            
            select a.*,ab.* from dbo.Abouts a 
            left join dbo.AboutGlobalizations ab 
            on a.Id = ab.AboutId AND ab.Culture ='zh-cn'
            
            **********************************************************/
            var query = from a in _dbContext.Abouts
                        join ag in _dbContext.AboutGlobalizations
                        on new { aboutId = a.Id, culture = Common.Globalization_Chinese }
                        equals new { aboutId = ag.AboutId, culture = ag.Culture }
                        into temp
                        from tt in temp.DefaultIfEmpty()
                        select new AboutSearch
                        {
                            TypeCode = a.TypeCode,
                            Sequence = a.Sequence,
                            TypeName = tt == null ? "" : tt.TypeName,
                            Content = tt == null ? "" : tt.Content,
                            Culture = tt == null ? "" : tt.Culture,

                        };
            //var query = _dbContext.Abouts.Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(aboutSearch.TypeName))
                query = query.Where(a => a.TypeName.Contains(aboutSearch.TypeName));
            if (!string.IsNullOrEmpty(aboutSearch.TypeCode))
                query = query.Where(a => a.TypeCode.Contains(aboutSearch.TypeCode));
            #endregion

            var result = query.ToList().Count();

            return result;
        }


        /// <summary>
        /// 通过查询条件过滤数据
        /// </summary>
        /// <param name="about"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<About> GetByMulitCondition(About about)
        {
            var query = _dbContext.Abouts.Include("AboutGlobalizations").Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(about.TypeCode))
                query = query.Where(a => a.TypeCode == about.TypeCode);
            //if (!string.IsNullOrEmpty(about.TypeName))
            //    query = query.Where(a => a.TypeName == about.TypeName);
            //if (!string.IsNullOrEmpty(about.Culture))
            //    query = query.Where(a => a.Culture == about.Culture);
            #endregion

            var result = query.OrderBy(a => a.Sequence)
            .ToList();

            return result;
        }

        public AboutGlobalization GetAboutGlobalizationByAboutIdAndCulture(int aboutId,string culture)
        {
            var result = _dbContext.AboutGlobalizations.Where(ag=>ag.AboutId == aboutId && ag.Culture == culture).FirstOrDefault();


            return result;
        }

        /// <summary>
        /// 根据ID查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public About GetAboutById(int id)
        {
            var about = _dbContext.Abouts.Include("AboutGlobalizations").FirstOrDefault(a => a.Id == id);
           
            return about;
        }

        
        public List<About> GetAll()
        {
            var abouts = _dbContext.Abouts.Include("AboutGlobalizations")
                .ToList();

            return abouts;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="about"></param>
        /// <returns></returns>
        public int AddAbout(About about)
        {
            _dbContext.Abouts.Add(about);
            _dbContext.SaveChanges();
            return about.Id;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="about"></param>
        /// <returns></returns>
        public int AddAboutGlobalization(AboutGlobalization aboutGlobalization)
        {
            var existData = _dbContext.AboutGlobalizations
                .Where(ag => ag.AboutId == aboutGlobalization.AboutId && ag.Culture == aboutGlobalization.Culture);
            if(existData.Count() >0)
            {
                _dbContext.AboutGlobalizations.RemoveRange(existData);
            }
            //重新添加
            _dbContext.AboutGlobalizations.Add(aboutGlobalization);

            _dbContext.SaveChanges();
            return aboutGlobalization.Id;
        }

        public int EditAbout(int id, About model)
        {
            var about = _dbContext.Abouts.FirstOrDefault(a => a.Id == id);

            about.TypeCode = model.TypeCode;
            about.Sequence = model.Sequence;

            var culture = model.AboutGlobalizations.First().Culture;
            //先删除原来子数据
            var ag = _dbContext.AboutGlobalizations
                .Where(a => a.AboutId == about.Id && a.Culture == culture);
            _dbContext.AboutGlobalizations.RemoveRange(ag);

            //重新添加
            _dbContext.AboutGlobalizations.AddRange(model.AboutGlobalizations);

            _dbContext.SaveChanges();
            return about.Id;
        }


        public void DeleteAbout(int id)
        {
            var about = _dbContext.Abouts.Where(i => i.Id == id).FirstOrDefault();
            if (about != null)
            {
                _dbContext.Abouts.Remove(about);
                _dbContext.SaveChanges();
            }
        }

        public void BatchDeleteAbout(List<int> ids)
        {
            var abouts = _dbContext.Abouts.Where(i => ids.Contains(i.Id)).ToList();
            if (abouts.Count > 0)
            {
                _dbContext.Abouts.RemoveRange(abouts);
                _dbContext.SaveChanges();
            }
        }
    }
}
