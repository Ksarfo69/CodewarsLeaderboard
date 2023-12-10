using CODL.Exceptions;
using CODL.Models;
using CODL.Services;
using Microsoft.AspNetCore.Mvc;

namespace CODL.Controllers;

[ApiController]
[Route("api/v[version:apiVersion]/[controller]")]
[Authenticate]
public class LeaderboardController : Controller
{
    private readonly ILeaderboardService _leaderboardService;

    public LeaderboardController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }
    
    [HttpPost("/add/{username}")]
    public async Task<IActionResult> AddCodewarsUser(string username)
    {
        return Created("" , await _leaderboardService.AddCodewarsUser(username));
    }
    
    [HttpDelete("/remove/{username}")]
    public async Task<IActionResult> RemoveCodewarsUser(string username)
    {
        await _leaderboardService.RemoveCodewarsUser(username);
        return NoContent();
    }
    
    [HttpGet("/rank/honor")]
    public async Task<IActionResult>RankByHonor()
    {
        return Ok(await _leaderboardService.RankByHonor());
    }
    
    [HttpGet("/rank/overall")]
    public async Task<IActionResult> RankByOverallLanguagesScore()
    {
        return Ok(await _leaderboardService.RankByOverallLanguagesScore());
    }
    
    [HttpGet("/rank/{language}")]
    public async Task<IActionResult> RankByLanguageScore(string language)
    {
        return Ok(await _leaderboardService.RankByLanguageScore(language));
    }
    
    [HttpGet("/profile/{username}")]
    public async Task<IActionResult> GetCodewarsUserProfile(string username)
    {
        return Ok(await _leaderboardService.GetCodewarsUserProfile(username));
    }
    
    [HttpPost("/comment")]
    public async Task<IActionResult> Comment(CommentAdd commentAdd)
    {
        string? loggedInUserId = HttpContext.Session.GetString("userId");
        if (string.IsNullOrEmpty(loggedInUserId)) return Unauthorized();
        
        return Created("", await _leaderboardService.Comment(loggedInUserId, commentAdd));
    }
}