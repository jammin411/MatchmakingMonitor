using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Windows;
using MatchMakingMonitor.Services;
using MatchMakingMonitor.SocketIO;

namespace MatchMakingMonitor
{
	/// <summary>
	///   Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		public static bool IsDebug;
		private SocketIOService _socketIoService;

		protected override void OnStartup(StartupEventArgs e)
		{
#if DEBUG
			IsDebug = true;
#endif
			IoCKernel.Init();
			ConfigureMainWindow();

			Current.MainWindow.Show();


			CheckForUpdate();
			_socketIoService = IoCKernel.Get<SocketIOService>();
#if DEBUG
			_socketIoService.Connect();
#endif
			base.OnStartup(e);
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

		private static async void CheckForUpdate()
		{
			try
			{
				var client = new HttpClient
				{
					BaseAddress = new Uri("http://monitor.pepespub.de")
				};

				var response = await client.GetAsync("/api/version/latest");
				if (!response.IsSuccessStatusCode) return;
				var versionString = (await response.Content.ReadAsStringAsync()).Replace("\"", string.Empty);
				var latestVersion = new Version(versionString);
				var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

				if (latestVersion.CompareTo(currentVersion) <= 0) return;
				var messageBoxResult =
					MessageBox.Show(
						$"A newer version is available for download.{Environment.NewLine}Current version: {currentVersion}, Latest version: {latestVersion}{Environment.NewLine}Go to download page?",
						"Newer version available", MessageBoxButton.YesNo);
				if (messageBoxResult == MessageBoxResult.Yes)
					Process.Start("http://monitor.pepespub.de/download/latest");
			}
			catch (Exception e)
			{
				var logger = IoCKernel.Get<ILogger>();
				logger.Error("Error while looking for newer version", e);
			}
		}
	}
}