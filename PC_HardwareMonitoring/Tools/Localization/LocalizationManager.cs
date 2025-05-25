using Avalonia.Markup.Xaml.Styling;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PC_HardwareMonitoring.Tools.Localization
{
	public class LocalizationManager
	{
		private LocalizationManager() { }
		private static LocalizationManager instance;
		public static LocalizationManager Instance => instance ?? (instance = new LocalizationManager());
		public async Task UpdateResourcesForCulture(CultureInfo culture)
		{
			// Clear existing localization resources
			var resources = App.Current.Resources.MergedDictionaries.Where(x => x is ResourceInclude resourceInclude &&
				resourceInclude.Source.AbsoluteUri.Contains("/Localization/")).ToList();
			foreach (var resource in resources)
			{
				App.Current.Resources.MergedDictionaries.Remove(resource);
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
			App.Current.Resources.MergedDictionaries.Add(resourceInclude);

			await Task.Delay(50);
		}

		// Method to change language at runtime
		public void ChangeLanguage(string languageCode)
		{
			var newCulture = new CultureInfo(languageCode);
			CultureInfo.DefaultThreadCurrentCulture = newCulture;
			CultureInfo.DefaultThreadCurrentUICulture = newCulture;

			UpdateResourcesForCulture(newCulture);
		}

		public string GetString(string key)
		{
			try
			{
				var resource = App.Current?.Resources[key];
				if (resource != null && !resource.ToString().Equals("Unset"))
					return resource.ToString();
			}
			catch { }

			return key; // Fallback to key
		}
	}
}
