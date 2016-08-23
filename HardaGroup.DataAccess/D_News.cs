using HardaGroup.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess
{
    public class D_News
    {
        HardaDBContext _dbContext = new HardaDBContext();

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="news"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<News> GetPageData(News news, int skip, int limit)
        {
            var query = _dbContext.News.Include("NewsScope").Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(news.Title))
                query = query.Where(a => a.Title.Contains(news.Title));
            if (!string.IsNullOrEmpty(news.SubTitle))
                query = query.Where(a => a.SubTitle.Contains(news.SubTitle));
            if (news.NewsScopeId != null)
                query = query.Where(a => a.NewsScopeId == news.NewsScopeId);
            

            #endregion

            var result = query.OrderByDescending(a => a.Pubtime)
            .Skip(skip)
            .Take(limit)
            .ToList();

            return result;
        }

        public int GetPageDataTotalCount(News news)
        {
            var query = _dbContext.News.Where(a => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(news.Title))
                query = query.Where(a => a.Title.Contains(news.Title));
            if (!string.IsNullOrEmpty(news.SubTitle))
                query = query.Where(a => a.SubTitle.Contains(news.SubTitle));
            if (news.NewsScopeId != null)
                query = query.Where(a => a.NewsScopeId == news.NewsScopeId);
            
            #endregion

            var result = query.ToList().Count();

            return result;
        }

        /// <summary>
        /// 根据ID查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public News GetNewsById(int id)
        {
            var news = _dbContext.News.Include("NewsScope").FirstOrDefault(a => a.Id == id);
            return news;
        }


        public List<News> GetAll()
        {
            var newss = _dbContext.News.Include("NewsScope")
                .ToList();

            return newss;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public string AddNews(News news)
        {
            _dbContext.News.Add(news);
            _dbContext.SaveChanges();
            return news.Id.ToString();
        }

        public string EditNews(int id, News model)
        {
            var news = _dbContext.News.FirstOrDefault(a => a.Id == id);

            news.NewsScopeId = model.NewsScopeId;
            news.Title = model.Title;
            news.SubTitle = model.SubTitle;
            news.Content = model.Content;
            news.Description = model.Description;
            news.ModifiedDate =model.ModifiedDate;
            news.Modifier = model.Modifier;
            news.Pubtime = model.Pubtime;

            _dbContext.SaveChanges();
            return news.Id.ToString();
        }

        /// <summary>
        /// 修改图片ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public string EditImageId(int id, int imageId)
        {
            var news = _dbContext.News.FirstOrDefault(a => a.Id == id);

            news.ImageId = imageId;

            _dbContext.SaveChanges();
            return news.Id.ToString();
        }

        public void DeleteNews(int id)
        {
            var news = _dbContext.News.Where(i => i.Id == id).FirstOrDefault();
            if (news != null)
            {
                _dbContext.News.Remove(news);
                _dbContext.SaveChanges();
            }
        }

        public void BatchDeleteNews(List<int> ids)
        {
            var newss = _dbContext.News.Where(i => ids.Contains(i.Id)).ToList();
            if (newss.Count > 0)
            {
                _dbContext.News.RemoveRange(newss);
                _dbContext.SaveChanges();
            }
        }
    }
}
