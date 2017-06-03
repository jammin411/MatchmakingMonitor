using Newtonsoft.Json;

namespace MatchMakingMonitor.Models.ResponseTypes
{
	public class WgPlayerShipsStatsResult
	{
		[JsonProperty("data")]
		public WgPlayerShipsStatsData Data { get; set; }
	}
}