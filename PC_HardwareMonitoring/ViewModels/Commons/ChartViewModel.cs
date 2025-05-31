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

		[ObservableProperty]
		private ObservableCollection<Axis> xAxis = new();

		[ObservableProperty]
		private string title = "";

		public ChartViewModel(List<double> series, string title, List<string> axisLabel)
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

			XAxis = new()
			{
				new Axis()
				{
					Name = title,
					NamePaint = new SolidColorPaint(SKColors.DodgerBlue),
					Labels = axisLabel,
					LabelsPaint = new SolidColorPaint(SKColors.Firebrick),
					Labeler = Labelers.SixRepresentativeDigits,
				}
			};

			Title = title;
		}
	}
}
