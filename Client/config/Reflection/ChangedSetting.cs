namespace MatchmakingMonitor.config.Reflection
{
  public class ChangedSetting
  {
    public ChangedSetting(object oldvalue, object newValue, string key = "UISetting")
    {
      Key = key;
      OldValue = oldvalue;
      NewValue = newValue;
    }

    public string Key { get; }
    public object OldValue { get; }
    public object NewValue { get; }
    public bool Initial { get; set; }
    public bool HasChanged => !Initial && !OldValue.Equals(NewValue);
  }
}