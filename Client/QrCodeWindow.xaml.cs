using MatchMakingMonitor.SocketIO;

namespace MatchMakingMonitor
{
	/// <summary>
	///   Interaction logic for QrCode.xaml
	/// </summary>
	public partial class QrCodeWindow
	{
		// ReSharper disable once NotAccessedField.Local
		private SocketIOService _socketIoService;

		public QrCodeWindow(SocketIOService socketIoService)
		{
			_socketIoService = socketIoService;
			InitializeComponent();
		}
	}
}