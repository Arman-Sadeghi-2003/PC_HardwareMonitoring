using PC_HardwareMonitoring.Models.CPU.Temperature;
using PC_HardwareMonitoring.Models.CPU.Usage;
using System.Collections.Generic;

namespace PC_HardwareMonitoring.Models.CPU
{
	public class CPU_Model
	{
		public string? Name { get; set; }
		public string? Manufacturer { get; set; }
		public string? Description { get; set; }
		public string? ProcessorId { get; set; }
		public string? Architecture { get; set; }
		public string? NumberOfCores { get; set; }
		public string? NumberOfLogicalProcessors { get; set; }
		public string? MaxClockSpeed { get; set; }
		public string? CurrentClockSpeed { get; set; }
		public string? SocketDesignation { get; set; }
		public string? L2CacheSize { get; set; }
		public string? L3CacheSize { get; set; }
		public string? ThreadCount { get; set; }
		public string? Status { get; set; }
		public string? VirtualizationEnabled { get; set; }
		public string? SecondLevelAddressTranslation { get; set; }
		public string? DataWidth { get; set; }
		public string? AddressWidth { get; set; }
		public string? Revision { get; set; }

		public List<CPU_Usage> CPU_Usages { get; set; } = new();
		public List<CPU_Temperature> CPU_Temperatures { get; set;} = new();
	}
}