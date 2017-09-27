using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using WebPush;

namespace PushNewVersion
{
  internal class Program
  {
    private static void Main()
    {
#if !DEBUG
			var path = "../registrations.json";
#endif
#if DEBUG
      var path = "../../../Backend.Test/registrations.json";
#endif
      var registrations = JsonConvert.DeserializeObject<List<Registration>>(File.ReadAllText(path));

      var vapidJson = File.ReadAllText("vapidDetails.json");
      var vapidDetails = JsonConvert.DeserializeObject<VapidDetails>(vapidJson);

      var webPushClient = new WebPushClient();

      var successList = new List<Registration>(5);
      foreach (var r in registrations)
        try
        {
          webPushClient.SendNotification(new PushSubscription(r.Endpoint, r.Key, r.AuthSecret),
            File.ReadAllText("notification.json"), vapidDetails);
          successList.Add(r);
        }
        catch (Exception e)
        {
          Console.WriteLine("Error at registration with key: " + r.Key);
          Console.WriteLine(e.Message);
          Console.WriteLine(e.InnerException?.Message);
          Console.WriteLine(e.InnerException?.InnerException?.Message);
        }
#if !DEBUG
			File.WriteAllText(path, JsonConvert.SerializeObject(successList));
#endif
      Console.ReadLine();
    }
  }

  public class Registration
  {
    public string Key { get; set; }
    public string AuthSecret { get; set; }
    public string Endpoint { get; set; }
  }
}