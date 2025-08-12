using System.Net.Http.Json;

namespace Blog.Services;

public class GitHubService
{
    public static string RawBaseAdddress => "https://raw.githubusercontent.com";
    public required string Owner { get; init; }
    public required string Repo { get; init; }
    public required string Ref { get; init; }
    public string Params => $"{Owner}/{Repo}/{Ref}";
    public string RawBaseAdddressWithParams => $"{RawBaseAdddress}/{Params}";

    private readonly HttpClient httpClient = new() { BaseAddress = new Uri(RawBaseAdddress) };

    public async Task<HttpResponseMessage> GetRawAsync(string path)
        => await httpClient.GetAsync($"{Params}/{path}");

    public async Task<string> GetRawStringAsync(string path)
        => await httpClient.GetStringAsync($"{Params}/{path}");

    public async Task<T?> GetRawFromJsonAsync<T>(string path)
        => await httpClient.GetFromJsonAsync<T>($"{Params}/{path}");

    public async Task<T?> GetRawFromYamlFrontMatterAsync<T>(string path)
        => Helper.DeserializeYamlFrontMatter<T>(await GetRawStringAsync(path));
}
