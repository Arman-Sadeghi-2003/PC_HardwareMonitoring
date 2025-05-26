using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using PC_HardwareMonitoring.Models.CPU;
using PC_HardwareMonitoring.Models.Settings;
using PC_HardwareMonitoring.Tools.Global;
using PC_HardwareMonitoring.Tools.Localization;
using SkiaSharp;
using System;
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

		// CPU Charts model

		[ObservableProperty]
		public ObservableCollection<ISeries> _CPUUsageSeries;
		[ObservableProperty]
		public ObservableCollection<ISeries> _CPUTemperatureSeries;

		[ObservableProperty]
		public ObservableCollection<Axis> _XAxis;
		[ObservableProperty]
		public ObservableCollection<Axis> _YAxis;

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
			timer = new();
			initialize();
		}

		private void initialize()
		{
			int cpuCounter = 1;
			CPU_Models = new(Data.Instance.CPUs.Select(c => new ComboBoxItem() { Content = $"CPU #{cpuCounter++}", Tag = c }));
			Selected_CPU = CPU_Models.FirstOrDefault();

			// Setup initial chart series
			CPUUsageSeries = new ObservableCollection<ISeries>
		{
			new LineSeries<ObservablePoint>
			{
				Values = new ObservableCollection<ObservablePoint>(),
				Fill = null,
				Stroke = new SolidColorPaint(SKColors.Blue, 2)
			}
		};

			CPUTemperatureSeries = new ObservableCollection<ISeries>
		{
			new LineSeries<ObservablePoint>
			{
				Values = new ObservableCollection<ObservablePoint>(),
				Fill = null,
				Stroke = new SolidColorPaint(SKColors.Red, 2)
			}
		};

			XAxis = new ObservableCollection<Axis>
		{
			new Axis { Labeler = value => DateTime.Now.AddSeconds(value).ToString("HH:mm:ss"), MinLimit = 0, MaxLimit = 60 }
		};

			YAxis = new ObservableCollection<Axis> { new Axis() };

			// Simulate update
			timer.Interval = TimeSpan.FromSeconds(SettingsModel.Instance.SelectedRefreshInterval);
			timer.Tick += Timer_Tick;
			timer.Start();
		}

		private void Timer_Tick(object? sender, EventArgs e)
		{
			var usage = GetCurrentCPUUsage();          // e.g., return 40.3
			var temperature = GetCurrentCPUTemperature(); // e.g., return 68.2
			int _timeCounter = 0;

			Avalonia.Threading.Dispatcher.UIThread.Post(() =>
			{
				var usagePoints = (ObservableCollection<ObservablePoint>)CPUUsageSeries[0].Values;
				var tempPoints = (ObservableCollection<ObservablePoint>)CPUTemperatureSeries[0].Values;

				usagePoints.Add(new ObservablePoint(_timeCounter, usage));
				tempPoints.Add(new ObservablePoint(_timeCounter, temperature));

				// Keep last 60 points (1 minute)
				if (usagePoints.Count > 60) usagePoints.RemoveAt(0);
				if (tempPoints.Count > 60) tempPoints.RemoveAt(0);

				_timeCounter++;
			});
		}

		private double GetCurrentCPUUsage()
		{
			// Fetch from your monitoring service or mock it
			return new Random().NextDouble() * 100;
		}

		private double GetCurrentCPUTemperature()
		{
			return 50 + new Random().NextDouble() * 20;
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