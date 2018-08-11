using System;
using MatchmakingMonitor.config;
using Newtonsoft.Json;

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
      if (settingsVersion.CompareTo(new Version("1.1.0.4")) <= 0)
        From1104(settings);
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

    public static void From1104(SettingsJson settings)
    {
      var defaults = JsonConvert.DeserializeObject<SettingsJson>(SettingsWrapper.Defaults());
      settings.AppIds[0].Id = defaults.AppIds[0].Id;
      settings.AppIds[1].Id = defaults.AppIds[1].Id;
      settings.AppIds[2].Id = defaults.AppIds[2].Id;
      settings.AppIds[3].Id = defaults.AppIds[3].Id;
    }
  }
}