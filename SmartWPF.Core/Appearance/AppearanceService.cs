using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartWPF.Core.Appearance;
using SmartWPF.Core.Logging;
using System.Windows;

namespace SmartWPF.Core.Appearance;

public class AppearanceService : IAppearanceService
{
    private static readonly string AsmName = typeof(AppearanceService).Assembly.GetName().Name!;

    private const string ColorTag = "/Tokens/Colors.";
    private const string DensityTag = "/Tokens/Density.";
    private const string ControlsTag = "/Controls/";

    private readonly Dictionary<string, Dictionary<ThemeMode, Uri>> themes = new();
    private readonly ILogService? log;
    private bool initialized;

    // 公开状态
    public ThemeMode CurrentTheme { get; private set; }
    public DensityMode CurrentDensity { get; private set; }
    public string CurrentThemeName { get; private set; } = "Default";

    public IReadOnlyList<string> RegisteredThemeNames => themes.Keys.ToList().AsReadOnly();

    // 事件
    public event Action<ThemeMode>? ThemeChanged;
    public event Action<DensityMode>? DensityChanged;
    public event Action<string>? ThemeNameChanged;

    public AppearanceService(ILogService? log = null)
    {
        this.log = log;
        RegisterBuiltInThemes();
    }

    // 初始化

