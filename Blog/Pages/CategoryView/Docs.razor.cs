using Blog.Models;
using Blog.Services;

namespace Blog.Pages.CategoryView;

public partial class Docs(GitHubService gitHubService)
{
    private FileTree docsTree;

    private IEnumerable<string> DocsList => docsTree.EnumerateFiles().Where(v => v.StartsWith(DocumentCategoryUri));

    protected override async Task OnInitializedAsync()
    {
        docsTree = await gitHubService.GetRawFromJsonAsync<FileTree>("DocsTree.json");
    }
}
