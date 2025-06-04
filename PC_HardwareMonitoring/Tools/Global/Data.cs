using PC_HardwareMonitoring.Models.CPU;
using System.Collections.Generic;

namespace PC_HardwareMonitoring.Tools.Global
{
	/// <summary>
	/// Represents a singleton class to hold global data, such as CPU information.
	/// </summary>
	public class Data
	{
		// Singleton instance
		private static Data instance;

		/// <summary>
		/// Gets the singleton instance of the <see cref="Data"/> class.
		/// </summary>
		public static Data Instance => instance ?? (instance = new Data());

		/// <summary>
		/// Prevents external instantiation of the <see cref="Data"/> class.
		/// </summary>
		private Data() { }

		/// <summary>
		/// Gets or sets the list of CPU models.
		/// </summary>
		public List<CPU_Model> CPUs { get; set; } = new();
	}
}