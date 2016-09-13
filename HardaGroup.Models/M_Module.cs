using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Models
{
    public class M_Module:BaseModel
    {
        public string Id { get; set; }


        [Required(ErrorMessage = "URL标识不能为空")]
        [MaxLength(200, ErrorMessage = "URL标识不能超过200个字符")]
        [RegularExpression(@"[A-Za-z0-9-]+", ErrorMessage = "URL标识只能是字母、数字或 - ")]
        public string ModuleURLId { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        [RegularExpression(@"^[1-9]\d*|0$", ErrorMessage = "顺序只能是0或正整数")]
        public string Sequence { get; set; }

        /// <summary>
        /// 模块类型
        /// </summary>
        [Required(ErrorMessage = "类型不能为空")]
        [MaxLength(200, ErrorMessage = "类型不能超过200个字符")]
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

        public List<M_ModuleGlobalization> ModuleGlobalizations { get; set; }
    }

    public class M_ModuleSearch:BaseModel
    {
        public string Id { get; set; }

        public string ModuleURLId { get; set; }

        public string Sequence { get; set; }

        public string Type { get; set; }

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

        private bool isHref;
        public bool IsHref { get; set; }
        /// <summary>
        /// 链接URL
        /// </summary>
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
        /// 创建者
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifiedDate { get; set; }

        public string Culture { get; set; }

        public List<M_ModuleGlobalization> ModuleGlobalizations { get; set; }
    }
}
