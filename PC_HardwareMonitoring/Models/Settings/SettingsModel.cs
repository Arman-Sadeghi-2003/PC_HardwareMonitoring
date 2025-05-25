using PC_HardwareMonitoring.Tools.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace PC_HardwareMonitoring.Models.Settings
{
	public class SettingsModel
	{
		public SettingsModel()
		{
			Initialize();
		}
		private static SettingsModel? instance;
		public static SettingsModel Instance => instance ?? (instance = FileSerializer.OpenSettings());

		#region General settings

		public bool RunAsStartup { get; set; }
		public bool ShowNotification { get; set; }

		[XmlIgnore]
		public List<string> WarningTemperatures { get; private set; }
		public string SelectedWarningTemperature { get; set; }

		[XmlIgnore]
		public Dictionary<string, string> LanguageCultures { get; private set; }
		public string selectedLanguage { get; set; }

		#endregion General settings

		#region Advanced
		[XmlIgnore]
		public List<string> RefreshIntervals { get; private set; } = new()
		{
			"1s",
			"2s",
			"3s",
			"4s",
			"5s",
		};

		public int SelectedRefreshInterval { get; set; }

		#endregion Advanced

		private void Initialize()
		{
			// General

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

			// Advanced

			SelectedRefreshInterval = 1;

		}
	}
}
