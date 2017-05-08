using System;
using System.IO;

namespace MatchMakingMonitor.Services
{
	public class FileLogger : ILogger
	{
		private static string LogPath => AppDomain.CurrentDomain.BaseDirectory + "/Log.txt";
		public FileLogger()
		{
			if (!File.Exists(LogPath))
			{
				File.Create(LogPath);
			}
		}
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
