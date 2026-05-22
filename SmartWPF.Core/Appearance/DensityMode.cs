using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWPF.Core.Appearance;

/// <summary>
/// 视觉密度模式
/// </summary>
public enum DensityMode
{
    Compact,        // 紧凑: 小字体、窄间距, 适合数据密集型/工具型应用
    Default,        // 默认: 标准桌面应用体验
    Presentation    // 演示: 大字体、宽间距, 适合演示/触屏/无障碍
}