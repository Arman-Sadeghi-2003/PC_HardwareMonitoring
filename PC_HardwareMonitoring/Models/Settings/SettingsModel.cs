using System.Collections.Generic;
using System.Linq;

namespace PC_HardwareMonitoring.Models.Settings
{
	public class SettingsModel
	{
		private SettingsModel()
		{
			LanguageCultures = new();
			LanguageCultures.Add("en-US", "English");
			LanguageCultures.Add("fa-IR", "فارسی");
			selectedLanguage = LanguageCultures.First().Key;

		}
		private static SettingsModel? instance;
		public static SettingsModel Instance => instance ?? (instance = new SettingsModel());

		#region General settings

		public bool RunAsStartup { get; set; }
		public bool ShowNotification { get; set; }
		public Dictionary<string, string> LanguageCultures { get; private set; }
		public string selectedLanguage { get; set; }

		#endregion General settings

	}
}
