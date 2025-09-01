using Blog.Helpers;
using Blog.Markdig;
using Blog.Models;
using Blog.Services;
using Markdig;
using Microsoft.AspNetCore.Components;

namespace Blog.Pages.PostView;

public partial class Document(GitHubService gitHubService)
{
    private DocumentMetadata documentMetadata;

    private MarkupString documentContent;

    protected override async Task OnParametersSetAsync()
    {
        var raw = await gitHubService.GetRawStringAsync(DocumentUri);

        documentMetadata = YamlHelper.DeserializeYamlFrontMatter<DocumentMetadata>(raw);

        MarkdownPipeline markdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().UseYamlFrontMatter().Use(new GitHubExtension(gitHubService)).Build();
        documentContent = (MarkupString)Markdown.ToHtml(raw, markdownPipeline);
    }
}
