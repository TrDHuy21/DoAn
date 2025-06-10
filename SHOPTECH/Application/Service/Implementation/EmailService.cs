using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.EmailModel;
using Application.Service.Interface;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Application.Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(EmailMessage emailMessage)
        {
            try
            {
                var message = new MimeMessage();

                // Người gửi
                message.From.Add(new MailboxAddress(_emailSettings.SenderName ?? "System", _emailSettings.SenderEmail));

                // Reply-To
                if (!string.IsNullOrEmpty(emailMessage.ReplyTo))
                {
                    message.ReplyTo.Add(new MailboxAddress("", emailMessage.ReplyTo));
                }

                // Người nhận
                foreach (var recipient in emailMessage.Recipients)
                {
                    var mailboxAddress = new MailboxAddress(recipient.Name ?? "", recipient.Email);

                    switch (recipient.Type)
                    {
                        case RecipientType.To:
                            message.To.Add(mailboxAddress);
                            break;
                        case RecipientType.Cc:
                            message.Cc.Add(mailboxAddress);
                            break;
                        case RecipientType.Bcc:
                            message.Bcc.Add(mailboxAddress);
                            break;
                    }
                }

                // Tiêu đề
                message.Subject = emailMessage.Subject;

                // Độ ưu tiên
                switch (emailMessage.Priority)
                {
                    case 1:
                        message.Priority = MessagePriority.Urgent;
                        break;
                    case 5:
                        message.Priority = MessagePriority.NonUrgent;
                        break;
                    default:
                        message.Priority = MessagePriority.Normal;
                        break;
                }

                // Tạo body
                var bodyBuilder = new BodyBuilder();

                if (emailMessage.IsHtml)
                {
                    bodyBuilder.HtmlBody = emailMessage.Body;
                }
                else
                {
                    bodyBuilder.TextBody = emailMessage.Body;
                }

                // Thêm tệp đính kèm
                foreach (var attachment in emailMessage.Attachments)
                {
                    bodyBuilder.Attachments.Add(
                        attachment.FileName,
                        attachment.FileContent,
                        ContentType.Parse(attachment.ContentType)
                    );
                }

                message.Body = bodyBuilder.ToMessageBody();

                // Gửi email
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, _emailSettings.EnableSsl);
                    await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                _logger.LogInformation($"Email sent successfully to {string.Join(", ", emailMessage.Recipients.Select(r => r.Email))}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendSimpleEmailAsync(string toEmail, string subject, string body, bool isHtml = true)
        {
            var emailMessage = new EmailMessage
            {
                Recipients = new List<EmailRecipient>
            {
                new EmailRecipient { Email = toEmail, Type = RecipientType.To }
            },
                Subject = subject,
                Body = body,
                IsHtml = isHtml
            };

            return await SendEmailAsync(emailMessage);
        }

        public async Task<bool> SendEmailWithAttachmentAsync(string toEmail, string subject, string body, string attachmentPath)
        {
            try
            {
                var fileBytes = await File.ReadAllBytesAsync(attachmentPath);
                var fileName = Path.GetFileName(attachmentPath);
                var contentType = GetContentType(fileName);

                var emailMessage = new EmailMessage
                {
                    Recipients = new List<EmailRecipient>
                {
                    new EmailRecipient { Email = toEmail, Type = RecipientType.To }
                },
                    Subject = subject,
                    Body = body,
                    Attachments = new List<EmailAttachment>
                {
                    new EmailAttachment
                    {
                        FileName = fileName,
                        FileContent = fileBytes,
                        ContentType = contentType
                    }
                }
                };

                return await SendEmailAsync(emailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email with attachment: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendBulkEmailAsync(List<string> toEmails, string subject, string body, bool isHtml = true)
        {
            var emailMessage = new EmailMessage
            {
                Recipients = toEmails.Select(email => new EmailRecipient
                {
                    Email = email,
                    Type = RecipientType.Bcc // Dùng BCC để ẩn danh sách người nhận
                }).ToList(),
                Subject = subject,
                Body = body,
                IsHtml = isHtml
            };

            // Thêm 1 người nhận To để tránh lỗi (có thể là chính email người gửi)
            emailMessage.Recipients.Insert(0, new EmailRecipient
            {
                Email = _emailSettings.SenderEmail,
                Type = RecipientType.To
            });

            return await SendEmailAsync(emailMessage);
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".txt" => "text/plain",
                ".html" => "text/html",
                ".zip" => "application/zip",
                ".rar" => "application/x-rar-compressed",
                _ => "application/octet-stream"
            };
        }
    }
}
