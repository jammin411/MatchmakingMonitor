using Newtonsoft.Json;

namespace MatchMakingMonitor.Models.ResponseTypes
{
	public class WargamingPlayer
	{
		[JsonProperty("nickname")]
		public string Nickname { get; set; }
		[JsonProperty("account_id")]
		public long AccountId { get; set; }
		[JsonProperty("Region")]
		public string Region { get; set; }
	}
}