using LibreHardwareMonitor.Hardware;
using PC_HardwareMonitoring.Infrastructure.NotificationManager;
using System;
using System.Management;
using System.Text;

namespace PC_HardwareMonitoring.Tools.HW
{
	public class Monitor
	{
		// ----> Singleton instance

		private Monitor()
		{ }

		private static Monitor instance;
		public static Monitor Instance => instance ?? (instance = new Monitor());

		// ----> Properties

		private bool cpuTempSent = false;

		// ----> Methods

		public string GetAllInfos()
		{
			var builder = new StringBuilder();

			Computer computer = new Computer
			{
				IsCpuEnabled = true,
				IsGpuEnabled = true,
				IsMemoryEnabled = true,
				IsMotherboardEnabled = true,
				IsControllerEnabled = true,
				IsNetworkEnabled = true,
				IsStorageEnabled = true,
			};

			computer.Open();
			computer.Accept(new UpdateVisitor());

			foreach (IHardware hardware in computer.Hardware)
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

			computer.Close();
			return builder.ToString();
		}

		public string GetMotherboardInfos()
		{
			var builder = new StringBuilder();

			try
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
			catch (Exception ex)
			{
				builder.Clear();
				builder.AppendLine(ex.Message);
			}

			return builder.ToString();
		}

		public string GetCPUInfos()
		{
			var builder = new StringBuilder();

			try
			{
				builder.AppendLine("CPU Information:\n");

				using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor"))
				{
					foreach (ManagementObject mo in searcher.Get())
					{
						builder.AppendLine($"Name: {mo["Name"]}");
						builder.AppendLine($"Manufacturer: {mo["Manufacturer"]}");
						builder.AppendLine($"Description: {mo["Description"]}");
						builder.AppendLine($"Processor Id: {mo["ProcessorId"]}");
						builder.AppendLine($"Architecture: {mo["Architecture"]}");
						builder.AppendLine($"Number Of Cores: {mo["NumberOfCores"]}");
						builder.AppendLine($"Number Of Logical Processors: {mo["NumberOfLogicalProcessors"]}");
						builder.AppendLine($"Max Clock Speed: {mo["MaxClockSpeed"]} MHz");
						builder.AppendLine($"Current Clock Speed: {mo["CurrentClockSpeed"]} MHz");
						builder.AppendLine($"Socket Designation: {mo["SocketDesignation"]}");
						builder.AppendLine($"L2 Cache Size: {mo["L2CacheSize"]} KB");
						builder.AppendLine($"L3 Cache Size: {mo["L3CacheSize"]} KB");
						builder.AppendLine($"Thread Count: {mo["ThreadCount"]}");
						builder.AppendLine($"Status: {mo["Status"]}");
						builder.AppendLine($"Virtualization Enabled: {mo["VirtualizationFirmwareEnabled"]}");
						builder.AppendLine($"Second Level Address Translation: {mo["SecondLevelAddressTranslationExtensions"]}");
						builder.AppendLine($"Data Width: {mo["DataWidth"]} bits");
						builder.AppendLine($"Address Width: {mo["AddressWidth"]} bits");
						builder.AppendLine($"Revision: {mo["Revision"]}");
						builder.AppendLine();
					}
				}
			}
			catch (Exception ex)
			{
				builder.Clear();
				builder.AppendLine(ex.Message);
			}

			return builder.ToString();
		}

		public string GetCPUTemp()
		{
			var builder = new StringBuilder();

			try
			{
				Computer computer = new Computer
				{
					IsCpuEnabled = true
				};
				computer.Open();

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
								if (sensor.Value >= 55 && sensor.Name == "Core Max" && !cpuTempSent)
								{
									NotificationGenerator.Instance.ShowCPUTempWarning(App.MainTopLevel, sensor.Value ?? 0);
									cpuTempSent = true;
								}
							}
						}
					}
				}

				computer.Close();
			}
			catch (Exception ex)
			{
				builder.Clear();
				builder.AppendLine(ex.Message);
			}

			return builder.ToString();
		}

		public string GetCPUUsage()
		{
			var builder = new StringBuilder();

			try
			{
				Computer computer = new Computer
				{
					IsCpuEnabled = true
				};
				computer.Open();

				foreach (var hardware in computer.Hardware)
				{
					if (hardware.HardwareType == HardwareType.Cpu)
					{
						hardware.Update();
						foreach (var sensor in hardware.Sensors)
						{
							if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("Core"))
							{
								builder.AppendLine($"{sensor.Name}: {sensor.Value}");
								if (sensor.Value >= 55 && sensor.Name == "Core Max" && !cpuTempSent)
								{
									NotificationGenerator.Instance.ShowCPUTempWarning(App.MainTopLevel, sensor.Value ?? 0);
									cpuTempSent = true;
								}
							}
						}
					}
				}

				computer.Close();
			}
			catch (Exception ex)
			{
				builder.Clear();
				builder.AppendLine(ex.Message);
			}

			return builder.ToString();
		}

		public string GetMemoryInfos()
		{
			var builder = new StringBuilder();

			using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory"))
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

			return builder.ToString();
		}

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
				}
			}
			catch (Exception ex)
			{
				builder.Clear();
				builder.AppendLine(ex.Message);
			}

			return builder.ToString();
		}

		public string GetGPUTemp()
		{
			var builder = new StringBuilder();

			try
			{
				var computer = new Computer
				{
					IsGpuEnabled = true
				};
				computer.Open();

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

				computer.Close();
			}
			catch (Exception ex)
			{
				builder.Clear();
				builder.AppendLine(ex.Message);
			}

			return builder.ToString();
		}

		private string FormatBytes(object bytesObj)
		{
			if (bytesObj == null || !ulong.TryParse(bytesObj.ToString(), out var bytes))
				return "Unknown";
			return $"{bytes / 1024 / 1024} MB";
		}
	}
}