using Newtonsoft.Json;

namespace MatchMakingMonitor.Models.Replay
{
	public class ReplayVehicle
	{
		[JsonProperty("id")]
		public long Id { get; set; }

		[JsonProperty("relation")]
		public int Relation { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("shipId")]
		public long ShipId { get; set; }
	}
}