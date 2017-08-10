using System.ComponentModel;
using System.Windows;
using MatchMakingMonitor.Services;
using MatchMakingMonitor.View.Settings;

namespace MatchMakingMonitor.View.Util
{
	public class ViewModelLocator
	{
		private readonly DependencyObject _dummy = new DependencyObject();

		public MainWindowViewModel MainWindowViewModel => IsInDesignMode()
			? new MainWindowViewModel()
			: IoCKernel.Get<MainWindowViewModel>();

		public HeaderViewModel HeaderViewModel => IsInDesignMode() ? new HeaderViewModel() : IoCKernel.Get<HeaderViewModel>();

		public SubHeaderViewModel SubHeaderViewModel => IsInDesignMode()
			? new SubHeaderViewModel()
			: IoCKernel.Get<SubHeaderViewModel>();

		public SettingsWindowViewModel SettingsViewModel => IsInDesignMode()
			? new SettingsWindowViewModel()
			: IoCKernel.Get<SettingsWindowViewModel>();

		public StatsViewModel StatsViewModel => IsInDesignMode() ? new StatsViewModel() : IoCKernel.Get<StatsViewModel>();

		public QrCodeViewModel QrCodeViewModel => IsInDesignMode() ? new QrCodeViewModel() : IoCKernel.Get<QrCodeViewModel>();

		private bool IsInDesignMode()
		{
			return DesignerProperties.GetIsInDesignMode(_dummy);
		}
	}
}