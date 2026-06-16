using System.Collections.ObjectModel;
using Avalonia.Collections;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Sentinel.Models;
using Sentinel.Services;

namespace Sentinel.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly IEquipmentService _equipmentService;

    public ReadOnlyObservableCollection<EquipmentUnit> Units
    {
        get => _units;
    }

    [ObservableProperty] private string? _filter;

    private readonly SourceList<EquipmentUnit> _source = new();
    private ReadOnlyObservableCollection<EquipmentUnit> _units;

    public HomeViewModel(IEquipmentService equipmentService)
    {
        _equipmentService = equipmentService;

        _equipmentService.TelemetryChanged += (_, args) =>
        {
            if (!Dispatcher.UIThread.CheckAccess())
            {
                Dispatcher.UIThread.Post(() => UpdateEquipmentFromTelemetry(args));
                return;
            }

            UpdateEquipmentFromTelemetry(args);
        };

        _ = LoadAsync();

        _source.Connect()
            .Filter(o =>
            {
                if (string.IsNullOrWhiteSpace(_filter)) return true;

                if (o is not { } unit) return false;

                return unit.Name.Contains(_filter, StringComparison.OrdinalIgnoreCase);
            })
            .Bind(out _units)
            .Subscribe();
    }

    private void UpdateEquipmentFromTelemetry(TelemetryChangedEventArgs args)
    {
        var equipment = _source.Items
            .Select(((unit, i) => (Unit: unit, Index: i)))
            .FirstOrDefault((x) => x.Unit.Id == args.EquipmentId);

        if (equipment.Unit is null)
            return;

        _source.ReplaceAt(equipment.Index, equipment.Unit with
        {
            Battery = args.EquipmentBattery,
            Temperature = args.EquipmentTemperature,
            Signal = args.EquipmentSignal,
            LastSeen = args.EquipmentLastSeen,
        });
    }

    [RelayCommand]
    private async Task LoadAsync(CancellationToken cancellationToken = default)
    {
        _source.Clear();
        _source.AddRange(await _equipmentService.GetFleetAsync(cancellationToken));
    }
}