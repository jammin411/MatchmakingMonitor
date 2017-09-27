using Newtonsoft.Json;

namespace MatchmakingMonitor.config.warshipsToday
{
  public class WarshipsTodayShipStats
  {
    [JsonProperty(PropertyName = "damage_dealt")]
    public double DamageDealt { get; set; }

    [JsonProperty(PropertyName = "xp")]
    public double Xp { get; set; }

    [JsonProperty(PropertyName = "frags")]
    public double Frags { get; set; }

    [JsonProperty(PropertyName = "battles")]
    public double Battles { get; set; }

    [JsonProperty(PropertyName = "wins")]
    public double Wins { get; set; }
  }
}