using HardaGroup.Entities;
using HardaGroup.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.DataAccess
{
    public class D_Module
    {
        HardaDBContext _dbContext = new HardaDBContext();

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="module"></param>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<ModuleSearch> GetPageData(ModuleSearch moduleSearch, int skip, int limit)
        {
            //根据国际化代码获取数据，
            var culture = moduleSearch.Culture;
            if (string.IsNullOrEmpty(culture))
            {
                culture = Common.Globalization_Chinese;
            }
            var query = from a in _dbContext.Modules
                        join ag in _dbContext.ModuleGlobalizations
                        on new { moduleId = a.Id, culture = culture }
                        equals new { moduleId = ag.ModuleId, culture = ag.Culture }
                        into temp
                        from tt in temp.DefaultIfEmpty()
                        //左链接国际化代码为中文的数据
                        join zhcndata in _dbContext.ModuleGlobalizations
                        on new { moduleId = a.Id, culture = Common.Globalization_Chinese }
                        equals new { moduleId = zhcndata.ModuleId, culture = zhcndata.Culture }
                        into tempzhcndata
                        from ttzhcndata in tempzhcndata.DefaultIfEmpty()

                        select new ModuleSearch
                        {
                            Id = a.Id,
                            Sequence = a.Sequence,
                            Type = a.Type,
                            CreateDate = a.CreateDate,
                            Creator = a.Creator,
                            ModifiedDate = a.ModifiedDate,
                            Modifier = a.Modifier,
                            ModuleURLId = a.ModuleURLId,
                            //如果当前culture获取不到数据，则默认取中文数据
                            ImageId = tt == null ? ttzhcndata.ImageId : tt.ImageId,
                            ImageClass = tt == null ? ttzhcndata.ImageClass : tt.ImageClass,
                            Title = tt == null ? ttzhcndata.Title : tt.Title,
                            SubTitle = tt == null ? ttzhcndata.SubTitle : tt.SubTitle,
                            Href = tt == null ? ttzhcndata.Href : tt.Href,
                            Content = tt == null ? ttzhcndata.Content : tt.Content,
                            Culture = tt == null ? ttzhcndata.Culture : tt.Culture,
                            Description = tt == null ? ttzhcndata.Description : tt.Description,

                        };

            #region　条件过滤
            if (!string.IsNullOrEmpty(moduleSearch.Title))
                query = query.Where(a => a.Title.Contains(moduleSearch.Title));
            if (!string.IsNullOrEmpty(moduleSearch.SubTitle))
                query = query.Where(a => a.SubTitle.Contains(moduleSearch.SubTitle));
            if (!string.IsNullOrEmpty(moduleSearch.Type))
                query = query.Where(a => a.Type == moduleSearch.Type);
            #endregion

            var result = query.OrderBy(a => a.Sequence)
            .Skip(skip)
            .Take(limit)
            .ToList();

            return result;

        }

        public int GetPageDataTotalCount(ModuleSearch moduleSearch)
        {
            var query = from a in _dbContext.Modules
                        join ag in _dbContext.ModuleGlobalizations
                        on new { moduleId = a.Id, culture = Common.Globalization_Chinese }
                        equals new { moduleId = ag.ModuleId, culture = ag.Culture }
                        into temp
                        from tt in temp.DefaultIfEmpty()
                        select new ModuleSearch
                        {
                            Id = a.Id,
                            Type = a.Type,
                            Sequence = a.Sequence,
                            CreateDate = a.CreateDate,
                            Creator = a.Creator,
                            ModifiedDate = a.ModifiedDate,
                            Modifier = a.Modifier,
                            ModuleURLId = a.ModuleURLId,
                            ImageId = tt == null ? null : tt.ImageId,
                            ImageClass = tt == null ? null : tt.ImageClass,
                            Title = tt == null ? "" : tt.Title,
                            SubTitle = tt == null ? "" : tt.SubTitle,
                            Href = tt == null ? "" : tt.Href,
                            Content = tt == null ? "" : tt.Content,
                            Culture = tt == null ? "" : tt.Culture,
                            Description = tt == null ? "" : tt.Description,

                        };

            #region　条件过滤
            if (!string.IsNullOrEmpty(moduleSearch.Title))
                query = query.Where(a => a.Title.Contains(moduleSearch.Title));
            if (!string.IsNullOrEmpty(moduleSearch.SubTitle))
                query = query.Where(a => a.SubTitle.Contains(moduleSearch.SubTitle));
            if (!string.IsNullOrEmpty(moduleSearch.Type))
                query = query.Where(a => a.Type == moduleSearch.Type);
            #endregion

            var result = query.ToList().Count();

            return result;
        }

        /// <summary>
        /// 根据ID查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Module GetModuleById(int id)
        {
            var module = _dbContext.Modules.Include("ModuleGlobalizations").FirstOrDefault(a => a.Id == id);
            return module;
        }

        /// <summary>
        /// 根据类型查找
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Module> GetModuleByType(string type)
        {
            var modules = _dbContext.Modules
                .Include("ModuleGlobalizations")
                .Where(a => a.Type == type)
                .OrderBy(v=>v.Sequence)
                .ToList();
            return modules;
        }

        
        public Module GetModuleByModuleUrlId(string moduleUrlId)
        {
            var module = _dbContext.Modules
                .Include("ModuleGlobalizations")
                .FirstOrDefault(a => a.ModuleURLId == moduleUrlId);
            return module;
        }

        public List<Module> GetAll()
        {
            var modules = _dbContext.Modules.Include("ModuleGlobalizations")
                .ToList();

            return modules;
        }

        public List<ModuleSearch> GetByMulitCondition(ModuleSearch searchModel)
        {

            var query = from a in _dbContext.Modules
                        join ag in _dbContext.ModuleGlobalizations
                        on new { moduleId = a.Id}
                        equals new { moduleId = ag.ModuleId}
                        into temp
                        from tt in temp.DefaultIfEmpty()
                       
                        select new ModuleSearch
                        {
                            Id = a.Id,
                            Sequence = a.Sequence,
                            Type = a.Type,
                            CreateDate = a.CreateDate,
                            Creator = a.Creator,
                            ModifiedDate = a.ModifiedDate,
                            Modifier = a.Modifier,
                            ModuleURLId = a.ModuleURLId,
                            //如果当前culture获取不到数据，则默认取中文数据
                            ImageId = tt == null ? null : tt.ImageId,
                            ImageClass = tt == null ? "" : tt.ImageClass,
                            Title = tt == null ? "" : tt.Title,
                            SubTitle = tt == null ? "" : tt.SubTitle,
                            Href = tt == null ? "" : tt.Href,
                            Content = tt == null ? "" : tt.Content,
                            Culture = tt == null ? "" : tt.Culture,
                            Description = tt == null ? "" : tt.Description,

                        };

            #region　条件过滤
            if (!string.IsNullOrEmpty(searchModel.Title))
                query = query.Where(a => a.Title.Contains(searchModel.Title));
            if (!string.IsNullOrEmpty(searchModel.SubTitle))
                query = query.Where(a => a.SubTitle.Contains(searchModel.SubTitle));
            if (!string.IsNullOrEmpty(searchModel.Type))
                query = query.Where(a => a.Type == searchModel.Type);
            if (!string.IsNullOrEmpty(searchModel.Culture))
                query = query.Where(a => a.Culture == searchModel.Culture);
            #endregion

            var result = query.OrderBy(a => a.Sequence).ToList();

            return result;
        }

        public List<Module> GetByMulitCondition(Module searchModel)
        {
            var query = _dbContext.Modules.Include("ModuleGlobalizations").Where(n => 1 == 1);

            #region　条件过滤
            if (!string.IsNullOrEmpty(searchModel.ModuleURLId))
                query = query.Where(a => a.ModuleURLId == searchModel.ModuleURLId);
            if (!string.IsNullOrEmpty(searchModel.Type))
                query = query.Where(a => a.Type == searchModel.Type);
            #endregion

            var result = query.OrderBy(a => a.Sequence)
            .ToList();

            return result;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public int AddModule(Module module)
        {
            _dbContext.Modules.Add(module);
            _dbContext.SaveChanges();
            return module.Id;
        }

        public int EditModule(int id, Module model)
        {
            var module = _dbContext.Modules.FirstOrDefault(a => a.Id == id);

            module.Type = model.Type;
            module.ModifiedDate = model.ModifiedDate;
            module.Modifier = model.Modifier;
            module.Sequence = model.Sequence;
            module.ModuleURLId = model.ModuleURLId;

            var culture = model.ModuleGlobalizations.First().Culture;
            //先删除原来子数据
            var ag = _dbContext.ModuleGlobalizations
                .Where(a => a.ModuleId == module.Id && a.Culture == culture);

            _dbContext.ModuleGlobalizations.RemoveRange(ag);

            _dbContext.ModuleGlobalizations.AddRange(model.ModuleGlobalizations);

            _dbContext.SaveChanges();
            return module.Id;
        }


        public void DeleteModule(int id)
        {
            var module = _dbContext.Modules.Where(i => i.Id == id).FirstOrDefault();
            if (module != null)
            {
                _dbContext.Modules.Remove(module);
                _dbContext.SaveChanges();
            }
        }

        public void BatchDeleteModule(List<int> ids)
        {
            var modules = _dbContext.Modules.Where(i => ids.Contains(i.Id)).ToList();
            if (modules.Count > 0)
            {
                _dbContext.Modules.RemoveRange(modules);
                _dbContext.SaveChanges();
            }
        }

        #region ModuleGlobalization

        public ModuleGlobalization GetModuleGlobalizationById(int moduleGlobalizationId)
        {
            var moduleGlobalization = _dbContext.ModuleGlobalizations.FirstOrDefault(a => a.Id == moduleGlobalizationId);
            return moduleGlobalization;
        }

        public List<ModuleGlobalization> GetModuleGlobalizationByModuleId(int moduleId)
        {
            var moduleGlobalization = _dbContext.ModuleGlobalizations.Where(a => a.ModuleId == moduleId).ToList();
            return moduleGlobalization;
        }

        /// <summary>
        /// 修改图片ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public string EditImageId(int moduleGlobalizationId, int imageId)
        {
            var moduleGlobalization = _dbContext.ModuleGlobalizations.FirstOrDefault(a => a.Id == moduleGlobalizationId);

            moduleGlobalization.ImageId = imageId;

            _dbContext.SaveChanges();
            return moduleGlobalization.Id.ToString();
        }


        public ModuleGlobalization GetModuleGlobalizationByModuleIdAndCulture(int moduleId, string culture)
        {
            var result = _dbContext.ModuleGlobalizations.Where(ag => ag.ModuleId == moduleId && ag.Culture == culture).FirstOrDefault();


            return result;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="about"></param>
        /// <returns></returns>
        public int AddModuleGlobalization(ModuleGlobalization moduleGlobalization)
        {
            var existData = _dbContext.ModuleGlobalizations
                .Where(ag => ag.ModuleId == moduleGlobalization.ModuleId && ag.Culture == moduleGlobalization.Culture);
            if (existData.Count() > 0)
            {
                _dbContext.ModuleGlobalizations.RemoveRange(existData);
            }
            //重新添加
            _dbContext.ModuleGlobalizations.Add(moduleGlobalization);

            _dbContext.SaveChanges();
            return moduleGlobalization.Id;
        }
        #endregion
    }
}
