using MatchMakingMonitor.SocketIO;

namespace MatchMakingMonitor
{
	/// <summary>
	///   Interaction logic for QrCode.xaml
	/// </summary>
	public partial class QrCodeWindow
	{
		// ReSharper disable once NotAccessedField.Local
		private SocketIoService _socketIoService;

		public QrCodeWindow(SocketIoService socketIoService)
		{
			_socketIoService = socketIoService;
			InitializeComponent();
		}
	}
}