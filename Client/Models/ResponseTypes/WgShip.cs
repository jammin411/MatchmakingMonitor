using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MatchMakingMonitor.Models.ResponseTypes
{
	public class WgShip
	{
		[JsonProperty("ship_id")]
		public long ShipId { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("type")]
		[JsonConverter(typeof(StringEnumConverter))]
		public ShipType Type { get; set; }

		[JsonProperty("tier")]
		public ShipTier Tier { get; set; }
	}
}