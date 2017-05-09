using MatchMakingMonitor.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MatchMakingMonitor.Models.Replay;
using MatchMakingMonitor.Models.ResponseTypes;

namespace MatchMakingMonitor.Services
{
	public class ApiService
	{
		private readonly Settings _settings;
		private readonly ILogger _logger;

		private List<ShipInfo> _shipInfos;
		public IReadOnlyList<ShipInfo> ShipInfos => _shipInfos.AsReadOnly();

		private HttpClient _httpClient;
		public ApiService(ILogger logger, Settings settings)
		{
			_settings = settings;
			_logger = logger;
#pragma warning disable 4014
			Ships();
#pragma warning restore 4014
		}

		private string ApplicationId => _settings.Get<string>("appId" + _settings.Region);

		private async void Ships()
		{
			try
			{
				var client = new HttpClient() { BaseAddress = new Uri("https://wowreplays.com") };

				var result = await client.PostAsync("/Home/GetShipsForMatchmakingMonitor", null);
				if (result.StatusCode != HttpStatusCode.OK) return;
				var shipsJson = await result.Content.ReadAsStringAsync();
				_shipInfos = await Task.Run(() => JsonConvert.DeserializeObject<List<ShipInfo>>(shipsJson));
			}
			catch (Exception e)
			{
				_logger.Error("Exception Occurred While Retrieving Ships", e);
				_shipInfos = new List<ShipInfo>();
			}
		}

		public async Task<IEnumerable<PlayerShip>> Players(Replay replay)
		{
			try
			{
				while (_shipInfos == null)
				{
					await Task.Delay(1000);
				}
				var baseUrl = _settings.Get<string>("baseUrl" + _settings.Region);
				_httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };

				var tasks = replay.Vehicles.Select(GetAsync).ToList();
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
		private async Task<PlayerShip> GetAsync(Vehicle vehicle)
		{
			var shipInfo = ShipInfos.SingleOrDefault(s => s.ShipId == vehicle.ShipId);

			var playerResponse = await _httpClient.GetAsync($"wows/account/list/?application_id={ApplicationId}&search={vehicle.Name}");
			if (playerResponse.StatusCode == HttpStatusCode.OK)
			{
				var playerJson = await playerResponse.Content.ReadAsStringAsync();
				var players = await Task.Run(() => JsonConvert.DeserializeObject<WargamingSearch>(playerJson));

				var player = players.Data.SingleOrDefault(p => p.Nickname == vehicle.Name);
				if (player != null)
				{
					var shipResponse = await _httpClient.GetAsync($"wows/ships/stats/?application_id={ApplicationId}&account_id={player.AccountId}&ship_id={vehicle.ShipId}");
					if (shipResponse.StatusCode == HttpStatusCode.OK)
					{
						var shipJson = await shipResponse.Content.ReadAsStringAsync();
						shipJson = shipJson.Replace("\"" + player.AccountId + "\":", "\"Ships\":");
						var stats = await Task.Run(() => JsonConvert.DeserializeObject<PlayerStats>(shipJson));
						if (stats?.Data?.Ships != null && stats.Data.Ships.Any())
						{
							var ship = stats.Data.Ships.First();
							return new PlayerShip(ship, player, shipInfo, vehicle.Relation);
						}
					}
				}
			}
			return new PlayerShip(shipInfo)
			{
				Nickname = vehicle.Name,
				Relation = vehicle.Relation,
				IsPrivateOrHidden = true
			};
		}
	}
}
