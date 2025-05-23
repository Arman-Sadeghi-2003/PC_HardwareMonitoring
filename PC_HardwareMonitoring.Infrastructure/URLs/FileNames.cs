using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_HardwareMonitoring.Infrastructure.URLs
{
	public class FileNames : BaseUrls
	{
		public static readonly string SettingFilePath = Path.Combine(HWLocalPath, "Setting.hwmAILC");

	}
}
