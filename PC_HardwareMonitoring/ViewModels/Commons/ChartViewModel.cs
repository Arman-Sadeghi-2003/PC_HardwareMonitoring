using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PC_HardwareMonitoring.ViewModels.Commons
{
	public partial class ChartViewModel : ViewModelBase
	{
		[ObservableProperty]
		private ObservableCollection<ISeries> series = new();

		public ChartViewModel(List<double> series)
		{
			Series = new()
			{
				new LineSeries<double>()
				{
					Values = series,
					Fill = null,
					Stroke = new SolidColorPaint(SKColors.Firebrick),
				}
			};
		}
	}
}
