using Newtonsoft.Json;

namespace MatchmakingMonitor.Models.ResponseTypes
{
  public class WgStatsAircraft
  {
    [JsonProperty("max_frags_battle")]
    public int MaxFragsBattle { get; set; }

    [JsonProperty("frags")]
    public int Frags { get; set; }
  }
}