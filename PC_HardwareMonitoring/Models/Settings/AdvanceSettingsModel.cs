using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_HardwareMonitoring.Models.Settings
{
	public class AdvanceSettingsModel
	{

		public List<string> RefreshInterval { get; private set; } = new()
		{
			"1s",
			"2s",
			"3s",
			"4s",
			"5s",
		};

		public int SelectedRefreshInterval { get; set; }

	}
}
