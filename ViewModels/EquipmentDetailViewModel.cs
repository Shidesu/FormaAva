using CommunityToolkit.Mvvm.Input;
using Sentinel.Services;

namespace Sentinel.ViewModels;

public partial class EquipmentDetailViewModel : ViewModelBase
{
    private readonly INavigationService _navigation;

    public EquipmentUnitViewModel Unit { get; }
    public EquipmentEditViewModel EditViewModel { get; }

    public EquipmentDetailViewModel(EquipmentUnitViewModel unit, INavigationService navigation)
    {
        Unit = unit;
        _navigation = navigation;
        EditViewModel = new EquipmentEditViewModel(unit);
    }

    [RelayCommand]
    private void Back() => _navigation.GoBack();
}
