using System.Globalization;
using System.Windows;
using MatchmakingMonitor.View.Util;

namespace MatchmakingMonitor.View.Settings
{
  public class WeightsViewModel : ViewModelBase
  {
    private readonly WeightsEditor _weightsEditor;
    private double _actualSum;

    private string _avgDmgWeight;

    private string _avgFragsWeight;

    private string _avgXpWeight;

    private string _battleWeight;
    private bool _suppressUpdate;

    private Visibility _validationError;

    private string _validationErrorText;

    private string _winRateWeight;

    public WeightsViewModel(WeightsEditor weightsEditor)
    {
      _weightsEditor = weightsEditor;
      _weightsEditor.RegisterValuesChanged(UpdateValuesFromEditor);
      UpdateValuesFromEditor();
    }

    public string BattleWeight
    {
      get => _battleWeight;
      set
      {
        _battleWeight = value;
        if (!_suppressUpdate)
          ValidationError = _weightsEditor.UpdateBattleWeight(_battleWeight, out _actualSum)
            ? Visibility.Hidden
            : Visibility.Visible;
        FirePropertyChanged();
      }
    }

    public string AvgFragsWeight
    {
      get => _avgFragsWeight;
      set
      {
        _avgFragsWeight = value;
        if (!_suppressUpdate)
          ValidationError = _weightsEditor.UpdateAvgFragsWeight(_avgFragsWeight, out _actualSum)
            ? Visibility.Hidden
            : Visibility.Visible;
        FirePropertyChanged();
      }
    }

    public string AvgXpWeight
    {
      get => _avgXpWeight;
      set
      {
        _avgXpWeight = value;
        if (!_suppressUpdate)
          ValidationError = _weightsEditor.UpdateAvgXpWeight(_avgXpWeight, out _actualSum)
            ? Visibility.Hidden
            : Visibility.Visible;
        FirePropertyChanged();
      }
    }

    public string AvgDmgWeight
    {
      get => _avgDmgWeight;
      set
      {
        _avgDmgWeight = value;
        if (!_suppressUpdate)
          ValidationError = _weightsEditor.UpdateAvgDmgWeight(_avgDmgWeight, out _actualSum)
            ? Visibility.Hidden
            : Visibility.Visible;
        FirePropertyChanged();
      }
    }

    public string WinRateWeight
    {
      get => _winRateWeight;
      set
      {
        _winRateWeight = value;
        if (!_suppressUpdate)
          ValidationError = _weightsEditor.UpdateWinRateWeight(_winRateWeight, out _actualSum)
            ? Visibility.Hidden
            : Visibility.Visible;
        FirePropertyChanged();
      }
    }

    public Visibility ValidationError
    {
      get => _validationError;
      set
      {
        _validationError = value;
        ValidationErrorText = $"Sum must be 5 but is {_actualSum}";
        FirePropertyChanged();
      }
    }

    public string ValidationErrorText
    {
      get => _validationErrorText;
      set
      {
        _validationErrorText = value;
        FirePropertyChanged();
      }
    }

    private void UpdateValuesFromEditor()
    {
      _suppressUpdate = true;
      BattleWeight = _weightsEditor.BattleWeight.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
      AvgFragsWeight = _weightsEditor.AvgFragsWeight.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
      AvgXpWeight = _weightsEditor.AvgXpWeight.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
      AvgDmgWeight = _weightsEditor.AvgDmgWeight.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
      WinRateWeight = _weightsEditor.WinRateWeight.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
      ValidationError = Visibility.Hidden;
      _suppressUpdate = false;
    }

    public void LoadValues()
    {
      _weightsEditor.LoadValues();
    }
  }
}