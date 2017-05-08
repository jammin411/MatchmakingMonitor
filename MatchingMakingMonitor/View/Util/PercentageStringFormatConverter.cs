using System;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;

namespace MatchMakingMonitor.View.Util
{
	class PercentageConverter : IValueConverter
	{
		private static readonly Regex PercentRegex = new Regex("^100$|^\\d{0,2}(,\\d{1,2})?$");

		public object Convert(object value, Type targetType, object parameter,
											System.Globalization.CultureInfo culture)
		{
			if (string.IsNullOrEmpty(value?.ToString())) return 0;

			return value is double ? value.ToString() : value;
		}

		public object ConvertBack(object value, Type targetType, object parameter,
															System.Globalization.CultureInfo culture)
		{
			if (string.IsNullOrEmpty(value?.ToString())) return 0;

			var trimmedValue = value.ToString().TrimEnd('%');
			if (!PercentRegex.IsMatch(trimmedValue))
			{
				return new ValidationResult(false, "String format error");
			}

			if (targetType != typeof(double)) return value;
			double result;
			return double.TryParse(trimmedValue, out result) ? result : value;
		}
	}
}
