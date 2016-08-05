using System;
using System.Collections.Generic;
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
        public string VirtualPath { get; set; }

        /// <summary>
        /// 图片物理路径
        /// </summary>
        public string PhysicalPath { get; set; }

        /// <summary>
        /// 图片原始名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图片保存名称
        /// </summary>
        public string SaveName { get; set; }

        /// <summary>
        /// 图片后缀
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 图片排列顺序
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 图片所属类型
        /// </summary>
        public string Type { get; set; }
    }
}
