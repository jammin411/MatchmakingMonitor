using MatchMakingMonitor.Services;

namespace MatchMakingMonitor.Models
{
	public class MobileSettings
	{
		public static MobileSettings FromSettings(SettingsWrapper settingsWrapper)
		{
			return new MobileSettings();
		}
	}
}