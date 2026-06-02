using System;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SmartWPF.Core.Controls;

/// <summary>
/// 窗体布局配置对象(可绑定到 ViewModel)。
/// </summary>
public partial class ShellLayoutConfig : ObservableObject
{
    [ObservableProperty] private ShellLayoutType layoutType = ShellLayoutType.LeftMain;

    [ObservableProperty] private bool showLeftPane = true;
    [ObservableProperty] private bool isLeftPaneExpanded = true;
    [ObservableProperty] private GridLength leftPaneOpenLength = new(260);
    [ObservableProperty] private GridLength leftPaneCompactLength = new(56);

    [ObservableProperty] private bool showRightPane;
    [ObservableProperty] private bool isRightPaneOpen = false;
    [ObservableProperty] private GridLength rightPaneLength = new(300);

    [ObservableProperty] private bool showBottomPane;
    [ObservableProperty] private bool isBottomPaneOpen = false;
    [ObservableProperty] private GridLength bottomPaneLength = new(220);

    [ObservableProperty] private bool showStatusBar = true;
    [ObservableProperty] private bool showTitleBarThemeMenu = true;
    [ObservableProperty] private double windowOpacity = 1.0;
    [ObservableProperty] private bool hasRightPaneContent = true;
    [ObservableProperty] private bool hasBottomPaneContent = true;

    public GridLength LeftPaneLength => ShowLeftPane
        ? (IsLeftPaneExpanded ? LeftPaneOpenLength : LeftPaneCompactLength)
        : new GridLength(0);

    public GridLength RightPaneEffectiveLength => (ShowRightPane && HasRightPaneContent && IsRightPaneOpen)
        ? RightPaneLength
        : new GridLength(0);

    public GridLength BottomPaneEffectiveLength => (ShowBottomPane && HasBottomPaneContent && IsBottomPaneOpen)
        ? BottomPaneLength
        : new GridLength(0);

    public Visibility StatusBarVisibility => ShowStatusBar ? Visibility.Visible : Visibility.Collapsed;
    public Visibility TitleBarThemeMenuVisibility => ShowTitleBarThemeMenu ? Visibility.Visible : Visibility.Collapsed;
    public Visibility LeftPaneToggleVisibility => ShowLeftPane ? Visibility.Visible : Visibility.Collapsed;
    public Visibility RightPaneToggleVisibility => (ShowRightPane && HasRightPaneContent) ? Visibility.Visible : Visibility.Collapsed;
    public Visibility BottomPaneToggleVisibility => (ShowBottomPane && HasBottomPaneContent) ? Visibility.Visible : Visibility.Collapsed;
    public Visibility RightPaneRegionVisibility => (ShowRightPane && HasRightPaneContent && IsRightPaneOpen) ? Visibility.Visible : Visibility.Collapsed;
    public Visibility BottomPaneRegionVisibility => (ShowBottomPane && HasBottomPaneContent && IsBottomPaneOpen) ? Visibility.Visible : Visibility.Collapsed;
    public Visibility RightPaneSettingVisibility => (ShowRightPane && HasRightPaneContent) ? Visibility.Visible : Visibility.Collapsed;
    public Visibility BottomPaneSettingVisibility => (ShowBottomPane && HasBottomPaneContent) ? Visibility.Visible : Visibility.Collapsed;

    public ShellLayoutConfig()
    {
        ApplyLayoutType(layoutType);
    }

    public void ToggleLeftPane()
    {
        if (!ShowLeftPane)
        {
            return;
        }

        IsLeftPaneExpanded = !IsLeftPaneExpanded;
        RaiseComputedProperties();
    }

    public void ToggleRightPane()
    {
        if (!ShowRightPane)
        {
            return;
        }

        IsRightPaneOpen = !IsRightPaneOpen;
        RaiseComputedProperties();
    }

    public void ToggleBottomPane()
    {
        if (!ShowBottomPane)
        {
            return;
        }

        IsBottomPaneOpen = !IsBottomPaneOpen;
        RaiseComputedProperties();
    }

