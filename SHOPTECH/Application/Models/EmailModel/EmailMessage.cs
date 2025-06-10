using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.EmailModel
{
    public class EmailMessage
    {
        public List<EmailRecipient> Recipients { get; set; } = new List<EmailRecipient>();
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; } = true;
        public List<EmailAttachment> Attachments { get; set; } = new List<EmailAttachment>();
        public string ReplyTo { get; set; }
        public int Priority { get; set; } = 3; // 1=High, 3=Normal, 5=Low
    }
}