    public void Initialize(ThemeMode theme = ThemeMode.Light, DensityMode density = DensityMode.Default, string themeName = "Default")
    {
        if (initialized)
        {
            return;
        }

        initialized = true;

        var app = Application.Current
            ?? throw new InvalidOperationException("Application.Current is null");

        log?.Information("Initializing appearance: Theme={ThemeName}/{Mode}, Density={Density}",
            themeName, theme, density);

        // 验证命名主题
        if (!themes.ContainsKey(themeName))
        {
            log?.Warning("Theme '{ThemeName}' not registered, falling back to Default", themeName);
            themeName = "Default";
        }

        // 验证模式
        if (!SupportsMode(themeName, theme))
        {
            var fallback = themes[themeName].Keys.First();
            log?.Warning("Theme '{ThemeName}' does not support {Mode}, using {Fallback}",
                themeName, theme, fallback);
            theme = fallback;
        }

        // 1. 密度令牌
        LoadResource(app, $"Themes/Tokens/Density.{density}.xaml");

        // 2. 颜色令牌
        var colorUri = themes[themeName][theme];
        app.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = colorUri });

        // 3. 排版及动画效果
        LoadResource(app, "Themes/Typography.xaml");
        LoadResource(app, "Themes/Motion.xaml");

        // 4. 控件样式
        var controlFiles = new[]
        {
            "Common", "ScrollBar", "Button", "TextBox",
            "ComboBox", "ListBox", "Card", "Window",
            "TreeView", "TabControl", "ProgressBar",
            "DataGrid", "Panel", "Message", "Icon",
            "NavigationView"
        };
        foreach (var ctrl in controlFiles)
        {
            try
            {
                LoadResource(app, $"Themes/Controls/{ctrl}.xaml");
            }
            catch (Exception ex)
            {
                log?.Debug("Control style '{Ctrl}' not found, skipped: {Msg}", ctrl, ex.Message);
            }
        }

        CurrentThemeName = themeName;
        CurrentTheme = theme;
        CurrentDensity = density;
    }

    // 主题切换

    public bool SetTheme(ThemeMode theme)
    {
        if (CurrentTheme == theme)
        {
            return true;
        }

        if (!SupportsMode(CurrentThemeName, theme))
        {
            log?.Warning("Theme '{ThemeName}' does not support mode {Mode}, switch rejected",
                CurrentThemeName, theme);
            return false;
        }

        var uri = themes[CurrentThemeName][theme];
        SwapResource(ColorTag, uri);

        CurrentTheme = theme;
        log?.Information("Theme mode changed to {Mode}", theme);
        ThemeChanged?.Invoke(theme);
        return true;
    }

    public bool SetNamedTheme(string name, ThemeMode? preferredMode = null)
    {
        if (!themes.TryGetValue(name, out var modemap))
        {
            log?.Warning("Theme '{ThemeName}' not registered", name);
            return false;
        }

        var mode = preferredMode ?? CurrentTheme;
        if (!modemap.ContainsKey(mode))
        {
            mode = modemap.Keys.First();
            log?.Information("Theme '{ThemeName}' doesn't support {Preferred}, falling back to {Fallback}",
                name, preferredMode ?? CurrentTheme, mode);
        }

        SwapResource(ColorTag, modemap[mode]);

        var oldName = CurrentThemeName;
        CurrentThemeName = name;
        CurrentTheme = mode;

        if (oldName != name)
        {
            log?.Information("Named theme changed to '{ThemeName}'", name);
            ThemeNameChanged?.Invoke(name);
        }
        ThemeChanged?.Invoke(mode);
        return true;
    }

    // 密度切换

    public void SetDensity(DensityMode density)
    {
        if (CurrentDensity == density)
        {
            return;
        }

        SwapResource(DensityTag, MakePackUri($"Themes/Tokens/Density.{density}.xaml"));

        CurrentDensity = density;
        log?.Information("Density changed to {Density}", density);
        DensityChanged?.Invoke(density);
    }

    // 命名主题注册

    public void RegisterTheme(string name, Dictionary<ThemeMode, Uri> modeResources)
    {
        themes[name] = new Dictionary<ThemeMode, Uri>(modeResources);
        log?.Information("Theme '{ThemeName}' registered with modes: {Modes}",
            name, string.Join(", ", modeResources.Keys));
    }

    public IReadOnlyList<ThemeMode> GetSupportedModes(string themeName)
    {
        return themes.TryGetValue(themeName, out var map)
            ? map.Keys.ToList().AsReadOnly()
            : Array.Empty<ThemeMode>().ToList().AsReadOnly();
    }

    public bool SupportsMode(string themeName, ThemeMode mode)
    {
        return themes.TryGetValue(themeName, out var map) && map.ContainsKey(mode);
    }

    // 内置主题注册

    private void RegisterBuiltInThemes()
    {
        RegisterTheme("Default", new()
        {
            { ThemeMode.Light, MakePackUri("Themes/Tokens/Colors.Light.xaml") },
            { ThemeMode.Dark, MakePackUri("Themes/Tokens/Colors.Dark.xaml") },
            { ThemeMode.HighContrast, MakePackUri("Themes/Tokens/Colors.HighContrast.xaml") }
        });

        RegisterTheme("Blue", new()
        {
            { ThemeMode.Light, MakePackUri("Themes/NamedThemes/Colors.Blue.Light.xaml") },
            { ThemeMode.Dark, MakePackUri("Themes/NamedThemes/Colors.Blue.Dark.xaml") },
            { ThemeMode.HighContrast, MakePackUri("Themes/NamedThemes/Colors.Blue.HighContrast.xaml") }
        });

        RegisterTheme("Orange", new()
        {
            { ThemeMode.Light, MakePackUri("Themes/NamedThemes/Colors.Orange.Light.xaml") },
            { ThemeMode.Dark, MakePackUri("Themes/NamedThemes/Colors.Orange.Dark.xaml") },
            { ThemeMode.HighContrast, MakePackUri("Themes/NamedThemes/Colors.Orange.HighContrast.xaml") }
        });

        RegisterTheme("Green", new()
        {
            { ThemeMode.Light, MakePackUri("Themes/NamedThemes/Colors.Green.Light.xaml") },
            { ThemeMode.Dark, MakePackUri("Themes/NamedThemes/Colors.Green.Dark.xaml") },
            { ThemeMode.HighContrast, MakePackUri("Themes/NamedThemes/Colors.Green.HighContrast.xaml") }
        });

        RegisterTheme("Purple", new()
        {
            { ThemeMode.Light, MakePackUri("Themes/NamedThemes/Colors.Purple.Light.xaml") },
            { ThemeMode.Dark, MakePackUri("Themes/NamedThemes/Colors.Purple.Dark.xaml") },
            { ThemeMode.HighContrast, MakePackUri("Themes/NamedThemes/Colors.Purple.HighContrast.xaml") }
        });
    }

    // 工具方法

    private static void SwapResource(string tagFragment, Uri newUri)
    {
        var app = Application.Current;
        if (app == null)
        {
            return;
        }

        var toRemove = app.Resources.MergedDictionaries
            .Where(d => d.Source?.OriginalString.Contains(tagFragment) == true)
            .ToList();
        foreach (var rd in toRemove)
        {
            app.Resources.MergedDictionaries.Remove(rd);
        }

        app.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = newUri });
    }

    private static void LoadResource(Application app, string path)
    {
        app.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = MakePackUri(path) });
    }

    private static Uri MakePackUri(string path)
        => new($"pack://application:,,,/{AsmName};component/{path}");
}
