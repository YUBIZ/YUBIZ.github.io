namespace Blog.Components;

public partial class FileTreeView
{
    private string CurrentDirectoryPath => Path.Combine(ParentPath, FileTree.Value.DirectoryName);

    private async Task OnNameSelectedAsync(string newSelectedName)
    {
        if (SelectedName != newSelectedName) await SelectedNameChanged.InvokeAsync(SelectedName = newSelectedName);
    }
}
