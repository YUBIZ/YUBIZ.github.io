using System.Net.Http.Json;

namespace Blog.Services;

public class GitHubService
{
    private readonly HttpClient httpClient = new() { BaseAddress = new Uri("https://raw.githubusercontent.com") };

    public async Task<HttpResponseMessage> GetRawAsync(string owner, string repo, string @ref, string path)
        => await httpClient.GetAsync($"{owner}/{repo}/{@ref}/{path}");

    public async Task<string> GetRawStringAsync(string owner, string repo, string @ref, string path)
        => await httpClient.GetStringAsync($"{owner}/{repo}/{@ref}/{path}");

    public async Task<T?> GetRawFromJsonAsync<T>(string owner, string repo, string @ref, string path)
        => await httpClient.GetFromJsonAsync<T>($"{owner}/{repo}/{@ref}/{path}");
}
