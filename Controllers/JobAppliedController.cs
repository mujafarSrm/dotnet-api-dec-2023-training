using DotNetAPI.Data;
using DotNetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public partial class JobAppliedController: ControllerBase
    {
        private readonly DataContextDapper _dapper;
        #pragma warning disable IDE0052 // Remove unread private members
        private readonly IConfiguration _config;
        #pragma warning restore IDE0052 // Remove unread private members

        public JobAppliedController(IConfiguration Config)
        {
             _config = Config;
            _dapper = new DataContextDapper(Config);
        }

        [HttpGet("getJobs")]
        public IEnumerable<JobApplied> GetJobs(int userid)
        {
            string sql = @"SELECT * FROM TutorialAppSchema.JobApplied WHERE UserId ="+ userid;
            Console.WriteLine("------------line in job applied contollere" + this.User.FindFirst("userId")?.Value);
            return _dapper.LoadData<JobApplied>(sql);
        }

        [HttpGet("getappliedusers")]
        public IEnumerable<JobAppliedUser> GetAppliedUsers(int userId)
        {
            //string sql = @"select * from TutorialAppSchema.Users where UserId in (SELECT UserId  from  TutorialAppSchema.JobApplied where JobId in (select jobid from TutorialAppSchema.JobPost where UserId = '"+ userId + "'));


          string sql = @"SELECT JA.JobId, JP.Title, U.* FROM TutorialAppSchema.Users U
                        JOIN TutorialAppSchema.JobApplied JA ON U.UserId = JA.UserId
                        JOIN TutorialAppSchema.JobPost JP ON JA.JobId = JP.JobId
                        WHERE JA.JobId IN (
                                  SELECT JP1.JobId
                                  FROM TutorialAppSchema.JobPost JP1
                                  WHERE JP1.UserId = " + userId + @"
                                  )
                        ORDER BY JA.JobId;";

            Console.WriteLine("-------------line in jobapplied to get users"+ sql);
            return _dapper.LoadData<JobAppliedUser>(sql);
        }

        [HttpPost("apply")]
        public IActionResult PostAppliedJob(JobApplied jobApplied)
        {
            string sql = @"
            INSERT INTO TutorialAppSchema.JobApplied(
                      [JobId],
                      [UserId]
                      )VALUES (
                        '" + jobApplied.JobId + @"',
                        '" + this.User.FindFirst("userId")?.Value  + "')";
            Console.WriteLine(sql);
            if(_dapper.ExcuteSql(sql))  
            {
                return Ok();
            }
            throw new Exception("Failed to Apply Job");
        }

        [HttpDelete("")]
        public IActionResult DeleteAppliedJob(int Jobid)
        //DELETE FROM TutorialAppSchema.JobApplied WHERE JobId 
        {
            string sql = @"DELETE FROM TutorialAppSchema.JobApplied WHERE JobId = "+ Jobid;
            if(_dapper.ExcuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Failed To Remove Applied Job");
        }
        
    }
}