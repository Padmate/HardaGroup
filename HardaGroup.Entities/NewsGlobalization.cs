using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Entities
{
    public class NewsGlobalization
    {
        public int Id { get; set; }

        /// <summary>
        /// 新闻标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 新闻副标题
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// 新闻描述
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// 内容
        /// </summary>
        [Column("Content", TypeName = "ntext")]
        public string Content { get; set; }

        /// <summary>
        /// 图片ID
        /// </summary>
        public int? ImageId { get; set; }

        /// <summary>
        /// 国际化代码
        /// </summary>
        public string Culture { get; set; }

        public int NewsId { get; set; }

        public virtual News News { get; set; }
    }
}
