using MatchMakingMonitor.config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchmakingMonitor.Services
{
  public static class UpgradeSettings
  {
    public static readonly Version InitialVersion = new Version("1.0.0.0");
    public static void Upgrade(SettingsJson settings, Version settingsVersion)
    {
      if (settingsVersion.CompareTo(new Version("1.0.0.0")) <= 0)
        From1000(settings);
    }
    public static void From1000(SettingsJson settings)
    {
      settings.AppIds[3].Region = Region.ASIA;
    }
  }
}
