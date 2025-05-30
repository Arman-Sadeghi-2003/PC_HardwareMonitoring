using CommunityToolkit.Mvvm.ComponentModel;
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
				{ 2, 5, 3, 5, -2, 0.5, -6, 8, 5, 7, 9, 8, 7, 15, 20, 10, 58, 63, 25, 54, 64, 50, 40, 30, 35, 45, 55, 65 });
		}
	}
}