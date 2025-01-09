using Blog.Services;

namespace Blog.Components;

public partial class PostCardList(StaticResourceService staticResourceService)
{
    public IEnumerable<string> PostUris { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        PostUris = await staticResourceService.GetFileListAsync("posts");
    }
}
