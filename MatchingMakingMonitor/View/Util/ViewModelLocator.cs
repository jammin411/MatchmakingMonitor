using System.ComponentModel;
using System.Windows;
using MatchMakingMonitor.Services;

namespace MatchMakingMonitor.View.Util
{
	public class ViewModelLocator
	{
		private readonly DependencyObject _dummy = new DependencyObject();
		public MainWindowViewModel MainWindowViewModel => IsInDesignMode() ? new MainWindowViewModel() : IoCKernel.Get<MainWindowViewModel>();

		public HeaderViewModel HeaderViewModel => IsInDesignMode() ? new HeaderViewModel() : IoCKernel.Get<HeaderViewModel>();

		public SubHeaderViewModel SubHeaderViewModel
		{
			get
			{
				if (IsInDesignMode())
				{
					return new SubHeaderViewModel();
				}
				return IoCKernel.Get<SubHeaderViewModel>();
			}
		}

		public SettingsWindowViewModel SettingsViewModel => IsInDesignMode() ? new SettingsWindowViewModel() : IoCKernel.Get<SettingsWindowViewModel>();

		public StatsViewModel StatsViewModel => IsInDesignMode() ? new StatsViewModel() : IoCKernel.Get<StatsViewModel>();

		private bool IsInDesignMode()
		{
			return DesignerProperties.GetIsInDesignMode(_dummy);
		}
	}
}
