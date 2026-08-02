using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartWPF.Core.Converters;

/// <summary>
/// 枚举值 -> [Description] 特性文本(无特性则返回 ToString)
/// </summary>
public class EnumDescriptionConverter : ValueConverterBase
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return "";
        }

        var field = value.GetType().GetField(value.ToString()!);
        var attr = field?.GetCustomAttribute<DescriptionAttribute>();

        return attr?.Description ?? value.ToString();
    }
}
