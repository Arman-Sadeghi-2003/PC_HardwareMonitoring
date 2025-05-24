using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Models.Settings;
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

		[ObservableProperty]
		private bool runAsStartup;

		[ObservableProperty]
		private ObservableCollection<ComboBoxItem> _languages;

		[ObservableProperty]
		private ComboBoxItem selectedLanguage;

		#endregion

		#region Methods

		private void InitializeProperties()
		{
			RunAsStartup = settingsModel.RunAsStartup;
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
			}
		}

		#endregion
	}
}
