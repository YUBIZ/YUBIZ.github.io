using System.Net.Http.Json;

namespace Blog.Components;

public partial class PostCardList(HttpClient httpClient)
{
    public IEnumerable<string> PostUris { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        PostUris = await httpClient.GetFromJsonAsync<string[]>("file-list/posts.json") ?? [];
    }
}
