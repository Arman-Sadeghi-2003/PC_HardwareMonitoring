using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_HardwareMonitoring.Tools.Global
{
	public class Data
	{
		// ----> Singleton instance

		private Data() { }
		private static Data instance;
		public static Data Instance => instance ?? (instance = new Data());

		// ----> Methods



	}
}
