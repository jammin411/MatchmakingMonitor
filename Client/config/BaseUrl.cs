using Newtonsoft.Json;

namespace MatchmakingMonitor.config
{
  public class AppId
  {
    [JsonProperty("region")]
    public Region Region { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }
  }
}