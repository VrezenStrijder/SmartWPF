using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartWPF.Core.Logging;
using System.Windows;

namespace SmartWPF.Core.Appearance;

/// <summary>
/// 响应式密度行为
/// (根据窗口宽度自动切换视觉密度)
/// </summary>
public class ResponsiveDensityBehavior
{
    private readonly IAppearanceService appearance;
    private readonly ILogService? log;
    private Window? window;
    private bool isActive = true;
    private DensityMode? lastAutoMode;

    /// <summary>
    /// 窗口宽度低于此值时切换到 Compact 
    /// </summary>
    public double CompactThreshold { get; set; } = 800;

    /// <summary>
    /// 窗口宽度超过此值时切换到 Presentation 
    /// </summary>
    public double PresentationThreshold { get; set; } = 1600;

    /// <summary>
    /// 是否启用自动密度 
    /// </summary>
    public bool IsActive
    {
        get => isActive;
        set
        {
            isActive = value;
            if (value && window != null)
            {
                Evaluate(window.ActualWidth);
            }
        }
    }

    public ResponsiveDensityBehavior(IAppearanceService appearance, ILogService? log = null)
    {
        this.appearance = appearance;
        this.log = log;
    }

    /// <summary>
    /// 附加到窗口 
    /// </summary>
    public void Attach(Window window)
    {
        this.window = window;
        window.SizeChanged += OnSizeChanged;

        if (window.IsLoaded)
        {
            Evaluate(window.ActualWidth);
        }
        else
        {
            window.Loaded += (s, e) => Evaluate(window.ActualWidth);
        }
    }

    /// <summary>
    /// 从窗口分离 
    /// </summary>
    public void Detach()
    {
        if (window != null)
        {
            window.SizeChanged -= OnSizeChanged;
            window = null;
        }
    }

    /// <summary>
    /// 暂停自动模式(用户手动选择密度时调用) 
    /// </summary>
    public void Pause() => isActive = false;

    /// <summary>
    /// 恢复自动模式 
    /// </summary>
    public void Resume()
    {
        isActive = true;
        if (window != null)
        {
            Evaluate(window.ActualWidth);
        }
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (isActive)
        {
            Evaluate(e.NewSize.Width);
        }
    }

    private void Evaluate(double width)
    {
        var target = width switch
        {
            _ when width < CompactThreshold => DensityMode.Compact,
            _ when width > PresentationThreshold => DensityMode.Presentation,
            _ => DensityMode.Default
        };

        if (target == lastAutoMode)
        {
            return;
        }

        lastAutoMode = target;
        appearance.SetDensity(target);
        log?.Debug("ResponsiveDensity: width={Width} -> {Density}", width, target);
    }
}