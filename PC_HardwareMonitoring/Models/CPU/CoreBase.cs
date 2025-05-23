using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_HardwareMonitoring.Models.CPU
{
	public class CoreBase
	{
		public string? Name { get; set; }
		public List<float> Values { get; set; } = new();
	}
}
