using Newtonsoft.Json;

namespace MatchMakingMonitor.Models.ResponseTypes
{
	public class Meta
	{
		[JsonProperty("count")]
		public int Count { get; set; }
		[JsonProperty("hidden")]
		public object Hidden { get; set; }
	}
}