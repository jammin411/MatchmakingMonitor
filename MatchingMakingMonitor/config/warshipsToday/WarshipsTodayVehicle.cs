using Newtonsoft.Json;

namespace MatchMakingMonitor.config.warshipsToday
{
	public class WarshipsTodayVehicle
	{
		[JsonProperty(PropertyName = "ship")]
		public WarshipsTodayShip Ship { get; set; }
	}
}