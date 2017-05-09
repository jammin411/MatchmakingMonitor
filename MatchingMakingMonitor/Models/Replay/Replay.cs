using System;
using System.Collections.Generic;
using Newtonsoft.Json;



namespace MatchMakingMonitor.Models.Replay
{
	public class Replay
	{
		[JsonProperty("clientVersionFromXml")]
		public string ClientVersionFromXml { get; set; }
		[JsonProperty("gameMode")]
		public int GameMode { get; set; }
		[JsonProperty("clientVersionFromExe")]
		public string ClientVersionFromExe { get; set; }
		[JsonProperty("mapDisplayName")]
		public string MapDisplayName { get; set; }
		[JsonProperty("mapId")]
		public int MapId { get; set; }
		[JsonProperty("matchGroup")]
		public string MatchGroup { get; set; }
		[JsonProperty("duration")]
		public int Duration { get; set; }
		[JsonProperty("gameLogic")]
		public string GameLogic { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("scenario")]
		public string Scenario { get; set; }
		[JsonProperty("playerID")]
		public int PlayerId { get; set; }
		[JsonProperty("vehicles")]
		public List<Vehicle> Vehicles { get; set; }
		[JsonProperty("playersPerTeam")]
		public int PlayersPerTeam { get; set; }
		[JsonProperty("dateTime")]
		public DateTime DateTime { get; set; }
		[JsonProperty("mapName")]
		public string MapName { get; set; }
		[JsonProperty("playerName")]
		public string PlayerName { get; set; }
		[JsonProperty("scenarioConfigId")]
		public int ScenarioConfigId { get; set; }
		[JsonProperty("teamsCount")]
		public int TeamsCount { get; set; }
		[JsonProperty("logic")]
		public string Logic { get; set; }
		[JsonProperty("playerVehicle")]
		public string PlayerVehicle { get; set; }
	}
}
