using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Sentinel.Controls;

public class SignalBars : TemplatedControl
{
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<SignalBars, double>(nameof(Value));

    public static readonly StyledProperty<double> MaximumProperty =
        AvaloniaProperty.Register<SignalBars, double>(nameof(Maximum), defaultValue: 100.0);

    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public double Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ValueProperty || change.Property == MaximumProperty)
            UpdatePseudoClasses(ComputeLevel());
    }

    private int ComputeLevel()
    {
        var max = Maximum;
        if (max <= 0) return 0;
        var ratio = Value / max;
        return ratio <= 0    ? 0 :
               ratio <= 0.25 ? 1 :
               ratio <= 0.50 ? 2 :
               ratio <= 0.75 ? 3 : 4;
    }

    private void UpdatePseudoClasses(int level)
    {
        PseudoClasses.Set(":l1", level >= 1);
        PseudoClasses.Set(":l2", level >= 2);
        PseudoClasses.Set(":l3", level >= 3);
        PseudoClasses.Set(":l4", level >= 4);
    }
}
