using System;

namespace PC_HardwareMonitoring.Models.CPU.Temperature
{
	public class CPU_Temperature : CoreBase
	{
		public DateTime Time { get; set; }
		public double TemperatureCelsius { get; set; }
	}
}