using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MatchmakingMonitor.config;

namespace MatchmakingMonitor.Services
{
  public class WatcherService
  {
    private readonly ILogger _logger;

    private readonly BehaviorSubject<string> _matchFoundSubject;

    private readonly SettingsWrapper _settingsWrapper;
    private bool _initialCheckDone;

    public WatcherService(ILogger logger, SettingsWrapper settingsWrapper)
    {
      _settingsWrapper = settingsWrapper;
      _logger = logger;

      _matchFoundSubject = new BehaviorSubject<string>(null);

      var fileSystemWatcher = new FileSystemWatcher
      {
        Filter = "tempArenaInfo.json",
        NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.Attributes |
                       NotifyFilters.LastAccess
      };
      fileSystemWatcher.Created += (obj, args) => { CallMatchFound(args.FullPath); };
      fileSystemWatcher.Changed += (obj, args) => { CallMatchFound(args.FullPath); };


      settingsWrapper.SettingChanged(nameof(SettingsJson.InstallDirectory)).Subscribe(key =>
      {
        var directory = settingsWrapper.CurrentSettings.InstallDirectory;
        if (Directory.Exists(Path.Combine(directory, "replays")) &&
            File.Exists(Path.Combine(directory, "WorldOfWarships.exe")))
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

      settingsWrapper.SettingChanged(nameof(SettingsJson.Region), false).Subscribe(change =>
      {
        if (!change.Initial)
          CheckStatic();
      });
    }

    public IObservable<string> MatchFound => _matchFoundSubject.Throttle(TimeSpan.FromMilliseconds(150)).AsObservable();

    private void CheckStatic()
    {
      var directory = _settingsWrapper.CurrentSettings.InstallDirectory;
      var path = Path.Combine(directory, "replays", "tempArenaInfo.json");
      _logger.Info("Checking for match in path " + path);
      if (File.Exists(path))
        CallMatchFound(path);
    }

    private void CallMatchFound(string path)
    {
      _logger.Info("Match found");
      _matchFoundSubject.OnNext(path);
    }
  }
}