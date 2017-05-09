using MatchMakingMonitor.Services;
// ReSharper disable InconsistentNaming

namespace MatchMakingMonitor.Models
{
	public class MobileSettings
	{
		public string color1 { get; set; }
		public string color2 { get; set; }
		public string color3 { get; set; }
		public string color4 { get; set; }
		public string color5 { get; set; }
		public string color6 { get; set; }
		public string color7 { get; set; }
		public string color8 { get; set; }
		public string color9 { get; set; }

		public static MobileSettings FromSettings(Settings settings)
		{
			return new MobileSettings()
			{
				color1 = settings.Color1,
				color2 = settings.Color2,
				color3 = settings.Color3,
				color4 = settings.Color4,
				color5 = settings.Color5,
				color6 = settings.Color6,
				color7 = settings.Color7,
				color8 = settings.Color8,
				color9 = settings.Color9
			};
		}

		public void ToSettings(Settings settings)
		{
			settings.Color1 = settings.Color1;
			settings.Color2 = settings.Color2;
			settings.Color3 = settings.Color3;
			settings.Color4 = settings.Color4;
			settings.Color5 = settings.Color5;
			settings.Color6 = settings.Color6;
			settings.Color7 = settings.Color7;
			settings.Color8 = settings.Color8;
			settings.Color9 = settings.Color9;
		}
	}
}
