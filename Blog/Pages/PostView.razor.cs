using Blog.Models;
using Blog.Services;
using Microsoft.AspNetCore.Components;

namespace Blog.Pages;

public partial class PostView(StaticResourceService staticResourceService)
{
    private string PostUri => $"posts/{PostNameWithCategory}";
    private string? Title { get; set; }
    private string[]? Tags { get; set; }
    public DateTime PublishedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }
    private MarkupString Content { get; set; }

    protected override async Task OnInitializedAsync()
    {
        PostMetadata postMetadata = await staticResourceService.GetPostMetadataAsync(PostUri);
        Title = postMetadata.Title;
        Tags = postMetadata.Tags;

        Commit[]? commitHistory = await staticResourceService.GetCommitHistoryAsync(PostUri);
        PublishedDateTime = (commitHistory?.FirstOrDefault() ?? default).Timestamp;
        UpdatedDateTime = (commitHistory?.LastOrDefault() ?? default).Timestamp;

        Content = await staticResourceService.GetPostContentAsync(PostUri);
    }
}