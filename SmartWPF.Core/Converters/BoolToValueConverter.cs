using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace SmartWPF.Core.Converters;

/// <summary>
/// 通用 Bool -> 任意值 转换器
/// 用法: 
///   {Binding IsActive, Converter={converters:BoolToValue TrueValue=Green, FalseValue=Red}}
///   可替代 StepCompletedToColorConverter, StepCurrentToFontWeightConverter 等
/// </summary>
public class BoolToValueConverter : ValueConverterBase
{
    public object? TrueValue { get; set; }
    public object? FalseValue { get; set; }
    public bool Invert { get; set; }

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var b = value is bool bv && bv;
        if (Invert)
        {
            b = !b;
        }

        return b ? TrueValue : FalseValue;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (Equals(value, TrueValue))
        {
            return !Invert;
        }

        if (Equals(value, FalseValue))
        {
            return Invert;
        }

        return DependencyProperty.UnsetValue;
    }
}
