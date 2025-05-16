using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Infrastructure.NotificationManager;
using PC_HardwareMonitoring.ViewModels.Tabs;

namespace PC_HardwareMonitoring.ViewModels
{
	public partial class MainWindowViewModel : ViewModelBase
	{
		#region Properties

		[ObservableProperty]
		private HomeTabViewModel homeTabVM = new();
		[ObservableProperty]
		private CPUTabViewModel cpuTabVM = new();
		[ObservableProperty]
		private GPUTabViewModel gpuTabVM = new();
		[ObservableProperty]
		private RAMTabViewModel ramTabVM = new();

		#endregion Properties

		public MainWindowViewModel()
		{

		}


	}
}
