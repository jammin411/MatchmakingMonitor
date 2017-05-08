using MatchMakingMonitor.config;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Media;
using MatchMakingMonitor.View.Util;

namespace MatchMakingMonitor.Services
{
	public class Settings : BaseViewBinding
	{
		private static Settings _instance;

		private static readonly string SettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "settings.json");

		private SettingsJson _currentSettings;

		#region Keys
		public const string KeyInstallDirectory = nameof(SettingsJson.installDirectory);

		public const string KeyAppIdNa = nameof(SettingsJson.appIdNA);
		public const string KeyAppIdEu = nameof(SettingsJson.appIdEU);
		public const string KeyAppIdRu = nameof(SettingsJson.appIdRU);
		public const string KeyAppIdSea = nameof(SettingsJson.appIdSEA);

		public const string KeyBaseUrlNa = nameof(SettingsJson.baseUrlNA);
		public const string KeyBaseUrlEu = nameof(SettingsJson.baseUrlEU);
		public const string KeyBaseUrlRu = nameof(SettingsJson.baseUrlRU);
		public const string KeyBaseUrlSea = nameof(SettingsJson.baseUrlSEA);

		public const string KeyRegion = nameof(SettingsJson.region);

		public const string KeyToken = nameof(SettingsJson.token);

		public const string KeyColor9 = nameof(SettingsJson.color9);
		public const string KeyColor8 = nameof(SettingsJson.color8);
		public const string KeyColor7 = nameof(SettingsJson.color7);
		public const string KeyColor6 = nameof(SettingsJson.color6);
		public const string KeyColor5 = nameof(SettingsJson.color5);
		public const string KeyColor4 = nameof(SettingsJson.color4);
		public const string KeyColor3 = nameof(SettingsJson.color3);
		public const string KeyColor2 = nameof(SettingsJson.color2);
		public const string KeyColor1 = nameof(SettingsJson.color1);

		public const string KeyBattleLimits = nameof(SettingsJson.battleLimits);
		public const string KeyWinLimits = nameof(SettingsJson.winLimits);
		public const string KeyFragsLimits = nameof(SettingsJson.fragsLimits);
		public const string KeyXpLimits = nameof(SettingsJson.xpLimits);
		public const string KeyDmgLimits = nameof(SettingsJson.dmgLimits);

		public const string KeyFontSize = nameof(SettingsJson.fontSize);

		public const string KeyBattleWeight = nameof(SettingsJson.battleWeight);
		public const string KeyWinWeight = nameof(SettingsJson.winWeight);
		public const string KeyFragsWeight = nameof(SettingsJson.fragsWeight);
		public const string KeyXpWeight = nameof(SettingsJson.xpWeight);
		public const string KeyDmgWeight = nameof(SettingsJson.dmgWeight);

		public static string[] KeysColors = new string[] { KeyColor9, KeyColor8, KeyColor7, KeyColor6, KeyColor5, KeyColor4, KeyColor3, KeyColor2, KeyColor1 };
		public static string[] KeysLimits = new string[] { KeyBattleLimits, KeyWinLimits, KeyFragsLimits, KeyXpLimits, KeyDmgLimits };
		public static string[] KeysWeights = new string[] { KeyBattleWeight, KeyWinWeight, KeyFragsWeight, KeyXpWeight, KeyDmgWeight };
		public static string[] KeysOthers = new string[] { KeyFontSize };
		public static string[] KeysUiSettings = KeysColors.Concat(KeysLimits).Concat(KeysWeights).ToArray();

		#endregion

		#region Settings
		public string InstallDirectory
		{
			get => _instance.Get<string>(KeyInstallDirectory);
			set => _instance.Set(KeyInstallDirectory, value);
		}

		public string AppIdNa
		{
			get => _instance.Get<string>(KeyAppIdNa);
			set => _instance.Set(KeyAppIdNa, value);
		}
		public string AppIdEu
		{
			get => _instance.Get<string>(KeyAppIdEu);
			set => _instance.Set(KeyAppIdEu, value);
		}
		public string AppIdRu
		{
			get => _instance.Get<string>(KeyAppIdRu);
			set => _instance.Set(KeyAppIdRu, value);
		}
		public string AppIdSea
		{
			get => _instance.Get<string>(KeyAppIdSea);
			set => _instance.Set(KeyAppIdSea, value);
		}

