using System;
using System.Reactive.Subjects;
using MatchmakingMonitor.config;
using MatchmakingMonitor.config.Reflection;

namespace MatchmakingMonitor.View.Settings
{
  public class WeightsEditor
  {
    private readonly Subject<ChangedSetting> _changedSubject;
    private readonly bool _initial;
    private readonly SettingsJson _settings;
    private Action _valuesChanged;

    public WeightsEditor(Subject<ChangedSetting> changedSubject, SettingsJson settings)
    {
      _changedSubject = changedSubject;
      _settings = settings;
      _initial = true;
      LoadValues();
      _initial = false;
    }

    public double BattleWeight { get; private set; }

    public double AvgFragsWeight { get; private set; }

    public double AvgXpWeight { get; private set; }

    public double AvgDmgWeight { get; private set; }

    public double WinRateWeight { get; private set; }

    public void RegisterValuesChanged(Action action)
    {
      _valuesChanged = action;
    }

    public void LoadValues()
    {
      BattleWeight = _settings.BattleWeight;
      AvgFragsWeight = _settings.AvgFragsWeight;
      AvgXpWeight = _settings.AvgXpWeight;
      AvgDmgWeight = _settings.AvgDmgWeight;
      WinRateWeight = _settings.WinRateWeight;
      _valuesChanged?.Invoke();
      if (!_initial)
        _changedSubject.OnNext(new ChangedSetting(true, false));
    }

    private static bool ValidateWeights(double winRateWeight, double battleWeight, double avgFragsWeight,
      double avgXpWeight, double avgDmgWeight, out double actualSum)
    {
      actualSum = 0;
      try
      {
        var sum = Math.Round(winRateWeight + battleWeight + avgFragsWeight + avgXpWeight + avgDmgWeight, 2);
        actualSum = sum;
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        return sum == 5;
      }
      catch
      {
        return false;
      }
    }

    private void SetSettings()
    {
      _settings.BattleWeight = BattleWeight;
      _settings.AvgFragsWeight = AvgFragsWeight;
      _settings.AvgXpWeight = AvgXpWeight;
      _settings.AvgDmgWeight = AvgDmgWeight;
      _settings.WinRateWeight = WinRateWeight;
    }

    public bool UpdateBattleWeight(string value, out double actualSum)
    {
      var oldValue = BattleWeight;
      double newValue;
      actualSum = 0;
      if (!double.TryParse(value, out newValue)) return false;
      BattleWeight = newValue;
      if (!ValidateWeights(WinRateWeight, newValue, AvgFragsWeight, AvgXpWeight, AvgDmgWeight,
        out actualSum)) return false;
      SetSettings();
      _changedSubject.OnNext(new ChangedSetting(oldValue, BattleWeight));
      return true;
    }

    public bool UpdateAvgFragsWeight(string value, out double actualSum)
    {
      var oldValue = AvgFragsWeight;
      double newValue;
      actualSum = 0;
      if (!double.TryParse(value, out newValue)) return false;
      AvgFragsWeight = newValue;
      if (!ValidateWeights(WinRateWeight, BattleWeight, newValue, AvgXpWeight, AvgDmgWeight,
        out actualSum)) return false;
      SetSettings();
      _changedSubject.OnNext(new ChangedSetting(oldValue, AvgFragsWeight));
      return true;
    }

    public bool UpdateAvgXpWeight(string value, out double actualSum)
    {
      var oldValue = AvgXpWeight;
      double newValue;
      actualSum = 0;
      if (!double.TryParse(value, out newValue)) return false;
      AvgXpWeight = newValue;
      if (!ValidateWeights(WinRateWeight, BattleWeight, AvgFragsWeight, newValue, AvgDmgWeight,
        out actualSum)) return false;
      SetSettings();
      _changedSubject.OnNext(new ChangedSetting(oldValue, AvgXpWeight));
      return true;
    }

    public bool UpdateAvgDmgWeight(string value, out double actualSum)
    {
      var oldValue = AvgDmgWeight;
      double newValue;
      actualSum = 0;
      if (!double.TryParse(value, out newValue)) return false;
      AvgDmgWeight = newValue;
      if (!ValidateWeights(WinRateWeight, BattleWeight, AvgFragsWeight, AvgXpWeight, newValue,
        out actualSum)) return false;
      SetSettings();
      _changedSubject.OnNext(new ChangedSetting(oldValue, AvgDmgWeight));
      return true;
    }

    public bool UpdateWinRateWeight(string value, out double actualSum)
    {
      var oldValue = WinRateWeight;
      double newValue;
      actualSum = 0;
      if (!double.TryParse(value, out newValue)) return false;
      WinRateWeight = newValue;
      if (!ValidateWeights(newValue, BattleWeight, AvgFragsWeight, AvgXpWeight, AvgDmgWeight,
        out actualSum)) return false;
      SetSettings();
      _changedSubject.OnNext(new ChangedSetting(oldValue, WinRateWeight));
      return true;
    }
  }
}