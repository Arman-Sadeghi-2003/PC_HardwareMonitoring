using PC_HardwareMonitoring.Infrastructure.URLs;
using PC_HardwareMonitoring.Models.Settings;

namespace PC_HardwareMonitoring.Tools.Helpers
{
	/// <summary>
	/// Provides methods for serializing and deserializing settings to and from a file.
	/// </summary>
	public static class FileSerializer
	{
		/// <summary>
		/// Saves the current settings to a file.
		/// </summary>
		public static void SaveSettings()
		{
			XmlSerialization.WriteToXmlFile(FileNames.SettingFilePath, SettingsModel.Instance);
		}

		/// <summary>
		/// Opens and deserializes settings from a file.
		/// </summary>
		/// <returns>The settings model, or a new instance if deserialization fails.</returns>
		public static SettingsModel OpenSettings()
		{
			try
			{
				return XmlSerialization.ReadFromXmlFile<SettingsModel>(FileNames.SettingFilePath);
			}
			catch
			{
				return new();
			}
		}
	}
}