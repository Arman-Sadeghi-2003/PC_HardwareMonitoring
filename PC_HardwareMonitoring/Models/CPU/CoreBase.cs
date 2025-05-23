using System.Collections.Generic;

namespace PC_HardwareMonitoring.Models.CPU
{
	public class CoreBase
	{
		public string? Name { get; set; }
		public List<float> Values { get; set; } = new();
	}
}
