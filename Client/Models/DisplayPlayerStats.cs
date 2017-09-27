using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using MatchmakingMonitor.Services;
using MatchmakingMonitor.View.Util;

namespace MatchmakingMonitor.Models
{
  public class DisplayPlayerStats : ViewModelBase
  {
    private readonly SettingsWrapper _settingsWrapper;

    private SolidColorBrush _background;

    private SolidColorBrush _color = Brushes.Black;

    private SolidColorBrush _colorAvgDamage = Brushes.Black;
    private int _colorAvgDamageKey;

    private SolidColorBrush _colorAvgFrags = Brushes.Black;
    private int _colorAvgFragsKey;

    private SolidColorBrush _colorAvgXp = Brushes.Black;
    private int _colorAvgXpKey;

    private SolidColorBrush _colorBattles = Brushes.Black;
    private int _colorBattlesKey;

    private SolidColorBrush _colorBorder = Brushes.Transparent;

    private int _colorKey;

    private SolidColorBrush _colorWinRate = Brushes.Black;
    private int _colorWinRateKey;

    private int _fontSize;
    private Visibility _hideLowBattlesVisibility = Visibility.Collapsed;
    private Visibility _privateVisibility;
    private Visibility _visibility;

    public DisplayPlayerStats(SettingsWrapper settingsWrapper, PlayerShip player) : this()
    {
      Player = player;
      _settingsWrapper = settingsWrapper;
      WinRate = Math.Round(player.Wins / player.Battles * 100, 2);
      AvgFrags = Math.Round(player.Frags / player.Battles, 2);
      AvgXp = Math.Round(player.XpEarned / player.Battles, 0);
      AvgDamage = Math.Round(player.DamageDealt / player.Battles, 0);

      ComputeUi();
    }

    public DisplayPlayerStats()
    {
    }

    public PlayerShip Player { get; set; }

    public SolidColorBrush Background
    {
      get => _background;
      set
      {
        _background = value;
        FirePropertyChanged();
      }
    }

    public SolidColorBrush Color
    {
      get => _color;
      set
      {
        _color = value;
        FirePropertyChanged();
      }
    }

    public SolidColorBrush ColorWinRate
    {
      get => _colorWinRate;
      set
      {
        _colorWinRate = value;
        FirePropertyChanged();
      }
    }

    public SolidColorBrush ColorAvgXp
    {
      get => _colorAvgXp;
      set
      {
        _colorAvgXp = value;
        FirePropertyChanged();
      }
    }

    public SolidColorBrush ColorAvgFrags
    {
      get => _colorAvgFrags;
      set
      {
        _colorAvgFrags = value;
        FirePropertyChanged();
      }
    }

    public SolidColorBrush ColorAvgDamage
    {
      get => _colorAvgDamage;
      set
      {
        _colorAvgDamage = value;
        FirePropertyChanged();
      }
    }

    public SolidColorBrush ColorBattles
    {
      get => _colorBattles;
      set
      {
        _colorBattles = value;
        FirePropertyChanged();
      }
    }

