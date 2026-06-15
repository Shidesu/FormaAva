using Sentinel.Models;

namespace Sentinel.Services;

public class TelemetryChangedEventArgs(
    Guid equipmentId,
    double equipmentBattery,
    double equipmentTemperature,
    int equipmentSignal,
    EquipmentStatus equipmentStatus,
    DateTimeOffset equipmentLastSeen) : EventArgs
{
    public Guid EquipmentId { get; } = equipmentId;
    public double EquipmentBattery { get; } = equipmentBattery;
    public double EquipmentTemperature { get; } = equipmentTemperature;
    public int EquipmentSignal { get; } = equipmentSignal;
    public EquipmentStatus EquipmentStatus { get; } = equipmentStatus;
    public DateTimeOffset EquipmentLastSeen { get; } = equipmentLastSeen;
}