		public string BaseUrlNa
		{
			get => _instance.Get<string>(KeyBaseUrlNa);
			set => _instance.Set(KeyBaseUrlNa, value);
		}
		public string BaseUrlEu
		{
			get => _instance.Get<string>(KeyBaseUrlEu);
			set => _instance.Set(KeyBaseUrlEu, value);
		}
		public string BaseUrlRu
		{
			get => _instance.Get<string>(KeyBaseUrlRu);
			set => _instance.Set(KeyBaseUrlRu, value);
		}
		public string BaseUrlSea
		{
			get => _instance.Get<string>(KeyBaseUrlSea);
			set => _instance.Set(KeyBaseUrlSea, value);
		}

		public string Region
		{
			get => _instance.Get<string>(KeyRegion);
			set => _instance.Set(KeyRegion, value);
		}

		public string Color9
		{
			get => _instance.Get<string>(KeyColor9);
			set { _instance.Set(KeyColor9, value); FirePropertyChanged(); }
		}
		public string Color8
		{
			get => _instance.Get<string>(KeyColor8);
			set { _instance.Set(KeyColor8, value); FirePropertyChanged(); }
		}
		public string Color7
		{
			get => _instance.Get<string>(KeyColor7);
			set { _instance.Set(KeyColor7, value); FirePropertyChanged(); }
		}
		public string Color6
		{
			get => _instance.Get<string>(KeyColor6);
			set { _instance.Set(KeyColor6, value); FirePropertyChanged(); }
		}
		public string Color5
		{
			get => _instance.Get<string>(KeyColor5);
			set { _instance.Set(KeyColor5, value); FirePropertyChanged(); }
		}
		public string Color4
		{
			get => _instance.Get<string>(KeyColor4);
			set { _instance.Set(KeyColor4, value); FirePropertyChanged(); }
		}
		public string Color3
		{
			get => _instance.Get<string>(KeyColor3);
			set { _instance.Set(KeyColor3, value); FirePropertyChanged(); }
		}
		public string Color2
		{
			get => _instance.Get<string>(KeyColor2);
			set { _instance.Set(KeyColor2, value); FirePropertyChanged(); }
		}
		public string Color1
		{
			get => _instance.Get<string>(KeyColor1);
			set { _instance.Set(KeyColor1, value); FirePropertyChanged(); }
		}

		public int FontSize
		{
			get => _instance.Get<int>(KeyFontSize);
			set { _instance.Set(KeyFontSize, value); FirePropertyChanged(); }
		}

		#region Limits
		private ObservableCollection<double> _battleLimits;
		public ObservableCollection<double> BattleLimits
		{
			get => _battleLimits ?? (_battleLimits =
							 new ObservableCollection<double>(_instance.Get<double[]>(KeyBattleLimits)));
			set
			{
				_battleLimits = null;
				_instance.Set(KeyBattleLimits, value);
			}
		}

		public void BattleLimitsChanged()
		{
			_instance.Set(KeyBattleLimits, _battleLimits.ToArray(), true);
		}

		private ObservableCollection<double> _winLimits;
		public ObservableCollection<double> WinLimits
		{
			get => _winLimits ?? (_winLimits = new ObservableCollection<double>(_instance.Get<double[]>(KeyWinLimits)));
			set
			{
				_winLimits = null;
				_instance.Set(KeyWinLimits, value);
			}
		}

		public void WinLimitsChanged()
		{
			_instance.Set(KeyWinLimits, _winLimits.ToArray(), true);
		}

		private ObservableCollection<double> _fragsLimits;
		public ObservableCollection<double> FragsLimits
		{
			get => _fragsLimits ?? (_fragsLimits = new ObservableCollection<double>(_instance.Get<double[]>(KeyFragsLimits)));
			set
			{
				_fragsLimits = null;
				_instance.Set(KeyFragsLimits, value);
			}
		}

		public void FragsLimitsChanged()
		{
			_instance.Set(KeyFragsLimits, _fragsLimits.ToArray(), true);
		}

