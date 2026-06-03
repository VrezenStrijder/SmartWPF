using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace SmartWPF.Core.Appearance;

/// <summary>
/// DPI 变化监听行为
/// (附加到 Window 上, 监听 DPI 变化并触发回调)
/// 
/// 用法: 
///   var window = new MainWindow();
///   DpiMonitorBehavior.Attach(window, info => {
///       // DPI 变化时的额外处理
///       Debug.WriteLine($"DPI changed to {info.NewDpiX}x{info.NewDpiY}");
///   });
/// </summary>
public static class DpiMonitorBehavior
{
    /// <summary>
    /// 附加 DPI 监听到窗口
    /// </summary>
    public static void Attach(Window window, Action<DpiChangeInfo>? onDpiChanged = null)
    {
        window.Loaded += (s, e) =>
        {
            var source = HwndSource.FromHwnd(new WindowInteropHelper(window).Handle);
            source?.AddHook((IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) =>
            {
                // WM_DPICHANGED = 0x02E0
                if (msg == 0x02E0)
                {
                    var newDpiX = wParam.ToInt32() & 0xFFFF;
                    var newDpiY = (wParam.ToInt32() >> 16) & 0xFFFF;

                    var info = new DpiChangeInfo
                    {
                        NewDpiX = newDpiX,
                        NewDpiY = newDpiY,
                        ScaleX = newDpiX / 96.0,
                        ScaleY = newDpiY / 96.0,
                        Window = window
                    };

                    onDpiChanged?.Invoke(info);
                }
                return IntPtr.Zero;
            });
        };
    }
}

public class DpiChangeInfo
{
    public int NewDpiX { get; init; }
    public int NewDpiY { get; init; }
    public double ScaleX { get; init; }
    public double ScaleY { get; init; }
    public Window? Window { get; init; }
}