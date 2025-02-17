using Microsoft.AspNetCore.Http;

namespace WebApp.Infrastructure.Mail
{
    public interface IEmailService
    {
        Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile>? attachments);
    }
}