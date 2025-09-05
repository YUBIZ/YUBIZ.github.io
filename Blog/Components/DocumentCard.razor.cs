using Blog.Misc.HttpClientExtensions;
using Models;

namespace Blog.Components;

public partial class DocumentCard(HttpClient httpClient)
{
    private DocumentMetadata documentMetadata;

    protected override async Task OnInitializedAsync()
    {
        documentMetadata = await httpClient.GetFromYamlFrontMatterAsync<DocumentMetadata>(DocumentUri);
    }
}
