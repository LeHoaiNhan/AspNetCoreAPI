﻿using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
})
.AddFacebook(options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:AppID"];
    options.ClientSecret = builder.Configuration["Authentication:Facebook:ClientSecret"];
    options.Fields.Add("picture"); 
    options.Events = new OAuthEvents
    {
        OnCreatingTicket = (context) =>
        {
            ClaimsIdentity identity = (ClaimsIdentity)context.Principal.Identity;
            string profileImg = context.User.GetProperty("picture").GetProperty("data").GetProperty("url").ToString();     
            identity.AddClaim(new Claim(JwtClaimTypes.Picture, profileImg));
            return Task.CompletedTask;
        }
    };
    options.CallbackPath = "/sigin-facebook";
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {        
            //you can configure your custom policy
            builder.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});
builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();                                             
app.MapControllerRoute(name: "default", pattern: "{Home}/{action}/{id?}");      
app.UseSwagger();
app.UseSwaggerUI();    
app.Run();
