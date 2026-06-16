using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Sentinel.ViewModels;

public partial class EquipmentEditViewModel : ObservableValidator
{
    private readonly EquipmentUnitViewModel _unit;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Le nom est obligatoire")]
    [MinLength(2, ErrorMessage = "Minimum 2 caractères")]
    private string _name = string.Empty;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "La zone est obligatoire")]
    private string _zone = string.Empty;

    public EquipmentEditViewModel(EquipmentUnitViewModel unit)
    {
        _unit = unit;
        _name = unit.Name;
        _zone = unit.Zone;

        ErrorsChanged += (_, _) => SaveCommand.NotifyCanExecuteChanged();
        ValidateAllProperties();
    }

    private bool CanSave => !HasErrors;

    [RelayCommand(CanExecute = nameof(CanSave))]
    private void Save()
    {
        _unit.Name = Name;
        _unit.Zone = Zone;
    }
}
