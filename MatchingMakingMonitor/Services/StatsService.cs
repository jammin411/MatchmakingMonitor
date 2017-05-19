using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using MatchMakingMonitor.config;
using MatchMakingMonitor.Models;
using MatchMakingMonitor.Models.Replay;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MatchMakingMonitor.Services
{
	public class StatsService
	{
		private readonly ApiService _apiService;

		private readonly ILogger _logger;
		private readonly SettingsWrapper _settingsWrapper;

		private readonly BehaviorSubject<StatsStatus> _statsStatusChangedSubject;

		private readonly BehaviorSubject<List<DisplayPlayerStats>> _statsSubject;
		private Region _currentRegion;
		private Replay _currentReplay;

		public StatsService(ILogger logger, SettingsWrapper settingsWrapper, WatcherService watcherService,
			ApiService apiService)
		{
			_logger = logger;
			_apiService = apiService;
			_settingsWrapper = settingsWrapper;

			_statsStatusChangedSubject = new BehaviorSubject<StatsStatus>(StatsStatus.Waiting);
			_statsSubject = new BehaviorSubject<List<DisplayPlayerStats>>(null);

			watcherService.MatchFound.Where(path => path != null)
				.Subscribe(path => { Task.Run(async () => { await StatsFound(path); }); });
		}

		public IObservable<StatsStatus> StatsStatusChanged => _statsStatusChangedSubject.AsObservable();
		public IObservable<List<DisplayPlayerStats>> Stats => _statsSubject.Where(s => s != null).AsObservable();

		private async Task StatsFound(string path)
		{
			Replay replay = null;
			var sr = new StreamReader(path);

			try
			{
				// ReSharper disable once AccessToDisposedClosure
				replay = await Task.Run(
					async () => JsonConvert.DeserializeObject<Replay>(await sr.ReadToEndAsync(), new IsoDateTimeConverter()));
			}
			catch (Exception e)
			{
				_logger.Error("Error while reading replay file", e);
			}
			sr.Dispose();

			if (replay != null)
			{
				var region = _settingsWrapper.CurrentSettings.Region;
				if (_currentReplay == null || region != _currentRegion ||
						_currentReplay != null && replay.DateTime > _currentReplay.DateTime)
				{
					_logger.Info("Valid replay found. Fetching stats");
					_currentReplay = replay;
					_currentRegion = region;
					_statsStatusChangedSubject.OnNext(StatsStatus.Fetching);
					var players = (await _apiService.Players(_currentReplay)).OrderByDescending(p => p.ShipType).ThenByDescending(p => p.ShipTier).ToArray();
					if (players.Count(p => p.AccountId != 0) > 6)
					{
						try
						{
							var stats = await ComputeDisplayPlayer(players);
							_statsStatusChangedSubject.OnNext(StatsStatus.Fetched);
							_statsSubject.OnNext(stats);
						}
						catch (Exception e)
						{
							_logger.Error("Exception occured when computing player for display", e);
							_statsStatusChangedSubject.OnNext(StatsStatus.Waiting);
						}
					}
					else
					{
						_logger.Info("Less than 6 players with stats found. Something seems to be wrong");
						_statsStatusChangedSubject.OnNext(StatsStatus.WrongRegion);
					}
				}
				else
				{
					_logger.Info("Replay was already shown");
				}
			}
		}

		private async Task<List<DisplayPlayerStats>> ComputeDisplayPlayer(IEnumerable<PlayerShip> players)
		{
			_logger.Info("Computing UI for players");
			return await Task.Run(() => players.Select(p => new DisplayPlayerStats(_settingsWrapper, p)).ToList());
		}
	}


	public enum StatsStatus
	{
		Waiting,
		Fetching,
		Fetched,
		WrongRegion
	}
}