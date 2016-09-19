using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Entities
{
    public class JobScope
    {
        public int Id { get; set; }

        /// <summary>
        /// 职位类型代码
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Sequence {get;set;}

        public virtual ICollection<JobScopeGlobalization> JobScopeGlobalizations { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }

        public JobScope()
        {
            JobScopeGlobalizations = new List<JobScopeGlobalization>();
            Jobs = new List<Job>();
        }
    }

    public class JobScopeSearch
    {
        public int Id { get; set; }

        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public string Culture { get; set; }
        public int Sequence { get; set; }
    }
}
