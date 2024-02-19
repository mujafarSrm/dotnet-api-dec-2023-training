using DotNetAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendMail;
using System.Threading.Tasks;

namespace DotNetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private readonly DataContextDapper _dapper;
        private readonly IConfiguration _config;
        public EmailController(IEmailSender emailSender, IConfiguration config)

        {
            _emailSender = emailSender;
            _config = config;
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("sendmail/userid")]
        public string Index1(int userid)
        {
            string str = @"SELECT Email FROM TutorialAppSchema.Users where UserId = '"+userid+"'";
            string userMail =  _dapper.LoadDataSingle<string>(str);

            return userMail;
        }

        [HttpPost("sendmail")]
        public async Task<IActionResult> Index(string subject, string message)
        {
            //http://localhost:5000/Email/sendmail?email=ali%40sutara.org&subject=hi&message=hi
            string str = @"SELECT Email FROM TutorialAppSchema.Users where UserId = '"+this.User.FindFirst("userId")?.Value +"'";
            string userMail =  _dapper.LoadDataSingle<string>(str);
            Console.WriteLine("find authorize mail id "+ userMail);
            var result = await _emailSender.SendEmailAsync(userMail, subject, message);
            return result;
        }
    }
}
