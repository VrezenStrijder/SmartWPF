using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartWPF.Core.Converters;

/// <summary>
/// Bool -> Visibility 转换器
/// 用法: 
///   {Binding IsActive, Converter={converters:BoolToVisibility}}
///   {Binding IsActive, Converter={converters:BoolToVisibility Invert=True}}
///   {Binding IsActive, Converter={converters:BoolToVisibility FalseValue=Hidden}}
/// </summary>
public class BoolToVisibilityConverter : ValueConverterBase
{
    /// <summary>
    /// 是否反转逻辑 
    /// </summary>
    public bool Invert { get; set; }

    /// <summary>
    /// false 时对应的 Visibility, 默认 Collapsed 
    /// </summary>
    public Visibility FalseValue { get; set; } = Visibility.Collapsed;

    public override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var boolValue = value is bool b && b;
        if (Invert)
        {
            boolValue = !boolValue;
        }

        return boolValue ? Visibility.Visible : FalseValue;
    }

    public override object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var isVisible = value is Visibility v && v == Visibility.Visible;
        return Invert ? !isVisible : isVisible;
    }
}
