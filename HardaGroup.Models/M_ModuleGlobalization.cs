using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Models
{
    public class M_ModuleGlobalization:BaseModel
    {
        public string Id { get; set; }

        public string ModuleId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        [Required(ErrorMessage = "二级标题不能为空")]
        public string SubTitle { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(200, ErrorMessage = "描述长度不能超过200个字符")]
        public string Description { get; set; }

        public bool IsHref { get; set; }
        /// <summary>
        /// 链接URL
        /// </summary>
        [RegularExpression(@"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?", ErrorMessage = "链接格式不正确")]
        public string Href { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public M_Image Image { get; set; }

        public string ImageClass { get; set; }

        /// <summary>
        /// 国际化代码
        /// </summary>
        public string Culture { get; set; }
    }
}
