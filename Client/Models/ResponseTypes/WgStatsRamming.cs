using Newtonsoft.Json;

namespace MatchMakingMonitor.Models.ResponseTypes
{
	public class WgStatsRamming
	{
		[JsonProperty("max_frags_battle")]
		public int MaxFragsBattle { get; set; }

		[JsonProperty("frags")]
		public int Frags { get; set; }
	}
}