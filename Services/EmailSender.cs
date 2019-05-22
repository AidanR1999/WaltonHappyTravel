using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Walton_Happy_Travel.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
         public SendGridSettings Options { get; set; }

        public EmailSender(IOptions<SendGridSettings> emailOptions)
        {
            Options = emailOptions.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.APIKey, subject, message, email);
        }

        private Task Execute(string APIKey, string subject, string message, string email)
        {
            var client = new SendGridClient(APIKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("waltonhappytravelgu@gmail.com", "Walton Happy Travel"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));
            try
            {
                return client.SendEmailAsync(msg);
            }
            catch (Exception)
            {
            }

            return null;
        }
    }
}
