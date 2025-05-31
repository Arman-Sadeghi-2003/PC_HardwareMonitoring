using LibreHardwareMonitor.Hardware;
using PC_HardwareMonitoring.Infrastructure.NotificationManager;
using PC_HardwareMonitoring.Models.CPU;
using PC_HardwareMonitoring.Tools.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace PC_HardwareMonitoring.Tools.HW
{
	/// <summary>
	/// Singleton class for monitoring hardware components including CPU, GPU, memory, and motherboard.
	/// Provides comprehensive hardware information retrieval and temperature monitoring with notifications.
	/// </summary>
	public class Monitor
	{
		#region Singleton Implementation

		private Monitor() { }

		private static Monitor instance;

		/// <summary>
		/// Gets the singleton instance of the Monitor class.
		/// </summary>
		public static Monitor Instance => instance ?? (instance = new Monitor());

		#endregion

		#region Private Fields

		private bool cpuTempSent = false;
		private const int CPU_TEMP_THRESHOLD = 55;
		private const int CPU_USAGE_THRESHOLD = 55;

		#endregion

		#region Public Methods - General Hardware Information

		/// <summary>
		/// Retrieves comprehensive information about all hardware components in the system.
		/// </summary>
		/// <returns>A formatted string containing detailed hardware information including sensors data.</returns>
		public string GetAllInfos()
		{
			var builder = new StringBuilder();

			try
			{
				var computer = CreateComputer(true, true, true, true, true, true, true);
				computer.Open();
				computer.Accept(new UpdateVisitor());

				foreach (IHardware hardware in computer.Hardware)
				{
					AppendHardwareInfo(builder, hardware);
				}

				computer.Close();
			}
			catch (Exception ex)
			{
				builder.Clear();
				builder.AppendLine($"Error retrieving hardware information: {ex.Message}");
			}

			return builder.ToString();
		}

		#endregion

		#region Public Methods - Motherboard Information

		/// <summary>
		/// Retrieves detailed motherboard information including manufacturer, model, and device specifications.
		/// </summary>
		/// <returns>A formatted string containing motherboard details and logical device information.</returns>
		public string GetMotherboardInfos()
		{
			var builder = new StringBuilder();

			try
			{
				AppendMotherboardBasicInfo(builder);
				AppendMotherboardDeviceInfo(builder);
			}
			catch (Exception ex)
			{
				builder.Clear();
				builder.AppendLine($"Error retrieving motherboard information: {ex.Message}");
			}

			return builder.ToString();
		}

		#endregion

		#region Public Methods - CPU Information

		/// <summary>
		/// Retrieves and stores comprehensive CPU information including specifications, cache sizes, and core details.
		/// Populates the global CPU data collection with detailed processor information.
		/// </summary>
		public void GetCPUInfos()
		{
			try
			{
				using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
				{
					foreach (ManagementObject mo in searcher.Get())
					{
						var model = CreateCPUModel(mo);
						model.CoreNames = GetCPUCoreNames();
						Data.Instance.CPUs.Add(model);
					}
				}
			}
			catch (Exception ex)
			{
				NotificationGenerator.Instance.ShowExceptionMessage(App.MainTopLevel, ex.Message);
			}
		}

		/// <summary>
		/// Retrieves the current temperature readings for all CPU cores.
		/// Triggers temperature warning notifications if thresholds are exceeded.
		/// </summary>
		/// <returns>A formatted string containing temperature readings for each CPU core.</returns>
		public string GetCPUTemp()
		{
			var builder = new StringBuilder();

			try
			{
				var computer = CreateComputer(isCpuEnabled: true);
				computer.Open();
				ProcessCPUTemperatureSensors(computer, builder);
				computer.Close();
			}
			catch (Exception ex)
			{
				builder.Clear();
				builder.AppendLine($"Error retrieving CPU temperature: {ex.Message}");
			}

			return builder.ToString();
		}

		/// <summary>
		/// Retrieves the current CPU usage/load percentages for all cores.
		/// Monitors CPU load and can trigger notifications for high usage scenarios.
		/// </summary>
		/// <returns>A formatted string containing CPU load percentages for each core.</returns>
		public string GetCPUUsage()
		{
			var builder = new StringBuilder();

			try
			{
				var computer = CreateComputer(isCpuEnabled: true);
				computer.Open();
				ProcessCPUUsageSensors(computer, builder);
				computer.Close();
			}
			catch (Exception ex)
			{
				builder.Clear();
				builder.AppendLine($"Error retrieving CPU usage: {ex.Message}");
			}

			return builder.ToString();
		}

		#endregion

		#region Public Methods - Memory Information

		/// <summary>
		/// Retrieves detailed information about all installed physical memory modules.
		/// </summary>
		/// <returns>A formatted string containing memory slot details, capacity, speed, and total installed RAM.</returns>
		public string GetMemoryInfos()
		{
			var builder = new StringBuilder();

			try
			{
				using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory"))
				{
					ProcessMemoryModules(searcher, builder);
				}
			}
			catch (Exception ex)
			{
				builder.Clear();
				builder.AppendLine($"Error retrieving memory information: {ex.Message}");
			}

			return builder.ToString();
		}

		#endregion

		#region Public Methods - GPU Information

		/// <summary>
		/// Retrieves comprehensive information about all installed graphics processing units.
		/// </summary>
		/// <returns>A formatted string containing GPU specifications, memory, driver version, and display settings.</returns>
		public string GetGPUInfos()
		{
			var builder = new StringBuilder();

			try
			{
				builder.AppendLine("GPU Information:\n");

				using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController"))
				{
					foreach (ManagementObject mo in searcher.Get())
					{
						AppendGPUInfo(builder, mo);
					}
				}
			}
			catch (Exception ex)
			{
				builder.Clear();
				builder.AppendLine($"Error retrieving GPU information: {ex.Message}");
			}

			return builder.ToString();
		}

		/// <summary>
		/// Retrieves current temperature readings for all supported graphics cards.
		/// Supports NVIDIA, AMD, and Intel GPU temperature monitoring.
		/// </summary>
		/// <returns>A formatted string containing GPU temperature readings.</returns>
		public string GetGPUTemp()
		{
			var builder = new StringBuilder();

			try
			{
				var computer = CreateComputer(isGpuEnabled: true);
				computer.Open();
				ProcessGPUTemperatureSensors(computer, builder);
				computer.Close();
			}
			catch (Exception ex)
			{
				builder.Clear();
				builder.AppendLine($"Error retrieving GPU temperature: {ex.Message}");
			}

			return builder.ToString();
		}

		#endregion

		#region Private Methods - CPU Core Detection

		/// <summary>
		/// Retrieves the names of all CPU cores and logical processors in the system.
		/// Uses WMI to query processor information and generates core names based on available data.
		/// </summary>
		/// <returns>A list of strings representing individual CPU core names.</returns>
		private List<string> GetCPUCoreNames()
		{
			var coreNames = new List<string>();

			try
			{
				// First, try to get core names from Win32_Processor
				using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
				{
					foreach (ManagementObject mo in searcher.Get())
					{
						var numberOfCores = Convert.ToInt32(mo["NumberOfCores"] ?? 0);
						var numberOfLogicalProcessors = Convert.ToInt32(mo["NumberOfLogicalProcessors"] ?? 0);
						var processorName = mo["Name"]?.ToString() ?? "Unknown Processor";

						// Generate core names based on physical and logical core count
						for (int i = 0; i < numberOfCores; i++)
						{
							if (numberOfLogicalProcessors > numberOfCores)
							{
								// Hyperthreading enabled - each physical core has multiple logical processors
								int threadsPerCore = numberOfLogicalProcessors / numberOfCores;
								for (int j = 0; j < threadsPerCore; j++)
								{
									coreNames.Add($"Core {i}, Thread {j}");
								}
							}
							else
							{
								// No hyperthreading - one logical processor per core
								coreNames.Add($"Core {i}");
							}
						}
						break; // Assume single CPU for now
					}
				}

				// If no cores were found, try alternative method using performance counters
				if (coreNames.Count == 0)
				{
					coreNames = GetCoreNamesFromPerformanceCounters();
				}

				// If still no cores found, create default core names
				if (coreNames.Count == 0)
				{
					int coreCount = Environment.ProcessorCount;
					for (int i = 0; i < coreCount; i++)
					{
						coreNames.Add($"Core {i}");
					}
				}
			}
			catch (Exception ex)
			{
				// Fallback: create default core names based on processor count
				int coreCount = Environment.ProcessorCount;
				coreNames.Clear();
				for (int i = 0; i < coreCount; i++)
				{
					coreNames.Add($"Core {i}");
				}
			}

			return coreNames;
		}

		/// <summary>
		/// Alternative method to retrieve CPU core names using performance counter data.
		/// </summary>
		/// <returns>A list of CPU core names derived from performance counters.</returns>
		private List<string> GetCoreNamesFromPerformanceCounters()
		{
			var coreNames = new List<string>();

			try
			{
				// Try to get processor information from Win32_PerfRawData_PerfOS_Processor
				using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfRawData_PerfOS_Processor"))
				{
					var processors = searcher.Get().Cast<ManagementObject>()
						.Where(mo => mo["Name"]?.ToString() != "_Total")
						.OrderBy(mo => mo["Name"]?.ToString())
						.ToList();

					foreach (var processor in processors)
					{
						var name = processor["Name"]?.ToString();
						if (!string.IsNullOrEmpty(name) && name != "_Total")
						{
							coreNames.Add($"Processor {name}");
						}
					}
				}
			}
			catch
			{
				// If this method fails, return empty list to trigger fallback
			}

			return coreNames;
		}

		#endregion

		#region Private Methods - Hardware Processing

		/// <summary>
		/// Creates a Computer instance with specified hardware monitoring capabilities.
		/// </summary>
		private Computer CreateComputer(bool isCpuEnabled = false, bool isGpuEnabled = false,
			bool isMemoryEnabled = false, bool isMotherboardEnabled = false,
			bool isControllerEnabled = false, bool isNetworkEnabled = false, bool isStorageEnabled = false)
		{
			return new Computer
			{
				IsCpuEnabled = isCpuEnabled,
				IsGpuEnabled = isGpuEnabled,
				IsMemoryEnabled = isMemoryEnabled,
				IsMotherboardEnabled = isMotherboardEnabled,
				IsControllerEnabled = isControllerEnabled,
				IsNetworkEnabled = isNetworkEnabled,
				IsStorageEnabled = isStorageEnabled,
			};
		}

		/// <summary>
		/// Appends hardware information including sensors data to the provided StringBuilder.
		/// </summary>
		private void AppendHardwareInfo(StringBuilder builder, IHardware hardware)
		{
			builder.AppendLine($"Hardware: {hardware.Name}");

			foreach (IHardware subhardware in hardware.SubHardware)
			{
				builder.AppendLine($"\tSubhardware: {subhardware.Name}");
				foreach (ISensor sensor in subhardware.Sensors)
				{
					builder.AppendLine($"\t\tSensor: {sensor.Name}, value: {sensor.Value}");
				}
			}

			foreach (ISensor sensor in hardware.Sensors)
			{
				builder.AppendLine($"\tSensor: {sensor.Name}, value: {sensor.Value}");
			}
		}

		/// <summary>
		/// Appends basic motherboard information to the StringBuilder.
		/// </summary>
		private void AppendMotherboardBasicInfo(StringBuilder builder)
		{
			builder.AppendLine("Motherboard Information:\n");

			using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard"))
			{
				foreach (ManagementObject mo in searcher.Get())
				{
					builder.AppendLine($"Manufacturer: {mo["Manufacturer"]}");
					builder.AppendLine($"Product: {mo["Product"]}");
					builder.AppendLine($"Model: {mo["Model"]}");
					builder.AppendLine($"Version: {mo["Version"]}");
					builder.AppendLine($"Serial Number: {mo["SerialNumber"]}");
					builder.AppendLine($"Powered On: {mo["PoweredOn"]}");
					builder.AppendLine($"Hosting Board: {mo["HostingBoard"]}");
					builder.AppendLine();
				}
			}
		}

		/// <summary>
		/// Appends motherboard device information to the StringBuilder.
		/// </summary>
		private void AppendMotherboardDeviceInfo(StringBuilder builder)
		{
			builder.AppendLine("Motherboard Device (Logical Info):\n");

			using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_MotherboardDevice"))
			{
				foreach (ManagementObject mo in searcher.Get())
				{
					builder.AppendLine($"Primary Bus Type: {mo["PrimaryBusType"]}");
					builder.AppendLine($"Secondary Bus Type: {mo["SecondaryBusType"]}");
					builder.AppendLine($"Status: {mo["Status"]}");
					builder.AppendLine($"Availability: {mo["Availability"]}");
					builder.AppendLine();
				}
			}
		}

		/// <summary>
		/// Creates a CPU model object from WMI ManagementObject data.
		/// </summary>
		private CPU_Model CreateCPUModel(ManagementObject mo)
		{
			return new CPU_Model()
			{
				Name = mo["Name"]?.ToString() ?? "Unknown CPU",
				Manufacturer = mo["Manufacturer"]?.ToString() ?? "",
				Description = mo["Description"]?.ToString() ?? "",
				ProcessorId = mo["ProcessorId"]?.ToString() ?? "",
				Architecture = mo["Architecture"]?.ToString() ?? "",
				NumberOfCores = mo["NumberOfCores"]?.ToString() ?? "",
				NumberOfLogicalProcessors = mo["NumberOfLogicalProcessors"]?.ToString() ?? "",
				MaxClockSpeed = $"{mo["MaxClockSpeed"]} MHz",
				CurrentClockSpeed = $"{mo["CurrentClockSpeed"]} MHz",
				SocketDesignation = mo["SocketDesignation"]?.ToString() ?? "",
				L2CacheSize = $"{mo["L2CacheSize"]} KB",
				L3CacheSize = $"{mo["L3CacheSize"]} KB",
				ThreadCount = mo["ThreadCount"]?.ToString() ?? "",
				Status = mo["Status"]?.ToString() ?? "",
				VirtualizationEnabled = mo["VirtualizationFirmwareEnabled"]?.ToString() ?? "",
				SecondLevelAddressTranslation = mo["SecondLevelAddressTranslationExtensions"]?.ToString() ?? "",
				DataWidth = $"{mo["DataWidth"]} bits",
				AddressWidth = $"{mo["AddressWidth"]} bits",
				Revision = mo["Revision"]?.ToString() ?? "",
			};
		}

		/// <summary>
		/// Processes CPU temperature sensors and handles temperature warnings.
		/// </summary>
		private void ProcessCPUTemperatureSensors(Computer computer, StringBuilder builder)
		{
			foreach (var hardware in computer.Hardware)
			{
				if (hardware.HardwareType == HardwareType.Cpu)
				{
					hardware.Update();
					foreach (var sensor in hardware.Sensors)
					{
						if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("Core"))
						{
							builder.AppendLine($"{sensor.Name}: {sensor.Value} °C");
							CheckTemperatureThreshold(sensor);
						}
					}
				}
			}
		}

		/// <summary>
		/// Processes CPU usage sensors and handles high usage warnings.
		/// </summary>
		private void ProcessCPUUsageSensors(Computer computer, StringBuilder builder)
		{
			foreach (var hardware in computer.Hardware)
			{
				if (hardware.HardwareType == HardwareType.Cpu)
				{
					hardware.Update();
					foreach (var sensor in hardware.Sensors)
					{
						if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("Core"))
						{
							builder.AppendLine($"{sensor.Name}: {sensor.Value}%");
							CheckUsageThreshold(sensor);
						}
					}
				}
			}
		}

		/// <summary>
		/// Processes memory modules and appends formatted information to StringBuilder.
		/// </summary>
		private void ProcessMemoryModules(ManagementObjectSearcher searcher, StringBuilder builder)
		{
			int slotCount = 0;
			ulong totalCapacity = 0;

			foreach (ManagementObject mo in searcher.Get())
			{
				slotCount++;
				var capacity = Convert.ToUInt64(mo["Capacity"]);
				totalCapacity += capacity;

				builder.AppendLine($"Slot {slotCount}:");
				builder.AppendLine($"  Capacity: {capacity / (1024 * 1024)} MB");
				builder.AppendLine($"  Manufacturer: {mo["Manufacturer"]}");
				builder.AppendLine($"  Part Number: {mo["PartNumber"]}");
				builder.AppendLine($"  Speed: {mo["Speed"]} MHz");
				builder.AppendLine($"  Serial Number: {mo["SerialNumber"]}");
				builder.AppendLine();
			}

			builder.AppendLine($"Total Slots Detected: {slotCount}");
			builder.AppendLine($"Total Installed RAM: {totalCapacity / (1024 * 1024)} MB");
		}

		/// <summary>
		/// Appends GPU information to the StringBuilder from ManagementObject data.
		/// </summary>
		private void AppendGPUInfo(StringBuilder builder, ManagementObject mo)
		{
			builder.AppendLine($"Name: {mo["Name"]}");
			builder.AppendLine($"Device ID: {mo["DeviceID"]}");
			builder.AppendLine($"Adapter RAM: {FormatBytes(mo["AdapterRAM"])}");
			builder.AppendLine($"Adapter DAC Type: {mo["AdapterDACType"]}");
			builder.AppendLine($"Driver Version: {mo["DriverVersion"]}");
			builder.AppendLine($"Video Processor: {mo["VideoProcessor"]}");
			builder.AppendLine($"Video Architecture: {mo["VideoArchitecture"]}");
			builder.AppendLine($"Video Memory Type: {mo["VideoMemoryType"]}");
			builder.AppendLine($"Current Resolution: {mo["CurrentHorizontalResolution"]} x {mo["CurrentVerticalResolution"]}");
			builder.AppendLine($"Refresh Rate: {mo["CurrentRefreshRate"]} Hz");
			builder.AppendLine($"Status: {mo["Status"]}");
			builder.AppendLine();
		}

		/// <summary>
		/// Processes GPU temperature sensors and appends readings to StringBuilder.
		/// </summary>
		private void ProcessGPUTemperatureSensors(Computer computer, StringBuilder builder)
		{
			foreach (IHardware hardware in computer.Hardware)
			{
				if (hardware.HardwareType == HardwareType.GpuNvidia ||
					hardware.HardwareType == HardwareType.GpuAmd ||
					hardware.HardwareType == HardwareType.GpuIntel)
				{
					hardware.Update();
					builder.AppendLine($"GPU: {hardware.Name}");

					foreach (ISensor sensor in hardware.Sensors)
					{
						if (sensor.SensorType == SensorType.Temperature)
						{
							builder.AppendLine($"  {sensor.Name}: {sensor.Value} °C");
						}
					}
				}
			}
		}

		/// <summary>
		/// Checks CPU temperature against threshold and triggers warnings if necessary.
		/// </summary>
		private void CheckTemperatureThreshold(ISensor sensor)
		{
			if (sensor.Value >= CPU_TEMP_THRESHOLD && sensor.Name == "Core Max" && !cpuTempSent)
			{
				NotificationGenerator.Instance.ShowCPUTempWarning(App.MainTopLevel, sensor.Value ?? 0);
				cpuTempSent = true;
			}
		}

		/// <summary>
		/// Checks CPU usage against threshold and triggers warnings if necessary.
		/// </summary>
		private void CheckUsageThreshold(ISensor sensor)
		{
			if (sensor.Value >= CPU_USAGE_THRESHOLD && sensor.Name == "Core Max" && !cpuTempSent)
			{
				NotificationGenerator.Instance.ShowCPUTempWarning(App.MainTopLevel, sensor.Value ?? 0);
				cpuTempSent = true;
			}
		}

		/// <summary>
		/// Formats byte values into a human-readable format.
		/// </summary>
		/// <param name="bytesObj">The byte value to format (as object).</param>
		/// <returns>A formatted string representing the byte value in MB, or "Unknown" if invalid.</returns>
		private string FormatBytes(object bytesObj)
		{
			if (bytesObj == null || !ulong.TryParse(bytesObj.ToString(), out var bytes))
				return "Unknown";
			return $"{bytes / 1024 / 1024} MB";
		}

		#endregion
	}
}