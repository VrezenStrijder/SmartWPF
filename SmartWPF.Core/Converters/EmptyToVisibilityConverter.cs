using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartWPF.Core.Converters;

/// <summary>
/// 集合为空 -> Visibility
/// 支持 ICollection、IEnumerable、int(Count 值)
/// </summary>
public class EmptyToVisibilityConverter : ValueConverterBase
{
    /// <summary>
    /// 空集合时显示 Visible(默认 false, 空集合时 Collapsed) 
    /// </summary>
    public bool Invert { get; set; }

    public override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isEmpty = value switch
        {
            null => true,
            int count => count == 0,
            ICollection col => col.Count == 0,
            IEnumerable enumerable => !enumerable.Cast<object>().Any(),
            _ => false
        };

        if (Invert)
        {
            isEmpty = !isEmpty;
        }

        return isEmpty ? Visibility.Visible : Visibility.Collapsed;
    }
}
