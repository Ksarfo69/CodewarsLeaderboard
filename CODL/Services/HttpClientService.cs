using System.Collections;
using System.Text.Json;
using CODL.Models;

namespace CODL.Services;

public interface IHttpClientService
{
    Task<IDictionary<bool, IList>> FetchCodewarsUserInfo(List<string> usernames);
    Task<CodewarsUserInfo?> FetchCodewarsUserInfo(String username);
}

public class HttpClientService : IHttpClientService
{
    private readonly ILogger<HttpClientService> _logger;
    private readonly string CODEWARS_ENDPOINT = "https://www.codewars.com/api/v1/users/";

    public HttpClientService(ILogger<HttpClientService> logger)
    {
        _logger = logger;
    }
    
    public async Task<IDictionary<bool, IList>> FetchCodewarsUserInfo(List<string> usernames)
    {
        List<Task> tasks = new List<Task>();
        IDictionary<bool, IList> results = new Dictionary<bool, IList>();
        
        results.Add(true, new List<CodewarsUserInfo>());
        results.Add(false, new List<string>());
        
        _logger.LogInformation($"Querying Codewars users information from {CODEWARS_ENDPOINT}");
        using (HttpClient client = new HttpClient())
        {
            foreach (var username in usernames)
            {
                tasks.Add(SendFetchRequestTask(client, username, results));
            }

            Task.WaitAll(tasks.ToArray());
        }
        _logger.LogInformation($"Codewars users information queried successfully from {CODEWARS_ENDPOINT}");
        return results;
    }

    public async Task<CodewarsUserInfo?> FetchCodewarsUserInfo(string username)
    {
        _logger.LogInformation($"Querying Codewars user: {username} information from {CODEWARS_ENDPOINT}");
        using (HttpClient client = new HttpClient())
        {
            CodewarsUserInfo info = await SendFetchRequestTask(client, username);
            _logger.LogInformation($"Codewars user: {username} information queried successfully from {CODEWARS_ENDPOINT}");
            return info;
        }
    }

    private async Task SendFetchRequestTask(HttpClient client, string username, IDictionary<bool, IList> results)
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync($"{CODEWARS_ENDPOINT}/{username}");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                CodewarsUserInfo info = JsonSerializer.Deserialize<CodewarsUserInfo>(responseBody);
                results[true].Add(info);
            }
            else
            {
                results[false].Add(username);
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("Request exception: " + ex.Message);
        }
    }

    private async Task<CodewarsUserInfo> SendFetchRequestTask(HttpClient client, string username)
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync($"{CODEWARS_ENDPOINT}/{username}");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                CodewarsUserInfo info = JsonSerializer.Deserialize<CodewarsUserInfo>(responseBody);
                return info;
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError("Request exception: " + ex.Message);
        }

        return default;
    }
}