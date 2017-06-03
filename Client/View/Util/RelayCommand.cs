using System;
using System.Windows.Input;

// ReSharper disable InconsistentNaming

namespace MatchMakingMonitor.View.Util
{
	public class RelayCommand : ICommand
	{
		private const bool _canExecute = true;
		protected Action Action;
		protected Action<object> ActionWithParam;

		public RelayCommand(Action<object> action)
		{
			ActionWithParam = action;
		}

		public RelayCommand(Action action)
		{
			Action = action;
		}

		public event EventHandler CanExecuteChanged;

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