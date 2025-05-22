using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace PC_HardwareMonitoring.ViewModels.Tabs
{
	public partial class HomeTabViewModel : ViewModelBase
	{
		[ObservableProperty]
		private ISeries[] _CPU_Series;

		[ObservableProperty]
		private Axis[] _xAxis;

		public HomeTabViewModel()
		{
			initializeChart();
		}

		private void initializeChart()
		{
			XAxis = new Axis[]
			{
				new Axis
				{
					Name = "sample",
					NamePaint = new SolidColorPaint(SKColors.DodgerBlue),
					Labels = ["first", "second", "third", "fourth", "fifth", "sixth", "seventh", "ninth"],
					LabelsPaint = new SolidColorPaint(SKColors.Firebrick),
					Labeler = Labelers.Currency,
				}
			};

			CPU_Series = new ISeries[]
			{
				new StepLineSeries<double>
				{
					Values = new double[] {2,5,3,5,-2,0.5,-6,8,5,7,9,8,7,15,20,10,58,63,25,54,64,50,40,30,35,45,55,65},
				}
			};
		}
	}
}

