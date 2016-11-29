using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Entities
{
    public class MailAttachment
    {
        public int Id { get; set; }
        public int MailId { get; set; }

        /// <summary>
        /// 包含url的完整文件名
        /// </summary>
        public int FileName { get; set; }

        public virtual Mail Mail { get; set; }
    }
}
