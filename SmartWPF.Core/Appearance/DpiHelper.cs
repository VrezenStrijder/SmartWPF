using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace SmartWPF.Core.Appearance;

/// <summary>
/// DPI 工具类
/// </summary>
public static class DpiHelper
{
    /// <summary>
    /// 获取 Visual 所在显示器的 DPI 缩放因子 
    /// </summary>
    public static double GetScale(Visual visual)
    {
        var source = PresentationSource.FromVisual(visual);
        return source?.CompositionTarget?.TransformToDevice.M11 ?? 1.0;
    }

    /// <summary>
    /// 获取 X/Y 方向的 DPI 值 
    /// </summary>
    public static (double X, double Y) GetDpi(Visual visual)
    {
        var source = PresentationSource.FromVisual(visual);
        var m = source?.CompositionTarget?.TransformToDevice;
        return (96.0 * (m?.M11 ?? 1.0), 96.0 * (m?.M22 ?? 1.0));
    }

    /// <summary>
    /// 逻辑像素 -> 物理像素 
    /// </summary>
    public static double ToPhysical(double logical, double scale) => logical * scale;

    /// <summary>
    /// 物理像素 -> 逻辑像素 
    /// </summary>
    public static double ToLogical(double physical, double scale) => physical / scale;

    /// <summary>
    /// 像素对齐(避免模糊边框) 
    /// </summary>
    public static double RoundToPixel(double value, double scale)
        => Math.Round(value * scale, MidpointRounding.AwayFromZero) / scale;

    /// <summary>
    /// 像素对齐 Thickness 
    /// </summary>
    public static Thickness RoundToPixel(Thickness thickness, double scale) => new(
        RoundToPixel(thickness.Left, scale),
        RoundToPixel(thickness.Top, scale),
        RoundToPixel(thickness.Right, scale),
        RoundToPixel(thickness.Bottom, scale));

    /// <summary>
    /// 获取系统主显示器 DPI(不依赖 Visual) 
    /// </summary>
    public static (double X, double Y) GetSystemDpi()
    {
        using var source = new HwndSource(new HwndSourceParameters());
        var m = source.CompositionTarget?.TransformToDevice;
        return (96.0 * (m?.M11 ?? 1.0), 96.0 * (m?.M22 ?? 1.0));
    }

    /// <summary>
    /// 创建 DPI 感知的 RenderTargetBitmap(截屏/导出时使用)
    /// </summary>
    public static RenderTargetBitmap CreateDpiAwareBitmap(Visual visual, int width, int height)
    {
        var (dpiX, dpiY) = GetDpi(visual);
        var rtb = new RenderTargetBitmap(
            (int)(width * dpiX / 96),
            (int)(height * dpiY / 96),
            dpiX, dpiY,
            PixelFormats.Pbgra32);
        rtb.Render(visual);
        return rtb;
    }
}
