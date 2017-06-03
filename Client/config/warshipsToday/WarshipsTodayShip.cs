using MatchMakingMonitor.Models.ResponseTypes;
using Newtonsoft.Json;

namespace MatchMakingMonitor.config.warshipsToday
{
	public class WarshipsTodayShip
	{
		[JsonProperty(PropertyName = "tier")]
		public int Tier { get; set; }

		[JsonProperty(PropertyName = "type")]
		public ShipType Type { get; set; }
	}
}