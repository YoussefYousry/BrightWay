using BrightWeb_BAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BrightWeb_BAL.ViewModels;

namespace BrightWeb_BAL.Services
{
	public class EmailService : IEmailService
	{
		private readonly System.Net.Mail.SmtpClient _smtpClient;
		private string _senderEmail;

		//public EmailService(IOptions<EmailSettings> emailSettings)
		//{
		//    var settings = emailSettings.Value;

		//    _smtpClient = new SmtpClient(settings.SmtpServer, settings.SmtpPort ?? 0)
		//    {
		//        UseDefaultCredentials = false,
		//        Credentials = new NetworkCredential(settings.SmtpUsername, settings.SmtpPassword),
		//        EnableSsl = settings.EnableSsl
		//    };

		//    _senderEmail = settings.SenderEmail;
		//}

		public async Task SendPasswordResetEmailAsync(string email, string callbackUrl)
		{
			try
			{
                var setting = new EmailSettings
                {
                    SenderEmail = "brightwayeng.services@gmail.com",
                    SmtpServer = "smtp.gmail.com",
                    EnableSsl = true,
                    SmtpPassword = "gfswctwglcbebdrn",
                    SmtpUsername = "brightwayeng.services@gmail.com",
                    SmtpPort = 587
                };
                _senderEmail = setting.SenderEmail;
                var _smtpClient = new SmtpClient(setting.SmtpServer, setting.SmtpPort ?? 0)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(setting.SmtpUsername, setting.SmtpPassword),
                    EnableSsl = setting.EnableSsl,
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_senderEmail),
                    Subject = "Password Reset",
                    Body = $"Click the following link to reset your password: {callbackUrl}",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(email);
                await _smtpClient.SendMailAsync(mailMessage);
			
		}catch(Exception ex)
            {

            }
	}
}
}
