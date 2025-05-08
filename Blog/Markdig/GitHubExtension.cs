using Blog.Services;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Blog.Markdig;

public class GitHubExtension(GitHubService gitHubService) : IMarkdownExtension
{
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        pipeline.DocumentProcessed += PrefixImageUri;
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer) { }

    private void PrefixImageUri(MarkdownDocument document)
    {
        foreach (var item in document.Descendants<LinkInline>())
        {
            if (item.IsImage && !string.IsNullOrEmpty(item.Url) && item.Url.StartsWith('/'))
            {
                item.Url = gitHubService.RawBaseAdddressWithParams + item.Url;
                item.GetAttributes().AddProperty("style", "max-width: 100%;");
            }
        }
    }
}
