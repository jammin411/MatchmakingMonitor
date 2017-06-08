using System;
using System.Reactive.Subjects;
using MatchMakingMonitor.config.Reflection;

namespace MatchMakingMonitor.View.Settings
{
	public class DoubleLimitsEditor : ILimitsEditor
	{
		private readonly Subject<ChangedSetting> _changedSubject;
		private readonly bool _initial;
		private readonly double[] _values;
		private double _value1;
		private double _value2;
		private double _value3;
		private double _value4;
		private double _value5;
		private double _value6;
		private double _value7;
		private double _value8;
		private double _value9;
		private Action _valuesChanged;

		public DoubleLimitsEditor(Subject<ChangedSetting> changedSubject, double[] values)
		{
			_changedSubject = changedSubject;
			_values = values;
			_initial = true;
			LoadValues();
			_initial = false;
		}


		public void LoadValues()
		{
			_value1 = _values[0];
			_value2 = _values[1];
			_value3 = _values[2];
			_value4 = _values[3];
			_value5 = _values[4];
			_value6 = _values[5];
			_value7 = _values[6];
			_value8 = _values[7];
			_value9 = _values[8];
			_valuesChanged?.Invoke();
			if (!_initial)
				_changedSubject.OnNext(new ChangedSetting(true, false));
		}

		public void RegisterValuesChanged(Action action)
		{
			_valuesChanged = action;
		}

		public double Value1 => _value1;
		public double Value2 => _value2;
		public double Value3 => _value3;
		public double Value4 => _value4;
		public double Value5 => _value5;
		public double Value6 => _value6;
		public double Value7 => _value7;
		public double Value8 => _value8;
		public double Value9 => _value9;

		public bool UpdateValue1(string value)
		{
			return UpdateValue(value, 0, ref _value1);
		}

		public bool UpdateValue2(string value)
		{
			return UpdateValue(value, 1, ref _value2);
		}

		public bool UpdateValue3(string value)
		{
			return UpdateValue(value, 2, ref _value3);
		}

		public bool UpdateValue4(string value)
		{
			return UpdateValue(value, 3, ref _value4);
		}

		public bool UpdateValue5(string value)
		{
			return UpdateValue(value, 4, ref _value5);
		}

		public bool UpdateValue6(string value)
		{
			return UpdateValue(value, 5, ref _value6);
		}

		public bool UpdateValue7(string value)
		{
			return UpdateValue(value, 6, ref _value7);
		}

		public bool UpdateValue8(string value)
		{
			return UpdateValue(value, 7, ref _value8);
		}

		public bool UpdateValue9(string value)
		{
			return UpdateValue(value, 8, ref _value9);
		}

		private bool UpdateValue(string value, int index, ref double field)
		{
			var oldValue = field;
			double newValue;
			if (!double.TryParse(value, out newValue)) return false;
			field = newValue;
			_values[index] = newValue;
			_changedSubject.OnNext(new ChangedSetting(oldValue, newValue));
			return true;
		}
	}
}