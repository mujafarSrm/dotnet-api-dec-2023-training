using Microsoft.AspNetCore.Mvc;

namespace SendMail
{
    public interface IEmailSender
    {
        Task<IActionResult> SendEmailAsync(string email, string subject, string message);
    }
}