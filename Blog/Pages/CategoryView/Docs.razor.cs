using Blog.Services;

namespace Blog.Pages.CategoryView;

public partial class Docs(GitHubService gitHubService)
{
    private string[]? docsList;

    private string[]? SubDocsList => docsList?.Where(v => v.StartsWith($"Docs/{CategoryUri}")).ToArray();

    protected override async Task OnInitializedAsync()
    {
        docsList = await gitHubService.GetRawFromJsonAsync<string[]>("DocsList.json");
    }
}
