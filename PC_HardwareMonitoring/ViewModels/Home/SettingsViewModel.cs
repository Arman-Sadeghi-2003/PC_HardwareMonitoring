using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Models.Settings;

namespace PC_HardwareMonitoring.ViewModels.Home
{
	public partial class SettingsViewModel : ViewModelBase
	{
		private SettingsViewModel()
		{
			settingsModel = SettingsModel.Instance;
			RunAsStartup = settingsModel.RunAsStartup;
		}
		private static SettingsViewModel instance;
		public static SettingsViewModel Instance => instance ?? (instance = new());

		private SettingsModel settingsModel;

		[ObservableProperty]
		private bool runAsStartup;
	}
}
