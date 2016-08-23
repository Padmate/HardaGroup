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
        public List<M_News> GetPageDataForBootstrapTable(M_News news)
        {
            News searchModel = new News()
            {

                Title = news.Title,
                SubTitle = news.SubTitle,
                NewsScopeId = System.Convert.ToInt32(news.NewsScopeId)
            };

            var offset = news.offset;
            var limit = news.limit;


            var pageResult = _dNews.GetPageData(searchModel, offset, limit);
            var result = pageResult.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }


        /// <summary>
        /// 获取分页数据 - bootstrap-paginator
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public List<M_News> GetPageDataForBootstrapPaginator(M_News news)
        {
           
            News searchModel = new News()
            {
                Title = news.Title,
                SubTitle = news.SubTitle,
                NewsScopeId = System.Convert.ToInt32(news.NewsScopeId)
            };

            var currentPage = news.page;
            var limit = news.limit;

            //page:第一页表示从第0条数据开始索引
            Int32 skip = System.Convert.ToInt32((currentPage - 1) * limit);

            B_Image bImage = new B_Image();
            var pageResult = _dNews.GetPageData(searchModel, skip, limit);
            var result = pageResult.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取分页数据总条数 
        /// </summary>
        /// <returns></returns>
        public int GetPageDataTotalCount(M_News news)
        {
            
            News searchModel = new News()
            {
                Title = news.Title,
                SubTitle = news.SubTitle,
                NewsScopeId = System.Convert.ToInt32(news.NewsScopeId)
            };

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
                //新增
                var news = new News()
                {
                    Title = model.Title,
                    SubTitle = model.SubTitle,
                    Description = model.Description,
                    Content = model.Content,
                    CreateDate = DateTime.Now,
                    Creator = _currentUser.UserName,
                    Pubtime = model.Pubtime,
                    NewsScopeId = System.Convert.ToInt32(model.NewsScopeId)
                    
                };

                message.ReturnStrId = _dNews.AddNews(news);

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
                var news = new News()
                {
                    Title = model.Title,
                    SubTitle = model.SubTitle,
                    Description = model.Description,
                    Content = model.Content,
                    ModifiedDate = DateTime.Now,
                    Modifier = _currentUser.UserName,
                    Pubtime = model.Pubtime,
                    NewsScopeId = System.Convert.ToInt32(model.NewsScopeId)
                };

                message.ReturnStrId = _dNews.EditNews(System.Convert.ToInt32(model.Id), news);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "修改失败，异常：" + e.Message;
            }
            return message;
        }


        /// <summary>
        /// 修改图片id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Message UpdateImageId(string newsId, int imageId)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "图片更新成功";
            try
            {
                var id = System.Convert.ToInt32(newsId);
                message.ReturnStrId = _dNews.EditImageId(id, imageId);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "图片更新失败:" + e.Message;
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
                if (news.ImageId != null)
                    bImage.DeleteImage(System.Convert.ToInt32(news.ImageId));

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
                    var news = _dNews.GetNewsById(id);
                    if (news.ImageId != null)
                        bImage.DeleteImage(System.Convert.ToInt32(news.ImageId));
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

    }
}