    partial void OnLayoutTypeChanged(ShellLayoutType value)
    {
        ApplyLayoutType(value);
        RaiseComputedProperties();
    }

    partial void OnShowLeftPaneChanged(bool value) => RaiseComputedProperties();
    partial void OnIsLeftPaneExpandedChanged(bool value) => RaiseComputedProperties();
    partial void OnLeftPaneOpenLengthChanged(GridLength value) => RaiseComputedProperties();
    partial void OnLeftPaneCompactLengthChanged(GridLength value) => RaiseComputedProperties();

    partial void OnShowRightPaneChanged(bool value) => RaiseComputedProperties();
    partial void OnIsRightPaneOpenChanged(bool value) => RaiseComputedProperties();
    partial void OnRightPaneLengthChanged(GridLength value) => RaiseComputedProperties();

    partial void OnShowBottomPaneChanged(bool value) => RaiseComputedProperties();
    partial void OnIsBottomPaneOpenChanged(bool value) => RaiseComputedProperties();
    partial void OnBottomPaneLengthChanged(GridLength value) => RaiseComputedProperties();

    partial void OnShowStatusBarChanged(bool value) => RaiseComputedProperties();
    partial void OnShowTitleBarThemeMenuChanged(bool value) => RaiseComputedProperties();
    partial void OnWindowOpacityChanged(double value)
    {
        var clamped = Math.Clamp(value, 0.65, 1.0);
        if (Math.Abs(clamped - value) > 0.0001)
        {
            WindowOpacity = clamped;
            return;
        }

        RaiseComputedProperties();
    }
    partial void OnHasRightPaneContentChanged(bool value)
    {
        if (!value)
        {
            IsRightPaneOpen = false;
        }

        RaiseComputedProperties();
    }
    partial void OnHasBottomPaneContentChanged(bool value)
    {
        if (!value)
        {
            IsBottomPaneOpen = false;
        }

        RaiseComputedProperties();
    }

    private void ApplyLayoutType(ShellLayoutType value)
    {
        switch (value)
        {
            case ShellLayoutType.MainOnly:
                ShowLeftPane = false;
                ShowRightPane = false;
                ShowBottomPane = false;
                break;
            case ShellLayoutType.LeftMain:
                ShowLeftPane = true;
                ShowRightPane = false;
                ShowBottomPane = false;
                break;
            case ShellLayoutType.LeftMainRight:
                ShowLeftPane = true;
                ShowRightPane = true;
                ShowBottomPane = false;
                break;
            case ShellLayoutType.LeftMainBottom:
                ShowLeftPane = true;
                ShowRightPane = false;
                ShowBottomPane = true;
                break;
            case ShellLayoutType.LeftMainRightBottom:
                ShowLeftPane = true;
                ShowRightPane = true;
                ShowBottomPane = true;
                break;
        }
    }

    private void RaiseComputedProperties()
    {
        OnPropertyChanged(nameof(LeftPaneLength));
        OnPropertyChanged(nameof(RightPaneEffectiveLength));
        OnPropertyChanged(nameof(BottomPaneEffectiveLength));
        OnPropertyChanged(nameof(StatusBarVisibility));
        OnPropertyChanged(nameof(TitleBarThemeMenuVisibility));
        OnPropertyChanged(nameof(LeftPaneToggleVisibility));
        OnPropertyChanged(nameof(RightPaneToggleVisibility));
        OnPropertyChanged(nameof(BottomPaneToggleVisibility));
        OnPropertyChanged(nameof(RightPaneRegionVisibility));
        OnPropertyChanged(nameof(BottomPaneRegionVisibility));
        OnPropertyChanged(nameof(RightPaneSettingVisibility));
        OnPropertyChanged(nameof(BottomPaneSettingVisibility));
    }
}

public enum ShellLayoutType
{
    MainOnly,
    LeftMain,
    LeftMainRight,
    LeftMainBottom,
    LeftMainRightBottom
}
