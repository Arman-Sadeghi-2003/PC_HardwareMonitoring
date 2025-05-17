using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PC_HardwareMonitoring.ViewModels.Home;

namespace PC_HardwareMonitoring.ViewModels
{
	public partial class MainWindowViewModel : ViewModelBase
	{
		[ObservableProperty]
		private ViewModelBase homeVM;
		public MainWindowViewModel()
		{
			HomeVM = HomeViewModel.Instance;
		}

		[RelayCommand]
		public void NavigateToHomeView()
		{
			HomeVM = HomeViewModel.Instance;
		}

		[RelayCommand]
		public void NavigateToSettingsView()
		{
			HomeVM = SettingsViewModel.Instance;
		}
	}
}
