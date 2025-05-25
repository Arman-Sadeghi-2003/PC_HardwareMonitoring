using PC_HardwareMonitoring.Infrastructure.URLs;
using PC_HardwareMonitoring.Models.Settings;

namespace PC_HardwareMonitoring.Tools.Helpers
{
	public static class FileSerializer
	{
		public static void SaveSettings()
		{
			XmlSerialization.WriteToXmlFile(FileNames.SettingFilePath, SettingsModel.Instance);
		}

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
