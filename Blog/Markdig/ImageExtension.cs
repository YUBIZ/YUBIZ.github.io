using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Blog.Markdig;

public class ImageExtension : IMarkdownExtension
{
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        pipeline.DocumentProcessed += SetupWidth;
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer) { }

    private void SetupWidth(MarkdownDocument document)
    {
        foreach (var item in document.Descendants<LinkInline>())
        {
            if (item.IsImage)
            {
                item.GetAttributes().AddProperty("style", "width: 100%; height: auto; aspect-ratio: 16 / 9; object-fit: contain;");
            }
        }
    }
}
