﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Entities
{
    /// <summary>
    /// 媒体资讯实体类
    /// </summary>
    public class News
    {
        public int Id { get; set; }

        /// <summary>
        /// 资讯URL唯一标示，用于显示在URL中
        /// </summary>
        public string NewsURLId { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// 发表时间
        /// </summary>
        public DateTime Pubtime { get; set; }


        public int NewsScopeId { get; set; }

        public virtual NewsScope NewsScope { get; set; }

        public virtual ICollection<NewsGlobalization> NewsGlobalizations { get; set; }

        public News()
        {
            NewsGlobalizations = new List<NewsGlobalization>();
        }

    }

    public class NewsSearch
    {
        public int Id { get; set; }

        public int? NewsScopeId { get; set; }
        /// <summary>
        /// 资讯URL唯一标示，用于显示在URL中
        /// </summary>
        public string NewsURLId { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// 发表时间
        /// </summary>
        public DateTime Pubtime { get; set; }

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
        public string Content { get; set; }

        /// <summary>
        /// 图片ID
        /// </summary>
        public int? ImageId { get; set; }

        /// <summary>
        /// 国际化代码
        /// </summary>
        public string Culture { get; set; }

    }

}
