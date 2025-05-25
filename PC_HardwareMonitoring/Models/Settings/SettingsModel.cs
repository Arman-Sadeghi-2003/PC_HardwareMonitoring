using System.Collections.Generic;
using System.Linq;

namespace PC_HardwareMonitoring.Models.Settings
{
	public class SettingsModel
	{
		private SettingsModel()
		{
			Initialize();

		}
		private static SettingsModel? instance;
		public static SettingsModel Instance => instance ?? (instance = new SettingsModel());

		#region General settings

		public bool RunAsStartup { get; set; }
		public bool ShowNotification { get; set; }

		public List<string> WarningTemperatures { get; private set; }
		public string SelectedWarningTemperature { get; set; }

		public Dictionary<string, string> LanguageCultures { get; private set; }
		public string selectedLanguage { get; set; }

		#endregion General settings

		private void Initialize()
		{
			LanguageCultures = new();
			LanguageCultures.Add("en-US", "English");
			LanguageCultures.Add("fa-IR", "فارسی");
			selectedLanguage = LanguageCultures.First().Key;

			// Temp

			WarningTemperatures = new()
			{
				"70°C",
				"75°C",
				"80°C",
				"85°C",
				"90°C",
			};
			SelectedWarningTemperature = WarningTemperatures[2];
		}
	}
}
