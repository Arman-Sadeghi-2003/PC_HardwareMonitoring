using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using PC_HardwareMonitoring.ViewModels;
using PC_HardwareMonitoring.Views;
using System;
using System.Globalization;
using System.Linq;

namespace PC_HardwareMonitoring
{
	public partial class App : Application
	{
		public static TopLevel? MainTopLevel;
		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);
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

			SetupLocalization();


			base.OnFrameworkInitializationCompleted();
		}

		private void SetupLocalization()
		{
			// Set the default culture
			var culture = new CultureInfo("en-US");
			CultureInfo.DefaultThreadCurrentCulture = culture;
			CultureInfo.DefaultThreadCurrentUICulture = culture;

			// Load the resources for the current culture
			UpdateResourcesForCulture(culture);
		}

		public void UpdateResourcesForCulture(CultureInfo culture)
		{
			// Clear existing localization resources
			var resources = Resources.MergedDictionaries.Where(x => x is ResourceInclude resourceInclude &&
				resourceInclude.Source.AbsoluteUri.Contains("/Localization/")).ToList();
			foreach (var resource in resources)
			{
				Resources.MergedDictionaries.Remove(resource);
			}

			// Add the resource dictionary for the specified culture
			var resourcePath = $"avares://PC_HardwareMonitoring/Assets/Localization/Strings.{culture.Name}.axaml";

			// Fallback to default language if the specific culture resource doesn't exist
			if (!Uri.TryCreate(resourcePath, UriKind.Absolute, out _) || culture.Name == "en-US")
			{
				resourcePath = "avares://PC_HardwareMonitoring/Assets/Localization/Strings.axaml";
			}

			var uri = new Uri(resourcePath);
			var resourceInclude = new ResourceInclude(uri);
			resourceInclude.Source = uri;  // Set the Source property before adding to MergedDictionaries
			Resources.MergedDictionaries.Add(resourceInclude);
		}

		// Method to change language at runtime
		public void ChangeLanguage(string languageCode)
		{
			var newCulture = new CultureInfo(languageCode);
			CultureInfo.DefaultThreadCurrentCulture = newCulture;
			CultureInfo.DefaultThreadCurrentUICulture = newCulture;

			UpdateResourcesForCulture(newCulture);
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