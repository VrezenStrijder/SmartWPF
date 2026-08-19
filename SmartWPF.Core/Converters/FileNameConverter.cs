using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWPF.Core.Converters;

public class FileNameConverter : ValueConverterBase
{
    /// <summary>
    /// 是否包含扩展名 
    /// </summary>
    public bool IncludeExtension { get; set; } = true;

    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string path || string.IsNullOrEmpty(path))
        {
            return "";
        }

        return IncludeExtension ? Path.GetFileName(path) : Path.GetFileNameWithoutExtension(path);
    }
}