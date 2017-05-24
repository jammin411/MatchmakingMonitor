using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MatchMakingMonitor.Models.ResponseTypes;
using MatchMakingMonitor.Services;
using Newtonsoft.Json;

namespace MatchMakingMonitor.config.warshipsToday
{
	internal static class RemoteStats
	{
		private static readonly int[] Tiers = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

		public static async Task Get(SettingsJson settings, ILogger logger)
		{
			var allRegions = (await GetEntries(Region.EU, logger))
				.Concat(await GetEntries(Region.RU, logger))
				.Concat(await GetEntries(Region.NA, logger))
				.Concat(await GetEntries(Region.ASIA, logger)).ToList();

			if (allRegions.Any())
				await Task.Run(() =>
				{
					settings.BattleLimits = new double[] {150, 100, 80, 60, 40, 30, 20, 10, 0};
					settings.WinRateLimits = allRegions.AvgWinRate().OrderedArray().Calc(0, true);
					settings.AvgFragsLimits = allRegions.AvgFrags().OrderedArray().Calc(0, true);
					settings.AvgXpLimits = Tiers
						.Select(t => allRegions.Tier(t).AvgXp().OrderedArray().Calc(5 * t, false).LimitsTier(t))
						.ToArray();

					settings.AvgDmgLimits.Battleship = Tiers.Select(t => allRegions.Tier(t).Type(ShipType.Battleship).AvgDmg()
						.OrderedArray().Calc(500 * t, false).LimitsTier(t)).ToArray();
					settings.AvgDmgLimits.Cruiser = Tiers.Select(t => allRegions.Tier(t).Type(ShipType.Cruiser).AvgDmg().OrderedArray()
						.Calc(500 * t, false).LimitsTier(t)).ToArray();
					settings.AvgDmgLimits.Destroyer = Tiers.Select(t => allRegions.Tier(t).Type(ShipType.Destroyer).AvgDmg()
						.OrderedArray().Calc(500 * t, false).LimitsTier(t)).ToArray();
					settings.AvgDmgLimits.AirCarrier = Tiers.Select(t => allRegions.Tier(t).Type(ShipType.AirCarrier).AvgDmg()
						.OrderedArray().Calc(500 * t, false).LimitsTier(t)).ToArray();
				});
		}

		private static async Task<WarshipsTodayEntry[]> GetEntries(Region region, ILogger logger)
		{
			try
			{
				var client = new HttpClient
				{
					BaseAddress = new Uri($"https://api.{region}.warships.today")
				};

				var response = await client.GetAsync("api/vehicles");

				if (!response.IsSuccessStatusCode) return new WarshipsTodayEntry[0];

				var json = await response.Content.ReadAsStringAsync();
				return await Task.Run(() => JsonConvert.DeserializeObject<WarshipsTodayEntry[]>(json));
			}
			catch (Exception e)
			{
				logger.Error($"Error retreiving data from url 'https://api.{region}.warships.today'", e);
				return new WarshipsTodayEntry[0];
			}
		}
	}
}