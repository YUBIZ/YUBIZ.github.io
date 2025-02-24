using Blog.Models;
using System.Text;
using System.Text.Json;
using YamlDotNet.Serialization;

namespace Blog.Services;

public class GitHubService(HttpClient httpClient, ConfigService configService)
{
    private GitHubPostsConfig GitHubPostsConfig => configService.GitHubPostsConfig;

    public async Task<Commit[]> GetCommitsAsync(string path, GitHubPostsConfig? gitHubPostsConfig = null)
    {
        gitHubPostsConfig ??= GitHubPostsConfig;

        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(gitHubPostsConfig.UserAgent);
        // @TODO: 토큰을 가져오는 방법 찾기
        //httpClient.DefaultRequestHeaders.Authorization = new("Bearer", gitHubPostsConfig.AccessToken);

        var response = await httpClient.GetAsync($"https://api.github.com/repos/{gitHubPostsConfig.Owner}/{gitHubPostsConfig.Repository}/commits?sha={gitHubPostsConfig.Branch}&path={path}");

        if (!response.IsSuccessStatusCode) return [];

        return JsonDocument.Parse(await response.Content.ReadAsStringAsync())
            .RootElement
            .EnumerateArray()
            .Select(
            e => new Commit(
                e.GetProperty("commit")
                 .GetProperty("message")
                 .GetString() ?? "",
                e.GetProperty("commit")
                 .GetProperty("author")
                 .GetProperty("date")
                 .GetDateTime()
                 .ToLocalTime()
                )
            )
            .ToArray();
    }

    public async Task<string> GetContentAsync(string path, GitHubPostsConfig? gitHubPostsConfig = null)
    {
        gitHubPostsConfig ??= GitHubPostsConfig;

        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(gitHubPostsConfig.UserAgent);
        // @TODO: 토큰을 가져오는 방법 찾기
        //httpClient.DefaultRequestHeaders.Authorization = new("Bearer", GitHubPostsConfig.AccessToken);

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

    public async Task<PostMetadata> GetPostMetadataAsync(string postUri)
    {
        string[] lines = (await GetContentAsync(postUri)).Split('\n')
                                                         .Select(static line => line.Trim())
                                                         .ToArray();
        if (lines.Length == 0 || lines.First() != "---") throw new("YAML Front Matter의 시작을 찾을 수 없습니다.");

        int lastIndex = Array.FindIndex(lines, 1, static line => line == "---");
        if (lastIndex == -1) throw new("YAML Front Matter의 끝을 찾을 수 없습니다.");

        return new Deserializer().Deserialize<PostMetadata>(string.Join('\n', lines[1..lastIndex]));
    }
}