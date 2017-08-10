using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Media;
using MatchmakingMonitor.config;
using MatchmakingMonitor.Services;
using MatchMakingMonitor.config;
using MatchMakingMonitor.config.Reflection;
using MatchMakingMonitor.config.warshipsToday;
using MatchMakingMonitor.SocketIO;
using MatchMakingMonitor.View.Util;
using Newtonsoft.Json;

namespace MatchMakingMonitor.Services
{
	public class SettingsWrapper : ViewModelBase
	{
		private static readonly string SettingsPath =
			Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config", "settings.json");

		private static readonly Type SettingsType = typeof(SettingsJson);

		private readonly ILogger _logger;

		private readonly Subject<object> _saveQueueSubject;
		private readonly SocketIoService _socketIoService;

		private readonly Subject<ChangedSetting> _uiSettingsChangedSubject;
		public readonly Subject<ChangedSetting> SettingChangedSubject;

		public SettingsWrapper(ILogger logger, SocketIoService socketIoService)
		{
			_logger = logger;
			_socketIoService = socketIoService;

			Init();

			socketIoService.Hub.OnSettingsRequested.Subscribe(_ => { SettingsRequested(); });

			_saveQueueSubject = new Subject<object>();

			_saveQueueSubject.Throttle(TimeSpan.FromSeconds(10)).Subscribe(async _ => { await InternalSaveAsync(); });

			SettingChangedSubject = new Subject<ChangedSetting>();
			SettingChangedSubject.Where(setting => setting?.Key != null && setting.HasChanged)
				.Do(setting => _logger.Info($"Setting ({setting.Key}) changed from '{setting.OldValue}' to '{setting.NewValue}'"))
				.Throttle(TimeSpan.FromSeconds(2))
				.Subscribe(key => Save());

			_uiSettingsChangedSubject = new Subject<ChangedSetting>();

			var uiSettingsChangedInternal = SettingChanged("UISetting")
				.Throttle(TimeSpan.FromMilliseconds(500));

			uiSettingsChangedInternal.Subscribe(changedSetting =>
			{
				SetBrushes();
				SettingsRequested();
				_uiSettingsChangedSubject.OnNext(changedSetting);
			});
			SetBrushes();
		}

		public SettingsJson CurrentSettings { get; private set; }

		public string AppId => CurrentSettings.AppIds.Single(appId => appId.Region == CurrentSettings.Region).Id;
		public string BaseUrl => CurrentSettings.BaseUrls.Single(baseUrl => baseUrl.Region == CurrentSettings.Region).Url;

		public IObservable<ChangedSetting> UiSettingsChanged => _uiSettingsChangedSubject.AsObservable();

		public SolidColorBrush[] Brushes { get; private set; }

		private void SettingsRequested()
		{
			var export = new Dictionary<string, object>
			{
				{FirstLetterToLower(nameof(SettingsJson.Colors)), CurrentSettings.Colors},
				{FirstLetterToLower(nameof(SettingsJson.HideLowBattles)), CurrentSettings.HideLowBattles},
				{FirstLetterToLower(nameof(SettingsJson.Region)), CurrentSettings.Region}
			};
			_socketIoService.Hub.SendSettings(export);
		}

		private async void Init()
		{
			if (!File.Exists(SettingsPath))
				WriteDefaults();
			FromJson();
			try
			{
				var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
				var settingsVersion = new Version(CurrentSettings.Version);
				var versionCheck = CheckVersion(currentVersion, settingsVersion);

				if (versionCheck == 1)
					if (settingsVersion.CompareTo(UpgradeSettings.InitialVersion) == -1)
					{
						WriteDefaults();
					}
					else
					{
						UpgradeSettings.Upgrade(CurrentSettings, settingsVersion);
						CurrentSettings.Version = currentVersion.ToString();
						await InternalSaveAsync();
					}
			}
			catch (Exception e)
			{
				_logger.Error("Error occured while checking versions", e);
				WriteDefaults();
			}
			if (CurrentSettings.AutomaticLimitsSync)
				await SyncWithRemoteSettings();
			if (string.IsNullOrEmpty(CurrentSettings.Token))
				CurrentSettings.Token = Guid.NewGuid().ToString();
			InternalSave();
			_socketIoService.StateChanged.Subscribe(state =>
			{
				if (state == ConnectionState.Connected)
					_socketIoService.Hub.SetToken(CurrentSettings.Token);
			});
		}

