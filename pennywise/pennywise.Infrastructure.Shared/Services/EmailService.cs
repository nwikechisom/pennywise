using System;
using System.Collections.Generic;
using System.Linq;
using pennywise.Application.DTOs.Email;
using pennywise.Application.Exceptions;
using pennywise.Application.Interfaces;
using pennywise.Domain.Settings;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;
using AutoMapper;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Mailjet.Client.TransactionalEmails.Response;
using Newtonsoft.Json;
using Attachment = System.Net.Mail.Attachment;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace pennywise.Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        public MailSettings _mailSettings { get; }
        public ILogger<EmailService> _logger { get; }

        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task SendAsync(EmailRequest request)
        {   
            try
            {
                MailjetClient client = new MailjetClient(_mailSettings.MailjetKey,_mailSettings.MailjetSecret);
                MailjetRequest mailjetRequest = new MailjetRequest
                {
                    Resource = SendV31.Resource,
                };
                var email = new TransactionalEmailBuilder()
                    .WithFrom(new SendContact(_mailSettings.EmailFrom))
                    .WithSubject(request.Subject)
                    .WithHtmlPart(request.Body)
                    .WithTo(new SendContact(request.To))
                    .Build();
                TransactionalEmailResponse response = await client.SendTransactionalEmailAsync(email);
                if (response.Messages.First().Errors != null)
                {
                    var errorMsg = ""; 
                    response.Messages.First().Errors.ForEach(x => errorMsg += $"{x.ErrorMessage}, \n");
                    throw new ApiException(errorMsg);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred on SendEmailAsync {@request}", request);
                throw new ApiException(ex.Message);
            }
        }
    }
}
