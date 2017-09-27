using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MatchmakingMonitor.Services;
using Newtonsoft.Json;

namespace MatchmakingMonitor.config.wowsNumbers
{
  internal static class RemoteStats
  {
    private static readonly int[] Tiers = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

    public static async Task<bool> Get(SettingsJson settings, ILogger logger)
    {
      var allRegions = (await GetEntries(Region.EU, logger))
        .Concat(await GetEntries(Region.RU, logger))
        .Concat(await GetEntries(Region.NA, logger))
        .Concat(await GetEntries(Region.ASIA, logger)).ToList();

      if (allRegions.Any())
        return await Task.Run(() =>
        {
          try
          {
            settings.BattleLimits = new double[] {150, 100, 80, 60, 40, 30, 20, 10, 0};
            settings.WinRateLimits = allRegions.AvgWinRate().OrderedArray().Calc(0, true);
            settings.AvgFragsLimits = allRegions.AvgFrags().OrderedArray().Calc(0, true);
            settings.AvgXpLimits = Tiers
              .Select(t => allRegions.Tier(t).AvgXp().OrderedArray().Calc(5 * t, false).LimitsTier(t))
              .ToArray();

            settings.AvgDmgLimits.Battleship = Tiers.Select(t => allRegions.Tier(t).Type(WowsNumbersShipType.Battleship)
              .AvgDmg()
              .OrderedArray().Calc(500 * t, false).LimitsTier(t)).ToArray();
            settings.AvgDmgLimits.Cruiser = Tiers.Select(t => allRegions.Tier(t).Type(WowsNumbersShipType.Cruiser)
              .AvgDmg()
              .OrderedArray()
              .Calc(500 * t, false).LimitsTier(t)).ToArray();
            settings.AvgDmgLimits.Destroyer = Tiers.Select(t => allRegions.Tier(t).Type(WowsNumbersShipType.Destroyer)
              .AvgDmg()
              .OrderedArray().Calc(500 * t, false).LimitsTier(t)).ToArray();
            settings.AvgDmgLimits.AirCarrier = Tiers.Select(t => allRegions.Tier(t)
              .Type(WowsNumbersShipType.AircraftCarrier).AvgDmg()
              .OrderedArray().Calc(500 * t, false).LimitsTier(t)).ToArray();
          }
          catch (Exception e)
          {
            logger.Error($"Error calculating average limits from warships.today", e);
          }

          return true;
        });

      return false;
    }

    private static async Task<WowsNumbersShipEntry[]> GetEntries(Region region, ILogger logger)
    {
      try
      {
        var regionString = region == Region.EU ? string.Empty : $"{region}.";
        var client = new HttpClient
        {
          BaseAddress = new Uri($"https://{regionString}wows-numbers.com")
        };

        var response = await client.GetAsync("ships");

        if (!response.IsSuccessStatusCode) return new WowsNumbersShipEntry[0];

        var html = await response.Content.ReadAsStringAsync();
        var regex = new Regex("dataProvider.ships = (\\[(.)*\\])", RegexOptions.Multiline);
        var match = regex.Match(html);
        if (!match.Success) throw new Exception("No ship data found");

        return await Task.Run(() => JsonConvert.DeserializeObject<WowsNumbersShipEntry[]>(match.Groups[1].Value));
      }
      catch (Exception e)
      {
        logger.Error($"Error retreiving data from url 'https://api.{region}.warships.today'", e);
        return new WowsNumbersShipEntry[0];
      }
    }
  }
}