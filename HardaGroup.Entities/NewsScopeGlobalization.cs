﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Entities
{
    public class NewsScopeGlobalization
    {
        public int Id { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 国际化代码
        /// </summary>
        public string Culture { get; set; }

        public int NewsScopeId { get; set; }

        public virtual NewsScope NewsScope { get; set; }
    }
}