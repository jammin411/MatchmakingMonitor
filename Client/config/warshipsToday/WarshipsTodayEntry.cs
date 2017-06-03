using Newtonsoft.Json;

namespace MatchMakingMonitor.config.warshipsToday
{
	public class WarshipsTodayEntry
	{
		[JsonProperty(PropertyName = "vehicle")]
		public WarshipsTodayVehicle Vehicle { get; set; }

		[JsonProperty(PropertyName = "statistics")]
		public WarshipsTodayStats Statistics { get; set; }
	}
}