using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Models
{
    public class M_Job : BaseModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "职位类型不能为空")]
        public string JobScopeId { get; set; }

        [Required(ErrorMessage = "URL标识不能为空")]
        [MaxLength(200, ErrorMessage = "URL标识不能超过200个字符")]
        [RegularExpression(@"[A-Za-z0-9-]+", ErrorMessage = "URL标识只能是字母、数字或 - ")]
        public string URLId { get; set; }

        /// <summary>
        /// 设置为热门职位
        /// </summary>
        public bool IsHot { get; set; }

        /// <summary>
        /// 发表时间
        /// </summary>
        [Required(ErrorMessage = "发布时间不能为空")]
        public DateTime Pubtime { get; set; }

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

        public List<M_JobGlobalization> JobGlobalizations { get; set; }

    }

    public class M_JobSearch : BaseModel
    {
        public string Id { get; set; }

        public string JobScopeId { get; set; }

        public string URLId { get; set; }

        public string IsHotSearch { get; set; }
        /// <summary>
        /// 设置为热门职位
        /// </summary>
        public bool IsHot { get; set; }

        /// <summary>
        /// 工作地点
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发表时间
        /// </summary>
        public DateTime Pubtime { get; set; }

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

        public string Culture { get; set; }

        public List<M_JobGlobalization> JobGlobalizations { get; set; }

        public string ScopeTypeCode { get; set; }

        public string ScopeTypeName { get; set; }

    }
}
