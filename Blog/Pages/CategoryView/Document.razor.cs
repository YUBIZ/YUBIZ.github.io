using Blog.Models;
using Blog.Services;

namespace Blog.Pages.CategoryView;

public partial class Document(GitHubService gitHubService)
{
    private FileTree documentFileTree;

    private FilePathWithCommitHistory[]? documentFilePathWithCommitHistoryCollection;

    protected override async Task OnInitializedAsync()
    {
        documentFileTree = await gitHubService.GetRawFromJsonAsync<FileTree>("DocumentFileTree.json");
        documentFilePathWithCommitHistoryCollection = await gitHubService.GetRawFromJsonAsync<FilePathWithCommitHistory[]>("DocumentFilePathWithCommitHistoryCollection.json");
    }
}
