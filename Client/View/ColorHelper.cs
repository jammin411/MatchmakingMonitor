using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using MatchmakingMonitor.Services;

namespace MatchmakingMonitor.View
{
  public static class ColorHelper
  {
    public static SolidColorBrush GetColor(SettingsWrapper settingsWrapper, double value, IEnumerable<double> limits,
      double oldTotal, double multiplier,
      out double total, out int key)
    {
      var limitsList = limits.ToArray();
      for (var i = 0; i < limitsList.Length; i++)
      {
        if (!(value >= limitsList[i])) continue;
        key = i + 1;
        total = oldTotal + key * multiplier;
        return settingsWrapper.Brushes[i];
      }
      total = oldTotal + 9 * multiplier;
      key = 9;
      return Brushes.Black;
    }
  }
}