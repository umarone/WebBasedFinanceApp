using MailKit.Net.Smtp;
using MimeKit;
using MudasirRehmanAlp.ModelsView;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Threading.Tasks;


namespace MudasirRehmanAlp.Infrastructure.AppServices
{
    public class EmailkitService
    {
        private async Task SendAsync(MailSettingViewModel mailSetting, MimeMessage message)
        {
            //EmailConfigurationOption _emailConfiguration = new EmailConfigurationOption();
            //SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            //_emailConfiguration.From = section.From;
            //_emailConfiguration.Host = section.Network.Host;
            //_emailConfiguration.Port = section.Network.Port;
            //_emailConfiguration.EnableSsl = section.Network.EnableSsl;
            //_emailConfiguration.Username = section.Network.UserName;
            //_emailConfiguration.Password = section.Network.Password;
            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(mailSetting.HostEmail,mailSetting.PortNo, mailSetting.EnableSSL);
                smtpClient.AuthenticationMechanisms.Clear();
                smtpClient.AuthenticationMechanisms.Add("PLAIN");
                await smtpClient.AuthenticateAsync(mailSetting.UserNameEmail, mailSetting.Password);
                await smtpClient.SendAsync(message);
                await smtpClient.DisconnectAsync(true);

            }
        }

        [Obsolete]
        public async Task<bool> SendAsync(MailSettingViewModel mailSetting, string recipients, List<string> ccRecipients, string subject, string mailBody)
        {
            var mimeMessage = new MimeMessage();


            mimeMessage.To.Add(new MailboxAddress(recipients));

            if (ccRecipients != null)
            {
                mimeMessage.Cc.AddRange(ccRecipients.Select(x => new MailboxAddress(x)));
            }

            mimeMessage.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = mailBody
            };

            mimeMessage.Body = builder.ToMessageBody();

            mimeMessage.From.Add(new MailboxAddress(mailSetting.UserNameEmail, mailSetting.UserNameEmail));

            try
            {
               
                await SendAsync(mailSetting, mimeMessage);
                return true;
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                return false;
            }
        }

        [Obsolete]
        public async Task<bool> SendAsyncAttachments(MailSettingViewModel mailSetting, string recipients, List<string> ccRecipients, string subject, string mailBody, Dictionary<string, Stream> attachments)
        {
            
            var mimeMessage = new MimeMessage();


            mimeMessage.To.Add(new MailboxAddress(recipients));

            if (ccRecipients != null)
            {
                mimeMessage.Cc.AddRange(ccRecipients.Select(x => new MailboxAddress(x)));
            }

            mimeMessage.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = mailBody
            };

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    builder.Attachments.Add(attachment.Key, attachment.Value);
                }
            }

            mimeMessage.Body = builder.ToMessageBody();

            mimeMessage.From.Add(new MailboxAddress(mailSetting.UserNameEmail, mailSetting.UserNameEmail));

            try
            {

                await SendAsync(mailSetting, mimeMessage);
                return true;
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                return false;
            }
        }

        [Obsolete]
        public async Task SendAsyncSenderAttachments(MailSettingViewModel mailSetting, string recipients, List<string> ccRecipients, string subject, string mailBody, Dictionary<string, Stream> attachments, string senderName, string senderEmail)
        {
        
            var mimeMessage = new MimeMessage();

            mimeMessage.To.Add(new MailboxAddress(recipients));

            if (ccRecipients != null)
            {
                mimeMessage.Cc.AddRange(ccRecipients.Select(x => new MailboxAddress(x)));
            }

            mimeMessage.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = mailBody
            };

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    builder.Attachments.Add(attachment.Key, attachment.Value);
                }
            }

            mimeMessage.Body = builder.ToMessageBody();

            mimeMessage.From.Add(new MailboxAddress(senderName, senderEmail ?? mailSetting.UserNameEmail));
            try
            {
                await SendAsync(mailSetting, mimeMessage);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
   
    
    }
}