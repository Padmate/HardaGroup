using HardaGroup.Entities;
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
        public List<About> GetPageData(About about, int skip, int limit)
        {
            var query = _dbContext.Abouts.Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(about.TypeName))
                query = query.Where(a => a.TypeName.Contains(about.TypeName));

            #endregion

            var result = query.OrderBy(a => a.Sequence)
            .Skip(skip)
            .Take(limit)
            .ToList();

            return result;
        }

        public int GetPageDataTotalCount(About about)
        {
            var query = _dbContext.Abouts.Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(about.TypeName))
                query = query.Where(a => a.TypeName.Contains(about.TypeName));

            #endregion

            var result = query.ToList().Count();

            return result;
        }

        /// <summary>
        /// 根据ID查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public About GetAboutById(int id)
        {
            var about = _dbContext.Abouts.FirstOrDefault(a => a.Id == id);
            return about;
        }


        public List<About> GetAll()
        {
            var abouts = _dbContext.Abouts
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

        public int EditAbout(int id, About model)
        {
            var about = _dbContext.Abouts.FirstOrDefault(a => a.Id == id);

            about.TypeCode = model.TypeCode;
            about.TypeName = model.TypeName;
            about.Sequence = model.Sequence;
            about.Content = model.Content;
            about.Culture = model.Culture;
            about.Sequence = model.Sequence;

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
