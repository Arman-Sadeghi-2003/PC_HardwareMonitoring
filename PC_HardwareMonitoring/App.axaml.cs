using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using PC_HardwareMonitoring.Infrastructure.URLs;
using PC_HardwareMonitoring.Models.Settings;
using PC_HardwareMonitoring.Tools.HW;
using PC_HardwareMonitoring.Tools.Localization;
using PC_HardwareMonitoring.ViewModels;
using PC_HardwareMonitoring.Views;
using System.IO;
using System.Linq;

namespace PC_HardwareMonitoring
{
	public partial class App : Application
	{
		public static TopLevel? MainTopLevel;
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);

			if (!Directory.Exists(FileNames.HWLocalPath))
				Directory.CreateDirectory(FileNames.HWLocalPath);

			Monitor.Instance.GetCPUInfos();
		}

		public override void OnFrameworkInitializationCompleted()
		{
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				// Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
				// More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
				DisableAvaloniaDataAnnotationValidation();
				var mainWindow = new MainWindow
				{
					DataContext = new MainWindowViewModel(),
				};
				MainTopLevel = MainWindow.GetTopLevel(mainWindow);
				desktop.MainWindow = mainWindow;
			}

			//SetupLocalization();
			LocalizationManager.Instance.ChangeLanguage(SettingsModel.Instance.selectedLanguage);

			base.OnFrameworkInitializationCompleted();
		}

		private void DisableAvaloniaDataAnnotationValidation()
		{
			// Get an array of plugins to remove
			var dataValidationPluginsToRemove =
				BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

			// remove each entry found
			foreach (var plugin in dataValidationPluginsToRemove)
			{
				BindingPlugins.DataValidators.Remove(plugin);
			}
		}
	}
}