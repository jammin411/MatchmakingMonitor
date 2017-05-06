﻿using MatchingMakingMonitor.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MatchingMakingMonitor.ViewModels
{
	public class SettingsWindowViewModel : BaseViewModel
	{
		public RelayCommand ResetCommand { get; set; }

		public ObservableCollection<int> FontSizes { get; private set; } = new ObservableCollection<int>() { 8, 9, 10, 11, 12, 13, 14 };

		private int fontSize;
		public int FontSize
		{
			get { return fontSize; }
			set
			{
				fontSize = value;
				settings.Set("FontSize", value);
				FirePropertyChanged();
			}
		}

		private Color overall9;
		public Color Overall9
		{
			get { return overall9; }
			set
			{
				overall9 = value;
				settings.Set("Overall9", value.ToString());
				FirePropertyChanged();
			}
		}

		private Color overall8;
		public Color Overall8
		{
			get { return overall8; }
			set
			{
				overall8 = value;
				settings.Set("Overall8", value.ToString());
				FirePropertyChanged();
			}
		}

		private Color overall7;
		public Color Overall7
		{
			get { return overall7; }
			set
			{
				overall7 = value;
				settings.Set("Overall7", value.ToString());
				FirePropertyChanged();
			}
		}

		private Color overall6;
		public Color Overall6
		{
			get { return overall6; }
			set
			{
				overall6 = value;
				settings.Set("Overall6", value.ToString());
				FirePropertyChanged();
			}
		}

		private Color overall5;
		public Color Overall5
		{
			get { return overall5; }
			set
			{
				overall5 = value;
				settings.Set("Overall5", value.ToString());
				FirePropertyChanged();
			}
		}

		private Color overall4;
		public Color Overall4
		{
			get { return overall4; }
			set
			{
				overall4 = value;
				settings.Set("Overall4", value.ToString());
				FirePropertyChanged();
			}
		}

		private Color overall3;
		public Color Overall3
		{
			get { return overall3; }
			set
			{
				overall3 = value;
				settings.Set("Overall3", value.ToString());
				FirePropertyChanged();
			}
		}

		private Color overall2;
		public Color Overall2
		{
			get { return overall2; }
			set
			{
				overall2 = value;
				settings.Set("Overall2", value.ToString());
				FirePropertyChanged();
			}
		}

		private Color overall1;
		public Color Overall1
		{
			get { return overall1; }
			set
			{
				overall1 = value;
				settings.Set("Overall1", value.ToString());
				FirePropertyChanged();
			}
		}

		private LoggingService loggingService;
		private Services.Settings settings;

		public SettingsWindowViewModel(LoggingService loggingService, Services.Settings settings)
		{
			this.loggingService = loggingService;
			this.settings = settings;

			this.ResetCommand = new RelayCommand(() =>
			{
				var result = MessageBox.Show("Are you sure you want to reset all settings?", "Reset settings", MessageBoxButton.YesNo);
				if (result == MessageBoxResult.Yes)
				{
					this.settings.Reset();
					initSettings();
				}
			});

			initSettings();
		}

		public SettingsWindowViewModel()
		{

		}

		private void initSettings()
		{
			try
			{
				FontSize = settings.Get<int>("FontSize");
				Overall9 = (Color)ColorConverter.ConvertFromString(settings.Get<string>("Overall9"));
				Overall8 = (Color)ColorConverter.ConvertFromString(settings.Get<string>("Overall8"));
				Overall7 = (Color)ColorConverter.ConvertFromString(settings.Get<string>("Overall7"));
				Overall6 = (Color)ColorConverter.ConvertFromString(settings.Get<string>("Overall6"));
				Overall5 = (Color)ColorConverter.ConvertFromString(settings.Get<string>("Overall5"));
				Overall4 = (Color)ColorConverter.ConvertFromString(settings.Get<string>("Overall4"));
				Overall3 = (Color)ColorConverter.ConvertFromString(settings.Get<string>("Overall3"));
				Overall2 = (Color)ColorConverter.ConvertFromString(settings.Get<string>("Overall2"));
				Overall1 = (Color)ColorConverter.ConvertFromString(settings.Get<string>("Overall1"));
			}
			catch (Exception e)
			{
				loggingService.Log($"Error while initializing settings. {e.Message}");
			}
		}
	}
}