    public SolidColorBrush ColorBorder
    {
      get => _colorBorder;
      set
      {
        _colorBorder = value;
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

    public Visibility Visibility
    {
      get => _visibility;
      set
      {
        _visibility = value;
        FirePropertyChanged();
      }
    }

    public Visibility PrivateVisibility
    {
      get => _privateVisibility;
      set
      {
        _privateVisibility = value;
        FirePropertyChanged();
      }
    }

    public Visibility HideLowBattlesVisibility
    {
      get => _hideLowBattlesVisibility;
      set
      {
        _hideLowBattlesVisibility = value;
        FirePropertyChanged();
      }
    }


    public string TextBattles => Player.Battles.ToString(CultureInfo.InvariantCulture);
    public string TextWins => Player.Wins.ToString(CultureInfo.InvariantCulture);
    public string TextWinRate => $"{WinRate}%";
    public string TextAvgXp => AvgXp.ToString(CultureInfo.InvariantCulture);
    public string TextAvgFrags => AvgFrags.ToString(CultureInfo.InvariantCulture);
    public string TextAvgDamage => AvgDamage.ToString(CultureInfo.InvariantCulture);
    public string TextName => Player?.Nickname;

    public string TextShipName => $"{ShipName} (Tier {Player.ShipTier})";


    public string[] CommandParams => new[] {AccountId, Player.Nickname};

    public string ShipName => Player?.ShipName;
    public string AccountId => Player?.AccountId.ToString();

    public double WinRate { get; private set; }
    public double AvgXp { get; private set; }
    public double AvgFrags { get; private set; }
    public double AvgDamage { get; private set; }

    public void ComputeUi()
    {
      try
      {
        FontSize = _settingsWrapper.CurrentSettings.FontSize;

        if (Player.IsPrivateOrHidden)
        {
          IsPrivateProfile();
          return;
        }

        if (_settingsWrapper.CurrentSettings.HideLowBattles && Player.Battles <= 10)
        {
          IsLowBattles();
          Color = Brushes.Black;
          SetBackground(Color);
          return;
        }

        IsNormal();
        double totalRating = 0;
        ColorWinRate = GetColor(WinRate, _settingsWrapper.CurrentSettings.WinRateLimits, totalRating,
          _settingsWrapper.CurrentSettings.WinRateWeight, out totalRating,
          out _colorWinRateKey);
        ColorAvgFrags = GetColor(AvgFrags, _settingsWrapper.CurrentSettings.AvgFragsLimits, totalRating,
          _settingsWrapper.CurrentSettings.AvgFragsWeight, out totalRating,
          out _colorAvgFragsKey);
        ColorAvgXp = GetColor(AvgXp,
          _settingsWrapper.CurrentSettings.AvgXpLimits.Single(l => l.ShipTier == Player.ShipTier).Values, totalRating,
          _settingsWrapper.CurrentSettings.AvgXpWeight, out totalRating,
          out _colorAvgXpKey);
        ColorAvgDamage = GetColor(AvgDamage,
          _settingsWrapper.CurrentSettings.AvgDmgLimits.GetLimits(Player.ShipType, Player.ShipTier), totalRating,
          _settingsWrapper.CurrentSettings.AvgDmgWeight, out totalRating, out _colorAvgDamageKey);
        ColorBattles = GetColor(Player.Battles, _settingsWrapper.CurrentSettings.BattleLimits, totalRating,
          _settingsWrapper.CurrentSettings.BattleWeight, out totalRating,
          out _colorBattlesKey);

        _colorKey = (int) Math.Floor(totalRating / 5);
        if (_colorKey == 0) _colorKey = 1;
        Color = _settingsWrapper.Brushes[_colorKey - 1];

        SetBackground(Color);
      }
      catch (Exception e)
      {
        IoCKernel.Get<ILogger>().Error("Error during UI computation", e);
      }
    }

    private void SetBackground(SolidColorBrush brush)
    {
      var color = brush.CloneCurrentValue();
      color.Opacity = 0.08;
      color.Freeze();
      Background = color;

      ColorBorder = Player.Relation == 0 ? Color : Background;
    }


    public static DisplayPlayerStats MockPlayer(int relation = 1, bool privateOrHidden = false)
    {
      return new DisplayPlayerStats
      {
        Player = new PlayerShip
        {
          Nickname = "Test",
          AccountId = 12323325,
          ShipName = "Name",
          IsPrivateOrHidden = privateOrHidden,
          Relation = relation
        },
        WinRate = 40,
        AvgFrags = 5,
        AvgXp = 1234,
        AvgDamage = 123242
      };
    }

    private void IsPrivateProfile()
    {
      Visibility = Visibility.Collapsed;
      PrivateVisibility = Visibility.Visible;
      HideLowBattlesVisibility = Visibility.Collapsed;
    }

    private void IsLowBattles()
    {
      Visibility = Visibility.Collapsed;
      PrivateVisibility = Visibility.Collapsed;
      HideLowBattlesVisibility = Visibility.Visible;
    }

    private void IsNormal()
    {
      Visibility = Visibility.Visible;
      PrivateVisibility = Visibility.Collapsed;
      HideLowBattlesVisibility = Visibility.Collapsed;
    }

    #region Colors

    private SolidColorBrush GetColor(double value, IEnumerable<double> limits, double oldTotal, double multiplier,
      out double total, out int key)
    {
      var limitsList = limits.ToArray();
      for (var i = 0; i < limitsList.Length; i++)
      {
        if (!(value >= limitsList[i])) continue;
        key = i + 1;
        total = oldTotal + key * multiplier;
        return _settingsWrapper.Brushes[i];
      }
      total = oldTotal + 9 * multiplier;
      key = 9;
      return Brushes.Black;
    }

    #endregion

    public MobilePlayerStats ToMobile()
    {
      return new MobilePlayerStats
      {
        Relation = Player.Relation,
        PrivateOrHidden = Player.IsPrivateOrHidden,
        IsLowBattles = Player.Battles <= 10,
        DisplayName = TextName,
        Name = Player.Nickname,
        AccountId = AccountId,
        ShipName = ShipName,
        ShipTier = Player.ShipTier.ToString(),
        Battles = Player.Battles,
        Wins = Player.Wins,
        WinRate = WinRate,
        AvgXp = AvgXp,
        AvgFrags = AvgFrags,
        AvgDamage = AvgDamage
      };
    }

    public MobileColorKeys GetColorKeys()
    {
      return new MobileColorKeys
      {
        AccountId = AccountId,
        ColorNameKey = _colorKey,
        ColorWinRateKey = _colorWinRateKey,
        ColorAvgFragsKey = _colorAvgFragsKey,
        ColorAvgXpKey = _colorAvgXpKey,
        ColorAvgDamageKey = _colorAvgDamageKey,
        ColorBattlesKey = _colorBattlesKey
      };
    }
  }
}