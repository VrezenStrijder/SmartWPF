using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWPF.Core.Converters;

/// <summary>
/// DateTime/DateTimeOffset -> 格式化字符串
/// </summary>
public class DateTimeFormatConverter : ValueConverterBase
{
    public string Format { get; set; } = "yyyy-MM-dd HH:mm:ss";
    public string NullText { get; set; } = "";

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // parameter 可作为运行时格式覆盖
        var fmt = parameter as string ?? Format;

        return value switch
        {
            DateTime dt => dt.ToString(fmt, culture),
            DateTimeOffset dto => dto.ToString(fmt, culture),
            _ => NullText
        };
    }
}
