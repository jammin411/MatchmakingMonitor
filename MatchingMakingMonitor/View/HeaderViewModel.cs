using System;
using System.Diagnostics;
using MatchMakingMonitor.Services;
using MatchMakingMonitor.SocketIO;
using MatchMakingMonitor.View.Util;

namespace MatchMakingMonitor.View
{
	public class HeaderViewModel : ViewModelBase
	{
		private readonly ILogger _logger;

		private string _connectionState;

		private SettingsWindow _settingsWindow;

		public HeaderViewModel()
		{
		}

		public HeaderViewModel(ILogger logger, SocketIoService socketIoService)
		{
			_logger = logger;

			LogoClickCommand = new RelayCommand(LogoClick);
			SettingsCommand = new RelayCommand(SettingsClick);

			socketIoService.StateChanged.Subscribe(state => { ConnectionState = state.ToString(); });
		}

		public RelayCommand LogoClickCommand { get; set; }
		public RelayCommand SettingsCommand { get; set; }

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
				Process.Start("https://wowreplays.com");
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
	}
}