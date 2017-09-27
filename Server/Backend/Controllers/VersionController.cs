using System;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Http;

namespace Backend.Controllers
{
  [RoutePrefix("version")]
  public class VersionController : ApiController
  {
    private static readonly string CacheKeyLatest = "latest" + Guid.NewGuid();
    private static readonly string CacheKeyAll = "all" + Guid.NewGuid();

    [Route("latest")]
    [HttpGet]
    public async Task<string> GetLatest()
    {
      return (await Latest()).ToString();
    }

    public static async Task<Version> Latest()
    {
      var cache = MemoryCache.Default;

      try
      {
        var cachedVersion = cache.Get(CacheKeyLatest) as Version;
#if !DEBUG
				if (cachedVersion != null)
					return cachedVersion;
#endif
        var version = await GetVersion();
        cache.Add(CacheKeyLatest, version,
          new CacheItemPolicy
          {
            Priority = CacheItemPriority.Default,
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10)
          });
        return version;
      }
      catch
      {
        return new Version();
      }
    }

    private static async Task<Version> GetVersion()
    {
      var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "versions", "latest.txt");
      var versionString = await Task.Run(() => File.ReadAllText(path));
      return new Version(versionString);
    }

    [Route("all")]
    [HttpGet]
    public async Task<string[]> GetAll()
    {
      var cache = MemoryCache.Default;
      try
      {
        var cachedArray = cache.Get(CacheKeyAll) as string[];
#if !DEBUG
				if (cachedArray != null)
					return cachedArray;
#endif
        var versions = await GetVersions();
        cache.Add(CacheKeyLatest, versions,
          new CacheItemPolicy
          {
            Priority = CacheItemPriority.Default,
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10)
          });
        return versions;
      }
      catch
      {
        return new string[0];
      }
    }

    private static async Task<string[]> GetVersions()
    {
      var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "versions");
      return await Task.Run(() =>
      {
        var versions = Directory.EnumerateFiles(path, "*.zip", SearchOption.TopDirectoryOnly)
          .Select(Path.GetFileNameWithoutExtension)
          .Where(fileName => fileName.StartsWith("MatchmakingMonitor-", StringComparison.InvariantCultureIgnoreCase))
          .Select(fileName => fileName.Replace("MatchmakingMonitor-", string.Empty))
          .Select(v => new Version(v)).Sort();
        return versions.Select(v => v.ToString()).ToArray();
      });
    }
  }
}