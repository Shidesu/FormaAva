using CommunityToolkit.Mvvm.ComponentModel;
using Sentinel.Models;
using Sentinel.Services;

namespace Sentinel.ViewModels;

public partial class EquipmentUnitViewModel : ObservableObject
{
    public Guid Id { get; }
    public string Name { get; }
    public string Zone { get; }

    [ObservableProperty] private double _battery;
    [ObservableProperty] private double _temperature;
    [ObservableProperty] private int _signal;
    [ObservableProperty] private EquipmentStatus _status;
    [ObservableProperty] private DateTimeOffset _lastSeen;

    public EquipmentUnitViewModel(EquipmentUnit unit)
    {
        Id = unit.Id;
        Name = unit.Name;
        Zone = unit.Zone;
        _battery = unit.Battery;
        _temperature = unit.Temperature;
        _signal = unit.Signal;
        _status = unit.Status;
        _lastSeen = unit.LastSeen;
    }

    public void ApplyTelemetry(TelemetryChangedEventArgs args)
    {
        Battery = args.EquipmentBattery;
        Temperature = args.EquipmentTemperature;
        Signal = args.EquipmentSignal;
        Status = args.EquipmentStatus;
        LastSeen = args.EquipmentLastSeen;
    }
}
