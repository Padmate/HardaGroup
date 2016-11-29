using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Entities
{
    public class Mail
    {
        public int Id { get; set; }

        /// <summary>
        /// 如果有多个，格式为123@qq.com,456@qq.com
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// 如果有多个，格式为123@qq.com,456@qq.com
        /// </summary>
        public string To { get; set; }
        public string Subject { get; set; }

        /// <summary>
        /// 抄送
        /// 如果有多个，格式为123@qq.com,456@qq.com
        /// </summary>
        public string Cc { get; set; }

        /// <summary>
        /// 邮件内容
        /// </summary>
        [Column("Body", TypeName = "ntext")]
        public string Body { get; set; }

        /// <summary>
        /// 邮件创建人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 邮件创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 邮件修改者
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 邮件修改时间
        /// </summary>
        public DateTime? ModifiedDate { get; set; }


        /// <summary>
        /// 邮件发送时间
        /// </summary>
        public DateTime? SendDate { get; set; }

        /// <summary>
        /// 邮件发送标记
        /// </summary>
        [DefaultValue(false)]
        public bool SendTag { get; set; }


        /// <summary>
        /// 是否读取邮件
        /// </summary>
        [DefaultValue(false)]
        public bool ReadTag { get; set; }

        /// <summary>
        /// 阅读邮件日期
        /// </summary>
        public DateTime? ReadDate { get; set; }

        public virtual ICollection<MailAttachment> MailAttachments { get; set; }

        public Mail()
        {
            MailAttachments = new List<MailAttachment>();
        }
    }
}