		private int CheckVersion(Version currentVersion, Version settingsVersion)
		{
			var result = currentVersion.CompareTo(settingsVersion);
			if (result != 0)
				_logger.Info("Settings are not matching the current version");
			return result;
		}

		private static void WriteDefaults()
		{
			// ReSharper disable once AssignNullToNotNullAttribute
			Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath));
			using (var f = File.CreateText(SettingsPath))
			{
				f.Write(Defaults());
			}
		}

		private static string Defaults()
		{
			var stream = Assembly.GetExecutingAssembly()
				.GetManifestResourceStream("MatchmakingMonitor.config.settings.default.json");
			var result = string.Empty;
			if (stream == null) return result;
			using (var sr = new StreamReader(stream))
			{
				result = sr.ReadToEnd();
			}
			return result.Replace("{{versionPlaceholder}}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
		}

		private void FromJson()
		{
			CurrentSettings =
				JsonConvert.DeserializeObject<SettingsJson>(File.ReadAllText(SettingsPath), JsonSerializerSettings);
		}

		public async Task ExportSettings(string path)
		{
			var exportJson = await Task.Run(() => JsonConvert.SerializeObject(GetExportSettings(), JsonSerializerSettings));

			using (var f = File.CreateText(path))
			{
				await f.WriteAsync(exportJson);
			}
		}

		private Dictionary<string, object> GetExportSettings()
		{
			var export = new Dictionary<string, object>();
			foreach (var field in GetExportSettingsProperties(SettingsType))
				export.Add(FirstLetterToLower(field.Name), field.GetValue(CurrentSettings));

			return export;
		}

		public async Task ImportUiSettings(string path)
		{
			if (File.Exists(path))
				try
				{
					var importJson = File.ReadAllText(path);
					var import = await Task.Run(() => JsonConvert.DeserializeObject<SettingsJson>(importJson, JsonSerializerSettings));
					await Task.Run(() => CopyUiSettings(CurrentSettings, import));
					await InternalSaveAsync();
				}
				catch (Exception e)
				{
					_logger.Error("Error during import", e);
				}
		}

		public async Task SyncWithRemoteSettings()
		{
			var sourceSettings = await Task.Run(() => JsonConvert.DeserializeObject<SettingsJson>(Defaults()));
			if (await RemoteStats.Get(sourceSettings, _logger))
				CopyUiSettings(CurrentSettings, sourceSettings);
		}

		public void Save()
		{
			_saveQueueSubject.OnNext(null);
		}

		private void InternalSave()
		{
			try
			{
				File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(CurrentSettings, JsonSerializerSettings));
			}
			catch (Exception e)
			{
				_logger.Error("Exception occured while saving settings to file", e);
			}
		}

		private async Task InternalSaveAsync()
		{
			await Task.Run(() => { InternalSave(); });
		}

		public IObservable<ChangedSetting> SettingChanged(string key, bool initial = true)
		{
			var obs = SettingChangedSubject.AsObservable().Where(s => s != null && (s.Key == key && s.HasChanged || s.Initial));
			if (initial)
				SettingChangedSubject.OnNext(new ChangedSetting(null, null, key) {Initial = true});
			return obs;
		}

		[SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
		private static void CopyUiSettings(SettingsJson targetSettings, SettingsJson sourceSettings = null)
		{
			if (sourceSettings == null)
				sourceSettings = JsonConvert.DeserializeObject<SettingsJson>(Defaults(), JsonSerializerSettings);

			if (sourceSettings.FontSize != 0)
				targetSettings.FontSize = sourceSettings.FontSize;

			if (sourceSettings.Colors != null && sourceSettings.Colors.Length == targetSettings.Colors.Length)
				for (var i = 0; i < sourceSettings.Colors.Length; i++)
					targetSettings.Colors[i] = sourceSettings.Colors[i];

			for (var i = 0; i < sourceSettings.BattleLimits.Length; i++)
				targetSettings.BattleLimits[i] = sourceSettings.BattleLimits[i];

			for (var i = 0; i < sourceSettings.AvgFragsLimits.Length; i++)
				targetSettings.AvgFragsLimits[i] = sourceSettings.AvgFragsLimits[i];

			for (var i = 0; i < sourceSettings.WinRateLimits.Length; i++)
				targetSettings.WinRateLimits[i] = sourceSettings.WinRateLimits[i];

			for (var i = 0; i < sourceSettings.AvgXpLimits.Length; i++)
			for (var x = 0; x < sourceSettings.AvgXpLimits[i].Values.Length; x++)
				targetSettings.AvgXpLimits[i].Values[x] = sourceSettings.AvgXpLimits[i].Values[x];

			for (var i = 0; i < sourceSettings.AvgDmgLimits.Battleship.Length; i++)
			for (var x = 0; x < sourceSettings.AvgDmgLimits.Battleship[i].Values.Length; x++)
				targetSettings.AvgDmgLimits.Battleship[i].Values[x] = sourceSettings.AvgDmgLimits.Battleship[i].Values[x];
			for (var i = 0; i < sourceSettings.AvgDmgLimits.Cruiser.Length; i++)
			for (var x = 0; x < sourceSettings.AvgDmgLimits.Cruiser[i].Values.Length; x++)
				targetSettings.AvgDmgLimits.Cruiser[i].Values[x] = sourceSettings.AvgDmgLimits.Cruiser[i].Values[x];
			for (var i = 0; i < sourceSettings.AvgDmgLimits.Destroyer.Length; i++)
			for (var x = 0; x < sourceSettings.AvgDmgLimits.Destroyer[i].Values.Length; x++)
				targetSettings.AvgDmgLimits.Destroyer[i].Values[x] = sourceSettings.AvgDmgLimits.Destroyer[i].Values[x];
			for (var i = 0; i < sourceSettings.AvgDmgLimits.AirCarrier.Length; i++)
			for (var x = 0; x < sourceSettings.AvgDmgLimits.AirCarrier[i].Values.Length; x++)
				targetSettings.AvgDmgLimits.AirCarrier[i].Values[x] = sourceSettings.AvgDmgLimits.AirCarrier[i].Values[x];

			if (sourceSettings.BattleWeight > 0)
				targetSettings.BattleWeight = sourceSettings.BattleWeight;
			if (sourceSettings.AvgDmgWeight > 0)
				targetSettings.AvgDmgWeight = sourceSettings.AvgDmgWeight;
			if (sourceSettings.AvgFragsWeight > 0)
				targetSettings.AvgFragsWeight = sourceSettings.AvgFragsWeight;
			if (sourceSettings.AvgXpWeight > 0)
				targetSettings.AvgXpWeight = sourceSettings.AvgXpWeight;
			if (sourceSettings.AvgFragsWeight > 0)
				targetSettings.AvgFragsWeight = sourceSettings.AvgFragsWeight;
		}

		private static IEnumerable<FieldInfo> GetExportSettingsProperties(Type type)
		{
			return type.GetFields().Where(field => field.GetCustomAttribute(typeof(ExportSettingAttribute)) != null);
		}

		private void SetBrushes()
		{
			Brushes = CurrentSettings.Colors.Select(colorValue =>
			{
				var convertFromString = ColorConverter.ConvertFromString(colorValue);
				if (convertFromString == null) return System.Windows.Media.Brushes.Black;
				var brush = new SolidColorBrush((Color) convertFromString);
				brush.Freeze();
				return brush;
			}).ToArray();
		}

		private static string FirstLetterToLower(string str)
		{
			if (str == null)
				return null;

			if (str.Length > 1)
				return char.ToLower(str[0]) + str.Substring(1);

			return str.ToLower();
		}

		public void SetLastWindowProperties(LastWindowProperties lastWindowProperties)
		{
			CurrentSettings.LastWindowProperties = lastWindowProperties;
			InternalSave();
		}
	}
}