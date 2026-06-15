using Sentinel.Models;

namespace Sentinel.Services;

public interface IEquipmentService
{
    /// <summary>
    /// Event raised when telemetry data is received.
    /// Must be raised on the UI thread.
    /// </summary>
    public event EventHandler<TelemetryChangedEventArgs>? TelemetryChanged;
    
    public Task<IReadOnlyList<EquipmentUnit>> GetFleetAsync(CancellationToken cancellationToken);
}