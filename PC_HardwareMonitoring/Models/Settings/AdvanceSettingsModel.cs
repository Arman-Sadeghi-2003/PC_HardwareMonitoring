using System.Collections.Generic;

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
