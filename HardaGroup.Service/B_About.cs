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
        public List<M_AboutSearch> GetPageData(M_AboutSearch about)
        {
            AboutSearch searchModel = new AboutSearch()
            {
                TypeCode = about.TypeCode,
                TypeName = about.TypeName
            };

            var offset = about.offset;
            var limit = about.limit;


            var pageResult = _dAbout.GetPageData(searchModel, offset, limit);
            var result = pageResult.Select(a => ConverSearchEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取分页数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetPageDataTotalCount(M_AboutSearch about)
        {
            AboutSearch searchModel = new AboutSearch()
            {
                TypeCode = about.TypeCode,
                TypeName = about.TypeName
            };

            var totalCount = _dAbout.GetPageDataTotalCount(searchModel);
            return totalCount;
        }

        /// <summary>
        /// 获取culture下的所数据
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public List<M_AboutSearch> GetAllCultureData(string culture)
        {
            var allDatas = this.GetAllData();
            //根据当前国际化代码过滤数据
            List<M_AboutSearch> cultureDatas = new List<M_AboutSearch>();
            foreach (var about in allDatas)
            {
                //根据国际化代码过滤数据，如果没有当前国际化代码的数据，则取中文数据
                var defaultGlobalizationData = about.AboutGlobalizations.Where(ag => ag.Culture == culture).FirstOrDefault();
                if (defaultGlobalizationData == null)
                {
                    defaultGlobalizationData = about.AboutGlobalizations.Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();
                }

                M_AboutSearch cultureData = new M_AboutSearch();
                cultureData.TypeCode = about.TypeCode;
                cultureData.TypeName = defaultGlobalizationData.TypeName;
                cultureData.Content = defaultGlobalizationData.Content;

                cultureDatas.Add(cultureData);
            }
            return cultureDatas;
        }

        /// <summary>
        /// 根据条件过滤数据
        /// </summary>
        /// <param name="about"></param>
        /// <returns></returns>
        public List<M_About> GetByMulitCond(M_About about)
        {
            About searchModel = new About()
            {
                TypeCode =about.TypeCode,
                //TypeName = about.TypeName,
                //Culture = about.Culture
                
            };
            var abouts = _dAbout.GetByMulitCondition(searchModel);
            var result = abouts.Select(a => ConverEntityToModel(a)).ToList();
            return result;
        }

        private M_About ConverEntityToModel(About about)
        {
            if (about == null) return null;

            var model = new M_About()
            {
                Id = about.Id.ToString(),
                TypeCode = about.TypeCode,
                Sequence = about.Sequence.ToString(),
                AboutGlobalizations = about.AboutGlobalizations.Select(ag => new M_AboutGlobalization() {
                    Id= ag.Id.ToString(),
                    TypeName = ag.TypeName,
                    Content = ag.Content,
                    Culture = ag.Culture,

                }).ToList()
            };
            return model;
        }

        private M_AboutSearch ConverSearchEntityToModel(AboutSearch about)
        {
            if (about == null) return null;

            var model = new M_AboutSearch()
            {
                Id = about.Id.ToString(),
                TypeCode = about.TypeCode,
                TypeName = about.TypeName,
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
                //只能有唯一的TypeCode
                var search = new About() { TypeCode = model.TypeCode};
                var data = _dAbout.GetByMulitCondition(search);
                if (data.Count > 0)
                {
                    message.Success = false;
                    message.Content = "系统中已存在代码为'" + model.TypeCode + "'的数据,不能重复添加。";
                    return message;

                }
                //新增
                var about = new About()
                {
                    TypeCode = model.TypeCode,
                    Sequence = string.IsNullOrEmpty(model.Sequence) ? 0 : System.Convert.ToInt32(model.Sequence),
                    AboutGlobalizations = model.AboutGlobalizations.Select(a => new AboutGlobalization() {
                        
                        TypeName = a.TypeName,
                        Content = a.Content,
                        Culture = a.Culture
                    }).ToList()
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
        /// 处理国际化数据
        /// </summary>
        /// <param name="model"></param>
        /// 
        /// <returns></returns>
        public Message DealAboutGlobalization(M_AboutGlobalization model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "新增成功";

            try
            {
                
                //新增
                var aboutGlobalization = new AboutGlobalization()
                {
                    AboutId = System.Convert.ToInt32(model.AboutId),
                    TypeName = model.TypeName,
                    Content = model.Content,
                    Culture = model.Culture
                };

                message.ReturnId = _dAbout.AddAboutGlobalization(aboutGlobalization);

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
                //只能有唯一的TypeCode
                var search = new About() { TypeCode = model.TypeCode };
                var data = _dAbout.GetByMulitCondition(search);
                if (data.Where(a => a.Id != System.Convert.ToInt32(model.Id)).Count() > 0)
                {
                    message.Success = false;
                    message.Content = "系统中已存在代码为'" + model.TypeCode + "'的数据,不能重复添加。";
                    return message;

                }

                //
                var about = new About()
                {
                    TypeCode = model.TypeCode,
                    Sequence = string.IsNullOrEmpty(model.Sequence) ? 0 : System.Convert.ToInt32(model.Sequence),
                    AboutGlobalizations = model.AboutGlobalizations.Select(a => new AboutGlobalization()
                    {
                        AboutId = System.Convert.ToInt32(a.AboutId),
                        TypeName = a.TypeName,
                        Content = a.Content,
                        Culture = a.Culture
                    }).ToList()
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


        /// <summary>
        /// 根据条件过滤数据
        /// </summary>
        /// <param name="about"></param>
        /// <returns></returns>
        public M_AboutGlobalization GetAboutGlobalizationByAboutIdAndCulture(string aboutId,string culture)
        {

            M_AboutGlobalization aboutGlobalization = null;
            var id = System.Convert.ToInt32(aboutId);
            var result = _dAbout.GetAboutGlobalizationByAboutIdAndCulture(id,culture);

            if(result !=null)
            {
                aboutGlobalization = new M_AboutGlobalization();
                aboutGlobalization.Id = result.Id.ToString();
                aboutGlobalization.TypeName = result.TypeName;
                aboutGlobalization.Content = result.Content;
                aboutGlobalization.Culture = result.Culture;
                aboutGlobalization.AboutId = result.AboutId.ToString();
                
            }
            return aboutGlobalization;
        }
    }
}
