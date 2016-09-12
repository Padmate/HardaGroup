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
    public class B_Module
    {
        D_Module _dModule = new D_Module();

        public B_Module()
        {

        }

        M_User _currentUser;
        string _mapPath;
        public B_Module(M_User currentUser)
        {
            _currentUser = currentUser;

        }

        public B_Module(M_User currentUser, string mapPath)
        {
            _currentUser = currentUser;
            _mapPath = mapPath;

        }

        /// <summary>
        /// 获取分页数据 - BootstrapTable
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public List<M_ModuleSearch> GetPageDataForBootstrapTable(M_ModuleSearch modules)
        {
            ModuleSearch searchModel = new ModuleSearch()
            {

                Title = modules.Title,
                SubTitle = modules.SubTitle,
                Culture = modules.Culture,
                Type = modules.Type
            };


            var offset = modules.offset;
            var limit = modules.limit;


            var pageResult = _dModule.GetPageData(searchModel, offset, limit);
            var result = pageResult.Select(a => ConverSearchEntityToModel(a)).ToList();

            return result;
        }


        /// <summary>
        /// 获取分页数据 - bootstrap-paginator
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public List<M_ModuleSearch> GetPageDataForBootstrapPaginator(M_ModuleSearch modules)
        {

            ModuleSearch searchModel = new ModuleSearch()
            {
                Title = modules.Title,
                SubTitle = modules.SubTitle,
                Culture = modules.Culture,
                Type = modules.Type
            };


            var currentPage = modules.page;
            var limit = modules.limit;

            //page:第一页表示从第0条数据开始索引
            Int32 skip = System.Convert.ToInt32((currentPage - 1) * limit);

            B_Image bImage = new B_Image();
            var pageResult = _dModule.GetPageData(searchModel, skip, limit);
            var result = pageResult.Select(a => ConverSearchEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 获取分页数据总条数 
        /// </summary>
        /// <returns></returns>
        public int GetPageDataTotalCount(M_ModuleSearch modules)
        {

            ModuleSearch searchModel = new ModuleSearch()
            {
                Title = modules.Title,
                SubTitle = modules.SubTitle,
                Culture = modules.Culture,
                Type = modules.Type
            };
            

            var totalCount = _dModule.GetPageDataTotalCount(searchModel);
            return totalCount;
        }

        /// <summary>
        /// 获取culture下的所数据
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public List<M_ModuleSearch> GetAllCultureDataByType(string culture,string type)
        {
            var allDatas = _dModule.GetModuleByType(type);
            //根据当前国际化代码过滤数据
            List<M_ModuleSearch> cultureDatas = new List<M_ModuleSearch>();
            B_Image bImage = new B_Image();
            foreach (var module in allDatas)
            {
                //根据国际化代码过滤数据，如果没有当前国际化代码的数据，则取中文数据
                var defaultGlobalizationData = module.ModuleGlobalizations.Where(ag => ag.Culture == culture).FirstOrDefault();
                if (defaultGlobalizationData == null)
                {
                    defaultGlobalizationData = module.ModuleGlobalizations.Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();
                }

                M_ModuleSearch cultureData = new M_ModuleSearch();
                cultureData.Id = module.Id.ToString();
                cultureData.ModuleURLId = module.ModuleURLId;
                cultureData.Title = defaultGlobalizationData.Title;
                cultureData.SubTitle = defaultGlobalizationData.SubTitle;
                cultureData.Description = defaultGlobalizationData.Description;
                cultureData.Content = defaultGlobalizationData.Content;
                cultureData.ImageClass = defaultGlobalizationData.ImageClass;
                cultureData.Image = defaultGlobalizationData.ImageId == null ? null : bImage.GetImageById(System.Convert.ToInt32(defaultGlobalizationData.ImageId));
                cultureData.CreateDate = module.CreateDate.ToString();
                cultureData.Creator = module.Creator;
                cultureData.ModifiedDate = module.ModifiedDate.ToString();
                cultureData.Modifier = module.Modifier;
                cultureData.Sequence = module.Sequence.ToString();
                cultureData.Type = module.Type;

                cultureDatas.Add(cultureData);
            }
            return cultureDatas;
        }

        public List<M_Module> GetAllData()
        {
            var modules = _dModule.GetAll();
            var result = modules.Select(a => ConverEntityToModel(a)).ToList();

            return result;
        }

        public M_Module GetModuleById(string modulesId)
        {
            B_Image bImage = new B_Image();

            int id = System.Convert.ToInt32(modulesId);
            var modules = _dModule.GetModuleById(id);
            var result = ConverEntityToModel(modules);
            return result;
        }

        public List<M_Module> GetModuleByType(string type)
        {
            B_Image bImage = new B_Image();

            var modules = _dModule.GetModuleByType(type);
            var result = modules.Select(a => ConverEntityToModel(a)).ToList();
            return result;
        }

        /// <summary>
        /// 根据条件查找
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public List<M_ModuleSearch> GetModulesByMulitCondition(M_ModuleSearch modules)
        {
            ModuleSearch searchModel = new ModuleSearch()
            {
                Culture = modules.Culture,
                Type = modules.Type
            };


            B_Image bImage = new B_Image();
            var pageResult = _dModule.GetByMulitCondition(searchModel);
            var result = pageResult.Select(a => ConverSearchEntityToModel(a)).ToList();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public M_ModuleSearch GetModuleByModuleUrlIdAndCulture(string modulesUrlId, string culture)
        {
            B_Image bImage = new B_Image();
            var modules = _dModule.GetModuleByModuleUrlId(modulesUrlId);

            if (modules == null) return null;
            M_ModuleSearch cultureData = new M_ModuleSearch();

            //根据国际化代码过滤数据，如果没有当前国际化代码的数据，则取中文数据
            var defaultGlobalizationData = modules.ModuleGlobalizations.Where(ag => ag.Culture == culture).FirstOrDefault();
            if (defaultGlobalizationData == null)
            {
                defaultGlobalizationData = modules.ModuleGlobalizations.Where(ag => ag.Culture == Common.Globalization_Chinese).FirstOrDefault();
            }

            cultureData.Id = modules.Id.ToString();
            cultureData.ModuleURLId = modules.ModuleURLId;
            cultureData.Title = defaultGlobalizationData.Title;
            cultureData.SubTitle = defaultGlobalizationData.SubTitle;
            cultureData.Description = defaultGlobalizationData.Description;
            cultureData.Content = defaultGlobalizationData.Content;
            cultureData.ImageClass = defaultGlobalizationData.ImageClass;
            cultureData.Image = defaultGlobalizationData.ImageId == null ? null : bImage.GetImageById(System.Convert.ToInt32(defaultGlobalizationData.ImageId));
            cultureData.CreateDate = modules.CreateDate.ToString();
            cultureData.Creator = modules.Creator;
            cultureData.ModifiedDate = modules.ModifiedDate.ToString();
            cultureData.Modifier = modules.Modifier;
            cultureData.Sequence = modules.Sequence.ToString();
            cultureData.Type = modules.Type;
            

            return cultureData;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// 
        /// <returns></returns>
        public Message AddModule(M_Module model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "新增成功";

            try
            {
                //只能有唯一的ModuleUrlId
                var search = new Module() { ModuleURLId = model.ModuleURLId };
                var data = _dModule.GetByMulitCondition(search);
                if (data.Count > 0)
                {
                    message.Success = false;
                    message.Content = "系统中已存在标识为'" + model.ModuleURLId + "'的数据,不能重复添加。";
                    return message;

                }

                //新增
                var modules = new Module()
                {

                    ModuleURLId = model.ModuleURLId,
                    CreateDate = DateTime.Now,
                    Creator = _currentUser.UserName,
                    Sequence = string.IsNullOrEmpty(model.Sequence) ? 0 : System.Convert.ToInt32(model.Sequence),
                    Type = model.Type,
                    ModuleGlobalizations = model.ModuleGlobalizations.Select(ng => new ModuleGlobalization()
                    {
                        Title = ng.Title,
                        SubTitle = ng.SubTitle,
                        Description = ng.Description,
                        Content = ng.Content,
                        Culture = ng.Culture,
                        ImageClass = ng.ImageClass

                    }).ToList()

                };

                int modulesId = _dModule.AddModule(modules);
                message.ReturnId = modulesId;

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
        public Message EditModule(M_Module model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "修改成功";

            try
            {
                var modulesId = System.Convert.ToInt32(model.Id);
                var culture = model.ModuleGlobalizations.First().Culture;

                //只能有唯一的ModuleUrlId
                var search = new Module() { ModuleURLId = model.ModuleURLId };
                var data = _dModule.GetByMulitCondition(search);
                if (data.Where(a => a.Id != System.Convert.ToInt32(model.Id)).Count() > 0)
                {
                    message.Success = false;
                    message.Content = "系统中已存在标识为'" + model.ModuleURLId + "'的数据,不能重复添加。";
                    return message;

                }
                //根据modulesId和culture，查找已存在的数据
                var oldData = _dModule.GetModuleGlobalizationByModuleIdAndCulture(modulesId, culture);

                var modules = new Module()
                {
                    ModuleURLId = model.ModuleURLId,
                    ModifiedDate = DateTime.Now,
                    Modifier = _currentUser.UserName,
                    Sequence = string.IsNullOrEmpty(model.Sequence) ? 0 : System.Convert.ToInt32(model.Sequence),
                    Type = model.Type,
                    ModuleGlobalizations = model.ModuleGlobalizations.Select(ng => new ModuleGlobalization()
                    {
                        ModuleId = modulesId,
                        Title = ng.Title,
                        SubTitle = ng.SubTitle,
                        Description = ng.Description,
                        Content = ng.Content,
                        Culture = ng.Culture,
                        ImageId = oldData.ImageId,
                        ImageClass = ng.ImageClass

                    }).ToList()
                };

                message.ReturnId = _dModule.EditModule(System.Convert.ToInt32(model.Id), modules);

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
        public Message DeleteModule(string modulesId)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "删除成功";
            try
            {
                var id = System.Convert.ToInt32(modulesId);
                B_Image bImage = new B_Image();
                //删除图标
                var modules = _dModule.GetModuleById(id);
                //if (modules.ImageId != null)
                //    bImage.DeleteImage(System.Convert.ToInt32(modules.ImageId));

                _dModule.DeleteModule(id);

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
                foreach (var id in ids)
                {
                    var modulesGlobalizations = _dModule.GetModuleGlobalizationByModuleId(id);
                    foreach (var ng in modulesGlobalizations)
                    {
                        if (ng.ImageId != null)
                            bImage.DeleteImage(System.Convert.ToInt32(ng.ImageId));
                    }

                }
                _dModule.BatchDeleteModule(ids);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "删除失败：" + e.Message;
            }

            return message;
        }

        private M_Module ConverEntityToModel(Module modules)
        {
            if (modules == null) return null;

            B_Image bImage = new B_Image();

            var model = new M_Module()
            {
                Id = modules.Id.ToString(),
                ModuleURLId = modules.ModuleURLId,
                CreateDate = modules.CreateDate,
                Creator = modules.Creator,
                ModifiedDate = modules.ModifiedDate,
                Modifier = modules.Modifier,
                Sequence = modules.Sequence.ToString(),
                Type = modules.Type,
                ModuleGlobalizations = modules.ModuleGlobalizations.Select(ng => new M_ModuleGlobalization()
                {
                    Title = ng.Title,
                    SubTitle = ng.SubTitle,
                    Description = ng.Description,
                    Content = ng.Content,
                    Culture = ng.Culture,
                    ImageClass = ng.ImageClass,
                    Image = ng.ImageId == null ? null : bImage.GetImageById(System.Convert.ToInt32(ng.ImageId)),
                }).ToList()
            };
            return model;
        }

        private M_ModuleSearch ConverSearchEntityToModel(ModuleSearch modules)
        {
            if (modules == null) return null;

            B_Image bImage = new B_Image();

            var model = new M_ModuleSearch()
            {
                Id = modules.Id.ToString(),
                ModuleURLId = modules.ModuleURLId,
                Title = modules.Title,
                SubTitle = modules.SubTitle,
                Description = modules.Description,
                Content = modules.Content,
                Image = modules.ImageId == null ? null : bImage.GetImageById(System.Convert.ToInt32(modules.ImageId)),
                CreateDate = modules.CreateDate.ToString(),
                Creator = modules.Creator,
                ModifiedDate = modules.ModifiedDate.ToString(),
                Modifier = modules.Modifier,
                Sequence = modules.Sequence.ToString(),
                Type = modules.Type,
                Culture = modules.Culture
            };
            return model;
        }

        #region ModuleGlobalization
        public M_ModuleGlobalization GetModuleGlobalizationByModuleIdAndCulture(string modulesId, string culture)
        {
            int id = System.Convert.ToInt32(modulesId);
            var modulesGlobalization = _dModule.GetModuleGlobalizationByModuleIdAndCulture(id, culture);
            var result = ConverModuleGlobalizationEntityToModel(modulesGlobalization);
            return result;
        }

        public M_ModuleGlobalization GetModuleGlobalizationById(string modulesGlobalizationId)
        {

            int id = System.Convert.ToInt32(modulesGlobalizationId);
            var modulesGlobalization = _dModule.GetModuleGlobalizationById(id);
            var result = ConverModuleGlobalizationEntityToModel(modulesGlobalization);
            return result;
        }

        public M_ModuleGlobalization GetModuleGlobalizationByIdAndCulture(string modulesGlobalizationId, string culture)
        {

            int id = System.Convert.ToInt32(modulesGlobalizationId);
            var modulesGlobalization = _dModule.GetModuleGlobalizationByModuleIdAndCulture(id, culture);
            var result = ConverModuleGlobalizationEntityToModel(modulesGlobalization);
            return result;
        }

        /// <summary>
        /// 处理国际化数据
        /// </summary>
        /// <param name="model"></param>
        /// 
        /// <returns></returns>
        public Message DealModuleGlobalization(M_ModuleGlobalization model)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "新增成功";

            try
            {
                var moduleId = System.Convert.ToInt32(model.ModuleId);
                //根据modulesId和culture，查找已存在的数据
                var oldData = _dModule.GetModuleGlobalizationByModuleIdAndCulture(moduleId,model.Culture);
                //新增
                var modulesGlobalization = new ModuleGlobalization()
                {
                    ModuleId = moduleId,
                    Title = model.Title,
                    SubTitle = model.SubTitle,
                    Description = model.Description,
                    Content = model.Content,
                    Culture = model.Culture,
                    ImageId = oldData !=null?oldData.ImageId:null,
                    ImageClass = model.ImageClass
                };

                message.ReturnId = _dModule.AddModuleGlobalization(modulesGlobalization);

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
        public Message UpdateImageId(string modulesGlobalizationId, int imageId)
        {
            Message message = new Message();
            message.Success = true;
            message.Content = "图片更新成功";
            try
            {
                var id = System.Convert.ToInt32(modulesGlobalizationId);
                message.ReturnStrId = _dModule.EditImageId(id, imageId);

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "图片更新失败:" + e.Message;
            }

            return message;
        }

        private M_ModuleGlobalization ConverModuleGlobalizationEntityToModel(ModuleGlobalization modulesGlobalization)
        {
            if (modulesGlobalization == null) return null;

            B_Image bImage = new B_Image();

            var model = new M_ModuleGlobalization()
            {
                Id = modulesGlobalization.Id.ToString(),
                Title = modulesGlobalization.Title,
                SubTitle = modulesGlobalization.SubTitle,
                Description = modulesGlobalization.Description,
                Content = modulesGlobalization.Content,
                Culture = modulesGlobalization.Culture,
                ImageClass = modulesGlobalization.ImageClass,
                Image = modulesGlobalization.ImageId == null ? null :
                            bImage.GetImageById(System.Convert.ToInt32(modulesGlobalization.ImageId))

            };
            return model;
        }

        #endregion

    }
}
