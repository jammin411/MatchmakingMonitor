using System;
using System.Collections.Generic;
using System.Linq;
using MatchmakingMonitor.Models.ResponseTypes;

namespace MatchmakingMonitor.config.wowsNumbers
{
  public static class Extensions
  {
    public static IEnumerable<WowsNumbersShipEntry> Tier(this IEnumerable<WowsNumbersShipEntry> entries, int tier)
    {
      return entries.Where(s => s.Tier == tier);
    }

    public static IEnumerable<WowsNumbersShipEntry> Type(this IEnumerable<WowsNumbersShipEntry> entries,
      WowsNumbersShipType type)
    {
      return entries.Where(s => s.Type == type);
    }

    public static IEnumerable<double> AvgWinRate(this IEnumerable<WowsNumbersShipEntry> entries)
    {
      return entries.Select(s => s.WinRate);
    }

    public static IEnumerable<double> AvgFrags(this IEnumerable<WowsNumbersShipEntry> entries)
    {
      return entries.Select(s => s.AvgFrags);
    }

    public static IEnumerable<double> AvgXp(this IEnumerable<WowsNumbersShipEntry> entries)
    {
      return entries.Select(s => s.AvgXp);
    }

    public static IEnumerable<double> AvgDmg(this IEnumerable<WowsNumbersShipEntry> entries)
    {
      return entries.Select(s => s.AvgDamageDealt);
    }

    public static double[] OrderedArray(this IEnumerable<double> entries)
    {
      return entries.OrderByDescending(s => s).Distinct().ToArray();
    }

    public static double[] Calc(this double[] values, double coef, bool useDecimal)
    {
      var arr = new double[9];

      if (!values.Any()) return new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0};

      var indexStep = (int) Math.Floor((double) values.Length / 7);
      var skiplast = indexStep * 7 == values.Length;


      for (var i = 0; i < 9; i++)
      {
        double val;
        switch (i)
        {
          case 0:
            val = values[0];
            break;
          case 8:
            arr[i] = 0;
            continue;
          default:
            if (i == 7 && skiplast)
              val = values[values.Length - 1];
            else
              val = values[indexStep * i];
            break;
        }
        var multi = 1 + (double) i;
        if (!useDecimal)
        {
          if (i <= 4) val = Math.Floor(val) - coef;
          if (i > 4) val = Math.Ceiling(val) - coef * multi;
        }
        else
        {
          if (i <= 4) val = Math.Floor((val - coef) * 100) / 100;
          if (i > 4) val = Math.Ceiling((val - coef * multi) * 100) / 100;
        }
        arr[i] = val;
      }

      return arr;
    }

    public static LimitsTier LimitsTier(this double[] values, int tier)
    {
      return new LimitsTier
      {
        ShipTier = (ShipTier) tier,
        Values = values
      };
    }
  }
}