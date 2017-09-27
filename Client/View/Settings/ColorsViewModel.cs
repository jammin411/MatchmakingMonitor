using System.Windows.Media;
using MatchmakingMonitor.View.Util;

namespace MatchmakingMonitor.View.Settings
{
  public class ColorsViewModel : ViewModelBase
  {
    private readonly ColorsEditor _colorsEditor;

    private Color _color1;

    private Color _color2;

    private Color _color3;

    private Color _color4;

    private Color _color5;

    private Color _color6;

    private Color _color7;

    private Color _color8;

    private Color _color9;
    private bool _suppressUpdate;

    public ColorsViewModel(string name, ColorsEditor colorsEditor)
    {
      Name = name;
      _colorsEditor = colorsEditor;
      _colorsEditor.RegisterValuesChanged(UpdateValuesFromEditor);
      UpdateValuesFromEditor();
    }

    public string Name { get; set; }

    public Color Color1
    {
      get => _color1;
      set
      {
        _color1 = value;
        if (!_suppressUpdate)
          _colorsEditor.UpdateColor1(_color1);
        FirePropertyChanged();
      }
    }

    public Color Color2
    {
      get => _color2;
      set
      {
        _color2 = value;
        if (!_suppressUpdate)
          _colorsEditor.UpdateColor2(_color2);
        FirePropertyChanged();
      }
    }

    public Color Color3
    {
      get => _color3;
      set
      {
        _color3 = value;
        if (!_suppressUpdate)
          _colorsEditor.UpdateColor3(_color3);
        FirePropertyChanged();
      }
    }

    public Color Color4
    {
      get => _color4;
      set
      {
        _color4 = value;
        if (!_suppressUpdate)
          _colorsEditor.UpdateColor4(_color4);
        FirePropertyChanged();
      }
    }

    public Color Color5
    {
      get => _color5;
      set
      {
        _color5 = value;
        if (!_suppressUpdate)
          _colorsEditor.UpdateColor5(_color5);
        FirePropertyChanged();
      }
    }

    public Color Color6
    {
      get => _color6;
      set
      {
        _color6 = value;
        if (!_suppressUpdate)
          _colorsEditor.UpdateColor6(_color6);
        FirePropertyChanged();
      }
    }

    public Color Color7
    {
      get => _color7;
      set
      {
        _color7 = value;
        if (!_suppressUpdate)
          _colorsEditor.UpdateColor7(_color7);
        FirePropertyChanged();
      }
    }

    public Color Color8
    {
      get => _color8;
      set
      {
        _color8 = value;
        if (!_suppressUpdate)
          _colorsEditor.UpdateColor8(_color8);
        FirePropertyChanged();
      }
    }

    public Color Color9
    {
      get => _color9;
      set
      {
        _color9 = value;
        if (!_suppressUpdate)
          _colorsEditor.UpdateColor9(_color9);
        FirePropertyChanged();
      }
    }

    private void UpdateValuesFromEditor()
    {
      _suppressUpdate = true;
      Color1 = _colorsEditor.Color1;
      Color2 = _colorsEditor.Color2;
      Color3 = _colorsEditor.Color3;
      Color4 = _colorsEditor.Color4;
      Color5 = _colorsEditor.Color5;
      Color6 = _colorsEditor.Color6;
      Color7 = _colorsEditor.Color7;
      Color8 = _colorsEditor.Color8;
      Color9 = _colorsEditor.Color9;
      _suppressUpdate = false;
    }

    public void LoadValues()
    {
      _colorsEditor.LoadValues();
    }
  }
}