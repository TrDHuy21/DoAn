using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.EmailModel;

namespace Application.Service.Interface
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailMessage emailMessage);
        Task<bool> SendSimpleEmailAsync(string toEmail, string subject, string body, bool isHtml = true);
        Task<bool> SendEmailWithAttachmentAsync(string toEmail, string subject, string body, string attachmentPath);
        Task<bool> SendBulkEmailAsync(List<string> toEmails, string subject, string body, bool isHtml = true);
    }
}
