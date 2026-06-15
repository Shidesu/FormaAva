using System.Collections.ObjectModel;
using Avalonia.Collections;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using Sentinel.Models;
using Sentinel.Services;

namespace Sentinel.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly IEquipmentService _equipmentService;

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
    }

    public AvaloniaList<EquipmentUnit> Units { get; } = [];

    private void UpdateEquipmentFromTelemetry(TelemetryChangedEventArgs args)
    {
        var equipment = Units
            .Select(((unit, i) => (Unit: unit, Index: i)))
            .FirstOrDefault((x) => x.Unit.Id == args.EquipmentId);

        Units[equipment.Index] = equipment.Unit with
        {
            Battery = args.EquipmentBattery,
            Temperature = args.EquipmentTemperature,
            Signal = args.EquipmentSignal,
            LastSeen = args.EquipmentLastSeen,
        };
    }

    [RelayCommand]
    private async Task LoadAsync(CancellationToken cancellationToken)
    {
        Units.Clear();
        Units.AddRange(await _equipmentService.GetFleetAsync(cancellationToken));
    }
}