using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DotNetAPI.Data;
using DotNetAPI.Dto;
using DotNetAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace DotNetAPI.Controllers
{
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly IConfiguration _config; 
        private readonly AuthHelper _authHelper;
        public AuthController (IConfiguration config)
        {
            _dapper = new DataContextDapper(config); 
            _config = config;
            _authHelper = new AuthHelper(config);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserForRegistrationDto userForRegistration)
        {
            Console.WriteLine("----line 25 in backend aut cont", userForRegistration);
            if(userForRegistration.Password ==  userForRegistration.PasswordConfirm)
            {
                string sql = @"SELECT Email FROM TutorialAppSchema.Auth WHERE Email = '" +
                    userForRegistration.Email + "'";
                Console.WriteLine(sql, userForRegistration);
                IEnumerable<string> existingUsers = _dapper.LoadData<string>(sql);
                if(existingUsers.Count()==0)
                {
                    Console.WriteLine("----line 33");
                    byte[] passwordSalt = new byte[128 / 8];
                    using (RandomNumberGenerator reg = RandomNumberGenerator.Create())
                    {
                        reg.GetNonZeroBytes(passwordSalt);
                    }
                    
                    #pragma warning disable CS8604 // Possible null reference argument.
                    byte[] passwordHash = _authHelper.GetPasswordHash(userForRegistration.Password, passwordSalt);
                    #pragma warning restore CS8604 // Possible null reference argument.
                string sqlAddAuth = @"
                        INSERT INTO TutorialAppSchema.Auth  ([Email],
                        [PasswordHash],
                        [PasswordSalt]) VALUES ('" + userForRegistration.Email +
                        "', @PasswordHash, @PasswordSalt)";
                    List<SqlParameter> sqlParameters = new List<SqlParameter>();
                    SqlParameter passwordSaltParameter = new SqlParameter("@passwordSalt", SqlDbType.VarBinary);
                    passwordSaltParameter.Value = passwordSalt;

                   // List<SqlParameter> sqlParameters = new List<SqlParameter>();
                    SqlParameter passwordHashParameter = new SqlParameter("@passwordHash", SqlDbType.VarBinary);
                    passwordHashParameter.Value = passwordHash;

                    sqlParameters.Add(passwordSaltParameter);
                    sqlParameters.Add(passwordHashParameter);
                    Console.WriteLine("----line 58");
                    if(_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
                    {
                        Console.WriteLine("----line 61");
                        string sqlAddUser = @"
                                INSERT INTO TutorialAppSchema.Users(
                                   [FirstName],
                                   [LastName],
                                   [Email],
                                   [Gender],
                                   [Role]
                                ) VALUES (" +
                                "'" + userForRegistration.FirstName + 
                                "', '" + userForRegistration.LastName +
                                "', '" + userForRegistration.Email + 
                                "', '" + userForRegistration.Gender + 
                                "', '" + userForRegistration.Role +
                                "')";
                            Console.WriteLine("line sql for register"+sqlAddUser);
                            if(_dapper.ExcuteSql(sqlAddUser))
                            {
                                Console.WriteLine("----line 78");
                                return Ok();
                            }
                        throw new Exception("Failed to add user");
                    }
                    throw new Exception("Failed to register User");
                }
                throw new Exception("User with this email already exists");
            }
            throw new Exception("Passwords do not match");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserForLoginDto userForLogin)
        {
            string sqlForHashAndSalt = @"
                SELECT [Email],
                [PasswordHash],
                [PasswordSalt] FROM TutorialAppSchema.Auth WHERE Email = '" +
            userForLogin.Email + "'";
            Console.WriteLine("99"+userForLogin.Email+ userForLogin.Password+ sqlForHashAndSalt );
            UserForLoginConformationDto userForConfirmation = _dapper
                .LoadDataSingle<UserForLoginConformationDto>(sqlForHashAndSalt);
            Console.WriteLine("102",userForLogin.Email, userForLogin.Password);
            #pragma warning disable CS8604 // Possible null reference argument.
            byte[] passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);
            Console.WriteLine("105",userForLogin.Email, userForLogin.Password);
            #pragma warning restore CS8604 // Possible null reference argument.
            //if(passwordHash == userForConfirmation.PasswordHash)

            for(int index = 0; index < passwordHash.Length; index++ )
            {
            #pragma warning disable CS8602 // Dereference of a possibly null reference.
                Console.WriteLine("line 112",userForLogin.Email ,userForLogin.Password.ToString(), passwordHash, passwordHash[index]);
                if (passwordHash[index] != userForConfirmation.PasswordHash[index])
                {
                    throw new Exception("Incorrect password!"); 
                }
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            // return Ok();
            string userIdSql = @"
                SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '" +
                userForLogin.Email + "'";

            int userId = _dapper.LoadDataSingle<int>(userIdSql);

            return Ok(new Dictionary<string, string> {
                {"token", _authHelper.CreateToken(userId)}
            });
        }

        [HttpGet("RefreshToken")]
        public string RefreshToken()
        {
            string userIdSql = @"
                SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = '" +
                User.FindFirst("userId")?.Value + "'";

            int userId = _dapper.LoadDataSingle<int>(userIdSql);

            return _authHelper.CreateToken(userId);
        } 
    }
}