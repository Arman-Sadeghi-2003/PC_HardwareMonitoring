/// <summary>
/// ViewModel for the GPU tab, displaying GPU-related information.
/// </summary>
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Models.Settings;    // Added
using PC_HardwareMonitoring.Tools.HW;
using PC_HardwareMonitoring.ViewModels.Commons; // Added
using System;                                   // Added for StringComparison
using System.Collections.Generic;               // Added
using System.Linq;                              // Added

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
		/// Gets or sets the GPU temperature chart ViewModel.
		/// </summary>
		[ObservableProperty]
		private ChartViewModel _GPUTemperatureChart;

		[ObservableProperty]
		private string _gpuLoad;

		[ObservableProperty]
		private string _gpuMemoryUsage;

		[ObservableProperty]
		private string _gpuFanSpeed;

		private DispatcherTimer timer;
		private const int MaxDataPoints = 30; // Max points to display on the chart
		private readonly List<double> gpuTemperatureHistory = new();
		private readonly List<string> chartLabels = new();
		private int timeStampCounter = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="GPUTabViewModel"/> class.
		/// </summary>
		public GPUTabViewModel()
		{
			GPU_Infos = Monitor.Instance.GetGPUInfos();

			// Initialize history lists and labels
			for (int i = 0; i < MaxDataPoints; i++)
			{
				gpuTemperatureHistory.Add(0); // Initialize with 0
				chartLabels.Add((i + 1).ToString());
			}

			GPUTemperatureChart = new ChartViewModel(
				new List<double>(gpuTemperatureHistory),
				"GPU Temperature (°C)", // Chart Title
				new List<string>(chartLabels),
				"GPU Core Temperature"); // Series Name

			timer = new DispatcherTimer();
			timer.Interval = System.TimeSpan.FromSeconds(SettingsModel.Instance.SelectedRefreshInterval);
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
			if (timer != null)
			{
				timer.Interval = System.TimeSpan.FromSeconds(SettingsModel.Instance.SelectedRefreshInterval);
			}

			// Update Temperature Chart
			var gpuTempSensors = Monitor.Instance.GetGPUTemp(); // Still use this for focused temp chart
			float? currentGpuTempForChart = null;
			if (gpuTempSensors.Any())
			{
				currentGpuTempForChart = gpuTempSensors.FirstOrDefault(s =>
					s.Name.Contains("Core", StringComparison.OrdinalIgnoreCase) ||
					s.Name.Contains("Hot Spot", StringComparison.OrdinalIgnoreCase) ||
					s.Name.Contains("Junction", StringComparison.OrdinalIgnoreCase))?.Value;
				if (!currentGpuTempForChart.HasValue)
				{
					currentGpuTempForChart = gpuTempSensors.FirstOrDefault(s => s.Value.HasValue)?.Value;
				}
			}
			UpdateHistory(gpuTemperatureHistory, currentGpuTempForChart ?? 0);

			timeStampCounter++;
			chartLabels.RemoveAt(0);
			chartLabels.Add(timeStampCounter.ToString());
			GPUTemperatureChart.UpdateData(new List<double>(gpuTemperatureHistory), new List<string>(chartLabels));

			// Update other GPU Sensors
			var gpuAllSensors = Monitor.Instance.GetGPUSensors();

			var loadSensor = gpuAllSensors.FirstOrDefault(s => s.Name.Contains("Load", StringComparison.OrdinalIgnoreCase) && s.Name.Contains("Core"));
			GpuLoad = loadSensor != null && loadSensor.Value.HasValue ? $"{loadSensor.Value:F1}%" : "N/A";

			// VRAM Usage can be tricky. Sensor names vary: "Memory Bus Load", "Memory Used", "D3D Primary VidMem Used"
			// SmallData is a generic type, so we rely on names.
			var memorySensor = gpuAllSensors.FirstOrDefault(s =>
				s.Name.Contains("Memory", StringComparison.OrdinalIgnoreCase) && (s.Name.Contains("Usage") || s.Name.Contains("Used") || s.Name.Contains("Load")));
			// Sometimes memory is reported in MB or GB directly by SmallData sensors
			if (memorySensor != null && memorySensor.Value.HasValue)
			{
				// Heuristic: if value is large, it might be MB. If small (<1000 usually), it might be %.
				// This part is highly dependent on LibreHardwareMonitor's output for specific GPUs.
				if (memorySensor.Name.Contains("%") || memorySensor.Value <= 100) // Assuming it's a percentage
				{
					GpuMemoryUsage = $"{memorySensor.Value:F1}%";
				}
				else // Assuming it's in MB or similar unit
				{
					// We need total VRAM to calculate percentage if only used memory is given.
					// This might be part of GPU_Infos or require another specific sensor.
					// For now, just display the value.
					GpuMemoryUsage = $"{memorySensor.Value:F0} MB (Used)"; // Placeholder, ideally need total to make it "X / Y MB" or percentage
				}
			}
			else
			{
				GpuMemoryUsage = "N/A";
			}

			var fanSensor = gpuAllSensors.FirstOrDefault(s => s.Name.Contains("Fan", StringComparison.OrdinalIgnoreCase) && (s.Name.Contains("Speed") || s.Name.Contains("%")));
			if (fanSensor != null && fanSensor.Value.HasValue)
			{
				GpuFanSpeed = fanSensor.Name.Contains("%") ? $"{fanSensor.Value:F1}%" : $"{fanSensor.Value:F0} RPM";
			}
			else
			{
				GpuFanSpeed = "N/A";
			}
		}

		private void UpdateHistory(List<double> history, double newValue)
		{
			history.RemoveAt(0);
			history.Add(newValue);
		}
	}
}
