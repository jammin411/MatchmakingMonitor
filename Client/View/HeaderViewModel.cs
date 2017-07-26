using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MatchmakingMonitor.Models;
using MatchMakingMonitor.Services;
using MatchMakingMonitor.SocketIO;
using MatchMakingMonitor.View.Util;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace MatchMakingMonitor.View
{
	public class HeaderViewModel : ViewModelBase
	{
		private readonly ILogger _logger;

		private string _connectionState;
		private Visibility _canExport;

		private SettingsWindow _settingsWindow;
		private QrCodeWindow _qrCodeWindow;

		public HeaderViewModel()
		{
		}

		public HeaderViewModel(ILogger logger, SocketIoService socketIoService, StatsService statsService)
		{
			_logger = logger;

			LogoClickCommand = new RelayCommand(LogoClick);
			SettingsCommand = new RelayCommand(SettingsClick);
			QrCodeClickCommand = new RelayCommand(QrCodeClick);
			ExportStatsCommand = new RelayCommand(async _ => await ExportStatsAsync());

			socketIoService.StateChanged.Subscribe(state => { ConnectionState = state.ToString(); });
			statsService.StatsStatusChanged.Subscribe(status =>
				{
					CanExport = status == StatsStatus.Fetched ? Visibility.Visible : Visibility.Collapsed;
				});
		}

		public RelayCommand LogoClickCommand { get; set; }
		public RelayCommand SettingsCommand { get; set; }
		public RelayCommand QrCodeClickCommand { get; set; }
		public RelayCommand ExportStatsCommand { get; set; }

		public Visibility CanExport
		{
			get => _canExport;
			set
			{
				_canExport = value;
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
				var exportJson = await Task.Run(() => JsonConvert.SerializeObject(IoCKernel.Get<StatsService>().CurrentStats.Select(ExportPlayerStats.FromPlayerStats).ToArray(), JsonSerializerSettings));

				using (var f = File.CreateText(sfd.FileName))
				{
					await f.WriteAsync(exportJson);
				}
			}
		}
	}
}