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
		public ChartViewModel(List<double> seriesData, string title, List<string> axisLabel, string seriesName = "Values")
		{
			// Create a new line series with the provided data.
			Series = new()
			{
				new LineSeries<double>()
				{
					Name = seriesName, // Added series name
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
					Name = title, // X-axis title (often the general category, like "Time")
					NamePaint = new SolidColorPaint(SKColors.DodgerBlue),
					Labels = axisLabel,
					LabelsPaint = new SolidColorPaint(SKColors.Firebrick),
					Labeler = Labelers.SixRepresentativeDigits, // Or adjust as needed
				}
			};

			Title = title; // Chart title (can be more specific than X-axis name)
		}

		/// <summary>
		/// Updates the chart's data series and X-axis labels.
		/// </summary>
		/// <param name="newValues">The new list of values for the series.</param>
		/// <param name="newLabels">The new list of labels for the X-axis.</param>
		public void UpdateData(List<double> newValues, List<string> newLabels)
		{
			if (Series.FirstOrDefault() is LineSeries<double> lineSeries)
			{
				lineSeries.Values = newValues;
			}

			if (XAxis.FirstOrDefault() is Axis axis)
			{
				axis.Labels = newLabels;
			}
		}
	}
}
