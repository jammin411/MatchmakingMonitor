using Newtonsoft.Json;

namespace MatchmakingMonitor.config.wowsNumbers
{
  public class WowsNumbersShipEntry
  {
    [JsonProperty(PropertyName = "average_damage_dealt")]
    public double AvgDamageDealt { get; set; }

    [JsonProperty(PropertyName = "average_xp")]
    public double AvgXp { get; set; }

    [JsonProperty(PropertyName = "average_frags")]
    public double AvgFrags { get; set; }

    [JsonProperty(PropertyName = "win_rate")]
    public double WinRate { get; set; }

    [JsonProperty(PropertyName = "tier")]
    public int Tier { get; set; }

    [JsonProperty(PropertyName = "ship_type")]
    public WowsNumbersShipType Type { get; set; }
  }
}