using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MatchMakingMonitor.Models;
using MatchMakingMonitor.Services;
using MatchMakingMonitor.SocketIO;
using MatchMakingMonitor.View.Util;

namespace MatchMakingMonitor.View
{
	public class StatsViewModel : ViewModelBase
	{
		private readonly SettingsWrapper _settingsWrapper;

		private double _avgBattlesEnemy;

		private double _avgBattlesFriendly;

		private double _avgWinrateEnemy;

		private double _avgWinrateFriendly;

		private double _avgXpEnemy;

		private double _avgXpFriendly;

		private ObservableCollection<DisplayPlayerStats> _enemyPlayers;

		private int _fontSize;

		private ObservableCollection<DisplayPlayerStats> _friendlyPlayers;


		private Visibility _listVisibility = Visibility.Collapsed;
		private List<DisplayPlayerStats> _stats;

		public StatsViewModel(ILogger logger, StatsService statsService, SettingsWrapper settingsWrapper,
			SocketIoService socketIoService)
		{
			_settingsWrapper = settingsWrapper;

			DetailCommand = new RelayCommand(param => OpenPlayerDetail((string[]) param));

			statsService.Stats.Subscribe(async stats =>
			{
				await Task.Run(() =>
				{
					_stats = stats;
					FriendlyPlayers = new ObservableCollection<DisplayPlayerStats>(stats.Where(p => p.Player.Relation != 2));
					EnemyPlayers = new ObservableCollection<DisplayPlayerStats>(stats.Where(p => p.Player.Relation == 2));
					AvgXpFriendly = TeamAvgXp(FriendlyPlayers);
					AvgXpEnemy = TeamAvgXp(EnemyPlayers);
					AvgWinrateFriendly = TeamAvgWinRate(FriendlyPlayers);
					AvgWinrateEnemy = TeamAvgWinRate(EnemyPlayers);
					AvgBattlesFriendly = TeamAvgBattles(FriendlyPlayers);
					AvgBattlesEnemy = TeamAvgBattles(EnemyPlayers);
					ListVisibility = Visibility.Visible;
				});
			});

			_settingsWrapper.UiSettingsChanged.Subscribe(async key =>
			{
				if (_stats == null) return;
				logger.Info("Re-computing UI for players");
				FontSize = _settingsWrapper.CurrentSettings.FontSize;
				foreach (var player in _stats)
					await Task.Run(() => { player.ComputeUi(); });
				socketIoService.Hub.SendColorKeys(_stats.Select(s => s.GetColorKeys()).ToList());
			});
		}

		public StatsViewModel()
		{
			FriendlyPlayers =
				new ObservableCollection<DisplayPlayerStats>(
					new List<DisplayPlayerStats> {DisplayPlayerStats.MockPlayer(), DisplayPlayerStats.MockPlayer(0)});
			EnemyPlayers =
				new ObservableCollection<DisplayPlayerStats>(
					new List<DisplayPlayerStats> {DisplayPlayerStats.MockPlayer(2), DisplayPlayerStats.MockPlayer(2, true)});
			ListVisibility = Visibility.Visible;
		}

		public RelayCommand DetailCommand { get; set; }

		public ObservableCollection<DisplayPlayerStats> FriendlyPlayers
		{
			get => _friendlyPlayers;
			set
			{
				_friendlyPlayers = value;
				FirePropertyChanged();
			}
		}

		public ObservableCollection<DisplayPlayerStats> EnemyPlayers
		{
			get => _enemyPlayers;
			set
			{
				_enemyPlayers = value;
				FirePropertyChanged();
			}
		}

		public int FontSize
		{
			get => _fontSize;
			set
			{
				_fontSize = value;
				FirePropertyChanged();
			}
		}

		public double AvgXpFriendly
		{
			get => _avgXpFriendly;
			set
			{
				_avgXpFriendly = value;
				FirePropertyChanged();
			}
		}

		public double AvgWinrateFriendly
		{
			get => _avgWinrateFriendly;
			set
			{
				_avgWinrateFriendly = value;
				FirePropertyChanged();
			}
		}

		public double AvgXpEnemy
		{
			get => _avgXpEnemy;
			set
			{
				_avgXpEnemy = value;
				FirePropertyChanged();
			}
		}

		public double AvgWinrateEnemy
		{
			get => _avgWinrateEnemy;
			set
			{
				_avgWinrateEnemy = value;
				FirePropertyChanged();
			}
		}

		public double AvgBattlesFriendly
		{
			get => _avgBattlesFriendly;
			set
			{
				_avgBattlesFriendly = value;
				FirePropertyChanged();
			}
		}

		public double AvgBattlesEnemy
		{
			get => _avgBattlesEnemy;
			set
			{
				_avgBattlesEnemy = value;
				FirePropertyChanged();
			}
		}

		public Visibility ListVisibility
		{
			get => _listVisibility;
			set
			{
				_listVisibility = value;
				FirePropertyChanged();
			}
		}

		private double TeamAvgXp(ObservableCollection<DisplayPlayerStats> players)
		{
			return Math.Round(
				players.Where(p => !double.IsNaN(p.AvgXp)).Select(p => p.AvgXp).Sum() /
				players.Where(p => !double.IsNaN(p.AvgXp)).Count(), 0);
		}

		private double TeamAvgWinRate(ObservableCollection<DisplayPlayerStats> players)
		{
			return Math.Round(
				players.Where(p => !double.IsNaN(p.WinRate)).Select(p => p.WinRate).Sum() /
				players.Where(p => !double.IsNaN(p.WinRate)).Count(), 2);
		}

		private double TeamAvgBattles(ObservableCollection<DisplayPlayerStats> players)
		{
			return Math.Round(
				players.Where(p => !double.IsNaN(p.Player.Battles)).Select(p => p.Player.Battles).Sum() /
				players.Where(p => !double.IsNaN(p.Player.Battles)).Count(), 0);
		}

		private void OpenPlayerDetail(IReadOnlyList<string> param)
		{
			if (param[0] != "0")
				Process.Start($"https://{_settingsWrapper.CurrentSettings.Region}.warships.today/player/{param[0]}/{param[1]}");
		}
	}
}