using HardaGroup.Entities;
using HardaGroup.Utility;
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
        public List<NewsSearch> GetPageData(NewsSearch newsSearch, int skip, int limit)
        {
            //根据国际化代码获取数据，
            var culture = newsSearch.Culture;
            if(string.IsNullOrEmpty(culture))
            {
                culture = Common.Globalization_Chinese;
            }
            var query = from a in _dbContext.News
                        join ag in _dbContext.NewsGlobalizations
                        on new { newsId = a.Id, culture = culture }
                        equals new { newsId = ag.NewsId, culture = ag.Culture }
                        into temp
                        from tt in temp.DefaultIfEmpty()
                        //左链接国际化代码为中文的数据
                        join zhcndata in _dbContext.NewsGlobalizations
                        on new { newsId = a.Id, culture = Common.Globalization_Chinese }
                        equals new { newsId = zhcndata.NewsId, culture = zhcndata.Culture }
                        into tempzhcndata
                        from ttzhcndata in tempzhcndata.DefaultIfEmpty()

                        select new NewsSearch
                        {
                            Id = a.Id,
                            NewsScopeId = a.NewsScopeId,
                            Pubtime = a.Pubtime,
                            CreateDate = a.CreateDate,
                            Creator = a.Creator,
                            ModifiedDate = a.ModifiedDate,
                            Modifier = a.Modifier,
                            NewsURLId = a.NewsURLId,
                            //如果当前culture获取不到数据，则默认取中文数据
                            ImageId = tt == null ? ttzhcndata.ImageId : tt.ImageId,
                            Title = tt == null ? ttzhcndata.Title : tt.Title,
                            SubTitle = tt == null ? ttzhcndata.SubTitle : tt.SubTitle,
                            Content = tt == null ? ttzhcndata.Content : tt.Content,
                            Culture = tt == null ? ttzhcndata.Culture : tt.Culture,
                            Description = tt == null ? ttzhcndata.Description : tt.Description,
                            
                        };

            #region　条件过滤
            if (!string.IsNullOrEmpty(newsSearch.Title))
                query = query.Where(a => a.Title.Contains(newsSearch.Title));
            if (!string.IsNullOrEmpty(newsSearch.SubTitle))
                query = query.Where(a => a.SubTitle.Contains(newsSearch.SubTitle));
            if (newsSearch.NewsScopeId != null)
                query = query.Where(a => a.NewsScopeId == newsSearch.NewsScopeId);
            #endregion

            var result = query.OrderByDescending(a => a.Pubtime)
            .Skip(skip)
            .Take(limit)
            .ToList();

            return result;

        }

        public int GetPageDataTotalCount(NewsSearch newsSearch)
        {
            var query = from a in _dbContext.News
                        join ag in _dbContext.NewsGlobalizations
                        on new { newsId = a.Id, culture = Common.Globalization_Chinese }
                        equals new { newsId = ag.NewsId, culture = ag.Culture }
                        into temp
                        from tt in temp.DefaultIfEmpty()
                        select new NewsSearch
                        {
                            Id = a.Id,
                            NewsScopeId = a.NewsScopeId,
                            Pubtime = a.Pubtime,
                            CreateDate = a.CreateDate,
                            Creator = a.Creator,
                            ModifiedDate = a.ModifiedDate,
                            Modifier = a.Modifier,
                            NewsURLId = a.NewsURLId,
                            ImageId = tt == null ? null : tt.ImageId,
                            Title = tt == null ? "" : tt.Title,
                            SubTitle = tt == null ? "" : tt.SubTitle,
                            Content = tt == null ? "" : tt.Content,
                            Culture = tt == null ? "" : tt.Culture,
                            Description = tt == null ? "" : tt.Description,

                        };

            #region　条件过滤
            if (!string.IsNullOrEmpty(newsSearch.Title))
                query = query.Where(a => a.Title.Contains(newsSearch.Title));
            if (!string.IsNullOrEmpty(newsSearch.SubTitle))
                query = query.Where(a => a.SubTitle.Contains(newsSearch.SubTitle));
            if (newsSearch.NewsScopeId != null)
                query = query.Where(a => a.NewsScopeId == newsSearch.NewsScopeId);
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
            var news = _dbContext.News.Include("NewsScope").Include("NewsGlobalizations").FirstOrDefault(a => a.Id == id);
            return news;
        }

        /// <summary>
        /// 根据id找找上一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public News GetPreviousDataByNewsURLId(string newsUrlId)
        {
            var currentData = _dbContext.News.Include("NewsGlobalizations").FirstOrDefault(a => a.NewsURLId == newsUrlId);
            var previousData = _dbContext.News
                .Where(a => a.NewsScopeId == currentData.NewsScopeId && a.Pubtime > currentData.Pubtime)
                .OrderBy(a => a.Pubtime)
                .FirstOrDefault();

            return previousData;
        }

        /// <summary>
        /// 根据id查找下一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public News GetNextDataByNewsURLId(string newsUrlId)
        {
            var currentData = _dbContext.News.Include("NewsGlobalizations").FirstOrDefault(a => a.NewsURLId == newsUrlId);
            var nextData = _dbContext.News
                .Where(a => a.NewsScopeId == currentData.NewsScopeId && a.Pubtime < currentData.Pubtime)
                .OrderByDescending(a => a.Pubtime)
                .FirstOrDefault();

            return nextData;
        }

        public News GetNewsByNewsUrlId(string newsUrlId)
        {
            var news = _dbContext.News.Include("NewsScope")
                .Include("NewsGlobalizations")
                .FirstOrDefault(a => a.NewsURLId == newsUrlId);
            return news;
        }

        public List<News> GetAll()
        {
            var newss = _dbContext.News.Include("NewsScope")
                .ToList();

            return newss;
        }

        public List<News> GetByMulitCondition(News searchModel)
        {
            var query = _dbContext.News.Include("NewsGlobalizations").Where(n=>1==1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(searchModel.NewsURLId))
                query = query.Where(a => a.NewsURLId == searchModel.NewsURLId);
            #endregion

            var result = query.OrderByDescending(a => a.Pubtime)
            .ToList();

            return result;
        }

       

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public int AddNews(News news)
        {
            _dbContext.News.Add(news);
            _dbContext.SaveChanges();
            return news.Id;
        }

        public int EditNews(int id, News model)
        {
            var news = _dbContext.News.FirstOrDefault(a => a.Id == id);

            news.NewsScopeId = model.NewsScopeId;
            news.ModifiedDate =model.ModifiedDate;
            news.Modifier = model.Modifier;
            news.Pubtime = model.Pubtime;
            news.NewsURLId = model.NewsURLId;

            var culture = model.NewsGlobalizations.First().Culture;
            //先删除原来子数据
            var ag = _dbContext.NewsGlobalizations
                .Where(a => a.NewsId == news.Id && a.Culture == culture);

            _dbContext.NewsGlobalizations.RemoveRange(ag);

            _dbContext.NewsGlobalizations.AddRange(model.NewsGlobalizations);

            _dbContext.SaveChanges();
            return news.Id;
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

        #region NewsGlobalization

        public NewsGlobalization GetNewsGlobalizationById(int newsGlobalizationId)
        {
            var newsGlobalization = _dbContext.NewsGlobalizations.FirstOrDefault(a => a.Id == newsGlobalizationId);
            return newsGlobalization;
        }

        public List<NewsGlobalization> GetNewsGlobalizationByNewsId(int newsId)
        {
            var newsGlobalization = _dbContext.NewsGlobalizations.Where(a => a.NewsId == newsId).ToList();
            return newsGlobalization;
        }

        /// <summary>
        /// 修改图片ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public string EditImageId(int newsGlobalizationId, int imageId)
        {
            var newsGlobalization = _dbContext.NewsGlobalizations.FirstOrDefault(a => a.Id == newsGlobalizationId);

            newsGlobalization.ImageId = imageId;

            _dbContext.SaveChanges();
            return newsGlobalization.Id.ToString();
        }


        public NewsGlobalization GetNewsGlobalizationByNewsIdAndCulture(int newsId, string culture)
        {
            var result = _dbContext.NewsGlobalizations.Where(ag => ag.NewsId == newsId && ag.Culture == culture).FirstOrDefault();


            return result;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="about"></param>
        /// <returns></returns>
        public int AddNewsGlobalization(NewsGlobalization newsGlobalization)
        {
            var existData = _dbContext.NewsGlobalizations
                .Where(ag => ag.NewsId == newsGlobalization.NewsId && ag.Culture == newsGlobalization.Culture);
            if (existData.Count() > 0)
            {
                _dbContext.NewsGlobalizations.RemoveRange(existData);
            }
            //重新添加
            _dbContext.NewsGlobalizations.Add(newsGlobalization);

            _dbContext.SaveChanges();
            return newsGlobalization.Id;
        }
        #endregion
    }
}
