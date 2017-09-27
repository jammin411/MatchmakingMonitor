using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using MatchmakingMonitor.Services;
using MatchmakingMonitor.SocketIO;

namespace MatchmakingMonitor
{
  /// <summary>
  ///   Interaction logic for App.xaml
  /// </summary>
  public partial class App
  {
    public static bool IsDebug;
    private SocketIoService _socketIoService;
#if !DEBUG
    private static readonly Uri BaseUri = new Uri("https://monitor.pepespub.de");
#endif
#if DEBUG
    private static readonly Uri BaseUri = new Uri("http://monitor.local");
#endif

    protected override void OnStartup(StartupEventArgs e)
    {
#if DEBUG
      IsDebug = true;
#endif
      IoCKernel.Init();
      ConfigureMainWindow();

      Current.MainWindow.Show();


      CheckForUpdate(IoCKernel.Get<SettingsWrapper>());
      _socketIoService = IoCKernel.Get<SocketIoService>();
      //_socketIoService.Connect();
      base.OnStartup(e);
      DispatcherUnhandledException += App_DispatcherUnhandledException;
    }

    private static void App_DispatcherUnhandledException(object sender,
      DispatcherUnhandledExceptionEventArgs e)
    {
      if (!e.Handled)
        IoCKernel.Get<ILogger>().Error("Error occured", e.Exception);
    }

    protected override void OnExit(ExitEventArgs e)
    {
      _socketIoService.Disconnect();
      base.OnExit(e);
    }

    private static void ConfigureMainWindow()
    {
      Current.MainWindow = IoCKernel.Get<MainWindow>();
    }

    private static async void CheckForUpdate(SettingsWrapper settingsWrapper)
    {
      try
      {
        var client = new HttpClient
        {
          BaseAddress = BaseUri
        };

        var response = await client.GetAsync("/api/version/latest");
        if (!response.IsSuccessStatusCode) return;
        var versionString = (await response.Content.ReadAsStringAsync()).Replace("\"", string.Empty);
        var latestVersion = new Version(versionString);
        var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

        if (latestVersion.CompareTo(currentVersion) <= 0) return;
        //TODO: Find a solution for automatic updates
        //if (settingsWrapper.CurrentSettings.AutomaticAppUpdate)
        //{
        //	new UpdateWindow(new Uri(BaseUri, "/api/download/latest")).Show();
        //}
        //else
        //{
        var messageBoxResult =
          MessageBox.Show(
            $"A newer version is available for download.{Environment.NewLine}Current version: {currentVersion}, Latest version: {latestVersion}{Environment.NewLine}Go to download page?",
            "Newer version available", MessageBoxButton.YesNo);
        if (messageBoxResult == MessageBoxResult.Yes)
          Process.Start("http://monitor.pepespub.de/download/latest");
        //}
      }
      catch (Exception e)
      {
        var logger = IoCKernel.Get<ILogger>();
        logger.Error("Error while looking for newer version", e);
      }
    }
  }
}