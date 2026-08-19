using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace SmartWPF.Core.Controls;

/// <summary>
/// 消息容器控件
/// (承载消息卡片的容器, 通常放在主窗口的 Grid 最上层)
/// </summary>
public class MessageContainer : ItemsControl
{
    public static readonly DependencyProperty MessagePositionProperty =
        DependencyProperty.Register(
            nameof(MessagePosition),
            typeof(MessagePosition),
            typeof(MessageContainer),
            new PropertyMetadata(MessagePosition.TopRight, OnPositionChanged));

    public MessagePosition MessagePosition
    {
        get => (MessagePosition)GetValue(MessagePositionProperty);
        set => SetValue(MessagePositionProperty, value);
    }

    static MessageContainer()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(MessageContainer),
            new FrameworkPropertyMetadata(typeof(MessageContainer)));
    }

    public MessageContainer()
    {
        IsHitTestVisible = false; // 不拦截底层控件的鼠标事件
    }

    private static void OnPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is MessageContainer container)
        {
            container.UpdateAlignment();
        }
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        UpdateAlignment();
    }

    private void UpdateAlignment()
    {
        switch (MessagePosition)
        {
            case MessagePosition.TopCenter:
                HorizontalAlignment = HorizontalAlignment.Center;
                VerticalAlignment = VerticalAlignment.Top;
                break;
            case MessagePosition.TopRight:
                HorizontalAlignment = HorizontalAlignment.Right;
                VerticalAlignment = VerticalAlignment.Top;
                break;
            case MessagePosition.BottomCenter:
                HorizontalAlignment = HorizontalAlignment.Center;
                VerticalAlignment = VerticalAlignment.Bottom;
                break;
            case MessagePosition.BottomRight:
                HorizontalAlignment = HorizontalAlignment.Right;
                VerticalAlignment = VerticalAlignment.Bottom;
                break;
        }
    }
}

public enum MessagePosition
{
    TopCenter,
    TopRight,
    BottomCenter,
    BottomRight
}
