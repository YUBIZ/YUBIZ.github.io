using Blog.Models;
using System.IO;
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

        HttpResponseMessage response = await httpClient.GetAsync($"https://api.github.com/repos/{gitHubPostsConfig.Owner}/{gitHubPostsConfig.Repository}/commits?sha={gitHubPostsConfig.Branch}&path={path}");

        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.BadRequest: throw new("잘못된 요청입니다.");
                case System.Net.HttpStatusCode.Unauthorized: throw new("인증되지 않았습니다.");
                case System.Net.HttpStatusCode.Forbidden: throw new("접근 권한이 없습니다.");
                case System.Net.HttpStatusCode.NotFound: throw new("요청을 찾을 수 없습니다.");
            }
        }

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

        var response = await httpClient.GetAsync($"https://raw.githubusercontent.com/{gitHubPostsConfig.Owner}/{gitHubPostsConfig.Repository}/{gitHubPostsConfig.Branch}/{path}");

        if (!response.IsSuccessStatusCode)
        {
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.BadRequest: throw new("잘못된 요청입니다.");
                case System.Net.HttpStatusCode.Unauthorized: throw new("인증되지 않았습니다.");
                case System.Net.HttpStatusCode.Forbidden: throw new("접근 권한이 없습니다.");
                case System.Net.HttpStatusCode.NotFound: throw new("요청을 찾을 수 없습니다.");
            }
        }

        return await response.Content.ReadAsStringAsync();
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