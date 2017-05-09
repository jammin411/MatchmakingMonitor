using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace MatchMakingMonitor.Services
{
	public class WatcherService
	{
		private bool _initialCheckDone;

		private readonly Settings _settings;
		private readonly ILogger _logger;

		private readonly BehaviorSubject<string> _matchFoundSubject;

		public IObservable<string> MatchFound => _matchFoundSubject.AsObservable();
		public WatcherService(ILogger logger, Settings settings)
		{
			_settings = settings;
			_logger = logger;

			_matchFoundSubject = new BehaviorSubject<string>(null);

			var fileSystemWatcher = new FileSystemWatcher
			{
				Filter = "tempArenaInfo.json",
				NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.Attributes |
											 NotifyFilters.LastAccess
			};
			fileSystemWatcher.Created += (obj, args) =>
			{
				CallMatchFound(args.FullPath);
			};
			fileSystemWatcher.Changed += (obj, args) =>
			{
				CallMatchFound(args.FullPath);
			};


			settings.SettingChanged(Settings.KeyInstallDirectory).Subscribe(key =>
			{
				var directory = settings.InstallDirectory;
				if (Directory.Exists(Path.Combine(directory, "replays")) && File.Exists(Path.Combine(directory, "WorldOfWarships.exe")))
				{
					fileSystemWatcher.Path = Path.Combine(directory, "replays");
					fileSystemWatcher.EnableRaisingEvents = true;
					if (_initialCheckDone) return;
					_initialCheckDone = true;
					CheckStatic();
				}
				else
				{
					fileSystemWatcher.EnableRaisingEvents = false;
				}
			});

			settings.SettingChanged(Settings.KeyRegion, false).Subscribe(key =>
			{
				CheckStatic();
			});
		}

		private void CheckStatic()
		{
			var directory = _settings.InstallDirectory;
			var path = Path.Combine(directory, "replays", "tempArenaInfo.json");
			_logger.Info("Checking for match in path " + path);
			if (File.Exists(path))
			{
				CallMatchFound(path);
			}
		}

		private void CallMatchFound(string path)
		{
			_logger.Info("Match found");
			_matchFoundSubject.OnNext(path);
		}
	}
}
