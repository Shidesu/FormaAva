using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sentinel.Services;

namespace Sentinel.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly INavigationService _navigation;

    [ObservableProperty] private ViewModelBase? _currentPage;

    public bool CanGoBack => _navigation.CanGoBack;

    public MainViewModel(HomeViewModel homeViewModel, INavigationService navigation)
    {
        _navigation = navigation;
        _navigation.Navigated += (_, _) =>
        {
            CurrentPage = _navigation.CurrentPage;
            OnPropertyChanged(nameof(CanGoBack));
            GoBackCommand.NotifyCanExecuteChanged();
        };

        _navigation.NavigateTo(homeViewModel);
    }

    [RelayCommand(CanExecute = nameof(CanGoBack))]
    private void GoBack() => _navigation.GoBack();

    [RelayCommand]
    private void ToggleTheme()
    {
        Application.Current!.RequestedThemeVariant =
            Application.Current.ActualThemeVariant == ThemeVariant.Dark
                ? ThemeVariant.Light
                : ThemeVariant.Dark;
    }
}
