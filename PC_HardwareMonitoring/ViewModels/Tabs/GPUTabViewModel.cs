using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Tools.HW;

namespace PC_HardwareMonitoring.ViewModels.Tabs
{
	public partial class GPUTabViewModel : ViewModelBase
	{
		[ObservableProperty]
		private string _GPU_Infos;

		[ObservableProperty]
		private string _GPU_Temp;

		private DispatcherTimer timer;

		public GPUTabViewModel()
		{
			GPU_Infos = Monitor.Instance.GetGPUInfos();
			GPU_Temp = Monitor.Instance.GetGPUTemp();

			timer = new DispatcherTimer();
			timer.Interval = System.TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		private void Timer_Tick(object? sender, System.EventArgs e)
		{
			GPU_Temp = Monitor.Instance.GetGPUTemp();
		}
	}
}