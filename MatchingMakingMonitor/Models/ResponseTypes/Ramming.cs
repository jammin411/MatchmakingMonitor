using Newtonsoft.Json;

namespace MatchMakingMonitor.Models.ResponseTypes
{
	public class Ramming
	{
		[JsonProperty("max_frags_battle")]
		public int MaxFragsBattle { get; set; }
		[JsonProperty("frags")]
		public int Frags { get; set; }
	}
}