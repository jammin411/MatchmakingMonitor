using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace Backend.Controllers
{
  [RoutePrefix("push")]
  public class PushController : ApiController
  {
    [HttpPost]
    [Route("register")]
    public async Task<bool> Register(Registration registration)
    {
      var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "registrations.json");
      if (!File.Exists(path))
        File.Create(path).Close();

      var registrations =
        await Task.Run(() => JsonConvert.DeserializeObject<List<Registration>>(File.ReadAllText(path))) ??
        new List<Registration>(1);
      if (registrations.All(r => r.Key != registration.Key))
        registrations.Add(registration);

      File.WriteAllText(path, await Task.Run(() => JsonConvert.SerializeObject(registrations)), Encoding.UTF8);

      return true;
    }
  }

  public class Registration
  {
    public string Key { get; set; }
    public string AuthSecret { get; set; }
    public string Endpoint { get; set; }
  }
}