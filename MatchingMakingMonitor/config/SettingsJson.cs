using MatchMakingMonitor.config.Reflection;
using Newtonsoft.Json;

namespace MatchMakingMonitor.config
{
	public class SettingsJson
	{
		[JsonProperty("appIds")] public AppId[] AppIds;

		[JsonProperty("avgDmgLimits")] [ExportSetting] public LimitsType AvgDmgLimits;

		[JsonProperty("avgDmgWeight")] [ExportSetting] public double AvgDmgWeight;

		[JsonProperty("avgFragsLimits")] [ExportSetting] public double[] AvgFragsLimits;

		[JsonProperty("avgFragsWeight")] [ExportSetting] public double AvgFragsWeight;

		[JsonProperty("avgXpLimits")] [ExportSetting] public LimitsTier[] AvgXpLimits;

		[JsonProperty("avgXpWeight")] [ExportSetting] public double AvgXpWeight;

		[JsonProperty("baseUrls")] public BaseUrl[] BaseUrls;

		[JsonProperty("battleLimits")] [ExportSetting] public double[] BattleLimits;

		[JsonProperty("battleWeight")] [ExportSetting] public double BattleWeight;

		[JsonProperty("colors")] [ExportSetting] public string[] Colors;

		[JsonProperty("fontSize")] [ExportSetting] public int FontSize;

		[JsonProperty("installDirectory")] public string InstallDirectory;

		[JsonProperty("region")] public Region Region;

		[JsonProperty("token")] public string Token;

		[JsonProperty("winRateLimits")] [ExportSetting] public double[] WinRateLimits;

		[JsonProperty("winRateWeight")] [ExportSetting] public double WinRateWeight;

		public static SettingsJson Empty()
		{
			return new SettingsJson
			{
				AvgDmgLimits = new LimitsType()
			};
		}
	}
}