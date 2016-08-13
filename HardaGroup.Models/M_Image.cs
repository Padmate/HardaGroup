using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Models
{
    public class M_Image
    {
        public int Id { get; set; }
        /// <summary>
        /// 图片虚拟路径
        /// </summary>
        [Required(ErrorMessage = "图片虚拟路径不能为空")]
        public string VirtualPath { get; set; }

        /// <summary>
        /// 图片物理路径
        /// </summary>
        public string PhysicalPath { get; set; }

        /// <summary>
        /// 图片原始名称
        /// </summary>
        [Required(ErrorMessage = "图片原始名称不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 图片保存名称
        /// </summary>
        [Required(ErrorMessage = "图片保存名称不能为空")]
        public string SaveName { get; set; }

        /// <summary>
        /// 图片后缀
        /// </summary>
        [Required(ErrorMessage = "图片后缀不能为空")]
        [MaxLength(10, ErrorMessage = "图片后缀不能超过10个字符")]
        public string Extension { get; set; }

        /// <summary>
        /// 图片排列顺序
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 图片所属类型
        /// </summary>
        [Required(ErrorMessage = "图片类型不能为空")]
        [MaxLength(50, ErrorMessage = "图片类型不能超过50个字符")]
        public string Type { get; set; }

        /// <summary>
        /// 国际化代码
        /// </summary>
        [Required(ErrorMessage="国际化代码不能为空")]
        public string Culture { get; set; }
    }
}
