using CODL.Models;
using CODL.Services;
using Microsoft.AspNetCore.Mvc;

namespace CODL.Controllers;

[ApiController]
[Route("api/v[version:apiVersion]/[controller]")]
public class AppUserController  : Controller
{
    private readonly IAppUserService _appUserService;
    
    public AppUserController(IAppUserService appUserService)
    {
        _appUserService = appUserService;
    }
    
    [HttpPost("/register")]
    public async Task<IActionResult> Register(AppUserRegister appUserRegister)
    {
        return Created("", await _appUserService.Register(appUserRegister));
    }

    [HttpPost("/login")]
    public async Task<IActionResult> LogIn(AppUserLogin appUserLogin)
    {
        ServiceResponse<AppUserResponse> res = await _appUserService.LogIn(appUserLogin);
        HttpContext.Session.SetString("userId", res.Data.Id);
        return Ok(res);
    }
    
    [HttpPost("/logout")]
    public async Task<IActionResult> LogOut()
    {
        HttpContext.Session.Clear();
        return Ok();
    }
}