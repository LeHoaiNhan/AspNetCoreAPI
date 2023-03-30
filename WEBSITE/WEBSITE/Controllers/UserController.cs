using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WEBSITE.Controllers
{                    
    public class UserController : Controller
    {
        [HttpGet]
        [Route("dang-nhap")]
        public async Task<IActionResult> login()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (result.Succeeded == true)
            {
                var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.Type,
                    claim.Value
                });
            }
            return View();
        }
        [HttpGet]
        [Route("nguoi-dung")]
        public async Task<IActionResult> UserPosts()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (result.Succeeded == true)
            {
                var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.Type,
                    claim.Value
                });
            }
            return View();
        }

    }
}
