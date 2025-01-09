using Blog.Models;
using Blog.Services;

namespace Blog.Components;

public partial class PostCard(StaticResourceService staticResourceService)
{
    private string Title { get; set; } = string.Empty;
    private string Summary { get; set; } = string.Empty;
    private string[] Tags { get; set; } = [];
    private DateTime PublishedDateTime { get; set; }
    private DateTime UpdatedDateTime { get; set; }

    protected override async Task OnInitializedAsync()
    {
        PostMetadata postMetadata = await staticResourceService.GetPostMetadataAsync(PostUri);
        Title = postMetadata.Title;
        Summary = postMetadata.Summary;
        Tags = postMetadata.Tags;

        Commit[]? commitHistory = await staticResourceService.GetCommitHistoryAsync(PostUri);
        PublishedDateTime = (commitHistory?.FirstOrDefault() ?? default).Timestamp;
        UpdatedDateTime = (commitHistory?.LastOrDefault() ?? default).Timestamp;
    }
}