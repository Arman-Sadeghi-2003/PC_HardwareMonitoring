using CommunityToolkit.Mvvm.ComponentModel;

namespace PC_HardwareMonitoring.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string greeting = "Welcome, Sir Arman!";
    }
}
