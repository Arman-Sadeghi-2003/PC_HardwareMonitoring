using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Tools.HW;

namespace PC_HardwareMonitoring.ViewModels.Tabs
{
	public partial class CPUTabViewModel : ViewModelBase
	{
		[ObservableProperty]
		private string _CPUInfos;

		public CPUTabViewModel()
		{
			CPUInfos = Monitor.Instance.GetCPUInfos();
		}
	}
}
