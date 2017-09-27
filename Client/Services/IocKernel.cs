using MatchmakingMonitor.SocketIO;
using MatchmakingMonitor.View;
using Ninject;

namespace MatchmakingMonitor.Services
{
  public static class IoCKernel
  {
    private static IKernel _kernel;

    public static void Init()
    {
      if (_kernel != null) return;
      _kernel = new StandardKernel();

      _kernel.Bind<ILogger>().To<FileLogger>().InSingletonScope();
      _kernel.Bind<SocketIoService>().To<SocketIoService>().InSingletonScope();
      _kernel.Bind<WatcherService>().To<WatcherService>().InSingletonScope();
      _kernel.Bind<StatsService>().To<StatsService>().InSingletonScope();
      _kernel.Bind<ApiService>().To<ApiService>().InSingletonScope();
      _kernel.Bind<SettingsWrapper>().To<SettingsWrapper>().InSingletonScope();

      _kernel.Bind<StatsViewModel>().To<StatsViewModel>().InSingletonScope();
      _kernel.Bind<QrCodeViewModel>().To<QrCodeViewModel>().InSingletonScope();
    }

    public static T Get<T>()
    {
      return _kernel.Get<T>();
    }
  }
}