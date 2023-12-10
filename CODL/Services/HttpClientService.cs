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
    private readonly string CODEWARS_ENDPOINT = "https://www.codewars.com/api/v1/users/";
    
    public async Task<IDictionary<bool, IList>> FetchCodewarsUserInfo(List<string> usernames)
    {
        List<Task> tasks = new List<Task>();
        IDictionary<bool, IList> results = new Dictionary<bool, IList>();
        
        results.Add(true, new List<CodewarsUserInfo>());
        results.Add(false, new List<string>());
        
        using (HttpClient client = new HttpClient())
        {
            foreach (var username in usernames)
            {
                tasks.Add(SendFetchRequestTask(client, username, results));
            }

            Task.WaitAll(tasks.ToArray());
        }

        return results;
    }

    public async Task<CodewarsUserInfo?> FetchCodewarsUserInfo(string username)
    {
        using (HttpClient client = new HttpClient())
        {
            CodewarsUserInfo info = await SendFetchRequestTask(client, username);
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
            Console.WriteLine("Request exception: " + ex.Message);
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
            Console.WriteLine("Request exception: " + ex.Message);
        }

        return default;
    }
}