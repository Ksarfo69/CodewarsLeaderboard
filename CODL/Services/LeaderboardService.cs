using System.Collections;
using CODL.Exceptions;
using CODL.Models;
using CODL.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace CODL.Services;

public interface ILeaderboardService {
    Task<ServiceResponse<string>> AddCodewarsUser (string username);
    Task RemoveCodewarsUser (string username);
    Task<ServiceResponse<List<HonorRanking>>> RankByHonor();
    Task<ServiceResponse<OverallLanguagesRanking>> RankByOverallLanguagesScore();
    Task<ServiceResponse<List<LanguageRanking>>> RankByLanguageScore(string language);
    Task<ServiceResponse<CodewarsUserResponse>> GetCodewarsUserProfile(string username);
    Task<ServiceResponse<string>> Comment(string commenterId, CommentAdd commentAdd);
}

public class LeaderboardService : ILeaderboardService
{
    private readonly ILogger<LeaderboardService> _logger;
    private readonly ICodewarsUserRepository _codewarsUserRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IAppUserRepository _appUserRepository;
    private readonly IHttpClientService _httpClientService;
    private static readonly MemoryCache cache = new MemoryCache(new MemoryCacheOptions());

    public LeaderboardService(
        ILogger<LeaderboardService> logger,
        ICodewarsUserRepository codewarsUserRepository,
        ICommentRepository commentRepository,
        IAppUserRepository appUserRepository,
        IHttpClientService httpClientService)
    {
        _logger = logger;
        _codewarsUserRepository = codewarsUserRepository;
        _commentRepository = commentRepository;
        _appUserRepository = appUserRepository;
        _httpClientService = httpClientService;
    }
    
    public async Task<ServiceResponse<string>> AddCodewarsUser(string username)
    {
        CodewarsUser? alreadyExists = await _codewarsUserRepository.GetCodewarsUserByUsername(username);

        if (alreadyExists != default)
        {
            _logger.LogError($"Codewars user: {username} already exists.");
            throw new ResourceAlreadyExistsException($"Username {username} already exists.");
        }
        
        CodewarsUserInfo? existsOnCodewars = await _httpClientService.FetchCodewarsUserInfo(username);

        if (existsOnCodewars == default)
        {
            _logger.LogError($"Username: {username} does not exist on Codewars.");
            throw new BadRequestException($"Username {username} is not a valid Codewars username.");
        }

        CodewarsUser newCodewarsUser = new CodewarsUser
        {
            Username = username
        };

        await _codewarsUserRepository.AddCodewarsUser(newCodewarsUser);
        
        // invalidate cache
        cache.Remove("usersInfo");
        
        _logger.LogInformation($"Codewars user: {newCodewarsUser.Username} added successfully");

        return new ServiceResponse<string>
        {
            Success = true,
            Message = "Codewars user added successfully.",
            Data = newCodewarsUser.Id.ToString()
        };
    }

    public async Task RemoveCodewarsUser(string username)
    {
        await _codewarsUserRepository.DeleteCodewarsUserByUsername(username);
        
        _logger.LogInformation($"Codewars user: {username} deleted successfully.");
        
        // invalidate cache
        cache.Remove("usersInfo");
    }

    public async Task<ServiceResponse<List<HonorRanking>>> RankByHonor()
    {
        List<CodewarsUserInfo> infos = await GetCodewarsUsersInfo();
        List<HonorRanking> rankings = infos.OrderByDescending(i => i.honor).Select(i => new HonorRanking
        {
            username = i.username,
            name = i.name,
            clan = i.clan,
            honor = i.honor,
            rank = i.ranks.overall.name,
            languages = i.ranks.languages.Keys.ToList()
        }).ToList();
        
        _logger.LogInformation($"Leaderboard ranked by honor successfully");
        
        return new ServiceResponse<List<HonorRanking>>
        {
            Success = true,
            Message = "Leaderboard ranked by honor successfully.",
            Data = rankings
        }
        ;
    }

    public async Task<ServiceResponse<OverallLanguagesRanking>> RankByOverallLanguagesScore()
    {
        List<CodewarsUserInfo> infos = await GetCodewarsUsersInfo();
        HashSet<string> languagesSet = new HashSet<string>();
        List<ScoreRanking> rankings = infos.OrderByDescending(i => i.ranks.overall.score)
            .Select(i =>
            {
                List<string> userLanguages = i.ranks.languages.Keys.ToList();

                languagesSet.UnionWith(userLanguages);
                
                return new ScoreRanking
                {
                    username = i.username,
                    name = i.name,
                    clan = i.clan,
                    score = i.ranks.overall.score,
                    rank = i.ranks.overall.name,
                    languages = userLanguages
                };
            }).ToList();
        
        _logger.LogInformation($"Leaderboard ranked by all languages successfully");
        
        return new ServiceResponse<OverallLanguagesRanking>
            {
                Success = true,
                Message = "Leaderboard ranked by all languages successfully.",
                Data = new OverallLanguagesRanking
                {
                    scores = rankings,
                    languages = languagesSet
                }
            }
            ;
    }

