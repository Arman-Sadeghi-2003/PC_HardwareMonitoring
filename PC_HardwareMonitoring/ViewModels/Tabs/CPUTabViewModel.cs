using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Tools.HW;
using System;
using System.Reflection;

namespace PC_HardwareMonitoring.ViewModels.Tabs
{
	public partial class CPUTabViewModel : ViewModelBase
	{
		[ObservableProperty]
		private string _CPUInfos;

		[ObservableProperty]
		private string _CPUTemp;

		DispatcherTimer timer;
		int counter = 0;

		public CPUTabViewModel()
		{
			CPUInfos = Monitor.Instance.GetCPUInfos();
			CPUTemp = Monitor.Instance.GetCPUTemp();

			timer = new();
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		private void Timer_Tick(object? sender, EventArgs e)
		{
			CPUTemp = Monitor.Instance.GetCPUTemp();
			CPUTemp += $"\ninterval: {counter++}";
		}
	}
}
