using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace MatchMakingMonitor.config
{
	[SuppressMessage("ReSharper", "InconsistentNaming")]
	public class SettingsJson
	{
		public string installDirectory { get; set; }
		public string region { get; set; }
		public string appIdNA { get; set; }
		public string appIdEU { get; set; }
		public string appIdRU { get; set; }
		public string appIdSEA { get; set; }
		public string baseUrlNA { get; set; }
		public string baseUrlEU { get; set; }
		public string baseUrlRU { get; set; }
		public string baseUrlSEA { get; set; }
		public string token { get; set; }
		public string color1 { get; set; }
		public string color2 { get; set; }
		public string color3 { get; set; }
		public string color4 { get; set; }
		public string color5 { get; set; }
		public string color6 { get; set; }
		public string color7 { get; set; }
		public string color8 { get; set; }
		public string color9 { get; set; }
		public int? fontSize { get; set; }
		public double[] battleLimits { get; set; }
		public double[] winLimits { get; set; }
		public double[] fragsLimits { get; set; }
		public double[] xpLimits { get; set; }
		public double[] dmgLimitsT1 { get; set; }
		public double[] dmgLimitsT2 { get; set; }
		public double[] dmgLimitsT3 { get; set; }
		public double[] dmgLimitsT4 { get; set; }
		public double[] dmgLimitsT5 { get; set; }
		public double[] dmgLimitsT6 { get; set; }
		public double[] dmgLimitsT7 { get; set; }
		public double[] dmgLimitsT8 { get; set; }
		public double[] dmgLimitsT9 { get; set; }
		public double[] dmgLimitsT10 { get; set; }
		public double? battleWeight { get; set; }
		public double? fragsWeight { get; set; }
		public double? xpWeight { get; set; }
		public double? dmgWeight { get; set; }
		public double? winWeight { get; set; }


		private readonly PropertyInfo[] properties;
		public SettingsJson()
		{
			properties = GetType().GetProperties();
		}

		public void Set(string key, object value)
		{
			if (value != null)
				properties.Single(p => p.Name == key).SetValue(this, value);
		}

		public T Get<T>(string key)
		{
			return (T)properties.Single(p => p.Name == key).GetValue(this);
		}

		public object Get(string key)
		{
			return Get<object>(key);
		}
	}
}
