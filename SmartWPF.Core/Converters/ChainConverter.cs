using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace SmartWPF.Core.Converters;

/// <summary>
/// 链式转换器 — 将多个转换器串联起来
/// 用法: 
/// <![CDATA[
/// <converters:ChainConverter>
///     <converters:MathConverter Operation="Multiply" Operand="100"/>
///     <converters:PercentageConverter/>
/// </converters:ChainConverter>
/// ]]>
/// </summary>
[ContentProperty(nameof(Converters))]
public class ChainConverter : ValueConverterBase
{
    public List<IValueConverter> Converters { get; set; } = [];

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Converters.Aggregate(value,
            (current, converter) => converter.Convert(current, targetType, parameter, culture));
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Converters
            .AsEnumerable()
            .Reverse()
            .Aggregate(value,
                (current, converter) => converter.ConvertBack(current, targetType, parameter, culture));
    }
}
