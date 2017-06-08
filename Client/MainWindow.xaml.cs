using System;
using System.ComponentModel;
using System.Windows;
using MatchmakingMonitor.config;
using MatchMakingMonitor.Services;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace MatchMakingMonitor
{
	public partial class MainWindow
	{
		private readonly SettingsWrapper _settingsWrapper;

		public MainWindow(SettingsWrapper settingsWrapper)
		{
			InitializeComponent();

			_settingsWrapper = settingsWrapper;
#if !DEBUG
			Left = _settingsWrapper.CurrentSettings.LastWindowProperties.Left;
			Top = _settingsWrapper.CurrentSettings.LastWindowProperties.Top;
			if (_settingsWrapper.CurrentSettings.LastWindowProperties.Width != 0)
				Width = _settingsWrapper.CurrentSettings.LastWindowProperties.Width;

			if (_settingsWrapper.CurrentSettings.LastWindowProperties.Height != 0)
				Height = _settingsWrapper.CurrentSettings.LastWindowProperties.Height;

			WindowState = _settingsWrapper.CurrentSettings.LastWindowProperties.WindowState;
#endif
		}

		private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
		{
			Scroller.Height = e.NewSize.Height - Header.ActualHeight - 40;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			_settingsWrapper.SetLastWindowProperties(new LastWindowProperties
			{
				Left = Left,
				Top = Top,
				Width = ActualWidth,
				Height = ActualHeight,
				WindowState = WindowState
			});
		}
	}
}