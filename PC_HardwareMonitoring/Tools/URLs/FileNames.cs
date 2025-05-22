using System.IO;

namespace PC_HardwareMonitoring.Tools.URLs
{
	internal abstract class FileNames : BaseUrls
	{
		public static readonly string SettingFilePath = Path.Combine(HWLocalPath, "Setting.hwmAILC");
		
	}
}
