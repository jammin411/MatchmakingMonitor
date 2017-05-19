using Newtonsoft.Json;

namespace MatchMakingMonitor.Models.ResponseTypes
{
	public class WgStatsTorpedoes
	{
		[JsonProperty("max_frags_battle")]
		public int MaxFragsBattle { get; set; }

		[JsonProperty("frags")]
		public int Frags { get; set; }

		[JsonProperty("hits")]
		public int Hits { get; set; }

		[JsonProperty("shots")]
		public int Shots { get; set; }
	}
}