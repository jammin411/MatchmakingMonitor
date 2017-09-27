using System;
using System.IO;

namespace MatchmakingMonitor.Services
{
  public class FileLogger : ILogger
  {
    public FileLogger()
    {
      if (!File.Exists(LogPath))
        File.Create(LogPath);
    }

    private static string LogPath => AppDomain.CurrentDomain.BaseDirectory + "/Log.txt";

    public void Info(string message)
    {
      Log($"Info - {message}");
    }

    public void Error(string message, Exception e)
    {
      Log($"Error - {message} - {e?.Message}");
    }

    private static async void Log(string message)
    {
      try
      {
        using (var sw = new StreamWriter(LogPath, true))
        {
          await sw.WriteLineAsync(DateTime.Now + " - " + message);
        }
      }
      catch
      {
        // ignored
      }
    }
  }
}