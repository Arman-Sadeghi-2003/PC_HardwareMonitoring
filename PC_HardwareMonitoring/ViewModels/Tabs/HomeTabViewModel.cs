/// <summary>
/// ViewModel for the Home tab.
/// </summary>
using Avalonia.Threading; // Added
using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Models.Hardware; // Added
using PC_HardwareMonitoring.Models.Settings;   // Added
using PC_HardwareMonitoring.Tools.HW;         // Added
using PC_HardwareMonitoring.ViewModels.Commons;
using System;               // Added
using System.Collections.Generic; // Added
using System.Linq;          // Added

namespace PC_HardwareMonitoring.ViewModels.Tabs
{
	public partial class HomeTabViewModel : ViewModelBase
	{
		[ObservableProperty]
		private ChartViewModel _cpuTemperatureChart;

		[ObservableProperty]
		private ChartViewModel _gpuTemperatureChart;

		private readonly List<double> cpuTemperatureHistory = new();
		private readonly List<double> gpuTemperatureHistory = new();
		private readonly List<string> chartLabels = new();
		private const int MaxDataPoints = 30;
		private long timeStampCounter = 0; // Changed to long for potentially long running apps
		private DispatcherTimer timer;

		/// <summary>
		/// Initializes a new instance of the <see cref="HomeTabViewModel"/> class.
		/// </summary>
		public HomeTabViewModel()
		{
			initializeCharts();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(SettingsModel.Instance.SelectedRefreshInterval);
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		/// <summary>
		/// Initializes the charts for the Home tab.
		/// </summary>
		private void initializeCharts()
		{
			for (int i = 0; i < MaxDataPoints; i++)
			{
				cpuTemperatureHistory.Add(0);
				gpuTemperatureHistory.Add(0);
				chartLabels.Add(string.Empty); // Initialize with empty strings or sequence
			}
			// Initialize timeStampCounter to fill labels correctly for the first display
			for (int i = 0; i < MaxDataPoints; i++)
            {
                chartLabels[i] = (timeStampCounter - (MaxDataPoints - 1 - i)).ToString();
            }


			CpuTemperatureChart = new ChartViewModel(
				new List<double>(cpuTemperatureHistory),
				"CPU Temperature (°C)",
				new List<string>(chartLabels),
				"CPU Package Temp");

			GpuTemperatureChart = new ChartViewModel(
				new List<double>(gpuTemperatureHistory),
				"GPU Temperature (°C)",
				new List<string>(chartLabels),
				"GPU Core Temp");
		}

		private void Timer_Tick(object? sender, EventArgs e)
		{
			if (timer != null)
			{
				timer.Interval = TimeSpan.FromSeconds(SettingsModel.Instance.SelectedRefreshInterval);
			}

			// --- Update CPU Temperature ---
			var cpuTemperatures = Monitor.Instance.GetCPUTemp();
			float? currentCpuTemp = null;
			if (cpuTemperatures.Any())
			{
				currentCpuTemp = cpuTemperatures.FirstOrDefault(s => s.Name.Equals("CPU Package", StringComparison.OrdinalIgnoreCase) || s.Name.Equals("CPU Total", StringComparison.OrdinalIgnoreCase))?.Value;
				if (!currentCpuTemp.HasValue)
				{
					var coreTemps = cpuTemperatures.Where(s => s.Name.Contains("Core", StringComparison.OrdinalIgnoreCase) && s.Value.HasValue).ToList();
					if (coreTemps.Any())
					{
						currentCpuTemp = coreTemps.Average(s => s.Value.Value);
					}
				}
			}
			UpdateHistory(cpuTemperatureHistory, currentCpuTemp ?? 0);

			// --- Update GPU Temperature ---
			var gpuTemperatures = Monitor.Instance.GetGPUTemp();
			float? currentGpuTemp = null;
			if (gpuTemperatures.Any())
			{
				currentGpuTemp = gpuTemperatures.FirstOrDefault(s =>
					s.Name.Contains("Core", StringComparison.OrdinalIgnoreCase) ||
					s.Name.Contains("Hot Spot", StringComparison.OrdinalIgnoreCase) ||
					s.Name.Contains("Junction", StringComparison.OrdinalIgnoreCase))?.Value;
				if (!currentGpuTemp.HasValue)
				{
					currentGpuTemp = gpuTemperatures.FirstOrDefault(s => s.Value.HasValue)?.Value;
				}
			}
			UpdateHistory(gpuTemperatureHistory, currentGpuTemp ?? 0);

			// --- Update Labels ---
			timeStampCounter++;
			chartLabels.RemoveAt(0);
			chartLabels.Add(timeStampCounter.ToString());

			// --- Update Charts ---
			CpuTemperatureChart.UpdateData(new List<double>(cpuTemperatureHistory), new List<string>(chartLabels));
			GpuTemperatureChart.UpdateData(new List<double>(gpuTemperatureHistory), new List<string>(chartLabels));
		}

		private void UpdateHistory(List<double> history, double newValue)
		{
			history.RemoveAt(0);
			history.Add(newValue);
		}
	}
}
