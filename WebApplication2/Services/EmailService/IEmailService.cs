namespace WebApplication2.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, bool ishtmlbody);
    }
}
