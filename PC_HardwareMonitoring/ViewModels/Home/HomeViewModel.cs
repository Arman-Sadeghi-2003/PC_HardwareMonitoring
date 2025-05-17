using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.ViewModels.Tabs;

namespace PC_HardwareMonitoring.ViewModels.Home
{
	public partial class HomeViewModel : ViewModelBase
	{
		private HomeViewModel() { }
		private static HomeViewModel instance;
		public static HomeViewModel Instance => instance ?? (instance = new());

		#region Properties

		[ObservableProperty]
		private HomeTabViewModel homeTabVM = new();
		[ObservableProperty]
		private CPUTabViewModel _CPUTabVM = new();
		[ObservableProperty]
		private GPUTabViewModel _GPUTabVM = new();
		[ObservableProperty]
		private RAMTabViewModel _RAMTabVM = new();
		[ObservableProperty]
		private MotherboardTabViewModel motherboardTabVM = new();

		#endregion Properties

	}
}
