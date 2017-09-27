using System;
using MatchmakingMonitor.config;

namespace MatchmakingMonitor.Services
{
  public static class UpgradeSettings
  {
    public static readonly Version InitialVersion = new Version("1.0.0.0");

    public static void Upgrade(SettingsJson settings, Version settingsVersion)
    {
      if (settingsVersion.CompareTo(new Version("1.0.0.0")) <= 0)
        From1000(settings);
      if (settingsVersion.CompareTo(new Version("1.0.2.0")) <= 0)
        From1020(settings);
      if (settingsVersion.CompareTo(new Version("1.0.3.0")) <= 0)
        From1030(settings);
      if (settingsVersion.CompareTo(new Version("1.0.4.0")) <= 0)
        From1040(settings);
    }

    public static void From1000(SettingsJson settings)
    {
      settings.AppIds[3].Region = Region.ASIA;
      settings.BaseUrls[3].Region = Region.ASIA;
    }

    public static void From1020(SettingsJson settings)
    {
      settings.LastWindowProperties = new LastWindowProperties();
    }

    public static void From1030(SettingsJson settings)
    {
      settings.BaseUrls[3].Region = Region.ASIA;
    }

    public static void From1040(SettingsJson settings)
    {
      settings.AutomaticLimitsSync = true;
      settings.AutomaticAppUpdate = true;
    }
  }
}