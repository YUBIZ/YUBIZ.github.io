using Blog.Services;
using Microsoft.AspNetCore.Components;

namespace Blog.Pages;

public partial class Post(StaticResourceService staticResourceService)
{
    private string PostUri => $"posts/{PostNameWithCategory}";
    private string? Title { get; set; }
    private MarkupString Content { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Title = (await staticResourceService.GetPostMetadataAsync(PostUri)).Title;
        Content = await staticResourceService.GetPostContentAsync(PostUri);
    }
}