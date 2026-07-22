using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SmartWPF.Core.Motion;

namespace SmartWPF.Core.Controls;

/// <summary>
/// FluentDialogWindow.xaml 的交互逻辑
/// </summary>
public partial class FluentDialogWindow : Window
{
    private FluentDialogResult result = FluentDialogResult.Cancel;
    private bool allowDrag = true;

    public FluentDialogResult Result => result;

    public FluentDialogWindow()
    {
        InitializeComponent();
        Loaded += OnWindowLoaded;
    }

    public void Configure(FluentDialogOptions options)
    {
        allowDrag = options.AllowDrag;

        if (allowDrag)
        {
            DialogCard.MouseLeftButtonDown += OnCardMouseLeftButtonDown;
        }

        TitleText.Text = options.Title;
        MessageText.Text = options.Message;

        // 图标和颜色
        var (iconKey, brushKey) = options.Type switch
        {
            FluentDialogType.Success => ("SuccessIconPath", "SuccessBrush"),
            FluentDialogType.Warning => ("WarningIconPath", "WarningBrush"),
            FluentDialogType.Error => ("ErrorIconPath", "ErrorBrush"),
            FluentDialogType.Confirm => ("ConfirmIconPath", "AccentPrimaryBrush"),
            _ => ("InfoIconPath", "InfoBrush")
        };

        IconPath.Data = (Geometry)FindResource(iconKey);
        IconPath.Fill = (Brush)FindResource(brushKey);

        // 危险/警告时按钮样式
        if (options.Type is FluentDialogType.Error)
        {
            PrimaryButton.Style = (Style)FindResource("ButtonDanger");
        }
        else if (options.Type is FluentDialogType.Warning)
        {
            PrimaryButton.Background = (Brush)FindResource("WarningBrush");
            PrimaryButton.BorderBrush = (Brush)FindResource("WarningBrush");
        }

        // 按钮文本与可见性
        switch (options.Buttons)
        {
            case FluentDialogButton.OK:
                PrimaryButton.Content = options.PrimaryButtonText ?? "OK";
                break;

            case FluentDialogButton.OKCancel:
                PrimaryButton.Content = options.PrimaryButtonText ?? "OK";
                SecondaryButton.Content = options.SecondaryButtonText ?? "Cancel";
                SecondaryButton.Visibility = Visibility.Visible;
                break;

            case FluentDialogButton.YesNo:
                PrimaryButton.Content = options.PrimaryButtonText ?? "Yes";
                SecondaryButton.Content = options.SecondaryButtonText ?? "No";
                SecondaryButton.Visibility = Visibility.Visible;
                break;

            case FluentDialogButton.YesNoCancel:
                PrimaryButton.Content = options.PrimaryButtonText ?? "Yes";
                SecondaryButton.Content = options.SecondaryButtonText ?? "No";
                SecondaryButton.Visibility = Visibility.Visible;
                CancelButton.Content = options.CancelButtonText ?? "Cancel";
                CancelButton.Visibility = Visibility.Visible;
                break;
        }
    }

    private void OnCardMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        // 如果点击的是按钮则不触发拖动
        if (e.OriginalSource is Button or TextBlock { Parent: Button })
        {
            return;
        }

        if (e.ButtonState == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    private async void OnWindowLoaded(object sender, RoutedEventArgs e)
    {
        // 进入动画
        AnimationHelper.For(DialogCard)
            .EnterDialog()
            .Start();
    }

    private async void CloseWithResult(FluentDialogResult dialogResult)
    {
        this.result = dialogResult;

        // 退出动画
        await AnimationHelper.For(DialogCard)
            .ExitDialog()
            .StartAsync();

        DialogResult = dialogResult == FluentDialogResult.Primary;
    }

    private void OnPrimaryClick(object sender, RoutedEventArgs e)
        => CloseWithResult(FluentDialogResult.Primary);

    private void OnSecondaryClick(object sender, RoutedEventArgs e)
        => CloseWithResult(FluentDialogResult.Secondary);

    private void OnCancelClick(object sender, RoutedEventArgs e)
        => CloseWithResult(FluentDialogResult.Cancel);
}

public enum FluentDialogResult
{
    Primary,
    Secondary,
    Cancel
}