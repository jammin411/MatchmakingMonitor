using System;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Backend.Controllers
{
	[RoutePrefix("changelog")]
	public class ChangelogController : ApiController
	{
		private static readonly string CacheKey = "Changelogs";

		[Route("list")]
		public async Task<string[]> GetList()
		{
			var cache = MemoryCache.Default;

			try
			{
				var cachedChangelogs = cache.Get(CacheKey) as string[];
#if !DEBUG
				if (cachedChangelogs != null)
					return cachedChangelogs;
#endif
				var changeLogs = await GetChangelogs();
				cache.Add(CacheKey, changeLogs,
					new CacheItemPolicy
					{
						Priority = CacheItemPriority.Default,
						AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10)
					});
				return changeLogs;
			}
			catch
			{
				return new string[0];
			}
		}

		private static async Task<string[]> GetChangelogs()
		{
			return await Task.Run(() => Directory.EnumerateFiles(
					Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "changelogs"),
					"*.md",
					SearchOption.TopDirectoryOnly).Select(Path.GetFileNameWithoutExtension).Select(name => new Version(name)).Sort()
				.Select(v => v.ToString()).ToArray());
		}

		[Route("detail/{version}")]
		public string GetChangelog(string version)
		{
			var dotdot = new Regex("\\.{2,}");
			version = dotdot.Replace(version, string.Empty);
			var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "changelogs", $"{version}.md");
			if (File.Exists(filePath))
				return File.ReadAllText(filePath);
			throw new HttpException(404, "Changelog not found");
		}
	}
}