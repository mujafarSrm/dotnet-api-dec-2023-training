
using System.Data;
using DotNetAPI.Data;
using DotNetAPI.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DotNetAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        DataContextDapper _dapper;
        public UserController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            Console.WriteLine(config.GetConnectionString("DefaultConnection"));
        }

        [HttpGet("TC")]
        public DateTime TC()
        {
            return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
        }

        [HttpGet("test/{testValue}")]
        public string[] TEST(string testValue)
        {
            string[] aArray = new string[]{
            "test1",
            "test2",
            testValue
        };
            return aArray;
        }

        [HttpGet("Users")]
        public IEnumerable<User> Users()
        {
            string sql = @"SELECT * FROM TutorialAppSchema.USERS";

            IEnumerable<User> AllUsers = _dapper.LoadData<User>(sql);

            return AllUsers;
        }

        [HttpGet("Users/{userId}")]
        public IEnumerable<User> SingleUser(int userId)
        {
            string sql = @"SELECT * FROM TutorialAppSchema.USERS WHERE UserId=" + userId;

            IEnumerable<User> User = _dapper.LoadData<User>(sql);

            return User;
        }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        string sql = @"
        UPDATE TutorialAppSchema.Users
            SET [FirstName] = '" + user.FirstName + 
                "', [LastName] = '" + user.LastName +
                "', [Email] = '" + user.Email + 
                "', [Gender] = '" + user.Gender + 
                "', [Active] = '" + user.Role + 
            "' WHERE UserId = " + user.UserId;
        
        Console.WriteLine(sql);

        if (_dapper.ExcuteSql(sql))
        {
            return Ok();
        } 

        throw new Exception("Failed to Update User");
    }


    [HttpPost("AddUser")]
    public IActionResult AddUser(UserDto user)
    {
        string sql = @"
            INSERT INTO TutorialAppSchema.Users(
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active]
            ) VALUES (" +
                "'" + user.FirstName + 
                "', '" + user.LastName +
                "', '" + user.Email + 
                "', '" + user.Gender + 
                "', '" + user.Role + 
            "')";
        
        Console.WriteLine(sql);

        if (_dapper.ExcuteSql(sql))
        {
            return Ok();
        } 

        throw new Exception("Failed to Add User");
    }
    [HttpDelete("deleteUser/{UserId}")]
    public IActionResult DeleteUser(int UserId)
    {
        string sql = @"DELETE  FROM TutorialAppSchema.Users WHERE UserId= "+UserId;
        if(_dapper.ExcuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("Failed to delete User");

    }
    }
}


