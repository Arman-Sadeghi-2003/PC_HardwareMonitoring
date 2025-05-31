using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.Models.CPU;
using PC_HardwareMonitoring.Models.Settings;
using PC_HardwareMonitoring.Tools.Global;
using PC_HardwareMonitoring.Tools.Localization;
using PC_HardwareMonitoring.ViewModels.Commons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace PC_HardwareMonitoring.ViewModels.Tabs
{
	public partial class CPUTabViewModel : ViewModelBase
	{
		private readonly DispatcherTimer timer;

		[ObservableProperty]
		private ObservableCollection<ComboBoxItem> _CPU_Models;

		[ObservableProperty]
		private ComboBoxItem? selected_CPU;

		[ObservableProperty]
		private ObservableCollection<ComboBoxItem> _CPUCores;

		[ObservableProperty]
		private ComboBoxItem? selected_CPUCore;

		// CPU Charts model

		[ObservableProperty]
		private ChartViewModel _CPU_Temperature;
		[ObservableProperty]
		private ChartViewModel _CPU_Usage;

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

		#endregion Features

		public CPUTabViewModel()
		{
			timer = new();
			initialize();
		}

		private void initialize()
		{
			int cpuCounter = 1;
			CPU_Models = new(Data.Instance.CPUs.Select(c => new ComboBoxItem() { Content = $"CPU #{cpuCounter++}", Tag = c }));
			Selected_CPU = CPU_Models.FirstOrDefault();

			CPUCores = new(((Selected_CPU?.Tag as CPU_Model)?.CoreNames ?? new List<string>()).Select(i => new ComboBoxItem() { Content = i }));
			Selected_CPUCore = CPUCores.FirstOrDefault();

			// Setup initial chart series
			CPU_Temperature = new(new() { 58, 63, 25, 54, 64, 50, 40, 30, 35, 45, 55, 65, 80, 85, 79, 76, 75, 73, 77, 78, 70 }, "Temperature",
				new()
				{ "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29" });
			CPU_Usage = new(new() { 2, 5, 3, 5, 8, 5, 7, 9, 8, 7, 15, 20, 10, 58, 63, 25, 54, 64, 50, 40, 30, 35, 45, 55, 65 }, "Usage",
				new()
				{ "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29" });

			// Simulate update
			timer.Interval = TimeSpan.FromSeconds(SettingsModel.Instance.SelectedRefreshInterval);
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		private void Timer_Tick(object? sender, EventArgs e)
		{

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