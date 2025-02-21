using Blog.Models;
using System.Text;
using System.Text.Json;

namespace Blog.Services;

public class GitHubService(HttpClient httpClient, ConfigService configService)
{
    private GitHubPostsConfig GitHubPostsConfig => configService.GitHubPostsConfig;

    public async Task<string> GetContentAsync(string path, GitHubPostsConfig? gitHubPostsConfig = null)
    {
        gitHubPostsConfig ??= GitHubPostsConfig;

        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(gitHubPostsConfig.UserAgent);
        httpClient.DefaultRequestHeaders.Authorization = new("Bearer", gitHubPostsConfig.AccessToken);

        var response = await httpClient.GetAsync($"https://api.github.com/repos/{gitHubPostsConfig.Owner}/{gitHubPostsConfig.Repository}/contents/{path}?ref={gitHubPostsConfig.Branch}");

        if (!response.IsSuccessStatusCode) return string.Empty;

        return Encoding.UTF8.GetString(
           Convert.FromBase64String(
               JsonDocument.Parse(
                   await response.Content.ReadAsStringAsync()
               ).RootElement.GetProperty("content").GetString() ?? "")
           );
    }

    public async Task<string[]> GetPostListAsync()
    {
        return JsonSerializer.Deserialize<string[]>(await GetContentAsync(GitHubPostsConfig.PostListFilePath)) ?? [];
    }
}