		private ObservableCollection<double> _xpLimits;
		public ObservableCollection<double> XpLimits
		{
			get => _xpLimits ?? (_xpLimits = new ObservableCollection<double>(_instance.Get<double[]>(KeyXpLimits)));
			set
			{
				_xpLimits = null;
				_instance.Set(KeyXpLimits, value);
			}
		}

		public void XpLimitsChanged()
		{
			_instance.Set(KeyXpLimits, _xpLimits.ToArray(), true);
		}

		private ObservableCollection<double> _dmgLimits;
		public ObservableCollection<double> DmgLimits
		{
			get => _dmgLimits ?? (_dmgLimits = new ObservableCollection<double>(_instance.Get<double[]>(KeyDmgLimits)));
			set
			{
				_dmgLimits = null;
				_instance.Set(KeyDmgLimits, value);
			}
		}

		public void DmgLimitsChanged()
		{
			_instance.Set(KeyDmgLimits, _dmgLimits.ToArray(), true);
		}

		public double BattleWeight
		{
			get => _instance.Get<double>(KeyBattleWeight);
			set => _instance.Set(KeyBattleWeight, value);
		}
		public double FragsWeight
		{
			get => _instance.Get<double>(KeyFragsWeight);
			set => _instance.Set(KeyFragsWeight, value);
		}
		public double XpWeight
		{
			get => _instance.Get<double>(KeyXpWeight);
			set => _instance.Set(KeyXpWeight, value);
		}
		public double DmgWeight
		{
			get => _instance.Get<double>(KeyDmgWeight);
			set => _instance.Set(KeyDmgWeight, value);
		}
		public double WinWeight
		{
			get => _instance.Get<double>(KeyWinWeight);
			set => _instance.Set(KeyWinWeight, value);
		}
		#endregion

		public string Token
		{
			get => _instance.Get<string>(KeyToken);
			set => _instance.Set(KeyToken, value);
		}

		#endregion

		private readonly Subject<string> _uiSettingsChangedSubject;
		public IObservable<string> UiSettingsChanged => _uiSettingsChangedSubject.AsObservable();

		private readonly ILogger _logger;
		private readonly BehaviorSubject<string> _settingChangedSubject;
		public Settings(ILogger logger)
		{
			_instance = this;
			_logger = logger;

			Init();

			_settingChangedSubject = new BehaviorSubject<string>(string.Empty);
			_settingChangedSubject.Where(key => key != null)
				.Do(key => _logger.Info($"Setting ({key}) changed"))
				.Throttle(TimeSpan.FromSeconds(2))
				.Subscribe(async key =>
			{
				await Save();
			});

			_uiSettingsChangedSubject = new Subject<string>();

			var uiSettingsChangedInternal = KeysUiSettings.Select(key => SettingChanged(key, false)).Merge().Throttle(TimeSpan.FromMilliseconds(500));

			uiSettingsChangedInternal.Subscribe(key =>
			{
				SetBrushes();
				_uiSettingsChangedSubject.OnNext(key);
			});
			SetBrushes();

			Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(BattleLimits, "CollectionChanged").Throttle(TimeSpan.FromMilliseconds(500)).Subscribe(e => BattleLimitsChanged());
			Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(WinLimits, "CollectionChanged").Throttle(TimeSpan.FromMilliseconds(500)).Subscribe(e => WinLimitsChanged());
			Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(XpLimits, "CollectionChanged").Throttle(TimeSpan.FromMilliseconds(500)).Subscribe(e => XpLimitsChanged());
			Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(DmgLimits, "CollectionChanged").Throttle(TimeSpan.FromMilliseconds(500)).Subscribe(e => DmgLimitsChanged());
			Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(FragsLimits, "CollectionChanged").Throttle(TimeSpan.FromMilliseconds(500)).Subscribe(e => FragsLimitsChanged());
		}

