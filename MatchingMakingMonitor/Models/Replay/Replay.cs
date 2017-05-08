using System;
using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace MatchMakingMonitor.Models.Replay
{
	public class Replay
	{
		public string clientVersionFromXml { get; set; }
		public int gameMode { get; set; }
		public string clientVersionFromExe { get; set; }
		public string mapDisplayName { get; set; }
		public int mapId { get; set; }
		public string matchGroup { get; set; }
		public int duration { get; set; }
		public string gameLogic { get; set; }
		public string name { get; set; }
		public string scenario { get; set; }
		public int playerID { get; set; }
		public List<Vehicle> vehicles { get; set; }
		public int playersPerTeam { get; set; }
		public DateTime dateTime { get; set; }
		public string mapName { get; set; }
		public string playerName { get; set; }
		public int scenarioConfigId { get; set; }
		public int teamsCount { get; set; }
		public string logic { get; set; }
		public string playerVehicle { get; set; }
	}
}
