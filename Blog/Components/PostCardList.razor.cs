using Blog.Services;
using System.Text.Json;

namespace Blog.Components;

public partial class PostCardList(GitHubService gitHubService)
{
    public IEnumerable<string> PostUris { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        PostUris = await gitHubService.GetPostListAsync();
    }
}
