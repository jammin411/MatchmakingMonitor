using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend
{
  public static class Extensions
  {
    public static List<Version> Sort(this IEnumerable<Version> versions)
    {
      var list = versions.ToList();
      list.Sort((x, y) => y.CompareTo(x));
      return list;
    }
  }
}