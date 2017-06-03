using System.Windows;

namespace MatchMakingMonitor
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void WindowSizeChanged(object sender, SizeChangedEventArgs e)
		{
			Scroller.Height = e.NewSize.Height - Header.ActualHeight;
		}
	}
}