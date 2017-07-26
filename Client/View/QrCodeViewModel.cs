using System;
using System.Drawing;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;
using MatchMakingMonitor.Services;
using MatchMakingMonitor.SocketIO;
using MatchMakingMonitor.View.Util;

namespace MatchMakingMonitor.View
{
	public class QrCodeViewModel : ViewModelBase
	{
		private string _token;

		public string Token
		{
			get => _token;
			set
			{
				_token = value;
				FirePropertyChanged();
			}
		}

		private BitmapImage _qrCode;

		public BitmapImage QrCode
		{
			get => _qrCode;
			set
			{
				_qrCode = value;
				FirePropertyChanged();
			}
		}

		public RelayCommand GenerateCommand { get; set; }

		// private TaskScheduler uiScheduler;

		private readonly SocketIoService _socketIoService;

		public QrCodeViewModel()
		{

		}

		public QrCodeViewModel(SocketIoService socketIoService, SettingsWrapper settingsWrapper)
		{
			// uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
			GenerateCommand = new RelayCommand(() => Generate());

			_socketIoService = socketIoService;

			if (string.IsNullOrEmpty(settingsWrapper.CurrentSettings.Token))
			{
				settingsWrapper.CurrentSettings.Token = Guid.NewGuid().ToString();
			}
			Generate(settingsWrapper.CurrentSettings.Token);
		}

		private async void Generate(string token = null)
		{
			Guid guid = Guid.Empty;
			if (!Guid.TryParse(token, out guid) || guid.Equals(Guid.Empty))
			{
				guid = Guid.NewGuid();
			}
			Token = guid.ToString();
			_socketIoService.Hub.SetToken(Token);
			await Task.Run(() =>
			{
				var qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
				var qrCode = qrEncoder.Encode(Token);

				GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);
				using (var ms = new MemoryStream())
				{
					renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
					var bitmapImage = new BitmapImage();
					bitmapImage.BeginInit();
					bitmapImage.StreamSource = ms;
					bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
					bitmapImage.EndInit();
					bitmapImage.Freeze();
					//Task.Factory.StartNew(() =>
					//{
					QrCode = bitmapImage;
					//}, CancellationToken.None, TaskCreationOptions.None, uiScheduler);
				}
			});
		}
	}
}