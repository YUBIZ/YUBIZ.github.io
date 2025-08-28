using Microsoft.JSInterop;

namespace Blog.Components;

public partial class GoToTopButton(IJSRuntime jsRuntime)
{
    private async Task GoToTop()
    {
        await jsRuntime.InvokeVoidAsync(
            "window.scrollTo",
            new { top = 0, behavior = "smooth" }
        );
    }
}
