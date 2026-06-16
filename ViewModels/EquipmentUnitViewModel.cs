using CommunityToolkit.Mvvm.ComponentModel;
using Sentinel.Models;
using Sentinel.Services;

namespace Sentinel.ViewModels;

public partial class EquipmentUnitViewModel : ObservableObject
{
    public Guid Id { get; }
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _zone = string.Empty;

    [ObservableProperty] private double _battery;
    [ObservableProperty] private double _temperature;
    [ObservableProperty] private int _signal;
    [ObservableProperty] private EquipmentStatus _status;
    [ObservableProperty] private DateTimeOffset _lastSeen;

    public EquipmentUnitViewModel(EquipmentUnit unit)
    {
        Id = unit.Id;
        _name = unit.Name;
        _zone = unit.Zone;
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
