using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.VisualTree;
using Avalonia.Controls.Shapes;

namespace CircleApp;

public partial class MainWindow : Window
{
    private const double MinimumRadius = 1.0;
    private double _radius = 100.0;

    private Canvas? _rootCanvas;
    private Ellipse? _circle;

    public MainWindow()
    {
        InitializeComponent();
        this.Opened += OnOpened;
        this.SizeChanged += OnSizeChanged;
    }

    private void OnOpened(object? sender, EventArgs e)
    {
        _rootCanvas = this.FindControl<Canvas>("RootCanvas");
        _circle = this.FindControl<Ellipse>("Circle");

        // Ensure we can receive keyboard input
        this.Focus();

        // Initialize circle geometry and position
        UpdateCircleSize();
        CenterCircle();
    }

    private void UpdateCircleSize()
    {
        if (_circle is null)
        {
            return;
        }

        double diameter = _radius * 2.0;
        _circle.Width = diameter;
        _circle.Height = diameter;
    }

    private void CenterCircle()
    {
        if (_rootCanvas is null || _circle is null)
        {
            return;
        }

        double windowWidth = this.ClientSize.Width;
        double windowHeight = this.ClientSize.Height;

        double left = (windowWidth - _circle.Width) / 2.0;
        double top = (windowHeight - _circle.Height) / 2.0;

        Canvas.SetLeft(_circle, Math.Max(0, left));
        Canvas.SetTop(_circle, Math.Max(0, top));
    }

    private void OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        CenterCircle();
    }

    private double GetMaxAllowedRadius()
    {
        // Maximum radius is half of the current window width, per requirement
        return Math.Max(MinimumRadius, this.ClientSize.Width / 2.0);
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        // Left: decrease radius, Right: increase radius
        const double step = 5.0;

        if (e.Key == Key.Left)
        {
            _radius = Math.Max(MinimumRadius, _radius - step);
            UpdateCircleSize();
            CenterCircle();
            e.Handled = true;
        }
        else if (e.Key == Key.Right)
        {
            double maxRadius = GetMaxAllowedRadius();
            _radius = Math.Min(maxRadius, _radius + step);
            UpdateCircleSize();
            CenterCircle();
            e.Handled = true;
        }
    }
}