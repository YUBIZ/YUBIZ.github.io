using Blog.Models;
using Blog.Services;

namespace Blog.Layout;

public partial class MainLayout(GitHubService gitHubService)
{
    private FileTree DocsTree { get; set; }

    protected override async Task OnInitializedAsync()
    {
        DocsTree = await gitHubService.GetRawFromJsonAsync<FileTree>("DocsTree.json");
    }
}
