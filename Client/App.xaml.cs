using System.Windows;
using MatchMakingMonitor.Services;
using System.Net.Http;
using System;
using System.Reflection;
using System.Diagnostics;

namespace MatchMakingMonitor
{
	/// <summary>
	///   Interaction logic for App.xaml
	/// </summary>
	public partial class App
	{
		public static bool IsDebug;

		protected override void OnStartup(StartupEventArgs e)
		{
#if DEBUG
			IsDebug = true;
#endif
			IoCKernel.Init();
			ConfigureMainWindow();

			Current.MainWindow.Show();


			CheckForUpdate();
			//socketIOService = IoCKernel.Get<SocketIOService>();
			//socketIOService.Connect();
			//socketIOService.StateChanged.Where(s => s == ConnectionState.Connected).Subscribe(_ =>
			//{
			//	if (!string.IsNullOrEmpty(MatchMakingMonitor.Properties.Settings.Default.Token))
			//	{
			//		socketIOService.Hub.SetToken(MatchMakingMonitor.Properties.Settings.Default.Token);
			//	}
			//});
			base.OnStartup(e);
		}

		private static void ConfigureMainWindow()
		{
			Current.MainWindow = IoCKernel.Get<MainWindow>();
		}

		private static async void CheckForUpdate()
		{
			try
			{
				var client = new HttpClient()
				{
					BaseAddress = new Uri("http://monitor.pepespub.de")
				};

				var response = await client.GetAsync("/api/version/latest");
				if (!response.IsSuccessStatusCode) return;
				var versionString = (await response.Content.ReadAsStringAsync()).Replace("\"", string.Empty);
				var latestVersion = new Version(versionString);
				var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

				if (latestVersion.CompareTo(currentVersion) <= 0) return;
				var messageBoxResult = MessageBox.Show($"A newer version is available for download.{Environment.NewLine}Current version: {currentVersion}, Latest version: {latestVersion}{Environment.NewLine}Go to download page?", "Newer version available", MessageBoxButton.YesNo);
				if (messageBoxResult == MessageBoxResult.Yes)
				{
					Process.Start("http://monitor.pepespub.de/download/latest");
				}
			}
			catch (Exception e)
			{
				var logger = IoCKernel.Get<ILogger>();
				logger.Error("Error while looking for newer version", e);
			}
		}
	}
}