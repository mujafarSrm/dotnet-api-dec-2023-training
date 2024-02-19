using DotNetAPI.Data;
using DotNetAPI.Dto;
using DotNetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class JobPostController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly IConfiguration _config;

        public JobPostController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _config = config;
        }

        [HttpGet("allJobs")]
        public IEnumerable<JobPost> GetPosts()
        {
            string sql = @"SELECT [JobId],
                    [UserId],
                    [Title],
                    [DESCRIPTION],
                    [Requirements],
                    [PostedAt],
                    [UpdateAt] FROM TutorialAppSchema.JobPost";
            return _dapper.LoadData<JobPost>(sql);
        }

        // #pragma warning disable ASP0023 // Route conflict detected between controller actions
        //         [HttpGet("/{jobId}")]
        // #pragma warning restore ASP0023 // Route conflict detected between controller actions
        //         public IEnumerable<JobPost> GetSingleJob(int jobId)
        //         {
        //             string sql = @"SELECT [JobId],
        //                     [UserId],
        //                     [Title],
        //                     [DESCRIPTION],
        //                     [Requirements],
        //                     [PostedAt],
        //                     [UpdateAt] FROM TutorialAppSchema.JobPost WHERE JobId= "+ jobId;
        //             return _dapper.LoadData<JobPost>(sql);
        //         }


#pragma warning disable ASP0023 // Route conflict detected between controller actions
        [HttpGet("/{userId}")]
#pragma warning restore ASP0023 // Route conflict detected between controller actions
        public IEnumerable<JobPost> GetMyJobs(int userId)
        {
            string sql = @"SELECT [JobId],
                    [UserId],
                    [Title],
                    [DESCRIPTION],
                    [Requirements],
                    [PostedAt],
                    [UpdateAt] FROM TutorialAppSchema.JobPost WHERE UserId= " + userId;
            Console.WriteLine(sql);
            return _dapper.LoadData<JobPost>(sql);
        }

        [HttpGet("myJobs")]
        public IEnumerable<JobPost> GetJobByUser(int userId)
        {
            string sql = @"SELECT [JobId],
                    [UserId],
                    [Title],
                    [DESCRIPTION],
                    [Requirements],
                    [PostedAt],
                    [UpdateAt] FROM TutorialAppSchema.JobPost WHERE UserId= " + userId;
            Console.WriteLine(sql);
            return _dapper.LoadData<JobPost>(sql);
        }

        [HttpPost("")]
        public IActionResult AddJob(JobToAddDto jobToAdd)
        {
            Console.WriteLine("----line 83 in post meth of job controller");
            string sql = @"
             INSERT INTO TutorialAppSchema.JobPost (
                [UserId],
                [Title],
                [DESCRIPTION],
                [Requirements],
                [PostedAt],
                [UpdateAt]
             ) VALUES (
        '" + this.User.FindFirst("userId")?.Value + @"',
        '" + jobToAdd.Title + @"',
        '" + jobToAdd.DESCRIPTION + @"',
        '" + jobToAdd.Requirements + @"',
        GETDATE(),
        GETDATE()
              )";

            // return _dapper.ExcuteSql(sql);
            Console.WriteLine("line 97 post method of job" + sql);
            if (_dapper.ExcuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to add A job");

        }

        [HttpPut("Job")]
public IActionResult EditJob(JobToEditDto jobToEdit, int UserId)
{
    string sql = @"
        UPDATE TutorialAppSchema.JobPost 
        SET Title = '" + jobToEdit.Title
        + "',[DESCRIPTION] ='" + jobToEdit.DESCRIPTION
        + "', Requirements = '" + jobToEdit.Requirements
        + "',  UpdateAt = GETDATE() WHERE JobId ='" + jobToEdit.JobId.ToString()
        + "' AND UserId = " + UserId;
    Console.WriteLine(sql);
    if (_dapper.ExcuteSql(sql))
    {
        return Ok();
    }
    throw new Exception("Failed to edit");
}


        [HttpDelete("deleteJob/{JobId}")]
        public IActionResult DeleteJob(int JobId)
        {
            string sql = @"DELETE FROM TutorialAppSchema.JobPost WHERE JobId = " + JobId;
            if (_dapper.ExcuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Failed to delete the job");
        }

        [HttpGet("jobDetails/{jobId}")]
        public IActionResult GetJobDetails(int jobId)
        {
            string sql = $@"
        SELECT [JobId],
               [UserId],
               [Title],
               [DESCRIPTION],
               [Requirements],
               [PostedAt],
               [UpdateAt]
        FROM TutorialAppSchema.JobPost
        WHERE JobId = {jobId}";

           Console.WriteLine("------------for get job by jobid"+ jobId);

            var jobDetails = _dapper.LoadData<JobPost>(sql);

            if (jobDetails.Any())
            {
                return Ok(jobDetails.First());
            }

            return NotFound();
        }

        [HttpGet("/search")]
        public IEnumerable<JobPost> SearchJob(string SearchTerm)
        {
            string sql = @"SELECT * FROM TutorialAppSchema.JobPost WHERE Title LIKE '%" + SearchTerm + "%' OR Requirements LIKE '%" + SearchTerm + "%'";
            Console.WriteLine("-------for search query"+sql);
            return _dapper.LoadData<JobPost>(sql);
        }

    }
}