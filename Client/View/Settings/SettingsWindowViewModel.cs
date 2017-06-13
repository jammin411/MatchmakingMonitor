using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using MatchMakingMonitor.config;
using MatchMakingMonitor.config.Reflection;
using MatchMakingMonitor.Services;
using MatchMakingMonitor.View.Util;
using Microsoft.Win32;

namespace MatchMakingMonitor.View.Settings
{
	public class SettingsWindowViewModel : ViewModelBase
	{
		private readonly SettingsWrapper _settingsWrapper;

		private int _fontSize;
		private bool _hideLowBattles;
		private bool _automaticLimitsSync;
		private bool _automaticAppUpdate;

		[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
		public SettingsWindowViewModel(SettingsWrapper settingsWrapper)
		{
			_settingsWrapper = settingsWrapper;

			ExportCommand = new RelayCommand(Export);
			ImportCommand = new RelayCommand(Import);
			ResetCommand = new RelayCommand(Reset);

			_fontSize = _settingsWrapper.CurrentSettings.FontSize;
			_hideLowBattles = _settingsWrapper.CurrentSettings.HideLowBattles;
			_automaticLimitsSync = _settingsWrapper.CurrentSettings.AutomaticLimitsSync;
			_automaticAppUpdate = _settingsWrapper.CurrentSettings.AutomaticAppUpdate;

			ColorsViewModel = new ColorsViewModel("Colors",
				new ColorsEditor(_settingsWrapper.SettingChangedSubject, ref settingsWrapper.CurrentSettings.Colors));

			StaticViewModels = new ObservableCollection<LimitsViewModel>
			{
				new LimitsViewModel("Battle limits",
					new DoubleLimitsEditor(_settingsWrapper.SettingChangedSubject, _settingsWrapper.CurrentSettings.BattleLimits)),
				new LimitsViewModel("Winrate limits",
					new DoubleLimitsEditor(_settingsWrapper.SettingChangedSubject, _settingsWrapper.CurrentSettings.WinRateLimits)),
				new LimitsViewModel("Avg frags limits",
					new DoubleLimitsEditor(_settingsWrapper.SettingChangedSubject, _settingsWrapper.CurrentSettings.AvgFragsLimits))
			};

			AvgXpViewModels = new ObservableCollection<LimitsViewModel>(_settingsWrapper.CurrentSettings.AvgXpLimits.Select(
				l => new LimitsViewModel($"Avg Xp (Tier {l.ShipTier.ToString()})",
					new DoubleLimitsEditor(_settingsWrapper.SettingChangedSubject, l.Values))));

			AvgDmgBattleshipViewModels = new ObservableCollection<LimitsViewModel>(
				_settingsWrapper.CurrentSettings.AvgDmgLimits.Battleship.Select(l => new LimitsViewModel(
					$"Avg Dmg (Tier {l.ShipTier.ToString()})",
					new DoubleLimitsEditor(_settingsWrapper.SettingChangedSubject, l.Values))));
			AvgDmgCruiserViewModels = new ObservableCollection<LimitsViewModel>(
				_settingsWrapper.CurrentSettings.AvgDmgLimits.Cruiser.Select(l => new LimitsViewModel(
					$"Avg Dmg (Tier {l.ShipTier.ToString()})",
					new DoubleLimitsEditor(_settingsWrapper.SettingChangedSubject, l.Values))));
			AvgDmgDestroyerViewModels = new ObservableCollection<LimitsViewModel>(
				_settingsWrapper.CurrentSettings.AvgDmgLimits.Destroyer.Select(l => new LimitsViewModel(
					$"Avg Dmg (Tier {l.ShipTier.ToString()})",
					new DoubleLimitsEditor(_settingsWrapper.SettingChangedSubject, l.Values))));
			AvgDmgAirCarrierViewModels = new ObservableCollection<LimitsViewModel>(
				_settingsWrapper.CurrentSettings.AvgDmgLimits.AirCarrier.Select(l => new LimitsViewModel(
					$"Avg Dmg (Tier {l.ShipTier.ToString()})",
					new DoubleLimitsEditor(_settingsWrapper.SettingChangedSubject, l.Values))));

			WeightsViewModel =
				new WeightsViewModel(new WeightsEditor(_settingsWrapper.SettingChangedSubject, _settingsWrapper.CurrentSettings));

			SetTextboxState(!_automaticLimitsSync);
		}

		public SettingsWindowViewModel()
		{
		}

		public RelayCommand ResetCommand { get; set; }
		public RelayCommand ExportCommand { get; set; }
		public RelayCommand ImportCommand { get; set; }

		public ObservableCollection<int> FontSizes { get; } = new ObservableCollection<int> { 8, 9, 10, 11, 12, 13, 14 };

		public ColorsViewModel ColorsViewModel { get; }
		public ObservableCollection<LimitsViewModel> StaticViewModels { get; }
		public ObservableCollection<LimitsViewModel> AvgXpViewModels { get; }
		public ObservableCollection<LimitsViewModel> AvgDmgBattleshipViewModels { get; }
		public ObservableCollection<LimitsViewModel> AvgDmgCruiserViewModels { get; }
		public ObservableCollection<LimitsViewModel> AvgDmgDestroyerViewModels { get; }
		public ObservableCollection<LimitsViewModel> AvgDmgAirCarrierViewModels { get; }

		public WeightsViewModel WeightsViewModel { get; }

		public int FontSize
		{
			get => _fontSize;
			set
			{
				var oldValue = _fontSize;
				_fontSize = value;
				_settingsWrapper.CurrentSettings.FontSize = value;
				_settingsWrapper.SettingChangedSubject.OnNext(new ChangedSetting(oldValue, _fontSize));
				FirePropertyChanged();
			}
		}

		public bool HideLowBattles
		{
			get => _hideLowBattles;
			set
			{
				var oldValue = _hideLowBattles;
				_hideLowBattles = value;
				_settingsWrapper.CurrentSettings.HideLowBattles = value;
				_settingsWrapper.SettingChangedSubject.OnNext(new ChangedSetting(oldValue, _hideLowBattles));
				FirePropertyChanged();
			}
		}

		public bool AutomaticLimitsSync
		{
			get => _automaticLimitsSync;
			set
			{
				var oldValue = _automaticLimitsSync;
				_automaticLimitsSync = value;
				_settingsWrapper.CurrentSettings.AutomaticLimitsSync = value;
				_settingsWrapper.SettingChangedSubject.OnNext(new ChangedSetting(oldValue, _automaticLimitsSync, nameof(SettingsJson.AutomaticLimitsSync)));
				SetTextboxState(!value);
				FirePropertyChanged();
			}
		}

		public bool AutomaticAppUpdate
		{
			get => _automaticAppUpdate;
			set
			{
				var oldValue = _automaticAppUpdate;
				_automaticAppUpdate = value;
				_settingsWrapper.CurrentSettings.AutomaticAppUpdate = value;
				_settingsWrapper.SettingChangedSubject.OnNext(new ChangedSetting(oldValue, _automaticAppUpdate, nameof(SettingsJson.AutomaticAppUpdate)));
				FirePropertyChanged();
			}
		}

		private async void Import()
		{
			var ofd = new OpenFileDialog
			{
				FileName = "settings.export.json",
				DefaultExt = ".json",
				Filter = "JSON Documents (.json)|*.json",
				CheckFileExists = true
			};
			if (ofd.ShowDialog() != true) return;
			await _settingsWrapper.ImportUiSettings(ofd.FileName);
			LoadValues();
		}

		private async void Export()
		{
			var sfd = new SaveFileDialog
			{
				FileName = "settings.export",
				DefaultExt = ".json",
				Filter = "JSON Documents (.json)|*.json"
			};
			if (sfd.ShowDialog() == true)
				await _settingsWrapper.ExportSettings(sfd.FileName);
		}

		private async void Reset()
		{
			var result = MessageBox.Show("Are you sure you want to reset all Settings?", "Reset Settings",
				MessageBoxButton.YesNo);
			if (result != MessageBoxResult.Yes) return;
			await _settingsWrapper.SyncWithRemoteSettings();
			LoadValues();
		}

		private void LoadValues()
		{
			FontSize = _settingsWrapper.CurrentSettings.FontSize;
			HideLowBattles = _settingsWrapper.CurrentSettings.HideLowBattles;
			AutomaticLimitsSync = _settingsWrapper.CurrentSettings.AutomaticLimitsSync;
			ColorsViewModel.LoadValues();
			WeightsViewModel.LoadValues();

			foreach (var viewModel in StaticViewModels
				.Concat(AvgXpViewModels)
				.Concat(AvgDmgBattleshipViewModels)
				.Concat(AvgDmgCruiserViewModels)
				.Concat(AvgDmgDestroyerViewModels)
				.Concat(AvgDmgAirCarrierViewModels))
				viewModel.LoadValues();
		}

		private void SetTextboxState(bool enabled)
		{
			foreach (var viewModel in StaticViewModels
				.Concat(AvgXpViewModels)
				.Concat(AvgDmgBattleshipViewModels)
				.Concat(AvgDmgCruiserViewModels)
				.Concat(AvgDmgDestroyerViewModels)
				.Concat(AvgDmgAirCarrierViewModels))
				viewModel.TextboxEnabled = enabled;
		}
	}
}