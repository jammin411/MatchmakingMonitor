using Newtonsoft.Json;

namespace MatchmakingMonitor.config
{
  public class BaseUrl
  {
    [JsonProperty("region")]
    public Region Region { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }
  }
}