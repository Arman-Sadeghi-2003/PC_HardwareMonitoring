/// <summary>
/// ViewModel for the CPU tab, displaying CPU-related information and charts.
/// </summary>
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

		/// <summary>
		/// Gets or sets the collection of available CPU models.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<ComboBoxItem> _CPU_Models;

		/// <summary>
		/// Gets or sets the selected CPU model.
		/// </summary>
		[ObservableProperty]
		private ComboBoxItem? selected_CPU;

		/// <summary>
		/// Gets or sets the collection of available CPU cores.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<ComboBoxItem> _CPUCores;

		/// <summary>
		/// Gets or sets the selected CPU core.
		/// </summary>
		[ObservableProperty]
		private ComboBoxItem? selected_CPUCore;

		// CPU Charts model

		/// <summary>
		/// Gets or sets the CPU temperature chart ViewModel.
		/// </summary>
		[ObservableProperty]
		private ChartViewModel _CPU_Temperature;

		/// <summary>
		/// Gets or sets the CPU usage chart ViewModel.
		/// </summary>
		[ObservableProperty]
		private ChartViewModel _CPU_Usage;

		#region Features

		/// <summary>
		/// Gets or sets the CPU name information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_Name;

		/// <summary>
		/// Gets or sets the CPU manufacturer information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_Manufacturer;

		/// <summary>
		/// Gets or sets the CPU description information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_Description;

		/// <summary>
		/// Gets or sets the CPU processor ID information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_ProcessorId;

		/// <summary>
		/// Gets or sets the CPU architecture information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_Architecture;

		/// <summary>
		/// Gets or sets the CPU number of cores information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_NumberOfCores;

		/// <summary>
		/// Gets or sets the CPU number of logical processors information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_NumberOfLogicalProcessors;

		/// <summary>
		/// Gets or sets the CPU maximum clock speed information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_MaxClockSpeed;

		/// <summary>
		/// Gets or sets the CPU current clock speed information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_CurrentClockSpeed;

		/// <summary>
		/// Gets or sets the CPU socket designation information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_SocketDesignation;

		/// <summary>
		/// Gets or sets the CPU L2 cache size information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_L2CacheSize;

		/// <summary>
		/// Gets or sets the CPU L3 cache size information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_L3CacheSize;

		/// <summary>
		/// Gets or sets the CPU thread count information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_ThreadCount;

		/// <summary>
		/// Gets or sets the CPU status information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_Status;

		/// <summary>
		/// Gets or sets the CPU virtualization enabled information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_VirtualizationEnabled;

		/// <summary>
		/// Gets or sets the CPU second level address translation information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_SecondLevelAddressTranslation;

		/// <summary>
		/// Gets or sets the CPU data width information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_DataWidth;

		/// <summary>
		/// Gets or sets the CPU address width information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_AddressWidth;

		/// <summary>
		/// Gets or sets the CPU revision information.
		/// </summary>
		[ObservableProperty]
		private InlineCollection _CPU_Revision;

		#endregion Features

		/// <summary>
		/// Initializes a new instance of the <see cref="CPUTabViewModel"/> class.
		/// </summary>
		public CPUTabViewModel()
		{
			timer = new();
			initialize();
		}

		/// <summary>
		/// Initializes the CPU tab ViewModel, populating CPU models, cores, and setting up charts.
		/// </summary>
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

		/// <summary>
		/// Handles the timer tick event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void Timer_Tick(object? sender, EventArgs e)
		{
			//TODO : Implement the Timer_Tick
		}

		/// <summary>
		/// Handles property changes and updates the UI accordingly.
		/// </summary>
		/// <param name="e">The event arguments containing information about the property change.</param>
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

		/// <summary>
		/// Updates the CPU information displayed based on the selected CPU.
		/// </summary>
		private void selecteCPU()
		{
			// Check if the selected CPU is null or not of type CPU_Model.
			if (Selected_CPU.Tag is not CPU_Model cpu)
				return;

			// Update the CPU information fields.
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

		/// <summary>
		/// Retrieves a localized string and formats it with the provided value.
		/// </summary>
		/// <param name="key">The localization key.</param>
		/// <param name="value">The value to format the string with.</param>
		/// <returns>The formatted <see cref="InlineCollection"/>.</returns>
		private InlineCollection getString(string key, string? value)
		{
			//If value is null or empty, set it to empty string to prevent null reference exceptions
			value = value ?? string.Empty;

			// Get the localized string from the LocalizationManager.
			var leftSide = LocalizationManager.Instance.GetString(key);

			// Create a bold Run with the localized string.
			var l = new Run()
			{
				Text = leftSide + "  ",
				FontWeight = Avalonia.Media.FontWeight.Bold,
			};
			// Create a Run with the provided value.
			var r = new Run()
			{
				Text = value,
			};

			// Return the InlineCollection containing the formatted string.
			return new() { l, r };
		}
	}
}
