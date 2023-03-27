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
   //  public HomeController(){}

    [HttpGet]
    [Route("")]
    [Route("trang-chu")]    
    public IActionResult Index()
    {
        
        CookieOptions options = new CookieOptions();
        options.Expires = DateTime.Now.AddDays(1);
        Response.Cookies.Append("name", "Nhanlh6", options);
        return View();
    }
    [HttpGet]
    [Route("vi-tri-cua-hang")]
    public ActionResult Location()
    {
        return View();
    }
    [HttpGet]
    [Route("dat-lich")]
    public ActionResult Clipboard()
    {
        return View();
    }
    [HttpGet]
    [Route("thong-tin-cua-hang")]
    public ActionResult Store()
    {
        return View();
    }
    [HttpGet]
    [Route("danh-sach")]
    public async Task<ActionResult> Menu()
    {       
        return View();
    }
    [HttpGet]
    [Route("nguoi-dung")]
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
    [HttpGet]
    [Route("dang-nhap-google")]
    public async Task LoginGoogle()
    {
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
        {
            RedirectUri = Url.Action("User", "Home")
        });
    }
    [HttpGet]
    [Route("dang-nhap-facebook")]
    public async Task LoginFacebook()
    {
        await HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme, new AuthenticationProperties()
        {
            RedirectUri = Url.Action("User", "Home")
        });
    }
    [HttpGet]
    [Authorize]
    [Route("dang-xuat")]
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync();
        return RedirectToAction("Menu");
    }
#endregion                                        

}

