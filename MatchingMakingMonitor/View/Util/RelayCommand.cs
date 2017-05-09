using System;
using System.Windows.Input;

// ReSharper disable InconsistentNaming

namespace MatchMakingMonitor.View.Util
{
	public class RelayCommand : ICommand
	{
		protected Action<object> ActionWithParam;
		protected Action Action;

		public RelayCommand(Action<object> action)
		{
			ActionWithParam = action;
		}
		public RelayCommand(Action action)
		{
			Action = action;
		}

		public event EventHandler CanExecuteChanged;

		private const bool _canExecute = true;

		public bool CanExecute(object parameter)
		{
			return _canExecute;
		}

		public void Execute(object parameter)
		{
			Action?.Invoke();
			ActionWithParam?.Invoke(parameter);
		}
	}
}
