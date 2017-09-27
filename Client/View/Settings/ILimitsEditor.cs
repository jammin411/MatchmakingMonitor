using System;

namespace MatchmakingMonitor.View.Settings
{
  public interface ILimitsEditor
  {
    double Value1 { get; }
    double Value2 { get; }
    double Value3 { get; }
    double Value4 { get; }
    double Value5 { get; }
    double Value6 { get; }
    double Value7 { get; }
    double Value8 { get; }
    double Value9 { get; }

    bool UpdateValue1(string value);
    bool UpdateValue2(string value);
    bool UpdateValue3(string value);
    bool UpdateValue4(string value);
    bool UpdateValue5(string value);
    bool UpdateValue6(string value);
    bool UpdateValue7(string value);
    bool UpdateValue8(string value);
    bool UpdateValue9(string value);

    void LoadValues();
    void RegisterValuesChanged(Action action);
  }
}