		private void Init()
		{
			if (!File.Exists(SettingsPath))
			{
				// ReSharper disable once AssignNullToNotNullAttribute
				Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));
				using (var f = File.CreateText(SettingsPath))
				{
					f.Write(Defaults());
				}
			}
			FromJson();
		}

		private static string Defaults()
		{
			var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MatchMakingMonitor.config.settings.default.json");
			var result = string.Empty;
			if (stream == null) return result;
			using (var sr = new StreamReader(stream))
			{
				result = sr.ReadToEnd();
			}
			return result;
		}

		private void FromJson()
		{
			_currentSettings = JsonConvert.DeserializeObject<SettingsJson>(File.ReadAllText(SettingsPath));
		}

		public async Task ExportUiSettings(string path)
		{
			var export = KeysUiSettings.Concat(KeysOthers).ToDictionary(key => key, key => _currentSettings.Get(key));

			var exportJson = await Task.Run(() => JsonConvert.SerializeObject(export));

			using (var f = File.CreateText(path))
			{
				await f.WriteAsync(exportJson);
			}
		}

		public async Task ImportUiSettings(string path)
		{
			if (File.Exists(path))
			{
				try
				{
					var importJson = File.ReadAllText(path);
					var import = await Task.Run(() => JsonConvert.DeserializeObject<SettingsJson>(importJson));
					await ResetUiSettings(import);
				}
				catch (Exception e)
				{
					_logger.Error("Error during import", e);
				}
			}
		}

		public async Task Save()
		{
			try
			{
				await Task.Run(() => { File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(_currentSettings)); });
			}
			catch (Exception e)
			{
				_logger.Error("Exception occured while saving settings to file", e);
			}
		}

		public IObservable<string> SettingChanged(string key, bool initial = true)
		{
			var obs = _settingChangedSubject.AsObservable().Where(k => key == k);
			if (initial)
			{
				_settingChangedSubject.OnNext(key);
			}
			return obs;
		}

		public T Get<T>(string key)
		{
			try
			{
				return _currentSettings.Get<T>(key);
			}
			catch (Exception e)
			{
				_logger.Error($"Exception occured while getting setting '{key}'", e);
				return default(T);
			}
		}

		public void Set<T>(string key, T value, bool forceChanged = false)
		{
			try
			{
				var changed = forceChanged || !value.Equals(Get<T>(key));
				_currentSettings.Set(key, value);
				if (changed) FireSettingChanged(key);
			}
			catch (Exception e)
			{
				_logger.Error($"Exception occured while setting setting '{key}'", e);
			}
		}

		[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
		public async Task ResetUiSettings(SettingsJson sourceSettings = null)
		{
			if (sourceSettings == null) sourceSettings = JsonConvert.DeserializeObject<SettingsJson>(Defaults());
			await Task.Run(() =>
			{
				foreach (var key in KeysColors.Concat(KeysOthers).Concat(KeysWeights))
				{
					_currentSettings.Set(key, sourceSettings.Get(key));
					FirePropertyChanged(FirstLetterToUpper(key));
				}
				foreach (var key in KeysLimits)
				{
					_currentSettings.Set(key, sourceSettings.Get(key));
					var collection = GetLimitCollectionByKey(key);
					if (collection != null)
					{
						var sourceLimit = sourceSettings.Get<double[]>(key);
						if (sourceLimit != null)
						{
							var limit = _currentSettings.Get<double[]>(key);
							for (var i = 0; i < sourceLimit.Length; i++)
							{
								collection[i] = limit[i];
							}
						}
					}
				}
			});
			await Save();
		}

		private void FireSettingChanged(string key)
		{
			_settingChangedSubject.OnNext(key);
		}

		private ObservableCollection<double> GetLimitCollectionByKey(string key)
		{
			switch (key)
			{
				case KeyBattleLimits:
					return _battleLimits;
				case KeyWinLimits:
					return _winLimits;
				case KeyFragsLimits:
					return _fragsLimits;
				case KeyXpLimits:
					return _xpLimits;
				case KeyDmgLimits:
					return _dmgLimits;
				default:
					return null;
			}
		}

		public SolidColorBrush[] Brushes { get; private set; }

		private void SetBrushes()
		{
			Brushes = KeysColors.Select(name =>
			{
				var convertFromString = ColorConverter.ConvertFromString(Get<string>(name));
				if (convertFromString == null) return System.Windows.Media.Brushes.Black;
				var brush = new SolidColorBrush((Color)convertFromString);
				brush.Freeze();
				return brush;
			}).ToArray();
		}

		private static string FirstLetterToUpper(string str)
		{
			if (str == null)
				return null;

			if (str.Length > 1)
				return char.ToUpper(str[0]) + str.Substring(1);

			return str.ToUpper();
		}
	}
}
