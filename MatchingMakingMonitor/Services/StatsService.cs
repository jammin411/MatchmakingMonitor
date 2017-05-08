using MatchMakingMonitor.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using MatchMakingMonitor.Models.Replay;

namespace MatchMakingMonitor.Services
{
	public class StatsService
	{
		private Replay _currentReplay;
		private string _currentRegion;

		private readonly ILogger _logger;
		private readonly ApiService _apiService;
		private readonly Settings _settings;

		private readonly BehaviorSubject<StatsStatus> _statsStatusChangedSubject;
		public IObservable<StatsStatus> StatsStatusChanged => _statsStatusChangedSubject.AsObservable();

		private readonly BehaviorSubject<List<DisplayPlayerStats>> _statsSubject;
		public IObservable<List<DisplayPlayerStats>> Stats => _statsSubject.Where(s => s != null).AsObservable();

		public StatsService(ILogger logger, Settings settings, WatcherService watcherService, ApiService apiService)
		{
			_logger = logger;
			_apiService = apiService;
			_settings = settings;

			_statsStatusChangedSubject = new BehaviorSubject<StatsStatus>(StatsStatus.Waiting);
			_statsSubject = new BehaviorSubject<List<DisplayPlayerStats>>(null);

			watcherService.MatchFound.Subscribe(path =>
			{
				Task.Run(async () =>
				{
					await StatsFound(path);
				});
			});
		}

		private async Task StatsFound(string path)
		{
			Replay replay = null;
			var sr = new StreamReader(path);

			try
			{
				// ReSharper disable once AccessToDisposedClosure
				replay = await Task.Run(async () => JsonConvert.DeserializeObject<Replay>(await sr.ReadToEndAsync(), new IsoDateTimeConverter()));
			}
			catch (Exception e)
			{
				_logger.Error("Error while reading replay file", e);
			}
			sr.Dispose();

			if (replay != null)
			{
				var region = _settings.Region;
				if (_currentReplay == null || region != _currentRegion ||
				    (_currentReplay != null && replay.dateTime > _currentReplay.dateTime))
				{
					_logger.Info("Valid replay found. Fetching stats");
					_currentReplay = replay;
					_currentRegion = region;
					_statsStatusChangedSubject.OnNext(StatsStatus.Fetching);
					var players = (await _apiService.Players(_currentReplay)).ToArray();
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
			return await Task.Run(() => players.Select(p => new DisplayPlayerStats(_settings, p)).ToList());
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
