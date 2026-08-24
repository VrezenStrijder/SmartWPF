using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWPF.Core.Converters;

public class FileSizeConverter : ValueConverterBase
{
    private static readonly string[] Units = ["B", "KB", "MB", "GB", "TB", "PB"];

    public int DecimalPlaces { get; set; } = 2;

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        double bytes = value switch
        {
            long l => l,
            int i => i,
            double d => d,
            _ => 0
        };

        int order = 0;
        while (bytes >= 1024 && order < Units.Length - 1)
        {
            order++;
            bytes /= 1024;
        }

        var fmt = $"F{DecimalPlaces}";
        return $"{bytes.ToString(fmt, culture)} {Units[order]}";
    }
}
