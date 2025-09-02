using Blog.Misc.HttpClientExtensions;
using Blog.Models;

namespace Blog.Components;

public partial class DocumentCard(HttpClient httpClient)
{
    private DocumentMetadata documentMetadata;

    protected override async Task OnInitializedAsync()
    {
        documentMetadata = await httpClient.GetFromYamlFrontMatterAsync<DocumentMetadata>(DocumentUri);
    }
}
