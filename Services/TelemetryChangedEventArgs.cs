using Sentinel.Models;

namespace Sentinel.Services;

public class TelemetryChangedEventArgs(
    Guid equipmentId,
    double equipmentBattery,
    double equipmentTemperature,
    int equipmentSignal,
    EquipmentStatus equipmentStatus,
    DateTimeOffset equipmentLastSeen) : EventArgs;