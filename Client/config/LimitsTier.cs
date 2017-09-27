using MatchmakingMonitor.Models.ResponseTypes;
using Newtonsoft.Json;

namespace MatchmakingMonitor.config
{
  public class LimitsTier
  {
    [JsonProperty("tier")] public ShipTier ShipTier;

    [JsonProperty("values")] public double[] Values;
  }
}