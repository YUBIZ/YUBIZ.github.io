using Blog.Models;
using System.Text;
using System.Text.Json;

namespace Blog.Services;

public class GitHubService(HttpClient httpClient, ConfigService configService)
{
    private GitHubPostsConfig GitHubPostsConfig => configService.GitHubPostsConfig;

    public async Task<string[]> GetPostListAsync()
    {
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Blog");
        httpClient.DefaultRequestHeaders.Authorization = new("Bearer", GitHubPostsConfig.AccessToken);

        var response = await httpClient.GetAsync($"https://api.github.com/repos/{GitHubPostsConfig.Owner}/{GitHubPostsConfig.Repository}/contents/{GitHubPostsConfig.PostListFilePath}?ref={GitHubPostsConfig.Branch}");

        if (!response.IsSuccessStatusCode) return [];

        return JsonSerializer.Deserialize<string[]>(
            Encoding.UTF8.GetString(
                Convert.FromBase64String(
                    JsonDocument.Parse(
                        await response.Content.ReadAsStringAsync()
                    ).RootElement.GetProperty("content").GetString() ?? "")
                )
            ) ?? [];
    }
}
