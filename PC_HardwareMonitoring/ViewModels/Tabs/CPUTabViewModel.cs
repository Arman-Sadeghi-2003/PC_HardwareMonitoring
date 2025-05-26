using Avalonia.Controls;
using Avalonia.Controls.Documents;
using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Models.CPU;
using PC_HardwareMonitoring.Tools.Global;
using PC_HardwareMonitoring.Tools.Localization;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace PC_HardwareMonitoring.ViewModels.Tabs
{
	public partial class CPUTabViewModel : ViewModelBase
	{
		[ObservableProperty]
		private ObservableCollection<ComboBoxItem> _CPU_Models;

		[ObservableProperty]
		private ComboBoxItem? selected_CPU;

		#region Features

		[ObservableProperty]
		private InlineCollection _CPU_Name;
		[ObservableProperty]
		private InlineCollection _CPU_Manufacturer;
		[ObservableProperty]
		private InlineCollection _CPU_Description;
		[ObservableProperty]
		private InlineCollection _CPU_ProcessorId;
		[ObservableProperty]
		private InlineCollection _CPU_Architecture;
		[ObservableProperty]
		private InlineCollection _CPU_NumberOfCores;
		[ObservableProperty]
		private InlineCollection _CPU_NumberOfLogicalProcessors;
		[ObservableProperty]
		private InlineCollection _CPU_MaxClockSpeed;
		[ObservableProperty]
		private InlineCollection _CPU_CurrentClockSpeed;
		[ObservableProperty]
		private InlineCollection _CPU_SocketDesignation;
		[ObservableProperty]
		private InlineCollection _CPU_L2CacheSize;
		[ObservableProperty]
		private InlineCollection _CPU_L3CacheSize;
		[ObservableProperty]
		private InlineCollection _CPU_ThreadCount;
		[ObservableProperty]
		private InlineCollection _CPU_Status;
		[ObservableProperty]
		private InlineCollection _CPU_VirtualizationEnabled;
		[ObservableProperty]
		private InlineCollection _CPU_SecondLevelAddressTranslation;
		[ObservableProperty]
		private InlineCollection _CPU_DataWidth;
		[ObservableProperty]
		private InlineCollection _CPU_AddressWidth;
		[ObservableProperty]
		private InlineCollection _CPU_Revision;

		#endregion

		public CPUTabViewModel()
		{
			initialize();
		}

		private void initialize()
		{
			int cpuCounter = 1;
			CPU_Models = new(Data.Instance.CPUs.Select(c => new ComboBoxItem() { Content = $"CPU #{cpuCounter++}", Tag = c }));
			Selected_CPU = CPU_Models.FirstOrDefault();
		}

		protected override void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			switch (e.PropertyName)
			{
				case nameof(Selected_CPU):
					{
						selecteCPU();
					}
					break;
			}
		}

		private void selecteCPU()
		{
			if (Selected_CPU.Tag is not CPU_Model cpu)
				return;

			CPU_Name = getString("CPUNameText", cpu.Name);
			CPU_Manufacturer = getString("CPUManufacturerText", cpu.Manufacturer);
			CPU_Description = getString("CPUDescriptionText", cpu.Description);
			CPU_ProcessorId = getString("CPUProcessorIdText", cpu.ProcessorId);
			CPU_Architecture = getString("CPUArchitectureText", cpu.Architecture);
			CPU_NumberOfCores = getString("CPUNumberOfCoresText", cpu.NumberOfCores);
			CPU_NumberOfLogicalProcessors = getString("CPUNumberOfLogicalProcessorsText", cpu.NumberOfLogicalProcessors);
			CPU_MaxClockSpeed = getString("CPUMaxClockSpeedText", cpu.MaxClockSpeed);
			CPU_CurrentClockSpeed = getString("CPUCurrentClockSpeedText", cpu.CurrentClockSpeed);
			CPU_SocketDesignation = getString("CPUSocketDesignationText", cpu.SocketDesignation);
			CPU_L2CacheSize = getString("CPUL2CacheSizeText", cpu.L2CacheSize);
			CPU_L3CacheSize = getString("CPUL3CacheSizeText", cpu.L3CacheSize);
			CPU_ThreadCount = getString("CPUThreadCountText", cpu.ThreadCount);
			CPU_Status = getString("CPUStatusText", cpu.Status);
			CPU_VirtualizationEnabled = getString("CPUVirtualizationEnabledText", cpu.VirtualizationEnabled);
			CPU_SecondLevelAddressTranslation = getString("CPUSecondLevelAddressTranslationText", cpu.SecondLevelAddressTranslation);
			CPU_DataWidth = getString("CPUDataWidthText", cpu.DataWidth);
			CPU_AddressWidth = getString("CPUAddressWidthText", cpu.AddressWidth);
			CPU_Revision = getString("CPURevisionText", cpu.Revision);
		}

		private InlineCollection getString(string key, string? value)
		{
			value = value ?? string.Empty;
			var leftSide = LocalizationManager.Instance.GetString(key);

			var l = new Run()
			{
				Text = leftSide + "  ",
				FontWeight = Avalonia.Media.FontWeight.Bold,
			};
			var r = new Run()
			{
				Text = value,
			};

			return new() { l, r };
		}
	}
}