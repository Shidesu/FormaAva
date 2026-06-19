using CommunityToolkit.Mvvm.ComponentModel;
using Sentinel.Models;
using Sentinel.Services;

namespace Sentinel.ViewModels;

public partial class EquipmentUnitViewModel : ObservableObject
{
    private const int HistoryCapacity = 30;

    public Guid Id { get; }
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _zone = string.Empty;

    [ObservableProperty] private double _battery;
    [ObservableProperty] private double _temperature;
    [ObservableProperty] private int _signal;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasAlert))]
    private EquipmentStatus _status;
    [ObservableProperty] private DateTimeOffset _lastSeen;

    public bool HasAlert => Status != EquipmentStatus.Operational;

    private readonly Queue<double> _batteryHistory = new();
    private readonly Queue<double> _temperatureHistory = new();
    private readonly Queue<double> _signalHistory = new();

    public IReadOnlyList<double> BatteryHistory     => _batteryHistory.ToList();
    public IReadOnlyList<double> TemperatureHistory => _temperatureHistory.ToList();
    public IReadOnlyList<double> SignalHistory      => _signalHistory.ToList();

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

        Push(_batteryHistory, _battery);
        Push(_temperatureHistory, _temperature);
        Push(_signalHistory, _signal);
    }

    public void ApplyTelemetry(TelemetryChangedEventArgs args)
    {
        Battery     = args.EquipmentBattery;
        Temperature = args.EquipmentTemperature;
        Signal      = args.EquipmentSignal;
        Status      = args.EquipmentStatus;
        LastSeen    = args.EquipmentLastSeen;

        Push(_batteryHistory, Battery);
        Push(_temperatureHistory, Temperature);
        Push(_signalHistory, Signal);

        OnPropertyChanged(nameof(BatteryHistory));
        OnPropertyChanged(nameof(TemperatureHistory));
        OnPropertyChanged(nameof(SignalHistory));
    }

    private static void Push(Queue<double> buffer, double value)
    {
        buffer.Enqueue(value);
        if (buffer.Count > HistoryCapacity)
            buffer.Dequeue();
    }
}
