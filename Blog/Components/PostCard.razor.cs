namespace Blog.Components;

public partial class PostCard
{
    private string Title { get; set; } = string.Empty;
    private string Summary { get; set; } = string.Empty;
    private string[] Tags { get; set; } = [];
    private DateTime PublishedDateTime { get; set; }
    private DateTime UpdatedDateTime { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // @TODO: 게시글 메타데이터를 가져오는 로직 구현하기
        // @TODO: 게시글 커밋 이력을 가져오는 로직 구현하기
    }
}