using System;
using MatchmakingMonitor.config;
using MatchMakingMonitor.config;

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
		}

		public static void From1000(SettingsJson settings)
		{
			settings.AppIds[3].Region = Region.ASIA;
		}

		public static void From1020(SettingsJson settings)
		{
			settings.LastWindowProperties = new LastWindowProperties();
		}
	}
}