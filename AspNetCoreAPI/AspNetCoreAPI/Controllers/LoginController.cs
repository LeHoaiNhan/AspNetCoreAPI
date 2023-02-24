using AspNetCoreAPI.Models.Error;
using AspNetCoreAPI.Models.User;
using AspNetCoreAPI.Pages;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        public IDbConnection _dbConnection => new SqlConnection(_config.GetConnectionString("sqlConnectionOnline"));
        public static User user = new User();
        public static Error_Model errorModel = new Error_Model();
        public LoginController(IConfiguration config)
        {
            _config = config;
        }
         
        //---------------------------------Register------------------------------------------

        public static string EncodePasswordToBase64(string password)
        {
            try {
                byte[] encData_byte = Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }catch (Exception ex) { throw new Exception("Error in base64Encode" + ex.Message); }
        }
        public string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;   
        }

        [HttpPost("register")]
        public async Task<Error_Model> Register(UserLogin request)
        {
            string PassworBase64 = EncodePasswordToBase64(request.Password);
            try
            {
                 using (var con = _dbConnection)
                {         
                    var result = await con.QueryFirstOrDefaultAsync<User>("Company.dbo.Register", new
                    {
                        Phone = request.Phone,
                        Password = PassworBase64
                    }, commandType: System.Data.CommandType.StoredProcedure);
                    if   (result != null)
                    {
                        errorModel.status = 200;
                        errorModel.message = "OK";
                        errorModel.data = result;
                        return errorModel;
                    }
                    else
                    {
                        errorModel.status = 400;
                        errorModel.message = "Bad Request";
                        errorModel.data = result;
                        return errorModel;
                    }
                }  
            }
            catch( Exception ex)
            {
                errorModel.status = 500;
                errorModel.message = ex.Message;
                errorModel.data = ex;
                return errorModel;
            }

        }

        //---------------------------------Register------------------------------------------

        [AllowAnonymous]
        [HttpPost]
        public async Task<Error_Model> Login(UserLogin userLogin)
        {                                                                      
            try
            {
                using (var con = _dbConnection)
                {
                    var result = await con.QueryFirstOrDefaultAsync<User>("Company.dbo.Login", new
                    {
                        Phone = userLogin.Phone,
                        Password= EncodePasswordToBase64(userLogin.Password)
                    }, commandType: System.Data.CommandType.StoredProcedure);
                    if(result!=null)
                    {
                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        var claims = new[]
                        {
                            new Claim(ClaimTypes.MobilePhone, result.Phone),
                            new Claim(ClaimTypes.Role, result.Role)
                        };

                        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                          _config["Jwt:Audience"],
                          claims,
                          expires: DateTime.Now.AddSeconds(21)
                          ,signingCredentials: credentials
                          );
                        result.Token = new JwtSecurityTokenHandler().WriteToken(token);
                        result.Expires= DateTime.Now.AddSeconds(21);
                    }
                    errorModel.status = 200;
                    errorModel.message = "OK";
                    errorModel.data = result;  

                    


                    return errorModel;
                }
            }
            catch (Exception ex)
            {
                errorModel.status = 500;
                errorModel.message = ex.Message;
                errorModel.data = ex;
                return errorModel;
            }

             
        }


    }
}
