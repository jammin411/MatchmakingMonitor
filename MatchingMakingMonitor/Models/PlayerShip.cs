using MatchMakingMonitor.Models.ResponseTypes;

namespace MatchMakingMonitor.Models
{
	public class PlayerShip
	{
		public long ShipId { get; set; }
		public long AccountId { get; set; }
		public string Nickname { get; set; }
		public int Relation { get; set; }
		public string ShipName { get; set; }
		public string ShipType { get; set; }
		public int ShipTier { get; set; }
		public int Frags { get; set; }
		public double Wins { get; set; }
		public double Battles { get; set; }
		public long DamageDealt { get; set; }
		public int XpEarned { get; set; }
		public bool IsPrivateOrHidden { get; set; }

		#region unused
		//public int MaxFrags { get; set; }
		//public int DamageToBuildings { get; set; }
		//public int SuppressionCount { get; set; }
		//public int MaxDamageScouting { get; set; }
		//public int ArtAgro { get; set; }
		//public int ShipSpotted { get; set; }
		//public int MaxDamageToBuildings { get; set; }
		//public int TorpedoAgro { get; set; }
		//public int MaxShipsSpotted { get; set; }
		//public int TeamCapturePoints { get; set; }
		//public int DamageScouting { get; set; }
		//public int MaxTotalAgro { get; set; }
		//public int MaxSuppressionCount { get; set; }
		//public int TeamDroppedCapPoints { get; set; }
		//public int BattlesSince512 { get; set; }

		//public int CapturePoints { get; set; }
		//public int Draws { get; set; }

		//public int Losses { get; set; }
		//public int MaxXp { get; set; }
		//public int PlanesKilled { get; set; }
		//public int MaxPlanesKilled { get; set; }
		//public int TorpMaxFrags { get; set; }
		//public int TorpFrags { get; set; }
		//public int TorpShots { get; set; }
		//public int TorpHits { get; set; }

		//public int MaxDamage { get; set; }

		//public int AircraftMaxFrags { get; set; }
		//public int AircraftFrags { get; set; }
		//public int RamMaxFrags { get; set; }
		//public int RamFrags { get; set; }
		//public int MainBatteryMaxFrags { get; set; }
		//public int MainBatteryFrags { get; set; }
		//public int MainBatteryHits { get; set; }
		//public int MainBatteryShots { get; set; }
		//public int SecondaryMaxFrags { get; set; }
		//public int SecondaryFrags { get; set; }
		//public long SecondaryHits { get; set; }
		//public long SecondaryShots { get; set; }
		//public int SurvivedWins { get; set; }

		//public int SurvivedBattles { get; set; }
		//public int DroppedCapPoints { get; set; }
		//public string LastUpdatedWG { get; set; }
		//public int TotalBattles { get; set; }

		//public int ShipRating { get; set; }
		//public int Passiveness { get; set; }
		#endregion

		public PlayerShip(Ship ship, WargamingPlayer player, ShipInfo shipInfo, int relationship) : this(shipInfo)
		{
			if (ship == null || player == null) return;
			ShipId = ship.ShipId;
			AccountId = ship.AccountId;
			Nickname = player.Nickname;
			Relation = relationship;
			Frags = ship.Pvp.Frags;
			Wins = ship.Pvp.Wins;
			Battles = ship.Pvp.Battles;
			DamageDealt = ship.Pvp.DamageDealt;
			XpEarned = ship.Pvp.Xp;
			IsPrivateOrHidden = ship.Private != null;
		}

		public PlayerShip(ShipInfo shipInfo)
		{
			if (shipInfo == null) return;
			ShipName = shipInfo.ShipName;
			ShipType = shipInfo.ShipType;
			ShipTier = shipInfo.Tier;
		}

		public PlayerShip()
		{

		}
	} //end class
} //end namespace
