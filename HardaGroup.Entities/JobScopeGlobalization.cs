using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Entities
{
    public class JobScopeGlobalization
    {
        public int Id { get; set; }
        /// <summary>
        /// 职位分类名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 国际化代码
        /// </summary>
        public string Culture { get; set; }

        public int JobScopeId { get; set; }

        public virtual JobScope JobScope { get; set; }
    }
}
