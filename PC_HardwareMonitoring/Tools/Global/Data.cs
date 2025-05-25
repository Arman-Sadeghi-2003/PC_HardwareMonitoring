using PC_HardwareMonitoring.Models.CPU;
using System.Collections.Generic;

namespace PC_HardwareMonitoring.Tools.Global
{
	public class Data
	{
		// ----> Singleton instance

		private Data() { }

		private static Data instance;
		public static Data Instance => instance ?? (instance = new Data());

		// ----> Methods

		public List<CPU_Model> CPUs { get; set; } = new();
	}
}