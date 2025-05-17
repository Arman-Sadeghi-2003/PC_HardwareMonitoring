using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_HardwareMonitoring.ViewModels.Home
{
	public partial class SettingsViewModel : ViewModelBase
	{
		private SettingsViewModel() { }
		private static SettingsViewModel instance;
		public static SettingsViewModel Instance => instance ?? (instance = new());
	}
}
