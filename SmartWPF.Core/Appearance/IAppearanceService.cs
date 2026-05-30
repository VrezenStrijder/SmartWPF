using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWPF.Core.Appearance;

public interface IAppearanceService
{
    // 当前状态
    ThemeMode CurrentTheme { get; }
    DensityMode CurrentDensity { get; }
    string CurrentThemeName { get; }

    // 初始化
    void Initialize(ThemeMode theme = ThemeMode.Light, DensityMode density = DensityMode.Default, string themeName = "Default");

    // 切换

    /// <summary>
    /// 切换主题模式。返回 false 表示当前命名主题不支持该模式。
    /// </summary>
    bool SetTheme(ThemeMode theme);

    void SetDensity(DensityMode density);

    /// <summary>
    /// 切换命名主题
    /// </summary>
    bool SetNamedTheme(string name, ThemeMode? preferredMode = null);

    // 命名主题注册

    void RegisterTheme(string name, Dictionary<ThemeMode, Uri> modeResources);
    IReadOnlyList<string> RegisteredThemeNames { get; }
    IReadOnlyList<ThemeMode> GetSupportedModes(string themeName);
    bool SupportsMode(string themeName, ThemeMode mode);

    // 事件
    event Action<ThemeMode>? ThemeChanged;
    event Action<DensityMode>? DensityChanged;
    event Action<string>? ThemeNameChanged;
}
