using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Backend.Controllers
{
  [RoutePrefix("download")]
  public class DownloadController : ApiController
  {
    [Route("latest")]
    [HttpGet]
    public async Task<HttpResponseMessage> GetLatest()
    {
      return await Specific(await VersionController.Latest());
    }

    [Route("specific/{version}")]
    [HttpGet]
    public async Task<HttpResponseMessage> GetSpecific(string version)
    {
      return await Specific(new Version(version.Replace('-', '.')));
    }

    private async Task<HttpResponseMessage> Specific(Version version)
    {
      return await GetResponseForVersion(version);
    }

    private static string GetPath(Version version)
    {
      return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "versions", $"MatchmakingMonitor-{version}.zip");
    }

    private static async Task<HttpResponseMessage> GetResponseForVersion(Version version)
    {
      var cache = MemoryCache.Default;
      var cacheKey = "version-" + version;

      var path = GetPath(version);

      var stream = new MemoryStream();
      var binaryWriter = new BinaryWriter(stream);

      try
      {
        var cachedBytes = cache.Get(cacheKey) as byte[];
#if !DEBUG
				if (cachedBytes != null && cachedBytes.Length > 0)
					binaryWriter.Write(cachedBytes);
				else
#endif
        await Task.Run(() =>
        {
          var bytes = File.ReadAllBytes(path);
          cache.Add(cacheKey, bytes,
            new CacheItemPolicy
            {
              Priority = CacheItemPriority.Default,
              AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10)
            });
          binaryWriter.Write(bytes);
        });
      }
      catch
      {
        throw new HttpException(500, "Error while trying to read the current version");
      }

      var httpResponseMessage = new HttpResponseMessage();

      stream.Seek(0, SeekOrigin.Begin);
      if (stream.Length <= 0) return httpResponseMessage;
      httpResponseMessage.Content = new StreamContent(stream);
      httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
      httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
      {
        FileName = Path.GetFileName(path)
      };

      return httpResponseMessage;
    }
  }
}