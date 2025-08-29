using Blog.Models;
using Blog.Services;

namespace Blog.Components;

public partial class DocumentCard(GitHubService gitHubService)
{
    private DocumentMetadata documentMetadata;

    protected override async Task OnInitializedAsync()
    {
        documentMetadata = await gitHubService.GetRawFromYamlFrontMatterAsync<DocumentMetadata>(DocumentUri);
    }

    private string GetThumbnailUri()
    {
        return (documentMetadata.ThumbnailUri.StartsWith('/') ? gitHubService.RawBaseAdddressWithParams : "") + documentMetadata.ThumbnailUri;
    }
}
