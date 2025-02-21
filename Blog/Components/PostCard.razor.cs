using Blog.Models;
using Blog.Services;

namespace Blog.Components;

public partial class PostCard(GitHubService gitHubService)
{
    private string Title { get; set; } = string.Empty;
    private string Summary { get; set; } = string.Empty;
    private string[] Tags { get; set; } = [];
    private DateTime PublishedDateTime { get; set; }
    private DateTime UpdatedDateTime { get; set; }

    protected override async Task OnInitializedAsync()
    {
        PostMetadata postMetadata = await gitHubService.GetPostMetadataAsync(PostUri);

        Title = postMetadata.Title;
        Summary = postMetadata.Summary;
        Tags = postMetadata.Tags;

        Commit[] commits = await gitHubService.GetCommitsAsync(PostUri);
        PublishedDateTime = commits.Last().Timestamp;
        UpdatedDateTime = commits.First().Timestamp;
    }
}