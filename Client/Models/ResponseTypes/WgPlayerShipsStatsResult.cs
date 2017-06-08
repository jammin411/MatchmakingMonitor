using Newtonsoft.Json;

namespace MatchMakingMonitor.Models.ResponseTypes
{
	public class WgPlayerShipsStatsResult
	{
		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("data")]
		public WgPlayerShipsStatsData Data { get; set; }
	}
}