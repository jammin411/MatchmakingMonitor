using System.Globalization;
using MatchmakingMonitor.View.Util;

namespace MatchmakingMonitor.View.Settings
{
  public class LimitsViewModel : ViewModelBase
  {
    private readonly ILimitsEditor _limitsEditor;
    private bool _suppressUpdate;

    private bool _textboxEnabled;

    private string _value1;

    private string _value2;

    private string _value3;

    private string _value4;

    private string _value5;

    private string _value6;

    private string _value7;

    private string _value8;

    private string _value9;

    public LimitsViewModel(string name, ILimitsEditor limitsEditor)
    {
      Name = name;
      _limitsEditor = limitsEditor;
      _limitsEditor.RegisterValuesChanged(UpdateValuesFromEditor);
      UpdateValuesFromEditor();
    }

    public bool TextboxEnabled
    {
      get => _textboxEnabled;
      set
      {
        _textboxEnabled = value;
        FirePropertyChanged();
      }
    }

    public string Name { get; set; }

    public string Value1
    {
      get => _value1;
      set
      {
        _value1 = value;
        if (!_suppressUpdate)
          _limitsEditor.UpdateValue1(_value1);
        FirePropertyChanged();
      }
    }

    public string Value2
    {
      get => _value2;
      set
      {
        _value2 = value;
        if (!_suppressUpdate)
          _limitsEditor.UpdateValue2(_value2);
        FirePropertyChanged();
      }
    }

    public string Value3
    {
      get => _value3;
      set
      {
        _value3 = value;
        if (!_suppressUpdate)
          _limitsEditor.UpdateValue3(_value3);
        FirePropertyChanged();
      }
    }

    public string Value4
    {
      get => _value4;
      set
      {
        _value4 = value;
        if (!_suppressUpdate)
          _limitsEditor.UpdateValue4(_value4);
        FirePropertyChanged();
      }
    }

    public string Value5
    {
      get => _value5;
      set
      {
        _value5 = value;
        if (!_suppressUpdate)
          _limitsEditor.UpdateValue5(_value5);
        FirePropertyChanged();
      }
    }

    public string Value6
    {
      get => _value6;
      set
      {
        _value6 = value;
        if (!_suppressUpdate)
          _limitsEditor.UpdateValue6(_value6);
        FirePropertyChanged();
      }
    }

    public string Value7
    {
      get => _value7;
      set
      {
        _value7 = value;
        if (!_suppressUpdate)
          _limitsEditor.UpdateValue7(_value7);
        FirePropertyChanged();
      }
    }

    public string Value8
    {
      get => _value8;
      set
      {
        _value8 = value;
        if (!_suppressUpdate)
          _limitsEditor.UpdateValue8(_value8);
        FirePropertyChanged();
      }
    }

    public string Value9
    {
      get => _value9;
      set
      {
        _value9 = value;
        if (!_suppressUpdate)
          _limitsEditor.UpdateValue9(_value9);
        FirePropertyChanged();
      }
    }

    private void UpdateValuesFromEditor()
    {
      _suppressUpdate = true;
      Value1 = _limitsEditor.Value1.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
      Value2 = _limitsEditor.Value2.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
      Value3 = _limitsEditor.Value3.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
      Value4 = _limitsEditor.Value4.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
      Value5 = _limitsEditor.Value5.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
      Value6 = _limitsEditor.Value6.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
      Value7 = _limitsEditor.Value7.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
      Value8 = _limitsEditor.Value8.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
      Value9 = _limitsEditor.Value9.ToString(CultureInfo.InvariantCulture).Replace('.', ',');
      _suppressUpdate = false;
    }

    public void LoadValues()
    {
      _limitsEditor.LoadValues();
    }
  }
}