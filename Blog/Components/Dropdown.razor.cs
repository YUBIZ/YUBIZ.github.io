namespace Blog.Components;

public partial class Dropdown<T>
{
    private async Task OnItemSelectedAsync(T newSelectedItem)
    {
        if (!EqualityComparer<T>.Default.Equals(SelectedItem, newSelectedItem)) await SelectedItemChanged.InvokeAsync(SelectedItem = newSelectedItem);
    }
}
