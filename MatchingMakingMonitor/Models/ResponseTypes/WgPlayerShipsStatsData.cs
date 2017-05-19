using System.Collections.Generic;
using Newtonsoft.Json;

namespace MatchMakingMonitor.Models.ResponseTypes
{
	public class WgPlayerShipsStatsData
	{
		[JsonProperty("ships")]
		public List<WgStatsShip> Ships { get; set; }
	}
}