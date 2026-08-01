using System.Windows;
using System.Windows.Input;
using System.Reflection;
using CommunityToolkit.Mvvm.Input;

namespace SmartWPF.Core.Controls;

/// <summary>
/// 支持自定义标题栏和可配置布局区域的基础窗体。
/// </summary>
public class ShellChromeWindow : Window
{
    public static readonly DependencyProperty LayoutConfigProperty =
        DependencyProperty.Register(
            nameof(LayoutConfig), typeof(ShellLayoutConfig), typeof(ShellChromeWindow),
            new PropertyMetadata(null));

    public static readonly DependencyProperty LeftPaneContentProperty =
        DependencyProperty.Register(
            nameof(LeftPaneContent), typeof(object), typeof(ShellChromeWindow),
            new PropertyMetadata(null));

    public static readonly DependencyProperty MainContentProperty =
        DependencyProperty.Register(
            nameof(MainContent), typeof(object), typeof(ShellChromeWindow),
            new PropertyMetadata(null));

    public static readonly DependencyProperty RightPaneContentProperty =
        DependencyProperty.Register(
            nameof(RightPaneContent), typeof(object), typeof(ShellChromeWindow),
            new PropertyMetadata(null));

    public static readonly DependencyProperty BottomPaneContentProperty =
        DependencyProperty.Register(
            nameof(BottomPaneContent), typeof(object), typeof(ShellChromeWindow),
            new PropertyMetadata(null));

    public static readonly DependencyProperty StatusBarContentProperty =
        DependencyProperty.Register(
            nameof(StatusBarContent), typeof(object), typeof(ShellChromeWindow),
            new PropertyMetadata(null));

    public static readonly DependencyProperty TitleBarLeftContentProperty =
        DependencyProperty.Register(
            nameof(TitleBarLeftContent), typeof(object), typeof(ShellChromeWindow),
            new PropertyMetadata(null));

    public static readonly DependencyProperty TitleBarRightContentProperty =
        DependencyProperty.Register(
            nameof(TitleBarRightContent), typeof(object), typeof(ShellChromeWindow),
            new PropertyMetadata(null));

    public static readonly DependencyProperty HeaderContentProperty =
        DependencyProperty.Register(
            nameof(HeaderContent), typeof(object), typeof(ShellChromeWindow),
            new PropertyMetadata(null));

    public static readonly DependencyProperty ShowSystemCaptionButtonsProperty =
        DependencyProperty.Register(
            nameof(ShowSystemCaptionButtons), typeof(bool), typeof(ShellChromeWindow),
            new PropertyMetadata(true));

    public ShellLayoutConfig LayoutConfig
    {
        get => (ShellLayoutConfig)GetValue(LayoutConfigProperty);
        set => SetValue(LayoutConfigProperty, value);
    }

    public object? LeftPaneContent
    {
        get => GetValue(LeftPaneContentProperty);
        set => SetValue(LeftPaneContentProperty, value);
    }

    public object? MainContent
    {
        get => GetValue(MainContentProperty);
        set => SetValue(MainContentProperty, value);
    }

    public object? RightPaneContent
    {
        get => GetValue(RightPaneContentProperty);
        set => SetValue(RightPaneContentProperty, value);
    }

    public object? BottomPaneContent
    {
        get => GetValue(BottomPaneContentProperty);
        set => SetValue(BottomPaneContentProperty, value);
    }

    public object? StatusBarContent
    {
        get => GetValue(StatusBarContentProperty);
        set => SetValue(StatusBarContentProperty, value);
    }

    public object? TitleBarLeftContent
    {
        get => GetValue(TitleBarLeftContentProperty);
        set => SetValue(TitleBarLeftContentProperty, value);
    }

    public object? TitleBarRightContent
    {
        get => GetValue(TitleBarRightContentProperty);
        set => SetValue(TitleBarRightContentProperty, value);
    }

    public object? HeaderContent
    {
        get => GetValue(HeaderContentProperty);
        set => SetValue(HeaderContentProperty, value);
    }

    public bool ShowSystemCaptionButtons
    {
        get => (bool)GetValue(ShowSystemCaptionButtonsProperty);
        set => SetValue(ShowSystemCaptionButtonsProperty, value);
    }

    public ICommand ToggleLeftPaneCommand { get; }
    public ICommand ToggleRightPaneCommand { get; }
    public ICommand ToggleBottomPaneCommand { get; }

    static ShellChromeWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(ShellChromeWindow),
            new FrameworkPropertyMetadata(typeof(ShellChromeWindow)));
    }

    public ShellChromeWindow()
    {
        LayoutConfig = new ShellLayoutConfig();
        DataContextChanged += OnDataContextChanged;

        ToggleLeftPaneCommand = new RelayCommand(() => LayoutConfig?.ToggleLeftPane());
        ToggleRightPaneCommand = new RelayCommand(() => LayoutConfig?.ToggleRightPane());
        ToggleBottomPaneCommand = new RelayCommand(() => LayoutConfig?.ToggleBottomPane());
    }
    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue == null)
        {
            return;
        }

        var vmType = e.NewValue.GetType();
        var windowLayoutProp = vmType.GetProperty("WindowLayout", BindingFlags.Public | BindingFlags.Instance);
        if (windowLayoutProp == null)
        {
            return;
        }

        if (windowLayoutProp.GetValue(e.NewValue) is ShellLayoutConfig config)
        {
            LayoutConfig = config;
        }
    }
}
