using Avalonia.Threading;
using Bogus;
using Sentinel.Models;

namespace Sentinel.Services;

public class MockEquipmentService : IEquipmentService, IDisposable
{
    public event EventHandler<TelemetryChangedEventArgs>? TelemetryChanged;
    private readonly List<EquipmentUnit> _equipmentUnits;
    private readonly Faker<EquipmentUnit> _equipmentUnitFaker;
    private readonly PeriodicTimer _timer;
    private readonly Task _backgroundTask;
    private readonly CancellationTokenSource _cts = new();

    public MockEquipmentService()
    {
        _equipmentUnitFaker = new Faker<EquipmentUnit>()
            .UseSeed(0)
            .CustomInstantiator((faker => new EquipmentUnit
            {
                Id = faker.Random.Guid(),
                Name = $"SENTINEL-{faker.IndexFaker}",
                Zone = faker.Address.City(),
                Battery = faker.Random.Double(0, 100),
                Temperature = faker.Random.Double(0, 100),
                Signal = faker.Random.Int(0, 100),
                LastSeen = faker.Date.Past()
            }));

        _equipmentUnits = _equipmentUnitFaker.Generate(15);

        _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));

        _backgroundTask = Task.Run(async () =>
        {
            while (await _timer.WaitForNextTickAsync())
            {
                foreach (var equipment in _equipmentUnits.Select(equipment => equipment with
                         {
                             Temperature = Random.Shared.NextDouble() * 100,
                             Signal = Random.Shared.Next(0, 100),
                             Battery = Random.Shared.NextDouble() * 100,
                             LastSeen = DateTime.UtcNow,
                         }))
                {
                    Dispatcher.UIThread.Post(() =>
                        TelemetryChanged?.Invoke(this,
                            new TelemetryChangedEventArgs(
                                equipment.Id,
                                equipment.Battery,
                                equipment.Temperature,
                                equipment.Signal,
                                equipment.Status,
                                equipment.LastSeen)));
                }
            }
        }, _cts.Token);
    }

    public async Task<IReadOnlyList<EquipmentUnit>> GetFleetAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(200, cancellationToken).ConfigureAwait(false);

        return _equipmentUnits;
    }

    public void Dispose()
    {
        _cts.Cancel();

        _timer.Dispose();
    }
}