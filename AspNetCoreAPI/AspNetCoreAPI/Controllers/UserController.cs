
using AspNetCoreAPI.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpGet("Admins")]
        [Authorize(Roles = "Administrator")]
        public IActionResult AdminsEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.Name}, you are an {currentUser.Role}");
        }


        [HttpGet("1111")]
        [Authorize(Roles = "User")]
        public IActionResult SellersEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.Name}, you are a {currentUser.Role}");
        }

        [HttpGet("AdminsAndSellers")]
        [Authorize(Roles = "Administrator,Seller")]
        public IActionResult AdminsAndSellersEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.Name}, you are an {currentUser.Role}");
        }

        [HttpGet("Public")]
        public String Public()
        {
            return "Hi, you're on public property";
        }
        [HttpGet]
        private User GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new User
                {
                    Phone = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.MobilePhone)?.Value,
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    Name = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value, 
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }
    }
}
