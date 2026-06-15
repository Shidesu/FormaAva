namespace Sentinel.Models;

public record EquipmentUnit
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Zone { get; init; }

    public EquipmentStatus Status => this switch
    {
        { Temperature: >= 90 } or { Signal: <= 10 and > 0 } or { Battery: <= 20 } => EquipmentStatus.Degraded,
        { Signal: 0 } => EquipmentStatus.Offline,
        _ => EquipmentStatus.Operational
    };

    public required double Battery { get; init; }
    public required double Temperature { get; init; }
    public required int Signal { get; init; }
    public required DateTimeOffset LastSeen { get; init; }
}