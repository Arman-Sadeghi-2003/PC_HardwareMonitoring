using Avalonia.Controls;
using PC_HardwareMonitoring.Infrastructure.NotificationManager;

namespace PC_HardwareMonitoring.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

		private void Window_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
		{
			NotificationGenerator.Instance.ShowGreetingNotification(App.MainTopLevel);

		}
	}
}