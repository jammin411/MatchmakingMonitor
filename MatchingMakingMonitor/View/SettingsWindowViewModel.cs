using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows;
using MatchMakingMonitor.Services;
using MatchMakingMonitor.View.Util;
using Microsoft.Win32;

namespace MatchMakingMonitor.View
{
	public class SettingsWindowViewModel : BaseViewBinding
	{
		public RelayCommand ResetCommand { get; set; }
		public RelayCommand ExportCommand { get; set; }
		public RelayCommand ImportCommand { get; set; }

		public ObservableCollection<int> FontSizes { get; } = new ObservableCollection<int> { 8, 9, 10, 11, 12, 13, 14 };


		private string _battleWeight;
		public string BattleWeight
		{
			get => _battleWeight ?? (_battleWeight = Settings.BattleWeight.ToString(CultureInfo.InvariantCulture));
			set
			{
				_battleWeight = value;
				ValidateWeights();
				FirePropertyChanged();
			}
		}

		private string _fragsWeight;
		public string FragsWeight
		{
			get => _fragsWeight ?? (_fragsWeight = Settings.FragsWeight.ToString(CultureInfo.InvariantCulture));
			set
			{
				_fragsWeight = value;
				ValidateWeights();
				FirePropertyChanged();
			}
		}

		private string _xpWeight;
		public string XpWeight
		{
			get => _xpWeight ?? (_xpWeight = Settings.XpWeight.ToString(CultureInfo.InvariantCulture));
			set
			{
				_xpWeight = value;
				ValidateWeights();
				FirePropertyChanged();
			}
		}

		private string _dmgWeight;
		public string DmgWeight
		{
			get => _dmgWeight ?? (_dmgWeight = Settings.DmgWeight.ToString(CultureInfo.InvariantCulture));
			set
			{
				_dmgWeight = value;
				ValidateWeights();
				FirePropertyChanged();
			}
		}

		private string _winWeight;
		public string WinWeight
		{
			get => _winWeight ?? (_winWeight = Settings.WinWeight.ToString(CultureInfo.InvariantCulture));
			set
			{
				_winWeight = value;
				ValidateWeights();
				FirePropertyChanged();
			}
		}

		private Visibility _weightsErrorVisibility = Visibility.Hidden;

		public Visibility WeightsErrorVisibility
		{
			get => _weightsErrorVisibility;
			set
			{
				_weightsErrorVisibility = value;
				FirePropertyChanged();
			}
		}

		private const string WeightsErrorTextTemplate = "Sum of weights needs to be 5. (Current: {0})";

		private string _weightsErrorText;

		public string WeightsErrorText
		{
			get => _weightsErrorText;
			set
			{
				_weightsErrorText = value;
				FirePropertyChanged();
			}
		}


		public Settings Settings { get; set; }

		[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
		public SettingsWindowViewModel(Settings settings)
		{
			Settings = settings;

			ExportCommand = new RelayCommand(Export);
			ImportCommand = new RelayCommand(Import);

			ResetCommand = new RelayCommand(async () =>
			{
				var result = MessageBox.Show("Are you sure you want to reset all Settings?", "Reset Settings", MessageBoxButton.YesNo);
				if (result != MessageBoxResult.Yes) return;
				_winWeight = null;
				_battleWeight = null;
				_fragsWeight = null;
				_xpWeight = null;
				_dmgWeight = null;
				await Settings.ResetUiSettings();
				FirePropertyChanged(nameof(WinWeight));
				FirePropertyChanged(nameof(BattleWeight));
				FirePropertyChanged(nameof(FragsWeight));
				FirePropertyChanged(nameof(XpWeight));
				FirePropertyChanged(nameof(DmgWeight));
			});
		}



		public SettingsWindowViewModel()
		{

		}

		private async void Import()
		{
			var ofd = new OpenFileDialog()
			{
				FileName = "settings.export.json",
				DefaultExt = ".json",
				Filter = "JSON Documents (.json)|*.json",
				CheckFileExists = true
			};
			if (ofd.ShowDialog() == true)
			{
				await Settings.ImportUiSettings(ofd.FileName);
			}
		}

		private async void Export()
		{
			var sfd = new SaveFileDialog()
			{
				FileName = "settings.export",
				DefaultExt = ".json",
				Filter = "JSON Documents (.json)|*.json"
			};
			if (sfd.ShowDialog() == true)
			{
				await Settings.ExportUiSettings(sfd.FileName);
			}
		}

		private void ValidateWeights()
		{
			try
			{
				var winWeightD = double.Parse(WinWeight);
				var battleWeightD = double.Parse(BattleWeight);
				var fragsWeightD = double.Parse(FragsWeight);
				var xpWeightD = double.Parse(XpWeight);
				var dmgWeightD = double.Parse(DmgWeight);
				var sum = winWeightD + battleWeightD + fragsWeightD + xpWeightD + dmgWeightD;
				// ReSharper disable once CompareOfFloatsByEqualityOperator
				var valid = sum == 5;

				if (valid)
				{
					Settings.WinWeight = winWeightD;
					Settings.BattleWeight = battleWeightD;
					Settings.FragsWeight = fragsWeightD;
					Settings.XpWeight = xpWeightD;
					Settings.DmgWeight = dmgWeightD;
					WeightsErrorVisibility = Visibility.Hidden;
				}
				else
				{
					WeightsErrorVisibility = Visibility.Visible;
					WeightsErrorText = string.Format(WeightsErrorTextTemplate, sum);
				}
			}
			catch
			{
				WeightsErrorVisibility = Visibility.Visible;
				WeightsErrorText = "Invalid number";
			}
		}
	}
}
