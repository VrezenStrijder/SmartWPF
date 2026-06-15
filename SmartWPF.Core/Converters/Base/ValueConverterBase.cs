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
/// 值转换器基类, 同时作为 MarkupExtension, 可直接在 XAML Binding 中使用
/// 用法: {Binding Path, Converter={converters:MyConverter Param=Value}}
/// </summary>
public abstract class ValueConverterBase : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public abstract object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture);

    public virtual object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException($"{GetType().Name} does not support ConvertBack.");
}

/// <summary>
/// 带类型约束的值转换器基类, 减少类型检查
/// </summary>
public abstract class ValueConverterBase<TSource, TTarget> : ValueConverterBase
{
    public override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TSource source)
        {
            return Convert(source, parameter, culture);
        }

        return DefaultTargetValue;
    }

    public override object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TTarget target)
        {
            return ConvertBack(target, parameter, culture);
        }

        return DefaultSourceValue;
    }

    protected abstract TTarget? Convert(TSource value, object? parameter, CultureInfo culture);

    protected virtual TSource? ConvertBack(TTarget value, object? parameter, CultureInfo culture)
        => throw new NotSupportedException($"{GetType().Name} does not support ConvertBack.");

    /// <summary>
    /// source 值不匹配时的默认返回 
    /// </summary>
    protected virtual TTarget? DefaultTargetValue => default;

    /// <summary>
    /// target 值不匹配时的默认返回 
    /// </summary>
    protected virtual TSource? DefaultSourceValue => default;
}
