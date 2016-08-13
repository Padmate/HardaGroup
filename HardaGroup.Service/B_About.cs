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
    public class B_About
    {
        D_About _dAbout = new D_About();

        public M_About GetById(int id)
        {
            var about = _dAbout.GetAboutById(id);
            var result = ConverEntityToModel(about);
            return result;
        }

        public List<M_About> GetAllData()
        {
            var abouts = _dAbout.GetAll();
            var result = abouts.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="about"></param>
        /// <returns></returns>
        public List<M_About> GetPageData(M_About about)
        {
            About searchModel = new About()
            {
                TypeName = about.TypeName
            };

            var offset = about.offset;
            var limit = about.limit;


            var pageResult = _dAbout.GetPageData(searchModel, offset, limit);
            var result = pageResult.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取分页数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetPageDataTotalCount(M_About about)
        {
            About searchModel = new About()
            {
                TypeName = about.TypeName
            };

            var totalCount = _dAbout.GetPageDataTotalCount(searchModel);
            return totalCount;
        }

        private M_About ConverEntityToModel(About about)
        {
            if (about == null) return null;

            var model = new M_About()
            {
                Id = about.Id.ToString(),
                TypeName = about.TypeName,
                TypeCode = about.TypeCode,
                Content = about.Content,
                Culture = about.Culture,
                Sequence = about.Sequence.ToString(),
            };
            return model;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// 
        /// <returns></returns>
        public Message AddAbout(M_About model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "新增成功";

            try
            {
                //新增
                var about = new About()
                {
                    TypeName = model.TypeName,
                    TypeCode = model.TypeCode,
                    Content = model.Content,
                    Culture = model.Culture,
                    Sequence = string.IsNullOrEmpty(model.Sequence) ? 0 : System.Convert.ToInt32(model.Sequence)
                };

                message.ReturnId = _dAbout.AddAbout(about);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "新增失败，异常：" + e.Message;
            }
            return message;
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Message EditAbout(M_About model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "修改成功";

            try
            {
                var about = new About()
                {
                    TypeName = model.TypeName,
                    TypeCode = model.TypeCode,
                    Content = model.Content,
                    Culture = model.Culture,
                    Sequence = string.IsNullOrEmpty(model.Sequence) ? 0 : System.Convert.ToInt32(model.Sequence)
                };

                message.ReturnId = _dAbout.EditAbout(System.Convert.ToInt32(model.Id), about);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "修改失败，异常：" + e.Message;
            }
            return message;
        }

        public Message BatchDeleteByIds(List<int> ids)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "删除成功";

            try
            {
                _dAbout.BatchDeleteAbout(ids);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "删除失败：" + e.Message;
            }

            return message;
        }

    }
}
