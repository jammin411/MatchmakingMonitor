using System.Collections.Generic;
using Newtonsoft.Json;

namespace MatchMakingMonitor.Models.ResponseTypes
{
	public class WgPlayerSearchResult
	{
		[JsonProperty("status")]
		public string Status { get; set; }

		public Dictionary<string, int> Meta { get; set; }

		[JsonProperty("data")]
		public List<WgStatsPlayer> Data { get; set; }
	}
}