using CommunityToolkit.Mvvm.ComponentModel;

namespace Sentinel.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentPage;

    public MainViewModel(HomeViewModel homeViewModel)
    {
        CurrentPage = homeViewModel;
    }
}
