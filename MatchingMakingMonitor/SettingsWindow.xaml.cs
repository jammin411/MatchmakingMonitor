using System.Text.RegularExpressions;
using System.Windows.Input;

namespace MatchMakingMonitor
{
	public partial class SettingsWindow
	{
		public SettingsWindow()
		{
			InitializeComponent();
		}

		private void IntValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			var regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		private void DoubleValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			var regex = new Regex("[^0-9,]+");
			e.Handled = regex.IsMatch(e.Text);
		}
	}
}
