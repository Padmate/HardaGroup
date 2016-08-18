using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Entities
{
    public class NewsScope
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 类型代码
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Sequence {get;set;}

        /// <summary>
        /// 国际化代码
        /// </summary>
        public string Culture { get; set; }

        public virtual ICollection<News> News { get; set; }

        public NewsScope()
        {
            News = new List<News>();
        }
    }
}
