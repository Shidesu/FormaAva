using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Sentinel.Controls;

public class CircularGauge : TemplatedControl
{
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<CircularGauge, double>(nameof(Value));

    public static readonly StyledProperty<double> MaximumProperty =
        AvaloniaProperty.Register<CircularGauge, double>(nameof(Maximum), defaultValue: 100.0);

    public static readonly StyledProperty<IBrush?> GaugeBrushProperty =
        AvaloniaProperty.Register<CircularGauge, IBrush?>(nameof(GaugeBrush));

    public static readonly StyledProperty<string> ValueTextProperty =
        AvaloniaProperty.Register<CircularGauge, string>(nameof(ValueText), defaultValue: "0%");

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

    public IBrush? GaugeBrush
    {
        get => GetValue(GaugeBrushProperty);
        private set => SetValue(GaugeBrushProperty, value);
    }

    public string ValueText
    {
        get => GetValue(ValueTextProperty);
        private set => SetValue(ValueTextProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == ValueProperty || change.Property == MaximumProperty)
            UpdateGaugeBrush();
    }

    private void UpdateGaugeBrush()
    {
        var ratio = Maximum > 0 ? Math.Clamp(Value / Maximum, 0, 1) : 0;

        var progressColor = ratio > 0.5 ? Color.Parse("#22DD77")
                          : ratio > 0.25 ? Color.Parse("#FFAA00")
                          : Color.Parse("#FF3333");

        var trackColor = Color.Parse("#2A2A3A");

        // Sharp boundary: two stops at the same offset for a clean cut
        var brush = new ConicGradientBrush
        {
            Center = RelativePoint.Center,
            Angle = -90,
            GradientStops =
            {
                new GradientStop(progressColor, 0),
                new GradientStop(progressColor, ratio),
                new GradientStop(trackColor,    ratio),
                new GradientStop(trackColor,    1),
            }
        };

        GaugeBrush = brush;
        ValueText = $"{Value:F0}%";
    }
}
