namespace Blog.Models;

public class GitHubPostsConfig
{
    // @TODO: 토큰을 가져오는 방법 찾기
    //public required string AccessToken { get; set; }
    public required string Owner { get; set; }
    public required string Repository { get; set; }
    public required string Branch { get; set; }
    public required string PostListFilePath { get; set; }
    public required string UserAgent { get; set; }
}
