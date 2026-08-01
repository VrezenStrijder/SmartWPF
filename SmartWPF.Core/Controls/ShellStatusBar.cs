using System.Windows;
using System.Windows.Controls;

namespace SmartWPF.Core.Controls;

/// <summary>
/// 自定义状态栏控件(支持左/中/右内容插槽)
/// </summary>
public class ShellStatusBar : Control
{
    public static readonly DependencyProperty LeftContentProperty =
        DependencyProperty.Register(
            nameof(LeftContent),
            typeof(object),
            typeof(ShellStatusBar),
            new PropertyMetadata(null));

    public static readonly DependencyProperty CenterContentProperty =
        DependencyProperty.Register(
            nameof(CenterContent),
            typeof(object),
            typeof(ShellStatusBar),
            new PropertyMetadata(null));

    public static readonly DependencyProperty RightContentProperty =
        DependencyProperty.Register(
            nameof(RightContent),
            typeof(object),
            typeof(ShellStatusBar),
            new PropertyMetadata(null));

    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    public object? CenterContent
    {
        get => GetValue(CenterContentProperty);
        set => SetValue(CenterContentProperty, value);
    }

    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    static ShellStatusBar()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(ShellStatusBar),
            new FrameworkPropertyMetadata(typeof(ShellStatusBar)));
    }
}
