using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Entities
{
    public class About
    {
        public int Id { get; set; }

        /// <summary>
        /// 类型代码
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 排列顺序
        /// </summary>
        public int Sequence { get; set; }

        public virtual ICollection<AboutGlobalization> AboutGlobalizations { get; set; }

        public About()
        {
            AboutGlobalizations = new List<AboutGlobalization>();

        }
    }

    public class AboutSearch
    {
        public int Id { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }

        public int Sequence { get; set; }
        public string Culture { get; set; }
        public string Content { get; set; }
    }
}
