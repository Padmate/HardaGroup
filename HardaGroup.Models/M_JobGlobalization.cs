using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Models
{
    public class M_JobGlobalization:BaseModel
    {
        public string Id { get; set; }

        public string JobId { get; set; }
        /// <summary>
        /// 工作地点
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        [Required(ErrorMessage = "职位名称不能为空")]
        public string JobTitle { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Required(ErrorMessage = "职位描述不能为空")]
        [MaxLength(200, ErrorMessage = "职位描述长度不能超过200个字符")]
        public string Description { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required(ErrorMessage = "内容不能为空")]
        public string Content { get; set; }


        /// <summary>
        /// 国际化代码
        /// </summary>
        public string Culture { get; set; }
    }
}
