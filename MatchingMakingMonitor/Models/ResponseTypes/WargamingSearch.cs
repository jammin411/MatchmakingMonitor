// ReSharper disable InconsistentNaming
using System.Collections.Generic;

namespace MatchMakingMonitor.Models.ResponseTypes
{
	public class WargamingSearch
	{
		public string status { get; set; }
		public Dictionary<string, int> meta { get; set; }
		public List<WargamingPlayer> data { get; set; }
	}
}
