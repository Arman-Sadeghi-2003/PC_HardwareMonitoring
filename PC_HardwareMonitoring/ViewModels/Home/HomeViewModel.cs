/// <summary>
/// ViewModel for the Home view.
/// </summary>
using CommunityToolkit.Mvvm.ComponentModel;
using PC_HardwareMonitoring.ViewModels.Tabs;

namespace PC_HardwareMonitoring.ViewModels.Home
{
	public partial class HomeViewModel : ViewModelBase
	{
		// Private constructor to enforce singleton pattern.
		private HomeViewModel() { }

		// Static instance of the HomeViewModel.
		private static HomeViewModel instance;

		/// <summary>
		/// Gets the singleton instance of the <see cref="HomeViewModel"/> class.
		/// </summary>
		public static HomeViewModel Instance => instance ?? (instance = new());

		#region Properties

		/// <summary>
		/// Gets or sets the Home tab ViewModel.
		/// </summary>
		[ObservableProperty]
		private HomeTabViewModel homeTabVM = new();

		/// <summary>
		/// Gets or sets the CPU tab ViewModel.
		/// </summary>
		[ObservableProperty]
		private CPUTabViewModel _CPUTabVM = new();

		/// <summary>
		/// Gets or sets the GPU tab ViewModel.
		/// </summary>
		[ObservableProperty]
		private GPUTabViewModel _GPUTabVM = new();

		/// <summary>
		/// Gets or sets the RAM tab ViewModel.
		/// </summary>
		[ObservableProperty]
		private RAMTabViewModel _RAMTabVM = new();

		/// <summary>
		/// Gets or sets the Motherboard tab ViewModel.
		/// </summary>
		[ObservableProperty]
		private MotherboardTabViewModel motherboardTabVM = new();

		#endregion Properties

	}
}
