using Blog.Misc;
using Blog.Models;
using Blog.Services;

namespace Blog.Pages.CategoryView;

public partial class Document(GitHubService gitHubService)
{
    private OrderType _currentOrderType = OrderType.CreateTime;
    private OrderType CurrentOrderType
    {
        get => _currentOrderType;
        set
        {
            _currentOrderType = value;
            FilterAndOrderCollection();
        }
    }

    private OrderDirection _currentOrderDirection = OrderDirection.Descending;
    private OrderDirection CurrentOrderDirection
    {
        get => _currentOrderDirection;
        set
        {
            _currentOrderDirection = value;
            FilterAndOrderCollection();
        }
    }

    private string _documentCategory = string.Empty;
    private string DocumentCategory
    {
        get => _documentCategory;
        set
        {
            _documentCategory = value;
            FilterAndOrderCollection();
        }
    }

    private FileTree documentFileTree;

    private FilePathAndCommitHistory[]? documentFilePathAndCommitHistoryCollection;

    private FilePathAndCommitHistory[]? filteredAndOrderedDocumentFilePathAndCommitHistoryCollection;

    protected override async Task OnInitializedAsync()
    {
        documentFileTree = await gitHubService.GetRawFromJsonAsync<FileTree>("DocumentFileTree.json");
        documentFilePathAndCommitHistoryCollection = await gitHubService.GetRawFromJsonAsync<FilePathAndCommitHistory[]>("DocumentFilePathAndCommitHistoryCollection.json");
        FilterAndOrderCollection();
    }

    private void FilterAndOrderCollection()
    {
        var collection = documentFilePathAndCommitHistoryCollection?.Where(v => v.FilePath.StartsWith(DocumentCategory));

        Func<FilePathAndCommitHistory, string> orderTitleFunc = v => v.FilePath;

        Func<FilePathAndCommitHistory, object> orderFunc = CurrentOrderType switch
        {
            OrderType.CreateTime => (v => v.CommitHistory.Min(v1 => v1.Date)),
            OrderType.LastUpdateTime => (v => v.CommitHistory.Max(v1 => v1.Date)),
            _ => throw new NotImplementedException()
        };

        collection = CurrentOrderDirection switch
        {
            OrderDirection.Ascending => collection?.OrderBy(orderFunc).ThenBy(orderTitleFunc),
            OrderDirection.Descending => collection?.OrderByDescending(orderFunc).ThenByDescending(orderTitleFunc),
            _ => throw new NotImplementedException()
        };

        filteredAndOrderedDocumentFilePathAndCommitHistoryCollection = collection?.ToArray();
    }
}
