using Blog.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Blog.Services;

public class ConfigService(WebAssemblyHostConfiguration configuration)
{
    public GitHubPostsConfig GitHubPostsConfig { get; } = configuration.GetSection("GitHub:Posts").Get<GitHubPostsConfig>() ?? throw new ArgumentNullException(nameof(GitHubPostsConfig));
}
