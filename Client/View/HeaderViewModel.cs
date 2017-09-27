using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using MatchmakingMonitor.config;
using MatchmakingMonitor.Models;
using MatchmakingMonitor.Services;
using MatchmakingMonitor.SocketIO;
using MatchmakingMonitor.View.Util;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace MatchmakingMonitor.View
{
  public class HeaderViewModel : ViewModelBase
  {
    private readonly ILogger _logger;
    private readonly SettingsWrapper _settingsWrapper;
    private bool _canEnableReplays;
    private bool _canExport;

    private string _connectionState;
    private QrCodeWindow _qrCodeWindow;

    private SettingsWindow _settingsWindow;


    public HeaderViewModel()
    {
    }

    public HeaderViewModel(ILogger logger, SocketIoService socketIoService, StatsService statsService,
      SettingsWrapper settingsWrapper)
    {
      _logger = logger;
      _settingsWrapper = settingsWrapper;

      OpenWebsiteCommand = new RelayCommand(LogoClick);
      SettingsCommand = new RelayCommand(SettingsClick);
      QrCodeClickCommand = new RelayCommand(QrCodeClick);
      ExportStatsCommand = new RelayCommand(async _ => await ExportStatsAsync());
      EnableReplayCommand = new RelayCommand(EnableReplays);
      SwitchThemeCommand = new RelayCommand(SwitchTheme);

      socketIoService.StateChanged.Subscribe(state => { ConnectionState = state.ToString(); });
      statsService.StatsStatusChanged.Subscribe(status => { CanExport = status == StatsStatus.Fetched; });

      _settingsWrapper.SettingChanged(nameof(SettingsJson.InstallDirectory)).Subscribe(s =>
      {
        var path = Path.Combine(_settingsWrapper.CurrentSettings.InstallDirectory, "preferences.xml");
        CanEnableReplays = File.Exists(path);
      });
    }

    public RelayCommand OpenWebsiteCommand { get; set; }
    public RelayCommand SettingsCommand { get; set; }
    public RelayCommand QrCodeClickCommand { get; set; }
    public RelayCommand ExportStatsCommand { get; set; }
    public RelayCommand EnableReplayCommand { get; set; }
    public RelayCommand SwitchThemeCommand { get; set; }

    public bool CanExport
    {
      get => _canExport;
      set
      {
        _canExport = value;
        FirePropertyChanged();
      }
    }

    public bool CanEnableReplays
    {
      get => _canEnableReplays;
      set
      {
        _canEnableReplays = value;
        FirePropertyChanged();
      }
    }

    public string ConnectionState
    {
      get => _connectionState;
      set
      {
        _connectionState = value;
        FirePropertyChanged();
      }
    }

    private void LogoClick()
    {
      try
      {
        Process.Start("https://monitor.pepespub.de");
      }
      catch (Exception e)
      {
        _logger.Error("Exception Throw on Logo Click", e);
      }
    }

    private void SettingsClick()
    {
      if (_settingsWindow != null) return;
      _settingsWindow = IoCKernel.Get<SettingsWindow>();
      _settingsWindow.Show();
      _settingsWindow.Closed += (sender, args) => { _settingsWindow = null; };
    }

    private void QrCodeClick()
    {
      if (_qrCodeWindow != null) return;
      _qrCodeWindow = IoCKernel.Get<QrCodeWindow>();
      _qrCodeWindow.Show();
      _qrCodeWindow.Closed += (sender, args) => { _qrCodeWindow = null; };
    }

    private void EnableReplays()
    {
      var path = Path.Combine(_settingsWrapper.CurrentSettings.InstallDirectory, "preferences.xml");
      if (!File.Exists(path)) return;
      var fileContent = File.ReadAllText(path);
      if (!new Regex("<isReplayEnabled>(\\s*)true(\\s*)<\\/isReplayEnabled>").IsMatch(fileContent))
      {
        if (MessageBox.Show(Application.Current.MainWindow, "Do you want to enable replays?", "Enable replays",
              MessageBoxButton.YesNo) ==
            MessageBoxResult.Yes)
        {
          var falseRegex = new Regex("<isReplayEnabled>(\\s*)false(\\s*)<\\/isReplayEnabled>");
          if (falseRegex.IsMatch(fileContent))
            fileContent = falseRegex.Replace(fileContent, "<isReplayEnabled>true</isReplayEnabled>");
          else
            fileContent = fileContent.Replace("<scriptsPreferences>",
              "<scriptsPreferences><isReplayEnabled>true</isReplayEnabled>");

          File.WriteAllText(path, fileContent);

          var replaysPath = Path.Combine(_settingsWrapper.CurrentSettings.InstallDirectory, "replays");

          if (!Directory.Exists(replaysPath))
            Directory.CreateDirectory(replaysPath);

          _settingsWrapper.TriggerInstallDirectoryChanged();
        }
        MessageBox.Show(Application.Current.MainWindow, "Replays enabled", string.Empty, MessageBoxButton.OK);
      }
      else
      {
        MessageBox.Show(Application.Current.MainWindow, "Replays already enabled", string.Empty, MessageBoxButton.OK);
      }
    }

    private static async Task ExportStatsAsync()
    {
      if (IoCKernel.Get<StatsService>().CurrentStats == null) return;
      var sfd = new SaveFileDialog
      {
        FileName = $"stats_{DateTime.Now.ToFileTimeUtc()}.export",
        DefaultExt = ".json",
        Filter = "JSON Documents (.json)|*.json"
      };
      if (sfd.ShowDialog() == true)
      {
        var exportJson =
          await Task.Run(() => JsonConvert.SerializeObject(
            IoCKernel.Get<StatsService>().CurrentStats.Select(ExportPlayerStats.FromPlayerStats).ToArray(),
            JsonSerializerSettings));

        using (var f = File.CreateText(sfd.FileName))
        {
          await f.WriteAsync(exportJson);
        }
      }
    }

    private static void SwitchTheme()
    {
      var existingResourceDictionary = Application.Current.Resources.MergedDictionaries
        .Where(rd => rd.Source != null)
        .SingleOrDefault(rd => Regex.Match(rd.Source.OriginalString,
          @"(\/MaterialDesignThemes.Wpf;component\/Themes\/MaterialDesignTheme\.)((Light)|(Dark))").Success);
      if (existingResourceDictionary == null)
        throw new ApplicationException("Unable to find Light/Dark base theme in Application resources.");

      var isLight = existingResourceDictionary.Source.OriginalString.Contains("Light");

      var source =
        $"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.{(isLight ? "Dark" : "Light")}.xaml";
      var newResourceDictionary = new ResourceDictionary {Source = new Uri(source)};

      Application.Current.Resources.MergedDictionaries.Remove(existingResourceDictionary);
      Application.Current.Resources.MergedDictionaries.Add(newResourceDictionary);
    }
  }
}