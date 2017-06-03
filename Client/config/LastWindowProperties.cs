using System.Windows;
using Newtonsoft.Json;

namespace MatchmakingMonitor.config
{
	public class LastWindowProperties
	{
		[JsonProperty("left")]
		public double Left { get; set; }

		[JsonProperty("top")]
		public double Top { get; set; }

		[JsonProperty("width")]
		public double Width { get; set; }

		[JsonProperty("height")]
		public double Height { get; set; }

		[JsonProperty("windowState")]
		public WindowState WindowState { get; set; }
	}
}