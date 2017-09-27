using Newtonsoft.Json;

namespace MatchmakingMonitor.config.warshipsToday
{
  public class WarshipsTodayVehicle
  {
    [JsonProperty(PropertyName = "ship")]
    public WarshipsTodayShip Ship { get; set; }
  }
}