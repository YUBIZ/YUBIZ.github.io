using Microsoft.AspNetCore.Components;

namespace Blog.Pages;

public partial class PostView
{
    private string PostUri => $"posts/{PostNameWithCategory}";
    private string? Title { get; set; }
    private string[]? Tags { get; set; }
    public DateTime PublishedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }
    private MarkupString Content { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // @TODO: 게시글 메타데이터를 가져오는 로직 구현하기
        // @TODO: 게시글 커밋 이력을 가져오는 로직 구현하기
        // @TODO: 게시글 내용을 가져오는 로직 구현하기
    }
}