using Newtonsoft.Json;

namespace MatchmakingMonitor.Models
{
  public class MobilePlayerStats
  {
    [JsonProperty("relation")]
    public int Relation { get; set; }

    [JsonProperty("privateOrHidden")]
    public bool PrivateOrHidden { get; set; }

    [JsonProperty("isLowBattles")]
    public bool IsLowBattles { get; set; }

    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("accountId")]
    public string AccountId { get; set; }

    [JsonProperty("shipName")]
    public string ShipName { get; set; }

    [JsonProperty("shipTier")]
    public string ShipTier { get; set; }

    [JsonProperty("battles")]
    public double Battles { get; set; }

    [JsonProperty("wins")]
    public double Wins { get; set; }

    [JsonProperty("winRate")]
    public double WinRate { get; set; }

    [JsonProperty("avgXp")]
    public double AvgXp { get; set; }

    [JsonProperty("avgFrags")]
    public double AvgFrags { get; set; }

    [JsonProperty("avgDamage")]
    public double AvgDamage { get; set; }
  }
}