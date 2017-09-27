using Newtonsoft.Json;

namespace MatchmakingMonitor.Models.ResponseTypes
{
  public class WgStatsPlayer
  {
    [JsonProperty("nickname")]
    public string Nickname { get; set; }

    [JsonProperty("account_id")]
    public long AccountId { get; set; }

    [JsonProperty("Region")]
    public string Region { get; set; }
  }
}