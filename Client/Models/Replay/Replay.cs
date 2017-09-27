using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace MatchmakingMonitor.Models.Replay
{
  public class Replay
  {
    private DateTime? _dateTime;

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
    public List<ReplayVehicle> Vehicles { get; set; }

    [JsonProperty("playersPerTeam")]
    public int PlayersPerTeam { get; set; }

    [JsonProperty("dateTime")]
    public string DateTimeString { get; set; }

    public DateTime DateTime
    {
      get
      {
        if (!_dateTime.HasValue)
          try
          {
            _dateTime = DateTime.ParseExact(DateTimeString, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
          }
          catch
          {
            _dateTime = DateTime.Now;
          }
        return _dateTime.Value;
      }
    }

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