using System;

namespace PC_HardwareMonitoring.Models.CPU.Usage
{
	public class CPU_Usage : CoreBase
	{
		public DateTime Time { get; set; }
		public double UsagePercent { get; set; }
	}
}