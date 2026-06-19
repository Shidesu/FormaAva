using Sentinel.ViewModels;

namespace Sentinel.Services;

public class NavigationService : INavigationService
{
    private readonly Stack<ViewModelBase> _stack = new();

    public ViewModelBase? CurrentPage => _stack.TryPeek(out var page) ? page : null;
    public bool CanGoBack => _stack.Count > 1;

    public event EventHandler? Navigated;

    public void NavigateTo(ViewModelBase page)
    {
        _stack.Push(page);
        Navigated?.Invoke(this, EventArgs.Empty);
    }

    public void GoBack()
    {
        if (!CanGoBack) return;
        _stack.Pop();
        Navigated?.Invoke(this, EventArgs.Empty);
    }
}
