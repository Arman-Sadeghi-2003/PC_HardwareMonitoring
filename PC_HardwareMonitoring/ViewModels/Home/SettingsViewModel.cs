using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Models.Settings;
using PC_HardwareMonitoring.Tools.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace PC_HardwareMonitoring.ViewModels.Home
{
	public partial class SettingsViewModel : ViewModelBase
	{
		public SettingsViewModel()
		{
			settingsModel = SettingsModel.Instance;
			InitializeProperties();
		}

		private SettingsModel settingsModel;

		#region Properties

		#region General
		[ObservableProperty]
		private bool runAsStartup;
		[ObservableProperty]
		private bool showNotification;

		[ObservableProperty]
		private ObservableCollection<ComboBoxItem> temperatures;

		[ObservableProperty]
		private ComboBoxItem? selectedTemperature;

		[ObservableProperty]
		private ObservableCollection<ComboBoxItem> _languages;

		[ObservableProperty]
		private ComboBoxItem? selectedLanguage;
		#endregion General

		#region Advanced
		[ObservableProperty]
		private ObservableCollection<ComboBoxItem> intervals;

		[ObservableProperty]
		private ComboBoxItem selectedInterval;
		#endregion

		#endregion

		#region Methods

		private void InitializeProperties()
		{
			// General

			RunAsStartup = settingsModel.RunAsStartup;
			ShowNotification = settingsModel.ShowNotification;
			Languages = new();

			foreach (var lang in settingsModel.LanguageCultures)
			{
				Languages.Add(new()
				{
					Content = lang.Value,
					Tag = lang.Key,
				});
				if (settingsModel.selectedLanguage == lang.Key)
					SelectedLanguage = Languages.Last();
			}

			Temperatures = new(settingsModel.WarningTemperatures.Select(t => new ComboBoxItem() { Content = t }));
			SelectedTemperature = Temperatures.FirstOrDefault(i => (i.Content?.ToString() ?? "") == settingsModel.SelectedWarningTemperature);

			// Advanced
			int intervalC = 1;
			Intervals = new(settingsModel.RefreshIntervals.Select(i => new ComboBoxItem() { Content = i, Tag = intervalC++ }));

			SelectedInterval = Intervals[settingsModel.SelectedRefreshInterval - 1];
		}

		protected override void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			switch (e.PropertyName)
			{
				case nameof(SelectedLanguage):
					{
						var iso = SelectedLanguage?.Tag?.ToString() ?? "en-US";
						((App)App.Current).ChangeLanguage(iso);
						settingsModel.selectedLanguage = iso;
					}
					break;
				case nameof(RunAsStartup):
					{
						settingsModel.RunAsStartup = RunAsStartup;
					}
					break;
				case nameof(ShowNotification):
					{
						settingsModel.ShowNotification = ShowNotification;
					}
					break;
				case nameof(SelectedTemperature):
					{
						settingsModel.SelectedWarningTemperature = SelectedTemperature?.Content?.ToString() ?? settingsModel.WarningTemperatures.First();
					}
					break;
				case nameof(SelectedInterval):
					{
						settingsModel.SelectedRefreshInterval = Convert.ToInt32(SelectedInterval.Tag);
					}
					break;
			}
			FileSerializer.SaveSettings();
		}

		#endregion
	}
}
