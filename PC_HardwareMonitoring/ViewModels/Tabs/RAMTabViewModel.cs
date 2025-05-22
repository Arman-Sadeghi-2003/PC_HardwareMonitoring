using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Tools.HW;

namespace PC_HardwareMonitoring.ViewModels.Tabs
{
	public partial class RAMTabViewModel : ViewModelBase
	{
		[ObservableProperty]
		private string ramInfos;

		public RAMTabViewModel()
		{
			RamInfos = Monitor.Instance.GetMemoryInfos();
		}
	}
}
