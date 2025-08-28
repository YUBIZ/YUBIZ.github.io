using Blog.Models;
using Blog.Services;

namespace Blog.Pages.CategoryView;

public partial class Docs(GitHubService gitHubService)
{
    private string[]? docsList;
    private FileTree docsTree;

    private string[]? SubDocsList => docsList?.Where(v => v.StartsWith($"Docs/{CategoryUri}")).ToArray();

    protected override async Task OnInitializedAsync()
    {
        docsTree = await gitHubService.GetRawFromJsonAsync<FileTree>("DocsTree.json");
        docsList = await gitHubService.GetRawFromJsonAsync<string[]>("DocsList.json");
    }
}
