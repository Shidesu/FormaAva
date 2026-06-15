namespace Sentinel.Models;

public record EquipmentUnit
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Zone { get; init; }
    public required EquipmentStatus Status { get; init; }
    public required double Battery { get; init; }
    public required double Temperature { get; init; }
    public required int Signal { get; init; }
    public required DateTimeOffset LastSeen { get; init; }
}