    public async Task<ServiceResponse<List<LanguageRanking>>> RankByLanguageScore(string language)
    {
        List<CodewarsUserInfo> infos = await GetCodewarsUsersInfo();
        List<LanguageRanking> rankings = infos.Where(i => i.ranks.languages.ContainsKey(language))
            .OrderByDescending(i => i.ranks.languages[language].score)
            .Select(i => new LanguageRanking
        {
            username = i.username,
            name = i.name,
            clan = i.clan,
            score = i.ranks.languages[language].score,
            rank = i.ranks.languages[language].name,
        }).ToList();
        
        _logger.LogInformation($"Leaderboard ranked by {language} successfully");
        
        return new ServiceResponse<List<LanguageRanking>>
            {
                Success = true,
                Message = $"Leaderboard ranked by {language} successfully.",
                Data = rankings
            }
            ;
    }

    public async Task<ServiceResponse<CodewarsUserResponse>> GetCodewarsUserProfile(string username)
    {
        CodewarsUser? codewarsUser = await _codewarsUserRepository.GetCodewarsUserByUsername(username);
        
        if (codewarsUser == default)
        {
            _logger.LogError($"Codewars user: {username} not found.");
            throw new ResourceNotFoundException($"Username {username} not found.");
        }
        
        CodewarsUserInfo? existsOnCodewars = await _httpClientService.FetchCodewarsUserInfo(username);

        if (existsOnCodewars == default)
        {
            _logger.LogError($"Username: {username} does not exist on Codewars.");
            throw new BadRequestException($"Username {username} is not a valid Codewars username.");
        }

        List<Comment> codewarsUserComments =
            await _commentRepository.GetAllByCodewarsUserId(codewarsUser.Id.ToString());
        
        _logger.LogInformation($"Fetched Codewars user: {username} profile successfully.");
        
        return new ServiceResponse<CodewarsUserResponse>
        {
            Success = true,
            Message = "Fetched Codewars user profile successfully.",
            Data = new CodewarsUserResponse
            {
                id = codewarsUser.Id.ToString(),
                username = existsOnCodewars.username,
                name = existsOnCodewars.name,
                honor = existsOnCodewars.honor,
                clan = existsOnCodewars.clan,
                skills = existsOnCodewars.skills,
                ranks = existsOnCodewars.ranks,
                codeChallenges = existsOnCodewars.codeChallenges,
                comments = codewarsUserComments.Select(c=> c.ToResponse()).ToList()
            }
        };
    }

    public async Task<ServiceResponse<string>> Comment(string commenterId, CommentAdd commentAdd)
    {
        AppUser? commenter = await _appUserRepository.GetAppUserById(commenterId);
        
        if (commenter == default)
        {
            _logger.LogError($"Logged in user with id: {commenterId} not recognized.");
            throw new ResourceNotFoundException($"Username with id: {commenterId} not found.");
        }

        CodewarsUser? commentee = await _codewarsUserRepository.GetCodewarsUserByUsername(commentAdd.CommenteeUsername);

        if (commentee == default)
        {
            _logger.LogError($"Codewars user: {commentAdd.CommenteeUsername} not found.");
            throw new ResourceNotFoundException($"Username {commentAdd.CommenteeUsername} not found.");
        }
        
        Comment comment = new Comment
        {
            Commenter = commenter,
            Commentee = commentee,
            Text = commentAdd.Text
        };

        await _commentRepository.AddComment(comment);
        
        _logger.LogInformation($"Comment added for Codewars user: {commentAdd.CommenteeUsername} successfully.");

        return new ServiceResponse<string>
        {
            Success = true,
            Message = "Comment added successfully.",
            Data = comment.Id.ToString()
        };
    }

    private async Task<List<CodewarsUserInfo>> GetCodewarsUsersInfo()
    {
        _logger.LogInformation("Retrieving leaderboard stats.");
        List<CodewarsUserInfo> data;
        
        // cache hit?
        if (cache.TryGetValue("usersInfo", out data))
        {
            _logger.LogInformation("Leaderboard stats retrieved successfully from cache.");
            return data;
        }
        
        // cache miss
        List<string> usernames = await _codewarsUserRepository.GetAllUsernames();

        IDictionary<bool, IList> res = await _httpClientService.FetchCodewarsUserInfo(usernames);

        data = res[true] as List<CodewarsUserInfo>;
        
        _logger.LogInformation("Leaderboard stats retrieved successfully from Codewars.");
        
        // add to cache
        _logger.LogInformation("Adding retrieved leaderboard stats from Codewars to cache.");
        cache.Set("usersInfo", data, TimeSpan.FromMinutes(5));
        _logger.LogInformation("Retrieved leaderboard stats from Codewars successfully added to cache.");
        
        // delete invalid usernames (invalid likely due to change of username.)
        List<string> invalidUsernames = res[false] as List<string>;

        if (invalidUsernames.Count > 0)
        {
            _logger.LogInformation("Deleting obsolete Codewars users.");
            _codewarsUserRepository.DeleteCodewarsUserByUsernameIn(invalidUsernames);  
            _logger.LogInformation("Obsolete Codewars users deleted successfully.");
        }
        
        return data;
    }
}