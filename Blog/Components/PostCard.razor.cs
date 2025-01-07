using Blog.Models;
using Blog.Extensions;
using System.Net.Http.Json;

namespace Blog.Components;

public partial class PostCard(HttpClient httpClient)
{
    private string Title { get; set; } = string.Empty;
    private string Summary { get; set; } = string.Empty;
    private string[] Tags { get; set; } = [];
    private DateTime PublishedDateTime { get; set; }
    private DateTime UpdatedDateTime { get; set; }

    protected override async Task OnInitializedAsync()
    {
        PostMetadata postMetadata = await httpClient.GetFromYamlFrontMatterAsync<PostMetadata>(Path.ChangeExtension(PostUri, "md"));
        Title = postMetadata.Title;
        Summary = postMetadata.Summary;
        Tags = postMetadata.Tags;

        Commit[]? commitHistory = await httpClient.GetFromJsonAsync<Commit[]>($"commit-history/{Path.ChangeExtension(PostUri, "json")}");
        PublishedDateTime = (commitHistory?.FirstOrDefault() ?? default).Timestamp;
        UpdatedDateTime = (commitHistory?.LastOrDefault() ?? default).Timestamp;
    }
}