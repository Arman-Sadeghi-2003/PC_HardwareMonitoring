using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PC_HardwareMonitoring.Views.Home;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
    }

	private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
	{
		((App)Application.Current).ChangeLanguage("fa-IR");
	}
}