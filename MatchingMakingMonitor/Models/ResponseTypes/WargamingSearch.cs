using System.Collections.Generic;
using Newtonsoft.Json;

namespace MatchMakingMonitor.Models.ResponseTypes
{
	public class WargamingSearch
	{
		[JsonProperty("status")]
		public string Status { get; set; }
		public Dictionary<string, int> Meta { get; set; }
		[JsonProperty("data")]
		public List<WargamingPlayer> Data { get; set; }
	}
}
