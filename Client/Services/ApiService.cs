using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MatchmakingMonitor.Models;
using MatchmakingMonitor.Models.Replay;
using MatchmakingMonitor.Models.ResponseTypes;
using Newtonsoft.Json;

namespace MatchmakingMonitor.Services
{
  public class ApiService
  {
    private static readonly Regex DateStart = new Regex("\"data\":{");
    private static readonly Regex ShipIdRegex = new Regex("\"(\\d){10}\":");
    private static readonly Regex DataEnd = new Regex("}}$");
    private readonly ILogger _logger;
    private readonly SettingsWrapper _settingsWrapper;

    private HttpClient _httpClient;

    private List<WgShip> _wgShips;

    public ApiService(ILogger logger, SettingsWrapper settingsWrapper)
    {
      _settingsWrapper = settingsWrapper;
      _logger = logger;
#pragma warning disable 4014
      Ships();
#pragma warning restore 4014
    }

    public IReadOnlyList<WgShip> WgShips => _wgShips.AsReadOnly();

    private string ApplicationId => _settingsWrapper.AppId;

    private async void Ships()
    {
      try
      {
        var baseUrl = _settingsWrapper.BaseUrl;
        var client = new HttpClient {BaseAddress = new Uri(baseUrl)};
        var i = 1;
        List<WgShip> tempList;
        var result = new List<WgShip>();
        do
        {
          tempList = await FetchShips(client, i);
          result = result.Concat(tempList).ToList();
          i++;
        } while (tempList.Count > 0);
        _wgShips = result;
      }
      catch (Exception e)
      {
        _logger.Error("Exception occurred while retrieving ships", e);
      }
    }

    private async Task<List<WgShip>> FetchShips(HttpClient client, int page = 1)
    {
      var result =
        await client.PostAsync(
          $"/wows/encyclopedia/ships/?application_id={ApplicationId}&language=en&page_no={page}&fields=name%2C+tier%2C+type%2C+ship_id",
          null);
      if (result.StatusCode != HttpStatusCode.OK) return new List<WgShip>(0);
      var shipsJson = await result.Content.ReadAsStringAsync();
      if (shipsJson.Contains("error")) return new List<WgShip>(0);
      shipsJson = DateStart.Replace(shipsJson, "\"data\":[");
      shipsJson = ShipIdRegex.Replace(shipsJson, string.Empty);
      shipsJson = DataEnd.Replace(shipsJson, "]}");
      return (await Task.Run(() => JsonConvert.DeserializeObject<WgShipResponse>(shipsJson)))?.Data;
    }

    public async Task<IEnumerable<PlayerShip>> Players(Replay replay)
    {
      try
      {
        while (_wgShips == null)
          await Task.Delay(1000);
        var baseUrl = _settingsWrapper.BaseUrl;
        _httpClient = new HttpClient {BaseAddress = new Uri(baseUrl)};

        var tasks = replay.Vehicles.Select(replayVehicle => GetAsync(replayVehicle,
          replay.MatchGroup.Equals("ranked", StringComparison.InvariantCultureIgnoreCase))).ToList();
        var list = await Task.WhenAll(tasks);
        return list.Where(p => p != null);
      }
      catch (Exception e)
      {
        _logger.Error("Error occured while fetching players", e);
        return new List<PlayerShip>();
      }
    }

    [SuppressMessage("ReSharper", "InvertIf")]
    private async Task<PlayerShip> GetAsync(ReplayVehicle replayVehicle, bool ranked)
    {
      var wgShip = WgShips.SingleOrDefault(s => s.ShipId == replayVehicle.ShipId);
      try
      {
        var playerResponse =
          await _httpClient.GetAsync($"wows/account/list/?application_id={ApplicationId}&search={replayVehicle.Name}");
        if (playerResponse.StatusCode == HttpStatusCode.OK)
        {
          var playerJson = await playerResponse.Content.ReadAsStringAsync();
          var players = await Task.Run(() => JsonConvert.DeserializeObject<WgPlayerSearchResult>(playerJson));
          if (players.Status != "error")
          {
            var player = players.Data.SingleOrDefault(p => p.Nickname == replayVehicle.Name);
            if (player != null)
            {
              var statsUrl = ranked
                ? $"wows/seasons/shipstats/?application_id={ApplicationId}&account_id={player.AccountId}&ship_id={replayVehicle.ShipId}&season_id=7"
                : $"wows/ships/stats/?application_id={ApplicationId}&account_id={player.AccountId}&ship_id={replayVehicle.ShipId}";
              var shipStatsResponse = await _httpClient.GetAsync(statsUrl);
              if (shipStatsResponse.StatusCode == HttpStatusCode.OK)
              {
                var shipStatsJson = await shipStatsResponse.Content.ReadAsStringAsync();
                shipStatsJson = ranked
                  ? shipStatsJson.Replace("\"" + player.AccountId + "\":", "\"season_wrapper\":")
                    .Replace("\"7\":", "\"season_data\":")
                  : shipStatsJson.Replace("\"" + player.AccountId + "\":", "\"Ships\":");
                var shipStats = await Task.Run(() =>
                  JsonConvert.DeserializeObject<WgPlayerShipsStatsResult>(shipStatsJson));
                if (shipStats?.Status != "error")
                {
                  if (ranked && shipStats?.Data?.SeasonsWrapper != null && shipStats.Data.SeasonsWrapper.Any())
                  {
                    var rankedShip = shipStats.Data.SeasonsWrapper.First().Seasons.SeasonData.Ship;
                    var ship = WgStatsShip.FromRanked(rankedShip, player.AccountId, replayVehicle.ShipId);
                    return new PlayerShip(ship, player, wgShip, replayVehicle.Relation);
                  }
                  if (shipStats?.Data?.Ships != null && shipStats.Data.Ships.Any())
                  {
                    var ship = shipStats.Data.Ships.First();
                    return new PlayerShip(ship, player, wgShip, replayVehicle.Relation);
                  }
                }
                else
                {
                  _logger.Error("Error occured while fetching player " + replayVehicle.Name, null);
                }
              }
            }
          }
          else
          {
            _logger.Error("Error occured while fetching player " + replayVehicle.Name, null);
          }
        }
      }
      catch (Exception e)
      {
        _logger.Error("Error occured while fetching player " + replayVehicle.Name, e);
      }

      return new PlayerShip(wgShip)
      {
        Nickname = replayVehicle.Name,
        Relation = replayVehicle.Relation,
        IsPrivateOrHidden = true
      };
    }
  }
}