using PC_HardwareMonitoring.Models.CPU;

namespace PC_HardwareMonitoring.Tools.Global
{
	public class Data
	{
		// ----> Singleton instance

		private Data()
		{ }

		private static Data instance;
		public static Data Instance => instance ?? (instance = new Data());

		// ----> Methods

		public CPU_Model CPU { get; set; }
	}
}