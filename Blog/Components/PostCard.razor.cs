using Blog.Models;
using Blog.Services;

namespace Blog.Components;

public partial class PostCard(GitHubService gitHubService)
{
    private string? thumbnailUri;

    protected override async Task OnInitializedAsync()
    {
        Document document = await gitHubService.GetRawFromYamlFrontMatterAsync<Document>(PostUri);

        thumbnailUri = !string.IsNullOrEmpty(document.ThumbnailUri) && document.ThumbnailUri.StartsWith('/')
            ? $"{GitHubService.RawBaseAdddress}/{gitHubService.Owner}/{gitHubService.Repo}/{gitHubService.Ref}/{document.ThumbnailUri}"
            : document.ThumbnailUri;
    }
}
