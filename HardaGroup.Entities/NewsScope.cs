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
        public int Id { get; set; }

        /// <summary>
        /// 类型代码
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Sequence {get;set;}

        public virtual ICollection<NewsScopeGlobalization> NewsScopeGlobalizations { get; set; }

        public virtual ICollection<News> News { get; set; }

        public NewsScope()
        {
            NewsScopeGlobalizations = new List<NewsScopeGlobalization>();
            News = new List<News>();
        }
    }

    public class NewsScopeSearch
    {
        public int Id {get;set;}

        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public string Culture { get; set; }
        public int Sequence { get; set; }
    }
}
