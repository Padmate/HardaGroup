using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Models
{
    public class M_News : BaseModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "资讯模块不能为空")]
        public string NewsScopeId { get; set; }

        [Required(ErrorMessage = "资讯标识不能为空")]
        [MaxLength(200, ErrorMessage = "资讯标识不能超过200个字符")]
        [RegularExpression(@"[A-Za-z0-9-]+", ErrorMessage = "资讯标识只能是字母、数字或 - ")]
        public string NewsURLId { get; set; }

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

        public List<M_NewsGlobalization> NewsGlobalizations { get; set; }

    }

    public class M_NewsSearch : BaseModel
    {
        public string Id { get; set; }

        public string NewsScopeId { get; set; }

        public string NewsURLId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public M_Image Image { get; set; }

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

        public List<M_NewsGlobalization> NewsGlobalizations { get; set; }

    }
}
