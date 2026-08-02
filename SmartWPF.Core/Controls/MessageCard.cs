using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SmartWPF.Core.Controls;

/// <summary>
/// 消息卡片控件
/// </summary>
public class MessageCard : ContentControl
{
    public static readonly DependencyProperty MessageTypeProperty =
        DependencyProperty.Register(
            nameof(MessageType),
            typeof(MessageType),
            typeof(MessageCard),
            new PropertyMetadata(MessageType.Information));

    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(MessageCard),
            new PropertyMetadata(""));

    public static readonly DependencyProperty IsClosingProperty =
        DependencyProperty.Register(
            nameof(IsClosing),
            typeof(bool),
            typeof(MessageCard),
            new PropertyMetadata(false));

    public MessageType MessageType
    {
        get => (MessageType)GetValue(MessageTypeProperty);
        set => SetValue(MessageTypeProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public bool IsClosing
    {
        get => (bool)GetValue(IsClosingProperty);
        set => SetValue(IsClosingProperty, value);
    }

    public static readonly RoutedEvent CloseRequestedEvent =
        EventManager.RegisterRoutedEvent(
            "CloseRequested",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(MessageCard));

    public event RoutedEventHandler CloseRequested
    {
        add => AddHandler(CloseRequestedEvent, value);
        remove => RemoveHandler(CloseRequestedEvent, value);
    }

    static MessageCard()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(MessageCard),
            new FrameworkPropertyMetadata(typeof(MessageCard)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild("PART_CloseButton") is Button closeBtn)
        {
            closeBtn.Click += (s, e) => RaiseEvent(new RoutedEventArgs(CloseRequestedEvent));
        }

        // 鼠标进入时暂停自动关闭
        IsHitTestVisible = true;
    }
}

public partial class MessageItem : ObservableObject
{
    public string Id { get; } = Guid.NewGuid().ToString("N");
    public string Title { get; init; } = "";
    public string Content { get; init; } = "";
    public MessageType Type { get; init; } = MessageType.Information;
    public int Duration { get; init; } = 3000;
    public DateTime CreatedAt { get; } = DateTime.Now;

    [ObservableProperty]
    private bool isClosing;

    public Action? OnClose { get; set; }
}

public enum MessageType
{
    Information,
    Success,
    Warning,
    Error
}

