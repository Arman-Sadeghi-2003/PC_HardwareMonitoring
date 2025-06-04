/// <summary>
/// ViewModel for the main application window.
/// </summary>
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PC_HardwareMonitoring.ViewModels.Home;

namespace PC_HardwareMonitoring.ViewModels
{
	public partial class MainWindowViewModel : ViewModelBase
	{
		/// <summary>
		/// Gets or sets the current ViewModel for the home view.
		/// </summary>
		[ObservableProperty]
		private ViewModelBase homeVM;

		/// <summary>
		/// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
		/// </summary>
		public MainWindowViewModel()
		{
			HomeVM = HomeViewModel.Instance;
		}

		/// <summary>
		/// Navigates to the home view.
		/// </summary>
		[RelayCommand]
		public void NavigateToHomeView()
		{
			HomeVM = HomeViewModel.Instance;
		}

		/// <summary>
		/// Navigates to the settings view.
		/// </summary>
		[RelayCommand]
		public void NavigateToSettingsView()
		{
			HomeVM = new SettingsViewModel();
		}
	}
}
