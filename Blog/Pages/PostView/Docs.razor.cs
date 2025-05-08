using Blog.Markdig;
using Blog.Models;
using Blog.Services;
using Markdig;
using Microsoft.AspNetCore.Components;

namespace Blog.Pages.PostView;

public partial class Docs(GitHubService gitHubService)
{
    private string Title => Path.GetFileNameWithoutExtension(PostUri);

    private Document document;

    private MarkupString content;

    protected override async Task OnParametersSetAsync()
    {
        var raw = await gitHubService.GetRawStringAsync($"Docs/{PostUri}");

        document = Helper.DeserializeYamlFrontMatter<Document>(raw);

        MarkdownPipeline markdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().UseYamlFrontMatter().Use(new GitHubExtension(gitHubService)).Build();
        content = (MarkupString)Markdown.ToHtml(raw, markdownPipeline);
    }
}
