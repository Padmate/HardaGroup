using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Models
{
    public class M_About : BaseModel
    {
        public string Id { get; set; }

        /// <summary>
        /// 类型代码
        /// </summary>
        [Required(ErrorMessage="类别代码不能为空")]
        [MaxLength(100,ErrorMessage = "类别代码长度不能超过100个字符")]
        [RegularExpression(@"[A-Za-z0-9-]+", ErrorMessage = "类别代码只能是字母、数字或 - ")]
        public string TypeCode { get; set; }

        /// <summary>
        /// 排列顺序
        /// </summary>
        [RegularExpression(@"^[1-9]\d*|0$", ErrorMessage = "顺序只能是0或正整数")]
        public string Sequence { get; set; }

        
        public List<M_AboutGlobalization> AboutGlobalizations { get; set; }

    }

    public class M_AboutSearch:BaseModel
    {
        public string Id { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }

        public string Sequence { get; set; }
        public string Culture { get; set; }
        public string Content { get; set; }
    }
}
