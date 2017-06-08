using MatchMakingMonitor.SocketIO;
using MatchMakingMonitor.View;
using Ninject;

namespace MatchMakingMonitor.Services
{
	public static class IoCKernel
	{
		private static IKernel _kernel;

		public static void Init()
		{
			if (_kernel != null) return;
			_kernel = new StandardKernel();

			_kernel.Bind<ILogger>().To<FileLogger>().InSingletonScope();
			_kernel.Bind<SocketIOService>().To<SocketIOService>().InSingletonScope();
			_kernel.Bind<WatcherService>().To<WatcherService>().InSingletonScope();
			_kernel.Bind<StatsService>().To<StatsService>().InSingletonScope();
			_kernel.Bind<ApiService>().To<ApiService>().InSingletonScope();
			_kernel.Bind<SettingsWrapper>().To<SettingsWrapper>().InSingletonScope();

			_kernel.Bind<StatsViewModel>().To<StatsViewModel>().InSingletonScope();
		}

		public static T Get<T>()
		{
			return _kernel.Get<T>();
		}
	}
}