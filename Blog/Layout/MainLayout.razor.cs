using Blog.Models;
using Blog.Services;

namespace Blog.Layout;

public partial class MainLayout(GitHubService gitHubService)
{
    private FileTree docsTree;

    private bool isSidebarOpen = false;

    protected override async Task OnInitializedAsync()
    {
        docsTree = await gitHubService.GetRawFromJsonAsync<FileTree>("DocsTree.json");
    }
}
