using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MatchMakingMonitor.Models;
using MatchMakingMonitor.Services;
using MatchMakingMonitor.View.Util;

namespace MatchMakingMonitor.View
{
	public class StatsViewModel : BaseViewBinding
	{
		public RelayCommand DetailCommand { get; set; }

		private ObservableCollection<DisplayPlayerStats> _friendlyPlayers;
		public ObservableCollection<DisplayPlayerStats> FriendlyPlayers
		{
			get => _friendlyPlayers;
			set
			{
				_friendlyPlayers = value;
				FirePropertyChanged();
			}
		}

		private ObservableCollection<DisplayPlayerStats> _enemyPlayers;
		public ObservableCollection<DisplayPlayerStats> EnemyPlayers
		{
			get => _enemyPlayers;
			set
			{
				_enemyPlayers = value;
				FirePropertyChanged();
			}
		}

		private int _fontSize;

		public int FontSize
		{
			get => _fontSize;
			set
			{
				_fontSize = value;
				FirePropertyChanged();
			}
		}


		private Visibility _listVisibility = Visibility.Collapsed;

		public Visibility ListVisibility
		{
			get => _listVisibility;
			set
			{
				_listVisibility = value;
				FirePropertyChanged();
			}
		}

		private readonly Settings _settings;
		private List<DisplayPlayerStats> _stats;
		public StatsViewModel(ILogger logger, StatsService statsService, Settings settings)
		{
			_settings = settings;

			DetailCommand = new RelayCommand(param => OpenPlayerDetail((string[])param));

			statsService.Stats.Subscribe(async stats =>
			{
				await Task.Run(() =>
				{
					_stats = stats;
					FriendlyPlayers = new ObservableCollection<DisplayPlayerStats>(stats.Where(p => p.Player.Relation != 2));
					EnemyPlayers = new ObservableCollection<DisplayPlayerStats>(stats.Where(p => p.Player.Relation == 2));
					ListVisibility = Visibility.Visible;
				});
			});

			_settings.UiSettingsChanged.Subscribe(async key =>
			{
				if (_stats == null) return;
				logger.Info("Re-computing UI for players");
				FontSize = _settings.FontSize;
				foreach (var player in _stats)
				{
					await Task.Run(() =>
					{
						player.ComputeUi();
					});
				}
			});
		}

		public StatsViewModel()
		{
			FriendlyPlayers = new ObservableCollection<DisplayPlayerStats>(new List<DisplayPlayerStats>() { DisplayPlayerStats.MockPlayer(), DisplayPlayerStats.MockPlayer(0) });
			EnemyPlayers = new ObservableCollection<DisplayPlayerStats>(new List<DisplayPlayerStats>() { DisplayPlayerStats.MockPlayer(2), DisplayPlayerStats.MockPlayer(2, true) });
			ListVisibility = Visibility.Visible;
		}

		private void OpenPlayerDetail(IReadOnlyList<string> param)
		{
			if (param[0] != "0")
			{
				System.Diagnostics.Process.Start($"https://{_settings.Region}.warships.today/player/{param[0]}/{param[1]}");
			}
		}
	}
}
