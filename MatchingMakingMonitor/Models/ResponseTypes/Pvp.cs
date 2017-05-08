// ReSharper disable InconsistentNaming
namespace MatchMakingMonitor.Models.ResponseTypes
{
	public class Pvp
	{
		public int max_xp { get; set; }
		public int damage_to_buildings { get; set; }
		public MainBattery main_battery { get; set; }
		public int suppressions_count { get; set; }
		public int max_damage_scouting { get; set; }
		public int art_agro { get; set; }
		public int ships_spotted { get; set; }
		public SecondBattery second_battery { get; set; }
		public int xp { get; set; }
		public int survived_battles { get; set; }
		public int dropped_capture_points { get; set; }
		public int max_damage_dealt_to_buildings { get; set; }
		public int torpedo_agro { get; set; }
		public int draws { get; set; }
		public int planes_killed { get; set; }
		public int battles { get; set; }
		public int max_ships_spotted { get; set; }
		public int team_capture_points { get; set; }
		public int frags { get; set; }
		public int damage_scouting { get; set; }
		public int max_total_agro { get; set; }
		public int max_frags_battle { get; set; }
		public int capture_points { get; set; }
		public Ramming ramming { get; set; }
		public Torpedoes torpedoes { get; set; }
		public Aircraft aircraft { get; set; }
		public int survived_wins { get; set; }
		public int max_damage_dealt { get; set; }
		public int wins { get; set; }
		public int losses { get; set; }
		public int damage_dealt { get; set; }
		public int max_planes_killed { get; set; }
		public int max_suppressions_count { get; set; }
		public int team_dropped_capture_points { get; set; }
		public int battles_since_512 { get; set; }
	}
}