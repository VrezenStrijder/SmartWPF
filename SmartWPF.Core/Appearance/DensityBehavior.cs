using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartWPF.Core.Appearance;

/// <summary>
/// 声明式启用响应式密度
///
/// 用法: 
///   <Window appearance:DensityBehavior.AutoDensity="True"
///           appearance:DensityBehavior.CompactBelow="800"
///           appearance:DensityBehavior.PresentationAbove="1600"/>
/// </summary>
public static class DensityBehavior
{
    private static readonly Dictionary<Window, ResponsiveDensityBehavior> behaviors = new();

    #region 自动密度

    public static readonly DependencyProperty AutoDensityProperty =
        DependencyProperty.RegisterAttached(
            "AutoDensity", typeof(bool), typeof(DensityBehavior),
            new PropertyMetadata(false, OnAutoDensityChanged));

    public static bool GetAutoDensity(DependencyObject obj) => (bool)obj.GetValue(AutoDensityProperty);
    public static void SetAutoDensity(DependencyObject obj, bool value) => obj.SetValue(AutoDensityProperty, value);

    #endregion

    #region CompactBelow

    public static readonly DependencyProperty CompactBelowProperty =
        DependencyProperty.RegisterAttached(
            "CompactBelow", typeof(double), typeof(DensityBehavior),
            new PropertyMetadata(800.0));

    public static double GetCompactBelow(DependencyObject obj) => (double)obj.GetValue(CompactBelowProperty);
    public static void SetCompactBelow(DependencyObject obj, double value) => obj.SetValue(CompactBelowProperty, value);

    #endregion

    #region PresentationAbove

    public static readonly DependencyProperty PresentationAboveProperty =
        DependencyProperty.RegisterAttached(
            "PresentationAbove", typeof(double), typeof(DensityBehavior),
            new PropertyMetadata(1600.0));

    public static double GetPresentationAbove(DependencyObject obj) => (double)obj.GetValue(PresentationAboveProperty);
    public static void SetPresentationAbove(DependencyObject obj, double value) => obj.SetValue(PresentationAboveProperty, value);

    #endregion

    #region 实现

    private static void OnAutoDensityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Window window)
        {
            return;
        }

        if ((bool)e.NewValue)
        {
            var appearance = GetServiceFromApp<IAppearanceService>();
            if (appearance == null)
            {
                return;
            }

            var behavior = new ResponsiveDensityBehavior(appearance)
            {
                CompactThreshold = GetCompactBelow(window),
                PresentationThreshold = GetPresentationAbove(window)
            };
            behavior.Attach(window);
            behaviors[window] = behavior;

            window.Closed += (s, _) =>
            {
                if (behaviors.Remove(window, out var b))
                {
                    b.Detach();
                }
            };
        }
        else
        {
            if (behaviors.Remove(window, out var behavior))
            {
                behavior.Detach();
            }
        }
    }

    private static T? GetServiceFromApp<T>() where T : class
    {
        // 通过全局服务定位器获取(仅在附加属性中使用)
        try
        {
            var prop = Application.Current?.GetType().GetProperty("Services",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            var provider = prop?.GetValue(null) as IServiceProvider;
            return provider?.GetService(typeof(T)) as T;
        }
        catch { return null; }
    }

    #endregion

}