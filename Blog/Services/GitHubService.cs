using Blog.Misc.HttpClientExtensions;
using Blog.Models.Config;
using System.Net.Http.Json;

namespace Blog.Services;

public class GitHubService(HttpClient httpClient, GitHubSettings gitHubSettings)
{
    public const string RawBaseAdddress = "https://raw.githubusercontent.com";
    public string RawBaseAdddressWithParams { get; } = $"{RawBaseAdddress}/{gitHubSettings.Owner}/{gitHubSettings.Repo}/{gitHubSettings.Ref}";

    public async Task<HttpResponseMessage> GetRawAsync(string path)
        => await httpClient.GetAsync($"{RawBaseAdddressWithParams}/{path}");

    public async Task<string> GetRawStringAsync(string path)
        => await httpClient.GetStringAsync($"{RawBaseAdddressWithParams}/{path}");

    public async Task<T?> GetRawFromJsonAsync<T>(string path)
        => await httpClient.GetFromJsonAsync<T>($"{RawBaseAdddressWithParams}/{path}");

    public async Task<T?> GetRawFromYamlFrontMatterAsync<T>(string path)
        => await httpClient.GetFromYamlFrontMatterAsync<T>($"{RawBaseAdddressWithParams}/{path}");
}
