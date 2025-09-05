using Blog.Helpers;
using Markdig;
using Microsoft.AspNetCore.Components;
using Models;

namespace Blog.Pages.PostView;

public partial class Document(HttpClient httpClient)
{
    private DocumentMetadata documentMetadata;

    private MarkupString documentContent;

    protected override async Task OnParametersSetAsync()
    {
        var raw = await httpClient.GetStringAsync(DocumentUri);

        documentMetadata = YamlHelper.DeserializeYamlFrontMatter<DocumentMetadata>(raw);

        MarkdownPipeline markdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().UseYamlFrontMatter().Build();
        documentContent = (MarkupString)Markdown.ToHtml(raw, markdownPipeline);
    }
}
