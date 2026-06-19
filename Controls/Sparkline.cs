using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace Sentinel.Controls;

public class Sparkline : TemplatedControl
{
    public static readonly StyledProperty<IReadOnlyList<double>?> ValuesProperty =
        AvaloniaProperty.Register<Sparkline, IReadOnlyList<double>?>(nameof(Values));

    public static readonly StyledProperty<IBrush?> StrokeProperty =
        AvaloniaProperty.Register<Sparkline, IBrush?>(nameof(Stroke));

    public IReadOnlyList<double>? Values
    {
        get => GetValue(ValuesProperty);
        set => SetValue(ValuesProperty, value);
    }

    public IBrush? Stroke
    {
        get => GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    private Polyline? _polyline;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _polyline = e.NameScope.Find<Polyline>("PART_Line");
        UpdatePoints();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == ValuesProperty || change.Property == BoundsProperty)
            UpdatePoints();
    }

    private void UpdatePoints()
    {
        if (_polyline is null) return;

        var values = Values;
        if (values is null || values.Count < 2)
        {
            _polyline.Points = [];
            return;
        }

        var w = Bounds.Width;
        var h = Bounds.Height;
        if (w <= 0 || h <= 0) return;

        var min = values.Min();
        var max = values.Max();
        var range = max - min;
        if (range == 0) range = 1;

        var count = values.Count;
        var pts = new AvaloniaList<Point>(count);
        for (int i = 0; i < count; i++)
        {
            var x = i / (double)(count - 1) * w;
            var y = (h - 2) - (values[i] - min) / range * (h - 2);
            pts.Add(new Point(x, y));
        }

        _polyline.Points = pts;
    }
}
