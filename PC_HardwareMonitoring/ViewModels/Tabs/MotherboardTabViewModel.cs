using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Tools.HW;

namespace PC_HardwareMonitoring.ViewModels.Tabs
{
	public partial class MotherboardTabViewModel : ViewModelBase
	{
		[ObservableProperty]
		private string motherboardInfos;

		public MotherboardTabViewModel()
		{
			MotherboardInfos = Monitor.Instance.GetMotherboardInfos();
		}
	}
}