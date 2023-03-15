using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Facebook;
using System.Web;

namespace WEBSITE.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;


                    
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
                 
    public IActionResult Index()
    {
        return View();
    }
    #region   --Login GOOGLE--        
    public async Task LoginGoogle()
    {
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
        {
            RedirectUri = Url.Action("loginResponse", "Home")
        });
    }
    public async Task<IActionResult> loginResponse()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
        {
            claim.Issuer,
            claim.OriginalIssuer,
            claim.Type,
            claim.Value
        });
        return Json(claims);
    }
    [Authorize]
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync();
        return RedirectToAction("Index");
    }
    #endregion
    public IActionResult login(string AppName)
    {                 
            var pro = new AuthenticationProperties()
            {
                RedirectUri = Url.Action("loginResponse", "Home")
            };
            if (AppName == "facebook")
            {
                return Challenge(pro, FacebookDefaults.AuthenticationScheme);
            }   else
            if (AppName == "google")
            {      
                return Challenge(pro, GoogleDefaults.AuthenticationScheme);
            }
            else
            {
                return BadRequest(500);
            }
    }                             
}

