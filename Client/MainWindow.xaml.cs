using System.ComponentModel;
using System.Reflection;
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

			Title += $" (v{Assembly.GetExecutingAssembly().GetName().Version})";

			_settingsWrapper = settingsWrapper;
#if !DEBUG
			Left = _settingsWrapper.CurrentSettings.LastWindowProperties.Left;
			Top = _settingsWrapper.CurrentSettings.LastWindowProperties.Top;
			if (_settingsWrapper.CurrentSettings.LastWindowProperties.Width > 0)
				Width = _settingsWrapper.CurrentSettings.LastWindowProperties.Width;

			if (_settingsWrapper.CurrentSettings.LastWindowProperties.Height > 0)
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
			var lwp = new LastWindowProperties();
			if (Left > 0)
				lwp.Left = 0;
			if (Top > 0)
				lwp.Top = 0;
			if (ActualWidth > 0)
				lwp.Width = 0;
			if (ActualHeight > 0)
				lwp.Height = 0;
			if (WindowState != WindowState.Minimized)
				lwp.WindowState = 0;

			_settingsWrapper.SetLastWindowProperties(lwp);
		}
	}
}