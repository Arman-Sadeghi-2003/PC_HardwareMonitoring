/// <summary>
/// ViewModel for the Settings view, handling application settings and configurations.
/// </summary>
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Models.Settings;
using PC_HardwareMonitoring.Tools.Helpers;
using PC_HardwareMonitoring.Tools.Localization;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace PC_HardwareMonitoring.ViewModels.Home
{
	public partial class SettingsViewModel : ViewModelBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
		/// </summary>
		public SettingsViewModel()
		{
			settingsModel = SettingsModel.Instance;
			InitializeProperties();
		}

		private SettingsModel settingsModel;

		#region Properties

		#region General

		/// <summary>
		/// Gets or sets a value indicating whether the application should run on startup.
		/// </summary>
		[ObservableProperty]
		private bool runAsStartup;

		/// <summary>
		/// Gets or sets a value indicating whether to show notifications.
		/// </summary>
		[ObservableProperty]
		private bool showNotification;

		/// <summary>
		/// Gets or sets the collection of available temperature options.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<ComboBoxItem> temperatures;

		/// <summary>
		/// Gets or sets the selected temperature option.
		/// </summary>
		[ObservableProperty]
		private ComboBoxItem? selectedTemperature;

		/// <summary>
		/// Gets or sets the collection of available languages.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<ComboBoxItem> _languages;

		/// <summary>
		/// Gets or sets the selected language.
		/// </summary>
		[ObservableProperty]
		private ComboBoxItem? selectedLanguage;

		#endregion General

		#region Advanced

		/// <summary>
		/// Gets or sets the collection of available refresh intervals.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<ComboBoxItem> intervals;

		/// <summary>
		/// Gets or sets the selected refresh interval.
		/// </summary>
		[ObservableProperty]
		private ComboBoxItem selectedInterval;

		#endregion

		#endregion

		#region Methods

		/// <summary>
		/// Initializes the properties of the ViewModel with values from the settings model.
		/// </summary>
		private void InitializeProperties()
		{
			// General

			RunAsStartup = settingsModel.RunAsStartup;
			ShowNotification = settingsModel.ShowNotification;
			Languages = new();

			// Populate the languages collection from the settings model.
			foreach (var lang in settingsModel.LanguageCultures)
			{
				Languages.Add(new()
				{
					Content = lang.Value,
					Tag = lang.Key,
				});
				// Select the language based on the settings model.
				if (settingsModel.selectedLanguage == lang.Key)
					SelectedLanguage = Languages.Last();
			}

			// Populate the temperatures collection from the settings model.
			Temperatures = new(settingsModel.WarningTemperatures.Select(t => new ComboBoxItem() { Content = t }));
			SelectedTemperature = Temperatures.FirstOrDefault(i => (i.Content?.ToString() ?? "") == settingsModel.SelectedWarningTemperature);

			// Advanced
			int intervalC = 1;
			Intervals = new(settingsModel.RefreshIntervals.Select(i => new ComboBoxItem() { Content = i, Tag = intervalC++ }));

			SelectedInterval = Intervals[settingsModel.SelectedRefreshInterval - 1];
		}

		/// <summary>
		/// Handles property changes and updates the settings model accordingly.
		/// </summary>
		/// <param name="e">The event arguments containing information about the property change.</param>
		protected override void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			// Update settings model based on property changes.
			switch (e.PropertyName)
			{
				case nameof(SelectedLanguage):
					{
						var iso = SelectedLanguage?.Tag?.ToString() ?? "en-US";
						LocalizationManager.Instance.ChangeLanguage(iso);
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
