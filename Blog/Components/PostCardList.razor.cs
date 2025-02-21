using Blog.Services;
using System.Text.Json;

namespace Blog.Components;

public partial class PostCardList
{
    public IEnumerable<string> PostUris { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
    }
}
