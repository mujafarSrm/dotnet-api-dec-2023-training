
// using System.Data;
// using DotNetAPI.Data;
// using DotNetAPI.Models;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Data.SqlClient;

// namespace DotNetAPI.Controllers
// {

//     [ApiController]
//     [Route("[controller]")]
//     public class JobsController : ControllerBase
//     {
//         DataContextDapper _dapper;
//         public JobsController(IConfiguration config)
//         {
//             _dapper = new DataContextDapper(config);
//             Console.WriteLine(config.GetConnectionString("DefaultConnection"));
//         }

//         [HttpGet()]
//         public IEnumerable<Jobs> GetJobs()
//         {
//             string sql = @"SELECT * FROM TutorialAppSchema.Jobs";
//             IEnumerable<Jobs> AllJobs = _dapper.LoadData<Jobs>(sql);
//             return AllJobs;
//         }

//         [HttpGet("/{JobId}")]
//         public Jobs GetJobs(int JobId)
//         {
//             string sql = @"SELECT * FROM TutorialAppSchema.Jobs WHERE JobId="+JobId;
//             Jobs SingleJob = _dapper.LoadDataSingle<Jobs>(sql);
//             return SingleJob;
//         }
        
//     }
// }


