using System.Net.Mail;
using System.Net;
using MimeKit;
using WebApplication2.Utilities;

namespace WebApplication2.Services.EmailService.Impl
{
    public class EmailService: IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string toEmail, string subject, string body, bool ishtmlbody)
        {
            MimeMessage sendEmail = new MimeMessage();
            sendEmail.From.Add(MailboxAddress.Parse(_configuration.GetSection(Constants.From).Value));
            sendEmail.To.Add(MailboxAddress.Parse(toEmail));
            sendEmail.Subject = subject;
            sendEmail.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
            SmtpClient client = new SmtpClient(_configuration.GetSection(Constants.Host).Value, Convert.ToInt32(_configuration[Constants.Port]))
            {
                Credentials = new NetworkCredential(_configuration.GetSection(Constants.FromEmailUsername).Value, _configuration.GetSection(Constants.FromEmailPassword).Value),
                EnableSsl = true,

            };
            MailMessage mailMessage = new MailMessage(_configuration.GetSection(Constants.From).Value, toEmail, subject, body)
            {
                IsBodyHtml = ishtmlbody
            };
            return client.SendMailAsync(mailMessage);
        }

    }
}
