/// <summary>
/// ViewModel for displaying charts.
/// </summary>
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
		/// <summary>
		/// The series of data to be displayed on the chart.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<ISeries> series = new();

		/// <summary>
		/// The X-axis configuration for the chart.
		/// </summary>
		[ObservableProperty]
		private ObservableCollection<Axis> xAxis = new();

		/// <summary>
		/// The title of the chart.
		/// </summary>
		[ObservableProperty]
		private string title = "";

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartViewModel"/> class.
		/// </summary>
		/// <param name="seriesData">The data series for the chart.</param>
		/// <param name="title">The title of the chart.</param>
		/// <param name="axisLabel">The labels for the X-axis.</param>
		public ChartViewModel(List<double> seriesData, string title, List<string> axisLabel)
		{
			// Create a new line series with the provided data.
			Series = new()
			{
				new LineSeries<double>()
				{
					Values = seriesData,
					Fill = null,
					Stroke = new SolidColorPaint(SKColors.Firebrick),
				}
			};

			// Configure the X-axis with the provided title and labels.
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
