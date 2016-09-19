using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Entities
{
    public class Job
    {
        public int Id { get; set; }

        /// <summary>
        /// URL唯一标示，用于显示在URL中
        /// </summary>
        public string URLId { get; set; }

        /// <summary>
        /// 是否为热门职位
        /// </summary>
        public bool IsHot { get; set; }

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
        /// 发表时间
        /// </summary>
        public DateTime Pubtime { get; set; }


        public int JobScopeId { get; set; }

        public virtual JobScope JobScope { get; set; }

        public virtual ICollection<JobGlobalization> JobGlobalizations { get; set; }

        public Job()
        {
            JobGlobalizations = new List<JobGlobalization>();
        }
    }

    public class JobSearch
    {
        public int Id { get; set; }

        /// <summary>
        /// URL唯一标示，用于显示在URL中
        /// </summary>
        public string URLId { get; set; }

        public string IsHotSearch { get; set; }
        /// <summary>
        /// 是否为热门职位
        /// </summary>
        public bool IsHot { get; set; }

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
        /// 发表时间
        /// </summary>
        public DateTime Pubtime { get; set; }


        public int? JobScopeId { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// 职位描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 工作地点
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 职位内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 国际化代码
        /// </summary>
        public string Culture { get; set; }
    }
}
