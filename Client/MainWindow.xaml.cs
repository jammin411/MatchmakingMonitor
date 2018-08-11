using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using MatchmakingMonitor.config;
using MatchmakingMonitor.Services;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace MatchmakingMonitor
{
  public partial class MainWindow
  {
    private readonly SettingsWrapper _settingsWrapper;

    public MainWindow(SettingsWrapper settingsWrapper)
    {
      InitializeComponent();

      Title += $" (v{Assembly.GetExecutingAssembly().GetName().Version})";

      _settingsWrapper = settingsWrapper;

      Left = _settingsWrapper.CurrentSettings.LastWindowProperties.Left;
      Top = _settingsWrapper.CurrentSettings.LastWindowProperties.Top;
      if (_settingsWrapper.CurrentSettings.LastWindowProperties.Width > 0)
        Width = _settingsWrapper.CurrentSettings.LastWindowProperties.Width;

      if (_settingsWrapper.CurrentSettings.LastWindowProperties.Height > 0)
        Height = _settingsWrapper.CurrentSettings.LastWindowProperties.Height;

      WindowState = _settingsWrapper.CurrentSettings.LastWindowProperties.WindowState;
    }

    private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
    {
      Scroller.Height = e.NewSize.Height - Header.ActualHeight - 40;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      var lwp = new LastWindowProperties();
      lwp.Left = Left;
      lwp.Top = Top;
      if (ActualWidth > 0)
        lwp.Width = ActualWidth;
      if (ActualHeight > 0)
        lwp.Height = ActualHeight;
      if (WindowState != WindowState.Minimized)
        lwp.WindowState = WindowState;

      _settingsWrapper.SetLastWindowProperties(lwp);
    }
  }
}