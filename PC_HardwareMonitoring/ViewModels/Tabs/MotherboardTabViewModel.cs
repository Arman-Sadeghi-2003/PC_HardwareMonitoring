/// <summary>
/// ViewModel for the Motherboard tab, displaying motherboard-related information.
/// </summary>
using Avalonia.Threading; // Added
using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Models.Hardware; // Added
using PC_HardwareMonitoring.Models.Settings; // Added
using PC_HardwareMonitoring.Tools.HW;
using System; // Added
using System.Collections.Generic; // Added
using System.Collections.ObjectModel; // Added
using System.Linq; // Added

namespace PC_HardwareMonitoring.ViewModels.Tabs
{
	public partial class MotherboardTabViewModel : ViewModelBase
	{
		/// <summary>
		/// Gets or sets the motherboard information.
		/// </summary>
		[ObservableProperty]
		private string _motherboardInfos;

		[ObservableProperty]
		private ObservableCollection<SensorValue> _motherboardTemperatures;

		[ObservableProperty]
		private ObservableCollection<SensorValue> _motherboardFanSpeeds;

		private DispatcherTimer timer;

		/// <summary>
		/// Initializes a new instance of the <see cref="MotherboardTabViewModel"/> class.
		/// </summary>
		public MotherboardTabViewModel()
		{
			MotherboardInfos = Monitor.Instance.GetMotherboardInfos();
			MotherboardTemperatures = new ObservableCollection<SensorValue>();
			MotherboardFanSpeeds = new ObservableCollection<SensorValue>();

			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(SettingsModel.Instance.SelectedRefreshInterval);
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		private void Timer_Tick(object? sender, EventArgs e)
		{
			if (timer != null)
			{
				timer.Interval = TimeSpan.FromSeconds(SettingsModel.Instance.SelectedRefreshInterval);
			}

			var sensors = Monitor.Instance.GetMotherboardSensors();

			// Efficiently update observable collections
			UpdateSensorCollection(MotherboardTemperatures, sensors.Where(s => s.Name.Contains("Temperature", StringComparison.OrdinalIgnoreCase) || s.Name.Contains("Temp", StringComparison.OrdinalIgnoreCase) || s.Name.EndsWith("°C")));
			UpdateSensorCollection(MotherboardFanSpeeds, sensors.Where(s => s.Name.Contains("Fan", StringComparison.OrdinalIgnoreCase) || s.Name.EndsWith("RPM")));
		}

		private void UpdateSensorCollection(ObservableCollection<SensorValue> collection, IEnumerable<SensorValue> newValues)
		{
			var newValuesList = newValues.ToList();
			// Remove items not in newValues
			for (int i = collection.Count - 1; i >= 0; i--)
			{
				if (!newValuesList.Any(nv => nv.Name == collection[i].Name))
				{
					collection.RemoveAt(i);
				}
			}

			// Add or update items
			foreach (var newValue in newValuesList)
			{
				var existingItem = collection.FirstOrDefault(item => item.Name == newValue.Name);
				if (existingItem != null)
				{
					existingItem.Value = newValue.Value;
				}
				else
				{
					collection.Add(newValue);
				}
			}
		}
	}
}
