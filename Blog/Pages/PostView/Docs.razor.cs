using Blog.Models;
using Blog.Services;
using Markdig;
using Microsoft.AspNetCore.Components;

namespace Blog.Pages.PostView;

public partial class Docs(GitHubService gitHubService)
{
    private string DocumentUri => $"Docs/{DocumentPath}";
    private string? Title { get; set; }
    private string[]? Tags { get; set; }
    public DateTime PublishedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }
    private MarkupString Content { get; set; }

    protected override async Task OnInitializedAsync()
    {
        PostMetadata postMetadata = await gitHubService.GetPostMetadataAsync(DocumentUri);
        Title = postMetadata.Title;
        Tags = postMetadata.Tags;

        Commit[] commits = await gitHubService.GetCommitsAsync(DocumentUri);
        PublishedDateTime = commits.Last().Timestamp;
        UpdatedDateTime = commits.First().Timestamp;

        MarkdownPipeline markdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().UseYamlFrontMatter().UseBootstrap().Build();
        Content = (MarkupString)Markdown.ToHtml(await gitHubService.GetContentAsync(DocumentUri), markdownPipeline);
    }
}