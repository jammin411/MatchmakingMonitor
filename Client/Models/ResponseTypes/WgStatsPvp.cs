using Newtonsoft.Json;

namespace MatchmakingMonitor.Models.ResponseTypes
{
  public class WgStatsPvp
  {
    [JsonProperty("max_xp")]
    public int MaxXp { get; set; }

    [JsonProperty("damage_to_buildings")]
    public int DamageToBuildings { get; set; }

    [JsonProperty("main_battery")]
    public WgStatsMainBattery MainBattery { get; set; }

    [JsonProperty("suppressions_count")]
    public int SuppressionsCount { get; set; }

    [JsonProperty("max_damage_scouting")]
    public int MaxDamageScouting { get; set; }

    [JsonProperty("art_agro")]
    public int ArtAgro { get; set; }

    [JsonProperty("ships_spotted")]
    public int ShipsSpotted { get; set; }

    [JsonProperty("second_battery")]
    public WgStatsSecondBattery SecondBattery { get; set; }

    [JsonProperty("xp")]
    public int Xp { get; set; }

    [JsonProperty("survived_battles")]
    public int SurvivedBattles { get; set; }

    [JsonProperty("dropped_capture_points")]
    public int DroppedCapturePoints { get; set; }

    [JsonProperty("max_damage_dealt_to_buildings")]
    public int MaxDamageDealtToBuildings { get; set; }

    [JsonProperty("torpedo_agro")]
    public int TorpedoAgro { get; set; }

    [JsonProperty("draws")]
    public int Draws { get; set; }

    [JsonProperty("planes_killed")]
    public int PlanesKilled { get; set; }

    [JsonProperty("battles")]
    public int Battles { get; set; }

    [JsonProperty("max_ships_spotted")]
    public int MaxShipsSpotted { get; set; }

    [JsonProperty("team_capture_points")]
    public int TeamCapturePoints { get; set; }

    [JsonProperty("frags")]
    public int Frags { get; set; }

    [JsonProperty("damage_scouting")]
    public int DamageScouting { get; set; }

    [JsonProperty("max_total_agro")]
    public int MaxTotalAgro { get; set; }

    [JsonProperty("max_frags_battle")]
    public int MaxFragsBattle { get; set; }

    [JsonProperty("capture_points")]
    public int CapturePoints { get; set; }

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

    [JsonProperty("max_suppressions_count")]
    public int MaxSuppressionsCount { get; set; }

    [JsonProperty("team_dropped_capture_points")]
    public int TeamDroppedCapturePoints { get; set; }

    [JsonProperty("battles_since_512")]
    public int BattlesSince512 { get; set; }

    public static WgStatsPvp FromRanked(WgStatsRankedShip rankedShip)
    {
      return new WgStatsPvp
      {
        Battles = rankedShip.Battles,
        Wins = rankedShip.Wins,
        DamageDealt = rankedShip.DamageDealt,
        Frags = rankedShip.Frags,
        Xp = rankedShip.Xp
      };
    }
  }
}