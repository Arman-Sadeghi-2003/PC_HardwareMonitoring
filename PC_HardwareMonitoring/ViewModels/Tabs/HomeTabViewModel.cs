﻿using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.ViewModels.Commons;

namespace PC_HardwareMonitoring.ViewModels.Tabs
{
	public partial class HomeTabViewModel : ViewModelBase
	{
		[ObservableProperty]
		private ViewModelBase testChart;

		public HomeTabViewModel()
		{
			initializeChart();
		}

		private void initializeChart()
		{
			TestChart = new ChartViewModel(new()
				{ 2, 5, 3, 5, -2, 0.5, -6, 8, 5, 7, 9, 8, 7, 15, 20, 10, 58, 63, 25, 54, 64, 50, 40, 30, 35, 45, 55, 65 }, "Test chart",
				new()
				{ "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29" });
		}
	}
}