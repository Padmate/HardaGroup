using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Entities
{
    public class JobGlobalization
    {
        public int Id { get; set; }

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
        [Column("Content", TypeName = "ntext")]
        public string Content { get; set; }

        /// <summary>
        /// 国际化代码
        /// </summary>
        public string Culture { get; set; }

        public int JobId { get; set; }

        public virtual Job Job { get; set; }
    }
}
