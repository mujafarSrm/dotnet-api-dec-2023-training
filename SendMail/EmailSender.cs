using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;


namespace SendMail
{
    public class EmailSender : IEmailSender
    {
        public async Task<IActionResult> SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("kolimemujafarali@gmail.com", "tvjiwbtlucabrsvt")
                };

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("kolimemujafarali@gmail.com");
                mailMessage.To.Add(new MailAddress(email));
                mailMessage.Subject = subject;
                mailMessage.Body = message;

                await client.SendMailAsync(mailMessage);

                // Returning success status along with a message
                return new OkObjectResult(new { Status = "Success", Message = "Email sent successfully" });
            }
            catch (Exception exception)
            {
                // Log the exception or handle it as needed

                // Returning 500 Internal Server Error status along with an error message
                Console.WriteLine(exception);
                return new ObjectResult(new { Status = "Error", Message = "Failed to send email" })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}