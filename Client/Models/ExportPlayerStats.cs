using Newtonsoft.Json;

namespace MatchmakingMonitor.Models
{
  public class ExportPlayerStats
  {
    [JsonProperty("player_name")]
    public string PlayerName { get; set; }

    [JsonProperty("ship_name")]
    public string ShipName { get; set; }

    [JsonProperty("accountId")]
    public string AccountId { get; set; }

    [JsonProperty("winRate")]
    public double WinRate { get; set; }

    [JsonProperty("avgXp")]
    public double AvgXp { get; set; }

    [JsonProperty("avgFrags")]
    public double AvgFrags { get; set; }

    [JsonProperty("avgDamage")]
    public double AvgDamage { get; set; }

    [JsonProperty("battles")]
    public double Battles { get; set; }

    [JsonProperty("wins")]
    public double Wins { get; set; }

    [JsonProperty("team")]
    public string Team { get; set; }

    [JsonProperty("isPrivateOrHidden")]
    public bool IsPrivateOrHidden { get; set; }

    public static ExportPlayerStats FromPlayerStats(DisplayPlayerStats playerStats)
    {
      return new ExportPlayerStats
      {
        IsPrivateOrHidden = playerStats.Player.IsPrivateOrHidden,
        PlayerName = playerStats.Player.Nickname,
        ShipName = playerStats.ShipName,
        AccountId = playerStats.AccountId,
        WinRate = playerStats.WinRate,
        AvgXp = playerStats.AvgXp,
        AvgFrags = playerStats.AvgFrags,
        AvgDamage = playerStats.AvgDamage,
        Battles = playerStats.Player.Battles,
        Wins = playerStats.Player.Wins,
        Team = playerStats.Player.Relation == 2 ? "enemy" : "friendly"
      };
    }
  }
}