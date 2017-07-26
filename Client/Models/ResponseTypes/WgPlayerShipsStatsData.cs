using System.Collections.Generic;
using Newtonsoft.Json;

namespace MatchMakingMonitor.Models.ResponseTypes
{
	public class WgPlayerShipsStatsData
	{
		[JsonProperty("ships")]
		public List<WgStatsShip> Ships { get; set; }

		[JsonProperty("season_wrapper")]
		public List<WgPlayerShipsStatsSeason> SeasonsWrapper { get; set; }
	}

	public class WgPlayerShipsStatsSeason
	{
		[JsonProperty("seasons")]
		public WgPlayerShipsStatsSeasons Seasons { get; set; }
	}

	public class WgPlayerShipsStatsSeasons
	{
		[JsonProperty("season_data")]
		public WgPlayerShipsStatsSeasonData SeasonData { get; set; }
	}

	public class WgPlayerShipsStatsSeasonData
	{
		[JsonProperty("rank_solo")]
		public WgStatsRankedShip Ship { get; set; }
	}

	public class WgStatsRankedShip
	{
		[JsonProperty("max_xp")]
		public int MaxXp { get; set; }

		[JsonProperty("main_battery")]
		public WgStatsMainBattery MainBattery { get; set; }

		[JsonProperty("second_battery")]
		public WgStatsSecondBattery SecondBattery { get; set; }

		[JsonProperty("xp")]
		public int Xp { get; set; }

		[JsonProperty("draws")]
		public int Draws { get; set; }

		[JsonProperty("planes_killed")]
		public int PlanesKilled { get; set; }

		[JsonProperty("battles")]
		public int Battles { get; set; }

		[JsonProperty("max_ships_spotted")]
		public int MaxShipsSpotted { get; set; }

		[JsonProperty("frags")]
		public int Frags { get; set; }

		[JsonProperty("max_frags_battle")]
		public int MaxFragsBattle { get; set; }

		[JsonProperty("ramming")]
		public WgStatsRamming Ramming { get; set; }

		[JsonProperty("torpedoes")]
		public WgStatsTorpedoes Torpedoes { get; set; }

		[JsonProperty("aircraft")]
		public WgStatsAircraft Aircraft { get; set; }

		[JsonProperty("survived_wins")]
		public int SurvivedWins { get; set; }

		[JsonProperty("max_damage_dealt")]
		public int MaxDamageDealt { get; set; }

		[JsonProperty("wins")]
		public int Wins { get; set; }

		[JsonProperty("losses")]
		public int Losses { get; set; }

		[JsonProperty("damage_dealt")]
		public int DamageDealt { get; set; }

		[JsonProperty("max_planes_killed")]
		public int MaxPlanesKilled { get; set; }
	}
}