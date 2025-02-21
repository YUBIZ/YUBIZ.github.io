namespace Blog.Models;

public class GitHubPostsConfig
{
    public required string AccessToken { get; set; }
    public required string Owner { get; set; }
    public required string Repository { get; set; }
    public required string Branch { get; set; }
    public required string PostListFilePath { get; set; }
    public required string UserAgent { get; set; }
}
