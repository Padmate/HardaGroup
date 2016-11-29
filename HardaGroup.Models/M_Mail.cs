using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Models
{
    public class M_Mail : BaseModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "发件人不能为空")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "发件人邮箱格式不正确")]
        public string From { get; set; }

        /// <summary>
        /// 如果有多个，格式为123@qq.com,456@qq.com
        /// </summary>
        [Required(ErrorMessage = "收件人不能为空")]
        [RegularExpression(@"^(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+([;.](([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}){1,25})+)*$"
            , ErrorMessage = "收件人邮箱格式不正确，多个邮箱请用';'隔开")]
        public string To { get; set; }

        /// <summary>
        /// 抄送
        /// 如果有多个，格式为123@qq.com,456@qq.com
        /// </summary>
        public string Cc { get; set; }

        [Required(ErrorMessage = "邮件主题不能为空")]
        [MaxLength(2000, ErrorMessage = "邮件主题不能超过2000个字符")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "邮件内容不能为空")]
        public string Body { get; set; }

        /// <summary>
        /// 邮件创建人
        /// </summary>
        [MaxLength(50, ErrorMessage = "邮件创建人不能超过50个字符")]
        public string Creator { get; set; }

        /// <summary>
        /// 邮件创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 邮件修改者
        /// </summary>
        [MaxLength(50, ErrorMessage = "邮件修改人不能超过50个字符")]
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
        public bool SendTag { get; set; }

        public string SendTagSearch { get; set; }

        /// <summary>
        /// 是否读取邮件
        /// </summary>
        public bool ReadTag { get; set; }

        public string ReadTagSearch { get; set; }

        /// <summary>
        /// 阅读邮件日期
        /// </summary>
        public DateTime? ReadDate { get; set; }

    }

    /// <summary>
    /// 邮件服务器
    /// </summary>
    public class MailSmtp
    {
        public string Server { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
    }

    public class MailConfig
    {
        /// <summary>
        /// 收件人
        /// 如果有多个，格式为123@qq.com,456@qq.com
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// 抄送
        /// 如果有多个，格式为123@qq.com,456@qq.com
        /// </summary>
        public string Cc { get; set; }
    }
}
