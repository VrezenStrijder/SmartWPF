using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWPF.Core.Controls;

public enum FluentDialogType
{
    Information,
    Success,
    Warning,
    Error,
    Confirm
}

public enum FluentDialogButton
{
    OK,
    OKCancel,
    YesNo,
    YesNoCancel
}

public class FluentDialogOptions
{
    public string Title { get; init; } = "";
    public string Message { get; init; } = "";
    public FluentDialogType Type { get; init; } = FluentDialogType.Information;
    public FluentDialogButton Buttons { get; init; } = FluentDialogButton.OK;
    public string? PrimaryButtonText { get; init; }
    public string? SecondaryButtonText { get; init; }
    public string? CancelButtonText { get; init; }

    /// <summary>
    /// 是否允许拖动对话框
    /// </summary>
    public bool AllowDrag { get; init; } = true;
}

