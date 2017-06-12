using Newtonsoft.Json;

namespace MatchMakingMonitor.Models
{
	public class MobileColorKeys
	{
		[JsonProperty("accountId")]
		public string AccountId { get; set; }

		[JsonProperty("colorNameKey")]
		public int ColorNameKey { get; set; }

		[JsonProperty("colorWinRateKey")]
		public int ColorWinRateKey { get; set; }

		[JsonProperty("colorAvgFragsKey")]
		public int ColorAvgFragsKey { get; set; }

		[JsonProperty("colorAvgXpKey")]
		public int ColorAvgXpKey { get; set; }

		[JsonProperty("colorAvgDamageKey")]
		public int ColorAvgDamageKey { get; set; }

		[JsonProperty("colorBattlesKey")]
		public int ColorBattlesKey { get; set; }
	}
}