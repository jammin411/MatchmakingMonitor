using MatchmakingMonitor.config.Reflection;
using Newtonsoft.Json;

namespace MatchmakingMonitor.config
{
  public class SettingsJson
  {
    [JsonProperty("appIds")] public AppId[] AppIds;

    [JsonProperty("automaticAppUpdate")] public bool AutomaticAppUpdate;

    [JsonProperty("automaticLimitsSync")] public bool AutomaticLimitsSync;

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

    [JsonProperty("hideLowBattles")] [ExportSetting] public bool HideLowBattles;

    [JsonProperty("installDirectory")] public string InstallDirectory;

    [JsonProperty("lastWindowProperties")] public LastWindowProperties LastWindowProperties;

    [JsonProperty("region")] public Region Region;

    [JsonProperty("token")] public string Token;

    [JsonProperty("version")] public string Version;

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