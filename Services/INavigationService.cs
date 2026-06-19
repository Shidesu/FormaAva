using Sentinel.ViewModels;

namespace Sentinel.Services;

public interface INavigationService
{
    ViewModelBase? CurrentPage { get; }
    bool CanGoBack { get; }
    event EventHandler? Navigated;
    void NavigateTo(ViewModelBase page);
    void GoBack();
}
