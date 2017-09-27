using System.Collections.Generic;
using Newtonsoft.Json;

namespace MatchmakingMonitor.Models.ResponseTypes
{
  public class WgShipResponse
  {
    [JsonProperty("data")]
    public List<WgShip> Data { get; set; }
  }
}