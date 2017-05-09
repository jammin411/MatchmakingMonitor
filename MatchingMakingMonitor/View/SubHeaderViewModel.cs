using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using MatchMakingMonitor.Services;
using MatchMakingMonitor.View.Util;

namespace MatchMakingMonitor.View
{
	public class SubHeaderViewModel : BaseViewBinding
	{
		public RelayCommand PathClickCommand { get; set; }

		public ObservableCollection<string> Regions { get; } = new ObservableCollection<string>() { "NA", "EU", "RU", "SEA" };

		private string _region = "NA";

		public string Region
		{
			get => _region;
			set
			{
				_region = value;
				_settings.Region = value;
				FirePropertyChanged();
			}
		}


		private string _installDirectoryText = "Check install directory";

		public string InstallDirectoryText
		{
			get => _installDirectoryText;
			set
			{
				_installDirectoryText = value;
				FirePropertyChanged();
			}
		}

		private SolidColorBrush _installDirectoryColor;

		public SolidColorBrush InstallDirectoryColor
		{
			get => _installDirectoryColor;
			set
			{
				_installDirectoryColor = value;
				FirePropertyChanged();
			}
		}

		private string _statusText = "Not currently in Battle...";

		public string StatusText
		{
			get => _statusText;
			set
			{
				_statusText = value;
				FirePropertyChanged();
			}
		}

		private SolidColorBrush _statusColor = Brushes.Black;

		public SolidColorBrush StatusColor
		{
			get => _statusColor;
			set
			{
				_statusColor = value;
				FirePropertyChanged();
			}
		}

		private bool _enableUi = true;
		public bool EnableUi
		{
			get => _enableUi;
			set
			{
				_enableUi = value;
				FirePropertyChanged();
			}
		}

		private Visibility _showProgress = Visibility.Collapsed;
		public Visibility ShowProgress
		{
			get => _showProgress;
			set
			{
				_showProgress = value;
				FirePropertyChanged();
			}
		}

		private readonly Settings _settings;

		public SubHeaderViewModel(Settings settings, StatsService statsService)
		{
			_settings = settings;

			PathClickCommand = new RelayCommand(PathClicked);

			_settings.SettingChanged(Settings.KeyInstallDirectory).Subscribe(InitPath);
			statsService.StatsStatusChanged.Subscribe(status =>
			{
				SetStatusText(status);
				if (status == StatsStatus.Fetching)
				{
					EnableUi = false;
					ShowProgress = Visibility.Visible;
				}
				else
				{
					EnableUi = true;
					ShowProgress = Visibility.Collapsed;
				}
			});


			Region = _settings.Region;
		}

		public SubHeaderViewModel()
		{
		}

		private void InitPath(string key)
		{
			var directory = _settings.Get<string>(key);
			if (Directory.Exists(Path.Combine(directory, "replays")) && File.Exists(Path.Combine(directory, "WorldOfWarships.exe")))
			{
				InstallDirectoryColor = Brushes.Green;
				InstallDirectoryText = directory;
			}
			else
			{
				InstallDirectoryColor = Brushes.OrangeRed;
				InstallDirectoryText = directory + " - Invalid Path or Replays not enabled! - Click here to update!";
			}
		}

		private void PathClicked()
		{
			var folderBrowser = new FolderBrowserDialog();
			var result = folderBrowser.ShowDialog();
			if (result == DialogResult.OK && !string.IsNullOrEmpty(folderBrowser.SelectedPath))
			{
				_settings.InstallDirectory = folderBrowser.SelectedPath;
			}
		}

		private void SetStatusText(StatsStatus status)
		{
			switch (status)
			{
				case StatsStatus.Fetching:
					StatusText = "Fetching player stats for current battle";
					StatusColor = Brushes.DarkOrange;
					break;
				case StatsStatus.Fetched:
					StatusText = "Player Stats Succesfully Updated";
					StatusColor = Brushes.Green;
					break;
				case StatsStatus.WrongRegion:
					StatusText = "Found a battle but no stats. Wrong region?";
					StatusColor = Brushes.Red;
					break;
				case StatsStatus.Waiting:
					StatusText = "Not Currently in Battle - Stats are from Last Battle";
					StatusColor = Brushes.Black;
					break;
				default:
					StatusText = string.Empty;
					StatusColor = Brushes.Black;
					break;
			}
		}
	}
}
