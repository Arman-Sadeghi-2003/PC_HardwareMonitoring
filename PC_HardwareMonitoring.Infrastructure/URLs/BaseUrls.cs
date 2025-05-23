using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_HardwareMonitoring.Infrastructure.URLs
{
	public abstract class BaseUrls
	{
		private static string localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

		protected static string HWLocalPath = Path.Combine(localPath, "PC_HW_Monitoring (AILC)");
	}
}
