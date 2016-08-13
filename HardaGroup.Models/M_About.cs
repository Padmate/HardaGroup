﻿using System;
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
        public string TypeCode { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        [Required(ErrorMessage = "类别名称不能为空")]
        [MaxLength(200, ErrorMessage = "类别名称长度不能超过200个字符")]
        public string TypeName { get; set; }

        /// <summary>
        /// 排列顺序
        /// </summary>
        [RegularExpression(@"^[1-9]\d*|0$", ErrorMessage = "顺序只能是0或正整数")]
        public string Sequence { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required(ErrorMessage = "内容不能为空")]
        public string Content { get; set; }

        /// <summary>
        /// 国际化代码
        /// </summary>
        [Required(ErrorMessage = "国际化代码不能为空")]
        [MaxLength(50, ErrorMessage = "国际化代码长度不能超过50个字符")]
        public string Culture { get; set; }
    }
}
