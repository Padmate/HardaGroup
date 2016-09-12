using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Entities
{
    /// <summary>
    /// 内容模块
    /// </summary>
    public class Module
    {
        public int Id { get; set; }

        /// <summary>
        /// 资讯URL唯一标示，用于显示在URL中
        /// </summary>
        public string ModuleURLId { get; set; }

        /// <summary>
        /// 排列顺序
        /// </summary>
        public int Sequence{get;set;}

        /// <summary>
        /// 模块类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<ModuleGlobalization> ModuleGlobalizations { get; set; }

        public Module()
        {
            ModuleGlobalizations = new List<ModuleGlobalization>();
        }
    }

    public class ModuleSearch
    {
        public int Id { get; set; }

        /// <summary>
        /// 资讯URL唯一标示，用于显示在URL中
        /// </summary>
        public string ModuleURLId { get; set; }

        public string Type { get; set; }

        public int Sequence { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifiedDate { get; set; }


        /// <summary>
        /// 新闻标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 新闻副标题
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// 新闻描述
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 图片ID
        /// </summary>
        public int? ImageId { get; set; }

        public string ImageClass { get; set; }

        /// <summary>
        /// 国际化代码
        /// </summary>
        public string Culture { get; set; }

    }
}
