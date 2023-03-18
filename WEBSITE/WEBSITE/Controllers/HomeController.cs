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
        
        CookieOptions options = new CookieOptions();
        options.Expires = DateTime.Now.AddDays(1);
        Response.Cookies.Append("name", "Nhanlh6", options);
        return View();
    }
    public async Task<IActionResult> User()
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
    #region   --Login GOOGLE--        
    public async Task LoginGoogle()
    {
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
        {
            RedirectUri = Url.Action("loginResponse", "Home")
        });
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
        try { 
        var pro = new AuthenticationProperties()
        {
            RedirectUri = Url.Action("Index", "Home")
        };
            if (AppName == "facebook")
            {
                return Challenge(pro, FacebookDefaults.AuthenticationScheme);
            }
            else
            if (AppName == "google")
            {
                return Challenge(pro, GoogleDefaults.AuthenticationScheme);
            }
            else
            {
                return BadRequest(500);
            }
        }
        catch (Exception ex)
        {
            return NotFound(ex);
        }
     }

}

