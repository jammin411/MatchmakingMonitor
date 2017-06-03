using MatchMakingMonitor.Models.ResponseTypes;
using Newtonsoft.Json;

namespace MatchMakingMonitor.config
{
	public class LimitsTier
	{
		[JsonProperty("tier")] public ShipTier ShipTier;

		[JsonProperty("values")] public double[] Values;
	}
}