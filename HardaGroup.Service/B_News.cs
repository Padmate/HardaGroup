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
    public class B_News
    {
        D_News _dNews = new D_News();

        public B_News()
        {

        }

        M_User _currentUser;
        string _mapPath;
        public B_News(M_User currentUser)
        {
            _currentUser = currentUser;

        }

        public B_News(M_User currentUser, string mapPath)
        {
            _currentUser = currentUser;
            _mapPath = mapPath;

        }

        /// <summary>
        /// 获取分页数据 - BootstrapTable
        /// </summary>
        /// <param name="about"></param>
        /// <returns></returns>
        public List<M_NewsSearch> GetPageDataForBootstrapTable(M_NewsSearch news)
        {
            NewsSearch searchModel = new NewsSearch()
            {

                Title = news.Title,
                SubTitle = news.SubTitle,
                Culture = news.Culture
            };

            if(string.IsNullOrEmpty(news.NewsScopeId))
            {
                searchModel.NewsScopeId = null;
            }
            else
            {
                searchModel.NewsScopeId = System.Convert.ToInt32(news.NewsScopeId);
            }

            var offset = news.offset;
            var limit = news.limit;


            var pageResult = _dNews.GetPageData(searchModel, offset, limit);
            var result = pageResult.Select(a => ConverSearchEntityToModel(a)).ToList();

            return result;
        }


        /// <summary>
        /// 获取分页数据 - bootstrap-paginator
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public List<M_NewsSearch> GetPageDataForBootstrapPaginator(M_NewsSearch news)
        {

            NewsSearch searchModel = new NewsSearch()
            {
                Title = news.Title,
                SubTitle = news.SubTitle,
                Culture = news.Culture
            };
            if (string.IsNullOrEmpty(news.NewsScopeId))
            {
                searchModel.NewsScopeId = null;
            }
            else
            {
                searchModel.NewsScopeId = System.Convert.ToInt32(news.NewsScopeId);
            }

            var currentPage = news.page;
            var limit = news.limit;

            //page:第一页表示从第0条数据开始索引
            Int32 skip = System.Convert.ToInt32((currentPage - 1) * limit);

            B_Image bImage = new B_Image();
            var pageResult = _dNews.GetPageData(searchModel, skip, limit);
            var result = pageResult.Select(a => ConverSearchEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取分页数据总条数 
        /// </summary>
        /// <returns></returns>
        public int GetPageDataTotalCount(M_NewsSearch news)
        {

            NewsSearch searchModel = new NewsSearch()
            {
                Title = news.Title,
                SubTitle = news.SubTitle,
                Culture = news.Culture
            };
            if (string.IsNullOrEmpty(news.NewsScopeId))
            {
                searchModel.NewsScopeId = null;
            }
            else
            {
                searchModel.NewsScopeId = System.Convert.ToInt32(news.NewsScopeId);
            }

            var totalCount = _dNews.GetPageDataTotalCount(searchModel);
            return totalCount;
        }


        public M_News GetNewsById(string newsId)
        {
            B_Image bImage = new B_Image();

            int id = System.Convert.ToInt32(newsId);
            var news = _dNews.GetNewsById(id);
            var result = ConverEntityToModel(news);
            return result;
        }

        /// <summary>
        /// 根据当前id获取上一条数据的id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public M_NewsSearch GetPreviousIdByNewsUrlIdAndCulture(string newsUrlId,string culture)
        {
            B_Image bImage = new B_Image();

            var news = _dNews.GetPreviousDataByNewsURLId(newsUrlId);

            if (news == null) return null;
            M_NewsSearch cultureData = new M_NewsSearch();

            //根据国际化代码过滤数据，如果没有当前国际化代码的数据，则取中文数据
            var defaultGlobalizationData = news.NewsGlobalizations.Where(ag => ag.Culture == culture).FirstOrDefault();
            if (defaultGlobalizationData == null)
            {
                defaultGlobalizationData = news.NewsGlobalizations.Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();
            }

            cultureData.Id = news.Id.ToString();
            cultureData.NewsURLId = news.NewsURLId;
            cultureData.Title = defaultGlobalizationData.Title;
            cultureData.SubTitle = defaultGlobalizationData.SubTitle;
            cultureData.Description = defaultGlobalizationData.Description;
            cultureData.Content = defaultGlobalizationData.Content;
            cultureData.Image = defaultGlobalizationData.ImageId == null ? null : bImage.GetImageById(System.Convert.ToInt32(defaultGlobalizationData.ImageId));
            cultureData.CreateDate = news.CreateDate;
            cultureData.Creator = news.Creator;
            cultureData.ModifiedDate = news.ModifiedDate;
            cultureData.Modifier = news.Modifier;
            cultureData.Pubtime = news.Pubtime;
            cultureData.NewsScopeId = news.NewsScopeId.ToString();

            return cultureData;
        }

        /// <summary>
        /// 根据当前id获取下一条数据的id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public M_NewsSearch GetNextIdByCurrentNewsUrlIdAndCulture(string newsUrlId,string culture)
        {
            B_Image bImage = new B_Image();

            var news = _dNews.GetNextDataByNewsURLId(newsUrlId);
            if (news == null) return null;
            M_NewsSearch cultureData = new M_NewsSearch();

            //根据国际化代码过滤数据，如果没有当前国际化代码的数据，则取中文数据
            var defaultGlobalizationData = news.NewsGlobalizations.Where(ag => ag.Culture == culture).FirstOrDefault();
            if (defaultGlobalizationData == null)
            {
                defaultGlobalizationData = news.NewsGlobalizations.Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();
            }

            cultureData.Id = news.Id.ToString();
            cultureData.NewsURLId = news.NewsURLId;
            cultureData.Title = defaultGlobalizationData.Title;
            cultureData.SubTitle = defaultGlobalizationData.SubTitle;
            cultureData.Description = defaultGlobalizationData.Description;
            cultureData.Content = defaultGlobalizationData.Content;
            cultureData.Image = defaultGlobalizationData.ImageId == null ? null : bImage.GetImageById(System.Convert.ToInt32(defaultGlobalizationData.ImageId));
            cultureData.CreateDate = news.CreateDate;
            cultureData.Creator = news.Creator;
            cultureData.ModifiedDate = news.ModifiedDate;
            cultureData.Modifier = news.Modifier;
            cultureData.Pubtime = news.Pubtime;
            cultureData.NewsScopeId = news.NewsScopeId.ToString();

            return cultureData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public M_NewsSearch GetNewsByNewsUrlIdAndCulture(string newsUrlId,string culture)
        {
            B_Image bImage = new B_Image();
            var news = _dNews.GetNewsByNewsUrlId(newsUrlId);

            if (news == null) return null;
            M_NewsSearch cultureData = new M_NewsSearch();

            //根据国际化代码过滤数据，如果没有当前国际化代码的数据，则取中文数据
            var defaultGlobalizationData = news.NewsGlobalizations.Where(ag => ag.Culture == culture).FirstOrDefault();
            if (defaultGlobalizationData == null)
            {
                defaultGlobalizationData = news.NewsGlobalizations.Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();
            }

            cultureData.Id = news.Id.ToString();
            cultureData.NewsURLId = news.NewsURLId;
            cultureData.Title = defaultGlobalizationData.Title;
            cultureData.SubTitle = defaultGlobalizationData.SubTitle;
            cultureData.Description = defaultGlobalizationData.Description;
            cultureData.Content = defaultGlobalizationData.Content;
            cultureData.Image = defaultGlobalizationData.ImageId == null ? null : bImage.GetImageById(System.Convert.ToInt32(defaultGlobalizationData.ImageId));
            cultureData.CreateDate = news.CreateDate;
            cultureData.Creator = news.Creator;
            cultureData.ModifiedDate = news.ModifiedDate;
            cultureData.Modifier = news.Modifier;
            cultureData.Pubtime = news.Pubtime;
            cultureData.NewsScopeId = news.NewsScopeId.ToString();

            return cultureData;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// 
        /// <returns></returns>
        public Message AddNews(M_News model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "新增成功";

            try
            {
                //只能有唯一的NewsUrlId
                var search = new News() { NewsURLId = model.NewsURLId };
                var data = _dNews.GetByMulitCondition(search);
                if (data.Count > 0)
                {
                    message.Success = false;
                    message.Content = "系统中已存在标识为'" + model.NewsURLId + "'的数据,不能重复添加。";
                    return message;

                }

                //新增
                var news = new News()
                {
                    
                    NewsURLId = model.NewsURLId,
                    CreateDate = DateTime.Now,
                    Creator = _currentUser.UserName,
                    Pubtime = model.Pubtime,
                    NewsScopeId = System.Convert.ToInt32(model.NewsScopeId),
                    NewsGlobalizations = model.NewsGlobalizations.Select(ng => new NewsGlobalization() {
                        Title = ng.Title,
                        SubTitle = ng.SubTitle,
                        Description = ng.Description,
                        Content = ng.Content,
                        Culture = ng.Culture
                        
                    }).ToList()

                };

                int newsId = _dNews.AddNews(news);
                message.ReturnId = newsId;
               
            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "新增失败，异常：" + e.Message;
            }
            return message;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Message EditNews(M_News model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "修改成功";

            try
            {
                var newsId = System.Convert.ToInt32(model.Id);
                var culture = model.NewsGlobalizations.First().Culture;

                //只能有唯一的NewsUrlId
                var search = new News() { NewsURLId = model.NewsURLId };
                var data = _dNews.GetByMulitCondition(search);
                if (data.Where(a => a.Id != System.Convert.ToInt32(model.Id)).Count() > 0)
                {
                    message.Success = false;
                    message.Content = "系统中已存在标识为'" + model.NewsURLId + "'的数据,不能重复添加。";
                    return message;

                }
                //根据newsId和culture，查找已存在的数据
                var oldData = _dNews.GetNewsGlobalizationByNewsIdAndCulture(newsId,culture);

                var news = new News()
                {
                    NewsURLId = model.NewsURLId,
                    ModifiedDate = DateTime.Now,
                    Modifier = _currentUser.UserName,
                    Pubtime = model.Pubtime,
                    NewsScopeId = System.Convert.ToInt32(model.NewsScopeId),
                    NewsGlobalizations = model.NewsGlobalizations.Select(ng => new NewsGlobalization()
                    {
                        NewsId = newsId,
                        Title = ng.Title,
                        SubTitle = ng.SubTitle,
                        Description = ng.Description,
                        Content = ng.Content,
                        Culture = ng.Culture,
                        ImageId = oldData.ImageId

                    }).ToList()
                };

                message.ReturnId = _dNews.EditNews(System.Convert.ToInt32(model.Id), news);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "修改失败，异常：" + e.Message;
            }
            return message;
        }

        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Message DeleteNews(string newsId)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "删除成功";
            try
            {
                var id = System.Convert.ToInt32(newsId);
                B_Image bImage = new B_Image();
                //删除图标
                var news = _dNews.GetNewsById(id);
                //if (news.ImageId != null)
                //    bImage.DeleteImage(System.Convert.ToInt32(news.ImageId));

                _dNews.DeleteNews(id);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "删除失败，异常：" + e.Message;
            }
            return message;
        }

        /// <summary>
        /// 根据ID批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Message BatchDeleteByIds(List<Int32> ids)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "删除成功";

            try
            {
                B_Image bImage = new B_Image();
                //删除图标
                foreach(var id in ids)
                {
                    var newsGlobalizations = _dNews.GetNewsGlobalizationByNewsId(id);
                    foreach(var ng in newsGlobalizations)
                    {
                        if (ng.ImageId != null)
                            bImage.DeleteImage(System.Convert.ToInt32(ng.ImageId));
                    }
                    
                }
                _dNews.BatchDeleteNews(ids);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "删除失败：" + e.Message;
            }

            return message;
        }

        private M_News ConverEntityToModel(News news)
        {
            if (news == null) return null;

            B_Image bImage = new B_Image();

            var model = new M_News()
            {
                Id = news.Id.ToString(),
                NewsURLId = news.NewsURLId,
                CreateDate = news.CreateDate,
                Creator = news.Creator,
                ModifiedDate = news.ModifiedDate,
                Modifier = news.Modifier,
                Pubtime = news.Pubtime,
                NewsScopeId = news.NewsScopeId.ToString(),
                NewsGlobalizations = news.NewsGlobalizations.Select(ng => new M_NewsGlobalization() {
                    Title = ng.Title,
                    SubTitle = ng.SubTitle,
                    Description = ng.Description,
                    Content = ng.Content,
                    Culture = ng.Culture,
                    Image = ng.ImageId == null ? null : bImage.GetImageById(System.Convert.ToInt32(ng.ImageId)),
                }).ToList()
            };
            return model;
        }

        private M_NewsSearch ConverSearchEntityToModel(NewsSearch news)
        {
            if (news == null) return null;

            B_Image bImage = new B_Image();

            var model = new M_NewsSearch()
            {
                Id = news.Id.ToString(),
                NewsURLId = news.NewsURLId,
                Title = news.Title,
                SubTitle = news.SubTitle,
                Description = news.Description,
                Content = news.Content,
                Image = news.ImageId == null ? null : bImage.GetImageById(System.Convert.ToInt32(news.ImageId)),
                CreateDate = news.CreateDate,
                Creator = news.Creator,
                ModifiedDate = news.ModifiedDate,
                Modifier = news.Modifier,
                Pubtime = news.Pubtime,
                NewsScopeId = news.NewsScopeId.ToString()
            };
            return model;
        }

        #region NewsGlobalization
        public M_NewsGlobalization GetNewsGlobalizationByNewsIdAndCulture(string newsId,string culture)
        {
            int id = System.Convert.ToInt32(newsId);
            var newsGlobalization = _dNews.GetNewsGlobalizationByNewsIdAndCulture(id,culture);
            var result = ConverNewsGlobalizationEntityToModel(newsGlobalization);
            return result;
        }

        public M_NewsGlobalization GetNewsGlobalizationById(string newsGlobalizationId)
        {

            int id = System.Convert.ToInt32(newsGlobalizationId);
            var newsGlobalization = _dNews.GetNewsGlobalizationById(id);
            var result = ConverNewsGlobalizationEntityToModel(newsGlobalization);
            return result;
        }

        public M_NewsGlobalization GetNewsGlobalizationByIdAndCulture(string newsGlobalizationId,string culture)
        {

            int id = System.Convert.ToInt32(newsGlobalizationId);
            var newsGlobalization = _dNews.GetNewsGlobalizationByNewsIdAndCulture(id,culture);
            var result = ConverNewsGlobalizationEntityToModel(newsGlobalization);
            return result;
        }

        /// <summary>
        /// 处理国际化数据
        /// </summary>
        /// <param name="model"></param>
        /// 
        /// <returns></returns>
        public Message DealNewsGlobalization(M_NewsGlobalization model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "新增成功";

            try
            {

                //新增
                var newsGlobalization = new NewsGlobalization()
                {
                    NewsId = System.Convert.ToInt32(model.NewsId),
                    Title = model.Title,
                    SubTitle = model.SubTitle,
                    Description = model.Description,
                    Content = model.Content,
                    Culture = model.Culture
                };

                message.ReturnId = _dNews.AddNewsGlobalization(newsGlobalization);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "新增失败，异常：" + e.Message;
            }
            return message;
        }

        /// <summary>
        /// 修改图片id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Message UpdateImageId(string newsGlobalizationId, int imageId)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "图片更新成功";
            try
            {
                var id = System.Convert.ToInt32(newsGlobalizationId);
                message.ReturnStrId = _dNews.EditImageId(id, imageId);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "图片更新失败:" + e.Message;
            }

            return message;
        }

        private M_NewsGlobalization ConverNewsGlobalizationEntityToModel(NewsGlobalization newsGlobalization)
        {
            if (newsGlobalization == null) return null;

            B_Image bImage = new B_Image();

            var model = new M_NewsGlobalization()
            {
                Id = newsGlobalization.Id.ToString(),
                Title = newsGlobalization.Title,
                SubTitle = newsGlobalization.SubTitle,
                Description = newsGlobalization.Description,
                Content = newsGlobalization.Content,
                Culture = newsGlobalization.Culture,
                Image = newsGlobalization.ImageId == null ? null : 
                            bImage.GetImageById(System.Convert.ToInt32(newsGlobalization.ImageId))
                
            };
            return model;
        }

        #endregion

    }
}
