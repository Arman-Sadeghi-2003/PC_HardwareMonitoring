using System.Collections.Generic;

namespace PC_HardwareMonitoring.Models.CPU
{
	public class CPU_Temperature
	{
		public string CoreName { get; set; }
		public List<float> Values { get; set; } = new();
	}
}