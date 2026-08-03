using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartWPF.Core.Converters;

/// <summary>
/// 值比较转换器 — 比较 value 和 parameter, 返回 bool/Visibility/自定义值
/// 可用于 RadioButton/绑定枚举等
/// </summary>
public class ComparisonConverter : ValueConverterBase
{
    public enum ResultMode { Boolean, Visibility }

    public ResultMode Mode { get; set; } = ResultMode.Boolean;

    public override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isEqual = Equals(value?.ToString(), parameter?.ToString());

        return Mode switch
        {
            ResultMode.Visibility => isEqual ? Visibility.Visible : Visibility.Collapsed,
            _ => isEqual
        };
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b && b)
        {
            // 尝试转为目标枚举
            if (targetType.IsEnum && parameter is string s)
            {
                return Enum.Parse(targetType, s);
            }

            return parameter;
        }
        return DependencyProperty.UnsetValue;
    }
}
