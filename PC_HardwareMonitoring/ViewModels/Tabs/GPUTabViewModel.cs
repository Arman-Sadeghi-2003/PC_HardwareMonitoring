/// <summary>
/// ViewModel for the GPU tab, displaying GPU-related information.
/// </summary>
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Tools.HW;

namespace PC_HardwareMonitoring.ViewModels.Tabs
{
	public partial class GPUTabViewModel : ViewModelBase
	{
		/// <summary>
		/// Gets or sets the GPU information.
		/// </summary>
		[ObservableProperty]
		private string _GPU_Infos;

		/// <summary>
		/// Gets or sets the GPU temperature.
		/// </summary>
		[ObservableProperty]
		private string _GPU_Temp;

		private DispatcherTimer timer;

		/// <summary>
		/// Initializes a new instance of the <see cref="GPUTabViewModel"/> class.
		/// </summary>
		public GPUTabViewModel()
		{
			GPU_Infos = Monitor.Instance.GetGPUInfos();
			//GPU_Temp = Monitor.Instance.GetGPUTemp();

			timer = new DispatcherTimer();
			timer.Interval = System.TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		/// <summary>
		/// Handles the timer tick event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void Timer_Tick(object? sender, System.EventArgs e)
		{
			//TODO: Implement the Timer_Tick
			//GPU_Temp = Monitor.Instance.GetCPUTemp();
		}
	}
}
