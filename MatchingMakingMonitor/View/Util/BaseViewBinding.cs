using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MatchMakingMonitor.View.Util
{
	public class BaseViewBinding : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		internal void FirePropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
