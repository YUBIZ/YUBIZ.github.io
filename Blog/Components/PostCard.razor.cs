using Blog.Models;
using Blog.Services;

namespace Blog.Components;

public partial class PostCard(GitHubService gitHubService)
{
    private string? thumbnailUri;

    protected override async Task OnInitializedAsync()
    {
        Document document = await gitHubService.GetRawFromYamlFrontMatterAsync<Document>(PostUri);

        thumbnailUri = document.ThumbnailUri;

        if (string.IsNullOrEmpty(thumbnailUri))
        {
            thumbnailUri = "error.svg";
        }
        else if (thumbnailUri.StartsWith('/'))
        {
            thumbnailUri = gitHubService.RawBaseAdddressWithParams + thumbnailUri;
        }
    }
}
