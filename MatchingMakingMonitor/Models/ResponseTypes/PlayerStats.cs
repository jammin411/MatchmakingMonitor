using Newtonsoft.Json;

namespace MatchMakingMonitor.Models.ResponseTypes
{
	public class PlayerStats
	{
		[JsonProperty("status")]
		public string Status { get; set; }
		[JsonProperty("meta")]
		public Meta Meta { get; set; }
		[JsonProperty("data")]
		public Data Data { get; set; }
	}
}
