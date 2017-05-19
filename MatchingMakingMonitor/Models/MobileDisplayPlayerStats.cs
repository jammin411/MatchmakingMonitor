namespace MatchMakingMonitor.Models
{
	public class MobileDisplayPlayerStats
	{
		public int Relation { get; set; }
		public bool PrivateOrHidden { get; set; }
		public string DisplayName { get; set; }
		public string Name { get; set; }
		public string AccountId { get; set; }
		public string ShipName { get; set; }
		public double Battles { get; set; }
		public double Wins { get; set; }
		public double WinRate { get; set; }
		public double AvgXp { get; set; }
		public double AvgFrags { get; set; }
		public double AvgDamage { get; set; }

		public int ColorNameKey { get; set; }
		public int ColorWinRateKey { get; set; }
		public int ColorAvgFragsKey { get; set; }
		public int ColorAvgXpKey { get; set; }
		public int ColorAvgDamageKey { get; set; }
		public int ColorBattlesKey { get; set; }
	}
}