using System;
using System.IO;

namespace PC_HardwareMonitoring.Tools.URLs
{
	internal abstract class BaseUrls
	{
		private static string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

		protected static string HWLocalPath = Path.Combine(localPath, "PC_HW_Monitoring (AILC)");
	}
}
