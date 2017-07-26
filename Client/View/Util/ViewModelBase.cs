using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace MatchMakingMonitor.View.Util
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		public static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore
		};

		public event PropertyChangedEventHandler PropertyChanged;

		internal void FirePropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}