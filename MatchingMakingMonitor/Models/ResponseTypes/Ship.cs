using Newtonsoft.Json;

// ReSharper disable InconsistentNaming
namespace MatchMakingMonitor.Models.ResponseTypes
{
	public class Ship
	{
		public int distance { get; set; }
		public int last_battle_time { get; set; }
		public long account_id { get; set; }
		public Pvp pvp { get; set; }
		public int updated_at { get; set; }
		public int battles { get; set; }
		public long ship_id { get; set; }
		[JsonProperty(PropertyName = "private")]
		public string @private { get; set; }
	}
}