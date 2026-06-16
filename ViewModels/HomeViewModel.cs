using System.Collections.ObjectModel;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sentinel.Services;

namespace Sentinel.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly IEquipmentService _equipmentService;
    private readonly ObservableCollection<EquipmentUnitViewModel> _units = new();
    private readonly Dictionary<Guid, EquipmentUnitViewModel> _unitMap = new();

    public DataGridCollectionView Units { get; }

    [ObservableProperty] private string? _filter;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasSelection))]
    private EquipmentUnitViewModel? _selectedUnit;

    [ObservableProperty] private EquipmentEditViewModel? _editViewModel;

    public bool HasSelection => SelectedUnit is not null;

    partial void OnSelectedUnitChanged(EquipmentUnitViewModel? value)
        => EditViewModel = value is not null ? new EquipmentEditViewModel(value) : null;

    public HomeViewModel(IEquipmentService equipmentService)
    {
        _equipmentService = equipmentService;

        Units = new DataGridCollectionView(_units)
        {
            Filter = o => o is EquipmentUnitViewModel vm &&
                          (string.IsNullOrWhiteSpace(_filter) ||
                           vm.Name.Contains(_filter, StringComparison.OrdinalIgnoreCase) ||
                           vm.Zone.Contains(_filter, StringComparison.OrdinalIgnoreCase) ||
                           vm.Status.ToString().Contains(_filter, StringComparison.OrdinalIgnoreCase))
        };

        _equipmentService.TelemetryChanged += (_, args) => UpdateEquipmentFromTelemetry(args);

        _ = LoadAsync();
    }

    partial void OnFilterChanged(string? value) => Units.Refresh();

    private void UpdateEquipmentFromTelemetry(TelemetryChangedEventArgs args)
    {
        if (_unitMap.TryGetValue(args.EquipmentId, out var vm))
            vm.ApplyTelemetry(args);
    }

    [RelayCommand]
    private async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        _unitMap.Clear();
        _units.Clear();

        var units = await _equipmentService.GetFleetAsync(cancellationToken);
        foreach (var unit in units)
        {
            var vm = new EquipmentUnitViewModel(unit);
            _unitMap[vm.Id] = vm;
            _units.Add(vm);
        }
    }
}
