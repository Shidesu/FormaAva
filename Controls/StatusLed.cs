using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Sentinel.Models;

namespace Sentinel.Controls;

public class StatusLed : TemplatedControl
{
    public static readonly StyledProperty<EquipmentStatus> StatusProperty =
        AvaloniaProperty.Register<StatusLed, EquipmentStatus>(nameof(Status), EquipmentStatus.Offline);

    public StatusLed()
    { 
         UpdatePseudoClasses(Status);
    }

    public EquipmentStatus Status
    {
        get => GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == StatusProperty)
            UpdatePseudoClasses((EquipmentStatus)change.NewValue!);
    }

    private void UpdatePseudoClasses(EquipmentStatus status)
    {
        PseudoClasses.Set(":operational", status == EquipmentStatus.Operational);
        PseudoClasses.Set(":degraded",    status == EquipmentStatus.Degraded);
        PseudoClasses.Set(":offline",     status == EquipmentStatus.Offline);
    }
}
